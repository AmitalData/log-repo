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
    public class CustDocMetaDataValuesWebServiceController : ApiController
    {
        public HttpResponseMessage Post(CustomsDocumentMetaDataValuePM customsDocumentFilingsId)//PostCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(string customsDocumentFilingsId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(customContext);
                if (customsDocumentFilingsId == null)
                    customsDocumentFilingsId = new Logitude.Customs.Def.EntityPMs.CustomsDocumentMetaDataValuePM();// "";
                List<CustomsDocumentMetaDataValuePM> CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQuery.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocumentFilingsId.CustomsDocumentId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, CustomsDocumentMetaDataValues);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomsDocumentMetaDataValuesByConnectedEntity(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(customContext);
                List<CustomsDocumentMetaDataValuePM> CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQuery.GetCustomsDocumentMetaDataValuesByConnectedEntity(entityId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, CustomsDocumentMetaDataValues);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}