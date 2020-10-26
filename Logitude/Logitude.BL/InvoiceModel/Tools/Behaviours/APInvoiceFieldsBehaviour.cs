using Logitude.BL.ExternalService;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours
{
    public class APInvoiceFieldsBehaviour : IServiceBehaviour
    {
        private APInvoicePM entityPM;

        private APInvoiceServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (APInvoiceServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                InitializeOnCreating();
            }

            entityPM.UpdateDate = initializer.TodayDateTime;

            if (entityPM.DueDate != null)
            {
                entityPM.DueDate = entityPM.DueDate.Value.Date;
            }

            if (entityPM.InvoiceDate != null)
            {
                entityPM.InvoiceDate = entityPM.InvoiceDate.Value.Date;
            }

            if (string.IsNullOrEmpty(entityPM.InternalNumber))
            {
                entityPM.InternalNumber = TableCounter.GetNumber(entityPM.Tenant, "APIC", "IN", null);
            }
        }

        private void InitializeOnCreating()
        {
            entityPM.CreateDate = initializer.TodayDateTime;
        }
    }

    public class EntityAutomationAPInvoiceMappingPMFields : IEntityAutomationMappingPMFields
    {
        public void Map<T1,T2>(T1 poco, T2 oldPMEntity) 
        {
            APInvoicePM myOldPMEntity = oldPMEntity as APInvoicePM;
            APInvoice myPoco = poco as APInvoice;
            CardRepository cardRepository = new CardRepository(myPoco.Tenant);
            Card card = cardRepository.GetSingleCard(myPoco.VendorId, myPoco.Tenant);
            myOldPMEntity.VendorContactId = card != null ? card.PrimaryContactId : null;
            myOldPMEntity.VendorVatNumber = card != null ? card.VatNumber : null;

        }
    }
}
