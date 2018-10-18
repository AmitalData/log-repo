using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentCustomerTypeQuery
    {
        ShipmentCustomerTypeRepository repository;
         
        public ShipmentCustomerTypeQuery(int tenant)
        {
            repository = new ShipmentCustomerTypeRepository(tenant);
        }

        public ShipmentCustomerTypeQuery(ShipmentCustomerTypeRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentCustomerTypePM GetSingleShipmentCustomerTypePM(string code)
        {
            return (from a in repository.context.ShipmentCustomerTypes
                    where a.Code == code
                    select new ShipmentCustomerTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public IQueryable<ShipmentCustomerTypeList> GetIQueryableEntityList(IQueryable<ShipmentCustomerType> iQueryable)
        {
            IQueryable<ShipmentCustomerTypeList> result = from entity in iQueryable
                                                          select new ShipmentCustomerTypeList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}