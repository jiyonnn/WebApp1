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
        public async Task<ActionResult<IEnumerable<ExpenseResponseDto>>> List()
        {
            var result = await _service.GetAllAsync();
            return Ok(result.Data!.Select(expense => expense.ToResponseDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseResponseDto>> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(new ApiResponseDto
                {
                    Message = result.Message
                });
            }

            return Ok(result.Data!.ToResponseDto());
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponseDto>> Post(CreateExpenseDto expense)
        {
            var result = await _service.AddAsync(expense.ToModel());

            return Ok(new ApiResponseDto
            {
                Message = result.Message
            });
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<ApiResponseDto>> Put(int id, UpdateExpenseDto expense)
        {
            var result = await _service.UpdateAsync(id, expense.ToModel(id));

            if (!result.Success)
            {
                return NotFound(new ApiResponseDto
                {
                    Message = result.Message
                });
            }

            return Ok(new ApiResponseDto
            {
                Message = result.Message
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApiResponseDto>> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.Success)
            {
                return NotFound(new ApiResponseDto
                {
                    Message = result.Message
                });
            }

            return Ok(new ApiResponseDto
            {
                Message = result.Message
            });
        }
    }
}
