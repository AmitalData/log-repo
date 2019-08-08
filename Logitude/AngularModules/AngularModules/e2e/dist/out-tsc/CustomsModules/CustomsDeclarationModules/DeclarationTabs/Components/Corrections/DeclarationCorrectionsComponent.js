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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var DeclarationMessagesService_1 = require("../../../../../Customs/Services/WebServices/DeclarationMessagesService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DeclarationCorrectionsComponent = /** @class */ (function (_super) {
    __extends(DeclarationCorrectionsComponent, _super);
    function DeclarationCorrectionsComponent(entityArgs, cd, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.DisplayOnlyMessage = "";
        _this.IsNoAmendmentsMsgVisible = false;
        //Grids data
        _this.AdditionalInformationlist = new ObservableCollection_1.ObservableCollection([]);
        _this.AmendmentViewsList = new ObservableCollection_1.ObservableCollection([]);
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService;
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsDescriptionVisible = false;
        _this.GeneralData = [];
        //#endregion
        //#region Version DDL
        _this.VersionsList = [];
        _this.SelectedVersion = '';
        // row selected
        _this.SelectedRow = null;
        //#region Get resources
        _this.isResourcesLoaded = false;
        _this.arrayLength = 0;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                        _this.EntityPM = _this.entityArgs.EntityPM;
                        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                        _this.Listen();
                        console.log("Declaration", _this.EntityPM);
                        _this.ReloadDeclarationCorrection();
                        //this.DisplayOnlyCheck();
                    });
                });
            });
        });
        return _this;
        ////Disable fields
        //if (this.IsDisplayOnly) {
        //    this.SetScreenFieldsEditability();
        //}
    }
    DeclarationCorrectionsComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.DisplayOnlyCheck();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCR") {
                        //this.DisplayOnlyCheck();
                    }
                }
            }));
            ;
        }
    };
    DeclarationCorrectionsComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(DeclarationCorrectionsComponent.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) {
            this.description = newValue;
        },
        enumerable: true,
        configurable: true
    });
    DeclarationCorrectionsComponent.prototype.ReloadDeclarationCorrection = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        //[1] GetDeclarationCorrections();
        this.declarationWebService.GetDeclarationCorrection(this.EntityPM.Id).subscribe(function (myServiceResponse) {
            console.log("[Response] GetDeclarationConstraints : ", myServiceResponse.Result);
            var res = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                _this.GeneralData = [];
                var amendmentViewsList = [];
                //sort data
                var data = res.GeneralDataViews ? res.GeneralDataViews.sort(function (a, b) { return (a.Version < b.Version) ? 1 : -1; }) : null;
                //build version list
                _this.BuildGeneralData(data);
                // select amendment for the first version
                var general = _this.GeneralData[0];
                _this.AmendmentViewsList = new ObservableCollection_1.ObservableCollection([]);
                _this.AdditionalInformationlist.InsertCollection(general.AdditionalInformation);
                general.AmendmentViews.forEach(function (el) {
                    amendmentViewsList.push(el);
                });
                _this.AmendmentViewsList.InsertCollection(amendmentViewsList);
                _this.GetResources(_this.AmendmentViewsList.Collection);
                _this.BuildSystemMessage(general.SystemMessageViews);
            }
            else {
                _this.IsNoAmendmentsMsgVisible = true;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DeclarationCorrectionsComponent.prototype.BuildGeneralData = function (data) {
        var _this = this;
        if (data) {
            var dateStr = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Date");
            var timeStr = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Time");
            var versionStr = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Version");
            this.VersionsList = [];
            data.forEach(function (el) {
                _this.GeneralData.push(el);
                var myFormats = Tools_1.DateTool.GetDateFormats(el.CorrectionDate);
                var dateValue = myFormats.ShortDateString;
                var timeValue = myFormats.ShortTimeString;
                _this.VersionsList.push(dateStr + ' ' + dateValue + ' ' + timeStr + ' ' + timeValue + ' ' + versionStr + ' ' + el.Version);
                _this.SelectedVersion = _this.VersionsList[0];
            });
        }
        else {
            console.log("No data to build versions list!!!!", data);
        }
    };
    DeclarationCorrectionsComponent.prototype.GeneralDataSelectionChanged = function (selectedIndex) {
        var _this = this;
        var selectedGeneral = this.GeneralData[selectedIndex]; // new selected version
        this.AmendmentViewsList = new ObservableCollection_1.ObservableCollection([]);
        this.AdditionalInformationlist.InsertCollection(selectedGeneral.AdditionalInformation);
        selectedGeneral.AmendmentViews.forEach(function (el) {
            _this.AmendmentViewsList.Insert(el);
        });
        this.GetResources(this.AmendmentViewsList.Collection);
    };
    DeclarationCorrectionsComponent.prototype.BuildSystemMessage = function (data) {
        this.Description = null;
        for (var _i = 0, data_1 = data; _i < data_1.length; _i++) {
            var error = data_1[_i];
            if (error.ListVersionID == "A") {
                if (!this.Description)
                    this.Description = "";
                this.Description += error.MessageError + ", ";
            }
        }
        if (this.Description) {
            this.IsDescriptionVisible = true;
            this.Description = this.Description.replace(/,\s*$/, ""); //remove last comma
        }
    };
    DeclarationCorrectionsComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.SelectedVersion != itemValue) {
            this.SelectedVersion = itemValue;
            this.GeneralDataSelectionChanged(this.VersionsList.indexOf(this.SelectedVersion));
        }
    };
    //#endregion
    DeclarationCorrectionsComponent.prototype.OpenAmendment = function (amendment) {
        console.log("open amendment: ", amendment);
        //selectedLine = amendment;
        this.EditEntity(amendment);
    };
    DeclarationCorrectionsComponent.prototype.EditEntity = function (amendmentView) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(amendmentView)) {
            console.warn("[!] There is no Amendment View!");
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            switch (amendmentView.EntityName.toLowerCase()) {
                case "declaration":
                case "consignment":
                    {
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 1000;
                        logWindow.Height = 700;
                        logWindow.ShowCloseButton = true;
                        logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/DeclarationGeneralComponent');
                        logWindow.WindowArgs = {
                            AmendmentView: amendmentView,
                            entityArgs: this.entityArgs,
                            entityPM: this.EntityPM,
                            IsDisplayOnly: this.IsDisplayOnly,
                        };
                        this.CurrentSession.StopBusyIndicator();
                        break;
                    }
                case "supplierinvoice": {
                    this.declarationWebService
                        .GetSupplierInvoiceBySequenceNumber(this.EntityPM.Id, +amendmentView.LineNumber, 0, 500)
                        .subscribe(function (response) {
                        console.log("[Response] GetSupplierInvoiceBySequenceNumber: ", response);
                        var supplierInvoicePM = response.Result;
                        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                            _this.CurrentSession.StartBusyIndicatorLoading();
                            var windowArgs = {};
                            windowArgs.EntityPM = supplierInvoicePM;
                            windowArgs.declarationPM = _this.EntityPM;
                            windowArgs.AmendmentView = amendmentView;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 1030;
                            logWindow.Height = 600;
                            if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + _this.EntityPM.DeclarationNumber;
                            }
                            else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.DeclarationNumber;
                            }
                            else if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
                            }
                            else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                            }
                            windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                            logWindow.ShowCloseButton = false;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.Title = _this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                            logWindow.WindowClosed.subscribe(function ($event) {
                            });
                            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                            _this.CurrentSession.StopBusyIndicator();
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            var window = new MessageWindow_1.MessageWindow();
                            window.Show("There is no invoice with such key in this declaration!!");
                        }
                    });
                    break;
                }
                case "supplierinvoiceitem":
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
                            console.log("No line number", amendmentView);
                            return;
                        }
                        var lines = amendmentView.LineNumber.split(',');
                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];
                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(this.EntityPM.Id, invSequence, itemSequence, 0, 500)
                            .subscribe(function (response) {
                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
                            var supplierInvoicePM = response.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                                _this.CurrentSession.StartBusyIndicatorLoading();
                                var windowArgs = {};
                                windowArgs.EntityPM = supplierInvoicePM;
                                windowArgs.declarationPM = _this.EntityPM;
                                windowArgs.AmendmentView = amendmentView;
                                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                logWindow.Width = 1030;
                                logWindow.Height = 600;
                                if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + _this.EntityPM.DeclarationNumber;
                                }
                                else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.DeclarationNumber;
                                }
                                else if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
                                }
                                else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                                }
                                windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                                logWindow.ShowCloseButton = false;
                                logWindow.WindowArgs = windowArgs;
                                logWindow.Title = _this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                                _this.CurrentSession.StopBusyIndicator();
                                logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                                _this.CurrentSession.StopBusyIndicator();
                            }
                            else {
                                _this.CurrentSession.StopBusyIndicator();
                                var window = new MessageWindow_1.MessageWindow();
                                window.Show("There is no invoice with such key in this declaration!!");
                            }
                        });
                        break;
                    }
                case "supplierinvioceitemscertificate":
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
                            console.log("No line number", amendmentView);
                            return;
                        }
                        var lines = amendmentView.LineNumber.split(',');
                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];
                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(this.EntityPM.Id, invSequence, itemSequence, 0, 500)
                            .subscribe(function (response) {
                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
                            var supplierInvoicePM = response.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                                _this.CurrentSession.StartBusyIndicatorLoading();
                                var invoiceItem = supplierInvoicePM.SupplierInvoiceItems.find(function (d) { return d.SequenceNumeric == amendmentView.ParentLine; });
                                if (!Tools_1.AppTool.IsNullOrEmpty(invoiceItem)) {
                                    // open certificate
                                    var windowArgs = {};
                                    windowArgs.SupplierInvoiceItemPM = invoiceItem;
                                    windowArgs.AmendmentView = amendmentView;
                                    windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                                    windowArgs.InvoiceNumber = supplierInvoicePM.InvoiceNumber;
                                    windowArgs.SupplierInvoicePM = supplierInvoicePM;
                                    var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 1000;
                                    logWindow.Height = 600;
                                    //if (supplierInvoicePM.ClassificationCode != null) {
                                    //    logWindow.Title = "אישורים לפרט מכס" + " " + supplierInvoicePM.ClassificationCode;
                                    //}
                                    //else {
                                    //    logWindow.Title = "אישורים לפרט מכס";
                                    //}
                                    logWindow.ShowCloseButton = false;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Title = _this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                                    _this.CurrentSession.StopBusyIndicator();
                                    logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
                                    //end open certificate
                                }
                                _this.CurrentSession.StopBusyIndicator();
                            }
                            else {
                                _this.CurrentSession.StopBusyIndicator();
                                var window = new MessageWindow_1.MessageWindow();
                                window.Show("There is no invoice with such key in this declaration!!");
                            }
                        });
                        break;
                    }
                default:
                    {
                        this.CurrentSession.StopBusyIndicator();
                        var window = new MessageWindow_1.MessageWindow();
                        window.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                        break;
                    }
            }
        }
    };
    DeclarationCorrectionsComponent.prototype.GetEditedScreenTitle = function (entityName, view) {
        var title = "";
        switch (entityName.toLowerCase()) {
            case "declaration":
            case "consignment":
                {
                    title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration");
                    break;
                }
            case "supplierinvoice":
                {
                    var declaration = this.EntityPM;
                    var supplierInvoicePM = declaration.SupplierInvoices.find(function (d) { return d.DeclarationId == declaration.Id && d.SequenceNumeric == view.Line; });
                    if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = supplierInvoicePM.InvoiceNumber + "-" + declaration.DeclarationNumber + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    else if ((Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && !Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = declaration.DeclarationNumber + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && (Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = supplierInvoicePM.InvoiceNumber + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    if ((Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && (Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    break;
                }
            case "supplierinvoiceitem":
                {
                    title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
                    break;
                }
            case "supplierinvioceitemscertificate":
                {
                    title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Certificates");
                    break;
                }
        }
        return title;
    };
    DeclarationCorrectionsComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    DeclarationCorrectionsComponent.prototype.GetResources = function (amendmentArray) {
        var _this = this;
        var tables = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(amendmentArray)) {
            this.arrayLength = amendmentArray.length;
            amendmentArray.forEach(function (el) {
                if (!Tools_1.AppTool.IsNullOrEmpty(el.FieldNameTextCode)) {
                    var splittedWords = el.FieldNameTextCode.split('.');
                    var objectTableName = splittedWords[0] + "." + splittedWords[1];
                    //#region Get resources
                    if (objectTableName == 'Customs.SupplierInvioceItemsCertificate') {
                        objectTableName = 'Customs.SupplierInvioceItemCertificat';
                    }
                    console.log("Get resources for ===> ", objectTableName);
                    _this.EntityResourceService.getEntityResourceByTableName(objectTableName).subscribe(function (response) {
                        if (_this.arrayLength != 1) {
                            _this.arrayLength--;
                        }
                        else {
                            //this.LoadConstriantsList(errors);
                        }
                    });
                    //#endregion 
                }
                else {
                    _this.arrayLength--;
                    console.log("No FieldNameTextCode", el);
                }
            });
        }
    };
    //#endregion
    //#region XML Errors
    //EditEntity(amendmentView: DeclarationErrorView) {
    //    if (AppTool.IsNullOrEmpty(amendmentView)) {
    //        console.warn("[!] There is no declaraion error for the constraint!");
    //    } else {
    //        this.CurrentSession.StartBusyIndicatorLoading();
    //        switch (amendmentView.EntityName.toLowerCase()) {
    //            case "declaration":
    //            case "consignment":
    //                {
    //                        var logWindow = new LogitudeWindow();
    //                        logWindow.Width = 1000;
    //                        logWindow.Height = 700;
    //                        logWindow.ShowCloseButton = true;
    //                        logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                        logWindow.Show('./Customs/Components/Declaration/EditTabs/General/DeclarationGeneralComponent');
    //                        logWindow.WindowArgs = {
    //                            DeclarationError: amendmentView,
    //                            entityArgs: this.entityArgs,
    //                            entityPM: this.EntityPM,
    //                            IsDisplayOnly: this.IsDisplayOnly,
    //                        };
    //                        this.CurrentSession.StopBusyIndicator();
    //                    break;
    //                }
    //            case "supplierinvoice": {
    //                this.declarationWebService
    //                    .GetSupplierInvoiceBySequenceNumber(amendmentView.DeclarationId, +amendmentView.LineNumber, 0, 500)
    //                    .subscribe((response: ServiceResponse) => {
    //                        console.log("[Response] GetSupplierInvoiceBySequenceNumber: ", response);
    //                        var supplierInvoicePM = response.Result;
    //                        if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {
    //                            this.CurrentSession.StartBusyIndicatorLoading();
    //                            var windowArgs: any = {};
    //                            windowArgs.EntityPM = supplierInvoicePM;
    //                            windowArgs.declarationPM = this.EntityPM;
    //                            windowArgs.DeclarationError = amendmentView;
    //                            var logWindow = new LogitudeWindow();
    //                            logWindow.Width = 1030;
    //                            logWindow.Height = 600;
    //                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;
    //                            }
    //                            else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;
    //                            }
    //                            else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
    //                            }
    //                            else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
    //                            }
    //                            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
    //                            logWindow.ShowCloseButton = false;
    //                            logWindow.WindowArgs = windowArgs;
    //                            logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                            logWindow.WindowClosed.subscribe(($event: any) => {
    //                            });
    //                            logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/AddEditSupplierInvoiceComponent');
    //                            this.CurrentSession.StopBusyIndicator();
    //                        }
    //                        else {
    //                            var window = new MessageWindow();
    //                            window.Show("There is no invoice with such key in this declaration!!");
    //                        }
    //                    });
    //                break;
    //            }
    //            case "supplierinvoiceitem":
    //                {
    //                    if (AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
    //                        console.log("No line number", amendmentView);
    //                        return;
    //                    }
    //                    var lines = amendmentView.LineNumber.split(',');
    //                    var invSequence = +lines[0];
    //                    var itemSequence = +lines[1];
    //                    this.declarationWebService
    //                        .GetSupplierInvoiceWithItemBySequenceNumber(amendmentView.DeclarationId, invSequence, itemSequence, 0, 500)
    //                        .subscribe((response: ServiceResponse) => {
    //                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
    //                            var supplierInvoicePM = response.Result;
    //                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {
    //                                this.CurrentSession.StartBusyIndicatorLoading();
    //                                var windowArgs: any = {};
    //                                windowArgs.EntityPM = supplierInvoicePM;
    //                                windowArgs.declarationPM = this.EntityPM;
    //                                windowArgs.DeclarationError = amendmentView;
    //                                var logWindow = new LogitudeWindow();
    //                                logWindow.Width = 1030;
    //                                logWindow.Height = 600;
    //                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;
    //                                }
    //                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;
    //                                }
    //                                else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
    //                                }
    //                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
    //                                }
    //                                windowArgs.IsDisplayOnly = this.IsDisplayOnly;
    //                                logWindow.ShowCloseButton = false;
    //                                logWindow.WindowArgs = windowArgs;
    //                                logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                                this.CurrentSession.StopBusyIndicator();
    //                                logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/AddEditSupplierInvoiceComponent');
    //                                this.CurrentSession.StopBusyIndicator();
    //                            }
    //                            else {
    //                                var window = new MessageWindow();
    //                                window.Show("There is no invoice with such key in this declaration!!");
    //                            }
    //                        });
    //                    break;
    //                }
    //            case "supplierinvioceitemscertificate":
    //                {
    //                    if (AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
    //                        console.log("No line number", amendmentView);
    //                        return;
    //                    }
    //                    var lines = amendmentView.LineNumber.split(',');
    //                    var invSequence = +lines[0];
    //                    var itemSequence = +lines[1];
    //                    this.declarationWebService
    //                        .GetSupplierInvoiceWithItemBySequenceNumber(amendmentView.DeclarationId, invSequence, itemSequence, 0, 500)
    //                        .subscribe((response: ServiceResponse) => {
    //                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
    //                            var supplierInvoicePM: SupplierInvoicePM = response.Result;
    //                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {
    //                                this.CurrentSession.StartBusyIndicatorLoading();
    //                                var invoiceItem = supplierInvoicePM.SupplierInvoiceItems.find(d => d.SequenceNumeric == amendmentView.ParentLine);
    //                                if (!AppTool.IsNullOrEmpty(invoiceItem)) {
    //                                    // open certificate
    //                                    var windowArgs: any = {};
    //                                    windowArgs.SupplierInvoiceItemPM = invoiceItem;
    //                                    windowArgs.DeclarationError = amendmentView;
    //                                    windowArgs.IsDisplayOnly = this.IsDisplayOnly;
    //                                    windowArgs.InvoiceNumber = supplierInvoicePM.InvoiceNumber;
    //                                    windowArgs.SupplierInvoicePM = supplierInvoicePM;
    //                                    var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
    //                                    var logWindow = new LogitudeWindow();
    //                                    logWindow.Width = 1000;
    //                                    logWindow.Height = 600;
    //                                    //if (supplierInvoicePM.ClassificationCode != null) {
    //                                    //    logWindow.Title = "אישורים לפרט מכס" + " " + supplierInvoicePM.ClassificationCode;
    //                                    //}
    //                                    //else {
    //                                    //    logWindow.Title = "אישורים לפרט מכס";
    //                                    //}
    //                                    logWindow.ShowCloseButton = false;
    //                                    logWindow.WindowArgs = windowArgs;
    //                                    logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                                    this.CurrentSession.StopBusyIndicator();
    //                                    logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
    //                                    //end open certificate
    //                                }
    //                                this.CurrentSession.StopBusyIndicator();
    //                            }
    //                            else {
    //                                var window = new MessageWindow();
    //                                window.Show("There is no invoice with such key in this declaration!!");
    //                            }
    //                        });
    //                    break;
    //                }
    //            default:
    //                {
    //                    var window = new MessageWindow();
    //                    window.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
    //                    break;
    //                }
    //        }
    //    }
    //}
    //ShowXMLErrors(error) {
    //    //if (!AppTool.IsNullOrEmpty(error.Field)) {
    //    //    this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
    //    //}
    //    var errors = [];
    //    if (!AppTool.IsNullOrEmpty(error.Description)) {
    //        var xmlErrors: any[] = error.Description.split(/,|:/);
    //        for (var xmlError of xmlErrors) {
    //            errors.push(xmlError);
    //        }
    //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
    //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
    //    }
    //    if (error.EntityName != null) {
    //        if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
    //            //if (OnShowXMLErrors != null) {
    //            //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
    //            //}
    //        }
    //    }
    //}
    //GetEditedScreenTitle(entityName: string, amendmentView: DeclarationErrorView) {
    //    var title = "";
    //    switch (entityName.toLowerCase()) {
    //        case "declaration":
    //        case "consignment":
    //            {
    //                title = TextCodeTranslator.Translate("Customs.Declaration");
    //                break;
    //            }
    //        case "supplierinvoice":
    //            {
    //                var declaration = this.EntityPM;
    //                var supplierInvoicePM = declaration.SupplierInvoices.find(d => d.DeclarationId == declaration.Id && d.SequenceNumeric == amendmentView.Line);
    //                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
    //                    title = supplierInvoicePM.InvoiceNumber + "-" + declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
    //                }
    //                else if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
    //                    title = declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
    //                }
    //                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
    //                    title = supplierInvoicePM.InvoiceNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
    //                }
    //                if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
    //                    title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
    //                }
    //                break;
    //            }
    //        case "supplierinvoiceitem":
    //            {
    //                title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
    //                break;
    //            }
    //        case "supplierinvioceitemscertificate":
    //            {
    //                title = TextCodeTranslator.Translate("Customs.Declaration.O.Certificates");
    //                break;
    //            }
    //    }
    //    return title;
    //}
    //#endregion
    DeclarationCorrectionsComponent.prototype.GetFieldName = function (item) {
        var translation = TextCodeTranslator_1.TextCodeTranslator.Translate(item.FieldNameTextCode);
        if (Tools_1.AppTool.IsNullOrEmpty(translation)) {
            return item.Field;
        }
        return translation;
    };
    DeclarationCorrectionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: '././DeclarationCorrectionsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], DeclarationCorrectionsComponent);
    return DeclarationCorrectionsComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationCorrectionsComponent = DeclarationCorrectionsComponent;
//# sourceMappingURL=DeclarationCorrectionsComponent.js.map