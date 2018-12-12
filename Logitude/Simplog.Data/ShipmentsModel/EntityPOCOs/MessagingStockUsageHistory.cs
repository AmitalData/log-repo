using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class MessagingStockUsageHistory
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string StockId { get; set; }
        public string EntityId { get; set; }
        public string EntityNumber { get; set; }
        public string MessageType { get; set; }
        public string MAWB { get; set; }
        public string HAWB { get; set; }
        public string ActionType { get; set; }
        public DateTime? FirstActionDate { get; set; }
        public DateTime? LastActionDate { get; set; }
        public string FirstActionByUserId { get; set; }
        public string LastActionByUserId { get; set; }

        [ForeignKey("StockId")]
        public MessagingStock Stock { get; set; }

        [ForeignKey("FirstActionByUserId")]
        public User FirstActionByUser { get; set; }

        [ForeignKey("LastActionByUserId")]
        public virtual User LastActionByUser { get; set; }
    }
}
