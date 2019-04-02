import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EmployeeGroupPM} from '../../../../CRM/EntityPMs/EmployeeGroupPM';
import {EmployeeGroupPMService} from '../../../../CRM/Services/StandardPMs/EmployeeGroupPMService';
import {EmployeeGroupPMInitService} from '../../../../CRM/EntityPMInitServices/EmployeeGroupPMInitService'; 
import {EmployeeGroupLinePM} from '../../../../CRM/EntityPMs/EmployeeGroupLinePM';
import {AppTool} from '../../../../Infrastructure/Tools'; 
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './NewEmployeeComponent.html',
})

export class NewEmployeeComponent extends BaseComponent {
    public ObjectTableName: string = "EmployeeGroup";
    public DataContext: NewEmployeeComponent = this;
    public ValidationErrorsList: string[] = [];
    private entityPM: EmployeeGroupPM;
    public EmployeeGroupLines: EmployeeGroupLineData[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.entityPM = new EmployeeGroupPM();
        this.EmployeeGroupLines = [];
        EmployeeGroupPMInitService.InitValues(this.entityPM,true);
    }

    get Name() { return this.entityPM.Name; }
    set Name(value:string)
    {
        if (this.entityPM.Name != value) {
            this.entityPM.Name = value;
        }
    }

    get Description() { return this.entityPM.Description; }
    set Description(value:string)
    {
        if (this.entityPM.Description != value) {
            this.entityPM.Description = value;
        }
    }

    get ManagerUserId() { return this.entityPM.ManagerUserId; }
    set ManagerUserId(value:string)
    {
        if (this.entityPM.ManagerUserId != value) {
            this.entityPM.ManagerUserId = value;
        }
    }

    get EscalationNotify(){ return this.entityPM.EscalationNotify; }
    set EscalationNotify(value:string)
    {
        if (this.entityPM.EscalationNotify != value) {
            this.entityPM.EscalationNotify = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (this.EmployeeGroupLines.length > 0) {
            this.EmployeeGroupLines.forEach(item => {
                if (AppTool.IsNullOrEmpty(item.UserId)) {
                    errors.push("Some lines have empty user field");
                }
            });
        }
       
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new EmployeeGroupPMService();
            service.insert(this.entityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit('ok');
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }

    }
    NewGroupLine() {
        var newLine: EmployeeGroupLinePM = new EmployeeGroupLinePM(null);
        newLine.Tenant = SessionLocator.Tenant
        newLine.EmployeeGroupId = this.entityPM.Id;
        this.EmployeeGroupLines.push(new EmployeeGroupLineData(newLine, this.entityPM, this));
    }
    DeleteGroupLine(deletedItem: EmployeeGroupLineData) {
        var selectedLinePM: EmployeeGroupLinePM = deletedItem.linePM;

        if (this.entityPM.EmployeeGroupLines.indexOf(selectedLinePM) != -1) {
            this.entityPM.RemoveEmployeeGroupLine(selectedLinePM);
        }
        var index = this.EmployeeGroupLines.indexOf(deletedItem);
        if (index > -1) {
            this.EmployeeGroupLines.splice(index,1);
        }
    }
}
export class EmployeeGroupLineData extends BaseComponent {
    public linePM: EmployeeGroupLinePM;
    private groupPM: EmployeeGroupPM;
    private trigger: NewEmployeeComponent;
    public ObjectTableName: string = "EmployeeGroupLine";
    public DataContext: EmployeeGroupLineData = this;
    constructor(entity: EmployeeGroupLinePM, group: EmployeeGroupPM, trigger: NewEmployeeComponent) {
        super();
        this.groupPM = group;
        this.linePM = entity;
        this.trigger = trigger;
    }

    get UserId() { return this.linePM.UserId; }
    set UserId(value:string) {
        if (this.linePM.UserId != value) {
            this.linePM.UserId = value;

            this.OnUserChanged();
        }
    }

    private OnUserChanged() {
        if (AppTool.IsNullOrEmpty(this.UserId)) {
            if (this.groupPM.EmployeeGroupLines.indexOf(this.linePM) != -1) {
                this.groupPM.RemoveEmployeeGroupLine(this.linePM);
            }
        }

        else {
            if (this.groupPM.EmployeeGroupLines.indexOf(this.linePM) == -1) {
                this.groupPM.AddEmployeeGroupLine(this.linePM);
            }
        }
    }
}
