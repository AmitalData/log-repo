import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {QuotesComponent} from './Components/Workspaces/QuotesComponent';
import {NewQuoteComponent} from './Components/NewEntity/NewQuoteComponent';
import {QuoteDimensionsComponent} from './Components/NewEntity/QuoteDimensionsComponent';
import {NewQuoteAddEditDimensionsComponent} from './Components/NewEntity/NewQuoteAddEditDimensionsComponent'
import {QuoteOPShortTitleComponent} from './Components/ShortTitles/QuoteOPShortTitleComponent';
import {QuoteOPHelperComponent} from './Components/Helpers/QuoteOPHelperComponent';
import {QuoteOPFiltersMenuComponent} from './Components/FiltersMenu/QuoteOPFiltersMenuComponent';
import {ApproveBuildShipmentComponent} from './Components/MenuButtons/ApproveBuildShipmentComponent';
import {QuoteEventNotesComponent} from './Components/MenuButtons/QuoteEventNotesComponent';

import { QuoteSaleCurrencyTypeComponent } from './Components/Shared/QuoteSaleCurrencyTypeComponent';
import { NewQuoteExpectedOrderComponent } from './Components/NewEntity/components/new-quote-expected-order/new-quote-expected-order.component';
import { NewQuoteGeneralComponent } from './Components/NewEntity/components/new-quote-general/new-quote-general.component';
import { NewQuoteLeftSideComponent } from './Components/NewEntity/components/new-quote-left-side/new-quote-left-side.component';
import { NewQuoteMyCustomersComponent } from './Components/NewEntity/components/new-quote-my-customers/new-quote-my-customers.component';
import { NewQuotePropertiesComponent } from './Components/NewEntity/components/new-quote-properties/new-quote-properties.component';
import { AddressTextareaComponent } from './Components/NewEntity/components/address-textarea/address-textarea.component';
import { AutocomplateTableComponent } from './Components/NewEntity/components/autocomplate-table/autocomplate-table.component';
import { NewQuoteComponentOld } from './Components/NewEntity/NewQuoteComponent.old';
import { NewQuoteAddressComponent } from './Components/NewEntity/components/new-quote-address/new-quote-address.component';
import { BtnMenuPlusComponent } from './Components/NewEntity/components/btn-menu-plus/btn-menu-plus.component';
import { NewQuotePartnerComponent } from './Components/NewEntity/components/new-quote-partner/new-quote-partner.component';

export const Components = [
        FieldTemplateComponent,
        QuotesComponent,
        NewQuoteComponent,
        NewQuoteComponentOld,
        QuoteDimensionsComponent,
        NewQuoteAddEditDimensionsComponent,        
        QuoteOPShortTitleComponent,
        QuoteOPHelperComponent, 
        QuoteOPFiltersMenuComponent,  
        ApproveBuildShipmentComponent,

        QuoteEventNotesComponent,

        NewQuoteLeftSideComponent,
        NewQuoteMyCustomersComponent,
        NewQuotePropertiesComponent,
        NewQuoteGeneralComponent,
        NewQuoteExpectedOrderComponent,
        AddressTextareaComponent,
        AutocomplateTableComponent,
        NewQuoteAddressComponent,
        BtnMenuPlusComponent,
        NewQuotePartnerComponent,
    ];

export const SharedComponents =
    [
        QuoteSaleCurrencyTypeComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "QuotesComponent": { myResult = QuotesComponent; break; }
            case "NewQuoteComponent": { myResult = NewQuoteComponent; break; }
            case "NewQuoteComponentOld": { myResult = NewQuoteComponentOld; break; }
            case "QuoteDimensionsComponent": { myResult = QuoteDimensionsComponent; break; }
            case "NewQuoteAddEditDimensionsComponent": { myResult = NewQuoteAddEditDimensionsComponent; break; }            
            case "QuoteOPShortTitleComponent": { myResult = QuoteOPShortTitleComponent; break; } 
            case "QuoteOPHelperComponent": { myResult = QuoteOPHelperComponent; break; }
            case "QuoteOPFiltersMenuComponent": { myResult = QuoteOPFiltersMenuComponent; break; }
            case "ApproveBuildShipmentComponent": { myResult = ApproveBuildShipmentComponent; break; }  
            case "QuoteEventNotesComponent": { myResult = QuoteEventNotesComponent; break; }                       
        }

        return myResult;
    }
}
