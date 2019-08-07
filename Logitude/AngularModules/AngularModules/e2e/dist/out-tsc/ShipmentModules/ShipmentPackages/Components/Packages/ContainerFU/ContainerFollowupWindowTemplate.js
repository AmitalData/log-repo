"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var Tools_2 = require("../../../../../Shipment/Tools");
var ContainerFollowupWindowTemplate = /** @class */ (function () {
    function ContainerFollowupWindowTemplate() {
        this.ObjectTableName = "ShipmentPackage";
        this.HasRouting = false;
        this.IsDeliveryConnectedWithMultiContainers = false;
    }
    ContainerFollowupWindowTemplate.prototype.Run = function (args) {
        var _this = this;
        this.Code = args['Code'];
        this.DataContext = args['DataContext'];
        this.FatherComponent = args['FatherComponent'];
        this.EntityPM = this.DataContext.EntityPM;
        this.TabComponent = this.DataContext.fatherComponent;
        if (this.Code == "D") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                var allPackages = this.TabComponent.EntityPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.DeliveryId; });
                if (allPackages.length > 1) {
                    this.IsDeliveryConnectedWithMultiContainers = true;
                }
            }
        }
        this.SetProperties();
    };
    ContainerFollowupWindowTemplate.prototype.SetProperties = function () {
        var _this = this;
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.DataContext.ShipmentPM);
        if (this.IsEditingEnabled) {
            if (!this.IsFollowup) {
                this.IsEditingEnabled = false;
            }
            else if (this.Code == "D") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                    var allPackages = this.TabComponent.EntityPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.DeliveryId; });
                    if (allPackages.length > 1) {
                        this.IsEditingEnabled = false;
                    }
                }
            }
        }
        switch (this.Code) {
            case "D": {
                this.IsFollowupProperty = "IsDeliveryFU";
                this.ETDProperty = "DeliveryETD";
                this.ATDProperty = "DeliveryATD";
                this.ETAProperty = "DeliveryETA";
                this.ATAProperty = "DeliveryATA";
                this.FromProperty = "DeliveryFrom";
                this.ToProperty = "DeliveryTo";
                this.TransportModeProperty = "DeliveryTransportModeCode";
                this.HasRouting = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId) ? false : true;
                this.ActionRoutingLinkText = this.HasRouting == false ? "Create Container Delivery" : "View Container Delivery";
                this.DeleteRoutingLinkText = "Delete Container Delivery";
                this.DataContext.SetUIProperties_ValidateActualDates_D();
                break;
            }
            case "R": {
                this.IsFollowupProperty = "IsEmptyContainerReturnFU";
                this.ETDProperty = "EmptyContainerReturnETD";
                this.ATDProperty = "EmptyContainerReturnATD";
                this.ETAProperty = "EmptyContainerReturnETA";
                this.ATAProperty = "EmptyContainerReturnATA";
                this.FromProperty = "EmptyContainerReturnFrom";
                this.ToProperty = "EmptyContainerReturnTo";
                this.TransportModeProperty = "ECRTransportModeCode";
                this.HasRouting = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EmptyContainerReturnId) ? false : true;
                this.ActionRoutingLinkText = this.HasRouting == false ? "Create Empty Container Return" : "View Empty Container Return";
                this.DeleteRoutingLinkText = "Delete Empty Container Return";
                this.DataContext.SetUIProperties_ValidateActualDates_R();
                break;
            }
        }
    };
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "IsFollowup", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.IsDeliveryFU;
                }
                case "R": {
                    return this.DataContext.IsEmptyContainerReturnFU;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.IsDeliveryFU = value;
                    break;
                }
                case "R": {
                    this.DataContext.IsEmptyContainerReturnFU = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "ETD", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryETD;
                }
                case "R": {
                    return this.DataContext.EmptyContainerReturnETD;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryETD = value;
                    break;
                }
                case "R": {
                    this.DataContext.EmptyContainerReturnETD = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "ATD", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryATD;
                }
                case "R": {
                    return this.DataContext.EmptyContainerReturnATD;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryATD = value;
                    break;
                }
                case "R": {
                    this.DataContext.EmptyContainerReturnATD = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "ETA", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryETA;
                }
                case "R": {
                    return this.DataContext.EmptyContainerReturnETA;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryETA = value;
                    break;
                }
                case "R": {
                    this.DataContext.EmptyContainerReturnETA = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "ATA", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryATA;
                }
                case "R": {
                    return this.DataContext.EmptyContainerReturnATA;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryATA = value;
                    break;
                }
                case "R": {
                    this.DataContext.EmptyContainerReturnATA = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "From", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryFrom;
                }
                case "R": {
                    return this.DataContext.EmptyContainerReturnFrom;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryFrom = value;
                    break;
                }
                case "R": {
                    this.DataContext.EmptyContainerReturnFrom = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "To", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryTo;
                }
                case "R": {
                    return this.DataContext.EmptyContainerReturnTo;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryTo = value;
                    break;
                }
                case "R": {
                    this.DataContext.EmptyContainerReturnTo = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWindowTemplate.prototype, "TransportModeCode", {
        get: function () {
            switch (this.Code) {
                case "D": {
                    return this.DataContext.DeliveryTransportModeCode;
                }
                case "R": {
                    return this.DataContext.ECRTransportModeCode;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (value) {
            switch (this.Code) {
                case "D": {
                    this.DataContext.DeliveryTransportModeCode = value;
                    break;
                }
                case "R": {
                    this.DataContext.ECRTransportModeCode = value;
                    break;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ContainerFollowupWindowTemplate.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "DeliveryETD":
            case "EmptyContainerReturnETD":
                {
                    this.ATD = Tools_1.DateTool.GetDateParts(this.ETD).DateObject;
                    break;
                }
            case "DeliveryETA":
            case "EmptyContainerReturnETA":
                {
                    this.ATA = Tools_1.DateTool.GetDateParts(this.ETA).DateObject;
                    break;
                }
        }
    };
    ContainerFollowupWindowTemplate.prototype.ActionRoutingLinkClicked = function () {
        var isValid = this.FatherComponent.Validate();
        if (isValid) {
            this.FatherComponent.Save("ActionLink");
        }
    };
    ContainerFollowupWindowTemplate.prototype.DeleteRoutingLinkClicked = function () {
        var _this = this;
        if (this.ATD || this.ATA) {
            var iWindow = new MessageWindow_1.MessageWindow();
            iWindow.Show("Actual Dates filled, can't delete delivery");
        }
        else {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisDelivery");
            if (this.Code == "R") {
                message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisEmptyCR");
            }
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    var isValid = _this.FatherComponent.Validate();
                    if (isValid) {
                        var myDeliveryId = null;
                        switch (_this.Code) {
                            case "D": {
                                myDeliveryId = _this.EntityPM.DeliveryId;
                                _this.DataContext.DeliveryId = null;
                                break;
                            }
                            case "R": {
                                myDeliveryId = _this.EntityPM.EmptyContainerReturnId;
                                _this.DataContext.EmptyContainerReturnId = null;
                                break;
                            }
                        }
                        if (myDeliveryId) {
                            var item = _this.TabComponent.EntityPM.ShipmentDeliveries.filter(function (f) { return f.Id == myDeliveryId; })[0];
                            if (item) {
                                _this.TabComponent.EntityPM.RemoveDelivery(item);
                                _this.FatherComponent.Save("DeleteLink");
                            }
                        }
                    }
                }
            });
        }
    };
    ContainerFollowupWindowTemplate.prototype.DisconnectDeliveryLinkClicked = function () {
        var _this = this;
        if (this.Code == "D") {
            var Delivery = this.TabComponent.EntityPM.ShipmentDeliveries.filter(function (f) { return f.Id == _this.EntityPM.DeliveryId; })[0];
            if (Delivery) {
                Delivery.ConnectedPackageId = null;
                if (Delivery.AllConnectedPackagesId) {
                    var index = Delivery.AllConnectedPackagesId.indexOf(this.EntityPM.Id);
                    if (index > -1) {
                        Delivery.AllConnectedPackagesId.splice(index, 1);
                    }
                }
                var DeliveryPackagePM = Delivery.ShipmentPickUpDeliveryPackages.filter(function (f) { return f.OriginalShipmentPackageId == _this.EntityPM.Id; })[0];
                if (DeliveryPackagePM) {
                    Delivery.RemovePackage(DeliveryPackagePM);
                }
            }
            this.DataContext.DeliveryId = null;
            this.FatherComponent.Save("DeleteLink");
        }
    };
    ContainerFollowupWindowTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContainerFollowupWindowTemplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], ContainerFollowupWindowTemplate);
    return ContainerFollowupWindowTemplate;
}());
exports.ContainerFollowupWindowTemplate = ContainerFollowupWindowTemplate;
//# sourceMappingURL=ContainerFollowupWindowTemplate.js.map