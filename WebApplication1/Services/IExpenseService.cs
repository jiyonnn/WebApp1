using App1.Models;
using App1.TransferObjects;

namespace App1.Services
{
    public interface IExpenseService
    {
        Task<ServiceResult<IEnumerable<ExpenseModel>>> GetAllAsync();
        Task<ServiceResult<ExpenseModel>> GetByIdAsync(int id);
        Task<ServiceResult<ExpenseModel>> AddAsync(ExpenseModel expense);
        Task<ServiceResult> UpdateAsync(int id, ExpenseModel expense);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
