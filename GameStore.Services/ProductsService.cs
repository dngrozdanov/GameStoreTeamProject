﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Data.Context.Abstract;
using GameStore.Data.Models;
using GameStore.Exceptions;
using GameStore.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IGameStoreContext storeContext;

        public ProductsService(IGameStoreContext storeContext)
        {
            this.storeContext = storeContext ?? throw new ArgumentNullException(nameof(storeContext));
        }

        /// <summary>
        ///     Creates a product from the given parameters and adds it to the database.
        /// </summary>
        /// <param name="productName">Product Name</param>
        /// <param name="productDescription">Product Description</param>
        /// <param name="productPrice">Product Price</param>
        /// <param name="productGenres">Product Genres (ICollection)</param>
        /// <param name="productComments">Product Comments (ICollection)</param>
        /// <returns></returns>
        public Product AddProduct(string productName, string productDescription, decimal productPrice,
            ICollection<Genre> productGenres = null, ICollection<Comment> productComments = null)
        {
            if (FindProduct(productName) != null)
                throw new UserException($"Product ({productName}) already exists.");

            var product = new Product
            {
                Name = productName,
                Description = productDescription,
                Price = productPrice,
                CreatedOn = DateTime.Now
            };

            if (productGenres != null)
                product.Genre = productGenres;

            if (productComments != null)
                product.Comments = productComments;

            storeContext.Products.Add(product);
            storeContext.SaveChanges();

            return product;
        }

        /// <summary>
        ///     Removes the first product that matches the given name parameter in the database.
        /// </summary>
        /// <param name="productName">Product Name</param>
        /// <returns></returns>
        public string RemoveProduct(string productName)
        {
            var product = storeContext.Products.ToList().SingleOrDefault(p => p.Name == productName);
            if (product == null || product.IsDeleted) return $"Product {productName} was not found.";

            product.IsDeleted = true;
            storeContext.SaveChanges();
            return $"Product {productName} has been successfully removed.";
        }

        /// <summary>
        ///     Finds the product in the database that matches the given productName in the parameters and returns it as Product
        ///     type.
        /// </summary>
        /// <param name="productName">Product Name</param>
        /// <returns></returns>
        public Product FindProduct(string productName)
        {
            var product = GetProducts().SingleOrDefault(p => p.Name == productName);

            return product == null || product.IsDeleted ? null : product;
        }

        public IEnumerable<Product> FindProductsByGenre(IEnumerable<Genre> productGenre)
        {
            var products = GetProducts().Where(p => { return productGenre.All(genre => p.Genre.Contains(genre)); });

            return !products.Any() ? null : products;
        }

        public IEnumerable<Product> GetProducts()
        {
            return storeContext.Products
                .Include(s => s.ShoppingCartProducts)
                .ThenInclude(cart => cart.Product)
                .Include(s => s.ShoppingCartProducts)
                .ThenInclude(cart => cart.ShoppingCart)
                .Include(c => c.Comments)
                .ThenInclude(comment => comment.Account)
                .Include(c => c.Comments)
                .ThenInclude(comment => comment.Product)
                .Include(g => g.Genre)
                .ToList();
        }

        public Product AddProduct(Product product)
        {
            return AddProduct(product.Name, product.Description, product.Price, product.Genre, product.Comments);
        }

        public IEnumerable<Product> FindProductsByGenre(Genre productGenre)
        {
            var products = GetProducts().Where(p => p.Genre.Contains(productGenre));

            return !products.Any() ? null : products;
        }
    }
}