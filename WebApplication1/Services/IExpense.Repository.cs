using App1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Repositories
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllAsync();
        Task<Expense> GetByIdAsync(int id);
        Task<Expense> AddAsync(Expense expense);
        Task<Expense> UpdateAsync(Expense expense);
        Task<bool> DeleteAsync(int id);
    }
}