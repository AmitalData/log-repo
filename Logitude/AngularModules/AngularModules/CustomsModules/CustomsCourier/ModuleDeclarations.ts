import { CMConnectedDeclarationTabComponent} from './Components/EditTabs/CMConnectedDeclarationTabComponent';
import { CourierPendingReasonGeneralComponent } from './Components/CourierPendingReason/CourierPendingReasonGeneralComponent';
import { NewCourierComponent} from './Components/NewEntity/NewCourierComponent';
import { AddEditCouriersVatComponent } from './Components/CourierVat/AddEditCouriersVatComponent';
import { AddEditCourierPendingReasonComponent } from './Components/CourierPendingReason/AddEditCourierPendingReasonComponent';
import { DropdownMenuFilterComponent} from './Components/CourierWorkSheet/DropdownMenuFilterComponent';
import { CourierMasterGeneralTabComponent} from './Components/EditTabs/CourierMasterGeneralTabComponent';
import { CourierWorksheetComponent} from './Components/CourierWorkSheet/CourierWorksheetComponent';
import { GetInternalBankComponent} from './Components/CourierWorkSheet/GetInternalBankComponent';
import { AddEditMamanStickerComponent } from './Components/MamanSpecialAction/AddEditMamanStickerComponent';
import { DeclarationMamanSpecialActionComponent } from './Components/MamanSpecialAction/DeclarationMamanSpecialActionComponent';
import { AddCourierPendingToUnifreightStatusComponent } from './Components/CourierPendingReason/AddCourierPendingToUnifreightStatusComponent';
import { GatepassRequestComponent } from './Components/GatepassRequest/GatepassRequestComponent';
import { GetStorageSiteCodeComponent } from './Components/CourierWorkSheet/GetStorageSiteCodeComponent';
import { AddEditPendingByKeywordComponent } from './Components/PendingByKeyword/AddEditPendingByKeywordComponent';
import { DeclarationPendingsGeneralComponent } from './Components/CourierPendingReason/DeclarationPendingsGeneralComponent';


export const Components =
    [
        CMConnectedDeclarationTabComponent,
        CourierPendingReasonGeneralComponent, 
        NewCourierComponent,
        AddEditCouriersVatComponent,
        AddEditCourierPendingReasonComponent,
        DropdownMenuFilterComponent,
        CourierMasterGeneralTabComponent,
        CourierWorksheetComponent,
        GetInternalBankComponent,
        AddEditMamanStickerComponent,
        AddCourierPendingToUnifreightStatusComponent,
        GatepassRequestComponent,
        DeclarationMamanSpecialActionComponent,
        GetStorageSiteCodeComponent,
        AddEditPendingByKeywordComponent,
        DeclarationPendingsGeneralComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CMConnectedDeclarationTabComponent": { myResult = CMConnectedDeclarationTabComponent; break; }
            case "CourierPendingReasonGeneralComponent": { myResult = CourierPendingReasonGeneralComponent; break; }
            case "NewCourierComponent": { myResult = NewCourierComponent; break; }
            case "AddEditCouriersVatComponent": { myResult = AddEditCouriersVatComponent; break; }
            case "AddEditCourierPendingReasonComponent": { myResult = AddEditCourierPendingReasonComponent; break; }
            case "DropdownMenuFilterComponent": { myResult = DropdownMenuFilterComponent; break; }
            case "CourierMasterGeneralTabComponent": { myResult = CourierMasterGeneralTabComponent; break; }
            case "CourierWorksheetComponent": { myResult = CourierWorksheetComponent; break; }
            case "GetInternalBankComponent": { myResult = GetInternalBankComponent; break; }
            case "AddEditMamanStickerComponent": { myResult = AddEditMamanStickerComponent; break; }
            case "AddCourierPendingToUnifreightStatusComponent": { myResult = AddCourierPendingToUnifreightStatusComponent; break; }
            case "GatepassRequestComponent": { myResult = GatepassRequestComponent; break; }
            case "DeclarationMamanSpecialActionComponent": { myResult = DeclarationMamanSpecialActionComponent; break; }
            case "GetStorageSiteCodeComponent": { myResult = GetStorageSiteCodeComponent; break; }
            case "AddEditPendingByKeywordComponent": { myResult = AddEditPendingByKeywordComponent; break; }
            case "DeclarationPendingsGeneralComponent": { myResult = DeclarationPendingsGeneralComponent; break; }
                
        }

        return myResult;
    }
}
