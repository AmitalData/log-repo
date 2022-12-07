import { DashboardComponent } from '../Dashboard/Components/Workspace/DashboardComponent';
import { ActivityStatusDetailsComponent } from '../Dashboard/Components/Workspace/ActivityStatusDetailsComponent';
import { AirLineDashboardComponent } from '../Dashboard/Components/Workspace/AirLineDashboardComponent';
import { AddEditDashboardComponent } from '../Dashboard/Components/Windows/AddEditDashboardComponent';
import { AddEditWidgetComponent } from '../Dashboard/Components/Windows/AddEditWidgetComponent';
import { CustomDashboardComponent } from '../Dashboard/Components/Workspace/CustomDashboardComponent';
import { WidgetFilterComponent } from './Components/Windows/Filter/WidgetFilterComponent';
import { ChooseUsersComponent } from './Components/Windows/ChooseUsersComponent';
import { ChooseUserCheckBoxComponent } from './Components/Windows/ChooseUserCheckBoxComponent';
import { CustomDashboardLayoutComponent } from './Components/Workspace/CustomDashboardLayoutComponent';
import { GlobalFilterComponent } from './Components/Windows/Filter/GlobalFilterComponent';
import { DashboardTabComponent } from './Components/Workspace/DashboardTabComponent';
import { DashboardDropDownComponent } from './Components/Windows/DashboardDropDown/DashboardDropDown';

export const Components =
    [
        DashboardComponent,
        ActivityStatusDetailsComponent,
        AirLineDashboardComponent,
        AddEditDashboardComponent,
        AddEditWidgetComponent,
        CustomDashboardComponent,
        WidgetFilterComponent,
        ChooseUsersComponent,
        ChooseUserCheckBoxComponent,
        CustomDashboardLayoutComponent,
        GlobalFilterComponent,
        DashboardTabComponent,
        DashboardDropDownComponent,
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
            case "AddEditDashboardComponent": { myResult = AddEditDashboardComponent; break; }
            case "AddEditWidgetComponent": { myResult = AddEditWidgetComponent; break; }
            case "CustomDashboardComponent": { myResult = CustomDashboardComponent; break; }
            case "WidgetFilterComponent": { myResult = WidgetFilterComponent; break; }
            case "ChooseUsersComponent": { myResult = ChooseUsersComponent; break; }
            case "ChooseUserCheckBoxComponent": { myResult = ChooseUserCheckBoxComponent; break; }
            case "CustomDashboardLayoutComponent": { myResult = CustomDashboardLayoutComponent; break; }
            case "GlobalFilterComponent": { myResult = GlobalFilterComponent; break; }
            case "DashboardTabComponent": { myResult = DashboardTabComponent; break; }
            case "DashboardDropDownComponent": { myResult = DashboardDropDownComponent; break; }
        }

        return myResult;
    }
}
