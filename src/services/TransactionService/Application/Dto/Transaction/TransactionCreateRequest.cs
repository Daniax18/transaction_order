using System.ComponentModel.DataAnnotations;

namespace TransactionService.Application.Dto.Transaction
{
    public class TransactionCreateRequest
    {
        [Required]
        public string OwnerId { get; set; } = string.Empty;

        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int Validity { get; set; }

        [Required]
        public string VideoHash { get; set; } = string.Empty;

        [Required]
        public string VideoSignature { get; set; } = string.Empty;

        [Required]
        public string SignatureKeyId { get; set; } = string.Empty;

        [Required]
        public string PublicKey { get; set; } = string.Empty;

        [Required]
        public IFormFile? MediaFile { get; set; }

    }
}
