import { TruckerPM } from '../../EntityPMs/TruckerPM'
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow'
import { MessageWindow } from '../../../Controls/Windows/MessageWindow'
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';

export class TruckerMenuButtonsHandler {
    public EntityPM: TruckerPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
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

                            menuButton.IsDisabled = false;

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

                        break;
                    }




                }
            }
        }
    }









}
