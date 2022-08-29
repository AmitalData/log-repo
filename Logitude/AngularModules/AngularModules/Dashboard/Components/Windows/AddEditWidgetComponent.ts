import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';

@Component({
    templateUrl: './AddEditWidgetComponent.html',
})

export class AddEditWidgetComponent extends BaseComponent {
    public EntityPM: WidgetPM;
    public DashboardPM: DashboardPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditWidgetComponent;
    private isNew: boolean = false;
    private dashboardService: DashboardPMService;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Widget";
    public ChartImageSrc: string;
    public WidgetMeasuresList: WidgetMeasureItem[];
    constructor() {
        super();
        this.dashboardService = new DashboardPMService();
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DashboardPM = windowArgs['DashboardPM'];
        this.DataContext = this;
        this.isNew = AppTool.IsNullOrEmpty(this.EntityPM.Id);
        this.ComputeChartImageSrc();
        this.BuildMeasures();
        this.Clone();
    }

    private ComputeChartImageSrc() {
        switch (this.EntityPM.TypeCode) {
            case "Pie": {
                this.ChartImageSrc = "./Images/PieChart.png";
                break;
            }

            case "Bar": {
                this.ChartImageSrc = "./Images/BarChart.png";
                break;
            }

            case "Are": {
                this.ChartImageSrc = "./Images/AreaChart.png";
                break;
            }

            case "Don": {
                this.ChartImageSrc = "./Images/DonutChart.png";
                break;
            }

            case "Co": {
                this.ChartImageSrc = "";
                break;
            }
        }
    }
    private BuildMeasures() {
        this.EntityPM.WidgetMeasures.forEach(item => {
            this.WidgetMeasuresList.push(new WidgetMeasureItem(item));
        });
    }

    get Title() { return this.EntityPM.Title }
    set Title(value: string) {
        if (this.EntityPM.Title != value) {
            this.EntityPM.Title = value;
        }
    }

    get EntityId() { return this.EntityPM.EntityId }
    set EntityId(value: string) {
        if (this.EntityPM.EntityId != value) {
            this.EntityPM.EntityId = value;
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            this.EntityPM.TypeCode = value;
        }
    }

    get GroupById() { return this.EntityPM.GroupById; }
    set GroupById(value: string) {
        if (this.EntityPM.GroupById != value) {
            this.EntityPM.GroupById = value;
        }
    }

    get StartPotistion() { return this.EntityPM.StartPotistion; }
    set StartPotistion(value: string) {
        if (this.EntityPM.StartPotistion != value) {
            this.EntityPM.StartPotistion = value;
        }
    }

    get EndPosition() { return this.EntityPM.EndPosition; }
    set EndPosition(value: string) {
        if (this.EntityPM.EndPosition != value) {
            this.EntityPM.EndPosition = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.isNew) {
                this.isNew = false;
                this.EntityPM.Tenant = SessionInfo.LoggedUserTenant;
                this.DashboardPM.AddWidget(this.EntityPM);
            }

            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }   

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Title');
        this.myCloner.AddField('TypeCode');
        this.myCloner.AddField('GroupBy');
        this.myCloner.AddField('StartPotistion');
        this.myCloner.AddField('EndPosition');

        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DashboardPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

export class WidgetMeasureItem {
    public ObjectTableName: string = "WidgetMeasure";
    public EntityPM: WidgetMeasurePM;
    constructor(entityPM: WidgetMeasurePM) {
        this.EntityPM = entityPM;
    }

    get MeasureFieldId() { return this.EntityPM.MeasureFieldId; }
    set MeasureFieldId(value: string) {
        if (this.EntityPM.MeasureFieldId != value) {
            this.EntityPM.MeasureFieldId = value;
        }
    }

    get MeasureCode() { return this.EntityPM.MeasureCode; }
    set MeasureCode(value: string) {
        if (this.EntityPM.MeasureCode != value) {
            this.EntityPM.MeasureCode = value;
        }
    }
}
