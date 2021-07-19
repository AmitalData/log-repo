using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerStatusSourceQuery
    {
        ContainerStatusSourceRepository repository;

        public ContainerStatusSourceQuery(int tenant)
        {
            repository = new ContainerStatusSourceRepository(tenant);
        }

        public ContainerStatusSourceQuery(ContainerStatusSourceRepository repository)
        {
            this.repository = repository;
        }

        public ContainerStatusSourcePM GetSingleShipmentCustomerTypePM(string code)
        {
            return (from a in repository.context.ContainerStatuses
                    where a.Code == code
                    select new ContainerStatusSourcePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public IQueryable<ContainerStatusSourceList> GetIQueryableEntityList(IQueryable<ContainerStatusSourceList> iQueryable)
        {
            IQueryable<ContainerStatusSourceList> result = from entity in iQueryable
                                                     select new ContainerStatusSourceList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
