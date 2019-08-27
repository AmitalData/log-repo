import { NewTransferComponent } from './Components/NewEntity/NewTransferComponent';
import { AMANACExportTransferComponent } from './Components/NewEntity/AMANACExportTransferComponent';
import { BlockedShipmentsComponent } from './Components/BlockedShipmentsComponent';

export const Components =
    [
        NewTransferComponent,
        AMANACExportTransferComponent,
        BlockedShipmentsComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewTransferComponent": { myResult = NewTransferComponent; break; }
            case "AMANACExportTransferComponent": { myResult = AMANACExportTransferComponent; break; }
            case "BlockedShipmentsComponent": { myResult = BlockedShipmentsComponent; break; }
        }

        return myResult;
    }
}
