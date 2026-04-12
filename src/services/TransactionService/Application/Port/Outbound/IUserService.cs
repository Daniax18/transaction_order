using TransactionService.Application.Dto;

namespace TransactionService.Application.Port.Outbound
{
    public interface IUserService
    {
        // In order to avoid multiple calls to the user service, we will get all the user names by their ids in one call
        Task<Result<Dictionary<string, string>>> GetUserNameByIdsAsync(string[] userIds);
    }
}
