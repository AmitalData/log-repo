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
    public class CarrierAreasPortQuery
    {
        CarrierAreasPortRepository repository;

        public CarrierAreasPortQuery()
        {
            repository = new CarrierAreasPortRepository();
        }

        public CarrierAreasPortQuery(int tenant)
        {
            repository = new CarrierAreasPortRepository(tenant);
        }

        public CarrierAreasPortQuery(CarrierAreasPortRepository CarrierAreasPortRepositoryRepository)
        {
            repository = CarrierAreasPortRepositoryRepository;
        }

        public CarrierAreasPortPM GetSinglePM(string id, int tenant)
        {
            CarrierAreasPortPM myResult = (from a in repository.context.CarrierAreasPorts
                                           where a.Id == id && a.Tenant == tenant
                                           select new CarrierAreasPortPM()
                                           {
                                               AddedDate = a.AddedDate,
                                               Id = a.Id,
                                               CarrierAreaId = a.CarrierAreaId,
                                               Name = a.Name,
                                               Tenant = a.Tenant,
                                               AddedByUserId = a.AddedByUserId,
                                               PortId = a.PortId,
                                           }).FirstOrDefault();

            return myResult;
        }

        internal List<CarrierAreasPortPM> GetCarrierAreasPortsPMsByCarrierId(string CarrierAreaId, int tenant)
        {

            IQueryable<CarrierAreasPort> iQueryable = (from a in repository.context.CarrierAreasPorts
                                                       where a.CarrierAreaId == CarrierAreaId && a.Tenant == tenant
                                                       select a);

            List<CarrierAreasPortPM> CarrierAreasPorts = this.MapPocoToPM(iQueryable);
            return CarrierAreasPorts.OrderBy(d => d.AddedDate).ToList();
        }

        private List<CarrierAreasPortPM> MapPocoToPM(IQueryable<CarrierAreasPort> iQueryable)
        {
            List<CarrierAreasPortPM> myResult = (from a in iQueryable
                                                 select new CarrierAreasPortPM()
                                                 {
                                                     Id = a.Id,
                                                     CarrierAreaId = a.CarrierAreaId,
                                                     Name = a.Name,
                                                     Tenant = a.Tenant,
                                                     AddedByUserId = a.AddedByUserId,
                                                     AddedDate = a.AddedDate,
                                                     PortId = a.PortId,
                                                 }).ToList();
            return myResult;
        }
    }
}
