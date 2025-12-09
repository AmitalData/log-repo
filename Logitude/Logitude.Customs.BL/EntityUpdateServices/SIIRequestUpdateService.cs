 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class SIIRequestUpdateService
   {

        protected override void OnCreating(SIIRequestPM entityPM, EntityPM entityParentPM)
        {
            if (string.IsNullOrEmpty(entityPM.Id))
                entityPM.Id = IdCounter.GetNumber("Customs.SIIRequest", entityPM.Tenant);
            entityPM.RequestDate = DateTime.Now;
        }
        protected override void UpdateComposition(SIIRequestPM entityPM)
        {
            SupplierInvoiceItemsReqListUpdateService supplierInvoiceItemsReqListUpdateService = new SupplierInvoiceItemsReqListUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            supplierInvoiceItemsReqListUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsReqLists, entityPM.DeletedSupplierInvoiceItemsReqLists, entityPM, true);

            base.UpdateComposition(entityPM);
        }

    }

}
	 