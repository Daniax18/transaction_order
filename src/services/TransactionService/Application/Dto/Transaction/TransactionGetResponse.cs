using System.Diagnostics.CodeAnalysis;

namespace TransactionService.Application.Dto.Transaction
{
    public class TransactionGetResponse
    {
        public string TransactionId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;

        public string ObjectName { get; set; } = string.Empty;

        public DateTime? UpdatedStatusAt { get; set; }

        public decimal Amount { get; set; }
    }
}
