using Amital.QuoteOPM.BL.EntityQueryServices;
using Amital.QuoteOPM.Data;
using Amital.QuoteOPM.Data.EntityListQueryServices;
using Amital.QuoteOPM.Data.EntityLists;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.QuoteOPMModel.DomainServices
{
    

    public partial class QuoteOPMDomainService
    {
        

        public QuoteOPCustomerTypePM GetSingleQuoteOPCustomerTypePM(string code, int tenant)
        {
            quoteOPMContext = QuoteOPMContext.GetContext(tenant);
            QuoteOPCustomerTypeQuery = new QuoteOPCustomerTypeQueryService(quoteOPMContext);
            QuoteOPCustomerTypePM QuoteOPCustomerType = QuoteOPCustomerTypeQuery.GetSingle(code, false, false);
            return QuoteOPCustomerType;
        }

        public QuoteOPCustomerTypeList GetSingleQuoteOPCustomerTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.QuoteOPCustomerType", "READ", tenant);

            if (quoteOPMContext == null)
            {
                quoteOPMContext = QuoteOPMContext.GetContext(tenant);
            }
            
            QuoteOPCustomerTypeListQueryService listService = new QuoteOPCustomerTypeListQueryService(quoteOPMContext);
            return listService.GetSingle(code);
        }

        public List<QuoteOPCustomerTypeList> GetQuoteOPCustomerTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.QuoteOPCustomerType", "READ", tenant);
            quoteOPMContext = QuoteOPMContext.GetContext(tenant);
            QuoteOPCustomerTypeListQueryService listService = new QuoteOPCustomerTypeListQueryService(quoteOPMContext);
            return listService.GetList(tenant);
        }


        public List<QuoteOPCustomerTypeList> GetQuoteOPCustomerTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.QuoteOPCustomerType", "READ", tenant);
            quoteOPMContext = QuoteOPMContext.GetContext(tenant);
            QuoteOPCustomerTypeListQueryService listService = new QuoteOPCustomerTypeListQueryService(quoteOPMContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetQuoteOPCustomerTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.QuoteOPCustomerType", "READ", tenant);
            quoteOPMContext = QuoteOPMContext.GetContext(tenant);
            QuoteOPCustomerTypeListQueryService queryService = new QuoteOPCustomerTypeListQueryService(quoteOPMContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}