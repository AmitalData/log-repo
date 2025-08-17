using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ResponsibilityRepository : IRepository<Responsibility>
    {
        ICommonDataContext commonDataContext;



        public ResponsibilityRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ResponsibilityRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public Responsibility GetSingleResponsibility(string code)
        {
            return (from a in context.Responsibilities
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public IQueryable<Responsibility> GetAll()
        {
            return from a in context.Responsibilities
                   select a;
        }

        public IQueryable<Responsibility> GetResponsibilities()
        {
            return from a in context.Responsibilities
                   select a;
        }

        public Responsibility GetSingle(EntityKeyFields entityKeys)
        {
            //ResponsibilityKeys keys = entityKeys as ResponsibilityKeys;
            //return (from a in context.Responsibilities
            //        where a.Code == keys.Code
            //        select a).FirstOrDefault();
            throw new NotImplementedException();
        }

        public void Add(Responsibility entity)
        {
            context.Responsibilities.Add(entity);
        }

        public void Remove(Responsibility entity)
        {
            context.Responsibilities.Attach(entity);
            context.Responsibilities.Remove(entity);
        }
        public void Update(Responsibility entity)
        {
            context.Responsibilities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Responsibility> All()
        {
            return context.Responsibilities.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Responsibility> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }
    }
}