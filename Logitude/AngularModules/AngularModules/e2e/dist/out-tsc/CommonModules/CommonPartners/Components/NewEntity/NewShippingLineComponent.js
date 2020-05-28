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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ShippingLinePM_1 = require("../../../../Common/EntityPMs/ShippingLinePM");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var ShippingLinePMService_1 = require("../../../../Common/Services/StandardPMs/ShippingLinePMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ShippingLineListService_1 = require("../../../../Common/Services/StandardLists/ShippingLineListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewShippingLineComponent = /** @class */ (function (_super) {
    __extends(NewShippingLineComponent, _super);
    function NewShippingLineComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "ShippingLine";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.ShippingLinePM = new ShippingLinePM_1.ShippingLinePM();
        _this.IsNewEntityCall = true;
        _this.IsVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Properties 
        _this.zeroExistsMessageVisibility = false;
        _this.message = "";
        _this.isSaveEnabled = true;
        _this.ShippingLinesList = [];
        _this.entityResourceService.getEntityResourceByTableName("ShippingLine", 0).subscribe(function (response) {
            _this.IsVisible = true;
        });
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.Initialize();
        return _this;
    }
    NewShippingLineComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.RequestPage = args.RequestPage;
            if (this.RequestPage == "SharedManifest") {
                if (!Tools_1.AppTool.IsNullOrEmpty(args.DefaultValues)) {
                    var defaultValuesArray = args.DefaultValues.split('^');
                    if (this.ShippingLinePM) {
                        this.ShippingLinePM.Code = !Tools_1.AppTool.IsNullOrEmpty(defaultValuesArray[0]) ? defaultValuesArray[0] : "";
                        this.ShippingLinePM.EnglishName = !Tools_1.AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";
                        this.ShippingLinePM.LocalName = !Tools_1.AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";
                    }
                }
            }
        }
    };
    NewShippingLineComponent.prototype.ngOnInit = function () {
        this.ShippingLinePM.Tenant = this.TenantPM.Id;
        this.ShippingLinePM.CarrierTypeId = "SL";
        this.ShippingLinePM.TransportModeId = "O";
        this.ShippingLinePM.AddedManually = true;
    };
    NewShippingLineComponent.prototype.Initialize = function () {
        this.IsEditEnabled = false;
        this.ZeroExistsMessageVisibility = false;
        this.LoadShippingLineListMethod();
    };
    Object.defineProperty(NewShippingLineComponent.prototype, "ZeroExistsMessageVisibility", {
        get: function () { return this.zeroExistsMessageVisibility; },
        set: function (value) {
            this.zeroExistsMessageVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "IsEditEnabled", {
        get: function () { return this.isEditEnabled; },
        set: function (value) {
            this.isEditEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "Code", {
        get: function () { return this.ShippingLinePM.Code; },
        set: function (value) {
            if (this.ShippingLinePM.Code != value) {
                this.ShippingLinePM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "Message", {
        get: function () { return this.message; },
        set: function (value) {
            this.message = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "IsSaveEnabled", {
        get: function () { return this.isSaveEnabled; },
        set: function (value) {
            this.isSaveEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "SCACCode", {
        get: function () { return this.ShippingLinePM.SCACCode; },
        set: function (value) {
            if (this.ShippingLinePM.SCACCode != value) {
                this.ShippingLinePM.SCACCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "CBSA", {
        get: function () { return this.ShippingLinePM.CBSA; },
        set: function (value) {
            if (this.ShippingLinePM.CBSA != value) {
                this.ShippingLinePM.CBSA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "CAAT", {
        get: function () { return this.ShippingLinePM.CAAT; },
        set: function (value) {
            if (this.ShippingLinePM.CAAT != value) {
                this.ShippingLinePM.CAAT = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "EnglishName", {
        get: function () { return this.ShippingLinePM.EnglishName; },
        set: function (value) {
            if (this.ShippingLinePM.EnglishName != value) {
                this.ShippingLinePM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "LocalName", {
        get: function () { return this.ShippingLinePM.LocalName; },
        set: function (value) {
            if (this.ShippingLinePM.LocalName != value) {
                this.ShippingLinePM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "Website", {
        get: function () { return this.ShippingLinePM.Website; },
        set: function (value) {
            if (this.ShippingLinePM.Website != value) {
                this.ShippingLinePM.Website = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "ShippingAgentId", {
        get: function () { return this.ShippingLinePM.ShippingAgentId; },
        set: function (value) {
            if (this.ShippingLinePM.ShippingAgentId != value) {
                this.ShippingLinePM.ShippingAgentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "Remark", {
        get: function () { return this.ShippingLinePM.Remark; },
        set: function (value) {
            if (this.ShippingLinePM.Remark != value) {
                this.ShippingLinePM.Remark = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShippingLineComponent.prototype, "InActive", {
        get: function () { return this.ShippingLinePM.InActive; },
        set: function (value) {
            if (this.ShippingLinePM.InActive != value) {
                this.ShippingLinePM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //Commands
    NewShippingLineComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewShippingLineComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.ShippingLinePM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingShippingLine();
        }
    };
    NewShippingLineComponent.prototype.SubmitCreatingShippingLine = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var myService = new ShippingLinePMService_1.ShippingLinePMService();
        myService.insert(this.ShippingLinePM).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            var mm = myResult;
            if (!mm.HasError) {
                if (_this.RequestPage == "SharedManifest") {
                    _this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                }
                else {
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
            }
        });
    };
    NewShippingLineComponent.prototype.CodeLostFocusMethod = function (code) {
        this.IsEditEnabled = false;
        this.ZeroExistsMessageVisibility = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(code)) {
            if (code.length >= 1) {
                this.GetZeroObjectIfExists(code);
            }
        }
    };
    NewShippingLineComponent.prototype.GetZeroObjectIfExists = function (code) {
        var x = this.ShippingLinesList.filter(function (a) { return a.Code.toLowerCase() == code.toLowerCase(); });
        if (x.length != 0) {
            this.Message = "This shipping line already exists!";
            this.ZeroExistsMessageVisibility = true;
            this.IsSaveEnabled = false;
            return;
        }
        else {
            this.IsEditEnabled = true;
            this.IsSaveEnabled = true;
        }
        this.GetShippingLineByCode(code, 0);
    };
    NewShippingLineComponent.prototype.GetShippingLineByCode = function (code, tenant) {
        var _this = this;
        var zeroEntity;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetShippingLineByCode(code, tenant).subscribe(function (myResult) {
            zeroEntity = myResult;
            _this.IsEditEnabled = true;
            if (zeroEntity != null) {
                _this.EnglishName = zeroEntity.EnglishName;
                _this.LocalName = zeroEntity.LocalName;
                _this.SCACCode = zeroEntity.SCACCode;
                _this.Website = zeroEntity.Website;
                _this.Remark = zeroEntity.Remark;
                _this.ShippingAgentId = zeroEntity.ShippingAgentId;
                _this.Message = "This shipping line already exists in our database and on save it will be copied to your shipping lines list";
                _this.ZeroExistsMessageVisibility = true;
            }
            else {
                _this.ZeroExistsMessageVisibility = false;
            }
            _this.IsEditEnabled = true;
        });
    };
    NewShippingLineComponent.prototype.LoadShippingLineListMethod = function () {
        var _this = this;
        var myService = new ShippingLineListService_1.ShippingLineListService();
        myService.getAll().subscribe(function (myResult) {
            _this.ShippingLinesList = myResult.Result;
        });
    };
    NewShippingLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewShippingLineComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewShippingLineComponent);
    return NewShippingLineComponent;
}(BaseComponent_1.BaseComponent));
exports.NewShippingLineComponent = NewShippingLineComponent;
//# sourceMappingURL=NewShippingLineComponent.js.map