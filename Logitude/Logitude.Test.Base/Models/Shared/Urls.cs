using System;

namespace Logitude.Test.Base.Models.Shared
{
    public static class Urls
    {
        public static string ShipmentController = "Shipment";
        public static string DirectController = "Direct";
        public static string AddressController = "Addresses";
        public static string StatesController = "States";
        public static string CountriesController = "Countries";
        public static string AuthenticationController = "Authentication";
        public static string GLAccountsController = "GLAccounts";
        public static string BankDepositsController = "bankdeposits";
        public static string BankAccountsController = "bankaccounts";
        public static string DocumentsFilingsController = "DocumentsFilings";
        public static string AccountingNotesController = "accountingnotes";
        public static string PaymentChequesController = "PaymentCheques";
        public static string CashBooksController = "cashbooks";
        public static string BankCodesController = "BankCodes";
        public static string JournalsController = "journals";
        public static string DocumentTypesController = "DocumentTypes";
        public static string DocumentTypeTemplatesController = "DocumentTypeTemplates";
        public static string JournalActionTypesController = "JournalActionTypes";
        public static string PutDocumentOut = "DocumentOutExtended/PutDocumentOut";

        public static string APPaymentsGetSingle(object aPPaymenId)
        {
            throw new NotImplementedException();
        }

        public static string AccountingPaymentMethodsController = "accountingpaymentmethods";
        public static string PostUploadFile = "ImageLibrary/PostUploadFile";
        public static string BranchesController = "branches";
        public static string AutomaticReconcileMethods = "AutomaticReconcileMethods";

        public static string AutomaticReconcileMethodViewsByFilters = "AutomaticReconcileMethodViews/GetByFilters";
        public static string CashBookViewsByFilters = "cashbookviews/getbyfilters";
        public static string JournalActionTypeViewsByFilters = "JournalActionTypeViews/GetByFilters";
        public static string AccountingPaymentMethodViewsByFilters = "accountingpaymentmethodviews/getbyfilters";
        public static string BranchviewsByFilters = "branchviews/getbyfilters";
        public static string GlaccountviewsByFilters = "GLAccountViews/GetByFilters";
        public static string ChartOfAccountViewsByFilters = "ChartOfAccountViews/GetByFilters";
        public static string ShipmentContainersWebServiceController = "ShipmentContainersWebService";
        public static string ChartOfAccountsController = "ChartOfAccounts";
        public static string FullAccountingSettingsController = "FullAccountingSettings";
        public static string AccountingPeriodsController = "AccountingPeriods";
        public static string ChargesGroupsController = "ChargesGroups";
        public static string UserViewsGetByFilters = "UserViews/GetByFilters";
        public static string ContactViewsGetByFilters = "ContactViews/GetByFilters";
        public static string BankCodeViewsGetByFilters = "BankCodeViews/GetByFilters";
        public static string AddressViewsGetByFilters = "AddressViews/GetByFilters";
        public static string BankAccountViewsGetByFilters = "BankAccountViews/GetByFilters";
        public static string ShipmentViewsGetByFilters = "ShipmentViews/GetByFilters";
        public static string FTPDetailViewsGetByFilters = "FTPDetailViews/GetByFilters";
        public static string APInvoiceViewsGetByFilters = "APInvoiceViews/GetByFilters";
        public static string CommunicationLogViewsGetByFilters = "CommunicationLogViews/GetByFilters";
        public static string APPaymentViewsGetByFilters = "APPaymentViews/GetByFilters";
        public static string DocumentTypeViewsGetByFilters = "documenttypeviews/getbyfilters";
        public static string ARInvoiceViewsGetByFilters = "ARInvoiceViews/GetByFilters";
        public static string ARPaymentViewsGetByFilters = "ARPaymentViews/GetByFilters";
        public static string PortViewsGetByFilters = "PortViews/GetByFilters";
        public static string PortViewsGetTenantImportByFilters = "PortViews/GetTenantImportByFilters";
        public static string CashBookLineViewsByFilters = "CashBookLineViews/getbyfilters";
        public static string CountryViewsGetByFilters = "CountryViews/GetByFilters";
        public static string GlobalZoneViewsGetByFilters = "GlobalZoneViews/GetByFilters";
        public static string StateViewsGetByFilters = "StateViews/GetByFilters";
        public static string OpportunityTypeViewsGetByFilters = "OpportunityTypeViews/GetByFilters";
        public static string TicketclassificationViewsGetByFilters = "TicketclassificationViews/GetByFilters";
        public static string StageViewsGetByFilters = "StageViews/GetByFilters";
        public static string TicketStageViewsGetByFilters = "TicketStageViews/GetByFilters";
        public static string ObjectTableViewsGetByFilters = "ObjectTableViews/GetByFilters";
        public static string TariffProductViews = "TariffProductViews/GetByFilters";
        public static string TariffViews = "TariffViews/GetByFilters";

