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
    }
}
