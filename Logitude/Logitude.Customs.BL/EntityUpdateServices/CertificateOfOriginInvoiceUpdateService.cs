using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;


namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CertificateOfOriginInvoiceUpdateService
	{
        protected override void OnCreating(CertificateOfOriginInvoicePM entityPM, CertificateOfOriginPM entityParentPM)
        {
            entityPM.CertificateOfOriginId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
        }
 
    }
}
