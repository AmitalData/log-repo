import { DashboardListComponent } from './Components/Workspace/DashboardLists/DashboardListComponent';

export const Components =
    [
        DashboardListComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DashboardListComponent": { myResult = DashboardListComponent; break; }
        }

        return myResult;
    }
}
