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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var AdvancedAccountingComponent = /** @class */ (function (_super) {
    __extends(AdvancedAccountingComponent, _super);
    function AdvancedAccountingComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 160;
        _this.ControlColumnWidth = 100;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        return _this;
    }
    AdvancedAccountingComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs;
        this.ObjectTableName = this.EntityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.Clone();
        this.SetUIProperties();
    };
    AdvancedAccountingComponent.prototype.CopyProperties = function () {
        this.myAccountingInformation1 = this.AccountingInformation1;
        this.myAccountingInformation2 = this.AccountingInformation2;
        this.myAccountingInformation3 = this.AccountingInformation3;
        this.myAccountingInformation4 = this.AccountingInformation4;
        this.myAccountingInformation5 = this.AccountingInformation5;
        this.myAccountingInformation6 = this.AccountingInformation6;
        this.myAccountingInformationIdentifierCode1 = this.AccountingInformationIdentifierCode1;
        this.myAccountingInformationIdentifierCode2 = this.AccountingInformationIdentifierCode2;
        this.myAccountingInformationIdentifierCode3 = this.AccountingInformationIdentifierCode3;
        this.myAccountingInformationIdentifierCode4 = this.AccountingInformationIdentifierCode4;
        this.myAccountingInformationIdentifierCode5 = this.AccountingInformationIdentifierCode5;
        this.myAccountingInformationIdentifierCode6 = this.AccountingInformationIdentifierCode6;
    };
    AdvancedAccountingComponent.prototype.ResetProperties = function () {
        this.AccountingInformation1 = this.myAccountingInformation1;
        this.AccountingInformation2 = this.myAccountingInformation2;
        this.AccountingInformation3 = this.myAccountingInformation3;
        this.AccountingInformation4 = this.myAccountingInformation4;
        this.AccountingInformation5 = this.myAccountingInformation5;
        this.AccountingInformation6 = this.myAccountingInformation6;
        this.AccountingInformationIdentifierCode1 = this.myAccountingInformationIdentifierCode1;
        this.AccountingInformationIdentifierCode2 = this.myAccountingInformationIdentifierCode2;
        this.AccountingInformationIdentifierCode3 = this.myAccountingInformationIdentifierCode3;
        this.AccountingInformationIdentifierCode4 = this.myAccountingInformationIdentifierCode4;
        this.AccountingInformationIdentifierCode5 = this.myAccountingInformationIdentifierCode5;
        this.AccountingInformationIdentifierCode6 = this.myAccountingInformationIdentifierCode6;
    };
    AdvancedAccountingComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode2", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode3", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode4", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode5", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode6", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_Field1();
        this.SetUIProperties_Field2();
        this.SetUIProperties_Field3();
        this.SetUIProperties_Field4();
        this.SetUIProperties_Field5();
        this.SetUIProperties_Field6();
    };
    AdvancedAccountingComponent.prototype.SetUIProperties_Field1 = function () {
        var isFieldEmpty = Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode1);
        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation1", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation1", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("AccountingInformation1", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation1", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation1) ? true : false);
        }
    };
    AdvancedAccountingComponent.prototype.SetUIProperties_Field2 = function () {
        var isFieldEmpty = Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode2);
        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation2", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation2", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("AccountingInformation2", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation2", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation2) ? true : false);
        }
    };
    AdvancedAccountingComponent.prototype.SetUIProperties_Field3 = function () {
        var isFieldEmpty = Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode3);
        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation3", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation3", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("AccountingInformation3", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation3", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation3) ? true : false);
        }
    };
    AdvancedAccountingComponent.prototype.SetUIProperties_Field4 = function () {
        var isFieldEmpty = Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode4);
        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation4", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation4", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("AccountingInformation4", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation4", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation4) ? true : false);
        }
    };
    AdvancedAccountingComponent.prototype.SetUIProperties_Field5 = function () {
        var isFieldEmpty = Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode5);
        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation5", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation5", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("AccountingInformation5", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation5", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation5) ? true : false);
        }
    };
    AdvancedAccountingComponent.prototype.SetUIProperties_Field6 = function () {
        var isFieldEmpty = Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode6);
        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation6", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation6", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("AccountingInformation6", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation6", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation6) ? true : false);
        }
    };
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformationIdentifierCode1", {
        // Properties
        get: function () { return this.EntityPM.AccountingInformationIdentifierCode1; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformationIdentifierCode1 != newValue) {
                this.EntityPM.AccountingInformationIdentifierCode1 = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AccountingInformation1 = null;
                }
                this.SetUIProperties_Field1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformationIdentifierCode2", {
        get: function () { return this.EntityPM.AccountingInformationIdentifierCode2; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformationIdentifierCode2 != newValue) {
                this.EntityPM.AccountingInformationIdentifierCode2 = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AccountingInformation2 = null;
                }
                this.SetUIProperties_Field2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformationIdentifierCode3", {
        get: function () { return this.EntityPM.AccountingInformationIdentifierCode3; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformationIdentifierCode3 != newValue) {
                this.EntityPM.AccountingInformationIdentifierCode3 = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AccountingInformation3 = null;
                }
                this.SetUIProperties_Field3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformationIdentifierCode4", {
        get: function () { return this.EntityPM.AccountingInformationIdentifierCode4; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformationIdentifierCode4 != newValue) {
                this.EntityPM.AccountingInformationIdentifierCode4 = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AccountingInformation4 = null;
                }
                this.SetUIProperties_Field4();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformationIdentifierCode5", {
        get: function () { return this.EntityPM.AccountingInformationIdentifierCode5; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformationIdentifierCode5 != newValue) {
                this.EntityPM.AccountingInformationIdentifierCode5 = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AccountingInformation5 = null;
                }
                this.SetUIProperties_Field5();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformationIdentifierCode6", {
        get: function () { return this.EntityPM.AccountingInformationIdentifierCode6; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformationIdentifierCode6 != newValue) {
                this.EntityPM.AccountingInformationIdentifierCode6 = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AccountingInformation6 = null;
                }
                this.SetUIProperties_Field6();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformation1", {
        get: function () { return this.EntityPM.AccountingInformation1; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformation1 != newValue) {
                this.EntityPM.AccountingInformation1 = newValue;
                this.SetUIProperties_Field1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformation2", {
        get: function () { return this.EntityPM.AccountingInformation2; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformation2 != newValue) {
                this.EntityPM.AccountingInformation2 = newValue;
                this.SetUIProperties_Field2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformation3", {
        get: function () { return this.EntityPM.AccountingInformation3; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformation3 != newValue) {
                this.EntityPM.AccountingInformation3 = newValue;
                this.SetUIProperties_Field3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformation4", {
        get: function () { return this.EntityPM.AccountingInformation4; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformation4 != newValue) {
                this.EntityPM.AccountingInformation4 = newValue;
                this.SetUIProperties_Field4();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformation5", {
        get: function () { return this.EntityPM.AccountingInformation5; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformation5 != newValue) {
                this.EntityPM.AccountingInformation5 = newValue;
                this.SetUIProperties_Field5();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedAccountingComponent.prototype, "AccountingInformation6", {
        get: function () { return this.EntityPM.AccountingInformation6; },
        set: function (newValue) {
            if (this.EntityPM.AccountingInformation6 != newValue) {
                this.EntityPM.AccountingInformation6 = newValue;
                this.SetUIProperties_Field6();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    AdvancedAccountingComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvancedAccountingComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var minMaxMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.MinMax");
        minMaxMessage = minMaxMessage.replace("%Minlength", "0");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode1) && Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation1")));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode2) && Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation2)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation2")));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode3) && Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation3)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation3")));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode4) && Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation4)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation4")));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode5) && Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation5)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation5")));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode6) && Tools_1.AppTool.IsNullOrEmpty(this.AccountingInformation6)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation6")));
        }
        if (this.AccountingInformation1 != null && this.AccountingInformation1.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation1")));
        }
        if (this.AccountingInformation2 != null && this.AccountingInformation2.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation2")));
        }
        if (this.AccountingInformation3 != null && this.AccountingInformation3.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation3")));
        }
        if (this.AccountingInformation4 != null && this.AccountingInformation4.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation4")));
        }
        if (this.AccountingInformation5 != null && this.AccountingInformation5.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation5")));
        }
        if (this.AccountingInformation6 != null && this.AccountingInformation6.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AccountingInformation6")));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    AdvancedAccountingComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('AccountingInformation1');
        this.myCloner.AddField('AccountingInformation2');
        this.myCloner.AddField('AccountingInformation3');
        this.myCloner.AddField('AccountingInformation4');
        this.myCloner.AddField('AccountingInformation5');
        this.myCloner.AddField('AccountingInformation6');
        this.myCloner.AddField('AccountingInformationIdentifierCode1');
        this.myCloner.AddField('AccountingInformationIdentifierCode2');
        this.myCloner.AddField('AccountingInformationIdentifierCode3');
        this.myCloner.AddField('AccountingInformationIdentifierCode4');
        this.myCloner.AddField('AccountingInformationIdentifierCode5');
        this.myCloner.AddField('AccountingInformationIdentifierCode6');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AdvancedAccountingComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AdvancedAccountingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AdvancedAccountingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AdvancedAccountingComponent);
    return AdvancedAccountingComponent;
}(BaseComponent_1.BaseComponent));
exports.AdvancedAccountingComponent = AdvancedAccountingComponent;
//# sourceMappingURL=AdvancedAccountingComponent.js.map