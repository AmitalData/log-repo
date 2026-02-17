import {QuotationComponent} from './Components/Quotation/QuotationComponent';
import {QuoteSettingsComponent} from './Components/Maintenance/QuoteSettingsComponent';

export const Components =
    [
        QuotationComponent,
        QuoteSettingsComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "QuotationComponent": { myResult = QuotationComponent; break; }
            case "QuoteSettingsComponent": { myResult = QuoteSettingsComponent; break; }
        }

        return myResult;
    }
}