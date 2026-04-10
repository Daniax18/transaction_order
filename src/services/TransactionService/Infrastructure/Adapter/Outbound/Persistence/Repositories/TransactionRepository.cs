using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Port.Outbound;
using TransactionService.Domain.Models;

namespace TransactionService.Infrastructure.Adapter.Outbound.Persistence.Repositories
{
    public class TransactionRepository : ITransactionPersistence
    {
        private readonly AppDbContext _dbContext;
        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteTransactionAsync(string transactionId)
        {
            using var transactionDb = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
                if (transaction != null)
                {
                    _dbContext.Transactions.Remove(transaction);
                    _dbContext.SaveChanges();
                    await transactionDb.CommitAsync();
                }
            }
            catch
            {
                await transactionDb.RollbackAsync();
                throw new Exception("Failed to delete transaction.");
            }
        }

        public async Task<Transaction> SaveTransactionAsync(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }
    }
}
