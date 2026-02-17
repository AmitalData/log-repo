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
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ProductTypeQuery
    {
        ProductTypeRepository repository;

        public ProductTypeQuery()
        {
            repository = new ProductTypeRepository();             
        }

        public ProductTypeQuery(int tenant)
        {
            repository = new ProductTypeRepository(tenant);
        }

        public ProductTypeQuery(ProductTypeRepository repository)
        {
            this.repository = repository;
        }

        public ProductTypePM GetSinglePM(string code, int tenant = 0)
        {
            ProductType entityPoco = repository.GetSingleProductType(code);
            ProductTypePM entityPM = new ProductTypePM() 
            {
                Code = entityPoco.Code,
                Name = entityPoco.Name,
                SearchFields = entityPoco.SearchFields ,
                Tenant=tenant,
                QuotationDefaultTemplateId = entityPoco.QuotationDefaultTemplateId,
                RoutingRQuoteDefaultTemplateId = entityPoco.RoutingRQuoteDefaultTemplateId,
            };

            ProductTypeModificationRepository modificationRep=new ProductTypeModificationRepository(tenant);
            ProductTypeModification modification = modificationRep.GetSingleProductTypeModification(code, tenant);

            entityPM.InActive = modification != null ? modification.InActive : false;
            entityPM.QuotationDefaultTemplateId = modification != null ? modification.QuotationDefaultTemplateId : null;
            entityPM.RoutingRQuoteDefaultTemplateId = modification != null ? modification.RoutingRQuoteDefaultTemplateId : null;




            return entityPM;
        }

        public IQueryable<ProductTypePM> GetProductTypePMs()
        {
            return (from a in repository.context.ProductTypes
                    select new ProductTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        QuotationDefaultTemplateId = a.QuotationDefaultTemplateId,
                        RoutingRQuoteDefaultTemplateId = a.RoutingRQuoteDefaultTemplateId,
                    });
        }

        public IQueryable<ProductTypeList> GetIQueryableEntityList(IQueryable<ProductType> iQueryable, int tenant)
        {
            List<ProductTypeModification> modifications = repository.GetProductTypeModificationsForTenant(tenant).ToList();

            var joinResult = from productType in iQueryable.ToList()
                             join modification in modifications on productType.Code equals modification.ProductTypeCode into j
                             from modificationJoin in j.DefaultIfEmpty()
                             select new { Code = productType.Code, Name = productType.Name, QuotationDefaultTemplateId = modificationJoin != null ? modificationJoin.QuotationDefaultTemplateId : null  , SearchFields = productType.SearchFields, InActive = modificationJoin != null ? modificationJoin.InActive : false, Tenant = modificationJoin != null ? modificationJoin.Tenant : 0, RoutingRQuoteDefaultTemplateId = modificationJoin != null ? modificationJoin.RoutingRQuoteDefaultTemplateId : null };

            List<ProductTypeList> result = (from entity in joinResult
                                                 select new ProductTypeList()
                                                 {
                                                     Name = entity.Name,
                                                     Code = entity.Code,
                                                     InActive = entity.InActive,
                                                     SearchFields = entity.SearchFields,
                                                     Id = entity.Code,
                                                     QuotationDefaultTemplateId = entity.QuotationDefaultTemplateId,
                                                     RoutingRQuoteDefaultTemplateId = entity.RoutingRQuoteDefaultTemplateId,
                                                 }).ToList();



            #region DefaultTemplate
            List<QuoteTemplateList> quoteTemplateLists = null;
            List<string> quoteTemplateIdLits = result.GroupBy(d => d.QuotationDefaultTemplateId).Select(d => d.First().QuotationDefaultTemplateId).ToList();
            if (quoteTemplateIdLits.Count > 0)
            {
                QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(tenant);
                quoteTemplateLists = quoteTemplateQuery.GetQuoteTemplateListsByIds(quoteTemplateIdLits);
            }


       

            foreach (ProductTypeList item in result)
            {
                if (!string.IsNullOrEmpty(item.QuotationDefaultTemplateId) && quoteTemplateLists != null)
                {
                    QuoteTemplateList quoteTemplateList = quoteTemplateLists.Where(d => d.Id == item.QuotationDefaultTemplateId).FirstOrDefault();
                    if (quoteTemplateList != null) item.DefaultTemplate = quoteTemplateList.Name;


                    quoteTemplateList = quoteTemplateLists.Where(d => d.Id == item.RoutingRQuoteDefaultTemplateId).FirstOrDefault();
                    if (quoteTemplateList != null) item.RoutingRQuoteDefaultTemplate = quoteTemplateList.Name;
                }

            }
            #endregion



            return result.AsQueryable();
        }


    }
}
