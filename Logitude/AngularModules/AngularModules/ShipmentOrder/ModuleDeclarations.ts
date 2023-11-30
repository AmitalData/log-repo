import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import { ShipmentOrderHelperComponent } from './Components/Helpers/ShipmentOrderHelperComponent';
import { ShipmentOrderDocsInTabComponent } from './Components/EditTabs/DocsIn/ShipmentOrderDocsInTabComponent';

export const Components =
    [
        FieldTemplateComponent,
        ShipmentOrderHelperComponent,
        ShipmentOrderDocsInTabComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "ShipmentOrderHelperComponent": { myResult = ShipmentOrderHelperComponent; break; }
            case "ShipmentOrderDocsInTabComponent": { myResult = ShipmentOrderDocsInTabComponent; break; }

        }


        return myResult;
    }
}
