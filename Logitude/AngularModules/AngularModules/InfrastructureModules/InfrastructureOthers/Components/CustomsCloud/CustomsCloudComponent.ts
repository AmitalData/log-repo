import { Component } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TaxesWebService } from "Customs/Services/WebServices/TaxesWebService";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

@Component({
    selector: 'app-customs-cloud-component',
    template: `
    <table style='width:450px; height: 200px'>
        <tr style='height: 50px'>
            <td style='width:140px'>
                <LogLabel [Text]="'Export Tenant'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogTextBox [DataContext]="DataContext" [Text]="exportTenant" [ObjectFieldName]='"exportTenant"'></LogTextBox>
            </td>
        </tr>
        <tr style='height: 50px'>
            <td style='width:80px'>
                <LogLabel [Text]="'Export Login Credential'" [DataContext]="DataContext"></LogLabel>
            </td>
            <td>
                <LogTextBox [DataContext]="DataContext" [Text]="exportLoginCredential" [ObjectFieldName]='"exportLoginCredential"'></LogTextBox>
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
    styles: [
        '',
    ]
})
export class CustomsCloudComponent extends BaseComponent {
    DataContext: CustomsCloudComponent = this;
    exportTenant: string = '';
    exportLoginCredential: string = '';
    windowInstance: LogitudeWindow = null;
    taxesWebService = new TaxesWebService();

    ngOnInit() {
        this.initCloudSettings();    
    }

    SetWindowArgs(args: CustomsCloudComponentArgs) {
        this.windowInstance = args.windowInstance;
    }
    
    async initCloudSettings() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        let cloudSettings: any;

        try {            
            cloudSettings = await this.taxesWebService.getCloudSettings();
            SessionLocator.SelectedSession.StopBusyIndicator();
        } catch (error) {
            SessionLocator.SelectedSession.StopBusyIndicator();  
            this.showErrorMessage();          
        }

        this.exportTenant = cloudSettings.exportTenant; 
        this.exportLoginCredential = cloudSettings.exportLoginCredential;        
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
            await this.taxesWebService.putCloudSettings(this.exportTenant, this.exportLoginCredential);
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

export type CustomsCloudComponentArgs =  {
    windowInstance: LogitudeWindow    
}