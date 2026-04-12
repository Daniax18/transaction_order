namespace TransactionService.Application.Port.Outbound
{
    public interface IVideoStorage
    {
        public Task<string> SaveVideoAsync(
            IFormFile mediaContent
        );

        public Task DeleteVideoAsync(
            string objectName
        );
        Task<Stream> GetStreamAsync(string objectName);
        Task<(string ContentType, long Size)> GetStatAsync(string objectName);
    }
}
