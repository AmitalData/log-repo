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
    public class AdditionalServiceRepository: IRepository<AdditionalService>
    {
        ICommonDataContext commonDataContext;


        public AdditionalServiceRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AdditionalServiceRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AdditionalService> GetAdditionalServices(int tenant)
        {
            return (from r in context.AdditionalServices where r.Tenant == tenant select r);
        }
        
        public AdditionalService GetSingleAdditionalService(string id, int tenant)
        {
            return (from record in context.AdditionalServices where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();          
        }
        
        public void Add(AdditionalService entity)
        {
            this.context.AdditionalServices.Add(entity);
        }

        public void Remove(AdditionalService entity)
        {
            try
            {
                this.context.AdditionalServices.Attach(entity);
            }

            catch
            {

            }

            this.context.AdditionalServices.Remove(entity);
        }

        public void Update(AdditionalService entity)
        {
            try
            {
                this.context.AdditionalServices.Attach(entity);
            }

            catch 
            { 

            }

            this.context.SetAsModified(entity);
        }

        public List<AdditionalService> All()
        {
            return this.context.AdditionalServices.ToList<AdditionalService>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<AdditionalService> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AdditionalService GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}