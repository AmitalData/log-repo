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
    public class AirlineAreaQuery
    {
        AirlineAreaRepository repository;

        public AirlineAreaQuery()
        {
            repository = new AirlineAreaRepository(); 
        }

        public AirlineAreaQuery(int tenant)
        {
            repository = new AirlineAreaRepository(tenant);
        }

        public AirlineAreaQuery(AirlineAreaRepository AirlineAreaRepositoryRepository)
        {
            repository = AirlineAreaRepositoryRepository;
        }



        public AirlineAreaPM GetSinglePM(string id, int tenant)
        {
            AirlineAreaPM myResult
                = (from a in repository.context.AirlineAreas
                   where a.Id == id && a.Tenant == tenant
                   select new AirlineAreaPM()
                   {
                       CreateDate = a.CreateDate,
                       Id = a.Id,
                       AirlineId = a.AirlineId,
                       CreatedByUserId = a.CreatedByUserId,
                       Description = a.Description,
                       Name = a.Name,
                       Tenant = a.Tenant,
                       UpdateDate = a.UpdateDate,
                       UpdatedByUserId = a.UpdatedByUserId,
                   }).FirstOrDefault();

            return myResult;
        }



        internal List<AirlineAreaPM> GetAirlineAreasPMsByAirlineId(string airlineId, int tenant)
        {

            IQueryable<AirlineArea> iQueryable = (from a in repository.context.AirlineAreas
                                                         where a.AirlineId == airlineId && a.Tenant == tenant
                                                         select a);

            List<AirlineAreaPM> airlineAreas = this.MapPocoToPM(iQueryable);

            foreach (AirlineAreaPM item in airlineAreas)
            {
                IQueryable<AirlineAreasPort> iQueryableChilds = (from a in repository.context.AirlineAreasPorts
                                                                   where a.AirlineAreaId == item.Id && a.Tenant == tenant
                                                                   select a);

                item.AirlineAreasPorts = this.MapInnerPocoToPM(iQueryableChilds);
            }

            return airlineAreas.OrderBy(d => d.CreateDate).ToList();



        }

        private List<AirlineAreaPM> MapPocoToPM(IQueryable<AirlineArea> iQueryable)
        {
            List<AirlineAreaPM> myResult = (from a in iQueryable
                                                   select new AirlineAreaPM()
                                                   {
                                                       Id = a.Id,
                                                       AirlineId = a.AirlineId,
                                                       Name = a.Name,
                                                       Tenant = a.Tenant,
                                                       Description = a.Description,
                                                       UpdatedByUserId = a.UpdatedByUserId,                                                     
                                                       UpdateDate = a.UpdateDate,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       
                                                                  }).ToList();
            return myResult;
        }

        private List<AirlineAreasPortPM> MapInnerPocoToPM(IQueryable<AirlineAreasPort> iQueryable)
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

        public List<AirlineAreaList> GetAirlineAreasByAirlineId(string airlineId, int tenant)
        {
            List<AirlineAreaList> myResult = (from a in repository.context.AirlineAreas
                                                 where a.Tenant == tenant && a.AirlineId == airlineId
                                                 select new AirlineAreaList()
                                                 {
                                                     CreateDate = a.CreateDate,
                                                     Id = a.Id,
                                                     AirlineId = a.AirlineId,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     Description = a.Description,
                                                     Name = a.Name,
                                                     Tenant = a.Tenant,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                 }).ToList();

            return myResult;
        }
    }
}
