import { Component } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ShaamSettings, ShaamWebService } from 'Shipment/Services/ShaamWebService';

export type ShaamSettingsArgs = {
    windowInstance: LogitudeWindow;
};

@Component({
    selector: 'app-shaam-settings',
    template: `           
    <table style='width:450px; height: 200px'>
        <tr style='height: 50px'>
            <td style='width:120px'>
                <LogLabel [Text]="'Api Key'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogTextBox [DataContext]="DataContext" [Text]="key" [ObjectFieldName]='"key"'></LogTextBox>
            </td>
        </tr>
        <tr style='height: 50px'>
            <td style='width:120px'>
                <LogLabel [Text]="'Api Sercet'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogTextBox [DataContext]="DataContext" [Text]="secret" [ObjectFieldName]='"secret"'></LogTextBox>
            </td>
        </tr>
        <tr style='height: 50px'>
            <td style='width:120px'>
                <LogLabel [Text]="'Is Test Environment'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogCheckBox [DataContext]="DataContext" [Checked]="DataContext['isTestEnvironment']" [ObjectFieldName]="'isTestEnvironment'"></LogCheckBox>
            </td>
        </tr>
        <tr style='height: 50px'>
            <td style='width:120px'>
                <LogLabel [Text]="'Approval API Version'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
            <select [(ngModel)]="approvalInvoiceVersion" class='approval-invoice-version'>
                <option *ngFor="let item of [1,2]" >{{item}}</option>
            </select>
            </td>
        </tr>
        <tr>
        </tr>
    </table>     
    
    <div style='float: left; display:flex;'>
        <button style='width: 60px;' class="RedButton" (click)="OkButtonClicked()">{{'General.B.Save' | TextCodeTranslationPipe}}</button>    
        <button style='width: 60px; margin-right: 10px;' class="Button" (click)="CancelButtonClicked()">{{'Customs.General.B.Cancel' |TextCodeTranslationPipe}}</button>
    <div>
    `,
    styles: [`
        .approval-invoice-version {
            width: 50px;
        }
    `]
})
export class ShaamSettingsComponent extends BaseComponent {
    DataContext: ShaamSettingsComponent = this;
    key: string = '';
    secret: string = '';
    isTestEnvironment: boolean = false;    
    windowInstance: LogitudeWindow = null;
    shaamWebService = new ShaamWebService();
    approvalInvoiceVersion: number = 1;

    ngOnInit() {
        this.initShaamSettings();
    }

    SetWindowArgs(args: ShaamSettingsArgs) {
        this.windowInstance = args.windowInstance;
    }

    async initShaamSettings() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        let shaamSettings: ShaamSettings;

        try {
            shaamSettings = await this.shaamWebService.getShaamSettings();
            SessionLocator.SelectedSession.StopBusyIndicator();
        } catch (error) {
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.showErrorMessage();
        }

        this.key = shaamSettings.clientId;
        this.secret = shaamSettings.secret;
        this.isTestEnvironment = shaamSettings.isTestEnvironment;
        this.approvalInvoiceVersion = shaamSettings.approvalInvoiceVersion;
    }

    async OkButtonClicked() {
        await this.updateShaamSettings();
        this.closeWindow();
    }

    CancelButtonClicked() {
        this.closeWindow()
    }

    async updateShaamSettings() {
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate('General.B.Save'));
        try {
            await this.shaamWebService.postShaamSettings(this.key, this.secret, this.isTestEnvironment, this.approvalInvoiceVersion);
            SessionLocator.SelectedSession.StopBusyIndicator();
        } catch (error) {
            SessionLocator.SelectedSession.StopBusyIndicator();
            console.log(error)
            await this.showErrorMessage();
        }
    }

    closeWindow() {
        this.windowInstance.Close('');
    }

    showErrorMessage(): Promise<void> {
        const msgWin: MessageWindow = new MessageWindow();
        msgWin.ShowErrorIcon = true;
        msgWin.Show(TextCodeTranslator.Translate('Customs.General.O.Fail'));

        return new Promise<void>(res => msgWin.WindowClosed.subscribe(() => res()));
    }
}