using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;


namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerGroupQuery
    {
        CustomerGroupRepository repository;
        public CustomerGroupQuery()
        {
            repository = new CustomerGroupRepository();
        }
        public CustomerGroupQuery(int tenant)
        {
            repository = new CustomerGroupRepository(tenant);
        }
        public CustomerGroupQuery(CustomerGroupRepository repository)
        {
            this.repository = repository;
        }

        public CustomerGroupPM GetSinglePM(string id, int tenant)
        {
            CustomerGroupPM customerGroup = (from a in repository.context.CustomerGroups
                                           where a.Id == id && a.Tenant == tenant
                                           select new CustomerGroupPM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               Name = a.Name,
                                               Description = a.Description,
                                               InActive = a.InActive,
                                               CreateDate = a.CreateDate,
                                               UpdateDate = a.UpdateDate,
                                               CreatedByUserId = a.CreatedByUserId,
                                               UpdatedByUserId = a.UpdatedByUserId,
                                           }).FirstOrDefault();

            return customerGroup;
        }

        public IQueryable<CustomerGroupPM> GetCustomerGroupPMsByTenant(int tenant)
        {
            IQueryable<CustomerGroupPM> customerGroups = from a in repository.context.CustomerGroups
                                                       where a.Tenant == tenant
                                                       select new CustomerGroupPM()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           Name = a.Name,
                                                           Description = a.Description,
                                                           InActive = a.InActive,
                                                           CreateDate = a.CreateDate,
                                                           UpdateDate = a.UpdateDate,
                                                           CreatedByUserId = a.CreatedByUserId,
                                                           UpdatedByUserId = a.UpdatedByUserId,
                                                       };

            return customerGroups;
        }


        public IQueryable<CustomerGroupList> GetIQueryableEntityList(IQueryable<CustomerGroup> iQueryable)
        {
            IQueryable<CustomerGroupList> result = from a in iQueryable
                                                  select new CustomerGroupList()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      Name = a.Name,
                                                      Description = a.Description,
                                                      InActive = a.InActive,
                                                      CreateDate = a.CreateDate,
                                                      UpdateDate = a.UpdateDate,
                                                      CreatedByUserId = a.CreatedByUserId,
                                                      UpdatedByUserId = a.UpdatedByUserId,
                                                  };
            return result;
        }
    }
}
