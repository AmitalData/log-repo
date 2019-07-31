import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools'
import {BusinessProcessQueuePMService} from '../../../../Infrastructure/Services/StandardPMs/BusinessProcessQueuePMService';
import {BusinessProcessQueuePM} from '../../../../Infrastructure/EntityPMs/BusinessProcessQueuePM';
import {BusinessProcessQueuePMInitService} from '../../../../Infrastructure/EntityPMInitServices/BusinessProcessQueuePMInitService';

@Component({
    selector: 'QueueNewComponent',
    moduleId: module.id,
    templateUrl: './QueueNewComponent.html',
})

export class QueueNewComponent extends BaseComponent {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: BusinessProcessQueuePM;
    public DataContext: QueueNewComponent = this;
    public ObjectTableName: string = "BusinessProcessQueue";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new BusinessProcessQueuePM();
        BusinessProcessQueuePMInitService.InitValues(this.EntityPM, true);
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("BusinessRoleId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.BusinessRoleId));
        this.UIProperties.SetRequired("ObjectTableId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ObjectTableId));
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

    public get BusinessRoleId() { return this.EntityPM.BusinessRoleId; }
    public set BusinessRoleId(value: string) {
        if (this.EntityPM.BusinessRoleId != value) {
            this.EntityPM.BusinessRoleId = value;
            this.SetUIProperties();
        }
    }

    public get ObjectTableId() { return this.EntityPM.ObjectTableId; }
    public set ObjectTableId(value: string) {
        if (this.EntityPM.ObjectTableId != value) {
            this.EntityPM.ObjectTableId = value;
            this.SetUIProperties();
        }
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
        if (AppTool.IsNullOrEmpty(this.ObjectTableId)) {
            errors.push("Entity field is required");
        }
        if (AppTool.IsNullOrEmpty(this.BusinessRoleId)) {
            errors.push("Business Role field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService: BusinessProcessQueuePMService = new BusinessProcessQueuePMService();
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
