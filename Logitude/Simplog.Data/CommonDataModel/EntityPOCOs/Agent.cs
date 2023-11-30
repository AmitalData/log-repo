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
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }

        public virtual Card Card { get; set; }
    }
}