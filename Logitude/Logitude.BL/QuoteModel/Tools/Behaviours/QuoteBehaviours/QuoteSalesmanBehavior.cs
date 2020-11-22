using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Initializers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.Behaviours.QuoteBehaviours
{
    public class QuoteSalesmanBehavior : IServiceBehaviour
    {
        private QuotePM entityPM;

        private QuoteServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (QuoteServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                if (entityPM.SalesmanUserId == null)
                {
                    GetCustomerSalesman();
                }

                if (entityPM.SalesmanUserId == null)
                {
                    entityPM.SalesmanUserId = entityPM.CreatedByUserId;
                }

                if (entityPM.BusinessUnitId == null)
                {
                    GetSalesmanBusinessUnit();
                }
            }
        }

        private void GetCustomerSalesman()
        {
            if (entityPM.CustomerId != null)
            {
                var output = (from d in initializer.CommonContext.CustomerSalesmanByProducts
                              where d.Tenant == initializer.Tenant
                              && d.ProductTypeCode == entityPM.ProductCode
                              && d.CustomerId == entityPM.CustomerId
                              select d.SalesmanUserId).FirstOrDefault();

                if (output == null)
                {
                    output = (from d in initializer.CommonContext.Customers
                              where d.Tenant == initializer.Tenant
                              && d.Id == entityPM.CustomerId
                              select d.SalesmanUserId).FirstOrDefault();
                }

                if (output != null)
                {
                    entityPM.SalesmanUserId = output;
                }
            }
        }

        private void GetSalesmanBusinessUnit()
        {
            if (entityPM.SalesmanUserId != null)
            {
                UserRepository userRepository = new UserRepository(initializer.CommonContext);
                User user = userRepository.GetSingleUser(entityPM.SalesmanUserId, initializer.Tenant, false);
                if (user != null)
                {
                    entityPM.BusinessUnitId = user.BusinessUnitId;
                }
            }
        }
    }
}
