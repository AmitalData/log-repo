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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CustomsPartnerFtpPM_1 = require("../../../Customs/EntityPMs/CustomsPartnerFtpPM");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomsPartnerFtpPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsPartnerFtpPMService");
var CustomsPartnerFtpListService_1 = require("../../../Customs/Services/StandardLists/CustomsPartnerFtpListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsPartnerFtpExtendedPMService_1 = require("../../../Customs/Services/ExtendedPMs/CustomsPartnerFtpExtendedPMService");
var FTPDetailPMService_1 = require("../../../Common/Services/StandardPMs/FTPDetailPMService");
var CustomsPartnerFtpListComponent = /** @class */ (function (_super) {
    __extends(CustomsPartnerFtpListComponent, _super);
    function CustomsPartnerFtpListComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.EntityPM = new CustomsPartnerFtpPM_1.CustomsPartnerFtpPM();
        _this.ObjectTableName = "Customs.CustomsPartnerFtp";
        _this.isControlEnabled = true;
        _this.IsLoaded = false;
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._CustomsPartnerFtpPMService = new CustomsPartnerFtpPMService_1.CustomsPartnerFtpPMService();
        _this._CustomsPartnerFtpListService = new CustomsPartnerFtpListService_1.CustomsPartnerFtpListService();
        _this._CustomsPartnerFtpExtendedPMService = new CustomsPartnerFtpExtendedPMService_1.CustomsPartnerFtpExtendedPMService();
        _this.ValidationErrorsList = [];
        _this._TypeCodeItems = [];
        _this._InterfaceNameItems = [];
        _this._PartnerCodeItems = [];
        _this._InEditMode = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._IsNew = false;
        _this._FetchCustomsPartnerFtpResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.myFTPService = new FTPDetailPMService_1.FTPDetailPMService();
        //this._TypeCodeItems.push({ 'Id': '', 'Name': '' });
        //this._TypeCodeItems.push({ 'Id': 'IN', 'Name': 'In' });
        //this._TypeCodeItems.push({ 'Id': 'OUT', 'Name': 'Out' });
        //this._PartnerCodeItems =//.push({ 'Id': 'Malam', 'Name': 'Malam' });
        //    [
        //        { 'Id': '', 'Name': '' },
        //        { 'Id': 'MAMAN', 'Name': 'Maman' },
        //    ];
        //this._InterfaceNameItems =
        //    [
        //    { 'Id': "", 'Name': '' },
        //    { 'Id': "SUBMANIFEST", 'Name': 'SubManifest' },
        //];
        _this.CurrentSession.StartBusyIndicator("");
        _this._EntityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (response) {
            _this._CustomsPartnerFtpExtendedPMService.GetScreenOption(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var screenOption = res.Result;
                _this._PartnerCodeItems = screenOption.PartnerCodeItems;
                //this._InterfaceNameItems = screenOption.InterfaceNameItems;
                _this._InterfaceNameItems = [];
                _this._InterfaceNameItems.push({ Key: '', Value: '' });
                _this._InterfaceDetailsItems = [];
                var listInterfaceDetailsItems = screenOption.InterfaceDetailsItems;
                listInterfaceDetailsItems.forEach(function (r) {
                    var val = r.Value;
                    var myInterfaceDetails = JSON.parse(val);
                    _this._InterfaceNameItems.push({ Key: myInterfaceDetails.Code, Value: myInterfaceDetails.Name });
                    _this._InterfaceDetailsItems.push(myInterfaceDetails);
                });
                _this._TypeCodeItems = screenOption.TypeCodeItems;
                _this.ReLoadList();
            });
        });
        return _this;
    }
    CustomsPartnerFtpListComponent.prototype.ngOnInit = function () {
    };
    CustomsPartnerFtpListComponent.prototype.IsRequierd = function () {
        if (this._CustomsPartnerFtpPM == null) {
            return;
        }
        this.UIProperties.SetRequired("InterfaceName1", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.InterfaceName));
        this.UIProperties.SetRequired("PartnerCode1", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PartnerCode));
        this.UIProperties.SetRequired("TypeCode1", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.TypeCode));
    };
    CustomsPartnerFtpListComponent.prototype.ReLoadList = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this._InEditMode = false;
        this._IsNew = false;
        this._CustomsPartnerFtpPM = null;
        this._CustomsPartnerFtpListService.getAll().subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            console.log("Get All CustomsPartnerFtp Definition: ", myResult);
            if (myResult != null && myResult.Result != null) {
                var _mappedListsArray = myResult.Result;
                _mappedListsArray.forEach(function (row) {
                    var detail = _this._InterfaceDetailsItems.filter(function (r) { return r.Code == row.InterfaceName; })[0];
                    row.InterfaceCodeName = detail.Name;
                });
                _this._FetchCustomsPartnerFtpResultList.InsertCollection(_mappedListsArray);
            }
        });
        this.IsLoaded = true;
    };
    CustomsPartnerFtpListComponent.prototype.CancelButtonClicked = function () {
        if (this._InEditMode) {
            this._InEditMode = false;
            this._CustomsPartnerFtpPM = null;
            return;
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsPartnerFtpListComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        this.ValidateWebApi();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        if (this._IsNew == true) {
            this._CustomsPartnerFtpPMService.insert(this._CustomsPartnerFtpPM)
                .subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var res = response;
                if (res.HasError) {
                    _this.ValidationErrorsList = res.ErrorsArray;
                    return;
                }
                else {
                    _this.ReLoadList();
                }
            });
        }
        else {
            this._CustomsPartnerFtpPMService.update(this._CustomsPartnerFtpPM).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var res = response;
                if (res.HasError) {
                    _this.ValidationErrorsList = res.ErrorsArray;
                    return;
                }
                else {
                    _this.ReLoadList();
                }
            });
        }
        //this.CurrentSession.CurrentWindow.StopBusyIndicator();
        //console.log("..Saved Successfully ");
        ///this.CurrentSession.CloseCurrentWindow();
    };
    CustomsPartnerFtpListComponent.prototype.AddCustomsPartnerFtpCommand = function () {
        this._IsNew = true;
        this._CustomsPartnerFtpPM = new CustomsPartnerFtpPM_1.CustomsPartnerFtpPM();
        this._CustomsPartnerFtpPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.ClearScreen(); //this._SettingsHost = null;
        this._InEditMode = true;
        //  this._CustomsPartnerFtpResultList.Insert(new CustomsPartnerFtpVM(new CustomsPartnerFtpPM(), true));
    };
    CustomsPartnerFtpListComponent.prototype.DeleteButtonClicked = function (item) {
        var _this = this;
        this._IsNew = false;
        //this._CustomsPartnerFtpResultList.Remove(item);
        //.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
        this._CustomsPartnerFtpExtendedPMService.delete(item.Id).subscribe(function (response) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var res = response;
            if (res.HasError) {
                _this.ValidationErrorsList = res.ErrorsArray;
                return;
            }
            else {
                _this.ReLoadList();
            }
        });
    };
    CustomsPartnerFtpListComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        this._IsNew = false;
        this.ClearScreen();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CustomsPartnerFtpPMService.get(item.Id)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                return;
            }
            _this._CustomsPartnerFtpPM = myResponse.Result;
            _this._InEditMode = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(_this._CustomsPartnerFtpPM.InterfaceName)) {
                _this._InterfaceDetail = _this._InterfaceDetailsItems.filter(function (r) { return r.Code == _this._CustomsPartnerFtpPM.InterfaceName; })[0];
            }
            switch (_this._InterfaceDetail.ViaMethod) {
                case "WEBAPI":
                    {
                        _this._WebApiDefinition = new WebApiDefinition();
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this._CustomsPartnerFtpPM.CommunicationDetails)) {
                            _this._WebApiDefinition = JSON.parse(_this._CustomsPartnerFtpPM.CommunicationDetails);
                        }
                    }
                    break;
                case "FTP":
                    {
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this._CustomsPartnerFtpPM.FtpDetailsId)) {
                            _this.LoadFTP(_this._CustomsPartnerFtpPM.FtpDetailsId, _this._CustomsPartnerFtpPM.TypeCode);
                        }
                    }
                    break;
                default:
                    break;
            }
        });
    };
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "Id", {
        get: function () { return this._CustomsPartnerFtpPM.Id; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.Id != newValue) {
            this._CustomsPartnerFtpPM.Id = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "Tenant", {
        get: function () { return this._CustomsPartnerFtpPM.Tenant; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.Tenant != newValue) {
            this._CustomsPartnerFtpPM.Tenant = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "TypeCode", {
        get: function () { return this._CustomsPartnerFtpPM.TypeCode; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.TypeCode != newValue) {
            this._CustomsPartnerFtpPM.TypeCode = newValue;
            this.IsRequierd();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "PartnerCode", {
        get: function () { return this._CustomsPartnerFtpPM.PartnerCode; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.PartnerCode != newValue) {
            this._CustomsPartnerFtpPM.PartnerCode = newValue;
            this.IsRequierd();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "InterfaceName", {
        get: function () { return this._CustomsPartnerFtpPM.InterfaceName; },
        set: function (newValue) {
            if (this._CustomsPartnerFtpPM.InterfaceName != newValue) {
                this._CustomsPartnerFtpPM.InterfaceName = newValue;
                this.IsRequierd();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "FtpDetailsId", {
        get: function () { return this._CustomsPartnerFtpPM.FtpDetailsId; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.FtpDetailsId != newValue) {
            this._CustomsPartnerFtpPM.FtpDetailsId = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "FileName", {
        get: function () { return this._CustomsPartnerFtpPM.FileName; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.FileName != newValue) {
            this._CustomsPartnerFtpPM.FileName = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "FileExt", {
        get: function () { return this._CustomsPartnerFtpPM.FileExt; },
        set: function (newValue) { if (this._CustomsPartnerFtpPM.FileExt != newValue) {
            this._CustomsPartnerFtpPM.FileExt = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "WEBAPIURL", {
        get: function () { return this._WebApiDefinition.WEBAPIURL; },
        set: function (newValue) { if (this._WebApiDefinition.WEBAPIURL != newValue) {
            this._WebApiDefinition.WEBAPIURL = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "WEBAPIAuthenticationURL", {
        get: function () { return this._WebApiDefinition.WEBAPIAuthenticationURL; },
        set: function (newValue) { if (this._WebApiDefinition.WEBAPIAuthenticationURL != newValue) {
            this._WebApiDefinition.WEBAPIAuthenticationURL = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "User", {
        get: function () { return this._WebApiDefinition.User; },
        set: function (newValue) { if (this._WebApiDefinition.User != newValue) {
            this._WebApiDefinition.User = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsPartnerFtpListComponent.prototype, "Password", {
        get: function () { return this._WebApiDefinition.Password; },
        set: function (newValue) { if (this._WebApiDefinition.Password != newValue) {
            this._WebApiDefinition.Password = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    CustomsPartnerFtpListComponent.prototype.ClearScreen = function () {
        this.ValidationErrorsList = [];
        this._SettingsHost = null;
        this._WebApiDefinition = new WebApiDefinition();
        this.IsRequierd();
        //this.WEBAPIAuthenticationURL = this.WEBAPIURL = null;
        //this.Password = this.User = null;
    };
    CustomsPartnerFtpListComponent.prototype.TypeCodeChanged = function (selectControl) {
        this._CustomsPartnerFtpPM.TypeCode = selectControl.value;
        this.IsRequierd();
    };
    CustomsPartnerFtpListComponent.prototype.PartnerCodeChanged = function (selectControl) {
        this._CustomsPartnerFtpPM.PartnerCode = selectControl.value;
        this.IsRequierd();
    };
    CustomsPartnerFtpListComponent.prototype.InterfaceNameChanged = function (selectControl) {
        this._CustomsPartnerFtpPM.InterfaceName = selectControl.value;
        this.IsRequierd();
        this.SetFromServer();
    };
    CustomsPartnerFtpListComponent.prototype.SetFromServer = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this._CustomsPartnerFtpPM.InterfaceName)) {
            this._InterfaceDetail = this._InterfaceDetailsItems.filter(function (r) { return r.Code == _this._CustomsPartnerFtpPM.InterfaceName; })[0];
            this.PartnerCode = this._InterfaceDetail.Partner;
            this.TypeCode = this._InterfaceDetail.TypeCode;
            if (this._InterfaceDetail.ViaMethod == "WEBAPI") {
                this._WebApiDefinition = new WebApiDefinition();
            }
        }
    };
    CustomsPartnerFtpListComponent.prototype.LoadFTP = function (id, code) {
        var _this = this;
        this.myFTPService.get(id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myEntity = myResponse.Result;
                if (myEntity != null) {
                    _this._SettingsHost = myEntity.Host;
                }
            }
        });
    };
    CustomsPartnerFtpListComponent.prototype.ValidateCustomsPartnerFtp = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.InterfaceName)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("שם מסר הינו חובה");
            return;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.TypeCode)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("קוד סוג הינו חובה");
            return;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.PartnerCode)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("קוד שותף הינו חובה");
            return;
        }
    };
    CustomsPartnerFtpListComponent.prototype.ValidateWebApi = function () {
        if (this._InterfaceDetail != null && this._InterfaceDetail.ViaMethod == 'WEBAPI') {
            if (Tools_1.AppTool.IsNullOrEmpty(this.WEBAPIURL)) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push("כתובת השירות הינו חובה");
                return;
            }
            var pattern = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
            if (!pattern.test(this.WEBAPIURL)) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push("כתובת השירות אינו חוקי");
                return;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.WEBAPIAuthenticationURL)) {
                if (!pattern.test(this.WEBAPIAuthenticationURL)) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push("כתובת אימות השירות אינו חוקי");
                    return;
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.WEBAPIAuthenticationURL) ||
                !Tools_1.AppTool.IsNullOrEmpty(this.User) ||
                !Tools_1.AppTool.IsNullOrEmpty(this.Password)) {
                /// MAYBE IF ONE IS NOT NULL ALL OTHER SHOULDNT BR NULL ALSO ??!!
            }
            this._CustomsPartnerFtpPM.CommunicationDetails = JSON.stringify(this._WebApiDefinition);
        }
    };
    CustomsPartnerFtpListComponent.prototype.AddFTP = function () {
        var _this = this;
        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { Code: this.TypeCode, IsNew: true };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.FtpDetailsId = comp.EntityPM.Id;
                    _this._SettingsHost = comp.EntityPM.Host;
                }
            });
        });
    };
    CustomsPartnerFtpListComponent.prototype.EditFTP = function () {
        var _this = this;
        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        var settingId = null;
        settingId = this.FtpDetailsId;
        if (!Tools_1.AppTool.IsNullOrEmpty(settingId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { Code: this.TypeCode, IsNew: false, EntityId: settingId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.FtpDetailsId = comp.EntityPM.Id;
                        _this._SettingsHost = comp.EntityPM.Host;
                    }
                });
            });
        }
    };
    CustomsPartnerFtpListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsPartnerFtpListComponent.html',
        })
        /// itzik:  bad pattren - Due Design paper - How to copy from  CustomsDocumentsDefinitionComponent - DING DING DING SHAME SHAME!!!
        ,
        __metadata("design:paramtypes", [])
    ], CustomsPartnerFtpListComponent);
    return CustomsPartnerFtpListComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsPartnerFtpListComponent = CustomsPartnerFtpListComponent;
var InterfaceDetails = /** @class */ (function () {
    function InterfaceDetails() {
    }
    return InterfaceDetails;
}());
var WebApiDefinition = /** @class */ (function () {
    function WebApiDefinition() {
    }
    return WebApiDefinition;
}());
//# sourceMappingURL=CustomsPartnerFtpListComponent.js.map