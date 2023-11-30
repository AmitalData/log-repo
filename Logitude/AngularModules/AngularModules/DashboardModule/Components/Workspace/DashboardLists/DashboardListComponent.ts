import { Component, OnInit, ComponentRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { AgGridNg2 } from 'ag-grid-angular';
import { DashboardAnalyticsService } from 'DashboardModule/Services/DashboardAnalyticsService';
import { WidgetPartArguments } from 'DashboardModule/DataContracts/WidgetPartArguments';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AnalyticsFactsFieldsMetaDataPM } from 'DashboardModule/EntityPMs/AnalyticsFactsFieldsMetaDataPM';
import { DashboardListLinkRendererComponent } from 'DashboardModule/Components/ListTemplates/DashboardListLinkRendererComponent';
import * as moment from 'moment';
import { DashboardMapping } from 'DashboardModule/Tools/DashboardMapping';
import { DashboardPM } from 'DashboardModule/EntityPMs/DashboardPM';
import { EntityPMService } from 'Infrastructure/Services/EntityPMService';
import { AnalyticsFactsMetaDataPMService } from 'DashboardModule/Services/StandardPMs/AnalyticsFactsMetaDataPMService';

@Component({
    templateUrl: 'DashboardListComponent.html',
    styleUrls: ['DashboardListComponent.scss'],
})

export class DashboardListComponent extends BaseComponent implements OnInit {
    @ViewChild('agGrid', { static: false }) agGrid: AgGridNg2;
    public ComponentRef: ComponentRef<DashboardListComponent>;
    public DataPointSelection: DataPointSelection;
    public Dashboard: DashboardPM;
    public ValidationErrorsList: string[] = [];
    public context = { componentParent: this };
    public columns: any[] = [];
    public rowData: any;
    private DashboardAnalyticsService = new DashboardAnalyticsService();
    private AnalyticsFactsMetaDataPMService = new AnalyticsFactsMetaDataPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    public entityPMService: EntityPMService;
    public Title: string;

    ObjectTableName: any;

    ngOnInit(): void {
        this.DashboardAnalyticsService = new DashboardAnalyticsService();
        this.AnalyticsFactsMetaDataPMService = new AnalyticsFactsMetaDataPMService();
        this.entityPMService = new EntityPMService();
    }


    Run(dataPointSelection: DataPointSelection) {
        this.DataPointSelection = dataPointSelection;
        this.GetObjectTableName();
        this.SetTitle();
    }

    SetTitle() {
        if (!this.Dashboard?.Name) return;
        if (this.Dashboard.Name.length <= 12) this.Title = this.Dashboard.Name;
        else this.Title = this.Dashboard.Name.substring(0, 9) + "...";
    }

    GetObjectTableName() {
        this.StartBusyIndicator();
        this.AnalyticsFactsMetaDataPMService.get(this.DataPointSelection.Widget.EntityId).subscribe((response: ServiceResponse) => {
            if (response.HasError) {
                return;
            }
            this.ObjectTableName = response.Result.ObjectTableName;
            this.GetList();
        });
    }

    GetList(isRefresh: boolean = false) {
        this.DashboardAnalyticsService.GetDataAnalyticPart(this.MapDataPointSelectionToWidgetPartArguments(this.DataPointSelection))
            .subscribe((res: any) => {
                this.StopBusyIndicator();
                var response: ServiceResponse = res;
                if (!response || response.HasError) return;
                this.BuildGrid(response.Result, isRefresh);
            });
    }

    private BuildGrid(result: any, isRefresh: boolean) {
        this.rowData = result.DataResult;
        if (isRefresh) return;
        var fields = result.Fields as AnalyticsFactsFieldsMetaDataPM[];
        this.columns = [];
        fields.forEach(metaDataField => {
            this.columns.push(this.BuildColumn(metaDataField));
        });
    }

    private BuildColumn(metaDataField: AnalyticsFactsFieldsMetaDataPM) {
        var column = {};
        column["field"] = metaDataField.FieldCode;
        column["headerName"] = metaDataField.DisplayName;
        column["sortable"] = true;
        this.SetClickColumn(metaDataField, column);
        if (metaDataField.DataTypeCode == "Date" || metaDataField.DataTypeCode == "DateTime") {
            column["cellRenderer"] = this.DateFormatter;
        }
        return column;
    }

    private SetClickColumn(metaDataField: AnalyticsFactsFieldsMetaDataPM, column: {}) {
        if ((metaDataField.FieldCode == "ShipmentNumber" && this.ObjectTableName == "Shipment") ||
            (metaDataField.FieldCode == "InvoiceNumber" && this.ObjectTableName == "APInvoice") ||
            (metaDataField.FieldCode == "InvoiceNumber" && this.ObjectTableName == "ARInvoice") ||
            (metaDataField.FieldCode == "ContainerNumber" && this.ObjectTableName == "Container") ||
            (metaDataField.FieldCode == "QuoteNumber" && this.ObjectTableName == "Quote") ||
            (metaDataField.FieldCode == "Subject" && this.ObjectTableName == "Opportunity")) {
            column["cellRendererFramework"] = DashboardListLinkRendererComponent;
        }
    }

    DateFormatter(params) {
        if(!params?.value) return '';
        return moment(params.value).format('DD/MM/YYYY');
    }

    public StartBusyIndicator(message: string = "Generating...", width: number = 200) {
        this.CurrentSession.StartBusyIndicator(message);

    }
    public StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }


    MapDataPointSelectionToWidgetPartArguments(dataPointSelection: DataPointSelection): WidgetPartArguments {
        var widgetPartArguments = new WidgetPartArguments();
        widgetPartArguments.GroupByValue = dataPointSelection.GroupById;
        widgetPartArguments.GroupBySecValue = dataPointSelection.GroupByIdSec;
        widgetPartArguments.MeasureFieldId = dataPointSelection.MeasureFieldId;
        widgetPartArguments.Widget = DashboardMapping.GetWidgetPMFromReact(dataPointSelection.Widget);
        return widgetPartArguments;
    }

    BackButtonClicked() {
        this.ComponentRef.destroy();
    }

    RefreshBtnClick() {
        this.StartBusyIndicator();
        this.GetList(true);
    }

    onGridReady(event: any) {
    }
    onSortChanged(event: any) {
    }
    onColumnResized(event: any) {
    }
    onColumnMoved(event: any) {
    }

    public OnDashboardListClick(id: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: this.ObjectTableName, BackButtonLabel: "Dashboard" });
            });
    }
}
