using App1.Models;
using App1.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Expense>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Expense> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<Expense> AddAsync(Expense expense) => _repository.AddAsync(expense);
        public Task<Expense> UpdateAsync(Expense expense) => _repository.UpdateAsync(expense);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
