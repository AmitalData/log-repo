import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { AmitalAPISchemaWebService, AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { TextBoxField } from "./components/LogTexBoxFormComponent";

@Component({
    template: `
        <log-text-box-form *ngIf='fieldsReady' [DataContext]='data' [fields]='fields' [dir]="'ltr'"></log-text-box-form>       
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>        
    `,
    styles: [`      
        .form-data-field div LogLabel {
            width: 170px !important;
        }
    `],
})
export class AmitalAPIAddSchemaWindowComponent {
    fieldsReady: boolean = false;
    logWindow: LogitudeWindow;
    data: AmitalApiSchema & any = { UIProperties: new UIProperties() };
    fields: TextBoxField[] = [
        { name: 'Name', label: 'Name' },
        { name: 'SchemaJson', label: 'Schema Json' },
        { name: 'Active', label: 'Active' },
        { name: 'Endpoint', label: 'Endpoint' },
        { name: 'SchemaType', label: 'Schema Type' },
        { name: 'ChunkSize', label: 'Chunk Size' },
        { name: 'tenants', label: 'Tenants' },
    ];

    fieldsForCreate: TextBoxField[] = [
        { name: 'Ref1', label: 'Ref1' },
        { name: 'Ref2', label: 'Ref2' },
        { name: 'Ref3', label: 'Ref3' },
        { name: 'Ref4', label: 'Ref4' },
        { name: 'SaveAsXml', label: 'Save As Xml' },
    ];

    SetWindowArgs({ row, logWindow }: { row: AmitalApiSchema, logWindow: LogitudeWindow }) {
        if (row)
            this.data = { ...this.data, ...row };
        else
            this.fields = this.fields.concat(this.fieldsForCreate);

        this.logWindow = logWindow;
        this.fieldsReady = true;
    }

    close(save: boolean) {
        delete this.data.UIProperties;
        this.logWindow.Close(save ? this.data : null);
    }

    static async openEditPopup(row: AmitalApiSchema, isUpdate: boolean): Promise<AmitalApiSchema | boolean> {
        let updateRow: AmitalApiSchema = await AmitalAPIAddSchemaWindowComponent.openWindow(row);
        if (!updateRow) return;

        updateRow.Tenants = updateRow['tenants']?.split(',').map(t => t.trim());
        let res: AmitalApiSchema | boolean = await AmitalAPIAddSchemaWindowComponent.sendToServer(isUpdate, updateRow);

        return res;
    }

    private static openWindow(row: AmitalApiSchema): Promise<AmitalApiSchema> {
        const logWindow = new LogitudeWindow();
        logWindow.Width = 975;
        logWindow.Height = 345;
        logWindow.Title = "Schema";
        logWindow.WindowArgs = { row, logWindow };
        logWindow.Show('./Common/Components/Maintenance/AmitalAPI/AmitalAPIAddSchemaWindowComponent');
        return new Promise<AmitalApiSchema>(resolve =>
            logWindow.WindowClosed.subscribe(async (row?: AmitalApiSchema) => resolve(row)));
    }

    private static async sendToServer(isUpdate: boolean, updateRow: AmitalApiSchema): Promise<AmitalApiSchema | boolean> {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const serviceApi = new AmitalAPISchemaWebService();
        let res: AmitalApiSchema | boolean = false;

        try {
            res = await (isUpdate ? serviceApi.update(updateRow) : serviceApi.add(updateRow));
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();

        if (!res) {
            const win = new MessageWindow();
            win.ShowErrorIcon = true;
            win.Show(isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') : TextCodeTranslator.Translate('General.O.AddFailed'));
        }

        return res;
    }
}