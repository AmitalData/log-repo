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
var DeclarationGeneralComponent = /** @class */ (function (_super) {
    __extends(DeclarationGeneralComponent, _super);
    function DeclarationGeneralComponent(entityArgs, cd, EntityResourceService) {
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
        _this.ConsigmentTabs = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region XML Errors
        _this.XMLErrors = [];
        _this.IsWindowMode = false;
        _this.DrawMe = true;
        _this.isImporterClicked = false;
        _this.Type = null;
        //#endregion
        //#region Tabs Component code
        _this.consignmentIndex = 0;
        _this.consignmentNumber = 0;
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
                                            console.log("DeclarationGeneralComponent/EntityPM ", _this.EntityPM);
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
                                                _this.BuildConsignments();
                                                _this.checkImportersVisibility();
                                                _this.DisplayOnlyCheck();
                                                _this.CheckRequrierdFieldsForSend();
                                                _this.PreceduralFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
                                                _this.PreceduralFilterItems.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "boolean");
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
    // used in show XML errors process in Customs Answers
    DeclarationGeneralComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DeclarationError)) {
                console.log("Error", args.DeclarationError);
                this.entityArgs = args.entityArgs;
                this.EntityPM = args.entityPM;
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.ObjectTableName = args.entityArgs.ObjectTableName;
                this.IsWindowMode = true;
                // initialize consignment tabs
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM)) {
                    this.BuildConsignments(); // [!] in the pilot branch, you should enable this line to work!!
                    //this.ConsigmentTabs = [];
                    //// create consignment tabs from entity
                    //for (let item of this.EntityPM.Consignments) {
                    //    var tab;
                    //    tab = new LogTab();
                    //    tab.EntityPM = item;
                    //    tab.Parent = this.EntityPM;
                    //    tab.Code = item.SequenceNumeric.toString();
                    //    tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
                    //    tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/General/ConsigmentTabContent/ConsigmentTabContentComponent";
                    //    this.ConsigmentTabs.push(tab);
                    //}
                    this.checkImportersVisibility();
                    this.DisplayOnlyCheck();
                }
                this.ShowXMLErrors(args.DeclarationError);
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(args.AmendmentView)) {
                console.log("[DeclarationGeneral] Amendment", args.AmendmentView);
                this.entityArgs = args.entityArgs;
                this.EntityPM = args.entityPM;
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.ObjectTableName = args.entityArgs.ObjectTableName;
                this.IsWindowMode = true;
                // initialize consignment tabs
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM)) {
                    this.BuildConsignments();
                    //// create consignment tabs from entity
                    //for (let item of this.EntityPM.Consignments) {
                    //    var tab;
                    //    tab = new LogTab();
                    //    tab.EntityPM = item;
                    //    tab.Code = item.SequenceNumeric.toString();
                    //    tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
                    //    tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/General/ConsigmentTabContent/ConsigmentTabContentComponent";
                    //    this.ConsigmentTabs.push(tab);
                    //}
                    this.checkImportersVisibility();
                    this.DisplayOnlyCheck();
                }
                this.ShowXMLCorrections(args.AmendmentView);
            }
        }
    };
    DeclarationGeneralComponent.prototype.ShowXMLErrors = function (error) {
        if (error.EntityName.toLowerCase() == "declaration") {
            var currentError = error;
            if (!Tools_1.AppTool.IsNullOrEmpty(error.Field)) {
                this.UIProperties.SetValidity(error.Field, "Customs.Declaration", false, error.Description);
            }
            var errors = [];
            var xmlErrors = error.Description.split(/,|:/);
            for (var xmlError in xmlErrors) {
                errors.push(xmlErrors[xmlError]);
            }
            this.XMLErrors = errors;
        }
        else if (error.EntityName.toLowerCase() == "consignment") {
            for (var _i = 0, _a = this.ConsigmentTabs; _i < _a.length; _i++) {
                var tab = _a[_i];
                tab.DecErrors = error;
                currentError = error;
                if (!Tools_1.AppTool.IsNullOrEmpty(error.Field)) {
                    tab.EntityPM.UIProperties.SetValidity(error.Field, "Customs.Consignment", false, error.Description);
                }
                var errors = [];
                errors.push(error.Description);
            }
            this.XMLErrors = errors;
        }
    };
    DeclarationGeneralComponent.prototype.ShowXMLCorrections = function (amendment) {
        if (amendment.EntityName.toLowerCase() == "declaration") {
            if (!Tools_1.AppTool.IsNullOrEmpty(amendment.Field)) {
                this.UIProperties.SetValidity(amendment.Field, "Customs.Declaration", false, amendment.ErrorType);
            }
            var errors = [];
            errors.push(amendment.ErrorType);
            this.XMLErrors = errors;
        }
        else if (amendment.EntityName.toLowerCase() == "consignment") {
            for (var _i = 0, _a = this.ConsigmentTabs; _i < _a.length; _i++) {
                var tab = _a[_i];
                tab.DecErrors = amendment;
                if (!Tools_1.AppTool.IsNullOrEmpty(amendment.Field)) {
                    tab.EntityPM.UIProperties.SetValidity(amendment.Field, "Customs.Consignment", false, amendment.Description);
                }
                var errors = [];
                errors.push(amendment.Description);
            }
            var errors = [];
            errors.push(amendment.ErrorType);
            this.XMLErrors = errors;
        }
    };
    DeclarationGeneralComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    };
    DeclarationGeneralComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!this.IsDisplayOnly) {
            this.declarationPMService.update(this.EntityPM).subscribe(function (response) {
                var res = response.Result;
                if (response.HasError) {
                    _this.XMLErrors = [];
                    _this.XMLErrors = response.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    //#endregion
    DeclarationGeneralComponent.prototype.ngAfterViewInit = function () {
    };
    DeclarationGeneralComponent.prototype.checkImportersVisibility = function () {
        if (this.IsDisplayOnly)
            return;
        if (this.EntityPM.IsCourierDeclaration) {
            this.IsImporerCodeEnabled = true;
            //if (!AppTool.IsNullOrEmpty(this.ImporterCode))
            //    if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
            //        this.IsImporerCodeEnabled = false;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
        }
        else {
            this.IsImporerCodeEnabled = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterName) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterAddress);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode)) {
                this.IsImporerCodeEnabled = true;
                if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    this.IsImporerCodeEnabled = false;
                    //if (!AppTool.IsNullOrEmpty(this.ImporterCode))
                    //    if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    //        this.IsImporerCodeEnabled = false;
                    this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.TransferImporterCode))
            if (this.TransferImporterCode.includes("F") || this.TransferImporterCode.includes("P")) {
                this.IsTransferImporterEnabled = false;
            }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntitleImporterCode))
            if (this.EntitleImporterCode.includes("F") || this.EntitleImporterCode.includes("P"))
                this.IsEntitleImporterEnabled = false;
    };
    DeclarationGeneralComponent.prototype.ngOnDestroy = function () {
        console.log("DeclarationGeneralComponent:ngOnDestroy():ConsigmentTabs");
        this.ConsigmentTabs.forEach(function (tab) {
            if (tab.ComponentReference && tab.ComponentReference.ngOnDestroy) {
                tab.ComponentReference.ngOnDestroy();
            }
            tab.ComponentReference = null;
        });
        this.ConsigmentTabs = null;
    };
    DeclarationGeneralComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildConsignments();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess && _this.CurrentSession.CurrentEditComponent) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.RefreshDatePicker = false;
                    if (_this.timerToken) {
                        clearTimeout(_this.timerToken);
                    }
                    _this.timerToken = setTimeout(function () {
                        _this.RefreshDatePicker = true;
                    }, 200);
                    _this.BuildConsignments();
                    _this.DisplayOnlyCheck();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEGC") {
                        _this.CurrentSession.StartBusyIndicatorLoading();
                        //this.RefreshEntity();
                        _this.checkImportersVisibility();
                        _this.DisplayOnlyCheck();
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            }));
        }
    };
    DeclarationGeneralComponent.prototype.SetScreenFieldsEditability = function () {
        this.UIProperties.SetEnabled("DeclarationOfficeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeclarationDocumentTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TaxationDateTime", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AutonomyRegionTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeclarationDocumentId", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitleImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
        this.IsTransferImporterEnabled = !this.IsDisplayOnly;
        this.IsEntitleImporterEnabled = !this.IsDisplayOnly;
        this.checkImportersVisibility();
    };
    Object.defineProperty(DeclarationGeneralComponent.prototype, "DeclarationOfficeCode", {
        //#region Properties
        get: function () { return this.EntityPM.DeclarationOfficeCode; },
        set: function (newValue) {
            this.EntityPM.DeclarationOfficeCode = newValue;
            this.ChangeTransportMode();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "ProcedureCurrentCode", {
        get: function () { return this.EntityPM.ProcedureCurrentCode; },
        set: function (newValue) {
            this.EntityPM.ProcedureCurrentCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "DeclarationDocumentTypeCode", {
        get: function () { return this.EntityPM.DeclarationDocumentTypeCode; },
        set: function (newValue) { this.EntityPM.DeclarationDocumentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "TaxationDateTime", {
        get: function () { return this.EntityPM.TaxationDateTime; },
        set: function (newValue) {
            this.EntityPM.TaxationDateTime = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "AutonomyRegionTypeCode", {
        get: function () { return this.EntityPM.AutonomyRegionTypeCode; },
        set: function (newValue) { this.EntityPM.AutonomyRegionTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "DeclarationDocumentId", {
        get: function () { return this.EntityPM.DeclarationDocumentId; },
        set: function (newValue) { this.EntityPM.DeclarationDocumentId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "ImporterCode", {
        get: function () { return this.EntityPM.ImporterCode; },
        set: function (newValue) {
            if (this.EntityPM.ImporterCode != newValue) {
                this.EntityPM.ImporterCode = newValue;
                this.EntityPM.ImporterTypeCode = "1";
                this.EntityPM.ImporterTypeName = "IL";
                this.EntityPM.MainImporterEntitlemntTypeCode = null;
                this.EntityPM.ImporterAddress = null;
                this.EntityPM.ImporterPassportNumber = null;
                // this.EntityPM.ImporterName = null;
                this.EntityPM.ImporterPassCountryCode = null;
                if (!this.EntityPM.IsCourierDeclaration) {
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
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterName)) {
                this.CalculatedImporterName = this.EntityPM.ImporterName;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "CalculatedImporterName", {
        get: function () {
            if (this.EntityPM.ImporterCode == null && this.EntityPM.ImporterName != null)
                return this.EntityPM.ImporterName;
            else
                return this.EntityPM.CalculatedImporterName;
        },
        set: function (newValue) {
            this.EntityPM.CalculatedImporterName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "TransferImporterCode", {
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
    Object.defineProperty(DeclarationGeneralComponent.prototype, "CalculatedTransferImporterName", {
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
    Object.defineProperty(DeclarationGeneralComponent.prototype, "EntitleImporterCode", {
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
    Object.defineProperty(DeclarationGeneralComponent.prototype, "CalculatedEntitleImporterName", {
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
    Object.defineProperty(DeclarationGeneralComponent.prototype, "EntitleImporterCountryCode", {
        get: function () { return this.EntityPM.EntitleImporterCountryCode; },
        set: function (newValue) { this.EntityPM.EntitleImporterCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "ImporterPassCountryCode", {
        get: function () { return this.EntityPM.ImporterPassCountryCode; },
        set: function (newValue) { this.EntityPM.ImporterPassCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "EntitleImporterCountryName", {
        get: function () { return this.EntityPM.EntitleImporterCountryName; },
        set: function (newValue) { this.EntityPM.EntitleImporterCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationGeneralComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    DeclarationGeneralComponent.prototype.ImporterClicked = function (type, client) {
        if (client) {
            this.isImporterClicked = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            switch (type) {
                case 'Importer': {
                    this.ImporterCode = client.Code;
                    this.EntityPM.ImporterId = client.Id;
                    //this.CalculatedImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    if (Tools_1.AppTool.IsNullOrEmpty(client)) {
                        this.CalculatedImporterName = this.EntityPM.ImporterName;
                    }
                    else {
                        this.CalculatedImporterName = client.FullName;
                    }
                    break;
                }
                case 'Transfer': {
                    this.TransferImporterCode = client.Code;
                    this.EntityPM.TransferImporterId = client.Id;
                    this.CalculatedTransferImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    break;
                }
                case 'Entitle': {
                    this.EntitleImporterCode = client.Code;
                    this.EntityPM.EntitleImporterId = client.Id;
                    this.CalculatedEntitleImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    this.CalculatedClient = Tools_1.AppTool.IsNullOrEmpty(client) ? null : client;
                    this.EntitleImporterCountryCode = Tools_1.AppTool.IsNullOrEmpty(this.CalculatedClient) ? null : this.CalculatedClient.PassportCountryCode;
                    this.EntitleImporterCountryName = Tools_1.AppTool.IsNullOrEmpty(this.CalculatedClient) ? null : this.CalculatedClient.PassportCountryName;
                    break;
                }
            }
        }
    };
    DeclarationGeneralComponent.prototype.ImporterLostFocus = function (type, item, importerSearchBox) {
        var _this = this;
        if (this.isImporterClicked != true) {
            switch (type) {
                case 'Importer': {
                    this.EntityPM.ImporterId = "";
                    //this.CalculatedImporterName = "";
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
                        this.CalculatedImporterName = this.EntityPM.ImporterName;
                    }
                    break;
                }
                case 'Transfer': {
                    this.EntityPM.TransferImporterId = "";
                    this.CalculatedTransferImporterName = "";
                    break;
                }
                case 'Entitle': {
                    this.EntityPM.EntitleImporterId = "";
                    this.CalculatedEntitleImporterName = "";
                    this.CalculatedClient = null;
                    this.EntitleImporterCountryCode = null;
                    this.EntitleImporterCountryName = null;
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
                case 'Transfer': {
                    this.TransferImporterCode = item;
                    break;
                }
                case 'Entitle': {
                    this.EntitleImporterCode = item;
                    break;
                }
            }
        }
    };
    DeclarationGeneralComponent.prototype.ImporterTextChanged = function (type, item) {
        //if (AppTool.IsNullOrEmpty(item)) {
        switch (type) {
            case 'Importer': {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CalculatedImporterName)) {
                    this.PreviusImporterCode = this.ImporterCode;
                    this.PreviusCalculatedImporterName = this.CalculatedImporterName;
                }
                this.ImporterCode = item;
                this.EntityPM.ImporterId = null;
                if (this.EntityPM.ImporterName) {
                    this.CalculatedImporterName = this.EntityPM.ImporterName;
                }
                else
                    this.CalculatedImporterName = "";
                break;
            }
            case 'Transfer': {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CalculatedTransferImporterName)) {
                    this.PreviusTransferImporterCode = this.TransferImporterCode;
                    this.PreviusTransferCalculatedImporterName = this.CalculatedTransferImporterName;
                }
                this.TransferImporterCode = item;
                this.EntityPM.TransferImporterId = null;
                if (this.EntityPM.TransferImporterName) {
                    this.CalculatedTransferImporterName = this.EntityPM.TransferImporterName;
                }
                else
                    this.CalculatedTransferImporterName = "";
                break;
            }
            case 'Entitle': {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CalculatedEntitleImporterName)) {
                    this.PreviusEntitleImporterCode = this.EntitleImporterCode;
                    this.PreviusEntitleCalculatedImporterName = this.CalculatedEntitleImporterName;
                    this.PreviusEntitleImporterCountryCode = this.EntitleImporterCountryCode;
                    this.PreviusEntitleImporterCountryName = this.EntitleImporterCountryName;
                    this.PreviusCalculatedClient = this.CalculatedClient;
                }
                this.EntitleImporterCode = item;
                this.EntityPM.EntitleImporterId = null;
                this.EntitleImporterCountryCode = null;
                this.EntitleImporterCountryName = null;
                if (this.EntityPM.EntitleImporterName) {
                    this.CalculatedEntitleImporterName = this.EntityPM.EntitleImporterName;
                }
                else
                    this.CalculatedEntitleImporterName = "";
                break;
            }
        }
        //}
    };
    DeclarationGeneralComponent.prototype.EditImporter = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.Type = "Importer";
        this.Type = "Importer";
        logWindow.Width = 550;
        logWindow.Height = this.EntityPM.IsCourierDeclaration ? 550 : 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.SetFieldsDisabled($event); });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');
    };
    DeclarationGeneralComponent.prototype.SearchImporter = function (type, item) {
        var _this = this;
        if (this.IsDisplayOnly) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();
        var importerCode;
        var passportNumber;
        var passportTypeCode;
        var passportCountryCode;
        var isExternalId = true;
        var isPassport = false;
        switch (type) {
            case "Importer":
                importerCode = this.ImporterCode;
                if (!this.IsImporerCodeEnabled) {
                    importerCode = "";
                    isExternalId = false;
                    isPassport = true;
                    passportNumber = this.EntityPM.ImporterPassportNumber;
                    passportCountryCode = this.EntityPM.ImporterPassCountryCode;
                    if (this.ImporterCode.includes("P")) {
                        passportTypeCode = "1";
                    }
                    else if (this.ImporterCode.includes("F")) {
                        passportTypeCode = "2";
                    }
                    if (this.EntityPM.ImporterTypeCode == "P") {
                        passportTypeCode = "1";
                    }
                }
                break;
            case "Transfer":
                importerCode = this.TransferImporterCode;
                if (!this.IsTransferImporterEnabled) {
                    isExternalId = false;
                    isPassport = true;
                    importerCode = "";
                    passportNumber = this.EntityPM.TransferPassportNumber;
                    passportCountryCode = this.EntityPM.TransferImporterCountryCode;
                    if (this.TransferImporterCode.includes("P")) {
                        passportTypeCode = "1";
                    }
                    else if (this.TransferImporterCode.includes("F")) {
                        passportTypeCode = "2";
                    }
                }
                break;
            case "Entitle":
                importerCode = this.EntitleImporterCode;
                if (!this.IsEntitleImporterEnabled) {
                    isExternalId = false;
                    isPassport = true;
                }
                break;
        }
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.NewClient");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.Mode = "DeclarationGeneralComponent";
        windowArgs.ImporterCode = importerCode;
        windowArgs.IsExternalId = isExternalId;
        windowArgs.IsPassport = isPassport;
        windowArgs.PassportNumber = passportNumber;
        windowArgs.PassportTypeCode = passportTypeCode;
        windowArgs.PassportCountryCode = passportCountryCode;
        logWindow.Width = 850;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnCustomFilesScreenWindowClosed(type, $event); });
        logWindow.Show('./CustomsModules/CustomsClient/Components/NewClient/NewClientComponent');
    };
    DeclarationGeneralComponent.prototype.OnCustomFilesScreenWindowClosed = function (type, arg) {
        if (!Tools_1.AppTool.IsNullOrEmpty(arg)) {
            switch (type) {
                case 'Importer': {
                    this.CalculatedImporterName = arg;
                    break;
                }
                case 'Transfer': {
                    this.CalculatedTransferImporterName = arg;
                    break;
                }
                case 'Entitle': {
                    this.CalculatedEntitleImporterName = arg;
                    break;
                }
            }
        }
    };
    DeclarationGeneralComponent.prototype.EditTransferImporter = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.Type = "Transfer";
        this.Type = "Transfer";
        logWindow.Width = 550;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.SetFieldsDisabled($event); });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');
    };
    DeclarationGeneralComponent.prototype.EditEntitleImporter = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.Type = "Entitle";
        this.Type = "Entitle";
        logWindow.Width = 550;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.SetFieldsDisabled($event); });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');
    };
    DeclarationGeneralComponent.prototype.SetFieldsDisabled = function (message) {
        if (message == "ok") {
            if (this.Type == "Importer") {
                this.IsImporerCodeEnabled = false;
            }
            else if (this.Type == "Transfer") {
                this.IsTransferImporterEnabled = false;
            }
            else if (this.Type == "Entitle") {
                this.IsEntitleImporterEnabled = false;
            }
        }
        else if (message == "!ok") {
            if (this.Type == "Importer") {
                this.IsImporerCodeEnabled = true;
            }
            else if (this.Type == "Transfer") {
                this.IsTransferImporterEnabled = true;
            }
            else if (this.Type == "Entitle") {
                this.IsEntitleImporterEnabled = true;
            }
        }
        this.checkImportersVisibility();
    };
    DeclarationGeneralComponent.prototype.AddConsigment = function (event) {
        if (this.IsDisplayOnly) {
            return;
        }
        //console.log("==>> add clicked");
        this.consignmentIndex = 0;
        this.consignmentNumber = 0;
        if (this.EntityPM.Consignments.length > 0) {
            var maxObj = this.EntityPM.Consignments.reduce(function (prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current; });
            if (maxObj != null) {
                if (this.consignmentIndex <= maxObj.SequenceNumeric)
                    this.consignmentIndex = maxObj.SequenceNumeric;
            }
            var maxObj = this.EntityPM.Consignments.reduce(function (prev, current) { return (prev.ConsignmentNumber > current.ConsignmentNumber) ? prev : current; });
            if (maxObj != null) {
                if (this.consignmentNumber <= maxObj.ConsignmentNumber)
                    this.consignmentNumber = maxObj.ConsignmentNumber;
            }
        }
        // new consignment
        var consignment = new ConsignmentPM_1.ConsignmentPM(this.EntityPM);
        consignment.DeclarationId = this.EntityPM.Id;
        consignment.Tenant = SessionLocator_1.SessionLocator.Tenant;
        consignment.IsLastReleaseFromWarehous = "F";
        consignment.SequenceNumeric = ++this.consignmentIndex;
        consignment.ConsignmentNumber = ++this.consignmentNumber;
        this.EntityPM.AddConsignment(consignment);
        // new tab
        var tab = new LogTabsComponent_1.LogTab();
        tab.EntityPM = consignment;
        tab.Parent = this.EntityPM;
        tab.Code = consignment.SequenceNumeric.toString();
        tab.Header = consignment.SequenceNumeric.toString();
        tab.ComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ConsigmentTabContentComponent";
        this.ConsigmentTabs.push(tab);
        // select the tab
        this.SelectedTab = tab;
        DeclarationEventManager_1.DeclarationEventManager.ConsignmentsChanged.emit({});
    };
    DeclarationGeneralComponent.prototype.DeleteConsigment = function (tab) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteConsignment");
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 300;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(msg);
            var t = tab;
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) { // YES
                    tab = t;
                    var index = _this.ConsigmentTabs.indexOf(tab);
                    if (index < 0) {
                        console.log("The tab was not found, could not delete it :( ", tab);
                        return;
                    }
                    _this.EntityPM.RemoveConsignment(tab.EntityPM);
                    _this.ConsigmentTabs.splice(index, 1);
                    //resequence consignments
                    for (var i = 0; i < _this.EntityPM.Consignments.length; i++) {
                        var consignment = _this.EntityPM.Consignments[i];
                        consignment.SequenceNumeric = i + 1;
                        //consignment.ConsignmentNumber = i + 1;
                    }
                    for (var i = 0; i < _this.ConsigmentTabs.length; i++) {
                        var consignment = _this.ConsigmentTabs[i].EntityPM;
                        consignment.SequenceNumeric = i + 1;
                        _this.ConsigmentTabs[i].Code = consignment.SequenceNumeric.toString();
                        _this.ConsigmentTabs[i].Header = (consignment.ManifestNumber ? (consignment.ManifestNumber + '-') : '') + consignment.SequenceNumeric;
                    }
                    // select the last tab
                    var tab = _this.ConsigmentTabs[0];
                    _this.SelectedTab = tab;
                    DeclarationEventManager_1.DeclarationEventManager.ConsignmentsChanged.emit({});
                }
            });
        }
    };
    DeclarationGeneralComponent.prototype.OnSelectedChanged = function (tab) {
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
            console.log("Tab selected: ", tab);
        }
    };
    //#endregion
    DeclarationGeneralComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationGeneralComponent.prototype.ChangeTransportMode = function () {
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
    DeclarationGeneralComponent.prototype.DisplayOnlyCheck = function () {
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
    DeclarationGeneralComponent.prototype.BuildConsignments = function () {
        this.ConsigmentTabs = [];
        for (var _i = 0, _a = this.EntityPM.Consignments; _i < _a.length; _i++) {
            var item = _a[_i];
            var tab = new LogTabsComponent_1.LogTab();
            tab.EntityPM = item;
            tab.Parent = this.EntityPM;
            tab.Code = item.SequenceNumeric.toString();
            tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
            tab.ComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ConsigmentTabContentComponent";
            this.ConsigmentTabs.push(tab);
        }
        if (this.ConsigmentTabs.length > 0) {
            this.SelectedTab = this.ConsigmentTabs[0];
        }
    };
    DeclarationGeneralComponent.prototype.CheckRequrierdFieldsForSend = function () {
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
    DeclarationGeneralComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationGeneralComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], DeclarationGeneralComponent);
    return DeclarationGeneralComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationGeneralComponent = DeclarationGeneralComponent;
//# sourceMappingURL=DeclarationGeneralComponent.js.map