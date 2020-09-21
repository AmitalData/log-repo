using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using System.ServiceModel;
using System.ServiceModel.Web;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace WebFreight.Web.App_Code
{
    public class TrainingResourcesController : ApiController
    {
        public List<HelpResource> PostHelpResources(TrainingResourcesFilters filters)
        {
            HelpResourceRepository rep = new HelpResourceRepository();
            GenericFilter filter = new GenericFilter();
            QueryOperations queryOperations = new QueryOperations();

            IQueryable<HelpResource> helpers = rep.GetAllActiveHelpResources();

            queryOperations.SetFilter("Language", filters.Language, false, "equals", null, true);
            queryOperations.SetFilter("Category", filters.Category, false, "equals", null, true);
            queryOperations.SetFilter("Type", filters.Type, false, "equals", null, true);
            queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, false);

            helpers = filter.GetFilteredQuery<HelpResource>(queryOperations, helpers);

            List<HelpResource> query = helpers.ToList();
            List<HelpResource> myResult = new List<HelpResource>();

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = null;
            if (!string.IsNullOrEmpty(token))
            {
                authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            }

            if (authToken != null)
            {
                bool allDataExists = SecurityUtility.CheckTableContactFeature("HelpResource", "Module", authToken.Tenant);

                if (allDataExists)
                {
                    foreach (HelpResource item in query)
                    {
                        myResult.Add(item);
                    }
                }
            }

            return myResult;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getsinglehelper/{code}")]
        public HelpResource GetSingleShipmentPM(string code)
        {
            HelpResourceRepository rep = new HelpResourceRepository();
            HelpResource resource = rep.GetSingleHelpResource(code, 0);
            
            return resource;
        }
    }
}