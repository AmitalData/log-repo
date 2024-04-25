import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { AmitalAPIClientWebService, AmitalApiClient } from "Common/Services/AmitalAPIClientWebService";
import { TextBoxField } from "../components/LogTexBoxFormComponent";
import { AmitalAPIAddWindowService } from "./AmitalAPIAddWindowService";
import { AmitalAPISchemaWebService, AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";

export type AmitalAPIAddClientWindowParams = {
    row?: AmitalApiClient;
    logWindow?: LogitudeWindow;
    isUpdate: boolean;
    schemas: AmitalApiSchema[];
};

@Component({
    template: `
        <log-text-box-form *ngIf='fieldsReady' [DataContext]='data' [fields]='fields' [dir]="'ltr'"></log-text-box-form>
        <div class='schema-section'>
            <LogLabel [DataContext]='data' [Text]="'Schemas'" [LayoutDirection]="'ltr'"></LogLabel>
            <div>
                <select #selectedData (change)='addSchema(selectedData.value)' class='schema-list' [ngClass]='{"DisableCell": data.Tenant == undefined}' >
                    <option value=''>{{'Customs.Declaration.O.Choose' | TextCodeTranslationPipe}}</option>
                    <option *ngFor='let schema of notClientSchemas' [value]='schema.Id'>
                        {{schema.Id}} - {{schema.Name}} - {{schema.SchemaType}}
                    </option>
                </select>

                <div class='schema-list schema-table' *ngIf='fieldsReady'>
                    <logitude-edit-grid GridHeight="100%" GridWidth="100%" [ItemSource]="clientSchemasCollection">
                        <log-column [header]="col" [width]="100" [Alignment]="'center'" [binding]="col" *ngFor='let col of ["Id", "Name", "SchemaType"]'>
                            <ng-template let-item>
                                <log-cell-template [IgnoreMods]="true" #logcelltemplate>
                                    <div *ngIf="logcelltemplate.IsDisplayMode" class="TextTrimming" style="text-align:center">
                                        {{item[col]}}
                                    </div>
                                </log-cell-template>
                            </ng-template>
                        </log-column>
                        <log-column [width]="'70'" [Alignment]="'center'">
                            <ng-template let-item>
                                <log-cell-template [IgnoreMods]="true" #logcelltemplate>
                                    <div *ngIf="logcelltemplate.IsDisplayMode" class="TextTrimming" style="text-align:center">
                                        <button class="Button RedButton" (click)="removeSchema(item.Id)">{{'General.B.Remove' | TextCodeTranslationPipe}}</button>                                
                                    </div>
                                </log-cell-template>
                            </ng-template>
                        </log-column>
                    </logitude-edit-grid>
                </div>

            </div>
        </div>
        <p *ngIf='requiredFieldError' class='error-message'>{{'Customs.General.O.RequiredFields' | TextCodeTranslationPipe}}!</p>
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>
        `,
    styleUrls: ['../fields.scss', './AmitalAPIAddClientWindowComponent.scss'],
})
export class AmitalAPIAddClientWindowComponent {
    requiredFieldError: boolean = false;
    fieldsReady: boolean = false;
    data: AmitalApiClient & any = { UIProperties: new UIProperties() };
    windowParams!: AmitalAPIAddClientWindowParams;
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    clientSchemasCollection: ObservableCollection = new ObservableCollection([]);
    originSchemas: AmitalApiSchema[] = [];
    // clientSchemas: AmitalApiSchema[] = [];
    notClientSchemas: AmitalApiSchema[] = [];

    fields: TextBoxField[] = [
        { name: 'Name', label: 'Name' },
        { name: 'Tenant', label: 'Tenant' },
        { name: 'AzureApiRegisterName', label: 'Azure Api Register Name' },
        { name: 'AzureClientId', label: 'Azure ClientId' },
        { name: 'AzureManagedApplObjId', label: 'Azure Managed Appl Objct Id' },
        { name: 'SecretExpired', label: 'Secret Expired', type: 'date'},
        { name: 'SecretValue', label: 'Secret Value' },
        { name: 'Active', label: 'Active', type: 'boolean' },
    ];

    SetWindowArgs(params: AmitalAPIAddClientWindowParams) {
        this.clientSchemasCollection.Clear();
        this.originSchemas = params.schemas;
        params.schemas = JSON.parse(JSON.stringify(params.schemas));
        this.windowParams = params;
        this.notClientSchemas = params.schemas.filter(schema => !schema.Tenants?.includes(params.row?.Tenant));

        if (params.isUpdate) {
            params.row.SecretExpired = new Date(params.row.SecretExpired) as any;
            this.data = { ...this.data, ...params.row };
            this.clientSchemasCollection.InsertCollection(params.schemas.filter(schema => schema.Tenants?.includes(params.row?.Tenant)));            
        }

        this.fieldsReady = true;
    }

    close(save: boolean) {
        if (save)
            this.saveData();
        else
            this.windowParams.logWindow.Close(null);
    }

    addSchema(schemaId: string) {
        const newSchema = this.notClientSchemas.find(schema => schema.Id === schemaId)
        newSchema.Tenants = newSchema.Tenants || [];
        newSchema.Tenants.push(this.data.Tenant);        
        this.notClientSchemas = this.notClientSchemas.filter(schema => schema.Id !== schemaId);
        this.clientSchemasCollection.Insert(newSchema);
    }

    removeSchema(schemaId: string) {
        const schema = this.clientSchemasCollection.Collection.find(schema => schema.Id === schemaId);
        schema.Tenants = schema.Tenants.filter(tenant => tenant !== this.data.Tenant);
        this.notClientSchemas.push(schema);
        this.clientSchemasCollection.Remove(schema)        
    }

    async saveData() {
        this.requiredFieldError = !this.amitalAPIAddWindowService.chekFormValidation(this.fields, this.data);
        if (this.requiredFieldError) return;

        delete this.data.UIProperties;

        const serviceApiClient = new AmitalAPIClientWebService();
        const serviceApiSchema = new AmitalAPISchemaWebService();
        const serverCallClient = this.windowParams.isUpdate ? serviceApiClient.update(this.data) : serviceApiClient.add(this.data);

        let schemasUpdate = this.windowParams.isUpdate ?
            this.clientSchemasCollection.Collection.concat(this.notClientSchemas).filter(schema =>
                this.originSchemas.some(schema2 => schema.Id === schema2.Id && schema.Tenants?.length !== schema2.Tenants?.length)) :
            this.clientSchemasCollection.Collection;

        const serverCalls: Promise<any>[] = schemasUpdate.map(schema => serviceApiSchema.update(schema));
        serverCalls.push(serverCallClient);
        const serverCall = Promise.all(serverCalls);
        const res: AmitalApiClient | boolean = await this.amitalAPIAddWindowService.sendToServer(this.windowParams.isUpdate, serverCall);

        this.windowParams.logWindow.Close(res as any);
    }

    public static openWindow(windowArgs: AmitalAPIAddClientWindowParams): Promise<AmitalApiClient | boolean> {
        const logWindow = new LogitudeWindow();
        windowArgs.logWindow = logWindow;
        logWindow.Width = 975;
        logWindow.Height = 550;
        logWindow.Title = TextCodeTranslator.Translate('Accounting.General.O.Receivables');
        logWindow.WindowArgs = windowArgs as AmitalAPIAddClientWindowParams;
        logWindow.Show('./Common/Components/Maintenance/AmitalAPI/WindowsComponent/AmitalAPIAddClientWindowComponent');
        return new Promise<AmitalApiClient>(resolve =>
            logWindow.WindowClosed.subscribe(async (row?: AmitalApiClient) => resolve(row)));
    }
}