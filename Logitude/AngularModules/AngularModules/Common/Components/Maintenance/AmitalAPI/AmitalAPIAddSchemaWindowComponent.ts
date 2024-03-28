import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { FieldData } from "./amitalApiTypes";
import { AmitalAPISchemaWebService, AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

@Component({
    template: `
        <div class='client-data-field margin-vertical' *ngIf='fieldsReady'>
            <div *ngFor='let field of fields'>
                <LogLabel [DataContext]="data" [Text]="field.label" [LayoutDirection]="'ltr'"></LogLabel>
                <LogTextBox [DataContext]="data" [ObjectFieldName]='field.name' [dir]="'ltr'"></LogTextBox>
            </div>
        </div>    
        <div class='button-wrapper'>
            <button class="Button" (click)="close(null)">{{'General.B.Close' | TextCodeTranslationPipe}}</button>
            <button class="Button RedButton" (click)="close(data)">{{'General.B.Save' | TextCodeTranslationPipe}}</button>
        </div>
    `,
    styleUrls: ['./fields.scss'],
    styles: [`      
        .client-data-field div LogLabel {
            width: 170px !important;
        }

        .button-wrapper {
            direction: ltr;
        }

        .Button {
            display: inline-block;
            width: 50px;
            margin: 0 5px;
        }
    `],
})
export class AmitalAPIAddSchemaWindowComponent {
    fieldsReady: boolean = false;
    logWindow: LogitudeWindow;
    data: AmitalApiSchema & any = { UIProperties: new UIProperties() };
    fields: FieldData[] = [
        { name: 'Name', label: 'Name' },
        { name: 'SchemaJson', label: 'Schema Json' },
        { name: 'Active', label: 'Active' },
        { name: 'Endpoint', label: 'Endpoint' },
        { name: 'SchemaType', label: 'Schema Type' },
        { name: 'ChunkSize', label: 'Chunk Size' },
        { name: 'tenants', label: 'Tenants' },
    ];

    fieldsForCreate: FieldData[] = [
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

    close(data: any) {
        delete this.data.UIProperties;
        this.logWindow.Close(data);
    }

    static async openEditPopup(row: AmitalApiSchema, isUpdate: boolean): Promise<AmitalApiSchema | boolean> {
        let updateRow: AmitalApiSchema = await AmitalAPIAddSchemaWindowComponent.openWindow(row);
        if (!updateRow) return;

        updateRow.Tenants = updateRow['tenants'].split(',').map(t => t.trim());
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

        if (!res)
            new MessageWindow().Show(isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') || 'Update failed!' : TextCodeTranslator.Translate('General.O.AddFailed') || 'Add failed!');

        return res;
    }
}