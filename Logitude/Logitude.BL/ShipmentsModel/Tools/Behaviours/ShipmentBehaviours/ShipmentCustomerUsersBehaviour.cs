using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentCustomerUsersBehaviour : IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (entityPM.CustomerId != null)
            {
                if (initializer.Customer != null)
                {
                    if (entityPM.SalesmanUserId == null || entityPM.AccountManagerUserId == null)
                    {
                        this.HandleSalesman();
                        this.HandleAccountManager();
                    }
                }
            }
        }

        private void HandleSalesman()
        {
            if (initializer.IsNewEntity)
            {
                if (entityPM.SalesmanUserId == null)
                {
                    entityPM.SalesmanUserId = initializer.Customer.SalesmanUserId;
                }

                if (entityPM.SalesmanUserId == null)
                {
                    entityPM.SalesmanUserId = entityPM.CreatedByUserId;
                }
            }
        }

        private void HandleAccountManager()
        {
            if (!entityPM.IsHybrid)
            {
                if (entityPM.AccountManagerUserId == null)
                {
                    entityPM.AccountManagerUserId = initializer.Customer.AccountManagerUserId;
                }
            }
        }
    }
}
