import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools'
import { CustomerTeamPM } from '../../../../Common/EntityPMs/CustomerTeamPM';
import { CustomerTeamPMService } from '../../../../Common/Services/StandardPMs/CustomerTeamPMService';

@Component({
    selector: 'NewCustomerTeamComponent',
    templateUrl: './NewCustomerTeamComponent.html',
})

export class NewCustomerTeamComponent extends BaseComponent {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: CustomerTeamPM;
    public DataContext: NewCustomerTeamComponent = this;
    public ObjectTableName: string = "CustomerTeam";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new CustomerTeamPM();
        this.InitValues();
    }

    private InitValues() {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
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

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length != 0) return;
        this.CurrentSession.StartBusyIndicatorSaving();
        var myService: CustomerTeamPMService = new CustomerTeamPMService();
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
