using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UsoCFDIRepository : IRepository<UsoCFDI>
    {
        ICommonDataContext commonDataContext;



        public UsoCFDIRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public UsoCFDIRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<UsoCFDI> GetUsoCFDIs()
        {
            return context.UsoCFDIs;
        }

        public IQueryable<UsoCFDI> GetAll()
        {
            return context.UsoCFDIs;
        }

        public UsoCFDI GetSingleUsoCFDI(string code, int tenant = 0)
        {
            return (from record in context.UsoCFDIs where record.Code == code select record).FirstOrDefault();
        }

        public UsoCFDI GetSingleUsoCFDIUpdate(string code, int tenant)
        {
            return (from record in context.UsoCFDIs where record.Code == code select record).FirstOrDefault();
        }

        public void Add(UsoCFDI entity)
        {
            context.UsoCFDIs.Add(entity);
        }

        public void Remove(UsoCFDI entity)
        {
            try
            {
                context.UsoCFDIs.Attach(entity);
            }
            catch { }
            context.UsoCFDIs.Remove(entity);
        }

        public void Update(UsoCFDI entity)
        {
            try
            {
                context.UsoCFDIs.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<UsoCFDI> All()
        {
            return context.UsoCFDIs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<UsoCFDI> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public UsoCFDI GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
