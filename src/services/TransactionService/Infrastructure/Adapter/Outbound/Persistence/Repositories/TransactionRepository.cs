using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Port.Outbound;
using TransactionService.Domain.Enum;
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

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(bool isOwner, string userId)
        {
            var transactions = await _dbContext.Transactions
                .Where(t => isOwner ? t.OwnerId == userId : t.ReceiverId == userId)
                .ToListAsync();

            return transactions;
        }

        public async Task<Transaction> SaveTransactionAsync(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateTransactionStatusAsync(string transactionId, TransactionOrderStatus status)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            transaction.Status = status;
            await _dbContext.SaveChangesAsync();
        }
    }
}
