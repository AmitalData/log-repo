using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ExternalSystemsSyncStatusQuery
    {

          ExternalSystemsSyncStatusRepository repository;
        public ExternalSystemsSyncStatusQuery()
        {
            repository = new ExternalSystemsSyncStatusRepository(); 
        }


        public ExternalSystemsSyncStatusQuery(ExternalSystemsSyncStatusRepository ExternalSystemsSyncStatusRepository)
        {
            repository = ExternalSystemsSyncStatusRepository;
        }

        public ExternalSystemsSyncStatusQuery(int tenant)
        {
            repository = new ExternalSystemsSyncStatusRepository(tenant);
        }

        public IQueryable<ExternalSystemsSyncStatusPM> GetExternalSystemsSyncStatusPMs()
        {
            return from a in repository.context.ExternalSystemsSyncStatuses
                   select new ExternalSystemsSyncStatusPM()
                   {
                       Id = a.Id,
                      Tenant = a.Tenant,
                      ProgressDetails = a.ProgressDetails,
                      Status = a.Status,
                      StatusDate = a.StatusDate,
                      Subject = a.Subject,

                   };
        }

        public ExternalSystemsSyncStatusPM GetSingleExternalSystemsSyncStatusPM(string id, int tenant)
        {
            return (from a in repository.context.ExternalSystemsSyncStatuses
                    where a.Id == id && a.Tenant == tenant
                    select new ExternalSystemsSyncStatusPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ProgressDetails = a.ProgressDetails,
                        Status = a.Status,
                        StatusDate = a.StatusDate,
                        Subject = a.Subject,
                    }).FirstOrDefault();
        }

        public ExternalSystemsSyncStatusPM GetExternalSystemsSyncStatusBySubject(string subject, int tenant)
        {
            return (from a in repository.context.ExternalSystemsSyncStatuses
                    where a.Subject == subject && a.Tenant == tenant
                    select new ExternalSystemsSyncStatusPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ProgressDetails = a.ProgressDetails,
                        Status = a.Status,
                        StatusDate = a.StatusDate,
                        Subject = a.Subject,
                    }).FirstOrDefault();
        }

        public IQueryable<ExternalSystemsSyncStatusPM> GetExternalSystemsSyncStatusPMsByTenant(int tenant)
        {
            IQueryable<ExternalSystemsSyncStatusPM> ExternalSystemsSyncStatuss = (from a in repository.context.ExternalSystemsSyncStatuses
                                                                                  where a.Tenant == tenant
                                                                                  select new ExternalSystemsSyncStatusPM()
                                                                                  {
                                                                                      Id = a.Id,
                                                                                      Tenant = a.Tenant,
                                                                                      ProgressDetails = a.ProgressDetails,
                                                                                      Status = a.Status,
                                                                                      StatusDate = a.StatusDate,
                                                                                      Subject = a.Subject,
                                                                                  });
            return ExternalSystemsSyncStatuss;
        }

  

        public IQueryable<ExternalSystemsSyncStatusList> GetIQueryableEntityList(IQueryable<ExternalSystemsSyncStatus> iQueryable)
        {
            IQueryable<ExternalSystemsSyncStatusList> result = from ExternalSystemsSyncStatus in iQueryable
                                                               select new ExternalSystemsSyncStatusList()
                                                               {
                                                                   Id = ExternalSystemsSyncStatus.Id,
                                                                   Tenant = ExternalSystemsSyncStatus.Tenant,
                                                                   ProgressDetails = ExternalSystemsSyncStatus.ProgressDetails,
                                                                   Status = ExternalSystemsSyncStatus.Status,
                                                                   StatusDate = ExternalSystemsSyncStatus.StatusDate,
                                                                   Subject = ExternalSystemsSyncStatus.Subject,
                                                               };
            return result;
        }



    }
}