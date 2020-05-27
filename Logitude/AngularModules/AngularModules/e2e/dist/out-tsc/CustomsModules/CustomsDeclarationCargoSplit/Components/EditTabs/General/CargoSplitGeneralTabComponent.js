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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var LogTabsComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationCargoSplitPM_1 = require("../../../../../Customs/EntityPMs/DeclarationCargoSplitPM");
var DecCargoSplitCargoIdentifierPM_1 = require("../../../../../Customs/EntityPMs/DecCargoSplitCargoIdentifierPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var DeclarationExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
//import {INF_MSG_GenericResponseData} from '../../DataContract/ResponseData/INF_MSG_GenericResponseData';//4
var IIGGeneralMessagesService_1 = require("../../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var DeclarationCargoSplitController_1 = require("../../Controller/DeclarationCargoSplitController");
var DeclarationCargoSplitPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService");
var DeclarationMessagesService_1 = require("../../../../../Customs/Services/WebServices/DeclarationMessagesService");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var CargoSplitRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/CargoSplitRequestParams");
var INF_MSG_GenericResponseData_1 = require("../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var DecCargoSplitConPM_1 = require("../../../../../Customs/EntityPMs/DecCargoSplitConPM");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var EntityListService_1 = require("../../../../../Infrastructure/Services/EntityListService");
//import {DecCargoSplitConComponent} from '../DecCargoSplitConComponent';
var CargoSplitGeneralTabComponent = /** @class */ (function (_super) {
    __extends(CargoSplitGeneralTabComponent, _super);
    function CargoSplitGeneralTabComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.FillValidationErrorList = new core_1.EventEmitter();
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationCargoSplit";
        _this.TabsItemsSource = [];
        _this.Tabs = [];
        _this.IsNewEntity = false;
        _this.IsDisplayOnly = false;
        _this.DisplayOnlyMessage = "";
        _this.ImporterCode = "";
        _this.IsCustomsFileRetrieved = false;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.IsDelete = false;
        _this.SendButtonEnabled = true;
        _this.OKButtonEnabled = true;
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.declarationCargoSplitPMService = new DeclarationCargoSplitPMService_1.DeclarationCargoSplitPMService();
        _this.declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.requestParams = new CargoSplitRequestParams_1.CargoSplitRequestParams();
        _this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        _this.XrayItems = [];
        //@Input()
        //set DeclarationCargoSplitParam(val: EntityArgs) {
        //    this.entityArgs = this._InputParam = val;
        //    this.Init();
        //}
        _this._EntityResourceFinished = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SendButtonsVisibility = false;
        _this.SelectedRow = null;
        _this.EntityPM = new DeclarationCargoSplitPM_1.DeclarationCargoSplitPM();
        _this.ItemsList = new ObservableCollection_1.ObservableCollection([]);
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //this.entityArgs.ObjectTableName = "Customs.DeclarationCargoSplit";
        _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitCargoIdentifier").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitCon").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitConsItem").subscribe(function (response) {
                            _this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitConsPackDet").subscribe(function (response) {
                                _this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(function (response) {
                                    //this.Init();
                                    //this.EntityPM = this.entityArgs.EntityPM;
                                    //this.ObjectTableName = this.entityArgs.ObjectTableName;
                                    _this._EntityResourceFinished = true;
                                    _this.Listen();
                                    //this.BuildTabs();
                                });
                            });
                        });
                    });
                });
            });
        });
        _this.declarationCargoSplitController = new DeclarationCargoSplitController_1.DeclarationCargoSplitController(_this.EntityPM);
        _this.CargoIdentifiersList = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM) && _this.EntityPM.DecCargoSplitCargoIdentifiers != null && _this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
            _this.CargoIdentifiersList.InsertCollection(_this.EntityPM.DecCargoSplitCargoIdentifiers);
        }
        _this._entityListService = new EntityListService_1.EntityListService();
        if (_this.IsDisplayOnly) {
            _this.SetDisplayFields(_this.ResponseStatusCode);
        }
        return _this;
    }
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "EntityPM", {
        get: function () { return this.entityPM; },
        set: function (val) {
            this.entityPM = val;
            this.BuildTabs();
        },
        enumerable: true,
        configurable: true
    });
    CargoSplitGeneralTabComponent.prototype.GetFileData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo))
            return;
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myDeclarationResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myDeclarationResponse.Result == null || (myDeclarationResponse.Result != null && Tools_1.AppTool.IsNullOrEmpty(myDeclarationResponse.Result.Id))) {
                _this.CustomFileNo = "";
                _this.EntityPM.DeclarationId = null;
            }
            _this._LastFetchDeclarationList = myDeclarationResponse.Result;
            if (Tools_1.AppTool.IsNullOrEmpty(_this._LastFetchDeclarationList)) {
                _this.NoConnectedConsignmentEnableField();
            }
            else {
                _this.CurrentSession.StartBusyIndicator("");
                _this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(_this.CustomFileNo)
                    .subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.FetchConsignment(myResponse, false);
                });
            }
        });
    };
    CargoSplitGeneralTabComponent.prototype.Init = function () {
        this.SetDisplayFields(this.ResponseStatusCode);
        this.GetFileData();
        this.InitCargoIdentifiers();
        //if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
        //    if (this.EntityPM != null && this.entityArgs != null) this.entityArgs.EntityPM = this.EntityPM;
        //    return;
        //}
        //if (this.EntityPM == null)this.EntityPM = this.entityArgs.EntityPM;
        //this.ObjectTableName = this.entityArgs.ObjectTableName;
    };
    CargoSplitGeneralTabComponent.prototype.SetDisplayFields = function (ResponseStatusCode) {
        this.UIProperties.SetEnabled("RequestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ResponseStatusCode", this.ObjectTableName, false);
        //if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
        //    if (this.IsNewEntity == false) return;
        //    if (this.RequestDate == null) this.RequestDate = DateTool.GetDateByDay(+0);
        //    return;
        //}
        if (this.ResponseStatusCode != "5" && this.ResponseStatusCode != "" && this.ResponseStatusCode != null) {
            this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestRemarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestReason", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, false);
            this.OKButtonEnabled = false;
            if (this.ResponseStatusCode != "3") {
                this.UIProperties.SetEnabled("ActionTypeCode", this.ObjectTableName, false);
                this.SendButtonEnabled = false;
            }
            else {
                this.UIProperties.SetEnabled("ActionTypeCode", this.ObjectTableName, true);
                this.SendButtonEnabled = true;
            }
        }
        else {
            this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestRemarks", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestReason", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ActionTypeCode", this.ObjectTableName, true);
            this.SendButtonEnabled = true;
            this.OKButtonEnabled = true;
        }
        if (this.SendButtonEnabled != true || this.OKButtonEnabled != true) {
            this.IsDisplayOnly = true;
        }
    };
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    CargoSplitGeneralTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        if (this.IsDisplayOnly != true && args.IsDisplayOnly == true) {
            this.IsDisplayOnly = args.IsDisplayOnly;
        }
        console.log("EntityPM", this.EntityPM);
    };
    //ngAfterViewInit() {
    //    if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
    //        if (this.IsNewEntity == false) return;
    //        if (this.RequestDate == null) {
    //            this.RequestDate = DateTool.GetDateByDay(+0);
    //        }
    //        else {
    //            this.RequestDate = this.RequestDate;
    //        }
    //        return;
    //    }
    //    // viewChildren is set
    //   // this.SetByAvailableTimeChecked();
    //}
    //ngAfterContentInit()
    //{
    //    if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
    //        if (this.IsNewEntity == false) return;
    //        if (this.RequestDate == null) {
    //            this.RequestDate = DateTool.GetDateByDay(+0);
    //        }
    //        else {
    //            this.RequestDate = this.RequestDate;
    //        }
    //        return;
    //    }
    //    //this.RequestDate = DateTool.GetDateByDay(+0);
    //    //this.ToDate = DateTool.GetDateByDay(+7);
    //    //this.SetByAvailableTimeChecked()
    //    //alert(this.AllDates);
    //    //this.UIProperties.SetRequired("RequestDate", this.ObjectTableName, true);
    //    //this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
    //}
    CargoSplitGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.RefreshEntity();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildTabs();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.currentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEGC") {
                        //this.RefreshEntity();
                        _this.DisplayOnlyCheck();
                    }
                }
            }));
        }
    };
    CargoSplitGeneralTabComponent.prototype.DisplayOnlyCheck = function () {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetDisplayFields(this.ResponseStatusCode);
            return;
        }
    };
    CargoSplitGeneralTabComponent.prototype.SetNewWizardArgs = function (args) {
        this.IsNewEntity = args['IsNewEntity'];
        if (this.IsDisplayOnly != true && args.IsDisplayOnly == true) {
            this.IsDisplayOnly = args.IsDisplayOnly;
        }
        this.Init();
        this.BuildTabs();
        this.RequestDate = Tools_1.DateTool.GetDateByDay(+0);
    };
    CargoSplitGeneralTabComponent.prototype.SetWindowArgs = function (winArg) {
        this.EntityPM = winArg.CurrentEntity;
        this.Init();
        if (Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode)) {
            if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
                this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
            }
            else if (this.IsCustomsFileRetrieved != true) {
                this.CustomFileNoTextChanged("ImporterOnly");
            }
        }
        this.BuildTabs();
        if (this.IsDisplayOnly != true && winArg.IsDisplayOnly == true) {
            this.IsDisplayOnly = winArg.IsDisplayOnly;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DecCargoSplitCargoIdentifiers)) {
            for (var _i = 0, _a = this.EntityPM.DecCargoSplitCargoIdentifiers; _i < _a.length; _i++) {
                var conItem = _a[_i];
                var item = new DecCargoSplitCargoIdentifierModel(conItem);
                this.ItemsList.Insert(item);
            }
        }
        //this.EntityPM = winArg.declarationPM;
    };
    CargoSplitGeneralTabComponent.prototype.AddItem = function () {
        if (!this.IsDisplayOnly) {
            var counter = 0;
            if (this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
                var items = this.EntityPM.DecCargoSplitCargoIdentifiers.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
                if (items.length == 0)
                    counter = 0;
                else {
                    counter = items[this.EntityPM.DecCargoSplitCargoIdentifiers.length - 1].LineNumber;
                }
            }
            counter += 1;
            var item = new DecCargoSplitCargoIdentifierPM_1.DecCargoSplitCargoIdentifierPM(this.EntityPM);
            item.DeclarationCargoSplitId = this.EntityPM.Id;
            item.Tenant = this.EntityPM.Tenant;
            item.LineNumber = counter;
            if (!this.EntityPM.DecCargoSplitCargoIdentifiers.includes(item)) {
                this.EntityPM.AddDecCargoSplitCargoIdentifier(item);
                var line = new DecCargoSplitCargoIdentifierModel(item);
                this.ItemsList.Insert(line);
            }
        }
    };
    CargoSplitGeneralTabComponent.prototype.OnRowEnded = function ($event) {
        console.log("this.ItemsList.Length : " + this.ItemsList.Length);
        if (($event) == this.ItemsList.Length) {
            this.AddItem();
        }
    };
    CargoSplitGeneralTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
        if (this.ItemsList.Length == 0) {
            this.AddItem();
        }
    };
    CargoSplitGeneralTabComponent.prototype.OnFocus = function () {
        if (this.ItemsList.Length == 0) {
            this.AddItem();
        }
    };
    CargoSplitGeneralTabComponent.prototype.DeleteButtonClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeletePackage"));
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.DeleteSelected(item);
            }
            else if (confirmWindow.No) {
            }
        });
    };
    CargoSplitGeneralTabComponent.prototype.DeleteSelected = function (item) {
        this.lastDeletedItem = item.EntityPM;
        this.ItemsList.Remove(item);
        this.EntityPM.RemoveDecCargoSplitCargoIdentifier(item.EntityPM);
    };
    CargoSplitGeneralTabComponent.prototype.BuildTabs = function () {
        var tab;
        this.Tabs = [];
        if (this.EntityPM.DecCargoSplitCons.length > 0) {
            var items = this.EntityPM.DecCargoSplitCons.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
            this.TabIndex = 0;
            for (var _i = 0, items_1 = items; _i < items_1.length; _i++) {
                var item = items_1[_i];
                tab = new LogTabsComponent_1.LogTab();
                tab.EntityPM = item;
                //tab.Code = item.LineNumber;
                //tab.Header = item.LineNumber;
                tab.Code = ++this.TabIndex;
                tab.Header = this.TabIndex;
                tab.Parent = this.EntityPM;
                tab.IsDisplayOnly = this.IsDisplayOnly;
                tab.ComponentPath = "./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/DecCargoSplitConComponent";
                this.Tabs.push(tab);
            }
        }
        else {
            this.AddTab(null);
        }
        this.SelectedTab = this.Tabs[0];
    };
    CargoSplitGeneralTabComponent.prototype.AddTab = function (event) {
        if (this.IsDisplayOnly) {
            return;
        }
        this.TabIndex = 0;
        if (this.Tabs.length > 0) {
            //var maxObj = this.EntityPM.DecCargoSplitCons.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current });
            var maxObj = this.Tabs.reduce(function (prev, current) { return (prev.Code > current.Code) ? prev : current; });
            var code = parseInt(maxObj.Code);
            if (maxObj != null) {
                if (this.TabIndex <= code)
                    this.TabIndex = code;
            }
        }
        var Tab = new DecCargoSplitConPM_1.DecCargoSplitConPM(this.EntityPM);
        Tab.DeclarationCargoSplitId = this.EntityPM.Id;
        Tab.Tenant = this.EntityPM.Tenant;
        //Tab.LineNumber = ++this.TabIndex;
        ++this.TabIndex;
        Tab.LineNumber = (Tools_1.ArrayTool.Max(this.EntityPM.DecCargoSplitCons, "LineNumber") + 1);
        this.EntityPM.AddDecCargoSplitCon(Tab);
        // new tab
        if (Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode)) {
            if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
                this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
            }
            else if (this.IsCustomsFileRetrieved != true) {
                this.CustomFileNoTextChanged("ImporterOnly");
            }
        }
        var tab = new LogTabsComponent_1.LogTab();
        Tab.ImporterCode = this.ImporterCode;
        tab.EntityPM = Tab;
        //tab.Code = Tab.LineNumber.toString();
        //tab.Header = Tab.LineNumber.toString();
        tab.Code = this.TabIndex.toString();
        tab.Header = this.TabIndex.toString();
        tab.Parent = this.EntityPM;
        tab.ComponentPath = "./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/DecCargoSplitConComponent";
        this.Tabs.push(tab);
        // select the tab
        this.SelectedTab = tab;
    };
    CargoSplitGeneralTabComponent.prototype.DeleteTab = function (tab) {
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
                    var index = _this.Tabs.indexOf(tab);
                    if (index < 0) {
                        console.log("The tab was not found, could not delete it :( ", tab);
                        return;
                    }
                    _this.EntityPM.RemoveDecCargoSplitCon(tab.EntityPM);
                    _this.Tabs.splice(index, 1);
                    for (var i = 0; i < _this.EntityPM.DecCargoSplitCons.length; i++) {
                        var Tab = _this.EntityPM.DecCargoSplitCons[i];
                        //Tab.LineNumber = i + 1;
                    }
                    for (var i = 0; i < _this.Tabs.length; i++) {
                        var DecCargoSplitConTab = _this.Tabs[i].EntityPM;
                        //DecCargoSplitConTab.LineNumber = i + 1;
                        //this.Tabs[i].Code = DecCargoSplitConTab.LineNumber.toString();
                        //this.Tabs[i].Header = DecCargoSplitConTab.LineNumber.toString();
                        _this.Tabs[i].Code = (i + 1).toString();
                        _this.Tabs[i].Header = (i + 1).toString();
                    }
                    // select the last tab
                    var tab = _this.Tabs[0];
                    _this.SelectedTab = tab;
                }
            });
        }
    };
    CargoSplitGeneralTabComponent.prototype.OnSelectedChanged = function (tab) {
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
        }
    };
    CargoSplitGeneralTabComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        var errorMessage = "";
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.IsCustomsFileRetrieved = false;
            this.EntityPM.DeclarationId = null;
            this._LastFetchDeclarationList = null;
            this._LastFetchConsignmentPMList = null;
            this.NoConnectedConsignmentEnableField();
        }
        else {
            this.IsCustomsFileRetrieved = true;
            this.CurrentSession.StartBusyIndicator("");
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe(function (myDeclarationResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myDeclarationResponse.Result == null || (myDeclarationResponse.Result != null && Tools_1.AppTool.IsNullOrEmpty(myDeclarationResponse.Result.Id))) {
                    _this.CustomFileNo = "";
                    _this.EntityPM.DeclarationId = null;
                    errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
                    //this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMessage);
                    _this.MessageCustomsFileWindow(errorMessage);
                    //return;
                }
                _this._LastFetchDeclarationList = myDeclarationResponse.Result;
                if (Tools_1.AppTool.IsNullOrEmpty(_this._LastFetchDeclarationList)) {
                    _this.NoConnectedConsignmentEnableField();
                }
                else {
                    _this.CurrentSession.StartBusyIndicator("");
                    _this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(_this.CustomFileNo)
                        .subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (searchtext == "ImporterOnly") {
                            _this._LastFetchConsignmentPMList = myResponse.Result;
                            if (_this._LastFetchConsignmentPMList != null && _this._LastFetchDeclarationList != null) {
                                _this.ImporterCode = _this._LastFetchDeclarationList.ImporterCode;
                                if (_this.SelectedTab != null) {
                                    _this.SelectedTab.EntityPM.ImporterCode = _this.ImporterCode;
                                    _this.SelectedTab.ComponentReference.DataContext.ImporterCode = _this.ImporterCode;
                                }
                            }
                        }
                        else {
                            _this.FetchConsignment(myResponse, false);
                        }
                    });
                }
            });
        }
    };
    CargoSplitGeneralTabComponent.prototype.NoConnectedConsignmentEnableField = function () {
        //this.ImporterCode = "";
        //this.CargoTypeCode = "";
        //this.ManifestNumber = "";
        //this.SecondCargoID = "";
        //this.ThirdCargoID = "";
        this.ImporterCode = "";
        if (this._LastFetchDeclarationList == null)
            this.EntityPM.DeclarationId = null;
    };
    CargoSplitGeneralTabComponent.prototype.FetchConsignment = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        this._LastFetchConsignmentPMList = myResponse.Result;
        if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
            var pm = this._LastFetchConsignmentPMList[0];
            this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
            if (this.SelectedTab != null) {
                this.SelectedTab.ComponentReference.DataContext.ImporterCode = this.ImporterCode;
                this.SelectedTab.EntityPM.ImporterCode = this.ImporterCode;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode)) {
                this.Tabs.forEach(function (tab) {
                    if (Tools_1.AppTool.IsNullOrEmpty(tab.EntityPM.ImporterCode)) {
                        tab.EntityPM.ImporterCode = _this.ImporterCode;
                        tab.ComponentReference.DataContext.ImporterCode = _this.ImporterCode;
                    }
                });
            }
            this.UIProperties.SetRequired("ImporterCode", "Customs.DecCargoSplitCon", Tools_1.AppTool.IsNullOrEmpty(this.SelectedTab.EntityPM.ImporterCode));
            this.CargoTypeCode = pm.CargoTypeCode;
            this.ManifestNumber = pm.ManifestNumber;
            this.SecondCargoID = pm.SecondCargoID;
            this.ThirdCargoID = pm.ThirdCargoID;
            if (this._LastFetchDeclarationList != null)
                this.EntityPM.DeclarationId = this._LastFetchDeclarationList.Id;
        }
        else {
            this.NoConnectedConsignmentEnableField();
        }
    };
    CargoSplitGeneralTabComponent.prototype.MessageCustomsFileWindow = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Width = 300;
        messageWindow.Height = 150;
        messageWindow.Show(message);
    };
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ErrorsList", {
        get: function () { return this.ValidationErrorsList; },
        set: function (val) {
            this.ValidationErrorsList = val;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ActionTypeCode", {
        //public get ActionTypeCode() { return this.EntityPM.ActionTypeCode; }
        get: function () { return this.EntityPM != null ? this.EntityPM.ActionTypeCode : null; },
        set: function (value) { this.EntityPM.ActionTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ActionTypeName", {
        //public get ActionTypeName() { return this.EntityPM.ActionTypeName; }
        get: function () { return this.EntityPM != null ? this.EntityPM.ActionTypeName : null; },
        set: function (value) { this.EntityPM.ActionTypeName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "CargoTypeCode", {
        //public get CargoTypeCode() { return this.EntityPM.CargoTypeCode; }
        get: function () { return this.EntityPM != null ? this.EntityPM.CargoTypeCode : null; },
        set: function (value) { this.EntityPM.CargoTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "CargoTypeName", {
        //public get CargoTypeName() { return this.EntityPM.CargoTypeName; }
        get: function () { return this.EntityPM != null ? this.EntityPM.CargoTypeName : null; },
        set: function (value) { this.EntityPM.CargoTypeName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "CustomFileNo", {
        //public get CustomFileNo() { return this.EntityPM.CustomFileNo; }
        get: function () { return this.EntityPM != null ? this.EntityPM.CustomFileNo : null; },
        set: function (value) { this.EntityPM.CustomFileNo = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ManifestNumber", {
        //public get ManifestNumber() { return this.EntityPM.ManifestNumber; }
        get: function () { return this.EntityPM != null ? this.EntityPM.ManifestNumber : null; },
        set: function (value) { this.EntityPM.ManifestNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "RequestDate", {
        //public get RequestDate() { return this.EntityPM.RequestDate; }
        get: function () { return this.EntityPM != null ? this.EntityPM.RequestDate : null; },
        set: function (value) { this.EntityPM.RequestDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "RequestNumber", {
        //public get RequestNumber() { return this.EntityPM.RequestNumber; }
        get: function () { return this.EntityPM != null ? this.EntityPM.RequestNumber : null; },
        set: function (value) { this.EntityPM.RequestNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "RequestReason", {
        //public get RequestReason() { return this.EntityPM.RequestReason; }
        get: function () { return this.EntityPM != null ? this.EntityPM.RequestReason : null; },
        set: function (value) { this.EntityPM.RequestReason = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "RequestReasonName", {
        //public get RequestReasonName() { return this.EntityPM.RequestReasonName; }
        get: function () { return this.EntityPM != null ? this.EntityPM.RequestReasonName : null; },
        set: function (value) { this.EntityPM.RequestReasonName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ResponseStatusCode", {
        //public get ResponseStatusCode() { return this.EntityPM.ResponseStatusCode; }
        get: function () { return this.EntityPM != null ? this.EntityPM.ResponseStatusCode : null; },
        set: function (value) { this.EntityPM.ResponseStatusCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ResponseStatusName", {
        //public get ResponseStatusName() { return this.EntityPM.ResponseStatusName; }
        get: function () { return this.EntityPM != null ? this.EntityPM.ResponseStatusName : null; },
        set: function (value) { this.EntityPM.ResponseStatusName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "SecondCargoID", {
        //public get SecondCargoID() { return this.EntityPM.SecondCargoID; }
        get: function () { return this.EntityPM != null ? this.EntityPM.SecondCargoID : null; },
        set: function (value) { this.EntityPM.SecondCargoID = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "ThirdCargoID", {
        //public get ThirdCargoID() { return this.EntityPM.ThirdCargoID; }
        get: function () { return this.EntityPM != null ? this.EntityPM.ThirdCargoID : null; },
        set: function (value) { this.EntityPM.ThirdCargoID = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoSplitGeneralTabComponent.prototype, "RequestRemarks", {
        get: function () { return this.EntityPM != null ? this.EntityPM.RequestRemarks : null; },
        set: function (value) { this.EntityPM.RequestRemarks = value; },
        enumerable: true,
        configurable: true
    });
    //get ImporterCode() { return this.SelectedTab != null ? this.SelectedTab.EntityPM.ImporterCode : null; }
    //set ImporterCode(value: string) {
    //    this.SelectedTab.EntityPM.ImporterCode = value;
    //this.UIProperties.SetRequired("ImporterCode", "Customs.Client", !AppTool.IsNullOrEmpty(value));
    //}
    //#region Send + Delete
    CargoSplitGeneralTabComponent.prototype.SendButtonClicked = function () {
        var _this = this;
        //this.SaveEntityChanges(null);
        //if (this.ValidationErrorsList.length == 0) return;
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs
        // validate DeclarationCargoSplit
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.DeclarationCargoSplit", errors);
        if (errors.length == 0) {
            errors = this.SendChecks();
        }
        if (errors.length == 0) {
            //           this.declarationCargoSplitPMService.update(this.EntityPM).subscribe(response => {
            //this.entityPMService.update(this.ObjectTableName, this.EntityPM).subscribe(response => {
            this.CurrentSession.StartBusyIndicator("");
            this.OnMassageDisplayMethod();
            var LoggingObjectTableId = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0].Id;
            this.requestParams = new CargoSplitRequestParams_1.CargoSplitRequestParams();
            this.requestParams.LoggingEnabled = true;
            this.requestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.requestParams.AppicationId = this.EntityPM.Id;
            this.requestParams.Tenant = this.EntityPM.Tenant;
            this.requestParams.RequestName = "Send Cargo Split Request";
            this.requestParams.ResponseName = "Send Cargo Split Response";
            this.requestParams.LoggingEntityId = this.EntityPM.Id;
            this.requestParams.RequestVIA = this.RequestVIA;
            this.requestParams.DeclarationCargoSplit = this.EntityPM.Id;
            LoggingObjectTableId = LoggingObjectTableId;
            CustomMessageProgressComponent_1.CustomMessageProgressComponent
                .ShowProgressBar(this.requestParams.PBId, "שליחת בקשה לפיצול מטען", false)
                .then(function (res) {
                _this.responseData = res;
                _this.OnMassageDisplayMethod();
            }).catch(function (err) {
                _this.ValidationErrorsList = [];
                _this.ValidationErrorsList.push(err);
            });
            this.declarationMessagesService.PostSendCargoSplit(this.requestParams)
                .subscribe(function (response) {
                if (response) {
                    if (!response.HasError) {
                        if (response.Result.Succeeded) {
                            _this.declarationCargoSplitPMService.get(_this.EntityPM.Id).subscribe(function (response) {
                                if (response) {
                                    if (!response.HasError) {
                                        _this.EntityPM = response.Result;
                                        _this.BuildTabs();
                                    }
                                }
                            });
                        }
                    }
                }
            });
            //           });
        }
        else {
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        }
    };
    CargoSplitGeneralTabComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.requestParams == null) {
            this.requestParams = new CargoSplitRequestParams_1.CargoSplitRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        }
    };
    CargoSplitGeneralTabComponent.prototype.OnSendCompleted = function () {
        if (this.IsDelete) {
            this.ApplyDeleteDeclarationCargoSplit();
        }
    };
    CargoSplitGeneralTabComponent.prototype.ApplyDeleteDeclarationCargoSplit = function () {
        this.IsDelete = false;
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
    };
    CargoSplitGeneralTabComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        var responseStatusCode = this.ResponseStatusCode;
        //this.ResponseStatusCode = null;
        //this.SaveEntityChanges(customSendOptionsArgs);
        //if (this.ValidationErrorsList.length != 0) {
        //   this.ResponseStatusCode = responseStatusCode;
        //}
        //else {
        //this.SendButtonClicked();
        this.SaveEntityChanges(customSendOptionsArgs);
        //}
    };
    CargoSplitGeneralTabComponent.prototype.OkButtonClicked = function () {
        //this.CurrentSession.CloseCurrentWindowEmit("Ok");
        this.SaveEntityChanges(null);
        return;
        //if (!this.IsDisplayOnly) {
        //    if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //        this.declarationCargoSplitPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
        //            var res = response.Result;
        //            if (response.HasError) {
        //                this.ValidationErrorsList = [];
        //                this.ValidationErrorsList = response.ErrorsArray;
        //            } else {
        //                this.CurrentSession.CloseCurrentWindow();
        //            }
        //        });
        //        this.RefreshEntity();
        //    }
        //    else {
        //        this.declarationCargoSplitPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
        //            var res = response.Result;
        //            if (response.HasError) {
        //                this.ValidationErrorsList = [];
        //                this.ValidationErrorsList = response.ErrorsArray;
        //            } else {
        //                this.CurrentSession.CloseCurrentWindow();
        //            }
        //        });
        //    }
        //}
    };
    CargoSplitGeneralTabComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    CargoSplitGeneralTabComponent.prototype.RefreshEntity = function () {
        var _this = this;
        //if ()this.EntityPM
        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        this.declarationCargoSplitController.CheckRequestsInProgress(this.EntityPM.DeclarationId).subscribe(function (response) {
            if (response.Result.IsDisplayOnly) {
                _this.SendButtonEnabled = false;
            }
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM) && this.EntityPM.DecCargoSplitCargoIdentifiers != null && this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
            this.CargoIdentifiersList.InsertCollection(this.EntityPM.DecCargoSplitCargoIdentifiers);
        }
    };
    CargoSplitGeneralTabComponent.prototype.InitCargoIdentifiers = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM) && this.EntityPM.DecCargoSplitCargoIdentifiers != null && this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
            this.CargoIdentifiersList.InsertCollection(this.EntityPM.DecCargoSplitCargoIdentifiers);
        }
    };
    CargoSplitGeneralTabComponent.prototype.SaveEntityChanges = function (customSendOptionsArgs) {
        var _this = this;
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var errors = [];
        this.FillValidationErrorList.emit(errors);
        this.ValidationErrorsList = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.DeclarationCargoSplit", errors);
        if (this.EntityPM.DecCargoSplitCons == null || this.EntityPM.DecCargoSplitCons.length < 1) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("חובה להזין נתונים לפחות ליבואן אחד"));
        }
        else {
            this.Tabs.forEach(function (consignment) {
                Validator_1.Validator.TryValidateObject(consignment.EntityPM, "Customs.DecCargoSplitCon", errors);
                errors.forEach(function (item, index) {
                    if (item.includes("DeclarationCargoSplitId"))
                        errors.splice(index, 1);
                });
                if (consignment.EntityPM.DecCargoSplitConsItems != null && consignment.EntityPM.DecCargoSplitConsItems.length > 0) {
                    consignment.EntityPM.DecCargoSplitConsItems.forEach(function (item) {
                        Validator_1.Validator.TryValidateObject(item, "Customs.DecCargoSplitConsItem", errors);
                        errors.forEach(function (item, index) {
                            if (item.includes("DeclarationCargoSplitId"))
                                errors.splice(index, 1);
                        });
                    });
                }
            });
            //this.EntityPM.DecCargoSplitCons.forEach((consignment) => {
            //    Validator.TryValidateObject(consignment, "Customs.DecCargoSplitCon", errors);
            //    if (consignment.DecCargoSplitConsItems != null && consignment.DecCargoSplitConsItems.length > 0) {
            //        consignment.DecCargoSplitConsItems.forEach((item) => {
            //            Validator.TryValidateObject(item, "Customs.DecCargoSplitConsItem", errors);
            //        });
            //    }
            //});
        }
        errors.forEach(function (item, index) {
            if (item.includes("DeclarationCargoSplitId"))
                errors.splice(index, 1);
        });
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        }
        else {
            if (this.ScreenChecks().length != 0) {
                this.ValidationErrorsList = this.ScreenChecks();
                this.FillValidationErrorList.emit(errors);
            }
        }
        //this.CancelButtonClicked();
        //return;
        /*
        if (this.EntityPM.DecCargoSplitCons == null || this.EntityPM.DecCargoSplitCons.length < 1) {
            this.ValidationErrorsList.push("חובה להזין נתונים לפחות ליבואן אחד");
        }
        for (let consignment of this.EntityPM.DecCargoSplitCons) {
            if (AppTool.IsNullOrEmpty(consignment.ImporterCode)) {
                this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ImporterCode")));
                break;
            }
            else {

                if (consignment.ProcedureCurrentCode == null) {
                    this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ProcedureCurrentCode")));
                    break;
                }
            }
        }
        if (this.ScreenChecks().length != 0) {
            this.ValidationErrorsList = this.ScreenChecks();
        }
        */
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            //this.declarationCargoSplitPMService.update(this.EntityPM).then((res: any) => {
            //    res.subscribe((myResponse: ServiceResponse) => {
            this.declarationCargoSplitPMService.update(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                        var myErrors = [];
                        myErrors.push("this.EntityPM.Id is null");
                        _this.ValidationErrorsList = myErrors;
                    }
                    //else if (this.ScreenChecks().length != 0){
                    //    this.ValidationErrorsList = this.ScreenChecks();
                    //}
                    else {
                        if (customSendOptionsArgs == null) {
                            _this.CancelButtonClicked();
                        }
                        else {
                            /*
                            CustomMessageProgressComponent
                                .ShowProgressBar("",
                                " ", true)
                                .then((res) => {
                                    console.log(res);
                                    //this.CancelButtonClicked();
                                }
                                ).catch((err) => {
                                    this.ValidationErrorsList.push(err);
                                    this.CancelButtonClicked();
                                });

                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
                            */
                            _this.SendButtonClicked();
                        }
                    }
                }
                /*
            }, error => {
                this.CurrentSession.StopBusyIndicator();
                var myErrors: string[] = [];
                myErrors.push(error.message);
                this.ValidationErrorsList = myErrors;
                //this.SaveCompleted.emit(false);
            
            });
               */
            });
            //this.RefreshEntity();
        }
        else {
            //this.declarationCargoSplitPMService.insert(this.EntityPM).then((res: any) => {
            //res.subscribe((myResponse: ServiceResponse) => {
            this.declarationCargoSplitPMService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                        var myErrors = [];
                        myErrors.push("this.EntityPM.Id is null");
                        _this.ValidationErrorsList = myErrors;
                    }
                    //else if (this.ScreenChecks().length != 0) {
                    //    this.ValidationErrorsList = this.ScreenChecks();
                    //}
                    else {
                        if (customSendOptionsArgs == null) {
                            _this.CancelButtonClicked();
                        }
                        else {
                            /*
                            CustomMessageProgressComponent
                                .ShowProgressBar("",
                                " ", true)
                                .then((res) => {
                                    console.log(res);
                                    this.CancelButtonClicked();
                                }
                                ).catch((err) => {
                                    this.ValidationErrorsList.push(err);
                                    this.CancelButtonClicked();
                                });

                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
                            */
                            //myIIGGeneralMessagesService.PostDeclarationCargoSplitRequest("")
                            //                                        .subscribe((myServiceResponse: ServiceResponse) => {
                            //                                      });
                            _this.SendButtonClicked();
                        }
                    }
                }
                /*
            }, error => {
                    this.CurrentSession.StopBusyIndicator();
                    var myErrors: string[] = [];
                    myErrors.push(error.message);
                    this.ValidationErrorsList = myErrors;
                    //this.SaveCompleted.emit(false);
                });
                */
            });
            //}
            //this.RefreshEntity();
        }
    };
    CargoSplitGeneralTabComponent.prototype.ScreenChecks = function () {
        var errors = [];
        if ((this.ActionTypeCode == "3" || this.ActionTypeCode == "4") && Tools_1.AppTool.IsNullOrEmpty(this.RequestNumber)) {
            errors.push("מס' בקשת פיצול חסר");
        }
        if (this.Tabs == null || this.Tabs.length < 1) {
            errors.push("חובה להזין נתונים לפחות ליבואן אחד");
        }
        for (var _i = 0, _a = this.Tabs; _i < _a.length; _i++) {
            var tab = _a[_i];
            if (Tools_1.AppTool.IsNullOrEmpty(tab.EntityPM.ImporterCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ImporterCode")));
                break;
            }
            else {
                if (tab.EntityPM.ProcedureCurrentCode == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ProcedureCurrentCode")));
                    break;
                }
            }
            for (var _b = 0, _c = tab.EntityPM.DecCargoSplitConsItems; _b < _c.length; _b++) {
                var item = _c[_b];
                if (Tools_1.AppTool.IsNullOrEmpty(item.ParentCargoConsinmentItem)) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.ParentCargoConsinmentItem")));
                    break;
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CargoDescription)) {
                        errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.CargoDescription")));
                        break;
                    }
                    else {
                        if (Tools_1.AppTool.IsNullOrEmpty(item.RequestReasonCode)) {
                            errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.RequestReasonCode")));
                            break;
                        }
                    }
                }
                for (var _d = 0, _e = item.DecCargoSplitConsPackDets; _d < _e.length; _d++) {
                    var pack = _e[_d];
                }
            }
        }
        /*
        if (this.EntityPM.DecCargoSplitCons == null || this.EntityPM.DecCargoSplitCons.length < 1) {
            errors.push("חובה להזין נתונים לפחות ליבואן אחד");
        }
        for (let consignment of this.EntityPM.DecCargoSplitCons) {
            if (AppTool.IsNullOrEmpty(consignment.ImporterCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ImporterCode")));
                break;
            }
            else {

                if (consignment.ProcedureCurrentCode == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ProcedureCurrentCode")));
                    break;
                }
            }
            for (let item of consignment.DecCargoSplitConsItems) {
                if (AppTool.IsNullOrEmpty(item.ParentCargoConsinmentItem)) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.ParentCargoConsinmentItem")));
                    break;
                }
                else {

                    if (AppTool.IsNullOrEmpty(item.CargoDescription)) {
                        errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.CargoDescription")));
                        break;
                    }
                    else {

                        if (AppTool.IsNullOrEmpty(item.RequestReasonCode)) {
                            errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.RequestReasonCode")));
                            break;
                        }
                    }
                }
                if (item.DecCargoSplitConsPackDets == null || item.DecCargoSplitConsPackDets.length < 1) {
                    errors.push("קיימות אריזות ללא פירוט");
                    break;
                }
                for (let pack of item.DecCargoSplitConsPackDets) {

                }
            }
        }
        */
        return errors;
    };
    CargoSplitGeneralTabComponent.prototype.SendChecks = function () {
        var errors = [];
        for (var _i = 0, _a = this.Tabs; _i < _a.length; _i++) {
            var tab = _a[_i];
            if (tab.EntityPM.DecCargoSplitConsItems == null || tab.EntityPM.DecCargoSplitConsItems.length < 1) {
                errors.push("חובה להזין נתוני אריזות");
            }
            for (var _b = 0, _c = tab.EntityPM.DecCargoSplitConsItems; _b < _c.length; _b++) {
                var item = _c[_b];
                if (item.DecCargoSplitConsPackDets == null || item.DecCargoSplitConsPackDets.length < 1) {
                    errors.push("קיימות אריזות ללא פירוט");
                    break;
                }
                for (var _d = 0, _e = item.DecCargoSplitConsPackDets; _d < _e.length; _d++) {
                    var pack = _e[_d];
                }
            }
        }
        //for (let consignment of this.EntityPM.DecCargoSplitCons) {
        //    if (consignment.DecCargoSplitConsItems == null || consignment.DecCargoSplitConsItems.length < 1) {
        //        errors.push("חובה להזין נתוני אריזות");
        //    }
        //    for (let item of consignment.DecCargoSplitConsItems) {
        //        if (item.DecCargoSplitConsPackDets == null || item.DecCargoSplitConsPackDets.length < 1) {
        //            errors.push("קיימות אריזות ללא פירוט");
        //            break;
        //        }
        //        for (let pack of item.DecCargoSplitConsPackDets) {
        //        }
        //    }
        //}
        return errors;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CargoSplitGeneralTabComponent.prototype, "FillValidationErrorList", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CargoSplitGeneralTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    CargoSplitGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CargoSplitGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CargoSplitGeneralTabComponent);
    return CargoSplitGeneralTabComponent;
}(BaseComponent_1.BaseComponent
//implements AfterViewInit, AfterContentInit{
));
exports.CargoSplitGeneralTabComponent = CargoSplitGeneralTabComponent;
var XRayAvailableItem = /** @class */ (function () {
    function XRayAvailableItem() {
    }
    return XRayAvailableItem;
}());
exports.XRayAvailableItem = XRayAvailableItem;
var DecCargoSplitCargoIdentifierModel = /** @class */ (function (_super) {
    __extends(DecCargoSplitCargoIdentifierModel, _super);
    function DecCargoSplitCargoIdentifierModel(line) {
        var _this = _super.call(this) || this;
        _this.EntityPM = line;
        return _this;
    }
    Object.defineProperty(DecCargoSplitCargoIdentifierModel.prototype, "CargoIdentifierKey1", {
        //#region Properties
        get: function () { return this.EntityPM.CargoIdentifierKey1; },
        set: function (newValue) { this.EntityPM.CargoIdentifierKey1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitCargoIdentifierModel.prototype, "CargoIdentifierKey2", {
        get: function () { return this.EntityPM.CargoIdentifierKey2; },
        set: function (newValue) { this.EntityPM.CargoIdentifierKey2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitCargoIdentifierModel.prototype, "CargoIdentifierKey3", {
        get: function () { return this.EntityPM.CargoIdentifierKey3; },
        set: function (newValue) { this.EntityPM.CargoIdentifierKey3 = newValue; },
        enumerable: true,
        configurable: true
    });
    return DecCargoSplitCargoIdentifierModel;
}(BaseComponent_1.BaseComponent));
exports.DecCargoSplitCargoIdentifierModel = DecCargoSplitCargoIdentifierModel;
var TabItem = /** @class */ (function () {
    function TabItem(code, textCode) {
        this.code = code;
        this.textCode = textCode;
    }
    return TabItem;
}());
//# sourceMappingURL=CargoSplitGeneralTabComponent.js.map