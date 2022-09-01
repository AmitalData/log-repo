import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { WidgetFilterItem } from './Filter/WidgetFilterItem';

@Component({
    templateUrl: './AddEditWidgetComponent.html',
})

export class AddEditWidgetComponent extends BaseComponent {
    public EntityPM: WidgetPM;
    public DashboardPM: DashboardPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditWidgetComponent;
    private isNew: boolean = false;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Widget";
    public ChartImageSrc: string;
    public WidgetMeasuresList: WidgetMeasureItem[];
    public GroupRoot: WidgetFilterItem = new WidgetFilterItem();
    public IsAddNewMeasureVisible: boolean = true;    
    constructor() {
        super();
        this.WidgetMeasuresList = [];
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DashboardPM = windowArgs['DashboardPM'];
        this.isNew = windowArgs['IsNew'];
        this.DataContext = this;        
        this.ComputeChartImageSrc();
        this.BuildMeasures();
        this.CheckMeasureAddVisiblity();
        this.Clone();
        this.GetFilters();
    }

    GetFilters() {
        if (!this.EntityPM.Filters) return;
        var filters = JSON.parse(this.EntityPM.Filters);
        if (!filters) return;
        this.GroupRoot = filters;
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
    public BuildMeasures() {
        this.WidgetMeasuresList = [];

        this.EntityPM.WidgetMeasures.forEach(item => {
            this.WidgetMeasuresList.push(new WidgetMeasureItem(item, false, this));
        });

        if (this.WidgetMeasuresList.length == 0) {
            var newItem: WidgetMeasurePM = new WidgetMeasurePM(null);
            newItem.Tenant = SessionInfo.LoggedUserTenant;
            newItem.WidgetId = this.EntityPM.Id;
            this.WidgetMeasuresList.push(new WidgetMeasureItem(newItem, true, this));
        }

        this.WidgetMeasuresList.forEach(item => {
            item.CheckMeasureDeleteVisiblity();
        });
    }
    public CheckMeasureAddVisiblity() {
        var isAddVisible: boolean = true;

        if (this.EntityPM.TypeCode == "donut" || this.EntityPM.TypeCode == "pie") {
            isAddVisible = false;
        }

        else if (this.WidgetMeasuresList.length != 1) {
            isAddVisible = false;
        }

        this.IsAddNewMeasureVisible = isAddVisible;
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
            this.GroupRoot = new WidgetFilterItem();
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

    public TableChanged() {
     
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.WidgetMeasuresList.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);
        });

        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.SetFiltersString();
            this.WidgetMeasuresList.forEach(item => {
                if (item.IsNew) {
                    if (this.EntityPM.WidgetMeasures.indexOf(item.EntityPM) == -1) {
                        item.IsNew = false;
                        this.DataContext.EntityPM.AddWidgetMeasure(item.EntityPM);
                    }
                }
            });

            //if(this.GroupRoot && this.GroupRoot.QueryFilterItems && this.GroupRoot.QueryFilterItems.length !=0)
              //  this.EntityPM.Filters = JSON.stringify(this.GroupRoot.QueryFilterItems[0]);
            if (this.isNew) {
                this.isNew = false;
                this.EntityPM.Tenant = SessionInfo.LoggedUserTenant;
                this.DashboardPM.AddWidget(this.EntityPM);
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private SetFiltersString() {
        if (!this.GroupRoot || !this.GroupRoot.QueryFilterItems || this.GroupRoot.QueryFilterItems.length == 0) return;
        this.EntityPM.Filters = JSON.stringify(this.GroupRoot.QueryFilterItems[0], function (key, val) {
            if (key !== "UIProperties" && key !== "IsGroup" && key !== "andOr" &&
                key !== "AndOrOps" && key !== "BooleanList" && key !== "IndexOrder" &&
                key !== "FieldCode" && key !== "Operators" && key !== "SelectedOperator" &&
                key !== "null" && key !== "SelectedField") return val;
        });
    }

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

    AddNewMeasureClicked() {
        var newItem: WidgetMeasurePM = new WidgetMeasurePM(null);
        newItem.Tenant = SessionInfo.LoggedUserTenant;
        newItem.WidgetId = this.EntityPM.Id;

        var newWidgetMeasureItem: WidgetMeasureItem = new WidgetMeasureItem(newItem, true, this);
        this.WidgetMeasuresList.push(newWidgetMeasureItem);
        newWidgetMeasureItem.CheckMeasureDeleteVisiblity();

        this.CheckMeasureAddVisiblity();
    }
}

export class WidgetMeasureItem extends BaseComponent {
    public ObjectTableName: string = "WidgetMeasure";
    public EntityPM: WidgetMeasurePM;
    public Widget: WidgetPM;
    public DataContext: WidgetMeasureItem = this;
    public IsNew: boolean = false;
    public IsDeleteMeasureVisible: boolean = false;
    constructor(entityPM: WidgetMeasurePM, isNew: boolean, public fatherComponent: AddEditWidgetComponent) {
        super();
        this.EntityPM = entityPM;
        this.Widget = fatherComponent.EntityPM;
        this.IsNew = isNew;
    }

    public CheckMeasureDeleteVisiblity() {
        var isVisible: boolean = false;

        var index = this.fatherComponent.WidgetMeasuresList.indexOf(this);

        this.IsDeleteMeasureVisible = (index == 1);
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

    DeleteMeasureClicked() {
        var index = this.fatherComponent.WidgetMeasuresList.indexOf(this);
        if (index != -1) {
            this.fatherComponent.WidgetMeasuresList.splice(index, 1);
        }

        this.fatherComponent.CheckMeasureAddVisiblity();
    }
}
