using System.Security.Cryptography;
using TransactionService.Application.Dto;
using TransactionService.Application.Dto.Transaction;
using TransactionService.Application.Port.Inbound;
using TransactionService.Application.Port.Outbound;

namespace TransactionService.Application.UseCase
{
    public class VerifyTransactionUseCase : IVerifyTransactionUseCase
    {

        private readonly IMediaPersistence _mediaPersistence;

        public VerifyTransactionUseCase(IMediaPersistence mediaPersistence)
        {
            _mediaPersistence = mediaPersistence;
        }

        public async Task<Result<bool>> ExecuteAsync(TransactionVerifyRequest request)
        {
            var media = await _mediaPersistence.GetMediaByTransactionIdAsync(request.TransactionId);

            if (media == null)
                return Result<bool>.NOk("Media not found for the given transaction ID.");
            byte[] publicKeyBytes;
            byte[] signatureBytes;
            byte[] videoHashBytes;

            try
            {
                // Décoder la clé publique, si elle est invalide -> exception explicite
                publicKeyBytes = VerifyFormat(request.PublicKey, "PublicKey");

                // Décoder la signature; si base64 invalide -> considérer la vérification comme fausse
                signatureBytes = VerifyFormat(media.VideoSignature, "VideoSignature");

                // Décoder le hash de la vidéo; si base64 invalide -> considérer la vérification comme fausse
                videoHashBytes = VerifyFormat(media.VideoHash, "VideoHash");

            }
            catch (FormatException ex)
            {
                return Result<bool>.NOk(ex.Message);
            }

            try
            {
                using var rsa = System.Security.Cryptography.RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

                return Result<bool>.Ok(rsa.VerifyData(
                    videoHashBytes,
                    signatureBytes,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                ));
            }
            catch (Exception ex)
            {
                return Result<bool>.NOk($"An error occurred during verification: {ex.Message}");
            }
        }

        // Verify que le string est un base64 valide et retourne les bytes décodés,
        // sinon throw une exception avec un message clair
        private byte[] VerifyFormat(string base64String, string fieldName)
        {
            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Invalid {fieldName} format: {ex.Message}");
            }
        }
    }
}
