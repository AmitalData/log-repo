using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools.Utils;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceItemVehicleAddUpdateService : EntityUpdateService<SupplierInvoiceItemVehicleAdd, SupplierInvoiceItemVehicleAddPM, SupplierInvoiceItemVehiclePM>
    {// moran 14.3.16 - AMI-55746

       // protected override void OnCreating(SupplierInvoiceItemVehicleAddPM entityPM, EntityPM entityPPM)
       // {

       //     var entityParentPM = entityPPM as SupplierInvoiceItemVehiclePM;
       //    entityPM.DeclarationId = entityParentPM.DeclarationId;
       //    entityPM.InvoiceCounterKey = entityParentPM.InvoiceCounterKey;
       //    entityPM.InvoiceItemLineNumber = entityParentPM.InvoiceItemLineNumber;
       //    entityPM.LineNumber = entityParentPM.LineNumber;
       //    base.OnCreating(entityPM, entityParentPM);
       //}
        // mohammad fix bug on uti.
        protected override void OnCreating(SupplierInvoiceItemVehicleAddPM entityPM, SupplierInvoiceItemVehiclePM entityParentPM)
        {
           entityPM.DeclarationId = entityParentPM.DeclarationId;
           entityPM.InvoiceCounterKey = entityParentPM.InvoiceCounterKey;
           entityPM.InvoiceItemLineNumber = entityParentPM.InvoiceItemLineNumber;
           entityPM.LineNumber = entityParentPM.LineNumber;
           base.OnCreating(entityPM, entityParentPM);
       }
        protected override void OnUpdating(SupplierInvoiceItemVehicleAddPM entityPM, SupplierInvoiceItemVehicleAdd entityPOCO)
        {
            try
            {
                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<SupplierInvoiceItemVehicleAddPM, SupplierInvoiceItemVehicleAdd>("2018062018HD312280.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
            
        }
        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as SupplierInvoiceItemVehicleAddRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleAddRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
