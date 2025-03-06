using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Ensure this is included
using Supermarket.Data;
using Supermarket.Models;

namespace Supermarket.Services
{
    public class InventoryService
    {
        private readonly SupermarketContext _context;

        public InventoryService(SupermarketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Product> GetProducts()
        {
            try
            {
                // Include related entities (Category and Supplier)
                return _context.Products
                    .Include(p => p.Category) // Include Category
                    .Include(p => p.Supplier) // Include Supplier
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products: {ex}");
                return new List<Product>();
            }
        }

        public Product GetProduct(int productId)
        {
            try
            {
                // Include related entities when fetching a single product
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier) 
                    .FirstOrDefault(p => p.ProductID == productId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product: {ex}");
                return null;
            }
        }

        public bool AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)  //Catch specific EF Core exception
            {
                // Handle database-related errors (e.g., constraint violations)
                Console.WriteLine($"DB Error adding product: {ex.Message}");

                //Log the inner exception details for specific constraint error
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return false; // Indicate failure
            }
            catch (Exception ex)
            {
                // Handle general errors
                Console.WriteLine($"General Error adding product: {ex}");
                return false;
            }
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex}");
                return false;
            }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                var product = _context.Products.Find(productId);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex}");
                return false;
            }
        }

    }
}