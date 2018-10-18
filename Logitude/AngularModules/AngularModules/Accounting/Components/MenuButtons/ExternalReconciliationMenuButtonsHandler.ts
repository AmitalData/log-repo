declare var window: any;
import {ExternalReconciliationPM} from '../../EntityPMs/ExternalReconciliationPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ExternalReconciliationPMService} from '../../Services/StandardPMs/ExternalReconciliationPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';


export class ExternalReconciliationMenuButtonsHandler {
    public EntityPM: ExternalReconciliationPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "ExternalReconciliation"

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'ExternalReconciliation')[0];


                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "CancelExtReco":
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
            case "CancelExtReco":
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
                            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                if (isSaveSuccess) {
                                    this.entityArgs.EditComponent.ReloadEntityPM();
                                }
                            });
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