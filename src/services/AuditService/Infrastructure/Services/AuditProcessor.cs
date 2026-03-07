using AuditService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.Infrastructure.Services
{
    public class AuditProcessor : IAuditProcessor
    {
        private readonly object _fileLock = new object();
        private readonly string _logFilePath = "logs/mainapp.log";
        private readonly ILogger<AuditProcessor> _logger;

        public AuditProcessor(ILogger<AuditProcessor> logger)
        {
            _logger = logger;
        }

        public async Task ProcessAuditEventAsync(string userId, string serviceName, string actionName, string actionStatus, DateTime actionTime)
        {
            string logEntry = $"{actionTime:yyyy-MM-dd HH:mm:ss} | User: {userId} | Service: {serviceName} | Action: {actionName} | Status: {actionStatus}";
            _logger.LogInformation("[Audit Service] Message : " + logEntry);
            lock (_fileLock)
            {
                RotateLogIfNeeded();

                System.IO.File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            _logger.LogInformation("[Audit Service] Process end");
            await Task.CompletedTask;
        }

        private void RotateLogIfNeeded()
        {
            if (!File.Exists(_logFilePath))
                return;

            int lineCount = File.ReadLines(_logFilePath).Count();

            if (lineCount >= 500)
            {
                int index = 1;

                while (File.Exists($"mainapp{index}.log"))
                {
                    index++;
                }

                File.Move(_logFilePath, $"mainapp{index}.log");
            }
        }
    }
}
