import {MessagingStockPM} from '../../EntityPMs/MessagingStockPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

export class MessagingStockMenuButtonsHandler {
    public EntityPM: MessagingStockPM;
    public entityArgs: EntityArgs
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    switch (menuButton.EventCode) {
                        case "Cancel": {
                            if (this.EntityPM.IsCancelled) {
                                menuButton.IsDisabled = true;
                            }

                            else {
                                menuButton.IsDisabled = false;
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
                    case "Cancel": {
                        this.Cancel();
                        break;
                    }
                }
            }
        }
    }

    private Cancel() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show('Are you sure you want to cancel this Stock ?');
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {            
                this.EntityPM.IsCancelled = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        });
    }
}
