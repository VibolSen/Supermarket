using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Discount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountID { get; set; }

        [Required]
        [MaxLength(255)]
        public string DiscountName { get; set; }

        [Required]
        [MaxLength(50)]
        public string DiscountType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        public int? CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
    }
}