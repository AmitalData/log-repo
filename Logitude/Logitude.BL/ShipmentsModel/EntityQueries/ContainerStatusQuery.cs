using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerStatusQuery
    {
        ContainerStatusRepository repository;

        public ContainerStatusQuery(int tenant)
        {
            repository = new ContainerStatusRepository(tenant);
        }

        public ContainerStatusQuery(ContainerStatusRepository repository)
        {
            this.repository = repository;
        }

        public ContainerStatusPM GetSingleShipmentCustomerTypePM(string code)
        {
            return (from a in repository.context.ContainerStatuses
                    where a.Code == code
                    select new ContainerStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public IQueryable<ContainerStatusList> GetIQueryableEntityList(IQueryable<ContainerStatusList> iQueryable)
        {
            IQueryable<ContainerStatusList> result = from entity in iQueryable
                                                          select new ContainerStatusList()
                                                          {
                                                              Name = entity.Name,
                                                              Code = entity.Code,
                                                              SearchFields = entity.SearchFields,
                                                          };
            return result;
        }
    }
}
