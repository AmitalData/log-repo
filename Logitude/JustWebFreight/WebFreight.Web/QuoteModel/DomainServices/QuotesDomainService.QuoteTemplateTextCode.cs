using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        public IQueryable<QuoteTemplateTextCode> GetQuoteTemplateTextCodes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextCodeRepository quotetemplatetextcoderepository;
            quotetemplatetextcoderepository = new QuoteTemplateTextCodeRepository(tenant);
            return quotetemplatetextcoderepository.GetQuoteTemplateTextCodes(0);
        }

        public IQueryable<QuoteTemplateTextCodePM> GetquoteTemplateTextCodesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);


            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(tenant);
            return quotetemplatetextcodeQuery.GetQuoteTemplateTextCodePMsByTenant(tenant);
        }

        public QuoteTemplateTextCodePM GetQuoteTemplateTextCodeById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(tenant);
            QuoteTemplateTextCodePM quotetemplatettextcode = quotetemplatetextcodeQuery.GetSinglePM(id, tenant);
            return quotetemplatettextcode;
         
        }

        public QuoteTemplateTextCodeList GetSingleQuoteTemplateTextCodeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextCodeRepository quotetemplatetextcodeRepository;
            quotetemplatetextcodeRepository = new QuoteTemplateTextCodeRepository(tenant);
            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(quotetemplatetextcodeRepository);
            QuoteTemplateTextCodeList quoteTemplatetextcodeList = null;
            QuoteTemplateTextCode quotetemplatetextcode = quotetemplatetextcodeRepository.GetSingleQuoteTemplateTextCode(id, tenant, false);

            if (quotetemplatetextcode != null)
            {
                List<QuoteTemplateTextCode> singleEntityList = new List<QuoteTemplateTextCode>();
                singleEntityList.Add(quotetemplatetextcode);

                IQueryable<QuoteTemplateTextCode> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateTextCodeList> iQueryableEntityList = quotetemplatetextcodeQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplatetextcodeList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplatetextcodeList;
        }

        public IQueryable<QuoteTemplateTextCodeList> GetQuoteTemplateTextCodeList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextCodeRepository quotetemplatetextcodeRepository;
            quotetemplatetextcodeRepository = new QuoteTemplateTextCodeRepository(tenant);
            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(quotetemplatetextcodeRepository);
            IQueryable<QuoteTemplateTextCode> quotetemplatestextcode = quotetemplatetextcodeRepository.GetQuoteTemplateTextCodes(tenant);
            IQueryable<QuoteTemplateTextCodeList> query2 = quotetemplatetextcodeQuery.GetIQueryableEntityList(quotetemplatestextcode);
            return query2;
        }


        public int GetQuoteTemplateTextCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateTextCodeRepository quotetemplatetextcodeRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quotetemplatetextcodeRepository = new QuoteTemplateTextCodeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateTextCode> quotetemplatetextcodes = quotetemplatetextcodeRepository.GetQuoteTemplateTextCodes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quotetemplatetextcodes = filter.GetFilteredQuery<QuoteTemplateTextCode>(nonListQueryOperation, quotetemplatetextcodes);
            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(quotetemplatetextcodeRepository);
            IQueryable<QuoteTemplateTextCodeList> query2 = quotetemplatetextcodeQuery.GetIQueryableEntityList(quotetemplatetextcodes);

            query2 = filter.GetFilteredQuery<QuoteTemplateTextCodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateTextCode(QuoteTemplateTextCodePM currentQuoteTemplateTextCode)
        {
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentQuoteTemplateTextCode.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentQuoteTemplateTextCode.Tenant);
            }
            QuoteTemplateTextCodeService service = new QuoteTemplateTextCodeService(objectContext, currentQuoteTemplateTextCode.Tenant);
            service.Create(currentQuoteTemplateTextCode);

            TableLastUpdateClass.UpdateTableHistory(currentQuoteTemplateTextCode.Tenant, "QuoteTemplateTextCode");



        }

        public void UpdateQuoteTemplateTextCode(QuoteTemplateTextCodePM currentQuoteTemplateTextCode)
        {
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentQuoteTemplateTextCode.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentQuoteTemplateTextCode.Tenant);
            }

            string entityName = "QuoteTemplateTextCode" + currentQuoteTemplateTextCode.Id + currentQuoteTemplateTextCode.Tenant;
            string entityPmName = "QuoteTemplateTextCodePM" + currentQuoteTemplateTextCode.Id + currentQuoteTemplateTextCode.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateTextCodeService service = new QuoteTemplateTextCodeService(objectContext, currentQuoteTemplateTextCode.Tenant);
            service.Update(currentQuoteTemplateTextCode);
            TableLastUpdateClass.UpdateTableHistory(currentQuoteTemplateTextCode.Tenant, "QuoteTemplateTextCode");

        }

        public void DeleteQuoteTemplateTextCode(QuoteTemplateTextCodePM quotetemplatetextcodepm)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quotetemplatetextcodepm.Tenant);
            }
            QuoteTemplateTextCodeRepository quotetemplatetextcodeRepository;
            quotetemplatetextcodeRepository = new QuoteTemplateTextCodeRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateTextCode entity = quotetemplatetextcodeRepository.GetSingleQuoteTemplateTextCode(quotetemplatetextcodepm.Id, quotetemplatetextcodepm.Tenant, false);
            quotetemplatetextcodeRepository.Remove(entity);
        }

        public IQueryable<QuoteTemplateTextCodePM> GetquoteTemplateTextCodesByByQuoteTemplateId(string quotetemplateid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);


            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(tenant);
            return quotetemplatetextcodeQuery.GetQuoteTemplateTextCodePMsByQuoteTemplateId(tenant, quotetemplateid);
        }


        public QuoteTemplateTextCodePM GetQuoteTemplateTextCodeByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateTextCodeQuery quotetemplatetextcodeQuery = new QuoteTemplateTextCodeQuery(tenant);
            QuoteTemplateTextCodePM quotetemplatettextcode = quotetemplatetextcodeQuery.GetQuoteTemplateTextCodeByCode(code, tenant);
            return quotetemplatettextcode;


        }

       
    }
}