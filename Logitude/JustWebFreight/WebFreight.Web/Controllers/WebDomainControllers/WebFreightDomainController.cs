using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Azure.Management.Dns;
using Microsoft.Azure.Management.Dns.Models;
using Microsoft.Practices.Unity;
using Microsoft.Rest.Azure.Authentication;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.BIReport;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;
using Microsoft.Azure.Management.ResourceManager;
using Simplog.Server.Infrastructure;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.SystemLogs;
using ICSharpCode.SharpZipLib.Checksum;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Linq;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class WebFreightDomainController : ApiController
    {
        public HttpResponseMessage Post(GeneralEntitiesArgs args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(args.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(args.Tenant);
                if (args.QueryColumnsPMs != null && args.QueryColumnsPMs.Count > 0)
                {
                    QueryColumnService service = new QueryColumnService(objectContext, args.Tenant);
                    foreach (var entityPM in args.QueryColumnsPMs)
                    {
                        service.Create(entityPM);
                    }

                }
                if (args.AdvancedQueryFilterPMs != null && args.AdvancedQueryFilterPMs.Count > 0)
                {
                    AdvancedQueryFilterService service = new AdvancedQueryFilterService(objectContext, args.Tenant);
                    foreach (var entityPM in args.AdvancedQueryFilterPMs)
                    {
                        service.Create(entityPM);
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutGeneralEntities(GeneralEntitiesArgs args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(args.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(args.Tenant);
                if (args.QueryColumnsPMs != null && args.QueryColumnsPMs.Count > 0)
                {
                    QueryColumnRepository repo = new QueryColumnRepository(args.Tenant);
                    QueryColumnQuery QCQuery = new QueryColumnQuery(repo);
                    QueryColumnService service = new QueryColumnService(objectContext, args.Tenant);
                    foreach (var entityPM in args.QueryColumnsPMs)
                    {
                        //var Column = QCQuery.GetQueryColumnsByFieldIdTenant(entityPM.ObjectFieldId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(entityPM.Id))
                        {
                            service.Update(entityPM);
                        }
                        else
                        {
                            service.Create(entityPM);
                        }
                    }

                }
                if (args.AdvancedQueryFilterPMs != null && args.AdvancedQueryFilterPMs.Count > 0)
                {
                    AdvancedQueryFilterRepository repo = new AdvancedQueryFilterRepository(args.Tenant);
                    AdvancedQueryFilterQuery QFQuery = new AdvancedQueryFilterQuery(repo);
                    AdvancedQueryFilterService service = new AdvancedQueryFilterService(objectContext, args.Tenant);
                    foreach (var entityPM in args.AdvancedQueryFilterPMs)
                    {
                        //var Query = QFQuery.GetPredefinedQueryFilterByFieldIdTenant(entityPM.ObjectFieldId, entityPM.Tenant);
                        //if (Query != null)
                        //{
                        //    entityPM.Id = Query.Id;
                        //    service.Update(entityPM);
                        //}
                        //AdvancedQueryFilterQuery advancedQueryFilterQuery = new AdvancedQueryFilterQuery(args.Tenant);
                        //var result = advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUser(args.Tenant, entityPM.UserId);
                        //AdvancedQueryFilterPM filter = null;
                        //if (result != null)
                        //{
                        //    filter = result.Where(a => a.ObjectFieldId == entityPM.ObjectFieldId && a.QueryId == queryid).FirstOrDefault();
                        //}
                        if (!string.IsNullOrEmpty(entityPM.Id))
                        {
                            service.Update(entityPM);
                        }
                        else
                        {
                            service.Create(entityPM);
                        }
                    }
                }

                if (args.RemovedQueryColumnsPMs != null && args.RemovedQueryColumnsPMs.Count > 0)
                {
                    QueryColumnRepository repo = new QueryColumnRepository(args.Tenant);
                    foreach (var entityPM in args.RemovedQueryColumnsPMs)
                    {
                        var Column = repo.GetSingleQueryColumn(entityPM.Id, entityPM.Tenant);
                        if (Column != null)
                        {
                            repo.Remove(Column);
                            repo.SubmitChanges();
                        }
                    }
                }

                if (args.RemovedQueryFilters != null && args.RemovedQueryFilters.Count > 0)
                {
                    AdvancedQueryFilterRepository repo = new AdvancedQueryFilterRepository(args.Tenant);
                    foreach (var entityPM in args.RemovedQueryFilters)
                    {
                        var Query = repo.GetSingleAdvancedQueryfilter(entityPM.Id);
                        if (Query != null)
                        {
                            repo.Remove(Query);
                            repo.SubmitChanges();
                        }
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetQueryToExcelData([FromUri] CustomApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var service = new QueryToExcelExportService();
                var result = service.ExportQueryDataToExcel(filters);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetQueryExportExecutionLogStatus(string logId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var queryExecutionLogRepository = new QueryExportExecutionLogRepository(authToken.Tenant);
                QueryExportExecutionLog queryExecutionLog = queryExecutionLogRepository.GetSingle(logId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, queryExecutionLog);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PutExportBIReportToExcel")]
        public HttpResponseMessage PutExportBIReportToExcel(BIReportXMLData bIReportXMLData)
        {
            string email = null;

            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                email = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                string ObjectTableName = "Shipment";
                var exportBIReportService = new ExportBIReportService();

                var data = exportBIReportService.Run(bIReportXMLData, tenant, true);

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = ObjectTableName + DateTime.Now.ToShortDateString(),
                    FolderName = "others",
                    Extension = exportBIReportService.GetBIReportExtensionFile(bIReportXMLData.ExportDataType),
                    Tenant = tenant,
                    FileSize = data.Length,

                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.Write(data, fileInfo);

                return Request.CreateResponse(HttpStatusCode.OK, ObjectTableName + DateTime.Now.ToShortDateString());
            }

            catch (Exception ex)
            {
               
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PutExportBIReportToExcelByWR")]
        public HttpResponseMessage PutExportBIReportToExcelByWR(BIReportXMLData bIReportXMLData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                BIReportsExecutionLogRepository reportExecutionLogRepository = new BIReportsExecutionLogRepository(tenant);
                var logId = IdCounter.GetNumber("BIReportsExecutionLog", tenant);
                bIReportXMLData.BIReportKey = Guid.NewGuid() + logId + "!BIReportName="+ bIReportXMLData.BIReportPM.Name;
                BIReportsExecutionLog bIReportExecutionLog = new BIReportsExecutionLog()
                {
                    Id = logId,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = bIReportXMLData.UserId,
                    ReportFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(bIReportXMLData),
                    Tenant = tenant,
                    StatusCode = "W",
                    BIReportId = bIReportXMLData.BIReportId,
                };
               

                reportExecutionLogRepository.Add(bIReportExecutionLog);
                reportExecutionLogRepository.SubmitChanges();

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("BIReportsExecutionLogQueue", bIReportExecutionLog.Tenant);
                queueservice.Send(new Dictionary<string, string>() {
                    { "BIReportExecutionLogId", bIReportExecutionLog.Id },
                    { "Tenant", bIReportExecutionLog.Tenant.ToString() }
                }, tenant, null, null, null, null);

                bIReportXMLData.BIReportsExecutionLogId = bIReportExecutionLog.Id;
                return Request.CreateResponse(HttpStatusCode.OK, bIReportXMLData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetBIReportLogStatus(string bIReportsExecutionLogId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                BIReportsExecutionLogQueryService bIReportsExecutionLogQueryService = new BIReportsExecutionLogQueryService(authToken.Tenant);
                BIReportsExecutionLogList bIReportsExecutionLogList = bIReportsExecutionLogQueryService.GetBIReportsExecutionLogList(bIReportsExecutionLogId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, bIReportsExecutionLogList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetGenerateDigitalPortalDomainAsync(string customerURL, int tenant)
        {
            try
            {
                customerURL = JsonConvert.DeserializeObject<string>(customerURL);
                await RunAddingDNSRecordAsync(customerURL, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"Digital portal generate domain {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

       
        private static async Task RunAddingDNSRecordAsync(string customerURL, int tenant)
        {
            customerURL = customerURL.ToLower();

            if (!IsValidDomain(customerURL))
            {
                throw new Exception("Invalid domain name");
            }

            var isSubDomainIOfTenantManagementUsed = IsSubDomainIOfTenantManagementUsed(customerURL, tenant);

            if (isSubDomainIOfTenantManagementUsed.Item1)
            {
                throw new Exception("The domain already defined for tenant No. " + isSubDomainIOfTenantManagementUsed.Item2);
            }

            var tenantId =  "a46b1446-9af4-4079-87ad-3304ee9ed758";
            var clientId = "23542def-2398-43e4-abc8-61469fffaa7f";
            var secret = LogitudeSettings.AzurePrincipalSecretKey;
            var subscriptionId = "faa01774-0b55-482b-a317-742a1f1479f8";
            var resourceGroupName = "globallogitude";
            var zoneName = LogitudeSettings.DNSZone; 
            var DNSIPAddress = LogitudeSettings.DNSIPAddress;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);
            var dnsClient = new DnsManagementClient(serviceCreds)
            {
                SubscriptionId = subscriptionId
            };

            if (CheckOnDNS(dnsClient, resourceGroupName, zoneName, customerURL, RecordType.CNAME) 
                 || CheckOnDNS(dnsClient, resourceGroupName, zoneName, customerURL, RecordType.A))
            {
                throw new Exception("The domain already exist on the dns");
            }

            try
            {
                // Build the service credentials and DNS management client
                //var recordSetParams = new RecordSet
                //{
                //    TTL = 3600,
                //    CnameRecord = new CnameRecord()
                //    {
                //       Cname = GetDomainData()
                //    }
                //};

                //var recordSet = dnsClient.RecordSets.CreateOrUpdateAsync(resourceGroupName, zoneName, customerURL, RecordType.CNAME, recordSetParams).Result;
                throw new Exception("Add new domain to DNS is not allowed now!!!!");
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                throw e;
            }
        }

        private static bool CheckOnDNS(DnsManagementClient dnsClient, string resourceGroupName, string zoneName, string customerURL, RecordType recordType)
        {
            try
            {
                if (dnsClient.RecordSets.Get(resourceGroupName, zoneName, customerURL, recordType) != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       

        private static bool IsValidDomain(string subDomain)
        {
            if (string.IsNullOrWhiteSpace(subDomain))
            {
                return false;
            }

            if (char.IsDigit(subDomain[0]))
            {
                return false;
            }

            if (subDomain.Contains("."))
            {
                return false;
            }

            var fullDomain = $"{subDomain}.logitudeworld.com";

            // Regex to check valid domain name.
            var pattern = "^(?!-)[A-Za-z0-9-]+([\\-\\.]{1}[a-z0-9]+)*\\.[A-Za-z]{2,6}$";

            var regex = new Regex(pattern);

            if (regex.Match(fullDomain).Success)
            {
                return true;
            }

            return false;
        }

        private static Tuple<bool, int?> IsSubDomainIOfTenantManagementUsed(string subDomain, int tenant)
        {
            var fullDomain = $"{subDomain}.logitudeworld.com";

            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            var tenantManagement = tenantManagementRepository.CheckSubDomainTenantManagement(fullDomain, tenant);
            return tenantManagement;
        }

        #region SendBlockToServer
        int counter = -1;
        long sentBytes = 0;
        int position = 0;
        byte[] currentData;
        string blockId2;
        double value;
        int buffersize = 100000; // 100k
        double blocksNumber;
        string encodedFileName;
        bool isUploadInProgress;
        List<string> blockIdsArray = new List<string>();

        private string SendBlockToServer(bool isFirstTry, byte[] fileData, string ObjectTableName, int tenant)
        {
            counter++;
            string Res = "Faild";
            int byteDifference2 = fileData.Length - Convert.ToInt32(sentBytes);
            if (byteDifference2 > buffersize)
            {
                currentData = new byte[buffersize];
                Buffer.BlockCopy(fileData, position, currentData, 0, buffersize);
            }
            else
            {
                currentData = new byte[byteDifference2];
                Buffer.BlockCopy(fileData, position, currentData, 0, byteDifference2);
            }

            blockId2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            blockIdsArray.Add(blockId2);
            sentBytes += currentData.Length;
            position = Convert.ToInt32(sentBytes);
            value = (Convert.ToDouble(!isFirstTry ? sentBytes : 0) / Convert.ToDouble(fileData.Length)) * 100;
            Uploader uploaderService = new Uploader();
            string result = uploaderService.UploadFile(ObjectTableName + DateTime.Now.ToShortDateString() + ".xls", currentData, fileData.Length, sentBytes, blockIdsArray.ToArray(), counter, null, tenant, "others", null, false, null);
            if (fileData != null)
            {
                if (sentBytes < fileData.Length)
                {
                    SendBlockToServer(false, fileData, ObjectTableName, tenant);
                    Res = ObjectTableName + DateTime.Now.ToShortDateString() + ".xls";
                }
                else
                {
                    // Uploading done successfully
                    if (sentBytes == fileData.Length && result != null)
                    {
                        Res = result;
                    }
                    else
                    {
                        Res = "Faild";
                    }
                }
            }

            return Res;
        }



        #endregion


        public HttpResponseMessage GetHypridPartnerLogo(string LogoId)
        {
            try
            {
                var data = GetFile(LogoId, "jpg", "images", 0);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public byte[] GetFile(string fileid, string extension, string location, int tenant)
        {

            try
            {

                byte[] datainByte;
                string filename = fileid + "." + extension;


                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileid,
                    FolderName = location,
                    Extension = extension,
                    Tenant = tenant,


                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                datainByte = storageservice.Read(fileInfo);

                return datainByte;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public HttpResponseMessage DownLoadAllFilesForShipments(string ShipmentId, string ObjectTableId, int Tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(Tenant);
                var documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                var AllDocs = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(ShipmentId, null, ObjectTableId, "I", Tenant);
                var guid = Guid.NewGuid();
                var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
                base64string = base64string.Substring(0, 22);
                base64string = base64string.Replace("/", "_");
                base64string = base64string.Replace("+", "-");
                base64string = base64string.Replace("_", "0");
                BlobFileInfo zipfileInfo = new BlobFileInfo()
                {
                    FileName = "ShipmentDocuments",
                    //FolderName = "others",
                    HasExternalContainer = true,
                    ExternalContainerName = "tenant" + Tenant.ToString(),
                    Extension = "zip",
                    Tenant = Tenant,
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                // CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(Tenant);
                //var DownLoadBlob = blobContainer.GetBlockBlobReference("ShipmentDocuments.zip");
                Crc32 crc32 = new Crc32();
                bool ShipmentHasFiles = false;
                using (MemoryStream blobStream = new MemoryStream())//DownLoadBlob.OpenWrite())
                {
                    ZipOutputStream stream = new ZipOutputStream(blobStream);
                    stream.SetLevel(3);
                    //if (blobContainer != null)
                    //{

                    foreach (var item in AllDocs)
                    {
                        if (item.HasFile && !item.IsDeleted)
                        {
                            ShipmentRepository ShRepos = new ShipmentRepository(item.Tenant);
                            var ShipmentNumber = ShRepos.GetShipmentNumberByShipmentIdTenant(item.EntityId, item.Tenant);
                            ShipmentHasFiles = true;
                            string documentName = item.DocumentId + "." + item.FileExtension;//TenantContext.Current.Id + "_" + item.DocumentId;// +"." + CurrentDocument.Extension;
                            //var blob = blobContainer.GetBlockBlobReference(documentName);
                            ZipEntry entry = new ZipEntry(item.DocumentTypeCode + "_" + ShipmentNumber + "_" + item.Code + "." + item.FileExtension);//Path.GetExtension(blob.Uri.AbsolutePath));//Path.GetFileName(blob.Uri.AbsolutePath));

                            //
                            string fileName = item.DocumentId + "." + item.FileExtension;
                            //string filePath = "tenant" + Tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");

                            BlobFileInfo fileInfo = new BlobFileInfo()
                            {
                                FileName = item.DocumentId,
                                FolderName = "docsin",
                                Extension = item.FileExtension,
                                Tenant = Tenant,
                                FileSize = item.FileSize,

                            };


                            byte[] datainByte = storageservice.Read(fileInfo);
                            if (datainByte != null)
                            {


                                entry.Size = datainByte.Length;
                                //entry.Name = item.DocumentTypeCode + "_" + item.EntityNumber + "_" + item.Code;
                                crc32.Reset();
                                crc32.Update(datainByte);
                                entry.Crc = crc32.Value;
                                stream.PutNextEntry(entry);
                                int size = 20480;
                                int remaining = datainByte.Length;
                                for (int i = 0; i < datainByte.Length; )
                                {
                                    if (remaining < size)
                                    {
                                        size = remaining;
                                    }
                                    stream.Write(datainByte, i, size);
                                    remaining = remaining - size;
                                    i = i + size;
                                    stream.Flush();
                                }
                            }


                            //
                            entry.DateTime = DateTime.Now;


                        }
                    }
                    stream.Finish();
                    stream.Close();

                    zipfileInfo.FileSize = blobStream.ToArray().Length;
                    storageservice.Write(blobStream.ToArray(), zipfileInfo);
                    //}

                }
                if (ShipmentHasFiles)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, "ShipmentDocuments.zip");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Faild");

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Faild");
            }
           
        }

        public HttpResponseMessage PutReleaseSetting(ReleaseArgs releaseArgs)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
               
                if(releaseArgs.IsDeleteRelease)
                {
                    this.DeleteUsersReleaseNotes(tenant);
                }

                this.UpdateReleaseSettings(releaseArgs);

                return Request.CreateResponse(HttpStatusCode.OK, releaseArgs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

            
        }
        private void DeleteUsersReleaseNotes(int tenant)
        {
            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("delete from UsersReleaseNotesDisplays", cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cn.Open();
                var output = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        private string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
        private void UpdateReleaseSettings(ReleaseArgs releaseArgs)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");

                if (setting == null) return;

                setting.ReleaseDateString = releaseArgs.ReleaseDateString;                
                setting.ReleaseNotesURL = releaseArgs.ReleaseCode;
                settingRepository.Update(setting);
                settingRepository.SubmitChanges();
                scope.Complete();
            }
        }
    }
}