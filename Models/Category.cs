//Category.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        //Add getter and setter
    }
}