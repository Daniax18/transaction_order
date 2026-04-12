using TransactionService.Application.Dto;
using TransactionService.Domain.Enum;

namespace TransactionService.Application.Port.Inbound
{
    public interface IUpdateStatusUseCase
    {
        public Task<Result<string>> ExecuteAsync(string transactionId, TransactionOrderStatus status);
    }
}
