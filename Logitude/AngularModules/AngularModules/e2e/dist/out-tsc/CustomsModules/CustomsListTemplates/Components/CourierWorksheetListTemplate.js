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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var CourierMasterService_1 = require("../../../Customs/Services/Others/CourierMasterService");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var DeclarationWebService_1 = require("../../../Customs/Services/WebServices/DeclarationWebService");
var CourierWorksheetSharedDataService_1 = require("../../../Customs/Services/DataChange/CourierWorksheetSharedDataService");
var SendDeclarationComponent_1 = require("../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendDeclarationComponent");
var SendManifestComponent_1 = require("../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendManifestComponent");
var DeclarationPMService_1 = require("../../../Customs/Services/StandardPMs/DeclarationPMService");
var DropdownMenuFilterComponent_1 = require("../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/DropdownMenuFilterComponent");
var DeclarationCourierStatusPMService_1 = require("../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService");
var DeclarationCourierStatusListService_1 = require("../../../Customs/Services/StandardLists/DeclarationCourierStatusListService");
var DeclarationMamanSpecialActionListService_1 = require("../../../Customs/Services/StandardLists/DeclarationMamanSpecialActionListService");
var DeclarationMamanSpecialActionPM_1 = require("../../../Customs/EntityPMs/DeclarationMamanSpecialActionPM");
var DeclarationMamanSpecialActionPMService_1 = require("../../../Customs/Services/StandardPMs/DeclarationMamanSpecialActionPMService");
var CourierWorksheetListTemplate = /** @class */ (function () {
    function CourierWorksheetListTemplate(_CourierWorksheetSharedDataService, CD) {
        this._CourierWorksheetSharedDataService = _CourierWorksheetSharedDataService;
        this.CD = CD;
        this.IsDocumentStatusGreen = false;
        this.IsDocumentStatusRed = false;
        this.IsDocumentStatusBlue = false;
        this.IsManifestStatusRed = false;
        this.IsManifestStatusGreen = false;
        this.IsManifestStatusBlue = false;
        this.IsManifestStatusOrange = false;
        this.IsDeclarationStatusRed = false;
        this.IsDeclarationStatusGreen = false;
        this.IsDeclarationStatusBlue = false;
        this.IsDeclarationStatusOrange = false;
        this.IsPaymentStatusBlueChecked = false;
        this.IsPaymentStatusGreen = false;
        this.IsPaymentStatusBlue = false;
        this.IsPaymentStatusOrange = false;
        this.IsHighLow = false;
        this.IsDeclarationChecked = false;
        this.MamanStickerDetails = null;
        this.IsReceivingDelayCertificate = false;
        this.IsPrintDocuments = false;
        this.IsMamanSticker = false;
        this._DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService_1.DeclarationCourierStatusPMService();
        this._CourierMasterService = new CourierMasterService_1.CourierMasterService();
        this._DeclarationMamanSpecialActionListService = new DeclarationMamanSpecialActionListService_1.DeclarationMamanSpecialActionListService();
        this._DeclarationMamanSpecialActionPMService = new DeclarationMamanSpecialActionPMService_1.DeclarationMamanSpecialActionPMService;
        this._DeclarationWebService = new DeclarationWebService_1.DeclarationWebService;
        //  @ViewChild( SplitButtonComponent)  public MySplitButtonComponent: SplitButtonComponent = new SplitButtonComponent(null,null);
        //@ViewChild('ShortTitle', { read: ViewContainerRef }) ShortTitleViewContainerRef: ViewContainerRef;
        //@ViewChild('MySplitButtonComponent', { read: SplitButtonComponent }) MySplitButtonComponent: SplitButtonComponent;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._IsDropdownMenuFilterReady = false;
    }
    CourierWorksheetListTemplate.prototype.FirePreventSelect = function () {
        this.CurrentSession.PseventRowSelectEvent.emit("CourierWorksheetListTemplate.SendSplitButton");
    };
    CourierWorksheetListTemplate.prototype.FireUnSelect = function () {
        this.CurrentSession.PseventRowSelectEvent.emit("FireUnSelect");
    };
    //[AdditionalData] = "{rowIndex:row.rowIndex,gridId:LogGridId,RowOutEvent:RowOutEvent,RowOverEvent:RowOverEvent}"
    CourierWorksheetListTemplate.prototype.RefreshData = function () {
        var _this = this;
        var noLocal = true;
        if (noLocal) {
            this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
            return;
        }
        var myDeclarationCourierStatusListService = new DeclarationCourierStatusListService_1.DeclarationCourierStatusListService();
        myDeclarationCourierStatusListService.getSingle(this._CourierWorksheet.DeclarationId)
            .subscribe(function (serviceResponse) {
            //this._CourierWorksheet = serviceResponse.Result;
            //this._CourierWorksheet.CourierCustomStatusCode = "X";
            var courierWorksheet = serviceResponse.Result;
            courierWorksheet.CourierCustomStatusCode = "X";
            //this.CD.detectChanges();
            _this.setVariables(courierWorksheet, _this.fieldName); /*, AdditionalData:any)*/
        });
    };
    CourierWorksheetListTemplate.prototype.DropdownMenuButtonClicked = function (event) {
        this.ButtonClick(event);
    };
    CourierWorksheetListTemplate.prototype.DropdownDisplayClose = function () {
        //if (!AppTool.IsNullOrEmpty(this.MySplitButtonComponent)) {
        //  this.MySplitButtonComponent.DropdownDisplayClose();
        //}
    };
    CourierWorksheetListTemplate.prototype.setVariables = function (courierWorksheet, fieldName) {
        this._CourierWorksheet = courierWorksheet;
        this.fieldName = fieldName;
        //this._AdditionalData = AdditionalData;
        this.DropdownDisplayClose();
        DropdownMenuFilterComponent_1.DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();
        if (this._CourierWorksheet.DocumentStatusCode != null) {
            switch (this._CourierWorksheet.DocumentStatusCode) {
                case "M":
                case "X": {
                    this.IsDocumentStatusRed = true;
                    break;
                }
                case "V": {
                    this.IsDocumentStatusGreen = true;
                    break;
                }
                case "I": {
                    this.IsDocumentStatusBlue = true;
                    break;
                }
            }
        }
        if (this._CourierWorksheet.CourierManifestStatusCode != null) {
            switch (this._CourierWorksheet.CourierManifestStatusCode) {
                case "M":
                case "X": {
                    this.IsManifestStatusRed = true;
                    break;
                }
                case "V": {
                    this.IsManifestStatusGreen = true;
                    break;
                }
                case "I": {
                    this.IsManifestStatusBlue = true;
                    break;
                }
                case "R": {
                    this.IsManifestStatusOrange = true;
                    break;
                }
            }
        }
        if (this._CourierWorksheet.CourierDeclarationStatusCode != null) {
            switch (this._CourierWorksheet.CourierDeclarationStatusCode) {
                case "M":
                case "X": {
                    this.IsDeclarationStatusRed = true;
                    break;
                }
                case "V": {
                    this.IsDeclarationStatusGreen = true;
                    break;
                }
                case "I": {
                    this.IsDeclarationStatusBlue = true;
                    break;
                }
                case "R": {
                    this.IsDeclarationStatusOrange = true;
                    break;
                }
            }
        }
        if (this._CourierWorksheet.CourierPaymentStatusCode != null) {
            switch (this._CourierWorksheet.CourierPaymentStatusCode) {
                case "R": {
                    this.IsPaymentStatusOrange = true;
                    break;
                }
                case "P": {
                    this.IsPaymentStatusBlueChecked = true;
                    break;
                }
                case "I": {
                    this.IsPaymentStatusBlue = true;
                    break;
                }
                case "O": {
                    this.IsPaymentStatusGreen = true;
                    break;
                }
            }
        }
        if (this._CourierWorksheet.HighLowValue == "H"
            || (this._CourierWorksheet.HighLowValue == "L" && this._CourierWorksheet.CourierCustomStatusCode == "2")) {
            this.IsHighLow = true;
        }
        else {
            this.IsHighLow = false;
        }
        if (this._CourierWorksheet.CourierCustomStatusCode == "2") {
            this.SuspentionReasonTip = this._CourierWorksheet.CourierSuspentionName;
        }
        this.SuspentionReasonText = this._CourierWorksheet.CourierCustomStatusName;
        this.BuildDeclarationsCheckBox();
        this.CD.detectChanges();
    };
    CourierWorksheetListTemplate.prototype.BuildDeclarationsCheckBox = function () {
        if (this._CourierWorksheetSharedDataService._SelectedItems.Collection.includes(this._CourierWorksheet.DeclarationId)) {
            this.IsDeclarationChecked = true;
        }
        else {
            this.IsDeclarationChecked = false;
        }
    };
    CourierWorksheetListTemplate.prototype.SendManifest = function (event) {
        var _this = this;
        this.ButtonClick(event);
        var myDeclarationPMService = new DeclarationPMService_1.DeclarationPMService();
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
            .subscribe(function (rsptPMget) {
            var entitypm = rsptPMget.Result;
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
            var _SendManifestService = new SendManifestComponent_1.SendManifestService();
            _SendManifestService.Run({ EntityPM: entitypm, ObjectTable: objectTable, CourierWorksheetmode: true });
            _SendManifestService.OnSuccessSendMethod =
                function (res1) {
                    _this.CurrentSession.StopBusyIndicator();
                    //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                    _this.RefreshData();
                };
            _SendManifestService.OnCustomSendOptionsButtonClick({
                RequestVIA: RequestParamsBase_1.SendRequestVIA.WebServiceInteractive,
                Option: "WI",
                ForcePersonalSign: false
            });
        });
    };
    CourierWorksheetListTemplate.prototype.SendButtonClicked = function () {
        this.ButtonClick(null);
    };
    CourierWorksheetListTemplate.prototype.SendDec = function (event) {
        var _this = this;
        //event.stopPropagation();
        //SplitButtonComponent.EnsureLastSplitButtonIsClosed();
        //DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();
        this.ButtonClick(event);
        var myDeclarationPMService = new DeclarationPMService_1.DeclarationPMService();
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
            .subscribe(function (rsptPMget) {
            var entitypm = rsptPMget.Result;
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
            var _SendDeclarationService = new SendDeclarationComponent_1.SendDeclarationService();
            _SendDeclarationService.Run({ EntityPM: entitypm, ObjectTable: objectTable, CourierWorksheetmode: true });
            _SendDeclarationService.OnSuccessSendMethod =
                function (res1) {
                    _this.CurrentSession.StopBusyIndicator();
                    //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                    _this.RefreshData();
                };
            _SendDeclarationService.OnCustomSendOptionsButtonClick({
                RequestVIA: RequestParamsBase_1.SendRequestVIA.WebServiceInteractive,
                Option: "WI",
                ForcePersonalSign: false
            });
        });
    };
    CourierWorksheetListTemplate.prototype.ButtonClick = function (event) {
        this._CourierWorksheetSharedDataService.SupperssOnRowSelectedAction = true;
        //event.stopPropagation();
        //this.RowSelect()
        this.DropdownDisplayClose(); //this.MySplitButtonComponent.DropdownDisplayClose();//SplitButtonComponent.EnsureLastSplitButtonIsClosed();
        //DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();
    };
    Object.defineProperty(CourierWorksheetListTemplate.prototype, "IsWebAPICourierGWMessageECTHRDataMamanEnable", {
        get: function () { return this._CourierWorksheetSharedDataService.IsWebAPICourierGWMessageECTHRDataMamanEnable; },
        enumerable: true,
        configurable: true
    });
    CourierWorksheetListTemplate.prototype.GetSendECTHRDataMaman = function (event) {
        var _this = this;
        this.ButtonClick(event);
        this.CurrentSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendECTHRDataMaman(this._CourierWorksheet['DeclarationId'])
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            var mess = "";
            if (res.HasError) {
                mess = res.ErrorsArray[0];
            }
            else {
                mess = res.Result;
            }
            myMessageWindow.Show(mess);
        });
    };
    CourierWorksheetListTemplate.prototype.SendPay = function (event) {
        var _this = this;
        this.ButtonClick(event);
        var BackButtonLabel = "תיק עמילות";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            //this.SelectionChanged(myDeclarationEditTab);
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: _this._CourierWorksheet['DeclarationId'],
                ObjectTableName: 'Customs.Declaration',
                BackButtonLabel: BackButtonLabel
            });
            var myEditComponent = cmpRef.instance;
            //let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
            //myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
            //  .subscribe(rsptPMget => {
            var sub = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(function (token1) {
                sub.unsubscribe();
                var entitypm = myEditComponent.EntityPM; //rsptPMget.Result;
                var objectTable = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
                var args = {
                    EntityPM: entitypm,
                };
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 1000;
                logWindow.Height = 700;
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.TH.Payments");
                logWindow.WindowArgs = args;
                logWindow.ShowCloseButton = true;
                logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/DeclarationPaymentComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                    myEditComponent.BackButtonClicked();
                    _this.RefreshData();
                });
                //TODO | !TODO   ???? >>>>this.ActivateUnifreightInstruction();
            });
        });
    };
    CourierWorksheetListTemplate.prototype.PrepareDropdownMenuFilter = function (event, declarationId) {
        var _this = this;
        this._IsDropdownMenuFilterReady = false;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.addAdditionalFilter("DeclarationId", declarationId, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        this._DeclarationMamanSpecialActionListService.getByFilters(filters).subscribe(function (response) {
            _this.IsReceivingDelayCertificate = false;
            _this.IsPrintDocuments = false;
            _this.MamanStickerDetails = null;
            if (!response.HasError && response.Result != null) {
                response.Result.forEach(function (declarationMamanSpecialActionPMItem) {
                    switch (declarationMamanSpecialActionPMItem.MamanSpecialActionCode) {
                        case "2": {
                            if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                _this.IsReceivingDelayCertificate = true;
                            }
                            break;
                        }
                        case "4": {
                            _this.MamanStickerDetails = declarationMamanSpecialActionPMItem;
                            if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                _this.IsMamanSticker = true;
                            }
                            break;
                        }
                        case "5": {
                            if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                _this.IsPrintDocuments = true;
                            }
                            break;
                        }
                    }
                });
            }
            _this._IsDropdownMenuFilterReady = true;
            _this.CD.detectChanges();
        });
        this.CD.detectChanges();
    };
    CourierWorksheetListTemplate.prototype.CourierPendingReasonCommand = function (event, declarationId, mode) {
        var _this = this;
        this.ButtonClick(event);
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        var declarationIdList = [];
        this._DeclarationCourierStatusPMService.get(declarationId).subscribe(function (response) {
            if (!response.HasError) {
                declarationIdList.push(response.Result);
                windowArgs.DeclarationIdList = declarationIdList;
                windowArgs.CourierHawb = _this._CourierWorksheet.CourierHawb;
                windowArgs.Mode = mode;
                if (mode == "Delete") {
                    var confirm = new ConfirmWindow_1.ConfirmWindow();
                    confirm.Width = 350;
                    confirm.Height = 200;
                    confirm.Title = "מחיקת Pending";
                    confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
                    confirm.ShowNoButton = true;
                    confirm.Show("האם למחוק Pending?");
                    confirm.WindowClosed.subscribe(function (event) {
                        if (confirm.Yes) {
                            _this.DeletePending(response.Result);
                        }
                        confirm.Close();
                    });
                }
                else {
                    if (mode == "Update") {
                        windowArgs.CourierPendingReasonCode = _this._CourierWorksheet.CourierPendingReasonCode;
                        windowArgs.PendingRemarks = _this._CourierWorksheet.PendingRemarks;
                    }
                    logitudeWindow.Width = 450;
                    logitudeWindow.Height = 280;
                    logitudeWindow.IsShowCloseButton = false;
                    logitudeWindow.Title = "סימון ב Pending"; //TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/CourierPendingReasonGeneralComponent');
                    logitudeWindow.WindowClosed.subscribe(function ($event) {
                        _this.RefreshData();
                    });
                }
            }
        });
        this.CD.detectChanges();
    };
    CourierWorksheetListTemplate.prototype.DeletePending = function (declarationCourierStatusPM) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        declarationCourierStatusPM.CourierPendingReasonCode = null;
        declarationCourierStatusPM.PendingRemarks = null;
        this._DeclarationCourierStatusPMService.update(declarationCourierStatusPM).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            _this.RefreshData();
        });
    };
    CourierWorksheetListTemplate.prototype.OnCheckedWithSystemEvent = function (eventM) {
        eventM.stopPropagation();
        this.IsDeclarationChecked = !this.IsDeclarationChecked;
        //if (event.IsChecked) {
        if (this.IsDeclarationChecked) {
            if (!this._CourierWorksheetSharedDataService._SelectedItems.Collection.includes(this._CourierWorksheet.DeclarationId)) {
                this._CourierWorksheetSharedDataService._SelectedItems.Insert(this._CourierWorksheet.DeclarationId);
            }
        }
        else {
            var removedIndex = null;
            for (var i = 0; i < this._CourierWorksheetSharedDataService._SelectedItems.Collection.length; i++) {
                if (this._CourierWorksheet.DeclarationId == this._CourierWorksheetSharedDataService._SelectedItems.Collection[i]) {
                    removedIndex = i;
                    break;
                }
            }
            if (removedIndex != null) {
                this._CourierWorksheetSharedDataService._SelectedItems.RemoveFromIndex(removedIndex);
            }
        }
    };
    CourierWorksheetListTemplate.prototype.MamanStickerCommand = function (event, declarationId, mode) {
        var _this = this;
        this.ButtonClick(event);
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.DeclarationId = declarationId;
        windowArgs.EntityPM = this.MamanStickerDetails;
        windowArgs.Mode = mode;
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 280;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = "פרטי מדבקה"; //TextCodeTranslator.Translate("Customs.CourierMaster.O.StickerDetails");;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/MamanSpecialAction/AddEditMamanStickerComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            _this.RefreshData();
        });
        this.CD.detectChanges();
    };
    CourierWorksheetListTemplate.prototype.SendMamanSpecialAction = function (declarationId, actionCode, mamanSpecialActionCode) {
        var _this = this;
        var titleText = "מסר פעולות מיוחדות";
        var questionText = "אשר שליחת מסר ביטול פעולה מיוחדת";
        switch (mamanSpecialActionCode) {
            case "2": {
                if (actionCode == "U") {
                    titleText = "הפקת תעודת עיכוב";
                    questionText = "אשר שליחת מסר פעולה מיוחדת של תעודת עיכוב למסוף";
                }
                else if (actionCode == "C") {
                    titleText = "ביטול תעודת עיכוב";
                    questionText = "אשר שליחת מסר ביטול פעולה מיוחדת של תעודת עיכוב למסוף";
                }
                break;
            }
            case "4": {
                titleText = "ביטול הפקת מדבקה";
                questionText = "אשר שליחת מסר ביטול פעולה מיוחדת של הדפסת מדבקה";
                break;
            }
            case "5": {
                if (actionCode == "U") {
                    titleText = "הדפסת מסמכים";
                    questionText = "אשר שליחת מסר פעולה מיוחדת של הדפסת מסמכים";
                }
                else if (actionCode == "C") {
                    titleText = "ביטול הדפסת מסמכים";
                    questionText = "אשר שליחת מסר ביטול פעולה מיוחדת של הדפסת מסמכים";
                }
                break;
            }
        }
        var confirm = new ConfirmWindow_1.ConfirmWindow();
        confirm.Width = 350;
        confirm.Height = 200;
        confirm.Title = titleText;
        confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
        confirm.ShowNoButton = true;
        confirm.Show(questionText);
        confirm.WindowClosed.subscribe(function (event) {
            if (confirm.Yes) {
                _this._IsDropdownMenuFilterReady = false;
                _this.CurrentSession.StartBusyIndicatorCreating();
                if (actionCode == "U") {
                    var declarationMamanSpecialActionPM = new DeclarationMamanSpecialActionPM_1.DeclarationMamanSpecialActionPM();
                    declarationMamanSpecialActionPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    declarationMamanSpecialActionPM.DeclarationId = declarationId;
                    declarationMamanSpecialActionPM.MamanSpecialActionCode = mamanSpecialActionCode;
                    _this._DeclarationMamanSpecialActionPMService.insert(declarationMamanSpecialActionPM).subscribe(function (res) {
                        _this._DeclarationWebService.GetDeclarationMamanSpecialAction(declarationId, _this._CourierWorksheet.Tenant, "U", mamanSpecialActionCode)
                            .subscribe(function (myResponse) {
                            _this.CurrentSession.StopBusyIndicator();
                            var myMessageWindow = new MessageWindow_1.MessageWindow();
                            myMessageWindow.Show(myResponse.Result);
                        });
                    });
                }
                else {
                    _this._DeclarationWebService.GetDeclarationMamanSpecialAction(declarationId, _this._CourierWorksheet.Tenant, "C", mamanSpecialActionCode)
                        .subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        var myMessageWindow = new MessageWindow_1.MessageWindow();
                        myMessageWindow.Show(myResponse.Result);
                    });
                }
            }
            confirm.Close();
        });
    };
    CourierWorksheetListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CourierWorksheetListTemplate.html',
        }),
        __metadata("design:paramtypes", [CourierWorksheetSharedDataService_1.CourierWorksheetSharedDataService, core_1.ChangeDetectorRef])
    ], CourierWorksheetListTemplate);
    return CourierWorksheetListTemplate;
}());
exports.CourierWorksheetListTemplate = CourierWorksheetListTemplate;
//# sourceMappingURL=CourierWorksheetListTemplate.js.map