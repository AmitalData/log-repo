import {QuotationComponent} from './Components/Quotation/QuotationComponent';
import {QuoteSettingsComponent} from './Components/Maintenance/QuoteSettingsComponent';

import {AttachmentQuotationComponent} from './Components/Quotation/AttachmentQuotationComponent';
export const Components =
    [
        QuotationComponent,
        QuoteSettingsComponent,
        AttachmentQuotationComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "QuotationComponent": { myResult = QuotationComponent; break; }
            case "QuoteSettingsComponent": { myResult = QuoteSettingsComponent; break; }
            case "AttachmentQuotationComponent": { myResult = AttachmentQuotationComponent; break; }
        }

        return myResult;
    }
}