using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;

using System.Linq;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Configuration;
using System.Xml.Serialization;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using System.Xml.Linq;
using Logitude.BL.CommonDataModel.Helpers;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
using System.Runtime.Remoting.Contexts;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentsFilingService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentsFiling Poco { get; set; }
        private DocumentsFilingPM entityPM;
        private ICustomContext customContext;
        private ICommonDataContext objectContext;
        private DocumentsFilingRepository entityRepository;
        private DocumentsFilingMetaDataValueRepository documentsFilingMetaDataValueRepository;
        private CustomsDocumentMetaDataValueRepository customsDocumentMetaDataValueRepository;

        private DocumentTypeRepository documentTypeRepository;
        private DocumentRepository documentRepository;
        private ObjectTableRepository ObjectTableRepository;
        private ShipmentComputedFieldsRepository shipmentComputedFieldsRepository;
        private TenantQuery tenantQuery;

        private bool _OnCreateUnifreightFillingMode;
        private const int FileSizeOnUnifreightConst = 20160220;
        HybridPartnerPM CurrentHybridPartner;
        public DocumentsFilingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
             this.customContext =  CustomContext.GetContext(tenant);
             this.entityRepository = new DocumentsFilingRepository(objectContext);
            this.documentsFilingMetaDataValueRepository = new DocumentsFilingMetaDataValueRepository(objectContext);
            this.customsDocumentMetaDataValueRepository = new CustomsDocumentMetaDataValueRepository(customContext);

            this.documentTypeRepository = new DocumentTypeRepository(objectContext);
            this.documentRepository = new DocumentRepository(objectContext);
            shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
            ObjectTableRepository = new ObjectTableRepository(tenant);
            SetHybridPartner(tenant);
        }

        private void SetHybridPartner(int myTenant)
        {
            HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(myTenant);
            CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(myTenant);
        }

        private bool CheckIfSignRequired(string EntityDirection, string DocTypeID, int myTenant)
        {
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(DocTypeID, myTenant);
            if (!string.IsNullOrEmpty(EntityDirection))
            {
                if ((EntityDirection == "A" && documentType.IsAirDigitalSignRequired) || (EntityDirection == "I" && documentType.IsInlandDigitalSignRequired) || (EntityDirection == "O" && documentType.IsOceanDigitalSignRequired))
                {
                    return true;
                }
            }
            return false;
        }
        public void Create(DocumentsFilingPM theEntityPm, byte[] fileData = null, string loggedUserId = null, bool FromService = false, string documentId = null)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;

            if (!entityPM.IsHybrid || entityPM.IsAttachment)
            {
                // this.entityPM.Id = IdCounter.GetNumber("Document", tenant).ToString();


                var guid = Guid.NewGuid();
                var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
                base64string = base64string.Substring(0, 22);
                base64string = base64string.Replace("/", "_");
                base64string = base64string.Replace("+", "-");
                this.entityPM.Id = base64string;
                bool inOracleCreateNewTransaction =
                 (
                 this.MyCustomDocumentsFilingParams != null &&
                 (this.MyCustomDocumentsFilingParams.MainInterfaceCode == "3053" || this.MyCustomDocumentsFilingParams.MainInterfaceCode == "8302") &&
                 this.MyCustomDocumentsFilingParams.IsCourier
                 //CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant).CompanyType == "B"//Courier
                
                 );
                
                this.entityPM.Code = CodeCounter.GetNumber("DocumentsFiling", tenant, inOracleCreateNewTransaction).ToString();
            }
            if (!entityPM.IsHybrid)
            {
                //entityPM.Code = CodeCounter.GetNumber("DocumentsFiling", entityPM.Tenant).ToString();
                string loggedUserEmail = "";
                UserRepository userRepository = new UserRepository(entityPM.Tenant);
                User loggedUser = null;
                if (entityPM.IsFromUnifreightPodMobile)
                {
                    loggedUserEmail = "system@tenant" + entityPM.Tenant + ".com";
                    loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, entityPM.Tenant, true);
                }
                else
                {
                    if (string.IsNullOrEmpty(loggedUserId))
                    {
                        loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                        loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, entityPM.Tenant, true);
                    }
                    else
                    {
                        loggedUser = userRepository.GetSingleUser(loggedUserId, entityPM.Tenant, true);
                    }
                }
                if (loggedUser != null)
                {
                    entityPM.SearchFields = entityPM.Code + "," + entityPM.DirectionCode + "," + loggedUser.Contact.EnglishName + "," + loggedUser.Contact.LocalName;
                    if (string.IsNullOrEmpty(entityPM.CreatedByUserId))
                    {
                        entityPM.CreatedByUserId = loggedUser.Id;
                    }
                    entityPM.OwnerId = entityPM.CreatedByUserId;
                    entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    entityPM.UpdatedByUserId = entityPM.CreatedByUserId;
                    entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }

            }

            this.entityPM.Id = this.entityPM.Id.PadRight(30, '0');

            //Added by Maheera
            //this.entityPM.SecurityId = entityPM.Id + System.Web.Security.Membership.GeneratePassword(10, 0);
            Random rnd = new Random();
            this.entityPM.SecurityId = entityPM.Id + RandomString(10);

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            this.Poco = new DocumentsFiling();
            this.Poco.Id = this.entityPM.Id;

            DocumentsFilingValidating.Validate(theEntityPm);
            DocumentsFilingTracing.Trace(theEntityPm, Poco, isNewEntity);
            _OnCreateUnifreightFillingMode = BlobFileInfoExt.IsUnifreightFillingModeBase(theEntityPm.Tenant, theEntityPm.Folder);

            if ((!FromService || _OnCreateUnifreightFillingMode) && documentId == null)
            {
                if (
                    (entityPM.DirectionCode == "I")
                    //|| 
                    //(entityPM.DirectionCode == "E")
                    )
                    
                {
                    entityPM.DocumentId = BuildDocument(fileData, true, entityPM.Id);

                }
            }
            else if (string.IsNullOrEmpty(entityPM.DocumentId))
            {
                entityPM.DocumentId = documentId;

            }

            tenantQuery = new TenantQuery(theEntityPm.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(theEntityPm.Tenant);

            if (string.IsNullOrEmpty(entityPM.EntityId) && tenantPM.IsDocumentsArchive == false)
            {
                DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(theEntityPm.Tenant);
                var LBC = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBC", theEntityPm.Tenant,true);
                CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(theEntityPm.Tenant);
                if (theEntityPm.CustomerTenantNumber != null && LBC != null)
                {
                    var TenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(theEntityPm.Tenant, (int)theEntityPm.CustomerTenantNumber);
                    if (TenantAccess != null)
                    {
                        if (entityPM.DocumentsFilingMetaDataValues == null)
                        {
                            entityPM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>();
                        }
                        CustomerTenantAccessCardQuery TenantAccessCardQuery = new CustomerTenantAccessCardQuery(theEntityPm.Tenant);
                        var Cards = TenantAccessCardQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(TenantAccess.Id, theEntityPm.Tenant).Where(a => a.StatusTypeCode != "IA").ToList();
                        foreach (var item in Cards)
                        {
                            var CusCode = item.CustomerCode;
                            if (string.IsNullOrEmpty(CusCode))
                            {
                                CardRepository repo = new CardRepository(theEntityPm.Tenant);
                                var mycustomer = repo.GetSingleCard(item.CustomerId, theEntityPm.Tenant);
                                if (mycustomer != null)
                                {
                                    CusCode = mycustomer.Code;
                                }
                            }

                            DocumentsFilingMetaDataValuePM value1 = new DocumentsFilingMetaDataValuePM();
                            value1.ChangeSetOp = ChangeSetOperation.Insert;
                            value1.DocumentsFilingId = entityPM.Id;
                            value1.MetaDataValue = CusCode;
                            value1.DocumentsMetaDataTypeId = LBC.Id;
                            value1.Tenant = theEntityPm.Tenant;
                            entityPM.DocumentsFilingMetaDataValues.Add(value1);
                        }
                    }

                }

            }

            foreach (DocumentsFilingMetaDataValuePM itemPM in entityPM.DocumentsFilingMetaDataValues)
            {
                this.CreateDocumentsFilingMetaDataValue(itemPM);
                this.CreateCustomsDocumentsFilingMetaDataValue(itemPM);
            }

            //if (tenantPM.IsHybrid)
            //{
            //    theEntityPm.IsSharedWithCustomer = true;
            //} 
            bool HavingDREL = false;




            var OldIsSigned = Poco.IsDigitallySigned;


            DocumentsFilingMapping.MapEntity(theEntityPm, Poco, isNewEntity);


            if (tenantPM.IsDocumentsArchive == true)
            {
                if (theEntityPm.DirectionCode == "I")
                {
                    var OTName = ObjectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, tenant, false);
                    if (OTName != null && OTName.Name == "Shipment" && !string.IsNullOrEmpty(this.Poco.EntityId))
                    {

                        var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(theEntityPm.EntityId, theEntityPm.Tenant);
                        ShipmentCompField.DocumentsSearchFields += "," + this.Poco.SearchFields;
                        //ShipmentCompField.LastDocumentDateTime = DateTime.Now;
                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                        var EntityDirection = documentsFilingQuery.GetDirectionForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        if (CheckIfSignRequired(EntityDirection, this.entityPM.DocumentTypeId, this.entityPM.Tenant) && !this.entityPM.IsDigitallySigned && this.entityPM.HasFile && (!string.IsNullOrEmpty(this.entityPM.FileExtension) && this.entityPM.FileExtension.ToLower() == "pdf"))
                        {
                            ShipmentCompField.IsDigitalSignRequired = true;
                            Poco.IsDigitalSignRequired = true;
                        }
                        else if (this.entityPM.IsDigitallySigned)
                        {
                            ShipmentCompField.IsDigitalSignRequired = documentsFilingQuery.GetIfSignRequiredForEntity(Poco.EntityId, tenant, false);
                            Poco.IsDigitalSignRequired = false;
                        }

                        //if (theEntityPm.HasFile)
                        //{

                        //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(this.Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        //    ShipmentCompField.IsMissingDocuments = hasmissing;
                        //}
                        //else
                        //{
                        //    if (theEntityPm.DocumentTypeCode == "740" || theEntityPm.DocumentTypeCode == "706" || theEntityPm.DocumentTypeCode == "380")
                        //    {
                        //        ShipmentCompField.IsMissingDocuments = true; 
                        //    }
                        //}
                        var document = documentRepository.GetSingleDocument(tenant, this.entityPM.DocumentId);
                        if (document != null && document.HasFile == true)
                        {
                            ShipmentCompField.LastDocumentDateTime = DateTime.Now;
                        }
                        ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        ShipmentCompField.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        if (ShipmentCompField.MissingDocumentsCount == 0)
                        {
                            ShipmentCompField.IsMissingDocuments = false;
                        }
                        else
                        {
                            ShipmentCompField.IsMissingDocuments = true;
                        }
                        var IsRequested = documentsFilingQuery.GetIfIsRequestedForEntity(Poco.EntityId, tenant);
                        if (IsRequested == false && Poco.IsRequested && !Poco.IsDeleted)
                        {
                            IsRequested = true;
                        }
                        ShipmentCompField.IsRequestedDocuments = IsRequested;
                        ShipmentCompField.RequestedDocumentsCount = documentsFilingQuery.GetRequestedDocCountForEntity(this.Poco.EntityId, tenant);
                        if (Poco.IsRequested && !Poco.IsDeleted)
                        {
                            ShipmentCompField.RequestedDocumentsCount++;
                        }


                        ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                        shipmentComputedFieldsHelper.UpdateShipmentComputedFields(ShipmentCompField, shipmentComputedFieldsRepository.context);

                        // shipmentComputedFieldsRepository.Update(ShipmentCompField);
                        // shipmentComputedFieldsRepository.SubmitChanges();
                        try
                        {
                            if (!entityPM.DontAddToQueue && ((entityPM.IsSharedWithForwarder && !theEntityPm.IsSharedWithCustomer) || (entityPM.IsSharedWithCustomer && entityPM.IsDigitallySigned && OldIsSigned == false)))
                            {
                                IQueueService queueservice = new DbQueueService();
                                queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", 0);
                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.EntityId }, { "DocumentFilingId", entityPM.Id }, { "Tenant", tenant.ToString() }, }, tenant);
                            }


                        }
                        catch (Exception ex)
                        {
                            string ip = "";
                            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                            {
                                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                                if (string.IsNullOrEmpty(currentIP))
                                {
                                    currentIP = HttpContext.Current.Request.UserHostAddress;
                                }
                                ip = currentIP;
                            }
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
                        }
                    }
                }
            }
            DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(theEntityPm.Tenant);
            DocumentsMetaDataType Dreltype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("DREL", theEntityPm.Tenant,true);
            DocumentsMetaDataType LBFtype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("LBF", theEntityPm.Tenant, true);
            if (Dreltype != null && LBFtype != null)
            {
                var DRELMetaData = entityPM.DocumentsFilingMetaDataValues.Where(a => (a.DocumentsMetaDataTypeId == Dreltype.Id || a.DocumentsMetaDataTypeId == LBFtype.Id));
                if (DRELMetaData != null && DRELMetaData.Count() > 0)
                {
                    HavingDREL = true;
                    //entityPM.IsSharedWithCustomer = true;
                }
            }


            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            ///move after adding (was Devart.Data.Oracle.OracleException: ORA-02291: אילוץ כלילות (AMINET_MAIN.FK_N1103284768) הופר - מפתח אב לא נמצא )
            if (!tenantPM.IsDocumentsArchive && LogitudeSettings.DeploymentStage != "Simplog")
            {
                AddToTasksQueue(theEntityPm, isNewEntity, loggedUserId);
            }

            AddImporterQueue(theEntityPm, tenantPM, HavingDREL);

            if (!string.IsNullOrEmpty(this.entityPM.DocumentId))
            {
                Document doc = documentRepository.GetSingleDocument(this.entityPM.Tenant, this.entityPM.DocumentId);
                if (doc != null && doc.HasFile)
                {
                    AddDocumentBackupLog();
                }
            }

        }

        private void AddImporterQueue(DocumentsFilingPM theEntityPm, TenantPM tenantPM, bool HavingDREL)
        {
            if (!LogitudeSettings.IsCostomsDeploy && !tenantPM.IsDocumentsArchive && IsDocumentMatchesLogboxConditions(theEntityPm, HavingDREL))
            {
                AddImportersShipmentDocumentsQueueMessage();
            }
        }

        private bool IsDocumentMatchesLogboxConditions(DocumentsFilingPM theEntityPm, bool HavingDREL)
        {
            if (theEntityPm.DirectionCode != "I" || !(!theEntityPm.IsSharedWithForwarder || (theEntityPm.IsSharedWithForwarder && HavingDREL == true)))
                return false;

            if (!IsConnectedToShipment())
                return false;

            bool isDocumentMatchsLogboxConditions = !entityPM.DontAddToQueue
            && (entityPM.IsSharedWithCustomer || (theEntityPm.IsSharedWithForwarder && HavingDREL == true))
            && (!string.IsNullOrEmpty(entityPM.EntityId) || entityPM.IsDeleted);

            if (!isDocumentMatchsLogboxConditions)
                return false;

            isDocumentMatchsLogboxConditions = HasConnectedShipmentInLogbox(theEntityPm);
            return isDocumentMatchsLogboxConditions;
        }

        private bool IsConnectedToShipment()
        {
            ObjectTable connectedObjectTable = ObjectTableRepository.GetSingleObjectTable(Poco.ObjectTableId, tenant, false);
            if (connectedObjectTable != null && connectedObjectTable.Name == "Shipment")
                return true;
            return false;
        }

        private bool HasConnectedShipmentInLogbox(DocumentsFilingPM documentsFilingPM)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(documentsFilingPM.Tenant);
            ShipmentPM connectedShipment = shipmentQuery.GetSingleShipmentPM(documentsFilingPM.EntityId, documentsFilingPM.Tenant);
            if (connectedShipment == null)
                return false;

            if (!string.IsNullOrEmpty(connectedShipment.CustomFileId))
            {
                return HasCustomFileShipmentInLogbox(documentsFilingPM, shipmentQuery, connectedShipment);
            }
            else
            {
                return !string.IsNullOrEmpty(connectedShipment.CustomerShipmentNumber);
            }
        }

        private bool HasCustomFileShipmentInLogbox(DocumentsFilingPM documentsFilingPM, ShipmentQuery shipmentQuery, ShipmentPM connectedShipment)
        {
            ShipmentPM connectedImportFileShipment = shipmentQuery.GetSingleShipmentPM(connectedShipment.CustomFileId, documentsFilingPM.Tenant);
            bool hasShipmentInLogbox = connectedImportFileShipment != null && !string.IsNullOrEmpty(connectedImportFileShipment.CustomerShipmentNumber);

            return hasShipmentInLogbox;
        }

        private void AddImportersShipmentDocumentsQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ImportersShipmentDocumentsQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.EntityId }, { "DocumentFilingId", entityPM.Id }, { "Tenant", tenant.ToString() }, }, tenant);
        }

        private  void MyTryBuildUD2LT(DocumentsFilingPM extDocPM)
        {
			Task.Run(async () =>
			{
				ICreateUD2LTService myICreateUD2LTService = ContainerAccessor.Container.Resolve(typeof(ICreateUD2LTService), "CreateUD2LTService", new ParameterOverride("", tenant)) as ICreateUD2LTService;

			    // Start a transaction
			    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
			    {
			    	try
			    	{
						LogitudeSettings.HandleLogMe("TOOK 1: ", true, "123", DateTime.Now);
						myICreateUD2LTService.JustDoIt(extDocPM);
						LogitudeSettings.HandleLogMe("TOOK 2: ", true, "123", DateTime.Now);
						// Commit the transaction if everything is successful
						transactionScope.Complete();
			    	}
			    	catch (Exception ex)
			    	{
			    		// Handle the exception or log it
			    		LogitudeSettings.HandleLogMe("Error in ICreateUD2LTService JustDoIt: " + ex.Message, true, "CreateUD2LTService.Error", DateTime.Now);
			    	}
			    }
				await Task.Delay(TimeSpan.FromSeconds(1)); // Delay for 1 second

			}).ContinueWith(async task1 =>
			{
				ISendBondedCustomDocumentService myISendBondedCustomDocumentService = ContainerAccessor.Container.Resolve(typeof(ISendBondedCustomDocumentService), "SendBondedCustomDocumentService", new ParameterOverride("", tenant)) as ISendBondedCustomDocumentService;

				// Start a transaction
				using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
				{
					try
					{
						LogitudeSettings.HandleLogMe("TOOK 3: ", true, "123", DateTime.Now);
						myISendBondedCustomDocumentService.JustDoIt(extDocPM);
						LogitudeSettings.HandleLogMe("TOOK 4: ", true, "123", DateTime.Now);
						// Commit the transaction if everything is successful
						transactionScope.Complete();
					}
					catch (Exception ex)
					{
						// Handle the exception or log it
						LogitudeSettings.HandleLogMe("Error in ISendBondedCustomDocumentService JustDoIt: " + ex.Message, true, "ISendBondedCustomDocumentService.Error", DateTime.Now);
					}
				}
			});

		}
        private  void MyTrySendBondedCustomDocument(DocumentsFilingPM extDocPM)
        {
            DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(extDocPM.Tenant);
            var ENDOC = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("ENDOC", extDocPM.Tenant,true);
            
            if (ENDOC != null)
            {
                if (extDocPM.DocumentsFilingMetaDataValues.Any(r => r.DocumentsMetaDataTypeCode == "ENDOC")
                    ||
                    extDocPM.DocumentsFilingMetaDataValues.Any(r => r.DocumentsMetaDataTypeId == ENDOC.Id))
                {
                    this.HaveENDOC_DocumentsFilingMetaDataValues = true;
                }
            }
			

		}
		private void TryBuildUD2LT(DocumentsFilingPM extDocPM)
		{
			ICreateUD2LTService myICreateUD2LTService = ContainerAccessor.Container.Resolve(typeof(ICreateUD2LTService), "CreateUD2LTService", new ParameterOverride("", tenant)) as ICreateUD2LTService;
			myICreateUD2LTService.JustDoIt(extDocPM);

		}
		private void TrySendBondedCustomDocument(DocumentsFilingPM extDocPM)
		{
			DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(extDocPM.Tenant);
			var ENDOC = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("ENDOC", extDocPM.Tenant, true);

			if (ENDOC != null)
			{
				if (extDocPM.DocumentsFilingMetaDataValues.Any(r => r.DocumentsMetaDataTypeCode == "ENDOC")
					||
					extDocPM.DocumentsFilingMetaDataValues.Any(r => r.DocumentsMetaDataTypeId == ENDOC.Id))
				{
					this.HaveENDOC_DocumentsFilingMetaDataValues = true;
				}
			}
			ISendBondedCustomDocumentService myISendBondedCustomDocumentService = ContainerAccessor.Container.Resolve(typeof(ISendBondedCustomDocumentService), "SendBondedCustomDocumentService", new ParameterOverride("", tenant)) as ISendBondedCustomDocumentService;
			myISendBondedCustomDocumentService.JustDoIt(extDocPM);
		}

		private void AddDocumentBackupLog()
        {
            var OTName = ObjectTableRepository.GetSingleObjectTable(entityPM.ObjectTableId, tenant, false);
            DocumentFilingBackupSettingQuery documentFilingBackupSettingQuery = new DocumentFilingBackupSettingQuery(tenant);
            DocumentFilingBackupSettingPM settingsPM = documentFilingBackupSettingQuery.GetSinglePM(tenant, tenant);
            if (settingsPM != null && settingsPM.IsActive)
            {
                TenantRepository tenantRepository = new TenantRepository(objectContext);
                Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);

                //string manifestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(manifestSL);
                byte[] logXML = LogitudeXmlSerializer.SerializeObject(entityPM);
                ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = tenant,
                    From = currentTenant.Company,
                    To = "FTP",
                    CommunicationLogTypeCode = "DCBK",
                    QueueName = "DocumentFillingBackupQueue",
                    Priority = 1,
                    InOut = "O",
                    Status = "W",
                    LoggingUserId = entityPM.CreatedByUserId,
                    LoggingObjectTableId = OTName.Id,
                    LoggingEntityId = entityPM.Id,
                    Subject = "Document Backup",
                    FolderName = "DocumentFillingBackupQueue",
                    ByteData = logXML,
                    LoggingEntityReference = entityPM.Code,
                };

                logParams.QueueParameters = new Dictionary<string, string>() { { "DocumentFilingId", entityPM.Id }, { "Tenant", tenant.ToString() },  };
                Communications.AddCommunicationLog(logParams);

                //entityPM.BackedupExternally = Poco.BackedupExternally = true;
            }
        }

        public void Update(DocumentsFilingPM theEntityPm, byte[] fileData = null, string loggedUserId = null, bool FromService = false)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentsFiling(theEntityPm.Id, theEntityPm.Tenant);
            if (string.IsNullOrWhiteSpace(this.Poco.EntityId) && !string.IsNullOrWhiteSpace(theEntityPm.EntityId))
            {
                this._Connect2EntityId = true;
            }
            if (!string.IsNullOrEmpty(Poco.CustomerDocumentId))
            {
                theEntityPm.CustomerDocumentId = Poco.CustomerDocumentId;
            }
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            bool addBackupQueue = false;
            if (entityPM.DocumentId != Poco.DocumentId && !string.IsNullOrEmpty(entityPM.DocumentId))
                addBackupQueue = true;

            DocumentsFilingValidating.Validate(theEntityPm);
            DocumentsFilingTracing.Trace(theEntityPm, Poco, isNewEntity);
            if (!FromService)
            {
                if (theEntityPm.DirectionCode == "I")
                {
                    entityPM.DocumentId = BuildDocument(fileData, false);
                    addBackupQueue = (fileData != null);
                }
            }

            tenantQuery = new TenantQuery(theEntityPm.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(theEntityPm.Tenant);
            //if (tenantPM.IsHybrid)
            //{
            //    entityPM.IsSharedWithCustomer = true;
            //} 
            bool HavingDREL = false;
            
            UpdateDocumentsFilingMetaDataValuesCollection();
            
            var OldIsSigned = Poco.IsDigitallySigned;
            var WasRequested = Poco.IsRequested;

            DocumentsFilingMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            if (string.IsNullOrEmpty(Poco.SecurityId))
            {
                Random rnd = new Random();
                Poco.SecurityId = entityPM.Id + RandomString(10);
            }
            if (tenantPM.IsDocumentsArchive == true)
            {
                if (theEntityPm.DirectionCode == "I")
                {
                    var OTName = ObjectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, tenant, false);
                    if (OTName != null && OTName.Name == "Shipment" && !string.IsNullOrEmpty(this.Poco.EntityId))
                    {
                        var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(this.Poco.EntityId, this.Poco.Tenant);
                        ShipmentCompField.DocumentsSearchFields = GetEntityDocumentsSearchFields();


                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                        var EntityDirection = documentsFilingQuery.GetDirectionForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        if (CheckIfSignRequired(EntityDirection, this.entityPM.DocumentTypeId, this.entityPM.Tenant) && !this.entityPM.IsDigitallySigned && this.entityPM.HasFile && (!string.IsNullOrEmpty(this.entityPM.FileExtension) && this.entityPM.FileExtension.ToLower() == "pdf"))
                        {
                            ShipmentCompField.IsDigitalSignRequired = true;
                            Poco.IsDigitalSignRequired = true;
                        }
                        else if (this.entityPM.IsDigitallySigned)
                        {
                            ShipmentCompField.IsDigitalSignRequired = documentsFilingQuery.GetIfSignRequiredForEntity(Poco.EntityId, tenant,false);
                            Poco.IsDigitalSignRequired = false;
                        }
                        //if (theEntityPm.HasFile)
                        //{


                        //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(this.Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        //    ShipmentCompField.IsMissingDocuments = hasmissing;
                        //}
                        //else
                        //{
                        //    if (theEntityPm.DocumentTypeCode == "740" || theEntityPm.DocumentTypeCode == "706" || theEntityPm.DocumentTypeCode == "380")
                        //    {
                        //        ShipmentCompField.IsMissingDocuments = true;
                        //    }
                        //}
                        //ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(this.Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        var document = documentRepository.GetSingleDocument(tenant, this.entityPM.DocumentId);
                        if (document != null && document.HasFile == true)
                        {
                            ShipmentCompField.LastDocumentDateTime = DateTime.Now;
                        }
                        ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        ShipmentCompField.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        if (ShipmentCompField.MissingDocumentsCount == 0)
                        {
                            ShipmentCompField.IsMissingDocuments = false;
                        }
                        else
                        {
                            ShipmentCompField.IsMissingDocuments = true;
                        }
                        var IsRequested = documentsFilingQuery.GetIfIsRequestedForEntity(Poco.EntityId, tenant);
                        if (IsRequested == false && Poco.IsRequested && !Poco.IsDeleted)
                        {
                            IsRequested = true;
                        }
                        ShipmentCompField.IsRequestedDocuments = IsRequested;
                        ShipmentCompField.RequestedDocumentsCount = documentsFilingQuery.GetRequestedDocCountForEntity(Poco.EntityId, tenant);
                        if (Poco.IsRequested && !Poco.IsDeleted)
                        {
                            ShipmentCompField.RequestedDocumentsCount++;
                        }

                        ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                        shipmentComputedFieldsHelper.UpdateShipmentComputedFields(ShipmentCompField, shipmentComputedFieldsRepository.context);
                        // shipmentComputedFieldsRepository.Update(ShipmentCompField);
                        //shipmentComputedFieldsRepository.SubmitChanges();
                        try
                        {
                             
                            if (!entityPM.DontAddToQueue && ((entityPM.IsSharedWithForwarder && !theEntityPm.IsSharedWithCustomer) || (entityPM.IsSharedWithCustomer && entityPM.IsDigitallySigned && OldIsSigned == false) || (entityPM.IsSharedWithCustomer && WasRequested && entityPM.HasFile == true)))
                            {
                                IQueueService queueservice = new DbQueueService();
                                queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", 0);
                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.EntityId }, { "DocumentFilingId", entityPM.Id }, { "Tenant", tenant.ToString() },  }, tenant);
                            }

                          
                        }
                        catch (Exception ex)
                        {
                            string ip = "";
                            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                            {
                                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                                if (string.IsNullOrEmpty(currentIP))
                                {
                                    currentIP = HttpContext.Current.Request.UserHostAddress;
                                }
                                ip = currentIP;
                            }
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
                        }

                    }
                }
            }
            DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(theEntityPm.Tenant);
            DocumentsMetaDataType Dreltype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("DREL", theEntityPm.Tenant, true);
            DocumentsMetaDataType LBFtype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("LBF", theEntityPm.Tenant, true);
            if (Dreltype != null && LBFtype != null)
            {
                var DRELMetaData = entityPM.DocumentsFilingMetaDataValues.Where(a => (a.DocumentsMetaDataTypeId == Dreltype.Id || a.DocumentsMetaDataTypeId == LBFtype.Id));
                if (DRELMetaData != null && DRELMetaData.Count() > 0)
                {
                    HavingDREL = true;
                    //entityPM.IsSharedWithCustomer = true;
                }
            }
            if (string.IsNullOrEmpty(Poco.SecurityId))
            {
                Random rnd = new Random();
                Poco.SecurityId = entityPM.Id + RandomString(10);
            }

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            if (!tenantPM.IsDocumentsArchive && LogitudeSettings.DeploymentStage != "Simplog")
            {
                AddToTasksQueue(theEntityPm, isNewEntity, loggedUserId);
            }
           
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            AddImporterQueue(theEntityPm, tenantPM, HavingDREL);

            if (addBackupQueue)
            {
                AddDocumentBackupLog();
            }
        }

        public void Update(DocumentsFilingPM theEntityPm, bool mapComposition = false)
        {
            if (mapComposition)
            {
                this.documentsFilingMetaDataValueChangeSet = theEntityPm.DocumentsFilingMetaDataValues;
            }
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentsFiling(theEntityPm.Id, theEntityPm.Tenant);
            if (!string.IsNullOrEmpty(Poco.CustomerDocumentId))
            {
                theEntityPm.CustomerDocumentId = Poco.CustomerDocumentId;
            }

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            DocumentsFilingValidating.Validate(theEntityPm);
            DocumentsFilingTracing.Trace(theEntityPm, Poco, isNewEntity);

            bool addBackupQueue = false;
            if (entityPM.DocumentId != Poco.DocumentId && !string.IsNullOrEmpty(entityPM.DocumentId))
                addBackupQueue = true;

            if (theEntityPm.DirectionCode == "I")
            {
                entityPM.DocumentId = BuildDocument(null, false);
            }


            tenantQuery = new TenantQuery(theEntityPm.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(theEntityPm.Tenant);
            //if (tenantPM.IsHybrid)
            //{
            //    entityPM.IsSharedWithCustomer = true;
            //} 
            UpdateDocumentsFilingMetaDataValuesCollection();
            bool HavingDREL = false;
           
            var OldIsSigned = Poco.IsDigitallySigned;
            var WasRequested = Poco.IsRequested;

            DocumentsFilingMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            if (tenantPM.IsDocumentsArchive == true)
            {
                if (theEntityPm.DirectionCode == "I")
                {
                    var OTName = ObjectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, tenant, false);
                    if (OTName != null && OTName.Name == "Shipment" && !string.IsNullOrEmpty(this.Poco.EntityId))
                    {
                        var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(this.Poco.EntityId, this.Poco.Tenant);
                        ShipmentCompField.DocumentsSearchFields = GetEntityDocumentsSearchFields();

                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                        var EntityDirection = documentsFilingQuery.GetDirectionForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        if (CheckIfSignRequired(EntityDirection, this.entityPM.DocumentTypeId, this.entityPM.Tenant) && !this.entityPM.IsDigitallySigned && this.entityPM.HasFile && (!string.IsNullOrEmpty(this.entityPM.FileExtension) && this.entityPM.FileExtension.ToLower() == "pdf"))
                        {
                            ShipmentCompField.IsDigitalSignRequired = true;
                            Poco.IsDigitalSignRequired = true;
                        }
                        else if (this.entityPM.IsDigitallySigned)
                        {
                            ShipmentCompField.IsDigitalSignRequired = documentsFilingQuery.GetIfSignRequiredForEntity(Poco.EntityId, tenant, false);
                            Poco.IsDigitalSignRequired = false;
                        }
                        //if (theEntityPm.HasFile)
                        //{


                        //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(this.Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        //    ShipmentCompField.IsMissingDocuments = hasmissing;
                        //}
                        //else
                        //{
                        //    if (theEntityPm.DocumentTypeCode == "740" || theEntityPm.DocumentTypeCode == "706" || theEntityPm.DocumentTypeCode == "380")
                        //    {
                        //        ShipmentCompField.IsMissingDocuments = true;
                        //    }
                        //}
                        //ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(this.Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        var document = documentRepository.GetSingleDocument(tenant, this.entityPM.DocumentId);
                        if (document != null && document.HasFile == true)
                        {
                            ShipmentCompField.LastDocumentDateTime = DateTime.Now;
                        }
                        ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        ShipmentCompField.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(Poco.EntityId, theEntityPm.ObjectTableId, tenant);
                        if (ShipmentCompField.MissingDocumentsCount == 0)
                        {
                            ShipmentCompField.IsMissingDocuments = false;
                        }
                        else
                        {
                            ShipmentCompField.IsMissingDocuments = true;
                        }
                        var IsRequested = documentsFilingQuery.GetIfIsRequestedForEntity(Poco.EntityId, tenant, entityPM);
                        if (IsRequested == false && Poco.IsRequested && !Poco.IsDeleted)
                        {
                            IsRequested = true;
                        }
                        ShipmentCompField.IsRequestedDocuments = IsRequested;
                        ShipmentCompField.RequestedDocumentsCount = documentsFilingQuery.GetRequestedDocCountForEntity(Poco.EntityId, tenant, entityPM);
                        if (Poco.IsRequested && !Poco.IsDeleted)
                        {
                            ShipmentCompField.RequestedDocumentsCount++;
                        }
                        ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                        shipmentComputedFieldsHelper.UpdateShipmentComputedFields(ShipmentCompField, shipmentComputedFieldsRepository.context);


                        //shipmentComputedFieldsRepository.Update(ShipmentCompField);
                        // shipmentComputedFieldsRepository.SubmitChanges();
                        try
                        {
                            
                            if (!entityPM.DontAddToQueue && ((entityPM.IsSharedWithForwarder && !theEntityPm.IsSharedWithCustomer) || (entityPM.IsSharedWithCustomer && entityPM.IsDigitallySigned && OldIsSigned == false) || (entityPM.IsSharedWithCustomer && WasRequested && entityPM.HasFile == true)))
                            {
                                IQueueService queueservice = new DbQueueService();
                                queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", 0);
                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.EntityId }, { "DocumentFilingId", entityPM.Id }, { "Tenant", tenant.ToString() }, }, tenant);
                            }
                        }
                        catch (Exception ex)
                        {
                            string ip = "";
                            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                            {
                                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                                if (string.IsNullOrEmpty(currentIP))
                                {
                                    currentIP = HttpContext.Current.Request.UserHostAddress;
                                }
                                ip = currentIP;
                            }
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
                        }

                    }
                }
            }

            DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(theEntityPm.Tenant);
            DocumentsMetaDataType Dreltype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("DREL", theEntityPm.Tenant,true);
            DocumentsMetaDataType LBFtype = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode("LBF", theEntityPm.Tenant,true);
            if (Dreltype != null && LBFtype != null)
            {
                var DRELMetaData = entityPM.DocumentsFilingMetaDataValues.Where(a => (a.DocumentsMetaDataTypeId == Dreltype.Id || a.DocumentsMetaDataTypeId == LBFtype.Id));
                if (DRELMetaData != null && DRELMetaData.Count() > 0)
                {
                    HavingDREL = true;
                    //entityPM.IsSharedWithCustomer = true;
                }
            }

            if (string.IsNullOrEmpty(Poco.SecurityId))
            {
                Random rnd = new Random();
                Poco.SecurityId = entityPM.Id + RandomString(10);
            }

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


            if (theEntityPm.IsUoloadedField)
            {
                DocumentPopulateAutomaticDateUpdateService documentPopulateAutomaticDateUpdateService = new DocumentPopulateAutomaticDateUpdateService();
               var OTName = ObjectTableRepository.GetSingleObjectTable(Poco.ObjectTableId, tenant, false);
                string objectTableName = OTName != null ? OTName.Name : "";
                documentPopulateAutomaticDateUpdateService.Update(new DocumentPopulateAutomaticDateArgs() { EntityId = theEntityPm.EntityId, ObjectTableName = objectTableName, DocumentTypeCode = theEntityPm.DocumentTypeCode, ProcessType = "Upload", Tenant = theEntityPm.Tenant });
            }


            if (!tenantPM.IsDocumentsArchive && !entityPM.DontAddToQueue)
            {
                AddToTasksQueue(theEntityPm, isNewEntity, null);
            }
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            AddImporterQueue(theEntityPm, tenantPM, HavingDREL);
           


            if (entityPM.IsUpdateSharedDocument)
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ResharedAgentDocumentQueue", tenant);
                queueservice.Send(new Dictionary<string, string>() { { "EntityId", entityPM.EntityId }, { "Tenant", entityPM.Tenant.ToString() }, { "DocumentTypeCode", entityPM.DocumentTypeCode }, { "SecurityId", entityPM.SecurityId } }, tenant, null, null, null, null);
                entityPM.IsUpdateSharedDocument = false;
            }

      
            if (addBackupQueue)
            {
                AddDocumentBackupLog();
            }

        }

        private string GetEntityDocumentsSearchFields()
        {
            var documentSearchFieldsLists = entityRepository.GetDocumentsFilingByEntityId(this.Poco.EntityId, this.Poco.ObjectTableId, this.Poco.Tenant).Where(d => d.Id != this.Poco.Id).Select(d => d.SearchFields).ToArray();
            var documentSearchFields = String.Join(",", documentSearchFieldsLists);
            documentSearchFields += ((!string.IsNullOrEmpty(documentSearchFields) ? "," :"") + this.Poco.SearchFields);
            return documentSearchFields;
        }

        private string BuildDocument(byte[] fileData, bool isnew, string DocumentsFilingId = null)
        {
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(this.entityPM.DocumentTypeId, this.entityPM.Tenant);
            Document document = null;
            if (!string.IsNullOrEmpty(this.entityPM.FileExtension))
            {
                this.entityPM.FileExtension = this.entityPM.FileExtension.ToLower();
            }

            if (isnew)
            {
                document = new Document()
                {
                    Id = IdCounter.GetNumber("Document", entityPM.Tenant).ToString(),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(this.entityPM.Tenant),
                    Tenant = this.entityPM.Tenant,
                    Folder = "docsin",

                };

                document.Extension = this.entityPM.FileExtension;

                if (this.entityPM.IsHybrid && !this.entityPM.IsAttachment && !string.IsNullOrEmpty(this.entityPM.Description))
                {
                    document.FileName = StringHelper.TruncateLongString(this.entityPM.Description, 120);
                }
                else
                {
                    document.FileName = !string.IsNullOrEmpty(this.entityPM.FileName) ? this.entityPM.FileName : documentType != null ? documentType.Name : "";
                }

                document.CalculatedFileName = document.FileName;
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
                //else
                //{ 
                //    document.HasFile = false;
                //    document.FileSize = this.entityPM.FileSize; 
                //}

                documentRepository.Add(document);
            }
            else //if (!!!!isnew) = update
            {
                if (!string.IsNullOrEmpty(this.entityPM.DocumentId))
                {
                    document = documentRepository.GetSingleDocument(tenant, this.entityPM.DocumentId);
                }

                if (entityPM.IsDeleted && !entityPM.DontDeleteRealFile)
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
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(this.entityPM.Tenant),
                            Tenant = this.entityPM.Tenant,
                            Folder = "docsin",

                        };

                        document.Extension = this.entityPM.FileExtension;

                        if (this.entityPM.IsHybrid && !this.entityPM.IsAttachment && !string.IsNullOrEmpty(this.entityPM.Description))
                        {
                            document.FileName = StringHelper.TruncateLongString(this.entityPM.Description, 120);
                        }
                        else
                        {
                            document.FileName = !string.IsNullOrEmpty(this.entityPM.FileName) ? this.entityPM.FileName : documentType.Name;
                        }

                        if (fileData != null)
                        {
                            document.HasFile = true;
                            document.FileSize = Convert.ToInt32(fileData.Length);
                        }
                        //else
                        //{
                        //    document.HasFile = false;
                        //    document.FileSize = this.entityPM.FileSize;
                        //}


                        documentRepository.Add(document);
                    }
                    else if (fileData != null) /// !!! if (document == null) 
                    {
                        document.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        document.Extension = this.entityPM.FileExtension;
                        document.FileSize = (fileData != null ? Convert.ToInt32(fileData.Length) : 0);
                        document.Tenant = Convert.ToInt32(entityPM.Tenant);
                        document.HasFile = true;
                        document.Folder = "docsin";
                        document.FileName = this.entityPM.FileName;
                        document.IsEncrypted = true;
                        documentRepository.Update(document);
                    }
                    else if (fileData == null)
                    {
                        if (string.IsNullOrEmpty(this.entityPM.FileExtension))
                        {
                            document.HasFile = false;
                            document.Extension = this.entityPM.FileExtension;
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
                        fileInfo.UFileVer = this.entityPM.LastVersion = 1;
                    }
                    else if (MyUniFileVerM != null) // get from DerivedClass  UnifreightDocumentsFilingService
                    {
                        if (fileDataMD5Hash != MyUniFileVerM.MD5HASH)
                        {
                            var PocoLastVersionPlus1 = this.Poco.LastVersion + 1;
                            fileInfo.UFileVer = this.entityPM.LastVersion = (MyUniFileVerM.VERSION + 1);
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

        private void UpdateShipmentLastDocumentDateTime(DocumentsFilingPM EntityPm)
        {
            var tenantQuery = new TenantQuery(EntityPm.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(EntityPm.Tenant);
            if (tenantPM.IsDocumentsArchive == true && EntityPm.HasFile)
            {
                var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(EntityPm.EntityId, tenant);
                if (ShipmentCompField != null)
                {
                    ShipmentCompField.LastDocumentDateTime = DateTime.Now;
                    shipmentComputedFieldsRepository.Update(ShipmentCompField);
                    shipmentComputedFieldsRepository.SubmitChanges();
                }

            }
        }

        public void AddToTasksQueue(DocumentsFilingPM extDocPM, bool isnew, string loggedUserId)
        {
            if (LogitudeSettings.IsCostomsDeploy && extDocPM.IsHybrid)//avoid non stop 
            {
				bool IsSendInTask = Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("SDT", extDocPM.Tenant);
				if (!IsSendInTask) 
                { 
                    TryBuildUD2LT(extDocPM);
                    TrySendBondedCustomDocument(extDocPM);
				}
                else
                {
					MyTryBuildUD2LT(extDocPM);
					MyTrySendBondedCustomDocument(extDocPM);
				}

			}
           
            if (!string.IsNullOrWhiteSpace(this.MetaDataVersionValue))
            {
                DocumentsFilingMetaDataValueQuery.UpSert_Del(extDocPM, "VER", this.MetaDataVersionValue);
            }

            if 
                (
                (!extDocPM.IsHybrid   && LogitudeSettings.IsCostomsDeploy) ||
                (LogitudeSettings.EnableHybridQueue && (CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner) && (!extDocPM.IsHybrid || (extDocPM.IsAttachment))
                && LogitudeSettings.DeploymentStage != "Simplog" && !extDocPM.NoAddToTasksQueue) &&
                !CustomsSettingsHelper.GetCache(tenant).StandAlone
                )
            {
                ObjectTable docTable = ObjectTableRepository.GetObjectTableById(extDocPM.ObjectTableId, extDocPM.Tenant);
                if (this.HaveENDOC_DocumentsFilingMetaDataValues ||
                    
                    (docTable != null && (docTable.Name == "Customer" || docTable.Name == "Shipment" ||
                    docTable.Name == "Customs.Declaration"
                    || docTable.Name == "Customs.Claim"
                    || docTable.Name == "Customs.PaymentOrder"
                    || docTable.Name == "Customs.Deficit"))
                    )
                {

                    if (!string.IsNullOrEmpty(extDocPM.DocumentId))
                    {

                        if (string.IsNullOrEmpty(loggedUserId))
                        {
                            string loggedUserEmail = HttpContext.Current.User.Identity.Name;
                            if (string.IsNullOrEmpty(loggedUserEmail))
                            {
                                loggedUserEmail = "system@tenant" + extDocPM.Tenant + ".com";
                            }

                            UserRepository userRepository = new UserRepository(tenant);
                            User loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, true);
                            loggedUserId = loggedUser.Id;

                        }

                        try
                        {
                            Document document = this.documentRepository.GetSingleDocument(extDocPM.Tenant, extDocPM.DocumentId);

                            if (document.HasFile == false)
                                return;

                            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                            BlobFileInfo fileInfo = new BlobFileInfo()
                            {
                                FileName = document.Id,
                                FolderName = document.Folder,
                                Extension = document.Extension,
                                Tenant = tenant,
                            };


                            bool sendOnlyMetaData = (
                                fileInfo.FolderName == "docsin" &&
                                LogitudeSettings.IsCostomsDeploy &&
                                LogitudeSettings.StorageServiceMode == "db" &&
                                !String.IsNullOrWhiteSpace(LogitudeSettings.GetLogitudeCustomsSettingsMInject(fileInfo.Tenant).OnPremiseFillingService)
                                );

                            byte[] fileData = null;
                            if (!sendOnlyMetaData)
                            {
                                fileData = storageservice.Read(fileInfo);

                                if (fileData == null)
                                    return;
                            }

                            //AzureLog.SaveLogsInStorage("Before adding document filing queue (Id:" + extDocPM.Id + ",Tenant:" + extDocPM.Tenant + ")", "L", DateTime.Now, "", "", 0, loggedUserId, loggedUserId, null);

                            ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                            ObjectTablePM table = tablesQuery.GetObjectTableByName("DocumentsFiling", 0);
                            CommunicationsParams logParams = new CommunicationsParams()
                            {
                                Tenant = tenant,
                                CommunicationLogTypeCode = "Q",
                                QueueName = "externaltasksqueue" + tenant + 1,
                                Priority = 1,
                                InOut = "O",
                                Status = "W",
                                LoggingUserId = loggedUserId,
                                LoggingObjectTableId = table.Id,
                                LoggingEntityId = extDocPM.Id,
                                Subject = "New Documents Filing Created",
                                FolderName = "ExternalTasksQueue",
                            };

                            if (!isnew)
                            {
                                logParams.Subject = "Documents Filing Data Updated";
                            }

                            DocumentsFilingPM mappedPM = DocumentsFilingHybridMapping.MapEntityToHybrid(extDocPM);

                            string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(mappedPM);

                            //54378
                            //string UseSend2UServer =ConfigurationManager.AppSettings["20190909.UseSend2UServer8302"]??"";
                            
                            string SuppressUseSend2UServer8302 = ConfigurationManager.AppSettings["20200123.SuppressUseSend2UServer8302"] ?? "";
                            if (string.IsNullOrWhiteSpace(SuppressUseSend2UServer8302)//!string.IsNullOrWhiteSpace(UseSend2UServer) 
                                && !string.IsNullOrWhiteSpace(this.MetaDataVersionValue)//DeclarationPrint
                                && CustomsSettingQueryService.GetLogitudeCustomsSettingsM(tenant).IsConnectedToUniFreight
                                )
                            {
                                Send2UServer(mappedPM, loggedUserId, extDocPM.Id);
                            }
                            else
                            {

                                //INSERT INTO "TOGGLES" (CODE, NAME, SEARCHFIELDS) VALUES ('HCD', 'Hybrid Courier document-Prevent feedback', 'Hybrid document-Prevent feedback')
                                //INSERT INTO "FEATURETOGGLES"(ID, TENANT, CREATEDATE, CREATEDBYUSERID, UPDATEDATE, UPDATEDBYUSERID, SEARCHFIELDS, TENANTNUMBER, INACTIVE, TOGGLECODE) VALUES('HCD', '1', TO_TIMESTAMP('2022-03-06 14:19:28.729000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', TO_TIMESTAMP('2022-03-06 14:19:46.456000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', 'HCD', '1', '0', 'HCD')

                                IDICustomsSettingQueryService customsSettingQueryService = ContainerAccessor.Container.Resolve(typeof(IDICustomsSettingQueryService), "DICustomsSettingQueryService", new ParameterOverride("", tenant)) as IDICustomsSettingQueryService;
                                var IsCourierTenant=customsSettingQueryService.IsCourierTenant(tenant);
                                bool sendHybridM = true;
                                if (IsCourierTenant)
                                {
                                    if (Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("HCD", tenant))
                                    {
                                        sendHybridM = false;
                                        if (extDocPM.ExternalEntityName == "CFIFILEM")
                                        {
                                            SendCustomsReferenceByTask(tenant, extDocPM.ExternalEntityReference, extDocPM.CustomReference, xmlstring, loggedUserId);
                                        }
                                    }
                                }
                                if (sendHybridM)
                                {
                                    List<QueueTask> queue1Tasks = new List<QueueTask>();


                                    queue1Tasks.Add(new QueueTask()
                                    {
                                        Action = "DocumentsFiling.Upsert",
                                        Parameters = new List<Parameter>()
                                             {
                                                new Parameter{ Name = "DocumentMetaData", Order = 1, Value = xmlstring }
                                             }
                                    });

                                    logParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue1Tasks);
                                    Communications.AddCommunicationLog(logParams);

                                    if (!sendOnlyMetaData)
                                    {

                                        CommunicationsParams task2logParams = new CommunicationsParams()
                                        {
                                            Tenant = tenant,
                                            CommunicationLogTypeCode = "Q",
                                            QueueName = "externaltasksqueue" + tenant + 2,
                                            Priority = 1,
                                            InOut = "O",
                                            Status = "W",
                                            LoggingUserId = loggedUserId,
                                            LoggingObjectTableId = table.Id,
                                            LoggingEntityId = extDocPM.Id,
                                            Subject = "Documents Filing Uploading binary file",
                                            FolderName = "ExternalTasksQueue",
                                        };

                                        // adding file data task
                                        List<QueueTask> queue2Tasks = new List<QueueTask>();
                                        string base64String = System.Convert.ToBase64String(fileData, 0, fileData.Length);
                                        queue2Tasks.Add(
                                            new QueueTask()
                                            {
                                                Action = "DocumentsFiling.UploadBinaryData",
                                                Parameters = new List<Parameter>()
                                            {
                                         new Parameter{ Name = "DocumentMetaData", Order = 1,Value =  xmlstring},
                                         new Parameter{ Name = "FileBinaryData",Order = 2,Value =  base64String},
                                            }
                                            });

                                        task2logParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue2Tasks);
                                        Communications.AddCommunicationLog(task2logParams);
                                    }
                                }
                            }
                            LogMessagingUtil.Instance.AppendLine("AddToTasksQueue (DocumentsFilingService)");
                            
                            // AzureLog.SaveLogsInStorage("After adding document filing queue (Id:" + extDocPM.Id + ",Tenant:" + extDocPM.Tenant + ")", "L", DateTime.Now, "", "", 0, loggedUserId, loggedUserId, null);
                        }
                        catch (Exception ex)
                        {
                            AzureLog.SaveLogsInStorage("Error While Adding Document filing queue (Id:" + extDocPM.Id + ",Tenant:" + extDocPM.Tenant + ")", "E", DateTime.Now, ex.Message, ex.StackTrace, 0, loggedUserId, loggedUserId, null);
                            throw ex;
                        }
                    }
                }
            }
        }

        private void SendCustomsReferenceByTask(int tenant, string unifreightCustomsFile, string cref, string xmlstring, string loggedUserId)
        {
            var _MyDeclarationPM = new DeclarationPM { Tenant = tenant };
            IDIUnifreightTaskService unifreightTaskService = ContainerAccessor.Container.Resolve(typeof(IDIUnifreightTaskService), "DIUnifreightTaskService", new ParameterOverride("", tenant)) as IDIUnifreightTaskService;
            LogMessagingUtil.Instance.AppendLine("OpenUnifreighTask for FILING " + unifreightCustomsFile + "  with reference " + cref );
            LogMessagingUtil.Instance.AppendLine(xmlstring);
            try
            {
                unifreightTaskService.OpenUnifreighTaskGen(_MyDeclarationPM, "CFIFILEM", unifreightCustomsFile, "L2UCREF", null, false, xmlstring, false);
            }
            catch (Exception ex)
            {
                AzureLog.SaveLogsInStorage("OpenUnifreighTask for FILING with reference " + cref, "E", DateTime.Now, ex.Message, ex.StackTrace, 0, loggedUserId, loggedUserId, null);
                throw ex;
            }
        }

        public void Send2UServer(DocumentsFilingPM myDocumentsFilingPM, string loggingUserId, string extDocPMId)
        {
            var ExternalEntityName = myDocumentsFilingPM.ExternalEntityName;
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
               "GGGHQHYBRID", "LogitudeTaskByUrouter")
            {
                Tenant = myDocumentsFilingPM.Tenant,
                objectTableName = "DocumentsFiling",
                CommunicationLoggingEntityReference = myDocumentsFilingPM.Id,
                EntityId = extDocPMId,
                UserId = loggingUserId,
                CommunicationSubject = "Documents Filing Data-HYBRID VIA USERVER",

                //LogitudeFile = myFile,
            };


            string xml = LogitudeXmlSerializer.SerializeObjectToUTF8XmlString<DocumentsFilingPM>(myDocumentsFilingPM);
                
                
            
            var myEnvelope = new Envelope() {
                 CommunicationLogId = Guid.NewGuid().ToString(),
                  Tasks= new List<QueueTask>() {
                      

                      new QueueTask() {

                          Action = "DocumentsFiling.Upsert",
                      Parameters = new List<Parameter>()
                      {
                            new Parameter()
                            {
                                 Value = xml
                            }
                      }
                  } }
                  
            };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, Envelope>(
                amitalCustomFileCommunicationModel, /*myDocumentsFilingPM*/ myEnvelope);
            bool pImmediately = true;
            var info = myUServerCommunicationService.Send(pImmediately);
            if (info.GenericResponseObj?.Status !="0" )//&&  !string.IsNullOrWhiteSpace(info.GenericResponseObj?.ErrorDescription))
            {
                throw new Exception($"Send 2 Urouter ErrorDescription{info.GenericResponseObj?.ErrorDescription}");
            }
        }

        private List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValueChangeSet;
        private bool HaveENDOC_DocumentsFilingMetaDataValues=false;
        private bool _Connect2EntityId;

        public void SetChangeSet(List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValueChangeSet)
        {
            this.documentsFilingMetaDataValueChangeSet = documentsFilingMetaDataValueChangeSet;
        }

        private void UpdateDocumentsFilingMetaDataValuesCollection()
        {
            var isExport = false;
            if (documentsFilingMetaDataValueChangeSet != null)
            {
                if (this.entityPM.ExternalEntityName == "EFIFILEM" || this.entityPM.ExternalEntityName == "MFIFILEM")
                    isExport = true;
                foreach (DocumentsFilingMetaDataValuePM itemPM in documentsFilingMetaDataValueChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateDocumentsFilingMetaDataValue(itemPM);
                                if(isExport)
                                    this.CreateCustomsDocumentsFilingMetaDataValue(itemPM);

                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateDocumentsFilingMetaDataValue(itemPM);
                                if (isExport)
                                    this.UpdateCustomsDocumentsFilingMetaDataValue(itemPM);

                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteDocumentsFilingMetaDataValue(itemPM);
                                if (isExport)
                                    this.DeleteCustomsDocumentsFilingMetaDataValue(itemPM);

                                break;
                            }
                      
                        default: { break; }
                    }
                }


                customContext.SaveChanges();
            }

           

        }

        private void CreateDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("DocumentsFilingMetaDataValue", tenant).ToString();
            itemPM.DocumentsFilingId = entityPM.Id;
            itemPM.Tenant = tenant;

            DocumentsFilingMetaDataValue itemPoco = new DocumentsFilingMetaDataValue()
            {
                Id = itemPM.Id,
            };

            DocumentsFilingMetaDataValueMapping.MapEntity(itemPM, itemPoco, true);
            documentsFilingMetaDataValueRepository.Add(itemPoco);
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
            documentsFilingMetaDataValueRepository.Remove(itemPoco);
        }

        private void CreateCustomsDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        {
            CustomsDocumentQueryService myCustomsDocumentQueryService = new CustomsDocumentQueryService(tenant);
            string documentsFilingId = myCustomsDocumentQueryService.GetDocumentInIdByCustomsDocId(itemPM.Id, tenant);

            if (documentsFilingId != null)
            {
                CustomsDocumentMetaDataValue customsDocumentMetaDataValue = new CustomsDocumentMetaDataValue();
                customsDocumentMetaDataValue.MetaDataTypeCode = itemPM.DocumentsMetaDataTypeCode;
                customsDocumentMetaDataValue.CustomsDocumentId = entityPM.Id;
                customsDocumentMetaDataValue.Tenant = tenant;
                customsDocumentMetaDataValue.MetaDataValue = itemPM.MetaDataValue;
                customsDocumentMetaDataValueRepository.Add(customsDocumentMetaDataValue);
            }
        }
        private void UpdateCustomsDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        {

            CustomsDocumentMetaDataValue customsDocumentMetaDataValue = customsDocumentMetaDataValueRepository.GetCustomsDocumentMetaDataValuesByCustomDocumentAndMetaDateValue(entityPM.Id, tenant, itemPM.DocumentsMetaDataTypeCode);
            if(customsDocumentMetaDataValue != null) {
                customsDocumentMetaDataValue.MetaDataValue = itemPM.MetaDataValue;
                customsDocumentMetaDataValueRepository.Update(customsDocumentMetaDataValue);
            }
           
        }
        private void DeleteCustomsDocumentsFilingMetaDataValue(DocumentsFilingMetaDataValuePM itemPM)
        {

            CustomsDocumentMetaDataValue customsDocumentMetaDataValue = customsDocumentMetaDataValueRepository.GetCustomsDocumentMetaDataValuesByCustomDocumentAndMetaDateValue(entityPM.Id, tenant,itemPM.DocumentsMetaDataTypeCode);
            if(customsDocumentMetaDataValue != null) 
               customsDocumentMetaDataValueRepository.Remove(customsDocumentMetaDataValue);
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        protected UniFileVerM MyUniFileVerM { get; set; }
        protected string MetaDataVersionValue { get; set; }
        
        public CustomDocumentsFilingParams MyCustomDocumentsFilingParams { get; set; }

        public DocumentsFilingMetaDataValuePM GetDocumentsFilingMetaDataValueByFilingIdAndCode(string documentsFilingId, string code, string type = null)
        {
            DocumentsFilingMetaDataValuePM MyDocumentMetaDataValues = null;
            if (string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(code))
            {
                var documentTypeMetaDataRepo = new DocumentsMetaDataTypeRepository(tenant);
                //                DocumentsMetaDataType myDocumentsMetaDataType = documentTypeMetaDataRepo.GetSingleDocumentsMetaDataTypeByCode(code, tenant);
                DocumentsMetaDataType myDocumentsMetaDataType = documentTypeMetaDataRepo.GetSingleDocumentsMetaDataTypeByCustomsMetaDataCode(code, tenant);
                if (myDocumentsMetaDataType != null) type = myDocumentsMetaDataType.Id;
            }
            
            if (!string.IsNullOrEmpty(type))
            {
                var documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                MyDocumentMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTypeTenant(documentsFilingId, type, tenant);
            }
            return MyDocumentMetaDataValues;
        }

    }
    public class UniFileVerM
    {
        public string COMID { get; set; }

        public int VERSION { get; set; }

        public string MD5HASH { get; set; }

        public string EXTENSION { get; set; }

    }
    public class CustomDocumentsFilingParams
    {
        public bool IsCourier { get; set; }
        public string MainInterfaceCode { get; set; }

        
    }
}