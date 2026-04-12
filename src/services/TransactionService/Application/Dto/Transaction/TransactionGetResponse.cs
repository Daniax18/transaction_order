using System.Diagnostics.CodeAnalysis;

namespace TransactionService.Application.Dto.Transaction
{
    public class TransactionGetResponse
    {
        public string TransactionId { get; set; } = string.Empty;
        public DateTime DateTransaction { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string StatusTransaction { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public DateTime UpdatedStatusAt { get; set; }
    }
}
