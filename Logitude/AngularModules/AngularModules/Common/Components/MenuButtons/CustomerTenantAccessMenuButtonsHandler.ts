declare var window;
import {CustomerTenantAccessPM} from '../../EntityPMs/CustomerTenantAccessPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CustomerTenantAccessPMService} from '../../Services/StandardPMs/CustomerTenantAccessPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CustomerActivationArgs} from '../../../Common/Args';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {CustomerTenantAccessExtendedPMService} from '../../Services/ExtendedPMs/CustomerTenantAccessExtendedPMService';

export class CustomerTenantAccessMenuButtonsHandler {
    public EntityPM: CustomerTenantAccessPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "CustomerTenantAccess";
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {;

                var table = window.ObjectTables.filter(d => d.Name === 'CustomerTenantAccess')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Deny":
                            {
                                button.IsDisabled = this.EntityPM.Status.toUpperCase() != "W";
                                break;
                            }           
                    }
                }
            }
        }

        return menuButtons;
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode) {
            case "Deny":
                {
                    this.Deny();
                    break;
                }
        }
    }

    Deny() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Deny Request";
        confirmWindow.Yes = true;
        confirmWindow.No = true;
        confirmWindow.Cancel = true;
        confirmWindow.Show("Are you sure you want to Deny this Request ?");
        confirmWindow.WindowClosed.subscribe(event => {
            if (confirmWindow.Yes) {
                var service: CustomerTenantAccessExtendedPMService = new CustomerTenantAccessExtendedPMService();
                service.DenyRequest(this.EntityPM.Id).subscribe(p => {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                });;
            }
        });
    }
    
    
}
