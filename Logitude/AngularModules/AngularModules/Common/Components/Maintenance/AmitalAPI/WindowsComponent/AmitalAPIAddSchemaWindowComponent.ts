import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { AmitalAPISchemaWebService, AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { TextBoxField } from "../components/LogTexBoxFormComponent";
import { AmitalAPIAddWindowService } from "./AmitalAPIAddWindowService";

export type AmitalAPIAddSchemaWindowParams = {
    row?: AmitalApiSchema;
    logWindow?: LogitudeWindow;
    isUpdate: boolean;
};

@Component({
    template: `
        <log-text-box-form *ngIf='fieldsReady' [DataContext]='data' [fields]='fields' [dir]="'ltr'"></log-text-box-form>       
        <p *ngIf='requiredFieldError' class='error-message'>{{'Customs.General.O.RequiredFields' | TextCodeTranslationPipe}}!</p>       
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>        
    `,
    styleUrls: ['../fields.scss'],
    styles: [`      
        .form-data-field div LogLabel {
            width: 170px !important;
        }
    `],
})
export class AmitalAPIAddSchemaWindowComponent {
    fieldsReady: boolean = false;
    requiredFieldError: boolean = false;
    windowParams!: AmitalAPIAddSchemaWindowParams;
    data: AmitalApiSchema & any = { UIProperties: new UIProperties() };
    amitalAPIAddWindowService: AmitalAPIAddWindowService = new AmitalAPIAddWindowService();
    fields: TextBoxField[] = [
        { name: 'Name', label: 'Name' },
        { name: 'SchemaJson', label: 'Schema Json' },
        { name: 'Active', label: 'Active', type: 'boolean' },
        { name: 'Endpoint', label: 'Endpoint' },
        { name: 'SchemaType', label: 'Schema Type' },
        { name: 'ChunkSize', label: 'Chunk Size' },
        // { name: 'tenants', label: 'Tenants' },
    ];

    fieldsForCreate: TextBoxField[] = [
        { name: 'Ref1', label: 'Ref1' },
        { name: 'Ref2', label: 'Ref2' },
        { name: 'Ref3', label: 'Ref3' },
        { name: 'Ref4', label: 'Ref4' },
        { name: 'SaveAsXml', label: 'Save As Xml', type: 'boolean' },
    ];

    SetWindowArgs(params: AmitalAPIAddSchemaWindowParams) {
        if (params.row){
            this.data = { ...this.data, ...params.row };
            this.fields.forEach(field => field.value = this.data[field.name]);
        } else
            this.fields = this.fields.concat(this.fieldsForCreate);

        this.windowParams = params;
        this.fieldsReady = true;
    }

    close(save: boolean) {
        if (save)
            this.saveData();
        else
            this.windowParams.logWindow.Close(null);
    }

    async saveData() {
        const requiredFields = this.fields.filter(f => !f.name.includes('Ref'));
        this.requiredFieldError = !this.amitalAPIAddWindowService.chekFormValidation(requiredFields, this.data);
        if (this.requiredFieldError) return;

        delete this.data.UIProperties;
        // this.data.Tenants = this.data.updateRow['tenants']?.split(',').map(t => t.trim());

        const serviceApi = new AmitalAPISchemaWebService();
        const serverCall = this.windowParams.isUpdate ? serviceApi.update(this.data) : serviceApi.add(this.data);
        const res: AmitalApiSchema | boolean = await this.amitalAPIAddWindowService.sendToServer(this.windowParams.isUpdate, serverCall);

        this.windowParams.logWindow.Close(res as any);
    }

    public static openWindow(windowArgs: AmitalAPIAddSchemaWindowParams): Promise<AmitalApiSchema | boolean> {
        const logWindow = new LogitudeWindow();
        windowArgs.logWindow = logWindow;
        logWindow.Width = 975;
        logWindow.Height = 345;
        logWindow.Title = "Schema";
        logWindow.WindowArgs = windowArgs as AmitalAPIAddSchemaWindowParams;
        logWindow.Show('./Common/Components/Maintenance/AmitalAPI/AmitalAPIAddSchemaWindowComponent');
        return new Promise<AmitalApiSchema>(resolve =>
            logWindow.WindowClosed.subscribe(async (row?: AmitalApiSchema) => resolve(row)));
    }
}