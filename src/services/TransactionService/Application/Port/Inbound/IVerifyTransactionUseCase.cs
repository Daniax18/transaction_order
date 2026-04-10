using TransactionService.Application.Dto;
using TransactionService.Application.Dto.Transaction;

namespace TransactionService.Application.Port.Inbound
{
    public interface IVerifyTransactionUseCase
    {
        public Task<Result<bool>> ExecuteAsync(TransactionVerifyRequest request);
    }
}
