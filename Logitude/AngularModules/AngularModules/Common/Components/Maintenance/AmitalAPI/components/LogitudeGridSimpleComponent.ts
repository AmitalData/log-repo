import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { RequestQuery } from 'Common/Services/AmitalAPISchemaWebService';
import { LogGridComponent } from 'Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { Subject } from 'rxjs';

@Component({
    selector: 'logitude-grid-simple',
    template: `
        <div class="grid-container" *ngIf="finishBuildColumns">
            <logitude-grid
                        [ngClass]='{"logitude-grid-ltr": !directionRTL}'
                        [dataSource]="tableDataSource"
                        [columns]="gridColumns"
                        [IsCustomTemplate]="true"
                        [autoLoad]="true">
            </logitude-grid>
        </div>
    `,
    styles: [`
      :host ::ng-deep .logitude-grid-ltr list-template span div {
            float: left !important;
        }
    `]
})
export class LogitudeGridSimpleComponent {
    @Input() getData: (skip: number, take: number, page: number) => Promise<any[]>;
    @Input() directionRTL: boolean = true;
    @Input() htmlTemplateComponentUrl!: string;
    @Input() set columns(value: GridColumn[]) {
        this.gridColumns = this.createGridColumns(value);
        this.finishBuildColumns = true;
        this.cd.detectChanges();
        this.UpdateToLTR();
    }    

    @ViewChild(LogGridComponent) logGridComponent!: LogGridComponent;

    finishBuildColumns: boolean = false;
    datasourceReady: boolean = false;
    tableDataSource = { getRows: this.getRows.bind(this) };
    gridColumns: GridColumn[] = [];
    listTemplateComponentsLength: number = 0;

    constructor(private cd: ChangeDetectorRef) { }

    ngOnInit(): void {
    }

    private UpdateToLTR() {
        if(this.directionRTL) return;

        this.logGridComponent.RTL = false;
        this.logGridComponent.ngOnInit();
        this.cd.detectChanges();
    }

    private async getRows(skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        const observable = new Subject();
        const page: number = take == 0 ? 1 : (skip / take) + 1;

        this.getData(skip, take, page).then(async (res: RequestQuery[]) => {
            res.forEach(x => { (<any>x.IsParent) = x.IsParent ? 'Yes' : 'No'; });

            const rowCount: number = page * take +
                (this.logGridComponent.viewportSize == res.length ? take : (-1 * take) + res.length);

            observable.next({ Result: res, Count: rowCount });

            this.logGridComponent.canvasHeight = { height: rowCount * this.logGridComponent.rowHeight + 'px' };
            this.logGridComponent.rowCount = rowCount;
        })

        return new Promise(resolve => resolve(observable));
    }

    private createGridColumns(columns: GridColumn[]) {
        const gridColumns: GridColumn[] = JSON.parse(JSON.stringify(columns));
        gridColumns.forEach(col => {
            col['FieldName'] = col.FieldName;
            col['DataTypeCode'] = col.DataTypeCode || 'string';
            col['Display'] = col.Display || col.FieldName;
            col['Styles'] = col.Styles || { width: '220px' };
            col['IsCustomTemplate'] = col.IsCustomTemplate || true;
            col['ServerSideSortable'] = col.ServerSideSortable || true;
            col['SortByName'] = col.SortByName || col.FieldName;

            if (col.isTemplate) {
                col['HtmlListComponentName'] = this.htmlTemplateComponentUrl.split('/').pop();
                col['HtmlListComponentUrl'] = this.htmlTemplateComponentUrl;
            }
        })

        return gridColumns;
    }

    public refreshTable() {
        SessionLocator.SelectedSession.StartBusyIndicator('');

        this.finishBuildColumns = false;
        this.cd.detectChanges()
        this.finishBuildColumns = true;
        this.cd.detectChanges()
        this.logGridComponent.init(true);
        this.UpdateToLTR();

        SessionLocator.SelectedSession.StopBusyIndicator();
    }
}

export type GridColumn = {
    FieldName: string;
    DataTypeCode?: string;
    Display?: string;
    Styles?: any;
    IsCustomTemplate?: boolean;
    ServerSideSortable?: boolean;
    SortByName?: string;
    isTemplate?: boolean;
};