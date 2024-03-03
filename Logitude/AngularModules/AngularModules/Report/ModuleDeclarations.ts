import { CustomerStatusReportFilterComponent } from './Components/FiltersComponent/Accounting/CustomerStatusReportFilterComponent';
import { ReportComponent } from './Components/Workspaces/ReportComponent';
import { MainReportsWorkspace } from './Components/Workspaces/MainReportsWorkspace';
import { MainReportSchedulerComponent } from './Components/Scheduler/MainReportSchedulerComponent';
import { TaskReportSchedulerComponent } from './Components/Scheduler/TaskReportSchedulerComponent';
import { AddEditReportTaskSchedulerComponent } from './Components/Scheduler/AddEditReportTaskSchedulerComponent';
import { AddEditReportSchedulerComponent } from './Components/Scheduler/AddEditReportSchedulerComponent';
import { ReportSchedulerDateListTemplate } from './Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate';
import { BIReportComponent } from './Components/Workspaces/BIReportComponent';
import { BIFolderReportComponent } from './Components/Workspaces/BIFolderReportComponent';
import {ReportTemplateComponent} from './Components/ReportTemplateComponent';
import {ReportsPreviewComponent} from './Components/ReportsPreviewComponent';
import {ReportsTemplateRestoreComponent} from './Components/ReportsTemplateRestoreComponent';
import {NewReportsTemplateComponent} from './Components/NewReportsTemplateComponent';

// Statisticsss
import {ActivityStatusDashboardFilterComponent} from './Components/FilterReportComponent/ActivityStatusDashboardFilterComponent';
import {EAWBFilterComponent} from './Components/FilterReportComponent/EAWBFilterComponent';
import {BookingFilterComponent} from './Components/FilterReportComponent/BookingFilterComponent';
import {FlightBookingFilterComponent} from './Components/FilterReportComponent/FlightBookingFilterComponent';
import {IATAStatisticsFilterComponent} from './Components/FilterReportComponent/IATAStatisticsFilterComponent';
import {ParticipantsUsersActivitiesFilterComponent} from './Components/FilterReportComponent/ParticipantsUsersActivitiesFilterComponent';
import {StatisticsByAgentFilterComponent} from './Components/FilterReportComponent/StatisticsByAgentFilterComponent';
import {CarrierStatisticFilterComponent} from './Components/FilterReportComponent/CarrierStatisticFilterComponent';
import {StatisticsByCustomerFilterComponent} from './Components/FilterReportComponent/StatisticsByCustomerFilterComponent';

// Operational
import {CASSReportFilterComponent} from './Components/FilterReportComponent/CASSReportFilterComponent';
import {ContainerDetailsVoyageFilterComponent} from './Components/FilterReportComponent/ContainerDetailsVoyageFilterComponent';
import {ContainerTruckingFilterComponent} from './Components/FilterReportComponent/ContainerTruckingFilterComponent';
import {OceanShipmentReportFilterComponent} from './Components/FilterReportComponent/OceanShipmentReportFilterComponent';
import {ProfitByShipmentFilterConmponent} from './Components/FilterReportComponent/ProfitByShipmentFilterConmponent';
import {InventoryReportFilterConmponent} from './Components/FilterReportComponent/InventoryReportFilterConmponent';
import {OpenShipmentsByCustomerFilterComponent} from './Components/FilterReportComponent/OpenShipmentsByCustomerFilterComponent';
import { UnicargoExportReportFilterComponent } from './Components/FiltersComponent/Operational/UnicargoExportReportFilterComponent';
import { ShipmentsEventsListFilterComponent } from './Components/FiltersComponent/Operational/ShipmentsEventsListFilterComponent';
import { ShipperReturnsReportFilterComponent } from './Components/FiltersComponent/Operational/ShipperReturnsReportFilterComponent';
import { FlightBookingsManifestFilterComponent } from './Components/FiltersComponent/Operational/FlightBookingsManifestFilterComponent';

