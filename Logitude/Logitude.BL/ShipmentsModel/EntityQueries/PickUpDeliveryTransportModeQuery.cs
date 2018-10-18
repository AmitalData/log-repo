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
    public class PickUpDeliveryTransportModeQuery
    {
        PickUpDeliveryTransportModeRepository repository;
        public PickUpDeliveryTransportModeQuery(int tenant)
        {
            repository = new PickUpDeliveryTransportModeRepository(tenant);
        }
        public PickUpDeliveryTransportModeQuery(PickUpDeliveryTransportModeRepository repository)
        {
            this.repository = repository;
        }

        public PickUpDeliveryTransportModeList GetSinglePickUpDeliveryTransportModeList(PickUpDeliveryTransportMode entity)
        {
            PickUpDeliveryTransportModeList myResult = null;

            if (entity != null)
            {
                myResult = new PickUpDeliveryTransportModeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<PickUpDeliveryTransportModeList> GetIQueryableEntityList(IQueryable<PickUpDeliveryTransportMode> iQueryable)
        {
            IQueryable<PickUpDeliveryTransportModeList> result = from entity in iQueryable
                                                                 select new PickUpDeliveryTransportModeList()
                                                                 {
                                                                     Name = entity.Name,
                                                                     Code = entity.Code,
                                                                     SearchFields = entity.SearchFields,
                                                                 };
            return result;
        }
    }
}
