using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System.Security.AccessControl;
using TransactionService.Application.Port.Outbound;

namespace TransactionService.Infrastructure.Adapter.Outbound.Storage.MinioAdapter
{
    public class MinioStorageAdapter : IVideoStorage
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioOptions _options;

        public MinioStorageAdapter(IMinioClient minioClient, IOptions<MinioOptions> options)
        {
            _minioClient = minioClient;
            _options = options.Value;
        }
        public Task<byte[]> GetVideoAsync(string videoUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveVideoAsync(IFormFile mediaContent)
        {
            try{
                var beArgs = new BucketExistsArgs()
                    .WithBucket(_options.BucketName);
                bool found = await _minioClient.BucketExistsAsync(beArgs);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(_options.BucketName);
                    await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                var objectName = GenerateObjectName(mediaContent.FileName);
                // Upload the file
                var putArgs = new PutObjectArgs()
                    .WithBucket(_options.BucketName)
                    .WithObject(objectName)
                    .WithStreamData(mediaContent.OpenReadStream())
                    .WithObjectSize(mediaContent.Length)
                    .WithContentType(mediaContent.ContentType);

                await _minioClient.PutObjectAsync(putArgs).ConfigureAwait(false);
                return "Successfully uploaded " + objectName;
            }
            catch (Exception ex)
            {
                throw new Exception($"File Upload Error to bucket {_options.BucketName} : {ex.Message} ");
            }
        }

        public async Task DeleteVideoAsync(string objectName)
        {
            try
            {
                var beArgs = new BucketExistsArgs()
                    .WithBucket(_options.BucketName);
                bool found = await _minioClient.BucketExistsAsync(beArgs);
                if (found)
                {
                    var rmArgs = new RemoveObjectArgs()
                   .WithBucket(_options.BucketName)
                   .WithObject(objectName);
                    await _minioClient.RemoveObjectAsync(rmArgs).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"File Delete Error from bucket {_options.BucketName} : {ex.Message}");
            }
        }

        // ── Private ────────────────────────────────────────────────

        private static string GenerateObjectName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var date = DateTime.UtcNow.ToString("yyyy/MM/dd");
            return $"videos/{date}/{Guid.NewGuid()}{extension}";
            // → videos/2026/03/29/550e8400-e29b-41d4-a716-446655440000.mp4
        }

       
    }
}
