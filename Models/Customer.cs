using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        public string? Address { get; set; } // Nullable

        [Required]
        public int LoyaltyPoints { get; set; } = 0;

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}