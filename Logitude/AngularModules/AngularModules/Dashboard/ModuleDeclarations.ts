import {DashboardComponent} from '../Dashboard/Components/Workspace/DashboardComponent';
import {ActivityStatusDetailsComponent} from '../Dashboard/Components/Workspace/ActivityStatusDetailsComponent';
import {AirLineDashboardComponent} from '../Dashboard/Components/Workspace/AirLineDashboardComponent';
import { AddEditDashboardComponent } from '../Dashboard/Components/Windows/AddEditDashboardComponent';
import { AddEditWidgetComponent } from '../Dashboard/Components/Windows/AddEditWidgetComponent';

export const Components =
    [
        DashboardComponent,
        ActivityStatusDetailsComponent,       
        AirLineDashboardComponent,
        AddEditDashboardComponent,
        AddEditWidgetComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DashboardComponent": { myResult = DashboardComponent; break; }
            case "ActivityStatusDetailsComponent": { myResult = ActivityStatusDetailsComponent; break; }
            case "AirLineDashboardComponent": { myResult = AirLineDashboardComponent; break; }
            case "AddEditDashboardComponent": { myResult = AddEditDashboardComponent; break; }
            case "AddEditWidgetComponent": { myResult = AddEditWidgetComponent; break; }
        }

        return myResult;
    }
}
