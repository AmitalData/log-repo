"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ReportComponent_1 = require("./Components/Workspaces/ReportComponent");
var MainReportsWorkspace_1 = require("./Components/Workspaces/MainReportsWorkspace");
var BIReportComponent_1 = require("./Components/Workspaces/BIReportComponent");
var BIFolderReportComponent_1 = require("./Components/Workspaces/BIFolderReportComponent");
var ReportTemplateComponent_1 = require("./Components/ReportTemplateComponent");
var ReportsPreviewComponent_1 = require("./Components/ReportsPreviewComponent");
var ReportsTemplateRestoreComponent_1 = require("./Components/ReportsTemplateRestoreComponent");
var NewReportsTemplateComponent_1 = require("./Components/NewReportsTemplateComponent");
// Statisticsss
var ActivityStatusDashboardFilterComponent_1 = require("./Components/FilterReportComponent/ActivityStatusDashboardFilterComponent");
var EAWBFilterComponent_1 = require("./Components/FilterReportComponent/EAWBFilterComponent");
var BookingFilterComponent_1 = require("./Components/FilterReportComponent/BookingFilterComponent");
var FlightBookingFilterComponent_1 = require("./Components/FilterReportComponent/FlightBookingFilterComponent");
var IATAStatisticsFilterComponent_1 = require("./Components/FilterReportComponent/IATAStatisticsFilterComponent");
var ParticipantsUsersActivitiesFilterComponent_1 = require("./Components/FilterReportComponent/ParticipantsUsersActivitiesFilterComponent");
var StatisticsByAgentFilterComponent_1 = require("./Components/FilterReportComponent/StatisticsByAgentFilterComponent");
var CarrierStatisticFilterComponent_1 = require("./Components/FilterReportComponent/CarrierStatisticFilterComponent");
var StatisticsByCustomerFilterComponent_1 = require("./Components/FilterReportComponent/StatisticsByCustomerFilterComponent");
// Operational
var CASSReportFilterComponent_1 = require("./Components/FilterReportComponent/CASSReportFilterComponent");
var ContainerDetailsVoyageFilterComponent_1 = require("./Components/FilterReportComponent/ContainerDetailsVoyageFilterComponent");
var ContainerTruckingFilterComponent_1 = require("./Components/FilterReportComponent/ContainerTruckingFilterComponent");
var OceanShipmentReportFilterComponent_1 = require("./Components/FilterReportComponent/OceanShipmentReportFilterComponent");
var ProfitByShipmentFilterConmponent_1 = require("./Components/FilterReportComponent/ProfitByShipmentFilterConmponent");
var InventoryReportFilterConmponent_1 = require("./Components/FilterReportComponent/InventoryReportFilterConmponent");
var OpenShipmentsByCustomerFilterComponent_1 = require("./Components/FilterReportComponent/OpenShipmentsByCustomerFilterComponent");
var UnicargoExportReportFilterComponent_1 = require("./Components/FiltersComponent/Operational/UnicargoExportReportFilterComponent");
var ShipmentsEventsListFilterComponent_1 = require("./Components/FiltersComponent/Operational/ShipmentsEventsListFilterComponent");
// Accounting
var AccountingLedgerFilterComponent_1 = require("./Components/FilterReportComponent/AccountingLedgerFilterComponent");
var AgedAccountsReceivableFilterComponent_1 = require("./Components/FilterReportComponent/AgedAccountsReceivableFilterComponent");
var InvoicesFilterComponent_1 = require("./Components/FilterReportComponent/InvoicesFilterComponent");
var ARInvoicesDepositReportFilterComponent_1 = require("./Components/FilterReportComponent/ARInvoicesDepositReportFilterComponent");
var ShipmentChargesAnalysisFilterComponent_1 = require("./Components/FilterReportComponent/ShipmentChargesAnalysisFilterComponent");
var StatementFilterComponent_1 = require("./Components/FilterReportComponent/StatementFilterComponent");
var StatementByInvoiceDateFilterComponent_1 = require("./Components/FilterReportComponent/StatementByInvoiceDateFilterComponent");
var InvoicesByPartnerFilterComponent_1 = require("./Components/FilterReportComponent/InvoicesByPartnerFilterComponent");
var ArchivoExportadoComponent_1 = require("./Components/FiltersComponent/Accounting/ArchivoExportadoComponent");
var InvoicesRoutingsFilterComponent_1 = require("./Components/FiltersComponent/Accounting/InvoicesRoutingsFilterComponent");
var LedgerTransactionsFilterControl_1 = require("./Components/FiltersComponent/Accounting/LedgerTransactionsFilterControl");
var AgingFilterComponent_1 = require("./Components/FiltersComponent/Accounting/AgingFilterComponent");
var RevenueExpenseFilterComponent_1 = require("./Components/FilterReportComponent/RevenueExpenseFilterComponent");
var TrailBalanceFiltersComponent_1 = require("./Components/FilterReportComponent/TrailBalanceFiltersComponent");
var ShipmentsStocksFiltersComponent_1 = require("./Components/FilterReportComponent/ShipmentsStocksFiltersComponent");
var DetailedShipmentChargesAnalysisComponent_1 = require("./Components/FiltersComponent/Accounting/DetailedShipmentChargesAnalysisComponent");
var VendorChargesAnalysisFilterComponent_1 = require("./Components/FiltersComponent/Accounting/VendorChargesAnalysisFilterComponent");
var AutomationTestReportFilterComponent_1 = require("./Components/FilterReportComponent/AutomationTestReportFilterComponent");
// Quotes
var QuotesFilterComponent_1 = require("./Components/FilterReportComponent/QuotesFilterComponent");
// CRM
var ApprovedOpportunitiesFilterComponent_1 = require("./Components/FilterReportComponent/ApprovedOpportunitiesFilterComponent");
var CustomerAdditionalServicesFilterComponent_1 = require("./Components/FilterReportComponent/CustomerAdditionalServicesFilterComponent");
var CustomerPotentialActualFilterComponent_1 = require("./Components/FilterReportComponent/CustomerPotentialActualFilterComponent");
var ExpectedIncomeFilterComponent_1 = require("./Components/FilterReportComponent/ExpectedIncomeFilterComponent");
var MonthlyConversionFilterComponent_1 = require("./Components/FilterReportComponent/MonthlyConversionFilterComponent");
var OpportunitiesAdditionalServicesFilterComponent_1 = require("./Components/FilterReportComponent/OpportunitiesAdditionalServicesFilterComponent");
var StageChangingFilterComponent_1 = require("./Components/FilterReportComponent/StageChangingFilterComponent");
var ShipmentProfitVSQuoteEstimateComponent_1 = require("./Components/FiltersComponent/CRM/ShipmentProfitVSQuoteEstimateComponent");
var ParentVsChildTenantsComponent_1 = require("./Components/FilterReportComponent/ParentVsChildTenantsComponent");
var UsersByTenantReportFilterComponent_1 = require("./Components/FiltersComponent/CRM/UsersByTenantReportFilterComponent");
var LicenseManagementFilterComponent_1 = require("./Components/FiltersComponent/Operational/LicenseManagementFilterComponent");
var VehiclesFilterComponent_1 = require("./Components/FiltersComponent/Operational/VehiclesFilterComponent");
// Time Sheet
var EmployeeTimeSheetFilterComponent_1 = require("./Components/FilterReportComponent/EmployeeTimeSheetFilterComponent");
var WorkDaysPerProjectFilterComponent_1 = require("./Components/FilterReportComponent/WorkDaysPerProjectFilterComponent");
var TasksOfNoProjectsFilterComponent_1 = require("./Components/FilterReportComponent/TasksOfNoProjectsFilterComponent");
//Shipment Details
var ShipmentDetailsFilterComponent_1 = require("./Components/FilterReportComponent/ShipmentDetailsFilterComponent");
//VDK Templates
var VDKFilterComponent_1 = require("./Components/FilterReportComponent/VDKFilterComponent");
exports.Components = [
    MainReportsWorkspace_1.MainReportsWorkspace,
    ReportComponent_1.ReportComponent,
    BIReportComponent_1.BIReportComponent,
    BIFolderReportComponent_1.BIFolderReportComponent,
    ReportTemplateComponent_1.ReportTemplateComponent,
    ReportsPreviewComponent_1.ReportsPreviewComponent,
    ReportsTemplateRestoreComponent_1.ReportsTemplateRestoreComponent,
    NewReportsTemplateComponent_1.NewReportsTemplateComponent,
    IATAStatisticsFilterComponent_1.IATAStatisticsFilterComponent,
    AccountingLedgerFilterComponent_1.AccountingLedgerFilterComponent,
    FlightBookingFilterComponent_1.FlightBookingFilterComponent,
    ParticipantsUsersActivitiesFilterComponent_1.ParticipantsUsersActivitiesFilterComponent,
    ActivityStatusDashboardFilterComponent_1.ActivityStatusDashboardFilterComponent,
    EAWBFilterComponent_1.EAWBFilterComponent,
    BookingFilterComponent_1.BookingFilterComponent,
    StatisticsByAgentFilterComponent_1.StatisticsByAgentFilterComponent,
    StatisticsByCustomerFilterComponent_1.StatisticsByCustomerFilterComponent,
    CarrierStatisticFilterComponent_1.CarrierStatisticFilterComponent,
    ContainerDetailsVoyageFilterComponent_1.ContainerDetailsVoyageFilterComponent,
    ContainerTruckingFilterComponent_1.ContainerTruckingFilterComponent,
    OceanShipmentReportFilterComponent_1.OceanShipmentReportFilterComponent,
    ProfitByShipmentFilterConmponent_1.ProfitByShipmentFilterConmponent,
    AgedAccountsReceivableFilterComponent_1.AgedAccountsReceivableFilterComponent,
    StatementFilterComponent_1.StatementFilterComponent,
    StatementByInvoiceDateFilterComponent_1.StatementByInvoiceDateFilterComponent,
    InvoicesByPartnerFilterComponent_1.InvoicesByPartnerFilterComponent,
    QuotesFilterComponent_1.QuotesFilterComponent,
    InvoicesFilterComponent_1.InvoicesFilterComponent,
    ApprovedOpportunitiesFilterComponent_1.ApprovedOpportunitiesFilterComponent,
    OpportunitiesAdditionalServicesFilterComponent_1.OpportunitiesAdditionalServicesFilterComponent,
    ShipmentChargesAnalysisFilterComponent_1.ShipmentChargesAnalysisFilterComponent,
    CustomerAdditionalServicesFilterComponent_1.CustomerAdditionalServicesFilterComponent,
    CustomerPotentialActualFilterComponent_1.CustomerPotentialActualFilterComponent,
    ExpectedIncomeFilterComponent_1.ExpectedIncomeFilterComponent,
    MonthlyConversionFilterComponent_1.MonthlyConversionFilterComponent,
    StageChangingFilterComponent_1.StageChangingFilterComponent,
    ARInvoicesDepositReportFilterComponent_1.ARInvoicesDepositReportFilterComponent,
    CASSReportFilterComponent_1.CASSReportFilterComponent,
    ShipmentProfitVSQuoteEstimateComponent_1.ShipmentProfitVSQuoteEstimateComponent,
    ArchivoExportadoComponent_1.ArchivoExportadoComponent,
    AgingFilterComponent_1.AgingFilterComponent,
    LedgerTransactionsFilterControl_1.LedgerTransactionsFilterControl,
    InventoryReportFilterConmponent_1.InventoryReportFilterConmponent,
    InvoicesRoutingsFilterComponent_1.InvoicesRoutingsFilterComponent,
    EmployeeTimeSheetFilterComponent_1.EmployeeTimeSheetFilterComponent,
    WorkDaysPerProjectFilterComponent_1.WorkDaysPerProjectFilterComponent,
    TasksOfNoProjectsFilterComponent_1.TasksOfNoProjectsFilterComponent,
    OpenShipmentsByCustomerFilterComponent_1.OpenShipmentsByCustomerFilterComponent,
    ParentVsChildTenantsComponent_1.ParentVsChildTenantsComponent,
    RevenueExpenseFilterComponent_1.RevenueExpenseFilterComponent,
    TrailBalanceFiltersComponent_1.TrailBalanceFiltersComponent,
    UsersByTenantReportFilterComponent_1.UsersByTenantReportFilterComponent,
    LicenseManagementFilterComponent_1.LicenseManagementFilterComponent,
    ShipmentsStocksFiltersComponent_1.ShipmentsStocksFiltersComponent,
    ShipmentDetailsFilterComponent_1.ShipmentDetailsFilterComponent,
    DetailedShipmentChargesAnalysisComponent_1.DetailedShipmentChargesAnalysisComponent,
    VendorChargesAnalysisFilterComponent_1.VendorChargesAnalysisFilterComponent,
    VDKFilterComponent_1.VDKFilterComponent,
    VehiclesFilterComponent_1.VehiclesFilterComponent,
    UnicargoExportReportFilterComponent_1.UnicargoExportReportFilterComponent,
    ShipmentsEventsListFilterComponent_1.ShipmentsEventsListFilterComponent,
    AutomationTestReportFilterComponent_1.AutomationTestReportFilterComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "MainReportsWorkspace": {
                myResult = MainReportsWorkspace_1.MainReportsWorkspace;
                break;
            }
            case "ReportComponent": {
                myResult = ReportComponent_1.ReportComponent;
                break;
            }
            case "BIReportComponent": {
                myResult = BIReportComponent_1.BIReportComponent;
                break;
            }
            case "BIFolderReportComponent": {
                myResult = BIFolderReportComponent_1.BIFolderReportComponent;
                break;
            }
            case "ReportsPreviewComponent": {
                myResult = ReportsPreviewComponent_1.ReportsPreviewComponent;
                break;
            }
            case "ReportTemplateComponent": {
                myResult = ReportTemplateComponent_1.ReportTemplateComponent;
                break;
            }
            case "ReportsTemplateRestoreComponent": {
                myResult = ReportsTemplateRestoreComponent_1.ReportsTemplateRestoreComponent;
                break;
            }
            case "NewReportsTemplateComponent": {
                myResult = NewReportsTemplateComponent_1.NewReportsTemplateComponent;
                break;
            }
            case "IATAStatisticsFilterComponent": {
                myResult = IATAStatisticsFilterComponent_1.IATAStatisticsFilterComponent;
                break;
            }
            case "AccountingLedgerFilterComponent": {
                myResult = AccountingLedgerFilterComponent_1.AccountingLedgerFilterComponent;
                break;
            }
            case "FlightBookingFilterComponent": {
                myResult = FlightBookingFilterComponent_1.FlightBookingFilterComponent;
                break;
            }
            case "ParticipantsUsersActivitiesFilterComponent": {
                myResult = ParticipantsUsersActivitiesFilterComponent_1.ParticipantsUsersActivitiesFilterComponent;
                break;
            }
            case "ActivityStatusDashboardFilterComponent": {
                myResult = ActivityStatusDashboardFilterComponent_1.ActivityStatusDashboardFilterComponent;
                break;
            }
            case "EAWBFilterComponent": {
                myResult = EAWBFilterComponent_1.EAWBFilterComponent;
                break;
            }
            case "BookingFilterComponent": {
                myResult = BookingFilterComponent_1.BookingFilterComponent;
                break;
            }
            case "StatisticsByAgentFilterComponent": {
                myResult = StatisticsByAgentFilterComponent_1.StatisticsByAgentFilterComponent;
                break;
            }
            case "StatisticsByCustomerFilterComponent": {
                myResult = StatisticsByCustomerFilterComponent_1.StatisticsByCustomerFilterComponent;
                break;
            }
            case "CarrierStatisticFilterComponent": {
                myResult = CarrierStatisticFilterComponent_1.CarrierStatisticFilterComponent;
                break;
            }
            case "ContainerDetailsVoyageFilterComponent": {
                myResult = ContainerDetailsVoyageFilterComponent_1.ContainerDetailsVoyageFilterComponent;
                break;
            }
            case "ContainerTruckingFilterComponent": {
                myResult = ContainerTruckingFilterComponent_1.ContainerTruckingFilterComponent;
                break;
            }
            case "OceanShipmentReportFilterComponent": {
                myResult = OceanShipmentReportFilterComponent_1.OceanShipmentReportFilterComponent;
                break;
            }
            case "ProfitByShipmentFilterConmponent": {
                myResult = ProfitByShipmentFilterConmponent_1.ProfitByShipmentFilterConmponent;
                break;
            }
            case "AgedAccountsReceivableFilterComponent": {
                myResult = AgedAccountsReceivableFilterComponent_1.AgedAccountsReceivableFilterComponent;
                break;
            }
            case "StatementFilterComponent": {
                myResult = StatementFilterComponent_1.StatementFilterComponent;
                break;
            }
            case "StatementByInvoiceDateFilterComponent": {
                myResult = StatementByInvoiceDateFilterComponent_1.StatementByInvoiceDateFilterComponent;
                break;
            }
            case "InvoicesByPartnerFilterComponent": {
                myResult = InvoicesByPartnerFilterComponent_1.InvoicesByPartnerFilterComponent;
                break;
            }
            case "QuotesFilterComponent": {
                myResult = QuotesFilterComponent_1.QuotesFilterComponent;
                break;
            }
            case "InvoicesFilterComponent": {
                myResult = InvoicesFilterComponent_1.InvoicesFilterComponent;
                break;
            }
            case "ApprovedOpportunitiesFilterComponent": {
                myResult = ApprovedOpportunitiesFilterComponent_1.ApprovedOpportunitiesFilterComponent;
                break;
            }
            case "OpportunitiesAdditionalServicesFilterComponent": {
                myResult = OpportunitiesAdditionalServicesFilterComponent_1.OpportunitiesAdditionalServicesFilterComponent;
                break;
            }
            case "ShipmentChargesAnalysisFilterComponent": {
                myResult = ShipmentChargesAnalysisFilterComponent_1.ShipmentChargesAnalysisFilterComponent;
                break;
            }
            case "CustomerAdditionalServicesFilterComponent": {
                myResult = CustomerAdditionalServicesFilterComponent_1.CustomerAdditionalServicesFilterComponent;
                break;
            }
            case "CustomerPotentialActualFilterComponent": {
                myResult = CustomerPotentialActualFilterComponent_1.CustomerPotentialActualFilterComponent;
                break;
            }
            case "ExpectedIncomeFilterComponent": {
                myResult = ExpectedIncomeFilterComponent_1.ExpectedIncomeFilterComponent;
                break;
            }
            case "MonthlyConversionFilterComponent": {
                myResult = MonthlyConversionFilterComponent_1.MonthlyConversionFilterComponent;
                break;
            }
            case "StageChangingFilterComponent": {
                myResult = StageChangingFilterComponent_1.StageChangingFilterComponent;
                break;
            }
            case "ARInvoicesDepositReportFilterComponent": {
                myResult = ARInvoicesDepositReportFilterComponent_1.ARInvoicesDepositReportFilterComponent;
                break;
            }
            case "CASSReportFilterComponent": {
                myResult = CASSReportFilterComponent_1.CASSReportFilterComponent;
                break;
            }
            case "ShipmentProfitVSQuoteEstimateComponent": {
                myResult = ShipmentProfitVSQuoteEstimateComponent_1.ShipmentProfitVSQuoteEstimateComponent;
                break;
            }
            case "ArchivoExportadoComponent": {
                myResult = ArchivoExportadoComponent_1.ArchivoExportadoComponent;
                break;
            }
            case "AgingFilterComponent": {
                myResult = AgingFilterComponent_1.AgingFilterComponent;
                break;
            }
            case "LedgerTransactionsFilterControl": {
                myResult = LedgerTransactionsFilterControl_1.LedgerTransactionsFilterControl;
                break;
            }
            case "InventoryReportFilterConmponent": {
                myResult = InventoryReportFilterConmponent_1.InventoryReportFilterConmponent;
                break;
            }
            case "InvoicesRoutingsFilterComponent": {
                myResult = InvoicesRoutingsFilterComponent_1.InvoicesRoutingsFilterComponent;
                break;
            }
            case "EmployeeTimeSheetFilterComponent": {
                myResult = EmployeeTimeSheetFilterComponent_1.EmployeeTimeSheetFilterComponent;
                break;
            }
            case "WorkDaysPerProjectFilterComponent": {
                myResult = WorkDaysPerProjectFilterComponent_1.WorkDaysPerProjectFilterComponent;
                break;
            }
            case "TasksOfNoProjectsFilterComponent": {
                myResult = TasksOfNoProjectsFilterComponent_1.TasksOfNoProjectsFilterComponent;
                break;
            }
            case "OpenShipmentsByCustomerFilterComponent": {
                myResult = OpenShipmentsByCustomerFilterComponent_1.OpenShipmentsByCustomerFilterComponent;
                break;
            }
            case "ParentVsChildTenantsComponent": {
                myResult = ParentVsChildTenantsComponent_1.ParentVsChildTenantsComponent;
                break;
            }
            case "RevenueExpenseFilterComponent": {
                myResult = RevenueExpenseFilterComponent_1.RevenueExpenseFilterComponent;
                break;
            }
            case "TrailBalanceFiltersComponent": {
                myResult = TrailBalanceFiltersComponent_1.TrailBalanceFiltersComponent;
                break;
            }
            case "UsersByTenantReportFilterComponent": {
                myResult = UsersByTenantReportFilterComponent_1.UsersByTenantReportFilterComponent;
                break;
            }
            case "LicenseManagementFilterComponent": {
                myResult = LicenseManagementFilterComponent_1.LicenseManagementFilterComponent;
                break;
            }
            case "ShipmentsStocksFiltersComponent": {
                myResult = ShipmentsStocksFiltersComponent_1.ShipmentsStocksFiltersComponent;
                break;
            }
            case "ShipmentDetailsFilterComponent": {
                myResult = ShipmentDetailsFilterComponent_1.ShipmentDetailsFilterComponent;
                break;
            }
            case "DetailedShipmentChargesAnalysisComponent": {
                myResult = DetailedShipmentChargesAnalysisComponent_1.DetailedShipmentChargesAnalysisComponent;
                break;
            }
            case "VendorChargesAnalysisFilterComponent": {
                myResult = VendorChargesAnalysisFilterComponent_1.VendorChargesAnalysisFilterComponent;
                break;
            }
            case "VDKFilterComponent": {
                myResult = VDKFilterComponent_1.VDKFilterComponent;
                break;
            }
            case "VehiclesFilterComponent": {
                myResult = VehiclesFilterComponent_1.VehiclesFilterComponent;
                break;
            }
            case "UnicargoExportReportFilterComponent": {
                myResult = UnicargoExportReportFilterComponent_1.UnicargoExportReportFilterComponent;
                break;
            }
            case "ShipmentsEventsListFilterComponent": {
                myResult = ShipmentsEventsListFilterComponent_1.ShipmentsEventsListFilterComponent;
                break;
            }
            case "AutomationTestReportFilterComponent": {
                myResult = AutomationTestReportFilterComponent_1.AutomationTestReportFilterComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map