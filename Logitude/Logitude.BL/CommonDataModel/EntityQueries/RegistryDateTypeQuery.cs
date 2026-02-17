using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class RegistryDateTypeQuery
    {
        RegistryDateTypeRepository repository;
        public RegistryDateTypeQuery(int tenant)
        {
            repository = new RegistryDateTypeRepository(tenant);
        }
        public RegistryDateTypeQuery(RegistryDateTypeRepository repository)
        {
            this.repository = repository;
        }

        public RegistryDateTypeList GetSingleRegistryDateTypeList(RegistryDateType entity)
        {
            RegistryDateTypeList myResult = null;

            if (entity != null)
            {
                myResult = new RegistryDateTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<RegistryDateTypeList> GetIQueryableEntityList(IQueryable<RegistryDateType> entities)
        {
            var myResult = (from entity in entities
                            select new RegistryDateTypeList()
                            {
                                Code = entity.Code,
                                Name = entity.Name,
                                SearchFields = entity.SearchFields,
                            });

            return myResult;
        }
    }
}
