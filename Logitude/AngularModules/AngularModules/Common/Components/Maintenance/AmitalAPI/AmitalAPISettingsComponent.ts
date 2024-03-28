import { Component } from "@angular/core";
import { AmitalAPIClientWebService, AmitalApiClient } from "Common/Services/AmitalAPIClientWebService";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { AmitalAPISchemaWebService, AmitalApiSchema, AmitalApiSettings } from "Common/Services/AmitalAPISchemaWebService";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { FieldData } from "./amitalApiTypes";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { AmitalAPIAddClientWindowComponent } from "./AmitalAPIAddClientWindowComponent";

@Component({
    selector: 'appAmitalAPISettings',
    template: `
        <h2 class='subTitle'>{{'Accounting.General.O.Receivables' | TextCodeTranslationPipe}}</h2>

        <button class="Button RedButton margin-vertical" (click)="openAddClientPopup()">{{'General.B.Add' | TextCodeTranslationPipe}}</button>

        <div class='margin-vertical user-select' style='height: 330px; width: 100%; position: relative;' *ngIf='clientTableReady'>
            <logitude-edit-grid GridHeight="100%" GridWidth="100%" [ItemSource]="clientDataSource">
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
        <app-amitalapi-schema-table></app-amitalapi-schema-table>
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

    clientColumns: (FieldData & { width: string })[] = [
        { name: 'AzureClientId', label: 'Client Id', width: '241' },
        { name: 'AzureApiRegisterName', label: 'Client Name', width: '224' },
        { name: 'SecretValue', label: 'Client Secret', width: '99' },
        { name: 'SecretExpired', label: 'Secret Expired', width: '250' },
        { name: 'AzureManagedApplObjId', label: 'Caller Objectid', width: '253' },
        { name: 'baseAddress', label: 'Base Addres', width: '298' },
        { name: 'authAddress', label: 'Auth Address', width: '265' },
        { name: 'Tenant', label: 'Tenant', width: '66' },
    ];

    async ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const [schemas, settings, clientData]: [AmitalApiSchema[], AmitalApiSettings, AmitalApiClient[]] = await this.getData();
        await this.initClientTable(clientData, schemas, settings)
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.schemas = schemas;
        this.settings = settings;
    }

    async getData(): Promise<[AmitalApiSchema[], AmitalApiSettings, AmitalApiClient[]]> {
        return Promise.all([
            this.amitalAPISchemaWebService.getAll(),
            this.amitalAPISchemaWebService.getSettings(),
            this.amitalAPIClientWebService.getAll(),
        ])
    }

    async initClientTable(clientData: AmitalApiClient[], schemas: AmitalApiSchema[], settings: AmitalApiSettings) {

        clientData.forEach(client => {
            client['baseAddress'] = settings.baseAddress;
            client['authAddress'] = settings.authAddress;
        });
        this.clientDataSource.InsertCollection(clientData);
        console.log(clientData)

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
        this.initClientTable(clientData, this.schemas, this.settings);
    }

    async openEditClientPopup(orginalRow: AmitalApiClient) {
        const row: AmitalApiClient = JSON.parse(JSON.stringify(orginalRow))
        delete row['$id'];
        delete row['baseAddress'];
        delete row['authAddress'];

        const res: AmitalApiClient | boolean = await AmitalAPIAddClientWindowComponent.openEditPopup(row, true);
        if (res)
            this.refreshClientTable()
    }

    async openAddClientPopup() {
        const res: AmitalApiClient | boolean = await AmitalAPIAddClientWindowComponent.openEditPopup(null, false);
        if (res)
            this.refreshClientTable()
    }
}
