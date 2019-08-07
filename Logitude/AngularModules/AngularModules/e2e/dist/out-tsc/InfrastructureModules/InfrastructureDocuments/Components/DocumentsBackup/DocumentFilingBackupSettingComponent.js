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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DocumentFilingBackupSettingPM_1 = require("../../../../Common/EntityPMs/DocumentFilingBackupSettingPM");
var FTPDetailPMService_1 = require("../../../../Common/Services/StandardPMs/FTPDetailPMService");
var DocumentFilingBackupSettingPMService_1 = require("../../../../Common/Services/StandardPMs/DocumentFilingBackupSettingPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DocumentFilingBackupSettingComponent = /** @class */ (function (_super) {
    __extends(DocumentFilingBackupSettingComponent, _super);
    function DocumentFilingBackupSettingComponent() {
        var _this = _super.call(this) || this;
        _this.IsLoad = false;
        _this.DataContext = _this;
        _this.IsNewDocumentFilingBackupSetting = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.documentFilingBackupSettingPMService = new DocumentFilingBackupSettingPMService_1.DocumentFilingBackupSettingPMService();
        _this.myFTPService = new FTPDetailPMService_1.FTPDetailPMService();
        return _this;
    }
    DocumentFilingBackupSettingComponent.prototype.ngOnInit = function () {
        this.LoadData();
    };
    DocumentFilingBackupSettingComponent.prototype.LoadData = function () {
        var _this = this;
        this.documentFilingBackupSettingPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            if (!res.HasError) {
                _this.documentFilingBackupSettingPM = res.Result;
                if (!_this.documentFilingBackupSettingPM) {
                    _this.documentFilingBackupSettingPM = new DocumentFilingBackupSettingPM_1.DocumentFilingBackupSettingPM();
                    _this.documentFilingBackupSettingPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.documentFilingBackupSettingPM.IsActive = false;
                    _this.IsNewDocumentFilingBackupSetting = true;
                }
            }
            else {
                if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(res.ErrorsArray[0]);
                }
            }
            _this.FTPDetailId = _this.documentFilingBackupSettingPM.FTPDetailId;
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.FTPDetailId)) {
                _this.myFTPService.get(_this.FTPDetailId).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        var myEntity = myResponse.Result;
                        _this.FTPDetailHost = myEntity.Host;
                    }
                });
            }
            _this.IsLoad = true;
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DocumentFilingBackupSettingComponent.prototype.AddEditFTPDetails = function (type) {
        var _this = this;
        if (type == "Edit" && Tools_1.AppTool.IsNullOrEmpty(this.FTPDetailId))
            return;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        if (type == "Edit") {
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { IsNew: false, EntityId: this.FTPDetailId };
        }
        else {
            logWindow.Title = "Add FTP Detail";
            logWindow.WindowArgs = { IsNew: true };
        }
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.FTPDetailId = _this.documentFilingBackupSettingPM.FTPDetailId = comp.EntityPM.Id;
                    _this.FTPDetailHost = comp.EntityPM.Host;
                }
            });
        });
    };
    Object.defineProperty(DocumentFilingBackupSettingComponent.prototype, "IsActive", {
        get: function () {
            if (this.documentFilingBackupSettingPM) {
                this.isActive = this.documentFilingBackupSettingPM.IsActive;
            }
            return this.isActive;
        },
        set: function (value) {
            if (this.isActive != value)
                if (this.documentFilingBackupSettingPM) {
                    this.documentFilingBackupSettingPM.IsActive = value;
                }
        },
        enumerable: true,
        configurable: true
    });
    DocumentFilingBackupSettingComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DocumentFilingBackupSettingComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.documentFilingBackupSettingPM) {
            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.StartBusyIndicatorSaving();
                if (this.IsNewDocumentFilingBackupSetting) {
                    this.documentFilingBackupSettingPMService.insert(this.documentFilingBackupSettingPM).subscribe(function (res) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!res.HasError) {
                            _this.IsNewDocumentFilingBackupSetting = false;
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                        else if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Show(res.ErrorsArray[0]);
                        }
                    });
                }
                else {
                    this.documentFilingBackupSettingPMService.update(this.documentFilingBackupSettingPM).subscribe(function (res) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!res.HasError) {
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                        else if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Show(res.ErrorsArray[0]);
                        }
                    });
                }
            }
        }
    };
    DocumentFilingBackupSettingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentFilingBackupSettingComponent',
            templateUrl: './DocumentFilingBackupSettingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DocumentFilingBackupSettingComponent);
    return DocumentFilingBackupSettingComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentFilingBackupSettingComponent = DocumentFilingBackupSettingComponent;
//# sourceMappingURL=DocumentFilingBackupSettingComponent.js.map