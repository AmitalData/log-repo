"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var WarehouseReleaseValidator_1 = require("../../Validators/WarehouseReleaseValidator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentDeliveryPM_1 = require("../../../Shipment/EntityPMs/ShipmentDeliveryPM");
var ShipmentPickUpDeliveryPackagePM_1 = require("../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var WarehouseReleasePMExtendedService_1 = require("../../Services/ExtendedPMs/WarehouseReleasePMExtendedService");
var WarehouseReleaseMenuButtonsHandler = /** @class */ (function () {
    function WarehouseReleaseMenuButtonsHandler() {
        this.status = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isValid = false;
        this.isButtonClicked = false;
        this.IsRunDelivery = false;
        this.IsRunCancelRelease = false;
    }
    WarehouseReleaseMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        if (this.entityArgs.EditComponent && this.entityArgs.EditComponent.EntityParentPM) {
            this.EntityParentPM = this.entityArgs.EditComponent.EntityParentPM;
        }
        this.Listen();
    };
    WarehouseReleaseMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'WarehouseRelease'; })[0];
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
    };
    WarehouseReleaseMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            switch (menuButton.EventCode) {
                case "CreateDelivery":
                    {
                        if (this.EntityParentPM) {
                            this.CreateDelivery();
                        }
                        else
                            this.isButtonClicked = false;
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
    };
    WarehouseReleaseMenuButtonsHandler.prototype.StopFlags = function () {
        this.IsRunDelivery = false;
        this.isButtonClicked = false;
        this.IsRunCancelRelease = false;
    };
    WarehouseReleaseMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                _this.isButtonClicked = false;
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.IsRunDelivery) {
                        _this.OpenDeliveryWindow();
                    }
                }
                _this.StopFlags();
            });
        }
    };
    WarehouseReleaseMenuButtonsHandler.prototype.Validate = function () {
        var validator = new WarehouseReleaseValidator_1.WarehouseReleaseValidator();
        var errors = validator.Validate(this.EntityPM);
        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;
        if (!this.isValid) {
            this.StopFlags();
        }
    };
    WarehouseReleaseMenuButtonsHandler.prototype.OpenDeliveryWindow = function () {
        var _this = this;
        var myDeliveryIndex = 1;
        if (this.EntityParentPM.ShipmentDeliveryIndex) {
            myDeliveryIndex = this.EntityParentPM.ShipmentDeliveryIndex + 1;
        }
        var newDeliveryPM = new ShipmentDeliveryPM_1.ShipmentDeliveryPM(null);
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
            this.EntityPM.WarehouseReleasePackages.forEach(function (item) {
                var itemPM = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(null);
                itemPM.Tenant = _this.EntityPM.Tenant;
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
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");
        logitudeWindow.WindowArgs = { ShipmentPM: this.EntityParentPM, EntityPM: newDeliveryPM, IsNewEntity: true, IsOutSource: true, WareHouseRelaseWareHouseId: this.EntityPM.WarehouseId, WareHouseRelaseCustomerId: this.EntityPM.CustomerId };
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;
        logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
        logitudeWindow.WindowClosed.subscribe(function (s) {
            _this.isButtonClicked = false;
        });
    };
    WarehouseReleaseMenuButtonsHandler.prototype.CreateDelivery = function () {
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.Validate();
            if (this.isValid) {
                this.IsRunDelivery = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
        else {
            this.OpenDeliveryWindow();
        }
    };
    WarehouseReleaseMenuButtonsHandler.prototype.CancelReleaseButtonClcik = function () {
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.Validate();
            if (this.isValid) {
                this.IsRunCancelRelease = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
        else {
            this.CancelRelease();
        }
    };
    WarehouseReleaseMenuButtonsHandler.prototype.CancelRelease = function () {
        var _this = this;
        this.isButtonClicked = false;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Title = "Cancel Release";
        confirmWindow.Show("Confirm cancelling this release");
        confirmWindow.YesButtonText = "Confirm";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentSession.StartBusyIndicator("Cancel Release");
                var warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService_1.WarehouseReleasePMExtendedService();
                warehouseReleasePMExtendedService.CancelRelease(_this.EntityPM).subscribe(function (res) {
                    var pmResponse = res;
                    _this.CurrentSession.StopBusyIndicator();
                    if (!pmResponse.HasError) {
                        _this.EntityPM = pmResponse.Result;
                        _this.CurrentSession.FireEvent("CancelRelease");
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                    else {
                        _this.entityArgs.EditComponent.ValidationErrorsList = pmResponse.ErrorsArray;
                    }
                });
            }
        });
    };
    return WarehouseReleaseMenuButtonsHandler;
}());
exports.WarehouseReleaseMenuButtonsHandler = WarehouseReleaseMenuButtonsHandler;
//# sourceMappingURL=WarehouseReleaseMenuButtonsHandler.js.map