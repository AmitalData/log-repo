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
var CustomsMenuItem_1 = require("../../DataContract/CustomsMenuItem");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
//ConfirmWindow
var CommunicationLogStepListService_1 = require("../../../Common/Services/ExtendedLists/CommunicationLogStepListService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var CustomsRequestMenuService = /** @class */ (function () {
    function CustomsRequestMenuService() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.WindowClosed = new core_1.EventEmitter();
        ///alert("CustomsRequestMenuService");
        this.buildCustomsList();
    }
    Object.defineProperty(CustomsRequestMenuService.prototype, "CustomsRequestMenuItems", {
        get: function () { return this._CustomsRequestMenuItems; },
        enumerable: true,
        configurable: true
    });
    CustomsRequestMenuService.prototype.buildCustomsList = function () {
        this._CustomsRequestMenuItems = [];
        // <!> Abdullah: Fill 'CustomsMenuItem.ObjectTableName' if you want to open a query screen
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DeclarationRestoreQuery"), "DeclarationRestoreQuery", './CustomsModules/CustomsRequests/Components/DeclarationRestoreComponent', 850, 500, "8373"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.MorningMessageQuery"), "MorningMessage", './CustomsModules/CustomsGeneralRequests/Components/MorningMessageComponent', 800, 600, "0102"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendRequest"), "DeclarationStatusQuery", './CustomsModules/CustomsRequests/Components/DeclarationRequests/DeclarationStatusComponent', 550, 650, "8250"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CourierBOLQuery"), "CourierBOLQuery", './CustomsModules/CustomsGeneralRequests/Components/CourierBOLQueryComponent', 750, 500, "9022"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.GuaranteeCertificateFilterQuery"), "GuaranteeCertificateQuery", './CustomsModules/CustomsRequests/Components/TapagRequests/GuaranteeCertificateComponent', 850, 670, "8306"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.FaultQuery"), "FaultQuery", './CustomsModules/CustomsRequests/Components/TapagRequests/FaultQueryComponent', 750, 680, "8332"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WarehouseBlockBalance"), "WarehouseBlockBalanceQuery", './CustomsModules/CustomsRequests/Components/DeclarationRequests/WarehouseBlockBalanceComponent', 850, 690, "8328"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.MasterBOLQuery"), "MasterBOLQuery", './CustomsModules/CustomsGeneralRequests/Components/MasterBOLQueryComponent', 600, 590, "9020"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CurrencyExchangeRateQuery"), "ExchangeRateQuery", './CustomsModules/CustomsGeneralRequests/Components/ExchangeRatesQueryComponent', 650, 590, "8347"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CustomItemLegalDemandsQuery"), "CustomItemLegalDemandsQuery", './CustomsModules/CustomsGeneralRequests/Components/CustomItemLegalDemandsQueryComponent', 950, 630, "8316"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DeclarationPrintQuery"), "DeclarationPrintQuery", './CustomsModules/CustomsRequests/Components/DeclarationRequests/PrintRequestComponent', 500, 490, "8302"));
        // Vendors
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.Vendors"), "Vendors", '', 850, 500, "", "", null, "Customs.CustomsVendor"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.NewVendor"), "NewVendor", '', 850, 500, ""));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.SearchVendors"), "SearchVendor", '', 850, 500, ""));
        // Clientsd
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.Clients"), "Clients", '', 850, 500, "", "", null, "Customs.Client"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.NewClient"), "NewClient", './CustomsModules/CustomsClient/Components/NewClient/NewClientComponent', 850, 500, "Customs.Client")); //3610
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.ClientSearchByIDQuery"), "ClientSearchByID", './CustomsModules/CustomsGeneralRequests/Components/ClientSearchByIDComponent', 850, 800, "8343"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.BlockListInWarehouseQuery"), "BlockList", './CustomsModules/CustomsRequests/Components/DeclarationRequests/BlockListInWarehouseComponent', 900, 520, "8330"));
        //this._CustomsRequestMenuItems.push(new CustomsMenuItem(TextCodeTranslator.Translate("Customs.General.O.CargoQuery"), "MainfestStatus", '', 850, 500, ""));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CargoQuery"), "MainfestStatus", './CustomsModules/CustomsGeneralRequests/Components/CargoQueryRequestComponent', 1010, 680, "8240"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.RestoreMessage"), "RestoreMessage", './CustomsModules/CustomsGeneralRequests/Components/CustomsRestoreMessagesComponent', 540, 380, "")); // 9010 or 9011
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.ImporterDeclarationQuery"), "ImporterDeclarationQuery", './CustomsModules/CustomsGeneralRequests/Components/ImporterDeclarationComponent', 850, 800, "8326"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.BankAccountToRefundQuery"), "BankAccountToRefundQuery", './CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deposit/BankAccountToRefundComponent', 600, 420, "2018"));
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "RECALLSUPPLIER")) {
            this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RecallSuppliersFromFile"), "RecallSuppliersFromFile", '', 850, 500, ""));
        }
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DeficitFileFilterQuery"), "DeficitFileFilterQuery", './CustomsModules/CustomsGeneralRequests/Components/DeficitFileFilterComponent', 850, 8304, "8304"));
        //this._CustomsRequestMenuItems.push(new CustomsMenuItem(TextCodeTranslator.Translate("Customs.General.O.MasavPaymentsToAgentQuery"), "MasavPaymentsToAgentQuery", './CustomsModules/CustomsRequests/Components/PaymentOrderRequests/MasavPaymentsToAgentComponent', 830, 650, "8368"));
        var my8368 = new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.MasavPaymentsToAgentQuery"), "MasavPaymentsToAgentQuery", './CustomsModules/CustomsRequests/Components/PaymentOrderRequests/MasavPaymentsToAgentComponent', 830, 650, "8368");
        my8368.CanExportExcel = true;
        this._CustomsRequestMenuItems.push(my8368);
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.NewPaymentOrder"), "NewPaymentOrder", './CustomsModules/CustomsPaymentOrder/Components/NewEntity/NewPaymentOrderComponent', 420, 300, "3053"));
        var myCustomsMenuItem8305 = new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.GuaranteeFileFilterQuery"), "GuaranteeFileFilterQuery", './CustomsModules/CustomsRequests/Components/TapagRequests/GuaranteeFileFilterQueryComponent', 900, 670, "8305");
        myCustomsMenuItem8305.CanExportExcel = true;
        this._CustomsRequestMenuItems.push(myCustomsMenuItem8305);
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DeclarationFilterQuery"), "DeclarationFilterQuery", './CustomsModules/CustomsRequests/Components/TapagRequests/DeclarationFilterComponent', 850, 620, ""));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DeclarationReshimonConversion"), "DeclarationReshimonConversion", './CustomsModules/CustomsGeneralRequests/Components/DeclarationReshimonConversionComponent', 400, 300, ""));
        //string uri = Simplog.Infrastructure.App.Current.Host.Source.AbsoluteUri;
        //if (!uri.StartsWith("http://amitaliis.cloudapp.net/unifreightIIG/")) {
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CreditQuery"), "CreditQuery", './CustomsModules/CustomsGeneralRequests/Components/CreditLimitQueryComponent', 650, 610, "8289"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("שאילתא לתקרת זהב", "CreditGoldQuery", './CustomsModules/CustomsGeneralRequests/Components/GoldCreditLimitQueryComponent', 850, 610, "8289Z"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PaymentQuery"), "Payments", './CustomsModules/CustomsRequests/Components/PaymentOrderRequests/PaymentOrderQueryComponent', 950, 650, "8285"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.SpecialActivityRequest"), "SpecialActivityRequest", './CustomsModules/CustomsGeneralRequests/Components/SpecialActivityRequestComponent', 920, 680, "40"));
        //this._CustomsRequestMenuItems.push(new CustomsMenuItem(TextCodeTranslator.Translate("Customs.General.O.SendClaim"), "SendClaim", '', 850, 500, "")); // Task 29851
        //}
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("שאילתא להצהרה יצוא", "ExportDeclarationDataRequest", './CustomsModules/CustomsRequests/Components/DeclarationRequests/ExportDeclarationDataComponent', 700, 680, "9070"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.ClaimFileFilterQuery"), "ClaimFileFilter", './CustomsModules/CustomsRequests/Components/ClaimRequests/ClaimFileFilterComponent', 870, 720, "8244"));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CustomsBookQuery"), "CustomsBookQuery", './CustomsModules/CustomsGeneralRequests/Components/CustomsBookQueryComponent', 850, 500, "8361"));
        //this._CustomsRequestMenuItems.push(new CustomsMenuItem(TextCodeTranslator.Translate("Customs.General.O.RecallSuppliersFromFile"), "RecallSuppliersFromFile", './CustomsModules/CustomsGeneralRequests/Components/RecallSuppliersFromFileComponent', 500, 400, ""));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("שליחת צרופה", "AddAttachmentResponse", './CustomsModules/CustomsGeneralRequests/Components/AddAttachmentResponseComponent', 430, 300, "2715", null, null, null, true));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("", "PaymentOrderReply", './CustomsModules/CustomsRequests/Components/PaymentOrderRequests/PaymentOrderReplyComponent', 730, 550, "3050", null, null, null, true));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("", "RequiredDocument", './CustomsModules/CustomsGeneralRequests/Components/RequiredDocumentComponent', 400, 410, "8227", null, null, null, true));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("", "RequiredDocument", './CustomsModules/CustomsGeneralRequests/Components/RequiredDocumentComponent', 400, 410, "8228", null, null, null, true));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("קליטת זמינויות", "StorageEntranceComponent", './CustomsModules/CustomsRequests/Components/Courier/StorageEntranceComponent', 800, 500, ""));
        this._CustomsRequestMenuItems.push(new CustomsMenuItem_1.CustomsMenuItem("מסר התרה לתיק", "ReleaseGoods", './CustomsModules/CustomsRequests/Components/DeclarationRequests/ReleaseGoodsComponent', 1010, 610, "2470", null, null, null, true));
    };
    CustomsRequestMenuService.prototype.ShowModalByIdAndIntreface = function (id, InterfaceTypeCode, RequestDescription) {
        if (Tools_1.AppTool.IsNullOrEmpty(id)) {
            console.warn("ShowModalByIdAndIntreface():id is must !!");
            return;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(InterfaceTypeCode)) {
            console.warn("ShowModalByIdAndIntreface():InterfaceTypeCode is must !!");
            return;
        }
        var list = this._CustomsRequestMenuItems.filter(function (r) { return r.MainInterfaceCode == InterfaceTypeCode; });
        if (list.length == 0) {
            ///console.warn("InterfaceTypeCode ${InterfaceTypeCode} not registered");
            this.ShowModalDefault(id, InterfaceTypeCode, RequestDescription);
            return;
        }
        this.ShowModal(list[0], id, null);
    };
    CustomsRequestMenuService.prototype.ShowModalAsEditMenuAction = function (InterfaceTypeCode, menuArg) {
        if (Tools_1.AppTool.IsNullOrEmpty(InterfaceTypeCode)) {
            console.warn("ShowModalByIdAndIntreface():InterfaceTypeCode is must !!");
            return;
        }
        var list = this._CustomsRequestMenuItems.filter(function (r) { return r.MainInterfaceCode == InterfaceTypeCode; });
        if (list.length == 0) {
            console.warn("InterfaceTypeCode ${InterfaceTypeCode} not registered");
            //this.ShowModalDefault(id, InterfaceTypeCode, RequestDescription);
            return;
        }
        this.ShowModal(list[0], null, menuArg);
    };
    CustomsRequestMenuService.prototype.ShowModalDefault = function (id, InterfaceTypeCode, RequestDescription) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 850;
        logitudeWindow.Height = 500;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Title = RequestDescription;
        var reqJson = "";
        var resJson = "";
        this.CurrentSession.StartBusyIndicator("");
        var myCommunicationLogStepListService = new CommunicationLogStepListService_1.CommunicationLogStepListService();
        //logId=1-212245&tenant=1
        var ary = [20, 30];
        var suppressHugeDataFeature = true;
        myCommunicationLogStepListService.GetCommunicationLogStepsDocumentDataBystringStepFilter(InterfaceTypeCode, id, SessionLocator_1.SessionLocator.Tenant, ary, suppressHugeDataFeature)
            .subscribe(function (response) {
            var myData = response.Result;
            var req = myData[0];
            var res = myData[1];
            reqJson = req.DocumentData;
            resJson = res.DocumentData;
            _this.CurrentSession.StopBusyIndicator();
            if (suppressHugeDataFeature && reqJson == "(item.DocumentData.Length * sizeof(Char) > sizeOf250KB)") {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show("Answer message is too large to be displayed, To open browser instead?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.ViewXMLClicked(req);
                    }
                });
                return;
            }
            logitudeWindow.WindowArgs = {
                'AnalyzeMessage': JSON.parse(resJson),
                'CustomResponse': JSON.parse(reqJson),
            };
            logitudeWindow.Show('./CustomsModules/CustomControls/Components/ObjectViewerComponent');
        });
    };
    //ViewXMLClicked(item) {
    //    var documentName: string =  item.DocumentId;
    //    //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
    //    //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(link);
    //    //    return;
    //    //}
    //    if (item.SecurityId) {
    //         DownloadManager.DownloadPage("",item.SecurityId);
    //    }
    //    else {
    //        documentName = item.DocumentId;
    //         DownloadManager.DownloadPage(documentName);
    //    }
    //}
    CustomsRequestMenuService.prototype.ViewXMLClicked = function (item) {
        var link = "";
        var documentName = item.DocumentId;
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(link);
        //    return;
        //}
        if (item.SecurityId) {
            DownloadManager_1.DownloadManager.DownloadPage("", item.SecurityId);
        }
        else {
            DownloadManager_1.DownloadManager.DownloadPage(item.DocumentId, null);
        }
    };
    CustomsRequestMenuService.prototype.ShowModal = function (item, logId, menuArg) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(item.URLContent)) {
            return;
        }
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = item.WindowWidth; //850;
        logitudeWindow.Height = item.WindowHeight + 10; //500;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Title = item.TranslatedName; //"Declaration restore query";
        //logitudeWindow.Show('./CustomsModules/CustomsRequests/Components/DeclarationRestoreComponent');
        //MorningMessageComponent
        var myLogId = logId;
        myLogId = myLogId || item.DemoLogId;
        var isComponentLoaded = false;
        if (Tools_1.AppTool.IsNullOrEmpty(myLogId)) {
            if (menuArg) {
                isComponentLoaded = true;
                logitudeWindow.ComponentLoaded.subscribe(function (compo) {
                    var myRequestsSheetMassagingView = compo;
                    if (myRequestsSheetMassagingView) {
                        myRequestsSheetMassagingView.MyCustomsMenuItem = item;
                        if (!Tools_1.AppTool.IsNullOrEmpty(menuArg)) {
                            try {
                                var myRequestsSheetMassagingViewAny = myRequestsSheetMassagingView;
                                myRequestsSheetMassagingViewAny.SetMenuArg(menuArg);
                            }
                            catch (err) {
                                console.warn("! SetMenuArg(menuArg)");
                            }
                        }
                        logitudeWindow.WindowClosed.subscribe(function (anyString) {
                            myRequestsSheetMassagingView.DisposeMyState();
                            _this.WindowClosed.emit(anyString);
                        });
                    }
                });
            }
            if (!isComponentLoaded) {
                logitudeWindow.ComponentLoaded.subscribe(function (compo) {
                    var myRequestsSheetMassagingView = compo;
                    if (myRequestsSheetMassagingView) {
                        myRequestsSheetMassagingView.MyCustomsMenuItem = item;
                    }
                });
            }
            logitudeWindow.Show(item.URLContent);
            return;
        }
        var reqJson = "";
        var resJson = "";
        this.CurrentSession.StartBusyIndicator("");
        var myCommunicationLogStepListService = new CommunicationLogStepListService_1.CommunicationLogStepListService();
        //logId=1-212245&tenant=1
        myCommunicationLogStepListService.getCommunicationLogStepsRequestParamResponseData(item.MainInterfaceCode, myLogId, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (response) {
            var myData = response.Result;
            var req = myData[0];
            var res = myData[1];
            reqJson = req.DocumentData;
            //alert(reqJson);
            resJson = res.DocumentData;
            //alert(resJson);
            _this.CurrentSession.StopBusyIndicator();
            _this.ShowAsRequestSheet(logitudeWindow, item, reqJson, resJson, menuArg, logId);
        });
    };
    CustomsRequestMenuService.prototype.ShowAsRequestSheet = function (logitudeWindow, item, reqJson, resJson, menuArg, logId) {
        var _this = this;
        var myRequestSheetState = item.requestSheetState || new CustomsMenuItem_1.RequestSheetState(true, true, false);
        logitudeWindow.ComponentLoaded.subscribe(function (compo) {
            var myRequestsSheetMassagingView = compo;
            if (myRequestsSheetMassagingView) {
                myRequestsSheetMassagingView.MyCustomsMenuItem = item;
                myRequestsSheetMassagingView.MyCommunicationLogId = logId;
                myRequestsSheetMassagingView.CustomRequestContentIsDisable = myRequestSheetState.CustomRequestContentIsDisable;
                myRequestsSheetMassagingView.CustomResponseContentIsDisable = myRequestSheetState.CustomRequestContentIsDisable;
                myRequestsSheetMassagingView.CustomSendOptionsButtonIsDisable = myRequestSheetState.CustomSendOptionsButtonIsDisable;
                myRequestsSheetMassagingView.MassageDisplay(reqJson, resJson);
                if (!Tools_1.AppTool.IsNullOrEmpty(menuArg)) {
                    try {
                        var myRequestsSheetMassagingViewAny = myRequestsSheetMassagingView;
                        myRequestsSheetMassagingViewAny.SetMenuArg(menuArg);
                    }
                    catch (err) {
                        console.warn("! SetMenuArg(menuArg)");
                    }
                }
                logitudeWindow.WindowClosed.subscribe(function (anyString) {
                    myRequestsSheetMassagingView.DisposeMyState();
                    _this.WindowClosed.emit("");
                });
            }
        });
        logitudeWindow.Show(item.URLContent);
    };
    CustomsRequestMenuService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustomsRequestMenuService);
    return CustomsRequestMenuService;
}());
exports.CustomsRequestMenuService = CustomsRequestMenuService;
//# sourceMappingURL=CustomsRequestMenuService.js.map