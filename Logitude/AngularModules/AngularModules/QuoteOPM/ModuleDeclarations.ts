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

export const Components =
    [
        FieldTemplateComponent,
        QuotesComponent,
        NewQuoteComponent,
        QuoteDimensionsComponent,
        NewQuoteAddEditDimensionsComponent,        
        QuoteOPShortTitleComponent,
        QuoteOPHelperComponent, 
        QuoteOPFiltersMenuComponent,  
        ApproveBuildShipmentComponent,

        QuoteEventNotesComponent,
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
