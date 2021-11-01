using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Context;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDBContext _context;
        public ProductController (ProductDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var result = await _context.Products.ToListAsync();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(Product product, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
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

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }

            var product = await _context.Products.FindAsync(productModel.Id);

            if (productModel == null)
            {
                return RedirectToAction("Index");
            }

            product.Name = productModel.Name;
            product.Category = productModel.Category;
            product.WeightKg = productModel.WeightKg;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
