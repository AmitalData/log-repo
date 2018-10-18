using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceItemsConDeclarUpdateService : EntityUpdateService<SupplierInvoiceItemsConDeclar, SupplierInvoiceItemsConDeclarPM, SupplierInvoiceItemPM>
    {
        protected override void OnCreating(SupplierInvoiceItemsConDeclarPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;

            entityParentPM.SupplierInvoiceItemsConnectedDeclarationLastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.SupplierInvoiceItemsConnectedDeclarationLastLineNumber;
       
           

            base.OnCreating(entityPM, entityParentPM);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsConDeclarRepository).FastDeleteMulti(entityKeyFields);
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsConDeclarRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
