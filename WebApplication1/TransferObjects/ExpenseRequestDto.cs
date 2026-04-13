using App1.Validation;
using System.ComponentModel.DataAnnotations;

namespace App1.TransferObjects
{
    public abstract class ExpenseRequestDto
    {
        [Required(ErrorMessage = "Code is required.")]
        [StringLength(10, ErrorMessage = "Code must not exceed 10 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "9999999999999999.99", ErrorMessage = "Amount must be between 0.01 and 9999999999999999.99.")]
        [DecimalPrecisionScale(18, 2, ErrorMessage = "Amount must have no more than 18 digits in total and 2 decimal places.")]
        public decimal Amount { get; set; }
    }
}
