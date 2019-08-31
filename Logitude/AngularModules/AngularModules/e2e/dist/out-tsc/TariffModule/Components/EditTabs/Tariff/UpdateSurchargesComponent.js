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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var TariffDomainService_1 = require("../../../Services/TariffDomainService");
var UpdateSurchargesComponent = /** @class */ (function (_super) {
    __extends(UpdateSurchargesComponent, _super);
    function UpdateSurchargesComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tariff";
        _this.Logs = [];
        _this.ValidationErrorsList = [];
        _this.SearchText = "";
        _this.SearchAreaButtonId = "SearchAreaButtonId";
        _this.FromObsList = [];
        _this.ToObsList = [];
        _this.isUpdateDone = false;
        return _this;
    }
    UpdateSurchargesComponent.prototype.SetWindowArgs = function (arg) {
        this.EntityPM = arg.Version;
        this.FillTariffCharges(arg.TariffCharges);
        this.LoadAirlineAreas(arg.AirlineId);
    };
    UpdateSurchargesComponent.prototype.LoadAirlineAreas = function (airlineId) {
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetAirlineAreas(airlineId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                //this.AirlineAreas = myResponse.Result;
            }
        });
    };
    UpdateSurchargesComponent.prototype.FillTariffCharges = function (myList) {
        var _this = this;
        this.TariffChargesObsList = [];
        myList.sort(function (p) { return p.Code_Int; }).forEach(function (item) {
            _this.TariffChargesObsList.push(new TariffCharge(item));
        });
    };
    Object.defineProperty(UpdateSurchargesComponent.prototype, "StartDate", {
        get: function () {
            return this.startDate;
        },
        set: function (value) {
            if (this.startDate != value) {
                this.startDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    UpdateSurchargesComponent.prototype.setToggleButtonMenuTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchAreaButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    UpdateSurchargesComponent.prototype.setToggleButtonMenu = function () {
        var ToggleBTN = document.getElementById(this.SearchAreaButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    UpdateSurchargesComponent.prototype.AddArea = function (item, i, type) {
        if (i === void 0) { i = null; }
        if (type === void 0) { type = null; }
        if (item.IsChecked == true) {
            if (type == "From") {
                if (!this.FromObsList.filter(function (d) { return d.Indication == "Area" && d.DisplayText == item.Name; })) {
                }
            }
        }
    };
    UpdateSurchargesComponent.prototype.AddPort = function (type) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;
        var itemComponent = new DestinationClass(this, type, null);
        logWindow.DataContext = itemComponent;
        logWindow.Title = "Add " + type + " Port";
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/ChoosePortComponent');
    };
    UpdateSurchargesComponent.prototype.DeleteDestination = function (item, type) {
        if (type == "From") {
            var index = this.FromObsList.indexOf(item);
            if (index > -1) {
                this.FromObsList.splice(index);
            }
        }
        else {
            var index = this.ToObsList.indexOf(item);
            if (index > -1) {
                this.ToObsList.splice(index);
            }
        }
    };
    UpdateSurchargesComponent.prototype.UpdateButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (this.FromObsList.length == 0) {
            errors.push("You have to choose from ports/areas");
        }
        if (this.ToObsList.length == 0) {
            errors.push("You have to choose to ports/areas");
        }
        if (this.StartDate == null) {
            errors.push("Start date is required");
        }
        if (this.TariffChargesObsList.filter(function (d) { return d.IsChargeChecked; }).length == 0) {
            errors.push("No surcharges updated");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var args = new TariffDomainService_1.UpdateSurchargeArgs();
            args.TariffId = this.EntityPM.TariffId;
            args.VersionNumber = this.EntityPM.Version;
            args.StartDate = this.StartDate;
            this.FromObsList.forEach(function (item) {
                args.From.push(item.Indication + "," + item.Id);
            });
            this.ToObsList.forEach(function (item) {
                args.To.push(item.Indication + "," + item.Id);
            });
            this.TariffChargesObsList.filter(function (d) { return d.IsChargeChecked; }).forEach(function (item) {
                args.Surcharge.push(item.ChargeId + "," + item.NewPrice + "," + item.Index);
            });
            var myService = new TariffDomainService_1.TariffDomainService();
            myService.PostUpdateSurcharge(args).subscribe(function (response) {
                if (!response.HasError) {
                    _this.isUpdateDone = true;
                }
                else {
                    _this.ValidationErrorsList = response.ErrorsArray;
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    UpdateSurchargesComponent.prototype.CloseButtonClicked = function () {
        if (this.isUpdateDone) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
            this.isUpdateDone = true;
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    UpdateSurchargesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UpdateSurchargesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], UpdateSurchargesComponent);
    return UpdateSurchargesComponent;
}(BaseComponent_1.BaseComponent));
exports.UpdateSurchargesComponent = UpdateSurchargesComponent;
var TariffCharge = /** @class */ (function (_super) {
    __extends(TariffCharge, _super);
    function TariffCharge(charge) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ChargeId = charge.Code;
        _this.ChargeCode = charge.Name;
        _this.DisplayText = charge.DisplyText;
        _this.Index = charge.Code_Int;
        _this.SetUIProperties();
        return _this;
    }
    TariffCharge.prototype.SetUIProperties = function () {
        var newPriceEnabled = false;
        if (this.IsChargeChecked) {
            newPriceEnabled = true;
        }
        this.UIProperties.SetEnabled("NewPrice", null, newPriceEnabled);
    };
    Object.defineProperty(TariffCharge.prototype, "IsChargeChecked", {
        get: function () {
            return this.isChargeChecked;
        },
        set: function (value) {
            if (this.isChargeChecked != value) {
                this.isChargeChecked = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffCharge.prototype, "NewPrice", {
        get: function () {
            return this.newPrice;
        },
        set: function (value) {
            if (this.newPrice != value) {
                this.newPrice = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return TariffCharge;
}(BaseComponent_1.BaseComponent));
exports.TariffCharge = TariffCharge;
var DestinationClass = /** @class */ (function (_super) {
    __extends(DestinationClass, _super);
    function DestinationClass(fatherComponent, type, Port) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.Type = type;
        if (Port != null) {
            _this.Indication = "Port";
            _this.DisplayText = Port.EnglishName;
            _this.Code = Port.Code;
            _this.Id = Port.Id;
        }
        return _this;
    }
    return DestinationClass;
}(BaseComponent_1.BaseComponent));
exports.DestinationClass = DestinationClass;
var AirlineAreaClass = /** @class */ (function () {
    function AirlineAreaClass(itemList, Parent, productTypeList) {
        this.Parent = Parent;
        this.productTypeList = productTypeList;
        this.ProductTypesByTenantList = [];
        this.ProductTypesByTenantList = productTypeList;
        this.entityList = itemList;
        //var isChecked = null;
        //var productPM = this.entityPM.CustomerProducts.filter(d => d.ProductTypeCode == this.entityList.Code)[0];
        //this.isChecked = false;
        //if (productPM != null) {
        //    isChecked = true;
        //    this.IsChecked = true;
        //}
    }
    Object.defineProperty(AirlineAreaClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineAreaClass.prototype, "Foreground", {
        get: function () { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineAreaClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                //if (value) {
                //    var newItem: CustomerProductPM = new CustomerProductPM(null);
                //    newItem.Tenant = this.TenantPM.Id;
                //    newItem.CustomerId = this.entityPM.Id;
                //    newItem.ProductTypeCode = this.Code;
                //    newItem.CommitmentChargeableWeight = 0;
                //    newItem.PotentialChargeableWeight = 0;
                //    newItem.CommitmentTEU = 0;
                //    newItem.PotentialTEU = 0;
                //    newItem.CommitmentNumberOfShipments = 0;
                //    newItem.PotentialNumberOfShipments = 0;
                //    newItem.CommitmentRevenue = 0;
                //    newItem.PotentialRevenue = 0;
                //    var type: string = null;
                //    var ProductsToggleButtonList = [];
                //    this.ProductTypesByTenantList.forEach((i) => {
                //        if (!i.InActive) {
                //            var item = new ProductTypeList();
                //            item.Code = i.Code;
                //            item.Name = i.Name;
                //            item.InActive = i.InActive;
                //            item.Id = i.Id;
                //            item.SearchFields = i.SearchFields;
                //            ProductsToggleButtonList.push(i);
                //        }
                //        else {
                //        }
                //    });
                //    ProductsToggleButtonList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
                //    var typelist: ProductTypeList = ProductsToggleButtonList.filter(d => d.Code == this.Code)[0];
                //    if (typelist != null) {
                //        type = typelist.Name;
                //    }
                //    newItem.ProductTypeName = type;
                //    var flag: boolean = true;
                //    for (var i = 0; i < this.entityPM.CustomerProducts.length; i++) {
                //        if (this.entityPM.CustomerProducts[i].ProductTypeCode == newItem.ProductTypeCode) {
                //            flag = false; break;
                //        }
                //    }
                //    if (flag) {
                //        this.entityPM.AddCustomerProductPM(newItem);
                //    }
                //    if (!this.entityPM.ActivityWatch)
                //        this.entityPM.ActivityWatch = true;
                //}
                //else {
                //    var item: CustomerProductPM = this.entityPM.CustomerProducts.filter(d => d.ProductTypeCode == this.Code)[0];
                //    if (item != null) {
                //        if (this.entityPM.CustomerProducts.includes(item)) {
                //            var CustomerProdArr: Array<CustomerProductPM> = [];
                //            this.entityPM.CustomerProducts.forEach(i => {
                //                if (i.ProductTypeCode != item.ProductTypeCode) {
                //                    CustomerProdArr.push(i);
                //                }
                //            });
                //            this.entityPM.RemoveCustomerProductPM(this.entityPM.CustomerProducts.filter(p => p.ProductTypeCode == this.Code)[0]);
                //        }
                //    }
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    return AirlineAreaClass;
}());
exports.AirlineAreaClass = AirlineAreaClass;
//# sourceMappingURL=UpdateSurchargesComponent.js.map