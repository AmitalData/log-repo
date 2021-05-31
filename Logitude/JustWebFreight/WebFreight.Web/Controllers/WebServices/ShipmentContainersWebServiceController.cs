using Logitude.Server.Tools.Counters;
using Logitude.XSD.Analyzers.INTTRAAnalyzer;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebServices
{
    public class ShipmentContainersWebServiceController : ApiController
    {
        public HttpResponseMessage Post(ShipmentContainerSimulator simulator)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    INTTRASimulator myResult = new INTTRASimulator();

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
                                //ContainerStatusesAnalyzer Analyzer = new ContainerStatusesAnalyzer(analyzeQueue, analyzeQueueReposiory);
                               // Analyzer.Run();
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

                            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                            {
                                //Subject = fileName.StartsWith("bl") ? "BL Response" : (fileName.StartsWith("voyage") ? "Voyage Response" : "Artemus Response"),
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                                From = "INTTRA",
                                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                                MessageBody = fileBytes,
                                Status = "W",
                                Retries = 0,
                                ConnectedToEntity = false,
                                ConnectedToTenant = false,
                                //Tenant = tenant,
                                FileSize = fileBytes.Length,
                                FileName = "XmlString Simulator",
                            };

                            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                            analyzeQueueReposiory.Add(analyzeQueue);
                            analyzeQueueReposiory.SubmitChanges();

                            //ContainerStatusesAnalyzer Analyzer = new ContainerStatusesAnalyzer(analyzeQueue, analyzeQueueReposiory);
                            //Analyzer.Run();
                        }

                        scope2.Complete();
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
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
                        throw new ApplicationException("The ScacCode code or Container number are empty, please fill them first");
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

    public class ShipmentContainerSimulator
    {
        public string XmlString { get; set; }
        public string AnalyzeQueueId { get; set; }
        public int FilesCount { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public ShipmentContainerSimulator()
        {
            this.Success = true;
            this.Errors = new List<string>();
        }
    }
}