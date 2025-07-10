using AmitalCloud.Infrastructure.APITools.DataMapping;
using AmitalCloud.Infrastructure.APITools.Validators;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data.Azure;
using AmitalCloud.Infrastructure.Data.Counters;
using AmitalCloud.Infrastructure.Data.DataMapping;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityKeys;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Enums;
using AmitalCloud.Infrastructure.Domain.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmitalCloud.Infrastructure.Model.Interfaces;

namespace AmitalCloud.Infrastructure.APITools.Services
{
    public class DocumentsFilingService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentsFiling Poco { get; set; }
        private IAmitalCloudContext currentContext;
        protected DocumentsFilingRepository entityRepository;
        private DocumentsFilingMetaDataValueRepository documentsFilingMetaDataValueRepository;
        private IRepository<DocumentType> documentTypeRepository;
        protected DocumentRepository documentRepository;
        protected ObjectTableRepository objectTableRepository;
        private bool _OnCreateUnifreightFillingMode;
        HybridPartnerPM CurrentHybridPartner;
        private List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValueChangeSet;
        private bool _Connect2EntityId;
        protected IUnitOfWork unitOfWork;

        public DocumentsFilingService(IAmitalCloudContext currentContext, int tenant)
        {
            this.tenant = tenant;
            this.currentContext = currentContext;
        }

        private void SetHybridPartner(int myTenant)
        {
            CurrentHybridPartner = GetSinglePMByPartnerTenant(myTenant);
        }
        protected UniFileVerM MyUniFileVerM { get; set; }
        protected string MetaDataVersionValue { get; set; }

        private HybridPartnerPM GetSinglePMByPartnerTenant(int PartnerTenant)
        {
            return new Repository<HybridPartner>(currentContext)
                .GetAll(tenant, true).Select(a => new HybridPartnerPM()
                {
                    Id = a.Id,
                    PartnerTenant = a.PartnerTenant,
                    Name = a.Name,
                    LocalName = a.LocalName,
                    LogoId = a.LogoId,
                    SearchFields = a.SearchFields,
                    SmallLogoId = a.SmallLogoId,
                    IsMislakaActivated = a.IsMislakaActivated,
                    IsExternalPartner = a.IsExternalPartner,
                    ReceiveAllStatuses = a.ReceiveAllStatuses,
                    AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                    InActive = a.InActive
                }).FirstOrDefault();
        }

