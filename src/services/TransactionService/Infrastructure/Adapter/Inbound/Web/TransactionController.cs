using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Dto.Transaction;
using TransactionService.Application.Port.Inbound;

namespace TransactionService.Infrastructure.Adapter.Inbound.Web
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ICreateTransactionUseCase _createTransactionUseCase;
        private readonly IVerifyTransactionUseCase _verifyTransactionUseCase;

        public TransactionController(
            ICreateTransactionUseCase createTransactionUseCase,
            IVerifyTransactionUseCase verifyTransactionUseCase
        )
        {
            _createTransactionUseCase = createTransactionUseCase;
            _verifyTransactionUseCase = verifyTransactionUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromForm] TransactionCreateRequest request)
        {
            var result = await _createTransactionUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyTransaction([FromBody] TransactionVerifyRequest request)
        {
            var result = await _verifyTransactionUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
