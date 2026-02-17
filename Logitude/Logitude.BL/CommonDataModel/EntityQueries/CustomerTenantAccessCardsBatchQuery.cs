using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerTenantAccessCardsBatchQuery
    {
        CustomerTenantAccessCardsBatchRepository repository;

        public CustomerTenantAccessCardsBatchQuery()
        {
            this.repository = new CustomerTenantAccessCardsBatchRepository();
        }

        public CustomerTenantAccessCardsBatchQuery(int tenant)
        {
            this.repository = new CustomerTenantAccessCardsBatchRepository(tenant);
        }

        public CustomerTenantAccessCardsBatchQuery(CustomerTenantAccessCardsBatchRepository repository)
        {
            this.repository = repository;
        }

        public CustomerTenantAccessCardsBatchPM GetSinglePM(string customerid,string customertenantaccessid, string batchnumber,int Tenant)
        {
            CustomerTenantAccessCardsBatchPM result = null;
            CustomerTenantAccessCardsBatch entityPoco = repository.GetSingleCustomerTenantAccessCardsBatch(customertenantaccessid, customerid,batchnumber,Tenant);

            if (entityPoco != null)
            {
                result = new CustomerTenantAccessCardsBatchPM()
                {
                    CustomerId = entityPoco.CustomerId,
                    Tenant = entityPoco.Tenant,
                    BatchNumber = entityPoco.BatchNumber,
                    CreateDateTime = entityPoco.CreateDateTime,
                    DoneDate = entityPoco.DoneDate,
                    CustomerTenantAccessId = entityPoco.CustomerTenantAccessId,
                    FromDatetime = entityPoco.FromDatetime,
                    Status = entityPoco.Status,
                    ToDatetime = entityPoco.ToDatetime,
                    TotalFailed = entityPoco.TotalFailed,
                    TotalShipment = entityPoco.TotalShipment,
                    Totalsucceeded = entityPoco.Totalsucceeded,
                };
            }

            return result;
        }

        public IQueryable<CustomerTenantAccessCardsBatchList> GetIQueryableEntityList(IQueryable<CustomerTenantAccessCardsBatch> iQueryable)
        {
            IQueryable<CustomerTenantAccessCardsBatchList> result = from entity in iQueryable
                                               select new CustomerTenantAccessCardsBatchList()
                                               {
                                                   CustomerId = entity.CustomerId,
                                                   Tenant = entity.Tenant,
                                                   BatchNumber = entity.BatchNumber,
                                                   CreateDateTime = entity.CreateDateTime,
                                                   DoneDate = entity.DoneDate,
                                                   CustomerTenantAccessId = entity.CustomerTenantAccessId,
                                                   FromDatetime = entity.FromDatetime,
                                                   Status = entity.Status,
                                                   ToDatetime = entity.ToDatetime,
                                                   TotalFailed = entity.TotalFailed,
                                                   TotalShipment = entity.TotalShipment,
                                                   Totalsucceeded = entity.Totalsucceeded,

                                               };
            return result;
        }

    }
}
