import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { AmitalAPIClientWebService, AmitalApiClient } from "Common/Services/AmitalAPIClientWebService";
import { TextBoxField } from "./components/LogTexBoxFormComponent";

@Component({
    template: `
        <log-text-box-form *ngIf='fieldsReady' [DataContext]='data' [fields]='fields' [dir]="'ltr'"></log-text-box-form>       
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>        
    `,
    styles: [`      
        :host ::ng-deep .form-data-field div LogLabel {
            width: 170px !important;
        }
    `],
})
export class AmitalAPIAddClientWindowComponent {
    fieldsReady: boolean = false;
    logWindow: LogitudeWindow;
    data: AmitalApiClient & any = { UIProperties: new UIProperties() };
    fields: TextBoxField[] = [
        { name: 'Name', label: 'Name' },
        { name: 'Tenant', label: 'Tenant' },
        { name: 'AzureApiRegisterName', label: 'Azure Api Register Name' },
        { name: 'AzureClientId', label: 'Azure ClientId' },
        { name: 'AzureManagedApplObjId', label: 'Azure Managed Appl Objct Id' },
        { name: 'SecretExpired', label: 'Secret Expired' },
        { name: 'SecretValue', label: 'Secret Value' },
        { name: 'Active', label: 'Active', type: 'boolean' },
    ];

    SetWindowArgs({ row, logWindow }: { row: AmitalApiClient, logWindow: LogitudeWindow }) {
        if (row)
            this.data = { ...this.data, ...row };

        this.logWindow = logWindow;
        this.fieldsReady = true;
    }

    close(save: boolean) {
        delete this.data.UIProperties;
        this.logWindow.Close(save ? this.data : null);
    }

    static async openEditPopup(row: AmitalApiClient, isUpdate: boolean): Promise<AmitalApiClient | boolean> {
        let updateRow: AmitalApiClient = await AmitalAPIAddClientWindowComponent.openWindow(row);
        if (!updateRow) return;

        let res: AmitalApiClient | boolean = await AmitalAPIAddClientWindowComponent.sendToServer(isUpdate, updateRow);
        return res;
    }

    private static openWindow(row: AmitalApiClient): Promise<AmitalApiClient> {
        const logWindow = new LogitudeWindow();
        logWindow.Width = 975;
        logWindow.Height = 310;
        logWindow.Title = TextCodeTranslator.Translate('Accounting.General.O.Receivables');
        logWindow.WindowArgs = { row, logWindow };
        logWindow.Show('./Common/Components/Maintenance/AmitalAPI//AmitalAPIAddClientWindowComponent');
        return new Promise<AmitalApiClient>(resolve =>
            logWindow.WindowClosed.subscribe(async (row?: AmitalApiClient) => resolve(row)));
    }

    private static async sendToServer(isUpdate: boolean, updateRow: AmitalApiClient): Promise<AmitalApiClient | boolean> {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const serviceApi = new AmitalAPIClientWebService();
        let res: AmitalApiClient | boolean = false;

        try {
            res = await (isUpdate ? serviceApi.update(updateRow) : serviceApi.add(updateRow));
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();

        if (!res) {
            const win = new MessageWindow();
            win.ShowErrorIcon = true;
            win.Show(isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') || 'Update failed!' : TextCodeTranslator.Translate('General.O.AddFailed') || 'Add failed!');
        }

        return res;
    }
}