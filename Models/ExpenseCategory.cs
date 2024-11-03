using System.ComponentModel.DataAnnotations;

namespace Expenses_App.Models
{
    public class ExpenseCategory
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }

}
