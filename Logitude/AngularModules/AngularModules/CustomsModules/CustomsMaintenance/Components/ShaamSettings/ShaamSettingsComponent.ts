import { Component } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ShaamWebService } from 'Shipment/Services/ShaamWebService';

export type ShaamSettingsArgs = {
    windowInstance: LogitudeWindow;
};

@Component({
    selector: 'app-shaam-settings',
    template: `           
    <table style='width:450px; height: 200px'>
        <tr style='height: 50px'>
            <td style='width:80px'>
                <LogLabel [Text]="'API Key'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogTextBox [DataContext]="DataContext" [Text]="key" [ObjectFieldName]='"key"'></LogTextBox>
            </td>
        </tr>
        <tr style='height: 50px'>
            <td style='width:80px'>
                <LogLabel [Text]="'API Secret'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogTextBox [DataContext]="DataContext" [Text]="secret" [ObjectFieldName]='"secret"'></LogTextBox>
            </td>
        </tr>
        <tr>
        </tr>
    </table>     
    
    <div style='float: left; display:flex;'>
        <button style='width: 60px;' class="RedButton" (click)="OkButtonClicked()">{{'General.B.Save' | TextCodeTranslationPipe}}</button>    
        <button style='width: 60px; margin-right: 10px;' class="Button" (click)="CancelButtonClicked()">{{'Customs.General.B.Cancel' |TextCodeTranslationPipe}}</button>
    <div>
    `
})
export class ShaamSettingsComponent extends BaseComponent {
    DataContext: ShaamSettingsComponent = this;
    key: string = '';
    secret: string = '';
    windowInstance: LogitudeWindow = null;
    shaamWebService = new ShaamWebService();

    ngOnInit() {
        this.initShaamSettings();    
    }

    SetWindowArgs(args: ShaamSettingsArgs) {
        this.windowInstance = args.windowInstance;
    }
    
    async initShaamSettings() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        let shaamSettings;

        try {            
            shaamSettings = await this.shaamWebService.getShaamSettings();
            SessionLocator.SelectedSession.StopBusyIndicator();
        } catch (error) {
            SessionLocator.SelectedSession.StopBusyIndicator();  
            this.showErrorMessage();          
        }

        this.key = shaamSettings.amitalTaxesUrl; 
        this.secret = shaamSettings.secret;        
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
            await this.shaamWebService.postShaamSettings(this.key, this.secret);
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