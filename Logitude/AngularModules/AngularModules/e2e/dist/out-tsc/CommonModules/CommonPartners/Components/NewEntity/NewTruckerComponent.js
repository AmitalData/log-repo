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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var TruckerPM_1 = require("../../../../Common/EntityPMs/TruckerPM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var NewTruckerComponent = /** @class */ (function () {
    function NewTruckerComponent() {
        this.EntityPM = new TruckerPM_1.TruckerPM();
        this.PartnerTypeId = "TR";
        this.ObjectTableName = "Trucker";
        this.ValidationErrorsList = [];
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.EntityPM.CarrierTypeId = this.PartnerTypeId;
        this.EntityPM.AddedManually = true;
        this.EntityPM.TransportModeId = "I";
        this.DomainService = new PartnersDomainService_1.PartnersDomainService();
        this.RunComponent();
    }
    NewTruckerComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            if (args.RequestPage == "SharedManifest") {
                this.DefaultValues = "Trucker^" + args.DefaultValues;
            }
        }
    };
    NewTruckerComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewTruckerComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewTruckerComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customer").subscribe(function (response2) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/NewPartnerTamplate", _this.viewContainerRef)
                    .then(function (cmpRef) {
                    _this.PartnerTamplate = cmpRef.instance;
                    _this.PartnerTamplate.EntityPM = _this.EntityPM;
                    _this.PartnerTamplate.DefaultValues = _this.DefaultValues; //Abed Code
                    _this.PartnerTamplate.CardTableName = _this.ObjectTableName;
                    _this.PartnerTamplate.PartnerTypeId = _this.PartnerTypeId;
                    _this.PartnerTamplate.DomainService = _this.DomainService;
                    _this.PartnerTamplate.InitTemplate();
                });
            });
        });
    };
    NewTruckerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewTruckerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = this.PartnerTamplate.Validate();
        if (errors.length == 0) {
            this.EntityPM.Code = this.PartnerTamplate.CardCode;
            this.EntityPM.EnglishName = this.PartnerTamplate.Name;
            this.EntityPM.LocalName = !Tools_1.AppTool.IsNullOrEmpty(this.PartnerTamplate.LocalName) ? this.PartnerTamplate.LocalName : this.PartnerTamplate.Name;
            this.EntityPM.VatNumber = this.PartnerTamplate.VatNumber;
            this.EntityPM.ExistedContactId = this.PartnerTamplate.ExistedContactId;
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var args = new PartnersDomainService_1.PartnerServicePM();
            args.Tenant = this.EntityPM.Tenant;
            args.PartnerTypeId = this.PartnerTypeId;
            args.Trucker = this.EntityPM;
            args.Address = this.PartnerTamplate.Address;
            if (this.PartnerTamplate.IsAddContactChecked) {
                args.Contact = this.PartnerTamplate.Contact;
            }
            this.DomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result.Trucker;
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewTruckerComponent.prototype, "viewContainerRef", void 0);
    NewTruckerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewTruckerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewTruckerComponent);
    return NewTruckerComponent;
}());
exports.NewTruckerComponent = NewTruckerComponent;
//# sourceMappingURL=NewTruckerComponent.js.map