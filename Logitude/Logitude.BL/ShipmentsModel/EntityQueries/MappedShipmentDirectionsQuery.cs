using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class MappedShipmentDirectionsQuery
    {
         MappedShipmentDirectionsRepository repository;

        public MappedShipmentDirectionsQuery()
        {
            repository = new MappedShipmentDirectionsRepository();
        }

        public MappedShipmentDirectionsQuery(int tenant)
        {
            repository = new MappedShipmentDirectionsRepository(tenant);
        }

        public MappedShipmentDirectionsQuery(MappedShipmentDirectionsRepository mappedShipmentDirectionsRepository)
        {
            repository = mappedShipmentDirectionsRepository;
        }

        public IQueryable<MappedShipmentDirectionsList> GetIQueryableEntityList(IQueryable<MappedShipmentDirections> iQueryable)
        {
            IQueryable<MappedShipmentDirectionsList> entity = from a in iQueryable.Include("Direction")
                                                              select new MappedShipmentDirectionsList()
                                                              {
                                                                  ShipmentDirectionId = a.ShipmentDirectionId,
                                                                  Tenant = a.Tenant,
                                                                  UpdateDateTime = a.UpdateDateTime

                                                              }; 
            return entity;

        }

        public MappedShipmentDirectionsPM GetSinglePM(int Tenant, string ShipmentDirectionId)
        {
            MappedShipmentDirectionsPM entity = (from a in repository.context.MappedShipmentDirections.Include("Direction")
                                                 where a.Tenant == Tenant && a.ShipmentDirectionId == ShipmentDirectionId
                                                 select new MappedShipmentDirectionsPM()
                                                 {
                                                     ShipmentDirectionId = a.ShipmentDirectionId,
                                                     Tenant = a.Tenant,
                                                     UpdateDateTime = a.UpdateDateTime

                                                 }).FirstOrDefault();


            return entity;


        }

        public bool IsShipmentDirectionIdExist(int Tenant, string ShipmentDirectionId)
        {
            bool temp = repository.GetMappedShipmentDirections(Tenant).Any(a => a.ShipmentDirectionId == ShipmentDirectionId); 
            return temp;


        } 
       
    }
}
