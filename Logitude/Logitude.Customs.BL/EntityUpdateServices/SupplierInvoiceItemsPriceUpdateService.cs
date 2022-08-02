using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class SupplierInvoiceItemsPriceUpdateService
    {

        protected override void OnCreating(SupplierInvoiceItemsPricePM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;
            int lastKey = 0;

            if (entityParentPM.SupplierInvoiceItemsPrices.Count > 0)
            {
                lastKey = entityParentPM.SupplierInvoiceItemsPrices.Max(d => d.LineNumber);
            }
            entityPM.LineNumber = lastKey + 1;

            base.OnCreating(entityPM, entityParentPM);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsPriceRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsPriceRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}
