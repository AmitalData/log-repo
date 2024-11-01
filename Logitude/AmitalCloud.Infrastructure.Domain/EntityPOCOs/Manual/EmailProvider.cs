using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EmailProvider
    {
        [Key]
        public string ProviderNumber { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Status { get; set; }
        public DateTime? LastTestSendDate { get; set; }
        public DateTime? LastTestReceivedDate { get; set; }
        public bool SupportsEmailDelivery { get; set; }
    }
}
