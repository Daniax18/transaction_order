using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.Domain
{
    public class AuditEvent
    {
        public string UserId { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string ActionStatus { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
    }
}
