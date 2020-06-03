import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportFolderPM } from '../../../../Infrastructure/EntityPMs/BIReportFolderPM';
import { BIReportFolderPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportFolderPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './NewBIReportFolderComponent.html',
})

export class NewBIReportFolderComponent extends BaseComponent {
    public EntityPM: BIReportFolderPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportFolderPMService;
    public DataContext: NewBIReportFolderComponent = this;
    public ObjectTableName: string = "BIReportFolder";
    public IsNewQuery = true;
    public IsReady:boolean = false;
    private IsNew: boolean = true;
    private FolderId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();        
        this.myService = new BIReportFolderPMService();
        this.SetUIProperties();
    }

    SetWindowArgs(args: any) {
        this.IsNew = args.IsNew;
        this.FolderId = args.FolderId;
        this.SetEntityPM();
    }

    SetEntityPM() {
        if (this.IsNew) {
            this.EntityPM = new BIReportFolderPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.IsReady = true;
        }
        else {
            this.CurrentSession.StartBusyIndicator("Loading...");
            this.myService.get(this.FolderId).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    this.EntityPM = myResponse.Result;
                    this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    this.IsReady = true;
                }
            });
        }
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
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.IsNew) {
                this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                    }
                });
            }
            else {
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Name);
                    }
                });
            }
        }
    }
}
