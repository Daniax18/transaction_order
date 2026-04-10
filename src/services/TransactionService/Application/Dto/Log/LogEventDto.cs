using System.ComponentModel.DataAnnotations;

namespace TransactionService.Application.Dto.Log
{
    public class LogEventDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        public string ServiceName { get; set; } = "TRANSACTION-SERVICE";

        [Required]
        public string ActionName { get; set; } = string.Empty;

        [Required]
        public string ActionStatus { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; } = DateTime.UtcNow;
    }
}
