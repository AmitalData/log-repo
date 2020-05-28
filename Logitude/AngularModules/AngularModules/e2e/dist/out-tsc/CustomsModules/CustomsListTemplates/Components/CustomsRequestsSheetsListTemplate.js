"use strict";
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var CustomsRequestMenuService_1 = require("../../../Customs/Services/Others/CustomsRequestMenuService");
var CustomsRequestSheetExtendedPMService_1 = require("../../../Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService");
var ResponseDataBase_1 = require("../../../Customs/DataContract/ResponseData/ResponseDataBase");
var CustomsRequestsSheetPM_1 = require("../../../Customs/EntityPMs/CustomsRequestsSheetPM");
var CustomsRequestsSheetsListTemplate = /** @class */ (function () {
    function CustomsRequestsSheetsListTemplate(CD) {
        this.CD = CD;
        this.ReAnalyzeButtonIsEnabled = false;
        this.ReAnalyzeButtonVisibility = false;
        this.CancleButtonOpacity = "1";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsCancelled = false;
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
    }
    CustomsRequestsSheetsListTemplate.prototype.setVariables = function (customsRequestsSheet, fieldName) {
        ///console.log(rowData);
        this._CustomsRequestsSheet = customsRequestsSheet;
        this.fieldName = fieldName;
        //#region Set Icons
        //#endregion 
        if (customsRequestsSheet.RequestStatusCode == "99" || customsRequestsSheet.RequestStatusCode == "30") {
            //CancleButtonVisibility = Visibility.Collapsed;
            this.IsCancelled = true;
        }
        this.ReAnalyzeButtonIsEnabled =
            customsRequestsSheet.RequestStatusCode == "25" //Analyze Failed 
                ||
                    (customsRequestsSheet.IsRestored || customsRequestsSheet.RequestStatusCode == "21"); //21,Received,תשובה תקינה
        ;
        var LoggedUserPMCode = SessionLocator_1.SessionLocator.LoggedUserPM.Code || "";
        LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
        this.ReAnalyzeButtonVisibility = SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare || (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital.")) ? true : false;
        if (!this.IsCancleButtonEnabled(customsRequestsSheet.RequestStatusCode, customsRequestsSheet.IsDCA)) {
            this.CancleButtonOpacity = "0.95";
        }
        this.CD.detectChanges();
    };
    CustomsRequestsSheetsListTemplate.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    CustomsRequestsSheetsListTemplate.prototype.ShowFormatedResponse = function (RequestComminicationId, InterfaceTypeCode) {
        ///alert("ShowFormatedResponse(id)" + RequestComminicationId);
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        customsRequestMenuService.ShowModalByIdAndIntreface(RequestComminicationId, InterfaceTypeCode, this._CustomsRequestsSheet.RequestDescription);
    };
    CustomsRequestsSheetsListTemplate.prototype.CancleButtonVisibility = function (RequestStatusCode, IsDCA) {
        if (RequestStatusCode == "30") {
            return false;
        }
        if (!this.IsCancelled && RequestStatusCode != "99") {
            return true;
        }
        return false;
    };
    CustomsRequestsSheetsListTemplate.prototype.IsCancleButtonEnabled = function (RequestStatusCode, IsDCA) {
        return ResponseDataBase_1.ResponseDataBase.RequestSheetCanCancelled(RequestStatusCode, IsDCA);
        ;
    };
    CustomsRequestsSheetsListTemplate.prototype.CancleRequestMethod = function (id) {
        var _this = this;
        //CancleButtonVisibility = Visibility.Collapsed;
        //FirePropertyChanged("CancleButtonVisibility");
        this.IsCancelled = true;
        //FirePropertyChanged("IsCancelled");
        this.CD.detectChanges();
        this.CurrentSession.StartBusyIndicator("");
        var mappedEntity = new CustomsRequestsSheetPM_1.CustomsRequestsSheetPM();
        mappedEntity.Id = id;
        mappedEntity.RequestStatusCode = "99";
        var myCustomsRequestSheetExtendedPMService = new CustomsRequestSheetExtendedPMService_1.CustomsRequestSheetExtendedPMService();
        myCustomsRequestSheetExtendedPMService.PostSetCustomsRequestSheetStatus(mappedEntity)
            .subscribe(function (r) {
            _this.CurrentSession.StopBusyIndicator();
            if (r.Result) {
                //alert(r.Result);
                _this.IsCancelled = false;
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Width = 400;
                messageWindow.Height = 150;
                messageWindow.Title = "Cancelled Customs Request Sheet Failed !!";
                messageWindow.Show(r.Result);
            }
            else {
                _this._CustomsRequestsSheet.RequestStatusCode = "6";
                _this._CustomsRequestsSheet.RequestStatusName = "מבוטלת";
            }
            _this.CD.detectChanges();
        });
        //InvokeOperation < string > op = context.SetCustomsRequestSheetStatus(Id, customsRequestsSheetList.Tenant, "99");
        //op.Completed += op_Completed;
    };
    CustomsRequestsSheetsListTemplate.prototype.CanShowFormatedResponseCommand = function () {
        if (this._CustomsRequestsSheet.RequestStatusCode == "99" ||
            Tools_1.AppTool.IsNullOrEmpty(this._CustomsRequestsSheet.RequestStatusCode)) {
            return false;
        }
        var myint;
        myint = this._CustomsRequestsSheet.RequestStatusCode;
        if (myint >= 21) {
            return true;
        }
        return false;
    };
    CustomsRequestsSheetsListTemplate.prototype.OnShowLogclick = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 900;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");
        ;
        logitudeWindow.WindowArgs = this._CustomsRequestsSheet;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationLogMoreDetailsComponent');
    };
    CustomsRequestsSheetsListTemplate.prototype.ReAnalyzeButtonCommandAction = function () {
        var _this = this;
        this.ReAnalyzeButtonIsEnabled = false;
        this.CD.detectChanges();
        this.CurrentSession.StartBusyIndicator("");
        var myCustomsRequestSheetExtendedPMService = new CustomsRequestSheetExtendedPMService_1.CustomsRequestSheetExtendedPMService();
        myCustomsRequestSheetExtendedPMService.PostSetCustomsRequestSheetStatus;
        myCustomsRequestSheetExtendedPMService.PostCustomsRequestSheetReQueue(this._CustomsRequestsSheet)
            .subscribe(function (r) {
            _this.CurrentSession.StopBusyIndicator();
            if (r.Result) {
                //alert(r.Result);
                _this.IsCancelled = false;
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Width = 400;
                messageWindow.Height = 150;
                messageWindow.Title = "Cancelle Customs Request Sheet Failed !!";
                messageWindow.Show(r.Result);
            }
            else {
                _this._CustomsRequestsSheet.RequestStatusName = "תשובה תקינה";
                _this._CustomsRequestsSheet.RequestStatusName = "תשובה תקינה";
            }
            _this.CD.detectChanges();
        });
    };
    CustomsRequestsSheetsListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsRequestsSheetsListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], CustomsRequestsSheetsListTemplate);
    return CustomsRequestsSheetsListTemplate;
}());
exports.CustomsRequestsSheetsListTemplate = CustomsRequestsSheetsListTemplate;
//# sourceMappingURL=CustomsRequestsSheetsListTemplate.js.map