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
    public class INTTRABookingTransStatusQuery
    {
        INTTRABookingTransStatusRepository repository;
        public INTTRABookingTransStatusQuery(int tenant)
        {
            repository = new INTTRABookingTransStatusRepository(tenant);
        }
        public INTTRABookingTransStatusQuery(INTTRABookingTransStatusRepository repository)
        {
            this.repository = repository;
        }

        public INTTRABookingTransStatusList GetSingleINTTRABookingTransStatusList(INTTRABookingTransStatus entity)
        {
            INTTRABookingTransStatusList myResult = null;

            if (entity != null)
            {
                myResult = new INTTRABookingTransStatusList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<INTTRABookingTransStatusList> GetIQueryableEntityList(IQueryable<INTTRABookingTransStatus> iQueryable)
        {
            IQueryable<INTTRABookingTransStatusList> result = from entity in iQueryable
                                                        select new INTTRABookingTransStatusList()
                                                        {
                                                            Name = entity.Name,
                                                            Code = entity.Code,
                                                            SearchFields = entity.SearchFields,
                                                        };
            return result;
        }

    }
}