        public static string SpecialServicesTypesController = "SpecialServicesTypes";
        public static string SpecialServicesTypeViewsGetByFilters = "SpecialServicesTypeViews/GetByFilters";
        public static string AccountingPeriodViewsGetByFilters = "AccountingPeriodViews/GetByFilters";

        public static string QuoteController = "Quotes";
        public static string QuoteViewsGetByFilters = "Quoteviews/Getbyfilters";

        public static string CrossDockController = "warehouseentries";
        public static string CrossReleaseGetController = "warehousereleases";
        public static string CrossReleaseController = "WarehouseReleaseExtended/postwarehousereleasepm";
        public static string PostSendHtmlDocument = "HtmlEditor/postsendhtmldocument";
        public static string ActivitiesController = "Activities";
        public static string TicketsController = "tickets";
        public static string OpportunitiesController = "Opportunities";
        public static string TariffsController = "Tariffs";
        public static string CRMDomainControllerInserNewTicket = "CRMDomain/InserNewTicket";
        public static string GetTenantTariffSetting = "TariffDomain/GetTenantTariffSetting";
        public static string GetBankAccountsSummary = "BankAccountViews/GetBankAccountsSummary";

        public static string PostFixEntegrityCheckErrorInBatch = "AccountingEntegrityCheck/PostFixEntegrityCheckErrorInBatch";

        //public static string QuotesGetSingle(string id)
        //{
        //    return "Quotes/GetSingle?id=" + id;
        //}

        public static string APInvoicesController = "APInvoices";
        public static string ARInvoicesController = "ARInvoices";
        public static string ARPaymentController = "arpayments";
        public static string APPaymentsController = "APPayments";
        public static string CargoTrackingSearchController = "CargoTrackingSearch";
        public static string Tariffsettings = "tariffsettings";

        public static string TMProjectViewsGetByFilters = "TMProjectViews/GetByFilters";
        public static string TmprojectsController = "Tmprojects";
        public static string TMBudgetViewsGetByFilters = "TMBudgetViews/GetByFilters";
        public static string TMBudgetsController = "TMBudgets";
        public static string TmprojectcategoryViewsByFilters = "TmprojectcategoryViews/GetByFilters";
        public static string TmprojectcategoriesController = "Tmprojectcategories";
        public static string SprintViewsByFilters = "SprintViews/GetByFilters";
        public static string SprintsController = "Sprints";
        public static string TimeManagementDomainController = "TimeManagementDomain";
        public static string ShipmentOrderController = "ShipmentOrder";

        #region Shipment Prepare Data URls
        //locations
        public static string VesselsController = "Vessels";
        public static string IncotermsController = "Incoterms";
        public static string CreditCardController = "creditcardtypes";
        public static string MoveTypesController = "MoveTypes";
        public static string PackageTypesController = "PackageTypes";
        public static string ShipmentSubTypesController = "ShipmentSubTypes";
        public static string CurrencyViewsGetByFilters = "CurrencyViews/GetByFilters";
        public static string IncotermViewsGetByFilters = "IncotermViews/GetByFilters";
        public static string CreditCardTypeViewsGetByFilters = "CreditCardTypeViews/GetByFilters";
        public static string MeasurementViewsGetByFilters = "MeasurementViews/GetByFilters";
        public static string ChargeTypeViewsGetByFilters = "ChargesTypeViews/GetByFilters";
        public static string PutSystem1000File = "AccountingOp/PutSystem1000File";
        public static string PostTestOperation = "AccountingOp/PostTestOperation";
        public static string ChargesgroupviewsGetByFilters = "chargesgroupviews/getbyfilters";
        public static string PackageTypeViewsGetByFilters = "PackageTypeViews/GetByFilters";
        public static string PaymentTermViewsGetByFilters = "PaymentTermViews/GetByFilters";
        public static string VatTypeViewsGetByFilters = "VatTypeViews/GetByFilters";
        public static string QuoteStageViewsGetByFilters = "QuoteStageViews/GetByFilters";
        public static string VesselViewsGetByFilters = "VesselViews/GetByFilters";
        public static string MoveTypeViewsGetByFilters = "MoveTypeViews/GetByFilters";
        public static string ShipmentSubTypeViewsGetByFilters = "ShipmentSubTypeViews/GetByFilters";

