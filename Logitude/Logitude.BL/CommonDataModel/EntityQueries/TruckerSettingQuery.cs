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
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.ExternalService;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TruckerSettingQuery
    {
        TruckerSettingRepository repository;



        public TruckerSettingQuery(int tenant)
        {
            repository = new TruckerSettingRepository(tenant);
        }

        public TruckerSettingQuery(TruckerSettingRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TruckerSettingList> GetIQueryableEntityList(IQueryable<TruckerSetting> iQueryable)
        {
            throw new NotImplementedException();
        }
        public bool AnyByAddressId(string addressId, int tenant)
        {
            return repository.context.TruckerSettings
                .Any(ts => ts.Tenant == tenant && ts.AddressId == addressId);
        }
        public IQueryable<TruckerSettingList> GetIQueryableEntityListByAddressId(string addressId, int tenant)
        {
            IQueryable<TruckerSettingList> entity = (from ts in repository.context.TruckerSettings.Include("CountryCity")
                                                     where ts.Tenant == tenant && ts.AddressId == addressId
                                                     select new TruckerSettingList()
                                                     {
                                                         Id = ts.Id,
                                                         AddressId = ts.AddressId,
                                                         FromAddressCityId = ts.FromAddressCityId,
                                                         ShipmentType = ts.ShipmentType,
                                                         ToAddressCityId = ts.ToAddressCityId,
                                                         TruckerId = ts.TruckerId,
                                                         Responsibility = ts.Responsibility,
                                                         ToAddressCityName = ts.ToCity != null ? (string.IsNullOrEmpty(ts.ToCity.LocalName) ? ts.ToCity.EnglishName : ts.ToCity.LocalName) : string.Empty,
                                                         FromAddressCityName = ts.FromAddressCityId != null ? (string.IsNullOrEmpty(ts.FromCity.LocalName) ? ts.FromCity.EnglishName : ts.FromCity.LocalName) : string.Empty,
                                                         Tenant = ts.Tenant,
                                                         ShipmentTypeName = ts.ShipmentTypeNavigation.Name,
                                                         ResponsibilityName = ts.ResponsibilityEntity.LocalName,
                                                         TruckerName = ts.Trucker != null ? (ts.Trucker.Card != null ? (string.IsNullOrEmpty(ts.Trucker.Card.LocalName) ? ts.Trucker.Card.EnglishName : ts.Trucker.Card.LocalName) : string.Empty) : string.Empty,
                                                     });
            return entity;
        }

        public IQueryable<TruckerSettingList> GetTruckerSettingForDefaultShipmentDelivery(string fromAddressCityId, string toAddressCityId, string shipmentType, int tenant)
        {
            IQueryable<TruckerSettingList> entity = (from ts in repository.context.TruckerSettings

                                                     where ts.Tenant == tenant 
                                                         && ts.FromAddressCityId == fromAddressCityId && ts.ToAddressCityId == toAddressCityId 
                                                         && (ts.ShipmentType == shipmentType || ts.ShipmentType == "all")
                                                     select new TruckerSettingList()
                                                     {
                                                         Id = ts.Id,
                                                         AddressId = ts.AddressId,
                                                         FromAddressCityId = ts.FromAddressCityId,
                                                         ShipmentType = ts.ShipmentType,
                                                         ToAddressCityId = ts.ToAddressCityId,
                                                         TruckerId = ts.TruckerId,
                                                         Responsibility = ts.Responsibility,
                                                         Tenant = ts.Tenant,
                                                         ResponsibilityName = ts.ResponsibilityEntity.LocalName,
                                                     });
            return entity;
        }

        public TruckerSettingPM GetSinglePM(string id, int tenant)
        {
            TruckerSettingPM entity = (from a in repository.context.TruckerSettings
                                       where a.Tenant == tenant && a.Id == id
                                       select new TruckerSettingPM()
                                       {
                                           Id   = a.Id,
                                           Tenant = a.Tenant,
                                           TruckerId = a.TruckerId,
                                           SearchFields = a.SearchFields,
                                           FromAddressCityId = a.FromAddressCityId,
                                           ToAddressCityId = a.ToAddressCityId,
                                           ShipmentType = a.ShipmentType,
                                           AddressId = a.AddressId,
                                           Responsibility = a.Responsibility
                                       }).FirstOrDefault();
            return entity;
        }
    }
}
