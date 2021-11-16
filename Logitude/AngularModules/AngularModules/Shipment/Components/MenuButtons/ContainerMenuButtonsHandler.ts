declare var window: any;
import { OnDestroy } from '@angular/core';
import { ContainerPM } from '../../EntityPMs/ContainerPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Validator } from '../../../Infrastructure/Validators/Validator';

export class ContainerMenuButtonsHandler implements OnDestroy {
    public EntityPM: ContainerPM;
    public entityArgs: EntityArgs
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    IsClosed: boolean = false;
    IsReopen: boolean = false;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }

                    if (this.IsClosed) {
                        this.CloseRepoenContainer(true);
                    }
                    if (this.IsReopen) {
                        this.CloseRepoenContainer(false);
                    }

                    this.StopFlags();
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }

                    this.StopFlags();
                });
            }
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Container')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "CloseContainer") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsClosed) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "ReopenContainer") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsClosed) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                }
                return menuButtons;
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            this.Validate();

            if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "CloseContainer": {
                        this.IsClosed = true;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                    case "ReopenContainer": {
                        this.IsReopen = true;
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
    private Validate() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Container", errors);
        if (errors.length == 0) {
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
            this.isValid = true;
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
            this.isValid = false;
        }
    }
    private CloseRepoenContainer(closed) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        var title = "";
        if (closed) {
            title = "Are you sure you want to close this container ?";
        }
        else {
            title = "Are you sure you want to Re-open this container ?";

        }
        confirmWindow.Show(title);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.entityArgs.EditComponent.ValidationErrorsList != null && this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
                    if (closed) {
                        this.EntityPM.IsClosed = true;
                    }
                    else {
                        this.EntityPM.IsClosed = false;
                    }
                    this.entityArgs.EditComponent.SaveChanges();
                    this.ResetButtonClicked();
                }
            }
        });
    }

    StopFlags() {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.IsClosed = false;
        this.IsReopen = false;
    }

    private ResetButtonClicked() {
        this.isButtonClicked = false;
    }
}
