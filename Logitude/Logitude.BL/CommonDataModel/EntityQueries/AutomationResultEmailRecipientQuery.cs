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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AutomationResultEmailRecipientQuery
    {
        AutomationResultEmailRecipientRepository repository;
        public AutomationResultEmailRecipientQuery()
        {
            repository = new AutomationResultEmailRecipientRepository();
        }

        public AutomationResultEmailRecipientQuery(int tenant)
        {
            repository = new AutomationResultEmailRecipientRepository(tenant);
        }

        public AutomationResultEmailRecipientQuery(AutomationResultEmailRecipientRepository AutomationResultEmailRecipientRepository)
        {
            repository = AutomationResultEmailRecipientRepository;
        }

        public List<AutomationResultEmailRecipientPM> GetAutomationResultEmailRecipientPMsByAutomationId(string automationId, int tenant)
        {
            List<AutomationResultEmailRecipientPM> AutomationResultEmailRecipientes = (from a in repository.context.AutomationResultEmailRecipients
                                                                                       where a.Tenant == tenant && a.AutomationsId == automationId
                                                                                       select new AutomationResultEmailRecipientPM()
                                                                                            {
                                                                                                Id = a.Id,
                                                                                                AutomationsId = a.AutomationsId,
                                                                                                RecipientValue = a.RecipientValue,
                                                                                                RecipientType = a.RecipientType,
                                                                                                Tenant = a.Tenant,
                                                                                                PartnerObjectFieldCode = a.PartnerObjectFieldCode,
                                                                                                IsNotifyBack = a.IsNotifyBack,
                                                                                            }).ToList();
            return AutomationResultEmailRecipientes;
        }

        public IQueryable<AutomationResultEmailRecipientList> GetAutomationResultEmailRecipientListsByAutomationId(string automationId, int tenant)
        {
            IQueryable<AutomationResultEmailRecipientList> automationResultEmailRecipientes = (from a in repository.context.AutomationResultEmailRecipients
                                                                                               where a.Tenant == tenant && a.AutomationsId == automationId
                                                                                               select new AutomationResultEmailRecipientList()
                                                                                               {
                                                                                                   Id = a.Id,
                                                                                                   AutomationsId = a.AutomationsId,
                                                                                                   RecipientValue = a.RecipientValue,
                                                                                                   RecipientType = a.RecipientType,
                                                                                                   Tenant = a.Tenant,
                                                                                                   PartnerObjectFieldCode = a.PartnerObjectFieldCode,
                                                                                                   IsNotifyBack = a.IsNotifyBack,
                                                                                               });
            return automationResultEmailRecipientes;
        }



        public AutomationResultEmailRecipientPM GetSinglePM(string id, int tenant)
        {

            var query = (from a in repository.context.AutomationResultEmailRecipients
                         where a.Tenant == tenant && a.Id == id
                         select new AutomationResultEmailRecipientPM()
                         {
                             Id = a.Id,
                             AutomationsId = a.AutomationsId,
                             RecipientValue = a.RecipientValue,
                             RecipientType = a.RecipientType,
                             Tenant = a.Tenant,
                             IsNotifyBack = a.IsNotifyBack,
                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<AutomationResultEmailRecipientList> GetIQueryableEntityList(IQueryable<AutomationResultEmailRecipient> iQueryable)
        {
            IQueryable<AutomationResultEmailRecipientList> result = from a in iQueryable
                                                                    select new AutomationResultEmailRecipientList()
                                                                    {
                                                                        Id = a.Id,
                                                                        AutomationsId = a.AutomationsId,
                                                                        RecipientValue = a.RecipientValue,
                                                                        RecipientType = a.RecipientType,
                                                                        Tenant = a.Tenant,
                                                                        PartnerObjectFieldCode = a.PartnerObjectFieldCode,
                                                                        IsNotifyBack = a.IsNotifyBack,
                                                                    };
            return result;
        }

        public AutomationResultEmailRecipient GetFirstAutomationResultEmailRecipientForTenant(int tenant)
        {
            return (from a in repository.context.AutomationResultEmailRecipients
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public List<AutomationResultEmailRecipientPM> GetAutomationResultEmailRecipientPMsByAutomationIds(List<string> automationIds, int tenant)
        {
            List<AutomationResultEmailRecipientPM> AutomationResultEmailRecipientes = (from a in repository.context.AutomationResultEmailRecipients
                                                                                       where a.Tenant == tenant && automationIds.Contains(a.AutomationsId) 
                                                                                       select new AutomationResultEmailRecipientPM()
                                                                                       {
                                                                                           RecipientValue = a.RecipientValue,
                                                                                           RecipientType = a.RecipientType,
                                                                                           AutomationsId = a.AutomationsId,
                                                                                           PartnerObjectFieldCode = a.PartnerObjectFieldCode,
                                                                                           IsNotifyBack = a.IsNotifyBack,
                                                                                       }).ToList();
            return AutomationResultEmailRecipientes;
        }

    }
}
