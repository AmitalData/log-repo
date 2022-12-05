declare var window: any;
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';

export class WorkFlowMenuButtonsHandler {
    public EntityPM: WorkFlowPM;
    public entityArgs: EntityArgs

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "SaveAs":
                            {
                                if (this.EntityPM) {
                                    button.IsDisabled = false;
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
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "SaveAs":
                {
                    this.saveAsWorkflow()
                    break;
                }
            case "Activate":
                {
                    console.log("Activate")
                    break;
                }
        }
    }

    saveAsWorkflow() {
        let propertiesComponentPath = "./Workflow/Components/WorkflowBuilder/CreateWorkflowVersionComponent";
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            WorkflowId: this.EntityPM.Id,
            FlowJson: this.EntityPM.FlowJson
        };
        propertiesWindow.Height = 340;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = "Save New Verison";
        propertiesWindow.WindowArgs = propertiesWindowArgs;

        propertiesWindow.Show(propertiesComponentPath);
        propertiesWindow.WindowClosed.subscribe((data: any) => { this.handleVersionPropertiesWindowClosed(data); });
    }
    handleVersionPropertiesWindowClosed(data) {
        console.log(data)
    }
}