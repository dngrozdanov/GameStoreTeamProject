﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GameStore.Data.Models;
using GameStore.Services.Abstract;
using GameStore.Web.Areas.Administration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [TempData] public string StatusMessage { get; set; }

        // GET: Administration/Products
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.GetAllProducts();
            var viewModel = new List<ProductsViewModel>();

            foreach (var product in products) viewModel.Add(new ProductsViewModel(product));

            return View(viewModel);
        }

        // GET: Administration/Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productsService.FindProductAsync(id, true);

            if (product == null) return NotFound();

            var newProduct = new ProductsViewModel(product);

            return View(newProduct);
        }

        // GET: Administration/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administration/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsViewModel product)
        {
            if (ModelState.IsValid)
            {
                var newModel = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    Publisher = product.Publisher,
                    Price = product.Price,
                    CreatedOn = DateTime.Now,
                    IsDeleted = product.IsDeleted,
                    IsOnSale = product.IsOnSale
                };

                await _productsService.AddProductAsync(newModel);
                StatusMessage = $"Successfully added {newModel.Name}";
                return RedirectToAction(nameof(Index));
            }

            StatusMessage = "Error: Something went wrong...";
            return View(product);
        }

        // GET: Administration/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsService.FindProductAsync(id, true);
            if (product == null) return NotFound();

            var newProduct = new ProductsViewModel(product);

            return View(newProduct);
        }

        // POST: Administration/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductsViewModel product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var newModel = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Publisher = product.Publisher,
                        Price = product.Price,
                        ProductImageName = product.ImageName,
                        CreatedOn = product.CreatedOn,
                        LastPurchased = product.LastPurchased,
                        IsDeleted = product.IsDeleted,
                        IsOnSale = product.IsOnSale
                    };

                    await _productsService.UpdateProductAsync(newModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _productsService.ProductExistsAsync(product.Id) == false)
                        return NotFound();
                    throw;
                }

                StatusMessage = $"Successfully modified {product.Name}";
                return RedirectToAction(nameof(Index));
            }

            StatusMessage = "Error: Something went wrong...";
            return View(product);
        }

        // GET: Administration/Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productsService.FindProductAsync(id, true);

            if (product == null) return NotFound();

            var newProduct = new ProductsViewModel(product);

            return View(newProduct);
        }

        // POST: Administration/Products/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productsService.FindProductAsync(id, true);
            var productName = product.Name;
            await _productsService.RemoveProductAsync(product);

            StatusMessage = $"Successfully deleted {productName}";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProductImage(int productId, IFormFile productImage)
        {
            if (productImage == null)
            {
                StatusMessage = "Error: Please provide an image";
                return RedirectToAction(nameof(Edit), new {id = productId});
            }

            if (!IsValidImage(productImage))
            {
                StatusMessage = "Error: Please provide a .jpg or .png file smaller than 2MB";
                return RedirectToAction(nameof(Edit), new {id = productId});
            }

            await _productsService.SaveProductImageAsync(
                GetUploadsRoot(),
                productImage.FileName,
                productImage.OpenReadStream(),
                productId
            );

            StatusMessage = "Product Image Updated Successfully";

            return RedirectToAction(nameof(Edit), new {id = productId});
        }

        private string GetUploadsRoot()
        {
            var environment = HttpContext.RequestServices
                .GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;

            return Path.Combine(environment.WebRootPath, "images", "products");
        }

        private bool IsValidImage(IFormFile image)
        {
            var type = image.ContentType;
            if (type != "image/png" && type != "image/jpg" && type != "image/jpeg") return false;

            return image.Length <= 2048 * 2048;
        }
    }
}