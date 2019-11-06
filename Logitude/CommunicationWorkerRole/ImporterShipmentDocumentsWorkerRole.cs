using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
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
    class ImporterShipmentDocumentsWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant;
        string URI = "";//"http://localhost:9996/api/";
        APILogsService apiLogsService;
        IWebFreightContext webFreightContext;
        int buffersize = 100000; // 100k
        int counter = -1;
        long sentBytes = 0;
        int position = 0;
        byte[] currentData;
        string blockId2;
        double value;
        List<string> blockIdsArray;
        double blocksNumber;
        string encodedFileName;
        bool isUploadInProgress;
        string DocId;
        bool AddDocumentFiling;
        public ImporterShipmentDocumentsWorkerRole(string tenant)
        {
            Tenant = int.Parse(tenant);
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

        private bool IsExportShipmentsAllowedForLogBox(TenantPM loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareExportFile == true)// && FeatureToggleHelper.HasFeatureToggle("LEX", loggedTenant.Id)
            {
                return (entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R");
            }
            else
            {
                return false;
            }
        }
        private bool IsImporterTenantHasExportFeatureForExportShipments(int ImporterTenant, ShipmentPM entityPM, int tenant)
        {
            if ((entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R") && !FeatureToggleHelper.HasFeatureToggle("LEX", ImporterTenant, tenant))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override async void AsyncRun()
        {
            try
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
                        APILogsPM LogPM = null;
                        try
                        {
                            //int tenant = 0;

                            queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentDocumentsQueue", Tenant);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;
                            int tenant = 0;
                            int importerTenant = 0;

                            if (response != null && response.MessageId != null)
                            {
                                string ShipmentId = response.MessageValues["ShipmentId"] != null ? response.MessageValues["ShipmentId"].ToString():"";
                                string DocumentFilingId = response.MessageValues["DocumentFilingId"].ToString();
                                int.TryParse(response.MessageValues["Tenant"], out tenant);
                                string CorrelationId = response.MessageValues["CorrelationId"].ToString();
                                ContactRepository contactRepository = new ContactRepository(tenant);
                                User = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
                                #region API Initialization
                                webFreightContext = WebFreightContext.GetContext(tenant);
                                var aPILogsRepository = new APILogsRepository(webFreightContext);
                                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, tenant);
                                
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
                                        Status = "I",
                                        QueueMessageMoreDetailsId = response.MessageId,
                                        Tenant = tenant
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
                                        QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId

                                    };
                                }
                                #endregion

                                try
                                {
                                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                                    DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, tenant);
                                    if (!string.IsNullOrEmpty(ShipmentId) && !DocumentFilingPM.IsDeleted)
                                    {
                                        LogPM.Tenant = tenant;
                                        LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
                                        LogPM.Refrence = DocumentFilingPM.Code;
                                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                        ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                        ShipmentPM ForwarderShipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, ForwarderShipment.CustomerId);
                                        var tenantQuery = new TenantQuery(ForwarderShipment.Tenant);
                                        var tenantPM = tenantQuery.GetSinglePM(ForwarderShipment.Tenant);
                                        TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
                                        if (customerTenantAccessInfo != null)
                                        {
                                            var customerTenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(tenant, customerTenantAccessInfo.CustomerTenant);
                                            if (customerTenantAccess != null)
                                            {
                                                LogPM.PartnerName = customerTenantAccess.CompanyName + " ( " + customerTenantAccess.CustomerTenant + " )";
                                            }
                                        }
                                        
                                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && tenantPM.IsCustomerTenantShare)// && (tenantPM.CustomerTenantShareImportFile ? ForwarderShipment.DirectionId.ToUpper() == "I" || ForwarderShipment.DirectionId.ToUpper() == "C" : ForwarderShipment.DirectionId.ToUpper() == "C"))
                                        {
                                            importerTenant = customerTenantAccessInfo.CustomerTenant;
                                            
                                            ShipmentPM ImporterShipment = null;

                                            string EntityNumber = "";
                                            if (ForwarderShipment != null)
                                            {
                                                if (!IsImporterTenantHasExportFeatureForExportShipments(importerTenant, ForwarderShipment, tenant))
                                                {
                                                    queueservice.Complete(); 
                                                }
                                                else
                                                {
                                                    LogPM.Tenant = ForwarderShipment.Tenant;
                                                    LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
                                                    LogPM.Refrence = DocumentFilingPM.Code;
                                                    if (IsNewLog)
                                                    {
                                                        apiLogsService = new APILogsService(webFreightContext, tenant);
                                                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                        LogPM.QueueType = "Document";
                                                        apiLogsService.Create(LogPM);
                                                        IsNewLog = false;
                                                    }
                                                    var msg = "Start Checking Parent Entity Direction" + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                                                    if (ForwarderShipment.DirectionId.ToUpper() == "I" && !string.IsNullOrEmpty(ForwarderShipment.CustomFileId))
                                                    {

                                                        var CustomsShipmentPM = shipmentQuery.GetSingleShipmentPM(ForwarderShipment.CustomFileId, tenant); // todo: I Should Ask About this
                                                        msg = "Getting Custom shipment Id for current import shipment ( " + ForwarderShipment.CustomFileId + " )" + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                                                        if (CustomsShipmentPM != null && !string.IsNullOrEmpty(CustomsShipmentPM.CustomerShipmentNumber))
                                                        {
                                                            EntityNumber = CustomsShipmentPM.CustomerShipmentNumber;
                                                        }

                                                    }
                                                    else if (ForwarderShipment.DirectionId.ToUpper() == "C" || (IsExportShipmentsAllowedForLogBox(tenantPM, ForwarderShipment)))//&& !string.IsNullOrEmpty(ForwarderShipment.CustomFileId))
                                                    {
                                                        //ImporterShipment = shipmentQuery.GetSingleShipmentPMByNumber(ForwarderShipment.CustomerShipmentNumber, importerTenant);
                                                        msg = "Getting Custom shipment number" + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                                                        EntityNumber = ForwarderShipment.CustomerShipmentNumber;// ImporterShipment.ShipmentNumber;
                                                    }
                                                }
                                                
                                            }
                                            if (!string.IsNullOrEmpty(EntityNumber))
                                            {
                                                #region Check if new or edit document

                                                var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + Uri.EscapeDataString(DocumentFilingPM.Id) + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;
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

                                                if (IsNew && !string.IsNullOrEmpty(DocumentFilingPM.CustomerDocumentId))
                                                {
                                                    GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + Uri.EscapeDataString(DocumentFilingPM.CustomerDocumentId) + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;
                                                    //IsNew = false;
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
                                                }

                                                #endregion

                                                byte[] datainByte = null;
                                                var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                                LogPM.ObjectTableId = Objecttable.Id;
                                                LogPM.EntityId = EntityNumber;
                                                LogPM.Refrence = DocumentFilingPM.Code;
                                                LogPM.Tenant = ForwarderShipment.Tenant;
                                                List<DocumentsFilingMetaDataValueAM> DocFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>();
                                                ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);


                                                using (var client = new HttpClient())
                                                {
                                                    client.DefaultRequestHeaders.Add("Token", Token);
                                                    client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                                    string ImporterShipmentDocumentsURI = URI + "ImporterShipmentDocuments";

                                                    #region Get Document File
                                                    BlobFileInfo fileInfo = null;
                                                    if (DocumentFilingPM.HasFile)
                                                    {
                                                        string fileName = DocumentFilingPM.DocumentId + "." + DocumentFilingPM.FileExtension;
                                                        string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), DocumentFilingPM.Folder);
                                                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                                                        fileInfo = new BlobFileInfo()
                                                        {
                                                            FileName = DocumentFilingPM.DocumentId,
                                                            FolderName = DocumentFilingPM.Folder,
                                                            Extension = DocumentFilingPM.FileExtension,
                                                            Tenant = tenant,
                                                            FileSize = DocumentFilingPM.FileSize,

                                                        };
                                                        datainByte = storageservice.Read(fileInfo);
                                                        if (datainByte == null)
                                                        {
                                                            throw new Exception("The physical file for this Document may be Damaged or not exists. ");
                                                        }

                                                    }
                                                    #endregion

                                                    Objecttable = objectTabelRepository.GetSingleObjectTable(DocumentFilingPM.ObjectTableId, tenant, true);
                                                    if (IsNew && !ForwarderShipment.IsCancelled)
                                                    {
                                                        #region New Document
                                                        #region DocumentsFilingAMProperties
                                                        DocumentsFilingAM NewDocumentFilingAM = new DocumentsFilingAM()
                                                        {
                                                            ImporterTenant = importerTenant,
                                                            ForwarderDocumentId = DocumentFilingPM.Id,
                                                            EntityNumber = EntityNumber,//ImporterShipment.Id,
                                                            DontAddToQueue = true,
                                                            DocumentType = new CodeProperties()
                                                            {
                                                                Code = DocumentFilingPM.DocumentTypeCode
                                                            },
                                                            DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>(),//DocFilingMetaDataValues,
                                                            Description = DocumentFilingPM.Description,
                                                            FileSize = datainByte != null ? datainByte.Length : 0,
                                                            ObjectTableName = Objecttable.Name,
                                                            //FileData = datainByte,
                                                            Extension = DocumentFilingPM.FileExtension,
                                                            IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
                                                            SignersList = DocumentFilingPM.SignersList,
                                                            FileName = DocumentFilingPM.FileName,
                                                            IsSharedWithCustomer = DocumentFilingPM.IsSharedWithCustomer,
                                                            IsDeleted = ForwarderShipment.IsCancelled ? true : DocumentFilingPM.IsDeleted,
                                                            Code = DocumentFilingPM.Code,
                                                            ExternalCode = DocumentFilingPM.Code,
                                                            IsRequested = DocumentFilingPM.IsRequested,
                                                            Tenant = DocumentFilingPM.Tenant,
                                                            Notes = DocumentFilingPM.Notes,

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
                                                                var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.DocumentsMetaDataTypeId, item.Tenant);
                                                                if (DocFilingType != null)
                                                                {
                                                                    NewDocumentFilingAM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValueAM() { DocumentsMetaDataType = new CodeProperties() { Code = DocFilingType.Code }, MetaDataValue = item.MetaDataValue, ChangeSetOp = (item.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.None ? Simplog.Server.Infrastructure.ChangeSetOperation.Insert : item.ChangeSetOp)  });
                                                                }
                                                                else
                                                                {
                                                                    throw new Exception("Missing Data Error", new Exception("DocumentsMetaDataType does not exist in Importer tenant"));
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            NewDocumentFilingAM.DocumentsFilingMetaDataValues = null;
                                                        }
                                                        if (fileInfo != null)
                                                        {
                                                            NewDocumentFilingAM.FileInfo = new FileInformation()
                                                            {
                                                                FileName = fileInfo.FileName + "." + fileInfo.Extension,
                                                                FileSize = datainByte.Length,
                                                                Tenant = importerTenant,
                                                            };
                                                            //StartUploading(fileInfo, datainByte, Token, importerTenant, NewDocumentFilingAM);
                                                            AddDocumentFiling = false;
                                                            blocksNumber = Math.Ceiling(Convert.ToDouble(datainByte.Length) / buffersize);
                                                            blockIdsArray = new List<string>();
                                                            encodedFileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('/', 'A').ToLower();
                                                            sentBytes = 0;
                                                            counter = -1;
                                                            position = 0;
                                                            while (sentBytes < datainByte.Length)
                                                            {

                                                                counter++;
                                                                int byteDifference2 = datainByte.Length - Convert.ToInt32(sentBytes);
                                                                if (byteDifference2 > buffersize)
                                                                {
                                                                    currentData = new byte[buffersize];
                                                                    Buffer.BlockCopy(datainByte, position, currentData, 0, buffersize);
                                                                }
                                                                else
                                                                {
                                                                    currentData = new byte[byteDifference2];
                                                                    Buffer.BlockCopy(datainByte, position, currentData, 0, byteDifference2);
                                                                }

                                                                blockId2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                                                                blockIdsArray.Add(blockId2);
                                                                sentBytes += currentData.Length;
                                                                position = Convert.ToInt32(sentBytes);
                                                                value = (Convert.ToDouble(sentBytes) / Convert.ToDouble(datainByte.Length)) * 100;
                                                                NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                                                NewDocumentFilingAM.FileInfo.buffer = currentData;
                                                                NewDocumentFilingAM.FileInfo.SentSize = sentBytes;
                                                                NewDocumentFilingAM.FileInfo.BufferNumber = counter;
                                                                NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                                                NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                                                var serializedObj = JsonConvert.SerializeObject(NewDocumentFilingAM);
                                                                var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                                                var resultData = await client.PostAsync(URI + "ImporterShipmentDocuments", contentData);
                                                                if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                                                                {
                                                                    string temp1 = resultData.Content.ReadAsStringAsync().Result;
                                                                    NewDocumentFilingAM.DocumentId = JsonConvert.DeserializeObject<string>(temp1);
                                                                    NewDocumentFilingAM.FileInfo.DocumentId = NewDocumentFilingAM.DocumentId;
                                                                }
                                                                else
                                                                {
                                                                    var temp1 = resultData.Content.ReadAsStringAsync().Result;
                                                                    APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                                    if (EXC != null)
                                                                    {
                                                                        var Failmsg = EXC.ErrorType + " Fail To Send New Document To Importer " + DateTime.Now;
                                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                                        throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                                    }
                                                                }

                                                            }
                                                            sentBytes = 0;
                                                            position = 0;
                                                            counter = -1;
                                                            NewDocumentFilingAM.FileInfo = null;
                                                        }
                                                        #endregion

                                                        #region Calling API and Back From It
                                                        LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
                                                        if (IsNewLog)
                                                        {
                                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                                            LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                            LogPM.QueueType = "Document";
                                                            apiLogsService.Create(LogPM);
                                                            IsNewLog = false;
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
                                                                DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, tenant);
                                                                if (DocumentFilingPM.IsDeleted || ForwarderShipment.IsCancelled)
                                                                {
                                                                    DocumentFilingPM.CustomerDocumentId = ImporterDocId;
                                                                }
                                                                else
                                                                {
                                                                    DocumentFilingPM.CustomerDocumentId = ImporterDocId;
                                                                }
                                                                DocumentFilingPM.CustomerTenantNumber = importerTenant;
                                                                DocumentFilingPM.DontAddToQueue = true;
                                                                DocumentFilingPM.NoAddToTasksQueue = true;
                                                                DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocumentFilingPM.Tenant);
                                                                documentsFilingService.Update(DocumentFilingPM, null, User.Id);
                                                                queueservice.Complete();
                                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, "Forwarder Document Updated Successfully" + DateTime.Now, null, null, null, "");

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                var Failmsg = ex.Message + " Fail To Update Forwarder Document " + DateTime.Now;
                                                                string errorMessage = ex.Message + Environment.NewLine;

                                                                if (ex.InnerException != null)
                                                                {

                                                                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

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
                                                            EntityNumber = EntityNumber,//ImporterShipment.Id,
                                                            ForwarderDocumentId = DocumentFilingPM.Id,
                                                            DontAddToQueue = true,
                                                            DocumentType = new CodeProperties()
                                                            {
                                                                Code = DocumentFilingPM.DocumentTypeCode
                                                            },
                                                            DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>(),//DocFilingMetaDataValues,
                                                            Description = DocumentFilingPM.Description,
                                                            FileSize = datainByte != null ? datainByte.Length : 0,
                                                            ObjectTableName = Objecttable.Name,
                                                            //FileData = datainByte,
                                                            CustomerDocumentId = DocumentFilingPM.CustomerDocumentId,
                                                            Extension = DocumentFilingPM.FileExtension,
                                                            IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
                                                            SignersList = DocumentFilingPM.SignersList,
                                                            FileName = DocumentFilingPM.FileName,
                                                            IsSharedWithCustomer = DocumentFilingPM.IsSharedWithCustomer,
                                                            IsDeleted = ForwarderShipment.IsCancelled ? true : DocumentFilingPM.IsDeleted,
                                                            Code = DocumentFilingPM.Code,
                                                            IsRequested = DocumentFilingPM.IsRequested,
                                                            Tenant = DocumentFilingPM.Tenant,
                                                            Notes = DocumentFilingPM.Notes,
                                                            ExternalCode = DocumentFilingPM.Code,

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
                                                                var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.DocumentsMetaDataTypeId, item.Tenant);
                                                                NewDocumentFilingAM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValueAM() { DocumentsMetaDataType = new CodeProperties() { Code = DocFilingType.Code }, MetaDataValue = item.MetaDataValue, ChangeSetOp = (item.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.None ? Simplog.Server.Infrastructure.ChangeSetOperation.Insert : item.ChangeSetOp) });
                                                            }
                                                        }
                                                        else
                                                        {
                                                            NewDocumentFilingAM.DocumentsFilingMetaDataValues = null;
                                                        }
                                                        if (fileInfo != null)
                                                        {
                                                            NewDocumentFilingAM.FileInfo = new FileInformation()
                                                            {
                                                                FileName = fileInfo.FileName + "." + fileInfo.Extension,
                                                                FileSize = datainByte.Length,
                                                                Tenant = importerTenant,
                                                            };
                                                            //StartUploading(fileInfo, datainByte, Token, importerTenant, NewDocumentFilingAM);
                                                            if (fileInfo != null)
                                                            {
                                                                NewDocumentFilingAM.FileInfo = new FileInformation()
                                                                {
                                                                    FileName = fileInfo.FileName + "." + fileInfo.Extension,
                                                                    FileSize = datainByte.Length,
                                                                    Tenant = importerTenant,
                                                                };
                                                                //StartUploading(fileInfo, datainByte, Token, importerTenant, NewDocumentFilingAM);
                                                                AddDocumentFiling = false;
                                                                blocksNumber = Math.Ceiling(Convert.ToDouble(datainByte.Length) / buffersize);
                                                                blockIdsArray = new List<string>();
                                                                encodedFileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('/', 'A').ToLower();
                                                                sentBytes = 0;
                                                                counter = -1;
                                                                position = 0;
                                                                while (sentBytes < datainByte.Length)
                                                                {

                                                                    counter++;
                                                                    int byteDifference2 = datainByte.Length - Convert.ToInt32(sentBytes);
                                                                    if (byteDifference2 > buffersize)
                                                                    {
                                                                        currentData = new byte[buffersize];
                                                                        Buffer.BlockCopy(datainByte, position, currentData, 0, buffersize);
                                                                    }
                                                                    else
                                                                    {
                                                                        currentData = new byte[byteDifference2];
                                                                        Buffer.BlockCopy(datainByte, position, currentData, 0, byteDifference2);
                                                                    }

                                                                    blockId2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                                                                    blockIdsArray.Add(blockId2);
                                                                    sentBytes += currentData.Length;
                                                                    position = Convert.ToInt32(sentBytes);
                                                                    value = (Convert.ToDouble(sentBytes) / Convert.ToDouble(datainByte.Length)) * 100;
                                                                    NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                                                    NewDocumentFilingAM.FileInfo.buffer = currentData;
                                                                    NewDocumentFilingAM.FileInfo.SentSize = sentBytes;
                                                                    NewDocumentFilingAM.FileInfo.BufferNumber = counter;
                                                                    NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                                                    NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                                                    var serializedObj = JsonConvert.SerializeObject(NewDocumentFilingAM);
                                                                    var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                                                    var resultData = await client.PutAsync(URI + "ImporterShipmentDocuments", contentData);
                                                                    if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                                                                    {
                                                                        string temp1 = resultData.Content.ReadAsStringAsync().Result;
                                                                        NewDocumentFilingAM.DocumentId = JsonConvert.DeserializeObject<string>(temp1);
                                                                        NewDocumentFilingAM.FileInfo.DocumentId = NewDocumentFilingAM.DocumentId;
                                                                    }
                                                                    else
                                                                    {
                                                                        var temp1 = resultData.Content.ReadAsStringAsync().Result;
                                                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                                        if (EXC != null)
                                                                        {
                                                                            var Failmsg = EXC.ErrorType + " Fail To Send New Document To Importer - Customer Side " + DateTime.Now;
                                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                                        }
                                                                    }

                                                                }
                                                                sentBytes = 0;
                                                                position = 0;
                                                                counter = -1;
                                                                NewDocumentFilingAM.FileInfo = null;
                                                            }
                                                        }
                                                        #endregion

                                                        #region Calling API and Back From It
                                                        LogPM.Subject = "Send Document Updates To Importer ";
                                                        if (IsNewLog)
                                                        {
                                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                                            LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                            LogPM.QueueType = "Document";
                                                            apiLogsService.Create(LogPM);
                                                            IsNewLog = false;
                                                        }
                                                        var msg = "Start Sending Document Updates To Importer " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingAM), null, null, "");

                                                        var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
                                                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                        var result = await client.PutAsync(ImporterShipmentDocumentsURI, content);
                                                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                                        {
                                                            var temp1 = result.Content.ReadAsStringAsync().Result;
                                                            string ImporterDocId = JsonConvert.DeserializeObject<string>(temp1);
                                                            DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, tenant);
                                                            DocumentFilingPM.CustomerTenantNumber = importerTenant;
                                                            if (DocumentFilingPM.IsDeleted || ForwarderShipment.IsCancelled)
                                                            {
                                                                DocumentFilingPM.CustomerDocumentId = null;
                                                                DocumentFilingPM.DontAddToQueue = true;

                                                            }
                                                            DocumentFilingPM.DontAddToQueue = true;
                                                            DocumentFilingPM.NoAddToTasksQueue = true;
                                                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocumentFilingPM.Tenant);
                                                            documentsFilingService.Update(DocumentFilingPM, null, User.Id);
                                                            var Donemsg = "Document Updates Sent To Importer Successfully " + DateTime.Now;
                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp1, null, "");
                                                            queueservice.Complete();
                                                        }
                                                        else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                        {
                                                            var temp1 = result.Content.ReadAsStringAsync().Result;
                                                            APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                            if (EXC != null)
                                                            {
                                                                var Failmsg = EXC.ErrorType + " Fail To Send Document Updates To Importer - Customer Side" + DateTime.Now;
                                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                                throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                            }
                                                        }
                                                        #endregion
                                                        #endregion
                                                    }


                                                }

                                            }
                                            else
                                            {
                                                throw new Exception("EntityNumber is null Or Document has No file");
                                            }
                                        }
                                        else
                                        {
                                            //throw new Exception("Customer Has No Access To send Document");
                                            queueservice.Complete();
                                            //var Failmsg = "There is no Customer tenant to send this document to";
                                            //if (IsNewLog)
                                            //{
                                            //    LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
                                            //    LogPM.Tenant = tenant;
                                            //    apiLogsService = new APILogsService(webFreightContext, tenant);
                                            //    LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                            //    LogPM.QueueType = "Document";
                                            //    apiLogsService.Create(LogPM);
                                            //    IsNewLog = false;
                                            //}
                                            //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, Failmsg, Failmsg);
                                        }

                                    }
                                    else
                                    {
                                        LogPM.Subject = "Delete Document From Importer Tenant ";
                                        LogPM.Tenant = tenant;
                                        LogPM.Refrence = DocumentFilingPM.Code;
                                        if (IsNewLog)
                                        {
                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                            LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                            LogPM.QueueType = "Document";
                                            apiLogsService.Create(LogPM);
                                            IsNewLog = false;
                                        }
                                        var Donemsg = "Start Deleting Document From Importer Tenant  Done Successfully /" + DateTime.Now;
                                        using (var client = new HttpClient())
                                        {
                                            string ImporterShipmentDocumentsURI = URI + "ImporterShipmentDocumentsCancle";
                                            client.DefaultRequestHeaders.Add("Token", Token);
                                            client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                            //var result = await client.DeleteAsync(String.Format("{0}/{1}", ImporterShipmentDocumentsURI, DocumentFilingPM.CustomerDocumentId + "," + DocumentFilingPM.CustomerTenantNumber));
                                            var NewDocumentFilingAM = new DocumentsFilingAM()
                                            {
                                                ImporterTenant = DocumentFilingPM.CustomerTenantNumber != null ? (int)DocumentFilingPM.CustomerTenantNumber : 0,
                                                CustomerDocumentId = DocumentFilingPM.CustomerDocumentId
                                            };

                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingAM), null, null, "");

                                            var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
                                            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                            var result = await client.PutAsync(ImporterShipmentDocumentsURI, content);
                                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                                if (DocumentFilingPM == null)
                                                {
                                                    DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, tenant);
                                                }
                                                try
                                                {
                                                    DocumentFilingPM.CustomerDocumentId = null;
                                                    DocumentFilingPM.CustomerTenantNumber = null;
                                                    DocumentFilingPM.DontAddToQueue = true;
                                                    DocumentFilingPM.NoAddToTasksQueue = true;
                                                    ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);
                                                    DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocumentFilingPM.Tenant);
                                                    documentsFilingService.Update(DocumentFilingPM, null, User.Id);
                                                    queueservice.Complete();
                                                    var ResponseData = result.Content.ReadAsStringAsync().Result;
                                                    Donemsg = "Deleting Document From Importer Tenant Done Successfully /" + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
                                                }
                                                catch (Exception ex)
                                                {
                                                    string errorMessage = ex.Message + Environment.NewLine;

                                                    if (ex.InnerException != null)
                                                    {

                                                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                                    }

                                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                                    throw new Exception(ex.Message, new Exception(errorMessage));
                                                }

                                            }
                                            else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                            {
                                                APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Content.ReadAsStringAsync().Result);
                                                if (EXC != null)
                                                {
                                                    var Failmsg = EXC.ErrorType + " Fail To Send Shipment Updates To Importer Tenant - Customer Side" + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                    throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                }
                                            }
                                        }

                                    }
                                    LogDoneItemInMemory();

                                }
                                catch (Exception ex)
                                {
                                    #region Exception handling
                                    string Status = "F";
                                    //if (ex.Message == "EntityNumber is null Or Document has No file" || ex.Message == "Customer Has No Access To send Document")
                                    //{
                                    //    Status = "D";
                                    //}
                                    //ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);

                                    var Failmsg = ex.Message + " " + DateTime.Now;
                                    string errorMessage = ex.Message + Environment.NewLine;

                                    if (ex.InnerException != null)
                                    {

                                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                    }

                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                    /// APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));


                                    if (response.RetryNumber <= 1)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                    }

                                    if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                    }
                                    if (response.RetryNumber >= 3)
                                    {
                                        if (ex.Message == "EntityNumber is null Or Document has No file")
                                        {
                                            Status = "D"; 
                                            queueservice.Complete();
                                        }
                                        else
                                        { 
                                            queueservice.CompleteAsFailed();
                                        }
                                        if (ex.Message != "Customer Has No Access To send Document")
                                        {
                                            if (IsNewLog)
                                            {
                                                LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
                                                LogPM.Tenant = tenant;
                                                apiLogsService = new APILogsService(webFreightContext, tenant);
                                                LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                LogPM.QueueType = "Document";
                                                apiLogsService.Create(LogPM);
                                                IsNewLog = false;
                                            }
                                           
                                            //else if ("The physical file for this Document may be Damaged or not exists. ")
                                            //{

                                            //}
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                                        }

                                    }




                                    #endregion
                                }
                            }
                            else
                            {
                                Thread.Sleep(10000);
                            }

                        }
                        catch (Exception ex)
                        {
                            if (LogPM != null)
                            {
                                //LogPM.CustomerId = CustomerId;
                                //LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                try
                                {
                                    string errorMessage = ex.Message + Environment.NewLine;

                                    if (ex.InnerException != null)
                                    {

                                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                    }

                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                    var msg = ex.Message + DateTime.Now;
                                    LogPM.DiagnosticLog = msg;
                                    LogPM.ExceptionsMessage = errorMessage;
                                    LogPM.QueueType = "Document";
                                    apiLogsService.Create(LogPM);
                                }
                                catch (Exception e)
                                {
                                    ExceptionHandler.HandleException(e, DateTime.Now, 0, null, "importer Documents worker role start", null, null);
                                }

                            }
                            ConnectClient();
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer Documents worker role start", null, null);
                            Thread.Sleep(10000);
                        }

                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
            }
            catch (Exception ex)
            {
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
                Thread.Sleep(10000);
            }
        }

        
        private async void StartUploading(BlobFileInfo fileInfo, byte[] FileData, string Token, int tenant, DocumentsFilingAM NewDocumentFilingAM)
        {
            if (FileData.Length > 0)
            {
                AddDocumentFiling = false;
                blocksNumber = Math.Ceiling(Convert.ToDouble(FileData.Length) / buffersize);
                blockIdsArray = new List<string>();
                encodedFileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('/', 'A').ToLower();
                SendBlockToServer(fileInfo, FileData, true, Token, tenant, NewDocumentFilingAM);
            }
        }
        private async void SendBlockToServer(BlobFileInfo fileInfo, byte[] FileData, bool isFirstTry, string Token, int tenant, DocumentsFilingAM NewDocumentFilingAM)
        {
            DocId = "";
            //FileInformation FInfo = new FileInformation()
            //{
            //    FileName = fileInfo.FileName,
            //    FileSize = FileData.Length,
            //    Tenant = tenant,

            //};
            Response response = null;
            while (sentBytes < FileData.Length)
            {

                counter++;
                int byteDifference2 = FileData.Length - Convert.ToInt32(sentBytes);
                if (byteDifference2 > buffersize)
                {
                    currentData = new byte[buffersize];
                    Buffer.BlockCopy(FileData, position, currentData, 0, buffersize);
                }
                else
                {
                    currentData = new byte[byteDifference2];
                    Buffer.BlockCopy(FileData, position, currentData, 0, byteDifference2);
                }

                blockId2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                blockIdsArray.Add(blockId2);
                sentBytes += currentData.Length;
                position = Convert.ToInt32(sentBytes);
                value = (Convert.ToDouble(!isFirstTry ? sentBytes : 0) / Convert.ToDouble(FileData.Length)) * 100;
                isFirstTry = !isFirstTry;
                NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                NewDocumentFilingAM.FileInfo.buffer = currentData;
                NewDocumentFilingAM.FileInfo.SentSize = sentBytes;
                NewDocumentFilingAM.FileInfo.BufferNumber = counter;
                NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                NewDocumentFilingAM.FileInfo.BlockIdsList = blockIdsArray.ToArray();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Token", Token);
                    var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(URI + "ImporterShipmentDocuments", content);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string temp1 = result.Content.ReadAsStringAsync().Result;
                        NewDocumentFilingAM.DocumentId = JsonConvert.DeserializeObject<string>(temp1);
                        NewDocumentFilingAM.FileInfo.DocumentId = NewDocumentFilingAM.DocumentId;
                    }
                }
            }
            AddDocumentFiling = true;
            sentBytes = 0;
            position = 0;
            counter = -1;
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
