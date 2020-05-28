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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ChargesTypePM_1 = require("../../../EntityPMs/ChargesTypePM");
var ChargesTypePMService_1 = require("../../../../Common/Services/StandardPMs/ChargesTypePMService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ChargesGroupListService_1 = require("../../../../Infrastructure/Services/StandardLists/ChargesGroupListService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var VatTypeListService_1 = require("../../../Services/StandardLists/VatTypeListService");
var NewChargesTypeComponent = /** @class */ (function (_super) {
    __extends(NewChargesTypeComponent, _super);
    function NewChargesTypeComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "ChargesType";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CustomsFieldsIsVisible = false;
        // Pages Properties
        _this.Page1Hidden = false;
        _this.Page2Hidden = true;
        _this.Page3Hidden = true;
        _this.IsPreviousEnabled = false;
        _this.IsNextEnabled = true;
        _this.IsFinishEnabled = false;
        _this.EntityPM = new ChargesTypePM_1.ChargesTypePM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.AddedManually = true;
        _this.IsAir = true;
        _this.IsInland = true;
        _this.IsOcean = true;
        _this.AWBPrintDescription = true;
        _this.ViewOrder = 100;
        if (SessionLocator_1.SessionLocator.TenantPM.TenantVATManagement == false) {
            var myService = new VatTypeListService_1.VatTypeListService();
            myService.getAllFromCache().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var allVats = myResponse.Result;
                    if (allVats) {
                        var myZEROVat = allVats.filter(function (f) { return f.Code == "ZERO"; })[0];
                        if (myZEROVat) {
                            _this.EntityPM.VatTypeId = myZEROVat.Id;
                        }
                    }
                }
            });
        }
        _this.SetUIProperties();
        return _this;
    }
    NewChargesTypeComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("MeasurementId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MeasurementId));
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, this.IsAir);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, this.IsAir);
        this.UIProperties.SetEnabled("AWBPrintDescription", this.ObjectTableName, this.IsAir);
        if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
            if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                this.CustomsFieldsIsVisible = true;
            }
        }
    };
    Object.defineProperty(NewChargesTypeComponent.prototype, "Code", {
        // Properties
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            if (this.EntityPM.Code != newValue) {
                this.EntityPM.Code = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "ChargesGroupCode", {
        get: function () { return this.EntityPM.ChargesGroupCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargesGroupCode != newValue) {
                this.EntityPM.ChargesGroupCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "ChargesGroupId", {
        get: function () { return this.EntityPM.ChargesGroupId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ChargesGroupId != newValue) {
                this.EntityPM.ChargesGroupId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    var myService = new ChargesGroupListService_1.ChargesGroupListService();
                    myService.getSingleFromCache(this.EntityPM.ChargesGroupId).subscribe(function (myResponse) {
                        if (!myResponse.HasError && myResponse.Result) {
                            _this.ChargesGroupCode = myResponse.Result.Code;
                        }
                    });
                }
                else
                    this.ChargesGroupCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (newValue) {
            if (this.EntityPM.MeasurementId != newValue) {
                this.EntityPM.MeasurementId = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "ContainerMeasurementId", {
        get: function () { return this.EntityPM.ContainerMeasurementId; },
        set: function (newValue) {
            if (this.EntityPM.ContainerMeasurementId != newValue) {
                this.EntityPM.ContainerMeasurementId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsAir", {
        get: function () { return this.EntityPM.IsAir; },
        set: function (newValue) {
            if (this.EntityPM.IsAir != newValue) {
                this.EntityPM.IsAir = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsInland", {
        get: function () { return this.EntityPM.IsInland; },
        set: function (newValue) {
            if (this.EntityPM.IsInland != newValue) {
                this.EntityPM.IsInland = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsOcean", {
        get: function () { return this.EntityPM.IsOcean; },
        set: function (newValue) {
            if (this.EntityPM.IsOcean != newValue) {
                this.EntityPM.IsOcean = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsAutoDisplayInQuote", {
        get: function () { return this.EntityPM.IsAutoDisplayInQuote; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInQuote != newValue) {
                this.EntityPM.IsAutoDisplayInQuote = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsAutoDisplayInShipment", {
        get: function () { return this.EntityPM.IsAutoDisplayInShipment; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInShipment != newValue) {
                this.EntityPM.IsAutoDisplayInShipment = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsAutoDisplayInConsolidation", {
        get: function () { return this.EntityPM.IsAutoDisplayInConsolidation; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInConsolidation != newValue) {
                this.EntityPM.IsAutoDisplayInConsolidation = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsAutoDisplayInCustoms", {
        get: function () { return this.EntityPM.IsAutoDisplayInCustoms; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInCustoms != newValue) {
                this.EntityPM.IsAutoDisplayInCustoms = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsImport", {
        get: function () { return this.EntityPM.IsImport; },
        set: function (newValue) {
            if (this.EntityPM.IsImport != newValue) {
                this.EntityPM.IsImport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsExport", {
        get: function () { return this.EntityPM.IsExport; },
        set: function (newValue) {
            if (this.EntityPM.IsExport != newValue) {
                this.EntityPM.IsExport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsDrop", {
        get: function () { return this.EntityPM.IsDrop; },
        set: function (newValue) {
            if (this.EntityPM.IsDrop != newValue) {
                this.EntityPM.IsDrop = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IsDomestic", {
        get: function () { return this.EntityPM.IsDomestic; },
        set: function (newValue) {
            if (this.EntityPM.IsDomestic != newValue) {
                this.EntityPM.IsDomestic = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "DueTypeCode", {
        get: function () { return this.EntityPM.DueTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.DueTypeCode != newValue) {
                this.EntityPM.DueTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "IATACodeId", {
        get: function () { return this.EntityPM.IATACodeId; },
        set: function (newValue) {
            if (this.EntityPM.IATACodeId != newValue) {
                this.EntityPM.IATACodeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "AWBPrintDescription", {
        get: function () { return this.EntityPM.AWBPrintDescription; },
        set: function (newValue) {
            if (this.EntityPM.AWBPrintDescription != newValue) {
                this.EntityPM.AWBPrintDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "VatTypeId", {
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (newValue) {
            if (this.EntityPM.VatTypeId != newValue) {
                this.EntityPM.VatTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewChargesTypeComponent.prototype, "ViewOrder", {
        get: function () { return this.EntityPM.ViewOrder; },
        set: function (newValue) {
            if (this.EntityPM.ViewOrder != newValue) {
                this.EntityPM.ViewOrder = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewChargesTypeComponent.prototype.PreviousButtonClicked = function () {
        this.IsFinishEnabled = true;
        if (!this.Page2Hidden) {
            this.IsPreviousEnabled = false;
            this.IsNextEnabled = true;
            this.Page1Hidden = false;
            this.Page2Hidden = true;
            this.Page3Hidden = true;
        }
        else if (!this.Page3Hidden) {
            this.IsPreviousEnabled = true;
            this.IsNextEnabled = true;
            this.Page1Hidden = true;
            this.Page2Hidden = false;
            this.Page3Hidden = true;
        }
    };
    NewChargesTypeComponent.prototype.NextButtonClicked = function () {
        this.IsFinishEnabled = true;
        if (!this.Page1Hidden) {
            this.IsNextEnabled = true;
            this.IsPreviousEnabled = true;
            this.Page1Hidden = true;
            this.Page2Hidden = false;
        }
        else if (!this.Page2Hidden) {
            this.IsPreviousEnabled = true;
            this.IsNextEnabled = false;
            this.Page1Hidden = true;
            this.Page2Hidden = true;
            this.Page3Hidden = false;
        }
        else if (!this.Page3Hidden) {
            this.IsPreviousEnabled = true;
            this.IsNextEnabled = false;
        }
    };
    NewChargesTypeComponent.prototype.FinishButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.EntityPM.IsAutoDisplayInQuote || this.EntityPM.IsAutoDisplayInShipment || this.EntityPM.IsAutoDisplayInConsolidation || this.EntityPM.IsAutoDisplayInCustoms) {
            if (!this.EntityPM.IsExport && !this.EntityPM.IsImport && !this.EntityPM.IsDomestic && !this.EntityPM.IsDrop) {
                errors.push("Please select at least one direction (export, import, domestic or drop)");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.EntityPM.IsReceivable = true;
            this.EntityPM.IsPayable = true;
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new ChargesTypePMService_1.ChargesTypePMService();
            myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    NewChargesTypeComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewChargesTypeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewChargesTypeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewChargesTypeComponent);
    return NewChargesTypeComponent;
}(BaseComponent_1.BaseComponent));
exports.NewChargesTypeComponent = NewChargesTypeComponent;
//# sourceMappingURL=NewChargesTypeComponent.js.map