import { DashboardListComponent } from './Components/Workspace/DashboardLists/DashboardListComponent';
import { DashboardListLinkRendererComponent } from './Components/ListTemplates/DashboardListLinkRendererComponent';

export const Components =
    [
        DashboardListComponent,
        DashboardListLinkRendererComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DashboardListComponent": { myResult = DashboardListComponent; break; }
            case "DashboardListLinkRendererComponent": { myResult = DashboardListLinkRendererComponent; break; }
        }

        return myResult;
    }
}
