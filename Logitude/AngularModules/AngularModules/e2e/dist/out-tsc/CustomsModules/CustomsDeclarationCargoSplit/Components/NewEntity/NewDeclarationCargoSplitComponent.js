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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationCargoSplitPM_1 = require("../../../../Customs/EntityPMs/DeclarationCargoSplitPM");
var DeclarationCargoSplitPMService_1 = require("../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService");
var DeclarationCargoSplitWebService_1 = require("../../../../Customs/Services/WebServices/DeclarationCargoSplitWebService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewDeclarationCargoSplitComponent = /** @class */ (function (_super) {
    __extends(NewDeclarationCargoSplitComponent, _super);
    function NewDeclarationCargoSplitComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationCargoSplit";
        _this.ValidationErrorsList = [];
        _this.QueryNameText = "";
        _this._DeclarationCargoSplitPMService = new DeclarationCargoSplitPMService_1.DeclarationCargoSplitPMService();
        _this._DeclarationCargoSplitWebService = new DeclarationCargoSplitWebService_1.DeclarationCargoSplitWebService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new DeclarationCargoSplitPM_1.DeclarationCargoSplitPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(function (response) { });
        return _this;
    }
    NewDeclarationCargoSplitComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.QueryNameText = Tools_1.AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator_1.TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    };
    NewDeclarationCargoSplitComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewDeclarationCargoSplitComponent.prototype, "ActionTypeCode", {
        //#region Properties
        get: function () { return this.EntityPM.ActionTypeCode; },
        set: function (value) {
            if (this.EntityPM.ActionTypeCode != value) {
                this.EntityPM.ActionTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewDeclarationCargoSplitComponent.prototype, "CustomFileNo", {
        get: function () { return this.EntityPM.CustomFileNo; },
        set: function (value) {
            if (this.EntityPM.CustomFileNo != value) {
                this.EntityPM.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewDeclarationCargoSplitComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.FileNumberMandatory"));
        }
        //if (AppTool.IsNullOrEmpty(this.DeclarationCargoSplitActionTypeCode)) {
        //    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O."));
        //}
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        //this._DeclarationCargoSplitWebService.CheckIfCorporationNameExists(this.EntityPM.Tenant)
        // .subscribe((myResponse: ServiceResponse) => {
        //     this.SubmitChanges(myResponse, true);
        // });
    };
    NewDeclarationCargoSplitComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewDeclarationCargoSplitComponent.prototype.SubmitChanges = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            var exists = myResponse.Result;
            if (!exists) {
                //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeclarationCargoSplits.O.CorporationNameNotExists"));
                return;
            }
        }
        this._DeclarationCargoSplitPMService.insert(this.EntityPM).subscribe(function (myResult) {
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
    NewDeclarationCargoSplitComponent = __decorate([
        core_1.Component({
            selector: 'NewDeclarationCargoSplitComponent',
            moduleId: module.id,
            templateUrl: './NewDeclarationCargoSplitComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewDeclarationCargoSplitComponent);
    return NewDeclarationCargoSplitComponent;
}(BaseComponent_1.BaseComponent));
exports.NewDeclarationCargoSplitComponent = NewDeclarationCargoSplitComponent;
//# sourceMappingURL=NewDeclarationCargoSplitComponent.js.map