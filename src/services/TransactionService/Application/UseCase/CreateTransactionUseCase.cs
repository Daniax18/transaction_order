using TransactionService.Application.Dto;
using TransactionService.Application.Dto.Log;
using TransactionService.Application.Dto.Transaction;
using TransactionService.Application.Port.Inbound;
using TransactionService.Application.Port.Outbound;
using TransactionService.Domain.Models;

namespace TransactionService.Application.UseCase
{
    public class CreateTransactionUseCase : ICreateTransactionUseCase
    {

        private readonly ITransactionPersistence _transactionPersistence;
        private readonly IMediaPersistence _mediaPersistence;
        private readonly IVideoStorage _videoStorage;
        private readonly ILogTransactionService _logTransactionService;

        public CreateTransactionUseCase(
            ITransactionPersistence transactionPersistence, 
            IMediaPersistence mediaPersistence, 
            IVideoStorage videoStorage,
            ILogTransactionService logTransactionService
        )
        {
            _transactionPersistence = transactionPersistence;
            _mediaPersistence = mediaPersistence;
            _videoStorage = videoStorage;
            _logTransactionService = logTransactionService;
        }

        public async Task<Result<string>> ExecuteAsync(TransactionCreateRequest request)
        {
            var objectName = string.Empty;
            var transactionId = string.Empty;
            try
            {
                if(request.MediaFile != null)
                {
                    var transaction = new Transaction
                   (
                       request.OwnerId,
                       request.ReceiverId,
                       request.Amount,
                       request.Validity
                   );

                    transaction = await _transactionPersistence.SaveTransactionAsync(transaction);
                    transactionId = transaction.Id;

                    objectName = await _videoStorage.SaveVideoAsync(request.MediaFile);

                    var media = new Media
                    (
                        transaction.Id,
                        objectName,
                        request.VideoHash,
                        request.VideoSignature,
                        request.SignatureKeyId,
                        request.PublicKey
                    );

                    media = await _mediaPersistence.SaveMediaAsync(media);

                    // TODO : How about ACID principel, if media is not saved, we have to delete the transaction and the video from the storage
                    await _logTransactionService.LogTransactionAsync(
                        ActionType.CREATE_TRANSACTION.ToString(),
                        true,
                        request.OwnerId
                    );

                    return Result<string>.Ok(transaction.Id);
                }
                await _logTransactionService.LogTransactionAsync(
                        ActionType.CREATE_TRANSACTION.ToString(),
                        false,
                        request.OwnerId,
                        "No media file provided"
                );
                return Result<string>.NOk("Video is considered as empty at this stage");

            } catch (Exception ex)
            {
                // Log the exception
                await _logTransactionService.LogTransactionAsync(
                    ActionType.CREATE_TRANSACTION.ToString(),
                    false,
                    request.OwnerId,
                    ex.Message
                );
                await RollbackTransactionAsync(transactionId, objectName);
                return Result<string>.NOk($"An error occurred while creating the transaction: {ex.Message}");
            }
        }

        private async Task RollbackTransactionAsync(string transactionId, string objectName)
        {
            await _mediaPersistence.DeleteMediaByTransactionIdAsync(transactionId);
            await _transactionPersistence.DeleteTransactionAsync(transactionId);
            await _videoStorage.DeleteVideoAsync(objectName);
        }
    }
}
