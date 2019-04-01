import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools'
import {TeamPMService} from '../../../../Infrastructure/Services/StandardPMs/TeamPMService';
import {TeamPM} from '../../../../Infrastructure/EntityPMs/TeamPM';
import {TeamPMInitService} from '../../../../Infrastructure/EntityPMInitServices/TeamPMInitService';

@Component({
    selector: 'TeamNewComponent',
    moduleId: module.id,
    templateUrl: './TeamNewComponent.html',
})

export class TeamNewComponent extends BaseComponent {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: TeamPM;
    public DataContext: TeamNewComponent = this;
    public ObjectTableName: string = "Team";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new TeamPM();
        TeamPMInitService.InitValues(this.EntityPM, true);
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) {
        if (this.EntityPM.LocalName != value)
            this.EntityPM.LocalName = value;
    }

    public get ManagerUserId() { return this.EntityPM.ManagerUserId; }
    public set ManagerUserId(value: string) {
        if (this.EntityPM.ManagerUserId != value)
            this.EntityPM.ManagerUserId = value;
    }

    public get Notify() { return this.EntityPM.Notify; }
    public set Notify(value: string) {
        if (this.EntityPM.Notify != value)
            this.EntityPM.Notify = value;
    }

    public get Notes() { return this.EntityPM.Notes; }
    public set Notes(value: string) {
        if (this.EntityPM.Notes != value)
            this.EntityPM.Notes = value;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService: TeamPMService = new TeamPMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
