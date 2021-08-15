
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
export const Components =
    [
        FieldTemplateComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }

        }


        return myResult;
    }
}
