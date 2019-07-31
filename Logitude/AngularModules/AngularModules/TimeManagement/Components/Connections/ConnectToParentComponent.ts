import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TMProjectPM} from '../../EntityPMs/TMProjectPM';
import {TMProjectPMService} from '../../Services/StandardPMs/TMProjectPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../Infrastructure/Tools';
import {TimeManagementDomainService} from '../../Services/TimeManagementDomainService';

@Component({
    selector: 'ConnectToParentComponent',
    moduleId: module.id,
    templateUrl: './ConnectToParentComponent.html',
})

export class ConnectToParentComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMProject";
    public EntityPM: TMProjectPM;
    public SelectedLocationFilter: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();              
    }


    SetWindowArgs(args) {
        this.EntityPM = args.EntityArgs;      
    }

    private projectId;

    get Id() {
        return this.projectId;
    }
    set Id(value: string) {
        if (this.projectId != value) {
            this.projectId= value;
        }
    }


  

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[]=[];
    ConnectButtonClicked() {
        if (this.EntityPM.IsInnerProject) {
            this.ValidationErrorsList.push('This is an inner project , cannot be connected to parent project');
        }
        else {
            if (this.Id == null) {
                this.ValidationErrorsList.push('you must select a project');
            }
            else {
                this.CurrentSession.StartBusyIndicator("Updating...");
                var myService: TimeManagementDomainService = new TimeManagementDomainService();
                myService.GetNewTMProjectConnect(this.EntityPM.Id, this.projectId).subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        this.EntityPM.ProjectNumber = myResponse.Result;
                        this.EntityPM.IsDirty = false;
                        this.CurrentSession.CloseCurrentWindowEmit('OK');
                    }
                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });

            }
        }

    }
}
