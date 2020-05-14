import { WorkerRoleNameExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/WorkerRoleNameExtendedPMService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { WorkerRoleNamePM } from '../../../../Infrastructure/EntityPMs/WorkerRoleNamePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Component } from '@angular/core';

@Component({
    templateUrl: './SetWorkerRoleNameComponent.html',
})

export class SetWorkerRoleNameComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: WorkerRoleNamePM;
    public ObjectTableName = "WorkerRoleName";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public IsReady: boolean = false;
    entityResourceService: EntityResourceService = new EntityResourceService();

    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.IsReady = true;
        });
        this.EntityPM = new WorkerRoleNamePM();
        if (SessionLocator.WorkerRoleName) {
            this.EntityPM.Name = SessionLocator.WorkerRoleName;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            errors.push("Worker Role Name is Required")
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            SessionLocator.WorkerRoleName = this.EntityPM.Name;
            //this.CurrentSession.CloseCurrentWindow();
            var myService: WorkerRoleNameExtendedPMService = new WorkerRoleNameExtendedPMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse) {
                    if (!myResponse.HasError) {
                        this.CurrentSession.CloseCurrentWindow();
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
}
