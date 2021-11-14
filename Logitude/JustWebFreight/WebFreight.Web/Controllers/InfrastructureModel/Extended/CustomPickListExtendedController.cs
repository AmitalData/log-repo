using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class CustomPickListExtendedController : ApiController
    {




        public HttpResponseMessage GetCustomPickListsByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);


                IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
                CustomPickListRepository customPickListsRepository = new CustomPickListRepository(objectContext);
                CustomPickListQuery customPickListQuery = new CustomPickListQuery(customPickListsRepository);
                List<CustomPickListPM> CustomPickListPMLists =   customPickListQuery.GetCustomPickListPMsByCode(tenant, code).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, CustomPickListPMLists);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }













        public HttpResponseMessage PutCreateUpdateCustomPickListPMs(List<CustomPickListPM> customPickListPMs)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IWebFreightContext objectContext = WebFreightContext.GetContext(authToken.Tenant);
                CustomPickListService customPickListService = new CustomPickListService(objectContext, authToken.Tenant);
                foreach (CustomPickListPM customPickList in customPickListPMs)
                {
                    SecurityUtility.AuthenticationOnEntityTenant("CustomPickList", customPickList.Tenant, authToken.Tenant);


                    if (!customPickList.IsDirty)
                    {
                        //Add & Edit
                        if (!string.IsNullOrEmpty(customPickList.Id)) customPickListService.Update(customPickList);
                        else customPickListService.Create(customPickList);
                      

                    }
                    else
                    {
                        //Delete
                        if (!string.IsNullOrEmpty(customPickList.Id))
                        {
                            CustomPickListRepository CustomPickListRepository;
                            CustomPickListRepository = new CustomPickListRepository(objectContext);
                            CustomPickList entity = CustomPickListRepository.GetSingleCustomPickList(customPickList.Id, customPickList.Tenant);
                            CustomPickListRepository.Remove(entity);
                            CustomPickListRepository.SubmitChanges();
                        }
                    }
                }

				TableLastUpdateClass.UpdateTableHistory(authToken.Tenant, "CustomPickList");

				return Request.CreateResponse(HttpStatusCode.OK, customPickListPMs);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}