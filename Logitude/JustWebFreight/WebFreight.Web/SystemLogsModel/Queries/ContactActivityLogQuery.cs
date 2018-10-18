using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using WebFreight.Web.Security;
using WebFreight.Web.SystemLogsModel.EntityList;
using WebFreight.Web.SystemLogsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.SystemLogsModel.Queries
{
    public class ContactActivityLogQuery
    {
        ContactActivityLogRepository repository;

        public ContactActivityLogQuery()
        {
            repository = new ContactActivityLogRepository(); 
        }       

        public ContactActivityLogQuery(ContactActivityLogRepository contactActivityLogRepository)
        {
            repository = contactActivityLogRepository;
        }

        public ContactActivityLogPM GetSinglePM(string id,int tenant)
        {
            ContactActivityLogPM securedPm = new ContactActivityLogPM();
            ContactActivityLog log = repository.GetSingleContactActivityLog(id, tenant);

            ContactActivityLogPM ContactActivityLogs = new ContactActivityLogPM()
            {
                Id = log.Id,
                Tenant = log.Tenant,
                Activity = log.Activity,
                ContactId = log.ContactId,
                IsSharedLogisticsContact = log.IsSharedLogisticsContact,
                LogDateTime = log.LogDateTime,
                Module = log.Module,
                GMTLogDateTime = log.GMTLogDateTime,
                CardId = log.CardId,
                PartnerTypeId = log.PartnerTypeId,
            };

            SecuredMapping.GetMappedPM(ContactActivityLogs, securedPm, "ContactActivityLog", ContactActivityLogs.Tenant);
            return securedPm;
        }

        public IQueryable<ContactActivityLogPM> GetContactActivityLogsPMsByTenant(int tenant)
        {
            IQueryable<ContactActivityLogPM> ContactActivityLogs = from a in repository.GetContactActivityLogs(tenant)
                                                where a.Tenant == tenant
                                                select new ContactActivityLogPM()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    Activity = a.Activity,
                                                    ContactId = a.ContactId,
                                                    IsSharedLogisticsContact = a.IsSharedLogisticsContact,
                                                    LogDateTime = a.LogDateTime,
                                                    Module = a.Module,
                                                    GMTLogDateTime = a.GMTLogDateTime,
                                                    CardId = a.CardId,
                                                    PartnerTypeId = a.PartnerTypeId,
                                                };
            return ContactActivityLogs;
        }

        public IQueryable<ContactActivityLogList> GetIQueryableEntityList(IQueryable<ContactActivityLogList> iQueryable)
        {
            IQueryable<ContactActivityLogList> result = from ContactActivityLogs in iQueryable
                                                        select new ContactActivityLogList()
                                                  {
                                                      Id = ContactActivityLogs.Id,
                                                      Tenant = ContactActivityLogs.Tenant,
                                                      Activity = ContactActivityLogs.Activity,
                                                      ContactId = ContactActivityLogs.ContactId,
                                                      IsSharedLogisticsContact = ContactActivityLogs.IsSharedLogisticsContact,
                                                      LogDateTime = ContactActivityLogs.LogDateTime,
                                                      Module = ContactActivityLogs.Module,
                                                      GMTLogDateTime = ContactActivityLogs.GMTLogDateTime,
                                                      CardId = ContactActivityLogs.CardId,
                                                      PartnerTypeId = ContactActivityLogs.PartnerTypeId,
                                                  };
            return result;
        }
    }
}