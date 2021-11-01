using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Context;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDBContext _context;
        public ProductController (ProductDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int categoryId = 0)
        {
            var result = _context.Products.AsQueryable();
            if (categoryId > 0) result = result.Where(p => p.ProductCategoryId == categoryId).AsQueryable();
            var productList = new List<ProductViewModel>();
            foreach (var product in await result.ToListAsync())
            {
                productList.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    WeightKg = product.WeightKg,
                    ProductCategoryId = product.ProductCategoryId,
                    ProductCategoryName = product.ProductCategory.Name
                }); ;
            }
            return View(productList);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var product = new ProductViewModel
            {
                Categories = await _context
                .ProductCategories
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToListAsync()
            };
            return View(product);
        }

        [HttpPost]
        public async Task <IActionResult> Create(ProductViewModel product, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                product.Categories = await _context
                .ProductCategories
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToListAsync();
                return View(product);
            }
            var productDB = new Product
            {
                Name = product.Name,
                WeightKg = product.WeightKg,
                ProductCategoryId = product.ProductCategoryId,
            };
            _context.Products.Add(productDB);
            await _context.SaveChangesAsync(token);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete (int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var result = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ProductCategoryId = product.ProductCategoryId,
                Categories = await _context.ProductCategories.
                    Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToListAsync()
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productModel)
        {
            if (!ModelState.IsValid)
            {
                productModel.Categories = await _context.ProductCategories.
                    Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToListAsync();
                return View(productModel);
            }

            var product = await _context.Products.FindAsync(productModel.Id);

            if (productModel == null)
            {
                return RedirectToAction("Index");
            }

            product.Name = productModel.Name;
            product.WeightKg = productModel.WeightKg;
            product.ProductCategoryId = productModel.ProductCategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
