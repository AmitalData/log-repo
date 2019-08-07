"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("./SessionLocator");
var Tools_1 = require("../Tools");
var core_1 = require("@angular/core");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var IIGGeneralMessagesService_1 = require("../../Customs/Services/WebServices/IIGGeneralMessagesService");
var TextCodeTranslator_1 = require("./TextCodeTranslator");
var AmitalGatewayUtil = /** @class */ (function () {
    function AmitalGatewayUtil() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.UnifaceRequestArrived = new core_1.EventEmitter();
        this._AmitalBrowserInUse = false;
        this.IsAmitalBackButtonDisable = false;
        this.IsTabCA23 = true; //the 1st tab ==> the default tab !!
        this.ShowDeclarationByIdReturnCloseSave = /** @class */ (function () {
            function class_1() {
            }
            class_1.StartDoIt = function (unifreightMessage, myEditTab, callback2TabZero) {
                var _this = this;
                //BackButtonLabel: "הצהרות ללא התרה"EntityId :"1-103991" ,ObjectTableName:"Customs.Declaration"
                var isSaved = false;
                var BackButtonLabel = "תיק עמילות";
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', myEditTab.SessionComponent.viewContainerRef)
                    .then(function (cmpRef) {
                    //this.SelectionChanged(myDeclarationEditTab);
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        EntityId: unifreightMessage.LogitudeEntityNumber,
                        ObjectTableName: unifreightMessage.LogitudeEntity,
                        BackButtonLabel: BackButtonLabel
                    });
                    var lockMess = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.LockedMessage");
                    var unifreightJumpTo = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.JumpTo");
                    console.log(lockMess);
                    var myEditComponent = cmpRef.instance;
                    if (!Tools_1.AppTool.IsNullOrEmpty(lockMess)) {
                        var sub_1 = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(function (token1) {
                            sub_1.unsubscribe();
                            var myDeclarationEditComponentController = myEditComponent.EditComponentController;
                            if (Tools_1.AppTool.IsNullOrEmpty(myDeclarationEditComponentController)) {
                                console.log("myDeclarationEditComponentController is null");
                            }
                            else {
                                myDeclarationEditComponentController.UnifaceStartAsLock(lockMess);
                            }
                        });
                    }
                    if (unifreightMessage.LogitudeCommandId == "ShowSupplierInvoiceSelectorByDecIdReturnChosenChildren") {
                        //"UnifreightEntity=CFIFILEM·;UnifreightEntityNumber=172900228·;LogitudeEntity=Customs.Declaration·;LogitudeEntityNumber=1-103927·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowSupplierInvoiceSelectorByDecIdReturnChosenChildren·;formtitle=חשבונית ספק"
                        //0042-17 //3822 
                        //cmpRef.instance.LoadCompleted.subscribe(loadSuccess => {
                        var sub_2 = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(function (token1) {
                            sub_2.unsubscribe();
                            var windowArgs = {};
                            var certificates = [];
                            windowArgs.DeclarationPM = myEditComponent.EntityPM;
                            windowArgs.Parent = {};
                            windowArgs.Parent.SelectedInvoiceItems = [];
                            windowArgs.Parent.SelectedInvoices = [];
                            //windowArgs.Protest = this.protestPM;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Height = 700;
                            logWindow.Width = 1000;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.WindowClosed.subscribe(function ($event) {
                                //alert("WindowClosed : ");
                                //if ("ok" == $event)
                                console.log(windowArgs);
                                var mySelectedInvoiceItems;
                                mySelectedInvoiceItems = windowArgs.Parent.SelectedInvoiceItems;
                                if (!Tools_1.AppTool.IsNullOrEmpty(mySelectedInvoiceItems) &&
                                    !Tools_1.AppTool.IsNullOrEmpty(mySelectedInvoiceItems.Collection) &&
                                    mySelectedInvoiceItems.Collection.length > 0) {
                                    var mySelectedInvoiceItem = mySelectedInvoiceItems.Collection[0];
                                    var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
                                    myIIGGeneralMessagesService.GetLOGISUPPACC(mySelectedInvoiceItem.DeclarationId, mySelectedInvoiceItem.CounterKey, mySelectedInvoiceItem.LineNumber, mySelectedInvoiceItem.Tenant).subscribe(function (serviceResponse) {
                                        var xmlSupplierInvoiceSelector = serviceResponse.Result;
                                        //xmlSupplierInvoiceSelector = serviceResponse.
                                        _this.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack(xmlSupplierInvoiceSelector, false);
                                        callback2TabZero();
                                    });
                                }
                                else {
                                    _this.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack("", true);
                                    callback2TabZero();
                                }
                                return;
                            });
                            //logWindow.Show('./Customs/Components/Declaration/DeclarationPayment/SupplierInvoiceSelectionComponent');
                            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/SupplierInvoiceSelectionComponent');
                        });
                        return;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(unifreightJumpTo)) {
                        var sub = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(function (token1) {
                            switch (unifreightJumpTo) {
                                case "Payment":
                                    {
                                        var servicelink = '../../Customs/Components/MenuButtons/DeclarationMenuButtonsHandler';
                                        servicelink = './Customs/Components/MenuButtons/DeclarationMenuButtonsHandler';
                                        SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (handler) {
                                            handler.EntityPM = myEditComponent.EntityPM;
                                            handler.OpenPaymentOrderWindow();
                                        });
                                        // this will cause the customs to build ..... mohammad.
                                        //var handler = new DeclarationMenuButtonsHandler();
                                        //handler.EntityPM=myEditComponent.EntityPM;
                                        //handler.OpenPaymentOrderWindow();
                                    }
                                    break;
                                case "RequestSheet":
                                    {
                                        //Code  :"DCCA""Customs.Declaration.TH.CustomsAnswers"
                                        //Code:                        "DCRS"                                                "Customs.Declaration.TH.RequestSheet"
                                        myEditComponent.PreSelectedTabCode = "DCRS";
                                    }
                                    break;
                                case "Answer":
                                    {
                                        //Code  :"DCCA""Customs.Declaration.TH.CustomsAnswers"
                                        myEditComponent.PreSelectedTabCode = "DCCA";
                                    }
                                    break;
                            }
                        });
                    }
                    cmpRef.instance.SaveCompleted.subscribe(function (saveIt) {
                        isSaved = true;
                    });
                    cmpRef.instance.BackCompleted.subscribe(function (bk) {
                        _this.ShowDeclarationByIdUnifreightCallBack(isSaved);
                        callback2TabZero();
                    });
                });
            };
            class_1.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack = function (SupplierInvoiceSelector, CancelButtonClick) {
                //args = args ?? new CustomizedEventArgs.ChildrenCoosingEventArgs();
                if (AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter(function (item) { return item[0] == "ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack"; }).length == 0) {
                    AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack", ""]);
                }
                //if (_BusyIndicatorStartEvent == null) {
                //    _BusyIndicatorStartEvent = _CurrentAssemlyLocator.EventAggregator.GetEvent<BusyIndicatorStartEvent>();
                //}
                //_BusyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Start = false });
                if (!Tools_1.AppTool.IsNullOrEmpty(SupplierInvoiceSelector) && !CancelButtonClick) {
                    AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["SupplierInvoiceSelector", SupplierInvoiceSelector]);
                }
                else {
                    AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset["CancelButtonClick"] = "CancelButtonClick";
                }
                var myRequestWrapperM = new RequestWrapperM();
                myRequestWrapperM.SenderID = "UnifreightMassageHandler.ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack";
                myRequestWrapperM.ReceiverID = "CFIHMAIN.LogitudeTask";
                myRequestWrapperM.MessageID = "ShowSupplierInvoiceSelectorByDecIdUnifreightCallBack";
                myRequestWrapperM.UnifreightMessage = AmitalGatewayUtil.Instance._LastUnifreightMessageM;
                AmitalGatewayUtil.Instance.SendRequestJSONToUnifreightAsync(myRequestWrapperM);
                //CloseEditWindow(false, false);
            };
            class_1.ShowDeclarationByIdUnifreightCallBack = function (save) {
                if (AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter(function (item) { return item[0] == "ShowDeclarationByIdUnifreightCallBack"; }).length == 0) {
                    AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.push(["ShowDeclarationByIdUnifreightCallBack", save.toString()]);
                }
                var tuple = AmitalGatewayUtil.Instance._LastUnifreightMessageM.Requset.filter(function (item) { return item[0] == "ShowDeclarationByIdUnifreightCallBack"; })[0];
                tuple[1] = save.toString();
                var myRequestWrapperM = new RequestWrapperM();
                myRequestWrapperM.SenderID = "UnifreightMassageHandler.ShowDeclarationByIdUnifreightCallBack";
                myRequestWrapperM.ReceiverID = "CFIHMAIN.LogitudeTask";
                myRequestWrapperM.MessageID = "ShowDeclarationByIdUnifreightCallBack";
                myRequestWrapperM.UnifreightMessage = AmitalGatewayUtil.Instance._LastUnifreightMessageM;
                AmitalGatewayUtil.Instance.SendRequestJSONToUnifreightAsync(myRequestWrapperM);
            };
            return class_1;
        }());
        this.GeneralMessaging = /** @class */ (function () {
            function class_2() {
            }
            Object.defineProperty(class_2, "ResponseEntityAlreadyLockKey", {
                get: function () { return "Response.EntityAlreadyLock"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_2, "ResponseEntityAlreadyLockMessage", {
                get: function () { return "Response.EntityAlreadyLockMessage"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_2, "RaiseUnlockIIGEntityMessageId", {
                get: function () { return "RaiseUnlockIIGEntityMessage"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_2, "RaiseLockIIGEntReturnEntityAlreadyLockMessage", {
                get: function () { return "RaiseLockIIGEntReturnEntityAlreadyLockMessage"; },
                enumerable: true,
                configurable: true
            });
            class_2.RaiseLockIIGEntityReturnEntityAlreadyLock = function (LogitudeEntityName, LogitudeEntityNumber, ViewModelName) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage("", LogitudeEntityNumber, ViewModelName);
                unifreightMessageM.LogitudeEntity = LogitudeEntityName;
                unifreightMessageM.Requset.push(["ExpectedCallBack", AmitalGatewayUtil.Instance.GeneralMessaging.ResponseEntityAlreadyLockKey]);
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("AmitalGatewayUtil.RaiseLockIIGEntReturnEntityAlreadyLock", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.GeneralMessaging.RaiseLockIIGEntReturnEntityAlreadyLockMessage, unifreightMessageM, "XXXEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseLockIIGEntReturnEntityAlreadyLock");
            };
            class_2.RaiseUnlockIIGEntity = function (LogitudeEntity, LogitudeEntityNumber, HaveSaved) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage("", LogitudeEntityNumber, "");
                unifreightMessageM.LogitudeEntity = LogitudeEntity;
                unifreightMessageM.Requset.push(["HaveSaved", HaveSaved.toString()]);
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("AmitalGatewayUtil.RaiseUnlockIIGEntity", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.GeneralMessaging.RaiseUnlockIIGEntityMessageId, unifreightMessageM, "XXXEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseUnlockIIGEntity");
            };
            return class_2;
        }());
        this.DeclarationMessaging = /** @class */ (function () {
            function class_3() {
            }
            Object.defineProperty(class_3, "UnifreightResponseStatus", {
                //public static string InstructionReturnCanIContinue { get { return "Response.InstructionReturnCanIContinue"; } }
                get: function () { return "Response.UnifreightResponseStatus"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "LogitudeEntityDeclaration", {
                get: function () { return "Declaration"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "ResponseCFIFILMAlreadyLockKey", {
                get: function () { return "Response.CFIFILMAlreadyLock"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "ResponseCFIFILMAlreadyLockMessgae", {
                get: function () { return "Response.CFIFILMAlreadyLockMessgae"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "RaiseInstructionReturnCanIContinueMessage", {
                get: function () { return "RaiseInstructionReturnCanIContinueMessage"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage", {
                get: function () { return "RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "RaiseUnlockCFIFILEMMessage", {
                get: function () { return "RaiseUnlockCFIFILEMMessage"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "PrintStimulReturnCanIContinue", {
                get: function () { return "Response.PrintStimulReturnCanIContinue"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "RaisePrintStimulReturnCanIContinueMessage", {
                get: function () { return "RaisePrintStimulReturnCanIContinueMessage"; } //Yuval Chalup 26.07.2015 TASK-14849
                ,
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "ResponseInstructionCancel", {
                get: function () { return "Response.InstructionCancel"; } //Yuval Chalup 11.10.2015 AMI-54798
                ,
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "OpenNewBrowser", {
                get: function () { return "OpenNewBrowser"; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(class_3, "UnifreightEntity", {
                get: function () { return "CFIFILEM"; },
                enumerable: true,
                configurable: true
            });
            class_3.RaiseInstructionReturnCanIContinue = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, ViewPlace) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
                unifreightMessageM.Requset.push(["ViewPlace", ViewPlace]);
                unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.InstructionReturnCanIContinue"]);
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.RaiseInstructionReturnCanIContinue", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseInstructionReturnCanIContinueMessage, unifreightMessageM, "AMI-49615 - הפעלת Instructions מתוך הצהרת יבוא");
            };
            class_3.RaiseCheckInsuranseReturnIsNeededAmount = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, action) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
                unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.Action,Response.InsuranseIsNeeded,Response.InsuranseIsSucceeded,Response.InsuranceHasOpen,Response.InsuranceMessage,Response.InsuranseAmount,Response.InsuranseCurrency,Response.ExpensesAmount,Response.ExpensesAmountCurr,Response.FreightAmount,Response.FreightAmountCurr,Response.FreightAmount2,Response.FreightAmountCurr2,Response.TotalFreightInFreightCurr"]);
                if (!Tools_1.AppTool.IsNullOrEmpty(action)) {
                    unifreightMessageM.Requset.push(["Action", action]);
                }
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.RaiseCheckInsuranseReturnIsNeededAmount", "CFIHMAIN.LogitudeTask", "RaiseCheckInsuranseReturnIsNeededAmount", unifreightMessageM, "AMI-49619 - ביטוח שער עולמי - פיתוח ממשק לבדיקה האם נדרש לתיק ביטוח");
            };
            class_3.RaiseOpenNewBrowser = function (url) {
                var letsTry = true;
                if (letsTry) {
                    var win = window.open(url);
                }
                else {
                    AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("It's time to open new Browser", "GGGQWBLOGITUDE", AmitalGatewayUtil.Instance.DeclarationMessaging.OpenNewBrowser, AmitalGatewayUtil.Instance.GetDefaultUnifreightMessageM(), url); //Due System shutdown (pop up blocker ) we will open new Browser from Uniface process
                }
            };
            class_3.RaiseCFIFILMLockReturnCFIFILMAlreadyLock = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
                unifreightMessageM.LogitudeCommandId;
                unifreightMessageM.Requset.push(["ExpectedCallBack", AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseCFIFILMAlreadyLockKey]);
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.RaiseCFIFILMLockReturnCFIFILMAlreadyLock", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage, unifreightMessageM, "DeclarationEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseCheckCFIFILMLockReturnIsCFIFILMLock");
            };
            class_3.RaiseUnlockCFIFILEM = function (UnifreightEntityNumber, LogitudeEntityNumber, HaveSaved) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, "");
                unifreightMessageM.Requset.push(["HaveSaved", HaveSaved.toString()]);
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.RaiseUnlockCFIFILEM", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseUnlockCFIFILEMMessage, unifreightMessageM, "DeclarationEditControlViewModelController.OnFirstTimeSingleDataLoaded.RaiseCheckCFIFILMLockReturnIsCFIFILMLock");
            };
            //<--- Yuval Chalup 26.07.2015 TASK-14849
            class_3.RaisePrintStimulReturnCanIContinue = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, ViewPlace) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
                unifreightMessageM.Requset.push(["ExpectedCallBack", "Response.PrintStimulReturnCanIContinue"]);
                if (ViewPlace == "RELEASE") // moran 29.2.16 - Task 19807 -->
                 {
                    unifreightMessageM.Requset.push(["FormToPrint", "RELEASE"]);
                    AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.RaisePrintStimulReturnCanIContinue", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.DeclarationMessaging.RaisePrintStimulReturnCanIContinueMessage, unifreightMessageM, " - הדפסת שחרור חלקי מהצהרה  - הפעלת טופס A/200 ביוניפירייט ");
                }
                else // moran 29.2.16 - Task 19807 <--
                 {
                    unifreightMessageM.Requset.push(["FormToPrint", ViewPlace]);
                    AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.RaisePrintStimulReturnCanIContinue", "CFIHMAIN.LogitudeTask", AmitalGatewayUtil.Instance.DeclarationMessaging.RaisePrintStimulReturnCanIContinueMessage, unifreightMessageM, "ASK-14849 - הדפסת צרופה מהצהרה  - הפעלת טופס A/33 ביוניפירייט ");
                }
            };
            //Yuval Chalup 26.07.2015 TASK-14849 --->
            class_3.GetMessage = function (UnifreightEntityNumber, LogitudeEntityNumber, LogitudeViewModel) {
                var unifreightMessageM = new UnifreightMessageM();
                unifreightMessageM.UnifreightEntity = AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity;
                unifreightMessageM.UnifreightEntityNumber = UnifreightEntityNumber;
                unifreightMessageM.LogitudeEntity = AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration;
                unifreightMessageM.LogitudeEntityNumber = LogitudeEntityNumber;
                unifreightMessageM.LogitudeViewModel = LogitudeViewModel;
                unifreightMessageM.Requset = [];
                return unifreightMessageM;
            };
            class_3.prototype.ShowDeclarationCertificatesByGroupsUnifreightCallBack = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, CustomerId) {
                var unifreightMessageM = AmitalGatewayUtil.Instance.DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
                unifreightMessageM.Requset.push(["CustomerId", CustomerId]);
                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.ShowDeclarationCertificatesByGroupsUnifreightCallBack", "CFIHMAIN.LogitudeTask", "ShowDeclarationCertificatesByGroupsUnifreightCallBack", unifreightMessageM, " אישורים נדרשים");
            };
            return class_3;
        }());
    }
    Object.defineProperty(AmitalGatewayUtil, "Instance", {
        get: function () {
            this._Instance = this._Instance || new AmitalGatewayUtil();
            return this._Instance;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AmitalGatewayUtil.prototype, "AmitalBrowserInUse", {
        get: function () { return (this._AmitalBrowserInUse === true); },
        set: function (newValue) { this._AmitalBrowserInUse = newValue; },
        enumerable: true,
        configurable: true
    });
    AmitalGatewayUtil.prototype.IsDeclarationInUse = function (CustomFileNo, IsConvertedDeclaration, IsConnectedToUnifreight) {
        if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(CustomFileNo)) {
            return false;
        }
        //if (IsConvertedDeclaration || IsConnectedToUnifreight) {
        if (IsConnectedToUnifreight) {
            return true;
        }
        else {
            return false;
        }
    };
    AmitalGatewayUtil.prototype.NoteUnifreightIamReady = function () {
        var myRequestWrapper = new RequestWrapperM();
        myRequestWrapper.MessageID = "NoteUnifreightIamReady";
        myRequestWrapper.UnifreightMessage = new UnifreightMessageM();
        this.SendRequestJSONToUnifreightAsync(myRequestWrapper);
    };
    AmitalGatewayUtil.prototype.GetDefaultUnifreightMessageM = function () {
        var unifreightMessageM = new UnifreightMessageM();
        unifreightMessageM.Requset = [];
        unifreightMessageM.Response = [];
        return unifreightMessageM;
    };
    AmitalGatewayUtil.prototype.ShowDeclarationCertificatesByGroups = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, CustomerId) {
        //var unifreightMessageM = GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
        var unifreightMessageM = AmitalGatewayUtil.Instance.
            DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
        unifreightMessageM.Requset.push(["CustomerId", CustomerId]);
        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.ShowDeclarationCertificatesByGroupsUnifreightCallBack", "CFIHMAIN.LogitudeTask", "ShowDeclarationCertificatesByGroupsUnifreightCallBack", unifreightMessageM, " אישורים נדרשים");
    };
    AmitalGatewayUtil.prototype.GetRihbitFromTransmissions = function (UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName, CustomerId) {
        var unifreightMessageM = AmitalGatewayUtil.Instance.
            DeclarationMessaging.GetMessage(UnifreightEntityNumber, LogitudeEntityNumber, ViewModelName);
        unifreightMessageM.Requset.push(["CustomerId", CustomerId]);
        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync("ScriptableGatewayUtil.GetRihbitFromTransmissions", "CFIHMAIN.LogitudeTask", "GetRihbitFromTransmissions", unifreightMessageM, "קבצי רכבים");
    };
    AmitalGatewayUtil.prototype.SendTotangoUserActivity = function (module, activity) {
        var req = new UnifreightMessageM();
        req.Requset.push(["module", module]);
        req.Requset.push(["activity", activity]);
        this.SendRequestToUnifreightAsync("", "", "SendUserActivity", req, "");
    };
    AmitalGatewayUtil.prototype.SendRequestToUnifreightAsync = function (SenderID, ReceiverID, MessageID, unifreightMessageM, MoreParams) {
        var requestWrapper = new RequestWrapperM();
        requestWrapper.SenderID = SenderID;
        requestWrapper.ReceiverID = ReceiverID;
        requestWrapper.MessageID = MessageID;
        requestWrapper.UnifreightMessage = unifreightMessageM;
        requestWrapper.MoreParams = MoreParams;
        this.SendRequestJSONToUnifreightAsync(requestWrapper);
    };
    AmitalGatewayUtil.prototype.SendRequestJSONToUnifreightAsync = function (myRequestWrapper) {
        if (Tools_1.AppTool.IsNullOrEmpty(window.parent._JavascriptGateway)) {
            alert("_JavascriptGateway not exist !!!");
            return;
        }
        else {
            var myRequestWrapperJSON = JSON.stringify(myRequestWrapper);
            window.parent._JavascriptGateway.SendRequestJSONToUnifreightAsync(myRequestWrapperJSON);
        }
    };
    AmitalGatewayUtil.prototype.AmitalBackButtonClicked = function () {
        var RequestWrapper = new RequestWrapperM();
        var myUnifreightMessageM = new UnifreightMessageM();
        //myUnifreightMessageM.LogitudeCommandId = "LogitudeCommandId";
        //myUnifreightMessageM.LogitudeEntity = "LogitudeEntity";
        //myUnifreightMessageM.Requset.push(["key1", "Value2"]);
        //myUnifreightMessageM.Requset.push(["key66", "Vafdgdflue2"]);
        //var j = JSON.stringify(myUnifreightMessageM);
        RequestWrapper.MessageID = "AmitalBackButtonCommandAction";
        RequestWrapper.DeclarePurpose = "Uniface have to close GGGQWBLOGITUDE Component";
        this.SendRequestJSONToUnifreightAsync(RequestWrapper);
    };
    AmitalGatewayUtil.prototype.UnifaceRequest = function (myParam, myEditTab, change2EditTab, change2CA23Tab) {
        var _this = this;
        var MaintenanceMenu = "General.MH.Maintenance";
        var unifreightMessage = myParam;
        //if (AppTool.IsNullOrEmpty(unifreightMessage.LogitudeCommandId)) {
        //    throw new Error("UnifaceRequest get bad  unifreightMessage (LogitudeCommandId is null !?!?!?)");
        //}
        this._LastUnifreightMessageM = unifreightMessage;
        this._LastUnifreightMessageM.Requset = this._LastUnifreightMessageM.Requset || [];
        this._LastUnifreightMessageM.Response = this._LastUnifreightMessageM.Response || [];
        switch (unifreightMessage.LogitudeCommandId) {
            case "this.CurrentSession.CurrentEditComponent.ReloadEntityPM()": {
                if (this.CurrentSession.CurrentEditComponent) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
                break;
            }
            case "ShowDeclarationByIdReturnCloseSave": {
                //change2EditTab();
                //if (AppTool.IsNullOrEmpty(myEditTab.SessionComponent)) {
                //    setTimeout(() => { AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(unifreightMessage, myEditTab, change2CA23Tab); }, 500);
                //} else {
                //    AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(unifreightMessage, myEditTab, change2CA23Tab);
                //}
                this.ShowDeclarationByIdReturnCloseSaveMethod(myParam, myEditTab, change2EditTab, change2CA23Tab);
                break;
            }
            case "ShowSupplierInvoiceSelectorByDecIdReturnChosenChildren":
                {
                    this.ShowDeclarationByIdReturnCloseSaveMethod(myParam, myEditTab, change2EditTab, change2CA23Tab);
                }
                break;
            case "ShowClientReturnIfExist":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var clientAction = new ClientAction();
                    clientAction.Run(myParam);
                }
                break;
            case "MapDocumentTypeCustomsData":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.SelectCustomsRequestMenu(MaintenanceMenu);
                    var mapDocumentTypeCustomsData = new MapDocumentTypeCustomsData();
                    mapDocumentTypeCustomsData.Run(myParam);
                }
                break;
            case "ShowGeneralLOVReturnSelected":
                {
                    this.SelectCustomsRequestMenu(MaintenanceMenu);
                    var mapGeneralLOV = new ShowGeneralLOVReturnSelected();
                    mapGeneralLOV.Run(myParam);
                }
                break;
            case "MapPendingReasonCodeData":
                {
                    this.SelectCustomsRequestMenu(MaintenanceMenu);
                    var mapPendingReasonCodeData = new MapPendingReasonCodeData();
                    mapPendingReasonCodeData.Run(myParam);
                }
                break;
            case "ShowDeclarationStatusQuery":
                {
                    this.SelectCustomsRequestMenu();
                    this.CurrentSession.StartBusyIndicator("");
                    var servicelink = '../../Customs/Services/StandardPMs/DeclarationPMService'; //mohammad
                    servicelink = './Customs/Services/StandardPMs/DeclarationPMService'; //itzik !!
                    SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (declarationPMService) {
                        declarationPMService.get(_this._LastUnifreightMessageM.LogitudeEntityNumber)
                            .subscribe(function (myDeclarationResponse) {
                            _this.CurrentSession.StopBusyIndicator();
                            var decList = myDeclarationResponse.Result;
                            var servicelink = '../../Customs/Services/Others/CustomsRequestMenuService';
                            servicelink = './Customs/Services/Others/CustomsRequestMenuService';
                            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                                var my = {
                                    "DeclarationNumber": decList.DeclarationNumber,
                                    "CustomsFile": _this._LastUnifreightMessageM.UnifreightEntityNumber,
                                    "DeclarationId": _this._LastUnifreightMessageM.LogitudeEntityNumber,
                                };
                                service.WindowClosed.subscribe(function (myarg) {
                                    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    AmitalGatewayUtil.Instance.
                                        //ShowClientReturnIfExistUnifreightCallBack(false);
                                        AmitalBackButtonClicked();
                                });
                                service.ShowModalAsEditMenuAction("8250", my);
                            });
                        });
                        // this will cause the customs to build in aot every time...mohammad.
                        //let declarationPMService: DeclarationPMService = new DeclarationPMService();
                        // this will cause the customs to build in aot every time...mohammad.
                        //let customsRequestMenuService = new CustomsRequestMenuService();
                        //let my = {
                        //    "DeclarationNumber": decList.DeclarationNumber,
                        //    "CustomsFile": this._LastUnifreightMessageM.UnifreightEntityNumber,
                        //    "DeclarationId": this._LastUnifreightMessageM.LogitudeEntityNumber,
                        //};
                        //customsRequestMenuService.WindowClosed.subscribe(
                        //    (myarg) => {
                        //        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        //        AmitalGatewayUtil.Instance.
                        //            //ShowClientReturnIfExistUnifreightCallBack(false);
                        //            AmitalBackButtonClicked();
                        //    }
                        //);
                        //customsRequestMenuService.ShowModalAsEditMenuAction("8250", my);
                    });
                }
                break;
            case "ShowManifestQuery": //..V_TEMP = "ShowManifestQuery"
                {
                    this.SelectCustomsRequestMenu();
                    this.CurrentSession.StartBusyIndicator("");
                    var servicelink = '../../Customs/Services/StandardPMs/DeclarationPMService';
                    servicelink = './Customs/Services/StandardPMs/DeclarationPMService';
                    SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (declarationPMService) {
                        declarationPMService.get(_this._LastUnifreightMessageM.LogitudeEntityNumber)
                            .subscribe(function (myDeclarationResponse) {
                            _this.CurrentSession.StopBusyIndicator();
                            var decList = myDeclarationResponse.Result;
                            decList.Consignments[0];
                            var servicelink = '../../Customs/Services/Others/CustomsRequestMenuService';
                            servicelink = './Customs/Services/Others/CustomsRequestMenuService';
                            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                                var my = {
                                    "Mode": "SendCargoQueryRequestFromDeclaration",
                                    "CargoTypeCode": decList.Consignments[0].CargoTypeCode,
                                    "ManifestNumber": decList.Consignments[0].ManifestNumber,
                                    "SecondCargoID": decList.Consignments[0].SecondCargoID,
                                    "DeclarationId": _this._LastUnifreightMessageM.LogitudeEntityNumber,
                                };
                                service.WindowClosed.subscribe(function (myarg) {
                                    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    AmitalGatewayUtil.Instance.
                                        //ShowClientReturnIfExistUnifreightCallBack(false);
                                        AmitalBackButtonClicked();
                                });
                                service.ShowModalAsEditMenuAction("8240", my);
                            });
                        });
                        // let declarationPMService: DeclarationPMService = new DeclarationPMService();
                        // this will cause the customs to build.
                        //let customsRequestMenuService = new CustomsRequestMenuService();
                        //let my = {
                        //    "Mode": "SendCargoQueryRequestFromDeclaration",
                        //    "CargoTypeCode": decList.Consignments[0].CargoTypeCode,
                        //    "ManifestNumber": decList.Consignments[0].ManifestNumber,
                        //    "SecondCargoID": decList.Consignments[0].SecondCargoID,
                        //    "DeclarationId": this._LastUnifreightMessageM.LogitudeEntityNumber,
                        //};
                        //customsRequestMenuService.WindowClosed.subscribe(
                        //    (myarg) => {
                        //        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        //        AmitalGatewayUtil.Instance.
                        //            //ShowClientReturnIfExistUnifreightCallBack(false);
                        //            AmitalBackButtonClicked();
                        //    }
                        //);
                        //customsRequestMenuService.ShowModalAsEditMenuAction("8240", my);
                    });
                }
                break;
            default: {
                //throw new Error("UnifaceRequest get bad  unifreightMessage (LogitudeCommandId is unknown ) " + unifreightMessage.LogitudeCommandId);
                this.UnifaceRequestArrived.emit(myParam);
                //break;
                //this.CurrentSession
                //SessionLocator.AllSessions[0].
            }
        }
    };
    AmitalGatewayUtil.prototype.SelectCustomsRequestMenu = function (menuCode) {
        if (menuCode === void 0) { menuCode = "General.MH.Customs"; }
        var mySelectedItem = this.CurrentSession.MainMenuComponent.MainMenuItems
            .filter(function (m) { return m.TextCode ==
            //"General.MH.Customs"
            menuCode; })[0];
        if (mySelectedItem != null) {
            this.CurrentSession.MainMenuComponent.SelectionChanged(mySelectedItem);
        }
    };
    AmitalGatewayUtil.prototype.ShowClientReturnIfExistUnifreightCallBack = function (exist) {
        this._LastUnifreightMessageM.Requset.push(["LogitudeReturnClientExist", exist.toString()]);
        this._LastUnifreightMessageM.Response.push(["LogitudeReturnClientExist", exist.toString()]);
        this.SendRequestToUnifreightAsync("UnifreightMassageHandler.ShowClientReturnIfExistUnifreightCallBack", "CFIHMAIN.LogitudeTask", "ShowClientReturnIfExistUnifreightCallBack", this._LastUnifreightMessageM, "Task ???");
        //CloseEditWindow(false, false);
    };
    AmitalGatewayUtil.prototype.ShowGeneralLOVReturnSelectedCallBack = function (event) {
        this._LastUnifreightMessageM.Requset.push(["ShowGeneralLOVReturnSelectedCancel", (event == "ShowGeneralLOVReturnSelectedCancel").toString()]);
        this._LastUnifreightMessageM.Requset.push(["ShowGeneralLOVReturnSelectedValue", event]);
        this.SendRequestToUnifreightAsync("UnifreightMassageHandler.ShowGeneralLOVReturnSelectedCallBack", "CFIHMAIN.LogitudeTask", "ShowGeneralLOVReturnSelectedCallBack", this._LastUnifreightMessageM, "Task ???");
        //CloseEditWindow(false, false);
    };
    AmitalGatewayUtil.prototype.ShowDeclarationByIdReturnCloseSaveMethod = function (myParam, myEditTab, change2EditTab, change2CA23Tab) {
        var _this = this;
        change2EditTab();
        if (Tools_1.AppTool.IsNullOrEmpty(myEditTab.SessionComponent)) {
            setTimeout(function () { AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(_this._LastUnifreightMessageM, myEditTab, change2CA23Tab); }, 500);
        }
        else {
            AmitalGatewayUtil.Instance.ShowDeclarationByIdReturnCloseSave.StartDoIt(this._LastUnifreightMessageM, myEditTab, change2CA23Tab);
        }
    };
    return AmitalGatewayUtil;
}());
exports.AmitalGatewayUtil = AmitalGatewayUtil;
var RequestWrapperM = /** @class */ (function () {
    function RequestWrapperM() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //    < ArrayOfEntry >
        //    <Entry>
        //    <Key><![CDATA[UnifreightEntity]] > </Key>
        //    < Value > <![CDATA[]] > </Value>
        //    < /Entry>
        //    < Entry >
        //    <Key><![CDATA[UnifreightEntityNumber]] > </Key>
        //    < Value > <![CDATA[]] > </Value>
        //    < /Entry>
        //    < Entry >
        //    <Key><![CDATA[LogitudeEntity]] > </Key>
        //    < Value > <![CDATA[]] > </Value>
        //    < /Entry>
        //    < Entry >
        //    <Key><![CDATA[LogitudeEntityNumber]] > </Key>
        //    < Value > <![CDATA[]] > </Value>
        //    < /Entry>
        //    < Entry >
        //    <Key><![CDATA[LogitudeViewModel]] > </Key>
        //    < Value > <![CDATA[]] > </Value>
        //    < /Entry>
        //    < Entry >
        //    <Key><![CDATA[LogitudeCommandId]] > </Key>
        //    < Value > <![CDATA[]] > </Value>
        //    < /Entry>
        //    < /ArrayOfEntry>" 
        this.MoreParams = "";
    }
    return RequestWrapperM;
}());
exports.RequestWrapperM = RequestWrapperM;
var UnifreightMessageM = /** @class */ (function () {
    function UnifreightMessageM() {
        this.Requset = [];
        this.Response = [];
    }
    UnifreightMessageM.GetStringValue = function (myUnifreightMessageM, theKey) {
        var StringValue = "";
        var listRes = myUnifreightMessageM.Requset
            .filter(function (itm) { return itm[0] == theKey; });
        if (listRes.length > -1 && !Tools_1.AppTool.IsNullOrEmpty(listRes[0])) {
            if (!Tools_1.AppTool.IsNullOrEmpty(listRes[0][1])) {
                StringValue = listRes[0][1];
            }
        }
        else {
            var listRes_1 = myUnifreightMessageM.Response
                .filter(function (itm) { return itm[0] == theKey; });
            if (listRes_1.length > -1 && !Tools_1.AppTool.IsNullOrEmpty(listRes_1[0])) {
                if (!Tools_1.AppTool.IsNullOrEmpty(listRes_1[0][1])) {
                    StringValue = listRes_1[0][1];
                }
            }
        }
        return StringValue;
    };
    return UnifreightMessageM;
}());
exports.UnifreightMessageM = UnifreightMessageM;
var ClientAction = /** @class */ (function () {
    function ClientAction() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ClientAction.prototype.Run = function (unifreightMessage) {
        var _this = this;
        //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
        var UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
        var ImporterVat = unifreightMessage.LogitudeEntityNumber;
        var servicelink = '../../Customs/Services/WebServices/ClientMessagesService';
        servicelink = './Customs/Services/WebServices/ClientMessagesService';
        SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
            service.GetSingleClientPMByCode(ImporterVat, false)
                .subscribe(function (rsp) {
                var myClientPM = rsp.Result;
                _this.CurrentSession.StopBusyIndicator();
                if (Tools_1.AppTool.IsNullOrEmpty(myClientPM)) {
                    AmitalGatewayUtil.Instance.ShowClientReturnIfExistUnifreightCallBack(false);
                }
                else {
                    _this.ShowClientEditControl(myClientPM);
                }
            });
        });
        //this will cause the customs to build ....mohammad.
        //var clientExtendedPMService = new ClientMessagesService();
        //clientExtendedPMService.GetSingleClientPMByCode(ImporterVat, false)
        //    .subscribe((rsp) => {
        //        var myClientPM: ClientPM = rsp.Result;
        //        this.CurrentSession.StopBusyIndicator();
        //        if (AppTool.IsNullOrEmpty(myClientPM)) {
        //            AmitalGatewayUtil.Instance.ShowClientReturnIfExistUnifreightCallBack(false);
        //        } else {
        //            this.ShowClientEditControl(myClientPM);
        //        }
        //    });
    };
    ClientAction.prototype.ShowClientEditControl = function (myClientPM) {
        //windowArgs.CurrentEntity = myResponse.Result;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.EditClient"); // "Edit Client";
        logWindow.WindowArgs = { "CurrentEntity": myClientPM, "isNewClient": false };
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent');
        logWindow.WindowClosed.subscribe(function ($event1) {
            AmitalGatewayUtil.Instance.ShowClientReturnIfExistUnifreightCallBack(true);
        });
    };
    return ClientAction;
}());
exports.ClientAction = ClientAction;
var ShowGeneralLOVReturnSelected = /** @class */ (function () {
    function ShowGeneralLOVReturnSelected() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ShowGeneralLOVReturnSelected.prototype.Run = function (unifreightMessage) {
        var _this = this;
        //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
        var UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
        //let ImporterVat = unifreightMessage.LogitudeEntityNumber;
        //let formtitle: string=            = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.formtitle");
        this.CurrentSession.StartBusyIndicatorLoading();
        var LOVText = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.LOVText");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 250;
        logWindow.Title = 'הזן ' + LOVText;
        logWindow.WindowArgs = {
            "LogitudeEntityNumber": unifreightMessage.LogitudeEntityNumber,
            "LogitudeEntity": unifreightMessage.LogitudeEntity,
            "LOVText": LOVText,
        };
        logWindow.ShowCloseButton = true;
        logWindow.Show(
        //'./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent'
        //'./Customs/Components/Maintenance/DocumentTypeCustomsDataComponent'
        './CustomsModules/CustomsMaintenance/Components/GeneralLOVComponent');
        logWindow.WindowClosed.subscribe(function (event1) {
            ///AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
            if (event1 == "Cancel") {
            }
            _this.CurrentSession.StopBusyIndicator();
            AmitalGatewayUtil.Instance.ShowGeneralLOVReturnSelectedCallBack(event1);
            //this.CurrentSession.StopBusyIndicator();
        });
    };
    return ShowGeneralLOVReturnSelected;
}());
exports.ShowGeneralLOVReturnSelected = ShowGeneralLOVReturnSelected;
var MapDocumentTypeCustomsData = /** @class */ (function () {
    function MapDocumentTypeCustomsData() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    MapDocumentTypeCustomsData.prototype.Run = function (unifreightMessage) {
        var _this = this;
        //"UnifreightEntity=GNDCARD·;UnifreightEntityNumber=10009065·;LogitudeEntity=Customs.Client·;LogitudeEntityNumber=049028392·;LogitudeViewModel=UnifreightMassageHandler·;LogitudeCommandId=ShowClientReturnIfExist·;formtitle=Client"
        var UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
        //let ImporterVat = unifreightMessage.LogitudeEntityNumber;
        var UnifaceNAME_HEB = UnifreightMessageM.GetStringValue(unifreightMessage, "Requset.UnifaceNAME_HEB");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 250;
        logWindow.Title = 'קשר סוג מסמך לשער עולמי'; //TextCodeTranslator.Translate("Customs.Client.O.EditClient");// "Edit Client";
        logWindow.WindowArgs = {
            "UnifaceDOC_ID": UnifreightEntityNumber,
            "UnifaceNAME_HEB": UnifaceNAME_HEB
        };
        logWindow.ShowCloseButton = true;
        logWindow.Show(
        //'./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent'
        //'./Customs/Components/Maintenance/DocumentTypeCustomsDataComponent'
        './CustomsModules/CustomsMaintenance/Components/DocumentTypeCustomsDataComponent');
        logWindow.WindowClosed.subscribe(function ($event1) {
            AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    return MapDocumentTypeCustomsData;
}());
exports.MapDocumentTypeCustomsData = MapDocumentTypeCustomsData;
var MapPendingReasonCodeData = /** @class */ (function () {
    function MapPendingReasonCodeData() {
    }
    MapPendingReasonCodeData.prototype.Run = function (unifreightMessage) {
        var UnifreightEntityNumber = unifreightMessage.UnifreightEntityNumber;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 400;
        logWindow.Title = 'קשר סטטוס לסיבת Pending';
        logWindow.WindowArgs = {
            "UnifreightStatusCode": UnifreightEntityNumber,
            "FromUnifreight": true,
        };
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/AddCourierPendingToUnifreightStatusComponent');
        logWindow.WindowClosed.subscribe(function ($event1) {
            AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
        });
    };
    return MapPendingReasonCodeData;
}());
exports.MapPendingReasonCodeData = MapPendingReasonCodeData;
//# sourceMappingURL=AmitalGatewayUtil.js.map