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

        public TruckerSettingQuery()
        {
            repository = new TruckerSettingRepository(); 
        }

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
