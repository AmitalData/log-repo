using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentCustomsTransmissionList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public DateTime? LastSendDate { get; set; }
        public string SentByUserId { get; set; }
        public string CommunicationLogId { get; set; }
        public string Error { get; set; }
        public string MessageCode { get; set; }
        public string Status { get; set; }
    }
}
