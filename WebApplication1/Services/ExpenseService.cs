using App1.Data;
using App1.Models;
using App1.TransferObjects;
using Microsoft.EntityFrameworkCore;

namespace App1.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly App1DbContext _context;

        public ExpenseService(App1DbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<ExpenseModel>>> GetAllAsync()
        {
            var expenses = await _context.Expenses
                .AsNoTracking()
                .Select(expense => expense.ToModel())
                .ToListAsync();

            return ServiceResult<IEnumerable<ExpenseModel>>.SuccessResult(expenses, "Expenses retrieved successfully.");
        }

        public async Task<ServiceResult<ExpenseModel>> GetByIdAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return ServiceResult<ExpenseModel>.Failure("Expense not found.");
            }

            return ServiceResult<ExpenseModel>.SuccessResult(expense.ToModel(), "Expense retrieved successfully.");
        }

        public async Task<ServiceResult<ExpenseModel>> AddAsync(ExpenseModel expense)
        {
            expense.CreatedDate = DateTime.UtcNow;
            expense.CreatedBy = string.IsNullOrWhiteSpace(expense.CreatedBy) ? "System" : expense.CreatedBy;

            var entity = expense.ToEntity();

            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync();

            return ServiceResult<ExpenseModel>.SuccessResult(entity.ToModel(), "Expense created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(int id, ExpenseModel expense)
        {
            var existingExpense = await _context.Expenses.FindAsync(id);

            if (existingExpense == null)
            {
                return ServiceResult.Failure("Expense not found.");
            }

            existingExpense.Code = expense.Code;
            existingExpense.Description = expense.Description;
            existingExpense.Amount = expense.Amount;

            await _context.SaveChangesAsync();

            return ServiceResult.SuccessResult("Expense updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return ServiceResult.Failure("Expense not found.");
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return ServiceResult.SuccessResult("Expense deleted successfully.");
        }
    }
}