// Accounting
import {AccountingLedgerFilterComponent} from './Components/FilterReportComponent/AccountingLedgerFilterComponent';
import {AgedAccountsReceivableFilterComponent} from './Components/FilterReportComponent/AgedAccountsReceivableFilterComponent';
import {InvoicesFilterComponent} from './Components/FilterReportComponent/InvoicesFilterComponent';
import {ARInvoicesDepositReportFilterComponent} from './Components/FilterReportComponent/ARInvoicesDepositReportFilterComponent';
import {ShipmentChargesAnalysisFilterComponent} from './Components/FilterReportComponent/ShipmentChargesAnalysisFilterComponent';
import {StatementFilterComponent} from './Components/FilterReportComponent/StatementFilterComponent';
import {StatementByInvoiceDateFilterComponent} from './Components/FilterReportComponent/StatementByInvoiceDateFilterComponent';
import {InvoicesByPartnerFilterComponent} from './Components/FilterReportComponent/InvoicesByPartnerFilterComponent';
import {ArchivoExportadoComponent} from './Components/FiltersComponent/Accounting/ArchivoExportadoComponent';
import {InvoicesRoutingsFilterComponent} from './Components/FiltersComponent/Accounting/InvoicesRoutingsFilterComponent';
import { LedgerTransactionsFilterControl } from './Components/FiltersComponent/Accounting/LedgerTransactionsFilterControl';
import {AgingFilterComponent} from './Components/FiltersComponent/Accounting/AgingFilterComponent';
import { RevenueExpenseFilterComponent } from './Components/FilterReportComponent/RevenueExpenseFilterComponent';
import { TrailBalanceFiltersComponent } from './Components/FilterReportComponent/TrailBalanceFiltersComponent';
import { ShipmentsStocksFiltersComponent } from './Components/FilterReportComponent/ShipmentsStocksFiltersComponent';
import { DetailedShipmentChargesAnalysisComponent } from './Components/FiltersComponent/Accounting/DetailedShipmentChargesAnalysisComponent';
import { VendorChargesAnalysisFilterComponent } from './Components/FiltersComponent/Accounting/VendorChargesAnalysisFilterComponent';
import { AutomationTestReportFilterComponent } from './Components/FilterReportComponent/AutomationTestReportFilterComponent';
import { ExternalReconciliationLinesReportFilterControl } from './Components/FilterReportComponent/ExternalReconciliationLinesReportFilterControl';
import { PerVendorReportFilterComponent } from './Components/FiltersComponent/Accounting/PerVendorReportFilterComponent';
import { ARinvoiceSequencesReportFilterComponent } from './Components/FiltersComponent/Accounting/ARinvoiceSequencesReportFilterComponent';
import { ControlInvoiceLinesReportFilterComponent } from './Components/FiltersComponent/Accounting/ControlInvoiceLinesReportFilterComponent';

// Quotes
import {QuotesFilterComponent} from './Components/FilterReportComponent/QuotesFilterComponent';
import {RacingQuotesComponent} from './Components/FiltersComponent/CRM/RacingQuotesComponent';



// CRM
import {ApprovedOpportunitiesFilterComponent} from './Components/FilterReportComponent/ApprovedOpportunitiesFilterComponent';
import {CustomerAdditionalServicesFilterComponent} from './Components/FilterReportComponent/CustomerAdditionalServicesFilterComponent';
import {CustomerPotentialActualFilterComponent} from './Components/FilterReportComponent/CustomerPotentialActualFilterComponent';
import {ExpectedIncomeFilterComponent} from './Components/FilterReportComponent/ExpectedIncomeFilterComponent';
import {MonthlyConversionFilterComponent} from './Components/FilterReportComponent/MonthlyConversionFilterComponent';
import {OpportunitiesAdditionalServicesFilterComponent} from './Components/FilterReportComponent/OpportunitiesAdditionalServicesFilterComponent';
import {StageChangingFilterComponent} from './Components/FilterReportComponent/StageChangingFilterComponent';
import {ShipmentProfitVSQuoteEstimateComponent} from './Components/FiltersComponent/CRM/ShipmentProfitVSQuoteEstimateComponent';
import {ParentVsChildTenantsComponent} from './Components/FilterReportComponent/ParentVsChildTenantsComponent';
import {UsersByTenantReportFilterComponent} from './Components/FiltersComponent/CRM/UsersByTenantReportFilterComponent';
import {LicenseManagementFilterComponent} from './Components/FiltersComponent/Operational/LicenseManagementFilterComponent';
import { VehiclesFilterComponent } from './Components/FiltersComponent/Operational/VehiclesFilterComponent';
import { BluesnapPaymentsReportFilterComponent } from './Components/FiltersComponent/CRM/BluesnapPaymentsReportFilterComponent';

// Time Sheet
import {EmployeeTimeSheetFilterComponent} from './Components/FilterReportComponent/EmployeeTimeSheetFilterComponent';
import {WorkDaysPerProjectFilterComponent} from './Components/FilterReportComponent/WorkDaysPerProjectFilterComponent';
import { TasksOfNoProjectsFilterComponent } from './Components/FilterReportComponent/TasksOfNoProjectsFilterComponent';
import { WorkDaysPerCategoryFilterComponent } from './Components/FilterReportComponent/WorkDaysPerCategoryFilterComponent';

