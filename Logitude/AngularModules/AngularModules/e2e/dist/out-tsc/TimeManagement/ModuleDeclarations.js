"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
// Workspaces
var TimeManagementWorkspaceComponent_1 = require("./Components/Workspaces/TimeManagementWorkspaceComponent");
var TimeSheetWorkspaceComponent_1 = require("./Components/Workspaces/TimeSheet/TimeSheetWorkspaceComponent");
var DailyTimeSheetComponent_1 = require("./Components/Workspaces/TimeSheet/DailyTimeSheetComponent");
var WeeklyTimeSheetComponent_1 = require("./Components/Workspaces/TimeSheet/WeeklyTimeSheetComponent");
var MonthlyTimeSheetComponent_1 = require("./Components/Workspaces/TimeSheet/MonthlyTimeSheetComponent");
var SettingsWorkspaceComponent_1 = require("./Components/Workspaces/SettingsWorkspaceComponent");
var ReportsWorkspaceComponent_1 = require("./Components/Workspaces/ReportsWorkspaceComponent");
var ProjectsWorkspaceComponent_1 = require("./Components/Workspaces/Projects/ProjectsWorkspaceComponent");
var ClockTimeComponent_1 = require("./Components/Workspaces/TimeSheet/ClockTimeComponent");
var VacationsComponent_1 = require("./Components/Workspaces/TimeSheet/VacationsComponent");
//Helpers
var TMProjectHelperComponent_1 = require("./Components/Helpers/TMProjectHelperComponent");
//Connections
var ConnectToParentComponent_1 = require("./Components/Connections/ConnectToParentComponent");
// New Screens 
var NewLineComponent_1 = require("./Components/NewEntity/NewLineComponent");
var NewProjectComponent_1 = require("./Components/NewEntity/NewProjectComponent");
var NewOfficeHourComponent_1 = require("./Components/NewEntity/NewOfficeHourComponent");
var NewSprintComponent_1 = require("./Components/NewEntity/NewSprintComponent");
var NewProjectCategoryComponent_1 = require("./Components/NewEntity/NewProjectCategoryComponent");
var NewGetProjectComponent_1 = require("./Components/NewEntity/NewGetProjectComponent");
exports.Components = [
    FieldTemplateComponent_1.FieldTemplateComponent,
    // Workspaces
    TimeManagementWorkspaceComponent_1.TimeManagementWorkspaceComponent,
    TimeSheetWorkspaceComponent_1.TimeSheetWorkspaceComponent,
    DailyTimeSheetComponent_1.DailyTimeSheetComponent,
    MonthlyTimeSheetComponent_1.MonthlyTimeSheetComponent,
    WeeklyTimeSheetComponent_1.WeeklyTimeSheetComponent,
    SettingsWorkspaceComponent_1.SettingsWorkspaceComponent,
    ReportsWorkspaceComponent_1.ReportsWorkspaceComponent,
    ProjectsWorkspaceComponent_1.ProjectsWorkspaceComponent,
    ClockTimeComponent_1.ClockTimeComponent,
    VacationsComponent_1.VacationsComponent,
    //Helpers
    TMProjectHelperComponent_1.TMProjectHelperComponent,
    //Connections
    ConnectToParentComponent_1.ConnectToParentComponent,
    // New Screens 
    NewLineComponent_1.NewLineComponent,
    NewProjectComponent_1.NewProjectComponent,
    NewOfficeHourComponent_1.NewOfficeHourComponent,
    NewSprintComponent_1.NewSprintComponent,
    NewProjectCategoryComponent_1.NewProjectCategoryComponent,
    NewGetProjectComponent_1.NewGetProjectComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            // Workspaces
            case "TimeManagementWorkspaceComponent": {
                myResult = TimeManagementWorkspaceComponent_1.TimeManagementWorkspaceComponent;
                break;
            }
            case "TimeSheetWorkspaceComponent": {
                myResult = TimeSheetWorkspaceComponent_1.TimeSheetWorkspaceComponent;
                break;
            }
            case "DailyTimeSheetComponent": {
                myResult = DailyTimeSheetComponent_1.DailyTimeSheetComponent;
                break;
            }
            case "WeeklyTimeSheetComponent": {
                myResult = WeeklyTimeSheetComponent_1.WeeklyTimeSheetComponent;
                break;
            }
            case "MonthlyTimeSheetComponent": {
                myResult = MonthlyTimeSheetComponent_1.MonthlyTimeSheetComponent;
                break;
            }
            case "SettingsWorkspaceComponent": {
                myResult = SettingsWorkspaceComponent_1.SettingsWorkspaceComponent;
                break;
            }
            case "ReportsWorkspaceComponent": {
                myResult = ReportsWorkspaceComponent_1.ReportsWorkspaceComponent;
                break;
            }
            case "ProjectsWorkspaceComponent": {
                myResult = ProjectsWorkspaceComponent_1.ProjectsWorkspaceComponent;
                break;
            }
            case "ClockTimeComponent": {
                myResult = ClockTimeComponent_1.ClockTimeComponent;
                break;
            }
            case "VacationsComponent": {
                myResult = VacationsComponent_1.VacationsComponent;
                break;
            }
            // Helpers
            case "TMProjectHelperComponent": {
                myResult = TMProjectHelperComponent_1.TMProjectHelperComponent;
                break;
            }
            //Connections
            case "ConnectToParentComponent": {
                myResult = ConnectToParentComponent_1.ConnectToParentComponent;
                break;
            }
            // New Screens 
            case "NewLineComponent": {
                myResult = NewLineComponent_1.NewLineComponent;
                break;
            }
            case "NewProjectComponent": {
                myResult = NewProjectComponent_1.NewProjectComponent;
                break;
            }
            case "NewOfficeHourComponent": {
                myResult = NewOfficeHourComponent_1.NewOfficeHourComponent;
                break;
            }
            case "NewSprintComponent": {
                myResult = NewSprintComponent_1.NewSprintComponent;
                break;
            }
            case "NewProjectCategoryComponent": {
                myResult = NewProjectCategoryComponent_1.NewProjectCategoryComponent;
                break;
            }
            case "NewGetProjectComponent": {
                myResult = NewGetProjectComponent_1.NewGetProjectComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map