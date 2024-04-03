import { DashboardListComponent } from './Components/Workspace/DashboardLists/DashboardListComponent';
import { DashboardListLinkRendererComponent } from './Components/ListTemplates/DashboardListLinkRendererComponent';
import { DashboardTabComponent } from './Components/Workspace/layout/DashboardTabComponent';
import { DashboardDropDownComponent } from './Components/Windows/DashboardDropDown/DashboardDropDown';
import { AddEditWidgetComponent } from './Components/Windows/AddEditWidget/AddEditWidgetComponent';
import { WidgetFilterComponent } from './Components/Windows/Filter/WidgetFilter/WidgetFilterComponent';
import { ChooseUserCheckBoxComponent } from './Components/Windows/Controls/ChooseUserCheckBoxComponent';
import { AddEditDashboardComponent } from './Components/Windows/AddEditDashboard/AddEditDashboardComponent';
import { CustomDashboardComponent } from './Components/Workspace/layout/CustomDashboardComponent';
import { ChooseUsersComponent } from './Components/Windows/Controls/ChooseUsersComponent';
import { CustomDashboardLayoutComponent } from './Components/Workspace/layout/CustomDashboardLayoutComponent';
import { GlobalFilterComponent } from './Components/Windows/Filter/GlobalFilter/GlobalFilterComponent';
import { GlobalFilterValueComponent } from './Components/Windows/Filter/GlobalFilter/GlobalFilterValueComponent';
import { GlobalFilterItemComponent } from './Components/Windows/Filter/GlobalFilter/GlobalFilterItemComponent';
import { MultiSelectDropDownComponent } from './Components/Windows/MultiSelectDropDown/MultiSelectDropDownComponent';

export const Components =
    [
        DashboardListComponent,
        DashboardListLinkRendererComponent,
        AddEditDashboardComponent,
        AddEditWidgetComponent,
        CustomDashboardComponent,
        WidgetFilterComponent,
        ChooseUsersComponent,
        ChooseUserCheckBoxComponent,
        CustomDashboardLayoutComponent,
        DashboardTabComponent,
        DashboardDropDownComponent,
        GlobalFilterComponent,
        GlobalFilterValueComponent,
        GlobalFilterItemComponent,
        MultiSelectDropDownComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DashboardListComponent": { myResult = DashboardListComponent; break; }
            case "DashboardListLinkRendererComponent": { myResult = DashboardListLinkRendererComponent; break; }
            case "DashboardTabComponent": { myResult = DashboardTabComponent; break; }
            case "DashboardDropDownComponent": { myResult = DashboardDropDownComponent; break; }
            case "AddEditDashboardComponent": { myResult = AddEditDashboardComponent; break; }
            case "AddEditWidgetComponent": { myResult = AddEditWidgetComponent; break; }
            case "CustomDashboardComponent": { myResult = CustomDashboardComponent; break; }
            case "WidgetFilterComponent": { myResult = WidgetFilterComponent; break; }
            case "ChooseUsersComponent": { myResult = ChooseUsersComponent; break; }
            case "ChooseUserCheckBoxComponent": { myResult = ChooseUserCheckBoxComponent; break; }
            case "CustomDashboardLayoutComponent": { myResult = CustomDashboardLayoutComponent; break; }
            case "GlobalFilterComponent": { myResult = GlobalFilterComponent; break; }
            case "GlobalFilterValueComponent": { myResult = GlobalFilterValueComponent; break; }
            case "GlobalFilterItemComponent": { myResult = GlobalFilterItemComponent; break; }
            case "MultiSelectDropDownComponent": { myResult = MultiSelectDropDownComponent; break; }
        }

        return myResult;
    }
}
