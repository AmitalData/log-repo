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
    public class AutomationHistoryQuery
    {
        AutomationHistoryRepository repository;
        public AutomationHistoryQuery()
        {
            repository = new AutomationHistoryRepository();
        }

        public AutomationHistoryQuery(int tenant)
        {
            repository = new AutomationHistoryRepository(tenant);
        }

        public AutomationHistoryQuery(AutomationHistoryRepository AutomationHistoryRepository)
        {
            repository = AutomationHistoryRepository;
        }

        public List<AutomationHistoryPM> GetAutomationHistoryPMsByAutomationId(string automationId,int tenant, bool withOutXmal=false)
        {
            List<AutomationHistoryPM> automationHistorye = null;
            if (withOutXmal)
            {
                automationHistorye = (from a in repository.context.AutomationHistorys
                                      where a.Tenant == tenant && a.AutomationsId == automationId
                                      select new AutomationHistoryPM()
                                      {
                                          Version = a.Version,
                                          AutomationsId = a.AutomationsId,
                                          CreateDate = a.CreateDate,
                                          Tenant = a.Tenant,

                                      }).ToList();
            }
            else
            {
                automationHistorye = (from a in repository.context.AutomationHistorys
                                      where a.Tenant == tenant && a.AutomationsId == automationId
                                      select new AutomationHistoryPM()
                                      {
                                          Version = a.Version,
                                          AutomationsId = a.AutomationsId,
                                          CreateDate = a.CreateDate,
                                          AutomationXML = a.AutomationXML,
                                          Tenant = a.Tenant,

                                      }).ToList();
            }


            return automationHistorye;
        }




        public string GetAutomationXMLFromAutomationHistoryByDate(DateTime? updatedate, string automationId, int tenant)
        {
            return (from a in repository.context.AutomationHistorys
                    where a.Tenant == tenant && a.AutomationsId == automationId && a.CreateDate <= updatedate
                    orderby a.CreateDate descending
                    select a.AutomationXML).FirstOrDefault();
        }


        public AutomationHistoryPM GetSinglePM(string automationId , int version, int tenant)
        {

            var query = (from a in repository.context.AutomationHistorys
                         where a.Tenant == tenant && a.Version == version &&a.AutomationsId == automationId
                         select new AutomationHistoryPM()
                         {
                             Version = a.Version,
                             AutomationsId = a.AutomationsId,
                             CreateDate = a.CreateDate,
                             AutomationXML = a.AutomationXML,
                             Tenant = a.Tenant,

                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<AutomationHistoryList> GetIQueryableEntityList(IQueryable<AutomationHistory> iQueryable)
        {
            IQueryable<AutomationHistoryList> result = from automationHistory in iQueryable
                                                         select new AutomationHistoryList()
                                                         {
                                                             Version = automationHistory.Version,
                                                             AutomationsId = automationHistory.AutomationsId,
                                                             CreateDate = automationHistory.CreateDate,
                                                             AutomationXML = automationHistory.AutomationXML,
                                                             Tenant = automationHistory.Tenant,
                                                         };
            return result;
        }

        public AutomationHistory GetFirstAutomationHistoryForTenant(int tenant)
        {
            return (from a in repository.context.AutomationHistorys
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }







        public string GetAutomationBackupDataByAutomationId(string automationId, int version, int tenant)
        {

            string result = "";
            var query = (from a in repository.context.AutomationHistorys
                         where a.Tenant == tenant && a.Version == version && a.AutomationsId == automationId
                         select new AutomationHistoryPM()
                         {
                             Version = a.Version,
                             AutomationsId = a.AutomationsId,
                             CreateDate = a.CreateDate,
                             AutomationXML = a.AutomationXML,
                             Tenant = a.Tenant,

                         }).FirstOrDefault();

            if (query != null)
            {
                result = query.AutomationXML;
            }

            return result;
        }
    }
}
