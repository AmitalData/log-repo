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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Args_1 = require("../../../../Infrastructure/Args");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClaimPM_1 = require("../../../../Customs/EntityPMs/ClaimPM");
var ClaimPMService_1 = require("../../../../Customs/Services/StandardPMs/ClaimPMService");
var ClaimWebService_1 = require("../../../../Customs/Services/WebServices/ClaimWebService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewClaimComponent = /** @class */ (function (_super) {
    __extends(NewClaimComponent, _super);
    function NewClaimComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Claim";
        _this.ValidationErrorsList = [];
        _this.QueryNameText = "";
        _this._ClaimPMService = new ClaimPMService_1.ClaimPMService();
        _this._ClaimWebService = new ClaimWebService_1.ClaimWebService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new ClaimPM_1.ClaimPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) { });
        return _this;
    }
    NewClaimComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.QueryNameText = Tools_1.AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator_1.TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    };
    NewClaimComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewClaimComponent.prototype, "ClaimOfficeCode", {
        //#region Properties
        get: function () { return this.EntityPM.CustomsBranchCode; },
        set: function (value) {
            if (this.EntityPM.CustomsBranchCode != value) {
                this.EntityPM.CustomsBranchCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClaimComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (value) {
            if (this.EntityPM.CustomerId != value) {
                this.EntityPM.CustomerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewClaimComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ClientIsMandatory"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ClaimOfficeCode)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationOfficeCodeMandatory"));
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this._ClaimWebService.CheckIfCorporationNameExists(this.EntityPM.Tenant)
            .subscribe(function (myResponse) {
            _this.SubmitChanges(myResponse, true);
        });
    };
    NewClaimComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewClaimComponent.prototype.SubmitChanges = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            var exists = myResponse.Result;
            if (!exists) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claims.O.CorporationNameNotExists"));
                return;
            }
        }
        this._ClaimPMService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: _this.ObjectTableName, BackButtonLabel: _this.QueryNameText });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.CancelButtonClicked();
                    });
                });
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewClaimComponent.prototype.AddCustomerClicked = function () {
        var _this = this;
        var args = new Args_1.NewEntityArgs();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.CustomerId = s;
            }
        });
    };
    NewClaimComponent = __decorate([
        core_1.Component({
            selector: 'NewClaimComponent',
            moduleId: module.id,
            templateUrl: './NewClaimComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewClaimComponent);
    return NewClaimComponent;
}(BaseComponent_1.BaseComponent));
exports.NewClaimComponent = NewClaimComponent;
//# sourceMappingURL=NewClaimComponent.js.map