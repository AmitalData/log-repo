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
    public RootFilter: WidgetFilterItem = new WidgetFilterItem();
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
        this.RootFilter = this.BuildRootFilter(filters);
    }

    private BuildRootFilter(oldValue: WidgetFilterItem) {
        let parentItem = new WidgetFilterItem();
        parentItem.QueryFilterItems.push(this.BuildGroupFilter(oldValue));
        return parentItem;
    }

    private BuildGroupFilter(oldFilter: WidgetFilterItem) {
        let groupTreeFilter = new WidgetFilterItem();
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(oldFilter.FilterType);
        oldFilter.QueryFilterItems?.forEach((oldField) => {
            groupTreeFilter.QueryFilterItems.push(this.BuildFilter(oldField));
        });
        return groupTreeFilter;
    }

    private BuildFilter(oldField: WidgetFilterItem) {
        if (oldField.QueryFilterItems.length == 0) return new WidgetFilterItem(oldField);
        return this.BuildGroupFilter(oldField);
    }


    private ComputeChartImageSrc() {
        switch (this.EntityPM.TypeCode) {
            case "pie": {
                this.ChartImageSrc = "./Images/Charts/PieChart.png";
                break;
            }

            case "bar": {
                this.ChartImageSrc = "./Images/Charts/BarChart.png";
                break;
            }

            case "line": {
                this.ChartImageSrc = "./Images/Charts/LineChart.png";
                break;
            }

            case "donut": {
                this.ChartImageSrc = "./Images/Charts/DonutChart.png";
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
            this.RootFilter = new WidgetFilterItem();
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

        this.WidgetMeasuresList.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);
        });

        this.ValidationErrorsList = errors;
        if (errors.length != 0) return;

        this.SetFiltersString();
        this.WidgetMeasuresList.forEach(item => {
            if (item.IsNew && this.EntityPM.WidgetMeasures.indexOf(item.EntityPM) == -1) {
                item.IsNew = false;
                this.DataContext.EntityPM.AddWidgetMeasure(item.EntityPM);
            }
        });
        if (this.isNew) {
            this.isNew = false;
            this.EntityPM.Tenant = SessionInfo.LoggedUserTenant;
            this.DashboardPM.AddWidget(this.EntityPM);
        }
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    private myCloner: Cloner;
    private SetFiltersString() {
        if (!this.RootFilter || !this.RootFilter.QueryFilterItems || this.RootFilter.QueryFilterItems.length == 0) {
            this.EntityPM.Filters = null;
            return;
        }
        this.EntityPM.Filters = JSON.stringify(this.RootFilter.QueryFilterItems[0], function (key, val) {
            if (key !== "UIProperties" && key !== "IsGroup" && key !== "andOr" &&
                key !== "AndOrOps" && key !== "BooleanList" && key !== "IndexOrder" &&
                key !== "FieldCode" && key !== "Operators" && key !== "SelectedOperator" &&
                key !== "null" && key !== "SelectedField" && key !== "DontRefreshFieldData") return val;
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
