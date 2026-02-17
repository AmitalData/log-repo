using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
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
    public class ForwarderShipmentDocumentsWorkerRole : WorkerEntryPoint
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
        public ForwarderShipmentDocumentsWorkerRole(string tenant)
        {
            Tenant = int.Parse(tenant);
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().ForwarderTenantsURL.TrimEnd('/') + "/api/";
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ForwarderShipmentDocuments";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();

            return base.OnStart();
        }
        string Token;
        Contact User;
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
                        queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", Tenant);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;
                        //int importerTenant = 0;

                        if (response != null && response.MessageId != null)
                        {
                            string ShipmentId = response.MessageValues["ShipmentId"].ToString();
                            string DocumentFilingId = response.MessageValues["DocumentFilingId"].ToString();
                            int.TryParse(response.MessageValues["Tenant"], out tenant);
                            string CorrelationId = response.MessageValues["CorrelationId"].ToString();
                            ContactRepository contactRepository = new ContactRepository(tenant);
                            User = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
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

                            try
                            {
                                if (!string.IsNullOrEmpty(ShipmentId))
                                {
                                    //ShipmentPM ForwarderShipment = null;
                                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                    ShipmentPM ImporterShipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                                    HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
                                    HybridPartner Partner = hybridPartnerRepository.GetSingleHybridPartner(ImporterShipment.ForwarderPartnerId);



                                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);

                                    DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, tenant);

                                    if (DocumentFilingPM != null && DocumentFilingPM != null)
                                    {
                                        if (DocumentFilingPM.HasFile)
                                        {
                                            var GetURI = URI + "ForwarderShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.Id + "&tenant=" + Partner.PartnerTenant;// + "&importertenant=" + importerTenant;
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
                                            //string DocumentTypeId = "";
                                            LogPM.Refrence = DocumentFilingPM.Code;
                                            byte[] datainByte = null;
                                            ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);
                                            using (var client = new HttpClient())
                                            {
                                                client.DefaultRequestHeaders.Add("Token", Token);
                                                client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                                string ImporterShipmentDocumentsURI = URI + "ForwarderShipmentDocuments";
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
                                                        //HasExternalContainer = true,
                                                        //ExternalContainerName = "tenant" + tenant

                                                    };
                                                    datainByte = storageservice.Read(fileInfo);
                                                   
                                                }
                                                ObjectTableRepository repo = new ObjectTableRepository(tenant);
                                                var Otable = repo.GetSingleObjectTable(DocumentFilingPM.ObjectTableId,tenant,true);
                                                var tableName = "";
                                                if(Otable != null)
                                                {
                                                    tableName = Otable.Name;
                                                }
                                                if (IsNew)
                                                {
                                                    #region DocumentsFilingPMProperties
                                                    DocumentsFilingPM NewDocumentFilingPM = new DocumentsFilingPM()
                                                    {
                                                        Tenant = Partner.PartnerTenant,
                                                        ForwarderDocumentId = DocumentFilingPM.ForwarderDocumentId,
                                                        EntityId = ImporterShipment.ForwarderShipmentNumber,
                                                        DontAddToQueue = true,
                                                        DocumentTypeCode = DocumentFilingPM.DocumentTypeCode,
                                                        DirectionCode = DocumentFilingPM.DirectionCode,
                                                        CreatedByUserId = User.Id,
                                                        UpdatedByUserId = User.Id,
                                                        OwnerId = User.Id,
                                                        //FileData = datainByte,
                                                        DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>(),
                                                        HasFile = DocumentFilingPM.HasFile,
                                                        //DocumentId = DocumentFilingPM.DocumentId,
                                                        ObjectTableId = DocumentFilingPM.ObjectTableId,
                                                        ObjectTableName = tableName,
                                                        ChildEntityId = DocumentFilingPM.ChildEntityId,
                                                        ChildEntityReference = DocumentFilingPM.ChildEntityReference,
                                                        ChildObjectTableId = DocumentFilingPM.ChildObjectTableId,
                                                        Notes = DocumentFilingPM.Notes,
                                                        Description = DocumentFilingPM.Description,
                                                        HasCopies = DocumentFilingPM.HasCopies,
                                                        FolderId = DocumentFilingPM.FolderId,
                                                        IsDeleted = DocumentFilingPM.IsDeleted,
                                                        //DeletedByUserId = DocumentFilingPM.DeletedByUserId,
                                                        //DeleteDateTime = DocumentFilingPM.DeleteDateTime,
                                                        IsSharedWithCustomer = DocumentFilingPM.IsSharedWithCustomer,
                                                        IsSharedWithForwarder = DocumentFilingPM.IsSharedWithForwarder,
                                                        CustomerDocumentId = DocumentFilingPM.Id,
                                                        SearchFields = DocumentFilingPM.SearchFields,
                                                        Received = DocumentFilingPM.Received,
                                                        ReceivedByUserId = DocumentFilingPM.ReceivedByUserId,
                                                        ReceivedDate = DocumentFilingPM.ReceivedDate,
                                                        StatusCode = DocumentFilingPM.StatusCode,
                                                        EntityReference = DocumentFilingPM.EntityReference,
                                                        ExternalEntityName = DocumentFilingPM.ExternalEntityName,
                                                        ExternalEntityReference = DocumentFilingPM.ExternalEntityReference,
                                                        IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
                                                        SignersList = DocumentFilingPM.SignersList,
                                                        FileExtension = DocumentFilingPM.FileExtension,
                                                        FileName = DocumentFilingPM.FileName,
                                                        CustomerTenantNumber = DocumentFilingPM.Tenant

                                                    };
                                                    if (DocumentFilingPM.DocumentsFilingMetaDataValues != null && DocumentFilingPM.DocumentsFilingMetaDataValues.Count > 0)
                                                    {

                                                        DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                                                        foreach (var item in DocumentFilingPM.DocumentsFilingMetaDataValues)
                                                        {
                                                            var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.DocumentsMetaDataTypeId, item.Tenant);
                                                            if (DocFilingType != null)
                                                            {
                                                                NewDocumentFilingPM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValuePM() { DocumentsMetaDataTypeId = DocFilingType.Code, MetaDataValue = item.MetaDataValue, ChangeSetOp = item.ChangeSetOp });
                                                            }
                                                            else
                                                            {
                                                                throw new Exception("Missing Data Error", new Exception("DocumentsMetaDataType does not exist in Importer tenant"));
                                                            }

                                                        }
                                                    }
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
                                                        NewDocumentFilingPM.BlockIdsList = blockIdsArray.ToArray();
                                                        NewDocumentFilingPM.buffer = currentData;
                                                        NewDocumentFilingPM.SentSize = sentBytes;
                                                        NewDocumentFilingPM.FileSize = datainByte.Length;
                                                        NewDocumentFilingPM.BufferNumber = counter;
                                                        NewDocumentFilingPM.FullFileName = DocumentFilingPM.FileName + "." + fileInfo.Extension;
                                                        var serializedObj = JsonConvert.SerializeObject(NewDocumentFilingPM);
                                                        var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                                        var resultData = await client.PostAsync(ImporterShipmentDocumentsURI, contentData);
                                                        if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                                                        {
                                                            string temp1 = resultData.Content.ReadAsStringAsync().Result;
                                                            NewDocumentFilingPM.DocumentId = JsonConvert.DeserializeObject<string>(temp1);
                                                            NewDocumentFilingPM.DocumentId = NewDocumentFilingPM.DocumentId;
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
                                                    NewDocumentFilingPM.buffer = null;
                                                    #endregion
                                                    LogPM.Tenant = tenant;
                                                    LogPM.Subject = "Send New Document To Forwarder By ForwarderShipmentDocuments Controller";
                                                    if (IsNewLog)
                                                    {
                                                        apiLogsService = new APILogsService(webFreightContext, tenant);
                                                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string,string>)response.MessageValues);
                                                        LogPM.QueueType = "SharedDocument";
                                                        apiLogsService.Create(LogPM);
                                                    }
                                                    var msg = "Start Sending New Document To Forwarder " + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingPM), null, null, "");

                                                    var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingPM);
                                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                    var result = await client.PostAsync(ImporterShipmentDocumentsURI, content);
                                                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                                    {
                                                        var Donemsg = "New Document Sent To Forwarder Successfully " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, null, null, "");
                                                        queueservice.Complete();
                                                    }
                                                    else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                    {
                                                        var temp1 = result.Content.ReadAsStringAsync().Result;
                                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                        if (EXC != null)
                                                        {
                                                            var Failmsg = EXC.ErrorType + " Fail To Send New Document To Forwarder " + DateTime.Now;
                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    Exception EXC = JsonConvert.DeserializeObject<Exception>(result.Content.ReadAsStringAsync().Result);
                                                    //    var Failmsg = EXC.Message;
                                                    //    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), EXC.Message.Substring(0, 249));
                                                    //    throw EXC;
                                                    //}


                                                }
                                                else
                                                {
                                                    //DocumentsFilingPM ImporterDocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingPM.CustomerDocumentId, importerTenant);

                                                    #region DocumentFilingPMProperties
                                                    DocumentsFilingPM NewDocumentFilingPM = new DocumentsFilingPM()
                                                    {
                                                        Id = DocumentFilingPM.ForwarderDocumentId,
                                                        Tenant = Partner.PartnerTenant,
                                                        ForwarderDocumentId = DocumentFilingPM.ForwarderDocumentId,
                                                        EntityId = ImporterShipment.ForwarderShipmentNumber,
                                                        DontAddToQueue = true,
                                                        DocumentTypeCode = DocumentFilingPM.DocumentTypeCode,
                                                        DirectionCode = DocumentFilingPM.DirectionCode,
                                                        CreatedByUserId = User.Id,
                                                        UpdatedByUserId = User.Id,
                                                        OwnerId = User.Id,
                                                        //FileData = datainByte,
                                                        DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>(),
                                                        HasFile = DocumentFilingPM.HasFile,
                                                        //DocumentId = DocumentFilingPM.DocumentId,
                                                        ObjectTableId = DocumentFilingPM.ObjectTableId,
                                                        ObjectTableName = tableName,
                                                        ChildEntityId = DocumentFilingPM.ChildEntityId,
                                                        ChildEntityReference = DocumentFilingPM.ChildEntityReference,
                                                        ChildObjectTableId = DocumentFilingPM.ChildObjectTableId,
                                                        Notes = DocumentFilingPM.Notes,
                                                        Description = DocumentFilingPM.Description,
                                                        HasCopies = DocumentFilingPM.HasCopies,
                                                        FolderId = DocumentFilingPM.FolderId,
                                                        IsDeleted = DocumentFilingPM.IsDeleted,
                                                        //DeletedByUserId = DocumentFilingPM.DeletedByUserId,
                                                        //DeleteDateTime = DocumentFilingPM.DeleteDateTime,
                                                        IsSharedWithCustomer = DocumentFilingPM.IsSharedWithCustomer,
                                                        IsSharedWithForwarder = DocumentFilingPM.IsSharedWithForwarder,
                                                        CustomerDocumentId = DocumentFilingPM.Id,
                                                        SearchFields = DocumentFilingPM.SearchFields,
                                                        Received = DocumentFilingPM.Received,
                                                        ReceivedByUserId = DocumentFilingPM.ReceivedByUserId,
                                                        ReceivedDate = DocumentFilingPM.ReceivedDate,
                                                        StatusCode = DocumentFilingPM.StatusCode,
                                                        EntityReference = DocumentFilingPM.EntityReference,
                                                        ExternalEntityName = DocumentFilingPM.ExternalEntityName,
                                                        ExternalEntityReference = DocumentFilingPM.ExternalEntityReference,
                                                        IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
                                                        SignersList = DocumentFilingPM.SignersList,
                                                        FileExtension = DocumentFilingPM.FileExtension,
                                                        FileName = DocumentFilingPM.FileName,
                                                        CustomerTenantNumber = DocumentFilingPM.Tenant

                                                    };
                                                    if (DocumentFilingPM.DocumentsFilingMetaDataValues != null && DocumentFilingPM.DocumentsFilingMetaDataValues.Count > 0)
                                                    {

                                                        DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                                                        foreach (var item in DocumentFilingPM.DocumentsFilingMetaDataValues)
                                                        {
                                                            var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(item.DocumentsMetaDataTypeId, item.Tenant);
                                                            if (DocFilingType != null)
                                                            {
                                                                NewDocumentFilingPM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValuePM() { DocumentsMetaDataTypeId = DocFilingType.Code, MetaDataValue = item.MetaDataValue, ChangeSetOp = item.ChangeSetOp });
                                                            }
                                                            else
                                                            {
                                                                throw new Exception("Missing Data Error", new Exception("DocumentsMetaDataType does not exist in Importer tenant"));
                                                            }

                                                        }
                                                    }
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
                                                        NewDocumentFilingPM.BlockIdsList = blockIdsArray.ToArray();
                                                        NewDocumentFilingPM.buffer = currentData;
                                                        NewDocumentFilingPM.SentSize = sentBytes;
                                                        NewDocumentFilingPM.FileSize = datainByte.Length;
                                                        NewDocumentFilingPM.BufferNumber = counter;
                                                        NewDocumentFilingPM.FullFileName = DocumentFilingPM.FileName + "." + fileInfo.Extension;
                                                        var serializedObj = JsonConvert.SerializeObject(NewDocumentFilingPM);
                                                        var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                                        var resultData = await client.PostAsync(ImporterShipmentDocumentsURI, contentData);
                                                        if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                                                        {
                                                            string temp1 = resultData.Content.ReadAsStringAsync().Result;
                                                            NewDocumentFilingPM.DocumentId = JsonConvert.DeserializeObject<string>(temp1);
                                                            NewDocumentFilingPM.DocumentId = NewDocumentFilingPM.DocumentId;
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
                                                    NewDocumentFilingPM.buffer = null;
                                                    #endregion
                                                    LogPM.Subject = "Send Document Updates To Forwarder ";
                                                    if (IsNewLog)
                                                    {
                                                        apiLogsService = new APILogsService(webFreightContext, tenant);
                                                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string,string>)response.MessageValues);
                                                        LogPM.QueueType = "SharedDocument"; 
                                                        apiLogsService.Create(LogPM);
                                                    }
                                                    var msg = "Start Sending Document Updates To Forwarder " + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingPM), null, null, "");

                                                    var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingPM);
                                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                    var result = await client.PutAsync(ImporterShipmentDocumentsURI, content);
                                                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                                    {
                                                        var temp1 = result.Content.ReadAsStringAsync().Result;
                                                        var Donemsg = "Document Updates Sent To Importer Successfully " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp1, null, "");
                                                        queueservice.Complete();
                                                    }
                                                    else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                    {
                                                        var temp1 = result.Content.ReadAsStringAsync().Result;
                                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                                        if (EXC != null)
                                                        {
                                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                        }
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
                                                    //else
                                                    //{
                                                    //    try
                                                    //    {
                                                    //        Exception EXC = JsonConvert.DeserializeObject<Exception>(result.Content.ReadAsStringAsync().Result);
                                                    //        var Failmsg = EXC.Message;
                                                    //        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), EXC.Message.Substring(0, 249));
                                                    //        throw EXC;
                                                    //    }
                                                    //    catch (Exception ex)
                                                    //    {

                                                    //        throw ex;
                                                    //    }

                                                    //}

                                                }
                                            }
                                        }
                                        else
                                        {
                                            queueservice.Complete();
                                        }
                                    }
                                    else
                                    {
                                        queueservice.CompleteAsFailed();
                                    }
                                }
                                else
                                {
                                    queueservice.CompleteAsFailed();
                                }

                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);

                                var Failmsg = ex.Message + " " + DateTime.Now;
                                string errorMessage = ex.Message + Environment.NewLine;

                                if (ex.InnerException != null)
                                {

                                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                }

                                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));


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
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                                }



                            }
                            LogDoneItemInMemory();
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
                queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }
    }
}
