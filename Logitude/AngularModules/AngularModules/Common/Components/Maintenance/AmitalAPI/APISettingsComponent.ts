import { Component } from "@angular/core";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { AmitalAPIClientWebService, AmitalApiClient } from "Common/Services/AmitalAPIClientWebService";
import { AmitalAPIClientapiWebService, AmitalApiClientapi } from "Common/Services/AmitalAPIClientapiWebService";
import { AmitalAPISchemaWebService, AmitalApiSchema, AmitalApiSettings } from "Common/Services/AmitalAPISchemaWebService";
import { AmitalAPIAddApiWindowComponent, AmitalAPIAddApiWindowPararms } from "./WindowsComponent/AmitalAPIAddApiWindowComponent";
import { FieldData, MoreParam } from "./amitalApiTypes";
import { AmitalAPIAddWindowService } from "./WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'appAPISettings',
    template: `    
    <div class='form-data-field' *ngIf='headerDataFieldReady' >
        <div *ngFor='let field of fields' class='form-data-field-field'>
            <LogLabel class='HeaderScreenLable' [DataContext]="DataContext" [Text]="field.label" [LayoutDirection]="'ltr'"></LogLabel>
            <span class='HeaderScreenValue' [dir]="'ltr'">{{DataContext[field.name]}}</span>
        </div>
    </div>      
    <div class='body-wrapper'>
        <h2 class='subTitle margin-vertical'>API</h2>

        <button class="Button RedButton margin-vertical" (click)="openAddPopup()">{{'General.B.Add' | TextCodeTranslationPipe}}</button>

        <div class='user-select' style='height: 600px; width: 100%; position: relative;' *ngIf='clientapiTableReady'>
            <logitude-edit-grid GridHeight="100%" GridWidth="100%" [ItemSource]="clientapiDataSource">
                <log-column [header]="col.label" [width]="col.width" [Alignment]="'center'" [binding]="col.name" *ngFor='let col of clientapiColumns'>
                    <ng-template let-item>
                        <log-cell-template [IgnoreMods]="true" #logcelltemplate>
                            <div *ngIf="logcelltemplate.IsDisplayMode" class="TextTrimming" style="text-align:center">
                                {{item[col.name]}}
                            </div>
                        </log-cell-template>
                    </ng-template>
                </log-column>
                <log-column [header]="''" [width]="'140'" [Alignment]="'center'">
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
    </div>
    `,
    styleUrls: ['./fields.scss', './amitalApi.scss'],
    styles: [`
        .body-wrapper {
            -webkit-border-radius: 8px;
            border: 1px solid #c8c8c8;
            padding: 0 15px;
        }

        :host .form-data-field {
            margin-bottom: 0px;
            padding-top: 5px;
            padding-left: 10px;
            padding-right: 10px;
            padding-bottom: 10px;
            background: #F7F7F7;
            border: 1px solid #DADADA;
            border-bottom-width: 0px;
            border-radius: 8px 8px 0px 0px;
            -moz-border-radius: 8px 8px 0px 0px;
            -webkit-border-radius: 8px 8px 0px 0px;    
        }

        :host .form-data-field div {
            flex: 0 0 390px; 
            display: flex;
            overflow: hidden;
        }

        .form-data-field div span {
            width:300px;
            align-self: center;
            text-overflow: ellipsis;
            overflow: hidden;
        }
    `],
})
export class APISettingsComponent extends BaseComponent {
    DataContext: APISettingsComponent | any = this;
    amitalAPIClientWebService = new AmitalAPIClientWebService();
    amitalAPIClientapiWebService = new AmitalAPIClientapiWebService();
    amitalAPISchemaWebService = new AmitalAPISchemaWebService();
    headerDataFieldReady: boolean = false;
    clientapiTableReady: boolean = false;
    schemas: AmitalApiSchema[] = [];
    settings: AmitalApiSettings = null;
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    fields: FieldData[] = [
        { name: 'AzureClientId', label: 'Client Id' },
        { name: 'AzureApiRegisterName', label: 'Client Name' },
        { name: 'SecretValue', label: 'Client Secret' },
        { name: 'SecretExpired', label: 'Secret Expired' },
        { name: 'AzureManagedApplObjId', label: 'Caller Objectid' },
        { name: 'baseAddress', label: 'Base Addres' },
        { name: 'authAddress', label: 'Auth Address' },
        { name: 'ClientTenant', label: 'Client / Tenant' },
    ];
    clientapiDataSource = new ObservableCollection([]);
    moreParamsList: MoreParam[] = [
        { name: 'interface_type', label: 'FTP-Type', schemaId: '002' },
        { name: 'host', label: 'FTP-Address', schemaId: '002' },
        { name: 'directory', label: 'FTP-Directory', schemaId: '002' },
        { name: 'user', label: 'FTP-User', schemaId: '002' },
        { name: 'password', label: 'FTP-Password', schemaId: '002' },
    ];
    clientapiColumns: (FieldData & { width: string })[] = [
        { name: 'Id', label: 'Id', width: '100' },
        { name: 'Schema', label: 'Schema', width: '100' },
        { name: 'ApiType', label: 'Api Type', width: '100' },
        { name: 'Address', label: 'Address', width: '100' },
        { name: 'PartnerName', label: 'PARTNER', width: '100' },
        { name: 'PartnerToken', label: 'Token', width: '240' },
    ].concat(this.moreParamsList.map(param => ({ name: param.name, label: param.label, width: '100' })));

