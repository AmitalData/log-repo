import { BatchTaskExecutionPM } from '../../EntityPMs/BatchTaskExecutionPM';
import { MenuButtonPM } from '../../EntityPMs/MenuButtonPM'
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { BatchTaskExecutionExtendedPMService } from 'Infrastructure/Services/ExtendedPMs/BatchTaskExecutionExtendedPMService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
declare var window: any;

export class BatchTaskExecutionMenuButtonsHandler {
    BatchTaskExecutionExtendedPMService: BatchTaskExecutionExtendedPMService;
    public EntityPM: BatchTaskExecutionPM;
    public entityArgs: EntityArgs;
    private CurrentSession = SessionLocator.SelectedSession;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.BatchTaskExecutionExtendedPMService = new BatchTaskExecutionExtendedPMService();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    switch (menuButton.EventCode) {
                        case "Cancel": {
                            if (this.EntityPM.StatusCode != "C" && this.EntityPM.StatusCode != "I") {
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

    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                switch (menuButton.EventCode) {
                    case "Cancel": {
                        this.CancelButtonClcik();
                        break;
                    }
                }
            }
        }
    }

    CancelButtonClcik() {
        this.CurrentSession.StartBusyIndicator("Canceling...");
        this.BatchTaskExecutionExtendedPMService.Cancel(this.EntityPM.Id).subscribe((res: any) => {
            if (!res.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = res.ErrorsArray;
            }
        });
    }
}
