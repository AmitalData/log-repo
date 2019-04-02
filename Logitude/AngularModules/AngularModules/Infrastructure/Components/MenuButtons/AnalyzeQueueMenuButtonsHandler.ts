import { AnalyzeQueuePM } from '../../EntityPMs/AnalyzeQueuePM';
import { MenuButtonPM } from '../../EntityPMs/MenuButtonPM'
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { InfrastructureDomainService } from '../../Services/InfrastructureDomainService'

export class AnalyzeQueueMenuButtonsHandler {
    public EntityPM: AnalyzeQueuePM;
    public entityArgs: EntityArgs;
    public ObjectTableName: string = "AnalyzeQueue"
    private iService: InfrastructureDomainService;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.iService = new InfrastructureDomainService();
        //this.Listen();
    }

    private CurrentSession = SessionLocator.SelectedSession;
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

        this.CurrentSession.StartBusyIndicator("Resending...");

        this.iService.ResendAnalyzeQueue(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {

            this.StopFlags();

            this.CurrentSession.StopBusyIndicator();
        });
    }
}
