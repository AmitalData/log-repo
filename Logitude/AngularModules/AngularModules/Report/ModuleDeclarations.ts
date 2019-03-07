import { ReportComponent } from './Components/Workspaces/ReportComponent';
import { MainReportsWorkspace } from './Components/Workspaces/MainReportsWorkspace';
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
import {AgingFilterComponent} from './Components/FiltersComponent/Accounting/AgingFilterComponent';
import { RevenueExpenseFilterComponent } from './Components/FilterReportComponent/RevenueExpenseFilterComponent';
import { TrailBalanceFiltersComponent } from './Components/FilterReportComponent/TrailBalanceFiltersComponent';
import { ShipmentsStocksFiltersComponent } from './Components/FilterReportComponent/ShipmentsStocksFiltersComponent';
import { DetailedShipmentChargesAnalysisComponent } from './Components/FiltersComponent/Accounting/DetailedShipmentChargesAnalysisComponent';
import { VendorChargesAnalysisFilterComponent } from './Components/FiltersComponent/Accounting/VendorChargesAnalysisFilterComponent';

// Quotes
import {QuotesFilterComponent} from './Components/FilterReportComponent/QuotesFilterComponent';

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

// Time Sheet
import {EmployeeTimeSheetFilterComponent} from './Components/FilterReportComponent/EmployeeTimeSheetFilterComponent';
import {WorkDaysPerProjectFilterComponent} from './Components/FilterReportComponent/WorkDaysPerProjectFilterComponent';
import { TasksOfNoProjectsFilterComponent } from './Components/FilterReportComponent/TasksOfNoProjectsFilterComponent';

//Shipment Details
import { ShipmentDetailsFilterComponent } from './Components/FilterReportComponent/ShipmentDetailsFilterComponent';


//VDK Templates
import { VDKFilterComponent } from './Components/FilterReportComponent/VDKFilterComponent';

export const Components =
    [
        MainReportsWorkspace,
        ReportComponent,
        BIReportComponent,
        BIFolderReportComponent,
        ReportTemplateComponent,
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
        InventoryReportFilterConmponent,
        InvoicesRoutingsFilterComponent,
        EmployeeTimeSheetFilterComponent,
        WorkDaysPerProjectFilterComponent,
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
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "MainReportsWorkspace": { myResult = MainReportsWorkspace; break; }
            case "ReportComponent": { myResult = ReportComponent; break; }
            case "BIReportComponent": { myResult = BIReportComponent; break; }
            case "BIFolderReportComponent": { myResult = BIFolderReportComponent; break; }
            case "ReportsPreviewComponent": { myResult = ReportsPreviewComponent; break; }
            case "ReportTemplateComponent": { myResult = ReportTemplateComponent; break; }     
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
            case "InventoryReportFilterConmponent": { myResult = InventoryReportFilterConmponent; break; } 
            case "InvoicesRoutingsFilterComponent": { myResult = InvoicesRoutingsFilterComponent; break; }
            case "EmployeeTimeSheetFilterComponent": { myResult = EmployeeTimeSheetFilterComponent; break; }  
            case "WorkDaysPerProjectFilterComponent": { myResult = WorkDaysPerProjectFilterComponent; break; }
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
        }

        return myResult;
    }
}
