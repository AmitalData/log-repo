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
    public class INTTRABookingStatusQuery
    {
        INTTRABookingStatusRepository repository;
        public INTTRABookingStatusQuery(int tenant)
        {
            repository = new INTTRABookingStatusRepository(tenant);
        }
        public INTTRABookingStatusQuery(INTTRABookingStatusRepository repository)
        {
            this.repository = repository;
        }

        public INTTRABookingStatusList GetSingleINTTRABookingStatusList(INTTRABookingStatus entity)
        {
            INTTRABookingStatusList myResult = null;

            if (entity != null)
            {
                myResult = new INTTRABookingStatusList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<INTTRABookingStatusList> GetIQueryableEntityList(IQueryable<INTTRABookingStatus> iQueryable)
        {
            IQueryable<INTTRABookingStatusList> result = from entity in iQueryable
                                                        select new INTTRABookingStatusList()
                                                        {
                                                            Name = entity.Name,
                                                            Code = entity.Code,
                                                            SearchFields = entity.SearchFields,
                                                        };
            return result;
        }
    }
}
