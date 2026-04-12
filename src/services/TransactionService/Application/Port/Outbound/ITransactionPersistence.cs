using TransactionService.Domain.Enum;
using TransactionService.Domain.Models;

namespace TransactionService.Application.Port.Outbound
{
    public interface ITransactionPersistence
    {
        public Task<Transaction> SaveTransactionAsync(
            Transaction transaction
        );

        public Task DeleteTransactionAsync(
            string transactionId
        );

        public Task<List<Transaction>> GetTransactionsByUserIdAsync(
            bool isOwner,
            string userId
        );

        public Task UpdateTransactionStatusAsync(
            string transactionId,
            TransactionOrderStatus status
        );
    }
}
