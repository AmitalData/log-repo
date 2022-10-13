import { Component, OnInit, ComponentRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { AgGridNg2 } from 'ag-grid-angular';
import { DashboardAnalyticsService } from 'DashboardModule/Services/DashboardAnalyticsService';
import { WidgetPartArguments } from 'DashboardModule/DataContracts/WidgetPartArguments';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AnalyticsFactsFieldsMetaDataPM } from 'DashboardModule/EntityPMs/AnalyticsFactsFieldsMetaDataPM';
import { ShipmentPMService } from 'Shipment/Services/StandardPMs/ShipmentPMService';
import { EditShipmentLinkRendererComponent } from 'DashboardModule/Components/ListTemplates/EditShipmentLinkRendererComponent';
import * as moment from 'moment';
import { DashboardMapping } from 'Dashboard/Services/DashboardMapping';
import { DashboardPM } from 'DashboardModule/EntityPMs/DashboardPM';

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
    private CurrentSession = SessionLocator.SelectedSession;
    public _ShipmentPMService: ShipmentPMService;

    ngOnInit(): void {
        this.DashboardAnalyticsService = new DashboardAnalyticsService();
        this._ShipmentPMService = new ShipmentPMService();
    }


    Run(dataPointSelection: DataPointSelection) {
        this.StartBusyIndicator();
        this.DataPointSelection = dataPointSelection;
        this.DashboardAnalyticsService.GetDataAnalyticPart(this.MapDataPointSelectionToWidgetPartArguments(dataPointSelection))
            .subscribe((res: any) => {
                this.StopBusyIndicator();
                var response: ServiceResponse = res;
                if (!response || response.HasError) return;
                this.BuildGrid(response.Result);
            });
    }

    private BuildGrid(result: any) {
        this.rowData = result.DataResult;
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
        if (metaDataField.FieldCode == "ShipmentNumber") {
            column["cellRendererFramework"] = EditShipmentLinkRendererComponent;
        }
        if (metaDataField.DataTypeCode == "Date" || metaDataField.DataTypeCode == "DateTime") {
            column["cellRenderer"] = this.DateFormatter;
        }
        return column;
    }

    DateFormatter(params) {
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
        widgetPartArguments.MeasureFieldId = dataPointSelection.MeasureFieldId;
        widgetPartArguments.Widget = DashboardMapping.GetWidgetPMFromReact(dataPointSelection.Widget);
        return widgetPartArguments;
    }

    BackButtonClicked() {
        this.ComponentRef.destroy();
    }

    RefreshBtnClick() {
        this.Run(this.DataPointSelection);
    }

    onGridReady(event: any) {
    }
    onSortChanged(event: any) {
    }
    onColumnResized(event: any) {
    }
    onColumnMoved(event: any) {
    }

    public OnShipmentNumberClick(cell) {
        this.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.getSingleByShipmentNumber(cell).subscribe((myResult: any) => {
            if (!myResult.HasError) {
                var Id = myResult.Result;
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: Id, ObjectTableName: 'Shipment', BackButtonLabel: "Dashboard" });
                    });
            }

            this.StopBusyIndicator();
        });
    }
}
