using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class CustDocsTicketWebServiceController : ApiController
    {
        public HttpResponseMessage GetCustomsDocumentsTicketsByEntityIdAndChilds(string entityId, string childEntityId1, string childEntityId2, string childEntityId3, string parentEntityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (childEntityId1 == "null")
                {
                    childEntityId1 = null;
                }
                if (childEntityId2 == "null")
                {
                    childEntityId2 = null;
                }
                if (childEntityId3 == "null")
                {
                    childEntityId3 = null;
                }
                ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);

                CustomsDocumentsTicketQueryService customsDocumentsTicketQuery = new CustomsDocumentsTicketQueryService(authToken.Tenant);
                List<CustomsDocumentsTicketPM> customsDocumentsTickets = customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityId, childEntityId1, childEntityId2, childEntityId3, authToken.Tenant, parentEntityCode);

                return Request.CreateResponse(HttpStatusCode.OK, customsDocumentsTickets);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}