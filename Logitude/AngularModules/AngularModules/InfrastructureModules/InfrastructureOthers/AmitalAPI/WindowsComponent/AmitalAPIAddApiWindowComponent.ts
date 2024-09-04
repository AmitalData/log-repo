import { ChangeDetectorRef, Component } from "@angular/core";
import { AmitalAPIClientapiWebService, AmitalApiClientapi } from "Common/Services/AmitalAPIClientapiWebService";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { MoreParam } from "../amitalApiTypes";
import { TextBoxField } from "../components/LogTexBoxFormComponent";
import { AmitalAPIAddWindowService } from "./AmitalAPIAddWindowService";

export type AmitalAPIAddApiWindowPararms = {
    row?: AmitalApiClientapi;
    logWindow?: LogitudeWindow;
    schemas: AmitalApiSchema[];
    moreParams: MoreParam[];
    isUpdate: boolean;
    clientapis?: AmitalApiClientapi[];
};

@Component({
    template: `        
        <log-text-box-form *ngIf='fieldsReady' class='form-data-field' [DataContext]='data' [fields]='fields' [dir]="'ltr'">
            <div class='form-data-field-field'>
                <LogLabel [DataContext]="data" [Text]="'Schema'" [LayoutDirection]="'ltr'"></LogLabel>
                <span>
                    <select #selectedData (change)='onSchemaSelect(selectedData.value)' [value]='SchemaSelected' [ngClass]='{"error-field": schemaRequired}' >
                        <option *ngFor='let schema of windowParams.schemas' [value]='schema.Id'>{{schema.Id}} - {{schema.Name}}</option>
                    </select>
                </span>
            </div>
        </log-text-box-form>

        <p *ngIf='requiredFieldError' class='error-message'>{{'Customs.General.O.RequiredFields' | TextCodeTranslationPipe}}!</p>
        
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>        
    `,
    styleUrls: ['../fields.scss'],
    styles: [`      
        .form-data-field-field {
            margin: 0;
            line-height: unset !important;
        }

        .form-data-field-field span {
            width: 300px;
        }

        .form-data-field-field span select {
            width: 262px;
            height: 22px;
            box-shadow : inset 0 0 3px #AAAAAA;
        }
    `],
})
export class AmitalAPIAddApiWindowComponent {
    fieldsReady: boolean = false;
    windowParams!: AmitalAPIAddApiWindowPararms;
    requiredFieldError: boolean = false;
    schemaRequired: boolean = false;
    SchemaSelected: string = '';
    data: AmitalApiClientapi & any = { UIProperties: new UIProperties() };
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    fields: TextBoxField[] = []
    basicFields: TextBoxField[] = [
        { name: 'PartnerName', label: 'PARTNER' },
        { name: 'Active', label: 'Active', type: 'boolean' },
    ];

    constructor(private readonly cdr: ChangeDetectorRef) { }

    SetWindowArgs(pramas: AmitalAPIAddApiWindowPararms) {
        if (pramas.isUpdate) {
            this.data = { ...this.data, ...pramas.row };
            this.SchemaSelected = pramas.schemas.find(x => x.Id === pramas.row.SchemaId)?.Id;
        }

        this.windowParams = pramas;
        this.updateFields();
    }

    close(save: boolean) {
        if (save)
            this.saveData();
        else
            this.windowParams.logWindow.Close(null);
    }

    async saveData() {
        this.requiredFieldError = !this.chekFormValidation();
        if (this.requiredFieldError) return;
        
        this.data = this.fixRowData(this.data, this.windowParams.row, this.windowParams.moreParams);
        
        const serverCall = this.windowParams.isUpdate ? new AmitalAPIClientapiWebService().update(this.data) : new AmitalAPIClientapiWebService().add(this.data);
        const res = await this.amitalAPIAddWindowService.sendToServer(this.windowParams.isUpdate, serverCall);
        
        this.windowParams.logWindow.Close(res);
    }
    
    chekFormValidation(): boolean {
        this.schemaRequired = !this.data.SchemaId;
        
        const clientAPIAlreadyExists: boolean = !this.windowParams.isUpdate && this.windowParams.clientapis.some(clientapi => clientapi.SchemaId + ';' + clientapi.PartnerName === this.data.SchemaId + ';' + this.data.PartnerName);
        if (clientAPIAlreadyExists)
            new MessageWindow().Show(TextCodeTranslator.Translate('General.O.ClientAPIAlreadyExists'));
    
        let hasError = false || !this.amitalAPIAddWindowService.chekFormValidation(this.fields, this.data) || this.schemaRequired || clientAPIAlreadyExists
        return !hasError;
    }

    onSchemaSelect(schemaId: string) {
        this.data.SchemaId = schemaId;
        this.data.SchemaName = this.windowParams.schemas.find(x => x.Id === schemaId)?.Name;
        this.updateFields();
    }

    updateFields() {
        this.schemaRequired = this.fieldsReady = this.requiredFieldError = false;
        this.SchemaSelected = this.data.SchemaId;

        this.fields = this.basicFields.concat(
            this.windowParams.moreParams
                .filter(x => x.schemaId === this.data.SchemaId)
                .map(x => ({ name: x.name, label: x.label })));
        
        this.fields.forEach(field => field.value = this.data[field.name]);
        this.fieldsReady = true
        this.cdr.detectChanges();
    }

    fixRowData(updateRow: AmitalApiClientapi, row: AmitalApiClientapi, moreParams: MoreParam[]): AmitalApiClientapi {
        const schemaChange: boolean = updateRow.SchemaId !== row?.SchemaId;
        if (schemaChange)
            updateRow.MoreParams = {} as any;

        moreParams
            .filter(param => param.schemaId === updateRow.SchemaId)
            .forEach(param => updateRow.MoreParams[param.name] = updateRow[param.name]);

        moreParams.forEach(param => delete updateRow[param.name]);
        delete updateRow['UIProperties'];

        return updateRow;
    }

    public static openWindow(windowArgs: AmitalAPIAddApiWindowPararms): Promise<AmitalApiClientapi | boolean> {
        const logWindow = new LogitudeWindow();
        windowArgs.logWindow = logWindow;
        logWindow.Width = 850;
        logWindow.Height = 340;
        logWindow.Title = "Client API";
        logWindow.WindowArgs = windowArgs as AmitalAPIAddApiWindowPararms;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/AmitalAPI/WindowsComponent/AmitalAPIAddApiWindowComponent');
        return new Promise<any>(resolve => logWindow.WindowClosed.subscribe(async (row?: any) => resolve(row)));
    }
}