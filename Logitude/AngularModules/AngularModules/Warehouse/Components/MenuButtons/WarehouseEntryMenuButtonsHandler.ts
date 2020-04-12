import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { WarehouseEntryPM } from '../../EntityPMs/WarehouseEntryPM';
import { WarehouseEntryPMExtendedService } from '../../Services/ExtendedPMs/WarehouseEntryPMExtendedService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { WarehouseReleasePMExtendedService } from '../../Services/ExtendedPMs/WarehouseReleasePMExtendedService';

export class WarehouseEntryMenuButtonsHandler {
    public EntityPM: WarehouseEntryPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "CancelEntry":
                            {
                                if (this.EntityPM.StatusCode == "CAEA") {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                    }
                }
            }
        }
    }

    isButtonClicked: boolean = false;
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {
            this.isButtonClicked = true;

            switch (menuButton.EventCode) {
                case "CancelEntry":
                    {
                        this.IsThereConnectedWarehouses();
                        break;
                    }
                default:
                    {

                    }
            }
        }
    }

    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                this.isButtonClicked = false;
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }

            });


        }
    }
    IsNoConnectedReleaseEntity: boolean;
    IsThereConnectedWarehouses() {
        var warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
        warehouseReleasePMExtendedService.GetNumberofConnectedWarehouseReleasesByEntryId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {

                    if (myResponse.Result && myResponse.Result > 0) {
                        this.IsNoConnectedReleaseEntity = false;
                    }
                    else {
                        this.IsNoConnectedReleaseEntity = true;
                    }
                    this.CancelEntry();
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
    
    CancelEntry() {
        this.isButtonClicked = false;

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Title = "Cancel Entry";
        if (this.IsNoConnectedReleaseEntity) {
            confirmWindow.Show("Confirm cancelling this entry");
        }
        else {
            confirmWindow.Show("Please confirm disconnecting all connected releases to cancel your entry");
        }
        confirmWindow.YesButtonText = "Confirm";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Cancel Entry");
                var warehouseEntryPMExtendedService: WarehouseEntryPMExtendedService = new WarehouseEntryPMExtendedService();
                warehouseEntryPMExtendedService.CancelEntry(this.EntityPM).subscribe((response: ServiceResponse) => {
                    var pmResponse: ServiceResponse = response;

                    this.CurrentSession.StopBusyIndicator();

                    if (!pmResponse.HasError) {
                        this.EntityPM = pmResponse.Result;
                        this.CurrentSession.FireEvent("CancelEntry");
                        this.entityArgs.EditComponent.SaveChanges();

                    }
                    else {
                        this.entityArgs.EditComponent.ValidationErrorsList = pmResponse.ErrorsArray;
                    }

                });
            }

        });
    }
}
