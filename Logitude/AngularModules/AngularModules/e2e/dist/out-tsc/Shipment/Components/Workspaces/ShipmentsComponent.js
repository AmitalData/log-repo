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
var Tools_1 = require("../../../Infrastructure/Tools");
var ShipmentDomainService_1 = require("../../Services/ShipmentDomainService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../Infrastructure/Args");
var Tools_2 = require("../../Tools");
var Args_2 = require("../../Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ShipmentsComponent = /** @class */ (function () {
    function ShipmentsComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.IsCloudDeployment = false;
        this.TestToggleIsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Queries Features
        this.IsNewButtonVisible = false;
        this.IsMessagingStockVisible = false;
        this.IsQueryVisible_OperationalOpenGroup = false;
        this.IsQueryVisible_Shipments = false;
        this.IsQueryVisible_ImportShipments = false;
        this.IsQueryVisible_Masters = false;
        this.IsQueryVisible_AccountinglOpenGroup = false;
        this.IsQueryVisible_OpenReceivables = false;
        this.IsQueryVisible_OpenPayables = false;
        this.IsQueryVisible_EAWBGroup = false;
        this.IsQueryVisible_ExpectedDepartures = false;
        this.IsQueryVisible_AirlinesUpdates = false;
        this.IsQueryVisible_OthersGroup = false;
        this.IsQueryVisible_AllFollowUps = false;
        this.IsQueryVisible_MyFollowUps = false;
        this.IsQueryVisible_AllShipments = false;
        this.IsQueryVisible_AllMasters = false;
        this.IsQueryVisible_CanceledShipments = false;
        this.IsQueryVisible_CreditLimitBlocked = false;
        this.IsQueryVisible_FSRGroup = false;
        this.IsQueryVisible_MyViewsGroup = false;
        this.IsQueryVisible_INTTRAGroup = false;
        this.IsQueryVisible_ExpectedDeparturesNotTransmitted = false;
        this.IsQueryVisible_ShippingInstructionsLast7Days = false;
        this.IsQueryVisible_ContainerStatusLast7Days = false;
        this.mySelectedDirectionFilter = "All";
        this.mySelectedTransportFilter = "All";
        this.RecentShipmentsList = [];
        this.IsNoDataVisible_RecentShipments = false;
        this.IsNoDataVisible_DeparturesArrivals = false;
        this.isLoadingDeparturesArrivals = false;
        // New Commands AWBWizardComponent
        this.isWindowOpened = false;
        this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage) {
                if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage.toLowerCase() == "amitalstorage") {
                    this.IsCloudDeployment = true;
                }
            }
        }
        var FeatureToggle = SessionLocator_1.SessionLocator.FeatureToggles.filter(function (d) { return d.ToggleCode == "TST" && d.TenantNumber == SessionLocator_1.SessionLocator.Tenant; })[0];
        if (FeatureToggle) {
            this.TestToggleIsVisible = true;
        }
    }
    ShipmentsComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    };
    ShipmentsComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    ShipmentsComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
        this.LoadRecentShipments();
        this.LoadDeparturesArrivals();
        this.ReloadUsersQuery();
    };
    ShipmentsComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    ShipmentsComponent.prototype.SetQueriesVisibility = function () {
        this.IsNewButtonVisible = false;
        if (!FeatureLocator_1.FeatureLocator.IsPackage_EAWB() && !SessionLocator_1.SessionLocator.TenantPM.IsHybrid) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "NEW")) {
                this.IsNewButtonVisible = true;
            }
        }
        this.IsQueryVisible_OperationalOpenGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SHIPMENTS") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "MASTERS") ? true : false;
        this.IsQueryVisible_Shipments = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SHIPMENTS") ? true : false;
        this.IsQueryVisible_ImportShipments = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "IMPORTSHIPMETNS") ? true : false;
        this.IsQueryVisible_Masters = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "MASTERS") ? true : false;
        this.IsQueryVisible_AccountinglOpenGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "OPENRECEIVABLESSHIPMENTS") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "OPENPAYABLESMASTERS") ? true : false;
        this.IsQueryVisible_OpenReceivables = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "OPENRECEIVABLESSHIPMENTS") ? true : false;
        this.IsQueryVisible_OpenPayables = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "OPENPAYABLESMASTERS") ? true : false;
        this.IsQueryVisible_EAWBGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "EXPECTEDDEPATURE") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "AIRLINESUPDATES") ? true : false;
        this.IsQueryVisible_ExpectedDepartures = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "EXPECTEDDEPATURE") ? true : false;
        this.IsQueryVisible_AirlinesUpdates = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "AIRLINESUPDATES") ? true : false;
        this.IsMessagingStockVisible = SessionLocator_1.SessionLocator.TenantManagementJS.IsAWBStockPrepaid;
        // Others
        this.IsQueryVisible_AllFollowUps = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ALLFOLLOWUPS") ? true : false;
        this.IsQueryVisible_MyFollowUps = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "MYFOLLOWUPS") ? true : false;
        this.IsQueryVisible_AllShipments = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ALLSHIPMENTS") ? true : false;
        this.IsQueryVisible_AllMasters = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ALLMASTERS") ? true : false;
        this.IsQueryVisible_CanceledShipments = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "CANCELLEDSHIPMENTS") ? true : false;
        if (this.IsQueryVisible_AllFollowUps || this.IsQueryVisible_MyFollowUps || this.IsQueryVisible_AllShipments || this.IsQueryVisible_AllMasters || this.IsQueryVisible_CanceledShipments) {
            this.IsQueryVisible_OthersGroup = true;
        }
        this.IsQueryVisible_CreditLimitBlocked = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "CreditLimitBlockedShipments") ? true : false;
        this.IsQueryVisible_FSRGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SENDREQUEST") ? true : false;
        this.IsQueryVisible_MyViewsGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        // INNTRA
        this.IsQueryVisible_ExpectedDeparturesNotTransmitted = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ExpectedDeparturesNotTransmitted") ? true : false;
        this.IsQueryVisible_ShippingInstructionsLast7Days = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ShippingInstructionsLast7Days") ? true : false;
        this.IsQueryVisible_ContainerStatusLast7Days = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ContainerStatusLast7Days") ? true : false;
        if (this.IsQueryVisible_ExpectedDeparturesNotTransmitted || this.IsQueryVisible_ShippingInstructionsLast7Days || this.IsQueryVisible_ContainerStatusLast7Days) {
            this.IsQueryVisible_INTTRAGroup = true;
        }
    };
    Object.defineProperty(ShipmentsComponent.prototype, "SelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (value) {
            if (this.mySelectedDirectionFilter != value) {
                this.mySelectedDirectionFilter = value;
                this.CurrentSession.ChangeSessionHeader({ DirectionId: value });
                this.LoadQueriesCounts();
                this.LoadDeparturesArrivals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentsComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (value) {
            if (this.mySelectedTransportFilter != value) {
                this.mySelectedTransportFilter = value;
                this.CurrentSession.ChangeSessionHeader({ TransportId: value });
                this.LoadQueriesCounts();
                this.LoadDeparturesArrivals();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentsComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        if (this.IsCloudDeployment == false) {
            this.myShipmentDomainService.GetShipmentsCounts(this.SelectedDirectionFilter, this.SelectedTransportFilter).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult = myResponse.Result;
                        if (myResult != null) {
                            _this.OperationalOpenCount_DH = myResult.OperationalOpenCount_DH > 1000 ? "1000+" : myResult.OperationalOpenCount_DH.toString();
                            _this.OperationalOpenCount_DC = myResult.OperationalOpenCount_DC > 1000 ? "1000+" : myResult.OperationalOpenCount_DC.toString();
                            _this.AccountingOpenCount_DH = myResult.AccountingOpenCount_DH > 1000 ? "1000+" : myResult.AccountingOpenCount_DH.toString();
                            _this.AccountingOpenCount_DC = myResult.AccountingOpenCount_DC > 1000 ? "1000+" : myResult.AccountingOpenCount_DC.toString();
                            _this.AllFollowUpsCount = myResult.AllFollowUpsCount > 1000 ? "1000+" : myResult.AllFollowUpsCount.toString();
                            _this.MyFollowUpsCount = myResult.MyFollowUpsCount >= 1000 ? "1000+" : myResult.MyFollowUpsCount.toString();
                            _this.OperationalOpenCount_ETD = myResult.OperationalOpenCount_ETD > 1000 ? "1000+" : myResult.OperationalOpenCount_ETD.toString();
                            _this.OperationalOpenCount_LWU = myResult.OperationalOpenCount_LWU > 1000 ? "1000+" : myResult.OperationalOpenCount_LWU.toString();
                            _this.LastSentFSRCount = myResult.LastSentFSRCount > 1000 ? "1000+" : myResult.LastSentFSRCount.toString();
                            _this.ImportShipmentsCount = myResult.ImportShipmentsCount > 1000 ? "1000+" : myResult.ImportShipmentsCount.toString();
                            _this.CreditLimitBlockedCount = myResult.CreditLimitBlockedCount > 1000 ? "1000+" : myResult.CreditLimitBlockedCount.toString();
                            _this.ExpectedDeparturesNotTransmittedCount = myResult.ExpectedDeparturesNotTransmittedCount > 1000 ? "1000+" : myResult.ExpectedDeparturesNotTransmittedCount.toString();
                            _this.ShippingInstructionsLast7DaysCount = myResult.ShippingInstructionsLast7DaysCount > 1000 ? "1000+" : myResult.ShippingInstructionsLast7DaysCount.toString();
                            _this.ContainerStatusLast7DaysCount = myResult.ContainerStatusLast7DaysCount > 1000 ? "1000+" : myResult.ContainerStatusLast7DaysCount.toString();
                        }
                    }
                }
            });
        }
    };
    ShipmentsComponent.prototype.LoadRecentShipments = function () {
        var _this = this;
        this.RecentShipmentsList = [];
        this.IsNoDataVisible_RecentShipments = false;
        this.myShipmentDomainService.GetRecentShipments().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.RecentShipmentsList = myResponse.Result;
                    if (_this.RecentShipmentsList.length == 0) {
                        _this.IsNoDataVisible_RecentShipments = true;
                    }
                }
            }
        });
    };
    ShipmentsComponent.prototype.LoadDeparturesArrivals = function () {
        var _this = this;
        if (!this.isLoadingDeparturesArrivals) {
            this.isLoadingDeparturesArrivals = true;
            this.FlightSummaryList = [];
            this.DepartureArrivalList = [];
            this.IsNoDataVisible_DeparturesArrivals = false;
            this.myShipmentDomainService.GetDeparturesArrivals(this.SelectedDirectionFilter, this.SelectedTransportFilter).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult = myResponse.Result;
                        _this.FlightSummaryList = myResult;
                        _this.BuildDeparturesArrivals();
                    }
                }
                _this.isLoadingDeparturesArrivals = false;
            });
        }
    };
    ShipmentsComponent.prototype.BuildDeparturesArrivals = function () {
        var _this = this;
        var myResult = [];
        if (this.FlightSummaryList != null) {
            var dataGroup = [];
            this.FlightSummaryList.forEach(function (item) {
                var dataResultItem = dataGroup.filter(function (d) { return d.CarrierId == item.CarrierId && d.CarrierNumber == item.CarrierNumber && d.DirectionId == item.DirectionId && d.TransportModeId == item.TransportModeId; })[0];
                if (dataResultItem == null) {
                    dataResultItem = new DepartureArrivalItem(item.DirectionId, item.TransportModeId);
                    dataResultItem.CarrierId = item.CarrierId;
                    dataResultItem.CarrierNumber = item.CarrierNumber;
                    dataGroup.push(dataResultItem);
                }
            });
            if (dataGroup.length > 0) {
                dataGroup.forEach(function (item) {
                    var myRecord = _this.FlightSummaryList.filter(function (f) { return f.CarrierId == item.CarrierId && f.CarrierNumber == item.CarrierNumber && f.DirectionId == item.DirectionId && f.TransportModeId == item.TransportModeId; })[0];
                    if (myRecord != null) {
                        item.LineBackground = "#FFFFFFFF";
                        item.CarrierCode = myRecord.CarrierCode;
                        item.CarrierNumber = myRecord.CarrierNumber;
                        item.CarrierCodeCellText = myRecord.CarrierCode == "No_Data" ? "-" : myRecord.CarrierCode;
                        item.CarrierNumberCellText = myRecord.CarrierNumber == "No_Data" ? "-" : myRecord.CarrierNumber;
                        item.CarrierCodeFontSize = myRecord.CarrierCode == "No_Data" ? 10 : 12;
                        item.CarrierCodeForeground = myRecord.CarrierCode == "No_Data" ? "Gray" : "#FF282E30";
                        item.CarrierNumberFontSize = myRecord.CarrierNumber == "No_Data" ? 10 : 12;
                        item.CarrierNumberForeground = myRecord.CarrierNumber == "No_Data" ? "Gray" : "#FF282E30";
                        var myCarrierList = _this.FlightSummaryList.filter(function (f) { return f.CarrierId == item.CarrierId && f.CarrierNumber == item.CarrierNumber && f.DirectionId == item.DirectionId && f.TransportModeId == item.TransportModeId; });
                        item.LastWeek.Build(myCarrierList);
                        item.Today.Build(myCarrierList);
                        item.Tomorrow.Build(myCarrierList);
                        item.NextWeek.Build(myCarrierList);
                        item.TotalsCount = item.LastWeek.Count + item.Today.Count + item.Tomorrow.Count + item.NextWeek.Count;
                        if (item.TotalsCount > 0) {
                            myResult.push(item);
                        }
                    }
                });
            }
        }
        // OrderByDescending
        myResult.sort(function (a, b) { return (b.Today.Count) - (a.Today.Count); }).forEach(function (myResultItem) {
            _this.DepartureArrivalList.push(myResultItem);
        });
        this.IsNoDataVisible_DeparturesArrivals = myResult.length == 0 ? true : false;
    };
    ShipmentsComponent.prototype.RunNewShipmentWizard = function (levelCode) {
        var _this = this;
        if (!this.isWindowOpened) {
            this.isWindowOpened = true;
            var windowTitle = "";
            switch (levelCode) {
                case "D": {
                    windowTitle = "New Direct shipment";
                    break;
                }
                case "H": {
                    windowTitle = "New House Shipment";
                    break;
                }
                case "C": {
                    windowTitle = "New Master";
                    break;
                }
                default: {
                    break;
                }
            }
            if (levelCode == "C") {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.Title = windowTitle;
                logWindow.Show('./Shipment/Components/NewShipment/NewMasterComponent');
                logWindow.WindowClosed.subscribe(function (s) {
                    _this.isWindowOpened = false;
                    if (s) {
                        _this.LoadAllScreenData();
                    }
                });
            }
            else {
                var args = new Args_2.NewShipmentComponentArgs();
                args.ShipmentLevelCode = levelCode;
                args.IsShipmentLevelFixed = true;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.WindowArgs = args;
                logWindow.Title = windowTitle;
                logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
                logWindow.WindowClosed.subscribe(function (s) {
                    _this.isWindowOpened = false;
                    if (s) {
                        _this.LoadAllScreenData();
                    }
                });
            }
        }
    };
    ShipmentsComponent.prototype.RunNewAWBWizard = function (levelCode) {
        var _this = this;
        if (!this.isWindowOpened) {
            this.isWindowOpened = true;
            var windowTitle = "";
            switch (levelCode) {
                case "D": {
                    windowTitle = "Direct AWB Wizard";
                    break;
                }
                case "H": {
                    windowTitle = "House AWB Wizard";
                    break;
                }
                case "C": {
                    windowTitle = "Master AWB Wizard";
                    break;
                }
                default: {
                    break;
                }
            }
            var windowArgs = new Args_2.AWBWizardArgs();
            windowArgs.IsNewEntity = true;
            windowArgs.ShipmentLevelCode = levelCode;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.LoadAllScreenData();
                _this.isWindowOpened = false;
            });
        }
    };
    // Edit Commands
    ShipmentsComponent.prototype.EditShipment = function (entity) {
        if (entity != null) {
            if (!this.isWindowOpened) {
                this.isWindowOpened = true;
                var myCodes = [];
                myCodes.push("EAWB");
                myCodes.push("BUBK");
                if (FeatureLocator_1.FeatureLocator.IsPackageOneOf(myCodes)) {
                    this.AWBEditShipment(entity);
                }
                else {
                    this.FullEditShipment(entity);
                }
            }
        }
    };
    ShipmentsComponent.prototype.AWBEditShipment = function (entity) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = Tools_2.ShipmentTool.GetAWBWizardHeader(entity.ShipmentLevelCode, entity.DirectionId);
        logWindow.WindowArgs = entity.Id;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.LoadAllScreenData();
            _this.isWindowOpened = false;
        });
    };
    ShipmentsComponent.prototype.FullEditShipment = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Shipment', BackButtonLabel: "Operations" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
                _this.isWindowOpened = false;
            });
        });
    };
    ShipmentsComponent.prototype.ViewShipmentQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var queryCode = myQueryCode;
            var objectTableName = "Shipment";
            var MethodName = null;
            var displayTitle = "";
            var backButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Operations");
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "Shipments":
                    {
                        displayTitle = "Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "Masters":
                    {
                        displayTitle = "Masters";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "Open Receivables Shipments":
                    {
                        displayTitle = "Open Receivables Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "Open Payables Masters":
                    {
                        displayTitle = "Open Payables Masters";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "Expected Departures":
                    {
                        displayTitle = "Expected Departures Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "Airlines Updates":
                    {
                        displayTitle = "Airlines Updates Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "All Follow Ups":
                    {
                        displayTitle = "All Follow Ups";
                        MethodName = "ShipmentFollowUp";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "My Follow Ups":
                    {
                        displayTitle = "My Follow Ups";
                        MethodName = "ShipmentFollowUp";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "All Shipments":
                    {
                        displayTitle = "All Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "All Masters":
                    {
                        displayTitle = "All Masters";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "Cancelled Shipments":
                    {
                        displayTitle = "Canceled Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "SentFSR":
                    {
                        displayTitle = "Sent FSR (last 7 days)";
                        break;
                    }
                case "ImportShipments":
                    {
                        displayTitle = "Import Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "CreditLimitBlockedShipments":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.Q.CreditLimitBlockedShipment");
                        this.SetDirectionTransportFilter();
                        break;
                    }
                case "ExpDepNotTransmitted":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.Q.ExpectedDeparturesNotTransmitted");
                        this.SetDirectionTransportFilter();
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName; //"Shipment";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Operations";
            listArgs.MethodName = MethodName;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    var filtersBar = null;
                    cmpRef.instance.FiltersBarLoaded.subscribe(function (myBar) {
                        filtersBar = myBar;
                        if (filtersBar) {
                            if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                                filtersBar.SetTransport(_this.SelectedTransportFilter);
                            }
                            if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                                filtersBar.SetDirection(_this.SelectedDirectionFilter);
                            }
                        }
                    });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        if (filtersBar) {
                            if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                                _this.mySelectedTransportFilter = filtersBar.SelectedValue;
                            }
                            if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                                _this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                            }
                        }
                        //this.CurrentSession.ChangeSessionHeader({ DirectionId: this.SelectedDirectionFilter });
                        //this.CurrentSession.ChangeSessionHeader({ TransportId: this.SelectedTransportFilter });
                        _this.LoadAllScreenData();
                    });
                    listArgs.SelectedDirection = _this.mySelectedDirectionFilter;
                    listArgs.SelectedTransportMode = _this.mySelectedTransportFilter;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    ShipmentsComponent.prototype.ViewDepartureArrivalQuery = function (item, myCode) {
        var _this = this;
        if (myCode != null) {
            var displayName = "";
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myCode) {
                case "LSW_EXP": {
                    displayName = "Last week expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.LastWeek.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
                case "LSW_ACT": {
                    displayName = "Last week actual shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.LastWeek.ActualShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
                case "TOD_EXP": {
                    displayName = "Today expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.Today.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
                case "TOD_ACT": {
                    displayName = "Today actual shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.Today.ActualShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
                case "TOM_EXP": {
                    displayName = "Tomorrow expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.Tomorrow.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
                case "NXW_EXP": {
                    displayName = "Next week expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.NextWeek.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Masters";
            listArgs.ObjectTableName = "Shipment";
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "Operations";
            listArgs.ShowViews = false;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    var filtersBar = null;
                    cmpRef.instance.FiltersBarLoaded.subscribe(function (myBar) {
                        filtersBar = myBar;
                        if (filtersBar) {
                            if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                                filtersBar.SetTransport(_this.SelectedTransportFilter);
                            }
                            if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                                filtersBar.SetDirection(_this.SelectedDirectionFilter);
                            }
                        }
                    });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        if (filtersBar) {
                            if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                                _this.mySelectedTransportFilter = filtersBar.SelectedValue;
                            }
                            if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                                _this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                            }
                        }
                        _this.LoadAllScreenData();
                    });
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    ShipmentsComponent.prototype.SetDirectionTransportFilter = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedDirectionFilter) && this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("DirectionId", this.SelectedDirectionFilter, null, null, "Equals", false, true, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTransportFilter) && this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string");
        }
    };
    ShipmentsComponent.prototype.MessagingStockClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;
        logWindow.Title = "Messaging Stock";
        logWindow.Show('./ShipmentModules/ShipmentStock/Components/MessagingStock/StockWindowComponent');
    };
    ShipmentsComponent.prototype.SendShipmentFSRClicked = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Master").subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Send FSR";
            logWindow.Width = 600;
            logWindow.Height = 370;
            logWindow.WindowArgs = _this;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/FSRWizard/SendShipmentFSRComponent');
        });
    };
    ShipmentsComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    ShipmentsComponent.prototype.CreateMissingMastersClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating Masters");
        this.myShipmentDomainService.CreateMissingMasters().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ShipmentsComponent.prototype, "ReloadUserQueries", void 0);
    ShipmentsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ShipmentsComponent);
    return ShipmentsComponent;
}());
exports.ShipmentsComponent = ShipmentsComponent;
var DepartureArrivalItem = /** @class */ (function () {
    function DepartureArrivalItem(myDirectionId, myTransportModeId) {
        this.DirectionId = myDirectionId;
        this.TransportModeId = myTransportModeId;
        switch (this.DirectionId) {
            case "E": {
                this.DirectionName = "Export";
                break;
            }
            case "I": {
                this.DirectionName = "Import";
                break;
            }
            case "R": {
                this.DirectionName = "Drop";
                break;
            }
            case "D": {
                this.DirectionName = "Domestic";
                break;
            }
        }
        switch (this.TransportModeId) {
            case "A": {
                this.TransportModeName = "Air";
                break;
            }
            case "O": {
                this.TransportModeName = "Ocean";
                break;
            }
            case "I": {
                this.TransportModeName = "Inland";
                break;
            }
        }
        this.LastWeek = new DepartureArrival("LSW");
        this.Today = new DepartureArrival("TOD");
        this.Tomorrow = new DepartureArrival("TOM");
        this.NextWeek = new DepartureArrival("NXW");
    }
    return DepartureArrivalItem;
}());
var DepartureArrival = /** @class */ (function () {
    function DepartureArrival(myCode) {
        this.Count = 0;
        this.ExpectedCount = 0;
        this.ActualCount = 0;
        this.ActualItems = [];
        this.ExpectedItems = [];
        this.Code = myCode;
    }
    DepartureArrival.prototype.Build = function (list) {
        if (list) {
            this.ActualItems = list.filter(function (d) { return d.ActualDate != null; });
            this.ExpectedItems = list.filter(function (d) { return d.ActualDate == null && d.ExpectedDate != null; });
            switch (this.Code) {
                case "LSW": {
                    this.Build_LSW();
                    break;
                }
                case "TOD": {
                    this.Build_TOD();
                    break;
                }
                case "TOM": {
                    this.Build_TOM();
                    break;
                }
                case "NXW": {
                    this.Build_NXW();
                    break;
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ExpectedCount)) {
                this.ExpectedCount = 0;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ActualCount)) {
                this.ActualCount = 0;
            }
            this.Count = this.ExpectedCount + this.ActualCount;
        }
    };
    DepartureArrival.prototype.Build_LSW = function () {
        var list_EXP = this.ExpectedItems.filter(function (d) { return Tools_1.DateTool.TruncateTime(d.ExpectedDate).valueOf() >= Tools_1.DateTool.GetDateByDay(-7).valueOf() && Tools_1.DateTool.TruncateTime(d.ExpectedDate).valueOf() <= Tools_1.DateTool.GetDateByDay(-1).valueOf(); });
        if (list_EXP.length > 0) {
            var ids = this.GetIdsList(list_EXP);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = Tools_1.AppTool.GetIdsArrayText(ids);
        }
        var list_ACT = this.ActualItems.filter(function (d) { return Tools_1.DateTool.TruncateTime(d.ActualDate).valueOf() >= Tools_1.DateTool.GetDateByDay(-7).valueOf() && Tools_1.DateTool.TruncateTime(d.ActualDate).valueOf() <= Tools_1.DateTool.GetDateByDay(-1).valueOf(); });
        if (list_ACT.length > 0) {
            var ids = this.GetIdsList(list_ACT);
            this.ActualCount = ids.length;
            this.ActualShipmentsIds = Tools_1.AppTool.GetIdsArrayText(ids);
        }
    };
    DepartureArrival.prototype.Build_TOD = function () {
        var list_EXP = this.ExpectedItems.filter(function (d) { return Tools_1.DateTool.TruncateTime(d.ExpectedDate).valueOf() == Tools_1.DateTool.GetDateByDay(0).valueOf(); });
        if (list_EXP.length > 0) {
            var ids = this.GetIdsList(list_EXP);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = Tools_1.AppTool.GetIdsArrayText(ids);
        }
        var list_ACT = this.ActualItems.filter(function (d) { return Tools_1.DateTool.TruncateTime(d.ActualDate).valueOf() == Tools_1.DateTool.GetDateByDay(0).valueOf(); });
        if (list_ACT.length > 0) {
            var ids = this.GetIdsList(list_ACT);
            this.ActualCount = ids.length;
            this.ActualShipmentsIds = Tools_1.AppTool.GetIdsArrayText(ids);
        }
    };
    DepartureArrival.prototype.Build_TOM = function () {
        var list = this.ExpectedItems.filter(function (d) { return Tools_1.DateTool.TruncateTime(d.ExpectedDate).valueOf() == Tools_1.DateTool.GetDateByDay(1).valueOf(); });
        if (list.length > 0) {
            var ids = this.GetIdsList(list);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = Tools_1.AppTool.GetIdsArrayText(ids);
        }
    };
    DepartureArrival.prototype.Build_NXW = function () {
        var list = this.ExpectedItems.filter(function (d) { return Tools_1.DateTool.TruncateTime(d.ExpectedDate).valueOf() >= Tools_1.DateTool.GetDateByDay(2).valueOf() && Tools_1.DateTool.TruncateTime(d.ExpectedDate).valueOf() <= Tools_1.DateTool.GetDateByDay(7).valueOf(); });
        if (list.length > 0) {
            var ids = this.GetIdsList(list);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = Tools_1.AppTool.GetIdsArrayText(ids);
        }
    };
    DepartureArrival.prototype.GetIdsList = function (list) {
        var myResult = [];
        list.forEach(function (item) {
            if (myResult.indexOf(item.ShipmentId) == -1) {
                myResult.push(item.ShipmentId);
            }
        });
        return myResult;
    };
    return DepartureArrival;
}());
//# sourceMappingURL=ShipmentsComponent.js.map