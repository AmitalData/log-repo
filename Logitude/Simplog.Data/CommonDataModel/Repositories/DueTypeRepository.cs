using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DueTypeRepository:IRepository<DueType>
    {
        ICommonDataContext commonDataContext;
        


        public DueTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DueTypeRepository(ICommonDataContext context)
        {
            commonDataContext= context;
        }

        public IQueryable<DueType> GetDueTypes()
        {
            return context.DueTypes;
        }

        public IQueryable<DueType> GetAll()
        {
            return context.DueTypes;
        }

        public DueType GetSingleDueType(string code,int tenant = 0)
        {
            return (from record in context.DueTypes where record.Code == code select record).FirstOrDefault();
        }

        public  DueType GetSingleDueTypeUpdate(string code, int tenant)
        {
            return (from record in context.DueTypes where record.Code == code select record).FirstOrDefault();
        }

        public void Add(DueType entity)
        {
            context.DueTypes.Add(entity);
        }

        public void Remove(DueType entity)
        {
            try
            {
                context.DueTypes.Attach(entity);
            }
            catch { }
            context.DueTypes.Remove(entity);
        }

        public void Update(DueType entity)
        {
            try
            {
                context.DueTypes.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<DueType> All()
        {
            return context.DueTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DueType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DueType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
