using TransactionService.Application.Dto;
using TransactionService.Application.Dto.Transaction;

namespace TransactionService.Application.Port.Inbound
{
    public interface ICreateTransactionUseCase
    {
        public Task<Result<string>> ExecuteAsync(TransactionCreateRequest request);
    }
}
