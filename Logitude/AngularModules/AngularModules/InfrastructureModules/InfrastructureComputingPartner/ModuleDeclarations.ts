import {AddEditComputingPartnerComponent} from './Components/AddEditComputingPartnerComponent';
import {NewComputingPartnerConmponent} from './Components/NewComputingPartnerConmponent';
import {ComputingPartnerGeneralTabComponent} from './Components/ComputingPartnerGeneralTabComponent';
import {ComputingPartnerTranslateComponent} from './Components/ComputingPartnerTranslateComponent';
import {TranslationDetailsComponent} from './Components/TranslationDetailsComponent';
import {EditTranslationComputingPartners} from './Components/EditTranslationComputingPartners';
import {btnComponentComputingPartner} from  './Components/QueryColumnsComponents/btnComponentComputingPartner';
import {btnComponentComputingPartnerEdit} from  './Components/QueryColumnsComponents/btnComponentComputingPartnerEdit';

export const Components =
    [
        AddEditComputingPartnerComponent,
        NewComputingPartnerConmponent,
        ComputingPartnerGeneralTabComponent,
        ComputingPartnerTranslateComponent,
        TranslationDetailsComponent,
        EditTranslationComputingPartners,
        btnComponentComputingPartner,
        btnComponentComputingPartnerEdit,  
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddEditComputingPartnerComponent": { myResult = AddEditComputingPartnerComponent; break; }
            case "NewComputingPartnerConmponent": { myResult = NewComputingPartnerConmponent; break; }
            case "ComputingPartnerGeneralTabComponent": { myResult = ComputingPartnerGeneralTabComponent; break; }
            case "ComputingPartnerTranslateComponent": { myResult = ComputingPartnerTranslateComponent; break; }
            case "TranslationDetailsComponent": { myResult = TranslationDetailsComponent; break; }
            case "EditTranslationComputingPartners": { myResult = EditTranslationComputingPartners; break; }
            case "btnComponentComputingPartner": { myResult = btnComponentComputingPartner; break; }
            case "btnComponentComputingPartnerEdit": { myResult = btnComponentComputingPartnerEdit; break; }     
        }

        return myResult;
    }
}