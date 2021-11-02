import { CustomsReportsComponent } from "./Components/CustomsReportsComponent";

export const Components =
    [
        CustomsReportsComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomsReportsComponent": { myResult = CustomsReportsComponent; break; }
        }

        return myResult;
    }
}
 