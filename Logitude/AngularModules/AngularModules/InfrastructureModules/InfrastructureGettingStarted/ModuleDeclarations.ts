import {GettingStartedComponent} from './Components/Workspaces/GettingStartedComponent';
import {CompanyAddressSettingsComponent} from  './Components/CompanyAddress/CompanyAddressSettingsComponent';
import {SystemCurrenciesComponent} from './Components/SystemCurrencies/SystemCurrenciesComponent';
import {CurrencyRatesComponent} from './Components/SystemCurrencies/CurrencyRatesComponent';
import {CountersComponent} from './Components/Counters/CountersComponent';
import {CounterHAWBComponent} from './Components/Counters/EditComponents/CounterHAWBComponent';
import {CounterTableComponent} from './Components/Counters/EditComponents/CounterTableComponent';
import {CounterInvoiceComponent} from './Components/Counters/EditComponents/CounterInvoiceComponent';
import {CounterAdvancedComponent} from './Components/Counters/EditComponents/CounterAdvancedComponent';
import {AccountingSettingsComponent} from './Components/AccountingSettings/AccountingSettingsComponent';
import { AccountingAdvancedSettingsComponent } from './Components/AccountingSettings/AccountingAdvancedSettingsComponent';
import { AccountingAdvancedAPSettingsComponent } from './Components/AccountingSettings/AccountingAdvancedAPSettingsComponent';
import {LocalSettingsComponent} from  './Components/LocalSettings/LocalSettingsComponent';
import {InvoiceSettingsComponent} from './Components/InvoiceSettings/InvoiceSettingsComponent';
import {AirlineSettingsComponent} from './Components/AirlineSettings/AirlineSettingsComponent';
import {UploadLogoComponent} from './Components/UploadImage/UploadLogoComponent';
import { PaymentGatewayComponent } from './Components/PaymentGateway/PaymentGatewayComponent';
import { ChangeCurrencyComponent } from './Components/SystemCurrencies/ChangeCurrencyComponent';
import { SystemDefaultsComponent } from './Components/SystemDefaults/SystemDefaultsComponent';
import { DefaultRatiosComponent } from './Components/SystemDefaults/DefaultRatiosComponent';
 import { ShaamTokensComponent } from './Components/ShaamSettings/shaamTokensComponent';
 import { CustomizedARInvoiceCounterComponent } from './Components/Counters/EditComponents/CustomizedARInvoiceCounterComponent';
  
 import {ReleaseSettingsComponent} from './Components/Workspaces/ReleaseSettingsComponent';

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
        AccountingAdvancedAPSettingsComponent,
        LocalSettingsComponent,
        InvoiceSettingsComponent,  
        AirlineSettingsComponent,
        UploadLogoComponent,
        PaymentGatewayComponent,
        ChangeCurrencyComponent,
        DefaultRatiosComponent,
         ShaamTokensComponent,
         CustomizedARInvoiceCounterComponent,
         ReleaseSettingsComponent,
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
            case "AccountingAdvancedAPSettingsComponent": { myResult = AccountingAdvancedAPSettingsComponent; break; }
            case "LocalSettingsComponent": { myResult = LocalSettingsComponent; break; }
            case "InvoiceSettingsComponent": { myResult = InvoiceSettingsComponent; break; }
            case "AirlineSettingsComponent": { myResult = AirlineSettingsComponent; break; }
            case "UploadLogoComponent": { myResult = UploadLogoComponent; break; }
            case "PaymentGatewayComponent": { myResult = PaymentGatewayComponent; break; }
            case "ChangeCurrencyComponent": { myResult = ChangeCurrencyComponent; break; }
            case "DefaultRatiosComponent": { myResult = DefaultRatiosComponent; break; }
             case "ShaamTokensComponent": { myResult = ShaamTokensComponent; break; }  
             case "CustomizedARInvoiceCounterComponent": { myResult = CustomizedARInvoiceCounterComponent; break; }
            case "ReleaseSettingsComponent": { myResult = ReleaseSettingsComponent; break; }

         }

        return myResult;
    }
}
