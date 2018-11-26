import { AnalyzeQueuePM } from '../../EntityPMs/AnalyzeQueuePM';
import { MenuButtonPM } from '../../EntityPMs/MenuButtonPM'
import { EntityArgs } from '../../DataContracts/EntityArgs';

export class AnalyzeQueueMenuButtonsHandler {
    public EntityPM: AnalyzeQueuePM;
    public entityArgs: EntityArgs;
    public ObjectTableName: string = "AnalyzeQueue"
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        //this.Listen();
    }

    isButtonClicked: boolean = false;
    StopFlags() {
        this.isButtonClicked = false;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM) {
            if (this.entityArgs.EditComponent) {

                menuButtons.forEach((button: MenuButtonPM) => {

                    switch (button.EventCode) {
                        case "Resend":
                            {                                
                                break;
                            }
                    }
                });
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (this.EntityPM) {
            if (this.entityArgs.EditComponent) {
                if (menuButton) {
                    if (!this.isButtonClicked) {
                        this.StopFlags();
                        this.isButtonClicked = true;

                        switch (menuButton.EventCode) {
                            case "Resend":
                                {
                                    this.ResendButtonClicked();
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }

    ResendButtonClicked() {

    }
}
