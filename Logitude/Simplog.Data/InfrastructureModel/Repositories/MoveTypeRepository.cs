using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class MoveTypeRepository:IRepository<MoveType>
    {
        IWebFreightContext webFreightContext;

        public MoveTypeRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public MoveTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public MoveTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<MoveType> GetMoveTypesByName(string name,int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in context.MoveTypes
                         where a.Tenant == tenant
                         select a).AsQueryable();
            IQueryable<MoveType> query2 = null;

            if (!string.IsNullOrEmpty(name))
            {
                query2 = query.Where(d => d.MoveTypeEnglishName.ToUpper().StartsWith(name.ToUpper()));
            }

            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<MoveType> GetMoveTypes()
        {
            return context.MoveTypes;
        }


        public IQueryable<MoveType> GetMoveTypes(int tenant)
        {
            IQueryable<MoveType> moveTypes = from a in context.MoveTypes
                                             where a.Tenant == tenant
                                             select a;
            return moveTypes;
        }

        public IQueryable<MoveType> GetMoveTypesByTenant(int tenant)
        {
            IQueryable<MoveType> moveTypes = from a in context.MoveTypes
                                             where a.Tenant == tenant
                                             select a;
            return moveTypes;
        }

        public MoveType GetSingleMoveTypesByCode(string code,int tenant)
        {
            return (from a in context.MoveTypes where a.Tenant == tenant && a.Code == code select a).FirstOrDefault();             
        }

        public MoveType GetSingleMoveType(string id, int tenant)
        {
            return (from a in context.MoveTypes where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public void Add(MoveType entity)
        {
           context.MoveTypes.Add(entity);
        }

        public void Remove(MoveType entity)
        {
            context.MoveTypes.Attach(entity);
            context.MoveTypes.Remove(entity);
        }

        public void Update(MoveType entity)
        {
            context.MoveTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MoveType> All()
        {
            return context.MoveTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<MoveType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MoveType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}