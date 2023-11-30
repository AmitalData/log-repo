import { DashboardComponent } from '../Dashboard/Components/Workspace/DashboardComponent';
import { ActivityStatusDetailsComponent } from '../Dashboard/Components/Workspace/ActivityStatusDetailsComponent';
import { AirLineDashboardComponent } from '../Dashboard/Components/Workspace/AirLineDashboardComponent';
export const Components =
    [
        DashboardComponent,
        ActivityStatusDetailsComponent,
        AirLineDashboardComponent,
    ];

export const Directives =
    [

    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DashboardComponent": { myResult = DashboardComponent; break; }
            case "ActivityStatusDetailsComponent": { myResult = ActivityStatusDetailsComponent; break; }
            case "AirLineDashboardComponent": { myResult = AirLineDashboardComponent; break; }
        }

        return myResult;
    }
}
