using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class SupplierInvoiceItemModVehicleUpdateService
    {

       protected override void OnCreating(SupplierInvoiceItemModVehiclePM entityPM, SupplierInvoiceItemPM entityParentPM)
       {
           entityPM.DeclarationId = entityParentPM.DeclarationId;
           entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
           entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;
          
           base.OnCreating(entityPM, entityParentPM);
       }

       public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as SupplierInvoiceItemModVehicleRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemModVehicleRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
