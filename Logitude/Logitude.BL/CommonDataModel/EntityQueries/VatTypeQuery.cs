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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class VatTypeQuery
    {
        VatTypeRepository repository;

        public VatTypeQuery()
        {
            repository = new VatTypeRepository(); 
        }
        public VatTypeQuery(int tenant)
        {
            repository = new VatTypeRepository(tenant);
        }
        public VatTypeQuery(VatTypeRepository repository)
        {
            this.repository = repository;
        }

        public List<VatTypePercentagePM> GetVatTypePercentagePMByDate(int tenant, DateTime? date)
        {
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(tenant);
            List<VatTypePercentagePM> result = new List<VatTypePercentagePM>();
            List<VatType> vatTypes = repository.GetVatTypes(tenant).ToList();

            foreach (VatType item in vatTypes)
            {
                VatTypePercentage resultItem = vatTypePercentageRepository.GetVatTypePercentageByDate(item.Id, tenant, date);
                if (resultItem != null)
                {
                    result.Add(new VatTypePercentagePM()
                    {
                        Id = resultItem.Id,
                        Tenant = resultItem.Tenant,
                        VatTypeId = resultItem.VatTypeId,
                        FromDate = resultItem.FromDate,
                        Percentage = resultItem.Percentage,
                       
                    });
                }
            }
            return result;
        }

        public VatTypePM GetSinglePM(string id, int tenant)
        {
            VATTypesGroupQuery vatTypesGroupQuery = new VATTypesGroupQuery(tenant);
            VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(tenant);

            var entityPM = (from a in repository.context.VatTypes
                            where a.Tenant == tenant && a.Id == id
                            select new VatTypePM()
                            {
                                AddedManually = a.AddedManually,
                                Code = a.Code,
                                EnglishName = a.EnglishName,
                                Id = a.Id,
                                InActive = a.InActive,
                                LocalName = a.LocalName,
                                SearchFields = a.SearchFields,
                                Tenant = a.Tenant,
                                Description = a.Description,
                                LocalDescription = a.LocalDescription,
                                ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                ReceivablesExternalId = a.ReceivablesExternalId,
                                PayablesExternalId=a.PayablesExternalId,
                                ExternalTAXItemId = a.ExternalTAXItemId,
                                IsMultiPercentage = a.IsMultiPercentage,
                                RecognizedPercentage = a.RecognizedPercentage,
                           }).FirstOrDefault();

            if (entityPM != null)
            {
                entityPM.VatTypeGroups = vatTypesGroupQuery.GetVATTypesGroupsByVatId(tenant, id);
                entityPM.VatTypePercentages = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, id).ToList();

                //if (entityPM.VatTypePercentages.Count > 0)
                //{
                //    entityPM.Percentage = entityPM.VatTypePercentages.OrderByDescending(d => d.FromDate).FirstOrDefault().Percentage;
                //}
            }

            VatTypePM securedPm = new VatTypePM();
            SecuredMapping.GetMappedPM(entityPM, securedPm, "VatType", tenant);

            return securedPm;
        }

        public VatTypePM GetSinglePMByCode(string code, int tenant)
        {
            VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
            var entityPM = (from a in repository.context.VatTypes
                           where a.Tenant == tenant && a.Code == code
                           select new VatTypePM()
                           {
                               AddedManually = a.AddedManually,
                               Code = a.Code,
                               EnglishName = a.EnglishName,
                               Id = a.Id,
                               InActive = a.InActive,
                               LocalName = a.LocalName,
                               SearchFields = a.SearchFields,
                               Tenant = a.Tenant,
                               Description = a.Description,
                               LocalDescription = a.LocalDescription,
                               ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                               ReceivablesExternalId = a.ReceivablesExternalId,
                               PayablesExternalId = a.PayablesExternalId,
                               ExternalTAXItemId = a.ExternalTAXItemId,
                               IsMultiPercentage = a.IsMultiPercentage,
                               RecognizedPercentage= a.RecognizedPercentage,

                           }).FirstOrDefault();

            VatTypePM securedPm = null;

            if (entityPM != null)
            {
                entityPM.VatTypePercentages = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, entityPM.Id).ToList();

                //if (entityPM.VatTypePercentages.Count > 0)
                //{
                //    entityPM.Percentage = entityPM.VatTypePercentages.OrderByDescending(d => d.FromDate).FirstOrDefault().Percentage;
                //}
                securedPm = new VatTypePM();
                SecuredMapping.GetMappedPM(entityPM, securedPm, "VatType", tenant);

            }

            return securedPm;
        }

        public IQueryable<VatTypePM> GetVatTypePMsByTenant(int tenant)
        {
            IQueryable<VatTypePM> vatTypes = from a in repository.context.VatTypes
                                             where a.Tenant == tenant
                                             select new VatTypePM()
                                             {
                                                 AddedManually = a.AddedManually,
                                                 Code = a.Code,
                                                 EnglishName = a.EnglishName,
                                                 Id = a.Id,
                                                 InActive = a.InActive,
                                                 LocalName = a.LocalName,
                                                 Tenant = a.Tenant,
                                                 SearchFields = a.SearchFields,
                                                 Description = a.Description,
                                                 LocalDescription = a.LocalDescription,
                                                 ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                 ReceivablesExternalId = a.ReceivablesExternalId,
                                                 PayablesExternalId = a.PayablesExternalId,
                                                 ExternalTAXItemId = a.ExternalTAXItemId,
                                                 IsMultiPercentage = a.IsMultiPercentage,
                                                 RecognizedPercentage= a.RecognizedPercentage,

                                             };
            return vatTypes;
        }


        public IQueryable<VatTypeList> GetIQueryableEntityList(IQueryable<VatType> iQueryable)
        {
            IQueryable<VatTypeList> myResult = (from f in iQueryable
                                                select new VatTypeList()
                                                {
                                                    AddedManually = f.AddedManually,
                                                    Id = f.Id,
                                                    InActive = f.InActive,
                                                    LocalName = f.LocalName,
                                                    EnglishName = f.EnglishName,
                                                    Code = f.Code,
                                                    SearchFields = f.SearchFields,
                                                    Tenant = f.Tenant,
                                                    Description = f.Description,
                                                    LocalDescription = f.LocalDescription,
                                                    ReceivablesExternalId = f.ReceivablesExternalId,
                                                    PayablesExternalId = f.PayablesExternalId,
                                                    ExternalTAXItemId = f.ExternalTAXItemId,
                                                    IsMultiPercentage = f.IsMultiPercentage,
                                                    RecognizedPercentage= f.RecognizedPercentage
                                                });

            //IQueryable<VatTypeList> myResult = (from f in iQueryable
            //                                    join db_Percentages in repository.context.VatTypePercentages on f.Id equals db_Percentages.VatTypeId into joinedData
            //                                    from myVatTypePercentage in joinedData.DefaultIfEmpty()
            //                                    select new VatTypeList()
            //                                    {
            //                                        AddedManually = f.AddedManually,
            //                                        Id = f.Id,
            //                                        InActive = f.InActive,
            //                                        LocalName = f.LocalName,
            //                                        EnglishName = f.EnglishName,
            //                                        Code = f.Code,
            //                                        SearchFields = f.SearchFields,
            //                                        Tenant = f.Tenant,
            //                                        Description = f.Description,
            //                                        LocalDescription = f.LocalDescription,
            //                                        ExternalVATCard = f.ExternalVATCard,
            //                                        ExternalTAXItemId = f.ExternalTAXItemId,
            //                                        Percentage = joinedData.Count() == 0 ? 0 : joinedData.OrderByDescending(o => o.FromDate).FirstOrDefault().Percentage,
            //                                    }).Distinct();

            return myResult;
        }
    }
    
}
