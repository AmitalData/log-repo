import { DashboardListComponent } from './Components/Workspace/DashboardLists/DashboardListComponent';
import { EditShipmentLinkRendererComponent } from './Components/ListTemplates/EditShipmentLinkRendererComponent';

export const Components =
    [
        DashboardListComponent,
        EditShipmentLinkRendererComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DashboardListComponent": { myResult = DashboardListComponent; break; }
            case "EditShipmentLinkRendererComponent": { myResult = EditShipmentLinkRendererComponent; break; }
        }

        return myResult;
    }
}