        //partners
        public static string PartnersDomainController = "PartnersDomain/PostPartnerAddress";
        public static string VendorViewsGetByFilters = "VendorViews/GetByFilters";
        public static string AgentViewsGetByFilters = "AgentViews/GetByFilters";
        public static string CustomerViewsGetByFilters = "CustomerViews/GetByFilters";
        public static string CustomAgentViewsGetByFilters = "CustomAgentViews/GetByFilters";
        public static string ShippingAgentViewsGetByFilters = "ShippingAgentViews/GetByFilters";
        public static string TruckerViewsGetByFilters = "TruckerViews/GetByFilters";
        public static string AirlineViewsGetByFilters = "AirlineViews/GetByFilters";
        public static string ShippingLineViewsGetByFilters = "ShippingLineViews/GetByFilters";
        public static string WarehouseViewsGetByFilters = "WarehouseViews/GetByFilters";
        public static string CarrierViewsGetTenantImportByFilters = "CarrierViews/GetTenantImportByFilters";
        public static string CountryCityViewsGetByFilters = "CountryCityViews/GetByFilters";
        public static string CountryCities = "CountryCities";
        public static string ChargesTypes = "ChargesTypes";
        public static string Airlines = "airlines";
        #endregion

        public static string TenantsGetSingle(int id)
        {
            return "Tenants/GetSingle?id=" + id.ToString();
        }
        public static string GetCustomerById(string id)
        {
            return $"PartnersDomain/GetCustomerById?id={id}" ;
        }
        public static string PostCreatePeriodsForYear(int year)
        {
            return $"accountingPeriods/PostCreatePeriodsForYear?year={year}" ;
        }

        public static string TenantsUpdate(int id)
        {
            return "Tenants/" + id.ToString();
        }

        public static string ContactsGetSingle(string id)
        {
            return "Contacts/GetSingle?id=" + id;
        }
        public static string GLAccountsGetSingle(string id)
        {
            return "GLAccounts/GetSingle?id=" + id;
        }
        public static string BankAccountsGetSingle(string id)
        {
            return "bankaccounts/GetSingle?id=" + id;
        }
        public static string GetBankDepositWithoutLines(string id)
        {
            return $"BankDeposit/GetSingleWithoutLines?id={id}";
        }
        public static string GetCardGLAccountConnect( int tenant)
        {
            return $"CardGLAccountConnect/GetCardGLAccountConnect?tenant={tenant}";
        }
        public static string GetSingleGLAccountByDispalyNumberAndTenant(string displayNumber, int tenant)
        {
            return $"GLAccounts/GetSingleByDispalyNumberAndTenant?displayNumber={displayNumber}&tenant={tenant}";
        }
        public static string GetSingleGLAccountByInternalNumberAndTenant(string internalNumber, int tenant)
        {
            return $"GLAccounts/GetSingleByInternalNumberAndTenant?internalNumber={internalNumber}&tenant={tenant}";
        }
        public static string DeleteGLAccountNote(string noteId)
        {
            return $"AccountingNotes/PostDeleteNote?noteId={noteId}";
        }
        public static string GetNotesByCard(string cardId)
        {
            return $"AccountingNoteViews/GetNotesByCard?cardId={cardId}";
        }
        public static string GetGenerate1000InAccountingOp(string email)
        {
            return $"AccountingOp/GetGenerate1000?email={email}";
        }
        public static string GetTestOperation(string operationId, string myparams)
        {
            return $"AccountingOp/GetTestOperation?operationId={operationId}&myparams={myparams}";
        }
        public static string PostCreatePeriodsForYear(string year)
        {
            return $"AccountingPeriods/PostCreatePeriodsForYear?year={year}";
        }
        public static string GetAccountingPeriodByYear(int year, string typeCode)
        {
            return $"AccountingPeriodViews/GetByYear?year={year}&typeCode={typeCode}";
        }
        public static string GetRunAllPayablePostDatedARPaymentCheques(int tenant)
        {
            return $"ARPaymentChequeOp/GetRunAllPayablePostDatedARPaymentCheques?tenant={tenant}";
        }
        public static string CustomersGetSingle(string id)
        {
            return "customers/getsingle?id=" + id;
        }
        public static string GetInsertControlAccount(string ControlAccountId,string ChartOfAccountsId)
        {
            return $"glaccountviews/GetInsertControlAccount?ControlAccountId={ControlAccountId}&ChartOfAccountsId={ChartOfAccountsId}";
        }
        public static string VendorssGetSingle(string id)
        {
            return "vendors/getsingle?id=" + id;
        }
        public static string PaymentChequesGetSingle(string id)
        {
            return "PaymentCheques/getsingle?id=" + id;
        }

