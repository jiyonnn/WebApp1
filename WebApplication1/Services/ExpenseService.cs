using App1.Data;
using App1.Entities;
using App1.Mappers;
using App1.Models;
using Microsoft.EntityFrameworkCore;

namespace App1.Services
{
    public class ExpenseService : IExpenseService
    {
        private const string DefaultCreatedBy = "System";
        private const string NotFoundMessage = "Expense not found.";
        private const string UnexpectedErrorMessage = "An unexpected error occurred while processing the expense.";

        private readonly App1DbContext _context;

        public ExpenseService(App1DbContext context)
        {
            _context = context;
        }

        // Methods
        // Get all
        public async Task<ServiceResult<IEnumerable<ExpenseModel>>> GetAllAsync(string? search = null, string? code = null, string? description = null)
        {
            try
            {
                var query = _context.Expenses
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var trimmedSearch = search.Trim();
                    query = query.Where(expense =>
                        expense.Code.Contains(trimmedSearch) ||
                        expense.Description.Contains(trimmedSearch));
                }

                if (!string.IsNullOrWhiteSpace(code))
                {
                    var trimmedCode = code.Trim();
                    query = query.Where(expense => expense.Code.Contains(trimmedCode));
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    var trimmedDescription = description.Trim();
                    query = query.Where(expense => expense.Description.Contains(trimmedDescription));
                }

                var expenses = await query
                    .Select(expense => expense.ToModel())
                    .ToListAsync();

                return new ServiceResult<IEnumerable<ExpenseModel>>
                {
                    Success = true,
                    Message = "Expenses retrieved successfully.",
                    Data = expenses
                };
            }
            catch
            {
                return new ServiceResult<IEnumerable<ExpenseModel>>
                {
                    Success = false,
                    Message = UnexpectedErrorMessage
                };
            }
        }

        //Get by id
        public async Task<ServiceResult<ExpenseModel>> GetByIdAsync(int id)
        {
            try
            {
                var expense = await _context.Expenses.FindAsync(id);

                if (expense == null)
                {
                    return new ServiceResult<ExpenseModel>
                    {
                        Success = false,
                        Message = NotFoundMessage
                    };
                }

                return new ServiceResult<ExpenseModel>
                {
                    Success = true,
                    Message = "Expense retrieved successfully.",
                    Data = expense.ToModel()
                };
            }
            catch
            {
                return new ServiceResult<ExpenseModel>
                {
                    Success = false,
                    Message = UnexpectedErrorMessage
                };
            }
        }

        // Create
        public async Task<ServiceResult<ExpenseModel>> AddAsync(ExpenseModel expense)
        {
            try
            {
                var validationMessage = ValidateExpense(expense);

                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    return new ServiceResult<ExpenseModel>
                    {
                        Success = false,
                        Message = validationMessage
                    };
                }

                PrepareNewExpense(expense);
                var entity = expense.ToEntity();

                _context.Expenses.Add(entity);
                await _context.SaveChangesAsync();

                return new ServiceResult<ExpenseModel>
                {
                    Success = true,
                    Message = "Expense created successfully.",
                    Data = entity.ToModel()
                };
            }
            catch
            {
                return new ServiceResult<ExpenseModel>
                {
                    Success = false,
                    Message = UnexpectedErrorMessage
                };
            }
        }

        // Update
        public async Task<ServiceResult> UpdateAsync(int id, ExpenseModel expense)
        {
            try
            {
                var validationMessage = ValidateExpense(expense);

                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = validationMessage
                    };
                }

                var existingExpense = await _context.Expenses.FindAsync(id);

                if (existingExpense == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = NotFoundMessage
                    };
                }

                MapExpenseForUpdate(existingExpense, expense);
                await _context.SaveChangesAsync();

                return new ServiceResult
                {
                    Success = true,
                    Message = "Expense updated successfully."
                };
            }
            catch
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = UnexpectedErrorMessage
                };
            }
        }

        // Delete
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var expense = await _context.Expenses.FindAsync(id);

                if (expense == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = NotFoundMessage
                    };
                }

                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();

                return new ServiceResult
                {
                    Success = true,
                    Message = "Expense deleted successfully."
                };
            }
            catch
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = UnexpectedErrorMessage
                };
            }
        }

        private static void PrepareNewExpense(ExpenseModel expense)
        {
            expense.CreatedDate = DateTime.UtcNow;
            expense.CreatedBy = string.IsNullOrWhiteSpace(expense.CreatedBy)
                ? DefaultCreatedBy
                : expense.CreatedBy;
        }

        private static void MapExpenseForUpdate(Expense entity, ExpenseModel expense)
        {
            entity.Code = expense.Code;
            entity.Description = expense.Description;
            entity.Amount = expense.Amount;
        }

        private static string? ValidateExpense(ExpenseModel expense)
        {
            if (string.IsNullOrWhiteSpace(expense.Code))
            {
                return "Code is required.";
            }

            if (expense.Code.Length > 10)
            {
                return "Code must not exceed 10 characters.";
            }

            if (string.IsNullOrWhiteSpace(expense.Description))
            {
                return "Description is required.";
            }

            if (expense.Description.Length > 200)
            {
                return "Description must not exceed 200 characters.";
            }

            if (expense.Amount <= 0)
            {
                return "Amount must be greater than 0.";
            }

            if (expense.Amount > 9999999999999999.99m)
            {
                return "Amount must not exceed 9999999999999999.99.";
            }

            if (decimal.Round(expense.Amount, 2) != expense.Amount)
            {
                return "Amount must have no more than 2 decimal places.";
            }

            return null;
        }
    }
}
