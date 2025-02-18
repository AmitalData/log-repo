using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.CommonDataModel.Repositories
{
   public class CargoTenantMilestoneDefinitionRepository : IRepository<CargoTenantMilestoneDefinition>
    {
        ICommonDataContext commonDataContext;

        public CargoTenantMilestoneDefinitionRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CargoTenantMilestoneDefinitionRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CargoTenantMilestoneDefinitionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CargoTenantMilestoneDefinition GetSingleCargoTenantMilestoneDefinition(string id, int tenant)
        {
            return (from d in context.CargoTenantMilestoneDefinitions where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public CargoTenantMilestoneDefinition GetSingleCargoTenantMilestoneDefinitionByCodeAndTenant(string code, int tenant)
        {
            return (from d in context.CargoTenantMilestoneDefinitions where d.Code == code && d.Tenant == tenant select d).FirstOrDefault();
        }

        public List<CargoTenantMilestoneDefinition> GetAll(int tenant)
        {
            return (from d in context.CargoTenantMilestoneDefinitions where d.Tenant == tenant select d).ToList();
        }
        public void Add(CargoTenantMilestoneDefinition entity)
        {
            context.CargoTenantMilestoneDefinitions.Add(entity);
        }

        public void Remove(CargoTenantMilestoneDefinition entity)
        {
            try
            {
                context.CargoTenantMilestoneDefinitions.Attach(entity);
            }
            catch
            {

            }
            context.CargoTenantMilestoneDefinitions.Remove(entity);
        }

        public void Update(CargoTenantMilestoneDefinition entity)
        {
            try
            {
                context.CargoTenantMilestoneDefinitions.Attach(entity);
            }
            catch
            {

            }
            context.SetAsModified(entity);
        }

        public List<CargoTenantMilestoneDefinition> All()
        {
            return context.CargoTenantMilestoneDefinitions.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CargoTenantMilestoneDefinition> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CargoTenantMilestoneDefinition GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
