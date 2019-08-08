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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var CustomerTenantAccessPMService_1 = require("../../Common/Services/StandardPMs/CustomerTenantAccessPMService");
var CustomerPMService_1 = require("../../Common/Services/StandardPMs/CustomerPMService");
var AddEditCustomerTenantAccessCardComponent = /** @class */ (function (_super) {
    __extends(AddEditCustomerTenantAccessCardComponent, _super);
    function AddEditCustomerTenantAccessCardComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditCustomerTenantAccessCardComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCustomerTenantAccessCardComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var checkIfCustomerSelected = this.viewModel.CardObsList.filter(function (a) { return a.IsSelectedSubmited == true; })[0];
        if (checkIfCustomerSelected == null) {
            this.ValidationErrorsList.push("Please Select Customer");
        }
        else {
            this.viewModel.EntityPM.CustomerId = checkIfCustomerSelected.Id;
            this.viewModel.EntityPM.CustomerCode = checkIfCustomerSelected.Code;
            this.viewModel.EntityPM.CustomerName = checkIfCustomerSelected.EnglishName;
            Validator_1.Validator.TryValidateObject(this.viewModel.EntityPM, "CustomerTenantAccessCard", this.ValidationErrorsList);
            if (this.ValidationErrorsList.length == 0) {
                var window = new ConfirmWindow_1.ConfirmWindow();
                window.Title = "Confirm build shipments";
                window.Width = 450;
                window.Height = 190;
                window.YesButtonText = "Yes";
                window.NoButtonText = "No";
                window.Show("Do you want to build Shipments & Documents from " + this.viewModel.HybridStartDate.toDateString() + " ?");
                window.WindowClosed.subscribe(function (event) {
                    if (window.Yes) {
                        _this.viewModel.EntityPM.BuildBatch = true;
                        _this.viewModel.StatusTypeCode = "IP";
                        _this.viewModel.StatusType = "In Progress";
                    }
                    else if (window.No) {
                        _this.viewModel.StatusTypeCode = "A";
                        _this.viewModel.StatusType = "Accepted";
                    }
                    _this.CompleteConfirmation(checkIfCustomerSelected);
                });
            }
        }
    };
    AddEditCustomerTenantAccessCardComponent.prototype.CompleteConfirmation = function (checkIfCustomerSelected) {
        var _this = this;
        if (this.viewModel.isNew) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.viewModel.isNew = false;
            var service = new CustomerPMService_1.CustomerPMService();
            service.get(this.viewModel.CustomerId).subscribe(function (res) {
                if (!res.HasError) {
                    var Customer = res.Result;
                    if (Customer != null) {
                        _this.viewModel.CustomerCode = Customer.Code;
                        _this.viewModel.CustomerName = Customer.EnglishName;
                        _this.viewModel.StatusType = _this.viewModel.EntityPM.StatusType;
                        _this.viewModel.CreateByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                        if (!_this.viewModel.Parent.ObsList.includes(_this.viewModel)) {
                            _this.viewModel.Parent.ObsList.push(_this.viewModel);
                            _this.viewModel.Parent.EntityPM.AddCustomerTenantAccessCardPM(_this.viewModel.EntityPM);
                        }
                        if (!_this.viewModel.Parent.RealCustomerTenantAccessPM.CustomerTenantAccessCards.includes(_this.viewModel.EntityPM)) {
                            _this.viewModel.Parent.RealCustomerTenantAccessPM.CustomerTenantAccessCards.push(_this.viewModel.EntityPM);
                            //this.viewModel.Parent.RealCustomerTenantAccessPM.AddCustomerTenantAccessCardPM(this.viewModel.EntityPM);
                        }
                        if (_this.viewModel.Parent.ObsList.filter(function (a) { return a.StatusType == "In Progress"; })[0]) {
                            _this.viewModel.customertenantAccessPM.Status = "IP";
                            _this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "IP";
                            _this.viewModel.Parent.EntityPM.Status = "IP";
                        }
                        else {
                            if (_this.viewModel.Parent.ObsList.filter(function (a) { return a.StatusType == "Accepted"; })[0]) {
                                _this.viewModel.customertenantAccessPM.Status = "A";
                                _this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "A";
                                _this.viewModel.Parent.EntityPM.Status = "A";
                            }
                            else if (_this.viewModel.Parent.ObsList.filter(function (a) { return a.StatusType != "Accepted"; })[0] && _this.viewModel.Parent.ObsList.filter(function (a) { return a.StatusType != "In Active"; })[0]) {
                                _this.viewModel.customertenantAccessPM.Status = "W";
                                _this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "W";
                                _this.viewModel.Parent.EntityPM.Status = "W";
                            }
                            else {
                                _this.viewModel.customertenantAccessPM.Status = "IA";
                                _this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "IA";
                                _this.viewModel.Parent.EntityPM.Status = "IA";
                            }
                        }
                        var service = new CustomerTenantAccessPMService_1.CustomerTenantAccessPMService();
                        service.update(_this.viewModel.Parent.EntityPM).subscribe(function (p) {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.viewModel.Parent.IsShowTipArea = false;
                            if (_this.viewModel.Parent.SelectedItem == null && _this.viewModel.Parent.ObsList.length > 0) {
                                _this.viewModel.Parent.SelectedItem = _this.viewModel.Parent.ObsList[0];
                            }
                            _this.CurrentSession.CloseCurrentWindow();
                        });
                    }
                }
            });
        }
    };
    AddEditCustomerTenantAccessCardComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.viewModel = args;
            this.DataLoaded = true;
        }
    };
    AddEditCustomerTenantAccessCardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'RelatedCustomerComponent',
            templateUrl: './AddEditCustomerTenantAccessCardComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditCustomerTenantAccessCardComponent);
    return AddEditCustomerTenantAccessCardComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomerTenantAccessCardComponent = AddEditCustomerTenantAccessCardComponent;
//# sourceMappingURL=AddEditCustomerTenantAccessCardComponent.js.map