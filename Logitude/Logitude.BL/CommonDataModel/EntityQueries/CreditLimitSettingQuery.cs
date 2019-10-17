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
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CreditLimitSettingQuery
    {
        CreditLimitSettingRepository repository;
        public CreditLimitSettingQuery()
        {
            repository = new CreditLimitSettingRepository();
        }
        public CreditLimitSettingQuery(int tenant)
        {
            repository = new CreditLimitSettingRepository(tenant);
        }
        public CreditLimitSettingQuery(CreditLimitSettingRepository repository)
        {
            this.repository = repository;
        }

        public CreditLimitSettingPM GetSinglePM(string id, int tenant)
        {
            if (LogitudeSettings.IsCostomsDeploy)//ihab + itzik , due no db - and take time !!!
            {
                return null;
            }
            CreditLimitSettingPM entity = (from a in repository.context.CreditLimitSettings
                                           where a.Tenant == tenant && a.Id == id
                                           select new CreditLimitSettingPM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               InvoiceCreationBlock = a.InvoiceCreationBlock,
                                               InvoiceCreationWarning = a.InvoiceCreationWarning,
                                               IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                               ShipmentCreationBlock = a.ShipmentCreationBlock,
                                           }).FirstOrDefault();


            return entity;
        }

        public IQueryable<CreditLimitSettingList> GetIQueryableEntityList(IQueryable<CreditLimitSetting> iQueryable)
        {
            IQueryable<CreditLimitSettingList> result = from a in iQueryable
                                                        select new CreditLimitSettingList()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            InvoiceCreationBlock = a.InvoiceCreationBlock,
                                                            InvoiceCreationWarning = a.InvoiceCreationWarning,
                                                            IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                                            ShipmentCreationBlock = a.ShipmentCreationBlock,
                                                        };
            return result;
        }
    }
}
