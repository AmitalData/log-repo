using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AirlineAreasPortQuery
    {
        AirlineAreasPortRepository repository;

        public AirlineAreasPortQuery()
        {
            repository = new AirlineAreasPortRepository(); 
        }

        public AirlineAreasPortQuery(int tenant)
        {
            repository = new AirlineAreasPortRepository(tenant);
        }

        public AirlineAreasPortQuery(AirlineAreasPortRepository AirlineAreasPortRepositoryRepository)
        {
            repository = AirlineAreasPortRepositoryRepository;
        }



        public AirlineAreasPortPM GetSinglePM(string id, int tenant)
        {
            AirlineAreasPortPM myResult
                = (from a in repository.context.AirlineAreasPorts
                   where a.Id == id && a.Tenant == tenant
                   select new AirlineAreasPortPM()
                   {
                       AddedDate = a.AddedDate,
                       Id = a.Id,
                       AirlineAreaId = a.AirlineAreaId,
                       Description = a.Description,
                       Name = a.Name,
                       Tenant = a.Tenant,
                       AddedByUserId = a.AddedByUserId,
                       PortId = a.PortId,
                   }).FirstOrDefault();

            return myResult;
        }



        internal List<AirlineAreasPortPM> GetAirlineAreasPortsPMsByAirlineId(string airlineAreaId, int tenant)
        {

            IQueryable<AirlineAreasPort> iQueryable = (from a in repository.context.AirlineAreasPorts
                                                         where a.AirlineAreaId == airlineAreaId && a.Tenant == tenant
                                                         select a);

            List<AirlineAreasPortPM> AirlineAreasPorts = this.MapPocoToPM(iQueryable);

            //foreach (AirlineAreasPortPM item in AirlineAreasPorts)
            //{
            //    IQueryable<ShipmentReceivable> iQueryableChilds = (from a in repository.context.AirlineAreasPorts
            //                                                       where a.ShipmentReceivableParentId == item.Id && a.Tenant == tenant
            //                                                       select a);

            //    item.ChildShipmentReceivables = this.MapPocoToPM(iQueryableChilds);
            //}

            return AirlineAreasPorts.OrderBy(d => d.AddedDate).ToList();



        }

        private List<AirlineAreasPortPM> MapPocoToPM(IQueryable<AirlineAreasPort> iQueryable)
        {
            List<AirlineAreasPortPM> myResult = (from a in iQueryable
                                                   select new AirlineAreasPortPM()
                                                   {
                                                       Id = a.Id,
                                                       AirlineAreaId = a.AirlineAreaId,
                                                       Name = a.Name,
                                                       Tenant = a.Tenant,
                                                       Description = a.Description,
                                                       AddedByUserId = a.AddedByUserId,                                                     
                                                       AddedDate = a.AddedDate,                                                                                               
                                                       PortId = a.PortId,
                                                       
                                                                  }).ToList();
            return myResult;
        }
    }
}
