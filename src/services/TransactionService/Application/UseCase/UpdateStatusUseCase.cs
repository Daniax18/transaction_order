using TransactionService.Application.Dto;
using TransactionService.Application.Port.Inbound;
using TransactionService.Application.Port.Outbound;
using TransactionService.Domain.Enum;

namespace TransactionService.Application.UseCase
{
    public class UpdateStatusUseCase : IUpdateStatusUseCase
    {

        private readonly ITransactionPersistence _transactionPersistence;
        public UpdateStatusUseCase(ITransactionPersistence transactionPersistence)
        {
            _transactionPersistence = transactionPersistence;
        }

        public async Task<Result<string>> ExecuteAsync(string transactionId, TransactionOrderStatus status)
        {
            try
            {
                await _transactionPersistence.UpdateTransactionStatusAsync(transactionId, status);
                return Result<string>.Ok("Status updated successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.NOk(ex.Message);
            }
        }
    }
}
