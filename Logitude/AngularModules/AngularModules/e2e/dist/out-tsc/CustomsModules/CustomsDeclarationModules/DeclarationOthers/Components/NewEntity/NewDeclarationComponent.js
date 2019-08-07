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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Args_1 = require("../../../../../Infrastructure/Args");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationPM_1 = require("../../../../../Customs/EntityPMs/DeclarationPM");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var DeclarationExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var CustomsHouseTypeExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var NewDeclarationComponent = /** @class */ (function (_super) {
    __extends(NewDeclarationComponent, _super);
    function NewDeclarationComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.ValidationErrorsList = [];
        _this.QueryNameText = "";
        _this.IsTransportModeMatch = true;
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        _this.declarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.customsHouseTypeExtendedPMService = new CustomsHouseTypeExtendedPMService_1.CustomsHouseTypeExtendedPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new DeclarationPM_1.DeclarationPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsTransportMode").subscribe(function (response) { });
        return _this;
    }
    NewDeclarationComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.QueryNameText = Tools_1.AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator_1.TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    };
    NewDeclarationComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewDeclarationComponent.prototype, "CustomFileNo", {
        //#region Properties
        get: function () { return this.EntityPM.CustomFileNo; },
        set: function (value) {
            if (this.EntityPM.CustomFileNo != value) {
                this.EntityPM.CustomFileNo = value;
                console.log("check number ...");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewDeclarationComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (value) {
            if (this.EntityPM.CustomerId != value) {
                this.EntityPM.CustomerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewDeclarationComponent.prototype, "DeclarationOfficeCode", {
        get: function () { return this.EntityPM.DeclarationOfficeCode; },
        set: function (value) {
            if (this.EntityPM.DeclarationOfficeCode != value) {
                this.EntityPM.DeclarationOfficeCode = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.setHouseTypeForDeclaration();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewDeclarationComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (value) {
            if (this.EntityPM.TransportModeId != value) {
                this.EntityPM.TransportModeId = value;
                this.IsTransportModeMatch = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(value) && !Tools_1.AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
                    this.checkMatchTransportMode();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewDeclarationComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = [];
        // Validation: Required and match transport
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ClientIsMandatory"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeclarationOfficeCode)) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationOfficeCodeMandatory"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TransportModeId)) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.TransportModeIdMandatory"));
        }
        if (!this.IsTransportModeMatch) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Match"));
        }
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            return;
        }
        // Exist check
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.SubmitChanges();
        }
        else {
            this.CustomFileNo = this.CustomFileNo.replace(/\s/g, '');
            this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo).subscribe(function (myResponse) {
                if (!Tools_1.AppTool.IsNullOrEmpty(myResponse)) {
                    var entity = myResponse.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
                        // exist
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CustomsFileNoExists"));
                        _this.ValidationErrorsList = errors;
                    }
                    else {
                        _this.SubmitChanges();
                    }
                }
            });
        }
    };
    NewDeclarationComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewDeclarationComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.declarationPMService.insert(this.EntityPM).subscribe(function (myResult) {
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
    NewDeclarationComponent.prototype.AddCustomerClicked = function () {
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
                //console.log(s);
                _this.CustomerId = s;
            }
        });
    };
    // Server Requests
    NewDeclarationComponent.prototype.checkMatchTransportMode = function () {
        var _this = this;
        this.customsHouseTypeExtendedPMService.GetHouseTypewithAdditional(this.DeclarationOfficeCode).subscribe(function (result) {
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                var houseType = result.Result;
                console.log("-- houseType: ", houseType);
                if (!Tools_1.AppTool.IsNullOrEmpty(houseType)) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(houseType.TransportModeId)) {
                        if (houseType.TransportModeId != _this.TransportModeId) {
                            _this.IsTransportModeMatch = false;
                        }
                    }
                }
            }
        });
    };
    NewDeclarationComponent.prototype.setHouseTypeForDeclaration = function () {
        var _this = this;
        this.customsHouseTypeExtendedPMService.GetHouseTypewithAdditional(this.DeclarationOfficeCode).subscribe(function (result) {
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                var houseType = result.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(houseType)) {
                    _this.TransportModeId = houseType.TransportModeId;
                    console.log("-- Transport Mode set to: ", houseType.TransportModeId);
                }
            }
        });
    };
    NewDeclarationComponent = __decorate([
        core_1.Component({
            selector: 'NewDeclarationComponent',
            moduleId: module.id,
            templateUrl: './NewDeclarationComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewDeclarationComponent);
    return NewDeclarationComponent;
}(BaseComponent_1.BaseComponent));
exports.NewDeclarationComponent = NewDeclarationComponent;
//# sourceMappingURL=NewDeclarationComponent.js.map