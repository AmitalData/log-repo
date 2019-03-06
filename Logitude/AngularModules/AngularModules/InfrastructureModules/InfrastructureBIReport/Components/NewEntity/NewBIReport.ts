import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './NewBIReport.html',
})

export class NewBIReport extends BaseComponent {
    public EntityPM: BIReportPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportPMService;
    public DataContext: NewBIReport = this;
    public ObjectTableName: string = "BIReport";
    public IsNewQuery = true;

    constructor() {
        super();
        this.EntityPM = new BIReportPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.TypeCode = "EXL";
        this.myService = new BIReportPMService();
        this.SetUIProperties();
    }
    SetWindowArgs(args: any) {
        this.DWQueryId = args.DWQueryId;
        this.EntityPM.BIReportFolderId = args.FolderId;
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("DWQueryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DWQueryId));
        if (!AppTool.IsNullOrEmpty(this.DWQueryId)) {
            this.IsNewQuery = false;
        }
        else {
            this.IsNewQuery = true;

        }
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

    get BIReportFolderId() { return this.EntityPM.BIReportFolderId; }
    set BIReportFolderId(newValue: string) {
        if (this.EntityPM.BIReportFolderId != newValue) {
            this.EntityPM.BIReportFolderId = newValue;
        }
    }

    get DWQueryId() { return this.EntityPM.DWQueryId; }
    set DWQueryId(newValue: string) {
        if (this.EntityPM.DWQueryId != newValue) {
            this.EntityPM.DWQueryId = newValue;
        }
    }


    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(newValue: string) {
        if (this.EntityPM.TypeCode != newValue) {
            this.EntityPM.TypeCode = newValue;
        }
    }

    ShowQueryBuilderClicked() {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DWQueryId = this.DWQueryId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null) {
                    this.EntityPM.DWQueryId = s.QID;
                    this.SetUIProperties();
                }
            });
        });
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        SessionLocator.CurrentSession.CloseCurrentWindow();

        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DWQueryId = this.DWQueryId;
        windowArgs.IsBIReportWorkspace = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null) {
                    this.EntityPM.DWQueryId = s.QID;

                    if (this.ValidationErrorsList.length == 0) {
                        SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                        this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                            SessionLocator.CurrentSession.StopBusyIndicator();
                            if (myResponse.HasError) {
                                this.ValidationErrorsList = myResponse.ErrorsArray;
                            }
                            else {
                                SessionLocator.CurrentSession.CloseCurrentWindow();

                                if (d != "cancel") {
                                    SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            cmpRef.instance.ComponentRef = cmpRef;
                                            cmpRef.instance.Run({
                                                DWQueryId: s.QID,
                                                ObjectTableName: 'BIReport',
                                                EntityId: this.EntityPM.Id,
                                            });

                                            //cmpRef.instance.BackCompleted.subscribe(($event1: any) => {

                                            //});
                                        });
                                }
                            }
                        });
                    }
                }
            });
        });
    }
}
