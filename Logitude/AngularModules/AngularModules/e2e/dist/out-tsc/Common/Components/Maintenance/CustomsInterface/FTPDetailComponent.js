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
var FTPDetailPM_1 = require("../../../EntityPMs/FTPDetailPM");
var FTPDetailPMService_1 = require("../../../Services/StandardPMs/FTPDetailPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var FTPDetailComponent = /** @class */ (function (_super) {
    __extends(FTPDetailComponent, _super);
    function FTPDetailComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "FTPDetail";
        _this.DataContext = _this;
        _this.ShowInactive = false;
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isINTTRA = false;
        return _this;
    }
    FTPDetailComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.Code = args['Code'];
        this.IsNew = args['IsNew'];
        this.isINTTRA = args["IsINTTRA"];
        this.ShowInactive = !this.IsNew;
        this.myService = new FTPDetailPMService_1.FTPDetailPMService();
        if (this.Code) {
            this.Code = this.Code.toUpperCase();
        }
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            if (_this.IsNew) {
                _this.EntityPM = new FTPDetailPM_1.FTPDetailPM();
                _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                _this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                if (_this.isINTTRA == true) {
                    var isTestMode = args["IsTestMode"];
                    if (_this.Code == "OUT") {
                        _this.EntityPM.Folder = "inbound";
                    }
                    else {
                        _this.EntityPM.Folder = "outbound";
                    }
                    if (isTestMode == true) {
                        _this.EntityPM.Host = "ftp.cvt.inttra.com";
                    }
                    else {
                        _this.EntityPM.Host = "ftp.inttraworks.inttra.com";
                    }
                }
                _this.IsResourcesReady = true;
                _this.Clone();
            }
            else {
                var entityId = args['EntityId'];
                _this.myService.get(entityId).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.EntityPM = myResponse.Result;
                    }
                    _this.IsResourcesReady = true;
                    _this.Clone();
                });
            }
        });
    };
    Object.defineProperty(FTPDetailComponent.prototype, "UserName", {
        get: function () { return this.EntityPM.UserName; },
        set: function (value) {
            if (this.EntityPM.UserName != value) {
                this.EntityPM.UserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FTPDetailComponent.prototype, "Password", {
        get: function () { return this.EntityPM.Password; },
        set: function (value) {
            if (this.EntityPM.Password != value) {
                this.EntityPM.Password = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FTPDetailComponent.prototype, "Host", {
        get: function () { return this.EntityPM.Host; },
        set: function (value) {
            if (this.EntityPM.Host != value) {
                this.EntityPM.Host = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FTPDetailComponent.prototype, "Folder", {
        get: function () { return this.EntityPM.Folder; },
        set: function (value) {
            if (this.EntityPM.Folder != value) {
                this.EntityPM.Folder = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FTPDetailComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FTPDetailComponent.prototype, "UseSFTP", {
        get: function () { return this.EntityPM.UseSFTP; },
        set: function (value) {
            if (this.EntityPM.UseSFTP != value) {
                this.EntityPM.UseSFTP = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    FTPDetailComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    FTPDetailComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (!this.isINTTRA) {
            var isValid = this.ValidateHost();
            if (!isValid) {
                errors.push("Invalid Host");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new FTPDetailPMService_1.FTPDetailPMService();
            if (this.IsNew) {
                myService.insert(this.EntityPM).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                myService.update(this.EntityPM).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    };
    FTPDetailComponent.prototype.ValidateHost = function () {
        var isValid = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Host)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.Host.match(ipformat)) {
                isValid = true;
            }
        }
        return isValid;
    };
    FTPDetailComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('UserName');
        this.myCloner.AddField('Password');
        this.myCloner.AddField('Host');
        this.myCloner.AddField('Folder');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    };
    FTPDetailComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    FTPDetailComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FTPDetailComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], FTPDetailComponent);
    return FTPDetailComponent;
}(BaseComponent_1.BaseComponent));
exports.FTPDetailComponent = FTPDetailComponent;
//# sourceMappingURL=FTPDetailComponent.js.map