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
var FTPDetailPMService_1 = require("../../../Services/StandardPMs/FTPDetailPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ArtemusSettingsComponent = /** @class */ (function (_super) {
    __extends(ArtemusSettingsComponent, _super);
    function ArtemusSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myFTPService = new FTPDetailPMService_1.FTPDetailPMService();
        return _this;
    }
    ArtemusSettingsComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "CustomsInterfaceSetting";
        this.GetData();
        this.Clone();
    };
    ArtemusSettingsComponent.prototype.GetData = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ArtemusOutSettingsId)) {
            this.Load(this.ArtemusOutSettingsId, "OUT");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ArtemusInSettingsId)) {
            this.Load(this.ArtemusInSettingsId, "IN");
        }
    };
    ArtemusSettingsComponent.prototype.Load = function (id, code) {
        var _this = this;
        this.myFTPService.get(id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myEntity = myResponse.Result;
                if (myEntity != null) {
                    if (code == "OUT") {
                        _this.ArtemusOutSettingsHost = myEntity.Host;
                    }
                    else if (code == "IN") {
                        _this.ArtemusInSettingsHost = myEntity.Host;
                    }
                }
            }
        });
    };
    Object.defineProperty(ArtemusSettingsComponent.prototype, "ArtemusOutSettingsId", {
        get: function () { return this.EntityPM.ArtemusOutSettingsId; },
        set: function (newValue) {
            if (this.EntityPM.ArtemusOutSettingsId != newValue) {
                this.EntityPM.ArtemusOutSettingsId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArtemusSettingsComponent.prototype, "ArtemusOutSettingsHost", {
        get: function () { return this.EntityPM.ArtemusOutSettingsHost; },
        set: function (newValue) {
            if (this.EntityPM.ArtemusOutSettingsHost != newValue) {
                this.EntityPM.ArtemusOutSettingsHost = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArtemusSettingsComponent.prototype, "ArtemusInSettingsId", {
        get: function () { return this.EntityPM.ArtemusInSettingsId; },
        set: function (newValue) {
            if (this.EntityPM.ArtemusInSettingsId != newValue) {
                this.EntityPM.ArtemusInSettingsId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArtemusSettingsComponent.prototype, "ArtemusInSettingsHost", {
        get: function () { return this.EntityPM.ArtemusInSettingsHost; },
        set: function (newValue) {
            if (this.EntityPM.ArtemusInSettingsHost != newValue) {
                this.EntityPM.ArtemusInSettingsHost = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ArtemusSettingsComponent.prototype.Add = function (myCode) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { Code: myCode, IsNew: true };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (myCode == "OUT") {
                        _this.ArtemusOutSettingsId = comp.EntityPM.Id;
                        _this.ArtemusOutSettingsHost = comp.EntityPM.Host;
                    }
                    else if (myCode == "IN") {
                        _this.ArtemusInSettingsId = comp.EntityPM.Id;
                        _this.ArtemusInSettingsHost = comp.EntityPM.Host;
                    }
                }
            });
        });
    };
    ArtemusSettingsComponent.prototype.Edit = function (myCode) {
        var _this = this;
        var settingId = null;
        if (myCode == "OUT") {
            settingId = this.EntityPM.ArtemusOutSettingsId;
        }
        else if (myCode == "IN") {
            settingId = this.EntityPM.ArtemusInSettingsId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(settingId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { Code: myCode, IsNew: false, EntityId: settingId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        if (myCode == "OUT") {
                            _this.ArtemusOutSettingsId = comp.EntityPM.Id;
                            _this.ArtemusOutSettingsHost = comp.EntityPM.Host;
                        }
                        else if (myCode == "IN") {
                            _this.ArtemusInSettingsId = comp.EntityPM.Id;
                            _this.ArtemusInSettingsHost = comp.EntityPM.Host;
                        }
                    }
                });
            });
        }
    };
    ArtemusSettingsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    ArtemusSettingsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    ArtemusSettingsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ArtemusOutSettingsId');
        this.myCloner.AddField('ArtemusInSettingsId');
        this.myCloner.AddEntity(this.EntityPM);
    };
    ArtemusSettingsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    ArtemusSettingsComponent = __decorate([
        core_1.Component({
            moduleId: './Common/Components/Maintenance/CustomsInterface/',
            templateUrl: 'ArtemusSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ArtemusSettingsComponent);
    return ArtemusSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.ArtemusSettingsComponent = ArtemusSettingsComponent;
//# sourceMappingURL=ArtemusSettingsComponent.js.map