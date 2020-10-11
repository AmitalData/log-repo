var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ChargesTypePM } from '../../../EntityPMs/ChargesTypePM';
import { ChargesTypePMService } from '../../../../Common/Services/StandardPMs/ChargesTypePMService';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ChargesGroupListService } from '../../../../Infrastructure/Services/StandardLists/ChargesGroupListService';
export var NewChargesTypeComponent = (function (_super) {
    __extends(NewChargesTypeComponent, _super);
    function NewChargesTypeComponent() {
        _super.call(this);
        this.DataContext = this;
        this.ObjectTableName = "ChargesType";
        this.CustomsFieldsIsVisible = false;
        // Pages Properties
        this.Page1Hidden = false;
        this.Page2Hidden = true;
        this.Page3Hidden = true;
        this.IsPreviousEnabled = false;
        this.IsNextEnabled = true;
        this.IsFinishEnabled = false;
        this.EntityPM = new ChargesTypePM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.AddedManually = true;
        this.IsAir = true;
        this.IsInland = true;
        this.IsOcean = true;
        this.AWBPrintDescription = true;
        this.ViewOrder = 100;
        this.SetUIProperties();
    }
    NewChargesTypeComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("MeasurementId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MeasurementId));
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, this.IsAir);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, this.IsAir);
        this.UIProperties.SetEnabled("AWBPrintDescription", this.ObjectTableName, this.IsAir);
        if (SessionLocator.CustomsInterfaceSettingPM != null) {
            if (SessionLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
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
                if (!AppTool.IsNullOrEmpty(newValue)) {
                    var myService = new ChargesGroupListService();
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
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.EntityPM.IsReceivable = true;
            this.EntityPM.IsPayable = true;
            SessionLocator.SelectedSession.StartBusyIndicatorSaving();
            var myService = new ChargesTypePMService();
            myService.insert(this.EntityPM).subscribe(function (myResponse) {
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                    SessionLocator.SelectedSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    NewChargesTypeComponent.prototype.CancelButtonClicked = function () {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    NewChargesTypeComponent.decorators = [
        { type: Component, args: [{
                    selector: 'NewChargesTypeComponent',
                    moduleId: module.id,
                    templateUrl: './NewChargesTypeComponent.html',
                },] },
    ];
    /** @nocollapse */
    NewChargesTypeComponent.ctorParameters = [];
    return NewChargesTypeComponent;
}(BaseComponent));
//# sourceMappingURL=NewChargesTypeComponent.js.map