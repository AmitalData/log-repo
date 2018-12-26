import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

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

    constructor() {
        super();
        this.EntityPM = new BIReportPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.TypeCode ="EXL";
        this.myService = new BIReportPMService();
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, false);
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
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null) {
                    this.DWQueryId = s.Id;
                }
            });
        });
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit('cancel');
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
            });
        }
    }
}
