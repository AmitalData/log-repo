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
var Tools_1 = require("../../../../Shipment/Tools");
var Tools_2 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ShipmentValidator_1 = require("../../../../Shipment/Validators/ShipmentValidator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var ShipmentPickUpDeliveryPackagePM_1 = require("../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var PickUpDeliveryPackageHarmonizePM_1 = require("../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var AddEditDeliveryComponent = /** @class */ (function () {
    function AddEditDeliveryComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ObjectTableName = "ShipmentPickUpDelivery";
        this.IsNewEntity = false;
        this.TabsItemsSource = [];
        this.IsResourcesReady = false;
        this.ValidationErrorsList = [];
        this.IsShowNewWarehouseReleaseButton = false;
        this.IsContainerFollowup = false;
        this.ContainerReturnDeliveryId = null;
        this.IsCreatingContainerDelivery = false;
        this.IsShipmentEditComponent = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.isViewInited = false;
        this.Retries = 0;
        this.PageChild_MAIN = null;
        this.PageChild_PACG = null;
        this.PageChild_DCSO = null;
        this.PageChild_DCSI = null;
        this.oldFollowups = [];
        this.isEntityAdded = false;
        this.oldPackages = [];
        this.myCardListService = new CardListService_1.CardListService();
    }
    AddEditDeliveryComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.IsNewEntity = args['IsNewEntity'];
        this.ShipmentPM = args['ShipmentPM'];
        this.EntityPM = args['EntityPM'];
        this.IsContainerFollowup = args['IsContainerFollowup'];
        this.ContainerReturnDeliveryId = args['ContainerReturnDeliveryId'];
        this.WareHouseRelaseCustomerId = args['WareHouseRelaseCustomerId'];
        this.WareHouseRelaseWareHouseId = args['WareHouseRelaseWareHouseId'];
        this.IsCreatingContainerDelivery = args["IsCreatingContainerDelivery"];
        var isOutSource = args['IsOutSource'];
        if (isOutSource)
            this.IsShipmentEditComponent = false;
        if (!this.EntityPM.TransportModeCode) {
            this.EntityPM.TransportModeCode = "BYTR";
        }
        this.Clone();
        if (this.ShipmentPM) {
            if (this.ShipmentPM.ShipmentLevelCode == "D" || this.ShipmentPM.ShipmentLevelCode == "H") {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "Module")) {
                    this.IsShowNewWarehouseReleaseButton = this.ShipmentPM.DirectionId == "I" ? true : false;
                }
            }
        }
        this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe(function (res) {
            _this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDeliveryPackage").subscribe(function (res2) {
                _this.IsResourcesReady = true;
                _this.BuildTabs();
                _this.RunComponent();
            });
        });
    };
    AddEditDeliveryComponent.prototype.ngOnDestroy = function () {
        Tools_2.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        this.SaveCompletedEvent = null;
    };
    AddEditDeliveryComponent.prototype.BuildTabs = function () {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("MAIN", "ShipmentPickUpDelivery.TH.Main"));
        this.TabsItemsSource.push(new TabItem("PACG", "ShipmentPickUpDelivery.TH.Packages"));
        this.TabsItemsSource.push(new TabItem("DCSO", "ShipmentPickUpDelivery.TH.DocsOut", this.IsNewEntity));
        this.TabsItemsSource.push(new TabItem("DCSI", "ShipmentPickUpDelivery.TH.DocsIn", this.IsNewEntity));
        this.selectedTabCode = "MAIN";
    };
    AddEditDeliveryComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
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
    AddEditDeliveryComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AddEditDeliveryComponent.prototype.InitializeComponent = function () {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    };
    AddEditDeliveryComponent.prototype.NewWarehouseReleaseButtonClicked = function () {
        var _this = this;
        if (this.ShipmentPM.IsDirty) {
            var errors = [];
            var myShipmentValidator = new ShipmentValidator_1.ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors.push("This shipment has validation errors you cant proceed adding cross dock release!");
            }
            if (errors.length == 0) {
                if (this.CurrentSession.CurrentEditComponent != null && this.IsShipmentEditComponent) {
                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                            if (isSaveSuccess) {
                                _this.ShipmentPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                                _this.Clone();
                                _this.InitializeWareHousReleaseWindow();
                            }
                            else {
                                _this.ValidationErrorsList = _this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                            }
                            Tools_2.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                            _this.SaveCompletedEvent = null;
                        });
                    }
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                }
                else if (this.IsContainerFollowup || !this.IsShipmentEditComponent) {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    var entityPMService = new ShipmentPMService_1.ShipmentPMService();
                    entityPMService.update(this.ShipmentPM).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.ShipmentPM = myResponse.Result;
                            _this.Clone();
                            _this.InitializeWareHousReleaseWindow();
                        }
                    });
                }
                else {
                    this.InitializeWareHousReleaseWindow();
                }
            }
            else {
                this.ValidationErrorsList = errors;
            }
        }
        else
            this.InitializeWareHousReleaseWindow();
    };
    AddEditDeliveryComponent.prototype.InitializeWareHousReleaseWindow = function () {
        var _this = this;
        if (this.EntityPM.PickUpDeliveryFromTypeCode == "PART" && !Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerCardId)) {
            this.myCardListService.getSingle(this.EntityPM.FromPartnerCardId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var myCardList = myResponse.Result;
                    if (myCardList && myCardList.PartnerTypeId == "WH") {
                        _this.OpenWareHousReleaseWindow(myCardList.Id);
                    }
                    else
                        _this.OpenWareHousReleaseWindow("");
                }
                else
                    _this.OpenWareHousReleaseWindow("");
            });
        }
        else {
            this.OpenWareHousReleaseWindow("");
        }
    };
    AddEditDeliveryComponent.prototype.OpenWareHousReleaseWindow = function (warehouseId) {
        var windowArgs = {};
        windowArgs.WarehouseId = warehouseId;
        windowArgs.ExpectedReleaseDate = this.EntityPM.ETA;
        windowArgs.ActualReleaseDate = this.EntityPM.ATA;
        windowArgs.ShipmentPM = this.ShipmentPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = "New Cross Dock Release";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/NewWarehouseReleaseComponent");
    };
    Object.defineProperty(AddEditDeliveryComponent.prototype, "SelectedTabCode", {
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
    AddEditDeliveryComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation_1 != null) {
                switch (this.SelectedTabCode) {
                    case "MAIN": {
                        if (this.PageChild_MAIN == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryMainTabComponent', myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.PageChild_MAIN = cmpRef.instance;
                                _this.PageChild_MAIN.InitTab(_this.EntityPM, _this.ShipmentPM);
                                if (_this.IsNewEntity) {
                                    if (_this.EntityPM.PickUpDeliveryTypeCode == "EMPT") {
                                        _this.PageChild_MAIN.FromTypeCode = "PART";
                                        _this.PageChild_MAIN.ToTypeCode = "PART";
                                        if (!Tools_2.AppTool.IsNullOrEmpty(_this.ContainerReturnDeliveryId)) {
                                            var myDelivery = _this.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.Id == _this.ContainerReturnDeliveryId; })[0];
                                        }
                                        if (myDelivery) {
                                            _this.PageChild_MAIN.FromTypeCode = myDelivery.PickUpDeliveryToTypeCode;
                                            switch (myDelivery.PickUpDeliveryToTypeCode) {
                                                case "PART": {
                                                    _this.PageChild_MAIN.EntityPM.FromPartnerCardId = myDelivery.ToPartnerCardId;
                                                    _this.PageChild_MAIN.SetUIProperties_From();
                                                    _this.PageChild_MAIN.FromAddressId = myDelivery.ToAddressId;
                                                    break;
                                                }
                                                case "PORT": {
                                                    _this.PageChild_MAIN.FromPortId = myDelivery.ToPortId;
                                                    break;
                                                }
                                                default: {
                                                    _this.PageChild_MAIN.FromAddressCity = myDelivery.ToAddressCity;
                                                    _this.PageChild_MAIN.FromAddressZipCode = myDelivery.ToAddressZipCode;
                                                    _this.PageChild_MAIN.FromAddressCountryId = myDelivery.ToAddressCountryId;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        if (!Tools_2.AppTool.IsNullOrEmpty(_this.WareHouseRelaseWareHouseId)) {
                                            _this.PageChild_MAIN.FromTypeCode = "PART";
                                            _this.PageChild_MAIN.FromPartnerCardId = _this.WareHouseRelaseWareHouseId;
                                        }
                                        // Task 47686: Export& Domestic Terminal: Delivery From
                                        //else if (!AppTool.IsNullOrEmpty(this.ShipmentPM.WarehouseLegWarehouseId)) {
                                        //    this.PageChild_MAIN.FromTypeCode = "PART";
                                        //    this.PageChild_MAIN.FromPartnerCardId = this.ShipmentPM.WarehouseLegWarehouseId;
                                        //}
                                        else {
                                            var fromPortId = _this.ShipmentPM.MainCarriageToPortId;
                                            if (!Tools_2.AppTool.IsNullOrEmpty(_this.ShipmentPM.OnCarriageToPortId)) {
                                                fromPortId = _this.ShipmentPM.OnCarriageToPortId;
                                            }
                                            else if (!Tools_2.AppTool.IsNullOrEmpty(_this.ShipmentPM.Transshipment3ToPortId)) {
                                                fromPortId = _this.ShipmentPM.Transshipment3ToPortId;
                                            }
                                            else if (!Tools_2.AppTool.IsNullOrEmpty(_this.ShipmentPM.Transshipment2ToPortId)) {
                                                fromPortId = _this.ShipmentPM.Transshipment2ToPortId;
                                            }
                                            else if (!Tools_2.AppTool.IsNullOrEmpty(_this.ShipmentPM.Transshipment1ToPortId)) {
                                                fromPortId = _this.ShipmentPM.Transshipment1ToPortId;
                                            }
                                            _this.PageChild_MAIN.FromTypeCode = "PORT";
                                            _this.PageChild_MAIN.FromPortId = fromPortId;
                                        }
                                        _this.PageChild_MAIN.ToTypeCode = "PART";
                                        _this.PageChild_MAIN.ToPartnerCardId = !Tools_2.AppTool.IsNullOrEmpty(_this.WareHouseRelaseCustomerId) ? _this.WareHouseRelaseCustomerId : _this.ShipmentPM.ConsigneeId;
                                    }
                                }
                                else {
                                    _this.PageChild_MAIN.OnEditMoodScreen();
                                }
                            });
                        }
                        break;
                    }
                    case "PACG": {
                        if (this.PageChild_PACG == null) {
                            this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDeliveryPackage").subscribe(function (response) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesTabComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_PACG = cmpRef.instance;
                                    _this.PageChild_PACG.InitTab(_this.EntityPM, _this.ShipmentPM, _this);
                                });
                            });
                        }
                        break;
                    }
                    case "DCSO": {
                        if (this.PageChild_DCSO == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryDocsOutTabComponent', myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.PageChild_DCSO = cmpRef.instance;
                                _this.PageChild_DCSO.InitTab(_this.EntityPM, _this.ShipmentPM);
                            });
                        }
                        break;
                    }
                    case "DCSI": {
                        if (this.PageChild_DCSI == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryDocsInTabComponent', myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.PageChild_DCSI = cmpRef.instance;
                                _this.PageChild_DCSI.InitTab(_this.EntityPM, _this.ShipmentPM);
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    AddEditDeliveryComponent.prototype.CloseClicked = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", "Delivery"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.Save(true);
                }
                else if (confirmWindow.No) {
                    _this.CloseWindow();
                }
            });
        }
        else {
            this.CloseWindow();
        }
    };
    AddEditDeliveryComponent.prototype.CloseWindow = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditDeliveryComponent.prototype.OkButtonClicked = function () {
        this.Save(false);
    };
    AddEditDeliveryComponent.prototype.Save = function (isClosingWindow) {
        var _this = this;
        var isValid = this.Validate();
        if (isValid) {
            var SavedEntityId = this.EntityPM.Id;
            var SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;
            if (this.IsNewEntity) {
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Container F/U", "Added Delivery");
                this.ShipmentPM.AddDelivery(this.EntityPM);
                this.isEntityAdded = true;
            }
            if (this.CurrentSession.CurrentEditComponent != null && this.IsShipmentEditComponent) {
                if (!this.SaveCompletedEvent) {
                    this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                        _this.OnSaveCompleted(isSaveSuccess, isClosingWindow, SavedEntityId, SavedEntityNumber);
                    });
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                }
            }
            else {
                if (this.IsContainerFollowup || !this.IsShipmentEditComponent) {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    var entityPMService = new ShipmentPMService_1.ShipmentPMService();
                    entityPMService.update(this.ShipmentPM).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindowEmit(myResponse.Result);
                        }
                    });
                }
            }
        }
    };
    AddEditDeliveryComponent.prototype.Validate = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        // From
        switch (this.EntityPM.PickUpDeliveryFromTypeCode) {
            case "PART": {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerCardId)) {
                    errors.push(msg.replace("%FieldName", "From Partner"));
                }
                break;
            }
            case "PORT": {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.FromPortId)) {
                    errors.push(msg.replace("%FieldName", "From Port"));
                }
                break;
            }
            case "CASL": {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.FromAddressCity) && Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.FromAddressZipCode)) {
                    errors.push("From City or from Zip Code is required");
                }
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.FromAddressCountryId)) {
                    errors.push(msg.replace("%FieldName", "From Country"));
                }
                break;
            }
        }
        // To
        switch (this.EntityPM.PickUpDeliveryToTypeCode) {
            case "PART": {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerCardId)) {
                    errors.push(msg.replace("%FieldName", "To Partner"));
                }
                break;
            }
            case "PORT": {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ToPortId)) {
                    errors.push(msg.replace("%FieldName", "To Port"));
                }
                break;
            }
            case "CASL": {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ToAddressCity) && Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ToAddressZipCode)) {
                    errors.push("To City or to Zip Code is required");
                }
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ToAddressCountryId)) {
                    errors.push(msg.replace("%FieldName", "To Country"));
                }
                break;
            }
        }
        // Actual Dates
        if (!Tools_2.DateTool.IsActualDateValid(this.EntityPM.ATD)) {
            errors.push(Tools_2.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATD")));
        }
        if (!Tools_2.DateTool.IsActualDateValid(this.EntityPM.ATA)) {
            errors.push(Tools_2.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATA")));
        }
        // Series Dates
        var ETD = Tools_2.DateTool.GetDateParts(this.EntityPM.ETD).DateTicks;
        var ETA = Tools_2.DateTool.GetDateParts(this.EntityPM.ETA).DateTicks;
        var ATD = Tools_2.DateTool.GetDateParts(this.EntityPM.ATD).DateTicks;
        var ATA = Tools_2.DateTool.GetDateParts(this.EntityPM.ATA).DateTicks;
        var isMainCarriageExists = true;
        var MainCarriageETD = Tools_2.DateTool.GetDateParts(this.ShipmentPM.MainCarriageETD).DateTicks;
        var MainCarriageETA = Tools_2.DateTool.GetDateParts(this.ShipmentPM.MainCarriageETA).DateTicks;
        var MainCarriageATD = Tools_2.DateTool.GetDateParts(this.ShipmentPM.MainCarriageATD).DateTicks;
        var MainCarriageATA = Tools_2.DateTool.GetDateParts(this.ShipmentPM.MainCarriageATA).DateTicks;
        var isTransshipment1Exists = (this.ShipmentPM.Transshipment1FromPortId != null && this.ShipmentPM.Transshipment1ToPortId != null) ? true : false;
        var Transshipment1ETD = isTransshipment1Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment1ETD).DateTicks : 0;
        var Transshipment1ETA = isTransshipment1Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment1ETA).DateTicks : 0;
        var Transshipment1ATD = isTransshipment1Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment1ATD).DateTicks : 0;
        var Transshipment1ATA = isTransshipment1Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment1ATA).DateTicks : 0;
        var isTransshipment2Exists = (this.ShipmentPM.Transshipment2FromPortId != null && this.ShipmentPM.Transshipment2ToPortId != null) ? true : false;
        var Transshipment2ETD = isTransshipment2Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment2ETD).DateTicks : 0;
        var Transshipment2ETA = isTransshipment2Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment2ETA).DateTicks : 0;
        var Transshipment2ATD = isTransshipment2Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment2ATD).DateTicks : 0;
        var Transshipment2ATA = isTransshipment2Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment2ATA).DateTicks : 0;
        var isTransshipment3Exists = (this.ShipmentPM.Transshipment3FromPortId != null && this.ShipmentPM.Transshipment3ToPortId != null) ? true : false;
        var Transshipment3ETD = isTransshipment3Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment3ETD).DateTicks : 0;
        var Transshipment3ETA = isTransshipment3Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment3ETA).DateTicks : 0;
        var Transshipment3ATD = isTransshipment3Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment3ATD).DateTicks : 0;
        var Transshipment3ATA = isTransshipment3Exists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.Transshipment3ATA).DateTicks : 0;
        var isOnCarriageExists = (this.ShipmentPM.OnCarriageFromPortId != null && this.ShipmentPM.OnCarriageToPortId != null) ? true : false;
        var OnCarriageETD = isOnCarriageExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.OnCarriageETD).DateTicks : 0;
        var OnCarriageETA = isOnCarriageExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.OnCarriageETA).DateTicks : 0;
        var OnCarriageATD = isOnCarriageExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.OnCarriageATD).DateTicks : 0;
        var OnCarriageATA = isOnCarriageExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.OnCarriageATA).DateTicks : 0;
        var isWarehouseLegExists = (this.ShipmentPM.WarehouseLegWarehouseId != null && this.ShipmentPM.DirectionId == "I") ? true : false;
        var WarehouseLegEED = isWarehouseLegExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.WarehouseLegExpectedEntryDate).DateTicks : 0;
        var WarehouseLegERD = isWarehouseLegExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.WarehouseLegExpectedReleaseDate).DateTicks : 0;
        var WarehouseLegAED = isWarehouseLegExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.WarehouseLegActualEntryDate).DateTicks : 0;
        var WarehouseLegARD = isWarehouseLegExists ? Tools_2.DateTool.GetDateParts(this.ShipmentPM.WarehouseLegActualReleaseDate).DateTicks : 0;
        // Self
        if (!Tools_1.RoutingHelper.IsRoutingLegDatesValid(ETD, ETA)) {
            errors.push("Expected departure must be less than Expected arrival");
        }
        if (!Tools_1.RoutingHelper.IsRoutingLegDatesValid(ATD, ATA)) {
            errors.push("Actual departure must be less than Actual arrival");
        }
        //if (RoutingHelper.CompairDateSeries(ETD, ETA, ">")) {
        //    errors.push("Expected departure must be less than Expected arrival");
        //}
        //if (RoutingHelper.CompairDateSeries(ATD, ATA, ">")) {
        //    errors.push("Actual departure must be less than Actual arrival");
        //}
        // Previous
        if (isWarehouseLegExists) {
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ETD, WarehouseLegERD)) {
                errors.push("Delivery expected departure must be bigger than Warehouse expected release");
            }
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ATD, WarehouseLegARD)) {
                errors.push("Delivery actual departure must be bigger than Warehouse actual release");
            }
        }
        else if (isOnCarriageExists) {
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ETD, OnCarriageETA)) {
                errors.push("Expected departure must be bigger than On-Carriage expected arrival");
            }
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ATD, OnCarriageATA)) {
                errors.push("Actual departure must be bigger than On-Carriage actual arrival");
            }
        }
        else if (isTransshipment3Exists) {
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ETD, Transshipment3ETA)) {
                errors.push("Expected departure must be bigger than Transshipment3 expected arrival");
            }
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ATD, Transshipment3ATA)) {
                errors.push("Actual departure must be bigger than Transshipment3 actual arrival");
            }
        }
        else if (isTransshipment2Exists) {
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ETD, Transshipment2ETA)) {
                errors.push("Expected departure must be bigger than Transshipment2 expected arrival");
            }
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ATD, Transshipment2ATA)) {
                errors.push("Actual departure must be bigger than Transshipment2 actual arrival");
            }
        }
        else if (isTransshipment1Exists) {
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ETD, Transshipment1ETA)) {
                errors.push("Expected departure must be bigger than Transshipment1 expected arrival");
            }
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ATD, Transshipment1ATA)) {
                errors.push("Actual departure must be bigger than Transshipment1 actual arrival");
            }
        }
        else {
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ETD, MainCarriageETA)) {
                errors.push("Expected departure must be bigger than Main-Carriage expected arrival");
            }
            if (Tools_1.RoutingHelper.IsDateSeriesSmaller(ATD, MainCarriageATA)) {
                errors.push("Actual departure must be bigger than Main-Carriage actual arrival");
            }
        }
        if (errors.length == 0) {
            var myShipmentValidator = new ShipmentValidator_1.ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.Routings.CantProceedAddingDelivery"));
            }
        }
        this.ValidationErrorsList = errors;
        var isValid = errors.length == 0 ? true : false;
        return isValid;
    };
    AddEditDeliveryComponent.prototype.OnSaveCompleted = function (isSaveSuccess, isClosingWindow, SavedEntityId, SavedEntityNumber) {
        if (isSaveSuccess) {
            if (this.IsNewEntity) {
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", "DeliveryOpen");
                this.IsNewEntity = false;
                this.TabsItemsSource.forEach(function (item) {
                    item.IsDisabled = false;
                });
            }
            if (isClosingWindow) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                this.ShipmentPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                if (SavedEntityId) {
                    this.EntityPM = this.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.Id == SavedEntityId; })[0];
                }
                else if (SavedEntityNumber) {
                    this.EntityPM = this.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.PickUpDeliveryNumber == SavedEntityNumber; })[0];
                }
                if (this.PageChild_MAIN) {
                    this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);
                }
                if (this.PageChild_PACG) {
                    this.PageChild_PACG.InitTab(this.EntityPM, this.ShipmentPM, this);
                }
                if (this.PageChild_DCSO) {
                    this.PageChild_DCSO.InitTab(this.EntityPM, this.ShipmentPM);
                }
                if (this.PageChild_DCSI) {
                    this.PageChild_DCSI.InitTab(this.EntityPM, this.ShipmentPM);
                }
                this.Clone();
            }
        }
        else {
            this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
        }
        Tools_2.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        this.SaveCompletedEvent = null;
    };
    AddEditDeliveryComponent.prototype.Clone = function () {
        var _this = this;
        this.ClonePackages();
        this.ShipmentPM.FollowUps.forEach(function (item) {
            var oldItem = new ShipmentFollowUpPM_1.ShipmentFollowUpPM(null);
            oldItem.Id = item.Id;
            oldItem.Date = item.Date;
            oldItem.Deleted = item.Deleted;
            oldItem.Done = item.Done;
            oldItem.DoneDateTime = item.DoneDateTime;
            oldItem.DoneNote = item.DoneNote;
            oldItem.EntityDateId = item.EntityDateId;
            oldItem.EventTypeFollowUpName = item.EventTypeFollowUpName;
            oldItem.EventTypeId = item.EventTypeId;
            oldItem.Tenant = item.Tenant;
            oldItem.ExternalDocumentId = item.ExternalDocumentId;
            oldItem.IsNew = item.IsNew;
            oldItem.JobId = item.JobId;
            oldItem.LegType = item.LegType;
            oldItem.ManualActivatedFollowUp = item.ManualActivatedFollowUp;
            oldItem.Note = item.Note;
            oldItem.OwnerUserId = item.OwnerUserId;
            oldItem.OwnerUserName = item.OwnerUserName;
            oldItem.ShipmentId = item.ShipmentId;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            _this.oldFollowups.push(oldItem);
        });
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('FullResponsibility');
        this.myCloner.AddField('FromTypeCode');
        this.myCloner.AddField('PickUpDeliveryFromTypeCode');
        this.myCloner.AddField('FromPartnerCardId');
        this.myCloner.AddField('FromPortId');
        this.myCloner.AddField('FromPortCode');
        this.myCloner.AddField('FromPortName');
        this.myCloner.AddField('FromPortCountryCode');
        this.myCloner.AddField('FromPortCountryName');
        this.myCloner.AddField('FromAddressId');
        this.myCloner.AddField('FromAddressCity');
        this.myCloner.AddField('FromAddressCity_Dummy');
        this.myCloner.AddField('FromAddressZipCode');
        this.myCloner.AddField('FromAddressCountryId');
        this.myCloner.AddField('FromAddressCountryCode');
        this.myCloner.AddField('FromAddressCountryName');
        this.myCloner.AddField('FromAddress');
        this.myCloner.AddField('ToTypeCode');
        this.myCloner.AddField('PickUpDeliveryToTypeCode');
        this.myCloner.AddField('ToPartnerCardId');
        this.myCloner.AddField('ToPortId');
        this.myCloner.AddField('ToPortCode');
        this.myCloner.AddField('ToPortName');
        this.myCloner.AddField('ToPortCountryCode');
        this.myCloner.AddField('ToPortCountryName');
        this.myCloner.AddField('ToAddressId');
        this.myCloner.AddField('ToAddressCity');
        this.myCloner.AddField('ToAddressCity_Dummy');
        this.myCloner.AddField('ToAddressZipCode');
        this.myCloner.AddField('ToAddressCountryId');
        this.myCloner.AddField('ToAddressCountryCode');
        this.myCloner.AddField('ToAddressCountryName');
        this.myCloner.AddField('ToAddress');
        this.myCloner.AddField('CarrierId');
        this.myCloner.AddField('CarrierCode');
        this.myCloner.AddField('CarrierName');
        this.myCloner.AddField('CarrierWebSite');
        this.myCloner.AddField('CarrierNumber');
        this.myCloner.AddField('Driver');
        this.myCloner.AddField('TruckNumber');
        this.myCloner.AddField('TrailerNumber');
        this.myCloner.AddField('TransportModeCode');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('EmptyDeliveryContainerPartnerId');
        this.myCloner.AddField('EmptyDeliveryDepotReference');
        this.myCloner.AddField('ETD');
        this.myCloner.AddField('ETA');
        this.myCloner.AddField('ATD');
        this.myCloner.AddField('ATA');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.ShipmentPM);
    };
    AddEditDeliveryComponent.prototype.RejectChanges = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            this.RejectPackages();
            var addedItems = [];
            var removedItems = [];
            this.oldFollowups.forEach(function (item) {
                var existingItem = _this.ShipmentPM.FollowUps.filter(function (f) { return f.LegType == item.LegType; })[0];
                if (!existingItem) {
                    removedItems.push(item);
                }
            });
            this.ShipmentPM.FollowUps.forEach(function (item) {
                var oldItem = _this.oldFollowups.filter(function (f) { return f.LegType == item.LegType; })[0];
                if (oldItem == null) {
                    addedItems.push(item);
                }
            });
            if (addedItems.length > 0 || removedItems.length > 0) {
                addedItems.forEach(function (item) {
                    _this.ShipmentPM.RemoveShipmentFollowUp(item);
                });
                removedItems.forEach(function (item) {
                    _this.ShipmentPM.AddShipmentFollowUp(item);
                });
                this.CurrentSession.FireEvent("FollowupsChanged");
            }
            if (this.isEntityAdded) {
                if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.ShipmentPM.RemoveDelivery(this.EntityPM);
                }
            }
            this.myCloner.RejectChanges();
        }
    };
    AddEditDeliveryComponent.prototype.ClonePackages = function () {
        var _this = this;
        var PackageFields = [];
        PackageFields.push("Id");
        PackageFields.push("Tenant");
        PackageFields.push("ContainerNumber");
        PackageFields.push("PackageTypeId");
        PackageFields.push("PackageTypeName");
        PackageFields.push("PackageTypeTEU");
        PackageFields.push("ShipmentPickUpDeliveryId");
        PackageFields.push("Quantity");
        PackageFields.push("Volume");
        PackageFields.push("Weight");
        PackageFields.push("Description");
        PackageFields.push("Harmonize");
        PackageFields.push("ShipperSeal");
        PackageFields.push("Width");
        PackageFields.push("Height");
        PackageFields.push("Length");
        PackageFields.push("OriginalShipmentPackageId");
        PackageFields.push("IsMultiHarmonize");
        PackageFields.push("IsDirty");
        PackageFields.push("ChangeSetOp");
        PackageFields.push("EntityParentPM");
        var PackageHarmonizeFields = [];
        PackageHarmonizeFields.push("Id");
        PackageHarmonizeFields.push("Tenant");
        PackageHarmonizeFields.push("PackageId");
        PackageHarmonizeFields.push("Harmonize");
        PackageHarmonizeFields.push("IsDirty");
        PackageHarmonizeFields.push("ChangeSetOp");
        PackageHarmonizeFields.push("EntityParentPM");
        this.EntityPM.ShipmentPickUpDeliveryPackages.forEach(function (item) {
            var oldItemPackage = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(null);
            PackageFields.forEach(function (x) {
                oldItemPackage[x] = item[x];
            });
            item.PickUpDeliveryPackageHarmonizes.forEach(function (itemHarmonize) {
                var oldItemPackageHarmonize = new PickUpDeliveryPackageHarmonizePM_1.PickUpDeliveryPackageHarmonizePM(null);
                PackageHarmonizeFields.forEach(function (r) {
                    oldItemPackageHarmonize[r] = itemHarmonize[r];
                });
                oldItemPackage.PickUpDeliveryPackageHarmonizes.push(oldItemPackageHarmonize);
            });
            _this.oldPackages.push(oldItemPackage);
        });
    };
    AddEditDeliveryComponent.prototype.RejectPackages = function () {
        var _this = this;
        this.EntityPM.ShipmentPickUpDeliveryPackages = [];
        this.oldPackages.forEach(function (item) {
            _this.EntityPM.ShipmentPickUpDeliveryPackages.push(item);
        });
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AddEditDeliveryComponent.prototype, "AllLocations", void 0);
    AddEditDeliveryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditDeliveryComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AddEditDeliveryComponent);
    return AddEditDeliveryComponent;
}());
exports.AddEditDeliveryComponent = AddEditDeliveryComponent;
var TabItem = /** @class */ (function () {
    function TabItem(myCode, myTextCode, isDisabled) {
        if (isDisabled === void 0) { isDisabled = false; }
        this.TextCode = null;
        this.IsDisabled = false;
        this.Code = myCode;
        this.TextCode = myTextCode;
        this.IsDisabled = isDisabled;
    }
    return TabItem;
}());
//# sourceMappingURL=AddEditDeliveryComponent.js.map