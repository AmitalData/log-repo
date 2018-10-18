using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceItemsSerialNumUpdateService
    {
        protected override void OnCreating(SupplierInvoiceItemsSerialNumPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;

            entityParentPM.SupplierInvoiceItemsSerialNumberLastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.SupplierInvoiceItemsSerialNumberLastLineNumber;
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsSerialNumRepository).FastDeleteMulti(entityKeyFields);
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsSerialNumRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
