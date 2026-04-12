using TransactionService.Application.Dto;
using TransactionService.Application.Dto.Transaction;

namespace TransactionService.Application.Port.Inbound
{
    public interface IGetTransactionUseCase
    {
        public Task<Result<List<TransactionGetResponse>>> ExecuteAsync(bool isOwner, string userId);
    }
}
