using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
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
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class AutomationResultEmailRecipientExtendedController : ApiController
    {
        public HttpResponseMessage GetAutomationResultEmailRecipientByAutomationId(string automationId, int tenant)
        {
            try
            {
                Authentication();


            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(tenant);
            List<AutomationResultEmailRecipientPM> myResult = automationResultEmailRecipientQuery.GetAutomationResultEmailRecipientPMsByAutomationId(automationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }






        public HttpResponseMessage PutAutomationResultEmailRecipient(List<AutomationArgs> items)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "UPDATE", authToken.Tenant);
                      
                        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                        AutomationResultEmailRecipientRepository entityRepository = new AutomationResultEmailRecipientRepository(MyContext);
                        AutomationResultEmailRecipientService service = new AutomationResultEmailRecipientService(MyContext, authToken.Tenant);
                        AutomationResultEmailRecipient Poco = null;
    
                        foreach (AutomationArgs item in items)
                        {
                            Poco = new AutomationResultEmailRecipient();
                            if (string.IsNullOrEmpty(item.Id))
                            {
                                item.Id = item.Id =  IdCounter.GetNumber("AutomationResultEmailRecipient", item.Tenant).ToString();
                                MapEntity(item, Poco, true);
                                entityRepository.Add(Poco);
                                TableLastUpdateClass.UpdateTableHistory(item.Tenant, "AutomationResultEmailRecipient");
                               
                            }
                            else
                            {
                                MapEntity(item, Poco, true);
                                entityRepository.Remove(Poco);

                            }
                        }

                        entityRepository.SubmitChanges();

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, true);
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


  
            public static void MapEntity(AutomationArgs entityPM, AutomationResultEmailRecipient entityPOCO, bool isNewState)
            {
                if (isNewState)
                {
                    entityPOCO.Id = entityPM.Id;
                    entityPOCO.Tenant = entityPM.Tenant;
                }


                entityPOCO.AutomationsId = entityPM.AutomationsId;
                entityPOCO.RecipientType = entityPM.RecipientType;
                entityPOCO.RecipientValue = entityPM.RecipientValue;



            }
        

        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "READ", authToken.Tenant);
        }

    }

}

