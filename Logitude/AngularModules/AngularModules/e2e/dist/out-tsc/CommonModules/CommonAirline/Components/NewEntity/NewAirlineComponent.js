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
var AirlinePM_1 = require("../../../../Common/EntityPMs/AirlinePM");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var AirlinePMService_1 = require("../../../../Common/Services/StandardPMs/AirlinePMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewAirlineComponent = /** @class */ (function (_super) {
    __extends(NewAirlineComponent, _super);
    function NewAirlineComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "Airline";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.AirlinePM = new AirlinePM_1.AirlinePM();
        _this.IsNewEntityCall = true;
        _this.IsVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Properties 
        _this.zeroExistsMessageVisibility = false;
        _this.message = "";
        _this.isSaveEnabled = true;
        _this.codeHasError = false;
        _this.icaoHasError = false;
        _this.zeroCodeHasError = false;
        _this.zeroIcaoHasError = false;
        _this.AirlinesList = [];
        _this.entityResourceService.getEntityResourceByTableName("Airline", 0).subscribe(function (response) {
            _this.IsVisible = true;
        });
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.Initialize();
        return _this;
    }
    NewAirlineComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.RequestPage = args.RequestPage;
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DefaultValues)) {
                var defaultValuesArray = args.DefaultValues.split('^');
                if (this.AirlinePM) {
                    this.AirlinePM.Code = !Tools_1.AppTool.IsNullOrEmpty(defaultValuesArray[0]) ? defaultValuesArray[0] : "";
                    this.AirlinePM.EnglishName = !Tools_1.AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";
                    this.AirlinePM.LocalName = !Tools_1.AppTool.IsNullOrEmpty(defaultValuesArray[1]) ? defaultValuesArray[1] : "";
                }
            }
        }
    };
    NewAirlineComponent.prototype.ngOnInit = function () {
        this.AirlinePM.Tenant = this.TenantPM.Id;
        this.AirlinePM.CarrierTypeId = "AL";
        this.AirlinePM.TransportModeId = "A";
        this.AirlinePM.AddedManually = true;
    };
    NewAirlineComponent.prototype.Initialize = function () {
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, false);
        this.ZeroExistsMessageVisibility = false;
        this.LoadAirlineListMethod();
    };
    Object.defineProperty(NewAirlineComponent.prototype, "ZeroExistsMessageVisibility", {
        get: function () { return this.zeroExistsMessageVisibility; },
        set: function (value) {
            this.zeroExistsMessageVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "Message", {
        get: function () { return this.message; },
        set: function (value) {
            this.message = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "IsSaveEnabled", {
        get: function () { return this.isSaveEnabled; },
        set: function (value) {
            this.isSaveEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "Code", {
        get: function () { return this.AirlinePM.Code; },
        set: function (value) {
            if (this.AirlinePM.Code != value) {
                this.AirlinePM.Code = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetEnabled("InActive", "Airline", false);
                    this.UIProperties.SetRequired("Code", "Airline", true);
                }
                else {
                    this.UIProperties.SetRequired("Code", "Airline", false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "ICAO", {
        get: function () { return this.AirlinePM.ICAO; },
        set: function (value) {
            if (this.AirlinePM.ICAO != value) {
                this.AirlinePM.ICAO = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "EnglishName", {
        get: function () { return this.AirlinePM.EnglishName; },
        set: function (value) {
            if (this.AirlinePM.EnglishName != value) {
                this.AirlinePM.EnglishName = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "Prefix", {
        get: function () { return this.AirlinePM.Prefix; },
        set: function (value) {
            if (this.AirlinePM.Prefix != value) {
                this.AirlinePM.Prefix = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("Prefix", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("Prefix", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "LocalName", {
        get: function () { return this.AirlinePM.LocalName; },
        set: function (value) {
            if (this.AirlinePM.LocalName != value) {
                this.AirlinePM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "Website", {
        get: function () { return this.AirlinePM.Website; },
        set: function (value) {
            if (this.AirlinePM.Website != value) {
                this.AirlinePM.Website = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "Remark", {
        get: function () { return this.AirlinePM.Remark; },
        set: function (value) {
            if (this.AirlinePM.Remark != value) {
                this.AirlinePM.Remark = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAirlineComponent.prototype, "InActive", {
        get: function () { return this.AirlinePM.InActive; },
        set: function (value) {
            if (this.AirlinePM.InActive != value) {
                this.AirlinePM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //Commands
    NewAirlineComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewAirlineComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.AirlinePM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingAirline();
        }
    };
    NewAirlineComponent.prototype.SubmitCreatingAirline = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var myService = new AirlinePMService_1.AirlinePMService();
        myService.insert(this.AirlinePM).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (response != null) {
                if (!response.HasError) {
                    if (_this.RequestPage == "SharedManifest") {
                        _this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                }
                else {
                    _this.ValidationErrorsList = response.ErrorsArray;
                }
            }
        });
    };
    NewAirlineComponent.prototype.CodeLostFocusMethod = function () {
        this.isICAOMode = false;
        this.codeHasError = false;
        this.zeroCodeHasError = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Code) && this.Code.length == 2) {
            this.GetZeroObjectIfExists();
        }
        else {
            this.BuildMessage();
            this.SetUIPropertiesPrefix(true);
        }
    };
    NewAirlineComponent.prototype.ICAOLostFocusMethod = function () {
        this.isICAOMode = true;
        this.icaoHasError = false;
        this.zeroIcaoHasError = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ICAO) && this.ICAO.length == 3) {
            this.GetZeroObjectIfExists();
        }
        else {
            this.BuildMessage();
        }
    };
    NewAirlineComponent.prototype.GetZeroObjectIfExists = function () {
        var _this = this;
        if (this.isICAOMode) {
            var x = this.AirlinesList.filter(function (a) { return !Tools_1.AppTool.IsNullOrEmpty(a.ICAO) && a.ICAO.toLocaleLowerCase() == _this.ICAO.toLocaleLowerCase(); });
            if (x.length > 0) {
                this.icaoHasError = true;
                this.BuildMessage();
            }
            else {
                this.StartBusyIndicator("");
                this.GetAirlineByICAO(this.ICAO, 0);
            }
        }
        else {
            var x = this.AirlinesList.filter(function (a) { return a.Code.toLocaleLowerCase() == _this.Code.toLocaleLowerCase(); });
            if (x.length > 0) {
                this.codeHasError = true;
                this.BuildMessage();
            }
            else {
                this.StartBusyIndicator("");
                this.GetAirlineByCode(this.Code, 0);
            }
        }
    };
    NewAirlineComponent.prototype.GetAirlineByCode = function (code, tenant) {
        var _this = this;
        var zeroEntity;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetAirlineByCode(code, tenant).subscribe(function (myResult) {
            _this.StopBusyIndicator();
            zeroEntity = myResult.Result;
            if (zeroEntity != null) {
                _this.GetZeroEntity_Completed(zeroEntity);
            }
            _this.BuildMessage();
        });
    };
    NewAirlineComponent.prototype.GetAirlineByICAO = function (code, tenant) {
        var _this = this;
        var zeroEntity;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetAirlineByICAO(code, tenant).subscribe(function (myResult) {
            _this.StopBusyIndicator();
            zeroEntity = myResult;
            if (zeroEntity != null) {
                _this.GetZeroEntity_Completed(zeroEntity);
            }
            _this.BuildMessage();
        });
    };
    NewAirlineComponent.prototype.GetZeroEntity_Completed = function (zeroEntity) {
        if (this.isICAOMode) {
            this.zeroIcaoHasError = true;
        }
        else {
            this.zeroCodeHasError = true;
        }
        this.Code = zeroEntity.Code;
        this.EnglishName = zeroEntity.EnglishName;
        this.LocalName = zeroEntity.LocalName;
        this.Prefix = zeroEntity.Prefix;
        this.ICAO = zeroEntity.ICAO;
        this.Website = zeroEntity.Website;
        this.Remark = zeroEntity.Remark;
        this.SetUIPropertiesPrefix(false);
    };
    NewAirlineComponent.prototype.LoadAirlineListMethod = function () {
        var _this = this;
        var myService = new AirlineListService_1.AirlineListService();
        myService.getAll().subscribe(function (response) {
            _this.AirlinesList = response.Result;
        });
    };
    NewAirlineComponent.prototype.BuildMessage = function () {
        var myMessage = "";
        var tenantZeroMessage = "This airline already exists in our database and on save it will be copied to your airlines list";
        if (this.codeHasError) {
            myMessage = "This airline already exists!";
        }
        else if (this.zeroCodeHasError) {
            myMessage = tenantZeroMessage;
        }
        if (this.icaoHasError) {
            myMessage += Tools_1.AppTool.IsNullOrEmpty(myMessage) ? "An airline with same ICAO already exists!" : ",     " + "An airline with same ICAO already exists!";
        }
        else if (this.zeroIcaoHasError) {
            if (!this.zeroCodeHasError) {
                myMessage += Tools_1.AppTool.IsNullOrEmpty(myMessage) ? tenantZeroMessage : ", " + tenantZeroMessage;
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myMessage)) {
            this.IsSaveEnabled = true;
            this.ZeroExistsMessageVisibility = false;
            this.UIProperties.SetEnabled("InActive", "Airline", true);
        }
        else {
            if (this.zeroIcaoHasError || this.zeroCodeHasError) {
                if (this.codeHasError || this.icaoHasError) {
                    this.IsSaveEnabled = false;
                }
                else {
                    this.IsSaveEnabled = true;
                }
            }
            else {
                this.IsSaveEnabled = false;
            }
            this.ZeroExistsMessageVisibility = true;
            this.UIProperties.SetEnabled("InActive", "Airline", false);
        }
        this.Message = myMessage;
    };
    NewAirlineComponent.prototype.SetUIPropertiesPrefix = function (isEnabled) {
        if (this.TenantPM.Id != 0) {
            this.UIProperties.SetEnabled("Prefix", "Airline", isEnabled);
        }
    };
    NewAirlineComponent.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    NewAirlineComponent.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    NewAirlineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewAirlineComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewAirlineComponent);
    return NewAirlineComponent;
}(BaseComponent_1.BaseComponent));
exports.NewAirlineComponent = NewAirlineComponent;
//# sourceMappingURL=NewAirlineComponent.js.map