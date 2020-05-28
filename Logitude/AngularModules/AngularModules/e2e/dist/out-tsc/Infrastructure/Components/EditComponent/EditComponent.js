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
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var LocationDirective_1 = require("../../Utilities/LocationDirective");
var EntityArgs_1 = require("../../DataContracts/EntityArgs");
var Tools_1 = require("../../Tools");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var EntityPMService_1 = require("../../Services/EntityPMService");
var EntityLastActivityService_1 = require("../../Services/EntityLastActivityService");
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var TotangoService_1 = require("../../Services/WebServices/TotangoService");
var CachedDataManager_1 = require("../../Utilities/CachedDataManager");
var LastFilterClass_1 = require("../../Utilities/LastFilterClass");
var Subscription_1 = require("rxjs/Subscription"); //itzik
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var EditComponent = /** @class */ (function () {
    function EditComponent(entityPMService, entityArgs, _entityResourceService, _totangoService, cd) {
        this.entityPMService = entityPMService;
        this.entityArgs = entityArgs;
        this._entityResourceService = _entityResourceService;
        this._totangoService = _totangoService;
        this.cd = cd;
        this.BackCompleted = new core_1.EventEmitter();
        this.LoadCompleted = new core_1.EventEmitter();
        this.SaveCompleted = new core_1.EventEmitter();
        this.TabSelected = new core_1.EventEmitter();
        this.TabChanged = new core_1.EventEmitter();
        this.SaveAndCloseCompleted = new core_1.EventEmitter();
        this.OnFirstTimeAfterSingleDataLoaded = new core_1.EventEmitter();
        this.EntityPM = null;
        this.EntityId = null;
        this.ComponentIndex = null;
        this.ValidationErrorsList = [];
        this.IsInsideWindow = false;
        this.PreSelectedTabCode = null;
        this.HasHelper = false;
        this.HasShortTitle = false;
        this.HasMenuButtons = false;
        this.IsTabsHidden = false;
        this.IsEntityLoaded = false;
        this.ComponentBackground = "white";
        this.IsEditValid = true;
        this.IsSaveBtnVisible = true;
        this.IsSaveBtnDisable = false;
        this.ShowWindowsOverEditComponent = false;
        this.LayoutDirection = 'ltr';
        this.WorkEnvironment = 'logitude';
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsReloadNeeded = false;
        this.EntityFields = null;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.HeaderScreenHeight = 65;
        this.HeaderScreenRowHeight = 25;
        this.HeaderScreenColumns = [];
        this.SavedWidthOfHeader = 0;
        this.FindHeaderRetries = 0;
        //public ObjectTableTabs: any[] = [];
        this.TabsItemsSource = [];
        this.LoadedTabsList = [];
        this.SingleDetailsTab = null;
        this.busyIndicatorText = null;
        this.showBusyIndicator = false;
        this._Subscription = new Subscription_1.Subscription(); //itzik///https://stackoverflow.com/a/42274637
        //#region Split Component
        this.IsSplitBtnVisible = false;
        this.IsSplitComponentOpened = false;
        //#endregion
        //navigation methods
        this.NextPreviousVisible = false;
        this.PreviousButtonDisabled = false;
        this.NextButtonDisabled = false;
        this.DeclarationNavigationMessage = "";
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        this.ComponentIndex = this.CurrentSession.GetNewEditComponentIndex();
        this.HeaderId = "HeaderScreen_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.ComponentId = "EditComponent_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.EditComponentCellId = "EditComponentCellId_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        this.WorkEnvironment = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "logitude" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment;
    }
    EditComponent.prototype.Run = function (args) {
        var _this = this;
        this.EntityId = args['EntityId'];
        this.EntityPM = args['EntityPM'];
        this.EntityParentPM = args['EntityParentPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.PreSelectedTabCode = args['SelectedTabCode'];
        this.BackButtonLabel = !Tools_1.AppTool.IsNullOrEmpty(args['BackButtonLabel']) ? args['BackButtonLabel'] : TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Back"); // "Back";
        this.ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.HasHelper = this.ObjectTable.HasHelper;
        this.HasShortTitle = this.ObjectTable.HasShortTitle;
        this.HasMenuButtons = this.ObjectTable.HasMenuButtons;
        this.IsTabsHidden = this.ObjectTable.IsTabsHidden;
        this.NavigationIds = args['NavigationIds'];
        this.EntityFields = args['EntityFields'];
        if (this.NavigationIds) {
            this.NextPreviousVisible = true;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrentNavigatedIndex) && this.NavigationIds) {
            this.CurrentNavigatedIndex = 0;
            this.DeclarationNavigationMessage = (this.CurrentNavigatedIndex + 1).toString() + " מתוך " + this.NavigationIds.length.toString();
            //this.PreviousButtonDisabled = true;
            this.SetNextPreviousButtonsEnablity();
        }
        if (this.EntityPM != null) {
            this.entityArgs.EntityPM = this.EntityPM;
            this.entityArgs.ObjectTableName = this.ObjectTableName;
            this.entityArgs.EditComponent = this;
            this.BuildComponent();
        }
        else if (this.EntityId != null) {
            this.LoadEntityPM();
        }
        if (this.ObjectTableName != "Country" && this.ObjectTableName != "PackageType") {
            this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "View " + this.ObjectTableName);
        }
        if (this.ObjectTableName == "CommunicationLog") {
            this.IsSaveBtnDisable = true;
        }
        this.IsSaveBtnVisible = this.ObjectTable.IsSaveButtonVisible;
        // Split Component
        var feature = FeatureLocator_1.FeatureLocator.Features.filter(function (d) { return d.Code == "SPLIT"; })[0];
        if (!Tools_1.AppTool.IsNullOrEmpty(feature)) { // granted
            //if (this.ObjectTable.Name == "Customs.Declaration")
            //    this.IsSplitBtnVisible = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTable.SplitComponentPath)) {
                this.IsSplitBtnVisible = true;
                this.ShowWindowsOverEditComponent = true;
            }
        }
        //fullaccounting => hide arpayment save btn for new arp entity
        var isNewEntity = true;
        if (this.EntityId || (this.EntityId && this.EntityPM.Id))
            isNewEntity = false;
        if ((this.ObjectTableName == "ARPayment" || this.ObjectTableName == "APPayment") && SessionLocator_1.SessionLocator.TenantPM.AccountingActivated && isNewEntity) {
            this.IsSaveBtnVisible = false;
        }
        //
    };
    EditComponent.prototype.LoadEntityPM = function () {
        var _this = this;
        this.entityPMService.getSingle(this.ObjectTableName, this.EntityId).then(function (response) {
            response.subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    _this.EntityPM = pmResponse.Result;
                    if (_this.EntityFields) {
                        _this.EntityFields.forEach(function (itemField) {
                            _this.EntityPM[itemField["FieldName"]] = itemField["FieldValue"];
                        });
                        _this.EntityPM.IsDirty = false;
                    }
                    if (_this.EntityPM) {
                        _this.entityArgs.EntityPM = _this.EntityPM;
                        _this.entityArgs.ObjectTableName = _this.ObjectTableName;
                        _this.entityArgs.EditComponent = _this;
                        _this.SendActivityLog();
                        _this.BuildComponent();
                    }
                    else {
                        _this.StopBusyIndicator();
                        _this.ValidationErrorsList.push("Error displaying this " + TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectTableName));
                    }
                }
                else {
                    _this.StopBusyIndicator();
                    _this.ValidationErrorsList = pmResponse.ErrorsArray;
                    //console.error(pmResponse.ErrorsArray);
                }
            }, function (error) {
                _this.StopBusyIndicator();
            });
        });
    };
    EditComponent.prototype.SendActivityLog = function () {
        if (this.ObjectTableName == "Customer") {
            if (this.EntityPM['IsCustomer']) {
                var myService = new EntityLastActivityService_1.EntityLastActivityService();
                myService.AddActivityLog(this.EntityId, this.ObjectTableId, SessionLocator_1.SessionLocator.LoggedUserId, 'V').subscribe();
            }
        }
        else {
            var myService = new EntityLastActivityService_1.EntityLastActivityService();
            myService.AddActivityLog(this.EntityId, this.ObjectTableId, SessionLocator_1.SessionLocator.LoggedUserId, 'V').subscribe();
        }
    };
    EditComponent.prototype.BuildComponent = function () {
        var _this = this;
        if (this.EntityPM) {
            this.IsEntityLoaded = true;
            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
                _this.GetControllerByTableName(_this.ObjectTableName).then(function (EditComponentController) {
                    //this.EditComponentController = EditComponentController as IEditComponentController;
                    _this.CurrentSession.AddEditComponent(_this);
                    _this.EditComponentController = EditComponentController;
                    _this.EditComponentController.OnFirstTimeAfterSingleDataLoaded(_this.EntityPM).then(function (isLock) {
                        _this.OnFirstTimeAfterSingleDataLoaded.emit(".EditComponentController.OnFirstTimeAfterSingleDataLoaded");
                        if (_this.EditComponentController.ToCancell) {
                            _this.Close();
                        }
                        else {
                            _this._SubEditComponentDefaultController =
                                _this.SaveCompleted.subscribe(function (isSaved) {
                                    if (isSaved) {
                                        _this.EditComponentController.HaveSaved = true;
                                        _this._SubEditComponentDefaultController.unsubscribe();
                                        _this._SubEditComponentDefaultController == null;
                                    }
                                });
                            // if (this.CurrentNavigatedIndex ==0) {
                            _this.BuildEditTabs();
                            //}
                            _this.RunComponent();
                            _this.StopBusyIndicator();
                        }
                    });
                });
            });
        }
    };
    EditComponent.prototype.RunComponent = function () {
        if (this.TabControlBodyViewContainerRef) {
            //if (this.AllLocations.length == 0) {
            //    this.RunComponentTimer();
            //}
            if (this.HasHelper && !this.HelperViewContainerRef) {
                this.RunComponentTimer();
            }
            else if (this.HasShortTitle && !this.ShortTitleViewContainerRef) {
                this.RunComponentTimer();
            }
            else if (this.HasMenuButtons && !this.MenuButtonsViewContainerRef) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.BuildHelperControl();
                this.BuildMenuButtons();
                this.BuildShortTitle();
                this.BuildHeaderScreen();
                this.SetSelectedTab();
                this.SetSplitComponentState();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    EditComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    EditComponent.prototype.BuildHelperControl = function () {
        if (this.HasHelper) {
            if (this.HelperViewContainerRef) {
                this.HelperViewContainerRef.clear();
                var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Helpers/" + this.ObjectTable.Name + "HelperComponent";
                SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.HelperViewContainerRef)
                    .then(function (cmpRef) {
                });
            }
        }
    };
    EditComponent.prototype.BuildMenuButtons = function () {
        var _this = this;
        if (this.HasMenuButtons) {
            if (this.MenuButtonsViewContainerRef) {
                this.MenuButtonsViewContainerRef.clear();
                var myComponentPath = './Infrastructure/Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponent';
                SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.MenuButtonsViewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.Run({ EntityPM: _this.EntityPM, ObjectTable: _this.ObjectTable });
                });
            }
        }
    };
    EditComponent.prototype.BuildShortTitle = function () {
        if (this.HasShortTitle) {
            if (this.ShortTitleViewContainerRef) {
                this.ShortTitleViewContainerRef.clear();
                if (this.ObjectTable.Name != null && this.ObjectTable.Name.includes("Customs")) {
                    var name = this.ObjectTable.Name.split('.');
                    var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/ShortTitles/" + name[1] + "ShortTitleComponent";
                }
                else {
                    var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/ShortTitles/" + this.ObjectTable.Name + "ShortTitleComponent";
                }
                SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.ShortTitleViewContainerRef);
            }
        }
    };
    EditComponent.prototype.BuildHeaderScreen = function () {
        var _this = this;
        var myHeaderScreen = window.Screens.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && d.Code.indexOf("HeaderScreen") != -1; })[0];
        var myObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId; });
        if (this.ObjectTableName == "Shipment") {
            if (this.EntityPM.ShipmentLevelCode == "C") {
                this._entityResourceService.getEntityResourceByTableName("Master", 0).subscribe(function (response) {
                    var myObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Master"; })[0];
                    var myObjectTableId = myObjectTable.Id;
                    myHeaderScreen = window.Screens.filter(function (d) { return d.ObjectTableId === myObjectTableId && d.Code.indexOf("HeaderScreen") != -1; })[0];
                    myObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === myObjectTableId; });
                    _this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
                });
            }
            else {
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }
        else if (this.ObjectTableName == "ARInvoice") {
            //get f. acc. Settings
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                var myObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "ARInvoice"; })[0];
                var myObjectTableId = myObjectTable.Id;
                myHeaderScreen = window.Screens.filter(function (d) { return d.ObjectTableId === myObjectTableId && d.Code == "ARInvoice.FullAccHeaderScreen"; })[0];
                myObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === myObjectTableId; });
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
            else {
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }
        //else if (this.ObjectTableName == "Customs.Declaration") {
        //    if (this.EntityPM.IsCourierDeclaration == true) {
        //        this._entityResourceService.getEntityResourceByTableName("Customs.CourierDeclaration", 0).subscribe(response => {
        //            var myObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.CourierDeclaration")[0];
        //            var myObjectTableId = myObjectTable.Id;
        //            myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code.indexOf("HeaderScreen") != -1)[0];
        //            myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
        //            this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        //        });
        //    }
        //    else {
        //        this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        //    }
        //}
        else {
            this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        }
    };
    EditComponent.prototype.RunFindHeaderTimer = function (HeaderScreen, ObjectFields) {
        var _this = this;
        if (this.FindHeaderTimerToken) {
            clearTimeout(this.FindHeaderTimerToken);
        }
        var element = document.getElementById(this.HeaderId);
        if (element) {
            this.GenerateHeaderScreen(HeaderScreen, ObjectFields);
        }
        else {
            this.FindHeaderRetries++;
            if (this.FindHeaderRetries < 3) {
                this.FindHeaderTimerToken = setTimeout(function () { return _this.RunFindHeaderTimer(HeaderScreen, ObjectFields); }, 1);
            }
        }
    };
    EditComponent.prototype.GenerateHeaderScreen = function (HeaderScreen, ObjectFields) {
        var element = document.getElementById(this.HeaderId);
        if (element == null) {
            this.RunFindHeaderTimer(HeaderScreen, ObjectFields);
        }
        else {
            this.HeaderScreenColumns = [];
            if (HeaderScreen != null) {
                if (HeaderScreen.NumberOfRows <= 1) {
                    this.HeaderScreenHeight = 40;
                    this.HeaderScreenRowHeight = 25;
                }
                else if (HeaderScreen.NumberOfRows == 2) {
                    this.HeaderScreenHeight = 65;
                    this.HeaderScreenRowHeight = 25;
                }
                else if (HeaderScreen.NumberOfRows == 3) {
                    this.HeaderScreenHeight = 75;
                    this.HeaderScreenRowHeight = 20;
                }
                var myScreenFields = window.ScreenFields.filter(function (d) { return d.ScreenId === HeaderScreen.Id && d.Tenant == SessionLocator_1.SessionLocator.Tenant; });
                if (myScreenFields.length == 0) {
                    myScreenFields = window.ScreenFields.filter(function (d) { return d.ScreenId === HeaderScreen.Id && d.Tenant == 0; });
                }
                var widthOfColumn = 0;
                if (element != null) {
                    var widthOfHeader = element.clientWidth;
                    if (widthOfHeader == 0) {
                        widthOfHeader = this.SavedWidthOfHeader;
                    }
                    else {
                        this.SavedWidthOfHeader = widthOfHeader;
                    }
                    if (widthOfHeader == 0) {
                        var ApplicationSession = document.getElementById("ApplicationSession");
                        if (ApplicationSession) {
                            var appWidth = ApplicationSession.clientWidth;
                            widthOfHeader = appWidth - 42;
                        }
                    }
                    var widthOfSeparator = (HeaderScreen.NumberOfColumns - 1) * 20;
                    widthOfColumn = (widthOfHeader - widthOfSeparator) / HeaderScreen.NumberOfColumns;
                }
                for (var c = 0; c < HeaderScreen.NumberOfColumns; c++) {
                    var myColumn = new HeaderScreenColumn(false);
                    var myColumnLabelWidth = 0;
                    for (var r = 0; r < HeaderScreen.NumberOfRows; r++) {
                        var myRow = new HeaderScreenRow();
                        var myScreenField = myScreenFields.filter(function (f) { return f.Column == c && f.Row == r; })[0];
                        if (myScreenField != null) {
                            var myObjectField = ObjectFields.filter(function (d) { return d.Id === myScreenField.ObjectFieldId; })[0];
                            if (myObjectField != null) {
                                myRow.Label = TextCodeTranslator_1.TextCodeTranslator.Translate(myObjectField.FullNameTextCodeCode);
                                myRow.ObjectField = myObjectField;
                                if (!Tools_1.AppTool.IsNullOrEmpty(myRow.Label)) {
                                    myRow.Label += ":";
                                }
                                var widthOfLabel = Tools_1.AppTool.GetTextWidth(myRow.Label);
                                if (widthOfLabel > myColumnLabelWidth) {
                                    myColumnLabelWidth = widthOfLabel;
                                }
                                if (myColumnLabelWidth > (widthOfColumn / 2)) {
                                    myColumnLabelWidth = widthOfColumn / 2;
                                }
                                myColumn.LabelWidth = (Math.ceil(myColumnLabelWidth) + 10) + "px";
                                myColumn.ValueMaxWidth = widthOfColumn - myColumnLabelWidth - 12;
                            }
                        }
                        myColumn.Rows.push(myRow);
                    }
                    this.HeaderScreenColumns.push(myColumn);
                    if (HeaderScreen.NumberOfColumns - c > 1) {
                        this.HeaderScreenColumns.push(new HeaderScreenColumn(true));
                    }
                }
            }
        }
    };
    EditComponent.prototype.BuildEditTabs = function () {
        if (this.IsTabsHidden) {
            this.BuildSingleEditTab();
        }
        else {
            this.BuildTabsItemsSource();
        }
    };
    EditComponent.prototype.BuildSingleEditTab = function () {
        var _this = this;
        var singleTab = window.ObjectTableTabs.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && d.IndexOrder === 0; })[0];
        if (singleTab) {
            if (FeatureLocator_1.FeatureLocator.IsFeatureGranted(singleTab.FeatureId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(singleTab.HtmlComponentUrl)) {
                    this.SingleDetailsTab = singleTab;
                }
            }
        }
    };
    EditComponent.prototype.BuildTabsItemsSource = function () {
        var _this = this;
        var allTabs = [];
        var myTabsSorted = [];
        this.TabsItemsSource = [];
        allTabs = window.ObjectTableTabs.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId; });
        allTabs = this.FilterTabs(allTabs);
        allTabs = allTabs.sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
        for (var i = 0; i < allTabs.length; i++) {
            var tab = allTabs[i];
            if (tab.ControlPath != null) {
                if (tab.ControlPath.indexOf("ExternalDocumentsControl") != -1) {
                    if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "DOCSIN")) {
                        continue;
                    }
                }
                if (tab.ControlPath.indexOf("EventsControl") != -1) {
                    var eventsTabCode = tab.ObjectTableName + ".Tab.Events";
                    var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "EVENTS" || f.Code == eventsTabCode) && f.ObjectTableId == tab.ObjectTableId; })[0];
                    if (eventsTabFeature = null) {
                        continue;
                    }
                }
                //if (tab.ControlPath.indexOf("WarehouseConnectionsTabComponent") != -1) {
                //    if (this.EntityPM && AppTool.IsNullOrEmpty(this.EntityPM.ShipmentId)) continue;
                //}
            }
            if (FeatureLocator_1.FeatureLocator.IsFeatureGranted(tab.FeatureId)) {
                if (this.ObjectTableName == "GLAccount") {
                    switch (tab.Code) {
                        case "GAAD":
                            {
                                if (this.EntityPM.AccountTypeCode == "2" || this.EntityPM.AccountTypeCode == "3")
                                    myTabsSorted.push(tab);
                                break;
                            }
                        case "GLTX":
                            {
                                if (this.EntityPM.AccountTypeCode == "3")
                                    myTabsSorted.push(tab);
                                break;
                            }
                        case "GAOV":
                            {
                                if (this.EntityPM.AccountTypeCode == "2") // 2- Customer GLAccount
                                    myTabsSorted.push(tab);
                                break;
                            }
                        default:
                            {
                                myTabsSorted.push(tab);
                                break;
                            }
                    }
                }
                else
                    myTabsSorted.push(tab);
            }
        }
        //this.ObjectTableTabs = myTabsSorted;
        myTabsSorted.forEach(function (item) {
            var itemTab = new TabItem(item);
            itemTab.IsDisabled = _this.EditComponentController.IsDisabled(itemTab.Code);
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                if (item.ControlPath.indexOf("Doc") > -1) {
                    switch (_this.ObjectTableName) {
                        case "ARInvoice":
                        case "APInvoice":
                        case "ARPayment":
                        case "APPayment":
                            {
                                itemTab.IsDisabled = true;
                                break;
                            }
                    }
                }
            }
            _this.TabsItemsSource.push(itemTab);
        });
    };
    EditComponent.prototype.FilterTabs = function (allTabs) {
        switch (this.ObjectTableName) {
            case "Master":
            case "Shipment":
                {
                    // SHCO: Shipment Consolidations
                    if (this.EntityPM.ShipmentLevelCode == "H" || this.EntityPM.ShipmentLevelCode == "D") {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHCO"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    // SHMS: Shipment Master
                    if (this.EntityPM.ShipmentLevelCode != "H") {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHMS"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    // SHCF: Customs File
                    if (this.EntityPM.ShipmentLevelCode != "D" && this.EntityPM.ShipmentLevelCode != "H") {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHCF"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    //Customs
                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHCT"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    else {
                        if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                            if (!ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                                var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHCT"; });
                                if (indexOfTab > -1) {
                                    allTabs.splice(indexOfTab, 1);
                                }
                            }
                        }
                    }
                    // SHFF: Freight Files
                    if (this.EntityPM.ShipmentLevelCode != "A") {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHFF"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    // MHGC: Master General
                    // SHGC: Shipment General
                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "SHGC"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    else {
                        var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "MHGC"; });
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    break;
                }
            case "User": {
                if (SessionLocator_1.SessionLocator.Tenant != 0) {
                    var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "USDS"; });
                    if (indexOfTab > -1) {
                        allTabs.splice(indexOfTab, 1);
                    }
                }
                break;
            }
            case "Customs.PaymentOrder": {
                if (!this.EntityPM.HasDeficit) {
                    var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "PODF"; });
                    if (indexOfTab > -1) {
                        allTabs.splice(indexOfTab, 1);
                    }
                }
                if (!this.EntityPM.HasDeposit) {
                    var indexOfTab = allTabs.findIndex(function (t) { return t.Code == "PODP"; });
                    if (indexOfTab > -1) {
                        allTabs.splice(indexOfTab, 1);
                    }
                }
                break;
            }
            case "Customs.Declaration": {
                //CustomsSettingList customsSetting = DataProvider.GetCachedList<CustomsSettingList>("Customs.CustomsSetting").FirstOrDefault();
                //if (customsSetting != null) {
                //    if (customsSetting.IsConnectedToUniFreight) {
                //        tabItem = objectTableTabs.Where(t => t.Code == "DCMF").FirstOrDefault();
                //        objectTableTabs.Remove(tabItem);
                //    }
                //}
                break;
            }
            case "ARPayment": {
                if (this.EntityPM.IsFullAccounting) {
                    var indexOfTab_1 = allTabs.findIndex(function (t) { return t.Code == 'ARPD'; });
                    if (indexOfTab_1 > -1)
                        allTabs.splice(indexOfTab_1, 1);
                }
                else {
                    var indexOfTab_2 = allTabs.findIndex(function (t) { return t.Code == 'PYDF'; });
                    if (indexOfTab_2 > -1)
                        allTabs.splice(indexOfTab_2, 1);
                }
                break;
            }
        }
        return allTabs;
    };
    EditComponent.prototype.OnEntityCreated = function () {
        switch (this.ObjectTableName) {
            case "ARInvoice":
            case "APInvoice":
            case "ARPayment":
            case "APPayment":
                {
                    if (this.EntityPM) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                            this.TabsItemsSource.forEach(function (item) {
                                if (item.EntityPM.ControlPath.indexOf("Doc") > -1) {
                                    item.IsDisabled = false;
                                }
                            });
                        }
                    }
                    break;
                }
        }
    };
    EditComponent.prototype.SetSelectedTab = function () {
        var _this = this;
        if (this.IsTabsHidden) {
            if (this.SingleDetailsTab) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditTabComponent", this.TabControlBodyViewContainerRef)
                    .then(function (cmpRef) {
                    if (_this.PreSelectedTabCode != null) {
                        _this.entityArgs.PreSelectedTabCode = _this.PreSelectedTabCode;
                    }
                    cmpRef.instance.CurrentlySelected = true;
                    cmpRef.instance.Run(_this.SingleDetailsTab.Code, _this.SingleDetailsTab.HtmlComponentUrl);
                });
            }
        }
        else {
            if (this.TabsItemsSource != null) {
                var selected = null;
                if (this.PreSelectedTabCode != null) {
                    selected = this.TabsItemsSource.filter(function (d) { return d.Code == _this.PreSelectedTabCode; })[0];
                }
                if (selected == null) {
                    selected = this.TabsItemsSource[0];
                }
                this.SelectionChanged(selected);
            }
        }
    };
    EditComponent.prototype.SelectionChanged = function (mySelectedTab) {
        var _this = this;
        if (this.SelectedTab != mySelectedTab) {
            this.SelectedTab = mySelectedTab;
            if (this.LoadedTabsList == null) {
                this.LoadedTabsList = [];
            }
            this.LoadedTabsList.forEach(function (item) {
                if (item.EditTabComponent) {
                    item.EditTabComponent.Selected = false;
                    item.EditTabComponent.CurrentlySelected = false;
                }
            });
            var myLoadedTabItem = this.LoadedTabsList.filter(function (d) { return d.Code == mySelectedTab.Code; })[0];
            if (myLoadedTabItem == null) {
                myLoadedTabItem = new LoadedTabItem(mySelectedTab.Code);
                this.LoadedTabsList.push(myLoadedTabItem);
                var myComponentName = null;
                var myComponentPath = null;
                switch (mySelectedTab.EntityPM.ControlPath) {
                    case "Simplog.Infrastructure.GeneralControls.GeneralTabControl": {
                        if (!Tools_1.AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
                        }
                        else {
                            myComponentName = "GeneralTabComponent";
                            myComponentPath = "./Infrastructure/GenericComponents/GeneralTabComponent";
                        }
                        break;
                    }
                    case "Simplog.Infrastructure.GeneralControls.BillingTabControl": {
                        if (!Tools_1.AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
                        }
                        else {
                            myComponentName = "BillingTabComponent";
                            myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/BillingTabComponent";
                        }
                        break;
                    }
                    case "Simplog.Infrastructure.Views.Events.EventsControl": {
                        myComponentName = "EventsTabComponent";
                        myComponentPath = "./Common/Components/Events/EventsTabComponent";
                        break;
                    }
                    case "Simplog.Infrastructure.Views.Communications.CommunicationsControl": {
                        myComponentName = "CommunicationsTabComponent";
                        myComponentPath = "./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent";
                        break;
                    }
                    case "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab": {
                        myComponentName = "ContactsTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent";
                        break;
                    }
                    case "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab": {
                        myComponentName = "AddressesTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/AddressesTabComponent";
                        break;
                    }
                    default: {
                        if (!Tools_1.AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
                        }
                        break;
                    }
                }
                if (myComponentPath != null) {
                    myLoadedTabItem.ComponentName = myComponentName;
                    myLoadedTabItem.ComponentPath = myComponentPath;
                    this.LoadTabComponent(myLoadedTabItem);
                }
            }
            else {
                this.TabSelected.emit(mySelectedTab.Code);
                if (myLoadedTabItem.EditTabComponent) {
                    myLoadedTabItem.EditTabComponent.Selected = true;
                    myLoadedTabItem.EditTabComponent.CurrentlySelected = true;
                }
            }
            if (this.SelectedTab.Code != "SHOV") {
                var myTab = window.ObjectTableTabs.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && d.Code === _this.SelectedTab.Code; })[0];
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(myTab.ObjectTableName, myTab.TabNameTextCodeDefaultText + " Tab View");
            }
            this.TabChanged.emit(mySelectedTab.Code);
        }
    };
    EditComponent.prototype.LoadTabComponent = function (loadedItem) {
        var _this = this;
        if (loadedItem != null) {
            if (loadedItem.ComponentPath != null) {
                if (!loadedItem.IsLoaded) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditTabComponent", this.TabControlBodyViewContainerRef)
                        .then(function (cmpRef) {
                        loadedItem.IsLoaded = true;
                        loadedItem.EditTabComponent = cmpRef.instance;
                        if (_this.SelectedTab.Code == loadedItem.Code) {
                            loadedItem.EditTabComponent.CurrentlySelected = true;
                        }
                        cmpRef.instance.Run(loadedItem.Code, loadedItem.ComponentPath);
                    });
                }
            }
        }
    };
    //private LoadTabComponent(loadedItem: LoadedTabItem) {
    //    if (loadedItem != null) {
    //        if (loadedItem.ComponentPath != null) {
    //            if (!loadedItem.IsLoaded) {
    //                let locs = this.AllLocations.toArray().filter(f => f.Code == 'EditTabLocation');
    //                let myLocation: LocationDirective = locs.filter(f => f.ItemCode == loadedItem.Code)[0];
    //                if (myLocation != null) {
    //                    SessionLocator.DynamicLoader.Load(loadedItem.ComponentPath, myLocation.viewContainerRef)
    //                        .then(cmpRef => {
    //                            loadedItem.IsLoaded = true;
    //                        });
    //                }
    //            }
    //        }
    //    }
    //}
    // Commands
    EditComponent.prototype.BackButtonClicked = function () {
        var _this = this;
        var isNeedingConfirmation = this.NeedCloseConfirmation();
        if (isNeedingConfirmation) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName)));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.SaveEntityChanges(true);
                }
                else if (confirmWindow.No) {
                    _this.Close();
                }
            });
        }
        else {
            this.Close();
        }
    };
    EditComponent.prototype.NeedCloseConfirmation = function () {
        var myResult = true;
        if (!this.EntityPM) {
            myResult = false;
        }
        else if (!this.EntityPM.IsDirty) {
            myResult = false;
        }
        else if (this.ObjectTableName == "TaxReport" || this.ObjectTableName == "BankDeposit") {
            myResult = false;
        }
        return myResult;
    };
    EditComponent.prototype.Close = function () {
        if (this.IsInsideWindow) {
            this.CurrentSession.CloseCurrentWindow();
        }
        if (this.EditComponentController) {
            this.EditComponentController.OnCloseEditControl();
        }
        this.DestroyEditControl();
        this.BackCompleted.emit(true);
    };
    EditComponent.prototype.SaveChanges = function (busyIndicatorText) {
        if (busyIndicatorText === void 0) { busyIndicatorText = null; }
        if (this.EntityPM.IsDirty) {
            if (this.IsEditValid) {
                this.SaveEntityChanges(false, busyIndicatorText);
            }
        }
        else {
            this.FireSaveCompleted(true);
        }
    };
    EditComponent.prototype.SaveChangesAndClose = function () {
        this.SaveEntityChanges(true);
    };
    EditComponent.prototype.SaveEntityChanges = function (isClosing, busyIndicatorText, loadNextEntity, loadPreviousEntity) {
        var _this = this;
        if (busyIndicatorText === void 0) { busyIndicatorText = null; }
        if (loadNextEntity === void 0) { loadNextEntity = false; }
        if (loadPreviousEntity === void 0) { loadPreviousEntity = false; }
        if (this.EntityPM.IsDirty) {
            this.ValidationErrorsList = [];
            if (!Tools_1.AppTool.IsNullOrEmpty(busyIndicatorText)) {
                this.StartBusyIndicator(busyIndicatorText);
            }
            else {
                this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            }
            if ((this.ObjectTableName == "ARInvoice" || this.ObjectTableName == "APInvoice" || this.ObjectTableName == "ARPayment" || this.ObjectTableName == "APPayment"
                || this.ObjectTableName == "BankDeposit" || this.ObjectTableName == "Journal" || this.ObjectTableName == "AccountingIntegrityCheck") && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) { // customs: notification defenetion, new declaration
                this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
                this.entityPMService.insert(this.ObjectTableName, this.EntityPM).then(function (res) {
                    res.subscribe(function (myResponse) {
                        _this.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.OnSavingFailed();
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                            _this.FireSaveCompleted(false);
                        }
                        else {
                            _this.EntityPM = myResponse.Result;
                            _this.EntityId = _this.EntityPM.Id;
                            _this.entityArgs.EntityPM = _this.EntityPM;
                            if (_this.ObjectTable.CacheOnClient) {
                                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                            }
                            if (isClosing) {
                                _this.SaveAndCloseCompleted.emit(true);
                                _this.Close();
                            }
                            else {
                                _this.OnEntityCreated();
                                _this.UpdateComponentMembers();
                                _this.FireSaveCompleted(true);
                                // this is for navigation
                                if (loadNextEntity) {
                                    _this.CurrentNavigatedIndex = _this.CurrentNavigatedIndex + 1;
                                    _this.LoadNextPreviousEntity();
                                }
                                if (loadPreviousEntity) {
                                    _this.CurrentNavigatedIndex = _this.CurrentNavigatedIndex - 1;
                                    _this.LoadNextPreviousEntity();
                                }
                                if (_this.nextPreviousTimerToken) {
                                    clearTimeout(_this.nextPreviousTimerToken);
                                }
                                _this.nextPreviousTimerToken = setTimeout(function () { return _this.SetNextPreviousButtonsEnablity(); }, 500);
                            }
                            _this.EditComponentController.HaveSaved = true;
                        }
                    }, function (error) {
                        _this.OnSavingFailed();
                        _this.StopBusyIndicator();
                        var myErrors = [];
                        myErrors.push(error.message);
                        _this.ValidationErrorsList = myErrors;
                        _this.FireSaveCompleted(false);
                    });
                });
            }
            else {
                this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "Edit " + this.ObjectTableName);
                this.entityPMService.update(this.ObjectTableName, this.EntityPM).then(function (res) {
                    res.subscribe(function (myResponse) {
                        _this.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.OnSavingFailed();
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                            _this.FireSaveCompleted(false);
                        }
                        else {
                            _this.EntityPM = myResponse.Result;
                            _this.entityArgs.EntityPM = _this.EntityPM;
                            if (_this.ObjectTable.CacheOnClient) {
                                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                            }
                            if (isClosing) {
                                _this.SaveAndCloseCompleted.emit(true);
                                _this.Close();
                            }
                            else {
                                _this.UpdateComponentMembers();
                                _this.FireSaveCompleted(true);
                                // this is for navigation
                                if (loadNextEntity) {
                                    _this.CurrentNavigatedIndex = _this.CurrentNavigatedIndex + 1;
                                    _this.LoadNextPreviousEntity();
                                    if (_this.nextPreviousTimerToken) {
                                        clearTimeout(_this.nextPreviousTimerToken);
                                    }
                                    _this.nextPreviousTimerToken = setTimeout(function () { return _this.SetNextPreviousButtonsEnablity(); }, 500);
                                }
                                if (loadPreviousEntity) {
                                    _this.CurrentNavigatedIndex = _this.CurrentNavigatedIndex - 1;
                                    _this.LoadNextPreviousEntity();
                                    if (_this.nextPreviousTimerToken) {
                                        clearTimeout(_this.nextPreviousTimerToken);
                                    }
                                    _this.nextPreviousTimerToken = setTimeout(function () { return _this.SetNextPreviousButtonsEnablity(); }, 500);
                                }
                            }
                        }
                    }, function (error) {
                        _this.OnSavingFailed();
                        _this.StopBusyIndicator();
                        var myErrors = [];
                        myErrors.push(error.message);
                        _this.ValidationErrorsList = myErrors;
                        _this.FireSaveCompleted(false);
                    });
                });
            }
        }
        else {
            this.Close();
        }
    };
    EditComponent.prototype.OnSavingFailed = function () {
        switch (this.ObjectTableName) {
            case "APInvoice": {
                var isDirty = this.EntityPM['IsDirty'];
                this.EntityPM['SetVoided'] = false;
                this.EntityPM['SetApproved'] = false;
                this.EntityPM['SetReTransfer'] = false;
                this.EntityPM['SetCancelApproval'] = false;
                this.EntityPM['IsDirty'] = isDirty;
                break;
            }
            case "ARInvoice": {
                var isDirty = this.EntityPM['IsDirty'];
                this.EntityPM['SetVoided'] = false;
                this.EntityPM['SetAsSent'] = false;
                this.EntityPM['SetApproved'] = false;
                this.EntityPM['SetReTransfer'] = false;
                this.EntityPM['SetCancelDraft'] = false;
                this.EntityPM['IsDirty'] = isDirty;
                break;
            }
        }
    };
    EditComponent.prototype.ReloadEntityPM = function () {
        var _this = this;
        if (this.EntityId) {
            this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
            this.entityPMService.getSingle(this.ObjectTableName, this.EntityId).then(function (res) {
                res.subscribe(function (myResponse) {
                    //this.StopBusyIndicator();
                    if (myResponse.HasError) {
                        _this.StopBusyIndicator();
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.LoadCompleted.emit(false);
                    }
                    else {
                        _this.EntityPM = myResponse.Result;
                        _this.entityArgs.EntityPM = _this.EntityPM;
                        _this.EditComponentController.OnReloadEntityPM().then(function (isLock) {
                            _this.StopBusyIndicator();
                            _this.UpdateComponentMembers();
                            _this.LoadCompleted.emit(true);
                        });
                    }
                });
            });
        }
    };
    EditComponent.prototype.UpdateComponentMembers = function () {
        //this.BuildHelperControl();
        //this.BuildMenuButtons();
        this.BuildHeaderScreen();
    };
    Object.defineProperty(EditComponent.prototype, "BusyIndicatorText", {
        get: function () { return this.busyIndicatorText; },
        set: function (value) {
            if (this.busyIndicatorText != value) {
                this.busyIndicatorText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditComponent.prototype, "ShowBusyIndicator", {
        get: function () { return this.showBusyIndicator; },
        set: function (value) {
            if (this.showBusyIndicator != value) {
                this.showBusyIndicator = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditComponent.prototype.StartBusyIndicator = function (myText) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    };
    EditComponent.prototype.StopBusyIndicator = function () {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    };
    EditComponent.prototype.GetControllerByTableName = function (objectTableName) {
        var notDefault = ["Declaration", "Vehicle"];
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "EditComponentController";
        var servicelink = './' + moduleName + '/Controller/' + servicename;
        return new Promise(function (resolve) {
            if (notDefault.indexOf(objectTableName) > -1) {
                SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink, true).then(function (service) {
                    resolve(service);
                    //}).catch((rejectReson) => {
                    //    var myEditComponentDefaultController = new EditComponentDefaultController()
                    //    resolve(myEditComponentDefaultController);
                });
            }
            else {
                var myEditComponentDefaultController = new EditComponentDefaultController();
                resolve(myEditComponentDefaultController);
            }
        });
    };
    EditComponent.prototype.StartBusyIndicatorSaving = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
    };
    EditComponent.prototype.StartBusyIndicatorLoading = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
    };
    EditComponent.prototype.FireSaveCompleted = function (isSaveSuccess) {
        this.SaveCompleted.emit(isSaveSuccess);
        //Abed Code
        if (this.ObjectTableName == "Shipment" && this.EntityPM.IsRefreshFollowUp) {
            this.EntityPM.IsRefreshFollowUp = false;
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    EditComponent.prototype.SubscriptionAdd = function (teardown) {
        //    this.someService.change.subscribe(() => {
        //[...]
        //    })
        this._Subscription.add(teardown);
    };
    EditComponent.prototype.DestroyEditControl = function () {
        if (this.ComponentRef != null) {
            this.CurrentSession.RemoveEditComponent(this);
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    };
    EditComponent.prototype.ngOnDestroy = function () {
        console.log("EditComp:ngOnDestroy");
        this.LoadedTabsList.forEach(function (item) {
            if (item.EditTabComponent) {
                item.EditTabComponent = null;
            }
        });
        this.LoadedTabsList = null;
        this.CurrentSession.UnsubscribeStaticEvent();
        this._Subscription.unsubscribe(); //itzik
        if (this._SubEditComponentDefaultController) {
            this._SubEditComponentDefaultController.unsubscribe();
            this._SubEditComponentDefaultController = null;
        }
        if (this.MenuButtonsHandlerREF && this.MenuButtonsHandlerREF.ngOnDestroy) {
            this.MenuButtonsHandlerREF.ngOnDestroy();
            this.MenuButtonsHandlerREF = null;
        }
    };
    EditComponent.prototype.SplitButtonClicked = function () {
        var _this = this;
        this.IsSplitComponentOpened = !this.IsSplitComponentOpened;
        this.cd.detectChanges(); // to let HTML read split component location
        if (this.IsSplitComponentOpened == true) {
            this.token = setTimeout(function () {
                _this.LoadSplitComponent();
            }, 100);
        }
        //// show and hide component with slide animation
        //if (this.IsSplitComponentOpened == true) {
        //    this.SplitWidth = 0;
        //    this.token = setTimeout(() => {
        //        this.IsSplitComponentOpened = false;
        //    }, 500);
        //} else {
        //    this.SplitWidth = 870;
        //    this.IsSplitComponentOpened = true;
        //    this.cd.detectChanges();
        //    this.LoadSplitComponent();
        //}
        // update opened/closed state
        LastFilterClass_1.LastFilterClass.UpdateFilter("DeclarationEditControl", this.EntityPM.Id, this.IsSplitComponentOpened ? "true" : "false");
    };
    EditComponent.prototype.LoadSplitComponent = function () {
        var _this = this;
        var locs = this.AllLocations.toArray();
        var myLocation = locs.filter(function (f) { return f.Code == 'SplitComponentLocation'; })[0];
        console.log("Load SplitComponent @ ", myLocation);
        if (myLocation) {
            //this.myLocation.clear();
            var splitComponentPath = this.ObjectTable.SplitComponentPath;
            //var splitComponentPath = "./Customs/AngularModules/AngularModules/Customs/Components/Declaration/DeclarationSplitComponent";
            SessionLocator_1.SessionLocator.DynamicLoader.Load(splitComponentPath, myLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.SetComponentArgs({ EntityPM: _this.EntityPM });
            });
        }
    };
    EditComponent.prototype.SetSplitComponentState = function () {
        if (this.IsSplitBtnVisible == false)
            return;
        // state: opened / closed
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue("DeclarationEditControl", this.EntityPM.Id);
        if (defaultFilterCode == "true") {
            this.SplitButtonClicked(); // open split section
        }
    };
    EditComponent.prototype.Next = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false, null, true);
        }
        else {
            this.CurrentNavigatedIndex = this.CurrentNavigatedIndex + 1;
            this.LoadNextPreviousEntity();
            if (this.nextPreviousTimerToken) {
                clearTimeout(this.nextPreviousTimerToken);
            }
            this.nextPreviousTimerToken = setTimeout(function () { return _this.SetNextPreviousButtonsEnablity(); }, 500);
        }
    };
    EditComponent.prototype.Previous = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false, null, false, true);
        }
        else {
            this.CurrentNavigatedIndex = this.CurrentNavigatedIndex - 1;
            this.LoadNextPreviousEntity();
            if (this.nextPreviousTimerToken) {
                clearTimeout(this.nextPreviousTimerToken);
            }
            this.nextPreviousTimerToken = setTimeout(function () { return _this.SetNextPreviousButtonsEnablity(); }, 500);
        }
    };
    EditComponent.prototype.LoadNextPreviousEntity = function () {
        this.NextButtonDisabled = true;
        this.PreviousButtonDisabled = true;
        this.cd.detectChanges();
        this.TabsItemsSource = [];
        this.LoadedTabsList.forEach(function (tab) {
            tab.EditTabComponent.DestroyCurrentTab();
        });
        this.LoadedTabsList = [];
        this.TabControlBodyViewContainerRef.clear();
        if (this.EditComponentController) {
            this.EditComponentController.OnCloseEditControl();
        }
        //if (this.ComponentRef != null) {
        //  this.CurrentSession.RemoveEditComponent(this);
        //  this.ComponentRef.destroy();
        //  this.ComponentRef = null;
        //}
        this.CurrentSession.RemoveEditComponent(this);
        this.ngOnDestroy();
        var args = {};
        args.EntityId = this.NavigationIds[this.CurrentNavigatedIndex];
        args.ObjectTableName = this.ObjectTableName;
        args.BackButtonLabel = this.BackButtonLabel;
        args.NavigationIds = this.NavigationIds;
        this.Run(args);
    };
    EditComponent.prototype.SetNextPreviousButtonsEnablity = function () {
        if (this.NavigationIds) {
            if (this.CurrentNavigatedIndex == 0) {
                this.PreviousButtonDisabled = true;
            }
            else {
                this.PreviousButtonDisabled = false;
            }
            if (this.CurrentNavigatedIndex == this.NavigationIds.length - 1) {
                this.NextButtonDisabled = true;
            }
            else {
                this.NextButtonDisabled = false;
            }
            this.DeclarationNavigationMessage = (this.CurrentNavigatedIndex + 1).toString() + " מתוך " + this.NavigationIds.length.toString();
        }
    };
    EditComponent.prototype.SetSelectedTabByCode = function (code) {
        if (this.TabsItemsSource != null) {
            var selected = this.TabsItemsSource.filter(function (d) { return d.Code == code; })[0];
            if (selected != null) {
                this.SelectionChanged(selected);
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "BackCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "LoadCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "SaveCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "TabSelected", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "TabChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "SaveAndCloseCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditComponent.prototype, "OnFirstTimeAfterSingleDataLoaded", void 0);
    __decorate([
        core_1.ViewChild('Helper', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditComponent.prototype, "HelperViewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('ShortTitle', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditComponent.prototype, "ShortTitleViewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('MenuButtons', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditComponent.prototype, "MenuButtonsViewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('SplitComponentLocation', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditComponent.prototype, "SplitComponentViewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('WindowLocation', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditComponent.prototype, "WindowLocationViewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('TabControlBody', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditComponent.prototype, "TabControlBodyViewContainerRef", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], EditComponent.prototype, "AllLocations", void 0);
    EditComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityPMService_1.EntityPMService, EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService, TotangoService_1.TotangoService, core_1.ChangeDetectorRef])
    ], EditComponent);
    return EditComponent;
}());
exports.EditComponent = EditComponent;
var HeaderScreenColumn = /** @class */ (function () {
    function HeaderScreenColumn(isSeparator) {
        this.IsSeparator = false;
        this.Width = "auto";
        this.Rows = [];
        this.LabelWidth = "auto";
        this.ValueMaxWidth = 0;
        this.IsSeparator = isSeparator;
        if (isSeparator) {
            this.Width = "20px";
        }
    }
    return HeaderScreenColumn;
}());
var HeaderScreenRow = /** @class */ (function () {
    function HeaderScreenRow() {
        this.Label = null;
        this.HideField = false;
    }
    return HeaderScreenRow;
}());
var TabItem = /** @class */ (function () {
    function TabItem(itemPM) {
        this.IsDisabled = false;
        this.Code = itemPM.Code;
        this.EntityPM = itemPM;
        this.TextCode = itemPM.TabNameTextCodeCode;
    }
    return TabItem;
}());
var LoadedTabItem = /** @class */ (function () {
    function LoadedTabItem(myCode) {
        this.IsLoaded = false;
        this.Code = myCode;
    }
    return LoadedTabItem;
}());
var EditComponentDefaultController = /** @class */ (function () {
    function EditComponentDefaultController() {
    }
    EditComponentDefaultController.prototype.OnFirstTimeAfterSingleDataLoaded = function (CurrentEntity) {
        return new Promise(function (resolve, reject) {
            resolve(false);
        });
    };
    EditComponentDefaultController.prototype.OnReloadEntityPM = function () {
        return new Promise(function (resolve, reject) {
            resolve();
        });
    };
    EditComponentDefaultController.prototype.OnCloseEditControl = function () {
    };
    EditComponentDefaultController.prototype.ResetMustRefresh = function () { };
    ;
    EditComponentDefaultController.prototype.IsDisabled = function (itemTabCode) {
        return false;
    };
    return EditComponentDefaultController;
}());
exports.EditComponentDefaultController = EditComponentDefaultController;
//# sourceMappingURL=EditComponent.js.map