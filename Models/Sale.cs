using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleID { get; set; }

        public int? CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(50)] // Removed [Required] to allow NULL values
        public string? PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountAmount { get; set; }

        public string? Notes { get; set; }  // Allow null values for Notes

        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
