using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class MessagingStockUsageHistoryList
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
        public string LastActionByUserName { get; set; }
    }
}