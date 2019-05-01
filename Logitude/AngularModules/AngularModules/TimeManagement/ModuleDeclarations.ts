import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';

// Workspaces
import {TimeManagementWorkspaceComponent} from './Components/Workspaces/TimeManagementWorkspaceComponent';
import {TimeSheetWorkspaceComponent} from './Components/Workspaces/TimeSheet/TimeSheetWorkspaceComponent';
import {DailyTimeSheetComponent} from './Components/Workspaces/TimeSheet/DailyTimeSheetComponent';
import {WeeklyTimeSheetComponent} from './Components/Workspaces/TimeSheet/WeeklyTimeSheetComponent';
import {MonthlyTimeSheetComponent} from './Components/Workspaces/TimeSheet/MonthlyTimeSheetComponent';
import {SettingsWorkspaceComponent} from './Components/Workspaces/SettingsWorkspaceComponent';
import {ReportsWorkspaceComponent} from './Components/Workspaces/ReportsWorkspaceComponent';
import {ProjectsWorkspaceComponent} from './Components/Workspaces/Projects/ProjectsWorkspaceComponent';
import {ClockTimeComponent} from './Components/Workspaces/TimeSheet/ClockTimeComponent';
import { VacationsComponent } from './Components/Workspaces/TimeSheet/VacationsComponent';

//Helpers
import {TMProjectHelperComponent} from './Components/Helpers/TMProjectHelperComponent';


//Connections
import {ConnectToParentComponent} from  './Components/Connections/ConnectToParentComponent';



// New Screens 
import {NewLineComponent} from './Components/NewEntity/NewLineComponent'; 
import {NewProjectComponent} from  './Components/NewEntity/NewProjectComponent';
import {NewOfficeHourComponent} from  './Components/NewEntity/NewOfficeHourComponent';
import {NewSprintComponent} from  './Components/NewEntity/NewSprintComponent';
import { NewProjectCategoryComponent } from './Components/NewEntity/NewProjectCategoryComponent';
import { NewGetProjectComponent } from './Components/NewEntity/NewGetProjectComponent';

export const Components =
    [
        FieldTemplateComponent,
        // Workspaces
        TimeManagementWorkspaceComponent,
        TimeSheetWorkspaceComponent,
        DailyTimeSheetComponent,
        MonthlyTimeSheetComponent,
        WeeklyTimeSheetComponent,
        SettingsWorkspaceComponent,
        ReportsWorkspaceComponent,
        ProjectsWorkspaceComponent,
        ClockTimeComponent,
        VacationsComponent,

        //Helpers
        TMProjectHelperComponent,


        //Connections
        ConnectToParentComponent,

        // New Screens 
        NewLineComponent,
        NewProjectComponent,
        NewOfficeHourComponent,
        NewSprintComponent,
        NewProjectCategoryComponent,
        NewGetProjectComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            // Workspaces
            case "TimeManagementWorkspaceComponent": { myResult = TimeManagementWorkspaceComponent; break; }
            case "TimeSheetWorkspaceComponent": { myResult = TimeSheetWorkspaceComponent; break; }
            case "DailyTimeSheetComponent": { myResult = DailyTimeSheetComponent; break; }
            case "WeeklyTimeSheetComponent": { myResult = WeeklyTimeSheetComponent; break; }
            case "MonthlyTimeSheetComponent": { myResult = MonthlyTimeSheetComponent; break; }
            case "SettingsWorkspaceComponent": { myResult = SettingsWorkspaceComponent; break; }
            case "ReportsWorkspaceComponent": { myResult = ReportsWorkspaceComponent; break; }
            case "ProjectsWorkspaceComponent": { myResult = ProjectsWorkspaceComponent; break; }
            case "ClockTimeComponent": { myResult = ClockTimeComponent; break; }
            case "VacationsComponent": { myResult = VacationsComponent; break; }

            // Helpers
            case "TMProjectHelperComponent": { myResult = TMProjectHelperComponent; break; }


          //Connections

            case "ConnectToParentComponent": { myResult = ConnectToParentComponent; break; }

                

                
            // New Screens 
            case "NewLineComponent": { myResult = NewLineComponent; break; }
            case "NewProjectComponent": { myResult = NewProjectComponent; break; }
            case "NewOfficeHourComponent": { myResult = NewOfficeHourComponent; break; }
            case "NewSprintComponent": { myResult = NewSprintComponent; break; }
            case "NewProjectCategoryComponent": { myResult = NewProjectCategoryComponent; break; }
            case "NewGetProjectComponent": { myResult = NewGetProjectComponent; break; }
                
        }

        return myResult;
    }
}
