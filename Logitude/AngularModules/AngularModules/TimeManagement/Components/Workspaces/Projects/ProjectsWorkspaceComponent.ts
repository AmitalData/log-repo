import {Component, Output, EventEmitter} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {TimeManagementDomainService} from '../../../Services/TimeManagementDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ExcelExportService} from '../../../../Common/Services/Others/ExcelExportService'
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';


declare var UploadLogoFile, ArrayBufferToBase64;

@Component({
    moduleId: module.id,
    templateUrl: './ProjectsWorkspaceComponent.html',
})

export class ProjectsWorkspaceComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadUserQueries = new EventEmitter();
    public MyProjectsCount: number = 0;
    public AllProjectsCount: number = 0;

    constructor() {
        
    }
    LoadAllScreenData() {
        this.SetQueriesVisibility();
        this.LoadQueriesCounts();
    }
    InitComponent() {
        this.LoadAllScreenData();
    }

    public AllProjectsQueriesVisibility: boolean = false;
    public MyProjectsQueriesVisibility: boolean = false;
    LoadQueriesCounts() {
        var myService: TimeManagementDomainService = new TimeManagementDomainService();
        myService.GetProjectsCounts(SessionLocator.LoggedUserId).subscribe(myResult => {
            if (myResult != null) {
                this.MyProjectsCount = myResult.MyProjectsCount > 1000 ? "1000+" : myResult.MyProjectsCount.toString();
                this.AllProjectsCount = myResult.AllProjectsCount > 1000 ? "1000+" : myResult.AllProjectsCount.toString();
            }
        });
    }
    private SetQueriesVisibility() {
        this.AllProjectsQueriesVisibility = FeatureLocator.HasFeaturePermession("TMProject", "TMProject.Q.AllProjects") ? true : false;
        this.MyProjectsQueriesVisibility = FeatureLocator.HasFeaturePermession("TMProject", "TMProject.Q.MyProjects") ? true : false;
    }




    UploadClockTimeFileClick() {
        document.getElementById(this.ClockTimeHtmlId).click();
    }

    public ClockTimeHtmlId: string = Guid.NewRandomString();
    UploadClockTimeFile(event: any) {

        var file: any = UploadLogoFile(this.ClockTimeHtmlId);
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            this.ArrayBufferToBase64(file, this);
        }
    }

    ArrayBufferToBase64(file: any, viewModel: any) {
        if (file) {
            var reader: FileReader = new FileReader();
            var reader = new FileReader();

            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewModel.ImportFeatures(window.btoa(binary));
            };

            reader.onerror = function (e) {
                SessionLocator.CurrentSession.StopBusyIndicator();

                var wind = new MessageWindow();
                wind.Show("Error Importing file");
            };

            reader.readAsArrayBuffer(file);
        }
    }

    ImportFeatures(data: any) {
        var service: ExcelExportService = new ExcelExportService();
        var file: ImageParameter = new ImageParameter();
        file.Base64String = data;
        service.ImportClockTimeData(file).subscribe(res => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            var wind = new MessageWindow();
            wind.Show("Import completed successfully");
        });
    }



    // Commands 
    RefreshButtonClicked() {

    }
    EditTMProject(args) {


    }
    NewProject() {
        this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "New Project";
            logWindow.Show('./TimeManagement/Components/NewEntity/NewProjectComponent');
            logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        });

    }
    filterAgrs: ApiQueryFilters;
    ViewProjectsQuery(code: string) {
        var displayTitle = "";
        this.filterAgrs = new ApiQueryFilters();
        switch (code) {
            case "All Projects":
                {
                    displayTitle = "All Projects";
                    break;
                }
            case "My Projects":
                {
                    displayTitle = "My Projects";
                    break;
                }
            default: { break; }
        }

        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMProject";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Projects";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    SessionLocator.CurrentSession.AddMenuReference(cmpRef);
                });
        });

    }
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }
}
