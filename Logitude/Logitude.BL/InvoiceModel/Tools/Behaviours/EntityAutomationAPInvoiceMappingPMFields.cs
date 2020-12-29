using Logitude.BL.ExternalService;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours
{
    public class EntityAutomationAPInvoiceMappingPMFields : IEntityAutomationMappingPMFields
    {
        public void Map<T1, T2>(T1 entityPM, T2 oldEntityPM)
        {
            APInvoicePM myEntityPM = entityPM as APInvoicePM;
            APInvoicePM myOldEntityPM = oldEntityPM as APInvoicePM;
            myOldEntityPM.VendorContactId = myEntityPM.VendorContactId;
            myOldEntityPM.VendorVatNumber = myEntityPM.VendorVatNumber;
            CardRepository cardRepository = new CardRepository(myEntityPM.Tenant);
            Card card = cardRepository.GetSingleCard(myEntityPM.VendorId, myEntityPM.Tenant);
            myEntityPM.VendorContactId = card != null ? card.PrimaryContactId : null;
            myEntityPM.VendorVatNumber = card != null ? card.VatNumber : null;

        }
    }
}
