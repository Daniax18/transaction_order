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
        private readonly IGetTransactionUseCase _getTransactionUseCase;

        public TransactionController(
            ICreateTransactionUseCase createTransactionUseCase,
            IVerifyTransactionUseCase verifyTransactionUseCase,
            IGetTransactionUseCase getTransactionUseCase
        )
        {
            _createTransactionUseCase = createTransactionUseCase;
            _verifyTransactionUseCase = verifyTransactionUseCase;
            _getTransactionUseCase = getTransactionUseCase;
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

        [HttpGet]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] Boolean isOwner,
            [FromQuery] string userId
        )
        {
            var result = await _getTransactionUseCase.ExecuteAsync(isOwner, userId);
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
