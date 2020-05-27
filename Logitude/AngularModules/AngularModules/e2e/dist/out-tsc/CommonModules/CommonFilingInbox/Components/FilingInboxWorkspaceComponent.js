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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var FilingInboxPMService_1 = require("../../../Common/Services/StandardPMs/FilingInboxPMService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ShipmentPMService_1 = require("../../../Shipment/Services/StandardPMs/ShipmentPMService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var UserPMService_1 = require("../../../Common/Services/StandardPMs/UserPMService");
var DocumentTypeListExtendedService_1 = require("../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var FilingInboxWorkspaceComponent = /** @class */ (function (_super) {
    __extends(FilingInboxWorkspaceComponent, _super);
    function FilingInboxWorkspaceComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ObjectTableName = "FilingInbox";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.FilingInboxAttachments = [];
        _this.LoggedUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.SettingsDomain = "domain.com";
        _this.IFrameURI = "";
        _this.QuickSearchItems = [];
        _this.Filters = null;
        _this.ObjectTableId = null;
        _this.ShipmentObjectTableId = null;
        _this.QuoteObjectTableId = null;
        _this.IsVisible = false;
        _this.IsHouseDisabled = false;
        _this.IsNewShipmentVisible = false;
        _this.IsHouseVisible = false;
        _this.IsConnectToFilterVisible = false;
        _this.IsDescriptionVisible = false;
        _this.IsShareAgentVisible = false;
        _this.IsDigitallySignVisible = false;
        _this.DontShowInboxToolTip = false;
        _this.IsLogBox = false;
        _this.IconBackground = "./Images/single-tick.png";
        _this.ShareAsDefault = false;
        _this.IsDSVConnectVisible = false;
        _this.IsDSVConnectEnable = false;
        _this.IsHebrewSettings = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsDigitallySignDisabled = false;
        //Search 
        _this.SearchFilter = "";
        //Tip 
        _this.DontShowagainText = "Don't Show again";
        _this.dontShowAgain = false;
        _this.MenuEvent = null;
        _this.DocumentTypeList = [];
        _this.count = 0;
        _this.SearchedList = [];
        /*Pager & Provider*/
        _this.queryPageIndex = 0;
        _this.pageIndex = 1;
        _this.totalPagesCount = 1;
        _this.pageSize = 100;
        /* Pager Buttons States */
        _this.isHitStateFirstButton = false;
        _this.opacityFirstButton = 0.5;
        _this.isHitStatePrevButton = false;
        _this.opacityPrevButton = 0.5;
        _this.isHitStateNextButton = false;
        _this.opacityNextButton = 0.5;
        _this.isHitStateLastButton = false;
        _this.opacityLastButton = 0.5;
        _this.description = null;
        // Filters 
        _this.myUserId = _this.LoggedUserId;
        _this.mySelectedUserFilter = "M";
        _this.ConnectToFilterLabel = "Master";
        _this.isShowDeletedEnabled = false;
        _this.IsItemSelected = false;
        _this.IsPDF = false;
        _this.selectedAttachment = new FilingInboxAttachment(null, _this);
        _this.isMailBody = false;
        _this.IsFilingButtonEnabled = true;
        _this.selectedFilingInbox = null;
        _this.IsRefreshClicked = false;
        _this.IsRefreshEnabled = true;
        _this.IsStopPreviewHtml = false;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.Listen();
        _this.SetDefaultValues();
        _this.Initialize();
        _this.InitializeFilters();
        //this.LoadAllData();
        _this.FillDocumentFiling();
        _this.SetUIPropertires();
        return _this;
    }
    FilingInboxWorkspaceComponent.prototype.ngOnInit = function () {
        if (this.IsLogBox || SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.IsHebrewSettings = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "DigitallySign")) {
            this.CheckDigitalSign();
        }
        this.Id = Guid_1.Guid.newGuid();
        var divId = this.CurrentSession.GetNewId("PreviewDiv");
        this.PreviewDivId = "PreviewDiv_" + divId;
        this.DontShowInboxToolTip = SessionLocator_1.SessionLocator.LoggedUserPM.ShowInboxToolTip;
        this.DontShowAgain = SessionLocator_1.SessionLocator.LoggedUserPM.ShowInboxToolTip;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "NewShipment")) {
            this.IsNewShipmentVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter")) {
            this.IsHouseVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "FilingInboxDescription")) {
            this.IsDescriptionVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "ShareWithAgent")) {
            this.IsShareAgentVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "DigitallySign")) {
            this.IsDigitallySignVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "ShipmentsFilter") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "QuotesFilter")) {
            this.IsConnectToFilterVisible = true;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter")) {
                this.SelectedConnectToFilter = "M";
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "ShipmentsFilter")) {
                this.SelectedConnectToFilter = "S";
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "QuotesFilter")) {
                this.SelectedConnectToFilter = "Q";
            }
            else {
                this.SelectedConnectToFilter = "";
            }
        }
        this.GetUser();
    };
    FilingInboxWorkspaceComponent.prototype.ngAfterViewInit = function () {
        this.SetHtml();
    };
    FilingInboxWorkspaceComponent.prototype.SetUIPropertires = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityId)) {
            this.IsHouseDisabled = true;
        }
        else {
            this.IsHouseDisabled = false;
        }
    };
    FilingInboxWorkspaceComponent.prototype.SetDefaultValues = function () {
        this.ShareAsDefault = SessionLocator_1.SessionLocator.TenantPM.DocumentShareAsDefault;
        this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        this.MyInActiveFilter = new ApiQueryFilters_1.ApiQueryFilters();
        this.MyInActiveFilter.Filter1Name = "InActive";
        this.MyInActiveFilter.Filter1Operator = "Equals";
        this.MyInActiveFilter.Filter1Value = false;
        var objecttabel = window.ObjectTables.filter(function (x) { return x.Name === "Shipment"; })[0];
        this.ShipmentObjectTableId = objecttabel.Id;
        this.ObjectTableId = this.ShipmentObjectTableId;
        this.EntityObjectTableName = "Master";
        objecttabel = window.ObjectTables.filter(function (x) { return x.Name === "Quote"; })[0];
        this.QuoteObjectTableId = objecttabel.Id;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
            this.IsLogBox = true;
        }
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.IsDSVConnectVisible = true;
        }
        else {
            this.IsDSVConnectVisible = false;
        }
        this.PageIndex = 1;
        this.QueryPageIndex = 0;
    };
    FilingInboxWorkspaceComponent.prototype.CheckDigitalSign = function () {
        var _this = this;
        if (this.myCommonDomainService == null) {
            this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        }
        this.myCommonDomainService.GetSignRequestReceived().subscribe(function (response) {
            if (response != null && !response.HasError) {
                var result = response.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    _this.IsDigitallySignDisabled = true;
                }
            }
        });
    };
    FilingInboxWorkspaceComponent.prototype.onSearchTextChangeEvent = function (event) {
        var temp = null;
        if (event) {
            temp = event.replace(/\s+$/, '');
        }
        this.searchFields = temp;
        this.SearchFilter = temp;
        this.IsVisible = false;
        this.LoadAllData();
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "DontShowAgain", {
        get: function () {
            return this.dontShowAgain;
        },
        set: function (value) {
            if (this.dontShowAgain != value) {
                this.dontShowAgain = value;
                SessionLocator_1.SessionLocator.LoggedUserPM.ShowInboxToolTip = value;
                var myPM = SessionLocator_1.SessionLocator.LoggedUserPM;
                this.UserPMService.update(myPM).subscribe(function (myResult) {
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxWorkspaceComponent.prototype.CloseToolTipArea = function (arg) {
        this.DontShowInboxToolTip = true;
    };
    FilingInboxWorkspaceComponent.prototype.OpenToolTipArea = function () {
        this.DontShowInboxToolTip = false;
    };
    FilingInboxWorkspaceComponent.prototype.Listen = function () {
        var _this = this;
        this.MenuEvent = this.CurrentSession.MainMenuComponent.SelectionChanging.subscribe(function (isSelectionChanging) {
            if (isSelectionChanging) {
                _this.ShowUnsaveChanges();
            }
        });
    };
    FilingInboxWorkspaceComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.MenuEvent);
    };
    FilingInboxWorkspaceComponent.prototype.ShowUnsaveChanges = function () {
        var _this = this;
        var hasChanges = false;
        if (this.SelectedFilingInbox != null) {
            this.SelectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        // save filing 
                        _this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        _this.ChangeMenu();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.ChangeMenu();
            }
        }
        else {
            this.ChangeMenu();
        }
    };
    FilingInboxWorkspaceComponent.prototype.ChangeMenu = function () {
        this.CurrentSession.MainMenuComponent.ChangeMenu();
    };
    FilingInboxWorkspaceComponent.prototype.SetHtml = function () {
        var element = document.getElementById(this.PreviewDivId);
        if (element) {
            element.innerHTML = this.EmailBody;
        }
    };
    FilingInboxWorkspaceComponent.prototype.GetUser = function () {
        var _this = this;
        this.UserPMService.get(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (myResult) {
            if (myResult != null && !myResult.HasError) {
                var domain = ObjectsLocator_1.ObjectsLocator.GlobalSetting.DocumentFilingEmailDomain;
                if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                    domain = "inbox.dsv.co.il";
                }
                _this.SettingsDomain = myResult.Result != null ? myResult.Result.DocumentFilingInbox + "@" + domain : "";
            }
        });
    };
    FilingInboxWorkspaceComponent.prototype.FillDocumentFiling = function () {
        var _this = this;
        this._DocumentTypeListService.getTop5DocumentTypesPMsByObjectTableAndTenant(SessionLocator_1.SessionLocator.Tenant, this.ObjectTableId).subscribe(function (res) {
            var MyType = "";
            res.Result.forEach(function (item) {
                MyType = item.Name.trim();
                var tempList = MyType.split(' ');
                var tempName = "";
                if (tempList.length == 1) {
                    tempName = tempList[0].substring(0, 3).toUpperCase();
                }
                else if (tempList.length == 2) {
                    tempName = (tempList[0].substring(0, 1) + tempList[1].substring(0, 2)).toUpperCase();
                }
                else {
                    tempName = (tempList[0].substring(0, 1) + tempList[1].substring(0, 1) + tempList[2].substring(0, 1)).toUpperCase();
                }
                item.OrderedDisplayName = tempName;
            });
            _this.DocumentTypeList = res.Result;
            _this.TopTypes = res.Result;
        });
    };
    FilingInboxWorkspaceComponent.prototype.InitializeFilters = function () {
        this.mySelectedUserFilter = "M";
        this.UIProperties.SetEnabled("UserId", "UserFilter", false);
        this.myUserId = this.LoggedUserId;
    };
    FilingInboxWorkspaceComponent.prototype.LoadAllData = function () {
        var _this = this;
        this.filters = new ApiQueryFilters_1.ApiQueryFilters();
        this.filters.PageIndex = this.QueryPageIndex;
        this.filters.PageSize = this.PageSize;
        if (this.IsRefreshClicked) {
            this.IsRefreshEnabled = false;
        }
        this.ItemsSource = [];
        this.myCommonDomainService.GetFilingInboxes(this.filters, this.myUserId, this.IsShowDeletedEnabled).subscribe(function (response) {
            if (!response.HasError) {
                var result = response.Result;
                if (result != null && !Tools_1.AppTool.IsNullOrEmpty(_this.searchFields)) {
                    result = result.filter(function (d) { return d.SearchFields != null && d.SearchFields && d.SearchFields.toUpperCase().indexOf(_this.searchFields.toUpperCase()) > -1; });
                }
                result.forEach(function (item) {
                    _this.ItemsSource.push(new FilingInboxData(item, _this));
                });
                if (_this.ItemsSource.length > 0) {
                    _this.SelectedFilingInbox = _this.ItemsSource[0];
                }
                else {
                    _this.SelectedFilingInbox = null;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.searchFields)) {
                    _this.count = result.length;
                }
                else {
                    _this.count = response.Count;
                }
                var size = _this.pageSize;
                _this.TotalPagesCount = Math.ceil(_this.count / size);
                if (_this.TotalPagesCount == 0) {
                    _this.TotalPagesCount = 1;
                }
                _this.SetPagerButtonsStates();
                _this.IsVisible = true;
                _this.IsRefreshEnabled = true;
            }
            else {
                if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                    _this.ShowMessage(response.ErrorsArray[0]);
                }
            }
        });
    };
    FilingInboxWorkspaceComponent.prototype.LoadDateCount = function () {
        var size = this.pageSize;
        this.TotalPagesCount = Math.ceil(this.count / size);
        if (this.TotalPagesCount == 0) {
            this.TotalPagesCount = 1;
        }
        this.SetPagerButtonsStates();
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "QueryPageIndex", {
        get: function () {
            return this.queryPageIndex;
        },
        set: function (value) {
            this.queryPageIndex = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "PageIndex", {
        get: function () {
            return this.pageIndex;
        },
        set: function (value) {
            this.pageIndex = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "TotalPagesCount", {
        get: function () {
            return this.totalPagesCount;
        },
        set: function (value) {
            this.totalPagesCount = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "PageSize", {
        get: function () {
            return this.pageSize;
        },
        set: function (value) {
            this.pageSize = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsHitState_FirstButton", {
        get: function () {
            return this.isHitStateFirstButton;
        },
        set: function (value) {
            this.isHitStateFirstButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "Opacity_FirstButton", {
        get: function () {
            return this.opacityFirstButton;
        },
        set: function (value) {
            this.opacityFirstButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsHitState_PrevButton", {
        get: function () {
            return this.isHitStatePrevButton;
        },
        set: function (value) {
            this.isHitStatePrevButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "Opacity_PrevButton", {
        get: function () {
            return this.opacityPrevButton;
        },
        set: function (value) {
            this.opacityPrevButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsHitState_NextButton", {
        get: function () {
            return this.isHitStateNextButton;
        },
        set: function (value) {
            this.isHitStateNextButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "Opacity_NextButton", {
        get: function () {
            return this.opacityNextButton;
        },
        set: function (value) {
            this.opacityNextButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsHitState_LastButton", {
        get: function () {
            return this.isHitStateLastButton;
        },
        set: function (value) {
            this.isHitStateLastButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "Opacity_LastButton", {
        get: function () {
            return this.opacityLastButton;
        },
        set: function (value) {
            this.opacityLastButton = value;
        },
        enumerable: true,
        configurable: true
    });
    /* First Page */
    FilingInboxWorkspaceComponent.prototype.SetPagerButtonsStates = function () {
        if (this.PageIndex == 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;
            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }
        else if (this.PageIndex == 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;
            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }
        else if (this.PageIndex > 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;
            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }
        else if (this.PageIndex > 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;
            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }
    };
    FilingInboxWorkspaceComponent.prototype.FirstPageClick = function () {
        var _this = this;
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        // save filing 
                        _this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        _this.FirstPageWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.FirstPageWork();
            }
        }
        else {
            this.FirstPageWork();
        }
    };
    FilingInboxWorkspaceComponent.prototype.FirstPageWork = function () {
        this.ClearConnectToFilter();
        this.PageIndex = 1;
        this.QueryPageIndex = 0;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    };
    FilingInboxWorkspaceComponent.prototype.PreviousPageClick = function () {
        var _this = this;
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        // save filing 
                        _this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        _this.PreviosButtonWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.PreviosButtonWork();
            }
        }
        else {
            this.PreviosButtonWork();
        }
    };
    FilingInboxWorkspaceComponent.prototype.PreviosButtonWork = function () {
        this.ClearConnectToFilter();
        this.PageIndex = this.PageIndex - 1;
        this.QueryPageIndex = this.QueryPageIndex - 100;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    };
    FilingInboxWorkspaceComponent.prototype.NextPageClick = function () {
        var _this = this;
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        // save filing 
                        _this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        _this.NextPageWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.NextPageWork();
            }
        }
        else {
            this.NextPageWork();
        }
    };
    FilingInboxWorkspaceComponent.prototype.NextPageWork = function () {
        this.ClearConnectToFilter();
        this.PageIndex = this.PageIndex + 1;
        this.QueryPageIndex = this.QueryPageIndex + 100;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    };
    FilingInboxWorkspaceComponent.prototype.LastPageClick = function () {
        var _this = this;
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        // save filing 
                        _this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        _this.LastPageWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.LastPageWork();
            }
        }
        else {
            this.LastPageWork();
        }
    };
    FilingInboxWorkspaceComponent.prototype.LastPageWork = function () {
        this.ClearConnectToFilter();
        this.PageIndex = this.TotalPagesCount;
        this.QueryPageIndex = (this.TotalPagesCount - 1) * this.pageSize;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    };
    FilingInboxWorkspaceComponent.prototype.Initialize = function () {
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myFilingInboxPMService = new FilingInboxPMService_1.FilingInboxPMService();
        this.UserPMService = new UserPMService_1.UserPMService();
        this._DocumentTypeListService = new DocumentTypeListExtendedService_1.DocumentTypeListExtendedService();
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "Description", {
        get: function () { return this.description; },
        set: function (value) {
            if (this.description != value) {
                this.description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "UserId", {
        get: function () { return this.myUserId; },
        set: function (value) {
            if (this.myUserId != value) {
                this.myUserId = value;
                this.IsVisible = false;
                this.LoadAllData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "SelectedUserFilter", {
        get: function () { return this.mySelectedUserFilter; },
        set: function (value) {
            if (this.mySelectedUserFilter != value) {
                this.mySelectedUserFilter = value;
                if (value == "M") {
                    this.myUserId = this.LoggedUserId;
                    this.UIProperties.SetEnabled("UserId", "UserFilter", false);
                }
                else {
                    this.myUserId = null;
                    this.UIProperties.SetEnabled("UserId", "UserFilter", true);
                    this.IsStopPreviewHtml = false;
                }
                this.IsVisible = false;
                this.LoadAllData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "SelectedConnectToFilter", {
        get: function () { return this.mySelectedConnectToFilter; },
        set: function (value) {
            if (this.mySelectedConnectToFilter != value) {
                this.ClearConnectToFilter();
                this.mySelectedConnectToFilter = value;
                this.GetConnectToFilterLabel();
                if (this.SelectedAttachment != null) {
                    this.SelectedAttachment.IsSharedWithAgent = this.ShareAsDefault;
                    this.SelectedAttachment.FillDocumentFiling();
                    if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedAttachment.Description)) {
                        this.SelectedAttachment.Description = this.SelectedAttachment.FileName != null ? this.SelectedAttachment.FileName.split('.')[0] : this.SelectedAttachment.FileName;
                    }
                }
                if (value == "M") {
                    if (this.SelectedAttachment != null) {
                        this.SelectedAttachment.SetHousesFilters();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxWorkspaceComponent.prototype.GetConnectToFilterLabel = function () {
        switch (this.SelectedConnectToFilter) {
            case "M": {
                this.ConnectToFilterLabel = "Master";
                this.EntityObjectTableName = "Master";
                this.IsLogBox = false;
                this.ObjectTableId = this.ShipmentObjectTableId;
                break;
            }
            case "S": {
                this.ConnectToFilterLabel = "Shipment";
                this.EntityObjectTableName = "Shipment";
                if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") {
                    this.IsLogBox = true;
                }
                this.ObjectTableId = this.ShipmentObjectTableId;
                break;
            }
            case "Q": {
                this.ConnectToFilterLabel = "Quote";
                this.EntityObjectTableName = "Quote";
                this.IsLogBox = false;
                this.ObjectTableId = this.QuoteObjectTableId;
                break;
            }
        }
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsShowDeletedEnabled", {
        get: function () {
            return this.isShowDeletedEnabled;
        },
        set: function (value) {
            if (this.isShowDeletedEnabled != value) {
                this.isShowDeletedEnabled = value;
                this.IsVisible = false;
                this.LoadAllData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "EntityId", {
        get: function () {
            return this.entityId;
        },
        set: function (value) {
            if (this.entityId != value) {
                this.entityId = value;
                if (this.SelectedAttachment != null) {
                    this.SelectedAttachment.SetHousesFilters();
                }
                this.SetUIPropertires();
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxWorkspaceComponent.prototype.QuickSearchTextChanged = function (entity) {
        this.IsItemSelected = true;
        if (this.IsLogBox || SessionLocator_1.SessionLocator.PrivateLableSettings) {
            if (this.EntityObjectTableName == "Master") {
                this.Customer = entity.AgentName;
            }
            else {
                this.Customer = entity.ShipperName;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entity.ForwarderShipmentNumber) && !Tools_1.AppTool.IsNullOrEmpty(entity.StatusName) && entity.StatusName.toLocaleLowerCase() != "in progress") {
                this.IsDSVConnectEnable = true;
            }
            else {
                this.IsDSVConnectEnable = false;
            }
            this.Route = entity.Routing;
            this.EntityNumber = Tools_1.AppTool.IsNullOrEmpty(entity.ForwarderShipmentNumber) ? entity.CustomerReference1 : entity.ForwarderShipmentNumber;
            this.EntityId = entity.Id;
        }
        else {
            if (this.EntityObjectTableName == "Master") {
                this.Customer = entity.AgentName;
            }
            else {
                this.Customer = entity.CustomerName;
            }
            this.Route = entity.Routing;
            if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master") {
                this.EntityNumber = entity.ShipmentNumber;
            }
            else {
                this.EntityNumber = entity.QuoteNumber;
            }
            this.EntityId = entity.Id;
        }
    };
    FilingInboxWorkspaceComponent.prototype.ChooseEntity = function (arg) {
        var _this = this;
        if (this.SelectedFilingInbox != null) {
            if (this.IsLogBox || SessionLocator_1.SessionLocator.PrivateLableSettings) {
                this.ChooseForwarderShipment();
            }
            else {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 800;
                logWindow.Height = 570;
                var args = {};
                if (arg == "house") {
                    logWindow.Title = "Houses Search";
                    args.EntityObjectTableName = "House";
                    args.EntityId = this.EntityId;
                }
                else {
                    if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master") {
                        logWindow.Title = "Shipments Search";
                    }
                    else {
                        logWindow.Title = "Quotes Search";
                    }
                    args.EntityObjectTableName = this.EntityObjectTableName;
                }
                if (arg == "house" && this.IsHouseDisabled) {
                }
                else {
                    logWindow.WindowArgs = args;
                    logWindow.Show('./CommonModules/CommonFilingInbox/Components/ChooseEntityComponent');
                    logWindow.ComponentLoaded.subscribe(function (s) {
                        logWindow.WindowClosed.subscribe(function (d) {
                            var entityList = null;
                            if (s.EntityObjectTableName == "Shipment" || s.EntityObjectTableName == "Master" || s.EntityObjectTableName == "House") {
                                entityList = s.SelectedShipment;
                            }
                            else {
                                entityList = s.SelectedQuote;
                            }
                            if (entityList != null) {
                                if (s.EntityObjectTableName == "Master") {
                                    _this.Customer = entityList.AgentName;
                                }
                                else {
                                    _this.Customer = entityList.CustomerName;
                                }
                                _this.Route = entityList.Routing;
                                if (s.EntityObjectTableName == "House") {
                                    _this.SelectedAttachment.HouseNumber = entityList.ShipmentNumber;
                                }
                                else if (s.EntityObjectTableName == "Shipment" || s.EntityObjectTableName == "Master") {
                                    _this.EntityNumber = entityList.ShipmentNumber;
                                }
                                else {
                                    _this.EntityNumber = entityList.QuoteNumber;
                                }
                                _this.EntityId = entityList.Id;
                            }
                        });
                    });
                }
            }
        }
    };
    FilingInboxWorkspaceComponent.prototype.ChooseForwarderShipment = function () {
        var _this = this;
        var newWindow = new LogitudeWindow_1.LogitudeWindow();
        newWindow.Width = 1050;
        newWindow.Height = 700;
        newWindow.Title = "Connect To Agent Shipment";
        var windowArgs = {};
        newWindow.WindowArgs = windowArgs;
        newWindow.Show('./CommonModules/CommonFilingInbox/Components/ForwarderChooseShipmentsComponent');
        newWindow.ComponentLoaded.subscribe(function (s) {
            newWindow.WindowClosed.subscribe(function (d) {
                var entityList = null;
                entityList = s.SelectedRow;
                if (entityList != null) {
                    if (_this.EntityObjectTableName == "Master") {
                        _this.Customer = entityList.AgentName;
                    }
                    else {
                        _this.Customer = entityList.ShipperName;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(entityList.ForwarderShipmentNumber) && !Tools_1.AppTool.IsNullOrEmpty(entityList.StatusName) && entityList.StatusName.toLocaleLowerCase() != "in progress") {
                        _this.IsDSVConnectEnable = true;
                    }
                    else {
                        _this.IsDSVConnectEnable = false;
                    }
                    _this.Route = entityList.Routing;
                    _this.EntityNumber = Tools_1.AppTool.IsNullOrEmpty(entityList.ForwarderShipmentNumber) ? entityList.CustomerReference1 : entityList.ForwarderShipmentNumber;
                    _this.EntityId = entityList.Id;
                }
            });
        });
    };
    FilingInboxWorkspaceComponent.prototype.ClearConnectToFilter = function () {
        this.QuickSearchItems = [];
        this.EntityNumber = null;
        this.Route = null;
        this.Customer = null;
        this.EntityId = null;
        this.IsDSVConnectEnable = false;
        if (this.SelectedFilingInbox != null) {
            this.SelectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                item.DocumentTypeId = null;
                item.House = null;
                item.Description = null;
                item.IsSharedWithAgent = false;
                item.IsDigitallySign = false;
                item.HouseNumber = null;
                item.TypeSelected = false;
                item.SelectedName = null;
                item.ShowTypes = false;
                item.SelectedValue = null;
                item.IsSingleTick = false;
                item.CellTooltipIconPath = "./Images/double-tick.png";
            });
        }
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "SelectedAttachment", {
        get: function () {
            return this.selectedAttachment;
        },
        set: function (value) {
            var _this = this;
            if (this.selectedAttachment != value) {
                this.selectedAttachment = value;
                this.selectedAttachment.IsSharedWithAgent = this.ShareAsDefault;
                if (Tools_1.AppTool.IsNullOrEmpty(this.selectedAttachment.Description)) {
                    this.selectedAttachment.Description = this.selectedAttachment.FileName != null ? this.selectedAttachment.FileName.split('.')[0] : this.selectedAttachment.FileName;
                }
                this.isMailBody = false;
                if (value != null) {
                    if (value.FileName != null && value.FileName == "Mail Body") {
                        this.isMailBody = true;
                    }
                    if (value.FileName != null && value.FileName.split('.') != null && value.FileName.split('.')[1] != null && (value.FileName.split('.')[1].toUpperCase().trim() == "PDF")) {
                        this.IsPDF = true;
                        this.myCommonDomainService.GetFilingAttachPdfReport(value.DocumentId).subscribe(function (response) {
                            if (!response.HasError) {
                                var buffer = EntityResourceService_1.EntityResourceService.base64ToBufferConvertor(response.Result);
                                var blob = new Blob([buffer], { type: 'application/pdf' });
                                var objectURL = URL.createObjectURL(blob);
                                _this.IFrameURI = objectURL;
                            }
                        });
                    }
                    else {
                        this.IsPDF = false;
                    }
                }
                this.SelectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                    if (value == item) {
                        item.CellTooltipIconPath = "./Images/double-tick-white.png";
                    }
                    else {
                        item.CellTooltipIconPath = "./Images/double-tick.png";
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxWorkspaceComponent.prototype.PreviewButtonClicked = function () {
        this.ViewAttachment();
    };
    FilingInboxWorkspaceComponent.prototype.ViewAttachment = function () {
        var documentName = this.SelectedAttachment.DocumentId;
        DownloadManager_1.DownloadManager.DownloadPage(documentName);
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsMailBody", {
        get: function () {
            return this.isMailBody;
        },
        set: function (value) {
            if (this.isMailBody != value) {
                this.isMailBody = value;
                this.selectedAttachment = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "SelectedFilingInbox", {
        get: function () {
            return this.selectedFilingInbox;
        },
        set: function (value) {
            var _this = this;
            if (this.selectedFilingInbox != value) {
                var hasChanges = false;
                if (this.selectedFilingInbox != null) {
                    this.selectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                            hasChanges = true;
                        }
                    });
                    if (hasChanges) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Width = 450;
                        confirmWindow.Height = 190;
                        confirmWindow.ShowCancelButton = true;
                        confirmWindow.NoButtonText = "Don't Save";
                        confirmWindow.YesButtonText = "Save ";
                        confirmWindow.CancelButtonText = "Cancel";
                        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                        confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                // save filing 
                                _this.FileButtonClicked();
                            }
                            else if (confirmWindow.No) {
                                _this.SetSelectedFilingInbox(value);
                            }
                            else if (confirmWindow.Cancel) {
                                // nth
                            }
                        });
                    }
                    else {
                        this.SetSelectedFilingInbox(value);
                    }
                }
                else {
                    this.SetSelectedFilingInbox(value);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxWorkspaceComponent.prototype.SetSelectedFilingInbox = function (value) {
        this.ClearConnectToFilter();
        this.selectedFilingInbox = value;
        this.EmailBody = value != null ? value.FilingInboxPM.EmailBody : "";
        this.FilingInboxAttachments = value != null ? value.FilingInboxAttachments : [];
        this.isMailBody = false;
        this.SetHtml();
        if (this.FilingInboxAttachments != null && this.FilingInboxAttachments.length > 0) {
            this.SelectedAttachment = this.FilingInboxAttachments[0];
            this.IsFilingButtonEnabled = true;
        }
        else {
            this.IsMailBody = true;
            this.IsFilingButtonEnabled = false;
        }
    };
    FilingInboxWorkspaceComponent.prototype.AttachmentChanged = function (item) {
        this.SelectedAttachment = item;
    };
    FilingInboxWorkspaceComponent.prototype.DeleteFilingInbox = function (filingInbox) {
        var _this = this;
        filingInbox.FilingInboxPM.IsDeleted = true;
        this.myFilingInboxPMService.update(filingInbox.FilingInboxPM).subscribe(function (myRespone) {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    _this.IsVisible = false;
                    _this.LoadAllData();
                }
            }
        });
    };
    FilingInboxWorkspaceComponent.prototype.ViewUser = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: _this.LoggedUserId, ObjectTableName: "User", BackButtonLabel: "Filing Inbox", SelectedTabCode: "USDF"
            });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.GetUser();
            });
        });
    };
    FilingInboxWorkspaceComponent.prototype.CopyDomain = function () {
        var selBox = document.createElement('textarea');
        selBox.style.position = 'fixed';
        selBox.style.left = '0';
        selBox.style.top = '0';
        selBox.style.opacity = '0';
        selBox.value = this.SettingsDomain;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
    };
    FilingInboxWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.IsRefreshClicked = true;
        this.IsVisible = false;
        this.LoadAllData();
    };
    FilingInboxWorkspaceComponent.prototype.FileButtonClicked = function () {
        var _this = this;
        if (this.SelectedFilingInbox != null) {
            this.CurrentSession.StartBusyIndicator("Filing ...");
            var summary = new CommonDomainService_1.FilingInboxSummary();
            var objectTableName = this.EntityObjectTableName;
            if (this.EntityObjectTableName == "Master") {
                objectTableName = "Shipment";
            }
            summary.ObjectTableName = objectTableName;
            summary.IsDeleted = false;
            summary.UserId = this.UserId;
            summary.Attaches = [];
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityNumber)) {
                var isValid = false;
                var isDescriptionFilled = true;
                var docsErrorMsg = "";
                this.SelectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                        if (item.AttachLogs != null && item.AttachLogs.length > 0) {
                            docsErrorMsg += item.FileName + ", ";
                        }
                        var attach = new CommonDomainService_1.FilingInboxAttachItem();
                        attach.FileName = item.FileName;
                        attach.EntityId = item.EntityId;
                        attach.DocumentType = item.DocumentTypeId;
                        attach.House = item.House;
                        attach.HouseNumber = item.HouseNumber;
                        attach.Description = item.Description;
                        attach.DocumentId = item.DocumentId;
                        attach.IsSharedWithAgent = item.IsSharedWithAgent;
                        attach.IsDigitallySign = item.IsDigitallySign;
                        item.IsSingleTick = false;
                        summary.Attaches.push(attach);
                        isValid = true;
                        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && _this.EntityObjectTableName == "Shipment") {
                            if (Tools_1.AppTool.IsNullOrEmpty(item.Description)) {
                                isDescriptionFilled = false;
                            }
                        }
                    }
                });
                if (isValid && isDescriptionFilled) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityNumber)) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(docsErrorMsg)) {
                            this.CurrentSession.StopBusyIndicator();
                            if (docsErrorMsg.match(/,/g).length == 1) {
                                docsErrorMsg = docsErrorMsg.replace(/,/g, '');
                                docsErrorMsg = "The document " + docsErrorMsg + " is already filled";
                            }
                            else {
                                docsErrorMsg = "The documents: " + docsErrorMsg + " are already filled";
                            }
                            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            confirmWindow.NoButtonText = "Cancel";
                            confirmWindow.YesButtonText = "Ok";
                            confirmWindow.Title = "Warning";
                            confirmWindow.Show(docsErrorMsg);
                            confirmWindow.WindowClosed.subscribe(function (event) {
                                if (confirmWindow.Yes) {
                                    _this.CompleteFiling(summary, false);
                                }
                            });
                        }
                        else {
                            this.CompleteFiling(summary, false);
                        }
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();
                        this.ShowMessage("Please Enter Entity number");
                    }
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    var msg = "";
                    if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
                        if (!isDescriptionFilled) {
                            msg += "Please fill Description fields for all attachments. ";
                        }
                    }
                    if (!isValid) {
                        msg += "Please enter at least one document type.";
                    }
                    this.ShowMessage(msg);
                }
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.ShowMessage("Please Enter Entity number");
            }
        }
    };
    FilingInboxWorkspaceComponent.prototype.FileAndDeleteButtonClicked = function (arg) {
        var _this = this;
        if (this.SelectedFilingInbox != null) {
            this.CurrentSession.StartBusyIndicator("Filing & Delete...");
            var summary = new CommonDomainService_1.FilingInboxSummary();
            var objectTableName = this.EntityObjectTableName;
            if (this.EntityObjectTableName == "Master") {
                objectTableName = "Shipment";
            }
            summary.ObjectTableName = objectTableName;
            summary.IsDeleted = true;
            this.SelectedFilingInbox.IsDeleted = true;
            summary.UserId = this.UserId;
            summary.EntityId = this.EntityId;
            summary.FilingId = this.SelectedFilingInbox.FilingInboxPM.Id;
            summary.Attaches = [];
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityNumber)) {
                var isValid = false;
                var isDescriptionFilled = true;
                var docsErrorMsg = "";
                this.SelectedFilingInbox.FilingInboxAttachments.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                        if (item.AttachLogs != null && item.AttachLogs.length > 0) {
                            docsErrorMsg += item.FileName + ", ";
                        }
                        var attach = new CommonDomainService_1.FilingInboxAttachItem();
                        attach.FileName = item.FileName;
                        attach.EntityId = item.EntityId;
                        attach.DocumentType = item.DocumentTypeId;
                        attach.House = item.House;
                        attach.HouseNumber = item.HouseNumber;
                        attach.Description = item.Description;
                        attach.DocumentId = item.DocumentId;
                        attach.IsSharedWithAgent = item.IsSharedWithAgent;
                        attach.IsDigitallySign = item.IsDigitallySign;
                        item.IsSingleTick = false;
                        summary.Attaches.push(attach);
                        isValid = true;
                        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && _this.EntityObjectTableName == "Shipment") {
                            if (Tools_1.AppTool.IsNullOrEmpty(item.Description)) {
                                isDescriptionFilled = false;
                            }
                        }
                    }
                });
                if (isValid && isDescriptionFilled) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityNumber)) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(docsErrorMsg)) {
                            this.CurrentSession.StopBusyIndicator();
                            if (docsErrorMsg.match(/,/g).length == 1) {
                                docsErrorMsg = docsErrorMsg.replace(/,/g, '');
                                docsErrorMsg = "The document " + docsErrorMsg + " is already filled";
                            }
                            else {
                                docsErrorMsg = "The documents: " + docsErrorMsg + " are already filled";
                            }
                            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            confirmWindow.NoButtonText = "Cancel";
                            confirmWindow.YesButtonText = "Ok";
                            confirmWindow.Title = "Warning";
                            confirmWindow.Show(docsErrorMsg);
                            confirmWindow.WindowClosed.subscribe(function (event) {
                                if (confirmWindow.Yes) {
                                    _this.CompleteFiling(summary, arg);
                                }
                            });
                        }
                        else {
                            this.CompleteFiling(summary, arg);
                        }
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();
                        this.ShowMessage("Please Enter Entity number");
                    }
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    var msg = "";
                    if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
                        if (!isDescriptionFilled) {
                            msg += "Please fill Description fields for all attachments. ";
                        }
                    }
                    if (!isValid) {
                        msg += "Please enter at least one document type.";
                    }
                    this.ShowMessage(msg);
                }
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.ShowMessage("Please Enter Entity number");
            }
        }
    };
    FilingInboxWorkspaceComponent.prototype.CompleteFiling = function (summary, arg) {
        var _this = this;
        summary.EntityId = this.EntityId;
        summary.EntityNumber = this.EntityNumber;
        summary.FilingId = this.SelectedFilingInbox.FilingInboxPM.Id;
        this.myCommonDomainService.PutFilingInboxLogs(summary).subscribe(function (response) {
            if (!response.HasError) {
                _this.ClearConnectToFilter();
                _this.IsVisible = false;
                _this.LoadAllData();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
        if (arg == true) {
            // open dsv window
            var hasSharedDocs;
            this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
            this._ShipmentPMService.get(this.EntityId).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                        _this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
                        _this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(_this.EntityId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                            if (res.Result == false && summary.Attaches.filter(function (a) { return a.IsSharedWithAgent == true; }).length == 0) {
                                hasSharedDocs = false;
                            }
                            else {
                                hasSharedDocs = true;
                            }
                            _this.CurrentSession.StopBusyIndicator();
                            var newWindow = new LogitudeWindow_1.LogitudeWindow();
                            newWindow.Width = 1050;
                            newWindow.Height = 700;
                            if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                                newWindow.Title = "Connect/Create new shipment in " + SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                            }
                            else {
                                newWindow.Title = "Connect To Agent Shipment";
                            }
                            var windowArgs = {};
                            windowArgs.SourceEntity = myResult.Result; //this.rowData;
                            windowArgs.HasSharedDocs = hasSharedDocs;
                            newWindow.WindowArgs = windowArgs;
                            newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/ForwarderShipmentsComponent');
                        });
                    }
                }
            });
        }
    };
    FilingInboxWorkspaceComponent.prototype.ShowMessage = function (msg) {
        var myMessageWindow = new MessageWindow_1.MessageWindow();
        myMessageWindow.Show(msg);
    };
    FilingInboxWorkspaceComponent.prototype.NewShipmentClicked = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(function (response) {
            var newWindow = new LogitudeWindow_1.LogitudeWindow();
            newWindow.Width = 600;
            newWindow.Height = 350;
            newWindow.Title = "Create New Shipment";
            var windowArgs = {};
            windowArgs.IsNew = true;
            newWindow.WindowArgs = windowArgs;
            if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditPrivateLabelShipmentComponent');
            }
            else if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") {
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterShipmentComponent');
            }
            else {
                var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
                str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable("Shipment"));
                newWindow.Title = str;
                newWindow.Width = 960;
                newWindow.Height = 570;
                newWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
            }
            newWindow.ComponentLoaded.subscribe(function (s) {
                newWindow.WindowClosed.subscribe(function (d) {
                    var shipment = s.EntityPM;
                    if (shipment != null) {
                        _this.EntityId = shipment.Id;
                        if ((ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") || SessionLocator_1.SessionLocator.PrivateLableSettings) {
                            _this.EntityNumber = Tools_1.AppTool.IsNullOrEmpty(shipment.ForwarderShipmentNumber) ? shipment.CustomerReference1 : shipment.ForwarderShipmentNumber;
                            if (Tools_1.AppTool.IsNullOrEmpty(shipment.ForwarderShipmentNumber) && !Tools_1.AppTool.IsNullOrEmpty(shipment.StatusName) && shipment.StatusName.toLocaleLowerCase() != "in progress") {
                                _this.IsDSVConnectEnable = true;
                            }
                            else {
                                _this.IsDSVConnectEnable = false;
                            }
                        }
                        else {
                            _this.EntityNumber = shipment.ShipmentNumber;
                        }
                    }
                });
            });
        });
    };
    Object.defineProperty(FilingInboxWorkspaceComponent.prototype, "IsSetHtml", {
        get: function () {
            var isSetHtml = false;
            var element = document.getElementById(this.PreviewDivId);
            if (element) {
                isSetHtml = true;
                if (!this.IsStopPreviewHtml) {
                    this.SetHtml();
                    this.IsStopPreviewHtml = true;
                }
            }
            return isSetHtml;
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxWorkspaceComponent.prototype.DSVConnectClicked = function () {
        this.FileAndDeleteButtonClicked(true);
    };
    FilingInboxWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FilingInboxWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FilingInboxWorkspaceComponent);
    return FilingInboxWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.FilingInboxWorkspaceComponent = FilingInboxWorkspaceComponent;
var FilingInboxData = /** @class */ (function () {
    function FilingInboxData(filingInboxPM, father) {
        this.father = father;
        this.FilingInboxAttachments = [];
        this.Background = "White";
        this.HasAttachmanets = false;
        this.FilingInboxPM = filingInboxPM;
        this.FillFilingInboxAttachments();
        this.GetHasAttachmanets();
        if (this.IsDeleted) {
            this.Background = "#F7E3E3";
        }
    }
    FilingInboxData.prototype.FillFilingInboxAttachments = function () {
        var _this = this;
        this.FilingInboxAttachments = [];
        this.FilingInboxAttachments = [];
        //this.FilingInboxPM.FilingInboxAttachments.filter(a => a.FileName != null && (a.FileName.split('.')[1] != null && a.FileName.split('.')[1].toUpperCase() == "PDF")).forEach(item => {
        //    this.FilingInboxAttachments.push(new FilingInboxAttachment(item, this.father));
        //});
        //this.FilingInboxPM.FilingInboxAttachments.filter(a => a.FileName != null && (a.FileName.split('.')[1] != null && a.FileName.split('.')[1].toUpperCase() != "PDF")).forEach(item => {
        //    this.FilingInboxAttachments.push(new FilingInboxAttachment(item, this.father));
        //});
        this.FilingInboxPM.FilingInboxAttachments.filter(function (a) { return a.FileName != null; }).forEach(function (item) {
            _this.FilingInboxAttachments.push(new FilingInboxAttachment(item, _this.father));
        });
    };
    FilingInboxData.prototype.GetHasAttachmanets = function () {
        if (this.FilingInboxAttachments.length > 0) {
            this.HasAttachmanets = true;
        }
        else {
            this.HasAttachmanets = false;
        }
    };
    Object.defineProperty(FilingInboxData.prototype, "Subject", {
        get: function () {
            return this.FilingInboxPM.Subject;
        },
        set: function (value) {
            if (this.FilingInboxPM.Subject != value) {
                this.FilingInboxPM.Subject = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxData.prototype, "IsDeleted", {
        get: function () {
            return this.FilingInboxPM.IsDeleted;
        },
        set: function (value) {
            if (this.FilingInboxPM.IsDeleted != value) {
                this.FilingInboxPM.IsDeleted = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxData.prototype, "CreateDate", {
        get: function () {
            return this.FilingInboxPM.CreateDate;
        },
        set: function (value) {
            if (this.FilingInboxPM.CreateDate != value) {
                this.FilingInboxPM.CreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxData.prototype, "SenderName", {
        get: function () {
            return this.FilingInboxPM.SenderName;
        },
        set: function (value) {
            if (this.FilingInboxPM.SenderName != value) {
                this.FilingInboxPM.SenderName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return FilingInboxData;
}());
exports.FilingInboxData = FilingInboxData;
var FilingInboxAttachment = /** @class */ (function (_super) {
    __extends(FilingInboxAttachment, _super);
    function FilingInboxAttachment(entity, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.DataContext = _this;
        _this.AttachLogs = [];
        _this.QuickSearchHouses = [];
        _this.HouseFilters = null;
        _this.IsSingleTick = false;
        _this.cellTooltipIconPath = "./Images/double-tick.png";
        _this.EntityNumberColumnWidth = 100;
        _this.house = null;
        _this.houseNumber = null;
        _this.description = null;
        _this.documentTypeId = null;
        // Document Filling 
        _this.ShowTypes = false;
        _this.SelectedValue = "";
        _this.SelectedName = "";
        _this.TypeSelected = false;
        _this.isSharedWithAgent = false;
        _this.isDigitallySign = false;
        _this.entity = entity;
        _this.AttachLogs = entity != null ? entity.AttachLogs : [];
        if (_this.AttachLogs != null && _this.AttachLogs.length > 0) {
            _this.IsSingleTick = false;
        }
        _this.IsSharedWithAgent = father.ShareAsDefault;
        _this.CalculatingWidth();
        _this.HouseFilters = new ApiQueryFilters_1.ApiQueryFilters();
        _this.HouseFilters.PageIndex = 0;
        _this.HouseFilters.PageSize = 10;
        _this.SetDescriptionUIProperties();
        _this.FillDocumentFiling();
        return _this;
    }
    Object.defineProperty(FilingInboxAttachment.prototype, "CellTooltipIconPath", {
        get: function () {
            return this.cellTooltipIconPath;
        },
        set: function (value) {
            if (this.cellTooltipIconPath != value) {
                this.cellTooltipIconPath = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxAttachment.prototype.SetDescriptionUIProperties = function () {
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.father.EntityObjectTableName == "Shipment") {
            // var isreq = AppTool.IsNullOrEmpty(this.Description);
            //this.UIProperties.SetRequired("Description", null, isreq);
        }
    };
    FilingInboxAttachment.prototype.CalculatingWidth = function () {
        var entityNumberColumnWidth = 100;
        if (this.AttachLogs != null && this.AttachLogs.length > 0) {
            this.AttachLogs.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityNumber)) {
                    var textWidth = Tools_1.AppTool.GetTextWidth(item.EntityNumber, 12) + 10;
                    if (textWidth > entityNumberColumnWidth) {
                        entityNumberColumnWidth = textWidth;
                    }
                }
            });
        }
        this.EntityNumberColumnWidth = entityNumberColumnWidth;
    };
    Object.defineProperty(FilingInboxAttachment.prototype, "EntityId", {
        get: function () {
            if (this.entity != null) {
                return this.entity.Id;
            }
            else {
                return null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxAttachment.prototype, "FileName", {
        get: function () {
            if (this.entity != null) {
                return this.entity.FileName;
            }
            else {
                return null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxAttachment.prototype, "DocumentId", {
        get: function () {
            if (this.entity != null) {
                return this.entity.DocumentId;
            }
            else {
                return null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxAttachment.prototype, "House", {
        get: function () {
            return this.house;
        },
        set: function (value) {
            if (this.house != value) {
                this.house = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxAttachment.prototype, "HouseNumber", {
        get: function () {
            return this.houseNumber;
        },
        set: function (value) {
            if (this.houseNumber != value) {
                this.houseNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxAttachment.prototype, "Description", {
        get: function () {
            return this.description;
        },
        set: function (value) {
            if (this.description != value) {
                this.description = value;
                this.SetDescriptionUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxAttachment.prototype.SetHousesFilters = function () {
        this.HouseFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.HouseFilters.PageIndex = 0;
        this.HouseFilters.PageSize = 100;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.father.EntityId)) {
            this.HouseFilters.addAdditionalFilter("MasterShipmentDataId", this.father.EntityId, null, null, "Equals", false, false, false, "string");
        }
        this.HouseFilters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, true, false, "string");
        this.HouseFilters.addAdditionalFilter("MasterConnectedHouses", true, null, null, "Equals", true, false, false, "Boolean");
    };
    FilingInboxAttachment.prototype.QuickSearchHouseChanged = function (entity) {
        if (entity != null) {
            this.House = entity.Id;
            this.HouseNumber = entity.ShipmentNumber;
        }
    };
    Object.defineProperty(FilingInboxAttachment.prototype, "DocumentTypeId", {
        get: function () {
            return this.documentTypeId;
        },
        set: function (value) {
            if (this.documentTypeId != value) {
                this.documentTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxAttachment.prototype.FillDocumentFiling = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DocumentTypeId)) {
            this.SelectedValue = "";
            this.ShowTypes = false;
        }
        else {
            if (this.father.DocumentTypeList.filter(function (a) { return a.Id == _this.DocumentTypeId; }).length == 0) {
                this.SelectedValue = "O";
                this.ShowTypes = false;
                this.TypeSelected = true;
                this.SelectedName = this.DocumentType.Name;
            }
            else {
                var myTempData = this.father.DocumentTypeList.filter(function (a) { return a.Id == _this.DocumentTypeId; })[0];
                this.DocumentTypeId = myTempData.Id;
                this.SelectedValue = "";
                this.ShowTypes = false;
                this.TypeSelected = true;
                this.SelectedName = myTempData.Name;
            }
        }
    };
    FilingInboxAttachment.prototype.itemClicked = function (itemValue, Name) {
        if (this.DocumentTypeId != itemValue) {
            if (itemValue == "O") {
                this.ShowTypes = true;
                this.SelectedValue = "O";
                this.DocumentTypeId = "";
                this.TypeSelected = false;
                this.SelectedName = "";
            }
            else {
                this.DocumentTypeId = itemValue;
                this.ShowTypes = false;
                this.SelectedValue = itemValue;
                this.TypeSelected = true;
                this.SelectedName = Name;
            }
            this.IsSingleTick = true;
        }
    };
    FilingInboxAttachment.prototype.RedxClick = function () {
        this.SelectedName = "";
        this.TypeSelected = false;
        this.DocumentTypeId = "";
        if (this.SelectedValue == "O") {
            this.ShowTypes = true;
        }
        this.IsSingleTick = false;
    };
    Object.defineProperty(FilingInboxAttachment.prototype, "DocumentType", {
        get: function () { return this.documentType; },
        set: function (newValue) {
            this.documentType = newValue;
            if (newValue) {
                this.DocumentTypeId = newValue.Id;
            }
        },
        enumerable: true,
        configurable: true
    });
    FilingInboxAttachment.prototype.OnDocumentTypeChanged = function (event) {
        if (event) {
            this.DocumentType = event;
            this.TypeSelected = true;
            this.SelectedName = event.Name;
            this.ShowTypes = false;
        }
    };
    Object.defineProperty(FilingInboxAttachment.prototype, "IsSharedWithAgent", {
        get: function () {
            return this.isSharedWithAgent;
        },
        set: function (value) {
            if (this.isSharedWithAgent != value) {
                this.isSharedWithAgent = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilingInboxAttachment.prototype, "IsDigitallySign", {
        get: function () {
            return this.isDigitallySign;
        },
        set: function (value) {
            if (this.isDigitallySign != value) {
                this.isDigitallySign = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return FilingInboxAttachment;
}(BaseComponent_1.BaseComponent));
exports.FilingInboxAttachment = FilingInboxAttachment;
//# sourceMappingURL=FilingInboxWorkspaceComponent.js.map