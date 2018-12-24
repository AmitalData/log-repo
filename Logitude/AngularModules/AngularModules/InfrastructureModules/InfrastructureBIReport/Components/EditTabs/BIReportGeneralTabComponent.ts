import { Component, OnInit, AfterViewInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'BIReportGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './BIReportGeneralTabComponent.html',
})

export class BIReportGeneralTabComponent extends BaseComponent {

    public EntityPM: BIReportPM;
    public ObjectTableName: string;
    public DataContext: BIReportGeneralTabComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
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

    get Description() { return this.EntityPM.Name; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get DWQueryId() { return this.EntityPM.Name; }
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
}
