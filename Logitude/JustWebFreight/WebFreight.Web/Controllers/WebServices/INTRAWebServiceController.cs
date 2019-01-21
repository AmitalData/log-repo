using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Logitude.XSD.Analyzers.INTTRAAnalyzer;
using Logitude.XSD.INTTRA.BL;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class INTRAWebServiceController: ApiController
    {
        public HttpResponseMessage GetMessageResult(string myShipmentId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    INTTRAHelper myHelper = new INTTRAHelper(myShipmentId, tenant);

                    myHelper.Run();

                    INTTRAResult myResult = myHelper.Result;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMessageResultValidate(string myShipmentId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    INTTRAHelper myHelper = new INTTRAHelper(myShipmentId, tenant);

                    INTTRAResult myResult = myHelper.Result;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Post(INTTRASimulator simulator)
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
                                INTTRAAnalyzer Analyzer = new INTTRAAnalyzer(analyzeQueue, analyzeQueueReposiory);
                                Analyzer.Run();
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

                            INTTRAAnalyzer Analyzer = new INTTRAAnalyzer(analyzeQueue, analyzeQueueReposiory);
                            Analyzer.Run();
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
        public HttpResponseMessage GetReadFTPFolder()
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

                    string AppPath = HttpRuntime.AppDomainAppPath;

                    int mySplitIndex = AppPath.IndexOf("JustWebFreight");
                    string DirectoryPath = AppPath.Substring(0, mySplitIndex) + "Logitude.XSD\\INTTRA\\FTPSimulate";

                    if (!Directory.Exists(DirectoryPath))
                    {
                        myResult.Success = false;
                        myResult.Errors.Add(DirectoryPath + " NOT Exists");
                    }

                    else
                    {
                        string[] files_XML = Directory.GetFiles(DirectoryPath, "*.xml", SearchOption.AllDirectories);

                        myResult.FilesCount = files_XML.Count();

                        AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

                        foreach (string filePath in files_XML)
                        {
                            FileStream stream = File.OpenRead(filePath);
                            byte[] fileBytes = new byte[stream.Length];

                            stream.Read(fileBytes, 0, fileBytes.Length);
                            stream.Close();

                            string filePath_Fixed = filePath.Replace("\\", "/");
                            string fileName = filePath_Fixed.Split('/')[filePath_Fixed.Split('/').Length - 1].ToLower();

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
                                FileName = fileName,
                            };

                            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                            analyzeQueueReposiory.Add(analyzeQueue);
                            analyzeQueueReposiory.SubmitChanges();
                        }
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
        public HttpResponseMessage GetContainerStatuses(string ShipmentId, string ContainerId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);

                    List<ShipmentContainerStatusList> myResult = new List<ShipmentContainerStatusList>();

                    myResult = (from d in myContext.ShipmentContainerStatuses.Include("INTTRAStatus").Include("LocationPort")
                                where d.Tenant == tenant
                                && d.ShipmentId == ShipmentId
                                && d.ContainerId == ContainerId
                                select new ShipmentContainerStatusList()
                                {
                                    Id = d.Id,
                                    Tenant = d.Tenant,
                                    ArrivalDate = d.ArrivalDate,
                                    ContainerId = d.ContainerId,
                                    ContainerNumber = d.ContainerNumber,
                                    DepartureDate = d.DepartureDate,
                                    Details = d.Details,
                                    EventDate = d.EventDate,
                                    FromPortId = d.FromPortId,
                                    ToPortId = d.ToPortId,
                                    ReceivingDate = d.ReceivingDate,
                                    ShipmentId = d.ShipmentId,
                                    ShippingLineName = d.ShippingLineName,
                                    VoyageNumber = d.VoyageNumber,
                                    VesselName = d.VesselName,
                                    StatusCode = d.StatusCode,
                                    TimeOfArrivalInfo = d.TimeOfArrivalInfo,
                                    TimeOfDepartureInfo = d.TimeOfDepartureInfo,
                                    StatusName = d.INTTRAStatus == null ? null : d.INTTRAStatus.Name,
                                    LocationCode = d.LocationPort == null ? "" : d.LocationPort.Code,
                                    LocationName = d.LocationPort == null ? "" : d.LocationPort.EnglishName,
                                    Location = d.Location,
                                }).ToList();


                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult.OrderByDescending(o => o.EventDate));
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }

    public class INTTRASimulator
    {
        public string XmlString { get; set; }
        public string AnalyzeQueueId { get; set; }
        public int FilesCount { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public INTTRASimulator()
        {
            this.Success = true;
            this.Errors = new List<string>();
        }
    }
}