"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Tools_1 = require("../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var AmitalGatewayUtil_1 = require("../../../Infrastructure/Utilities/AmitalGatewayUtil");
var UnifreightController_1 = require("../../Controller/UnifreightController");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var DeclarationPMService_1 = require("../../Services/StandardPMs/DeclarationPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationCourierStatusPMService_1 = require("../../Services/StandardPMs/DeclarationCourierStatusPMService");
var CustomsRequestMenuService_1 = require("../../Services/Others/CustomsRequestMenuService");
var IIGGeneralMessagesService_1 = require("../../Services/WebServices/IIGGeneralMessagesService");
var CustomFileCreditRequestParams_1 = require("../../DataContract/RequestParams/CustomFileCreditRequestParams");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var DeclarationMessagesService_1 = require("../../Services/WebServices/DeclarationMessagesService");
var DeclarationWebService_1 = require("../../Services/WebServices/DeclarationWebService");
var DeclarationDisplayOnlyChecks_1 = require("../../Utilities/DeclarationDisplayOnlyChecks");
var VehicleReductionTypeListService_1 = require("../../Services/StandardLists/VehicleReductionTypeListService");
var MenuButtonsEvents_1 = require("../../../Infrastructure/Utilities/events/MenuButtonsEvents");
var PrintRequestRequestParams_1 = require("../../DataContract/RequestParams/PrintRequestRequestParams");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var DeclarationEventManager_1 = require("../../Utilities/DeclarationEventManager");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var DeclarationMenuButtonsHandler = /** @class */ (function () {
    function DeclarationMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        ///aaaaa: string = 10;
        //-----------------properties---------------------------//
        this.isValid = false;
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.checkTransfer = ""; // moran 4.8.16 - AMI-56804
        //------------------------------------------------------//
        //Services
        this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        this.declarationWebService = new DeclarationWebService_1.DeclarationWebService();
        this.declarationCourierStatusPMService = new DeclarationCourierStatusPMService_1.DeclarationCourierStatusPMService();
        this._DocumentDeclarationId = null;
        this._DeclarationNumberandVersionId = null;
        this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
    }
    DeclarationMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
        this.IdentityKey = Tools_1.AppTool.GetNewGuid();
    };
    //private _SubMenuButtonsStateChanged;
    //private _SubSaveCompleted;
    //private _SubLoadCompleted;
    //private _SubDisplayModeChanged;
    DeclarationMenuButtonsHandler.prototype.ngOnDestroy = function () {
        console.log("DeclarationMenuButtonsHandler:ngOnDestroy");
        //if (this._SubMenuButtonsStateChanged) {
        //    this._SubMenuButtonsStateChanged.unsubscribe();
        //    this._SubMenuButtonsStateChanged = null;
        //}
        //if (this._SubSaveCompleted) {
        //    this._SubSaveCompleted.unsubscribe();
        //    this._SubSaveCompleted = null;
        //}
        //if (this._SubLoadCompleted) {
        //    this._SubLoadCompleted.unsubscribe();
        //    this._SubLoadCompleted = null;
        //}
        //if (this._SubDisplayModeChanged) {
        //    this._SubDisplayModeChanged.unsubscribe();
        //    this._SubDisplayModeChanged = null;
        //}
    };
    DeclarationMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            //this._SubMenuButtonsStateChanged =
            this.CurrentSession.SubscriptionAdd(this.MenuButtonsStateChangedEvent = MenuButtonsEvents_1.MenuButtonsEvents.MenuButtonsStateChanged.subscribe(function (args) {
                if (!_this.IsDisplayOnly) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.CurrentSession.CurrentEditComponent.EditComponentController)) {
                        _this.IsDisplayOnly = _this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
                    }
                    _this.ApplyCheckMenuButtonsState(_this.MenuButtons);
                }
            }));
            //this._SubSaveCompleted =
            this.CurrentSession.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    switch (_this.MenuButtonCode) {
                    }
                }
            }));
            //this._SubLoadCompleted =
            this.CurrentSession.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            //this._SubDisplayModeChanged =
            this.CurrentSession.SubscriptionAdd(DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.subscribe(function (IsDisplayOnly) {
                _this.CheckButtonState(_this.MenuButtons);
            }));
        }
        //if (this.CurrentSession.CurrentEditComponent != null) {
        //    this.CurrentSession.CurrentEditComponent.MenuButtonsHandlerREF = this;
        //}
    };
    DeclarationMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        this.MenuButtons = menuButtons;
        this.DisplayOnlyCheck();
    };
    DeclarationMenuButtonsHandler.prototype.ApplyCheckMenuButtonsState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "More") {
                        button.IsDisabled = true;
                        button.IsHidden = true;
                    }
                    if (button.EventCode == "SendDeclaration") {
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                            button.IsHidden = false;
                        }
                        else {
                            button.IsDisabled = false;
                            button.IsHidden = false;
                        }
                    }
                    if (button.EventCode == "SendManifest") {
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                            //  button.IsHidden = false;
                        }
                        else {
                            button.IsDisabled = false;
                            // button.IsHidden = false;
                        }
                        if (this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = false;
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "DeclarationPayment") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                        button.Width = 120;
                    }
                    if (button.EventCode == "Forms") {
                        button.Width = 60;
                    }
                    if (button.EventCode == "PrintDeclarationForm") {
                        //if (!declaration.HasDocument)
                        //if (AppTool.IsNullOrEmpty(this.EntityPM.DocumentDeclarationId)) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                    }
                    if (button.EventCode == "Actions") {
                        button.Width = 70;
                    }
                    if (button.EventCode == "PrintRelease") // moran 29.2.16 - Task 19807
                     {
                        //if (this.EntityPM.IsReleaseFile && !AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                        if (this.EntityPM.IsReleaseFile && AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "PrintTzrufa") // moran 2.3.16 - Task 19807
                     {
                        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
                            //if (!this.EntityPM.IsAccumulated) {
                            //    button.IsDisabled = false;
                            //} else {
                            //    button.IsDisabled = true;
                            //}
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "PrintAccumaltedTzrufa") {
                        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
                            if (this.EntityPM.IsAccumulated) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "TransferToCollector") // moran 4.8.16 - AMI-56804
                     {
                        if (this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                        else {
                            if (this.checkTransfer == "1") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                    }
                    if (button.EventCode == "Vehicle Modifications") {
                        if (this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ResetDeclarationNumber") { //Eitan H 26/11/17 34387
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                            button.IsHidden = false;
                        }
                        else {
                            button.IsDisabled = false;
                            button.IsHidden = false;
                        }
                    }
                    if (button.EventCode == "CourierPendingReason") {
                        if (!this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "CourierPendingReasonDel") {
                        if (!this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "Declaration Closure") {
                        if (this.EntityPM.IsClose) {
                            button.IsHidden = true;
                        }
                        else {
                            button.IsHidden = false;
                        }
                    }
                    if (button.EventCode == "Cancel Declaration Closure") {
                        if (!this.EntityPM.IsClose) {
                            button.IsHidden = true;
                        }
                        else {
                            button.IsHidden = false;
                        }
                    }
                }
                this.IsDisplayOnlyCheckDone = true;
                return menuButtons;
            }
        }
    };
    DeclarationMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        if (true) { //!this.isButtonClicked) { this is temporary for testing.
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                // save changes
                this.CurrentSession.StartBusyIndicatorSaving();
                this.declarationPMService.update(this.EntityPM).subscribe(function (response) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.CurrentSession.StopBusyIndicator();
                    _this.MenuButtonClickDo();
                });
            }
            else {
                this.MenuButtonClickDo();
            }
        }
    };
    DeclarationMenuButtonsHandler.prototype.MenuButtonClickDo = function () {
        var _this = this;
        if (true) { //this.isValid) { this is also for testing temp of course
            switch (this.MenuButtonCode) {
                case "SendDeclaration":
                    {
                        ////SendDeclaration();
                        // SendDeclaration(declarationViewModel);
                        break;
                    }
                case "DeclarationReset":
                    {
                        //InvokeOperation < string > op = declarationViewModel.Context.ResetDeclarationNumber(declaration.Id, declaration.Tenant);
                        //op.Completed += op_ResetDeclarationNumberCompleted;
                    }
                    break;
                case "DeclarationPayment":
                    {
                        this.OpenPaymentOrderWindow();
                        break;
                    }
                case "PrintTzrufa":
                    {
                        this.PrintTzrufaMethod(false);
                        break;
                    }
                case "PrintAccumaltedTzrufa":
                    {
                        this.PrintTzrufaMethod(true);
                        break;
                    }
                case "PrintDeclarationForm":
                    {
                        this.PrintDeclarationFormMethod(); //declarationViewModel);
                        break;
                    }
                case "DeclarationsStatusRequest":
                    {
                        this.DeclarationsStatusRequestMethod();
                        //SaveDeclarationMethod("DeclarationsStatusRequest");
                        break;
                    }
                case "DeclarationRestore":
                    {
                        this.DeclarationRestoreMethod(); //SaveDeclarationMethod("DeclarationRestore");
                        break;
                    }
                case "ResetDeclarationNumber":
                    {
                        this.ResetDeclarationNumberMethod();
                        var toDo = false;
                        if (toDo) {
                            this.CurrentSession.StartBusyIndicator("");
                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
                            myIIGGeneralMessagesService.GetResetDeclarationNumber(this.EntityPM.Id, this.EntityPM.Tenant)
                                .subscribe(function (myServiceResponse) {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                _this.CurrentSession.StopBusyIndicator();
                                if (myServiceResponse.HasError) {
                                    messageWindow.Show(myServiceResponse.ErrorsArray[0]);
                                }
                                else {
                                    if (myServiceResponse.Result != null) {
                                        messageWindow.Show(myServiceResponse.Result);
                                    }
                                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                }
                            });
                        }
                        break;
                    }
                case "Copy":
                    {
                        this.SaveDeclarationMethod("Copy");
                        break;
                    }
                case "PrintRelease": // moran 29.2.16 - Task 19807
                    {
                        this.PrintReleaseMethod();
                        break;
                    }
                // moran 5.6.16 - AMI-56804 - add TransferToCollector
                case "TransferToCollector":
                    {
                        // SaveDeclarationMethod("TransferToCollector");
                        this.TransferToCollectorMethod();
                        break;
                    }
                case "Vehicle Modifications":
                    {
                        this.DisplayDeclarationVehicleModificationsMethod();
                        break;
                    }
                case "SpecialActionRequest":
                    {
                        this.SpecialActionRequestMethod();
                        break;
                    }
                case "CourierPendingReason":
                    {
                        this.CourierPendingReasonMethod();
                        break;
                    }
                case "CourierPendingReasonDel":
                    {
                        this.CourierPendingReasonDeleteMethod();
                        break;
                    }
                case "Declaration Closure":
                    {
                        this.DeclarationClosureMethod();
                        break;
                    }
                case "Cancel Declaration Closure":
                    {
                        this.CancelDeclarationClosureMethod();
                        break;
                    }
            }
        }
    };
    DeclarationMenuButtonsHandler.prototype.DisplayDeclarationVehicleModificationsMethod = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myVehicleReductionTypeListService = new VehicleReductionTypeListService_1.VehicleReductionTypeListService();
        myVehicleReductionTypeListService.getAllFromCache().
            subscribe(function (res) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder", 0).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.CopyDeclaration");
                var windowArgs = {};
                windowArgs.EntityPM = _this.EntityPM;
                logWindow.WindowArgs = windowArgs;
                logWindow.ShowCloseButton = true;
                logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/VehicleModificationsComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                });
            });
        });
    };
    DeclarationMenuButtonsHandler.prototype.SaveDeclarationMethod = function (ActionName) {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            var confirm_1 = new ConfirmWindow_1.ConfirmWindow();
            confirm_1.WindowClosed.subscribe(function (event) {
                if (confirm_1.Yes) {
                    _this.CurrentSession.CurrentEditComponent.SaveChanges();
                    if (_this.EntityPM.SupplierInvoices.length == 0) {
                        _this.CopyMethod();
                    }
                    else {
                        var window_1 = new MessageWindow_1.MessageWindow();
                        window_1.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CantCopy"));
                    }
                }
            });
            confirm_1.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SaveDeclaration"));
        }
        else {
            if (this.EntityPM.SupplierInvoices.length == 0) {
                this.CopyMethod();
            }
            else {
                var window_2 = new MessageWindow_1.MessageWindow();
                window_2.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CantCopy"));
            }
        }
    };
    DeclarationMenuButtonsHandler.prototype.CopyMethod = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.DeclarationPM = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 900;
        logWindow.Height = 800;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CopyDeclaration");
        logWindow.WindowArgs = windowArgs;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationQueryComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.ReloadEntity($event);
        });
    };
    DeclarationMenuButtonsHandler.prototype.ReloadEntity = function (message) {
        if (message != "cancel") {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
    };
    DeclarationMenuButtonsHandler.prototype.ResetDeclarationNumberMethod = function () {
        var _this = this;
        this._DeclarationNumberandVersionId = null; //itzik:clear onstart on the house !!!
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.GetAnyRequest("2755", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
            .subscribe(function (response) {
            if (!response.HasError) {
                var requestSheets = response.Result;
                var haveRS2755 = false;
                if (requestSheets == null || requestSheets.length == 0) {
                }
                else {
                    haveRS2755 = true;
                }
                if (haveRS2755) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.Title = "איפוס מספר הצהרה";
                    messageWindow.Show("לא ניתן לאפס מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                    return;
                }
                var confirm_2 = new ConfirmWindow_1.ConfirmWindow();
                confirm_2.WindowClosed.subscribe(function (event) {
                    if (confirm_2.Yes) {
                        _this.EntityPM.ResetDeclarationNumber = true;
                        _this.EntityPM.DeclarationNumber = null;
                        _this.EntityPM.VersionId = null;
                        _this.EntityPM.IsSignedVersion = false;
                        _this.EntityPM.DeclarationStatusTypeCode = null;
                        _this.EntityPM.DeclarationNumberandVersionId = null;
                        if (_this.EntityPM.IsCourierDeclaration) { //Reset Courier Fields
                            _this.EntityPM.CourierCustomStatusCode = null;
                            _this.EntityPM.CourierSuspentionReasonCode = null;
                            //this.EntityPM.AcceptanceStatusCode = null;
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.ExternalDeclarationNumber)) {
                            if (_this.EntityPM.ExternalDeclarationNumber.includes("-")) {
                                var index = _this.EntityPM.ExternalDeclarationNumber.indexOf("-");
                                var var1 = _this.EntityPM.ExternalDeclarationNumber.substring(index + 1);
                                var var2 = //int.Parse(var1)
                                 Number(var1) + 1;
                                _this.EntityPM.ExternalDeclarationNumber = _this.EntityPM.ExternalDeclarationNumber.substring(0, index + 1) + var2.toString();
                            }
                            else {
                                _this.EntityPM.ExternalDeclarationNumber = _this.EntityPM.ExternalDeclarationNumber + "-1";
                            }
                            var token_1 = _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSave) {
                                token_1.unsubscribe();
                                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                if (isSave) {
                                    _this._DeclarationNumberandVersionId = null;
                                    var window_3 = new MessageWindow_1.MessageWindow();
                                    window_3.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationReset"));
                                }
                            });
                            _this.CurrentSession.CurrentEditComponent.SaveChanges();
                        }
                    }
                });
                confirm_2.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ResetDeclaration"));
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.TransferToCollectorMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        //confirmWindow.Show("אשר העברה לגובה");
        //confirmWindow.Unloaded += confirmWindow_Unloaded;
        //confirmWindow.cancelButton.Visibility = Visibility.Collapsed;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.TransferToCollector"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.ActualSendToTransfer();
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.ActualSendToTransfer = function () {
        var _this = this;
        var objecttable //: ObjectTablePM = //window.ObjectTable.Where(d => d.Name == "Customs.Declaration").FirstOrDefault();
         = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        var searchParams = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingObjectTableId = objecttable.Id;
        searchParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        searchParams.RequestName = "Send Transfer Request";
        searchParams.ResponseName = "Get Transfer Response";
        searchParams.Mode = "Transfer";
        //searchParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        //ForcePersonalSign = _ForcePersonalSign,
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(searchParams.PBId, 5, true);
        var declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        declarationMessagesService.PostSendTransferRequest(searchParams)
            .subscribe(function (myServiceResponse) {
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
            var responseData = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(CustomMessageProgressComponent_1.CustomMessageProgressComponent.CurrCustomMessageProgressHelper)) {
                CustomMessageProgressComponent_1.CustomMessageProgressComponent.CurrCustomMessageProgressHelper.MessageArrived = true;
            }
            _this.CurrentSession.StopBusyIndicator();
            //this.AnalyzeActualSendToTransfer(result);
            if (responseData.CreditStatus == "1") // moran 16.8.16 - AMI-57900
             {
                var confirmWindow_1 = new ConfirmWindow_1.ConfirmWindow();
                //confirmWindow.cancelButton.Visibility = Visibility.Collapsed;
                confirmWindow_1.Show(responseData.UserMessage);
                confirmWindow_1.WindowClosed.subscribe(function (event) {
                    if (!confirmWindow_1.Yes) {
                        _this.ActualSendToReTransfer();
                    }
                });
                return;
            }
            if (!responseData.HasException && responseData.Succeeded) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                if (_this.CurrentSession.CurrentWindow != null) {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            }
            else {
                if (!responseData.Succeeded && !Tools_1.AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    //confirmWindow.cancelButton.Visibility = Visibility.Collapsed;
                    messageWindow.Show(responseData.UserMessage);
                    return;
                }
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.ActualSendToReTransfer = function () {
        var _this = this;
        var objecttable //: ObjectTablePM = //window.ObjectTable.Where(d => d.Name == "Customs.Declaration").FirstOrDefault();
         = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        var searchParams = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingObjectTableId = objecttable.Id;
        searchParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        searchParams.RequestName = "Send ReTransfer Request";
        searchParams.ResponseName = "Get ReTransfer Response";
        searchParams.Mode = "ReTransfer";
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(searchParams.PBId, 5, true);
        var declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        declarationMessagesService.PostSendTransferRequest(searchParams)
            .subscribe(function (myServiceResponse) {
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
            var responseData = myServiceResponse.Result;
            //this.AnalyzeResponseMessageForsendToReTransfer(responseData);
            if (!responseData.HasException && responseData.Succeeded) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                if (_this.CurrentSession.CurrentWindow != null) {
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    //message = "Send ReTransfer Request Failed";
                }
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.AnalyzeResponseMessageForsendToReTransfer = function (responseData) {
        var message = "";
        if (responseData != null) {
            //message = responseData.UserMessage;
            if (Tools_1.AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                if (!responseData.HasException && responseData.Succeeded) {
                    message = "Send ReTransfer Request Succeeded";
                }
                else {
                    message = "Send ReTransfer Request Failed";
                }
            }
        }
        else {
            message = "Service returned a null response!";
        }
        return message;
    };
    DeclarationMenuButtonsHandler.prototype.DeclarationsStatusRequestMethod = function () {
        var _this = this;
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        var my = {
            "DeclarationNumber": this.EntityPM.DeclarationNumber,
            "CustomsFile": this.EntityPM.CustomFileNo,
            "DeclarationId": this.EntityPM.Id,
        };
        customsRequestMenuService.WindowClosed.subscribe(function (myarg) { _this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); });
        customsRequestMenuService.ShowModalAsEditMenuAction("8250", my);
    };
    DeclarationMenuButtonsHandler.prototype.SpecialActionRequestMethod = function () {
        var _this = this;
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        var my = {
            "DeclarationNumber": this.EntityPM.DeclarationNumber,
            "CustomFileNo": this.EntityPM.CustomFileNo,
            "DeclarationId": this.EntityPM.Id,
        };
        customsRequestMenuService.WindowClosed.subscribe(function (myarg) { _this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); });
        customsRequestMenuService.ShowModalAsEditMenuAction("40", my);
    };
    DeclarationMenuButtonsHandler.prototype.DeclarationRestoreMethod = function () {
        var _this = this;
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.CheckIfRequestInProgress("2750", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
            .subscribe(function (response) {
            if (!response.HasError) {
                var requestSheets = response.Result;
                var haveRS2750 = false;
                if ((requestSheets == null || requestSheets.length == 0)
                    || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                    haveRS2750 = false;
                }
                else {
                    haveRS2750 = true;
                }
                if (haveRS2750) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.Title = "שיחזור מספר הצהרה";
                    messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הצהרה בתהליך ");
                    return;
                }
                var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
                declarationDisplayOnlyChecks.CheckIfRequestInProgress("2755", _this.EntityPM.CustomFileNo, _this.EntityPM.Tenant)
                    .subscribe(function (response) {
                    if (!response.HasError) {
                        var requestSheets = response.Result;
                        var haveRS2755 = false;
                        if ((requestSheets == null || requestSheets.length == 0)
                            || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                            haveRS2755 = false;
                        }
                        else {
                            haveRS2755 = true;
                        }
                        if (haveRS2755) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Width = 400;
                            messageWindow.Height = 150;
                            messageWindow.Title = "שיחזור מספר הצהרה";
                            messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                            return;
                        }
                        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
                        var my = {
                            "DeclarationNumber": _this.EntityPM.DeclarationNumber,
                            "CustomsFile": _this.EntityPM.CustomFileNo,
                            "DeclarationId": _this.EntityPM.Id,
                        };
                        customsRequestMenuService.WindowClosed.subscribe(function (myarg) { _this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); });
                        customsRequestMenuService.ShowModalAsEditMenuAction('8373', my);
                    }
                });
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.PrintDeclarationFormMethod = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this._DocumentDeclarationId) && this._DeclarationNumberandVersionId == this.EntityPM.DeclarationNumberandVersionId) {
            this.ShowDocumentDeclaration();
            return;
        }
        var myDeclarationWebService = new DeclarationWebService_1.DeclarationWebService();
        myDeclarationWebService
            .GetDocumentDeclarationId(this.EntityPM.Id)
            .subscribe(function (myResponse) {
            var myRes = myResponse.Result;
            _this._DocumentDeclarationId = myRes.DocumentDeclarationId;
            _this._DeclarationNumberandVersionId = myRes.DeclarationVersion;
            if (Tools_1.AppTool.IsNullOrEmpty(_this._DocumentDeclarationId)) {
                if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Width = 350;
                    msg.Show("There are no DeclarationNumber & Declaration form  Document (button IsDisabled)");
                    return;
                }
                else {
                    if (_this._DeclarationNumberandVersionId != _this.EntityPM.DeclarationNumberandVersionId) {
                        _this.CheckBeforeSendPrintRequest();
                        return;
                    }
                }
            }
            if (_this._DeclarationNumberandVersionId != _this.EntityPM.DeclarationNumberandVersionId) {
                _this.CheckBeforeSendPrintRequest();
                return;
            }
            _this.ShowDocumentDeclaration();
        });
    };
    DeclarationMenuButtonsHandler.prototype.CheckBeforeSendPrintRequest = function () {
        var _this = this;
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.CheckIfGeneralRequestInProgress("8302", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
            .subscribe(function (response) {
            if (!response.HasError) {
                var requestSheets = response.Result;
                var haveRS8302 = false;
                if ((requestSheets == null || requestSheets.length == 0)
                    || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                    haveRS8302 = false;
                }
                else {
                    haveRS8302 = true;
                }
                if (haveRS8302) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.Title = "טופס הצהרה";
                    messageWindow.Show("לא ניתן להציג טופס הצהרה ,קיימת בקשה דומה בתהליך ");
                    return;
                }
                _this.SendPrintRequest();
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.SendPrintRequest = function () {
        var _this = this;
        var declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        var currRequestParams = new PrintRequestRequestParams_1.PrintRequestRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        //currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        //currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.IsSearchByDeclarationRadio = true;
        currRequestParams.IsSearchByCargoRadio = false;
        currRequestParams.DeclarationNumber = [];
        currRequestParams.DeclarationNumber.push(this.EntityPM.DeclarationNumber);
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא להדפסת הצהרה", true)
            .then(function (res) {
            var sub = _this.CurrentSession.CurrentEditComponent.LoadCompleted
                .subscribe(function (succ) {
                sub.unsubscribe();
                var myDeclarationWebService = new DeclarationWebService_1.DeclarationWebService();
                myDeclarationWebService
                    .GetDocumentDeclarationId(_this.EntityPM.Id)
                    .subscribe(function (myResponse) {
                    var myRes = myResponse.Result;
                    _this._DocumentDeclarationId = myRes.DocumentDeclarationId;
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this._DocumentDeclarationId)) {
                        _this.ShowDocumentDeclaration();
                    }
                });
            });
            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }).catch(function (err) {
            //?????
        });
        declarationMessagesService.PostPrintRequestRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    DeclarationMenuButtonsHandler.prototype.ShowDocumentDeclaration = function () {
        DownloadManager_1.DownloadManager.DownloadPage(this._DocumentDeclarationId);
    };
    DeclarationMenuButtonsHandler.prototype.PrintTzrufaMethod = function (IsAccumalated) {
        //<--- Yuval Chalup 29.07.2015 TASK-14849
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            //CustomDomainContext customDomainContext = new CustomDomainContext();
            ;
            var myUnifreightPrintStimulController = new UnifreightController_1.UnifreightController(//customDomainContext,
            this.EntityPM, "Logitude.Customs.MenuButtonHandlers.DeclarationMenuButtonsHandler.MyUnifreightPrintStimulController");
            var TSRUFA = "TSRUFA";
            if (IsAccumalated) {
                TSRUFA = "AccumalatedTSRUFA";
            }
            myUnifreightPrintStimulController.SendRequestPrintStimulToUnifreightAsync(TSRUFA);
            myUnifreightPrintStimulController.GetPromise().
                then(function (e) {
                var UnifreightResponseStatus = e.UnifreightResponseStatus;
                var UnifreightMessage = e.UnifreightMessage;
            });
        }
        else {
            var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
            var uri = Tools_1.AppTool.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=PrintTzrufa&entityId=" + this.EntityPM.Id + "&tempId=" + token;
            var win = window.open(uri, '_blank');
            win.focus();
        }
        ///Yuval Chalup 29.07.2015 TASK-14849 --->
    };
    DeclarationMenuButtonsHandler.prototype.PrintReleaseMethod = function () {
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            var myUnifreightPrintStimulController = new UnifreightController_1.UnifreightController(this.EntityPM, "Logitude.Customs.MenuButtonHandlers.DeclarationMenuButtonsHandler.MyUnifreightPrintStimulController");
            myUnifreightPrintStimulController.SendRequestPrintStimulToUnifreightAsync("RELEASE");
            myUnifreightPrintStimulController.GetPromise().
                then(function (e) {
                var UnifreightResponseStatus = e.UnifreightResponseStatus;
                var UnifreightMessage = e.UnifreightMessage;
            });
        }
        else {
            var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
            var uri = Tools_1.AppTool.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=PrintRelease&entityId=" + this.EntityPM.Id + "&tempId=" + token;
            var win = window.open(uri, '_blank');
            win.focus();
        }
    };
    DeclarationMenuButtonsHandler.prototype.OpenPaymentOrderWindow = function () {
        var _this = this;
        if (this.EntityPM) {
            var args = {
                EntityPM: this.EntityPM,
            };
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 700;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.TH.Payments");
            logWindow.WindowArgs = args;
            logWindow.ShowCloseButton = true;
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/DeclarationPaymentComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });
            this.ActivateUnifreightInstruction();
        }
        else {
            console.log("No entityPM in menu buttons!!!");
        }
    };
    DeclarationMenuButtonsHandler.prototype.ActivateUnifreightInstruction = function () {
        var _this = this;
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            var myEnterViewUnifreightInstructionController = new UnifreightController_1.UnifreightController(this.EntityPM, "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyEnterViewUnifreightInstructionController");
            myEnterViewUnifreightInstructionController
                .GetPromise().then(function (e) {
                if (e.UnifreightResponseStatus) {
                    return;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
            myEnterViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_ENTER");
        }
    };
    DeclarationMenuButtonsHandler.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        }
        if (this.IsDisplayOnly) {
            this.ApplyCheckMenuButtonsState(this.MenuButtons);
            return;
        }
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe(function (response) {
            if (!response.HasError) {
                var displayOnlyCheckResult = response.Result;
                _this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
                _this.ApplyCheckMenuButtonsState(_this.MenuButtons);
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.CourierPendingReasonMethod = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.Mode = "FromDeclaration";
        windowArgs.DeclarationId = this.EntityPM.Id;
        windowArgs.CourierHawb = this.EntityPM.MAWBCourierMaster;
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 280;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.MarkPending");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/CourierPendingReasonGeneralComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
        });
    };
    DeclarationMenuButtonsHandler.prototype.CourierPendingReasonDeleteMethod = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.declarationCourierStatusPMService.get(this.EntityPM.Id).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            var declarationCourierStatusPM = response.Result;
            if (declarationCourierStatusPM != null && (!Tools_1.AppTool.IsNullOrEmpty(declarationCourierStatusPM.CourierPendingReasonCode) || !Tools_1.AppTool.IsNullOrEmpty(declarationCourierStatusPM.PendingRemarks))) {
                var confirm = new ConfirmWindow_1.ConfirmWindow();
                confirm.Width = 350;
                confirm.Height = 200;
                confirm.Title = "מחיקת Pending";
                confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
                confirm.ShowNoButton = true;
                confirm.Show("האם למחוק Pending?");
                confirm.WindowClosed.subscribe(function (event) {
                    if (confirm.Yes) {
                        _this.CurrentSession.StartBusyIndicatorSaving();
                        declarationCourierStatusPM.CourierPendingReasonCode = null;
                        declarationCourierStatusPM.PendingRemarks = null;
                        _this.declarationCourierStatusPMService.update(declarationCourierStatusPM).subscribe(function (response) {
                            _this.CurrentSession.StopBusyIndicator();
                        });
                    }
                    confirm.Close();
                });
            }
            else {
                var window_4 = new MessageWindow_1.MessageWindow();
                window_4.Show(" Pending לא ניתן לבצע מחיקה, לתיק לא מוגדר ");
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.DeclarationClosureMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("האם ברצונך לסגור את ההצהרה ?"); //TextCodeTranslator.Translate("Customs.PhysicalCheck.O.IsClosePhysicalCheck"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.declarationWebService.DeclarationClosureMethod(_this.EntityPM.Id, _this.EntityPM.Tenant)
                    .subscribe(function (response) {
                    console.log("[response] DeclarationClosureMethod: ", response);
                    if (!response.HasError) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 300;
                        messageWindow.Height = 180;
                        messageWindow.Show("ההצהרה נסגרה בהצלחה"); //TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                    }
                });
            }
        });
    };
    DeclarationMenuButtonsHandler.prototype.CancelDeclarationClosureMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("האם ברצונך לבטל סגירת ההצהרה ?"); //TextCodeTranslator.Translate("Customs.PhysicalCheck.O.IsClosePhysicalCheck"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.declarationWebService.CancelDeclarationClosureMethod(_this.EntityPM.Id, _this.EntityPM.Tenant)
                    .subscribe(function (response) {
                    console.log("[response] CancelDeclarationClosureMethod: ", response);
                    if (!response.HasError) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 300;
                        messageWindow.Height = 180;
                        messageWindow.Show("ביטול סגירה בוצע בהצלחה"); //TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                    }
                });
            }
        });
    };
    return DeclarationMenuButtonsHandler;
}());
exports.DeclarationMenuButtonsHandler = DeclarationMenuButtonsHandler;
//# sourceMappingURL=DeclarationMenuButtonsHandler.js.map