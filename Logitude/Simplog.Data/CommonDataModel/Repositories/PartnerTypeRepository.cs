using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PartnerTypeRepository:IRepository<PartnerType>
    {
        ICommonDataContext commonDataContext;

        public PartnerTypeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public PartnerTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PartnerTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PartnerType> GetPartnerTypes()
        {
            return context.PartnerTypes;
        }
        public IQueryable<PartnerType> GetAll()
        {
            return context.PartnerTypes;
        }

        public PartnerType GetSinglePartnerType(string id)
        {
            return (from a in context.PartnerTypes
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(PartnerType entity)
        {
            context.PartnerTypes.Add(entity);
        }

        public void Remove(PartnerType entity)
        {
            context.PartnerTypes.Attach(entity);
            context.PartnerTypes.Remove(entity);
        }

        public void Update(PartnerType entity)
        {
            context.PartnerTypes.Attach(entity);
            context.SetAsModified(entity);


        }

        public List<PartnerType> All()
        {
            return context.PartnerTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PartnerType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PartnerType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
