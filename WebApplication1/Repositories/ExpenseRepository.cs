using App1.Data;
using App1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly App1DbContext _context;

        public ExpenseRepository(App1DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllAsync() =>
            await _context.Expenses.ToListAsync();

        public async Task<Expense> GetByIdAsync(int id) =>
            await _context.Expenses.FindAsync(id);

        public async Task<Expense> AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense> UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return false;
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