        public void Update(DocumentsFilingPM entityPM, byte[] fileData = null, string loggedUserId = null, bool FromService = false)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork<IAmitalCloudContext>(currentContext))
            {
                this.unitOfWork = unitOfWork;
                this.entityRepository = new DocumentsFilingRepository(unitOfWork);
                this.documentsFilingMetaDataValueRepository = new DocumentsFilingMetaDataValueRepository(unitOfWork);
                //this.customsDocumentMetaDataValueRepository = new CustomsDocumentMetaDataValueRepository(customContext);
                this.documentTypeRepository = new Repository<DocumentType>(unitOfWork);
                this.documentRepository = new DocumentRepository(unitOfWork);
                //shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                objectTableRepository = new ObjectTableRepository(unitOfWork);
                SetHybridPartner(tenant);


                this.isNewEntity = false;
                //entityPM = entityPM;
                this.Poco = entityRepository.GetSingleDocumentsFiling(entityPM.Id, entityPM.Tenant);
                if (string.IsNullOrWhiteSpace(this.Poco.EntityId) && !string.IsNullOrWhiteSpace(entityPM.EntityId))
                {
                    this._Connect2EntityId = true;
                }
                if (!string.IsNullOrEmpty(Poco.CustomerDocumentId))
                {
                    entityPM.CustomerDocumentId = Poco.CustomerDocumentId;
                }
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                bool addBackupQueue = false;
                if (entityPM.DocumentId != Poco.DocumentId && !string.IsNullOrEmpty(entityPM.DocumentId))
                    addBackupQueue = true;

                DocumentsFilingValidating.Validate(entityPM);
                if (!FromService)
                {
                    if (entityPM.DirectionCode == "I")
                    {
                        entityPM.DocumentId = BuildDocument(fileData, false, entityPM);
                        addBackupQueue = (fileData != null);
                    }
                }
                TenantPM tenantPM = new TenantQueryService(tenant).GetSingle(entityPM.Tenant, false, true);
                UpdateDocumentsFilingMetaDataValuesCollection(entityPM);
                var OldIsSigned = Poco.IsDigitallySigned;
                var WasRequested = Poco.IsRequested;
                DocumentsFilingMapping.MapEntity(entityPM, Poco, isNewEntity);
                if (string.IsNullOrEmpty(Poco.SecurityId))
                {
                    Random rnd = new Random();
                    // Poco.SecurityId = entityPM.Id + RandomString(10);
                    string com_id = entityPM.Id;        // Length = 30
                    string com_md5 = CreateMD5(com_id); // Length = 32 
                    string com_short = entityPM.Id.Substring(0, 8);
                    Poco.SecurityId = com_short + com_md5; // Length = 40
                }
                //TODO: Check if this is needed
                //if (tenantPM.IsDocumentsArchive == true)
                //{
                //    HandleComputedFields(entityPM, OldIsSigned, WasRequested);
                //}
                unitOfWork.Save();
                unitOfWork.Commit();
            }
        }
        protected virtual void HandleComputedFields(DocumentsFilingPM entityPM, bool OldIsSigned, bool WasRequested)
        {
        }

        //        DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(entityPM.Tenant);



        protected bool CheckIfSignRequired(string EntityDirection, string DocTypeID, int myTenant)
        {
            DocumentType documentType = documentTypeRepository.GetMulti(a => a.Id == DocTypeID && a.Tenant == myTenant).FirstOrDefault();
            if (!string.IsNullOrEmpty(EntityDirection))
            {
                if ((EntityDirection == "A" && documentType.IsAirDigitalSignRequired) || (EntityDirection == "I" && documentType.IsInlandDigitalSignRequired) || (EntityDirection == "O" && documentType.IsOceanDigitalSignRequired))
                {
                    return true;
                }
            }
            return false;
        }
        //    public void Create(DocumentsFilingPM entityPM, byte[] fileData = null, string loggedUserId = null, bool FromService = false, string documentId = null)
        //    {
        //        this.isNewEntity = true;
        //        entityPM = entityPM;

        //        if (!entityPM.IsHybrid || entityPM.IsAttachment)
        //        {
        //            // entityPM.Id = IdCounter.GetNumber("Document", tenant).ToString();


        //            var guid = Guid.NewGuid();
        //            var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
        //            base64string = base64string.Substring(0, 22);
        //            base64string = base64string.Replace("/", "_");
        //            base64string = base64string.Replace("+", "-");
        //            entityPM.Id = base64string;
        //            bool inOracleCreateNewTransaction =
        //             (
        //             this.MyDocumentsFilingParams != null &&
        //             (this.MyDocumentsFilingParams.MainInterfaceCode == "3053" || this.MyDocumentsFilingParams.MainInterfaceCode == "8302") &&
        //             this.MyDocumentsFilingParams.IsCourier
        //             //CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant).CompanyType == "B"//Courier

        //             );

        //            entityPM.Code = CodeCounter.GetNumber("DocumentsFiling", tenant, inOracleCreateNewTransaction).ToString();
        //        }
        //        if (!entityPM.IsHybrid)
        //        {
        //            //entityPM.Code = CodeCounter.GetNumber("DocumentsFiling", entityPM.Tenant).ToString();
        //            string loggedUserEmail = "";
        //            UserRepository userRepository = new UserRepository(entityPM.Tenant);
        //            User loggedUser = null;
        //            if (entityPM.IsFromUnifreightPodMobile)
        //            {
        //                loggedUserEmail = "system@tenant" + entityPM.Tenant + ".com";
        //                loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, entityPM.Tenant, true);
        //            }
        //            else
        //            {
        //                if (string.IsNullOrEmpty(loggedUserId))
        //                {
        //                    loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
        //                    loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, entityPM.Tenant, true);
        //                }
        //                else
        //                {
        //                    loggedUser = userRepository.GetSingleUser(loggedUserId, entityPM.Tenant, true);
        //                }
        //            }
        //            if (loggedUser != null)
        //            {
        //                entityPM.SearchFields = entityPM.Code + "," + entityPM.DirectionCode + "," + loggedUser.Contact.EnglishName + "," + loggedUser.Contact.LocalName;
        //                if (string.IsNullOrEmpty(entityPM.CreatedByUserId))
        //                {
        //                    entityPM.CreatedByUserId = loggedUser.Id;
        //                }
        //                entityPM.OwnerId = entityPM.CreatedByUserId;
        //                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
        //                entityPM.UpdatedByUserId = entityPM.CreatedByUserId;
        //                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
        //            }

        //        }

        //        entityPM.Id = entityPM.Id.PadRight(30, '0');

        //        //Added by Maheera
        //        //entityPM.SecurityId = entityPM.Id + System.Web.Security.Membership.GeneratePassword(10, 0);
        //        Random rnd = new Random();
        //        //entityPM.SecurityId = entityPM.Id + RandomString(10);
        //        string com_id = entityPM.Id;        // Length = 30
        //        string com_md5 = CreateMD5(com_id); // Length = 32 
        //        string com_short = entityPM.Id.Substring(0, 8);
        //        entityPM.SecurityId = com_short + com_md5; // Length = 40

        //        entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        //        entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

        //        this.Poco = new DocumentsFiling();
        //        this.Poco.Id = entityPM.Id;

        //        DocumentsFilingValidating.Validate(entityPM);
        //        DocumentsFilingTracing.Trace(entityPM, Poco, isNewEntity);
        //        _OnCreateUnifreightFillingMode = BlobFileInfoExt.IsUnifreightFillingModeBase(entityPM.Tenant, entityPM.Folder);

        //        if ((!FromService || _OnCreateUnifreightFillingMode) && documentId == null)
        //        {
        //            if (
        //                (entityPM.DirectionCode == "I")
        //                //|| 
        //                //(entityPM.DirectionCode == "E")
        //                )

        //            {
        //                entityPM.DocumentId = BuildDocument(fileData, true, entityPM.Id);

        //            }
        //        }
        //        else if (string.IsNullOrEmpty(entityPM.DocumentId))
        //        {
        //            entityPM.DocumentId = documentId;

        //        }

        //        tenantQuery = new TenantQuery(entityPM.Tenant);
        //        TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);

        //        if (string.IsNullOrEmpty(entityPM.EntityId) && tenantPM.IsDocumentsArchive == false)
        //        {
        //            DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(entityPM.Tenant);
        //            var LBC = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBC", entityPM.Tenant, true);
        //            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(entityPM.Tenant);
        //            if (entityPM.CustomerTenantNumber != null && LBC != null)
        //            {
        //                var TenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(entityPM.Tenant, (int)entityPM.CustomerTenantNumber);
        //                if (TenantAccess != null)
        //                {
        //                    if (entityPM.DocumentsFilingMetaDataValues == null)
        //                    {
        //                        entityPM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>();
        //                    }
        //                    CustomerTenantAccessCardQuery TenantAccessCardQuery = new CustomerTenantAccessCardQuery(entityPM.Tenant);
        //                    var Cards = TenantAccessCardQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(TenantAccess.Id, entityPM.Tenant).Where(a => a.StatusTypeCode != "IA").ToList();
        //                    foreach (var item in Cards)
        //                    {
        //                        var CusCode = item.CustomerCode;
        //                        if (string.IsNullOrEmpty(CusCode))
        //                        {
        //                            CardRepository repo = new CardRepository(entityPM.Tenant);
        //                            var mycustomer = repo.GetSingleCard(item.CustomerId, entityPM.Tenant);
        //                            if (mycustomer != null)
        //                            {
        //                                CusCode = mycustomer.Code;
        //                            }
        //                        }

        //                        DocumentsFilingMetaDataValuePM value1 = new DocumentsFilingMetaDataValuePM();
        //                        value1.ChangeSetOp = ChangeSetOperation.Insert;
        //                        value1.DocumentsFilingId = entityPM.Id;
        //                        value1.MetaDataValue = CusCode;
        //                        value1.DocumentsMetaDataTypeId = LBC.Id;
        //                        value1.Tenant = entityPM.Tenant;
        //                        entityPM.DocumentsFilingMetaDataValues.Add(value1);
        //                    }
        //                }

        //            }

        //        }

        //        foreach (DocumentsFilingMetaDataValuePM itemPM in entityPM.DocumentsFilingMetaDataValues)
        //        {
        //            this.CreateDocumentsFilingMetaDataValue(itemPM);
        //        }

        //        //if (tenantPM.IsHybrid)
        //        //{
        //        //    entityPM.IsSharedWithCustomer = true;
        //        //} 
        //        bool HavingDREL = false;




        //        var OldIsSigned = Poco.IsDigitallySigned;


        //        DocumentsFilingMapping.MapEntity(entityPM, Poco, isNewEntity);


        //        if (tenantPM.IsDocumentsArchive == true)
        //        {
        //            if (entityPM.DirectionCode == "I")
        //            {
        //                var OTName = ObjectTableRepository.GetSingleObjectTable(entityPM.ObjectTableId, tenant, false);
        //                if (OTName != null && OTName.Name == "Shipment" && !string.IsNullOrEmpty(this.Poco.EntityId))
        //                {

        //                    var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(entityPM.EntityId, entityPM.Tenant);
        //                    ShipmentCompField.DocumentsSearchFields += "," + this.Poco.SearchFields;
        //                    //ShipmentCompField.LastDocumentDateTime = DateTime.Now;
        //                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
        //                    var EntityDirection = documentsFilingQuery.GetDirectionForEntity(Poco.EntityId, entityPM.ObjectTableId, tenant);
        //                    if (CheckIfSignRequired(EntityDirection, entityPM.DocumentTypeId, entityPM.Tenant) && !entityPM.IsDigitallySigned && entityPM.HasFile && (!string.IsNullOrEmpty(entityPM.FileExtension) && entityPM.FileExtension.ToLower() == "pdf"))
        //                    {
        //                        ShipmentCompField.IsDigitalSignRequired = true;
        //                        Poco.IsDigitalSignRequired = true;
        //                    }
        //                    else if (entityPM.IsDigitallySigned)
        //                    {
        //                        ShipmentCompField.IsDigitalSignRequired = documentsFilingQuery.GetIfSignRequiredForEntity(Poco.EntityId, tenant, false);
        //                        Poco.IsDigitalSignRequired = false;
        //                    }

        //                    //if (entityPM.HasFile)
        //                    //{

        //                    //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(this.Poco.EntityId, entityPM.ObjectTableId, tenant);
        //                    //    ShipmentCompField.IsMissingDocuments = hasmissing;
        //                    //}
        //                    //else
        //                    //{
        //                    //    if (entityPM.DocumentTypeCode == "740" || entityPM.DocumentTypeCode == "706" || entityPM.DocumentTypeCode == "380")
        //                    //    {
        //                    //        ShipmentCompField.IsMissingDocuments = true; 
        //                    //    }
        //                    //}
        //                    var document = documentRepository.GetSingleDocument(tenant, entityPM.DocumentId);
        //                    if (document != null && document.HasFile == true)
        //                    {
        //                        ShipmentCompField.LastDocumentDateTime = DateTime.Now;
        //                    }
        //                    ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(Poco.EntityId, entityPM.ObjectTableId, tenant);
        //                    ShipmentCompField.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(Poco.EntityId, entityPM.ObjectTableId, tenant);
        //                    if (ShipmentCompField.MissingDocumentsCount == 0)
        //                    {
        //                        ShipmentCompField.IsMissingDocuments = false;
        //                    }
        //                    else
        //                    {
        //                        ShipmentCompField.IsMissingDocuments = true;
        //                    }
        //                    var IsRequested = documentsFilingQuery.GetIfIsRequestedForEntity(Poco.EntityId, tenant);
        //                    if (IsRequested == false && Poco.IsRequested && !Poco.IsDeleted)
        //                    {
        //                        IsRequested = true;
        //                    }
        //                    ShipmentCompField.IsRequestedDocuments = IsRequested;
        //                    ShipmentCompField.RequestedDocumentsCount = documentsFilingQuery.GetRequestedDocCountForEntity(this.Poco.EntityId, tenant);
        //                    if (Poco.IsRequested && !Poco.IsDeleted)
        //                    {
        //                        ShipmentCompField.RequestedDocumentsCount++;
        //                    }


        //                    ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
        //                    shipmentComputedFieldsHelper.UpdateShipmentComputedFields(ShipmentCompField, shipmentComputedFieldsRepository.context);

        //                    // shipmentComputedFieldsRepository.Update(ShipmentCompField);
        //                    // shipmentComputedFieldsRepository.SubmitChanges();
        //                    bool shouldBeSentToForwarder = !entityPM.DontAddToQueue && ((entityPM.IsSharedWithForwarder && !entityPM.IsSharedWithCustomer) || (entityPM.IsSharedWithCustomer && entityPM.IsDigitallySigned && OldIsSigned == false));
        //                    SendShipmentToForwarder(shouldBeSentToForwarder);
        //                }
        //                else if (OTName != null && OTName.Name == "ShipmentOrder" && !string.IsNullOrEmpty(this.Poco.EntityId))
        //                {
        //                    bool shouldBeSentToForwarder = !entityPM.DontAddToQueue && ((entityPM.IsSharedWithForwarder && !entityPM.IsSharedWithCustomer) || (entityPM.IsSharedWithCustomer && entityPM.IsDigitallySigned && OldIsSigned == false));
        //                    SendShipmentToForwarder(shouldBeSentToForwarder);
        //                }
        //            }
        //        }
        //        DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(entityPM.Tenant);
        //        DocumentsMetaDataType Dreltype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("DREL", entityPM.Tenant, true);
        //        DocumentsMetaDataType LBFtype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("LBF", entityPM.Tenant, true);
        //        if (Dreltype != null && LBFtype != null)
        //        {
        //            var DRELMetaData = entityPM.DocumentsFilingMetaDataValues.Where(a => (a.DocumentsMetaDataTypeId == Dreltype.Id || a.DocumentsMetaDataTypeId == LBFtype.Id));
        //            if (DRELMetaData != null && DRELMetaData.Count() > 0)
        //            {
        //                HavingDREL = true;
        //                //entityPM.IsSharedWithCustomer = true;
        //            }
        //        }


        //        entityRepository.Add(Poco);
        //        entityRepository.SubmitChanges();
        //        this.OpenKPIDocumentUploderQueue(entityPM);
        //        RunDocumentPopulateAutomaticDatesService(entityPM);
        //        RunAutomation(entityPM, "OnDocumentUpdate");
        //        ///move after adding (was Devart.Data.Oracle.OracleException: ORA-02291: אילוץ כלילות (AMINET_MAIN.FK_N1103284768) הופר - מפתח אב לא נמצא )
        //        if (!tenantPM.IsDocumentsArchive)
        //        {
        //            AddToTasksQueue(entityPM, isNewEntity, loggedUserId);
        //        }
        //        foreach (DocumentsFilingMetaDataValuePM itemPM in entityPM.DocumentsFilingMetaDataValues)
        //        {
        //            this.CreateCustomsDocumentsFilingMetaDataValue(itemPM);
        //        }
        //        AddImporterQueue(entityPM, tenantPM, HavingDREL);
        //        AddImporterQueueWithDelay(entityPM, tenantPM, HavingDREL);
        //        new ShipmentOrderDocumentsQueueService().Build(entityPM);

        //        if (!string.IsNullOrEmpty(entityPM.DocumentId))
        //        {
        //            Document doc = documentRepository.GetSingleDocument(entityPM.Tenant, entityPM.DocumentId);
        //            if (doc != null && doc.HasFile)
        //            {
        //                AddDocumentBackupLog();
        //            }
        //        }

        //    }


        private static string CreateMD5(string input)

        {

            // Use input string to calculate MD5 hash

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())

            {

                byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(input);

                byte[] hashBytes = md5.ComputeHash(inputBytes);

                //return Convert.ToHexString(hashBytes); // .NET 5 +

                //Convert the byte array to hexadecimal string prior to.NET 5

                StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)

                {

                    sb.Append(hashBytes[i].ToString("X2"));

                }

                return sb.ToString();

            }

        }

        protected string GetEntityDocumentsSearchFields()
        {
            var documentSearchFieldsLists = entityRepository.GetDocumentsFilingByEntityId(this.Poco.EntityId, this.Poco.ObjectTableId, this.Poco.Tenant).Where(d => d.Id != this.Poco.Id).Select(d => d.SearchFields).ToArray();
            var documentSearchFields = String.Join(",", documentSearchFieldsLists);
            documentSearchFields += ((!string.IsNullOrEmpty(documentSearchFields) ? "," : "") + this.Poco.SearchFields);
            return documentSearchFields;
        }

        private string BuildDocument(byte[] fileData, bool isnew, DocumentsFilingPM entityPM, string DocumentsFilingId = null)
        {
            DocumentType documentType = documentTypeRepository.GetSingle(new DocumentTypeKeys<string> { Id = entityPM.DocumentTypeId });
            Document document = null;
            //if (!string.IsNullOrEmpty(entityPM.FileExtension))
            //{
            //    entityPM.FileExtension = entityPM.FileExtension.ToLower();
            //}

            if (isnew)
            {
                document = new Document()
                {
                    Id = IdCounter.GetNumber("Document", entityPM.Tenant).ToString(),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                    Tenant = entityPM.Tenant,
                    Folder = "docsin",

                };

                document.Extension = entityPM.Document.Extension;

                if (entityPM.IsHybrid && !entityPM.IsAttachment && !string.IsNullOrEmpty(entityPM.Description))
                {
                    document.FileName = StringHelper.TruncateLongString(entityPM.Description, 120);
                }
                else
                {
                    document.FileName = !string.IsNullOrEmpty(entityPM.Document.FileName) ? entityPM.Document.FileName : documentType != null ? documentType.Name : "";
                }

                // document.CalculatedFileName = new DocumentTypeCalculateFileNameService(entityPM).Calculate();
                if (fileData != null)
                {
                    document.HasFile = true;
                    document.FileSize = Convert.ToInt32(fileData.Length);
                }
                else if (_OnCreateUnifreightFillingMode)//docsin => get from UniDMStore ver =1
                {

                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        UFileVer = 1 // on create

                    };
                    fileInfo.UDocumentsFilingId = DocumentsFilingId;
                    fileInfo.UCreateDate = document.CreateDate;
                    var uFileData = storageservice.Read(fileInfo);

                    document.HasFile = true;
                    document.FileSize = uFileData.Length;
                }

                documentRepository.Insert(document);
            }
            else
            {
                if (!string.IsNullOrEmpty(entityPM.DocumentId))
                {
                    document = documentRepository.GetSingleDocument(tenant, entityPM.DocumentId);
                }

                if (entityPM.IsDeleted )
                {
                    if (document != null)
                    {
                        string fileName = document.Id + "." + document.Extension;
                        string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), document.Folder);

                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = tenant,
                            FileSize = document.FileSize,

                        };
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        storageservice.Delete(fileInfo);

                        document.Extension = null;
                        document.FileSize = 0;
                        document.HasFile = false;
                        document.Folder = "";
                        document.FileName = null;
                        documentRepository.Update(document);
                    }
                }
                else //if !!!!(entityPM.IsDeleted)  = update 
                {
                    if (document == null)
                    {
                        document = new Document()
                        {
                            Id = IdCounter.GetNumber("Document", entityPM.Tenant).ToString(),
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                            Tenant = entityPM.Tenant,
                            Folder = "docsin",

                        };

                        document.Extension = entityPM.Document.Extension;

                        if (entityPM.IsHybrid && !entityPM.IsAttachment && !string.IsNullOrEmpty(entityPM.Description))
                        {
                            document.FileName = StringHelper.TruncateLongString(entityPM.Description, 120);
                        }
                        else
                        {
                            document.FileName = !string.IsNullOrEmpty(entityPM.Document.FileName) ? entityPM.Document.FileName : documentType.Name;
                        }

                        if (fileData != null)
                        {
                            document.HasFile = true;
                            document.FileSize = Convert.ToInt32(fileData.Length);
                        }
                        //else
                        //{
                        //    document.HasFile = false;
                        //    document.FileSize = entityPM.FileSize;
                        //}


                        documentRepository.Insert(document);
                    }
                    else if (fileData != null) /// !!! if (document == null) 
                    {
                        document.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        document.Extension = entityPM.Document.Extension;
                        document.FileSize = (fileData != null ? Convert.ToInt32(fileData.Length) : 0);
                        document.Tenant = Convert.ToInt32(entityPM.Tenant);
                        document.HasFile = true;
                        document.Folder = "docsin";
                        document.FileName = entityPM.Document.FileName;
                        document.IsEncrypted = true;
                        documentRepository.Update(document);
                    }
                    else if (fileData == null)
                    {
                        if (string.IsNullOrEmpty(entityPM.Document.Extension))
                        {
                            document.HasFile = false;
                            document.Extension = entityPM.Document.Extension;
                            documentRepository.Update(document);
                        }


                    }
                }



            }//if (isnew)


            documentRepository.SubmitChanges();

            if (fileData != null && document != null)
            {
                string fileextension = !string.IsNullOrEmpty(document.Extension) ? document.Extension.ToLower() : document.Extension;
                string fileName = document.Id + "." + fileextension;
                //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = fileData.Length,

                };
                if (document.Folder == "docsin" && fileInfo.IsUnifreightFillingMode(isnew))
                {
                    var fileDataMD5Hash = MD5HashUtil.GetMD5Hash(fileData);
                    if (isnew)
                    {
                        fileInfo.UFileVer = entityPM.LastVersion = 1;
                    }
                    else if (MyUniFileVerM != null) // get from DerivedClass  UnifreightDocumentsFilingService
                    {
                        if (fileDataMD5Hash != MyUniFileVerM.MD5HASH)
                        {
                            var PocoLastVersionPlus1 = this.Poco.LastVersion + 1;
                            fileInfo.UFileVer = entityPM.LastVersion = (MyUniFileVerM.VERSION + 1);
                            if (PocoLastVersionPlus1 != fileInfo.UFileVer)
                            {
                                var waitHybridDidNotAnalyzeLastVersionMessage = true;
                                if (waitHybridDidNotAnalyzeLastVersionMessage && PocoLastVersionPlus1 > fileInfo.UFileVer)
                                {
                                    bool SuppresswaitHybridDidNotAnalyzeLastVersionMessage = true;
                                    //ConfigurationManager.AppSettings["20180226.SuppresswaitHybridDidNotAnalyzeLastVersionMessage"] == "1";
                                    if (!SuppresswaitHybridDidNotAnalyzeLastVersionMessage)
                                    {
                                        throw new Exception("Unifreight Hybrid yet,did not Analyze Last Version Document Message (preventing new queue message to Create), Please Wait "); // prevent 2 queue for 1 Document 
                                    }
                                }
                            }
                        }
                        else
                        {
                            fileInfo.USuppressWriteDueSameMD5Hash = true;
                        }
                    }

                    fileInfo.UDocumentsFilingId = DocumentsFilingId;
                    fileInfo.UCreateDate = document.CreateDate;
                }//if (document.Folder == "docsin" && fileInfo.IsUnifreightFillingMode())
                storageservice.Write(fileData, fileInfo);

            }


            return document != null ? document.Id : null;
        }


        private void UpdateDocumentsFilingMetaDataValuesCollection(DocumentsFilingPM entityPM)
        {
            if (documentsFilingMetaDataValueChangeSet == null)
            {
                return;
            }


            foreach (DocumentsFilingMetaDataValuePM itemPM in documentsFilingMetaDataValueChangeSet)
            {
                switch (itemPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            this.CreateDocumentsFilingMetaDataValue(itemPM, entityPM);

                            break;
                        }

                    case ChangeSetOperation.Update:
                        {
                            this.UpdateDocumentsFilingMetaDataValue(itemPM);

                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            this.DeleteDocumentsFilingMetaDataValue(itemPM);

                            break;
                        }

                    default: { break; }
                }
            }
        }
        //    private void UpdateCustomsDocumentMetaDataValuesCollection()
        //    {

        //        if (documentsFilingMetaDataValueChangeSet != null)
        //        {
        //            if (entityPM.ExternalEntityName == "EFIFILEM" || entityPM.ExternalEntityName == "MFIFILEM")
        //            {

        //                foreach (DocumentsFilingMetaDataValuePM itemPM in documentsFilingMetaDataValueChangeSet)
        //                {
        //                    switch (itemPM.ChangeSetOp)
        //                    {
        //                        case ChangeSetOperation.Insert:
        //                            {
        //                                this.CreateCustomsDocumentsFilingMetaDataValue(itemPM);
        //                                break;
        //                            }

        //                        case ChangeSetOperation.Update:
        //                            {
        //                                this.UpdateCustomsDocumentsFilingMetaDataValue(itemPM);
        //                                break;
        //                            }

        //                        case ChangeSetOperation.Delete:
        //                            {
        //                                this.DeleteCustomsDocumentsFilingMetaDataValue(itemPM);
        //                                break;
        //                            }

        //                        default: { break; }
        //                    }
        //                }
        //            }

        //            customContext.SaveChanges();
        //        }



        //    }

        private void CreateDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM, DocumentsFilingPM entityPM)
        {
            itemPM.Id = IdCounter.GetNumber("DocumentsFilingMetaDataValue", tenant).ToString();
            itemPM.DocumentsFilingId = entityPM.Id;
            itemPM.Tenant = tenant;

            DocumentsFilingMetaDataValue itemPoco = new DocumentsFilingMetaDataValue()
            {
                Id = itemPM.Id,
            };

            DocumentsFilingMetaDataValueMapping.MapEntity(itemPM, itemPoco, true);
            documentsFilingMetaDataValueRepository.Insert(itemPoco);
        }
        private void UpdateDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        {
            DocumentsFilingMetaDataValue itemPoco = documentsFilingMetaDataValueRepository.GetSingleDocumentsFilingMetaDataValue(itemPM.Id, tenant);
            DocumentsFilingMetaDataValueMapping.MapEntity(itemPM, itemPoco, false);
            documentsFilingMetaDataValueRepository.Update(itemPoco);
        }
        private void DeleteDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        {
            DocumentsFilingMetaDataValue itemPoco = documentsFilingMetaDataValueRepository.GetSingleDocumentsFilingMetaDataValue(itemPM.Id, tenant);
            documentsFilingMetaDataValueRepository.Delete(itemPoco);
        }

        //    private void CreateCustomsDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        //    {
        //        //todo vladi resolve CustomsSettingQueryService problem - curcular dependency
        //        //CustomsDocumentQueryService myCustomsDocumentQueryService = new CustomsDocumentQueryService(tenant);
        //        //string documentsFilingId = myCustomsDocumentQueryService.GetDocumentInIdByCustomsDocId(entityPM.Id, tenant);

        //        //if (documentsFilingId != null)
        //        {
        //            CustomsDocumentMetaDataValue customsDocumentMetaDataValue = new CustomsDocumentMetaDataValue();
        //            customsDocumentMetaDataValue.MetaDataTypeCode = itemPM.DocumentsMetaDataTypeCode;
        //            customsDocumentMetaDataValue.CustomsDocumentId = entityPM.Id;
        //            customsDocumentMetaDataValue.Tenant = tenant;
        //            customsDocumentMetaDataValue.MetaDataValue = itemPM.MetaDataValue;
        //            customsDocumentMetaDataValueRepository.Insert(customsDocumentMetaDataValue);
        //        }
        //    }
        //    private void UpdateCustomsDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        //    {

        //        CustomsDocumentMetaDataValue customsDocumentMetaDataValue = customsDocumentMetaDataValueRepository.GetCustomsDocumentMetaDataValuesByCustomDocumentAndMetaDateValue(entityPM.Id, tenant, itemPM.DocumentsMetaDataTypeCode);
        //        if (customsDocumentMetaDataValue != null)
        //        {
        //            customsDocumentMetaDataValue.MetaDataValue = itemPM.MetaDataValue;
        //            customsDocumentMetaDataValueRepository.Update(customsDocumentMetaDataValue);
        //        }

        //    }
        //    private void DeleteCustomsDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        //    {

        //        CustomsDocumentMetaDataValue customsDocumentMetaDataValue = customsDocumentMetaDataValueRepository.GetCustomsDocumentMetaDataValuesByCustomDocumentAndMetaDateValue(entityPM.Id, tenant, itemPM.DocumentsMetaDataTypeCode);
        //        if (customsDocumentMetaDataValue != null)
        //            customsDocumentMetaDataValueRepository.Delete(customsDocumentMetaDataValue);
        //    }

        //    public string RandomString(int length)
        //    {
        //        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //        var random = new Random();
        //        return new string(Enumerable.Repeat(chars, length)
        //          .Select(s => s[random.Next(s.Length)]).ToArray());
        //    }

        //    public DocumentsFilingParams MyDocumentsFilingParams { get; set; }

        //    public DocumentsFilingMetaDataValuePM GetDocumentsFilingMetaDataValueByFilingIdAndCode(string documentsFilingId, string code, string type = null)
        //    {
        //        DocumentsFilingMetaDataValuePM MyDocumentMetaDataValues = null;
        //        if (string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(code))
        //        {
        //            var documentTypeMetaDataRepo = new DocumentsMetaDataTypeRepository(tenant);
        //            //                DocumentsMetaDataType myDocumentsMetaDataType = documentTypeMetaDataRepo.GetSingleDocumentsMetaDataTypeByCode(code, tenant);
        //            DocumentsMetaDataType myDocumentsMetaDataType = documentTypeMetaDataRepo.GetSingleDocumentsMetaDataTypeByCustomsMetaDataCode(code, tenant);
        //            if (myDocumentsMetaDataType != null) type = myDocumentsMetaDataType.Id;
        //        }

        //        if (!string.IsNullOrEmpty(type))
        //        {
        //            var documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
        //            MyDocumentMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTypeTenant(documentsFilingId, type, tenant);
        //        }
        //        return MyDocumentMetaDataValues;
        //    }

        //    private bool IsAPDNCNDocumentUploaded(DocumentsFilingPM documentFiling)
        //    {
        //        var objectTable = ObjectTableRepository.GetSingleObjectTable(documentFiling.ObjectTableId, tenant, false);
        //        if (objectTable == null)
        //        {
        //            return false;
        //        }
        //        if (!((objectTable.Name == "APInvoice") || objectTable.Name == "Shipment"))
        //        {
        //            return false;
        //        }
        //        if (!(documentFiling.DocumentTypeCode == "APDNCN"))
        //        {
        //            return false;
        //        }
        //        if (!(documentFiling.HasFile))
        //        {
        //            return false;
        //        }
        //        if (!(documentFiling.Received))
        //        {
        //            return false;
        //        }
        //        return true;
        //    }


        //    #region Digital Portal 

        //    public string UploadDigitalDocument(DigitalUploaderInfo info, int tenant, Contact loggedContact)
        //    {
        //        if (loggedContact == null)
        //        {
        //            return null;
        //        }

        //        var objecttableId = GetObjectTableId(info.ObjectTableName, tenant);
        //        var todatDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        //        DocumentsFilingPM newDocument = new DocumentsFilingPM()
        //        {
        //            DocumentTypeId = info.DocumentTypeId,
        //            EntityId = info.EntityId,
        //            Tenant = tenant,
        //            ObjectTableName = info.ObjectTableName,
        //            ObjectTableId = objecttableId,
        //            DirectionCode = "I",
        //            ReceivedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
        //            FileExtension = info.FileExtension,
        //            FileSize = info.FileSize,
        //            ReceivedByByContactId = loggedContact.Id,
        //            Notes = info.Notes,
        //            CreateDate = todatDate,
        //            UpdateDate = todatDate,
        //            IsFromUnifreightPodMobile = true,
        //            IsApprovalRequired = info.IsApprovalRequired,
        //            HasFile = true,
        //            Received = true,
        //            IsFromDigital = true,
        //            ReceivedByPartner = "Digital",
        //        };

        //        UserRepository userRepository = new UserRepository(tenant);
        //        string loggedUserId = loggedContact.Id;
        //        newDocument.CreatedByUserId = loggedUserId;
        //        newDocument.UpdatedByUserId = loggedUserId;
        //        newDocument.ReceivedByUserId = loggedUserId;
        //        newDocument.OwnerId = loggedUserId;
        //        bool isContactUser = false;
        //        isContactUser = userRepository.IsContactIdExist(loggedContact.Id, tenant);

        //        if (!isContactUser)
        //        {
        //            var loggedUserEmail = "system@tenant" + tenant + ".com";
        //            var loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, true);
        //            loggedUserId = loggedUser.Id;
        //            newDocument.ReceivedByUserId = loggedUserId;
        //            newDocument.CreatedByUserId = loggedUserId;
        //            newDocument.UpdatedByUserId = loggedUserId;
        //            newDocument.OwnerId = loggedUserId;
        //        }

        //        newDocument.SearchFields = newDocument.Code + "," + newDocument.DirectionCode + "," + loggedContact.EnglishName + "," + loggedContact.LocalName;

        //        if (string.IsNullOrEmpty(info.Id))
        //        {
        //            Create(newDocument, null);
        //        }
        //        else
        //        {
        //            newDocument.Id = info.Id;
        //            Update(newDocument, true);
        //        }

        //        return newDocument?.Id;
        //    }

        //    private string GetObjectTableId(string objectTableName, int tenant)
        //    {
        //        ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
        //        string objectTableId = objectTableQuery.GetObjectTableIdByName(objectTableName);
        //        return objectTableId;
        //    }

        //    #endregion Digital Portal 

    }
    public class UniFileVerM
    {
        public string COMID { get; set; }

        public int VERSION { get; set; }

        public string MD5HASH { get; set; }

        public string EXTENSION { get; set; }

    }
    public class DocumentsFilingParams
    {
        public bool IsCourier { get; set; }
        public string MainInterfaceCode { get; set; }


    }

}
