using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PaymentGatewayPartnersRepository : IRepository<PaymentGatewayPartners>
    {
        ICommonDataContext commonDataContext;

        public PaymentGatewayPartnersRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PaymentGatewayPartnersRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public PaymentGatewayPartnersRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PaymentGatewayPartners> GetPaymentGatewayPartners()
        {
            return this.context.PaymentGatewayPartners;
        }
         
        public PaymentGatewayPartners GetSinglePaymentGatewayPartner(string id)
        {
            PaymentGatewayPartners entity = (from a in context.PaymentGatewayPartners where a.Code == id select a).FirstOrDefault();                      
            return entity;
        }

        

        

        public void Add(PaymentGatewayPartners entity)
        {
            this.context.PaymentGatewayPartners.Add(entity);
        }

        public void Remove(PaymentGatewayPartners entity)
        {
            try
            {
                this.context.PaymentGatewayPartners.Attach(entity);
            }
            catch { }
            this.context.PaymentGatewayPartners.Remove(entity);
        }

        public void Update(PaymentGatewayPartners entity)
        {
            try
            {
                this.context.PaymentGatewayPartners.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<PaymentGatewayPartners> All()
        {
            return this.context.PaymentGatewayPartners.ToList<PaymentGatewayPartners>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<PaymentGatewayPartners> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PaymentGatewayPartners GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<PaymentGatewayPartners>  GetTenantListByListIds(List<string> ids)
        {

            return (from d in this.context.PaymentGatewayPartners
                    where ids.Contains(d.Code)
                    select d).ToList();
      
        }
    }
}
