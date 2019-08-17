import { NewTransferComponent } from './Components/NewEntity/NewTransferComponent';

export const Components =
    [
        NewTransferComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewTransferComponent": { myResult = NewTransferComponent; break; }
        }

        return myResult;
    }
}
