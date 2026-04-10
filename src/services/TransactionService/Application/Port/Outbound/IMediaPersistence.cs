using TransactionService.Domain.Models;

namespace TransactionService.Application.Port.Outbound
{
    public interface IMediaPersistence
    {
        public Task<Media> SaveMediaAsync(
            Media media
        );

        public Task<Media?> GetMediaByTransactionIdAsync(
            string transactionId
        );

        public Task DeleteMediaByTransactionIdAsync(
            string transactionId
        );
    }
}
