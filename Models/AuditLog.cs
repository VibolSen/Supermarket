using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public int? EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }

        [Required]
        [MaxLength(255)]
        public string TableName { get; set; }

        [Required]
        public int RecordID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Action { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }
    }
}