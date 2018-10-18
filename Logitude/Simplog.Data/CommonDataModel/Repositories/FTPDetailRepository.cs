using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FTPDetailRepository : IRepository<FTPDetail>
    {
        ICommonDataContext commonDataContext;

        public FTPDetailRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public FTPDetailRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public FTPDetailRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FTPDetail GetSingleFTPDetail(string id, int tenant)
        {
            return (from d in context.FTPDetails where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public IQueryable<FTPDetail> GetFTPDetails(int tenant)
        {
            return (from d in context.FTPDetails where d.Tenant == tenant select d);
        }
        
        public void Add(FTPDetail entity)
        {
            context.FTPDetails.Add(entity);
        }

        public void Remove(FTPDetail entity)
        {
            context.FTPDetails.Attach(entity);
            context.FTPDetails.Remove(entity);
        }

        public void Update(FTPDetail entity)
        {
            context.FTPDetails.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FTPDetail> All()
        {
            return context.FTPDetails.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FTPDetail> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public FTPDetail GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
