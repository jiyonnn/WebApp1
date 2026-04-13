using App1.Entities;
using App1.Models;
using App1.TransferObjects;

namespace App1.Mappers
{
    public static class ExpenseMappingExtensions
    {
        public static ExpenseModel ToModel(this Expense expense)
        {
            return new ExpenseModel
            {
                Id = expense.Id,
                Code = expense.Code,
                Description = expense.Description,
                Amount = expense.Amount,
                CreatedBy = expense.CreatedBy,
                CreatedDate = expense.CreatedDate
            };
        }

        public static Expense ToEntity(this ExpenseModel expense)
        {
            return new Expense
            {
                Id = expense.Id,
                Code = expense.Code,
                Description = expense.Description,
                Amount = expense.Amount,
                CreatedBy = expense.CreatedBy,
                CreatedDate = expense.CreatedDate
            };
        }

        public static ExpenseModel ToModel(this CreateExpenseDto expense)
        {
            return new ExpenseModel
            {
                Code = expense.Code,
                Description = expense.Description,
                Amount = expense.Amount
            };
        }

        public static ExpenseModel ToModel(this UpdateExpenseDto expense, int id)
        {
            return new ExpenseModel
            {
                Id = id,
                Code = expense.Code,
                Description = expense.Description,
                Amount = expense.Amount
            };
        }

        public static ExpenseResponseDto ToResponseDto(this ExpenseModel expense)
        {
            return new ExpenseResponseDto
            {
                Id = expense.Id,
                Code = expense.Code,
                Description = expense.Description,
                Amount = expense.Amount,
                CreatedBy = expense.CreatedBy,
                CreatedDate = expense.CreatedDate.ToString("yyyy-MM-dd")
            };
        }
    }
}
