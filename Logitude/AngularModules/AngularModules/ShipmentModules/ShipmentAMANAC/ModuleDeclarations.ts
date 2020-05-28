import { NewTransferComponent } from './Components/NewEntity/NewTransferComponent';
import { AMANACExportTransferComponent } from './Components/NewEntity/AMANACExportTransferComponent';
import { BlockedShipmentsComponent } from './Components/BlockedShipmentsComponent';
import { CustomsTransferGeneralTabComponent } from './Components/EditTabs/CustomsTransferGeneralTabComponent';

export const Components =
    [
        NewTransferComponent,
        AMANACExportTransferComponent,
        BlockedShipmentsComponent,
        CustomsTransferGeneralTabComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewTransferComponent": { myResult = NewTransferComponent; break; }
            case "AMANACExportTransferComponent": { myResult = AMANACExportTransferComponent; break; }
            case "BlockedShipmentsComponent": { myResult = BlockedShipmentsComponent; break; }
            case "CustomsTransferGeneralTabComponent": { myResult = CustomsTransferGeneralTabComponent; break; }
        }

        return myResult;
    }
}
