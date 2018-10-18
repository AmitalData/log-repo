using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class SupplierInvoiceItemsLevyUpdateService
    {

       protected override void OnCreating(SupplierInvoiceItemsLevyPM entityPM, SupplierInvoiceItemPM entityParentPM)
       {
           entityPM.DeclarationId = entityParentPM.DeclarationId;
           entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
           entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;

           entityParentPM.SupplierInvoiceItemsLevyLastLineNumber += 1;
           entityPM.LineNumber = entityParentPM.SupplierInvoiceItemsLevyLastLineNumber;



           base.OnCreating(entityPM, entityParentPM);
       }

       public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsLevyRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsLevyRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
