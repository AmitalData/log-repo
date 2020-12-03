
namespace WebFreight.Web.CustomModel.DomainServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using Logitude.Customs.Data;
    using Logitude.Customs.Data.Repsitories;
    using WebFreight.Web.Helpers;
    
    using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
    using System.Data.Entity.Core;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.Messaging.Customs;
    using Logitude.CustomsMessaging.Common.Gen;


    // TODO: Create methods containing your application logic.
   
    [EnableClientAccess()]
    public partial class CustomDomainService : LogitudeDomainService
    {
        ICustomContext customContext;

        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        
        AmitalContext GetAmitalContext(int tenant) 
        {
            var tenantAmitalContext =_AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
            if (tenantAmitalContext == null)
            {
                  
                tenantAmitalContext =AmitalContext.GetContext(tenant);
                _AmitalContextList.Add(tenantAmitalContext);
            }
            return tenantAmitalContext;
        }
        

        protected override void OnError(DomainServiceErrorInfo errorInfo)
        {
            var customsRequestsSheetDomainModelServiceException = errorInfo.Error as CustomsRequestsSheetDomainModelServiceException;
            if (customsRequestsSheetDomainModelServiceException !=null && customsRequestsSheetDomainModelServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
            {
                errorInfo.Error = new DomainException(errorInfo.Error.Message, (int)CustomDomainServiceErrorCodesEnum.NoAvailableSignServer);
            }
            base.OnError(errorInfo);
        }
        private DeclarationRepository declarationRepository;
        private PhysicalCheckRepository physicalCheckRepository;
        private CheckEntityTypeRepository checkEntityTypeRepository;
        private CheckRepresentativeTypeRepository checkRepresentativeTypeRepository;
        private CheckQueueTypeRepository CheckQueueTypeRepository;
        private CargoIdentifireTypeRepository cargoIdentifireTypeRepository;
        private PhysicalCheckOperationRepository physicalCheckOperationRepository;
        private PhysicalCheckStatusMessageRepository physicalCheckStatusMessageRepository;
        private SiteLookupRepository siteRepository;
        private CustomsVendorRepository vendorRepository;
        private VendorTypeRepository vendorTypeRepository;
        private CommunicationTypeRepository communicationTypeRepository;
        private CustomsCountryRepository customsCountryRepository;
        private SubCountryRepository subCountryRepository;
        private VendorCommunicationRepository vendorCommunicationRepository;
        private GovernmentProcedureTypeRepository governmentProcedureTypeRepository;
        private AutonomyTypeRepository autonomyTypeRepository;
        private ConsignmentRepository consignmentRepository;
        private ConsignmentPackageRepository consignmentPackageRepository;
        private ClientRepository clientRepository;
        private CountryGroupRepository countryGroupRepository;
        private EntitlementTypeRepository entitlementTypeRepository;
        private LeadDocumentTypeRepository leadDocumentTypeRepository;
        private PackageMeasureQualifierRepository packageMeasureQualifierRepository;
        private PackingTypeRepository packingTypeRepository;
        private SiteTypeRepository siteTypeRepository;
        private InvoiceTypeRepository invoiceTypeRepository;
        private CurrencyTypeRepository currencyTypeRepository;
        private PaymentTypeRepository paymentTypeRepository;
        private TermsOfSaleTypeRepository TermsOfSaleTypeRepository;
        private CustomsPaymentTermRepository paymentTermRepository;
        private DangerousGoodsPackingReqRepository dangerousGoodsPackingReqRepository;
        private ItemGovernmentProcedureTypeRepository itemGovernmentProcedureTypeRepository;
        private CustomsBookTypeRepository customsBookTypeRepository;
        private SalesTaxExemptionTypeRepository salesTaxExemptionTypeRepository;
        private SupplierInvoiceRepository supplierInvoiceRepository;
        
        private DeclarationTaxRepository declarationTaxRepository;
        private ParagraphTypeRepository paragraphTypeRepository;
        private CustomerActivityTypeRepository customerActivityTypeRepository;
        private OrganizationUnitTypeRepository organizationUnitTypeRepository;
        private PaymentOrderRepository paymentOrderRepository;
        private PaymentOrderLineRepository paymentOrderLineRepository;
        private PaymentOrderStatusRepository paymentOrderStatusRepository;
        private PaymentOrderTypeRepository paymentOrderTypeRepository;
        private PaymentProcessRepository paymentProcessRepository;
        private BankRepository bankRepository;
        private CustomsBranchRepository branchRepository;
        private PaymentOrderMethodRepository paymentOrderMethodRepository;
        private PaymentOrderProtestReasonRepository paymentOrderProtestReasonRepository;
        private PaymentProtestTypeRepository paymentProtestTypeRepository;
        private PaymentMethodStatusRepository paymentMethodStatusRepository;
        private PaymentMethodTypeRepository paymentMethodTypeRepository;
        private PaymentOrderConnectionTableRepository paymentOrderConnectionTableRepository;
        private DeclarationPaymentRepository declarationPaymentRepository;
        private DeclarationPaymentMethodRepository declarationPaymentMethodRepository;
        private DeclarationPaymentProtestRepository declarationPaymentProtestRepository;
        private PayerActivityTypeRepository dayerActivityTypeRepository;
        private ProductIdentificationTypeRepository productIdentificationTypeRepository;
        private CustomsRequestsSheetStatusRepository customsRequestsSheetStatusRepository;
        private ConstraintProcessTypeRepository constraintProcessTypeRepository;
        private ConstraintApprovalDecisionRepository constraintApprovalDecisionRepository;
        private CustomsPartnersItemRepository customsPartnersItemRepository;
        private CustomsTransportModeRepository customsTransportModeRepository;
        private InterfaceManagementRepository interfaceManagementRepository;
        private VehicleRepository vehicleRepository;
        private SplitOrMergeReasonRepository splitOrMergeReasonRepository;
        private ActionCodeRepository actionCodeRepository;
        private CargoSplitRequestStatusRepository cargoSplitRequestStatusRepository;
        private DeclarationCargoSplitRepository declarationCargoSplitRepository;
        private DecCargoSplitConRepository decCargoSplitConRepository;
        private TreatmentWayRepository treatmentWayRepository;
        private DecCargoSplitConsItemRepository decCargoSplitConsItemRepository;
        private DecCargoSplitConsPackDetRepository decCargoSplitConsPackDetRepository;
        private DecCargoSplitCargoIdentifierRepository decCargoSplitCargoIdentifierRepository;

        private DeclarationQueryService declarationQuery;
        private PhysicalCheckQueryService physicalCheckQuery;
        private CheckEntityTypeQueryService checkEntityTypeQuery;     
        private CheckRepresentativeTypeQueryService checkRepresentativeTypeQuery;
        private CheckQueueTypeQueryService CheckQueueTypeQuery;
        private CargoIdentifireTypeQueryService cargoIdentifireTypeQuery;
        private PhysicalCheckOperationQueryService physicalCheckOperationQuery;
        private PhysicalCheckStatusMessageQueryService physicalCheckStatusMessageQuery;
        private SiteLookupQueryService siteQuery;
        private CustomsShipQueryService customsShipQueryService;
        private CustomsVendorQueryService vendorQuery;
        private VendorTypeQueryService vendorTypeQuery;
        private CommunicationTypeQueryService communicationTypeQuery;
        private CustomsCountryQueryService customsCountryQuery;
        private SubCountryQueryService subCountryQuery;
        private VendorCommunicationQueryService vendorCommunicationQuery;
        private GovernmentProcedureTypeQueryService governmentProcedureTypeQuery;
        private AutonomyTypeQueryService autonomyTypeQuery;
        private ConsignmentQueryService consignmentQuery;
        private ConsignmentPackageQueryService consignmentPackageQuery;
        private ClientQueryService clientQuery;
        private CountryGroupQueryService countryGroupQuery;
        private EntitlementTypeQueryService entitlementTypeQuery;
        private LeadDocumentTypeQueryService leadDocumentTypeQuery;
        private PackageMeasureQualifierQueryService packageMeasureQualifierQuery;
        private PackingTypeQueryService packingTypeQuery;
        private SiteTypeQueryService siteTypeQuery;
        private InvoiceTypeQueryService invoiceTypeQuery;
        private CurrencyTypeQueryService currencyTypeQuery;
        private PaymentTypeQueryService paymentTypeQuery;
        private TermsOfSaleTypeQueryService termsOfSaleTypeQuery;
        private CustomsPaymentTermQueryService customsPaymentTermQuery;
        private DangerousGoodsPackingReqQueryService dangerousGoodsPackingReqQuery;
        private ItemGovernmentProcedureTypeQueryService itemGovernmentProcedureTypeQuery;
        private CustomsBookTypeQueryService customsBookTypeQuery;
        private SalesTaxExemptionTypeQueryService salesTaxExemptionTypeQuery;
        private SupplierInvoiceQueryService supplierInvoiceQuery;
        private CustomsExchangeRateQueryService customsExchangeRateQuery;
        
        private DeclarationTaxQueryService declarationTaxQuery;
        private ParagraphTypeQueryService paragraphTypeQuery;
        private SupplierInvoiceItemsTaxQueryService supplierInvoiceItemsTaxQuery;
        private MeasurmentUnitQueryService measurmentUnitQuery;
        private TradeAgreementQueryService tradeAgreementQuery;
        private ConfirmationTypeQueryService confirmationTypeQuery;
        private CertificateExemptionTypeQueryService certificateExemptionTypeQuery;
        private AttachmentTypeQueryService attachmentTypeQuery;
        private ModificationAndDiscountTypeQueryService modificationAndDiscountTypeQuery;
        private CustomerActivityTypeQueryService customerActivityTypeQueryService;
        private OrganizationUnitTypeQueryService organizationUnitTypeQueryService;
        private PaymentProcessQueryService paymentProcessQueryService;
        private PaymentOrderTypeQueryService paymentOrderTypeQueryService;
        private PaymentOrderStatusQueryService paymentOrderStatusQueryService;
        private PaymentOrderQueryService paymentOrderQueryService;
        private PaymentOrderLineQueryService paymentOrderLineQueryService;
        private PaymentOrderMethodQueryService paymentOrderMethodQuery;
        private PaymentOrderProtestReasonQueryService paymentOrderProtestReasonQuery;
        private PaymentProtestTypeQueryService paymentProtestTypeQuery;
        private PaymentMethodStatusQueryService paymentMethodStatusQuery;
        private PaymentMethodTypeQueryService paymentMethodTypeQuery;
        private PaymentOrderConnectionTableQueryService paymentOrderConnectionTableQuery;
        private CustomsBranchQueryService CustomsBranchQuery;
        private BankQueryService bankQuery;
        private CustomerTypeGeneralQueryService customerTypeGeneralQuery;
        private GenderQueryService genderQuery;
        private PassportTypeQueryService passportTypeQuery;
        private AddressContactStateQueryService addressContactStateQuery;
        private CustomsAddressTypeQueryService customsAddressTypeQuery;
        private AddressPurposeQueryService addressPurposeQuery;
        private ContactRoleTypeQueryService contactRoleTypeQuery;
        private AuthorizedSignerPermitQueryService authorizedSignerPermitQuery;
        private CityQueryService cityQuery;
        private DeclarationStatusTypeQueryService declarationStatusTypeQuery;
        private DeclarationPaymentQueryService declarationPaymentQuery;
        private DeclarationPaymentMethodQueryService declarationPaymentMethodQuery;
        private DeclarationPaymentProtestQueryService declarationPaymentProtestQuery;
        private PayerActivityTypeQueryService payerActivityTypeQuery;
        private InternationalSiteQueryService internationalSiteQuery;
        private CustomBankQueryService customBankQuery;
        private PayerTypeQueryService payerTypeQuery;
        private PointerLevelQueryService pointerLevelQuery;

        private ProductIdentificationTypeQueryService productIdentificationTypeQuery;
        private ProductNameTypeQueryService productNameTypeQuery;
        private ClientsAddressCommTypeQueryService clientsAddressCommunicationTypeQuery;
        private DeficitQueryService deficitQuery;
        private NotificationTypeQueryService notificationTypeQuery;
        private TapagQueryService tapagQuery;
        private TapagTypeQueryService tapagTypeQuery;
        private SpecializationTypeQueryService specializationTypeQuery;
        private CustomerRoleTypeQueryService customerRoleTypeQuery;
        private CustomsDocumentQueryService customsDocumentQuery;
        private CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQuery;
        private CustomsDocumentStatusTypeQueryService customsDocumentStatusTypeQuery;
        private CustomDocumentTypeQueryService customDocumentTypeQuery;
        private CustomDocumentTypeMetaDataQueryService customDocumentTypeMetaDataQuery;
        private CustomMetaDataTypeQueryService customMetaDataTypeQuery;
        private CustomsDocumentPointerQueryService customsDocumentPointerQuery;
        private DeclarationConstraintQueryService declarationConstraintQuery;
        private ConstraintTypeQueryService constraintTypeQuery;
        private ConstraintStatusQueryService constraintStatusQuery;
        private MeasureQualifierQueryService measureQualifierQuery;
        private DeliverySiteTypeQueryService deliverySiteTypeQuery;
        private InternalBorderSiteTypeQueryService internalBorderSiteTypeQuery;
        private RegisteredWarehouseSiteTypeQueryService registeredWarehouseSiteTypeQuery;

        private CustomsCollateralQueryService customsCollateralQuery;
        private CustomsCollateralsAnswerQueryService customsCollateralsAnswerQuery;
        private CustomsCollateralsConditionQueryService customsCollateralsConditionQuery;
        private CustomsHouseTypeQueryService customsHouseTypeQuery;
        private CollateralAnswerTypeQueryService collateralAnswerTypeQuery;
        private CollateralTypeQueryService collateralTypeQuery;
        private CollateralRequestStatusQueryService collateralRequestStatusQuery;
        private ReturnConditionQueryService returnConditionQuery;
        private CollateralAnswerStatusQueryService collateralAnswerStatusQuery;
        private CustomsClosedTableQueryService customsClosedTableQuery;
        private ClosedTableStatusQueryService closedTableStatusQuery;
        private CustomsRequiredFieldQueryService customsRequiredFieldQuery;
        private LeadDocumentExceptionTypeQueryService leadDocumentExceptionTypeQuery;
        private EntityTypeLookupQueryService entityTypeLookupQuery;
        private CustomsRequestsSheetStatusQueryService customsRequestsSheetStatusQuery;
        
        private CustomsRequestsSheetQueryService customsRequestsSheetQuery;
        private InterfaceSendOptionQueryService interfaceSendOptionQuery;
        private ConstraintProcessTypeQueryService constraintProcessTypeQuery;
        private ConstraintApprovalDecisionQueryService constraintApprovalDecisionQuery;
        private CustomsSettingQueryService customsSettingQuery;
        private CustomsPartnersItemQueryService customsPartnersItemQuery;
        private CustomsTransportModeQueryService customsTransportModeQuery;
        private InterfaceManagementQueryService interfaceManagementQuery;
        private NotificationQueryService notificationQuery;
        private AssigneeNotificationTypeQueryService assigneeNotificationTypeQuery;
        private NotificationDefinitionQueryService notificationDefinitionQuery;
        private DeclarationConstraintQueryService declarationConstraintQueryService;
        private DepositQueryService depositQuery;
        private DepositEssenceTypeQueryService depositEssenceTypeQuery;
        private UnloadingSiteTypeQueryService unloadingSiteTypeQuery;
        private MorningMessageTypeQueryService morningMessageTypeQuery;
        private VendorStatusQueryService vendorStatusQuery;
        private VendorTransactionTypeQueryService vendorTransactionTypeQuery;
        private DeficitConnFileParagraphTypeQueryService deficitConnectedFileParagraphTypeQuery;
        private ValidCustomsItemQueryService validCustomsItemQuery;
        private CustomBanksCardQueryService customBanksCardQuery;
       // private TenantCustomsBranchQueryService tenantCustomsBranchQuery;
        private CustomsDocumentsTicketQueryService customsDocumentsTicketQuery;
        private TradeLevyExamptTypeQueryService tradeLevyExamptTypeQuery;
        private ImporterPeriodicDeclarStatusQueryService importerPeriodicDeclarationStatusQuery;
        private GuaranteeQueryService guaranteeQuery;
        private GuaranteeCertificateTypeQueryService guaranteeCertificateTypeQuery;
        private GuaranteeConditionQueryService guaranteeConditionQuery;
        private RequiredGuaranteeTypeQueryService requiredGuaranteeTypeQuery;
        private ImporterDespositionQueryService importerDespositionQuery;
        private DebtNotificationTypeQueryService debtNotificationTypeQuery;
        private DepositCustomerActivityQueryService depositCustomerActivityQuery;
        private RequestStatusQueryService requestStatusQuery;
        private CustomsEnvoirmentTypeQueryService customsEnvoirmentTypeQuery;
        private DemanderTypeQueryService demanderTypeQuery;
        private StorageMessageTypeQueryService storageMessageTypeQuery;
        private SpecialActionDescriptionTypeQueryService specialActionDescriptionTypeQuery;
        private CustomsHouseTypeAdditionalQueryService customsHouseTypeAdditionalQuery;
        private LastReleaseFromWarehouseQueryService lastReleaseFromWarehouseQuery;
        private FaultInspectionTypeQueryService faultInspectionTypeQuery;
        private ProceduralFaultInProcessTypeQueryService proceduralFaultInputProcessTypeQuery;
        private ProceduralFaultInSourceTypeQueryService proceduralFaultInputSourceTypeQuery;
        private ProceduralFaultStatusQueryService proceduralFaultStatusQuery;
        private ProceduralFaultTypeQueryService proceduralFaultTypeQuery;
        private RansomViolationTypeQueryService ransomViolationTypeQuery;
        private ProceduralFaultQueryService proceduralFaultQuery;
        private CargoIdentityQualifierQueryService cargoIdentityQualifierQuery;
        private DepositFileTypeQueryService depositFileTypeQuery;
        private VehicleQueryService vehicleQuery;
        private VehiclePoolTypeQueryService vehiclePoolTypeQuery;
        private VehiclePriceListTypeQueryService vehiclePriceListTypeQuery;
        private VehicleManufacturerQueryService vehicleManufacturerQuery;
        private ConverterTypeQueryService converterTypeQuery;
        private VehicleTecnologyTypeQueryService vehicleTecnologyTypeQuery;
        private FuelTypeQueryService fuelTypeQuery;
        private VehicleTypeQueryService vehicleTypeQuery;
        private VehicleStatusQueryService vehicleStatusQuery;
        private VehicleSafeAccessoryInstlTypeQueryService vehicleSafetyAccessoryInstallationTypeQuery;
        private TapagConnectionTableQueryService tapagConnectionTableQuery;
        private CustomerIdentifyTypeQueryService customerIdentifyTypeQuery;
        private AuthorityQueryService authorityQuery;
        private CheckEssenceLookupQueryService checkEssenceLookupQuery;
        private VehicleReductionTypeQueryService vehicleReductionTypeQueryService;
        private VehicleSafetyAccessoryTypeQueryService vehicleSafetyAccessoryTypeQuery;
        private GuaranteeCustomerActivityQueryService guaranteeCustomerActivityQuery;
        private SupplierInvioceItemCertificatQueryService supplierInvioceItemCertificatQuery;
        private CheckTypeLookupQueryService checkTypeLookupQuery;
        private AmendmentRequestStatusQueryService amendmentRequestStatusQuery;
        private AmendmentFieldReasonTypeQueryService amendmentFieldReasonTypeQuery;
        private DeclarationStatementTypeQueryService declarationStatementTypeQuery;
        private ContinuousMessagesTypeCodeQueryService continuousMessagesTypeCodeQuery;
        private ImporterDeclarationTypeQueryService importerDeclarationTypeQuery;
        private CommercialSaleQueryService commercialSaleQuery;
        private ClaimExplanationCodeQueryService claimExplanationCodeQuery;
        private ClaimEntityQueryService claimEntityQuery;
        private CourtInstanceQueryService courtInstanceQuery;
        private FacilitationTypeQueryService facilitationTypeQueryService;
        private CustomsInsuranceCompanyQueryService customsInsuranceCompanyQuery;
        private AccumalationStateQueryService accumalationStateQueryService;
        private FreightPaymentMethodQueryService freightPaymentMethodQueryService;
        private SplitOrMergeReasonQueryService splitOrMergeReasonQueryService;
        private ActionCodeQueryService actionCodeQueryService;
        private CargoSplitRequestStatusQueryService cargoSplitRequestStatusQueryService;
        private DeclarationCargoSplitQueryService declarationCargoSplitQueryService;
        private DecCargoSplitConQueryService decCargoSplitConQueryService;
        private TreatmentWayQueryService treatmentWayQueryService;
        private DecCargoSplitConsItemQueryService decCargoSplitConsItemQueryService;
        private DecCargoSplitConsPackDetQueryService decCargoSplitConsPackDetQueryService;
        private DecCargoSplitCargoIdentifierQueryService decCargoSplitCargoIdentifierQueryService;
        private PendingByKeywordQueryService pendingByKeywordQuery;
        private DeclarationPendingQueryService declarationPendingQueryService;

        // private CustomsHouse
        protected override bool PersistChangeSet()
        {
            try
            {
                if (customContext != null)
                {
                    customContext.SaveChanges();
                }
            }

            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }

            return base.PersistChangeSet();
        }
    }
}


