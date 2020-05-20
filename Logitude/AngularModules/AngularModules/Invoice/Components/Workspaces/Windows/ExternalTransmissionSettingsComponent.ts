import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { AccountingSettingPM } from '../../../../Common/EntityPMs/AccountingSettingPM';
import { AccountingSettingPMService } from '../../../../Common/Services/StandardPMs/AccountingSettingPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    templateUrl: './ExternalTransmissionSettingsComponent.html',
})

export class ExternalTransmissionSettingsComponent extends BaseComponent {
    public EntityPM: AccountingSettingPM;
    public TransmissionCode: string;
    public DataContext = this;
    public ObjectTableName: string = "AccountingSetting";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();        
    }

    SetWindowArgs(args: any) {
        this.TransmissionCode = args['TransmissionCode'];
        this.EntityPM = args['EntityPM'];

        this.UIProperties.SetEnabled("TransferFTPDetailHost", this.ObjectTableName, false);
    }

    get TransferFTPDetailHost() { return this.EntityPM.TransferFTPDetailHost; }
    set TransferFTPDetailHost(value: string) {
        if (this.EntityPM.TransferFTPDetailHost != value) {
            this.EntityPM.TransferFTPDetailHost = value;
        }
    }

    get TransferFTPDetailId() { return this.EntityPM.TransferFTPDetailId; }
    set TransferFTPDetailId(value: string) {
        if (this.EntityPM.TransferFTPDetailId != value) {
            this.EntityPM.TransferFTPDetailId = value;
        }
    }

    ConnectDropBox() {
        var windowTitle = "Dropbox Connection";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 225;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
    }

    AddFTPClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { IsNew: true };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.TransferFTPDetailId = comp.EntityPM.Id;
                    this.TransferFTPDetailHost = comp.EntityPM.Host;
                }
            });
        });
    }
    EditFTPClicked() {
        if (!AppTool.IsNullOrEmpty(this.TransferFTPDetailId)) {

            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { IsNew: false, EntityId: this.TransferFTPDetailId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.TransferFTPDetailHost = comp.EntityPM.Host;
                    }
                });
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }
}
