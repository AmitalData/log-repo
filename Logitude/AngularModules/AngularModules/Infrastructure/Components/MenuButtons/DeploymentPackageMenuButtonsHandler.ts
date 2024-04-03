import {MenuButtonPM} from '../../EntityPMs/MenuButtonPM'
import {EntityArgs} from '../../DataContracts/EntityArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import { DeploymentPackagePM } from '../../EntityPMs/DeploymentPackagePM';
declare var window: any;

export class DeploymentPackageMenuButtonsHandler {
    public EntityPM: DeploymentPackagePM;
    public entityArgs: EntityArgs;
    public ObjectTableName: string = "DeploymentPackage"
    public OpenExportAfterSaving: boolean;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private Listen() {
        if (this.entityArgs.EditComponent == null) return;
        this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (!isSaveSuccess) return;
            this.EntityPM = this.entityArgs.EditComponent.EntityPM;

            if (this.OpenExportAfterSaving) {
                this.ExportMenuButtonClicked();
            }
        });
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM == null) return;
        if (this.entityArgs.EditComponent == null) return;
        for (var i = 0; i < menuButtons.length; i++) {
            this.HandleMenuButtonsUIProperties(menuButtons, i);
        }

        return menuButtons;
    }

    private HandleMenuButtonsUIProperties(menuButtons: MenuButtonPM[], index: number) {
        let menuButton = menuButtons[index];
        switch (menuButton.EventCode) {
            case "Export":
                {
                    menuButton.IsDisabled = this.DisableMenuButton(menuButton.FeatureUniqeCode);
                    break;
                }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "Export":
                {
                    this.ExportMenuButtonClicked();
                    break;
                }
        }
    }

    DisableMenuButton(featureUniqeCode: string) {
        if (featureUniqeCode != "DeploymentPackage.Export") return false;
        if (this.EntityPM.DirectionId == 'E') return false;
        return true;
    }
    private ExportMenuButtonClicked() {
        if (this.EntityPM == null) return;
        this.OpenExportAfterSaving = this.EntityPM.IsDirty;
        if (this.EntityPM.IsDirty) {
            this.entityArgs.EditComponent.SaveChanges();
            return;
        }
        let logWindow = new LogitudeWindow();
        let windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Export Package";
        logWindow.Show('./InfrastructureModules/InfrastructureDeploymentPackage/Component/MenuButtons/ExportMenuButtonComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            if (!event) return;
        });
    }
}
