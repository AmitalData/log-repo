import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowVersionPM } from 'Workflow/EntityPMs/WorkFlowVersionPM';
import { WorkFlowVersionPMService } from 'Workflow/Services/StandardPMs/WorkFlowVersionPMService';
import { WorkflowVersionExtendedService } from 'Workflow/Services/Extended/WorkflowVersionExtendedService';

export class WorkFlowMenuButtonsHandler {
    public EntityPM: WorkFlowPM;
    public entityArgs: EntityArgs
    public MenuButtons: MenuButtonPM[]
    private CurrentSession = SessionLocator.SelectedSession;
    public WorkFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();
    public WorkFlowVersionExtendedService: WorkflowVersionExtendedService = new WorkflowVersionExtendedService();

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.Listen();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        this.MenuButtons = menuButtons;
        this.MenuButtons.forEach(btn => {
            btn.IsDisabled = true;
        });

        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                this.SetButtonStates(menuButtons)
            }
        }
    }

    private Listen() {
        this.entityArgs.EntityArgEventEmitter.subscribe(
            theMessage => {
                if (theMessage == "RefreshWorkflowButtons") {
                    this.EntityPM = this.entityArgs.EntityPM;
                    this.SetButtonStates(this.MenuButtons)
                }
            }
        );
    }

    public SetButtonStates(menuButtons) {
        var HasChanges: boolean = this.entityArgs.EditComponentArgument?.HasChanges ? true : false
        var IsActiveDisabled: boolean = this.entityArgs.EditComponentArgument?.IsActiveDisabled ? true : false
        var IsNewVersionDisabled: boolean = this.entityArgs.EditComponentArgument?.IsNewVersionDisabled ? true : false
        var CurrentDisplayedVersionId = this.entityArgs.EditComponentArgument?.CurrentDisplayedVersionId
        var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == CurrentDisplayedVersionId);

        for (var i = 0; i < menuButtons.length; i++) {
            var button = menuButtons[i];
            button.IsDisabled = true;
            switch (button.EventCode) {
                case "NewVersion":
                    {
                        if (!HasChanges) {
                            button.IsDisabled = IsNewVersionDisabled;
                        } else {
                            button.IsDisabled = true;
                        }
                        break;
                    }
                case "Activate":
                    {
                        if (!HasChanges && !IsActiveDisabled) {
                            if (this.isActiveVersion(version.StatusCode)) {
                                button.IsDisabled = false;
                                button.DisplayText = "Deactivate";
                            } else if (this.isDraftVersion(version.StatusCode) || this.isInactiveVersion(version.StatusCode)) {
                                button.IsDisabled = false;
                                button.DisplayText = "Activate";
                            }
                        } else {
                            button.IsDisabled = true;
                        }
                        break;
                    }
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "NewVersion":
                {
                    this.CreateNewVersion();
                    break;
                }
            case "Activate":
                {
                    this.ActivateDeactiveVersion();
                    break;
                }
        }
    }

    CreateNewVersion() {
        var CurrentDisplayedVersionId = this.entityArgs.EditComponentArgument?.CurrentDisplayedVersionId
        var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == CurrentDisplayedVersionId);

        let propertiesComponentPath = "./Workflow/Components/WorkflowBuilder/CreateWorkflowVersionComponent";
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            WorkflowId: this.EntityPM.Id,
            FlowJson: version.FlowJson,
            Entity: version.Entity,
            Trigger: version.Trigger
        };
        propertiesWindow.Height = 340;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = "Save New Verison";
        propertiesWindow.WindowArgs = propertiesWindowArgs;

        propertiesWindow.Show(propertiesComponentPath);
        propertiesWindow.WindowClosed.subscribe((data: any) => { this.handleCreateNewVersionResponse(data); });
    }

    handleCreateNewVersionResponse(data: WorkFlowVersionPM) {
        if (data) {
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, UpdatedVersion: data.Id }
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, ClickedVersionRow: null }
            this.entityArgs.SendMessage("WorkflowVersionsUpdated");
        }
    }

    ActivateDeactiveVersion() {
        var CurrentDisplayedVersionId = this.entityArgs.EditComponentArgument?.CurrentDisplayedVersionId
        var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == CurrentDisplayedVersionId);

        this.StartBusyIndicator("Saving ...");

        let isActivate: boolean = version.StatusCode == "INVE" || version.StatusCode == "DRFT"
        if (isActivate) {
            this.WorkFlowVersionExtendedService.putActivate(version.Id).subscribe((serviceResponse: ServiceResponse) => {
                if (serviceResponse != null && !serviceResponse.HasError) {
                    this.handleActivateDeactiveWorkflowVersionResponse(serviceResponse.Result);
                    this.entityArgs.SendMessage({ Code: "SetWorkflowErrorMessages", Messages: null });
                    this.StopBusyIndicator();
                } else {
                    this.entityArgs.SendMessage({ Code: "SetWorkflowErrorMessages", Messages: serviceResponse.ErrorsArray });
                    this.StopBusyIndicator();
                }
            });
        }
        else {
            version.StatusCode = "INVE";
            this.WorkFlowVersionPMService.update(version).subscribe((serviceResponse: ServiceResponse) => {
                if (serviceResponse != null && !serviceResponse.HasError) {
                    this.handleActivateDeactiveWorkflowVersionResponse(serviceResponse.Result)
                    this.StopBusyIndicator();
                } else {
                    this.StopBusyIndicator();
                }
            });
        }
    }

    handleActivateDeactiveWorkflowVersionResponse(data: WorkFlowVersionPM) {
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, UpdatedVersion: data.Id }
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, ClickedVersionRow: null }
        this.EntityPM.StatusCode = data.StatusCode;
        this.EntityPM.Entity = data.Entity;
        this.EntityPM.Trigger = data.Trigger;
        this.EntityPM.FlowJson = data.FlowJson;
        this.EntityPM.IsDirty = false;
        this.entityArgs.SendMessage("WorkflowVersionsUpdated");
    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }

    //flags
    isDraftVersion(varsionCode: string) {
        return varsionCode == "DRFT"
    }

    isActiveVersion(varsionCode: string) {
        return varsionCode == "ACVE"
    }

    isInactiveVersion(varsionCode: string) {
        return varsionCode == "INVE"
    }
}
