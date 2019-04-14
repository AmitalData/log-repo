import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ARInvoiceStockPM } from '../../EntityPMs/ARInvoiceStockPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

declare var window: any;

export class ARInvoiceStockMenuButtonsHandler {
    public EntityPM: ARInvoiceStockPM;
    public entityArgs: EntityArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        
        this.Listen();
    }

    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.isCancelled) {
                        this.isCancelled = false;
                        this.SetIsCancelled();
                    }

                    if (this.isReActivated) {
                        this.isReActivated = false;
                        this.SetReActivated();
                    }

                    if (this.actionCompleted) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        //this.CurrentSession.FireEvent("RefreshARInvoiceStockScreen");
                    }
                }
            });
        }
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'ARInvoiceStock')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Cancel":
                            {
                                if (this.EntityPM.StatusCode == "C") {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "ReActivate":
                            {
                                if (this.EntityPM.StatusCode == "C") {
                                    button.IsDisabled = false;
                                }

                                else {
                                    button.IsDisabled = true;
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
            case "Cancel": {
                this.ResetAllFlags();
                this.isCancelled = true;
                this.entityArgs.EditComponent.SaveChanges();
                break;
            }

            case "ReActivate":
                {
                    this.ResetAllFlags();
                    this.isReActivated = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
        }
    }

    private isCancelled: boolean;
    private isReActivated: boolean;
    private actionCompleted: boolean;
    private ResetAllFlags() {
        this.isCancelled = false;
        this.isReActivated = false;
        this.actionCompleted = false;
    }


    SetIsCancelled() {
        this.EntityPM.StatusCode = "C";
        this.EntityPM.Inactive = true; 
        this.EntityPM.Cancelled = true;

        this.actionCompleted = true;
        this.entityArgs.EditComponent.SaveChanges();
    }

    SetReActivated() {
        this.EntityPM.StatusCode = "A";
        this.EntityPM.Inactive = false; 
        this.EntityPM.Reactivated = true;

        this.actionCompleted = true;
        this.entityArgs.EditComponent.SaveChanges();
    }
}
