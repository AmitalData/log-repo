import {GettingStartedComponent} from './Components/Workspaces/GettingStartedComponent';
import {SystemDefaultsComponent} from './Components/SystemDefaults/SystemDefaultsComponent';
import {CompanyAddressSettingsComponent} from  './Components/CompanyAddress/CompanyAddressSettingsComponent';
import {SystemCurrenciesComponent} from './Components/SystemCurrencies/SystemCurrenciesComponent';
import {CurrencyRatesComponent} from './Components/SystemCurrencies/CurrencyRatesComponent';
import {CountersComponent} from './Components/Counters/CountersComponent';
import {CounterHAWBComponent} from './Components/Counters/EditComponents/CounterHAWBComponent';
import {CounterTableComponent} from './Components/Counters/EditComponents/CounterTableComponent';
import {CounterInvoiceComponent} from './Components/Counters/EditComponents/CounterInvoiceComponent';
import {CounterAdvancedComponent} from './Components/Counters/EditComponents/CounterAdvancedComponent';
import {AccountingSettingsComponent} from './Components/AccountingSettings/AccountingSettingsComponent';
import {AccountingAdvancedSettingsComponent} from './Components/AccountingSettings/AccountingAdvancedSettingsComponent';
import {LocalSettingsComponent} from  './Components/LocalSettings/LocalSettingsComponent';
import {InvoiceSettingsComponent} from './Components/InvoiceSettings/InvoiceSettingsComponent';
import {AirlineSettingsComponent} from './Components/AirlineSettings/AirlineSettingsComponent';
import {UploadLogoComponent} from './Components/UploadImage/UploadLogoComponent';

export const Components =
    [
        GettingStartedComponent,
        SystemDefaultsComponent,
        CompanyAddressSettingsComponent,
        SystemCurrenciesComponent,
        CurrencyRatesComponent,
        CountersComponent,
        CounterHAWBComponent,
        CounterTableComponent,
        CounterInvoiceComponent,
        CounterAdvancedComponent,
        AccountingSettingsComponent,
        AccountingAdvancedSettingsComponent,
        LocalSettingsComponent,
        InvoiceSettingsComponent,  
        AirlineSettingsComponent,
        UploadLogoComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "GettingStartedComponent": { myResult = GettingStartedComponent; break; }
            case "SystemDefaultsComponent": { myResult = SystemDefaultsComponent; break; }
            case "CompanyAddressSettingsComponent": { myResult = CompanyAddressSettingsComponent; break; }
            case "SystemCurrenciesComponent": { myResult = SystemCurrenciesComponent; break; }  
            case "CurrencyRatesComponent": { myResult = CurrencyRatesComponent; break; }
            case "CountersComponent": { myResult = CountersComponent; break; }
            case "CounterHAWBComponent": { myResult = CounterHAWBComponent; break; }
            case "CounterTableComponent": { myResult = CounterTableComponent; break; }
            case "CounterInvoiceComponent": { myResult = CounterInvoiceComponent; break; }
            case "CounterAdvancedComponent": { myResult = CounterAdvancedComponent; break; }
            case "AccountingSettingsComponent": { myResult = AccountingSettingsComponent; break; }
            case "AccountingAdvancedSettingsComponent": { myResult = AccountingAdvancedSettingsComponent; break; }
            case "LocalSettingsComponent": { myResult = LocalSettingsComponent; break; }
            case "InvoiceSettingsComponent": { myResult = InvoiceSettingsComponent; break; }
            case "AirlineSettingsComponent": { myResult = AirlineSettingsComponent; break; }
            case "UploadLogoComponent": { myResult = UploadLogoComponent; break; }

        }

        return myResult;
    }
}