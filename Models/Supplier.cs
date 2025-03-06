// Supplier.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }

        [Required]
        [MaxLength(255)]
        public string SupplierName { get; set; }
        //Add getter and setter
        public string ContactPerson { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public string Address { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}