﻿using System.Collections.Generic;
using System.Linq;
using GameStore.Data.Context;
using GameStore.Data.Models;
using GameStore.Exceptions;
using GameStore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Tests.ProductServiceTests
{
    [TestClass]
    public class AddProduct_Should
    {
        [TestMethod]
        public void AddProduct_WhenInput_IsValid()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameStoreContext>()
                .UseInMemoryDatabase("AddProduct_WhenInput_IsValid").Options;

            var productToAdd = new Product
            {
                Name = "Test",
                Description = "test description",
                ShoppingCartProducts = new List<ShoppingCartProducts>(),
                Comments = new List<Comment>()
            };
            //Act
            using (var curContext = new GameStoreContext(options))
            {
                var sut = new ProductsService(curContext);
                curContext.Products.Add(productToAdd);
                curContext.SaveChanges();
            }


            //Assert
            using (var curContext = new GameStoreContext(options))
            {
                Assert.IsTrue(curContext.Products.Count() == 1);
            }
        }

        
        //ToDO
        public async void ThrowException_WhenInput_AlreadyExists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GameStoreContext>()
                .UseInMemoryDatabase("ThrowException_WhenInput_AlreadyExists").Options;

            var productToAdd = new Product
            {
                Name = "Test",
                Description = "test description",
                ShoppingCartProducts = new List<ShoppingCartProducts>(),
                Comments = new List<Comment>()
            };
            //Act
            using (var curContext = new GameStoreContext(options))
            {
                var sut = new ProductsService(curContext);
                curContext.Products.Add(productToAdd);

                curContext.SaveChanges();
            }

            //Act
            using (var curContext = new GameStoreContext(options))
            {
                var sut = new ProductsService(curContext);
                await sut.AddProductAsync(productToAdd);
            } 
        }
    }
}