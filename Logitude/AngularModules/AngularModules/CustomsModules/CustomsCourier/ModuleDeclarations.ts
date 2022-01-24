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
import { GetUnloadPortCodeComponent } from './Components/CourierWorkSheet/GetUnloadPortCodeComponent';
import { AddEditPendingByKeywordComponent } from './Components/PendingByKeyword/AddEditPendingByKeywordComponent';
import { DeclarationPendingsGeneralComponent } from './Components/CourierPendingReason/DeclarationPendingsGeneralComponent';
import { CourierDeclarationWorkspaceComponent } from './Components/CourierWorkspaces/CourierDeclarationWorkspaceComponent';
import { AutonomyKeywordComponent } from './Components/AutonomyKeyword/AutonomyKeywordComponent';
//import { VirtualScrollNG } from './Components/CourierWorkspaces/VirtualScrollNG';
//import { VirtualScrollNGScroll } from './Components/CourierWorkspaces/VirtualScrollNGScroll';
//import { CourierWorksheetNGTComponent } from './Components/CourierWorkspaces/CourierWorksheetNGTComponent';
import { CourierWorksheetNGComponent } from './Components/CourierWorkSheet/CourierWorksheetNGComponent';
import { CourierWorksheetNGListTemplate } from '../CustomsListTemplates/Components/CourierWorksheetNGListTemplate';
import { BulkFeedPendingComponent } from './Components/CourierWorkSheet/bulk-feed-pending/BulkFeedPendingComponent';
import { DeclarationPendingsBulkFeedingComponent } from './Components/CourierPendingReason/DeclarationPendingsBulkFeedingComponent';

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
        DeclarationPendingsBulkFeedingComponent,
        CourierDeclarationWorkspaceComponent,
        AutonomyKeywordComponent,
        GetUnloadPortCodeComponent,
        BulkFeedPendingComponent,
        //VirtualScrollNG,
        //VirtualScrollNGScroll,
        //CourierWorksheetNGTComponent,
        CourierWorksheetNGComponent,
        CourierWorksheetNGListTemplate,
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
            case "GetUnloadPortCodeComponent": { myResult = GetUnloadPortCodeComponent; break; }
            case "AddEditPendingByKeywordComponent": { myResult = AddEditPendingByKeywordComponent; break; }
            case "DeclarationPendingsGeneralComponent": { myResult = DeclarationPendingsGeneralComponent; break; }
            case "DeclarationPendingsBulkFeedingComponent": { myResult = DeclarationPendingsBulkFeedingComponent; break; }
            case "CourierDeclarationWorkspaceComponent": { myResult = CourierDeclarationWorkspaceComponent; break; }
            case "BulkFeedPendingComponent": { myResult = BulkFeedPendingComponent; break; }
                
            case "AutonomyKeywordComponent": { myResult = AutonomyKeywordComponent; break; }
            //case "VirtualScrollNG": { myResult = VirtualScrollNG; break; }
            //case "VirtualScrollNGScroll": { myResult = VirtualScrollNGScroll; break; }
            //case "CourierWorksheetNGTComponent": { myResult = CourierWorksheetNGTComponent; break; }
            case "CourierWorksheetNGComponent": { myResult = CourierWorksheetNGComponent; break; }
            case "CourierWorksheetNGListTemplate": { myResult = CourierWorksheetNGListTemplate; break; }
                
                
                
                
        }

        return myResult;
    }
}
