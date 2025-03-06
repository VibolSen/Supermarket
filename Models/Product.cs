using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int ProductID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; }

        public int? CategoryID { get; set; } // Foreign Key

        [ForeignKey("CategoryID")]
        public Category Category { get; set; }  // Navigation Property

        // Allow Description to be null
        public string? Description { get; set; } // Use nullable reference type

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int SupplierID { get; set; } // Foreign Key

        [ForeignKey("SupplierID")]
        public Supplier Supplier { get; set; } // Navigation property

        [Required]
        public int StockQuantity { get; set; } = 0; // Default value

        [Required]
        public int ReorderLevel { get; set; } = 10;

        // Allow ImageURL to be null
        [MaxLength(255)]
        public string? ImageURL { get; set; } // Use nullable reference type

        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();  // Navigation Property
    }
}