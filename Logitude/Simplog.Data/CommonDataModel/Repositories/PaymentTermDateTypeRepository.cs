using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PaymentTermDateTypeRepository : IRepository<PaymentTermDateType>
    {
        ICommonDataContext Context;
        public PaymentTermDateTypeRepository()
        {
            Context = new CommonDataContext();

        }
        public PaymentTermDateTypeRepository(ICommonDataContext context)
        {
            Context = context;

        }
        public PaymentTermDateTypeRepository(int tenant)
        {
            Context = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PaymentTermDateType> GetPaymentTermDateTypes()
        {
            return context.PaymentTermDateTypes;
        }

        public IQueryable<PaymentTermDateType> GetAll()
        {
            return context.PaymentTermDateTypes;
        }

        public PaymentTermDateType GetSinglePaymentTermDateType(string code)
        {
            return (from a in context.PaymentTermDateTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(PaymentTermDateType entity)
        {
            context.PaymentTermDateTypes.Add(entity);
        }

        public void Remove(PaymentTermDateType entity)
        {
            context.PaymentTermDateTypes.Remove(entity);
        }

        public void Update(PaymentTermDateType entity)
        {
            context.PaymentTermDateTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentTermDateType> All()
        {
            return context.PaymentTermDateTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return Context; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<PaymentTermDateType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PaymentTermDateType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
