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
import { AddCourierPendingToUnifreightStatusComponent } from './Components/CourierPendingReason/AddCourierPendingToUnifreightStatusComponent';


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
        }

        return myResult;
    }
}
