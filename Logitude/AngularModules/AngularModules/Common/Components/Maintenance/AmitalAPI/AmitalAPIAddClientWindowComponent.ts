import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { FieldData } from "./amitalApiTypes";
import { AmitalAPIClientWebService, AmitalApiClient } from "Common/Services/AmitalAPIClientWebService";

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
export class AmitalAPIAddClientWindowComponent {
    fieldsReady: boolean = false;
    logWindow: LogitudeWindow;
    data: AmitalApiClient & any = { UIProperties: new UIProperties() };
    fields: FieldData[] = [
        { name: 'AzureApiRegisterName', label: 'Azure Api Register Name' },
        { name: 'AzureClientId', label: 'Azure ClientId' },
        { name: 'Token', label: 'Token' },
        { name: 'AzureManagedApplObjId', label: 'Azure Managed Appl Objct Id' },
        { name: 'SecretExpired', label: 'Secret Expired' },
        { name: 'SecretValue', label: 'Secret Value' },
        { name: 'Active', label: 'Active' },
    ];

    SetWindowArgs({ row, logWindow }: { row: AmitalApiClient, logWindow: LogitudeWindow }) {
        if (row)
            this.data = { ...this.data, ...row };

        this.logWindow = logWindow;
        this.fieldsReady = true;
    }

    close(data: any) {
        console.log(data);
        delete this.data.UIProperties;
        this.logWindow.Close(data);
    }

    static async openEditPopup(row: AmitalApiClient, isUpdate: boolean): Promise<AmitalApiClient | boolean> {
        let updateRow: AmitalApiClient = await AmitalAPIAddClientWindowComponent.openWindow(row);
        if (!updateRow) return;

        let res: AmitalApiClient | boolean  = await AmitalAPIAddClientWindowComponent.sendToServer(isUpdate, updateRow);
        return res;
    }

    private static openWindow(row: AmitalApiClient): Promise<AmitalApiClient> {
        const logWindow = new LogitudeWindow();
        logWindow.Width = 975;
        logWindow.Height = 300;
        logWindow.Title = TextCodeTranslator.Translate('Accounting.General.O.Receivables');
        logWindow.WindowArgs = { row, logWindow };
        logWindow.Show('./Common/Components/Maintenance/AmitalAPI//AmitalAPIAddClientWindowComponent');
        return new Promise<AmitalApiClient>(resolve =>
            logWindow.WindowClosed.subscribe(async (row?: AmitalApiClient) => resolve(row)));
    }

    private static async sendToServer(isUpdate: boolean, updateRow: AmitalApiClient): Promise<AmitalApiClient | boolean>{
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const serviceApi = new AmitalAPIClientWebService();
        let res: AmitalApiClient | boolean = false;

        try {
            res = await (isUpdate ? serviceApi.update(updateRow) : serviceApi.add(updateRow));
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();
        
        if(!res) 
            new MessageWindow().Show(isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') || 'Update failed!' : TextCodeTranslator.Translate('General.O.AddFailed') || 'Add failed!');
        return res;
    }
}