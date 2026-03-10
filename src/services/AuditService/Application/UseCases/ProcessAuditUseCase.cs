using AuditService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.Application.UseCases
{
    public class ProcessAuditUseCase
    {
        private readonly IAuditProcessor _auditProcessor;

        public ProcessAuditUseCase(IAuditProcessor auditProcessor)
        {
            _auditProcessor = auditProcessor;
        }

        public async Task ExecuteAsync(string userId, string serviceName, string actionName, string actionStatus, DateTime actionTime)
        {
            await _auditProcessor.ProcessAuditEventAsync(userId, serviceName, actionName, actionStatus, actionTime);
        }
    }
}
