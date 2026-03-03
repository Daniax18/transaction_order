using AuditService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.Infrastructure.Services
{
    internal class AuditProcessor : IAuditProcessor
    {
        private readonly object _fileLock = new object();
        private readonly string _logFilePath = "mainapp.log";
        public async Task ProcessAuditEventAsync(string userId, string serviceName, string actionName, string actionStatus, DateTime actionTime)
        {
            string logEntry = $"{actionTime:yyyy-MM-dd HH:mm:ss} | User: {userId} | Service: {serviceName} | Action: {actionName} | Status: {actionStatus}";
            lock (_fileLock)
            {
                RotateLogIfNeeded();

                System.IO.File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }

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
