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
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './EmployeeGroupGeneralTabComponent.html',
})

export class EmployeeGroupGeneralTabComponent extends BaseComponent {
    public ObjectTableName: string = "EmployeeGroup";
    public DataContext: EmployeeGroupGeneralTabComponent = this;
    public ValidationErrorsList: string[] = [];
    public entityPM: EmployeeGroupPM;
    public EmployeeGroupLines: EmployeeGroupLineData[];

    constructor(public entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        this.FillGroupLines();
    }

    public FillGroupLines() {
        this.EmployeeGroupLines = [];
        if (this.entityPM.EmployeeGroupLines.length > 0) {
            this.entityPM.EmployeeGroupLines.forEach(item => {
                this.EmployeeGroupLines.push(new EmployeeGroupLineData(item, this));
            });
        }
    }

    get Name() { return this.entityPM.Name; }
    set Name(value: string) {
        if (this.entityPM.Name != value) {
            this.entityPM.Name = value;
        }
    }

    get Description() { return this.entityPM.Description; }
    set Description(value: string) {
        if (this.entityPM.Description != value) {
            this.entityPM.Description = value;
        }
    }

    get ManagerUserId() { return this.entityPM.ManagerUserId; }
    set ManagerUserId(value: string) {
        if (this.entityPM.ManagerUserId != value) {
            this.entityPM.ManagerUserId = value;
        }
    }

    get EscalationNotify() { return this.entityPM.EscalationNotify; }
    set EscalationNotify(value: string) {
        if (this.entityPM.EscalationNotify != value) {
            this.entityPM.EscalationNotify = value;
        }
    }

    get Inactive() { return this.entityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.entityPM.Inactive != value) {
            this.entityPM.Inactive = value;
        }
    }

    // Commands 
    NewGroupLine() {
        var newLine: EmployeeGroupLinePM = new EmployeeGroupLinePM(null);
        newLine.Tenant = SessionLocator.Tenant
        newLine.EmployeeGroupId = this.entityPM.Id;
        this.EmployeeGroupLines.push(new EmployeeGroupLineData(newLine, this));
    }
    DeleteGroupLine(deletedItem: EmployeeGroupLineData) {
        var selectedLinePM: EmployeeGroupLinePM = deletedItem.linePM;

        if (this.entityPM.EmployeeGroupLines.indexOf(selectedLinePM) != -1) {
            this.entityPM.RemoveEmployeeGroupLine(selectedLinePM);
        }
        var index = this.EmployeeGroupLines.indexOf(deletedItem);
        if (index > -1) {
            this.EmployeeGroupLines.splice(index, 1);
        }
    }

    RefreshLines(args:string) {
        if (args != null) {
            var myUser = args.toString().split(':')[0];
            this.EmployeeGroupLines.forEach(item => {
                item.myCheck = args.toString().split(':')[1];
                if (item.UserId == myUser) {
                    item.isDefaultOwner = true;
                }
                else {
                    item.isDefaultOwner = false;
                }
            });
        }
    }
}
export class EmployeeGroupLineData extends BaseComponent {
    public linePM: EmployeeGroupLinePM;
    private groupPM: EmployeeGroupPM;
    private trigger: EmployeeGroupGeneralTabComponent;
    public ObjectTableName: string = "EmployeeGroupLine";
    public DataContext: EmployeeGroupLineData = this;
    public myCheck: string = "F";

    constructor(entity: EmployeeGroupLinePM, trigger: EmployeeGroupGeneralTabComponent) {
        super();
        this.groupPM = trigger.entityPM;
        this.linePM = entity;
        this.trigger = trigger;
    }

    get UserId() { return this.linePM.UserId; }
    set UserId(value: string) {
        if (this.linePM.UserId != value) {
            this.linePM.UserId = value;
            this.OnUserChanged();
        }
    }

    public isDefaultOwner = false;
    get IsDefaultOwner() {
        if (this.myCheck == "T") {
            var myValue = this.linePM.IsDefaultOwner;
            this.myCheck = "F";
            myValue = this.isDefaultOwner;
            this.linePM.IsDefaultOwner = myValue;
        }
        return this.linePM.IsDefaultOwner;
    }
    set IsDefaultOwner(value: boolean) {
        if (this.linePM.IsDefaultOwner != value) {
            this.linePM.IsDefaultOwner = value;
            if (this.myCheck == "F") {
                this.FireRefreshOwner();
            }
        }
    }

    private FireRefreshOwner() {
        var args = this.UserId + ":" + "T";
        if (args != null) {
            this.myCheck = args.toString().split(':')[1];
            this.trigger.RefreshLines(args);
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
