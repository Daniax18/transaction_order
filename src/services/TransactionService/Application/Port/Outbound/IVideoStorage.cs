namespace TransactionService.Application.Port.Outbound
{
    public interface IVideoStorage
    {
        public Task<string> SaveVideoAsync(
            IFormFile mediaContent
        );

        public Task<byte[]> GetVideoAsync(
            string videoUrl
        );

        public Task DeleteVideoAsync(
            string objectName
        );
    }
}
