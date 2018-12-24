import { NewBIReport } from './Components/NewEntity/NewBIReport';


export const Components =
    [
        NewBIReport,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewBIReport": { myResult = NewBIReport; break; }
        }

        return myResult;
    }
}
