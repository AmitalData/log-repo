import {NewStackComponent} from './Components/Partners/AWBStock/NewStackComponent';
import {NewCurrencyComponent} from './Components/Maintenance/Currency/NewCurrencyComponent';
import {NewChargesTypeComponent} from './Components/Maintenance/ChargesType/NewChargesTypeComponent';
import {NewVatTypePercentageComponent} from './Components/Maintenance/VatType/NewVatTypePercentageComponent';
import {CustomerHelperComponent} from './Components/Helpers/CustomerHelperComponent';
import {CustomerShortTitleComponent} from './Components/ShortTitles/CustomerShortTitleComponent';
import {AssignComponent} from './Components/Partners/AWBStock/AssignComponent';
import {StackSelectionComponent} from './Components/Partners/AWBStock/StackSelectionComponent';
import {AssignToShipperComponent} from './Components/Partners/AWBStock/AssignToShipperComponent';
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {EventsTabComponent} from './Components/Events/EventsTabComponent';
import {AddEditEventComponent} from './Components/Events/AddEditEventComponent';
import {RatesHistoryComponent} from './Components/Maintenance/RatesHistoryComponent';
import {RatesMainTabComponent} from  './Components/Maintenance/RatesMainTabComponent';
import {EditLastRateComponent} from './Components/Maintenance/EditLastRateComponent';
import {LogBoxSettings} from '../Common/Components/Maintenance/LogBoxSettings';
import {TenantImportComponent} from './Components/Maintenance/TenantImportComponent';
import {MeasurementGeneralComponent} from './Components/Maintenance/Measurement/MeasurementGeneralComponent';
import {ChargesTypeGeneralTabComponent} from './Components/Maintenance/ChargesType/ChargesTypeGeneralTabComponent';
import {CreditLimitSettingsComponent} from './Components/Maintenance/CreditLimit/CreditLimitSettingsComponent';
import {CustomerActivationSettingsComponent} from '../Common/Components/Maintenance/CustomerActivationSettingsComponent';
import {CompetitorComponent} from  '../Common/Components/Maintenance/CompetitorComponent';
import {NewBusinessUnitComponent} from './Components/Maintenance/BusinessUnit/NewBusinessUnitComponent';
import {BusinessUnitGeneralTabComponent} from  './Components/Maintenance/BusinessUnit/BusinessUnitGeneralTabComponent';
import {AccountingTabComponent} from './Components/AccountingTab/AccountingTabComponent';
import {AccountingTab_ChargesType} from './Components/AccountingTab/AccountingTab_ChargesType';
import {AccountingTab_Currency} from './Components/AccountingTab/AccountingTab_Currency';
import {AccountingTab_Full} from './Components/AccountingTab/AccountingTab_Full';
import {AccountingTab_Partners} from './Components/AccountingTab/AccountingTab_Partners';
import {AccountingTab_PaymentTerm} from './Components/AccountingTab/AccountingTab_PaymentTerm';
import {AccountingTab_QuickBooksOnline} from './Components/AccountingTab/AccountingTab_QuickBooksOnline';
import {AccountingTab_VatType} from './Components/AccountingTab/AccountingTab_VatType';
import {ExternalAccountsByProductsComponent} from './Components/Partners/AddEdit/ExternalAccountsByProductsComponent';
import {CustomsInterfaceSettingsComponent} from './Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent';
import {CustomsInterfaceCredintialsComponent} from './Components/Maintenance/CustomsInterface/CustomsInterfaceCredintialsComponent';
import {FTPDetailComponent} from './Components/Maintenance/CustomsInterface/FTPDetailComponent';
import {ArtemusSettingsComponent} from './Components/Maintenance/CustomsInterface/ArtemusSettingsComponent';
import { VatTypeGeneralTabComponent } from './Components/Maintenance/VatType/VatTypeGeneralTabComponent';
import { VatTypePercentagesTabComponent } from './Components/Maintenance/VatType/VatTypePercentagesTabComponent';
import {AccountingTab_Branch} from './Components/AccountingTab/AccountingTab_Branch';
import { AddEditCustomerFieldsUpdateSettingComponent } from './Components/Maintenance/CustomerFieldsUpdateSetting/AddEditCustomerFieldsUpdateSettingComponent';
import {BranchGeneralTabComponent} from './Components/Maintenance/Branch/BranchGeneralTabComponent';
import {AddEditBranchAddressComponent} from './Components/Maintenance/Branch/AddEditBranchAddressComponent';
import {QuoteStageGeneralTabComponent} from './Components/Maintenance/QuoteStage/QuoteStageGeneralTabComponent';
import {AddEditCustomerSalesNoteComponent} from './Components/Helpers/AddEditCustomerSalesNoteComponent';
import {CustomerTenantAccessFiltersMenuComponent} from './Components/FiltersMenu/CustomerTenantAccessFiltersMenuComponent';
import {DocumentsFilingShortTitleComponent} from './Components/ShortTitles/DocumentsFilingShortTitleComponent';
import {ChargesExternalByProductsComponent} from '../Common/Components/AccountingTab/Advanced/ChargesExternalByProductsComponent';
import {DocumentFilingEmailSettingsComponent} from  './Components/Maintenance/DocumentFilingEmailSettings/DocumentFilingEmailSettingsComponent';
import {CountryCityGeneralTabComponent} from './Components/Maintenance/CountryCity/CountryCityGeneralTabComponent';
import {VesselGeneralTabComponent} from './Components/Maintenance/Vessel/VesselGeneralTabComponent';
import {CustomsShipperFiltersMenuComponent} from './Components/FiltersMenu/CustomsShipperFiltersMenuComponent';
export const Components =
    [
        CustomerHelperComponent,
        CustomerShortTitleComponent,
        AssignComponent,
        NewStackComponent,
        StackSelectionComponent,
        AssignToShipperComponent,
        FieldTemplateComponent,
        EventsTabComponent,
        AddEditEventComponent,
        RatesHistoryComponent,
        RatesMainTabComponent,
        EditLastRateComponent,        
        LogBoxSettings,
        TenantImportComponent,       
        MeasurementGeneralComponent,
        NewCurrencyComponent,
        NewChargesTypeComponent,
        ChargesTypeGeneralTabComponent,
        NewVatTypePercentageComponent,        
        CreditLimitSettingsComponent,
        CompetitorComponent,        
        NewBusinessUnitComponent,
        BusinessUnitGeneralTabComponent,
        CustomerActivationSettingsComponent,
        AccountingTabComponent,
        AccountingTab_ChargesType,
        AccountingTab_Currency,
        AccountingTab_Full,
        AccountingTab_Partners,
        AccountingTab_PaymentTerm,
        AccountingTab_QuickBooksOnline,
        AccountingTab_VatType,
        ExternalAccountsByProductsComponent,        
        CustomsInterfaceSettingsComponent,
        CustomsInterfaceCredintialsComponent,
        FTPDetailComponent,
        ArtemusSettingsComponent,
        VatTypeGeneralTabComponent,
        VatTypePercentagesTabComponent,
        AccountingTab_Branch,
        AddEditCustomerFieldsUpdateSettingComponent,
        BranchGeneralTabComponent,
        VesselGeneralTabComponent,
        AddEditBranchAddressComponent,        
        QuoteStageGeneralTabComponent,   
        AddEditCustomerSalesNoteComponent,
        CustomerTenantAccessFiltersMenuComponent,
        DocumentsFilingShortTitleComponent,
        ChargesExternalByProductsComponent,
        CountryCityGeneralTabComponent,
        DocumentFilingEmailSettingsComponent,
        CustomsShipperFiltersMenuComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomerHelperComponent": { myResult = CustomerHelperComponent; break; }
            case "CustomerShortTitleComponent": { myResult = CustomerShortTitleComponent; break; }
            case "AssignComponent": { myResult = AssignComponent; break; }
            case "NewStackComponent": { myResult = NewStackComponent; break; }
            case "StackSelectionComponent": { myResult = StackSelectionComponent; break; }
            case "AssignToShipperComponent": { myResult = AssignToShipperComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "EventsTabComponent": { myResult = EventsTabComponent; break; }
            case "AddEditEventComponent": { myResult = AddEditEventComponent; break; }
            case "RatesHistoryComponent": { myResult = RatesHistoryComponent; break; }
            case "RatesMainTabComponent": { myResult = RatesMainTabComponent; break; }
            case "EditLastRateComponent": { myResult = EditLastRateComponent; break; }            
            case "LogBoxSettings": { myResult = LogBoxSettings; break; }
            case "TenantImportComponent": { myResult = TenantImportComponent; break; }
            case "MeasurementGeneralComponent": { myResult = MeasurementGeneralComponent; break; }
            case "NewCurrencyComponent": { myResult = NewCurrencyComponent; break; }
            case "NewChargesTypeComponent": { myResult = NewChargesTypeComponent; break; }
            case "ChargesTypeGeneralTabComponent": { myResult = ChargesTypeGeneralTabComponent; break; }                        
            case "NewVatTypePercentageComponent": { myResult = NewVatTypePercentageComponent; break; }
            case "CreditLimitSettingsComponent": { myResult = CreditLimitSettingsComponent; break; }
            case "CustomerActivationSettingsComponent": { myResult = CustomerActivationSettingsComponent; break }           
            case "CompetitorComponent": { myResult = CompetitorComponent; break }            
            case "CreditLimitSettingsComponent": { myResult = CreditLimitSettingsComponent; break; }
            case "NewBusinessUnitComponent": { myResult = NewBusinessUnitComponent; break; }
            case "BusinessUnitGeneralTabComponent": { myResult = BusinessUnitGeneralTabComponent; break; }
            case "CreditLimitSettingsComponent": { myResult = CreditLimitSettingsComponent; break; }
            case "AccountingTabComponent": { myResult = AccountingTabComponent; break; }
            case "AccountingTab_ChargesType": { myResult = AccountingTab_ChargesType; break; }
            case "AccountingTab_Currency": { myResult = AccountingTab_Currency; break; }
            case "AccountingTab_Full": { myResult = AccountingTab_Full; break; }
            case "AccountingTab_Partners": { myResult = AccountingTab_Partners; break; }
            case "AccountingTab_PaymentTerm": { myResult = AccountingTab_PaymentTerm; break; }
            case "AccountingTab_QuickBooksOnline": { myResult = AccountingTab_QuickBooksOnline; break; }
            case "AccountingTab_VatType": { myResult = AccountingTab_VatType; break; }
            case "ExternalAccountsByProductsComponent": { myResult = ExternalAccountsByProductsComponent; break; }
            case "CustomsInterfaceSettingsComponent": { myResult = CustomsInterfaceSettingsComponent; break; }
            case "CustomsInterfaceCredintialsComponent": { myResult = CustomsInterfaceCredintialsComponent; break; }
            case "FTPDetailComponent": { myResult = FTPDetailComponent; break; }
            case "ArtemusSettingsComponent": { myResult = ArtemusSettingsComponent; break; }
            case "VatTypeGeneralTabComponent": { myResult = VatTypeGeneralTabComponent; break; }
            case "VatTypePercentagesTabComponent": { myResult = VatTypePercentagesTabComponent; break; }
            case "AccountingTab_Branch": { myResult = AccountingTab_Branch; break; }
            case "AddEditCustomerFieldsUpdateSettingComponent": { myResult = AddEditCustomerFieldsUpdateSettingComponent; break; }
            case "BranchGeneralTabComponent": { myResult = BranchGeneralTabComponent; break; }
            case "AddEditBranchAddressComponent": { myResult = AddEditBranchAddressComponent; break; }
            case "VesselGeneralTabComponent": { myResult = VesselGeneralTabComponent; break; }           
            case "QuoteStageGeneralTabComponent": { myResult = QuoteStageGeneralTabComponent; break; }
            case "AddEditCustomerSalesNoteComponent": { myResult = AddEditCustomerSalesNoteComponent; break; }
            case "CustomerTenantAccessFiltersMenuComponent": { myResult = CustomerTenantAccessFiltersMenuComponent; break; }
            case "DocumentsFilingShortTitleComponent": { myResult = DocumentsFilingShortTitleComponent; break; }
            case "ChargesExternalByProductsComponent": { myResult = ChargesExternalByProductsComponent; break; }
            case "CountryCityGeneralTabComponent": { myResult = CountryCityGeneralTabComponent; break; }
            case "DocumentFilingEmailSettingsComponent": { myResult = DocumentFilingEmailSettingsComponent; break; }
            case "CustomsShipperFiltersMenuComponent": { myResult = CustomsShipperFiltersMenuComponent; break; }

                
        }

        return myResult;
    }
}
