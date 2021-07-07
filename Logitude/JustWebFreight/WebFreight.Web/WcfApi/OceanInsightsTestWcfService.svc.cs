using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using System.Xml;
using Unifreight.ContainerTasks;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OceanInsightsWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OceanInsightsWcfService.svc or OceanInsightsWcfService.svc.cs at the Solution Explorer and start debugging.
    //  ss
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OceanInsightsTestWcfService : IOceanInsightsWcfService
    {
        public Response Insert(int Tenant, string ScacCode, string ReferenceNo, string Type)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(Tenant);
                string OIToken = LogitudeSettings.OceanInsightsToken;
                //SecurityUtility.CheckContactFeature("Shipment", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(Tenant);
                    OceanInsightsRequestRepository oceanInsightsRequestRepository = new OceanInsightsRequestRepository(objectContext);

                    if (string.IsNullOrEmpty(ScacCode))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "ScacCode must have value";
                        return response;
                    }
                    if (string.IsNullOrEmpty(ReferenceNo))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "ReferenceNo # must have value";
                        return response;
                    }
                    if (response.HasError)
                    {
                        return response;
                    }
                    OceanInsightsRequestQuery query = new OceanInsightsRequestQuery(Tenant);
                    OceanInsightsRequestPM OceanInsightsRequestPm;// = new OceanInsightsRequestPM();
                    if (Type == "c_id")
                    {
                        OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(ScacCode, ReferenceNo, Tenant);
                    }
                    else
                    {
                        OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByCareierScacBLNoTenant(ScacCode, ReferenceNo, Tenant);
                        if (OceanInsightsRequestPm == null)
                        {
                            OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(ScacCode, ReferenceNo, Tenant);
                        }
                    }

                    bool UseOIV2 = FeatureToggleHelper.HasFeatureToggle("OI2", 0);
                    ContainerTasks Task = new ContainerTasks(UseOIV2);
                    string Result = "";
                    string Status;
                    string Errors;
                    object Temp = null;//STARTMONITOR
                    if (OceanInsightsRequestPm == null)
                    {
                        OceanInsightsRequestPm = new OceanInsightsRequestPM();
                        string Id = GetEightDigitssRandomNumber();
                       
                        OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, Tenant);
                        if (Type == "c_id")
                        {
                            OceanInsightsRequestPm.ContainerNumber = ReferenceNo;
                        }
                        else
                        {
                            OceanInsightsRequestPm.BLNumber = ReferenceNo;
                        }
                        OceanInsightsRequestPm.SCACCode = ScacCode;
                        OceanInsightsRequestPm.Tenant = Tenant;
                        OceanInsightsRequestPm.OceanInsigntId = Id;
                        OceanInsightsRequestPm.Type = Type;

                        service.Create(OceanInsightsRequestPm);

                    }
                    response.Result = OceanInsightsRequestPm.OceanInsigntId;
                    scope.Complete();
                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;

            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        private string GetEightDigitssRandomNumber()
        {
            Random random = new Random();
            int randomNo = random.Next(10000000, 99999999);
            return randomNo.ToString();
        }
        public Response GetStatus(string RequestId, string Type)
        {
            Response response = new Response();
            bool UseOIV2 = FeatureToggleHelper.HasFeatureToggle("OI2", 0);
            ContainerTasks Task = new ContainerTasks(UseOIV2);
            string Result;
            string Status;
            string Errors;
            string OIToken = LogitudeSettings.OceanInsightsToken;
            Task.GetStatus(RequestId, OIToken, out Result, out Status, out Errors, Type);//ActivateOperation("STARTMONITOR", ref Table, ref Temp, out Result, out Status, out Errors);
            if (!string.IsNullOrEmpty(Errors) || !string.IsNullOrWhiteSpace(Errors))
            {
                response.HasError = true;
                response.ErrorMessage = Errors;
                return response;
            }
            else
            {
                response.HasError = false;
                response.Result = Result;
                return response;
            }
        }
    }
}
