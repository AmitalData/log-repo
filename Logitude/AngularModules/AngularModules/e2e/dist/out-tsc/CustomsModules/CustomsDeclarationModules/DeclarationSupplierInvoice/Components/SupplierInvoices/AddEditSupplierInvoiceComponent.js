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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LocationDirective_1 = require("../../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var SupplierInvoicePMService_1 = require("../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService");
var CustomsDocumentPointersExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomsDocumentPointersExtendedPMService");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var SupplierInvoicePM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoicePM");
var SupplierInvoiceFreightAmountPM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceFreightAmountPM");
var SupplierInvoiceModificationPM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceModificationPM");
var AmitalGatewayUtil_1 = require("../../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var SupplierInvoiceExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
var GITITEMExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/GITITEMExtendedPMService");
var DeclarationEventManager_1 = require("../../../../../Customs/Utilities/DeclarationEventManager");
var ObjectsLocator_1 = require("../../../../../Infrastructure/Locators/ObjectsLocator");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var VendorCommissionPMService_1 = require("../../../../../Customs/Services/StandardPMs/VendorCommissionPMService");
var ModificationAndDiscountTypeListService_1 = require("../../../../../Customs/Services/StandardLists/ModificationAndDiscountTypeListService");
var SupplierInvoiceItemPM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceItemPM");
var VendorCommissionService_1 = require("../../../../../Customs/Services/WebServices/VendorCommissionService");
var CustomsSettingExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var GITITEMCacheService_1 = require("../../../../../Customs/Services/Others/GITITEMCacheService");
var AddEditSupplierInvoiceComponent = /** @class */ (function (_super) {
    __extends(AddEditSupplierInvoiceComponent, _super);
    function AddEditSupplierInvoiceComponent() {
        var _this = _super.call(this) || this;
        _this.LayoutDirection = 'ltr';
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.SupplierInvoice";
        _this.ValidationErrorsList = [];
        _this.TabsItemsSource = [];
        _this.IsDisplayOnly = false;
        _this.IsNewEntity = false;
        _this.IsSaveAndNewVisible = true;
        _this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        _this.VendorCommissionPMService = new VendorCommissionPMService_1.VendorCommissionPMService();
        _this.vendorCommissionService = new VendorCommissionService_1.VendorCommissionService(); // extended
        // services
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        _this._ModificationAndDiscountTypeListService = new ModificationAndDiscountTypeListService_1.ModificationAndDiscountTypeListService();
        _this.customsDocumentPointersExtendedPMService = new CustomsDocumentPointersExtendedPMService_1.CustomsDocumentPointersExtendedPMService();
        _this.FirstCurrentLine = 1;
        _this.skipedItems = 0;
        _this.IsNextButtonEnabled = true;
        _this.IsPreviousButtonEnabled = false;
        _this.NextPreviousVisible = false;
        _this.IsFromCustomsAnswer = false;
        _this.IsSelectedRowTextBoxVisibile = false;
        //public ItemCode_LocalCache: ItemCodeComponent[];
        _this.GITITEMExtendedPMService = new GITITEMExtendedPMService_1.GITITEMExtendedPMService();
        _this.NewInvoices = [];
        _this._SkipAutoInsurance = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SearchItemsFound = false;
        _this.SearchItemsMessage = null;
        _this.Retries = 0;
        _this.isViewInited = false;
        _this.GENERAL = null;
        _this.MORE = null;
        //#region properties
        _this.DifferenceColor = "#282E30";
        _this.difference = 0;
        _this.totalForeignCurrency = 0;
        _this.pointers = [];
        //#region Save Code
        _this.SaveAndNew = false;
        _this._IsInitiateNewInstance = false;
        _this.loadingNextItems = false;
        _this._My1stSupplierInvoicePM = null;
        _this.ForceSave = false;
        _this.LineDoesNotExist = false;
        _this.LineDoesNotExistMessage = null;
        //#endregion
        //#region Commission Code
        _this.ReloadModificationEvent = new core_1.EventEmitter();
        _this.CustomerCommissionsList = [];
        _this._DropdownDisplay = 'none';
        //this.ItemCode_LocalCache = [];
        _this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        _this.ikeaFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "IKEA") && f.ObjectTableId == table.Id; })[0];
        _this.screenHeight = _this.GetScreenHeight();
        //this.GetCurrencies();
        _this.ToggleButtonTopPosition = (_this.screenHeight > 768) ? (21) : -81;
        return _this;
    }
    AddEditSupplierInvoiceComponent.prototype.GetScreenHeight = function () {
        return self.innerHeight;
    };
    AddEditSupplierInvoiceComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OldEntityPM = this.EntityPM;
            this.declarationPM = args.declarationPM;
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.IsNewEntity = args.IsNewEntity;
            this.NumberOfLoadedItems = args.NumberOfLoadedItems;
            this.takenItems = this.NumberOfLoadedItems;
            this.SetScreenFieldsEditability();
            this.WindowTitle = args.WindowTitle;
            this.IsFromCustomsAnswer = args.IsFromCustomsAnswer;
            this.IsInvoiceAnswer = args.IsInvoiceAnswer;
            this.FromClassificationJumpToSII = args.FromClassificationJumpToSII;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.FromClassificationJumpToSII)) {
                this.IsSaveAndNewVisible = false;
            }
            if (!this.IsNewEntity && this.EntityPM.FullItemsCount <= 500) { //bug 35255 yarons comment about leting every one see this box.
                this.IsSelectedRowTextBoxVisibile = true;
            }
            else if (this.IsNewEntity) { //bug 35255 yarons comment about leting every one see this box.
                this.IsSelectedRowTextBoxVisibile = true;
            }
            // Document Filing
            this.GetDocumentFilingId();
            if (this.EntityPM.IsAccumalated) {
                this.AccumulatedFilter = "parent";
            }
            this.CurrentSession.SubscriptionAdd(this.CurrentSession.AccumulatedFilterChangedEvent.subscribe(function (res) {
                // this.EntityPM = res.entityPM;
                _this.skipedItems = 0;
                _this.takenItems = 500;
                _this.FirstCurrentLine = null;
                if (res.filter == "Accumulated") {
                    _this.AccumulatedFilter = "parent";
                    if (_this.EntityPM.FullParentsCount <= 500) {
                        _this.NextPreviousVisible = false;
                        //if (this.ikeaFeature) { bug 35255 yarons comment about leting every one see this box.
                        _this.IsSelectedRowTextBoxVisibile = true;
                        // }
                    }
                    else {
                        _this.InvoiceItemsMessage = "לחשבון זה קיימות  " + _this.EntityPM.FullParentsCount + " שורות , מציג שורות  " + 1 + " עד " + _this.NumberOfLoadedItems;
                        _this.NextPreviousVisible = true;
                        _this.IsSelectedRowTextBoxVisibile = false;
                    }
                    _this.IsNextButtonEnabled = true;
                    _this.IsPreviousButtonEnabled = false;
                    //  this.ReloadSupplierInvoiceWithItems(0, 500);
                }
                else if (res.filter == "NotAccumulated") {
                    _this.AccumulatedFilter = "child";
                    if (_this.EntityPM.FullChildrenCount <= 500) {
                        _this.NextPreviousVisible = false;
                        // if (this.ikeaFeature) { bug 35255 yarons comment about leting every one see this box.
                        _this.IsSelectedRowTextBoxVisibile = true;
                        //}
                    }
                    else {
                        _this.InvoiceItemsMessage = "לחשבון זה קיימות  " + _this.EntityPM.FullChildrenCount + " שורות , מציג שורות  " + 1 + " עד " + _this.NumberOfLoadedItems;
                        _this.NextPreviousVisible = true;
                        _this.IsSelectedRowTextBoxVisibile = false;
                    }
                    _this.IsNextButtonEnabled = true;
                    _this.IsPreviousButtonEnabled = false;
                    //  this.ReloadSupplierInvoiceWithItems(0, 500);
                }
                else {
                    //this.AccumulatedFilter = null;
                    _this.NextPreviousVisible = true;
                    _this.IsSelectedRowTextBoxVisibile = false;
                    //   this.ReloadSupplierInvoiceWithItems(0, 500);
                }
            }));
            this.CurrentSession.SearchFilterChangedEvent.subscribe(function (res) {
                if (res.count != null && res.count != 0) {
                    _this.SearchItemsFound = true;
                    _this.SearchItemsMessage = "נמצאו  " + res.count + " תוצאות שתואמות לחיפוש";
                }
                else {
                    _this.SearchItemsFound = false;
                    _this.SearchItemsMessage = null;
                }
            });
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DeclarationError)) {
                this.IsSaveAndNewVisible = false;
                this.ShowXMLErrors(args.DeclarationError);
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(args.AmendmentView)) {
                this.IsSaveAndNewVisible = false;
                this.ShowXMLCorrections(args.AmendmentView);
            }
            this.EntityPM.IsValueForCustomsOnly = this.declarationPM.IsValueForCustomsOnly;
            this.getModTypeName();
            //Calculate commission percentage value
            //this.CalculateCommissionPercentage();
            this.GetCustomerCommissions();
        }
        var customsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        customsSettingExtendedListService.GetSkipAutoInsurancePromise(this.declarationPM.CustomerCode, this.declarationPM.Tenant).subscribe(function (myResult) {
            var res = myResult;
            if (res.Result.SkipAutoInsurance === true) {
                _this._SkipAutoInsurance = true;
            }
            if (FeatureLocator_1.FeatureLocator.IsFeatureGrantedByCode("IFRITZ")) { // If FRITZ always check insurance- Task 37656
                _this._SkipAutoInsurance = false;
            }
            _this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceFreightAmount").subscribe(function (response) {
                    _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceModification").subscribe(function (response) {
                        _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(function (response) {
                            _this.BuildTabs();
                            _this.RunComponent();
                            for (var i = 0; i < _this.EntityPM.SupplierInvoiceItems.length; i++) {
                                _this.itemsLineNumbers = _this.itemsLineNumbers + "," + _this.EntityPM.SupplierInvoiceItems[i].LineNumber;
                            }
                            //    this.itemsLineNumbers = this.itemsLineNumbers.substring(0);
                            _this.GetPointers();
                        });
                    });
                });
            });
        });
        this.IsNextButtonEnabled = true;
        this.IsPreviousButtonEnabled = false;
        if ((this.EntityPM.FullItemsCount > this.NumberOfLoadedItems) && !this.EntityPM.IsAccumalated) {
            //MessageBorderVisibility = Visibility.Visible;
            this.InvoiceItemsMessage = "לחשבון זה קיימות  " + this.EntityPM.FullItemsCount + " שורות , מציג שורות  " + 1 + " עד " + this.NumberOfLoadedItems;
            this.NextPreviousVisible = true;
            this.IsSelectedRowTextBoxVisibile = false;
        }
        else if (this.EntityPM.IsAccumalated && this.EntityPM.FullParentsCount > 500) {
            this.InvoiceItemsMessage = "לחשבון זה קיימות  " + this.EntityPM.FullParentsCount + " שורות , מציג שורות  " + 1 + " עד " + this.NumberOfLoadedItems;
            this.NextPreviousVisible = true;
            this.IsSelectedRowTextBoxVisibile = false;
        }
        else if (this.EntityPM.FullItemsCount < 500) { //&& this.ikeaFeature) {bug 35255 yarons comment about leting every one see this box.
            this.IsSelectedRowTextBoxVisibile = true;
        }
    };
    AddEditSupplierInvoiceComponent.prototype.SetScreenFieldsEditability = function () {
    };
    AddEditSupplierInvoiceComponent.prototype.BuildTabs = function () {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        //this.TabsItemsSource.push(new TabItem("DOCUMENT", "Customs.Declaration.TH.Documents"));
        this.TabsItemsSource.push(new TabItem("MORE", "Customs.Declaration.TH.More"));
        this.selectedTabCode = "GENERAL";
    };
    AddEditSupplierInvoiceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    AddEditSupplierInvoiceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AddEditSupplierInvoiceComponent.prototype.InitializeComponent = function () {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    };
    Object.defineProperty(AddEditSupplierInvoiceComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditSupplierInvoiceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {
                    case "GENERAL": {
                        if (this.GENERAL == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.GENERAL = cmpRef.instance;
                                _this.GENERAL.FromClassificationJumpToSII = _this.FromClassificationJumpToSII;
                                _this.GENERAL.InitTab(_this.EntityPM, _this, _this.IsDisplayOnly, true, _this.IsNewEntity, _this.IsFromCustomsAnswer, _this.IsInvoiceAnswer);
                                _this.GENERAL.ReloadEntityEvent.subscribe(function (response) {
                                    _this.ReloadPromise().then(function () {
                                        _this.GENERAL.InitTab(_this.EntityPM, _this, _this.IsDisplayOnly, true, _this.IsNewEntity, _this.IsFromCustomsAnswer, _this.IsInvoiceAnswer);
                                    });
                                });
                            });
                        }
                        break;
                    }
                    case "DOCUMENTS": {
                        break;
                    }
                    case "MORE": {
                        if (this.MORE == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceMoreTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.MORE = cmpRef.instance;
                                _this.MORE.SetTabArgs({ InvoicePM: _this.EntityPM, IsDisplayOnly: _this.IsDisplayOnly, DeclarationPM: _this.declarationPM, Parent: _this });
                                _this.MORE.FillValidationErrorList.subscribe(function (response) {
                                    _this.ValidationErrorsList = response;
                                });
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    Object.defineProperty(AddEditSupplierInvoiceComponent.prototype, "Difference", {
        get: function () { return this.difference; },
        set: function (newValue) {
            if (this.difference != newValue) {
                this.difference = newValue;
                if (this.TotalForeignCurrency != 0) {
                    if (this.difference != null) {
                        if (this.difference != 0) {
                            this.DifferenceColor = Tools_1.FontTool.Red; //red
                        }
                        else {
                            this.DifferenceColor = Tools_1.FontTool.Green; //green
                        }
                    }
                }
                else {
                    this.DifferenceColor = Tools_1.FontTool.Black;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditSupplierInvoiceComponent.prototype, "TotalForeignCurrency", {
        get: function () { return this.totalForeignCurrency; },
        set: function (newValue) {
            if (this.totalForeignCurrency != newValue) {
                this.totalForeignCurrency = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditSupplierInvoiceComponent.prototype.GetPointers = function () {
        var _this = this;
        this.customsDocumentPointersExtendedPMService.GetCustomDocumentPointersForItems(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey.toString(), this.itemsLineNumbers).subscribe(function (response) {
            var result = response.Result;
            _this.pointers = result;
        });
    };
    // Buttons Handlers
    AddEditSupplierInvoiceComponent.prototype.OkButtonClicked = function () {
        this.SaveAndNew = false;
        //if (this.EntityPM.InvoiceNumber) {
        //    var supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
        //    supplierInvoiceService.GetCheckIfInvoiceNumberExists(this.EntityPM.DeclarationId, this.EntityPM.InvoiceNumber, this.EntityPM.InvoiceCounterKey).subscribe((resp: ServiceResponse) => {
        //        if (!resp.HasError) {
        //            if (resp.Result) {
        //                var confirm = new ConfirmWindow();
        //                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        //                confirm.ShowNoButton = true; 
        //                confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה"+ resp.Result.SequenceNumeric + "- האם להמשיך ?");
        //                confirm.WindowClosed.subscribe((event: any) => {
        //                    if (confirm.Yes) {
        //                        confirm.Close();
        //                        this.SaveButtonClicked();
        //                    }
        //                    else {
        //                        confirm.Close();
        //                    }
        //                });
        //            }
        //            else {
        //                this.SaveButtonClicked();
        //            }
        //        }
        //    });
        //}
        //else {
        //    this.SaveButtonClicked();
        //}
        this.SaveButtonClicked();
    };
    AddEditSupplierInvoiceComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        if ((this.EntityPM.IsDirty || this.ForceSave) && !this.IsDisplayOnly && !this.IsFromCustomsAnswer) {
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
            confirm.WindowClosed.subscribe(function (event) {
                if (confirm.Yes) {
                    confirm.Close();
                    _this.OkButtonClicked();
                }
                else {
                    _this.EntityPM.RejectChanges();
                    _this.GENERAL.Dispose();
                    if (_this.SaveAndNew) {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit('cancel');
                    }
                }
            });
        }
        else {
            //this.EntityPM.RejectChanges();
            this.GENERAL.Dispose();
            if (this.SaveAndNew) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit('cancel');
            }
        }
    };
    Object.defineProperty(AddEditSupplierInvoiceComponent.prototype, "SendMode", {
        get: function () { return this.sendMode; },
        set: function (value) {
            this.sendMode = value;
        },
        enumerable: true,
        configurable: true
    });
    AddEditSupplierInvoiceComponent.prototype.SaveAndNewButtonClicked = function () {
        //if (this.EntityPM.InvoiceNumber) {
        //    var supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
        //    supplierInvoiceService.GetCheckIfInvoiceNumberExists(this.EntityPM.DeclarationId, this.EntityPM.InvoiceNumber, this.EntityPM.InvoiceCounterKey).subscribe((resp: ServiceResponse) => {
        //        if (!resp.HasError) {
        //            if (resp.Result) {
        //                var confirm = new ConfirmWindow();
        //                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        //                confirm.ShowNoButton = true;
        //                confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + resp.Result.SequenceNumeric + "- האם להמשיך ?");
        //                confirm.WindowClosed.subscribe((event: any) => {
        //                    if (confirm.Yes) {
        //                        confirm.Close();
        //                        this.ApplySaveAndNew();
        //                    }
        //                    else {
        //                        confirm.Close();
        //                    }
        //                });
        //            }
        //            else {
        //                this.ApplySaveAndNew();
        //            }
        //        }
        //    });
        //}
        //else {
        //    this.ApplySaveAndNew();
        //}
        this.NewInvoices.push(this.EntityPM);
        ;
        //remove LineDoesNotExist msg
        this.LineDoesNotExist = false;
        this.ApplySaveAndNew();
    };
    AddEditSupplierInvoiceComponent.prototype.ApplySaveAndNew = function () {
        var _this = this;
        var errors = this.ValidateModifications();
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            return;
        }
        var valid = this.PreSaveAndNewValidate();
        var checkFreightValues = true;
        this._IsInitiateNewInstance = true;
        if (this.declarationPM.SupplierInvoices.length > 1) {
            for (var _i = 0, _a = this.declarationPM.SupplierInvoices; _i < _a.length; _i++) {
                var item = _a[_i];
                if (!Tools_1.AppTool.IsNullOrEmpty(item.IncotermCode)) {
                    if (item.IncotermCode.startsWith("E") || item.IncotermCode.startsWith("F")) {
                        //if (item.SupplierInvoiceFreightAmounts.length > 0 && !AppTool.IsNullOrEmpty(item.InsuranceAmount)) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.InsuranceAmount)) {
                            checkFreightValues = false;
                        }
                    }
                    else if (item.IncotermCode == "CPT" || item.IncotermCode == "CFR") {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.InsuranceAmount)) {
                            checkFreightValues = false;
                        }
                    }
                }
            }
        }
        if (valid) {
            if (checkFreightValues) {
                this.SaveAndNew = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IncotermCode) && (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F"))) {
                    if ((this.EntityPM.SupplierInvoiceFreightAmounts.length == 0 && !this.declarationPM.InvoiceHasFreight) || ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null))) {
                        this.CurrentSession.StopBusyIndicator();
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                        confirmWindow.Show(msg);
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.ConfirmWindowYesButton();
                            }
                        });
                        this.closeWindow = false;
                    }
                    else {
                        this.closeWindow = false;
                        //this.SaveChanges();
                        //this._IsInitiateNewInstance = true;
                        //this.InitiateNewInstance();
                        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
                        this.SaveChangesSync();
                    }
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IncotermCode) && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
                    if ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null)) {
                        this.CurrentSession.StopBusyIndicator();
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                        confirmWindow.Show(msg);
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.ConfirmWindowYesButton();
                            }
                        });
                        this.closeWindow = false;
                    }
                    else {
                        this.closeWindow = false;
                        //this.SaveChanges();
                        //this._IsInitiateNewInstance = true;
                        //this.InitiateNewInstance();
                        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
                        this.SaveChangesSync();
                    }
                }
                else {
                    this.closeWindow = false;
                    //this.SaveChanges()//;
                    //    .then(
                    //    (a) => {
                    //        this._IsInitiateNewInstance = true;
                    //        this.InitiateNewInstance();
                    //    });
                    this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
                    this.SaveChangesSync();
                }
            }
            else {
                this.closeWindow = false;
                //this.SaveChanges();
                //this._IsInitiateNewInstance = true;
                //this.InitiateNewInstance();
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
                this.SaveChangesSync();
            }
        }
    };
    AddEditSupplierInvoiceComponent.prototype.SaveButtonClicked = function () {
        // return;
        var errors = this.ValidateModifications();
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            return;
        }
        this.CurrentSession.StartBusyIndicatorSaving();
        //if (InvoiceModificationsObslist.Count > 0) {
        //    bool exist = (from a in InvoiceModificationsObslist
        //    where !a.isValid
        //    select a).Any();
        //    if (exist) {
        //        return;
        //    }
        //}
        var valid = this.PreSaveAndNewValidate();
        var checkFreightValues = true;
        if (this.declarationPM.SupplierInvoices.length > 1) {
            for (var _i = 0, _a = this.declarationPM.SupplierInvoices; _i < _a.length; _i++) {
                var item = _a[_i];
                if (item.IncotermCode != null && (item.IncotermCode.startsWith("E") || item.IncotermCode.startsWith("F"))) {
                    //if (item.SupplierInvoiceFreightAmounts.length > 0 && item.InsuranceAmount != null) {
                    if (item.InsuranceAmount != null) {
                        checkFreightValues = false;
                        break;
                    }
                }
                else if (item.IncotermCode != null && (item.IncotermCode == "CPT" || item.IncotermCode == "CFR")) {
                    if (item.InsuranceAmount != null) {
                        checkFreightValues = false;
                        break;
                    }
                }
            }
        }
        if (valid) {
            this.ContinueSaving(checkFreightValues); // if not use old calclate commestion code
            //if (this.calculateCommission) {
            //    this.VendorCommissionPMService.get(this.EntityPM.VendorId, this.declarationPM.CustomerId).subscribe(myResult => {
            //        if (myResult) {
            //            if (!myResult.HasError) {
            //                if (myResult.Result) {
            //                    this.EntityPM.VendorComissionPercentage = myResult.Result.CommisionPercentage;
            //                    if (this.EntityPM.SupplierInvoiceModifications.length > 0) {
            //                        for (let item of this.EntityPM.SupplierInvoiceModifications) {
            //                            if (item.CurrencyTypeCode == this.EntityPM.InvoiceCurrencyTypeCode && item.TypeCode == "I10" && item.Amount == (this.EntityPM.InvoiceAmount * this.EntityPM.VendorComissionPercentage)) {
            //                                this.ContinueSaving(checkFreightValues);
            //                            }
            //                            else if (item.CurrencyTypeCode != this.EntityPM.InvoiceCurrencyTypeCode || item.Amount != (this.EntityPM.InvoiceAmount * this.EntityPM.VendorComissionPercentage)) {
            //                                this.CurrentSession.StopBusyIndicator();
            //                                var confirm = new ConfirmWindow;
            //                                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            //                                confirm.ShowNoButton = true;
            //                                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.UpdateCommision"));
            //                                confirm.WindowClosed.subscribe((event: any) => {
            //                                    if (confirm.Yes) {
            //                                        item.CurrencyTypeCode = this.EntityPM.InvoiceCurrencyTypeCode;
            //                                        item.Amount = (this.EntityPM.VendorComissionPercentage / 100) * this.EntityPM.InvoiceAmount;
            //                                        this.ContinueSaving(checkFreightValues);
            //                                        confirm.Close();
            //                                    }
            //                                    else {
            //                                        this.ContinueSaving(checkFreightValues);
            //                                        confirm.Close();
            //                                    }
            //                                });
            //                            }
            //                        }
            //                    }
            //                    else {
            //                        if (myResult.Result.CommisionPercentage && myResult.Result.CommisionPercentage > 0) {
            //                            var supplierInvoiceModificationPM = new SupplierInvoiceModificationPM(this.EntityPM);
            //                            supplierInvoiceModificationPM.DeclarationId = this.EntityPM.DeclarationId;
            //                            supplierInvoiceModificationPM.Tenant = this.EntityPM.Tenant;
            //                            supplierInvoiceModificationPM.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
            //                            supplierInvoiceModificationPM.Amount = this.EntityPM.InvoiceAmount * (this.EntityPM.VendorComissionPercentage / 100);
            //                            supplierInvoiceModificationPM.CurrencyTypeCode = this.EntityPM.InvoiceCurrencyTypeCode;
            //                            supplierInvoiceModificationPM.TypeCode = "I10";
            //                            this.EntityPM.SupplierInvoiceModifications.push(supplierInvoiceModificationPM);
            //                        }
            //                        this.ContinueSaving(checkFreightValues);
            //                    }
            //                }
            //                else {
            //                    this.ContinueSaving(checkFreightValues);
            //                }
            //            }
            //            else {
            //                this.ValidationErrorsList = myResult.ErrorsArray;
            //                this.CurrentSession.StopBusyIndicator();
            //            }
            //        }
            //        else {
            //            this.ContinueSaving(checkFreightValues);
            //        }
            //    });
            //}
            //else {
            //    this.ContinueSaving(checkFreightValues);
            //}
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    AddEditSupplierInvoiceComponent.prototype.ContinueSaving = function (checkFreightValues) {
        var _this = this;
        if (checkFreightValues /*&& !this.loadingNextItems*/) { // maybe this should be done because i'm saving the invoice in case next and previous.
            if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F"))) {
                if ((this.EntityPM.SupplierInvoiceFreightAmounts.length == 0 && !this.declarationPM.InvoiceHasFreight) || ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null))) {
                    this.CurrentSession.StopBusyIndicator();
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                    confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.ConfirmWindowYesButton();
                        }
                    });
                    this.closeWindow = false;
                }
                else {
                    this.closeWindow = true;
                    //this.SaveChanges();
                    this.SaveChangesSync();
                }
            }
            else if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
                if ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null)) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.StopBusyIndicator();
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                    confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.ConfirmWindowYesButton();
                        }
                    });
                    this.closeWindow = false;
                }
                else {
                    if (!this.loadingNextItems) {
                        this.closeWindow = true;
                    }
                    else {
                        this.closeWindow = true;
                    }
                    //this.SaveChanges();
                    this.SaveChangesSync();
                }
            }
            else {
                if (this.SendMode) {
                    this.closeWindow = false;
                }
                else if (!this.loadingNextItems) {
                    this.closeWindow = true;
                }
                //this.SaveChanges();
                this.SaveChangesSync();
            }
        }
        else {
            if (this.SendMode) {
                this.closeWindow = false;
            }
            else if (!this.loadingNextItems) {
                this.closeWindow = true;
            }
            //this.SaveChanges();
            this.SaveChangesSync();
        }
    };
    //ValidateModifications() {
    //    if (this.EntityPM.SupplierInvoiceModifications) {
    //        var validationErrors = [];
    //        this.EntityPM.SupplierInvoiceModifications.forEach((mod) => {
    //            var typeCode = mod.TypeCode;
    //            if (typeCode == "I02") {
    //                //     validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee"));
    //            } else {
    //                var exists = [];
    //                if (this.EntityPM.SupplierInvoiceModifications.length != 0) {
    //                    exists = this.EntityPM.SupplierInvoiceModifications.filter(d => d.TypeCode == typeCode);
    //                }
    //                if (exists.length > 1) {
    //                    validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
    //                }
    //            }
    //        });
    //        return validationErrors;
    //    }
    //}
    AddEditSupplierInvoiceComponent.prototype.SaveChanges = function () {
        //var errors = [];
        var _this = this;
        //if (errors.length == 0) {
        if (this.IsNewEntity) {
            this.supplierInvoicePMService.insert(this.EntityPM).subscribe(function (myResult) {
                var res = myResult;
                if (!res.HasError) {
                    _this.entity = res.Result;
                    console.log("..Saved Successfully ", _this.entity);
                    return true;
                }
                else {
                    _this.ValidationErrorsList = res.ErrorsArray;
                }
                _this.CurrentSession.StopBusyIndicator();
                return false;
            });
        }
        else {
            this.supplierInvoicePMService.update(this.EntityPM).subscribe(function (myResult) {
                var res = myResult;
                if (!res.HasError) {
                    _this.entity = res.Result;
                    console.log("..Saved Successfully ", _this.entity);
                    return true;
                }
                else {
                    _this.ValidationErrorsList = res.ErrorsArray;
                }
                _this.CurrentSession.StopBusyIndicator();
                return false;
            });
        }
        //}
    };
    AddEditSupplierInvoiceComponent.prototype.ReloadPromise = function () {
        var _this = this;
        return new Promise(function (resolve) {
            if (_this.loadingNextItems || _this.NextPreviousVisible) {
                resolve(true);
            }
            else {
                _this.supplierInvoicePMService
                    .get(_this.EntityPM.DeclarationId, _this.EntityPM.InvoiceCounterKey)
                    .subscribe(function (myResult) {
                    var res = myResult;
                    var entity = res.Result;
                    _this.EntityPM = entity; // now we have the Sequence from server (and can update )
                    resolve(true);
                });
            }
        });
    };
    AddEditSupplierInvoiceComponent.prototype.Reload1stSIPromise = function () {
        var _this = this;
        return new Promise(function (resolve) {
            if (_this.loadingNextItems || _this.NextPreviousVisible) {
                resolve(true);
            }
            else {
                ///reload the 1st all the time !! <<<<<
                var siFirst = _this.Get1SupplierInvoice();
                if (siFirst.InvoiceCounterKey == _this.EntityPM.InvoiceCounterKey) {
                    resolve(true);
                }
                else {
                    _this.supplierInvoicePMService
                        .get(siFirst.DeclarationId, siFirst.InvoiceCounterKey)
                        .subscribe(function (my1stResult) {
                        var res1 = my1stResult;
                        _this.declarationPM.SupplierInvoices[0] = _this.supplierInvoicePMService.MapJsonToEntityPM(res1.Result);
                        //this.EntityPM = this.declarationPM.SupplierInvoices[0];// now we have the Sequence from server (and can update )
                        resolve(true);
                    });
                }
            }
        });
    };
    AddEditSupplierInvoiceComponent.prototype.SavingPromise = function (isChromeMode) {
        var _this = this;
        return new Promise(function (resolve) {
            if (_this.IsNewEntity) {
                _this.supplierInvoicePMService.insert(_this.EntityPM).subscribe(function (myResult) {
                    var res = myResult;
                    if (!res.HasError) {
                        _this.entity = res.Result;
                        _this.EntityPM = _this.entity; // now we have the Sequence from server (and can update )
                        console.log("..Saved Successfully ", _this.entity);
                        resolve(true);
                        if (isChromeMode) {
                            if (_this.closeWindow) {
                                _this.CurrentSession.CloseCurrentWindow();
                            }
                            else if (_this._IsInitiateNewInstance) {
                                if (_this.copyInvoiceWithItem && _this.entity.FullItemsCount > 500) {
                                    if (_this.OldEntityPM.FullItemsCount > 500) {
                                        var msg = new MessageWindow_1.MessageWindow();
                                        msg.Show("ההצהרה מכילה יותר מ-500 פריטים, לא ניתן להעתיק אותה");
                                        _this.NextPreviousVisible = true;
                                    }
                                }
                                else {
                                    _this.GENERAL.Dispose();
                                    _this.InitiateNewInstance();
                                    _this._IsInitiateNewInstance = _this.closeWindow = false;
                                }
                                //return Promise.reject(new Error('Finish InitiateNewInstance'));
                            }
                            else if (_this.loadingNextItems) {
                                _this.GENERAL.Dispose();
                                _this.ReloadSupplierInvoiceWithItems(_this.skipedItems, _this.takenItems);
                            }
                        }
                    }
                    else {
                        _this.ValidationErrorsList = res.ErrorsArray;
                    }
                    _this.CurrentSession.StopBusyIndicator();
                    resolve(false);
                });
            }
            else {
                _this.supplierInvoicePMService.update(_this.EntityPM).subscribe(function (myResult) {
                    var res = myResult;
                    if (!res.HasError) {
                        _this.entity = res.Result;
                        _this.CurrentSession.StopBusyIndicator();
                        console.log("..Saved Successfully ", _this.entity);
                        resolve(true);
                        if (isChromeMode) {
                            if (_this.closeWindow) {
                                _this.CurrentSession.CloseCurrentWindow();
                            }
                            else if (_this._IsInitiateNewInstance) {
                                if (_this.copyInvoiceWithItem && _this.entity.FullItemsCount > 500) {
                                    if (_this.OldEntityPM.FullItemsCount > 500) {
                                        var msg = new MessageWindow_1.MessageWindow();
                                        msg.Show("ההצהרה מכילה יותר מ-500 פריטים, לא ניתן להעתיק אותה");
                                        _this.NextPreviousVisible = true;
                                    }
                                }
                                else {
                                    _this.GENERAL.Dispose();
                                    _this.InitiateNewInstance();
                                    _this._IsInitiateNewInstance = _this.closeWindow = false;
                                }
                            }
                            else if (_this.loadingNextItems) {
                                _this.GENERAL.Dispose();
                                _this.ReloadSupplierInvoiceWithItems(_this.skipedItems, _this.takenItems);
                            }
                        }
                    }
                    else {
                        _this.ValidationErrorsList = res.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    resolve(false);
                });
            }
        });
    };
    AddEditSupplierInvoiceComponent.prototype.Get1SupplierInvoice = function () {
        //let supplierInvoice = this.EntityPM;
        var supplierInvoice = null;
        if (this.declarationPM.SupplierInvoices && this.declarationPM.SupplierInvoices.length > 0) {
            supplierInvoice = this.declarationPM.SupplierInvoices[0];
        }
        else {
            if (this._My1stSupplierInvoicePM != null) {
                supplierInvoice = this._My1stSupplierInvoicePM;
            }
            else {
                supplierInvoice = this.EntityPM;
                this._My1stSupplierInvoicePM = supplierInvoice;
            }
        }
        //itzik : the this.declarationPM.SupplierInvoices[0] is not the same reference so old will be data update 
        if ( //supplierInvoice.DeclarationId == this.EntityPM.DeclarationId
        supplierInvoice.InvoiceCounterKey == this.EntityPM.InvoiceCounterKey &&
            supplierInvoice.DeclarationId == this.EntityPM.DeclarationId) {
            supplierInvoice = this.EntityPM;
        }
        return supplierInvoice;
    };
    AddEditSupplierInvoiceComponent.prototype.SaveChangesSync = function () {
        //this.supplierInvoiceExtendedPMService.PutSupplierInvoicePercentage(this.EntityPM).subscribe(myResult => {
        var _this = this;
        //    if (myResult) {
        //        if (!myResult.HasError) {
        //        }
        //    }
        //});
        this.ValidationErrorsList = [];
        this._FinishPromiseDoWhatPlannedDone = false;
        this._LastUnifreightMessageM = null;
        if (!this.EntityPM.IsDirty && !this.ForceSave) {
            this.FinishPromiseDoWhatPlanned(true);
            return;
        }
        //this.SaveItemCodeLocalCache();
        GITITEMCacheService_1.GITITEMCacheService.Instance.SaveItemCodeLocalCache();
        if (!AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            if (this.EntityPM.IsDirty || this.ForceSave) {
                return this.SavingPromise(true);
            }
            else {
                this.FinishPromiseDoWhatPlanned(true);
            }
        }
        Promise.resolve("lets go")
            .then(function (res) {
            ///Just Saving  .....
            _this.LogMe("this.SavingPromise();");
            if (_this.EntityPM.IsDirty || _this.ForceSave) {
                return _this.SavingPromise(false);
            }
            else {
                _this.FinishPromiseDoWhatPlanned(true);
            }
        })
            .then(function (saveWithoutError) {
            if (_this.ValidationErrorsList.length > 0) {
                //return Promise.reject(new Error('bad Save '));
                throw new Error('bad Save ');
            }
            var goToInsuranceInUNF = false;
            if (!_this._SkipAutoInsurance) {
                if (_this.declarationPM.IsConnectedToUnifreight) {
                    if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(_this.declarationPM.CustomFileNo, _this.declarationPM.IsConvertedDeclaration, _this.declarationPM.IsConnectedToUnifreight)) { //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && !AppTool.IsNullOrEmpty(this.declarationPM.CustomFileNo)) {
                        goToInsuranceInUNF = true;
                    }
                }
            }
            if (!goToInsuranceInUNF) {
                _this.LogMe("!goToInsuranceInUNF");
                _this.FinishPromiseDoWhatPlanned();
                //return Promise.reject(new Error('Finish '));
                throw new Error('Finish');
            }
            return "Go Onn";
        })
            .then(function (goON) {
            _this.LogMe("ReloadPromise()");
            return _this.ReloadPromise();
        })
            .then(function (goON) {
            _this.LogMe("ReloadPromise()");
            return _this.Reload1stSIPromise();
        })
            .then(function (goON) {
            if (_this.loadingNextItems) {
                throw new Error('Next\prev =>Finish (Check Insurance only on save)');
            }
            _this.LogMe("goToInsuranceInUNF");
            _this.CurrentSession.StartBusyIndicator("Check Insurance ...");
            var toPromise = true;
            return _this.SendUnifaceRequestAndWaitPromise();
        })
            .then(function (res) {
            return _this.UnifreightInsuranceCallbackAction(_this._LastUnifreightMessageM);
        })
            //.catch(getInsuranceError => {
            //})
            .then(function (saveInsurance) {
            return new Promise(function (resolve, reject) {
                if (saveInsurance == "Save1stSupplierInvoice") {
                    var supplierInvoice = _this.Get1SupplierInvoice(); //this.declarationPM.SupplierInvoices[0];
                    ////itzik : the this.declarationPM.SupplierInvoices[0] is not the same reference so old will be data update 
                    //if (//supplierInvoice.DeclarationId == this.EntityPM.DeclarationId
                    //    supplierInvoice.InvoiceCounterKey == this.EntityPM.InvoiceCounterKey &&
                    //    supplierInvoice.DeclarationId == this.EntityPM.DeclarationId) {
                    //    supplierInvoice = this.EntityPM;
                    //}
                    _this.supplierInvoicePMService.update(supplierInvoice).subscribe(function (myResult) {
                        var res = myResult;
                        if (!res.HasError) {
                            _this.entity = res.Result;
                            console.log("..Saved Successfully ", _this.entity);
                            //resolveInsuranceCallback("ok")
                            _this.CurrentSession.StopBusyIndicator();
                            resolve(true);
                        }
                        else {
                            _this.ValidationErrorsList = res.ErrorsArray;
                            _this.CurrentSession.StopBusyIndicator();
                            console.warn("Error Saving UnifreightInsurance");
                            resolve(false);
                        }
                    });
                }
                else {
                    _this.FinishPromiseDoWhatPlanned();
                    //throw new Error("go to end");
                }
            });
        })
            //.catch(saveInsuranceError => {
            //})
            .then(function (InsuranceSaved) {
            if (!InsuranceSaved) {
                _this.closeWindow = false;
                _this._IsInitiateNewInstance = false;
                var closeWidowsWhenSaveInsuranceFailed = true;
                if (closeWidowsWhenSaveInsuranceFailed) {
                    _this.closeWindow = true;
                    _this.loadingNextItems = false;
                    console.warn("Due Error in Saving UnifreightInsurance ...closeWindow");
                    _this._FinishPromiseDoWhatPlannedDone = false; //i didnt finsh  - pls do !!
                    _this.FinishPromiseDoWhatPlanned();
                }
            }
            else {
                _this.FinishPromiseDoWhatPlanned();
            }
        })
            //.catch(saveInsuranceError => {
            //})
            .catch(function (finish) {
            _this.FinishPromiseDoWhatPlanned(true);
            _this.LogMe("catch(finish !!");
            _this.CurrentSession.StopBusyIndicator();
            _this._IsInitiateNewInstance = _this.closeWindow = false;
        });
    };
    AddEditSupplierInvoiceComponent.prototype.FinishPromiseDoWhatPlanned = function (inCatchBlock) {
        if (this.ValidationErrorsList && this.ValidationErrorsList.length > 0)
            return; //Show Err Validation !!
        if (this._FinishPromiseDoWhatPlannedDone)
            return;
        this._FinishPromiseDoWhatPlannedDone = true;
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.LogMe("FinishPromiseDoWhatPlanned");
        if (this.closeWindow) {
            this.GENERAL.Dispose();
            this.CurrentSession.CloseCurrentWindow();
            this._IsInitiateNewInstance = this.closeWindow = false;
            //return Promise.reject(new Error('Finish CloseCurrentWindow'));
            if (inCatchBlock != true) {
                throw new Error('Finish CloseCurrentWindow');
            }
        }
        if (this._IsInitiateNewInstance) {
            if (this.copyInvoiceWithItem && this.OldEntityPM.FullItemsCount > 500) {
                if (this.entity.FullItemsCount > 500) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Show("ההצהרה מכילה יותר מ-500 פריטים, לא ניתן להעתיק אותה");
                    this.NextPreviousVisible = true;
                }
            }
            else {
                this.GENERAL.Dispose();
                this.InitiateNewInstance();
                this._IsInitiateNewInstance = this.closeWindow = false;
                //return Promise.reject(new Error('Finish InitiateNewInstance'));
                if (inCatchBlock != true) {
                    throw new Error('Finish InitiateNewInstance');
                }
            }
        }
        if (this.loadingNextItems) {
            this.GENERAL.Dispose();
            this.ReloadSupplierInvoiceWithItems(this.skipedItems, this.takenItems);
        }
    };
    AddEditSupplierInvoiceComponent.prototype.SendUnifaceRequestAndWaitPromise = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(function (mess) {
                var IsMatchUnifreightCallbackCommand = (mess.UnifreightEntityNumber == _this.declarationPM.CustomFileNo &&
                    mess.LogitudeViewModel == "AddEditSupplierInvoiceComponent");
                if (IsMatchUnifreightCallbackCommand) {
                    sub.unsubscribe();
                    _this.LogMe("UnifaceRequestArrived");
                    _this._LastUnifreightMessageM = mess;
                    resolve("Arrived mess from unifrieght");
                    //Promise.resolve("Arrived mess from unifrieght")
                }
            });
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseCheckInsuranseReturnIsNeededAmount(_this.declarationPM.CustomFileNo, _this.declarationPM.Id + _this.EntityPM.InvoiceCounterKey.toString(), "AddEditSupplierInvoiceComponent", "OPEN");
        });
    };
    AddEditSupplierInvoiceComponent.prototype.PreSaveAndNewValidate = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);
        // FreightAmounts
        for (var _i = 0, _a = this.EntityPM.SupplierInvoiceFreightAmounts; _i < _a.length; _i++) {
            var frAmount = _a[_i];
            Validator_1.Validator.TryValidateObject(frAmount, "Customs.SupplierInvoiceFreightAmount", errors);
            if (!Tools_1.AppTool.IsNullOrEmpty(frAmount.CurrencyTypeCode) && Tools_1.AppTool.IsNullOrEmpty(frAmount.Amount)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceFreightAmount.F.Amount"));
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(frAmount.CurrencyTypeCode) && !Tools_1.AppTool.IsNullOrEmpty(frAmount.Amount)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceFreightAmount.F.CurrencyTypeCode"));
            }
        }
        // SupplierInvoiceItem
        for (var _b = 0, _c = this.EntityPM.SupplierInvoiceItems; _b < _c.length; _b++) {
            var item = _c[_b];
            Validator_1.Validator.TryValidateObject(item, "Customs.SupplierInvoiceItem", errors);
            //Validator.TryValidateObject(item, new ValidationContext(item, null, null), errors);
            for (var _d = 0, _e = item.SupplierInvoiceItemsMods; _d < _e.length; _d++) {
                var itemModification = _e[_d];
                itemModification.ChangeSetOp = "None";
            }
        }
        // IncotermCode
        //if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP")) {
        //    if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
        //        for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
        //            this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
        //        }
        //    }
        //}
        //else if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
        //    if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
        //        for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
        //            this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
        //        }
        //    }
        //}
        // InsruanceCurrencyTypeCode
        if (this.EntityPM.InsruanceCurrencyTypeCode != null && this.EntityPM.InsuranceAmount == null) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoice.F.InsuranceAmount"));
        }
        else if (this.EntityPM.InsruanceCurrencyTypeCode == null && this.EntityPM.InsuranceAmount != null) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoice.F.InsruanceCurrencyTypeCode"));
        }
        // SupplierInvoiceModifications
        for (var _f = 0, _g = this.EntityPM.SupplierInvoiceModifications; _f < _g.length; _f++) {
            var item = _g[_f];
            //Validator.TryValidateObject(item, new ValidationContext(item, null, null), errors);
            if (Tools_1.AppTool.IsNullOrEmpty(item.CurrencyTypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.CurrencyTypeCode") + " - מסך נוספים");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.TypeCode") + " - מסך נוספים");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.Amount)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.Amount") + " - מסך נוספים");
            }
        }
        if (errors.length == 0) {
            //if (isNewEntity) {
            //    //supplier invoice is removed from composition. mohammad
            //    if (!context.SupplierInvoicePMs.Contains(entityPM)) {
            //        context.SupplierInvoicePMs.Add(entityPM);
            //    }
            //}
            for (var _h = 0, _j = this.EntityPM.SupplierInvoiceModifications; _h < _j.length; _h++) {
                var item = _j[_h];
                item.ChangeSetOp = "None";
            }
            return true;
        }
        else {
            this.ValidationErrorsList = errors;
            return false;
        }
    };
    AddEditSupplierInvoiceComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    AddEditSupplierInvoiceComponent.prototype.UnifreightInsuranceCallbackAction = function (unifreightMessageM) {
        var _this = this;
        this.LogMe("UnifreightInsuranceCallbackAction");
        return new Promise(function (resolveInsuranceCallback, reject) {
            _this.LogMe("UnifreightInsuranceCallbackAction Promise ");
            var sAction = "";
            sAction = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.Action");
            var sInsuranseIsNeeded = "";
            sInsuranseIsNeeded = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsNeeded");
            var sInsuranseIsSucceeded = "";
            sInsuranseIsSucceeded = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsSucceeded");
            var sInsuranceHasOpen = "";
            sInsuranceHasOpen = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseHasOpen");
            var sInsuranceMessage = "";
            sInsuranceMessage = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranceMessage");
            var sInsuranseAmount = "";
            sInsuranseAmount = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseAmount");
            var sInsuranseCurrency = "";
            sInsuranseCurrency = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseCurrency");
            var sExpensesAmount = "";
            sExpensesAmount = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmount");
            var sExpensesAmountCurr = "";
            sExpensesAmountCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmountCurr");
            var sFreightAmount = "";
            sFreightAmount = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount");
            var sFreightAmountCurr = "";
            sFreightAmountCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr");
            //Task 40723:
            var sFreightAmount2 = "";
            sFreightAmount2 = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount2");
            var sFreightAmountCurr2 = "";
            sFreightAmountCurr2 = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr2");
            var sTotalFreightInFreightCurr = "";
            sTotalFreightInFreightCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.TotalFreightInFreightCurr");
            var sForceMessage = "";
            sFreightAmountCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ForceMessage");
            switch (sAction) {
                case "OPEN":
                    {
                        if (_this.declarationPM.SupplierInvoices != null
                        //&& this.declarationPM.SupplierInvoices.length > 0
                        ) {
                            var supplierInvoice = null;
                            supplierInvoice = _this.Get1SupplierInvoice(); //this.declarationPM.SupplierInvoices[0];
                            var execAsService = true;
                            if (execAsService) {
                                var service = new AnalyzeUnifreightInsuranceService();
                                service.Open(unifreightMessageM, _this.declarationPM, supplierInvoice);
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(sForceMessage) ||
                                (!Tools_1.AppTool.IsNullOrEmpty(sInsuranceMessage) && !_this.declarationPM.IsChanged)) {
                                if (sInsuranseIsNeeded.toString().toLowerCase() == "true" &&
                                    sInsuranseIsSucceeded === "false") {
                                    /*this.CurrentSession.StopBusyIndicator(); */ _this._IsInitiateNewInstance = _this.closeWindow = false;
                                    _this.closeWindow = true;
                                    ///setTimeout(() => {
                                    _this.CurrentSession.StopBusyIndicator();
                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                    messageWindow.Width = 400;
                                    messageWindow.Height = 150;
                                    messageWindow.Title = "";
                                    messageWindow.Show(sInsuranceMessage);
                                    //}, 500);
                                }
                                else {
                                    console.log(sInsuranceMessage);
                                }
                                resolveInsuranceCallback("Show Error Message - Stop");
                            }
                            if (_this.declarationPM.IsChanged == true) {
                                resolveInsuranceCallback("Save1stSupplierInvoice");
                            }
                            else {
                                _this.CurrentSession.StopBusyIndicator();
                                resolveInsuranceCallback("nothing done ");
                            }
                        }
                        else {
                            resolveInsuranceCallback("nothing done ");
                        }
                    }
                    break;
                case "CHECK":
                    {
                        if (sInsuranseIsNeeded == "true") {
                            //VisibilityInsurance = "Visible";
                        }
                        else {
                            //VisibilityInsurance = "Collapsed";
                        }
                        resolveInsuranceCallback("nothing done ");
                        break;
                    }
                default:
                    {
                        resolveInsuranceCallback("nothing done ");
                        break;
                    }
            }
        });
    };
    AddEditSupplierInvoiceComponent.prototype.CopyInvoiceClicked = function () {
        this.copyInvoice = true;
        this.SaveAndNewButtonClicked();
        this.DropdownDisplayClose();
    };
    AddEditSupplierInvoiceComponent.prototype.CopyInvoiceWithItemsClicked = function () {
        this.copyInvoice = true;
        this.copyInvoiceWithItem = true;
        this.SaveAndNewButtonClicked();
        this.DropdownDisplayClose();
    };
    AddEditSupplierInvoiceComponent.prototype.InitiateNewInstance = function () {
        var _this = this;
        this.ForceSave = true;
        var itemPM = new SupplierInvoicePM_1.SupplierInvoicePM();
        this.TextValue = null;
        if (this.EntityPM) {
            if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
                this.declarationPM.InvoiceHasFreight = true;
            }
        }
        if (this.copyInvoice) {
            itemPM.VendorId = this.EntityPM.VendorId;
            itemPM.IssueCountryCode = this.EntityPM.IssueCountryCode;
            itemPM.AccountTypeCode = this.EntityPM.AccountTypeCode;
            itemPM.IssueDate = this.EntityPM.IssueDate;
            itemPM.InvoiceCurrencyTypeCode = this.EntityPM.InvoiceCurrencyTypeCode;
            itemPM.IncotermCode = this.EntityPM.IncotermCode;
            itemPM.InvoiceAmount = null;
            itemPM.InsruancePercentage = null;
            itemPM.InsuranceAmount = null;
            itemPM.InsruanceCurrencyTypeCode = null;
        }
        itemPM.DeclarationId = this.declarationPM.Id;
        itemPM.IsValueForCustomsOnly = this.EntityPM.IsValueForCustomsOnly; //this.declarationPM.IsValueForCustomsOnly;
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.InvoiceCounterKey = 0;
        itemPM.SequenceNumeric = 0;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        this.NextPreviousVisible = false;
        this.Difference = 0;
        this.TotalForeignCurrency = 0;
        this.accumulationFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCUMULATION") && f.ObjectTableId == table.Id; })[0];
        if (this.accumulationFeature == null) {
            itemPM.AccumalationStateCode = "3";
        }
        else {
            itemPM.AccumalationStateCode = "1";
        }
        itemPM.IsAccumalated = false;
        this.TotalForeignCurrency = 0;
        this.IsNewEntity = true;
        var counterKey = this.EntityPM.InvoiceCounterKey;
        this.EntityPM = itemPM;
        this.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.NewInvoice");
        if (this.copyInvoiceWithItem) {
            this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.DeclarationId, counterKey, 0, this.NumberOfLoadedItems, "child").subscribe(function (response) {
                if (!response.HasError) {
                    _this.OldEntityPM = response.Result;
                    var supplierinvoiceitems = [];
                    var line = 1;
                    var sequence = 1;
                    // itemPM.SupplierInvoiceItems = this.OldEntityPM.SupplierInvoiceItems;
                    for (var _i = 0, _a = _this.OldEntityPM.SupplierInvoiceItems; _i < _a.length; _i++) {
                        var item = _a[_i];
                        var newItem = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(itemPM);
                        newItem.ClassificationCode = item.ClassificationCode;
                        newItem.TradeAgreementCode = item.TradeAgreementCode;
                        newItem.InvoiceQuantityType = item.InvoiceQuantityType;
                        newItem.OriginCountryCode = item.OriginCountryCode;
                        newItem.OriginCountryName = item.OriginCountryName;
                        newItem.TradeAgreementName = item.TradeAgreementName;
                        newItem.ChangeSetOp = "Insert";
                        newItem.DeclarationId = item.DeclarationId;
                        newItem.LineNumber = line;
                        newItem.CounterKey = itemPM.InvoiceCounterKey;
                        newItem.SequenceNumeric = sequence;
                        newItem.Tenant = _this.OldEntityPM.Tenant;
                        _this.EntityPM.AddSupplierInvoiceItem(newItem);
                        line += 1;
                        sequence += 1;
                    }
                    _this.GENERAL.InitTab(_this.EntityPM, _this, _this.IsDisplayOnly, false, true);
                }
                else {
                    //this.ValidationErrorsList = response.ErrorArray;
                }
            });
        }
        else {
            this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, false, true);
        }
        //select general tab
        this.SelectedTabCode = "GENERAL";
        this.copyInvoice = false;
        this.copyInvoiceWithItem = false;
        //distroy old more tab content
        if (this.MORE)
            this.DistroyMoreTab();
        // this.cd.detectChanges();
        //newViewModel.isNewEntity = true;
        //if (this.OnInitiateNewInvoice != null) {
        //    this.OnInitiateNewInvoice(newViewModel, new EventArgs());
        //}
        //newViewModel.SetEntityPM(itemPM, true, this._IsEnabled, this._ViewDisableMessage);
        //newViewModel.selectedIndex = 0;
        //newViewModel.OnSelectedIndexChanged();
    };
    AddEditSupplierInvoiceComponent.prototype.ConfirmWindowYesButton = function () {
        if (this.SaveAndNew || this.loadingNextItems) {
            this.closeWindow = false;
            //this.SaveChanges()//;
            //    .then(
            //    (a) => {
            //        this.InitiateNewInstance();
            //    });
            this.SaveChangesSync();
        }
        else {
            if (this.SendMode) {
                this.closeWindow = false;
            }
            else {
                this.closeWindow = true;
            }
            //this.SaveChanges().then(res => { this.CloseWindowAfterSaveIfNeeded() });
            this.SaveChangesSync();
        }
    };
    //SaveItemCodeLocalCache() {
    //    if (this.ItemCode_LocalCache != null && this.ItemCode_LocalCache.length > 0) {
    //        for (let item of this.ItemCode_LocalCache) {
    //            if (item.IsNew) {
    //                var myGITITEMPM = new GITITEMDto();
    //                myGITITEMPM.PARTNERID = this.declarationPM.CustomerCode;
    //                if (AppTool.IsNullOrEmpty(item.VendorNumber)) {
    //                    item.VendorNumber = "NULL";
    //                }
    //                myGITITEMPM.SAPAKID = item.VendorNumber;
    //                myGITITEMPM.ITEMNO = item.ItemCode;
    //                myGITITEMPM.PRATID = item.ClassificationCode;
    //                myGITITEMPM.NAMEENG = item.ItemDescription;
    //                myGITITEMPM.ORIGINCOUNTRY = item.OriginCountryCode;
    //                myGITITEMPM.UNITID = item.InvoiceQuantityType;
    //                this.GITITEMExtendedPMService.insert(myGITITEMPM).subscribe(myResult => {
    //                    var mm: ServiceResponse = myResult;
    //                    if (!mm.HasError) {
    //                        this.entity = mm.Result;
    //                    }
    //                });
    //            }
    //        }
    //    }
    //}
    //#endregion
    AddEditSupplierInvoiceComponent.prototype.DistroyMoreTab = function () {
        var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == "MORE"; })[0];
        myLocation.viewContainerRef.clear();
        this.MORE = null;
    };
    AddEditSupplierInvoiceComponent.prototype.ShowXMLErrors = function (error) {
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
        }
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors = error.Description.split(/,|:/);
            for (var _i = 0, xmlErrors_1 = xmlErrors; _i < xmlErrors_1.length; _i++) {
                var xmlError = xmlErrors_1[_i];
                errors.push(xmlError);
            }
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
                //if (OnShowXMLErrors != null) {
                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
                //}
            }
        }
    };
    AddEditSupplierInvoiceComponent.prototype.ShowXMLCorrections = function (amendment) {
        if (amendment.EntityName.toLowerCase() == "supplierinvoiceitem") {
            //if (OnShowXMLErrors != null) {
            //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { 
            //        SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == amendment.Line).FirstOrDefault(),
            //    });
            //}
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(amendment.Field)) {
                this.UIProperties.SetValidity(amendment.Field, "Customs.SupplierInvoice", false, amendment.ErrorType);
            }
            var errors = [];
            errors.push(amendment.ErrorType);
            this.ValidationErrorsList = errors;
        }
    };
    AddEditSupplierInvoiceComponent.prototype.CloseWindowAfterSaveIfNeeded = function () {
        if (this.closeWindow === true) {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditSupplierInvoiceComponent.prototype.LogMe = function (mess) {
        var alertIt = false;
        if (alertIt) {
            alert(mess);
        }
        else {
            console.log(mess);
        }
    };
    AddEditSupplierInvoiceComponent.prototype.Previous = function () {
        var fullCount = 0;
        if (this.AccumulatedFilter == "parent") {
            fullCount = this.EntityPM.FullParentsCount;
        }
        else if (this.AccumulatedFilter == "child") {
            fullCount = this.EntityPM.FullChildrenCount;
        }
        else {
            fullCount = this.EntityPM.FullItemsCount;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
        var Z;
        if (this.FirstCurrentLine == null) {
            this.FirstCurrentLine = 1;
        }
        if (this.FirstCurrentLine <= this.NumberOfLoadedItems) {
            this.skipedItems = 0;
            this.FirstCurrentLine = 1;
            Z = this.NumberOfLoadedItems;
            this.takenItems = this.NumberOfLoadedItems;
        }
        else {
            this.skipedItems = this.FirstCurrentLine - 1 - this.NumberOfLoadedItems; //this.skipedItems - this.NumberOfLoadedItems;
            if (this.skipedItems < 0) {
                this.skipedItems = 0;
            }
            this.FirstCurrentLine = this.skipedItems + 1;
            Z = this.FirstCurrentLine + this.NumberOfLoadedItems - 1;
        }
        this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;
        this.loadingNextItems = true;
        if (this.skipedItems == 0) {
            this.IsPreviousButtonEnabled = false;
        }
        else {
            this.IsPreviousButtonEnabled = true;
        }
        if (!this.IsNextButtonEnabled) {
            this.IsNextButtonEnabled = true;
        }
        this.SaveChangesSync();
    };
    AddEditSupplierInvoiceComponent.prototype.Next = function () {
        var fullCount = 0;
        if (this.AccumulatedFilter == "parent") {
            fullCount = this.EntityPM.FullParentsCount;
        }
        else if (this.AccumulatedFilter == "child") {
            fullCount = this.EntityPM.FullChildrenCount;
        }
        else {
            fullCount = this.EntityPM.FullItemsCount;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
        var Z = 0;
        if (this.FirstCurrentLine == null) {
            this.FirstCurrentLine = 1;
        }
        if (this.FirstCurrentLine == 1) {
            this.IsPreviousButtonEnabled = false;
        }
        if (this.skipedItems + 1 != this.FirstCurrentLine) {
            if (this.FirstCurrentLine >= fullCount) {
                this.skipedItems = fullCount - this.NumberOfLoadedItems + 1;
                this.takenItems = this.NumberOfLoadedItems + 1;
                Z = fullCount;
                if (Z > fullCount) {
                    Z = fullCount;
                }
                this.FirstCurrentLine = this.skipedItems + 1;
                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;
            }
            else {
                this.skipedItems = this.FirstCurrentLine - 1;
                this.takenItems = this.NumberOfLoadedItems + 1; //501;
                Z = this.FirstCurrentLine + this.NumberOfLoadedItems;
                if (Z > fullCount) {
                    Z = fullCount;
                }
                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;
            }
        }
        else {
            if (this.FirstCurrentLine >= fullCount) {
                this.skipedItems = fullCount - this.NumberOfLoadedItems + 1;
                this.takenItems = this.NumberOfLoadedItems + 1;
                Z = fullCount;
                this.FirstCurrentLine = this.skipedItems + 1;
                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;
            }
            else {
                this.skipedItems = this.skipedItems + this.NumberOfLoadedItems;
                this.FirstCurrentLine = this.skipedItems + 1;
                if (this.FirstCurrentLine >= fullCount) {
                    Z = fullCount;
                }
                else {
                    if (this.takenItems > 1) {
                        Z = this.FirstCurrentLine + this.NumberOfLoadedItems - 1;
                    }
                    else {
                        Z = this.FirstCurrentLine + this.NumberOfLoadedItems;
                    }
                    if (Z > fullCount) {
                        Z = fullCount;
                    }
                }
                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;
            }
        }
        if (this.AccumulatedFilter == "parent") {
            var diff = this.EntityPM.FullParentsCount - this.FirstCurrentLine;
            if (diff <= 500) {
                this.IsNextButtonEnabled = false;
            }
        }
        else if (this.AccumulatedFilter == "child") {
            var diff = this.EntityPM.FullChildrenCount - this.FirstCurrentLine;
            if (diff <= 500) {
                this.IsNextButtonEnabled = false;
            }
        }
        else {
            if (Z == fullCount) {
                this.IsNextButtonEnabled = false;
            }
            else {
                this.IsNextButtonEnabled = true;
            }
        }
        this.loadingNextItems = true;
        if (!this.IsPreviousButtonEnabled && this.FirstCurrentLine != 1) {
            this.IsPreviousButtonEnabled = true;
        }
        this.SaveChangesSync();
    };
    AddEditSupplierInvoiceComponent.prototype.TextBoxKeyUp = function (event) {
        if (this.FirstCurrentLine == null) {
            this.FirstCurrentLine = 1;
        }
        if (!this.IsNextButtonEnabled && (this.FirstCurrentLine < this.EntityPM.FullItemsCount)) {
            this.IsNextButtonEnabled = true;
        }
        if (this.FirstCurrentLine == 1) {
            this.IsPreviousButtonEnabled = false;
        }
    };
    AddEditSupplierInvoiceComponent.prototype.SelectedTextBoxKeyUp = function (event) {
        var _this = this;
        if (event) {
            //this.SelectInvoiceItemEvent = this.CurrentSession.SelectInvoiceItemEvent.emit({ filter: this.TextValue });
            this.GENERAL.SelectInvoiceItemMethod({ filter: this.TextValue });
        }
        if (!this.TextValue) {
            this.LineDoesNotExist = false;
        }
        else {
            var value = this.EntityPM.SupplierInvoiceItems.filter(function (d) { return d.SequenceNumeric == _this.TextValue; })[0];
            if (!value) {
                this.LineDoesNotExist = true;
                this.LineDoesNotExistMessage = "מספר שורה לא נמצא";
            }
            else {
                this.LineDoesNotExist = false;
            }
        }
    };
    AddEditSupplierInvoiceComponent.prototype.ReloadSupplierInvoiceWithItems = function (skippedItems, takenItems) {
        var _this = this;
        this.loadingNextItems = false;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey, skippedItems, takenItems, this.AccumulatedFilter).subscribe(function (response) {
            _this.EntityPM = response.Result;
            _this.selectedTabCode = "GENERAL";
            _this.GENERAL.InitTab(_this.EntityPM, _this, _this.IsDisplayOnly, false, false);
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    AddEditSupplierInvoiceComponent.prototype.ValidateModifications = function () {
        var _this = this;
        if (this.EntityPM.SupplierInvoiceModifications) {
            var validationErrors = [];
            this.EntityPM.SupplierInvoiceModifications.forEach(function (mod) {
                var typeCode = mod.TypeCode;
                if (typeCode == "I02") {
                    //itzik+yaron20180201 validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee") + " - מסך נוספים");
                }
                else {
                    var exists = [];
                    if (_this.EntityPM.SupplierInvoiceModifications.length != 0) {
                        exists = _this.EntityPM.SupplierInvoiceModifications.filter(function (d) { return d.TypeCode == typeCode; });
                    }
                    if (exists.length > 1) {
                        validationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
                    }
                }
            });
            return validationErrors;
        }
    };
    AddEditSupplierInvoiceComponent.prototype.GetDocumentFilingId = function () {
        var _this = this;
        console.log(" --->> Getting related document filing ...");
        this.supplierInvoiceExtendedPMService.GetDocumentFilingIdForForInvoice(this.declarationPM.Id, this.EntityPM.InvoiceCounterKey).subscribe(function (response) {
            console.log("[Reponse] GetDocumentFilingIdForForInvoice: ", response);
            var result = response.Result;
            if (result) {
                _this.DocumentFilingId = response.Result;
                console.log("sending document filing document filing ...");
                DeclarationEventManager_1.DeclarationEventManager.DeclarationSplitDocumentSelection.emit(_this.DocumentFilingId);
                //(new MessageWindow()).Show("document filing found: " + this.DocumentFilingId);
            }
            else
                console.log("[!] No related document filing found!!");
        });
    };
    AddEditSupplierInvoiceComponent.prototype.GetCustomerCommissions = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.vendorCommissionService.GetCommissionsForCustomer(this.declarationPM.CustomerId).subscribe(function (response) {
            console.log("[Reponse] GetCommissionsForCustomer: ", response);
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var result = response.Result;
            if (result) {
                _this.CustomerCommissionsList = response.Result;
                _this.CalculateCommissionPercentage();
            }
        });
    };
    AddEditSupplierInvoiceComponent.prototype.CalculateCommissionPercentage = function () {
        var _this = this;
        if (this.EntityPM.VendorId && this.declarationPM.CustomerId && this.EntityPM.InvoiceAmount && this.EntityPM.InvoiceCurrencyTypeCode) {
            // GET percentage for vendor, customer from server
            //this.VendorCommissionPMService.get(this.EntityPM.VendorId, this.declarationPM.CustomerId).subscribe((response: ServiceResponse) => {
            //if (response && !response.HasError) {
            //var commission: VendorCommissionPM = response.Result;
            var commission = this.CustomerCommissionsList.filter(function (d) { return d.VendorId == _this.EntityPM.VendorId; })[0];
            if (commission) {
                if (commission.CommisionPercentage) {
                    //init new mod values
                    var newCurrency = this.EntityPM.InvoiceCurrencyTypeCode;
                    var newAmount = this.precisionRound((commission.CommisionPercentage / 100) * this.EntityPM.InvoiceAmount, 2);
                    // if commission found:
                    // 1- update Field VendorCommisionPercentage.SupplierInvoice
                    this.EntityPM.VendorComissionPercentage = commission.CommisionPercentage;
                    console.log("[!] invoice commission changed to:", this.EntityPM.VendorComissionPercentage);
                    // 2- In case there’s mod record , update it
                    var modTypeI10 = this.EntityPM.SupplierInvoiceModifications.filter(function (d) { return d.TypeCode == "I10"; })[0];
                    if (modTypeI10) {
                        // 3- In case there’s record with same type (I10) 
                        //    and it's with different currency OR value ask user
                        if (modTypeI10.Amount != newAmount || modTypeI10.CurrencyTypeCode != newCurrency) {
                            //somthing changed, amount or currency
                            //ask user to change it
                            var confirm = new ConfirmWindow_1.ConfirmWindow;
                            var msgTxt = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CommissionChangedFromTo");
                            msgTxt = msgTxt.replace("#oldValue", modTypeI10.Amount.toFixed(2).toString() + " " + modTypeI10.CurrencyTypeCode);
                            msgTxt = msgTxt.replace("#newValue", newAmount.toFixed(2).toString() + " " + newCurrency); // new values
                            confirm.Show(msgTxt);
                            confirm.WindowClosed.subscribe(function (event) {
                                if (confirm.Yes) {
                                    //update record
                                    modTypeI10.CurrencyTypeCode = newCurrency;
                                    modTypeI10.CurrencyTypeName = _this.invoiceCurrencyName;
                                    modTypeI10.Amount = newAmount;
                                    _this.UpdateModificationsList();
                                    confirm.Close();
                                }
                                else {
                                    //don't update
                                    confirm.Close();
                                }
                            });
                        }
                        else {
                            // same currency and amount
                            if (modTypeI10.Amount == newAmount && modTypeI10.CurrencyTypeCode == newCurrency) {
                                // no changes
                            }
                        }
                    }
                    else {
                        //In case there’s no mod record I10, create a record in SupplierInvoiceModification 
                        var newMod = new SupplierInvoiceModificationPM_1.SupplierInvoiceModificationPM(this.EntityPM);
                        newMod.Tenant = this.EntityPM.Tenant;
                        newMod.DeclarationId = this.EntityPM.DeclarationId;
                        newMod.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
                        newMod.TypeCode = "I10";
                        newMod.TypeName = this.typeNameForI10;
                        newMod.CurrencyTypeCode = newCurrency;
                        newMod.CurrencyTypeName = this.invoiceCurrencyName;
                        newMod.Amount = newAmount;
                        this.EntityPM.SupplierInvoiceModifications.push(newMod);
                        this.UpdateModificationsList();
                    }
                }
            }
            else {
                //No commission for this vendor , delete existing commission ?
                var msg = "לא קיימים נתוני עמלה לספק זה , האם למחוק נתוני עמלה קיימים ?";
                if (this.EntityPM.VendorComissionPercentage) {
                    var confirm = new ConfirmWindow_1.ConfirmWindow();
                    confirm.Show(msg);
                    confirm.WindowClosed.subscribe(function (event) {
                        if (confirm.Yes) {
                            //delete commission
                            _this.EntityPM.VendorComissionPercentage = null;
                            console.log("[!] invoice commission deleted");
                            var modTypeI10 = _this.EntityPM.SupplierInvoiceModifications.filter(function (d) { return d.TypeCode == "I10"; })[0];
                            _this.EntityPM.RemoveSupplierInvoiceModification(modTypeI10);
                            _this.UpdateModificationsList();
                            confirm.Close();
                        }
                        else {
                            //don't
                            confirm.Close();
                        }
                    });
                }
            }
            //}
            //});
        }
        else {
            console.log("[!] Cannot calculate commession percentage, some fields are required!");
        }
    };
    AddEditSupplierInvoiceComponent.prototype.UpdateModificationsList = function () {
        this.ReloadModificationEvent.emit("");
    };
    AddEditSupplierInvoiceComponent.prototype.precisionRound = function (number, digitsAfterPoint) {
        var factor = Math.pow(10, digitsAfterPoint);
        return Math.round(number * factor) / factor;
    };
    AddEditSupplierInvoiceComponent.prototype.getModTypeName = function () {
        var _this = this;
        this._ModificationAndDiscountTypeListService.getSingleFromCache("I10").subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var type = myResponse.Result;
                if (type)
                    _this.typeNameForI10 = type.LocalName;
            }
        });
    };
    AddEditSupplierInvoiceComponent.prototype.DropdownDisplayClose = function () {
        this._DropdownDisplay = 'none';
    };
    AddEditSupplierInvoiceComponent.prototype.getScreenHeight = function () { return self.innerHeight; };
    AddEditSupplierInvoiceComponent.prototype.dropdowndisplayToggle = function () {
        var item = document.getElementById("savebutton");
        var itemRect = item.getBoundingClientRect();
        //document.getElementById(this._CustomSendOptionsComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
        //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
        document.getElementById("dropdowmenu").style.top =
            itemRect.top + 'px';
        var DDLHeight = 67; //    height: 22px; * 3 +30 
        var Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
        if (itemRect.bottom + DDLHeight > this.getScreenHeight()) { //this.PaintTop = true                
            document.getElementById("dropdowmenu").style.top =
                (itemRect.bottom - DDLHeight - Extra) + 'px';
        }
        //document.getElementById("dropdowmenu").style.left =
        //    (itemRect.left + 100) + 'px';
        if (this._DropdownDisplay == 'none') {
            this._DropdownDisplay = 'block';
        }
        else {
            this._DropdownDisplay = 'none';
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AddEditSupplierInvoiceComponent.prototype, "AllLocations", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], AddEditSupplierInvoiceComponent.prototype, "ReloadModificationEvent", void 0);
    AddEditSupplierInvoiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditSupplierInvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditSupplierInvoiceComponent);
    return AddEditSupplierInvoiceComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditSupplierInvoiceComponent = AddEditSupplierInvoiceComponent;
var TabItem = /** @class */ (function () {
    function TabItem(Code, TextCode) {
        this.code = Code;
        this.textCode = TextCode;
    }
    return TabItem;
}());
var AnalyzeUnifreightInsuranceService = /** @class */ (function () {
    function AnalyzeUnifreightInsuranceService() {
    }
    //public IsFritzFeatureIsOn: boolean
    ///public SuppressUpdateFreight: boolean
    AnalyzeUnifreightInsuranceService.prototype.Open = function (unifreightMessageM, declarationPM, supplierInvoice) {
        var sAction = "";
        sAction = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.Action");
        var sInsuranseIsNeeded = "";
        sInsuranseIsNeeded = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsNeeded");
        var sInsuranseIsSucceeded = "";
        sInsuranseIsSucceeded = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsSucceeded");
        var sInsuranceHasOpen = "";
        sInsuranceHasOpen = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseHasOpen");
        var sInsuranceMessage = "";
        sInsuranceMessage = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranceMessage");
        var sInsuranseAmount = "";
        sInsuranseAmount = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseAmount");
        var sInsuranseCurrency = "";
        sInsuranseCurrency = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseCurrency");
        var sExpensesAmount = "";
        sExpensesAmount = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmount");
        var sExpensesAmountCurr = "";
        sExpensesAmountCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmountCurr");
        var sFreightAmount = "";
        sFreightAmount = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount");
        var sFreightAmountCurr = "";
        sFreightAmountCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr");
        //Task 40723:
        var sFreightAmount2 = "";
        sFreightAmount2 = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount2");
        var sFreightAmountCurr2 = "";
        sFreightAmountCurr2 = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr2");
        var sTotalFreightInFreightCurr = "";
        sTotalFreightInFreightCurr = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.TotalFreightInFreightCurr");
        console.log("Fritz FreightAmount: sFreightAmount= " + sFreightAmount + " sFreightAmount2=" + sFreightAmount2 + " TotalFreightInFreightCurr=" + sTotalFreightInFreightCurr);
        if (sAction != "OPEN") {
            throw Error("sAction != OPEN  !!!!");
        }
        var toUpdateFreightAmount = true;
        //if (this.IsFritzFeatureIsOn) {
        //if (!this.SuppressUpdateFreight) {
        if (declarationPM.IsValueForCustomsOnly) { //o	יש לבדוק את השדה החדש, ואם הערך true אז לא לעדכן הובלה. אחרת לעדכן הובלה, אם התקבל ערך
            toUpdateFreightAmount = false;
            ;
            console.warn("FritzFeatureIsOn but SuppressUpdateFreight is true soo Suppress Update Freight ");
        }
        //}
        if (FeatureLocator_1.FeatureLocator.IsFeatureGrantedByCode("IFRITZ")) { //    o        לאחר שמירה ובדיקת שדות לשליחה, יש לבדוק Feature כפי שבודקים במסך חשבון ספק
            console.log("FritzFeatureIsON .. ");
            if (toUpdateFreightAmount && !Tools_1.AppTool.IsNullOrEmpty(sFreightAmount) && !Tools_1.AppTool.IsNullOrEmpty(sFreightAmountCurr)) {
                console.warn("Update Freight ");
                var FreightAmount = 0;
                var FreightAmount2 = 0; //task 40723
                if (Number(sFreightAmount2) != NaN) {
                    FreightAmount2 = Number(sFreightAmount2);
                }
                var TotalFreightInFreightCurr = 0;
                if (Number(sTotalFreightInFreightCurr) != NaN) {
                    TotalFreightInFreightCurr = Number(sTotalFreightInFreightCurr);
                }
                if (Number(sFreightAmount) != NaN) { //(decimal.TryParse(sFreightAmount, out FreightAmount)) {
                    FreightAmount = Number(sFreightAmount);
                    if (FreightAmount2 == 0) { //task 40723
                        supplierInvoice.TotalFreightInFreightCurrency = FreightAmount;
                        supplierInvoice.FreightCurrencyTypeCode = sFreightAmountCurr;
                    }
                    else { //TODO: calc tot amount...
                        supplierInvoice.TotalFreightInFreightCurrency = TotalFreightInFreightCurr;
                        supplierInvoice.FreightCurrencyTypeCode = sFreightAmountCurr;
                    }
                    //supplierInvoice.TotalFreightInFreightCurrency = FreightAmount;
                    //supplierInvoice.FreightCurrencyTypeCode = sFreightAmountCurr;
                    var supplierInvoiceFreightAmountPM = void 0;
                    if (supplierInvoice.SupplierInvoiceFreightAmounts.length > 0) {
                        //for (let item of supplierInvoice.SupplierInvoiceFreightAmounts) {
                        //    supplierInvoice.RemoveSupplierInvoiceFreightAmount(item);
                        //}
                        var lengthCounter = supplierInvoice.SupplierInvoiceFreightAmounts.length;
                        for (var i = 0; i < lengthCounter; i++) {
                            supplierInvoice.RemoveSupplierInvoiceFreightAmount(supplierInvoice.SupplierInvoiceFreightAmounts[0]);
                        }
                    }
                    supplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM_1.SupplierInvoiceFreightAmountPM(supplierInvoice);
                    supplierInvoice.AddSupplierInvoiceFreightAmount(supplierInvoiceFreightAmountPM);
                    supplierInvoiceFreightAmountPM.DeclarationId = supplierInvoice.DeclarationId;
                    supplierInvoiceFreightAmountPM.Tenant = supplierInvoice.Tenant;
                    supplierInvoiceFreightAmountPM.InvoiceCounterKey = supplierInvoice.InvoiceCounterKey;
                    supplierInvoiceFreightAmountPM.Amount = FreightAmount;
                    supplierInvoiceFreightAmountPM.CurrencyTypeCode = sFreightAmountCurr;
                    if (FreightAmount2 != 0) { //task 40723
                        var supplierInvoiceFreightAmount2PM = void 0;
                        supplierInvoiceFreightAmount2PM = new SupplierInvoiceFreightAmountPM_1.SupplierInvoiceFreightAmountPM(supplierInvoice);
                        supplierInvoice.AddSupplierInvoiceFreightAmount(supplierInvoiceFreightAmount2PM);
                        supplierInvoiceFreightAmount2PM.DeclarationId = supplierInvoice.DeclarationId;
                        supplierInvoiceFreightAmount2PM.Tenant = supplierInvoice.Tenant;
                        supplierInvoiceFreightAmount2PM.InvoiceCounterKey = supplierInvoice.InvoiceCounterKey;
                        supplierInvoiceFreightAmount2PM.Amount = FreightAmount2;
                        supplierInvoiceFreightAmount2PM.CurrencyTypeCode = sFreightAmountCurr2;
                    }
                    //supplierInvoice.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //this.declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    declarationPM.IsChanged = true;
                }
            }
            //o	בקבלת התשובה יש לעדכן הוצאות (160) אם התקבל ערך
            if (!Tools_1.AppTool.IsNullOrEmpty(sExpensesAmount) && !Tools_1.AppTool.IsNullOrEmpty(sExpensesAmountCurr)) {
                console.log("Update  sExpensesAmount");
                var ExpensesAmount = 0;
                if (Number(sExpensesAmount) != NaN) { //(decimal.TryParse(sExpensesAmount, out ExpensesAmount)) {
                    ExpensesAmount = Number(sExpensesAmount);
                    var supplierInvoiceModificationPM = void 0;
                    var list160 = supplierInvoice.SupplierInvoiceModifications.filter(function (rec) { return rec.TypeCode == "160"; });
                    if (list160.length > 0) {
                        //supplierInvoiceModificationPM = list160[0];
                        supplierInvoice.RemoveSupplierInvoiceModification(list160[0]);
                    }
                    supplierInvoiceModificationPM = new SupplierInvoiceModificationPM_1.SupplierInvoiceModificationPM(supplierInvoice);
                    supplierInvoice.AddSupplierInvoiceModification(supplierInvoiceModificationPM);
                    supplierInvoiceModificationPM.DeclarationId = supplierInvoice.DeclarationId;
                    supplierInvoiceModificationPM.Tenant = supplierInvoice.Tenant;
                    supplierInvoiceModificationPM.InvoiceCounterKey = supplierInvoice.InvoiceCounterKey;
                    supplierInvoiceModificationPM.Amount = ExpensesAmount;
                    supplierInvoiceModificationPM.CurrencyTypeCode = sExpensesAmountCurr;
                    //supplierInvoiceModificationPM.ModificationCounterKey = 1;
                    supplierInvoiceModificationPM.TypeCode = "160";
                    declarationPM.IsChanged = true;
                }
            }
        }
        //o	ערך ביטוח מעדכנים תמיד, בתנאי שחזק מבדיקת הביטוח
        if (!Tools_1.AppTool.IsNullOrEmpty(sInsuranseAmount) && !Tools_1.AppTool.IsNullOrEmpty(sInsuranseCurrency)) {
            var insuranseAmount = 0;
            if (Number(sInsuranseAmount) != NaN) { //if (decimal.TryParse(sInsuranseAmount, out insuranseAmount)) {
                insuranseAmount = Number(sInsuranseAmount);
                supplierInvoice.InsuranceAmount = insuranseAmount;
                supplierInvoice.InsruanceCurrencyTypeCode = sInsuranseCurrency;
                supplierInvoice.InsruancePercentage = null;
                //supplierInvoice.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                //declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                declarationPM.IsChanged = true;
                //VisibilityInsurance = "Collapsed";
                //var submit = context.SubmitChanges();
                //submit.Completed += submit_Completed;
            }
        }
    };
    return AnalyzeUnifreightInsuranceService;
}());
exports.AnalyzeUnifreightInsuranceService = AnalyzeUnifreightInsuranceService;
//# sourceMappingURL=AddEditSupplierInvoiceComponent.js.map