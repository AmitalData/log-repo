import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportFolderPM } from '../../../../Infrastructure/EntityPMs/BIReportFolderPM';
import { BIReportFolderPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportFolderPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './NewBIReportFolderComponent.html',
})

export class NewBIReportFolderComponent extends BaseComponent {
    public EntityPM: BIReportFolderPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportFolderPMService;
    public DataContext: NewBIReportFolderComponent = this;
    public ObjectTableName: string = "BIReportFolder";
    public IsNewQuery = true;

    constructor() {
        super();
        this.EntityPM = new BIReportFolderPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        
        this.myService = new BIReportFolderPMService();
        this.SetUIProperties();
    }

    SetWindowArgs() {        
        this.SetUIProperties();
    }

    SetUIProperties() {
        
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Index() { return this.EntityPM.Index; }
    set Index(newValue: number) {
        if (this.EntityPM.Index != newValue) {
            this.EntityPM.Index = newValue;
        }
    }
    
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    SessionLocator.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }
            });
        }
    }
}
