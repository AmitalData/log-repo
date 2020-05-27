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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var CustomerProductExtendedService_1 = require("../../Common/Services/ExtendedPMs/CustomerProductExtendedService");
var ViewBlocedCustomerComponent = /** @class */ (function () {
    function ViewBlocedCustomerComponent(_customerProductExtendedService) {
        this._customerProductExtendedService = _customerProductExtendedService;
        this.StatusCodeColor = "#E483FB";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
    }
    ViewBlocedCustomerComponent.prototype.ngOnInit = function () {
        this.Run();
    };
    ViewBlocedCustomerComponent.prototype.Run = function () {
        if (this.entityPM) {
            this.Code = this.entityPM.Code;
            this.Name = this.entityPM.EnglishName;
            this.VAT = this.entityPM.VatNumber;
            this.Status = this.entityPM.CustomerStatusName;
            this.StatusCode = this.entityPM.CustomerStatusCode;
            this.Salesman = this.entityPM.SalesmanUserEnglishName;
            this.StartWorkingDate = this.entityPM.StartWorkingDate;
            this.LastShipmentDate = this.entityPM.LastShipmentDate;
            this.LastInteractionDate = this.entityPM.LastInteractionDate;
            if (this.StatusCode != null) {
                if (this.StatusCode == "POT") {
                    this.StatusCodeColor = "#E36C0A";
                }
                else if (this.StatusCode == "ACT") {
                    this.StatusCodeColor = "#00B076";
                }
                else if (this.StatusCode == "WAC") {
                    this.StatusCodeColor = "#FF0000";
                }
                else if (this.StatusCode == "INA") {
                    this.StatusCodeColor = "#F40CB2";
                }
            }
            this.LoadData();
        }
    };
    ViewBlocedCustomerComponent.prototype.LoadData = function () {
        var _this = this;
        this.ProductsObslist = [];
        this._customerProductExtendedService.GetCustomerProducts(this.entityPM.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        _this.ProductsObslist.push(new ProductObslistItemClass(item));
                    });
                    _this.NoProductsVisibility = _this.ProductsObslist.length == 0 ? true : false;
                }
            }
        });
    };
    ViewBlocedCustomerComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ViewBlocedCustomerComponent.prototype.SetWindowArgs = function (args) {
        this.entityPM = args.CustomerPM;
    };
    ViewBlocedCustomerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ViewBlocedCustomerComponent',
            templateUrl: './ViewBlocedCustomerComponent.html',
            providers: [CustomerProductExtendedService_1.CustomerProductExtendedService],
        }),
        __metadata("design:paramtypes", [CustomerProductExtendedService_1.CustomerProductExtendedService])
    ], ViewBlocedCustomerComponent);
    return ViewBlocedCustomerComponent;
}());
exports.ViewBlocedCustomerComponent = ViewBlocedCustomerComponent;
var ProductObslistItemClass = /** @class */ (function () {
    function ProductObslistItemClass(item) {
        this.Name = item.ProductTypeName;
        this.LastShipmentDate = item.LastShipmentDate;
        this.entityPM = item;
    }
    return ProductObslistItemClass;
}());
//# sourceMappingURL=ViewBlocedCustomerComponent.js.map