using App1.Mappers;
using App1.Services;
using App1.TransferObjects;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("list")]
        public async Task<ActionResult> List([FromQuery] string? search, [FromQuery] string? code, [FromQuery] string? description)
        {
            var result = await _service.GetAllAsync(search, code, description);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result.Data!.Select(expense => expense.ToResponseDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.Success)
            {
                return BadRequest(new {result.Message});
            }

            return Ok(result.Data!.ToResponseDto());
        }

        [HttpPost("create")]
        public async Task<ActionResult> Post(CreateExpenseDto expense)
        {
            var result = await _service.AddAsync(expense.ToModel());

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new
            {
                Message = result.Message
            });
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> Put(int id, UpdateExpenseDto expense)
        {
            var result = await _service.UpdateAsync(id, expense.ToModel(id));

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new
            {
                Message = result.Message
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new
            {
                Message = result.Message
            });
        }

    }
}
