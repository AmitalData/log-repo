import {TenantManagementPM} from '../../EntityPMs/TenantManagementPM';
import {MenuButtonPM} from '../../EntityPMs/MenuButtonPM'
import {EntityArgs} from '../../DataContracts/EntityArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
declare var window: any;

export class TenantManagementMenuButtonsHandler {
    public EntityPM: TenantManagementPM;
    public entityArgs: EntityArgs;
    public ObjectTableName: string = "TenantManagement"
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

                    if (this.isEraseData) {
                        this.isEraseData = false;
                        this.OpenEraseDataComponent();
                    }
                }
            });
        }
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Customer')[0];
                var buttonEnabled: boolean = true;

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "EraseData":
                            {
                                if (this.EntityPM.IsTrial) {
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

        return menuButtons;
    }

    private isEraseData: boolean = false;
    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "EraseData":
                {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Height = 200;
                    confirmWindow.Width = 350;
                    confirmWindow.ShowCheckBox = true;
                    confirmWindow.IsYesEnabled = false;
                    confirmWindow.Show("Please notice that records can't be restored after deleting, the deletion will result in losing this data forever");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes && confirmWindow.IsChecked) { 
                            this.isEraseData = true;
                            this.entityArgs.EditComponent.SaveChanges();
                        }
                    });
                                        
                    break;
                }
        }
    }

    private OpenEraseDataComponent() {
        if (this.EntityPM != null) {
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = this.EntityPM.Id;
            logWindow.Title = "Erase Data";
            logWindow.Show('./Infrastructure/Components/MenuButtons/EraseTenantManagementDataComponent');
        }
    }
}