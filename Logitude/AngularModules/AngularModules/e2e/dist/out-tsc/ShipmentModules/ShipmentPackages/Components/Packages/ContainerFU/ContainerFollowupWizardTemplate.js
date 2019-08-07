"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var ContainerFollowupWizardTemplate = /** @class */ (function (_super) {
    __extends(ContainerFollowupWizardTemplate, _super);
    function ContainerFollowupWizardTemplate() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "ShipmentPackage";
        _this.IsDeliveryConnectedWithMultiContainers = false;
        _this.HasRouting_D = false;
        _this.HasRouting_R = false;
        _this.IsEditingEnabled = true;
        _this.IsEditingEnabled_D = true;
        return _this;
    }
    ContainerFollowupWizardTemplate.prototype.Run = function (args) {
        this.FatherComponent = args['FatherComponent'];
        this.EntityPM = this.FatherComponent.EntityPM;
        this.ShipmentPM = this.FatherComponent.ShipmentPM;
        this.IsDeliveryConnectedWithMultiContainers = this.FatherComponent.IsDeliveryConnectedWithMultiContainers;
        this.SetProperties();
        this.SetUIProperties();
    };
    ContainerFollowupWizardTemplate.prototype.SetProperties = function () {
        this.HasRouting_D = Tools_1.AppTool.IsNullOrEmpty(this.DeliveryId) ? false : true;
        this.HasRouting_R = Tools_1.AppTool.IsNullOrEmpty(this.EmptyContainerReturnId) ? false : true;
        this.ActionRoutingLinkText_D = this.HasRouting_D == false ? "Create Container Delivery" : "View Container Delivery";
        this.ActionRoutingLinkText_R = this.HasRouting_R == false ? "Create Empty Container Return" : "View Empty Container Return";
    };
    ContainerFollowupWizardTemplate.prototype.SetUIProperties = function () {
        this.SetUIProperties_ContainerFU();
        this.SetUIProperties_ValidateActualDates_D();
        this.SetUIProperties_ValidateActualDates_R();
    };
    ContainerFollowupWizardTemplate.prototype.SetUIProperties_ContainerFU = function () {
        var isDeliveryFieldsEnabled = false;
        var isDeliveryPlacesEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsDeliveryFU) {
                if (this.IsDeliveryConnectedWithMultiContainers == false) {
                    isDeliveryFieldsEnabled = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.DeliveryId)) {
                        isDeliveryPlacesEnabled = true;
                    }
                }
            }
        }
        this.IsEditingEnabled_D = isDeliveryFieldsEnabled;
        this.UIProperties.SetEnabled("IsDeliveryFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DeliveryETD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryETA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryTransportModeCode", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryFrom", this.ObjectTableName, isDeliveryPlacesEnabled);
        this.UIProperties.SetEnabled("DeliveryTo", this.ObjectTableName, isDeliveryPlacesEnabled);
        var isEmptyContainerReturnFieldsEnabled = false;
        var isEmptyContainerReturnPlacesEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsEmptyContainerReturnFU) {
                isEmptyContainerReturnFieldsEnabled = true;
                if (Tools_1.AppTool.IsNullOrEmpty(this.EmptyContainerReturnId)) {
                    isEmptyContainerReturnPlacesEnabled = true;
                }
            }
        }
        this.UIProperties.SetEnabled("IsEmptyContainerReturnFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("ECRTransportModeCode", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnFrom", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnTo", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
    };
    ContainerFollowupWizardTemplate.prototype.SetUIProperties_ValidateActualDates_D = function () {
        this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.DeliveryATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD"));
            this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.DeliveryATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA"));
            this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, false, errorMessage);
        }
    };
    ContainerFollowupWizardTemplate.prototype.SetUIProperties_ValidateActualDates_R = function () {
        this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.EmptyContainerReturnATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD"));
            this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.EmptyContainerReturnATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA"));
            this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, false, errorMessage);
        }
    };
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "IsDeliveryFU", {
        get: function () { return this.EntityPM.IsDeliveryFU; },
        set: function (value) {
            if (this.EntityPM.IsDeliveryFU != value) {
                this.EntityPM.IsDeliveryFU = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryId", {
        get: function () { return this.EntityPM.DeliveryId; },
        set: function (value) {
            if (this.EntityPM.DeliveryId != value) {
                this.EntityPM.DeliveryId = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryFrom", {
        get: function () { return this.EntityPM.DeliveryFrom; },
        set: function (value) {
            if (this.EntityPM.DeliveryFrom != value) {
                this.EntityPM.DeliveryFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryTo", {
        get: function () { return this.EntityPM.DeliveryTo; },
        set: function (value) {
            if (this.EntityPM.DeliveryTo != value) {
                this.EntityPM.DeliveryTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryTransportModeCode", {
        get: function () { return this.EntityPM.DeliveryTransportModeCode; },
        set: function (value) {
            if (this.EntityPM.DeliveryTransportModeCode != value) {
                this.EntityPM.DeliveryTransportModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryETD", {
        get: function () { return this.EntityPM.DeliveryETD; },
        set: function (value) {
            if (this.EntityPM.DeliveryETD != value) {
                this.EntityPM.DeliveryETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryETA", {
        get: function () { return this.EntityPM.DeliveryETA; },
        set: function (value) {
            if (this.EntityPM.DeliveryETA != value) {
                this.EntityPM.DeliveryETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryATD", {
        get: function () { return this.EntityPM.DeliveryATD; },
        set: function (value) {
            if (this.EntityPM.DeliveryATD != value) {
                this.EntityPM.DeliveryATD = value;
                this.SetUIProperties_ValidateActualDates_D();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "DeliveryATA", {
        get: function () { return this.EntityPM.DeliveryATA; },
        set: function (value) {
            if (this.EntityPM.DeliveryATA != value) {
                this.EntityPM.DeliveryATA = value;
                this.SetUIProperties_ValidateActualDates_D();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "IsEmptyContainerReturnFU", {
        get: function () { return this.EntityPM.IsEmptyContainerReturnFU; },
        set: function (value) {
            if (this.EntityPM.IsEmptyContainerReturnFU != value) {
                this.EntityPM.IsEmptyContainerReturnFU = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnId", {
        get: function () { return this.EntityPM.EmptyContainerReturnId; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnId != value) {
                this.EntityPM.EmptyContainerReturnId = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnFrom", {
        get: function () { return this.EntityPM.EmptyContainerReturnFrom; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnFrom != value) {
                this.EntityPM.EmptyContainerReturnFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnTo", {
        get: function () { return this.EntityPM.EmptyContainerReturnTo; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnTo != value) {
                this.EntityPM.EmptyContainerReturnTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "ECRTransportModeCode", {
        get: function () { return this.EntityPM.ECRTransportModeCode; },
        set: function (value) {
            if (this.EntityPM.ECRTransportModeCode != value) {
                this.EntityPM.ECRTransportModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnETD", {
        get: function () { return this.EntityPM.EmptyContainerReturnETD; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnETD != value) {
                this.EntityPM.EmptyContainerReturnETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnETA", {
        get: function () { return this.EntityPM.EmptyContainerReturnETA; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnETA != value) {
                this.EntityPM.EmptyContainerReturnETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnATD", {
        get: function () { return this.EntityPM.EmptyContainerReturnATD; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnATD != value) {
                this.EntityPM.EmptyContainerReturnATD = value;
                this.SetUIProperties_ValidateActualDates_R();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowupWizardTemplate.prototype, "EmptyContainerReturnATA", {
        get: function () { return this.EntityPM.EmptyContainerReturnATA; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnATA != value) {
                this.EntityPM.EmptyContainerReturnATA = value;
                this.SetUIProperties_ValidateActualDates_R();
            }
        },
        enumerable: true,
        configurable: true
    });
    ContainerFollowupWizardTemplate.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "DeliveryETD": {
                this.DeliveryATD = Tools_1.DateTool.GetDateParts(this.DeliveryETD).DateObject;
                break;
            }
            case "DeliveryETA": {
                this.DeliveryATA = Tools_1.DateTool.GetDateParts(this.DeliveryETA).DateObject;
                break;
            }
            case "EmptyContainerReturnETD": {
                this.EmptyContainerReturnATD = Tools_1.DateTool.GetDateParts(this.EmptyContainerReturnETD).DateObject;
                break;
            }
            case "EmptyContainerReturnETA": {
                this.EmptyContainerReturnATA = Tools_1.DateTool.GetDateParts(this.EmptyContainerReturnETA).DateObject;
                break;
            }
        }
    };
    ContainerFollowupWizardTemplate.prototype.ActionRoutingLinkClicked = function (typeCode) {
        var isValid = this.FatherComponent.Validate();
        if (isValid) {
            this.FatherComponent.Save("ActionLink_" + typeCode);
        }
    };
    ContainerFollowupWizardTemplate.prototype.DeleteRoutingLinkClicked = function (typeCode) {
        var _this = this;
        var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisDelivery");
        if (typeCode == "R") {
            message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisEmptyCR");
        }
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(message);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var isValid = _this.FatherComponent.Validate();
                if (isValid) {
                    var myDeliveryId = null;
                    switch (typeCode) {
                        case "D": {
                            myDeliveryId = _this.DeliveryId;
                            _this.DeliveryId = null;
                            break;
                        }
                        case "R": {
                            myDeliveryId = _this.EmptyContainerReturnId;
                            _this.EmptyContainerReturnId = null;
                            break;
                        }
                    }
                    if (myDeliveryId) {
                        var item = _this.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.Id == myDeliveryId; })[0];
                        if (item) {
                            _this.ShipmentPM.RemoveDelivery(item);
                            _this.FatherComponent.Save("DeleteLink");
                        }
                    }
                }
            }
        });
    };
    ContainerFollowupWizardTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContainerFollowupWizardTemplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], ContainerFollowupWizardTemplate);
    return ContainerFollowupWizardTemplate;
}(BaseComponent_1.BaseComponent));
exports.ContainerFollowupWizardTemplate = ContainerFollowupWizardTemplate;
//# sourceMappingURL=ContainerFollowupWizardTemplate.js.map