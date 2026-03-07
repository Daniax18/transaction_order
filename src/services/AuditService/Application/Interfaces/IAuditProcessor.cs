using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.Application.Interfaces
{
    public interface IAuditProcessor
    {
           Task ProcessAuditEventAsync(string userId, string serviceName, string actionName, string actionStatus, DateTime actionTime);
    }
}
