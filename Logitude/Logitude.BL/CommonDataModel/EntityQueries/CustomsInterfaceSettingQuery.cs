using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomsInterfaceSettingQuery
    {
        CustomsInterfaceSettingRepository repository;

        public CustomsInterfaceSettingQuery()
        {
            repository = new CustomsInterfaceSettingRepository();
        }

        public CustomsInterfaceSettingQuery(int tenant)
        {
            repository = new CustomsInterfaceSettingRepository(tenant);
        }

        public CustomsInterfaceSettingQuery(CustomsInterfaceSettingRepository customsInterfaceSettingRepository)
        {
            repository = customsInterfaceSettingRepository;
        }

        public CustomsInterfaceSettingPM GetSinglePM(int id, int otherTenant)
        {
            CustomsInterfaceSettingPM entityPM = (from a in repository.context.CustomsInterfaceSettings.Include("ArtemusOutSettings").Include("ArtemusInSettings")
                                                  where a.Tenant == id
                                                  select new CustomsInterfaceSettingPM()
                                                  {
                                                      Tenant = a.Tenant,
                                                      LocalCustomsInterfaceCode = a.LocalCustomsInterfaceCode,
                                                      ImportToUSAInterfaceCode = a.ImportToUSAInterfaceCode,
                                                      ExportFromUSAInterfaceCode = a.ExportFromUSAInterfaceCode,
                                                      LocalCompanyId = a.LocalCompanyId,
                                                      LocalUserId = a.LocalUserId,
                                                      LocalPassword = a.LocalPassword,
                                                      ActivateCustomsManagementInShipments = a.ActivateCustomsManagementInShipments,
                                                      ArtemusInSettingsId = a.ArtemusInSettingsId,
                                                      ArtemusOutSettingsId = a.ArtemusOutSettingsId,
                                                      ArtemusOutSettingsHost = a.ArtemusOutSettings == null ? null : a.ArtemusOutSettings.Host,
                                                      ArtemusInSettingsHost = a.ArtemusInSettings == null ? null : a.ArtemusInSettings.Host,
                                                  }).FirstOrDefault();

            return entityPM;
        }

        public IQueryable<CustomsInterfaceSettingPM> GetAccountSettingPMs()
        {
            IQueryable<CustomsInterfaceSettingPM> list = (from a in repository.context.CustomsInterfaceSettings.Include("ArtemusOutSettings").Include("ArtemusInSettings")
                                                          select new CustomsInterfaceSettingPM()
                                                          {
                                                              Tenant = a.Tenant,
                                                              LocalCustomsInterfaceCode = a.LocalCustomsInterfaceCode,
                                                              ImportToUSAInterfaceCode = a.ImportToUSAInterfaceCode,
                                                              ExportFromUSAInterfaceCode = a.ExportFromUSAInterfaceCode,
                                                              LocalCompanyId = a.LocalCompanyId,
                                                              LocalUserId = a.LocalUserId,
                                                              LocalPassword = a.LocalPassword,
                                                              ActivateCustomsManagementInShipments = a.ActivateCustomsManagementInShipments,
                                                              ArtemusInSettingsId = a.ArtemusInSettingsId,
                                                              ArtemusOutSettingsId = a.ArtemusOutSettingsId,
                                                              ArtemusOutSettingsHost = a.ArtemusOutSettings == null ? null : a.ArtemusOutSettings.Host,
                                                              ArtemusInSettingsHost = a.ArtemusInSettings == null ? null : a.ArtemusInSettings.Host,
                                                          });
            return list;
        }

        public IQueryable<CustomsInterfaceSettingList> GetIQueryableEntityList(IQueryable<CustomsInterfaceSetting> iQueryable)
        {
            IQueryable<CustomsInterfaceSettingList> result = from a in iQueryable
                                                       select new CustomsInterfaceSettingList()
                                                       {
                                                           Tenant = a.Tenant,
                                                           LocalCustomsInterfaceCode = a.LocalCustomsInterfaceCode,
                                                           ImportToUSAInterfaceCode = a.ImportToUSAInterfaceCode,
                                                           ExportFromUSAInterfaceCode = a.ExportFromUSAInterfaceCode,
                                                           LocalCompanyId = a.LocalCompanyId,
                                                           LocalUserId = a.LocalUserId,
                                                           LocalPassword = a.LocalPassword,
                                                           ActivateCustomsManagementInShipments = a.ActivateCustomsManagementInShipments,
                                                           ArtemusInSettingsId = a.ArtemusInSettingsId,
                                                           ArtemusOutSettingsId = a.ArtemusOutSettingsId,
                                                       };
            return result;
        }

        public IQueryable<CustomsInterfaceSettingList> GetIQueryableEntityListByTenant(int tenant)
        {
            var result = (from a in repository.context.CustomsInterfaceSettings
                          select new CustomsInterfaceSettingList()
                          {
                              Tenant = a.Tenant,
                              LocalCustomsInterfaceCode = a.LocalCustomsInterfaceCode,
                              ImportToUSAInterfaceCode = a.ImportToUSAInterfaceCode,
                              ExportFromUSAInterfaceCode = a.ExportFromUSAInterfaceCode,
                              LocalCompanyId = a.LocalCompanyId,
                              LocalUserId = a.LocalUserId,
                              LocalPassword = a.LocalPassword,
                              ActivateCustomsManagementInShipments = a.ActivateCustomsManagementInShipments,
                              ArtemusInSettingsId = a.ArtemusInSettingsId,
                              ArtemusOutSettingsId = a.ArtemusOutSettingsId,
                          });

            return result;
        }

    }
}
