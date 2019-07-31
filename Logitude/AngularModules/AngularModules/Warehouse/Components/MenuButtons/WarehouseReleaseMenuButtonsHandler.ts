

declare var window: any;
import {WarehouseReleasePM} from '../../EntityPMs/WarehouseReleasePM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';

import {WarehouseReleaseValidator}  from '../../Validators/WarehouseReleaseValidator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {DateTool} from '../../../Infrastructure/Tools';

import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentDeliveryPM} from '../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {WarehouseReleasePMExtendedService} from '../../Services/ExtendedPMs/WarehouseReleasePMExtendedService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

export class WarehouseReleaseMenuButtonsHandler {
    public EntityPM: WarehouseReleasePM;
    public entityArgs: EntityArgs
    private status: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    EntityParentPM: ShipmentPM;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;

        if (this.entityArgs.EditComponent && this.entityArgs.EditComponent.EntityParentPM) {
            this.EntityParentPM = this.entityArgs.EditComponent.EntityParentPM;
        }

        this.Listen();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'WarehouseRelease')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "CreateDelivery":
                            {
                                if (!this.EntityParentPM) {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                        case "WRMO":
                            {
                                button.IsHidden = true;

                                break;
                            }
                        case "CancelRelease":
                            {
                                if (this.EntityPM.StatusCode == "CARE") {
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
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.isButtonClicked = true;

            switch (menuButton.EventCode) {
                case "CreateDelivery":
                    {
                        if (this.EntityParentPM) {
                            this.CreateDelivery();
                        } else this.isButtonClicked = false
                        break;
                    }
                case "CancelRelease":
                    {
                      this.CancelRelease();
             
                        break;
                    }


                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }
        }
    }



    isValid: boolean = false;
    isButtonClicked: boolean = false;
    IsRunDelivery: boolean = false;
    StopFlags() {
        this.IsRunDelivery = false;
        this.isButtonClicked = false;
        this.IsRunCancelRelease = false;

    }
    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                this.isButtonClicked = false;
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    if (this.IsRunDelivery) {
                        this.OpenDeliveryWindow();
                    }
                }

                this.StopFlags();
            });


        }
    }

    Validate() {
        var validator = new WarehouseReleaseValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.StopFlags();
        }
    }


    OpenDeliveryWindow() {

        var myDeliveryIndex = 1;
        if (this.EntityParentPM.ShipmentDeliveryIndex) {
            myDeliveryIndex = this.EntityParentPM.ShipmentDeliveryIndex + 1;
        }

        var newDeliveryPM = new ShipmentDeliveryPM(null);
        newDeliveryPM.FullResponsibility = true;
        newDeliveryPM.Tenant = this.EntityParentPM.Tenant;
        newDeliveryPM.ShipmentId = this.EntityParentPM.Id;
        newDeliveryPM.ShipmentNumber = this.EntityParentPM.ShipmentNumber;
        newDeliveryPM.PickUpDeliveryNumber = this.EntityParentPM.ShipmentNumber + "/" + myDeliveryIndex;
        newDeliveryPM.PickUpDeliveryTypeCode = "DELV";
        newDeliveryPM.ETD = this.EntityPM.ExpectedReleaseDate;
        newDeliveryPM.ATD = this.EntityPM.ActualReleaseDate;
        newDeliveryPM.FromPartnerCardId = this.EntityPM.WarehouseId;
        newDeliveryPM.ToPartnerCardId = this.EntityPM.CustomerId;

        if (this.EntityPM.WarehouseReleasePackages && this.EntityPM.WarehouseReleasePackages.length > 0) {
            newDeliveryPM.ShipmentPickUpDeliveryPackages = [];
            this.EntityPM.WarehouseReleasePackages.forEach(item => {
                var itemPM = new ShipmentPickUpDeliveryPackagePM(null);
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.ShipmentPickUpDeliveryId = newDeliveryPM.Id;
                itemPM.Width = item.Width;
                itemPM.Height = item.Height;
                itemPM.Length = item.Length;
                itemPM.Description = item.Description;
                itemPM.Quantity = item.Quantity;
                itemPM.Volume = item.Volume;
                itemPM.Weight = item.Weight;
                itemPM.ContainerNumber = item.ContainerNumber;
                itemPM.ShipperSeal = item.Seal;
                itemPM.Tenant = item.Tenant;
                itemPM.Harmonize = item.Harmonize;
                itemPM.PackageTypeId = item.PackageTypeId;
                itemPM.PackageTypeName = item.PackageTypeName;
                newDeliveryPM.ShipmentPickUpDeliveryPackages.push(itemPM);
            });
        }





        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");
        logitudeWindow.WindowArgs = { ShipmentPM: this.EntityParentPM, EntityPM: newDeliveryPM, IsNewEntity: true, IsOutSource: true, WareHouseRelaseWareHouseId: this.EntityPM.WarehouseId, WareHouseRelaseCustomerId: this.EntityPM.CustomerId };
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;
        logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
        logitudeWindow.WindowClosed.subscribe(s => {
            this.isButtonClicked = false;
        });

    }


    CreateDelivery() {
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.Validate();
            if (this.isValid) {
                this.IsRunDelivery = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        } else {
            this.OpenDeliveryWindow();
        }
    }




    IsRunCancelRelease: boolean = false;
    CancelReleaseButtonClcik() {
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.Validate();
            if (this.isValid) {
                this.IsRunCancelRelease = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        } else {
            this.CancelRelease();
        }

    }


    CancelRelease() {
        this.isButtonClicked = false;

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Title = "Cancel Release";
        confirmWindow.Show("Confirm cancelling this release");
        confirmWindow.YesButtonText = "Confirm";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Cancel Release");
                var warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
                warehouseReleasePMExtendedService.CancelRelease(this.EntityPM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;

                    this.CurrentSession.StopBusyIndicator();

                    if (!pmResponse.HasError) {
                        this.EntityPM = pmResponse.Result;
                        this.CurrentSession.FireEvent("CancelRelease");
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                    } else {

                        this.entityArgs.EditComponent.ValidationErrorsList = pmResponse.ErrorsArray;

                    }

                });
            }

        });


    

    }
}
