import {AddEditFCLChargeComponent} from './Components/AddEditFCLChargeComponent';
import {AddEditLCLChargeComponent} from './Components/AddEditLCLChargeComponent';
import {AddEditPriceStepComponent} from './Components/AddEditPriceStepComponent';
import {ChargesTabComponent} from './Components/ChargesTabComponent';
import {FCLChargesComponent} from './Components/FCLChargesComponent';
import {LCLChargesComponent} from './Components/LCLChargesComponent';
import {QuoteVATDetailsComponent} from './Components/QuoteVATDetailsComponent';
import { SelectBreaksComponent } from './Components/SelectBreaksComponent';

export const Components =
    [
        AddEditFCLChargeComponent,
        AddEditLCLChargeComponent,
        AddEditPriceStepComponent,
        ChargesTabComponent,
        FCLChargesComponent,
        LCLChargesComponent,
        QuoteVATDetailsComponent,
        SelectBreaksComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddEditFCLChargeComponent": { myResult = AddEditFCLChargeComponent; break; }
            case "AddEditLCLChargeComponent": { myResult = AddEditLCLChargeComponent; break; }
            case "AddEditPriceStepComponent": { myResult = AddEditPriceStepComponent; break; }
            case "ChargesTabComponent": { myResult = ChargesTabComponent; break; }
            case "FCLChargesComponent": { myResult = FCLChargesComponent; break; }
            case "LCLChargesComponent": { myResult = LCLChargesComponent; break; }
            case "QuoteVATDetailsComponent": { myResult = QuoteVATDetailsComponent; break; }
            case "SelectBreaksComponent": { myResult = SelectBreaksComponent; break; }                
        }

        return myResult;
    }
}
