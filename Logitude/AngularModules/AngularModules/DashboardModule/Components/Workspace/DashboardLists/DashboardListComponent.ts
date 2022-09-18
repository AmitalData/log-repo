import { Component, OnInit, ComponentRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { AgGridNg2 } from 'ag-grid-angular';

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
    public rowData: any;
    ngOnInit(): void {

    }


    Run(dataPointSelection: DataPointSelection) {
        this.DataPointSelection = dataPointSelection;
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
