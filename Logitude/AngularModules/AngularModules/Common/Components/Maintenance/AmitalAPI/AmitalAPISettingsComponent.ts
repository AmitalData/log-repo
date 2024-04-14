import { Component } from "@angular/core";
import { AmitalAPIClientWebService, AmitalApiClient } from "Common/Services/AmitalAPIClientWebService";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { AmitalAPISchemaWebService, AmitalApiSchema, AmitalApiSettings } from "Common/Services/AmitalAPISchemaWebService";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { FieldData } from "./amitalApiTypes";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { AmitalAPIAddClientWindowComponent, AmitalAPIAddClientWindowParams } from "./WindowsComponent/AmitalAPIAddClientWindowComponent";
import { BehaviorSubject, Subject } from "rxjs";

@Component({
    selector: 'appAmitalAPISettings',
    template: `
        <h2 class='subTitle'>{{'Accounting.General.O.Receivables' | TextCodeTranslationPipe}}</h2>

        <button class="Button RedButton margin-vertical" (click)="openAddClientPopup()">{{'General.B.Add' | TextCodeTranslationPipe}}</button>

        <div class='margin-vertical user-select' style='height: 330px; width: 100%; position: relative;' *ngIf='clientTableReady'>
            <logitude-edit-grid GridHeight="100%" GridWidth="100%" [ItemSource]="clientDataSource" (SelectedItemChanged)='clientTenantSelected = $event.Tenant'>
                <log-column [header]="col.label" [width]="col.width" [Alignment]="'center'" [binding]="col.name" *ngFor='let col of clientColumns'>
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
                                <button class="Button RedButton" (click)="openEditClientPopup(item)">{{'General.B.Edit' | TextCodeTranslationPipe}}</button>
                            </div>
                        </log-cell-template>
                    </ng-template>
                </log-column>
            </logitude-edit-grid>
        </div>
        <app-amitalapi-schema-table [schemasTable]='$schemasTable | async' (schemasTableChange)='refreshSchemas()'  [clientTenant]='clientTenantSelected'></app-amitalapi-schema-table>
    `,
    styleUrls: ['./amitalApi.scss'],
    styles: [``],
})
export class AmitalAPISettingsComponent {
    amitalAPIClientWebService = new AmitalAPIClientWebService();
    amitalAPISchemaWebService = new AmitalAPISchemaWebService();
    schemas: AmitalApiSchema[] = [];
    settings: AmitalApiSettings = null;
    clientDataSource = new ObservableCollection([]);
    clientTableReady: boolean = false;
    clientTenantSelected: string = '';
    $schemasTable: BehaviorSubject<AmitalApiSchema[]> = new BehaviorSubject<AmitalApiSchema[]>(null);

    clientColumns: (FieldData & { width: string })[] = [
        { name: 'Name', label: 'Name', width: '100' },
        { name: 'AzureClientId', label: 'Client Id', width: '241' },
        { name: 'AzureApiRegisterName', label: 'Register Name', width: '224' },
        { name: 'SecretValue', label: 'Client Secret', width: '99' },
        { name: 'SecretExpired', label: 'Secret Expired', width: '250' },
        { name: 'AzureManagedApplObjId', label: 'Caller Objectid', width: '253' },
        { name: 'baseAddress', label: 'Base Addres', width: '220' },
        { name: 'authAddress', label: 'Auth Address', width: '220' },
        { name: 'Tenant', label: 'Tenant', width: '66' },
    ];

    async ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const [[settings, clientData],]: [[AmitalApiSettings, AmitalApiClient[]], void] = await Promise.all([
            this.getData(),
            this.refreshSchemas(),
        ]);
        this.initClientTable(clientData, settings);
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.settings = settings;
    }

    async getData(): Promise<[AmitalApiSettings, AmitalApiClient[]]> {
        return Promise.all([
            this.amitalAPISchemaWebService.getSettings(),
            this.amitalAPIClientWebService.getAll(),
        ])
    }

    initClientTable(clientData: AmitalApiClient[], settings: AmitalApiSettings) {
        clientData.forEach(client => {
            client.SecretExpired = new Date(client.SecretExpired).toLocaleString() as any;
            client['baseAddress'] = settings.baseAddress;
            client['authAddress'] = settings.authAddress;
        });
        this.clientDataSource.InsertCollection(clientData);

        this.clientTableReady = true
    }

    async openRemovePopup(id: string) {
        const win = new ConfirmWindow();
        win.Show(TextCodeTranslator.Translate('Accounting.General.O.Areyousuredeleteline'));
        const deleteConfirm: boolean = await win.WindowClosedPromise() as boolean;

        if (!deleteConfirm) return;

        SessionLocator.SelectedSession.StartBusyIndicator('');
        try {
            await this.amitalAPIClientWebService.delete(id);
            await this.refreshClientTable();
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    async refreshClientTable() {
        const clientData: AmitalApiClient[] = await this.amitalAPIClientWebService.getAll();
        this.initClientTable(clientData, this.settings);
    }

    async openEditClientPopup(orginalRow: AmitalApiClient) {
        const row: AmitalApiClient = JSON.parse(JSON.stringify(orginalRow))
        delete row['$id'];
        delete row['baseAddress'];
        delete row['authAddress'];

        const windowArgs: AmitalAPIAddClientWindowParams = { row: row, isUpdate: true, schemas: this.$schemasTable.value };
        const res: AmitalApiClient | boolean = await AmitalAPIAddClientWindowComponent.openWindow(windowArgs);

        if (res) {
            this.refreshSchemas();
            this.refreshClientTable()
        }
    }

    async openAddClientPopup() {
        const windowArgs: AmitalAPIAddClientWindowParams = { isUpdate: false, schemas: this.$schemasTable.value };
        const res: AmitalApiClient | boolean = await AmitalAPIAddClientWindowComponent.openWindow(windowArgs);

        if (res) {
            this.refreshSchemas();
            this.refreshClientTable()
        }
    }

    async refreshSchemas() {
        const schemas = await this.amitalAPISchemaWebService.getAll();
        this.$schemasTable.next(schemas);
    }
}
