using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity.ModelConfiguration.Conventions;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using Logitude.Customs.Data.EntityMapping; 
using Logitude.Customs.Data; 
using Logitude.Customs.Data.EntityPOCOs;

namespace Logitude.Customs.Data
{
   public partial class CustomContext: DbContextBase, ICustomContext
    {
        public CustomContext()
        {
            Database.SetInitializer<CustomContext>(null);            
        }

        public CustomContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<CustomContext>(null);
        }

        public static ICustomContext GetContext(int tenant)
        {           
            GlobalDB currentDb;
			currentDb = GlobalDbHelper.GetGlobalDB(tenant);
			string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            CustomContext context = new CustomContext(connection);
            return context;
        }
		public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return Simplog.Server.Infrastructure.LogitudeDBSchema.LOGITUDE_MAIN; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
		    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
                config.Workarounds.IgnoreSchemaName = true;
                
            }

            Database.SetInitializer<CustomContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			
            modelBuilder.Configurations.Add(new AcceptanceStatusMap());
	
            modelBuilder.Configurations.Add(new AccumalationStateMap());
	
            modelBuilder.Configurations.Add(new ActionCodeMap());
	
            modelBuilder.Configurations.Add(new AddressContactStateMap());
	
            modelBuilder.Configurations.Add(new AddressPurposeMap());
	
            modelBuilder.Configurations.Add(new AgentTalkBackTypeMap());
	
            modelBuilder.Configurations.Add(new AmedmentTypeMap());
	
            modelBuilder.Configurations.Add(new AmendCancellRequestInitiatorMap());
	
            modelBuilder.Configurations.Add(new AmendmentFieldReasonTypeMap());
	
            modelBuilder.Configurations.Add(new AmendmentFieldStatusTypeMap());
	
            modelBuilder.Configurations.Add(new AmendmentRequestStatusMap());
	
            modelBuilder.Configurations.Add(new AmendmentStatusMap());
	
            modelBuilder.Configurations.Add(new AmendmentTypeMap());
	
            modelBuilder.Configurations.Add(new AmendRequestRejectReasonTypeMap());
	
            modelBuilder.Configurations.Add(new AmountTypeMap());
	
            modelBuilder.Configurations.Add(new ApprovedProfessionMap());
	
            modelBuilder.Configurations.Add(new AssigneeNotificationTypeMap());
	
            modelBuilder.Configurations.Add(new AttachmentTypeMap());
	
            modelBuilder.Configurations.Add(new AuthorityMap());
	
            modelBuilder.Configurations.Add(new AuthorizedSignerPermitMap());
	
            modelBuilder.Configurations.Add(new AutonomyRegionTypeMap());
	
            modelBuilder.Configurations.Add(new AutonomyTypeMap());
	
            modelBuilder.Configurations.Add(new BankMap());
	
            modelBuilder.Configurations.Add(new BuyerRoleTypeMap());
	
            modelBuilder.Configurations.Add(new CancellationReasonRequestTypeMap());
	
            modelBuilder.Configurations.Add(new CancellationRequestStatusMap());
	
            modelBuilder.Configurations.Add(new CancelRequestRejectReasonTypeMap());
	
            modelBuilder.Configurations.Add(new CarEngineVolumeMap());
	
            modelBuilder.Configurations.Add(new CargoIdentifireTypeMap());
	
            modelBuilder.Configurations.Add(new CargoIdentityQualifierMap());
	
            modelBuilder.Configurations.Add(new CargoSealMap());
	
            modelBuilder.Configurations.Add(new CargoSealIdentifierMap());
	
            modelBuilder.Configurations.Add(new CargoSplitRequestStatusMap());
	
            modelBuilder.Configurations.Add(new CargoStatusMap());
	
            modelBuilder.Configurations.Add(new CargoTypeMap());
	
            modelBuilder.Configurations.Add(new CarWeightMap());
	
            modelBuilder.Configurations.Add(new CB_CustomsItemMap());
	
            modelBuilder.Configurations.Add(new CB_CustomsItemDetailsHistoryMap());
	
            modelBuilder.Configurations.Add(new CB_PropertiesDetailsHistoryMap());
	
            modelBuilder.Configurations.Add(new CertificateExemptionTypeMap());
	
            modelBuilder.Configurations.Add(new CertificateOfOriginMap());
	
            modelBuilder.Configurations.Add(new CertificateOfOriginInvoiceMap());
	
            modelBuilder.Configurations.Add(new CertificateOfOriginItemMap());
	
            modelBuilder.Configurations.Add(new CertificateOfOriginStatusCodeEnumMap());
	
            modelBuilder.Configurations.Add(new CertificateOfOriginTypeCodeEnumMap());
	
            modelBuilder.Configurations.Add(new CertificatesStatusMap());
	
            modelBuilder.Configurations.Add(new ChangeTypeMap());
	
            modelBuilder.Configurations.Add(new CheckEntityTypeMap());
	
            modelBuilder.Configurations.Add(new CheckEssenceLookupMap());
	
            modelBuilder.Configurations.Add(new CheckQueueTypeMap());
	
            modelBuilder.Configurations.Add(new CheckRepresentativeTypeMap());
	
            modelBuilder.Configurations.Add(new CheckTypeLookupMap());
	
            modelBuilder.Configurations.Add(new CityMap());
	
            modelBuilder.Configurations.Add(new ClaimMap());
	
            modelBuilder.Configurations.Add(new ClaimEntityMap());
	
            modelBuilder.Configurations.Add(new ClaimExplanationCodeMap());
	
            modelBuilder.Configurations.Add(new ClaimImporterDeclarsP3LoiMap());
	
            modelBuilder.Configurations.Add(new ClaimImporterDeclarsPage3Map());
	
            modelBuilder.Configurations.Add(new ClaimImporterDeclarsPage3AMap());
	
            modelBuilder.Configurations.Add(new ClaimImporterDeclarsPage3BMap());
	
            modelBuilder.Configurations.Add(new ClaimReasonTypeMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesAmountMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesReasonMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesRefundMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesSeizureMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntityMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntsExpDeclarMap());
	
            modelBuilder.Configurations.Add(new ClaimsRelatedEntsReasonsExpMap());
	
            modelBuilder.Configurations.Add(new ClassificationTypeMap());
	
            modelBuilder.Configurations.Add(new ClientMap());
	
            modelBuilder.Configurations.Add(new ClientAddressMap());
	
            modelBuilder.Configurations.Add(new ClientDrivingLicenseMap());
	
            modelBuilder.Configurations.Add(new ClientDrivingLicenseTypeMap());
	
            modelBuilder.Configurations.Add(new ClientIndicationMap());
	
            modelBuilder.Configurations.Add(new ClientItemMap());
	
            modelBuilder.Configurations.Add(new ClientsAddressCommTypeMap());
	
            modelBuilder.Configurations.Add(new ClientsPoaMap());
	
            modelBuilder.Configurations.Add(new ClientsTapagMap());
	
            modelBuilder.Configurations.Add(new ClosedTableStatusMap());
	
            modelBuilder.Configurations.Add(new CollateralAnswerStatusMap());
	
            modelBuilder.Configurations.Add(new CollateralAnswerTypeMap());
	
            modelBuilder.Configurations.Add(new CollateralRequestStatusMap());
	
            modelBuilder.Configurations.Add(new CollateralsRequestFileCondMap());
	
            modelBuilder.Configurations.Add(new CollateralTypeMap());
	
            modelBuilder.Configurations.Add(new CommercialSaleMap());
	
            modelBuilder.Configurations.Add(new CommunicationTypeMap());
	
            modelBuilder.Configurations.Add(new ComputationMethodMap());
	
            modelBuilder.Configurations.Add(new ConditionalExemptionTypeMap());
	
            modelBuilder.Configurations.Add(new ConfirmationNumberTokenLogMap());
	
            modelBuilder.Configurations.Add(new ConfirmationTypeMap());
	
            modelBuilder.Configurations.Add(new ConsignmentMap());
	
            modelBuilder.Configurations.Add(new ConsignmentInternalTransitionMap());
	
            modelBuilder.Configurations.Add(new ConsignmentPackageMap());
	
            modelBuilder.Configurations.Add(new ConsignmentPackDangerMap());
	
            modelBuilder.Configurations.Add(new ConstraintApprovalDecisionMap());
	
            modelBuilder.Configurations.Add(new ConstraintProcessTypeMap());
	
            modelBuilder.Configurations.Add(new ConstraintStatusMap());
	
            modelBuilder.Configurations.Add(new ConstraintTypeMap());
	
            modelBuilder.Configurations.Add(new ContactRoleTypeMap());
	
            modelBuilder.Configurations.Add(new ContainerizationMap());
	
            modelBuilder.Configurations.Add(new ContainerizationHataraStatusMap());
	
            modelBuilder.Configurations.Add(new ContainerizationStatusCodeMap());
	
            modelBuilder.Configurations.Add(new ContainerTypeMap());
	
            modelBuilder.Configurations.Add(new ContinuousMessagesTypeCodeMap());
	
            modelBuilder.Configurations.Add(new ContinuousRequestTypeMap());
	
            modelBuilder.Configurations.Add(new ConverterTypeMap());
	
            modelBuilder.Configurations.Add(new CoolingReportingMethodMap());
	
            modelBuilder.Configurations.Add(new CountryCurrencyMap());
	
            modelBuilder.Configurations.Add(new CountryGroupMap());
	
            modelBuilder.Configurations.Add(new CourierCustomStatusMap());
	
            modelBuilder.Configurations.Add(new CourierDeclarationMap());
	
            modelBuilder.Configurations.Add(new CourierDeclarationStatusMap());
	
            modelBuilder.Configurations.Add(new CourierHawbFromExcelMap());
	
            modelBuilder.Configurations.Add(new CourierManifestStatusMap());
	
            modelBuilder.Configurations.Add(new CourierMasterMap());
	
            modelBuilder.Configurations.Add(new CourierPaymentStatusMap());
	
            modelBuilder.Configurations.Add(new CourierPendingReasonMap());
	
            modelBuilder.Configurations.Add(new CourierStatusMap());
	
            modelBuilder.Configurations.Add(new CouriersVatMap());
	
            modelBuilder.Configurations.Add(new CourtInstanceMap());
	
            modelBuilder.Configurations.Add(new CurrencyTypeMap());
	
            modelBuilder.Configurations.Add(new CurrencyTypeTenantMap());
	
            modelBuilder.Configurations.Add(new CustomBankMap());
	
            modelBuilder.Configurations.Add(new CustomBanksCardMap());
	
            modelBuilder.Configurations.Add(new CustomDocumentTypeMap());
	
            modelBuilder.Configurations.Add(new CustomDocumentTypeMetaDataMap());
	
            modelBuilder.Configurations.Add(new CustomDocumentTypeTenantMap());
	
            modelBuilder.Configurations.Add(new CustomerActivityTypeMap());
	
            modelBuilder.Configurations.Add(new CustomerClassificationTypeMap());
	
            modelBuilder.Configurations.Add(new CustomerIdentificationTypeMap());
	
            modelBuilder.Configurations.Add(new CustomerIdentifyTypeMap());
	
            modelBuilder.Configurations.Add(new CustomerIndicationTypeMap());
	
            modelBuilder.Configurations.Add(new CustomerRoleTypeMap());
	
            modelBuilder.Configurations.Add(new CustomerTypeGeneralMap());
	
            modelBuilder.Configurations.Add(new CustomMetaDataTypeMap());
	
            modelBuilder.Configurations.Add(new CustomsAddressTypeMap());
	
            modelBuilder.Configurations.Add(new CustomsAirlineMap());
	
            modelBuilder.Configurations.Add(new CustomsAutonomyKeywordMap());
	
            modelBuilder.Configurations.Add(new CustomsBookMap());
	
            modelBuilder.Configurations.Add(new CustomsBookTypeMap());
	
            modelBuilder.Configurations.Add(new CustomsBranchMap());
	
            modelBuilder.Configurations.Add(new CustomsClosedTableMap());
	
            modelBuilder.Configurations.Add(new CustomsCollateralMap());
	
            modelBuilder.Configurations.Add(new CustomsCollateralsAnswerMap());
	
            modelBuilder.Configurations.Add(new CustomsCollateralsConditionMap());
	
            modelBuilder.Configurations.Add(new CustomsCountryMap());
	
            modelBuilder.Configurations.Add(new CustomsCountryTenantMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentMetaDataValueMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentPointerMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentsDefinitionMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentStatusTypeMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentsTicketMap());
	
            modelBuilder.Configurations.Add(new CustomsDocumentUploadMap());
	
            modelBuilder.Configurations.Add(new CustomsEntityStatusMap());
	
            modelBuilder.Configurations.Add(new CustomsEnvironmentSettingMap());
	
            modelBuilder.Configurations.Add(new CustomsEnvoirmentTypeMap());
	
            modelBuilder.Configurations.Add(new CustomsExchangeRateMap());
	
            modelBuilder.Configurations.Add(new CustomsGeneralMap());
	
            modelBuilder.Configurations.Add(new CustomsHouseTypeMap());
	
            modelBuilder.Configurations.Add(new CustomsHouseTypeAdditionalMap());
	
            modelBuilder.Configurations.Add(new CustomsHouseTypeTenantMap());
	
            modelBuilder.Configurations.Add(new CustomsInsuranceCompanyMap());
	
            modelBuilder.Configurations.Add(new CustomsItemMap());
	
            modelBuilder.Configurations.Add(new CustomsItemCategoryMap());
	
            modelBuilder.Configurations.Add(new CustomsItemDetailsHistoryMap());
	
            modelBuilder.Configurations.Add(new CustomsItemHierarchicLocationMap());
	
            modelBuilder.Configurations.Add(new CustomsPartnerFtpMap());
	
            modelBuilder.Configurations.Add(new CustomsPartnersItemMap());
	
            modelBuilder.Configurations.Add(new CustomsPaymentTermMap());
	
            modelBuilder.Configurations.Add(new CustomsRequestsSheetMap());
	
            modelBuilder.Configurations.Add(new CustomsRequestsSheetStatusMap());
	
            modelBuilder.Configurations.Add(new CustomsRequiredFieldMap());
	
            modelBuilder.Configurations.Add(new CustomsSettingMap());
	
            modelBuilder.Configurations.Add(new CustomsShipMap());
	
            modelBuilder.Configurations.Add(new CustomsTransportModeMap());
	
            modelBuilder.Configurations.Add(new CustomsVendorMap());
	
            modelBuilder.Configurations.Add(new CustomsVerificationStatusTypeMap());
	
            modelBuilder.Configurations.Add(new DangerousGoodsPackingReqMap());
	
            modelBuilder.Configurations.Add(new DBMigrationMap());
	
            modelBuilder.Configurations.Add(new DBMigrationLineMap());
	
            modelBuilder.Configurations.Add(new DebtNotificationTypeMap());
	
            modelBuilder.Configurations.Add(new DecCargoSplitCargoIdentifierMap());
	
            modelBuilder.Configurations.Add(new DecCargoSplitConMap());
	
            modelBuilder.Configurations.Add(new DecCargoSplitConsItemMap());
	
            modelBuilder.Configurations.Add(new DecCargoSplitConsPackDetMap());
	
            modelBuilder.Configurations.Add(new DecDangersContactMap());
	
            modelBuilder.Configurations.Add(new DecisionTypeMap());
	
            modelBuilder.Configurations.Add(new DeclarationMap());
	
            modelBuilder.Configurations.Add(new DeclarationCargoSplitMap());
	
            modelBuilder.Configurations.Add(new DeclarationCasualDetailsMap());
	
            modelBuilder.Configurations.Add(new DeclarationConsAcceptanceMap());
	
            modelBuilder.Configurations.Add(new DeclarationConstraintMap());
	
            modelBuilder.Configurations.Add(new DeclarationCounterMap());
	
            modelBuilder.Configurations.Add(new DeclarationCourierStatusMap());
	
            modelBuilder.Configurations.Add(new DeclarationErrorMappingMap());
	
            modelBuilder.Configurations.Add(new DeclarationExportRecipientMap());
	
            modelBuilder.Configurations.Add(new DeclarationFollowUpMap());
	
            modelBuilder.Configurations.Add(new DeclarationMamanSpecialActionMap());
	
            modelBuilder.Configurations.Add(new DeclarationPaymentMap());
	
            modelBuilder.Configurations.Add(new DeclarationPaymentMethodMap());
	
            modelBuilder.Configurations.Add(new DeclarationPaymentProtestMap());
	
            modelBuilder.Configurations.Add(new DeclarationPendingMap());
	
            modelBuilder.Configurations.Add(new DeclarationReferantDataMap());
	
            modelBuilder.Configurations.Add(new DeclarationStatementTypeMap());
	
            modelBuilder.Configurations.Add(new DeclarationStatusMap());
	
            modelBuilder.Configurations.Add(new DeclarationStatusTypeMap());
	
            modelBuilder.Configurations.Add(new DeclarationTaxMap());
	
            modelBuilder.Configurations.Add(new DefaultTypeMap());
	
            modelBuilder.Configurations.Add(new DefaultValueMap());
	
            modelBuilder.Configurations.Add(new DeficitMap());
	
            modelBuilder.Configurations.Add(new DeficitConnFileParagraphTypeMap());
	
            modelBuilder.Configurations.Add(new DeficitDecisionMap());
	
            modelBuilder.Configurations.Add(new DeliverySiteTypeMap());
	
            modelBuilder.Configurations.Add(new DeliveryTypeMap());
	
            modelBuilder.Configurations.Add(new DemanderTypeMap());
	
            modelBuilder.Configurations.Add(new DepositMap());
	
            modelBuilder.Configurations.Add(new DepositConditionMap());
	
            modelBuilder.Configurations.Add(new DepositCustomerActivityMap());
	
            modelBuilder.Configurations.Add(new DepositEssenceTypeMap());
	
            modelBuilder.Configurations.Add(new DepositFileTypeMap());
	
            modelBuilder.Configurations.Add(new DocumentRejectTypeMap());
	
            modelBuilder.Configurations.Add(new DocumentTypeCustomsDataMap());
	
            modelBuilder.Configurations.Add(new EntitlementTypeMap());
	
            modelBuilder.Configurations.Add(new EntityTypeLookupMap());
	
            modelBuilder.Configurations.Add(new EntryExitTypeMap());
	
            modelBuilder.Configurations.Add(new ExceptionReasonMap());
	
            modelBuilder.Configurations.Add(new ExportDeclarationClosingDataMap());
	
            modelBuilder.Configurations.Add(new ExportDeliveryDocumentMessageMap());
	
            modelBuilder.Configurations.Add(new ExporterRoleTypeMap());
	
            modelBuilder.Configurations.Add(new ExportLogisticPermitActionMap());
	
            modelBuilder.Configurations.Add(new ExportReferenceMap());
	
            modelBuilder.Configurations.Add(new ExportStorageMap());
	
            modelBuilder.Configurations.Add(new ExternalFieldMappingMap());
	
            modelBuilder.Configurations.Add(new FacilitationTypeMap());
	
            modelBuilder.Configurations.Add(new FaultInspectionTypeMap());
	
            modelBuilder.Configurations.Add(new FclLclCodeMap());
	
            modelBuilder.Configurations.Add(new FreightPaymentMethodMap());
	
            modelBuilder.Configurations.Add(new FuelTypeMap());
	
            modelBuilder.Configurations.Add(new FullnessCodeMap());
	
            modelBuilder.Configurations.Add(new GatepassRequestMap());
	
            modelBuilder.Configurations.Add(new GatepassReturnCodeMap());
	
            modelBuilder.Configurations.Add(new GenderMap());
	
            modelBuilder.Configurations.Add(new GovernmentProcedureTypeMap());
	
            modelBuilder.Configurations.Add(new GovernmentProcTypeTenantMap());
	
            modelBuilder.Configurations.Add(new GuaranteeMap());
	
            modelBuilder.Configurations.Add(new GuaranteeCertificateTypeMap());
	
            modelBuilder.Configurations.Add(new GuaranteeConditionMap());
	
            modelBuilder.Configurations.Add(new GuaranteeCustomerActivityMap());
	
            modelBuilder.Configurations.Add(new HandingCodeMap());
	
            modelBuilder.Configurations.Add(new HazardousSubstanceMap());
	
            modelBuilder.Configurations.Add(new ImporterDeclarationTypeMap());
	
            modelBuilder.Configurations.Add(new ImporterDespositionMap());
	
            modelBuilder.Configurations.Add(new ImporterPeriodicDeclarStatusMap());
	
            modelBuilder.Configurations.Add(new ImporterTypeForClaimMap());
	
            modelBuilder.Configurations.Add(new InceptionCodeMap());
	
            modelBuilder.Configurations.Add(new IncotemrsFileValidationMap());
	
            modelBuilder.Configurations.Add(new InterConditionsRelationshipMap());
	
            modelBuilder.Configurations.Add(new InterfaceManagementMap());
	
            modelBuilder.Configurations.Add(new InterfaceSendOptionMap());
	
            modelBuilder.Configurations.Add(new InterfaceTenantDefinitionMap());
	
            modelBuilder.Configurations.Add(new InternalBorderSiteTypeMap());
	
            modelBuilder.Configurations.Add(new InternationalSiteMap());
	
            modelBuilder.Configurations.Add(new InternationalSiteTenantMap());
	
            modelBuilder.Configurations.Add(new InvoiceTypeMap());
	
            modelBuilder.Configurations.Add(new ItemGovernmentProcedureTypeMap());
	
            modelBuilder.Configurations.Add(new LastReleaseFromWarehouseMap());
	
            modelBuilder.Configurations.Add(new LeadDocumentExceptionTypeMap());
	
            modelBuilder.Configurations.Add(new LeadDocumentTypeMap());
	
            modelBuilder.Configurations.Add(new LevyTrustMap());
	
            modelBuilder.Configurations.Add(new LoadingSiteTypeMap());
	
            modelBuilder.Configurations.Add(new LogisticActionRequestMap());
	
            modelBuilder.Configurations.Add(new LogisticActionRequestTypeMap());
	
            modelBuilder.Configurations.Add(new LogisticActionResponseReqSMap());
	
            modelBuilder.Configurations.Add(new LogisticPermitMap());
	
            modelBuilder.Configurations.Add(new LogisticsReferenceTypeMap());
	
            modelBuilder.Configurations.Add(new MamanSpecialActionMap());
	
            modelBuilder.Configurations.Add(new MamanSpecialActionStatusMap());
	
            modelBuilder.Configurations.Add(new MamanStatusMap());
	
            modelBuilder.Configurations.Add(new ManifestCargoStatusMap());
	
            modelBuilder.Configurations.Add(new MAWBTypeMap());
	
            modelBuilder.Configurations.Add(new MeasureQualifierMap());
	
            modelBuilder.Configurations.Add(new MeasurmentUnitMap());
	
            modelBuilder.Configurations.Add(new ModificationAndDiscountTypeMap());
	
            modelBuilder.Configurations.Add(new MorningMessageTypeMap());
	
            modelBuilder.Configurations.Add(new NbcDeclarationTypeMap());
	
            modelBuilder.Configurations.Add(new NDMessageActionCodeMap());
	
            modelBuilder.Configurations.Add(new NotificationMap());
	
            modelBuilder.Configurations.Add(new NotificationDefinitionMap());
	
            modelBuilder.Configurations.Add(new NotificationReplyMap());
	
            modelBuilder.Configurations.Add(new NotificationTenantDefinitionMap());
	
            modelBuilder.Configurations.Add(new NotificationTypeMap());
	
            modelBuilder.Configurations.Add(new OcrDocumentMap());
	
            modelBuilder.Configurations.Add(new OcrStatusMap());
	
            modelBuilder.Configurations.Add(new OrganizationUnitTypeMap());
	
            modelBuilder.Configurations.Add(new OriginCriterionMap());
	
            modelBuilder.Configurations.Add(new PackageMeasureQualifierMap());
	
            modelBuilder.Configurations.Add(new PackingTypeMap());
	
            modelBuilder.Configurations.Add(new ParagraphTypeMap());
	
            modelBuilder.Configurations.Add(new PartyRelationshipTypeMap());
	
            modelBuilder.Configurations.Add(new PassportTypeMap());
	
            modelBuilder.Configurations.Add(new PayerActivityTypeMap());
	
            modelBuilder.Configurations.Add(new PayerTypeMap());
	
            modelBuilder.Configurations.Add(new PaymentMethodStatusMap());
	
            modelBuilder.Configurations.Add(new PaymentMethodTypeMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderConnectionTableMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderLineMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderMethodMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderProtestReasonMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderStatusMap());
	
            modelBuilder.Configurations.Add(new PaymentOrderTypeMap());
	
            modelBuilder.Configurations.Add(new PaymentProcessMap());
	
            modelBuilder.Configurations.Add(new PaymentProtestTypeMap());
	
            modelBuilder.Configurations.Add(new PaymentTypeMap());
	
            modelBuilder.Configurations.Add(new PendingByKeywordMap());
	
            modelBuilder.Configurations.Add(new PendingErrorPlaceMap());
	
            modelBuilder.Configurations.Add(new PerYearFrequencyMap());
	
            modelBuilder.Configurations.Add(new PhysicalCheckMap());
	
            modelBuilder.Configurations.Add(new PhysicalCheckCodeMap());
	
            modelBuilder.Configurations.Add(new PhysicalCheckOperationMap());
	
            modelBuilder.Configurations.Add(new PhysicalCheckSearchResultTypeMap());
	
            modelBuilder.Configurations.Add(new PhysicalCheckStatusMessageMap());
	
            modelBuilder.Configurations.Add(new PoaAuthorizationTypeLookupMap());
	
            modelBuilder.Configurations.Add(new PoaStatusTypeLookUpMap());
	
            modelBuilder.Configurations.Add(new PointerLevelMap());
	
            modelBuilder.Configurations.Add(new ProceduralFaultMap());
	
            modelBuilder.Configurations.Add(new ProceduralFaultInProcessTypeMap());
	
            modelBuilder.Configurations.Add(new ProceduralFaultInSourceTypeMap());
	
            modelBuilder.Configurations.Add(new ProceduralFaultsConnEntityMap());
	
            modelBuilder.Configurations.Add(new ProceduralFaultStatusMap());
	
            modelBuilder.Configurations.Add(new ProceduralFaultTypeMap());
	
            modelBuilder.Configurations.Add(new ProcessingReasonMap());
	
            modelBuilder.Configurations.Add(new ProductIdentificationTypeMap());
	
            modelBuilder.Configurations.Add(new ProductNameTypeMap());
	
            modelBuilder.Configurations.Add(new PropertiesDetailsHistoryMap());
	
            modelBuilder.Configurations.Add(new QuotaComputationBasisMap());
	
            modelBuilder.Configurations.Add(new QuotaIncrementMap());
	
            modelBuilder.Configurations.Add(new RansomViolationTypeMap());
	
            modelBuilder.Configurations.Add(new ReferantExceptionMap());
	
            modelBuilder.Configurations.Add(new ReferantTeamMap());
	
            modelBuilder.Configurations.Add(new ReferenceInputTypeMap());
	
            modelBuilder.Configurations.Add(new ReferenceStatusMap());
	
            modelBuilder.Configurations.Add(new RefundCustomerActivityTypeMap());
	
            modelBuilder.Configurations.Add(new RegisteredWarehouseSiteTypeMap());
	
            modelBuilder.Configurations.Add(new RegularityPublicationMap());
	
            modelBuilder.Configurations.Add(new RegularitySourceMap());
	
            modelBuilder.Configurations.Add(new ReleaseMessageTypeMap());
	
            modelBuilder.Configurations.Add(new RenewalMethodMap());
	
            modelBuilder.Configurations.Add(new RequestReasonCodeEnumMap());
	
            modelBuilder.Configurations.Add(new RequestStatusMap());
	
            modelBuilder.Configurations.Add(new RequestToAdvanceAQueueMap());
	
            modelBuilder.Configurations.Add(new RequestTypeMap());
	
            modelBuilder.Configurations.Add(new RequiredGuaranteeTypeMap());
	
            modelBuilder.Configurations.Add(new ReturnConditionMap());
	
            modelBuilder.Configurations.Add(new SalesTaxExemptionTypeMap());
	
            modelBuilder.Configurations.Add(new SchedulerParamMap());
	
            modelBuilder.Configurations.Add(new SealCompletenesMap());
	
            modelBuilder.Configurations.Add(new SealTypeMap());
	
            modelBuilder.Configurations.Add(new SealUpdateReasonTypeMap());
	
            modelBuilder.Configurations.Add(new SecurityClearenceTypeCodeMap());
	
            modelBuilder.Configurations.Add(new SeizureFactorTypeMap());
	
            modelBuilder.Configurations.Add(new SeizureMethodTypeMap());
	
            modelBuilder.Configurations.Add(new ServersNameMap());
	
            modelBuilder.Configurations.Add(new SignatureTypeMap());
	
            modelBuilder.Configurations.Add(new SignStationMap());
	
            modelBuilder.Configurations.Add(new SiteLookupMap());
	
            modelBuilder.Configurations.Add(new SiteTypeMap());
	
            modelBuilder.Configurations.Add(new SpecialActionDescriptionTypeMap());
	
            modelBuilder.Configurations.Add(new SpecializationTypeMap());
	
            modelBuilder.Configurations.Add(new SplitOrMergeReasonMap());
	
            modelBuilder.Configurations.Add(new StatusCodeMap());
	
            modelBuilder.Configurations.Add(new StatusFieldTypeMap());
	
            modelBuilder.Configurations.Add(new StorageMessageTypeMap());
	
            modelBuilder.Configurations.Add(new StorageStatusMap());
	
            modelBuilder.Configurations.Add(new StorageStatusTableMap());
	
            modelBuilder.Configurations.Add(new StuffingSiteTypeMap());
	
            modelBuilder.Configurations.Add(new SubCountryMap());
	
            modelBuilder.Configurations.Add(new SuppInvoiceItemsAbachStatementMap());
	
            modelBuilder.Configurations.Add(new SupplierInvioceExportDefaultMap());
	
            modelBuilder.Configurations.Add(new SupplierInvioceItemCertificatMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceFreightAmountMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemModVehicleMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemProcesTypeMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsConDeclarMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsDescriptMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsLevyMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsModMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsPriceMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsProdIdentMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsSerialNumMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemsTaxMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemVehicleMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemVehicleAddMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceItemVehicleModMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceModificationMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoicePaymentMap());
	
            modelBuilder.Configurations.Add(new SupplierInvoiceUCRMap());
	
            modelBuilder.Configurations.Add(new SupplierPartyTypeMap());
	
            modelBuilder.Configurations.Add(new TapagMap());
	
            modelBuilder.Configurations.Add(new TapagConnectionTableMap());
	
            modelBuilder.Configurations.Add(new TapagTypeMap());
	
            modelBuilder.Configurations.Add(new TarifRelatedToQuotaMap());
	
            modelBuilder.Configurations.Add(new TermsOfSaleTypeMap());
	
            modelBuilder.Configurations.Add(new TPGFileTypeMap());
	
            modelBuilder.Configurations.Add(new TradeAgreementMap());
	
            modelBuilder.Configurations.Add(new TradeAgreementProtocolMap());
	
            modelBuilder.Configurations.Add(new TradeLevyExamptTypeMap());
	
            modelBuilder.Configurations.Add(new TradeLevyStatusMap());
	
            modelBuilder.Configurations.Add(new TransactionNatureTypeMap());
	
            modelBuilder.Configurations.Add(new TransferCargoMethodTypeMap());
	
            modelBuilder.Configurations.Add(new TransportMeansTypeMap());
	
            modelBuilder.Configurations.Add(new TreatmentWayMap());
	
            modelBuilder.Configurations.Add(new UIMessageMap());
	
            modelBuilder.Configurations.Add(new UIMessageAdditionalMap());
	
            modelBuilder.Configurations.Add(new UIMessageTenantMap());
	
            modelBuilder.Configurations.Add(new UnloadingSiteTypeMap());
	
            modelBuilder.Configurations.Add(new UpdateCodeMap());
	
            modelBuilder.Configurations.Add(new ValidCustomsItemMap());
	
            modelBuilder.Configurations.Add(new VehicleMap());
	
            modelBuilder.Configurations.Add(new VehicleManufacturerMap());
	
            modelBuilder.Configurations.Add(new VehicleOwnerMap());
	
            modelBuilder.Configurations.Add(new VehiclePoolTypeMap());
	
            modelBuilder.Configurations.Add(new VehiclePriceListTypeMap());
	
            modelBuilder.Configurations.Add(new VehicleReductionTypeMap());
	
            modelBuilder.Configurations.Add(new VehicleSafeAccessoryInstlTypeMap());
	
            modelBuilder.Configurations.Add(new VehicleSafetyAccessoryMap());
	
            modelBuilder.Configurations.Add(new VehicleSafetyAccessoryTypeMap());
	
            modelBuilder.Configurations.Add(new VehicleStatusMap());
	
            modelBuilder.Configurations.Add(new VehicleTecnologyTypeMap());
	
            modelBuilder.Configurations.Add(new VehicleTypeMap());
	
            modelBuilder.Configurations.Add(new VendorCommissionMap());
	
            modelBuilder.Configurations.Add(new VendorCommunicationMap());
	
            modelBuilder.Configurations.Add(new VendorCurrencyMap());
	
            modelBuilder.Configurations.Add(new VendorStatusMap());
	
            modelBuilder.Configurations.Add(new VendorTransactionTypeMap());
	
            modelBuilder.Configurations.Add(new VendorTypeMap());
	
				
			modelBuilder.Entity<CB_PropertiesDetailsHistory>().Property(x => x.VatDiscountRate).HasPrecision(2, 2);
				
			modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SaleAmountAfter).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SaleAmountClaim).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SaleAmountBefore).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.InventoryAmount).HasPrecision(12, 5);
				
			modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SoldGoodsAmount).HasPrecision(12, 5);
				
			modelBuilder.Entity<ClaimsRelatedEntitiesAmount>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimsRelatedEntitiesRefund>().Property(x => x.RefundQuntity).HasPrecision(16, 6);
				
			modelBuilder.Entity<ClaimsRelatedEntitiesSeizure>().Property(x => x.SeizureAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimsRelatedEntity>().Property(x => x.ClaimAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimsRelatedEntity>().Property(x => x.DeclarationVersion).HasPrecision(5, 3);
				
			modelBuilder.Entity<ClaimsRelatedEntity>().Property(x => x.DepositingAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClaimsRelatedEntity>().Property(x => x.RefundAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ClientAddress>().Property(x => x.LocalApartment).HasPrecision(4, 0);
				
			modelBuilder.Entity<CollateralsRequestFileCond>().Property(x => x.RequestedAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<ConsignmentPackage>().Property(x => x.GrossMassMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<CourierMaster>().Property(x => x.GrossMassMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<CustomsCollateralsAnswer>().Property(x => x.AllocatedAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<CustomsCollateralsAnswer>().Property(x => x.RequestFileAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<CustomsCollateralsCondition>().Property(x => x.RequestedAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<CustomsExchangeRate>().Property(x => x.ExchangeRate).HasPrecision(15, 10);
				
			modelBuilder.Entity<DBMigration>().Property(x => x.MajorVersion).HasPrecision(5, 2);
				
			modelBuilder.Entity<DecCargoSplitConsItem>().Property(x => x.GrossMassMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<DecCargoSplitConsPackDet>().Property(x => x.GrossMassMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.LoadingFactor).HasPrecision(18, 10);
				
			modelBuilder.Entity<Declaration>().Property(x => x.DealValue).HasPrecision(16, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.CIFValue).HasPrecision(16, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.TotalTax).HasPrecision(16, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.PlatformFee).HasPrecision(18, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.DealValueWithoutFactor).HasPrecision(16, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.DealValueWithFactor).HasPrecision(16, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.FOBValueNIS).HasPrecision(16, 2);
				
			modelBuilder.Entity<Declaration>().Property(x => x.FOBValueDollar).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeclarationConsAcceptance>().Property(x => x.GrossWeight).HasPrecision(11, 3);
				
			modelBuilder.Entity<DeclarationCourierStatus>().Property(x => x.TotalInvoiceAmountInUSD).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeclarationPaymentMethod>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeclarationPaymentProtest>().Property(x => x.GoodsItemLineNumber).HasPrecision(16, 5);
				
			modelBuilder.Entity<DeclarationPaymentProtest>().Property(x => x.AmountInDispute).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeclarationReferantData>().Property(x => x.Weight).HasPrecision(15, 3);
				
			modelBuilder.Entity<DeclarationTax>().Property(x => x.TotalAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeclarationTax>().Property(x => x.DeferredTaxAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeclarationTax>().Property(x => x.TaxBaseAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeficitConnFileParagraphType>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeficitDecision>().Property(x => x.TotalComponentAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeficitDecision>().Property(x => x.TotalEstimatedAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeficitDecision>().Property(x => x.TotalFinancialPenaltyAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeficitDecision>().Property(x => x.TotalInterestAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DeficitDecision>().Property(x => x.TotalLinkingAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<Deposit>().Property(x => x.DepositAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<DepositCondition>().Property(x => x.DepositAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ExportStorage>().Property(x => x.PackageQuantity).HasPrecision(10, 0);
				
			modelBuilder.Entity<ExportStorage>().Property(x => x.GrossMassMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<ExportStorage>().Property(x => x.IsDangerousGoods).HasPrecision(1, 0);
				
			modelBuilder.Entity<GuaranteeCondition>().Property(x => x.GuaranteeAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<LogisticActionRequest>().Property(x => x.Quantity).HasPrecision(10, 2);
				
			modelBuilder.Entity<OcrDocument>().Property(x => x.Score).HasPrecision(4, 2);
				
			modelBuilder.Entity<PaymentOrder>().Property(x => x.TotalSumToPay).HasPrecision(18, 2);
				
			modelBuilder.Entity<PaymentOrder>().Property(x => x.PaymentOrderLeftAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<PaymentOrderLine>().Property(x => x.Amount).HasPrecision(18, 2);
				
			modelBuilder.Entity<PaymentOrderMethod>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<PaymentOrderProtestReason>().Property(x => x.AmountInDispute).HasPrecision(16, 2);
				
			modelBuilder.Entity<ProceduralFault>().Property(x => x.RansomViolationSum).HasPrecision(16, 2);
				
			modelBuilder.Entity<RequiredGuaranteeType>().Property(x => x.GuaranteeAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.InvoiceAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.ActualPayedAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.TotalFreightInFreightCurrency).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.TotalFreightInNIS).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.ExchangeRate).HasPrecision(12, 10);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.InsuranceAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.InsruancePercentage).HasPrecision(7, 4);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.VendorComissionPercentage).HasPrecision(7, 4);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.InvoiceAmountInUSD).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.ItemFOBAmountForeign).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoice>().Property(x => x.ItemFOBAmountNIS).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceFreightAmount>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.ItemPrice).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.NonCustomsItemPrice).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.WholeSaleItemPrice).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.OptionalTamaPercentage).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.StatisticQuantity).HasPrecision(14, 3);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.InvoiceQuantity).HasPrecision(14, 3);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.AdditionalQuantity).HasPrecision(14, 3);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.DeferredCustomsTax).HasPrecision(5, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.DeferredPurchaseTax).HasPrecision(5, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.Weight).HasPrecision(15, 3);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.OcrHeight).HasPrecision(5, 0);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.OcrTop).HasPrecision(5, 0);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.OcrPageNumber).HasPrecision(3, 0);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.ItemFOBAmountForeign).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.ItemFOBAmountNIS).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemModVehicle>().Property(x => x.DeductAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsConDeclar>().Property(x => x.Quantity).HasPrecision(14, 3);
				
			modelBuilder.Entity<SupplierInvoiceItemsMod>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsPrice>().Property(x => x.AdditionalPrice).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TaxRate).HasPrecision(17, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TaxBaseAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TaxAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.DeferedTaxAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.DefinedPerUnitMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.AlternateDefinedPerUnitMeasure).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.DefinedPerUnitQuantity).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.AlternateDefinedPerUnitQuant).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TotalBtlCoverageNIS).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.AlternateRate).HasPrecision(18, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemVehicleAdd>().Property(x => x.ChassisVat).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceItemVehicleMod>().Property(x => x.DeductAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoiceModification>().Property(x => x.Amount).HasPrecision(16, 2);
				
			modelBuilder.Entity<SupplierInvoicePayment>().Property(x => x.PaymentAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<Vehicle>().Property(x => x.GreenIndex).HasPrecision(9, 3);
				
			modelBuilder.Entity<Vehicle>().Property(x => x.VehiclePowerKW).HasPrecision(12, 2);
				
			modelBuilder.Entity<Vehicle>().Property(x => x.VehicleSafetyAccessoryPoints).HasPrecision(4, 2);
				
			modelBuilder.Entity<Vehicle>().Property(x => x.VehicleMaxPowerKW).HasPrecision(7, 2);
				
			modelBuilder.Entity<VendorCommission>().Property(x => x.CommisionPercentage).HasPrecision(7, 4);
						  #region
            modelBuilder.Configurations.Add(new AccountingSystemMap());
            modelBuilder.Configurations.Add(new AccountingSettingMap());
            modelBuilder.Configurations.Add(new Accounts1Map());
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressTypeMap());
            modelBuilder.Configurations.Add(new AdvancedQueryFilterMap());
            modelBuilder.Configurations.Add(new AgentMap());
            modelBuilder.Configurations.Add(new AirlineMap());
            modelBuilder.Configurations.Add(new APInvoiceEntityMap());
            modelBuilder.Configurations.Add(new APInvoiceLineMap());
            modelBuilder.Configurations.Add(new APInvoicePaymentMap());
            modelBuilder.Configurations.Add(new APInvoiceMap());
            modelBuilder.Configurations.Add(new APInvoiceStatuMap());
            modelBuilder.Configurations.Add(new APInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new APInvoiceTypeMap());
            modelBuilder.Configurations.Add(new APPaymentMap());
            modelBuilder.Configurations.Add(new APPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceEntityMap());
            modelBuilder.Configurations.Add(new ARInvoiceLineMap());
            modelBuilder.Configurations.Add(new ARInvoicePaymentMap());
            modelBuilder.Configurations.Add(new ARInvoiceMap());
            modelBuilder.Configurations.Add(new ARInvoiceStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new ARInvoiceTypeMap());
            modelBuilder.Configurations.Add(new AccountingPaymentMethodMap());
            modelBuilder.Configurations.Add(new ARPaymentMap());
            modelBuilder.Configurations.Add(new ARPaymentStatuMap());
            modelBuilder.Configurations.Add(new AWBChargesCodeMap());
            modelBuilder.Configurations.Add(new AWBSpecialHandlingCodeMap());
            modelBuilder.Configurations.Add(new AWBStatuMap());
            //modelBuilder.Configurations.Add(new BlobMap());
            //modelBuilder.Configurations.Add(new BlobTypeMap());
            modelBuilder.Configurations.Add(new BranchMap());
            modelBuilder.Configurations.Add(new CardContactMap());
            modelBuilder.Configurations.Add(new CardMap());
            modelBuilder.Configurations.Add(new CategoryTypeMap());
            modelBuilder.Configurations.Add(new ChargesGroupMap());
            modelBuilder.Configurations.Add(new ChargesTypeMap());
            modelBuilder.Configurations.Add(new CommunicationAttachmentMap());
            modelBuilder.Configurations.Add(new CommunicationLogMap());
            modelBuilder.Configurations.Add(new CommunicationLogTypeMap());
            modelBuilder.Configurations.Add(new CommunicationStatusTypeMap());

            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ContactTenantRoleSetMap());
            modelBuilder.Configurations.Add(new ContactTenantMap());
            modelBuilder.Configurations.Add(new CounterDefinitionMap());
            modelBuilder.Configurations.Add(new CounterLastNumberMap());
            modelBuilder.Configurations.Add(new CounterMap());
            modelBuilder.Configurations.Add(new CounterStatMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new CreditCardTypeMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new CustomAgentMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new CustomPickListMap());
            modelBuilder.Configurations.Add(new CustomTableMap());
            modelBuilder.Configurations.Add(new DataBasePropertyMap());
            modelBuilder.Configurations.Add(new DBIdCounterMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new DescriptionOfGoodMap());
            modelBuilder.Configurations.Add(new DimensionsUnitMap());
            modelBuilder.Configurations.Add(new DirectionMap());
            //modelBuilder.Configurations.Add(new DocPrintCopyMap());
            //modelBuilder.Configurations.Add(new DocPrintMap());
            //modelBuilder.Configurations.Add(new DocTypeMap());
            //modelBuilder.Configurations.Add(new DocumentInMap());
			modelBuilder.Configurations.Add(new DocumentsFilingMap());
            modelBuilder.Configurations.Add(new DocumentOutCopyMap());
            modelBuilder.Configurations.Add(new DocumentOutMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new DocumentTypeCopyMap());
            modelBuilder.Configurations.Add(new DocumentTypeCustomFields1Map());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new DocumentTypeTemplateMap());
            modelBuilder.Configurations.Add(new DueTypeMap());
            modelBuilder.Configurations.Add(new EntityDateMap());
            modelBuilder.Configurations.Add(new EntityLastActivityMap());
            modelBuilder.Configurations.Add(new EntityLastActivityTypeMap());
            modelBuilder.Configurations.Add(new EntityLastUpdateMap());
            modelBuilder.Configurations.Add(new EntityStatuMap());
            modelBuilder.Configurations.Add(new EventTypeMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new FeatureTypeMap());
            modelBuilder.Configurations.Add(new FHLStatuMap());
            modelBuilder.Configurations.Add(new FieldDataTypeMap());
            modelBuilder.Configurations.Add(new FollowUpMap());
            modelBuilder.Configurations.Add(new FormCustomFields1Map());
            modelBuilder.Configurations.Add(new FWBStatuMap());
            modelBuilder.Configurations.Add(new GlobalZoneMap());
            modelBuilder.Configurations.Add(new IATACodeMap());
            modelBuilder.Configurations.Add(new ImageDetailMap());
            modelBuilder.Configurations.Add(new IncotermMap());
            modelBuilder.Configurations.Add(new InsideShipmentPackageMap());
          
            modelBuilder.Configurations.Add(new MarkUpTypeMap());
            modelBuilder.Configurations.Add(new MAWBStackMap());
            modelBuilder.Configurations.Add(new MeasurementMap());
            modelBuilder.Configurations.Add(new MenuButtonGroupMap());
            modelBuilder.Configurations.Add(new MenuButtonMap());
            modelBuilder.Configurations.Add(new MenusTableMap());
            modelBuilder.Configurations.Add(new MenuTypeMap());
            modelBuilder.Configurations.Add(new MoveTypeMap());
            modelBuilder.Configurations.Add(new NextLegMap());
            modelBuilder.Configurations.Add(new ObjectFieldModificationMap());
            modelBuilder.Configurations.Add(new ObjectFieldMap());
            modelBuilder.Configurations.Add(new ObjectFieldValidationMap());
            modelBuilder.Configurations.Add(new ObjectTableHelperControlMap());
            modelBuilder.Configurations.Add(new ObjectTableRuleFieldMap());
            modelBuilder.Configurations.Add(new ObjectTableRuleMap());
            modelBuilder.Configurations.Add(new ObjectTableMap());
            modelBuilder.Configurations.Add(new ObjectTableTabMap());
            modelBuilder.Configurations.Add(new ObjectTableTypeMap());
            modelBuilder.Configurations.Add(new PackageFeatureMap());
            modelBuilder.Configurations.Add(new PackageMap());
            modelBuilder.Configurations.Add(new PackageTypeMap());
            modelBuilder.Configurations.Add(new PartnerTypeMap());
            modelBuilder.Configurations.Add(new PasswordPolicyMap());

            modelBuilder.Configurations.Add(new PaymentTermMap());
            modelBuilder.Configurations.Add(new PermissionTypeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryFromToTypeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryTypeMap());
            modelBuilder.Configurations.Add(new PortMap());
            modelBuilder.Configurations.Add(new PrepaidCollectMap());
            //modelBuilder.Configurations.Add(new PrintTemplateCopyMap());
            //modelBuilder.Configurations.Add(new PrintTemplateCustomFieldMap());
            //modelBuilder.Configurations.Add(new PrintTemplateFormCustomFieldMap());
            //modelBuilder.Configurations.Add(new PrintTemplateOptionMap());
            //modelBuilder.Configurations.Add(new PrintTemplateMap());
            //modelBuilder.Configurations.Add(new PrintTemplateVersionMap());
            modelBuilder.Configurations.Add(new QueryMap());
            modelBuilder.Configurations.Add(new QueryColumnMap());
            modelBuilder.Configurations.Add(new QueryGroupMap());
            modelBuilder.Configurations.Add(new QuoteChargeMap());
            modelBuilder.Configurations.Add(new QuoteCustomerTypeMap());
            modelBuilder.Configurations.Add(new QuotePriceStepMap());
            modelBuilder.Configurations.Add(new QuoteMap());
            modelBuilder.Configurations.Add(new QuoteTypeMap());
            modelBuilder.Configurations.Add(new RankMap());
            modelBuilder.Configurations.Add(new RateClassMap());
            modelBuilder.Configurations.Add(new RatesTableMap());
            modelBuilder.Configurations.Add(new RestrictionMap());
            modelBuilder.Configurations.Add(new RoleFeatureMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RoleTypeMap());
            modelBuilder.Configurations.Add(new RuleConditionFieldMap());
            modelBuilder.Configurations.Add(new RuleNotificationTypeMap());
            modelBuilder.Configurations.Add(new RuleTypeMap());
            modelBuilder.Configurations.Add(new ScreenFieldMap());
            modelBuilder.Configurations.Add(new ScreenModificationMap());
            modelBuilder.Configurations.Add(new ScreenMap());
            modelBuilder.Configurations.Add(new SharedLogisticsUpdateMap());
            modelBuilder.Configurations.Add(new SharedLogisticsUpdateStatuMap());
            modelBuilder.Configurations.Add(new ShipmentAWBPrintOnlyMap());
            modelBuilder.Configurations.Add(new ShipmentCarrierStatusMap());
            modelBuilder.Configurations.Add(new ShipmentCustomerTypeMap());
            modelBuilder.Configurations.Add(new ShipmentLevelMap());
            modelBuilder.Configurations.Add(new ShipmentMasterDataMap());
            modelBuilder.Configurations.Add(new ShipmentOrderPackageMap());
            modelBuilder.Configurations.Add(new ShipmentPackageMap());
            modelBuilder.Configurations.Add(new ShipmentPayableAmountTypeMap());
            modelBuilder.Configurations.Add(new ShipmentPayableLineStatuMap());
            modelBuilder.Configurations.Add(new ShipmentPayableMap());
            modelBuilder.Configurations.Add(new ShipmentPayableStatuMap());
            modelBuilder.Configurations.Add(new ShipmentPickUpDeliveryMap());
            modelBuilder.Configurations.Add(new ShipmentPickUpDeliveryPackageMap());
            modelBuilder.Configurations.Add(new ShipmentReceivableLineStatuMap());
            modelBuilder.Configurations.Add(new ShipmentReceivableMap());
            modelBuilder.Configurations.Add(new ShipmentReceivableStatuMap());
            modelBuilder.Configurations.Add(new ShipmentMap());
            modelBuilder.Configurations.Add(new ShipmentTypeMap());
            modelBuilder.Configurations.Add(new ShippingAgentMap());
            modelBuilder.Configurations.Add(new ShippingLineMap());
            modelBuilder.Configurations.Add(new SpecialServiceMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new TarrifChargeMap());
            modelBuilder.Configurations.Add(new TarrifFromToMap());
            modelBuilder.Configurations.Add(new TarrifFromToTypeMap());
            modelBuilder.Configurations.Add(new TarrifHeaderMap());
            modelBuilder.Configurations.Add(new TarrifStepMap());
            modelBuilder.Configurations.Add(new TarrifTypeMap());
            modelBuilder.Configurations.Add(new TemplateFormatMap());
            modelBuilder.Configurations.Add(new TenantMap());
            modelBuilder.Configurations.Add(new TenantSettingMap());
            modelBuilder.Configurations.Add(new TermsofUsMap());
            modelBuilder.Configurations.Add(new TermsofUseSignatureMap());
            modelBuilder.Configurations.Add(new TextCodeMap());
            modelBuilder.Configurations.Add(new TextCodeTypeMap());
            modelBuilder.Configurations.Add(new TipMap());
            modelBuilder.Configurations.Add(new TipsVisibilityMap());
            modelBuilder.Configurations.Add(new TraceEventMap());
            modelBuilder.Configurations.Add(new TranslationHeaderMap());
            modelBuilder.Configurations.Add(new TranslationMap());
            modelBuilder.Configurations.Add(new TransportModeMap());
            modelBuilder.Configurations.Add(new TriggerTypeMap());
            modelBuilder.Configurations.Add(new TruckerMap());
            modelBuilder.Configurations.Add(new UserLastLoginMap());
            modelBuilder.Configurations.Add(new UserLoginLogMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new ValidationTypeMap());
            modelBuilder.Configurations.Add(new VatTypePercentageMap());
            modelBuilder.Configurations.Add(new VatTypeMap());
            modelBuilder.Configurations.Add(new VendorMap());
            modelBuilder.Configurations.Add(new VesselMap());
            modelBuilder.Configurations.Add(new VolumeUnitMap());
            modelBuilder.Configurations.Add(new WarehouseMap());
            modelBuilder.Configurations.Add(new WeightUnitMap());
            modelBuilder.Configurations.Add(new ChargeTypeAccountingMap());
            modelBuilder.Configurations.Add(new ReportMap());
            modelBuilder.Configurations.Add(new ContactLastLoginMap());
			modelBuilder.Configurations.Add(new SharedLogisticsContactLastLoginMap());
            modelBuilder.Configurations.Add(new ContactLoginLogMap());
            modelBuilder.Configurations.Add(new SmallDocumentMap());
            modelBuilder.Configurations.Add(new CommunicationLogStepMap());
            modelBuilder.Configurations.Add(new SharedLogisticsInvitationStatusMap());
            modelBuilder.Configurations.Add(new ReportGroupMap());
            modelBuilder.Configurations.Add(new UserPermittedBranchMap());
            modelBuilder.Configurations.Add(new LeadSourceMap());
            modelBuilder.Configurations.Add(new IndustryMap());
            modelBuilder.Configurations.Add(new ProductPeriodMap());
            modelBuilder.Configurations.Add(new ProductTypeMap());
            modelBuilder.Configurations.Add(new CustomerProductMap());
            modelBuilder.Configurations.Add(new CustomerProductActualDataMap());
            modelBuilder.Configurations.Add(new CustomerProductLocationMap());
            modelBuilder.Configurations.Add(new CustomerProductLocationActualDataMap());
            modelBuilder.Configurations.Add(new CompetitorMap());
            modelBuilder.Configurations.Add(new CustomerCompetitorMap());
            modelBuilder.Configurations.Add(new CustomerCompetitorProductMap());
            modelBuilder.Configurations.Add(new ContactDoneMethodMap());
			modelBuilder.Configurations.Add(new AdditionalServiceMap());
			modelBuilder.Configurations.Add(new QuotePackageMap());
			modelBuilder.Configurations.Add(new QuoteClosingReasonMap());
			modelBuilder.Configurations.Add(new CustomerStatusMap());
			modelBuilder.Configurations.Add(new UserPermittedProductMap());
			modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
			modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
			modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());
			            modelBuilder.Configurations.Add(new CustomsShipperMap());
            #endregion 
            base.OnModelCreating(modelBuilder);
        }

        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }

        public int SaveChanges()
        {
		    DetectChanges();
		    return base.SaveChanges();
       
	    }

        public DbConnection GetConnection()
		{
			return this.Database.Connection;
		}

		public DbContext GetActiveDbContext()
		{
			return this;
		}

 

	 public IDbSet<AcceptanceStatus> AcceptanceStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AccumalationState> AccumalationStates 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActionCode> ActionCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AddressContactState> AddressContactStates 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AddressPurpose> AddressPurposes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AgentTalkBackType> AgentTalkBackTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmedmentType> AmedmentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendCancellRequestInitiator> AmendCancellRequestInitiators 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendmentFieldReasonType> AmendmentFieldReasonTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendmentFieldStatusType> AmendmentFieldStatusTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendmentRequestStatus> AmendmentRequestStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendmentStatus> AmendmentStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendmentType> AmendmentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmendRequestRejectReasonType> AmendRequestRejectReasonTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AmountType> AmountTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ApprovedProfession> ApprovedProfessions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AssigneeNotificationType> AssigneeNotificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AttachmentType> AttachmentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Authority> Authorities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AuthorizedSignerPermit> AuthorizedSignerPermits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AutonomyRegionType> AutonomyRegionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AutonomyType> AutonomyTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Bank> Banks 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<BuyerRoleType> BuyerRoleTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CancellationReasonRequestType> CancellationReasonRequestTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CancellationRequestStatus> CancellationRequestStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CancelRequestRejectReasonType> CancelRequestRejectReasonTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CarEngineVolume> CarEngineVolumes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoIdentifireType> CargoIdentifireTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoIdentityQualifier> CargoIdentityQualifiers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoSeal> CargoSeals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoSealIdentifier> CargoSealIdentifiers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoSplitRequestStatus> CargoSplitRequestStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoStatus> CargoStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CargoType> CargoTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CarWeight> CarWeights 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CB_CustomsItem> CB_CustomsItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CB_CustomsItemDetailsHistory> CB_CustomsItemDetailsHistorys 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CB_PropertiesDetailsHistory> CB_PropertiesDetailsHistorys 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificateExemptionType> CertificateExemptionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificateOfOrigin> CertificateOfOrigins 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificateOfOriginInvoice> CertificateOfOriginInvoices 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificateOfOriginItem> CertificateOfOriginItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificateOfOriginStatusCodeEnum> CertificateOfOriginStatusCodeEnums 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificateOfOriginTypeCodeEnum> CertificateOfOriginTypeCodeEnums 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CertificatesStatus> CertificatesStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ChangeType> ChangeTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CheckEntityType> CheckEntityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CheckEssenceLookup> CheckEssenceLookups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CheckQueueType> CheckQueueTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CheckRepresentativeType> CheckRepresentativeTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CheckTypeLookup> CheckTypeLookups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<City> Cities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Claim> Claims 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimEntity> ClaimEntities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimExplanationCode> ClaimExplanationCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimImporterDeclarsP3Loi> ClaimImporterDeclarsP3Lois 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimImporterDeclarsPage3> ClaimImporterDeclarsPage3s 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimImporterDeclarsPage3A> ClaimImporterDeclarsPage3As 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimImporterDeclarsPage3B> ClaimImporterDeclarsPage3Bs 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimReasonType> ClaimReasonTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntitiesAmount> ClaimsRelatedEntitiesAmounts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntitiesReason> ClaimsRelatedEntitiesReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntitiesRefund> ClaimsRelatedEntitiesRefunds 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntitiesSeizure> ClaimsRelatedEntitiesSeizures 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntity> ClaimsRelatedEntities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntsExpDeclar> ClaimsRelatedEntsExpDeclars 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClaimsRelatedEntsReasonsExp> ClaimsRelatedEntsReasonsExps 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClassificationType> ClassificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Client> Clients 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientAddress> ClientAddresses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientDrivingLicense> ClientDrivingLicenses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientDrivingLicenseType> ClientDrivingLicenseTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientIndication> ClientIndications 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientItem> ClientItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientsAddressCommType> ClientsAddressCommTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientsPoa> ClientsPoas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClientsTapag> ClientsTapags 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ClosedTableStatus> ClosedTableStatus 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CollateralAnswerStatus> CollateralAnswerStatus 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CollateralAnswerType> CollateralAnswerTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CollateralRequestStatus> CollateralRequestStatus 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CollateralsRequestFileCond> CollateralsRequestFileConds 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CollateralType> CollateralTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CommercialSale> CommercialSales 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CommunicationType> CommunicationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ComputationMethod> ComputationMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConditionalExemptionType> ConditionalExemptionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConfirmationNumberTokenLog> ConfirmationNumberTokenLogs 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConfirmationType> ConfirmationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Consignment> Consignments 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConsignmentInternalTransition> ConsignmentInternalTransitions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConsignmentPackage> ConsignmentPackages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConsignmentPackDanger> ConsignmentPackDangers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConstraintApprovalDecision> ConstraintApprovalDecisions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConstraintProcessType> ConstraintProcessTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConstraintStatus> ConstraintStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConstraintType> ConstraintTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ContactRoleType> ContactRoleTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Containerization> Containerizations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ContainerizationHataraStatus> ContainerizationHataraStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ContainerizationStatusCode> ContainerizationStatusCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ContainerType> ContainerTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ContinuousMessagesTypeCode> ContinuousMessagesTypeCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ContinuousRequestType> ContinuousRequestTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ConverterType> ConverterTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CoolingReportingMethod> CoolingReportingMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CountryCurrency> CountryCurrencies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CountryGroup> CountryGroups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierCustomStatus> CourierCustomStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierDeclaration> CourierDeclarations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierDeclarationStatus> CourierDeclarationStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierHawbFromExcel> CourierHawbFromExcels 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierManifestStatus> CourierManifestStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierMaster> CourierMasters 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierPaymentStatus> CourierPaymentStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierPendingReason> CourierPendingReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourierStatus> CourierStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CouriersVat> CouriersVats 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CourtInstance> CourtInstances 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CurrencyType> CurrencyTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CurrencyTypeTenant> CurrencyTypeTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomBank> CustomBanks 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomBanksCard> CustomBanksCards 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomDocumentType> CustomDocumentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomDocumentTypeMetaData> CustomDocumentTypeMetaData 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomDocumentTypeTenant> CustomDocumentTypeTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerActivityType> CustomerActivityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerClassificationType> CustomerClassificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerIdentificationType> CustomerIdentificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerIdentifyType> CustomerIdentifyTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerIndicationType> CustomerIndicationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerRoleType> CustomerRoleTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomerTypeGeneral> CustomerTypeGenerals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomMetaDataType> CustomMetaDataTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsAddressType> CustomsAddressTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsAirline> CustomsAirlines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsAutonomyKeyword> CustomsAutonomyKeywords 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsBook> CustomsBooks 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsBookType> CustomsBookTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsBranch> CustomsBranches 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsClosedTable> CustomsClosedTables 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsCollateral> CustomsCollaterals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsCollateralsAnswer> CustomsCollateralsAnswers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsCollateralsCondition> CustomsCollateralsConditions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsCountry> CustomsCountries 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsCountryTenant> CustomsCountryTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocument> CustomsDocuments 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocumentMetaDataValue> CustomsDocumentMetaDataValues 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocumentPointer> CustomsDocumentPointers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocumentsDefinition> CustomsDocumentsDefinitions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocumentStatusType> CustomsDocumentStatusTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocumentsTicket> CustomsDocumentsTickets 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsDocumentUpload> CustomsDocumentUploads 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsEntityStatus> CustomsEntityStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsEnvironmentSetting> CustomsEnvironmentSettings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsEnvoirmentType> CustomsEnvoirmentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsExchangeRate> CustomsExchangeRates 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsGeneral> CustomsGenerals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsHouseType> CustomsHouseTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsHouseTypeAdditional> CustomsHouseTypeAdditionals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsHouseTypeTenant> CustomsHouseTypeTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsInsuranceCompany> CustomsInsuranceCompanies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsItem> CustomsItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsItemCategory> CustomsItemCategories 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsItemDetailsHistory> CustomsItemDetailsHistorys 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsItemHierarchicLocation> CustomsItemHierarchicLocations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsPartnerFtp> CustomsPartnerFtps 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsPartnersItem> CustomsPartnersItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsPaymentTerm> CustomsPaymentTerms 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsRequestsSheet> CustomsRequestsSheets 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsRequestsSheetStatus> CustomsRequestsSheetStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsRequiredField> CustomsRequiredFields 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsSetting> CustomsSettings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsShip> CustomsShips 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsTransportMode> CustomsTransportModes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsVendor> CustomsVendors 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CustomsVerificationStatusType> CustomsVerificationStatusTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DangerousGoodsPackingReq> DangerousGoodsPackingReqs 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DBMigration> DBMigrations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DBMigrationLine> DBMigrationLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DebtNotificationType> DebtNotificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DecCargoSplitCargoIdentifier> DecCargoSplitCargoIdentifiers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DecCargoSplitCon> DecCargoSplitCons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DecCargoSplitConsItem> DecCargoSplitConsItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DecCargoSplitConsPackDet> DecCargoSplitConsPackDets 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DecDangersContact> DecDangersContacts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DecisionType> DecisionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Declaration> Declarations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationCargoSplit> DeclarationCargoSplits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationCasualDetails> DeclarationCasualDetailses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationConsAcceptance> DeclarationConsAcceptances 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationConstraint> DeclarationConstraints 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationCounter> DeclarationCounters 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationCourierStatus> DeclarationCourierStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationErrorMapping> DeclarationErrorMappings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationExportRecipient> DeclarationExportRecipients 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationFollowUp> DeclarationFollowUps 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationMamanSpecialAction> DeclarationMamanSpecialActions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationPayment> DeclarationPayments 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationPaymentMethod> DeclarationPaymentMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationPaymentProtest> DeclarationPaymentProtests 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationPending> DeclarationPendings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationReferantData> DeclarationReferantDatas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationStatementType> DeclarationStatementTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationStatus> DeclarationStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationStatusType> DeclarationStatusTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeclarationTax> DeclarationTaxes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DefaultType> DefaultTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DefaultValue> DefaultValues 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Deficit> Deficits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeficitConnFileParagraphType> DeficitConnFileParagraphTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeficitDecision> DeficitDecisions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeliverySiteType> DeliverySiteTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DeliveryType> DeliveryTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DemanderType> DemanderTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Deposit> Deposits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DepositCondition> DepositConditions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DepositCustomerActivity> DepositCustomerActivities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DepositEssenceType> DepositEssenceTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DepositFileType> DepositFileTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DocumentRejectType> DocumentRejectTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<DocumentTypeCustomsData> DocumentTypeCustomsData 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EntitlementType> EntitlementTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EntityTypeLookup> EntityTypeLookups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EntryExitType> EntryExitTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExceptionReason> ExceptionReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExportDeclarationClosingData> ExportDeclarationClosingDatas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExportDeliveryDocumentMessage> ExportDeliveryDocumentMessages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExporterRoleType> ExporterRoleTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExportLogisticPermitAction> ExportLogisticPermitActions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExportReference> ExportReferences 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExportStorage> ExportStorages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExternalFieldMapping> ExternalFieldMappings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FacilitationType> FacilitationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FaultInspectionType> FaultInspectionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FclLclCode> FclLclCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FreightPaymentMethod> FreightPaymentMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FuelType> FuelTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FullnessCode> FullnessCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GatepassRequest> GatepassRequests 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GatepassReturnCode> GatepassReturnCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Gender> Genders 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GovernmentProcedureType> GovernmentProcedureTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GovernmentProcTypeTenant> GovernmentProcTypeTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Guarantee> Guarantees 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GuaranteeCertificateType> GuaranteeCertificateTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GuaranteeCondition> GuaranteeConditions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GuaranteeCustomerActivity> GuaranteeCustomerActivities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<HandingCode> HandingCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<HazardousSubstance> HazardousSubstances 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ImporterDeclarationType> ImporterDeclarationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ImporterDesposition> ImporterDespositions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ImporterPeriodicDeclarStatus> ImporterPeriodicDeclarStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ImporterTypeForClaim> ImporterTypeForClaims 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InceptionCode> InceptionCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<IncotemrsFileValidation> IncotemrsFileValidations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterConditionsRelationship> InterConditionsRelationships 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterfaceManagement> InterfaceManagements 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterfaceSendOption> InterfaceSendOptions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterfaceTenantDefinition> InterfaceTenantDefinitions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InternalBorderSiteType> InternalBorderSiteTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InternationalSite> InternationalSites 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InternationalSiteTenant> InternationalSiteTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InvoiceType> InvoiceTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ItemGovernmentProcedureType> ItemGovernmentProcedureTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LastReleaseFromWarehouse> LastReleaseFromWarehouses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LeadDocumentExceptionType> LeadDocumentExceptionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LeadDocumentType> LeadDocumentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LevyTrust> LevyTrusts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LoadingSiteType> LoadingSiteTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LogisticActionRequest> LogisticActionRequests 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LogisticActionRequestType> LogisticActionRequestTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LogisticActionResponseReqS> LogisticActionResponseReqSes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LogisticPermit> LogisticPermits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LogisticsReferenceType> LogisticsReferenceTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MamanSpecialAction> MamanSpecialActions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MamanSpecialActionStatus> MamanSpecialActionStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MamanStatus> MamanStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ManifestCargoStatus> ManifestCargoStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MAWBType> MAWBTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MeasureQualifier> MeasureQualifier 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MeasurmentUnit> MeasurmentUnits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ModificationAndDiscountType> ModificationAndDiscountTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<MorningMessageType> MorningMessageTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<NbcDeclarationType> NbcDeclarationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<NDMessageActionCode> NDMessageActionCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Notification> Notifications 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<NotificationDefinition> NotificationDefinitions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<NotificationReply> NotificationReplies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<NotificationTenantDefinition> NotificationTenantDefinition 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<NotificationType> NotificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OcrDocument> OcrDocuments 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OcrStatus> OcrStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OrganizationUnitType> OrganizationUnitTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OriginCriterion> OriginCriterions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PackageMeasureQualifier> PackageMeasureQualifiers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PackingType> PackingTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ParagraphType> ParagraphTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PartyRelationshipType> PartyRelationshipTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PassportType> PassportTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PayerActivityType> PayerActivityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PayerType> PayerTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentMethodStatus> PaymentMethodStatus 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentMethodType> PaymentMethodTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrder> PaymentOrders 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrderConnectionTable> PaymentOrderConnectionTables 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrderLine> PaymentOrderLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrderMethod> PaymentOrderMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrderProtestReason> PaymentOrderProtestReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrderStatus> PaymentOrderStatus 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentOrderType> PaymentOrderTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentProcess> PaymentProcesses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentProtestType> PaymentProtestTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentType> PaymentTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PendingByKeyword> PendingByKeywords 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PendingErrorPlace> PendingErrorPlaces 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PerYearFrequency> PerYearFrequencies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PhysicalCheck> PhysicalChecks 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PhysicalCheckCode> PhysicalCheckCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PhysicalCheckOperation> PhysicalCheckOperations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PhysicalCheckSearchResultType> PhysicalCheckSearchResultTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PhysicalCheckStatusMessage> PhysicalCheckStatusMessages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PoaAuthorizationTypeLookup> PoaAuthorizationTypeLookups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PoaStatusTypeLookUp> PoaStatusTypeLookUps 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PointerLevel> PointerLevels 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProceduralFault> ProceduralFaults 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProceduralFaultInProcessType> ProceduralFaultInProcessTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProceduralFaultInSourceType> ProceduralFaultInSourceTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProceduralFaultsConnEntity> ProceduralFaultsConnEntities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProceduralFaultStatus> ProceduralFaultStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProceduralFaultType> ProceduralFaultTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProcessingReason> ProcessingReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProductIdentificationType> ProductIdentificationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ProductNameType> ProductNameTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PropertiesDetailsHistory> PropertiesDetailsHistorys 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<QuotaComputationBasis> QuotaComputationBasises 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<QuotaIncrement> QuotaIncrements 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RansomViolationType> RansomViolationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReferantException> ReferantExceptions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReferantTeam> ReferantTeams 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReferenceInputType> ReferenceInputTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReferenceStatus> ReferenceStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RefundCustomerActivityType> RefundCustomerActivityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RegisteredWarehouseSiteType> RegisteredWarehouseSiteTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RegularityPublication> RegularityPublications 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RegularitySource> RegularitySources 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReleaseMessageType> ReleaseMessageTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RenewalMethod> RenewalMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RequestReasonCodeEnum> RequestReasonCodeEnums 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RequestStatus> RequestStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RequestToAdvanceAQueue> RequestToAdvanceAQueues 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RequestType> RequestTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RequiredGuaranteeType> RequiredGuaranteeTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReturnCondition> ReturnConditions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SalesTaxExemptionType> SalesTaxExemptionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SchedulerParam> SchedulerParams 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SealCompletenes> SealCompleteness 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SealType> SealTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SealUpdateReasonType> SealUpdateReasonTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SecurityClearenceTypeCode> SecurityClearenceTypeCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SeizureFactorType> SeizureFactorTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SeizureMethodType> SeizureMethodTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ServersName> ServersNames 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SignatureType> SignatureTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SignStation> SignStations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SiteLookup> SiteLookups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SiteType> SiteTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SpecialActionDescriptionType> SpecialActionDescriptionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SpecializationType> SpecializationTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SplitOrMergeReason> SplitOrMergeReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<StatusCode> StatusCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<StatusFieldType> StatusFieldTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<StorageMessageType> StorageMessageTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<StorageStatus> StorageStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<StorageStatusTable> StorageStatusTables 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<StuffingSiteType> StuffingSiteTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SubCountry> SubCountries 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SuppInvoiceItemsAbachStatement> SuppInvoiceItemsAbachStatement 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvioceExportDefault> SupplierInvioceExportDefaults 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvioceItemCertificat> SupplierInvioceItemCertificats 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoice> SupplierInvoices 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceFreightAmount> SupplierInvoiceFreightAmounts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItem> SupplierInvoiceItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemModVehicle> SupplierInvoiceItemModVehicles 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemProcesType> SupplierInvoiceItemProcesTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsConDeclar> SupplierInvoiceItemsConDeclars 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsDescript> SupplierInvoiceItemsDescripts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsLevy> SupplierInvoiceItemsLevies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsMod> SupplierInvoiceItemsMods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsPrice> SupplierInvoiceItemsPrices 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsProdIdent> SupplierInvoiceItemsProdIdents 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsSerialNum> SupplierInvoiceItemsSerialNums 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemsTax> SupplierInvoiceItemsTaxes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemVehicle> SupplierInvoiceItemVehicles 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemVehicleAdd> SupplierInvoiceItemVehicleAdds 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceItemVehicleMod> SupplierInvoiceItemVehicleMods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceModification> SupplierInvoiceModifications 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoicePayment> SupplierInvoicePayments 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierInvoiceUCR> SupplierInvoiceUCRs 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupplierPartyType> SupplierPartyTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Tapag> Tapags 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TapagConnectionTable> TapagConnectionTables 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TapagType> TapagTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TarifRelatedToQuota> TarifRelatedToQuotas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TermsOfSaleType> TermsOfSaleTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TPGFileType> TPGFileTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TradeAgreement> TradeAgreements 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TradeAgreementProtocol> TradeAgreementProtocols 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TradeLevyExamptType> TradeLevyExamptTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TradeLevyStatus> TradeLevystatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TransactionNatureType> TransactionNatureTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TransferCargoMethodType> TransferCargoMethodTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TransportMeansType> TransportMeansTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TreatmentWay> TreatmentWays 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<UIMessage> UIMessages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<UIMessageAdditional> UIMessageAdditionals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<UIMessageTenant> UIMessageTenants 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<UnloadingSiteType> UnloadingSiteType 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<UpdateCode> UpdateCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ValidCustomsItem> ValidCustomsItems 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Vehicle> Vehicles 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleManufacturer> VehicleManufacturers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleOwner> VehicleOwners 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehiclePoolType> VehiclePoolTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehiclePriceListType> VehiclePriceListType 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleReductionType> VehicleReductionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleSafeAccessoryInstlType> VehicleSafeAccessoryInstlTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleSafetyAccessory> VehicleSafetyAccessories 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleSafetyAccessoryType> VehicleSafetyAccessoryTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleStatus> VehicleStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleTecnologyType> VehicleTecnologyTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VehicleType> VehicleTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VendorCommission> VendorCommissions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VendorCommunication> VendorCommunications 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VendorCurrency> VendorCurrencies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VendorStatus> VendorStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VendorTransactionType> VendorTransactionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VendorType> VendorTypes 
	 {
	      get; set;
	 
	 }
	  
 }


}