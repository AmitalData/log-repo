import {ReportExecutionLogPM} from '../../EntityPMs/ReportExecutionLogPM'
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow'
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {PasswordChangeService} from '../../Services/Others/PasswordChangeService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { ReportExecutionLogPMService } from 'Common/Services/StandardPMs/ReportExecutionLogPMService';

export class ReportExecutionLogMenuButtonsHandler {
    public EntityPM: ReportExecutionLogPM;
    public entityArgs: EntityArgs
    ReportExecutionLogPMService: ReportExecutionLogPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.ReportExecutionLogPMService = new ReportExecutionLogPMService();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    switch (menuButton.EventCode) {
                        case "Cancel": {
                            if (this.EntityPM.StatusCode != "P" && this.EntityPM.StatusCode != "W" ) {
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
        this.ReportExecutionLogPMService.Cancel(this.EntityPM.Id).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
        });
    }
}
