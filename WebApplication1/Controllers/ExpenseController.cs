using App1.Models;
using App1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Controllers
{
    [Route("api/expenses/v1")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpenseController(IExpenseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Expense>> Get() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> Get(int id)
        {
            var expense = await _service.GetByIdAsync(id);
            if (expense == null) return NotFound();
            return expense;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> Post(Expense expense)
        {
            var created = await _service.AddAsync(expense);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Expense>> Put(int id, Expense expense)
        {
            if (id != expense.Id) return BadRequest();
            var updated = await _service.UpdateAsync(expense);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
