using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PaymentGatewayPartnerRepository : IRepository<PaymentGatewayPartner>
    {
        ICommonDataContext commonDataContext;

        public PaymentGatewayPartnerRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public PaymentGatewayPartnerRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PaymentGatewayPartner> GetPaymentGatewayPartners()
        {
            return this.context.PaymentGatewayPartners;
        }
         
        public PaymentGatewayPartner GetSinglePaymentGatewayPartner(string id)
        {
            PaymentGatewayPartner entity = (from a in context.PaymentGatewayPartners where a.Code == id select a).FirstOrDefault();                      
            return entity;
        }


        public IQueryable<PaymentGatewayPartner> GetAll()
        {
            return commonDataContext.PaymentGatewayPartners;
        }



        public void Add(PaymentGatewayPartner entity)
        {
            this.context.PaymentGatewayPartners.Add(entity);
        }

        public void Remove(PaymentGatewayPartner entity)
        {
            try
            {
                this.context.PaymentGatewayPartners.Attach(entity);
            }
            catch { }
            this.context.PaymentGatewayPartners.Remove(entity);
        }

        public void Update(PaymentGatewayPartner entity)
        {
            try
            {
                this.context.PaymentGatewayPartners.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<PaymentGatewayPartner> All()
        {
            return this.context.PaymentGatewayPartners.ToList<PaymentGatewayPartner>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<PaymentGatewayPartner> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PaymentGatewayPartner GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<PaymentGatewayPartner>  GetTenantListByListIds(List<string> ids)
        {

            return (from d in this.context.PaymentGatewayPartners
                    where ids.Contains(d.Code)
                    select d).ToList();
      
        }
    }
}
