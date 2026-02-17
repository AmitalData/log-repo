using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class LogBoxTenantSetting
    {
        [Key]
       
        public int Id { get; set; }
        

        public bool IsDocumentsArchive { get; set; }
       
        public bool CustomerTenantShareImportFile { get; set; }
        
        public string LogBoxAdminUserId { get; set; }

        [ForeignKey("LogBoxAdminUserId")]
        public Contact LogBoxAdminUser { get; set; }

        
        public bool DocumentShareAsDefault { get; set; }
       
        public string StockTypeCode { get; set; }
        public bool AutoArchiveOnInvoice { get; set; }

        public Tenant Tenant { get; set; }
    }
}