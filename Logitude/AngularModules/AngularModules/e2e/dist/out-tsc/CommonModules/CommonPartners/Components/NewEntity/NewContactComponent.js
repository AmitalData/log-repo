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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ContactPM_1 = require("../../../../Common/EntityPMs/ContactPM");
var ContactPMService_1 = require("../../../../Common/Services/StandardPMs/ContactPMService");
var ContactInputTemplate_1 = require("../Templates/ContactInputTemplate");
var NewContactComponent = /** @class */ (function () {
    function NewContactComponent() {
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CustomerId = null;
        this.CardDependencyProperty1 = null;
        this.CustomerLable = null;
        this.CardDependencyProperty1IsList = false;
        this.ComponentName = null;
        this.Retries = 0;
        this.EntityPM = new ContactPM_1.ContactPM();
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.myService = new ContactPMService_1.ContactPMService();
        this.RunComponent();
    }
    NewContactComponent.prototype.SetWindowArgs = function (args) {
        this.CustomerId = args.CustomerId;
        this.CardDependencyProperty1 = args.CardDependencyProperty1;
        this.CustomerLable = args.CustomerLable;
        this.CardDependencyProperty1IsList = args.CardDependencyProperty1IsList;
        this.ComponentName = args.ComponentName;
    };
    NewContactComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewContactComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewContactComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/ContactInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.ContactTemplate = cmpRef.instance;
            var args = new ContactInputTemplate_1.ContactInputTemplateArgs();
            args.IsNewEntity = true;
            args.EntityPM = _this.EntityPM;
            args.IsCustomerVisible = true;
            args.CustomerId = _this.CustomerId;
            args.CardDependencyProperty1 = _this.CardDependencyProperty1;
            args.CustomerLable = _this.CustomerLable;
            args.ComponentName = _this.ComponentName;
            _this.ContactTemplate.InitTemplate(args);
        });
    };
    NewContactComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    };
    NewContactComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = this.ContactTemplate.Validate();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
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
    ], NewContactComponent.prototype, "viewContainerRef", void 0);
    NewContactComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewContactComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewContactComponent);
    return NewContactComponent;
}());
exports.NewContactComponent = NewContactComponent;
//# sourceMappingURL=NewContactComponent.js.map