using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Agent
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CASSCode { get; set; }
        public string IATACode { get; set; }
        public string RegulatedAgentCode { get; set; }
        public string AgentSharedLogisticsKey { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
        public bool BlockNewInvoiceCreation { get; set; }
        public bool BlockNewShipmentCreation { get; set; }      
        public string PrimaryContactName { get; set; }        
        public string PrimaryContactEmail { get; set; }        
        public string PrimaryContactPhone { get; set; }

        public virtual Card Card { get; set; }
    }
}