//Shipment Details
import { ShipmentDetailsFilterComponent } from './Components/FilterReportComponent/ShipmentDetailsFilterComponent';

//VDK Templates
import { VDKFilterComponent } from './Components/FilterReportComponent/VDKFilterComponent';
import { UserDefinedReportFilterControl } from './Components/FilterReportComponent/UserDefinedReportFilterControl';
import { LogitudeCRMReportFilterComponent } from './Components/FiltersComponent/CRM/LogitudeCRMReportFilterComponent';
import { ExcelReportTemplateComponent } from './Components/ExcelReportTemplateComponent';
import { SpotRateQuoteReportFilterComponent } from './Components/FiltersComponent/Quote/SpotRate/SpotRateQuoteReportFilterComponent';
import { ReportVariablesComponent } from './Components/ReportVariablesComponent';

export const Components =
    [
        MainReportsWorkspace,
        MainReportSchedulerComponent,
        TaskReportSchedulerComponent,
        AddEditReportTaskSchedulerComponent,
        AddEditReportSchedulerComponent,
        ReportSchedulerDateListTemplate,
        ReportComponent,
        BIReportComponent,
        BIFolderReportComponent,
        ReportTemplateComponent,
        ReportVariablesComponent,
        ReportsPreviewComponent,
        ReportsTemplateRestoreComponent,
        NewReportsTemplateComponent,
        IATAStatisticsFilterComponent,
        AccountingLedgerFilterComponent,
        FlightBookingFilterComponent,
        ParticipantsUsersActivitiesFilterComponent,
        ActivityStatusDashboardFilterComponent,
        EAWBFilterComponent,
        BookingFilterComponent,
        StatisticsByAgentFilterComponent,
        StatisticsByCustomerFilterComponent,
        CarrierStatisticFilterComponent,
        ContainerDetailsVoyageFilterComponent,
        ContainerTruckingFilterComponent,
        OceanShipmentReportFilterComponent,
        ProfitByShipmentFilterConmponent,
        AgedAccountsReceivableFilterComponent,
        StatementFilterComponent,
        StatementByInvoiceDateFilterComponent,
        InvoicesByPartnerFilterComponent,
        QuotesFilterComponent,
        InvoicesFilterComponent,
        ApprovedOpportunitiesFilterComponent,
        OpportunitiesAdditionalServicesFilterComponent,
        ShipmentChargesAnalysisFilterComponent,
        CustomerAdditionalServicesFilterComponent,
        CustomerPotentialActualFilterComponent,
        ExpectedIncomeFilterComponent,
        MonthlyConversionFilterComponent,
        StageChangingFilterComponent,
        ARInvoicesDepositReportFilterComponent,
        CASSReportFilterComponent,
        ShipmentProfitVSQuoteEstimateComponent,
        ArchivoExportadoComponent,
        AgingFilterComponent,
        CustomerStatusReportFilterComponent,
        LedgerTransactionsFilterControl,
        InventoryReportFilterConmponent,
        InvoicesRoutingsFilterComponent,
        EmployeeTimeSheetFilterComponent,
        WorkDaysPerProjectFilterComponent,
        WorkDaysPerCategoryFilterComponent,
        TasksOfNoProjectsFilterComponent,
        OpenShipmentsByCustomerFilterComponent,
        ParentVsChildTenantsComponent,
        RevenueExpenseFilterComponent,
        TrailBalanceFiltersComponent,
        UsersByTenantReportFilterComponent,
        LicenseManagementFilterComponent,
        ShipmentsStocksFiltersComponent,
        ShipmentDetailsFilterComponent,
        DetailedShipmentChargesAnalysisComponent,
        VendorChargesAnalysisFilterComponent,
        VDKFilterComponent,
        VehiclesFilterComponent,
        UnicargoExportReportFilterComponent,
        ShipmentsEventsListFilterComponent,
        AutomationTestReportFilterComponent,
        ShipperReturnsReportFilterComponent,
        FlightBookingsManifestFilterComponent,
        RacingQuotesComponent,
        BluesnapPaymentsReportFilterComponent,
        ExternalReconciliationLinesReportFilterControl,
        UserDefinedReportFilterControl,
        LogitudeCRMReportFilterComponent,
        ExcelReportTemplateComponent,
        PerVendorReportFilterComponent,
        ARinvoiceSequencesReportFilterComponent,
        SpotRateQuoteReportFilterComponent,
        ControlInvoiceLinesReportFilterComponent

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "MainReportsWorkspace": { myResult = MainReportsWorkspace; break; }
            case "MainReportSchedulerComponent": { myResult = MainReportSchedulerComponent; break; }
            case "TaskReportSchedulerComponent": { myResult = TaskReportSchedulerComponent; break; }
            case "AddEditReportTaskSchedulerComponent": { myResult = AddEditReportTaskSchedulerComponent; break; }
            case "AddEditReportSchedulerComponent": { myResult = AddEditReportSchedulerComponent; break; }
            case "ReportSchedulerDateListTemplate": { myResult = ReportSchedulerDateListTemplate; break; }
            case "ReportComponent": { myResult = ReportComponent; break; }
            case "BIReportComponent": { myResult = BIReportComponent; break; }
            case "BIFolderReportComponent": { myResult = BIFolderReportComponent; break; }
            case "ReportsPreviewComponent": { myResult = ReportsPreviewComponent; break; }
            case "ReportTemplateComponent": { myResult = ReportTemplateComponent; break; }
            case "ReportVariablesComponent": { myResult = ReportVariablesComponent; break; }
            case "ReportsTemplateRestoreComponent": { myResult = ReportsTemplateRestoreComponent; break; }
            case "NewReportsTemplateComponent": { myResult = NewReportsTemplateComponent; break; }
            case "IATAStatisticsFilterComponent": { myResult = IATAStatisticsFilterComponent; break; }
            case "AccountingLedgerFilterComponent": { myResult = AccountingLedgerFilterComponent; break; }
            case "FlightBookingFilterComponent": { myResult = FlightBookingFilterComponent; break; }
            case "ParticipantsUsersActivitiesFilterComponent": { myResult = ParticipantsUsersActivitiesFilterComponent; break; }
            case "ActivityStatusDashboardFilterComponent": { myResult = ActivityStatusDashboardFilterComponent; break; }
            case "EAWBFilterComponent": { myResult = EAWBFilterComponent; break; }
            case "BookingFilterComponent": { myResult = BookingFilterComponent; break; }
            case "StatisticsByAgentFilterComponent": { myResult = StatisticsByAgentFilterComponent; break; }
            case "StatisticsByCustomerFilterComponent": { myResult = StatisticsByCustomerFilterComponent; break; }
            case "CarrierStatisticFilterComponent": { myResult = CarrierStatisticFilterComponent; break; }
            case "ContainerDetailsVoyageFilterComponent": { myResult = ContainerDetailsVoyageFilterComponent; break; }
            case "ContainerTruckingFilterComponent": { myResult = ContainerTruckingFilterComponent; break; }
            case "OceanShipmentReportFilterComponent": { myResult = OceanShipmentReportFilterComponent; break; }
            case "ProfitByShipmentFilterConmponent": { myResult = ProfitByShipmentFilterConmponent; break; }
            case "AgedAccountsReceivableFilterComponent": { myResult = AgedAccountsReceivableFilterComponent; break; }
            case "StatementFilterComponent": { myResult = StatementFilterComponent; break; }
            case "StatementByInvoiceDateFilterComponent": { myResult = StatementByInvoiceDateFilterComponent; break; }
            case "InvoicesByPartnerFilterComponent": { myResult = InvoicesByPartnerFilterComponent; break; }
            case "QuotesFilterComponent": { myResult = QuotesFilterComponent; break; }
            case "InvoicesFilterComponent": { myResult = InvoicesFilterComponent; break; }
            case "ApprovedOpportunitiesFilterComponent": { myResult = ApprovedOpportunitiesFilterComponent; break; }
            case "OpportunitiesAdditionalServicesFilterComponent": { myResult = OpportunitiesAdditionalServicesFilterComponent; break; }
            case "ShipmentChargesAnalysisFilterComponent": { myResult = ShipmentChargesAnalysisFilterComponent; break; }
            case "CustomerAdditionalServicesFilterComponent": { myResult = CustomerAdditionalServicesFilterComponent; break; }
            case "CustomerPotentialActualFilterComponent": { myResult = CustomerPotentialActualFilterComponent; break; }
            case "ExpectedIncomeFilterComponent": { myResult = ExpectedIncomeFilterComponent; break; }
            case "MonthlyConversionFilterComponent": { myResult = MonthlyConversionFilterComponent; break; }
            case "StageChangingFilterComponent": { myResult = StageChangingFilterComponent; break; }
            case "ARInvoicesDepositReportFilterComponent": { myResult = ARInvoicesDepositReportFilterComponent; break; }
            case "CASSReportFilterComponent": { myResult = CASSReportFilterComponent; break; }
            case "ShipmentProfitVSQuoteEstimateComponent": { myResult = ShipmentProfitVSQuoteEstimateComponent; break; }
            case "ArchivoExportadoComponent": { myResult = ArchivoExportadoComponent; break; }
            case "AgingFilterComponent": { myResult = AgingFilterComponent; break; }
            case "CustomerStatusReportFilterComponent": { myResult = CustomerStatusReportFilterComponent; break; }
            case "LedgerTransactionsFilterControl": { myResult = LedgerTransactionsFilterControl; break; }
            case "InventoryReportFilterConmponent": { myResult = InventoryReportFilterConmponent; break; }
            case "InvoicesRoutingsFilterComponent": { myResult = InvoicesRoutingsFilterComponent; break; }
            case "EmployeeTimeSheetFilterComponent": { myResult = EmployeeTimeSheetFilterComponent; break; }
            case "WorkDaysPerProjectFilterComponent": { myResult = WorkDaysPerProjectFilterComponent; break; }
            case "WorkDaysPerCategoryFilterComponent": { myResult = WorkDaysPerCategoryFilterComponent; break; }
            case "TasksOfNoProjectsFilterComponent": { myResult = TasksOfNoProjectsFilterComponent; break; }
            case "OpenShipmentsByCustomerFilterComponent": { myResult = OpenShipmentsByCustomerFilterComponent; break; }
            case "ParentVsChildTenantsComponent": { myResult = ParentVsChildTenantsComponent; break; }
            case "RevenueExpenseFilterComponent": { myResult = RevenueExpenseFilterComponent; break; }
            case "TrailBalanceFiltersComponent": { myResult = TrailBalanceFiltersComponent; break; }
            case "UsersByTenantReportFilterComponent": { myResult = UsersByTenantReportFilterComponent; break; }
            case "LicenseManagementFilterComponent": { myResult = LicenseManagementFilterComponent; break; }
            case "ShipmentsStocksFiltersComponent": { myResult = ShipmentsStocksFiltersComponent; break; }
            case "ShipmentDetailsFilterComponent": { myResult = ShipmentDetailsFilterComponent; break; }
            case "DetailedShipmentChargesAnalysisComponent": { myResult = DetailedShipmentChargesAnalysisComponent; break; }
            case "VendorChargesAnalysisFilterComponent": { myResult = VendorChargesAnalysisFilterComponent; break; }
            case "VDKFilterComponent": { myResult = VDKFilterComponent; break; }
            case "VehiclesFilterComponent": { myResult = VehiclesFilterComponent; break; }
            case "UnicargoExportReportFilterComponent": { myResult = UnicargoExportReportFilterComponent; break; }
            case "ShipmentsEventsListFilterComponent": { myResult = ShipmentsEventsListFilterComponent; break; }
            case "AutomationTestReportFilterComponent": { myResult = AutomationTestReportFilterComponent; break; }
            case "ShipperReturnsReportFilterComponent": { myResult = ShipperReturnsReportFilterComponent; break; }
            case "FlightBookingsManifestFilterComponent": { myResult = FlightBookingsManifestFilterComponent; break; }
            case "RacingQuotesComponent": { myResult = RacingQuotesComponent; break; }
            case "BluesnapPaymentsReportFilterComponent": { myResult = BluesnapPaymentsReportFilterComponent; break; }
            case "ExternalReconciliationLinesReportFilterControl": { myResult = ExternalReconciliationLinesReportFilterControl; break; }
            case "UserDefinedReportFilterControl": { myResult = UserDefinedReportFilterControl; break; }
            case "LogitudeCRMReportFilterComponent": { myResult = LogitudeCRMReportFilterComponent; break; }
            case "ExcelReportTemplateComponent": { myResult = ExcelReportTemplateComponent; break; }
            case "PerVendorReportFilterComponent": { myResult = PerVendorReportFilterComponent; break; }
            case "ARinvoiceSequencesReportFilterComponent": { myResult = ARinvoiceSequencesReportFilterComponent; break; }
            case "SpotRateQuoteReportFilterComponent": { myResult = SpotRateQuoteReportFilterComponent; break; }
            case "ControlInvoiceLinesReportFilterComponent": { myResult = ControlInvoiceLinesReportFilterComponent; break; }

        }

        return myResult;
    }
}
