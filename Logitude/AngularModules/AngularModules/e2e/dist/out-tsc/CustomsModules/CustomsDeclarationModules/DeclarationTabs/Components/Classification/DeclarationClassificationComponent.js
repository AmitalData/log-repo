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
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConsignmentPM_1 = require("../../../../../Customs/EntityPMs/ConsignmentPM");
var LogTabsComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var CardPMService_1 = require("../../../../../Common/Services/StandardPMs/CardPMService");
var CustomsHouseTypeExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService");
var DeclarationDisplayOnlyChecks_1 = require("../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var DeclarationEventManager_1 = require("../../../../../Customs/Utilities/DeclarationEventManager");
var CustomsRequiredFieldListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var SupplierInvoiceExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
var GITITEMExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/GITITEMExtendedPMService");
var GITITEMCacheService_1 = require("../../../../../Customs/Services/Others/GITITEMCacheService");
var DeclarationClassificationComponent = /** @class */ (function (_super) {
    __extends(DeclarationClassificationComponent, _super);
    function DeclarationClassificationComponent(entityArgs, cd, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.IsGetTableName = true;
        _this.IsImporerCodeEnabled = true;
        _this.IsTransferImporterEnabled = true;
        _this.IsEntitleImporterEnabled = true;
        _this.DisplayOnlyMessage = "";
        _this.cardService = new CardPMService_1.CardPMService;
        _this.customsHouseTypeExtendedPMService = new CustomsHouseTypeExtendedPMService_1.CustomsHouseTypeExtendedPMService;
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService;
        _this.SInvoiceTabs = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region XML Errors
        _this.IsWindowMode = false;
        _this.ForceSave = false;
        //#region Properties
        _this._GrossMassMeasure = "0.00";
        _this._FreightAmount = ""; //41322
        _this._TotalInvoiceAmountInUSD = "";
        _this.DrawMe = true;
        _this.isImporterClicked = false;
        _this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        //#region Tabs Component code
        _this.consignmentIndex = 0;
        _this.consignmentNumber = 0;
        _this.PreceduralFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.PreceduralFilterItems.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "boolean");
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.ConsignmentPackage").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.ConsignmentInternalTransition").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(function (response) {
                            _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(function (response) {
                                _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(function (response) {
                                    _this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(function (response) {
                                        _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
                                            _this.EntityPM = _this.entityArgs.EntityPM;
                                            _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                                            _this.Listen();
                                            //var tab;
                                            console.log("DeclarationClassificationComponent/EntityPM ", _this.EntityPM);
                                            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM)) {
                                                // create consignment tabs from entity 
                                                //for (let item of this.EntityPM.Consignments) {
                                                //    tab = new LogTab();
                                                //    tab.EntityPM = item;
                                                //    tab.Code = item.SequenceNumeric.toString();
                                                //    tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
                                                //    tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/General/ConsigmentTabContent/ConsigmentTabContentComponent";
                                                //    this.ConsigmentTabs.push(tab);
                                                //}
                                                //this.checkImportersVisibility();
                                                _this.DisplayOnlyCheck();
                                                _this.CheckRequrierdFieldsForSend();
                                                _this.BuildScreen();
                                            }
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
        //Disable fields
        if (_this.IsDisplayOnly) {
            _this.SetScreenFieldsEditability();
        }
        return _this;
    }
    DeclarationClassificationComponent.prototype.ngOnDestroy = function () {
        console.log("DeclarationClassificationComponent:ngOnDestroy():ConsigmentTabs");
        this.SInvoiceTabs.forEach(function (tab) {
            if (tab.ComponentReference && tab.ComponentReference.ngOnDestroy) {
                tab.ComponentReference.ngOnDestroy();
            }
            tab.ComponentReference = null;
        });
        this.SInvoiceTabs = null;
    };
    DeclarationClassificationComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.ForceSave = true;
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.SInvoiceTabs.forEach(
                    //    tab => {
                    //        (tab.Parent.AddEditSupplierInvoiceDUMMYManager as AddEditSupplierInvoiceDUMMY).
                    //            SaveItemCodeLocalCache();
                    //    });
                    GITITEMCacheService_1.GITITEMCacheService.Instance.SaveItemCodeLocalCache();
                    _this.BuildScreen();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess && _this.CurrentSession.CurrentEditComponent) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.RefreshDatePicker = false;
                    _this.BuildScreen();
                    _this.DisplayOnlyCheck();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCF") {
                        _this.CurrentSession.StartBusyIndicatorLoading();
                        //this.RefreshEntity();
                        _this.ForceSave = true;
                        _this.SInvoiceTabs = [];
                        _this.BuildScreen();
                        _this.DisplayOnlyCheck();
                    }
                    else {
                        if (_this.ForceSave && _this.EntityPM.IsDirty) {
                            _this.CurrentSession.StartBusyIndicatorSaving();
                            _this.CurrentSession.CurrentEditComponent.SaveAndCloseCompleted
                                .subscribe(function (isSuccess) {
                                if (isSuccess) {
                                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    _this.CurrentSession.StopBusyIndicator();
                                }
                            });
                            _this.CurrentSession.CurrentEditComponent.SaveChanges();
                        }
                        _this.ForceSave = false;
                    }
                }
            }));
        }
    };
    DeclarationClassificationComponent.prototype.SetScreenFieldsEditability = function () {
        this.UIProperties.SetEnabled("CasualSupplierName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CalculatedImporterName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IncotermCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("GrossMassMeasure", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalInvoiceAmountInUSD", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("_InvoiceAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("_PackageQuantity", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("_FreightAmount", this.ObjectTableName, false); //41322
        this.UIProperties.SetEnabled("FreightAmount", this.ObjectTableName, false); //41322
        this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDescription", this.ObjectTableName, !this.IsDisplayOnly);
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
        this.IsTransferImporterEnabled = !this.IsDisplayOnly;
        this.IsEntitleImporterEnabled = !this.IsDisplayOnly;
        if (!this.IsDisplayOnly) {
            this.IsImporerCodeEnabled = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterName) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterAddress);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode)) {
                this.IsImporerCodeEnabled = true;
                if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    this.IsImporerCodeEnabled = false;
                }
            }
        }
    };
    Object.defineProperty(DeclarationClassificationComponent.prototype, "GrossMassMeasure", {
        get: function () { return this._GrossMassMeasure; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "FreightAmount", {
        get: function () { return this._FreightAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "TotalInvoiceAmountInUSD", {
        get: function () { return this._TotalInvoiceAmountInUSD; },
        set: function (newValue) {
            this._TotalInvoiceAmountInUSD = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "DeclarationOfficeCode", {
        get: function () { return this.EntityPM.DeclarationOfficeCode; },
        set: function (newValue) {
            this.EntityPM.DeclarationOfficeCode = newValue;
            this.ChangeTransportMode();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "ProcedureCurrentCode", {
        get: function () { return this.EntityPM.ProcedureCurrentCode; },
        set: function (newValue) {
            this.EntityPM.ProcedureCurrentCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "DeclarationDocumentTypeCode", {
        get: function () { return this.EntityPM.DeclarationDocumentTypeCode; },
        set: function (newValue) { this.EntityPM.DeclarationDocumentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "ImporterCode", {
        get: function () { return this.EntityPM.ImporterCode; },
        set: function (newValue) {
            if (this.EntityPM.ImporterCode != newValue) {
                this.EntityPM.ImporterCode = newValue;
                this.EntityPM.ImporterTypeCode = "1";
                //this.EntityPM.ImporterTypeName = "IL";
                this.EntityPM.MainImporterEntitlemntTypeCode = null;
                this.EntityPM.ImporterAddress = null;
                this.EntityPM.ImporterPassportNumber = null;
                // this.EntityPM.ImporterName = null;
                this.EntityPM.ImporterPassCountryCode = null;
                var needTodELETE = false;
                if (!this.EntityPM.IsCourierDeclaration) { //due courier
                    this.EntityPM.ImporterName = ""; //
                    this.EntityPM.CasualImporterAddress1 = "";
                    this.EntityPM.CasualImporterAddress2 = "";
                    this.EntityPM.CasualImporterCity = "";
                    this.EntityPM.CasualImporterZipCode = "";
                    this.EntityPM.CasualImporterFax = "";
                    this.EntityPM.CasualImporterEmail = "";
                    this.EntityPM.CasualImporterTel = "";
                    this.EntityPM.CasualImporterContact = "";
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationClassificationComponent.prototype.ImporterLostFocus = function (item, importerSearchBox) {
        var _this = this;
        var type = 'Importer';
        if (this.isImporterClicked != true) {
            switch (type) {
                case 'Importer': {
                    this.EntityPM.ImporterId = "";
                    this.CalculatedImporterName = "";
                    break;
                }
            }
        }
        this.isImporterClicked = false;
        if (type == 'Importer' && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerVatNo) && !Tools_1.AppTool.IsNullOrEmpty(item) && this.EntityPM.CustomerVatNo != item) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.VatChanged"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) { // YES
                    _this.EntityPM.VatChanged = true;
                    _this.ImporterCode = item;
                }
                if (confirmWindow.No) { // NO
                    SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
                    var element = document.getElementById(importerSearchBox.InputId);
                    if (element) {
                        element.focus();
                    }
                }
            });
        }
        else {
            switch (type) {
                case 'Importer': {
                    this.ImporterCode = item;
                    break;
                }
            }
        }
    };
    DeclarationClassificationComponent.prototype.ImporterClicked = function (client) {
        var type = 'Importer';
        if (client) {
            this.isImporterClicked = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            switch (type) {
                case 'Importer': {
                    this.ImporterCode = client.Code;
                    this.EntityPM.ImporterId = client.Id;
                    this.CalculatedImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    break;
                }
            }
        }
    };
    DeclarationClassificationComponent.prototype.EditImporter = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.Type = "Importer";
        ///this.Type = "Importer";
        logWindow.Width = 550;
        logWindow.Height = this.EntityPM.IsCourierDeclaration ? 550 : 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.SetFieldsDisabled($event); });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');
    };
    DeclarationClassificationComponent.prototype.SetFieldsDisabled = function (message) {
        this.SetScreenFieldsEditability();
    };
    Object.defineProperty(DeclarationClassificationComponent.prototype, "CasualSupplierName", {
        get: function () {
            return this.EntityPM.CasualSupplierName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "CalculatedImporterName", {
        get: function () {
            if (this.EntityPM.ImporterCode == null && this.EntityPM.ImporterName != null)
                return this.EntityPM.ImporterName;
            else
                return this.EntityPM.CalculatedImporterName;
        },
        set: function (newValue) { this.EntityPM.CalculatedImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "TransferImporterCode", {
        get: function () {
            return this.EntityPM.TransferImporterCode;
        },
        set: function (newValue) {
            if (this.EntityPM.TransferImporterCode != newValue) {
                this.EntityPM.TransferImporterCode = newValue;
                this.EntityPM.TransferImporterTypeCode = "1";
                this.EntityPM.TransferImporterTypeName = "IL";
                this.EntityPM.TransImporterEntitleTypeCode = null;
                this.EntityPM.TransferImporterAddress = null;
                this.EntityPM.TransferPassportNumber = null;
                //     this.EntityPM.TransferImporterName = null;
                this.EntityPM.TransferImporterCountryCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "CalculatedTransferImporterName", {
        get: function () {
            if (this.EntityPM.TransferImporterCode == null && this.EntityPM.TransferImporterName != null)
                return this.EntityPM.TransferImporterName;
            else
                return this.EntityPM.CalculatedTransferImporterName;
        },
        set: function (newValue) { this.EntityPM.CalculatedTransferImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "EntitleImporterCode", {
        get: function () { return this.EntityPM.EntitleImporterCode; },
        set: function (newValue) {
            if (this.EntityPM.EntitleImporterCode != newValue) {
                this.EntityPM.EntitleImporterCode = newValue;
                this.EntityPM.EntitleImporterTypeCode = "1";
                this.EntityPM.EntitleImporterTypeName = "IL";
                this.EntityPM.ImporterEntitlementTypeCode = null;
                this.EntityPM.EntitleImporterAddress = null;
                this.EntityPM.EntitlePassportNumber = null;
                //   this.EntityPM.EntitleImporterName = null;
                this.EntityPM.EntitleImporterCountryCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "CalculatedEntitleImporterName", {
        get: function () {
            if (this.EntityPM.EntitleImporterCode == null && this.EntityPM.EntitleImporterName != null)
                return this.EntityPM.EntitleImporterName;
            else
                return this.EntityPM.CalculatedEntitleImporterName;
        },
        set: function (newValue) { this.EntityPM.CalculatedEntitleImporterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "EntitleImporterCountryCode", {
        get: function () { return this.EntityPM.EntitleImporterCountryCode; },
        set: function (newValue) { this.EntityPM.EntitleImporterCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "ImporterPassCountryCode", {
        get: function () { return this.EntityPM.ImporterPassCountryCode; },
        set: function (newValue) { this.EntityPM.ImporterPassCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "EntitleImporterCountryName", {
        get: function () { return this.EntityPM.EntitleImporterCountryName; },
        set: function (newValue) { this.EntityPM.EntitleImporterCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
            if (this.selectedTab) {
                var si = this.selectedTab.EntityPM;
                //var jsonSI = mySupplierInvoiceExtendedPMService.clone(si);
                //mySupplierInvoiceExtendedPMService.MapSupplierInvoiceItems(si, jsonSI, true);
                this.GetDocumentFilingId(si.InvoiceCounterKey);
            }
            //this.selectedTab.EntityPM.InvoiceCounterKey
        },
        enumerable: true,
        configurable: true
    });
    DeclarationClassificationComponent.prototype.GetDocumentFilingId = function (InvoiceCounterKey) {
        var _this = this;
        console.log(" --->> Getting related document filing ...");
        this.supplierInvoiceExtendedPMService.GetDocumentFilingIdForForInvoice(this.EntityPM.Id, InvoiceCounterKey).subscribe(function (response) {
            console.log("[Reponse] GetDocumentFilingIdForForInvoice: ", response);
            var result = response.Result;
            if (result) {
                _this.DocumentFilingId = response.Result;
                console.log("sending document filing document filing ...");
                var mohammadAdviseNotItzik = true;
                if (mohammadAdviseNotItzik) {
                    var t = setTimeout(function () {
                        DeclarationEventManager_1.DeclarationEventManager.DeclarationSplitDocumentSelection.emit(_this.DocumentFilingId);
                        ;
                        clearTimeout(t);
                    }, 400);
                }
                else {
                    DeclarationEventManager_1.DeclarationEventManager.DeclarationSplitDocumentSelection.emit(_this.DocumentFilingId);
                    ;
                }
                //(new MessageWindow()).Show("document filing found: " + this.DocumentFilingId);
            }
            else
                console.log("[!] No related document filing found!!");
        });
    };
    DeclarationClassificationComponent.prototype.OnSelectedChanged = function (tab) {
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
            console.log("Tab selected: ", tab);
        }
    };
    //#endregion
    DeclarationClassificationComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationClassificationComponent.prototype.ChangeTransportMode = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
            this.customsHouseTypeExtendedPMService.GetHouseTypewithAdditional(this.DeclarationOfficeCode).subscribe(function (result) {
                if (!Tools_1.AppTool.IsNullOrEmpty(result.Result)) {
                    var houseType = result.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(houseType)) {
                        _this.EntityPM.TransportModeId = houseType.TransportModeId;
                        console.log("...TransportModeId changed to ", houseType.TransportModeId);
                    }
                }
            });
        }
    };
    DeclarationClassificationComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        this.DrawMe = true;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe(function (response) {
            var displayOnlyCheckResult = response.Result;
            _this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (_this.IsDisplayOnly) {
                _this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (_this.EntityPM.StorageStatusCode) {
                _this.ShowStorageStatusMessage = true;
                _this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + _this.EntityPM.StorageStatusName;
            }
            _this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(_this.IsDisplayOnly);
        });
    };
    Object.defineProperty(DeclarationClassificationComponent.prototype, "CargoDescription", {
        get: function () { return this._CargoDescription; },
        set: function (value) {
            var _this = this;
            if (this._CargoDescription != value) {
                this._CargoDescription = value;
                this.EntityPM.Consignments.forEach(function (r) {
                    r.CargoDescription = _this._CargoDescription;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "IncotermCode", {
        get: function () { return this._IncotermCode; },
        enumerable: true,
        configurable: true
    });
    DeclarationClassificationComponent.prototype.BuildScreen = function () {
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.CalcFields();
        this.BuildSInvoiceTabs();
        //this.cd.detectChanges();
    };
    DeclarationClassificationComponent.prototype.CalcFields = function () {
        var MyPrimarySupplierInvoice;
        if (this.EntityPM.SupplierInvoices.length > 0) {
            MyPrimarySupplierInvoice = this.EntityPM.SupplierInvoices.filter(function (r) { return r.IsPrimarySupplierInvoice == true; })[0];
            if (!Tools_1.AppTool.IsNullOrEmpty(MyPrimarySupplierInvoice)) {
                this._IncotermCode = MyPrimarySupplierInvoice.IncotermCode;
            }
        }
        var _My1stConsignmentPM;
        if (this.EntityPM.Consignments.length > 0) {
            _My1stConsignmentPM = this.EntityPM.Consignments[0];
        }
        else {
            this.EntityPM.AddConsignment(new ConsignmentPM_1.ConsignmentPM(this.EntityPM));
            ;
            _My1stConsignmentPM = this.EntityPM.Consignments[0];
        }
        this.CargoDescription = _My1stConsignmentPM.CargoDescription;
        var gross = 0;
        this._PackageQuantity = 0;
        for (var _i = 0, _a = this.EntityPM.Consignments; _i < _a.length; _i++) {
            var c = _a[_i];
            for (var _b = 0, _c = c.ConsignmentPackages.filter(function (r) { return r.PackageMeasureQualifierCode == "2"; }); _b < _c.length; _b++) {
                var cmqa = _c[_b];
                this._PackageQuantity += cmqa.PackageQuantity;
                gross += cmqa.GrossMassMeasure;
            }
        }
        this._GrossMassMeasure = Number(gross).toFixed(2);
        var SupplierInvoicesWithInvoiceAmount = this.EntityPM.SupplierInvoices.filter(function (r) { return !Tools_1.AppTool.IsNullOrZero(r.InvoiceAmount); });
        var ObjectThatEachPropertyIsArray = Tools_1.ArrayTool.GroupIt(SupplierInvoicesWithInvoiceAmount, function (item) { return item.InvoiceCurrencyTypeCode; });
        this._InvoiceAmount = null;
        if (Object.keys(ObjectThatEachPropertyIsArray).length == 1) {
            var invoiceAmount = Tools_1.ArrayTool.Sum(this.EntityPM.SupplierInvoices, "InvoiceAmount");
            var numberFix2 = Number(invoiceAmount).toFixed(2);
            //this._InvoiceAmount = numberFix2 as string;
            this._InvoiceAmount = this.EntityPM.SupplierInvoices[0].InvoiceCurrencyTypeCode + " " + numberFix2; //41322
        }
        var SupplierInvoicesWithFreight = this.EntityPM.SupplierInvoices.filter(function (r) { return !Tools_1.AppTool.IsNullOrZero(r.TotalFreightInFreightCurrency); });
        //41322:
        var FreightCurrenciesArray = Tools_1.ArrayTool.GroupIt(SupplierInvoicesWithFreight, function (item) { return item.FreightCurrencyTypeCode; });
        this._FreightAmount = null;
        if (Object.keys(FreightCurrenciesArray).length == 1) {
            var freightAmount = Tools_1.ArrayTool.Sum(this.EntityPM.SupplierInvoices, "TotalFreightInFreightCurrency");
            var freightFix2 = Number(freightAmount).toFixed(2);
            this._FreightAmount = this.EntityPM.SupplierInvoices[0].FreightCurrencyTypeCode + " " + freightFix2;
        }
        else {
            var freightAmount = Tools_1.ArrayTool.Sum(this.EntityPM.SupplierInvoices, "TotalFreightInNIS");
            var freightFix2 = Number(freightAmount).toFixed(2);
            this._FreightAmount = "ILS " + freightFix2;
        }
        var InvoiceAmountInUSD = Tools_1.ArrayTool.Sum(this.EntityPM.SupplierInvoices, "InvoiceAmountInUSD");
        this._TotalInvoiceAmountInUSD = Number(InvoiceAmountInUSD).toFixed(2);
    };
    ///public ParentAddEditSupplierInvoiceDUMMY: AddEditSupplierInvoiceDUMMY = new AddEditSupplierInvoiceDUMMY();
    DeclarationClassificationComponent.prototype.BuildSInvoiceTabs = function () {
        //this.ReconnectSII();
        this.SInvoiceTabs = [];
        for (var _i = 0, _a = this.EntityPM.SupplierInvoices; _i < _a.length; _i++) {
            var item = _a[_i];
            if (Tools_1.AppTool.IsNullOrEmpty(item.SequenceNumeric)) {
                continue;
            }
            var si = item;
            //var jsonSI = mySupplierInvoiceExtendedPMService.clone(si);
            //mySupplierInvoiceExtendedPMService.MapSupplierInvoiceItems(si, jsonSI, true);
            var tab = new LogTabsComponent_1.LogTab();
            tab.EntityPM = item;
            tab.Parent = {
                DeclarationPM: this.EntityPM,
                AddEditSupplierInvoiceDUMMYManager: new AddEditSupplierInvoiceDUMMY(this.EntityPM.CustomerCode)
            };
            tab.Code = item.SequenceNumeric.toString();
            tab.Header = (si.InvoiceNumber ? (item.InvoiceNumber + '-') : '') + item.SequenceNumeric;
            tab.ComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Classification/SInvoiceClassificationTabComponent";
            this.SInvoiceTabs.push(tab);
        }
        if (this.SInvoiceTabs.length > 0) {
            this.SelectedTab = this.SInvoiceTabs[0];
        }
        this.CurrentSession.StopBusyIndicator();
    };
    DeclarationClassificationComponent.prototype.ReconnectSII = function () {
        var mySupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        //due 
        var reconnectedSupplierInvoices = new Array();
        for (var _i = 0, _a = this.EntityPM.SupplierInvoices; _i < _a.length; _i++) {
            var item = _a[_i];
            var si = item;
            //var jsonSI = mySupplierInvoiceExtendedPMService.clone(si);
            //mySupplierInvoiceExtendedPMService.MapSupplierInvoiceItems(si, jsonSI, true);
            var newSIWithSIItemMap = mySupplierInvoiceExtendedPMService.MapJsonToEntityPM(si);
            reconnectedSupplierInvoices.push(newSIWithSIItemMap);
        }
        this.EntityPM.SupplierInvoices = null;
        this.EntityPM.SupplierInvoices = new Array();
        for (var _b = 0, reconnectedSupplierInvoices_1 = reconnectedSupplierInvoices; _b < reconnectedSupplierInvoices_1.length; _b++) {
            var connectedSI = reconnectedSupplierInvoices_1[_b];
            connectedSI.IsDirty = false;
            this.EntityPM.SupplierInvoices.push(connectedSI);
            for (var _c = 0, _d = connectedSI.SupplierInvoiceItems; _c < _d.length; _c++) {
                var sii = _d[_c];
                //connectedSI.PropertyChanged.subscribe
                //sii.PropertyChanged.subscribe(s => { this.EntityPM.IsDirty = true; });// should do cleanup ?!?!?
            }
        }
    };
    DeclarationClassificationComponent.prototype.CheckRequrierdFieldsForSend = function () {
        var _this = this;
        var customsRequiredFieldListService = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(function (d) { return d.Name == 'Customs.Declaration'; })[0];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        customsRequiredFieldListService.getAllFromCache(filters).subscribe(function (response) {
            var requiredFields = response.Result;
            requiredFields.forEach(function (field) {
                var objectField = window.ObjectFields.filter(function (d) { return d.Id == field.ObjectfieldId; })[0];
                _this.UIProperties.SetWarning(objectField.FieldName, 'Customs.Declaration', true);
            });
        });
    };
    Object.defineProperty(DeclarationClassificationComponent.prototype, "DifferenceColor", {
        get: function () {
            if (this.SelectedTab == null)
                return null;
            return this.SelectedTab.Parent.AddEditSupplierInvoiceDUMMYManager.DifferenceColor;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "Difference", {
        get: function () {
            if (this.SelectedTab == null)
                return null;
            return this.SelectedTab.Parent.AddEditSupplierInvoiceDUMMYManager.Difference;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationClassificationComponent.prototype, "TotalForeignCurrency", {
        get: function () {
            if (this.SelectedTab == null)
                return null;
            return this.SelectedTab.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency;
        },
        enumerable: true,
        configurable: true
    });
    DeclarationClassificationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationClassificationComponent.html',
        })
        //
        ,
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], DeclarationClassificationComponent);
    return DeclarationClassificationComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationClassificationComponent = DeclarationClassificationComponent;
var AddEditSupplierInvoiceDUMMY = /** @class */ (function () {
    function AddEditSupplierInvoiceDUMMY(declarationCustomerCode) {
        this.declarationCustomerCode = declarationCustomerCode;
        this.IsNewEntity = false;
        //#region properties
        ///public ItemCode_LocalCache: ItemCodeComponent[]=[];
        this.DifferenceColor = "#282E30";
        this.difference = 0;
        this.totalForeignCurrency = 0;
        //#endregion
        this.GITITEMExtendedPMService = new GITITEMExtendedPMService_1.GITITEMExtendedPMService();
    }
    Object.defineProperty(AddEditSupplierInvoiceDUMMY.prototype, "Difference", {
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
    Object.defineProperty(AddEditSupplierInvoiceDUMMY.prototype, "TotalForeignCurrency", {
        get: function () { return this.totalForeignCurrency; },
        set: function (newValue) {
            if (this.totalForeignCurrency != newValue) {
                this.totalForeignCurrency = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return AddEditSupplierInvoiceDUMMY;
}());
exports.AddEditSupplierInvoiceDUMMY = AddEditSupplierInvoiceDUMMY;
//# sourceMappingURL=DeclarationClassificationComponent.js.map