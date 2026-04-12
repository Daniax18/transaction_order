using System.ComponentModel.DataAnnotations;

namespace TransactionService.Application.Dto.Transaction
{
    public class TransactionVerifyRequest
    {
        public string? UserId { get; set; }

        [Required]
        public string TransactionId { get; set; } = string.Empty;

        [Required]
        public string PublicKey { get; set; } = string.Empty;
    }
}
