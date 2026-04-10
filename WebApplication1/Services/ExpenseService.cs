using App1.Data;
using App1.Entities;
using App1.Models;
using App1.TransferObjects;
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
        public async Task<ServiceResult<IEnumerable<ExpenseModel>>> GetAllAsync()
        {
            try
            {
                var expenses = await _context.Expenses
                    .AsNoTracking()
                    .Select(expense => expense.ToModel())
                    .ToListAsync();

                return ServiceResult<IEnumerable<ExpenseModel>>.SuccessResult(expenses, "Expenses retrieved successfully.");
            }
            catch
            {
                return ServiceResult<IEnumerable<ExpenseModel>>.Failure(UnexpectedErrorMessage);
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
                    return ServiceResult<ExpenseModel>.Failure(NotFoundMessage);
                }

                return ServiceResult<ExpenseModel>.SuccessResult(expense.ToModel(), "Expense retrieved successfully.");
            }
            catch
            {
                return ServiceResult<ExpenseModel>.Failure(UnexpectedErrorMessage);
            }
        }

        // Create
        public async Task<ServiceResult<ExpenseModel>> AddAsync(ExpenseModel expense)
        {
            try
            {
                PrepareNewExpense(expense);
                var entity = expense.ToEntity();

                _context.Expenses.Add(entity);
                await _context.SaveChangesAsync();

                return ServiceResult<ExpenseModel>.SuccessResult(entity.ToModel(), "Expense created successfully.");
            }
            catch
            {
                return ServiceResult<ExpenseModel>.Failure(UnexpectedErrorMessage);
            }
        }

        // Update
        public async Task<ServiceResult> UpdateAsync(int id, ExpenseModel expense)
        {
            try
            {
                var existingExpense = await _context.Expenses.FindAsync(id);

                if (existingExpense == null)
                {
                    return ServiceResult.Failure(NotFoundMessage);
                }

                MapExpenseForUpdate(existingExpense, expense);
                await _context.SaveChangesAsync();

                return ServiceResult.SuccessResult("Expense updated successfully.");
            }
            catch
            {
                return ServiceResult.Failure(UnexpectedErrorMessage);
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
                    return ServiceResult.Failure(NotFoundMessage);
                }

                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();

                return ServiceResult.SuccessResult("Expense deleted successfully.");
            }
            catch
            {
                return ServiceResult.Failure(UnexpectedErrorMessage);
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
    }
}
