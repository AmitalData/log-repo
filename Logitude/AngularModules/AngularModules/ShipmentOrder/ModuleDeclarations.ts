import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import { ShipmentOrderHelperComponent } from './Components/Helpers/ShipmentOrderHelperComponent';
export const Components =
    [
        FieldTemplateComponent,
        ShipmentOrderHelperComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "ShipmentOrderHelperComponent": { myResult = ShipmentOrderHelperComponent; break; }

        }


        return myResult;
    }
}
