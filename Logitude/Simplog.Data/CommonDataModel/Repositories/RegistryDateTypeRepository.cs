using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RegistryDateTypeRepository : IRepository<RegistryDateType>
    {
        ICommonDataContext commonDataContext;
        public RegistryDateTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }
        public RegistryDateTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public IQueryable<RegistryDateType> GetRegistryDateTypes()
        {
            return context.RegistryDateTypes;
        }

        public IQueryable<RegistryDateType> GetAll()
        {
            return context.RegistryDateTypes;
        }

        public RegistryDateType GetSingleRegistryDateType(string code)
        {
            return (from a in context.RegistryDateTypes where a.Code == code select a).FirstOrDefault();
        }

        public void Add(RegistryDateType entity)
        {
            commonDataContext.RegistryDateTypes.Add(entity);
        }

        public void Remove(RegistryDateType entity)
        {
            commonDataContext.RegistryDateTypes.Remove(entity);
        }

        public void Update(RegistryDateType entity)
        {
            commonDataContext.RegistryDateTypes.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<RegistryDateType> All()
        {
            return context.RegistryDateTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<RegistryDateType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RegistryDateType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
