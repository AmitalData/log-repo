import { Component } from "@angular/core";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { AmitalAPISchemaWebService, AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { FieldData } from "./amitalApiTypes";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { AmitalAPIAddSchemaWindowComponent } from "./AmitalAPIAddSchemaWindowComponent";

@Component({
    selector: 'app-amitalapi-schema-table',
    template: `
        <h2 class='subTitle'>Schemas</h2>

        <button class="Button RedButton margin-vertical" (click)="openAddPopup()">{{'General.B.Add' | TextCodeTranslationPipe}}</button>

        <div class='margin-vertical user-select' style='height: 330px; width: 100%; position: relative;' *ngIf='schemaTableReady'>
            <logitude-edit-grid GridHeight="100%" GridWidth="100%" [ItemSource]="schemaDataSource">
                <log-column [header]="col.label" [width]="col.width" [Alignment]="'center'" [binding]="col.name" *ngFor='let col of schemaColumns'>
                    <ng-template let-item>
                        <log-cell-template [IgnoreMods]="true" #logcelltemplate>
                            <div *ngIf="logcelltemplate.IsDisplayMode" class="TextTrimming" style="text-align:center">
                                {{item[col.name]}}
                            </div>
                        </log-cell-template>
                    </ng-template>
                </log-column>
                <log-column [width]="'140'" [Alignment]="'center'">
                    <ng-template let-item>
                        <log-cell-template [IgnoreMods]="true" #logcelltemplate>
                            <div *ngIf="logcelltemplate.IsDisplayMode" class="TextTrimming" style="text-align:center">
                                <button class="Button RedButton" (click)="openRemovePopup(item.Id)">{{'General.B.Remove' | TextCodeTranslationPipe}}</button>
                                <button class="Button RedButton" (click)="openEditPopup(item)">{{'General.B.Edit' | TextCodeTranslationPipe}}</button>
                            </div>
                        </log-cell-template>
                    </ng-template>
                </log-column>
            </logitude-edit-grid>
        </div>
    `,
    styleUrls: ['./amitalApi.scss'],
    styles: [``],
})
export class AmitalAPISchemaTable {
    amitalAPISchemaWebService = new AmitalAPISchemaWebService();
    schemaDataSource = new ObservableCollection([]);
    schemaTableReady: boolean = false;
    schemaColumns: (FieldData & { width: string })[] = [
        { name: 'Id', label: 'Id', width: '37' },
        { name: 'Name', label: 'Name', width: '100' },
        { name: 'SchemaJson', label: 'Schema Json', width: '100' },
        { name: 'Ref1', label: 'Ref1', width: '42' },
        { name: 'Ref2', label: 'Ref2', width: '42' },
        { name: 'Ref3', label: 'Ref3', width: '42' },
        { name: 'Ref4', label: 'Ref4', width: '42' },
        { name: 'CreateDate', label: 'CreateDate', width: '170' },
        { name: 'UpdateDate', label: 'UpdateDate', width: '170' },
        { name: 'Active', label: 'Active', width: '50' },
        { name: 'Endpoint', label: 'Endpoint', width: '100' },
        { name: 'SchemaType', label: 'Schema Type', width: '100' },
        { name: 'SaveAsXml', label: 'Save As Xml', width: '100' },
        { name: 'ChunkSize', label: 'Chunk Size', width: '100' },
        { name: 'tenants', label: 'Tenants', width: '60' },
    ];

    async ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const schemas: AmitalApiSchema[] = await this.amitalAPISchemaWebService.getAll();
        this.initTable(schemas)
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.schemaTableReady = true;
    }

    async openRemovePopup(id: string) {
        const win = new ConfirmWindow();
        win.Show(TextCodeTranslator.Translate('Accounting.General.O.Areyousuredeleteline'));
        const deleteConfirm: boolean = await win.WindowClosedPromise() as boolean;

        if (!deleteConfirm) return;

        SessionLocator.SelectedSession.StartBusyIndicator('');
        try {
            await this.amitalAPISchemaWebService.delete(id);
            await this.refreshTable();
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    async refreshTable() {
        const schemasData: AmitalApiSchema[] = await this.amitalAPISchemaWebService.getAll();
        this.initTable(schemasData);
    }

    initTable(schemasData: AmitalApiSchema[]) {
        schemasData.forEach(schema => {
            schema['tenants'] = schema.Tenants?.join(', ')
            schema.UpdateDate = new Date(schema.UpdateDate).toLocaleString() as any;
            schema.CreateDate = new Date(schema.CreateDate).toLocaleString() as any;
        });

        this.schemaDataSource.InsertCollection(schemasData);
    }

    async openEditPopup(orginalRow: AmitalApiSchema) {
        const row: AmitalApiSchema = JSON.parse(JSON.stringify(orginalRow))
        const res: AmitalApiSchema | boolean = await AmitalAPIAddSchemaWindowComponent.openEditPopup(row, true);
        if (res)
            this.refreshTable();
    }

    async openAddPopup() {
        const res: AmitalApiSchema | boolean = await AmitalAPIAddSchemaWindowComponent.openEditPopup(null, false);
        if (res)
            this.refreshTable();
    }
}
