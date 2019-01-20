
using System;

using WebFreight.Web.Security;
using WebFreight.Web.Helpers;

using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class QuantityTypeController : ApiController
    {

        public HttpResponseMessage GetQuantityType(string classificationCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                PropertiesDetailsHistoryPM propertiesDetailsHistory = null;
                MeasurmentUnitPM measurmentUnit = null;
                string QuantityTypeCode = null;
                
                if (!String.IsNullOrWhiteSpace( classificationCode))
                {
                    var key = "GetQuantityType," + classificationCode;
                    if (CacheManager.CacheWrapper.Get(key) == null)
                    {
                        CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
                        //CustomsItemPM customsItem = customsItemQueryService.GetCustomsItemByClassificationCode(classificationCode);


                        //PropertiesDetailsHistoryQueryService propertiesDetailsHistoryQueryService = new PropertiesDetailsHistoryQueryService(tenant);
                        //if (customsItem != null)
                        //{
                        //    propertiesDetailsHistory = propertiesDetailsHistoryQueryService.GetPropertiesDetailsHistoryByCustomsItemId(customsItem.ID);

                        //}
                        //MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(tenant);

                        //if (propertiesDetailsHistory != null && propertiesDetailsHistory.MeasurementUnitID.HasValue)
                        //{

                        //    measurmentUnit = measurmentUnitQueryService.GetMeasurmentUnitByMalamId(propertiesDetailsHistory.MeasurementUnitID.Value);

                        //}

                        //if (measurmentUnit != null)
                        //{
                        //    QuantityTypeCode = measurmentUnit.Code;
                        //}

                        QuantityTypeCode = customsItemQueryService.GetQuantityTypeByClassificationCode(classificationCode, tenant);
                        if (QuantityTypeCode != null)
                        {

                            CacheManager.CacheWrapper.Insert(key, QuantityTypeCode, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        QuantityTypeCode = (string)CacheManager.CacheWrapper.Get(key);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, QuantityTypeCode);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}