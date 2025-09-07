using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
   public class CustomerTenantAccessStatusTypeQuery
    {
       CustomerTenantAccessStatusTypeRepository repository;

  

        public CustomerTenantAccessStatusTypeQuery(int tenant)
        {
            repository = new CustomerTenantAccessStatusTypeRepository(tenant);
        }

        public CustomerTenantAccessStatusTypeQuery(CustomerTenantAccessStatusTypeRepository repository)
        {
            this.repository = repository;
        }

        public CustomerTenantAccessStatusTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.CustomerTenantAccessStatusTypes
                    where a.Code == code
                    select new CustomerTenantAccessStatusTypePM()
                    {
                        Code = a.Code,
                        EnglishName =a.EnglishName,
                        LocalName=a.LocalName,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<CustomerTenantAccessStatusTypePM> GetCustomerTenantAccessStatusTypePMs()
        {
            return from a in repository.context.CustomerTenantAccessStatusTypes
                   select new CustomerTenantAccessStatusTypePM()
                   {
                       Code = a.Code,
                       EnglishName = a.EnglishName,
                       LocalName = a.LocalName,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<CustomerTenantAccessStatusTypeList> GetIQueryableEntityList(IQueryable<CustomerTenantAccessStatusType> iQueryable)
        {
            IQueryable<CustomerTenantAccessStatusTypeList> result = from a in iQueryable
                                               select new CustomerTenantAccessStatusTypeList()
                                               {
                                                   Code = a.Code,
                                                   EnglishName = a.EnglishName,
                                                   LocalName = a.LocalName,
                                                   SearchFields = a.SearchFields,
                                               };
            return result;
        }
    }
}
