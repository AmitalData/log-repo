import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {QuotesComponent} from './Components/Workspaces/QuotesComponent';
import {NewQuoteComponent} from './Components/NewEntity/NewQuoteComponent';
import {QuoteDimensionsComponent} from './Components/NewEntity/QuoteDimensionsComponent';
import {NewQuoteAddEditAddressComponent} from './Components/NewEntity/NewQuoteAddEditAddressComponent';
import {NewQuoteAddEditDimensionsComponent} from './Components/NewEntity/NewQuoteAddEditDimensionsComponent'
import {QuoteShortTitleComponent} from './Components/ShortTitles/QuoteShortTitleComponent';
import {QuoteHelperComponent} from './Components/Helpers/QuoteHelperComponent';
import {QuoteFiltersMenuComponent} from './Components/FiltersMenu/QuoteFiltersMenuComponent';
import {ApproveBuildShipmentComponent} from './Components/MenuButtons/ApproveBuildShipmentComponent';
import {QuoteEventNotesComponent} from './Components/MenuButtons/QuoteEventNotesComponent';

export const Components =
    [
        FieldTemplateComponent,
        QuotesComponent,
        NewQuoteComponent,
        QuoteDimensionsComponent,
        NewQuoteAddEditAddressComponent,
        NewQuoteAddEditDimensionsComponent,        
        QuoteShortTitleComponent,
        QuoteHelperComponent, 
        QuoteFiltersMenuComponent,  
        ApproveBuildShipmentComponent,
        QuoteEventNotesComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "QuotesComponent": { myResult = QuotesComponent; break; }
            case "NewQuoteComponent": { myResult = NewQuoteComponent; break; }
            case "QuoteDimensionsComponent": { myResult = QuoteDimensionsComponent; break; }
            case "NewQuoteAddEditAddressComponent": { myResult = NewQuoteAddEditAddressComponent; break; }
            case "NewQuoteAddEditDimensionsComponent": { myResult = NewQuoteAddEditDimensionsComponent; break; }            
            case "QuoteShortTitleComponent": { myResult = QuoteShortTitleComponent; break; } 
            case "QuoteHelperComponent": { myResult = QuoteHelperComponent; break; }
            case "QuoteFiltersMenuComponent": { myResult = QuoteFiltersMenuComponent; break; }
            case "ApproveBuildShipmentComponent": { myResult = ApproveBuildShipmentComponent; break; }  
            case "QuoteEventNotesComponent": { myResult = QuoteEventNotesComponent; break; }                       
        }

        return myResult;
    }
}