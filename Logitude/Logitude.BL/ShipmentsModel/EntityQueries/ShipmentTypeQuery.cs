using System.Linq;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentTypeQuery
    {
        ShipmentTypeRepository repository;
         
        public ShipmentTypeQuery(int tenant)
        {
            repository = new ShipmentTypeRepository(tenant);
        }

        public ShipmentTypeQuery(ShipmentTypeRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentTypePM GetSingleShipmentTypePM(string id)
        {
            ShipmentType entity = repository.GetSingleShipmentType(id);
            ShipmentTypePM pm = null;

            if (entity != null)
            {
                pm = new ShipmentTypePM()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    TransportModeId = entity.TransportModeId,
                    SearchFields = entity.SearchFields,
                };
            }

            return pm;
        }

        public ShipmentTypePM GetSinglePM(string id, int tenant)
        {
            ShipmentType entity = repository.GetSingleShipmentType(id);
            ShipmentTypePM pm = null;

            if (entity != null)
            {
                pm = new ShipmentTypePM()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    TransportModeId = entity.TransportModeId,
                    SearchFields = entity.SearchFields,
                };
            }

            return pm;
        }

        public ShipmentTypePM GetSinglePM(string id)
        {
            ShipmentType entity = repository.GetSingleShipmentType(id);
            ShipmentTypePM pm = null;

            if (entity != null)
            {
                pm = new ShipmentTypePM()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    TransportModeId = entity.TransportModeId,
                    SearchFields = entity.SearchFields,
                };
            }

            return pm;
        }

        public IQueryable<ShipmentTypePM> GetShipmentTypePMs()
        {
            return from i in repository.context.ShipmentTypes
                   select new ShipmentTypePM()
                   {
                       Id = i.Id,
                       Name = i.Name,
                       TransportModeId = i.TransportModeId,
                       SearchFields = i.SearchFields,
                   };
        }

        public IQueryable<EntityLists.ShipmentTypeList> GetIQueryableEntityList(IQueryable<ShipmentType> iQueryable)
        {
            IQueryable<ShipmentTypeList> result = (from a in iQueryable
                                                   select new ShipmentTypeList()
                                                               {
                                                                   Id = a.Id,
                                                                   Name = a.Name,
                                                                   TransportModeId = a.TransportModeId,
                                                                   SearchFields = a.SearchFields,
                                                               });
            return result;
        }
    }
}