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
var VATTypesGroupPM_1 = require("../../../EntityPMs/VATTypesGroupPM");
var VatTypeListService_1 = require("../../../Services/StandardLists/VatTypeListService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CommonDomainService_1 = require("../../../Services/CommonDomainService");
var VatTypeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(VatTypeGeneralTabComponent, _super);
    function VatTypeGeneralTabComponent(args) {
        var _this = _super.call(this) || this;
        _this.args = args;
        _this.DataContext = _this;
        _this.IsNewEntity = true;
        _this.ObjectTableName = "VatType";
        _this.ItemsSource = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsPercentagesAreaVisible = false;
        _this.allVatTypes = [];
        _this.allVatPercentages = [];
        _this.EntityPM = args.EntityPM;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
            _this.IsNewEntity = true;
            _this.NewEntityPercentageDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        else {
            _this.IsNewEntity = false;
            _this.Listen();
        }
        _this.SetUIProperties();
        _this.LoadMultiPercentages();
        return _this;
    }
    VatTypeGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildItemsSource();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildItemsSource();
                    }
                });
            }
        }
    };
    VatTypeGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    VatTypeGeneralTabComponent.prototype.SetUIProperties = function () {
        var isPercentagesAreaVisible = false;
        if (this.IsNewEntity) {
            var isNewEntityPercentageRequired = false;
            var isNewEntityPercentageDateRequired = false;
            if (SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                isPercentagesAreaVisible = true;
            }
            if (!this.IsMultiPercentage) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentage)) {
                    isNewEntityPercentageRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentageDate)) {
                    isNewEntityPercentageDateRequired = true;
                }
            }
            this.UIProperties.SetRequired("NewEntityPercentage", this.ObjectTableName, isNewEntityPercentageRequired);
            this.UIProperties.SetRequired("NewEntityPercentageDate", this.ObjectTableName, isNewEntityPercentageDateRequired);
        }
        else {
            if (this.IsMultiPercentage) {
                isPercentagesAreaVisible = true;
            }
            else {
                isPercentagesAreaVisible = false;
            }
        }
        this.IsPercentagesAreaVisible = isPercentagesAreaVisible;
    };
    VatTypeGeneralTabComponent.prototype.LoadMultiPercentages = function () {
        var _this = this;
        var myVatsService = new VatTypeListService_1.VatTypeListService();
        myVatsService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.allVatTypes = myResponse.Result;
                var myService = new CommonDomainService_1.CommonDomainService();
                myService.GetVatTypePercentagePMByDate(Tools_1.DateTool.GetCurrentDateAsUtc()).subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        _this.allVatPercentages = myResponse2.Result;
                        _this.BuildItemsSource();
                    }
                });
            }
        });
    };
    VatTypeGeneralTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.IsNewEntity) {
            this.allVatTypes.filter(function (f) { return f.IsMultiPercentage == false; }).forEach(function (item) {
                if (item.Id != _this.EntityPM.Id) {
                    var myPercentagesPM = _this.allVatPercentages.filter(function (f) { return f.VatTypeId == item.Id; })[0];
                    _this.ItemsSource.push(new MultiPercentageItem(item, myPercentagesPM, _this));
                }
            });
        }
        else {
            this.EntityPM.VatTypeGroups.forEach(function (item) {
                var myVatTypeList = _this.allVatTypes.filter(function (f) { return f.Id == item.SingleVATTypeId; })[0];
                var myPercentagesPM = _this.allVatPercentages.filter(function (f) { return f.VatTypeId == item.SingleVATTypeId; })[0];
                _this.ItemsSource.push(new MultiPercentageItem(myVatTypeList, myPercentagesPM, _this));
            });
        }
    };
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "NewEntityPercentage", {
        get: function () { return this.EntityPM.NewEntityPercentage; },
        set: function (value) {
            if (this.EntityPM.NewEntityPercentage != value) {
                this.EntityPM.NewEntityPercentage = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "NewEntityPercentageDate", {
        get: function () { return this.EntityPM.NewEntityPercentageDate; },
        set: function (value) {
            if (this.EntityPM.NewEntityPercentageDate != value) {
                this.EntityPM.NewEntityPercentageDate = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "LocalDescription", {
        get: function () { return this.EntityPM.LocalDescription; },
        set: function (value) {
            if (this.EntityPM.LocalDescription != value) {
                this.EntityPM.LocalDescription = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypeGeneralTabComponent.prototype, "IsMultiPercentage", {
        get: function () { return this.EntityPM.IsMultiPercentage; },
        set: function (value) {
            if (this.EntityPM.IsMultiPercentage != value) {
                this.EntityPM.IsMultiPercentage = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    VatTypeGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VatTypeGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], VatTypeGeneralTabComponent);
    return VatTypeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VatTypeGeneralTabComponent = VatTypeGeneralTabComponent;
var MultiPercentageItem = /** @class */ (function () {
    function MultiPercentageItem(itemList, itemPM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.isChecked = false;
        this.Name = itemList.EnglishName;
        if (itemPM) {
            this.Percentage = itemPM.Percentage;
            this.PercentageText = itemPM.Percentage + "%";
        }
        this.EntityPM = fatherComponent.EntityPM.VatTypeGroups.filter(function (f) { return f.SingleVATTypeId == itemList.Id; })[0];
        if (this.EntityPM) {
            this.isChecked = true;
        }
        else {
            this.EntityPM = new VATTypesGroupPM_1.VATTypesGroupPM(null);
            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            this.EntityPM.GroupVATTypeId = this.fatherComponent.EntityPM.Id;
            this.EntityPM.SingleVATTypeId = itemList.Id;
        }
    }
    Object.defineProperty(MultiPercentageItem.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    this.fatherComponent.EntityPM.AddVATTypesGroupPM(this.EntityPM);
                }
                else {
                    this.fatherComponent.EntityPM.RemoveVATTypesGroupPM(this.EntityPM);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return MultiPercentageItem;
}());
exports.MultiPercentageItem = MultiPercentageItem;
//# sourceMappingURL=VatTypeGeneralTabComponent.js.map