namespace TransactionService.Infrastructure.Adapter.Outbound.Storage.MinioAdapter
{
    public class MinioOptions
    {
        public const string Section = "Minio";
        public string Endpoint { get; init; } = string.Empty;
        public string AccessKey { get; init; } = string.Empty;
        public string SecretKey { get; init; } = string.Empty;
        public string BucketName { get; init; } = string.Empty;
        public bool UseSSL { get; init; } = false;
    }
}
