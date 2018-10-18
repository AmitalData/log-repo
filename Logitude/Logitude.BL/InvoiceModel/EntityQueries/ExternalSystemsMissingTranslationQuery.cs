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
    public class ExternalSystemsMissingTranslationQuery
    {

        ExternalSystemsMissingTranslationRepository repository;
        public ExternalSystemsMissingTranslationQuery()
        {
            repository = new ExternalSystemsMissingTranslationRepository(); 
        }


        public ExternalSystemsMissingTranslationQuery(ExternalSystemsMissingTranslationRepository ExternalSystemsMissingTranslationRepository)
        {
            repository = ExternalSystemsMissingTranslationRepository;
        }

        public ExternalSystemsMissingTranslationQuery(int tenant)
        {
            repository = new ExternalSystemsMissingTranslationRepository(tenant);
        }

        public IQueryable<ExternalSystemsMissingTranslationPM> GetExternalSystemsMissingTranslationPMs()
        {
            return from a in repository.context.ExternalSystemsMissingTranslations
                   select new ExternalSystemsMissingTranslationPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                        ExternalCode = a.ExternalCode,
                        IsResolved = a.IsResolved,
                        LogitudeId = a.LogitudeId,
                        Split1 = a.Split1,
                        Split2 = a.Split2,
                       LogitudeTable = a.LogitudeTable,
                    
                   };
        }

        public ExternalSystemsMissingTranslationPM GetSingleExternalSystemsMissingTranslationPM(string id, int tenant)
        {
            return (from a in repository.context.ExternalSystemsMissingTranslations
                    where a.Id == id && a.Tenant == tenant
                    select new ExternalSystemsMissingTranslationPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ExternalCode = a.ExternalCode,
                        IsResolved = a.IsResolved,
                        LogitudeId = a.LogitudeId,
                        Split1 = a.Split1,
                        Split2 = a.Split2,
                        LogitudeTable = a.LogitudeTable,
                    }).FirstOrDefault();
        }

        public IQueryable<ExternalSystemsMissingTranslationPM> GetExternalSystemsMissingTranslationPMsByTenant(int tenant)
        {
            IQueryable<ExternalSystemsMissingTranslationPM> ExternalSystemsMissingTranslations = (from a in repository.context.ExternalSystemsMissingTranslations
                                              where a.Tenant == tenant
                                              select new ExternalSystemsMissingTranslationPM()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  ExternalCode = a.ExternalCode,
                                                  IsResolved = a.IsResolved,
                                                  LogitudeId = a.LogitudeId,
                                                  Split1 = a.Split1,
                                                  Split2 = a.Split2,
                                                  LogitudeTable = a.LogitudeTable,
                                              });
            return ExternalSystemsMissingTranslations;
        }

 
        public IQueryable<ExternalSystemsMissingTranslationList> GetIQueryableEntityList(IQueryable<ExternalSystemsMissingTranslation> iQueryable)
        {
            IQueryable<ExternalSystemsMissingTranslationList> result = from ExternalSystemsMissingTranslation in iQueryable
                                             select new ExternalSystemsMissingTranslationList()
                                             {
                                                 Id = ExternalSystemsMissingTranslation.Id,
                                                 Tenant =ExternalSystemsMissingTranslation.Tenant,
                                                 ExternalCode =ExternalSystemsMissingTranslation.ExternalCode,
                                                 LogitudeId =ExternalSystemsMissingTranslation.LogitudeId,
                                                 LogitudeTable =ExternalSystemsMissingTranslation.LogitudeTable,
                                                Split1 = ExternalSystemsMissingTranslation.Split1,
                                                Split2 = ExternalSystemsMissingTranslation.Split2,
                                                 IsResolved =ExternalSystemsMissingTranslation.IsResolved,
                                             };
            return result;
        }



    }
}