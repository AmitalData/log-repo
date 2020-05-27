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
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var MessageRestoreRequestParams_1 = require("../../../Customs/DataContract/RequestParams/MessageRestoreRequestParams");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsRestoreMessagesComponent = /** @class */ (function (_super) {
    __extends(CustomsRestoreMessagesComponent, _super);
    function CustomsRestoreMessagesComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration"; //TODO
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        //_LastFetchDeclarationList: DeclarationList;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ResponseStatusXML = "";
        _this._isVisible = false;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this._IsRestoreByDates = false;
        _this._IsRestoreByCorrelation = true;
        _this._entityResourceService.getEntityResourceByTableName("Customs.CustomsExchangeRate", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customs.Declaration", 0).subscribe(function (response) {
                _this._isVisible = true;
                _this._TodayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            });
        });
        return _this;
    }
    CustomsRestoreMessagesComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    CustomsRestoreMessagesComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new MessageRestoreRequestParams_1.MessageRestoreRequestParams();
            //this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
        }
        if (this.ResponseData) {
            if (this.ResponseData != null) {
                if (!this.ResponseData.HasException && this.ResponseData.Succeeded) {
                    this.ResponseStatusXML = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SentToDCA"); //, TenantContext.Current.Id);
                    //נשלח למכס בהצלחה , משוב יתקבל בכספת
                }
                else {
                    //message = responseData.UserMessage;
                }
            }
            else {
                //message = "Service returned a null response!";
            }
        }
    };
    CustomsRestoreMessagesComponent.prototype.CorrelationNoTextChanged = function (ev) {
        var pattren = new RegExp('^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i');
        if (pattren.test(ev)) {
        }
        else {
            //this.UIProperties.SetValidity("CorrelationNo", this.ObjectTableName, false,"GUID");
        }
    };
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "CorrelationNo", {
        get: function () { return this.RequestParams.CorrelationID; },
        set: function (value) {
            if (this.RequestParams.CorrelationID != value) {
                this.RequestParams.CorrelationID = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("CorrelationNo", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("CorrelationNo", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "FromDate", {
        get: function () { return this.RequestParams.FromDate; },
        set: function (value) {
            if (this.RequestParams.FromDate != value) {
                this.RequestParams.FromDate = value;
                this.FromDateLostFocusMethod(value);
                if (value && !Tools_1.AppTool.IsNullOrEmpty(this.FromDateTime)) {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsRestoreMessagesComponent.prototype.FromDateLostFocusMethod = function (startDateItem) {
        this.ValidationErrorsList = [];
        this.FromDateTime = null;
        if (Tools_1.AppTool.IsNullOrEmpty(startDateItem)) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(startDateItem) && startDateItem > this.ToDate) {
            var msg = "תאריך התחלה לא יכול להיות אחרי תאריך סיום";
            this.ValidationErrorsList.push(msg);
        }
        if (startDateItem.getDate() == this._TodayDate.getDate()) {
            var date = new Date();
            date.setMinutes(0);
            this.FromDateTime = Tools_1.DateTool.AddHour(date, 3);
        }
    };
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "FromDateTime", {
        get: function () { return this._FromDateTime; },
        set: function (value) {
            if (this._FromDateTime != value) {
                this._FromDateTime = value;
                if (value) {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "ToDate", {
        get: function () { return this.RequestParams.ToDate; },
        set: function (value) {
            if (this.RequestParams.ToDate != value) {
                this.RequestParams.ToDate = value;
                this.ToDateLostFocusMethod(value);
                if (value && !Tools_1.AppTool.IsNullOrEmpty(this.ToDateTime)) {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsRestoreMessagesComponent.prototype.ToDateLostFocusMethod = function (endDateItem) {
        this.ValidationErrorsList = [];
        this.ToDateTime = null;
        if (Tools_1.AppTool.IsNullOrEmpty(endDateItem)) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate) && this.FromDate > endDateItem) {
            var msg = "תאריך התחלה לא יכול להיות אחרי תאריך סיום";
            this.ValidationErrorsList.push(msg);
        }
        if (endDateItem.getDate() == this._TodayDate.getDate()) {
            var date = new Date();
            date.setMinutes(0);
            this.ToDateTime = Tools_1.DateTool.AddHour(date, 3);
        }
    };
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "ToDateTime", {
        get: function () { return this._ToDateTime; },
        set: function (value) {
            if (this._ToDateTime != value) {
                this._ToDateTime = value;
                if (value) {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "InterfaceManagementsCode", {
        get: function () { return this._InterfaceManagementsCode; },
        set: function (value) {
            if (this._InterfaceManagementsCode != value) {
                this._InterfaceManagementsCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "InterfaceManagementSelectedItem", {
        get: function () { return this._InterfaceManagementSelectedItem; },
        set: function (value) {
            this._InterfaceManagementSelectedItem = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "IsRestoreByDates", {
        get: function () { return this._IsRestoreByDates; },
        set: function (value) {
            if (this._IsRestoreByDates == value) {
                return;
            }
            this._IsRestoreByDates = value;
            if (!this._IsRestoreByDates) {
                this._IsRestoreByCorrelation = true;
            }
            else {
                this._IsRestoreByCorrelation = false;
            }
            this.ClearData();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRestoreMessagesComponent.prototype, "IsRestoreByCorrelation", {
        get: function () { return this._IsRestoreByCorrelation; },
        set: function (value) {
            if (this._IsRestoreByCorrelation == value) {
                return;
            }
            this._IsRestoreByCorrelation = value;
            if (!this._IsRestoreByCorrelation) {
                this._IsRestoreByDates = true;
            }
            else {
                this._IsRestoreByDates = false;
            }
            this.ClearData();
        },
        enumerable: true,
        configurable: true
    });
    CustomsRestoreMessagesComponent.prototype.ClearData = function () {
        this.InterfaceManagementsCode = null;
        this.ToDate = this.FromDate = null;
    };
    CustomsRestoreMessagesComponent.prototype.FillErrors = function () {
        //var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //this.ValidationErrorsList = errors;
        if (this.IsRestoreByCorrelation) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CorrelationNo)) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrelationNumberIsMandatory"));
            }
        }
        else {
            var LoggedUserPMCode = SessionLocator_1.SessionLocator.LoggedUserPM.Code || "";
            LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
            if (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital.")) {
                console.warn("User Amital* suppress check InterfaceManagements");
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this.InterfaceManagementsCode)) {
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.InterfaceManagementsCodeIsMandatory");
                    this.ValidationErrorsList.push(msg);
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.InterfaceManagementSelectedItem)) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this._InterfaceManagementSelectedItem.DcaPrefixName)) {
                        var msg = "No Dca";
                        this.ValidationErrorsList.push(msg);
                    }
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.FromDate) || Tools_1.AppTool.IsNullOrEmpty(this.FromDateTime)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
                if (Tools_1.AppTool.IsNullOrEmpty(this.FromDateTime)) {
                    msg = msg + " (כולל שעה)";
                }
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ToDate) || Tools_1.AppTool.IsNullOrEmpty(this.ToDateTime)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
                if (Tools_1.AppTool.IsNullOrEmpty(this.ToDateTime)) {
                    msg = msg + " (כולל שעה)";
                }
                this.ValidationErrorsList.push(msg);
            }
            //if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            //    if (DateTool.GetDateFromDate(this.FromDate) > DateTool.GetDateFromDate(this.ToDate)) {
            //        var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            //        this.ValidationErrorsList.push(msg);
            //    }
            //}
        }
    };
    CustomsRestoreMessagesComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        //alert(customSendOptionsArgs.Option);
        this.ValidationErrorsList = [];
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var dcaPrefix = "";
        var interfaceManagementsCodeValue = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InterfaceManagementsCode)) {
            if (this._InterfaceManagementSelectedItem.DcaPrefixName.endsWith("_Out.")) {
                dcaPrefix = this._InterfaceManagementSelectedItem.DcaPrefixName.substring(0, this._InterfaceManagementSelectedItem.DcaPrefixName.length - 5);
            }
            else {
                dcaPrefix = this._InterfaceManagementSelectedItem.DcaPrefixName;
            }
            interfaceManagementsCodeValue = this._InterfaceManagementSelectedItem.Code;
        }
        var currRequestParams = new MessageRestoreRequestParams_1.MessageRestoreRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CorrelationNo)) {
            currRequestParams.CorrelationID = "";
        }
        else {
            currRequestParams.CorrelationID = this.CorrelationNo;
        }
        currRequestParams.InterfaceManagementsCode = dcaPrefix;
        currRequestParams.InterfaceManagementsCodeValue = interfaceManagementsCodeValue;
        currRequestParams.FromDate = this.FromDate;
        if (this.FromDateTime != null) {
            var fromDate = this.FromDate;
            fromDate.setHours(this.FromDateTime.getHours());
            fromDate.setMinutes(this.FromDateTime.getMinutes());
            currRequestParams.FromDate = fromDate;
        }
        currRequestParams.ToDate = this.ToDate;
        if (this.ToDateTime != null) {
            var toDate = this.ToDate;
            toDate.setHours(this.ToDateTime.getHours());
            toDate.setMinutes(this.ToDateTime.getMinutes());
            currRequestParams.ToDate = toDate;
        }
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשיחזור מסרים", false)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostMessageRestoreRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], CustomsRestoreMessagesComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    CustomsRestoreMessagesComponent = __decorate([
        core_1.Component({
            selector: 'CustomsRestoreMessagesComponent',
            moduleId: module.id,
            templateUrl: './CustomsRestoreMessagesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsRestoreMessagesComponent);
    return CustomsRestoreMessagesComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.CustomsRestoreMessagesComponent = CustomsRestoreMessagesComponent;
//# sourceMappingURL=CustomsRestoreMessagesComponent.js.map