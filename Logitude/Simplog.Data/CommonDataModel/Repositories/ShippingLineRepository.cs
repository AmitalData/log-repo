using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ShippingLineRepository:IRepository<ShippingLine>
    {
        ICommonDataContext commonDataContext;



        public ShippingLineRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ShippingLineRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public int GetShippinngLinesCount(int tenant)
        {
            return (from record in context.ShippingLines.Include("Card").Include("ShippingAgent") where record.Tenant == tenant select record).Count();
        }

        public IQueryable<ShippingLine> GetShippinngLines(int tenant)
        {
            return (from record in context.ShippingLines.Include("Card").Include("ShippingAgent") where record.Tenant == tenant select record);
        }
        public IQueryable<ShippingLine> GetShippingLines(int tenant)
        {
            return (from record in context.ShippingLines.Include("Card").Include("ShippingAgent") where record.Tenant == tenant select record);
        }
        
        public ShippingLine GetSingleShippingLine(string id, int tenant)
        {
            return (from record in context.ShippingLines.Include("Card").Include("ShippingAgent") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public ShippingLine GetSingleShippingLineByCode(string code, int tenant)
        {
            return (from record in context.ShippingLines.Include("Card").Include("ShippingAgent") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(ShippingLine entity)
        {
            context.ShippingLines.Add(entity);
        }

        public void Remove(ShippingLine entity)
        {
            context.ShippingLines.Remove(entity);
        }

        public void Update(ShippingLine entity)
        {
            context.ShippingLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShippingLine> All()
        {
            return context.ShippingLines.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShippingLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShippingLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ShippingLine> GetAllTenantsShippingLinesByCode(string code)
        {
            IQueryable<ShippingLine> myResult = (from d in context.ShippingLines.Include("Card")
                                                 where d.Card != null
                                                 && d.Card.Code == code
                                                 && d.Card.PartnerTypeId == "SL"
                                                 select d);

            return myResult;

        }
    }
}