using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Role { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string Attendance { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}