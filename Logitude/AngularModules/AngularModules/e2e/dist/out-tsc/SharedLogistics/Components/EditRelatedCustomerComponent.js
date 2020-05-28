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
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var CustomerTenantAccessPMService_1 = require("../../Common/Services/StandardPMs/CustomerTenantAccessPMService");
var CustomerTenantAccessStatusTypeListService_1 = require("../../Common/Services/StandardLists/CustomerTenantAccessStatusTypeListService");
var CommonDomainService_1 = require("../../Common/Services/CommonDomainService");
var Tools_1 = require("../../Infrastructure/Tools");
var EditRelatedCustomerComponent = /** @class */ (function (_super) {
    __extends(EditRelatedCustomerComponent, _super);
    function EditRelatedCustomerComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "CustomerTenantAccessCard";
        _this.StatusList = [];
        _this.ValidationErrorsList = [];
        _this.SelectedStatus = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    EditRelatedCustomerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(EditRelatedCustomerComponent.prototype, "StatusTypeCode", {
        get: function () {
            return this.EntityPM.StatusTypeCode;
        },
        set: function (value) {
            if (this.EntityPM.StatusTypeCode != value)
                this.EntityPM.StatusTypeCode = value;
        },
        enumerable: true,
        configurable: true
    });
    EditRelatedCustomerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new CommonDomainService_1.CommonDomainService();
        this.ValidationErrorsList = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "CustomerTenantAccessCard", this.ValidationErrorsList);
        this.EntityPM.BuildBatch = true;
        if (this.ValidationErrorsList.length == 0) {
            this.StatusTypeCode = this.StatusSelectedItem.Code;
            this.EntityPM.StatusTypeCode = this.StatusSelectedItem.Code;
            this.EntityPM.StatusType = this.StatusSelectedItem.EnglishName;
            if (this.Parent.EntityPM.CustomerTenantAccessCards.filter(function (a) { return a.StatusTypeCode == "IP"; })[0]) {
                this.Parent.EntityPM.Status = "IP";
            }
            else {
                if (this.Parent.EntityPM.CustomerTenantAccessCards.filter(function (a) { return a.StatusTypeCode == "A"; })[0]) {
                    this.Parent.EntityPM.Status = "A";
                    this.Parent.RealCustomerTenantAccessPM.Status = "A";
                }
                else if (this.Parent.EntityPM.CustomerTenantAccessCards.filter(function (a) { return a.StatusTypeCode != "A"; })[0] && this.Parent.EntityPM.CustomerTenantAccessCards.filter(function (a) { return a.StatusTypeCode != "IA"; })[0]) {
                    this.Parent.EntityPM.Status = "W";
                    this.Parent.RealCustomerTenantAccessPM.Status = "W";
                }
                else {
                    this.Parent.EntityPM.Status = "IA";
                    this.Parent.RealCustomerTenantAccessPM.Status = "IA";
                }
            }
            var updateService = new CustomerTenantAccessPMService_1.CustomerTenantAccessPMService();
            updateService.update(this.Parent.EntityPM).subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            });
        }
    };
    EditRelatedCustomerComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.EntityPM = args.EntityPM;
            this.Parent = args.Parent;
            this.fillComboBox();
        }
    };
    EditRelatedCustomerComponent.prototype.fillComboBox = function () {
        var _this = this;
        var entityService = new EntityResourceService_1.EntityResourceService();
        entityService.getEntityResourceByTableName("CustomerTenantAccessStatusType", 0).subscribe(function (p) {
            var service = new CustomerTenantAccessStatusTypeListService_1.CustomerTenantAccessStatusTypeListService();
            service.getAllFromCache().subscribe(function (res) {
                if (!res.HasError) {
                    _this.StatusList = res.Result.filter(function (d) { return d.Code == "A" || d.Code == "IA"; });
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.StatusTypeCode)) {
                        _this.StatusSelectedItem = _this.StatusList.filter(function (s) { return s.Code == _this.EntityPM.StatusTypeCode; })[0];
                    }
                }
            });
        });
    };
    EditRelatedCustomerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EditRelatedCustomerComponent',
            templateUrl: './EditRelatedCustomerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditRelatedCustomerComponent);
    return EditRelatedCustomerComponent;
}(BaseComponent_1.BaseComponent));
exports.EditRelatedCustomerComponent = EditRelatedCustomerComponent;
//# sourceMappingURL=EditRelatedCustomerComponent.js.map