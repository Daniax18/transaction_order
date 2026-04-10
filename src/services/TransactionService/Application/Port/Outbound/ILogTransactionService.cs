using TransactionService.Application.Dto.Log;

namespace TransactionService.Application.Port.Outbound
{
    public interface ILogTransactionService
    {
        Task LogTransactionAsync(
            string actionName,
            bool isSuccess,
            string userName,
            string? errorMessage = null
        );
    }
}

