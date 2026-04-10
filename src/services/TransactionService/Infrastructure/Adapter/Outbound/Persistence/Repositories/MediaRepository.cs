using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Port.Outbound;
using TransactionService.Domain.Models;

namespace TransactionService.Infrastructure.Adapter.Outbound.Persistence.Repositories
{
    public class MediaRepository : IMediaPersistence
    {
        private readonly AppDbContext _dbContext;

        public MediaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteMediaByTransactionIdAsync(string transactionId)
        {
            using var transactionDb = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var media = await _dbContext.Medias.FirstOrDefaultAsync(m => m.TransactionId == transactionId);
                if (media != null)
                {
                    _dbContext.Medias.Remove(media);
                    await _dbContext.SaveChangesAsync();
                    await transactionDb.CommitAsync();
                }
            }
            catch
            {
                await transactionDb.RollbackAsync();
                throw;
            }
        }

        public async Task<Media?> GetMediaByTransactionIdAsync(string transactionId)
        {
            var media = await _dbContext.Medias.FirstOrDefaultAsync(m => m.TransactionId == transactionId);
            return media;
        }

        public async Task<Media> SaveMediaAsync(Media media)
        {
            await _dbContext.Medias.AddAsync(media);
            await _dbContext.SaveChangesAsync();
            return media;
        }
    }
}
