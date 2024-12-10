using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class InterestReportsConnectInvoiceUpdateService
    {

        protected override void OnCreating(InterestReportsConnectInvoicePM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("InterestReportsConnectInvoice", entityPM.Tenant);

        }



        protected override void Trace(InterestReportsConnectInvoicePM entityPM, InterestReportsConnectInvoice entityPOCO, string changesXml)
        {
            
 
        }

    }
}
