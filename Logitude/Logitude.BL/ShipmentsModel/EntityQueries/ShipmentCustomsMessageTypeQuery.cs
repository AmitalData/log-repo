using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentCustomsMessageTypeQuery
    {
        ShipmentCustomsMessageTypeRepository repository;
        public ShipmentCustomsMessageTypeQuery(int tenant)
        {
            repository = new ShipmentCustomsMessageTypeRepository(tenant);
        }
        public ShipmentCustomsMessageTypeQuery(ShipmentCustomsMessageTypeRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentCustomsMessageTypeList GetSingleShipmentCustomsMessageTypeList(ShipmentCustomsMessageType entity)
        {
            ShipmentCustomsMessageTypeList myResult = null;

            if (entity != null)
            {
                myResult = new ShipmentCustomsMessageTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<ShipmentCustomsMessageTypeList> GetIQueryableEntityList(IQueryable<ShipmentCustomsMessageType> entities)
        {
            var myResult = (from entity in entities
                            select new ShipmentCustomsMessageTypeList()
                            {
                                Code = entity.Code,
                                Name = entity.Name,
                                SearchFields = entity.SearchFields,
                            });

            return myResult;
        }

    }
}
