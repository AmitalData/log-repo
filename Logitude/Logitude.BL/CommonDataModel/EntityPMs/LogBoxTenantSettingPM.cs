using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class LogBoxTenantSettingPM
    {
        [Key]
        public int Id { get; set; }

        public bool IsDocumentsArchive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool CustomerTenantShareImportFile { get; set; }

       
        public bool DocumentShareAsDefault { get; set; }
       
        public string StockTypeCode { get; set; }

        public bool AutoArchiveOnInvoice { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LogBoxAdminUserId { get; set; }
    }
}
