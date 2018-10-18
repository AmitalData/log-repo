using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Generated.PMControllers
{
    public partial class ComputingPartnersController : ApiController
    {

        public HttpResponseMessage Post(ComputingPartnerPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("ComputingPartner", "NEW", authToken.Tenant);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                        ComputingPartnerService service = new ComputingPartnerService(MyContext,entityPM);
                        service.Create();

                        //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                        // ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Airline", 0, true);
                        //string email = HttpContext.Current.User.Identity.Name;
                        // ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                        //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                        //if (loggedContact != null)
                        //{
                        //    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                        //}
                        TableLastUpdateClass.UpdateTableHistory(authToken.Tenant, "ComputingPartner");

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage Put(ComputingPartnerPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("ComputingPartner", "UPDATE", authToken.Tenant);

                        string entityName = "ComputingPartner" + entityPM.Id + authToken.Tenant;
                        string entityPmName = "ComputingPartnerPM" + entityPM.Id + authToken.Tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }

                        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                        ComputingPartnerService service = new ComputingPartnerService(MyContext, entityPM);
                        service.Update(entityPM.PartnerTables);

                        //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                        //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Airline", 0, true);
                        //string email = HttpContext.Current.User.Identity.Name;
                        //ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                        //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                        //if (loggedContact != null)
                        //{
                        //   ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                        //}

                        TableLastUpdateClass.UpdateTableHistory(authToken.Tenant, "ComputingPartner");

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


    }
}