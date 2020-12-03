 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Logitude.CargoTracking.Def.DataContracts;

namespace Logitude.CargoTracking.Data.Repositories
{
   public partial class CargoTrackingShipmentSearchRepository:IRepository<CargoTrackingShipmentSearch>
   {
        
		public List<CargoTrackingShipmentSearch> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<CargoTrackingShipmentSearch> GetShipmentSearchEntities(string searchField, int tenant)
        {
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = (from searchEntity in currentContext.CargoTrackingShipmentSearches
                         where 
                            
                            searchEntity.Tenant == tenant &&
                            searchEntity.SearchFields == searchField &&
                            searchEntity.IsPublic == true
                            orderby searchEntity.ShipmentDate descending
                            select searchEntity
                            
                            ).Take(20);

            return shipmentsSearchEntities;
        }
        public IQueryable<CargoTrackingShipmentSearch> GetShipmentSearchs( int tenant)
        {
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = (from searchEntity in currentContext.CargoTrackingShipmentSearches
                                                                               where

                                                                                  searchEntity.Tenant == tenant 
                                                                               orderby searchEntity.ShipmentDate descending
                                                                               select searchEntity

                            );

            return shipmentsSearchEntities;
        }
        public IQueryable<CargoTrackingShipmentSearch> GetShipmentSearchs(string searchKey, int tenant)
        {
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = (from searchEntity in currentContext.CargoTrackingShipmentSearches
                                                                               where

                                                                                  searchEntity.Tenant == tenant
                                                                                  && (searchKey == null || searchEntity.SearchFields.Contains(searchKey))
                                                                               orderby searchEntity.ShipmentDate descending
                                                                               select searchEntity

                            );

            return shipmentsSearchEntities;
        }
        public List<CargoTrackingShipmentSearch> GetShipmentSearchBySecurityKeys(string ShipmentId, int tenant)
        {
            List<CargoTrackingShipmentSearch> shipmentsSearchEntities = (from searchEntity in currentContext.CargoTrackingShipmentSearches
                                                                               where
                                                                                  searchEntity.ShipmentId == ShipmentId
                                                                                  && searchEntity.Tenant == tenant
                                                                               orderby searchEntity.ShipmentDate descending
                                                                               select searchEntity  ).ToList();

            return shipmentsSearchEntities;
        }

    }

}
   