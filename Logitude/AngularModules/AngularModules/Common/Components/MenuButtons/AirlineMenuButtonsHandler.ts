import { AirlinePM } from '../../EntityPMs/AirlinePM'
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow'
import { MessageWindow } from '../../../Controls/Windows/MessageWindow'
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { CardExtendedPMService } from '../../Services/ExtendedPMs/CardExtendedPMService';

export class AirlineMenuButtonsHandler {
    public EntityPM:AirlinePM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    cardExtendedPMService: CardExtendedPMService = new CardExtendedPMService();


    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    switch (menuButton.EventCode) {
                        case "Disconnect": {

                            if (this.EntityPM.GLAccountId) {
                                menuButton.IsDisabled = false;
                            }
                            else {
                                menuButton.IsDisabled = true;
                            }




                            break;
                        }
                        case "More": {
                            if (!SessionLocator.TenantPM.AccountingActivated) {
                                menuButton.IsHidden = true;
                            }
                            else {
                                menuButton.IsHidden = false;
                            }
                            break;
                        }

                    }
                });
            }
        }

        return menuButtons;
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                switch (menuButton.EventCode) {
                    case "Disconnect": {
                        this.DisconnectGLAccount()
                        break;
                    }




                }
            }
        }
    }


    private DisconnectGLAccount() {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.cardExtendedPMService.DisconnectGLAccountFromCard(this.EntityPM.Id, "AL", "DIST").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });





    }








}
