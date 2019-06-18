declare var window: any;
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ReconciliationPMService} from '../../Services/StandardPMs/ReconciliationPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';


export class ReconciliationMenuButtonsHandler {
    public EntityPM: ReconciliationPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "Reconciliation"

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Reconciliation')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "CancelReco":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode)
        {
            case "CancelReco":
                {
                    var confirmWindow = new ConfirmWindow();
                    var msg = TextCodeTranslator.Translate("Accounting.General.O.WantToCancelCurrentReconciliation");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe((event: any) =>
                    {
                        if (confirmWindow.Yes)
                        {
                            this.EntityPM.IsCancelled = true;
                            this.entityArgs.EditComponent.SaveChanges();
                        }
                    });

                    break;
                }

        }  


        
    }

    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }
}