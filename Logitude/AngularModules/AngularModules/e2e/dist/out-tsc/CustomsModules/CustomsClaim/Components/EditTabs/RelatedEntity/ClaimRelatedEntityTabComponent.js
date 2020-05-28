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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LocationDirective_1 = require("../../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var ClaimPMService_1 = require("../../../../../Customs/Services/StandardPMs/ClaimPMService");
var ObjectsLocator_1 = require("../../../../../Infrastructure/Locators/ObjectsLocator");
var ClaimRelatedEntityTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityTabComponent, _super);
    function ClaimRelatedEntityTabComponent(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.LayoutDirection = 'ltr';
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Claim";
        _this.ValidationErrorsList = [];
        _this.TabsItemsSource = [];
        _this.IsDisplayOnly = false;
        _this.IsNewEntity = false;
        // services
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ClaimPMService = new ClaimPMService_1.ClaimPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.isViewInited = false;
        _this.FileData = null;
        _this.ExportDeclaration = null;
        _this.ReasonsAndExplanitaions = null;
        _this.CustomAnswer = null;
        //#region Save Code
        _this.loadingNextItems = false;
        _this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        return _this;
    }
    ClaimRelatedEntityTabComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.EntityCounterKey = args.EntityCounterKey;
            this.ClaimPM = args.ClaimPM;
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.IsNewEntity = args.IsNewEntity;
            this.WindowTitle = args.WindowTitle;
        }
        this.entityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesAmount").subscribe(function (response) {
                    _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntsReasonsExp").subscribe(function (response) {
                        _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesReason").subscribe(function (response) {
                            _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntsExpDeclar").subscribe(function (response) {
                                _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesSeizure").subscribe(function (response) {
                                    _this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesRefund").subscribe(function (response) {
                                        _this.BuildTabs();
                                        _this.RunComponent();
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    };
    ClaimRelatedEntityTabComponent.prototype.BuildTabs = function () {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("FileData", "Customs.Claim.O.FileData"));
        this.TabsItemsSource.push(new TabItem("ReasonsAndExplanitaions", "Customs.Claim.TH.ReasonsAndExplanitaions"));
        this.TabsItemsSource.push(new TabItem("ExportDeclaration", "Customs.Claim.O.ClaimsRelatedEntityAdditional"));
        this.TabsItemsSource.push(new TabItem("ClaimDecision", "Customs.Claim.TH.ClaimDecision"));
        this.TabsItemsSource.push(new TabItem("CustomAnswer", "Customs.Claim.TH.CustomAnswer"));
        this.selectedTabCode = "FileData";
    };
    ClaimRelatedEntityTabComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    ClaimRelatedEntityTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ClaimRelatedEntityTabComponent.prototype.InitializeComponent = function () {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    };
    Object.defineProperty(ClaimRelatedEntityTabComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityTabComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {
                    case "FileData": {
                        if (this.FileData == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityGeneralTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.FileData = cmpRef.instance;
                                _this.FileData.InitTab(_this.ClaimPM.ClaimsRelatedEntities.filter(function (d) { return d.EntityCounterKey == _this.EntityCounterKey; })[0], _this.ClaimPM, !_this.IsDisplayOnly);
                            });
                        }
                        break;
                    }
                    case "ReasonsAndExplanitaions": {
                        if (this.ReasonsAndExplanitaions == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityReasonsTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.ReasonsAndExplanitaions = cmpRef.instance;
                                _this.ReasonsAndExplanitaions.InitTab(_this.ClaimPM.ClaimsRelatedEntities.filter(function (d) { return d.EntityCounterKey == _this.EntityCounterKey; })[0], _this.ClaimPM, !_this.IsDisplayOnly);
                            });
                        }
                        break;
                    }
                    case "ExportDeclaration": {
                        if (this.ExportDeclaration == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityAdditionalDataTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.ExportDeclaration = cmpRef.instance;
                                _this.ExportDeclaration.InitTab(_this.ClaimPM.ClaimsRelatedEntities.filter(function (d) { return d.EntityCounterKey == _this.EntityCounterKey; })[0], _this.ClaimPM, !_this.IsDisplayOnly);
                            });
                        }
                        break;
                    }
                    case "CustomAnswer": {
                        if (this.CustomAnswer == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityCustomAnswerTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.CustomAnswer = cmpRef.instance;
                                _this.CustomAnswer.InitTab(_this.ClaimPM.ClaimsRelatedEntities.filter(function (d) { return d.EntityCounterKey == _this.EntityCounterKey; })[0], _this.ClaimPM, !_this.IsDisplayOnly);
                            });
                        }
                        break;
                    }
                    case "ClaimDecision": {
                        if (this.CustomAnswer == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityClaimDecisionTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.CustomAnswer = cmpRef.instance;
                                _this.CustomAnswer.InitTab(_this.ClaimPM.ClaimsRelatedEntities.filter(function (d) { return d.EntityCounterKey == _this.EntityCounterKey; })[0], _this.ClaimPM, !_this.IsDisplayOnly);
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    // Buttons Handlers
    ClaimRelatedEntityTabComponent.prototype.OkButtonClicked = function () {
        this.SaveButtonClicked();
    };
    ClaimRelatedEntityTabComponent.prototype.CancelButtonClicked = function () {
        this.ClaimPM.RejectChanges();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    };
    Object.defineProperty(ClaimRelatedEntityTabComponent.prototype, "SendMode", {
        get: function () { return this.sendMode; },
        set: function (value) {
            this.sendMode = value;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityTabComponent.prototype.SaveButtonClicked = function () {
        var valid = this.PreSaveValidate();
        this.closeWindow = true;
        if (valid) {
            this.SaveChanges();
        }
    };
    ClaimRelatedEntityTabComponent.prototype.SaveChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        //if (this.IsNewEntity) {
        //    this.ClaimPMService.insert(this.ClaimPM).subscribe(myResult => {
        //        var res: ServiceResponse = myResult;
        //        if (!res.HasError) {
        //            var entity = res.Result;
        //            console.log("..Saved Successfully ", entity);
        //            if (this.closeWindow) {
        //                this.CurrentSession.CloseCurrentWindow();
        //            }
        //        }
        //        else {
        //            this.ValidationErrorsList = res.ErrorsArray;
        //        }
        //        this.CurrentSession.StopBusyIndicator();
        //        return false;
        //    });
        //} else {
        //this.ClaimPMService.update(this.ClaimPM)
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (myResult) {
            var res = myResult;
            if (!res.HasError) {
                var entity = res.Result;
                console.log("..Saved Successfully ", entity);
                if (_this.closeWindow) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.CurrentSession.CloseCurrentWindowEmit('ok');
                }
            }
            else {
                _this.ValidationErrorsList = res.ErrorsArray;
            }
            _this.CurrentSession.StopBusyIndicator();
            return false;
        });
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        //}
    };
    ClaimRelatedEntityTabComponent.prototype.PreSaveValidate = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.ClaimPM, this.ObjectTableName, errors);
        var entityPM = this.ClaimPM.ClaimsRelatedEntities.filter(function (d) { return d.EntityCounterKey == _this.EntityCounterKey; })[0];
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ClaimEntityTypeCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.ClaimEntityTypeCode"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ClaimEntityNumber)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.ClaimEntityNumber"));
        }
        //if (AppTool.IsNullOrEmpty(this.EntityPM.IsFinancialRefundDemand)) {
        //    errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.IsFinancialRefundDemand"));
        //}
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ClaimExplanation)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.ClaimExplanation"));
        }
        if (entityPM.IsFinancialRefundDemand == true) {
            if (entityPM.ClaimsRelatedEntitiesAmounts != null && entityPM.ClaimsRelatedEntitiesAmounts.length > 0) {
                for (var _i = 0, _a = entityPM.ClaimsRelatedEntitiesAmounts; _i < _a.length; _i++) {
                    var amountItem = _a[_i];
                    if (Tools_1.AppTool.IsNullOrEmpty(amountItem.PaymentTypeCode)) {
                        errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntitiesAmount.F.PaymentTypeCode"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(amountItem.Amount)) {
                        errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntitiesAmount.F.Amount"));
                    }
                }
            }
        }
        if (entityPM.ClaimsRelatedEntitiesReasons != null && entityPM.ClaimsRelatedEntitiesReasons.length > 0) {
            for (var _b = 0, _c = entityPM.ClaimsRelatedEntitiesReasons; _b < _c.length; _b++) {
                var reasonItem = _c[_b];
                if (Tools_1.AppTool.IsNullOrEmpty(reasonItem.ReasonListTypeCode)) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntitiesReason.F.ReasonListTypeCode"));
                }
            }
        }
        if (errors.length == 0) {
            return true;
        }
        else {
            this.ValidationErrorsList = errors;
            return false;
        }
    };
    ClaimRelatedEntityTabComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    //#endregion
    ClaimRelatedEntityTabComponent.prototype.LogMe = function (mess) {
        var alertIt = false;
        if (alertIt) {
            alert(mess);
        }
        else {
            console.log(mess);
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], ClaimRelatedEntityTabComponent.prototype, "AllLocations", void 0);
    ClaimRelatedEntityTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntityTabComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ClaimRelatedEntityTabComponent);
    return ClaimRelatedEntityTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityTabComponent = ClaimRelatedEntityTabComponent;
var TabItem = /** @class */ (function () {
    function TabItem(Code, TextCode) {
        this.code = Code;
        this.textCode = TextCode;
    }
    return TabItem;
}());
//# sourceMappingURL=ClaimRelatedEntityTabComponent.js.map