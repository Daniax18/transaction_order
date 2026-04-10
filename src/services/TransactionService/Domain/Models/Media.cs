namespace TransactionService.Domain.Models
{
    public class Media
    {
        public string Id { get; set; }
        public string TransactionId { get; private set; } = string.Empty;
        public string FileName { get; private set; } = string.Empty;
        public string VideoHash { get; private set; } = string.Empty;
        public string VideoSignature { get; private set; } = string.Empty;
        public string SignatureKeyId { get; private set; } = string.Empty;
        public string PublicKey { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Media() { }

        public Media(
            string transactionId,
            string fileName,
            string videoHash,
            string videoSignature,
            string signatureKeyId,
            string publicKey
        )
        {
            Id = Guid.NewGuid().ToString();
            TransactionId = transactionId;
            FileName = fileName;
            VideoHash = videoHash;
            VideoSignature = videoSignature;
            SignatureKeyId = signatureKeyId;
            PublicKey = publicKey;
            CreatedAt = DateTime.UtcNow;
        }
               
    }
}
