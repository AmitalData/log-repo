using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class IndustryRepository : IRepository<Industry>
    {
        ICommonDataContext commonDataContext;



        public IndustryRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IndustryRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Industry> GetIndustries(int tenant)
        {
            return (from record in context.Industries where record.Tenant == tenant select record);
        }

        public Industry GetSingleIndustryByCode(string code, int tenant)
        {
            return (from f in context.Industries where f.Code == code && f.Tenant == tenant select f).FirstOrDefault();
        }

        public Industry GetSingleIndustry(string id, int tenant)
        {
            return (from record in context.Industries where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(Industry entity)
        {
            this.context.Industries.Add(entity);
        }

        public void Remove(Industry entity)
        {
            try
            {
                this.context.Industries.Attach(entity);
            }
            catch { }
            this.context.Industries.Remove(entity);

        }

        public void Update(Industry entity)
        {
            try
            {
                this.context.Industries.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<Industry> All()
        {
            return this.context.Industries.ToList<Industry>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<Industry> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Industry GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
