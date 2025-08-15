using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
   public class CustomerOpenFilesAmountRepository  : IRepository<CustomerOpenFilesAmount>
    {
        ICommonDataContext commonDataContext;



        public CustomerOpenFilesAmountRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerOpenFilesAmountRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }



        public CustomerOpenFilesAmount GetSingleCustomerOpenFilesAmount(string customerId, int tenant)
        {
            return (from record in context.CustomerOpenFilesAmounts where record.CustomerId == customerId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(CustomerOpenFilesAmount entity)
        {
            this.context.CustomerOpenFilesAmounts.Add(entity);
        }

        public void Remove(CustomerOpenFilesAmount entity)
        {
            try
            {
                this.context.CustomerOpenFilesAmounts.Attach(entity);
            }

            catch
            {

            }

            this.context.CustomerOpenFilesAmounts.Remove(entity);
        }

        public void Update(CustomerOpenFilesAmount entity)
        {
            try
            {
                this.context.CustomerOpenFilesAmounts.Attach(entity);
            }

            catch
            {

            }

            this.context.SetAsModified(entity);
        }

        public List<CustomerOpenFilesAmount> All()
        {
            return this.context.CustomerOpenFilesAmounts.ToList<CustomerOpenFilesAmount>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<CustomerOpenFilesAmount> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerOpenFilesAmount GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
