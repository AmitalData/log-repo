using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CarrierAreaQuery
    {
        CarrierAreaRepository repository;



        public CarrierAreaQuery(int tenant)
        {
            repository = new CarrierAreaRepository(tenant);
        }

        public CarrierAreaQuery(CarrierAreaRepository CarrierAreaRepositoryRepository)
        {
            repository = CarrierAreaRepositoryRepository;
        }

        public CarrierAreaPM GetSinglePM(string id, int tenant)
        {
            CarrierAreaPM myResult
                = (from a in repository.context.CarrierAreas.Include("User").Include("User.Contact")
                   where a.Id == id && a.Tenant == tenant
                   select new CarrierAreaPM()
                   {
                       CreateDate = a.CreateDate,
                       Id = a.Id,
                       CarrierId = a.CarrierId,
                       CreatedByUserId = a.CreatedByUserId,
                       Description = a.Description,
                       Name = a.Name,
                       Tenant = a.Tenant,
                       UpdateDate = a.UpdateDate,
                       UpdatedByUserId = a.UpdatedByUserId,
                       CreatedByUserName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact.EnglishName,
                       UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                       TransportModeCode = a.TransportModeCode,
                   }).FirstOrDefault();

            return myResult;
        }

        public List<CarrierAreaPM> GetCarrierAreasPMsByCarrierId(string CarrierId, int tenant)
        {
            IQueryable<CarrierArea> iQueryable = (from a in repository.context.CarrierAreas.Include("User").Include("User.Contact")
                                                  where a.CarrierId == CarrierId && a.Tenant == tenant
                                                  select a);

            List<CarrierAreaPM> CarrierAreas = this.MapPocoToPM(iQueryable);

            foreach (CarrierAreaPM item in CarrierAreas)
            {
                IQueryable<CarrierAreasPort> iQueryableChilds = (from a in repository.context.CarrierAreasPorts.Include("Port").Include("Port.Country")
                                                                 where a.CarrierAreaId == item.Id && a.Tenant == tenant
                                                                 select a);

                item.CarrierAreasPorts = this.MapInnerPocoToPM(iQueryableChilds);
            }

            return CarrierAreas.OrderBy(d => d.CreateDate).ToList();
        }

        private List<CarrierAreaPM> MapPocoToPM(IQueryable<CarrierArea> iQueryable)
        {
            List<CarrierAreaPM> myResult = (from a in iQueryable
                                            select new CarrierAreaPM()
                                            {
                                                Id = a.Id,
                                                CarrierId = a.CarrierId,
                                                Name = a.Name,
                                                Tenant = a.Tenant,
                                                Description = a.Description,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                UpdateDate = a.UpdateDate,
                                                CreateDate = a.CreateDate,
                                                CreatedByUserId = a.CreatedByUserId,
                                                CreatedByUserName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact.EnglishName,
                                                UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                                TransportModeCode = a.TransportModeCode,
                                            }).ToList();
            return myResult;
        }

        private List<CarrierAreasPortPM> MapInnerPocoToPM(IQueryable<CarrierAreasPort> iQueryable)
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
                                                     Code = a.Port == null ? null : a.Port.Code,
                                                     CombinedCode = a.Port == null ? null : a.Port.CombinedCode,
                                                     CountryCode = a.Port == null ? null : (a.Port.Country == null ? null : a.Port.Country.Code),
                                                 }).ToList();

            return myResult;
        }

        public List<CarrierAreaList> GetCarrierAreasByCarrierId(string CarrierId, int tenant)
        {
            List<CarrierAreaList> myResult = (from a in repository.context.CarrierAreas
                                              where a.Tenant == tenant && a.CarrierId == CarrierId
                                              select new CarrierAreaList()
                                              {
                                                  CreateDate = a.CreateDate,
                                                  Id = a.Id,
                                                  CarrierId = a.CarrierId,
                                                  CreatedByUserId = a.CreatedByUserId,
                                                  Description = a.Description,
                                                  Name = a.Name,
                                                  Tenant = a.Tenant,
                                                  UpdateDate = a.UpdateDate,
                                                  UpdatedByUserId = a.UpdatedByUserId,
                                                  TransportModeCode = a.TransportModeCode,
                                              }).ToList();

            return myResult;
        }

        public IQueryable<CarrierAreaList> GetIQueryableEntityList(IQueryable<CarrierArea> iQueryable)
        {
            IQueryable<CarrierAreaList> result = from a in iQueryable
                                                 select new CarrierAreaList()
                                                 {
                                                     CreateDate = a.CreateDate,
                                                     Id = a.Id,
                                                     CarrierId = a.CarrierId,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     Description = a.Description,
                                                     Name = a.Name,
                                                     Tenant = a.Tenant,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     TransportModeCode = a.TransportModeCode,
                                                 };
            return result;
        }

        public CarrierAreaPM GetSinglePMWithComposition(string id, int tenant)
        {
            CarrierAreaPM myResult
                = (from a in repository.context.CarrierAreas.Include("User").Include("User.Contact")
                   where a.Id == id && a.Tenant == tenant
                   select new CarrierAreaPM()
                   {
                       CreateDate = a.CreateDate,
                       Id = a.Id,
                       CarrierId = a.CarrierId,
                       CreatedByUserId = a.CreatedByUserId,
                       Description = a.Description,
                       Name = a.Name,
                       Tenant = a.Tenant,
                       UpdateDate = a.UpdateDate,
                       UpdatedByUserId = a.UpdatedByUserId,
                       CreatedByUserName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact.EnglishName,
                       UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                       TransportModeCode = a.TransportModeCode,
                   }).FirstOrDefault();

            IQueryable<CarrierAreasPort> iQueryableChilds = (from a in repository.context.CarrierAreasPorts.Include("Port").Include("Port.Country")
                                                             where a.CarrierAreaId == id && a.Tenant == tenant
                                                             select a);

            myResult.CarrierAreasPorts = this.MapInnerPocoToPM(iQueryableChilds);

            return myResult;
        }
    }
}
