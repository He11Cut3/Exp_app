using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Expenses_App.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        public ExpenseCategory Category { get; set; }
    }

}