        public static string AddressesGetSingle(string id)
        {
            return "Addresses/GetSingle?id=" + id;
        }
        public static string FullAccountingSettingsGetSingle(int tenant)
        {
            return $"fullaccountingsettings/getsingle?id={tenant}";
        }

        public static string ShipmentGetSingle(string id)
        {
            return "Shipment/GetSingle?id=" + id;
        }
        public static string ContainerGetSingle(string id)
        {
            return "Containers/GetSingle?id=" + id;
        }
        public static string PostReturnCheque(PostReturnChequeArgs postReturnChequeArgs)
        {
            return $"BankDeposit/PostReturnCheque?bankDepositId={postReturnChequeArgs.BankDepositId}&arpChequeId={postReturnChequeArgs.ARPChequeId}&returnType={postReturnChequeArgs.ReturnType}&notes={postReturnChequeArgs.Notes}";
        }

        public static string FTPDetailsGetSingle(string id)
        {
            return "FTPDetails/GetSingle?id=" + id;
        }

        public static string APInvoicesGetSingle(string id)
        {
            return "APInvoices/GetSingle?id=" + id;
        }

        public static string APPaymentsGetSingle(string id)
        {
            return "APPayments/GetSingle?id=" + id;
        }

        public static string ARInvoicesGetSingle(string id)
        {
            return "ARInvoices/GetSingle?id=" + id;
        }

        public static string ARPaymentsGetSingle(string id)
        {
            return "ARPayments/GetSingle?id=" + id;
        }

        public static string AirlineGetSingle(string id)
        {
            return "airlines/getsingle?id=" + id;
        }
        public static string GetAllAddressesPMsbyCardId(string CardId)
        {
            return $"PartnersDomain/GetAllAddressesPMsbyCardId?myCardId={CardId}";
        }

        public static string GetTransactionsForARPayment(string glAccountId,string currencyId )
        {
            return $"LedgerTransactions/GetTransactionsForARPayment?arpaymentId=undefined&billToGLAccountId={glAccountId}&paymentCurrencyId={currencyId}";
        }

        public static string CommonDomainGetPortCopyToCurrentTenant(string portId)
        {
            return "CommonDomain/GetPortCopyToCurrentTenant?entityId=" + portId;
        }

        public static string PartnersDomainGetCarrierCopyToCurrentTenant(string carrierId)
        {
            return "PartnersDomain/GetCarrierCopyToCurrentTenant?entityId=" + carrierId;
        }

        public static string CommonDomainGetCopyCurrencyToTenant(string currencyId)
        {
            return "CommonDomain/GetCopyCurrencyToTenant?currencyId=" + currencyId + "&CurrencyRate=4&RateDate=2019-6-24%2015:2:53.564";
        }

        public static string QuoteGetSingle(string id)
        {
            return "Quotes/GetSingle?id=" + id;
        }
        public static string QuoteGetSingleList(string id)
        {
            return "Quoteviews/getsingle/?id=" + id;
        }

