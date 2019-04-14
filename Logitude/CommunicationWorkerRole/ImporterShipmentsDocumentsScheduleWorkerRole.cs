using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    class ImporterShipmentsDocumentsScheduleWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant;
        string URI = "";//"http://localhost:9996/api/";
        APILogsService apiLogsService;
        IWebFreightContext webFreightContext;

        public ImporterShipmentsDocumentsScheduleWorkerRole(int tenant)
        {
            Tenant = tenant;
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterShipmentDocuments";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }
        string Token;
        Contact User;
        string CorrelationId;
        private bool IsImportShipmentsAllowedForLogBox(TenantPM loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareImportFile == true)
            {
                return (entityPM.DirectionId.ToUpper() == "I");
            }
            else
            {
                return false;
            }
        }

        private bool IsExportShipmentsAllowedForLogBox(TenantPM loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareExportFile == true && FeatureToggleHelper.HasFeatureToggle("LEX", loggedTenant.Id))
            {
                return (entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R");
            }
            else
            {
                return false;
            }
        }
        public override async void AsyncRun()
        {
            APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
            {
                PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
                SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
            };
            using (var client = new HttpClient())
            {
                //var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;

                string AuthURI = URI + "APIAuthentication";
                var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(AuthURI, content);
                var tempUser = result.Content.ReadAsStringAsync().Result;
                ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
                Token = User.Token;
            }

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        //int tenant = 0;

                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("ImportersShipmentsDocsScheduleQueue", 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;
                        int importerTenant = 0;

                        if (response != null && response.MessageId != null)
                        {
                            
                            string ShipmentId = response.MessageValues["ShipmentId"].ToString();
                            string BatchNumber = response.MessageValues["BatchNumber"].ToString();
                            string CustomerId = response.MessageValues["CustomerId"].ToString();
                            //string ImporterShipmentId = response.MessageValues["ImporterShipmentId"].ToString();
                            //int.TryParse(response.MessageValues["ImporterTenant"], out importerTenant);
                            int.TryParse(response.MessageValues["Tenant"], out Tenant);
                            tenant = Tenant;

                            string TempCorrelationId = response.MessageValues["CorrelationId"].ToString();
                            if (string.IsNullOrEmpty(CorrelationId))
                            {
                                CorrelationId = TempCorrelationId;
                            }
                            ContactRepository contactRepository = new ContactRepository(tenant);
                            User = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
                            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                            var DocumentFilingPMsIds = documentsFilingQuery.GetDocumentsFilingPMsIdsByEntityId(ShipmentId, "I", Tenant);
                            if (DocumentFilingPMsIds != null && DocumentFilingPMsIds.Count > 0)
                            {
                                #region API Initialization
                                webFreightContext = WebFreightContext.GetContext(tenant);
                                var aPILogsRepository = new APILogsRepository(webFreightContext);
                                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, tenant);
                                APILogsPM LogPM;
                                bool IsNewLog = false;
                                if (Log == null)
                                {
                                    IsNewLog = true;

                                    LogPM = new APILogsPM()
                                    {
                                        Id = IdCounter.GetNumber("APILogs", tenant),
                                        CorrelationId = CorrelationId,
                                        CreateDate = DateTime.Now,
                                        CreateDateUTC = DateTime.UtcNow,
                                        Direction = "O",
                                        LastUpdateDate = DateTime.Now,
                                        LastUpdateDateUTC = DateTime.UtcNow,
                                        NumberOfRetries = 1,
                                        ExpirationDate = DateTime.Now.AddDays(90),
                                        Status = "I"
                                    };
                                }
                                else
                                {
                                    IsNewLog = false;

                                    LogPM = new APILogsPM()
                                    {
                                        Id = Log.Id,
                                        CorrelationId = Log.CorrelationId,
                                        CreateDate = Log.CreateDate,
                                        CreateDateUTC = Log.CreateDateUTC,
                                        Direction = Log.Direction,
                                        EntityId = Log.EntityId,
                                        LastUpdateDate = Log.LastUpdateDate,
                                        LastUpdateDateUTC = Log.LastUpdateDateUTC,
                                        NumberOfRetries = Log.NumberOfRetries++,
                                        ObjectTableId = Log.ObjectTableId,
                                        ExpirationDate = Log.ExpirationDate,
                                        Refrence = Log.Refrence,
                                        Status = "I",
                                        Tenant = Log.Tenant,

                                    };
                                }
                                #endregion

                                try
                                {
                                    foreach (var DocumentFilingPMId in DocumentFilingPMsIds)
                                    {
                                        DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingPMId, tenant);
                                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                        ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                        ShipmentPM ForwarderShipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, ForwarderShipment.CustomerId);
                                        var tenantQuery = new TenantQuery(ForwarderShipment.Tenant);
                                        var tenantPM = tenantQuery.GetSinglePM(ForwarderShipment.Tenant);

                                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && tenantPM.IsCustomerTenantShare && (ForwarderShipment.DirectionId.ToUpper() == "C" || IsImportShipmentsAllowedForLogBox(tenantPM, ForwarderShipment) || IsExportShipmentsAllowedForLogBox(tenantPM,ForwarderShipment)))
                                        {
                                            importerTenant = customerTenantAccessInfo.CustomerTenant;
                                            ShipmentPM ImporterShipment = null;
                                            //DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, tenant);
                                            string EntityId = "";
                                            if (ForwarderShipment != null)
                                            {
                                                if (ForwarderShipment.DirectionId.ToUpper() == "I" && !string.IsNullOrEmpty(ForwarderShipment.CustomFileId))
                                                {

                                                    var CustomsShipmentPM = shipmentQuery.GetSingleShipmentPMByNumber(ForwarderShipment.CustomFileId, tenant); // todo: I Should Ask About this
                                                    if (CustomsShipmentPM != null && !string.IsNullOrEmpty(CustomsShipmentPM.CustomerShipmentNumber))
                                                    {
                                                        EntityId = CustomsShipmentPM.Id;
                                                    }

                                                }
                                                else if (ForwarderShipment.DirectionId.ToUpper() == "C" || ForwarderShipment.DirectionId.ToUpper() == "E")//&& !string.IsNullOrEmpty(ForwarderShipment.CustomFileId))
                                                {
                                                    ImporterShipment = shipmentQuery.GetSingleShipmentPMByNumber(ForwarderShipment.CustomerShipmentNumber, importerTenant);
                                                    EntityId = ImporterShipment.Id;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(EntityId) && !string.IsNullOrEmpty(DocumentFilingPMId))
                                            {
                                                #region Check if new or edit document

                                                var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;
                                                bool IsNew = false;
                                                using (var client = new HttpClient())
                                                {
                                                    client.DefaultRequestHeaders.Add("Token", Token);
                                                    using (var apiresponse = await client.GetAsync(GetURI))
                                                    {
                                                        if (apiresponse.IsSuccessStatusCode)
                                                        {

                                                            var IsNewJsonString = apiresponse.Content.ReadAsStringAsync().Result;
                                                            var tempResult = JsonConvert.DeserializeObject(IsNewJsonString);
                                                            if (tempResult != null)
                                                            {
                                                                IsNew = (bool)tempResult;
                                                            }
                                                        }
                                                    }
                                                }

                                                #endregion

                                                byte[] datainByte = null;
                                                var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                                LogPM.ObjectTableId = Objecttable.Id;
                                                LogPM.EntityId = ForwarderShipment.Id;
                                                LogPM.Refrence = ForwarderShipment.ShipmentNumber;
                                                LogPM.Tenant = ForwarderShipment.Tenant;
                                                List<DocumentsFilingMetaDataValueAM> DocFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>();
                                                ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);
                                                if (DocumentFilingPM.DocumentsFilingMetaDataValues != null && DocumentFilingPM.DocumentsFilingMetaDataValues.Count > 0)
                                                {
                                                    DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                                                    foreach (var item in DocumentFilingPM.DocumentsFilingMetaDataValues)
                                                    {
                                                        var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.Id, item.Tenant);
                                                        DocFilingMetaDataValues.Add(new DocumentsFilingMetaDataValueAM() { DocumentsMetaDataType = new CodeProperties() { Code = DocFilingType.Code }, MetaDataValue = item.MetaDataValue, ChangeSetOp = item.ChangeSetOp });
                                                    }
                                                }
                                                using (var client = new HttpClient())
                                                {
                                                    client.DefaultRequestHeaders.Add("Token", Token);
                                                    client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                                    string ImporterShipmentDocumentsURI = URI + "ImporterShipmentDocuments";

                                                    #region Get Document File
                                                    if (DocumentFilingPM.HasFile)
                                                    {
                                                        string fileName = DocumentFilingPM.DocumentId + "." + DocumentFilingPM.FileExtension;
                                                        string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), DocumentFilingPM.Folder);
                                                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                                                        BlobFileInfo fileInfo = new BlobFileInfo()
                                                        {
                                                            FileName = DocumentFilingPM.DocumentId,
                                                            FolderName = DocumentFilingPM.Folder,
                                                            Extension = DocumentFilingPM.FileExtension,
                                                            Tenant = tenant,
                                                            FileSize = DocumentFilingPM.FileSize,

                                                        };
                                                        datainByte = storageservice.Read(fileInfo);
                                                         
                                                    }
                                                    #endregion

                                                    Objecttable = objectTabelRepository.GetSingleObjectTable(DocumentFilingPM.ObjectTableId, tenant, true);
                                                    if (IsNew)
                                                    {
                                                        #region New Document
                                                        #region DocumentsFilingAMProperties
                                                        DocumentsFilingAM NewDocumentFilingAM = new DocumentsFilingAM()
                                                        {
                                                            ImporterTenant = importerTenant,
                                                            ForwarderDocumentId = DocumentFilingPM.Id,
                                                            EntityNumber = ImporterShipment.Id,
                                                            DontAddToQueue = true,
                                                            DocumentType = new CodeProperties()
                                                            {
                                                                Code = DocumentFilingPM.DocumentTypeCode
                                                            },
                                                            DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>(),//DocFilingMetaDataValues,
                                                            Description = DocumentFilingPM.Description,
                                                            FileSize = datainByte != null ? datainByte.Length : 0,
                                                            ObjectTableName = Objecttable.Name,
                                                            FileData = datainByte,
                                                            Extension = DocumentFilingPM.FileExtension,
                                                            IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
                                                            SignersList = DocumentFilingPM.SignersList

                                                        };

                                                        if (DocumentFilingPM.DocumentsFilingMetaDataValues != null && DocumentFilingPM.DocumentsFilingMetaDataValues.Count > 0)
                                                        {
                                                            //if (NewDocumentFilingAM.DocumentsFilingMetaDataValues == null)
                                                            //{
                                                            //    NewDocumentFilingAM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>();
                                                            //}
                                                            DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                                                            foreach (var item in DocumentFilingPM.DocumentsFilingMetaDataValues)
                                                            {
                                                                var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.Id, item.Tenant);
                                                                NewDocumentFilingAM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValueAM() { DocumentsMetaDataType = new CodeProperties() { Code = DocFilingType.Code }, MetaDataValue = item.MetaDataValue, ChangeSetOp = item.ChangeSetOp });
                                                            }
                                                        }

                                                        #endregion

                                                        #region Calling API and Back From It
                                                        LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
                                                        if (IsNewLog)
                                                        {
                                                            LogPM.BatchNumber = BatchNumber;
                                                            LogPM.CustomerId = CustomerId;
                                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                                            apiLogsService.Create(LogPM);
                                                        }
                                                        var msg = "Start Sending New Document To Importer " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingAM), null, null, "");

                                                        var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
                                                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                        var result = await client.PostAsync(ImporterShipmentDocumentsURI, content);
                                                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                                        {
                                                            var temp1 = result.Content.ReadAsStringAsync().Result;
                                                            var Donemsg = "New Document Sent To Importer Successfully " + DateTime.Now;
                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp1, null, "");


                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, "Start Update CustomerDocumentId In Forwarder Document " + DateTime.Now, null, null, null, "");
                                                            try
                                                            {
                                                                string ImporterDocId = JsonConvert.DeserializeObject<string>(temp1);
                                                                var DocFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingPM.Id, tenant);
                                                                DocFilingPM.CustomerDocumentId = ImporterDocId;
                                                                DocFilingPM.DontAddToQueue = true;
                                                                DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocFilingPM.Tenant);
                                                                documentsFilingService.Update(DocFilingPM, null, User.Id);
                                                                CorrelationId = Guid.NewGuid().ToString();
                                                                //queueservice.Complete();
                                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, "Forwarder Document Updated Successfully" + DateTime.Now, null, null, null, "");

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                var Failmsg = ex.Message + " Fail To Update Forwarder Document " + DateTime.Now;
                                                                string errorMessage = ex.Message + Environment.NewLine;

                                                                if (ex.InnerException != null)
                                                                {

                                                                    errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

                                                                }

                                                                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                                                                throw (ex);
                                                            }
                                                        }
                                                        else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                        {
                                                            var temp1 = result.Content.ReadAsStringAsync().Result;
                                                            APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                            if (EXC != null)
                                                            {
                                                                var Failmsg = EXC.ErrorType + " Fail To Send New Document To Importer " + DateTime.Now;
                                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                                throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                            }
                                                        }
                                                        #endregion
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Edit Document
                                                        #region DocumentFilingPMProperties

                                                        DocumentsFilingAM NewDocumentFilingAM = new DocumentsFilingAM()
                                                        {
                                                            ImporterTenant = importerTenant,
                                                            EntityNumber = ImporterShipment.Id,
                                                            DontAddToQueue = true,
                                                            DocumentType = new CodeProperties()
                                                            {
                                                                Code = DocumentFilingPM.DocumentTypeCode
                                                            },
                                                            DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>(),//DocFilingMetaDataValues,
                                                            Description = DocumentFilingPM.Description,
                                                            FileSize = datainByte != null ? datainByte.Length : 0,
                                                            ObjectTableName = Objecttable.Name,
                                                            FileData = datainByte,
                                                            CustomerDocumentId = DocumentFilingPM.CustomerDocumentId,
                                                            Extension = DocumentFilingPM.FileExtension,
                                                            IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
                                                            SignersList = DocumentFilingPM.SignersList


                                                        };

                                                        if (DocumentFilingPM.DocumentsFilingMetaDataValues != null && DocumentFilingPM.DocumentsFilingMetaDataValues.Count > 0)
                                                        {
                                                            //if (NewDocumentFilingAM.DocumentsFilingMetaDataValues == null)
                                                            //{
                                                            //    NewDocumentFilingAM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>();
                                                            //}
                                                            DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                                                            foreach (var item in DocumentFilingPM.DocumentsFilingMetaDataValues)
                                                            {
                                                                var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.Id, item.Tenant);
                                                                NewDocumentFilingAM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValueAM() { DocumentsMetaDataType = new CodeProperties() { Code = DocFilingType.Code }, MetaDataValue = item.MetaDataValue, ChangeSetOp = item.ChangeSetOp });
                                                            }
                                                        }

                                                        #endregion

                                                        #region Calling API and Back From It
                                                        LogPM.Subject = "Send Document Updates To Importer ";
                                                        if (IsNewLog)
                                                        {
                                                            LogPM.BatchNumber = BatchNumber;
                                                            LogPM.CustomerId = CustomerId;
                                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                                            apiLogsService.Create(LogPM);
                                                        }
                                                        var msg = "Start Sending Document Updates To Importer " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingAM), null, null, "");

                                                        var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
                                                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                        var result = await client.PutAsync(ImporterShipmentDocumentsURI, content);
                                                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                                        {
                                                            CorrelationId = Guid.NewGuid().ToString();
                                                            var temp1 = result.Content.ReadAsStringAsync().Result;
                                                            var Donemsg = "Document Updates Sent To Importer Successfully " + DateTime.Now;
                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp1, null, "");
                                                            //queueservice.Complete();
                                                        }
                                                        else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                        {
                                                            var temp1 = result.Content.ReadAsStringAsync().Result;
                                                            APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                            if (EXC != null)
                                                            {
                                                                var Failmsg = EXC.ErrorType + " Fail To Send Document Updates To Importer " + DateTime.Now;
                                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                                throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                            }
                                                        }
                                                        #endregion
                                                        #endregion
                                                    }
                                                }

                                            }
                                            CorrelationId = Guid.NewGuid().ToString();
                                            queueservice.Complete(); 
                                        } 
                                        LogDoneItemInMemory();
                                    }


                                }
                                catch (Exception ex)
                                {
                                    #region Exception handling
                                    ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);

                                    var Failmsg = ex.Message + " " + DateTime.Now;
                                    string errorMessage = ex.Message + Environment.NewLine;

                                    if (ex.InnerException != null)
                                    {

                                        errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

                                    }

                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                    // APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage)); 
                                    if (response.RetryNumber <= 1)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                    }

                                    if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                    }
                                    if (response.RetryNumber >= 3)
                                    {
                                        queueservice.CompleteAsFailed();
                                    }



                                    #endregion
                                }
                            }
                            else
                            {
                                queueservice.Complete();
                            }
                        }
                        else
                        {
                            Thread.Sleep(10000);
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("ImportersShipmentDocumentsQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }

    }
}
