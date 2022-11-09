using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalCustomizationController : ApiController
    {
        [HttpGet]
        [Route("DigitalCustomization/GetTranslationCodes")]
        public HttpResponseMessage GetTranslationCodes(int tenant, string objectTableName)
        {
            try
            {
                var textCodeRepository = new TextCodeRepository(tenant);
                var tenantRepo = new TenantRepository(tenant);
                var translationRepo = new TranslationRepository(tenant);

                var tenantLang = tenantRepo.GetSingleTenantWithOutIncluded(tenant);

                var textCodes = textCodeRepository.GetDigitalTextCodesByTenantAndObjectTable(tenant, objectTableName);

                var translationCodes = translationRepo.GetDigitalTranslationsByTenant(tenant, objectTableName, tenantLang.Language);

                var data = new Dictionary<string, string>();

                foreach (var item in textCodes)
                {
                    if (translationCodes.ContainsKey(item.Code))
                    {
                        var translatedKey = translationCodes[item.Code];

                        data.Add(item.Code, translatedKey);
                    }
                    else
                    {
                        data.Add(item.Code, item.DefaultText);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}
