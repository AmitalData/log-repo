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
        var errors = [];
        if (errors.length == 0) {

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
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
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
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Title = "Cancel Entry";
        if (this.IsNoConnectedReleaseEntity) {
            confirmWindow.Show("Confirm cancelling this entry");
            confirmWindow.YesButtonText = "Confirm";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CurrentSession.StartBusyIndicator("Cancel Entry");
                    var warehouseEntryPMExtendedService: WarehouseEntryPMExtendedService = new WarehouseEntryPMExtendedService();
                    this.EntityPM.WarehouseEntryPackages.forEach(entryPackage => {
                        entryPackage.Quantity = 0;
                    });
                    warehouseEntryPMExtendedService.CancelEntry(this.EntityPM).subscribe((response: ServiceResponse) => {
                        var pmResponse: ServiceResponse = response;

                        this.CurrentSession.StopBusyIndicator();

                        if (!pmResponse.HasError) {
                            this.EntityPM = pmResponse.Result;
                            this.entityArgs.EditComponent.SaveChanges();
                            this.entityArgs.EditComponent.ReloadEntityPM();

                        }
                        else {
                            this.entityArgs.EditComponent.ValidationErrorsList = pmResponse.ErrorsArray;
                        }

                    });
                }
            });
        }
        else {
            confirmWindow.Show("You should cancel all connected releases first.");
            confirmWindow.YesButtonText = "Ok";
            confirmWindow.ShowNoButton = false;
        }
    }
}
