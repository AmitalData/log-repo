using System;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class InvoiceStatusDashboardModel
    {
        public string PaidStatus { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
