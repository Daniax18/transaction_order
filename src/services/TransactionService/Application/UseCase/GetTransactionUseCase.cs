using System.Text.Json;
using TransactionService.Application.Dto;
using TransactionService.Application.Dto.Transaction;
using TransactionService.Application.Port.Inbound;
using TransactionService.Application.Port.Outbound;

namespace TransactionService.Application.UseCase
{
    public class GetTransactionUseCase : IGetTransactionUseCase
    {
        private readonly ITransactionPersistence transactionPersistence;
        private readonly IUserService userService;
        private readonly IMediaPersistence mediaPersistence;
        public GetTransactionUseCase(
            ITransactionPersistence transactionPersistence,
            IUserService userService,
            IMediaPersistence mediaPersistence
        )
        {
            this.transactionPersistence = transactionPersistence;
            this.userService = userService;
            this.mediaPersistence = mediaPersistence;
        }

        public async Task<Result<List<TransactionGetResponse>>> ExecuteAsync(bool isOwner, string userId)
        {
            var transactionTemp = await transactionPersistence.GetTransactionsByUserIdAsync(isOwner, userId);

            if (!transactionTemp.Any())
                return Result<List<TransactionGetResponse>>.Ok(new List<TransactionGetResponse>()); ;

            var userIds = transactionTemp
                .Select(t => isOwner ? t.ReceiverId : t.OwnerId)
                .Distinct()
                .ToList();

            var userNames = await userService.GetUserNameByIdsAsync(userIds.ToArray());

            if(!userNames.IsSuccess)
                return Result<List<TransactionGetResponse>>.NOk("Error on getting names : " +userNames.Message);

            var medias = await Task.WhenAll(
                transactionTemp.Select(t => mediaPersistence.GetMediaByTransactionIdAsync(t.Id)
            ));

            var result = transactionTemp.Select((t, i) =>
            {
                var userName = userNames.Value[isOwner ? t.ReceiverId : t.OwnerId];
                return new TransactionGetResponse
                {
                    TransactionId = t.Id,
                    StatusTransaction = t.Status.ToString(),
                    UserName = userName,
                    DateTransaction = t.createdAt,
                    PublicKey = medias[i]?.PublicKey ?? "",
                    UpdatedStatusAt = (DateTime) t.updatedStatusAt
                };
            }).ToList();

            return Result<List<TransactionGetResponse>>.Ok(result);
        }
    }
}
