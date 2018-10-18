using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SpecialServicesRepository:IRepository<SpecialService>
    {
        IWebFreightContext webFreightContext;
        public SpecialServicesRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public SpecialServicesRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public SpecialServicesRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<SpecialService> GetSpecialServices()
        {
            return context.SpecialServices;
        }

        public IQueryable<SpecialService> GetSpecialServicesByTenant(int tenant)
        {
            IQueryable<SpecialService> specialServices = from a in context.SpecialServices
                                                         where a.Tenant == tenant
                                                         select a;
            return specialServices;
        }

        public IQueryable<SpecialService> GetSpecialServicesByCodeOrName(string code, string name,int tenant)
        {

            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in context.SpecialServices
                         where a.Tenant == tenant
                         select a).AsQueryable();//context.SpecialServices.AsQueryable();
            IQueryable<SpecialService> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.SpecialServiceEnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.SpecialServiceEnglishName.ToUpper().StartsWith(name.ToUpper()));

                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;

        }
        public void Add(SpecialService entity)
        {
            context.SpecialServices.Add(entity);

        }

        public void Remove(SpecialService entity)
        {
            context.SpecialServices.Attach(entity);
            context.SpecialServices.Remove(entity);

        }

        public void Update(SpecialService entity)
        {
            context.SpecialServices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SpecialService> All()
        {
            return context.SpecialServices.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<SpecialService> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SpecialService GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}