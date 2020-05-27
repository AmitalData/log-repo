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
var DeclarationDisplayOnlyChecks_1 = require("../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var DeclarationEventManager_1 = require("../../../../../Customs/Utilities/DeclarationEventManager");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var MultiCertificatesService_1 = require("../../../../../Customs/Services/Others/MultiCertificatesService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var CertificateTabComponent = /** @class */ (function (_super) {
    __extends(CertificateTabComponent, _super);
    function CertificateTabComponent(entityArgs, CD, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.ShowStorageStatusMessage = false;
        _this.DisplayOnlyMessage = "";
        _this.SelectedItem = null;
        _this.multiCertificatesService = new MultiCertificatesService_1.MultiCertificatesService();
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        //private declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.showTemplate = false;
        _this.CertificateTicketsList = [];
        _this.preventSelect = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ThereIsNoInvoices = false;
        _this.IsFirstTime = true;
        //#endregion
        //#region DemandState Filter Methods
        _this.DemandStateFilterSelectedValue = 'All';
        //#endregion
        //#region LevelSelection Filter Methods
        _this.LevelSelectionFilterSelectedValue = 'DeclarationConect';
        //#endregion
        //#region Filter Methods
        // public FilterSelectedValue: string = null;
        //FilterItemClicked(itemValue: string) {
        //    if (this.FilterSelectedValue != itemValue) {
        //        this.FilterSelectedValue = itemValue;
        //    }
        //}
        //#endregion
        //#region Properties
        _this.SelectedCounterKey = null;
        _this.DemandState = null;
        _this.selectedInvoiceNumber = null;
        _this.columns = null;
        _this.DataSource = {
            pageSize: 10,
            rowCount: null,
            sortingCol: "InvoiceNumber",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.ignoreCount = false;
        _this.IsCheckBoxVisible = false;
        _this.SelectedRow = null;
        _this.originalCertificateList = [];
        _this.SelectedItemsCount = 0;
        _this.CurrentSession.SubscriptionAdd(_this.CurrentSession.CurrentEditComponent.SubscriptionAdd(_this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res == "certificate") {
                _this.preventSelect = true;
            }
        })));
        //SessionLocator.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
        //    SessionLocator.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(response => {
        _this.DeclarationPM = _this.entityArgs.EntityPM;
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.BuildColumns();
        _this.Listen();
        console.log("Declaration", _this.DeclarationPM);
        _this.connectedItems = new ObservableCollection_1.ObservableCollection([]);
        _this.ExcludedItems = new ObservableCollection_1.ObservableCollection([]);
        _this.DisplayOnlyCheck();
        _this.CheckDeclarationInvoices();
        return _this;
        //  this.GetCertificates(null);
        //this.CurrentSession.SelectItemEvent.subscribe((res) => {
        //    if (res.selected) {
        //        //  this.showTemplate = true;
        //        if (!this.connectedItems.Collection.includes(res.Data))
        //            this.connectedItems.Insert(res.data);
        //        this.SelectedItemsCount += 1;
        //        if (this.dataCount != null) {
        //            if (this.SelectedItemsCount == this.dataCount) {
        //                //    this.IsSelected = true;
        //            }
        //            this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();
        //        }
        //    }
        //    else {
        //        this.connectedItems.Remove(res.data);
        //        this.SelectedItemsCount -= 1;
        //        if (this.IsSelected) this.IsSelected = false;
        //        if (this.dataCount != null) {
        //            this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();
        //        }
        //    }
        //    if (this.SelectedItemsCount > 0) {
        //        this.IsVisible = true;
        //    }
        //    else {
        //        this.IsVisible = false;
        //    }
        //});
        //});
        //});
    }
    ;
    CertificateTabComponent.prototype.CheckDeclarationInvoices = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
        this.multiCertificatesService.DeclarationHasInvoices(this.DeclarationPM.Id).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (!response.Result) {
                if (!_this.IsDisplayOnly) {
                    _this.ThereIsNoInvoices = true;
                    _this.NoInvoicesMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EnterAtLeastInvoice");
                }
            }
            else {
                _this.ReloadCertificateTickets(true);
            }
        });
    };
    CertificateTabComponent.prototype.ngOnInit = function () {
        //  this.BuildColumns();
    };
    CertificateTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.DeclarationPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            ;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.DeclarationPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    //   this.GetCertificates(null);
                    _this.DisplayOnlyCheck();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DECR") {
                        _this.activeItem = null;
                        _this.selecteCertificate = null;
                        //   this.ReloadCertificateTickets(true);
                        _this.DisplayOnlyCheck();
                        _this.ConfirmationTypeCode = null;
                        _this.CheckDeclarationInvoices();
                    }
                }
            }));
        }
    };
    CertificateTabComponent.prototype.ReloadCertificateTickets = function (isFirstTime) {
        this.IsFirstTime = isFirstTime;
        this.GetCertificates(null);
    };
    CertificateTabComponent.prototype.GetCertificates = function (message) {
        var _this = this;
        if (message == "ok" || message == null) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
            this.connectedItems.Collection = [];
            this.ExcludedItems.Collection = [];
            this.ThereIsNoInvoices = false;
            this.IsVisible = false;
            this.SelectedItemsCountText = null;
            this.SelectedItemsCount = 0;
            this.IsSelected = false;
            var confirmation = null;
            if (this.ConfirmationType) {
                confirmation = this.ConfirmationType.Code;
            }
            this.declarationWebService.GetCertificateTickets(this.DeclarationPM.Id, confirmation, this.selectedInvoiceNumber, this.SelectedCounterKey, this.DemandState)
                .subscribe(function (response) {
                console.log("[Response] GetCertificateTickets: ", response);
                _this.CertificateTicketsList = [];
                _this.ConfirmationTypesFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
                var reqConfirmationCodes = "";
                var res = response.Result;
                if (res) {
                    if (res.length > 0) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                            _this.IsCheckBoxVisible = true;
                            for (var i = 0; i < res.length; i++) {
                                reqConfirmationCodes = reqConfirmationCodes + res[i].ReqConfirmationTypeCode + ',';
                                var item = new CertificateTicketListItem(res[i], _this);
                                //if (i == 0) {
                                //    if (this.activeItem == null) {
                                //        this.activeItem = item;
                                //    }
                                //}
                                if (item.AttachmentTypeCode == "4") {
                                    item.ExepmtVisibility = true;
                                }
                                else {
                                    item.ConfirmationVisibility = true;
                                }
                                _this.CertificateTicketsList.push(item);
                            }
                            if (reqConfirmationCodes != null) {
                                reqConfirmationCodes = reqConfirmationCodes.substr(0, reqConfirmationCodes.length - 1);
                            }
                            _this.ConfirmationTypesFilterItems.addAdditionalFilter("Code", reqConfirmationCodes, null, null, "InListExact", false, false, false, "string", false, true);
                            if (_this.activeItem != null) {
                                _this.activeItem = _this.CertificateTicketsList.filter(function (d) { return d.AttachmentTypeCode == _this.activeItem.AttachmentTypeCode && d.ReqConfirmationTypeCode == _this.activeItem.ReqConfirmationTypeCode && d.ResConfirmationTypeCode == _this.activeItem.ResConfirmationTypeCode && d.CustomsAttachmentId == _this.activeItem.CustomsAttachmentId && d.CertificateNumber == _this.activeItem.CertificateNumber; })[0];
                            }
                            if (_this.activeItem == null) {
                                _this.activeItem = _this.CertificateTicketsList[0];
                            }
                            if (_this.selecteCertificate != null) {
                                var selected = _this.CertificateTicketsList.filter(function (d) { return d.AttachmentTypeCode == _this.selecteCertificate.AttachmentTypeCode && d.ReqConfirmationTypeCode == _this.selecteCertificate.ReqConfirmationTypeCode && d.ResConfirmationTypeCode == _this.selecteCertificate.ResConfirmationTypeCode && d.CustomsAttachmentId == _this.selecteCertificate.CustomsAttachmentId && d.CertificateNumber == _this.selecteCertificate.CertificateNumber; })[0];
                            }
                            if (selected != null)
                                _this.selecteCertificate = selected.ticket;
                            if (selected == null) {
                                _this.selecteCertificate = _this.CertificateTicketsList[0].ticket;
                            }
                            _this.GetCertificatesCompleted();
                        }
                    }
                    else {
                        _this.selecteCertificate = null;
                        _this.IsCheckBoxVisible = false;
                    }
                }
                _this.LoadConnectedItems(null);
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    CertificateTabComponent.prototype.LoadConnectedItems = function (message) {
        this.preventSelect = false;
        if (message == "ok" || message == null) {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }
    };
    CertificateTabComponent.prototype.GetCertificatesCompleted = function () {
        /*** put you code here, alaa ***/
        if (this.SelectedInvoiceNumber == null) {
            this.FillInvoiceNumbersList();
        }
    };
    CertificateTabComponent.prototype.InvoicesSelectionChanged = function (selectedItem) {
        if (selectedItem != null) {
            this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
            this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
            this.SelectedInvoiceReqConfirmation = selectedItem.ReqConfirmationTypeCode;
            this.ReloadCertificateTickets(false);
        }
    };
    CertificateTabComponent.prototype.FillInvoiceNumbersList = function () {
        var _this = this;
        this.declarationWebService.GetDeclarationInvoicesNumbers(this.DeclarationPM.Id)
            .subscribe(function (response) {
            console.log("[Response] GetDeclarationInvoicesNumbers: ", response);
            var res = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                _this.InvoicesNumbersList = [];
                _this.InvoicesNumbersList = res;
            }
        });
    };
    CertificateTabComponent.prototype.DemandStateFilterItemClicked = function (itemValue) {
        if (this.DemandStateFilterSelectedValue != itemValue) {
            this.DemandStateFilterSelectedValue = itemValue;
            this.DemandState = itemValue == 'All' ? null : itemValue;
            this.ReloadCertificateTickets(false);
        }
    };
    CertificateTabComponent.prototype.LevelSelectionFilterItemClicked = function (itemValue) {
        if (this.LevelSelectionFilterSelectedValue != itemValue) {
            this.LevelSelectionFilterSelectedValue = itemValue;
            if (itemValue != "Invoice") {
                this.SelectedInvoiceNumber = null;
                this.SelectedCounterKey = null;
                this.ReloadCertificateTickets(false);
            }
        }
    };
    Object.defineProperty(CertificateTabComponent.prototype, "SelectedInvoiceNumber", {
        get: function () { return this.selectedInvoiceNumber; },
        set: function (newValue) {
            this.selectedInvoiceNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    CertificateTabComponent.prototype.RefreshEntity = function () {
        //if (this.IsDisplayOnly) {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        //}
        if (!this.ThereIsNoInvoices) {
            this.activeItem = null;
            this.selecteCertificate = null;
            this.ReloadCertificateTickets(false);
            this.preventSelect = false;
        }
    };
    CertificateTabComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            //this.SetScreenFieldsEditability();
            this.timerToken = setTimeout(function () {
                DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(_this.IsDisplayOnly);
            }, 200);
            return;
        }
        else if (this.DeclarationPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.DeclarationPM.StorageStatusName;
        }
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.DeclarationPM).subscribe(function (response) {
            var displayOnlyCheckResult = response.Result;
            _this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (_this.IsDisplayOnly) {
                _this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (_this.DeclarationPM.StorageStatusCode) {
                _this.ShowStorageStatusMessage = true;
                _this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + _this.DeclarationPM.StorageStatusName;
            }
            _this.timerToken = setTimeout(function () {
                DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(_this.IsDisplayOnly);
            }, 200);
        });
    };
    CertificateTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            IsCheckBox: true
            // HtmlListComponentName: 'CertificateCheckBoxComponent',
            // HtmlListComponentUrl: './Customs/Components/ListTemplates/CertificateCheckBoxComponent',
        });
        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: 'מספר חשבון',
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'SequenceNumeric',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.SequenceNumeric"),
            Styles: { width: '30px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TradeAgreementName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementCode"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'OriginCountryName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryName"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: "CatalogNumber",
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.CatalogNumber"),
            IsCustomTemplate: true,
            Styles: { width: '80px' },
            HtmlListComponentName: 'CertificateTextBoxComponent',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CertificateTextBoxComponent',
        });
    };
    Object.defineProperty(CertificateTabComponent.prototype, "IsSelected", {
        get: function () { return this.isSelected; },
        set: function (value) {
            this.isSelected = value;
            //this.SelectedItemsCount = 0;
            if (this.isSelected) {
                this.dataCount = this.DataSource.rowCount;
                this.SelectedItemsCount = this.dataCount;
                if (this.dataCount) {
                    this.SelectedItemsCountText = "נבחרו " + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
                }
                this.IsVisible = true;
                this.ignoreCount = true;
                if (this.selecteCertificate != null) {
                    this.selecteCertificate.IsAllSelected = true;
                }
            }
            else {
                this.SelectedItemsCount = 0;
                this.IsVisible = false;
                if (this.selecteCertificate != null) {
                    this.selecteCertificate.IsAllSelected = false;
                }
            }
            //    //this.CurrentSession.ConnectedItemSelectedEvent.emit({ Count: "All" });
            //    this.SelectedItemsCount = this.dataCount;
            //    this.selecteCertificate.IsAllSelected = true;
            //    this.SelectedItemsCountText = "נבחרו " + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
            //    this.IsVisible = true;
            //}
            //else {
            //    if (this.dataCount == this.SelectedItemsCount) {
            //       // this.CurrentSession.ConnectedItemSelectedEvent.emit({ Count: "None" });
            //        this.SelectedItemsCount = 0;
            //        this.SelectedItemsCountText = null;
            //        if (this.selecteCertificate) {
            //            this.selecteCertificate.IsAllSelected = false;
            //        }
            //        this.IsVisible = false;
            //    }
            //}
        },
        enumerable: true,
        configurable: true
    });
    ;
    CertificateTabComponent.prototype.onCheckBoxChecked = function ($event) {
        if ($event.IsChecked) {
            if (!this.connectedItems.Collection.includes($event.rowData))
                this.connectedItems.Insert($event.rowData);
            if (this.IsSelected) {
                if (this.ExcludedItems.Collection.includes($event.rowData)) {
                    this.ExcludedItems.Remove($event.rowData);
                }
            }
            //if (this.ignoreCount == false) {
            this.SelectedItemsCount += 1;
            //}
            this.dataCount = this.DataSource.rowCount;
            if (this.dataCount != null) {
                this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();
            }
            if (this.SelectedItemsCount == this.dataCount) {
                this.IsSelected = true;
            }
        }
        else {
            this.connectedItems.Remove($event.rowData);
            if (this.IsSelected) {
                if (!this.ExcludedItems.Collection.includes($event.rowData)) {
                    this.ExcludedItems.Insert($event.rowData);
                }
            }
            //if (this.ignoreCount == false) {
            this.SelectedItemsCount -= 1;
            //}
            //if (this.IsSelected) this.IsSelected = false;
            if (this.dataCount != null) {
                this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();
            }
        }
        if (this.SelectedItemsCount > 0) {
            this.IsVisible = true;
        }
        else {
            this.IsVisible = false;
        }
        this.CD.detectChanges();
    };
    CertificateTabComponent.prototype.ViewInitCompleted = function ($event) {
        /// this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    CertificateTabComponent.prototype.OnRowSelected = function (CurrentRow) {
        var _this = this;
        if (this.preventSelect == false) {
            //if (!this.showTemplate) {
            this.SelectedRow = CurrentRow.rowData;
            this.CurrentSession.StartBusyIndicatorLoading();
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(function (response) {
                _this.declarationWebService
                    .GetSupplierInvoiceWithSpecificItemByCounterKey(_this.SelectedRow.DeclarationId, _this.SelectedRow.InvoiceCounterKey, _this.SelectedRow.SequenceNumeric)
                    .subscribe(function (response) {
                    console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
                    _this.preventSelect = true;
                    var supplierInvoicePM = response.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                        _this.CurrentSession.StartBusyIndicatorLoading();
                        var windowArgs = {};
                        windowArgs.EntityPM = supplierInvoicePM;
                        windowArgs.declarationPM = _this.DeclarationPM;
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 1030;
                        logWindow.Height = 600;
                        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.DeclarationPM.DeclarationNumber)) {
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + _this.DeclarationPM.DeclarationNumber;
                        }
                        else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.DeclarationPM.DeclarationNumber)) {
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.DeclarationNumber;
                        }
                        else if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.DeclarationPM.DeclarationNumber)) {
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
                        }
                        else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.DeclarationPM.DeclarationNumber)) {
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                        }
                        windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                        logWindow.ShowCloseButton = false;
                        logWindow.WindowArgs = windowArgs;
                        logWindow.WindowClosed.subscribe(function ($event) {
                            _this.LoadConnectedItems($event);
                            _this.CD.reattach();
                            _this.RefreshEntity();
                        });
                        _this.CD.detach();
                        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        var window = new MessageWindow_1.MessageWindow();
                        window.Show("There is no invoice with such key in this declaration!!");
                        _this.preventSelect = false;
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            });
            //}
            //this.showTemplate = false;
        }
        else {
            //this.preventSelect = false;
        }
    };
    CertificateTabComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        if (this.SelectedInvoiceNumber != null) {
            filters.addAdditionalFilter("InvoiceNumber", this.SelectedInvoiceNumber, null, null, "Equals", false, false, false, "string");
        }
        if (this.selecteCertificate) {
            if (this.ConfirmationType) {
                return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, this.selecteCertificate.AttachmentTypeCode, this.ConfirmationType.Code, this.selecteCertificate.CertificateExemptionTypeCode, this.selecteCertificate.CertificateNumber, this.selecteCertificate.ResConfirmationTypeCode);
            }
            else {
                return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, this.selecteCertificate.AttachmentTypeCode, this.selecteCertificate.ReqConfirmationTypeCode, this.selecteCertificate.CertificateExemptionTypeCode, this.selecteCertificate.CertificateNumber, this.selecteCertificate.ResConfirmationTypeCode);
            }
        }
        //else if (this.ConfirmationType != null){
        //    return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, null, this.ConfirmationType.Code, null, null,null);
        //}
        else {
            return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, "10", null, null, null, null);
        }
    };
    CertificateTabComponent.prototype.CreateMethod = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.Ticket = this.selecteCertificate;
        windowArgs.IsAllSelected = this.IsSelected;
        windowArgs.IsNew = true;
        windowArgs.DeclarationId = this.DeclarationPM.Id;
        windowArgs.ConnectedItems = this.connectedItems.Collection;
        windowArgs.ExcludedItems = this.ExcludedItems.Collection;
        windowArgs.Parent = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        if (this.selecteCertificate.ReqConfirmationTypeName != null) {
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate") + " - " + this.selecteCertificate.ReqConfirmationTypeName + " " + this.selecteCertificate.ReqConfirmationTypeCode;
        }
        else {
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate");
        }
        logWindow.Width = 550;
        logWindow.Height = 300;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "ok") {
                _this.GetCertificates($event);
                _this.CD.reattach();
                _this.RefreshEntity();
            }
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CreateEditTicketComponent');
    };
    CertificateTabComponent.prototype.MoveMethod = function () {
        var _this = this;
        var windowArgs = {};
        var certificates = [];
        windowArgs.Ticket = this.selecteCertificate;
        windowArgs.IsAllSelected = this.IsSelected;
        windowArgs.certificateList = this.CertificateTicketsList.filter(function (item) { return item.ticket != _this.selecteCertificate && item.ReqConfirmationTypeCode == _this.selecteCertificate.ReqConfirmationTypeCode; });
        windowArgs.ConnectedItems = this.connectedItems.Collection;
        windowArgs.ExcludedItems = this.ExcludedItems.Collection;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "העבר לאישור";
        logWindow.Width = 900;
        logWindow.Height = 600;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.GetCertificates($event);
            _this.CD.reattach();
            _this.RefreshEntity();
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CertificateSelectionComponent');
    };
    Object.defineProperty(CertificateTabComponent.prototype, "ConfirmationType", {
        get: function () { return this.confirmationType; },
        set: function (value) {
            if (this.confirmationType != value) {
                this.confirmationType = value;
                if (value) {
                    this.ConfirmationTypeCode = value.Code;
                }
                this.GetCertificates(null);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTabComponent.prototype, "ConfirmationTypeCode", {
        get: function () { return this.confirmationTypeCode; },
        set: function (value) {
            this.confirmationTypeCode = value;
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CertificateTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    CertificateTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CertificateTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], CertificateTabComponent);
    return CertificateTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CertificateTabComponent = CertificateTabComponent;
var CertificateTicketListItem = /** @class */ (function (_super) {
    __extends(CertificateTicketListItem, _super);
    function CertificateTicketListItem(entity, Parent) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.SupplierInvioceItemCertificat";
        _this.DataContext = _this;
        _this.multiCertificatesService = new MultiCertificatesService_1.MultiCertificatesService();
        _this.ticket = entity;
        _this.parent = Parent;
        _this.IsDisplayOnly = _this.parent.IsDisplayOnly;
        //   this.parent.selecteCertificate = entity;
        if (!Tools_1.AppTool.IsNullOrEmpty(entity.ReqConfirmationTypeName)) {
            _this.Title = entity.ReqConfirmationTypeName + " " + entity.ReqConfirmationTypeCode;
        }
        _this.FilterSelectedValue = _this.ticket.AttachmentTypeCode;
        if (_this.FilterSelectedValue == "4") {
            _this.ConfirmationVisibility = false;
            _this.ExepmtVisibility = true;
        }
        else {
            _this.ConfirmationVisibility = true;
            _this.ExepmtVisibility = false;
        }
        return _this;
    }
    Object.defineProperty(CertificateTicketListItem.prototype, "ConfirmationVisibility", {
        get: function () { return this.confirmationVisibility; },
        set: function (newValue) {
            this.confirmationVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "ExepmtVisibility", {
        get: function () { return this.exepmtVisibility; },
        set: function (newValue) {
            this.exepmtVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "CertificateNumber", {
        get: function () { return this.ticket.CertificateNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "ReqConfirmationTypeCode", {
        get: function () { return this.ticket.ReqConfirmationTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "ResConfirmationTypeName", {
        get: function () { return this.ticket.ResConfirmationTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "AttachmentTypeCode", {
        get: function () { return this.ticket.AttachmentTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "ReqConfirmationTypeName", {
        get: function () { return this.ticket.ReqConfirmationTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "AttachmentTypeName", {
        get: function () {
            return this.ticket.AttachmentTypeName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "CertificateExemptionTypeName", {
        get: function () { return this.ticket.CertificateExemptionTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "CertificateExemptionTypeCode", {
        get: function () { return this.ticket.CertificateExemptionTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "CustomsAttachmentId", {
        get: function () { return this.ticket.CustomsAttachmentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CertificateTicketListItem.prototype, "ResConfirmationTypeCode", {
        get: function () { return this.ticket.ResConfirmationTypeCode; },
        enumerable: true,
        configurable: true
    });
    CertificateTicketListItem.prototype.CertificateItemClicked = function (item) {
        this.parent.SelectedItem = this;
        this.parent.IsVisible = false;
        this.parent.selecteCertificate = this.ticket;
        this.parent.activeItem = item;
        this.parent.SelectedItemsCountText = null;
        this.parent.SelectedItemsCount = 0;
        this.parent.IsSelected = false;
        this.parent.selecteCertificate.IsAllSelected = false;
        this.parent.LoadConnectedItems(null);
    };
    CertificateTicketListItem.prototype.EditTicket = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item) && !this.parent.IsDisplayOnly) {
            //this.multiCertificatesService.GetCertificateConnectedItems(this.parent.DeclarationPM.Id, this.AttachmentTypeCode, this.ReqConfirmationTypeCode, this.ticket.CertificateExemptionTypeCode, this.CertificateNumber, this.ticket.ResConfirmationTypeCode)
            //    .subscribe((response: ServiceResponse) => {
            //if (!response.HasError) {
            var windowArgs = {};
            this.ticket.IsAllSelected = true;
            windowArgs.IsAllSelected = true;
            windowArgs.Ticket = this.ticket;
            windowArgs.DeclarationId = this.parent.DeclarationPM.Id;
            windowArgs.Items = [];
            windowArgs.Parent = this.parent;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            if (item.ticket.ReqConfirmationTypeName != null) {
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate") + " - " + this.ticket.ReqConfirmationTypeName + " " + this.ticket.ReqConfirmationTypeCode;
            }
            else {
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate");
            }
            logWindow.Width = 550;
            logWindow.Height = 300;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "ok") {
                    _this.ReloadCertificates($event);
                    _this.parent.CD.reattach();
                    _this.parent.RefreshEntity();
                }
            });
            this.parent.CD.detach();
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CreateEditTicketComponent');
            //}
            //});
        }
    };
    CertificateTicketListItem.prototype.ReloadCertificates = function (message) {
        if (message == "ok") {
            this.parent.GetCertificates("ok");
        }
    };
    return CertificateTicketListItem;
}(BaseComponent_1.BaseComponent));
exports.CertificateTicketListItem = CertificateTicketListItem;
//# sourceMappingURL=CertificateTabComponent.js.map