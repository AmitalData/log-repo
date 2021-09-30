using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.XSD.Analyzers.INTTRAAnalyzer;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.Analyzers;
using WebFreight.Web.Security;
using WebFreight.Web.WcfApi;
using ContainerOISimulator;
using Logitude.BL.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class ShipmentContainersWebServiceController : ApiController
    {
      

        public HttpResponseMessage Post(ShipmentContainerSimulator simulator)
        {
            try
            {
                ShipmentContainerSimulator shipmentContainerSimulator = new ShipmentContainerSimulator();
                if (simulator.IsFromContainer)
                {
                    shipmentContainerSimulator = this.RunFullContainerStatusSimulator(simulator);
                }
                else
                {
                    shipmentContainerSimulator = RunContainerStatusResponseSimulator(simulator);
                }
                return Request.CreateResponse(HttpStatusCode.OK, shipmentContainerSimulator);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private ShipmentContainerSimulator RunFullContainerStatusSimulator(ShipmentContainerSimulator simulator)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            ContainerSimulator containerSimulator = new ContainerSimulator();
            containerSimulator.Run(simulator, tenant);
            return simulator;
        }


        private ShipmentContainerSimulator RunContainerStatusResponseSimulator(ShipmentContainerSimulator simulator)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                ShipmentContainerSimulator myResult = new ShipmentContainerSimulator();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                using (TransactionScope scope2 = TransactionFactory.GetTransaction())
                {
                    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

                    if (simulator.AnalyzeQueueId != null)
                    {
                        AnalyzeQueue analyzeQueue = analyzeQueueReposiory.GetSingleAnalyzeQueue(simulator.AnalyzeQueueId);
                        if (analyzeQueue == null)
                        {
                            myResult.Success = false;
                            myResult.Errors.Add("The Analyze Queue does not exists");
                        }

                        else
                        {
                            try
                            {
                                ContainerStatusesConnecterAnalyzer analyzer = new ContainerStatusesConnecterAnalyzer(analyzeQueue, analyzeQueueReposiory);
                                analyzer.Run();
                            }
                            catch (Exception ex)
                            {
                                myResult.Success = false;
                                myResult.Errors.Add(ex.Message);
                            }
                        }
                    }

                    else if (simulator.XmlString != null)
                    {
                        byte[] fileBytes = null;
                        try
                        {
                            fileBytes = Encoding.ASCII.GetBytes(simulator.XmlString);
                        }
                        catch (Exception ex)
                        {
                            myResult.Success = false;
                            myResult.Errors.Add("Xml Text is not valid");
                        }

                        try
                        {
                            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                            {
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                                From = "ContainerStatusesReceiver",
                                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                                MessageBody = fileBytes,
                                Status = "W",
                                Retries = 0,
                                ConnectedToEntity = false,
                                ConnectedToTenant = false,
                                FileSize = fileBytes.Length,
                                Tenant = tenant,
                                FileName = "XmlString Simulator",
                            };

                            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                            analyzeQueueReposiory.Add(analyzeQueue);
                            analyzeQueueReposiory.SubmitChanges();
                            ContainerStatusesConnecterAnalyzer analyzer = new ContainerStatusesConnecterAnalyzer(analyzeQueue, analyzeQueueReposiory);
                            analyzer.Run();
                        }
                        catch (Exception ex)
                        {
                            myResult.Success = false;
                            myResult.Errors.Add(ex.Message);
                        }
                    }

                    scope2.Complete();
                }

                scope.Complete();
                return myResult;
            }
        }

        public HttpResponseMessage GetContainerStatusRequest(string shipmentId, string containerId, bool isContainer)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    ContainerStatusesHelper myHelper = new ContainerStatusesHelper(shipmentId, containerId, isContainer, tenant);
                    if (myHelper.Validate())
                    {
                        myHelper.SendContainerStatusRequest();
                    }
                    else
                    {
                        if (!myHelper.IsValidShippingLine())
                        {
                            throw new ApplicationException("The Shipping Line is not supported by Ocean Insights.");
                        }
                        else
                        {
                            throw new ApplicationException("The SCAC Code of the Shipping Line or the Container Number is empty, please make sure they are filled.");
                        }
                    }
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }


}