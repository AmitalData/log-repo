using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using System.Configuration;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial  class SupplierInvoiceItemVehicleModUpdateService
    {

        int? maxCounter;
        protected override void OnCreating(SupplierInvoiceItemVehicleModPM entityPM, SupplierInvoiceItemVehiclePM entityParentPM)
       {
           entityPM.DeclarationId = entityParentPM.DeclarationId;
           entityPM.InvoiceCounterKey = entityParentPM.InvoiceCounterKey;
           entityPM.InvoiceItemLineNumber = entityParentPM.InvoiceItemLineNumber;
           entityPM.VehicleLineNumber = entityParentPM.LineNumber;
            var useMaxCounter = true;// ConfigurationManager.AppSettings["20180130.TestFeatures"]=="1";
            if (useMaxCounter)
            {

                LogMessagingUtil.Instance.AppendLine("DSV - משוב להצהרה - ניתוח נכשל - >>  Suppress AMINET_MAIN.USP_GETNEXTTABLECODEVALUE");
                if (!this.maxCounter.HasValue)
                {

                    var myRepo = (this.Repository as SupplierInvoiceItemVehicleModRepository) ?? new SupplierInvoiceItemVehicleModRepository(EntityPM.Tenant);
                    this.maxCounter = myRepo.GetMaxLineNumber(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.InvoiceItemLineNumber, entityPM.VehicleLineNumber, entityPM.Tenant);
                }
                entityPM.LineNumber = maxCounter.Value + 1;
                maxCounter = entityPM.LineNumber;
            }
            else
            {
                entityPM.LineNumber = CodeCounter.GetNumber("Customs.SupplierInvoiceItemVehicleMod", entityPM.Tenant);
            }

           base.OnCreating(entityPM, entityParentPM);
       }

       public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as SupplierInvoiceItemVehicleModRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleModRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
