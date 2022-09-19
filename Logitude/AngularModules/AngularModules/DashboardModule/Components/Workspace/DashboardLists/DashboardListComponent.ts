import { Component, OnInit, ComponentRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { AgGridNg2 } from 'ag-grid-angular';
import { DashboardAnalyticsService } from 'DashboardModule/Services/DashboardAnalyticsService';
import { WidgetPartArguments } from 'DashboardModule/DataContracts/WidgetPartArguments';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

@Component({
    templateUrl: 'DashboardListComponent.html',
    styleUrls: ['DashboardListComponent.scss'],
})

export class DashboardListComponent extends BaseComponent implements OnInit {
    @ViewChild('agGrid', { static: false }) agGrid: AgGridNg2;
    public ComponentRef: ComponentRef<DashboardListComponent>;
    public DataPointSelection: DataPointSelection;
    public ValidationErrorsList: string[] = [];
    public context = { componentParent: this };
    public columns: any[] = []; 
    private DashboardAnalyticsService = new DashboardAnalyticsService();
    ngOnInit(): void {
        this.DashboardAnalyticsService = new DashboardAnalyticsService();
    }


    Run(dataPointSelection: DataPointSelection) {
        this.DataPointSelection = dataPointSelection;
        this.DashboardAnalyticsService.GetDataAnalyticPart(this.MapDataPointSelectionToWidgetPartArguments(dataPointSelection))
        .subscribe((res:any) => {
            var response: ServiceResponse = res;
            if(!response || response.HasError) return;
        });
    }

     MapDataPointSelectionToWidgetPartArguments(dataPointSelection: DataPointSelection): WidgetPartArguments {
        var widgetPartArguments = new WidgetPartArguments();
        widgetPartArguments.GroupByValue = dataPointSelection.GroupById;
        widgetPartArguments.MeasureFieldId = dataPointSelection.MeasureFieldId;
        widgetPartArguments.Widget = JSON.parse(JSON.stringify(dataPointSelection.Widget));
        return widgetPartArguments;
    }

    BackButtonClicked() {
        this.ComponentRef.destroy();
    }

    RefreshBtnClick() {

    }

    onGridReady(event: any) {
    }
    onSortChanged(event: any) {
    }
    onColumnResized(event: any) {
    }
    onColumnMoved(event: any) {
    }

    BuildColumns(){
        this.columns = [];
        this.agGrid.api.setColumnDefs(this.columns);
    }
}
