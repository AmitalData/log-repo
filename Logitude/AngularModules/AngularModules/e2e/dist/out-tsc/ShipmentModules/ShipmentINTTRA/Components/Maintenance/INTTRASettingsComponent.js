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
var INTTRADomainService_1 = require("../../Services/INTTRADomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var INTTRASettingsComponent = /** @class */ (function (_super) {
    __extends(INTTRASettingsComponent, _super);
    function INTTRASettingsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "INTTRASetting";
        _this.DataContext = _this;
        _this.Branches = [];
        _this.RegistrationList = [];
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (res1) {
            _this.entityResourceService.getEntityResourceByTableName("Branch").subscribe(function (res2) {
                _this.LoadData();
            });
        });
        return _this;
    }
    INTTRASettingsComponent.prototype.LoadData = function () {
        var _this = this;
        this.myService = new INTTRADomainService_1.INTTRADomainService();
        this.myService.GetINTTRASettings().subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.Helper = myResponse.Result;
                _this.EntityPM = _this.Helper.INTTRASetting;
                _this.Branches = [];
                _this.Helper.Branches.sort(function (a, b) { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1; }).forEach(function (item) {
                    _this.Branches.push(item);
                });
                _this.BuildRegistrationList();
                _this.SetUIProperties();
                _this.IsResourcesReady = true;
            }
        });
    };
    INTTRASettingsComponent.prototype.BuildRegistrationList = function () {
        var _this = this;
        this.RegistrationList = [];
        this.Helper.Items.filter(function (f) { return f.IsLineItem == true; }).forEach(function (line) {
            _this.RegistrationList.push(new RegistrationItem(line, _this));
        });
    };
    INTTRASettingsComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("OutSettingsHost", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InSettingsHost", this.ObjectTableName, false);
    };
    Object.defineProperty(INTTRASettingsComponent.prototype, "INTTRASettingModeCode", {
        get: function () { return this.EntityPM.INTTRASettingModeCode; },
        set: function (value) {
            if (this.EntityPM.INTTRASettingModeCode != value) {
                this.EntityPM.INTTRASettingModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingsComponent.prototype, "OutSettingsId", {
        get: function () { return this.EntityPM.OutSettingsId; },
        set: function (value) {
            if (this.EntityPM.OutSettingsId != value) {
                this.EntityPM.OutSettingsId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingsComponent.prototype, "OutSettingsHost", {
        get: function () { return this.EntityPM.OutSettingsHost; },
        set: function (value) {
            if (this.EntityPM.OutSettingsHost != value) {
                this.EntityPM.OutSettingsHost = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingsComponent.prototype, "InSettingsId", {
        get: function () { return this.EntityPM.InSettingsId; },
        set: function (value) {
            if (this.EntityPM.InSettingsId != value) {
                this.EntityPM.InSettingsId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingsComponent.prototype, "InSettingsHost", {
        get: function () { return this.EntityPM.InSettingsHost; },
        set: function (value) {
            if (this.EntityPM.InSettingsHost != value) {
                this.EntityPM.InSettingsHost = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingsComponent.prototype, "INTTRAId", {
        get: function () { return this.EntityPM.INTTRAId; },
        set: function (value) {
            if (this.EntityPM.INTTRAId != value) {
                this.EntityPM.INTTRAId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingsComponent.prototype, "INTTRAAlias", {
        get: function () { return this.EntityPM.INTTRAAlias; },
        set: function (value) {
            if (this.EntityPM.INTTRAAlias != value) {
                this.EntityPM.INTTRAAlias = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    INTTRASettingsComponent.prototype.AddFTPClicked = function (Code) {
        var _this = this;
        var isTest = this.INTTRASettingModeCode == "TEST" ? true : false;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { Code: Code, IsNew: true, IsINTTRA: true, IsTestMode: isTest };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (Code == "Out") {
                        _this.OutSettingsId = comp.EntityPM.Id;
                        _this.OutSettingsHost = comp.EntityPM.Host;
                    }
                    else {
                        _this.InSettingsId = comp.EntityPM.Id;
                        _this.InSettingsHost = comp.EntityPM.Host;
                    }
                }
            });
        });
    };
    INTTRASettingsComponent.prototype.EditFTPClicked = function (Code) {
        var _this = this;
        var settingId = null;
        if (Code == "Out") {
            settingId = this.OutSettingsId;
        }
        else {
            settingId = this.InSettingsId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(settingId)) {
            var isTest = this.INTTRASettingModeCode == "TEST" ? true : false;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { Code: Code, IsNew: false, IsINTTRA: true, IsTestMode: isTest, EntityId: settingId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        if (Code == "Out") {
                            _this.OutSettingsHost = comp.EntityPM.Host;
                        }
                        else {
                            _this.InSettingsHost = comp.EntityPM.Host;
                        }
                    }
                });
            });
        }
    };
    INTTRASettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    INTTRASettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.Branches.forEach(function (branch) {
            Validator_1.Validator.TryValidateObject(branch, "Branch", errors);
        });
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myService.UpdateINTTRASettings(this.Helper).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    INTTRASettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './INTTRASettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], INTTRASettingsComponent);
    return INTTRASettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.INTTRASettingsComponent = INTTRASettingsComponent;
var RegistrationItem = /** @class */ (function () {
    function RegistrationItem(line, father) {
        var _this = this;
        this.father = father;
        this.Carriers = [];
        this.CompinedId = line.CompinedId;
        this.Code = line.Code;
        this.Name = line.Name;
        this.Notes = line.Notes;
        this.ShippingLineId = line.ShippingLineId;
        father.Branches.forEach(function (branch) {
            var itemCarrier = _this.father.Helper.Items.filter(function (f) { return f.ShippingLineId == _this.ShippingLineId && f.BranchId == branch.Id && f.IsLineItem == false; })[0];
            if (!itemCarrier) {
            }
            _this.Carriers.push(new RegistrationItemCarrier(itemCarrier));
        });
    }
    return RegistrationItem;
}());
var RegistrationItemCarrier = /** @class */ (function () {
    function RegistrationItemCarrier(itemCarrier) {
        this.itemCarrier = itemCarrier;
    }
    Object.defineProperty(RegistrationItemCarrier.prototype, "IsRegistered", {
        get: function () { return this.itemCarrier.IsRegistered; },
        set: function (value) {
            if (this.itemCarrier.IsRegistered != value) {
                this.itemCarrier.IsRegistered = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return RegistrationItemCarrier;
}());
//# sourceMappingURL=INTTRASettingsComponent.js.map