        public static string CrossDockGetSingle(string id)
        {
            return "warehouseentries/GetSingle?id=" + id;
        }

        public static string CargoTrackingShipmentGetSingleList(string securityKey, int tenant)
        {
            return "CargoTrackingSearch/GetSingleShipmentList?SecurityKey=" + securityKey + "&tenant=" + tenant;
        }

        public static string CrossDockReleaseGetSingle(string id)
        {
            return "warehousereleases/getsingle?id=" + id;
        }

        public static string VesselGetSingle(string id)
        {
            return "vessels/getsingle?id=" + id;
        }

        public static string ActivitySingle(string id)
        {
            return "activities/GetSingle?id=" + id;
        }
        public static string OpportunitySingle(string id)
        {
            return "Opportunities/GetSingle?id=" + id;
        }
        public static string TicketSingle(string id)
        {
            return "Tickets/GetSingle?id=" + id;
        }
        public static string TariffSingle(string id)
        {
            return "Tariffs/GetSingle?id=" + id;
        }
        public static string CashbooksSingle(string id)
        {
            return "cashbooks/GetSingle?id=" + id;
        }
        public static string BankDepositsSingle(string id)
        {
            return "bankdeposits/GetSingle?id=" + id;
        }

        public static string ShipmentOrderSingle(string orderNumber)
        {
            return "ShipmentOrder?orderNumber=" + orderNumber;
        }
        public static string GetFileSizeFormat(int size)
        {
            return $"DocumentsFilingExtended?fileBytes={size}";
        }
        public static string DownloadPage(string securityId,string tempId)
        {
            return $"WebPages/DownloadPage.aspx?securityId={securityId}&tempId={tempId}";
        }

        public static string GetDataEntryTimeSheetList(string employeeUserId, string locationCode, DateTime startDate, DateTime endDate)
        {
            return "TimeManagementDomain/GetDataEntryTimeSheetList?employeeUserId=" + employeeUserId + "&locationCode=" +
                locationCode + "&startDate=" + startDate.ToString("yyyy:M:d:H:m:s") + "&endDate=" + endDate.ToString("yyyy:M:d:H:m:s");
        }
        public static string GetCreateDocumentsFiling(GetCreateDocumentsFilingArgs arguments)
        {
            return $"DocumentsFilingExtended/GetCreateDocumentsFiling?documentTypeId={arguments.DocumentTypeId}&entityId={arguments.EntityId}&childEntityId=&childReference=&objectTableId={arguments.ObjectTableId}&directionCode={arguments.DirectionCode}&tenant={arguments.Tenant}";
        }
        public static string GetCreateDocumentsOut(GetCreateDocumentsFilingArgs arguments)
        {
            return $"DocumentOutExtended/getcreatedocumentout?documentTypeId={arguments.DocumentTypeId}&entityId={arguments.EntityId}&childEntityId=&childReference=&objectTableId={arguments.ObjectTableId}&directionCode={arguments.DirectionCode}&tenant={arguments.Tenant}";
        }
        public static string GetDocumentType(string id, string documentOutId, int tenant)
        {
            return $"DocumentTypeExtended/getsingledocumenttype/?id={id}&documentOutId={documentOutId}&tenant={tenant}";
        }
        public static string GetDocumentTypeTemplate(string documentOutId, int tenant)
        {
            return $"DocumentTypeTemplateExtended/getdocumenttypetemplatelistsfordocumenttype?documentTypeId={documentOutId}&tenant={tenant}";
        }
        public static string GetDocumentCopy(GetDocumentCopyArgs getDocumentCopy)
        {
            return $"ExportDocument?documentTypeId={getDocumentCopy.DocumentTypeId}&entityId={getDocumentCopy.EntityId}&entityObjectTableId={getDocumentCopy.EntityObjectTableId}&childEntityId=&childObjectTableId=&documentOutId={getDocumentCopy.DocumentOutId}&tenant={getDocumentCopy.Tenant}&documentTypeCopyId={getDocumentCopy.DocumentTypeCopyId}&userId={getDocumentCopy.UserId}";
        }
        


    }
}