import { NewTransferComponent } from './Components/NewEntity/NewTransferComponent';
import { AMANACExportTransferComponent } from './Components/NewEntity/AMANACExportTransferComponent';

export const Components =
    [
        NewTransferComponent,
        AMANACExportTransferComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewTransferComponent": { myResult = NewTransferComponent; break; }
            case "AMANACExportTransferComponent": { myResult = AMANACExportTransferComponent; break; }
        }

        return myResult;
    }
}
