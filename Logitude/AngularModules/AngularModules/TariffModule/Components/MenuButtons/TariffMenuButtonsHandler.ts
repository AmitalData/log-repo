
import { TariffPM } from '../../EntityPMs/TariffPM'
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow'
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

export class TariffMenuButtonsHandler {
    public EntityPM: TariffPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    menuButton.IsDisabled = false;
                    switch (menuButton.EventCode) {
                        case "Inactive": {
                            if (this.EntityPM.InActive)
                                menuButton.IsDisabled = true;
                            break;
                        }

                        case "Reactivate": {
                            if (!this.EntityPM.InActive) {
                                menuButton.IsDisabled = true;
                            }

                            break;
                        }
                    }
                });
            }
        }

        return menuButtons;
    }


    private StopFlags() {
        this.isButtonClicked = false;
    }


    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;                   
                }

                this.StopFlags();

            });
        }
    }


    isButtonClicked: boolean = false;
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.isButtonClicked = true;
            switch (menuButton.EventCode) {
                case "Inactive":
                    {
                        this.EntityPM.SetAsInActive = true;
                        this.EntityPM.InActive = true;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }

                case "Reactivate":
                    {
                        this.EntityPM.SetAsReActive = true;
                        this.EntityPM.InActive = false;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }
        }
    }

    



}