    async ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const [clientapiData, schemas, settings, clientData]: [AmitalApiClientapi[], AmitalApiSchema[], AmitalApiSettings, AmitalApiClient] = await this.getData();
        await Promise.all([
            this.initHeaderFields(settings, clientData),
            this.initClientapiTable(clientapiData, schemas, settings),
        ]);
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.schemas = schemas;
        this.settings = settings;
    }

    async getData(): Promise<[AmitalApiClientapi[], AmitalApiSchema[], AmitalApiSettings, AmitalApiClient]> {
        return Promise.all([
            this.amitalAPIClientapiWebService.getAll(),
            this.amitalAPISchemaWebService.getAll(),
            this.amitalAPISchemaWebService.getSettings(),
            this.amitalAPIClientWebService.get(SessionLocator.Tenant),
        ])
    }

    async initClientapiTable(clientapiData: AmitalApiClientapi[], schemas: AmitalApiSchema[], settings: AmitalApiSettings) {
        clientapiData.forEach((x: AmitalApiClientapi) => {
            const schema: AmitalApiSchema = schemas.find(y => y.Id === x.SchemaId);
            x['Schema'] = x.SchemaId + ' - ' + x.SchemaName;
            x['ApiType'] = schema?.SchemaType;
            x['Address'] = settings.baseAddress + '/' + schema?.Endpoint;
            if (x.MoreParams)
                this.moreParamsList.forEach(param => x[param.name] = x.MoreParams[param.name]);
        })

        this.clientapiDataSource.InsertCollection(clientapiData);

        this.clientapiTableReady = true;
    }

    async initHeaderFields(settings: AmitalApiSettings, clientData: AmitalApiClient) {
        clientData['ClientTenant'] = clientData.Name + ' / ' + clientData.Tenant;

        this.DataContext = { ...this.DataContext, ...clientData, ...settings };

        this.fields.forEach(field => this.UIProperties.SetEnabled(field.name, null, false))
        this.DataContext.fields = this.fields;
        this.headerDataFieldReady = true;
    }

    async refreshClientApiTable() {
        const clientapiData: AmitalApiClientapi[] = await this.amitalAPIClientapiWebService.getAll();
        this.initClientapiTable(clientapiData, this.schemas, this.settings);
    }

    async openRemovePopup(id: string) {
        const win = new ConfirmWindow();
        win.Show(TextCodeTranslator.Translate('Accounting.General.O.Areyousuredeleteline'));
        const deleteConfirm: boolean = await win.WindowClosedPromise() as boolean;

        if (!deleteConfirm) return;

        SessionLocator.SelectedSession.StartBusyIndicator('');
        try {
            await this.amitalAPIClientapiWebService.delete(id);
            await this.refreshClientApiTable();
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    async openAddPopup() {
        const windowArgs: AmitalAPIAddApiWindowPararms = { schemas: this.schemas, moreParams: this.moreParamsList, clientapis: this.clientapiDataSource.Collection, isUpdate: false };
        const res: AmitalApiClientapi | boolean = await AmitalAPIAddApiWindowComponent.openWindow(windowArgs);

        if (res)
            this.refreshClientApiTable();
    }

    async openEditPopup(orginalRow: AmitalApiClientapi) {
        const row: AmitalApiClientapi = JSON.parse(JSON.stringify(orginalRow))
        delete row['Schema'];
        delete row['ApiType'];
        delete row['Address'];

        const windowArgs: AmitalAPIAddApiWindowPararms = { row: row, schemas: this.schemas, moreParams: this.moreParamsList, isUpdate: true };
        const res: AmitalApiClientapi | boolean = await AmitalAPIAddApiWindowComponent.openWindow(windowArgs);

        if (res)
            this.refreshClientApiTable();
    }
}
