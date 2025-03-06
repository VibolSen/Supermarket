using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class SaleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleItemID { get; set; }

        [Required]
        public int SaleID { get; set; }

        [ForeignKey("SaleID")]
        public Sale Sale { get; set; }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; }
    }
}