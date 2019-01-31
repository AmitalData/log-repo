using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class AWBMessagingStockService
    {
        private int tenant;
        private bool isNewEntity;
        private Contact loggedContact;
        public AWBMessagingStock entityPoco { get; set; }
        private AWBMessagingStockPM entityPM;
        private IShipmentsContext objectContext;
        private AWBMessagingStockRepository entityRepository;
        public AWBMessagingStockService(IShipmentsContext objectContext, AWBMessagingStockPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.DummyTenant;
            this.objectContext = objectContext;
            this.entityRepository = new AWBMessagingStockRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        public void Create()
        {
            this.isNewEntity = true;
            this.entityPM.Id = IdCounter.GetNumber("AWBMessagingStock", tenant).ToString();
            this.entityPoco = new AWBMessagingStock()
            {
                Id = entityPM.Id,
                TenantNumber = entityPM.TenantNumber
            };

            this.InitializeComponent();

            AWBMessagingStockTracing.Trace(entityPM, entityPoco, loggedContact.Id, isNewEntity);
            ShipmentMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.entityPoco = entityRepository.GetSingleAWBMessagingStock(entityPM.Id);
            
            this.InitializeComponent();

            AWBMessagingStockTracing.Trace(entityPM, entityPoco, loggedContact.Id, isNewEntity);
            ShipmentMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;
                entityPM.Remaining = entityPM.Amount;
                entityPM.Status = "New";
            }

            else
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                int usedCount = entityPM.StockUsageHistories.Count;
                entityPM.Remaining = entityPM.Amount - usedCount;
            }
        }
    }
}