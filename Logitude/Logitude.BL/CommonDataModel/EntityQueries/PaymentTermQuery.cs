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
    public class PaymentTermQuery
    {
        PaymentTermRepository repository;

        public PaymentTermQuery()
        {
            repository = new PaymentTermRepository(); 
        }
        public PaymentTermQuery(int tenant)
        {
            repository = new PaymentTermRepository(tenant);
        }
        public PaymentTermQuery(PaymentTermRepository repository)
        {
            this.repository = repository;
        }

        public PaymentTermPM GetSinglePM(string id, int tenant)
        {
            PaymentTermPM entityPM = null;
            PaymentTerm entityPOCO = repository.GetSinglePaymentTerm(id, tenant);

            if (entityPOCO != null)
            {
                entityPM = new PaymentTermPM()
                {
                    AddedManually = entityPOCO.AddedManually,
                    Days = entityPOCO.Days,
                    EnglishName = entityPOCO.EnglishName,
                    Id = entityPOCO.Id,
                    InActive = entityPOCO.InActive,
                    LocalName = entityPOCO.LocalName,
                    Tenant = entityPOCO.Tenant,
                    DisplayInLOV = entityPOCO.DisplayInLOV,
                    Description = entityPOCO.Description,
                    LocalDescription = entityPOCO.LocalDescription,
                    SearchFields = entityPOCO.SearchFields,
                    ComputedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    IsManuallySet = entityPOCO.IsManuallySet,
                    ExternalId = entityPOCO.ExternalId,
                    EndOfMonth = entityPOCO.EndOfMonth,
                    NumberOfMonths = entityPOCO.NumberOfMonths,
                    FromDateTypeCode = entityPOCO.FromDateTypeCode,
                    CalculatedEnglishName = string.IsNullOrEmpty(entityPOCO.EnglishName) ? entityPOCO.LocalName : entityPOCO.EnglishName,
                    CalculatedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    Code = entityPOCO.Code,
                };
            }

            PaymentTermPM securedPm = new PaymentTermPM();
            SecuredMapping.GetMappedPM(entityPM, securedPm, "PaymentTerm", tenant);

            return securedPm;
        }

        public PaymentTermPM GetSinglePMByCode(string Code, int tenant)
        {
            PaymentTermPM entityPM = null;
            PaymentTerm entityPOCO = repository.GetSinglePaymentTermByCode(Code, tenant);

            if (entityPOCO != null)
            {
                entityPM = new PaymentTermPM()
                {
                    AddedManually = entityPOCO.AddedManually,
                    Days = entityPOCO.Days,
                    EnglishName = entityPOCO.EnglishName,
                    Id = entityPOCO.Id,
                    InActive = entityPOCO.InActive,
                    LocalName = entityPOCO.LocalName,
                    Tenant = entityPOCO.Tenant,
                    DisplayInLOV = entityPOCO.DisplayInLOV,
                    Description = entityPOCO.Description,
                    LocalDescription = entityPOCO.LocalDescription,
                    SearchFields = entityPOCO.SearchFields,
                    ComputedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    IsManuallySet = entityPOCO.IsManuallySet,
                    ExternalId = entityPOCO.ExternalId,
                    EndOfMonth = entityPOCO.EndOfMonth,
                    NumberOfMonths = entityPOCO.NumberOfMonths,
                    FromDateTypeCode = entityPOCO.FromDateTypeCode,
                    CalculatedEnglishName = string.IsNullOrEmpty(entityPOCO.EnglishName) ? entityPOCO.LocalName : entityPOCO.EnglishName,
                    CalculatedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    Code = entityPOCO.Code,
                };
            }

            PaymentTermPM securedPm = new PaymentTermPM();
            SecuredMapping.GetMappedPM(entityPM, securedPm, "PaymentTerm", tenant);

            return securedPm;
        }

        public PaymentTermPM GetSinglePMByExternalId(string externalId , int tenant)
        {

            PaymentTermPM entityPM = null;
            PaymentTerm entityPOCO = repository.GetSinglePaymentTermByExternalId(externalId, tenant);

            if (entityPOCO != null)
            {
                entityPM = new PaymentTermPM()
                {
                    AddedManually = entityPOCO.AddedManually,
                    Days = entityPOCO.Days,
                    EnglishName = entityPOCO.EnglishName,
                    Id = entityPOCO.Id,
                    InActive = entityPOCO.InActive,
                    LocalName = entityPOCO.LocalName,
                    Tenant = entityPOCO.Tenant,
                    DisplayInLOV = entityPOCO.DisplayInLOV,
                    Description = entityPOCO.Description,
                    LocalDescription = entityPOCO.LocalDescription,
                    SearchFields = entityPOCO.SearchFields,
                    ComputedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    IsManuallySet = entityPOCO.IsManuallySet,
                    ExternalId = entityPOCO.ExternalId,
                    EndOfMonth = entityPOCO.EndOfMonth,
                    NumberOfMonths = entityPOCO.NumberOfMonths,
                    FromDateTypeCode = entityPOCO.FromDateTypeCode,
                    CalculatedEnglishName = string.IsNullOrEmpty(entityPOCO.EnglishName) ? entityPOCO.LocalName : entityPOCO.EnglishName,
                    CalculatedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    Code = entityPOCO.Code,
                };
            }

            return entityPM;
        }

        public PaymentTerm GetSingleByDaysDifference(int daysDifference, int tenant)
        {

            PaymentTermPM entityPM = null;
            PaymentTerm entityPOCO = repository.GetSingleByDaysDifference(daysDifference, tenant);
            

            return entityPOCO;
        }

        public IQueryable<PaymentTermPM> GetPaymenTermPMsByTenant(int tenant)
        {
            IQueryable<PaymentTermPM> paymentTerms = from a in repository.context.PaymentTerms
                                                     where a.Tenant == tenant
                                                     select new PaymentTermPM()
                                                     {
                                                         AddedManually = a.AddedManually,
                                                         Days = a.Days,
                                                         EnglishName = a.EnglishName,
                                                         Id = a.Id,
                                                         InActive = a.InActive,
                                                         LocalName = a.LocalName,
                                                         Tenant = a.Tenant,
                                                         DisplayInLOV = a.DisplayInLOV,
                                                         Description = a.Description,
                                                         LocalDescription = a.LocalDescription,
                                                         SearchFields = a.SearchFields,
                                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                         IsManuallySet = a.IsManuallySet,
                                                         ExternalId = a.ExternalId,
                                                         EndOfMonth = a.EndOfMonth,
                                                         NumberOfMonths = a.NumberOfMonths,
                                                         FromDateTypeCode = a.FromDateTypeCode,
                                                         CalculatedEnglishName = string.IsNullOrEmpty(a.EnglishName) ? a.LocalName : a.EnglishName,
                                                         CalculatedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                         Code = a.Code,
                                                     };
            return paymentTerms;
        }

        public IQueryable<PaymentTermPM> GetPaymentTermPMsByTenant(int tenant)
        {
            IQueryable<PaymentTermPM> paymentTerms = from a in repository.context.PaymentTerms
                                                     where a.Tenant == tenant
                                                     select new PaymentTermPM()
                                                     {
                                                         AddedManually = a.AddedManually,
                                                         Days = a.Days,
                                                         EnglishName = a.EnglishName,
                                                         Id = a.Id,
                                                         InActive = a.InActive,
                                                         LocalName = a.LocalName,
                                                         Tenant = a.Tenant,
                                                         DisplayInLOV = a.DisplayInLOV,
                                                         Description = a.Description,
                                                         LocalDescription = a.LocalDescription,
                                                         SearchFields = a.SearchFields,
                                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                         IsManuallySet = a.IsManuallySet,
                                                         ExternalId = a.ExternalId,
                                                         EndOfMonth = a.EndOfMonth,
                                                         NumberOfMonths = a.NumberOfMonths,
                                                         FromDateTypeCode = a.FromDateTypeCode,
                                                         CalculatedEnglishName = string.IsNullOrEmpty(a.EnglishName) ? a.LocalName : a.EnglishName,
                                                         CalculatedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                         Code = a.Code,
                                                     };
            return paymentTerms;
        }

        public IQueryable<PaymentTermPM> GetPaymentTermsByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.PaymentTerms
                         where a.Tenant == tenant
                         select new PaymentTermPM()
                         {
                             AddedManually = a.AddedManually,
                             Days = a.Days,
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Tenant = a.Tenant,
                             DisplayInLOV = a.DisplayInLOV,
                             Description = a.Description,
                             LocalDescription = a.LocalDescription,
                             SearchFields = a.SearchFields,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             IsManuallySet = a.IsManuallySet,
                             ExternalId = a.ExternalId,
                             EndOfMonth = a.EndOfMonth,
                             NumberOfMonths = a.NumberOfMonths,
                             FromDateTypeCode = a.FromDateTypeCode,
                             CalculatedEnglishName = string.IsNullOrEmpty(a.EnglishName) ? a.LocalName : a.EnglishName,
                             CalculatedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             Code = a.Code,
                         }).AsQueryable();

            IQueryable<PaymentTermPM> query2 = null;
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }

                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }

            if (query2 != null)
            {
                return query2;
            }

            else
            {
                return query;
            }
        }

        public IQueryable<PaymentTermList> GetIQueryableEntityList(IQueryable<PaymentTerm> iQueryable)
        {
            var result = from f in iQueryable
                         select new PaymentTermList()
                         {
                             AddedManually = f.AddedManually,
                             Id = f.Id,
                             InActive = f.InActive,
                             LocalName = f.LocalName,
                             EnglishName = f.EnglishName,
                             Days = f.Days,
                             Tenant = f.Tenant,
                             DisplayInLOV = f.DisplayInLOV,
                             Description = f.Description,
                             LocalDescription = f.LocalDescription,
                             SearchFields = f.SearchFields,
                             IsManuallySet = f.IsManuallySet,
                             ExternalId =  f.ExternalId,
                             EndOfMonth = f.EndOfMonth,
                             NumberOfMonths = f.NumberOfMonths,
                             FromDateTypeCode = f.FromDateTypeCode,
                             CalculatedEnglishName = string.IsNullOrEmpty(f.EnglishName) ? f.LocalName : f.EnglishName,
                             CalculatedLocalName = string.IsNullOrEmpty(f.LocalName) ? f.EnglishName : f.LocalName,
                             Code = f.Code,
                         };

          
            return result;
        }
    }
}