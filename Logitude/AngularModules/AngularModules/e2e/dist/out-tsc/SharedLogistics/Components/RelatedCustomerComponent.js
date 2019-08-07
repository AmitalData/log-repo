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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CustomerTenantAccessCardPM_1 = require("../../Common/EntityPMs/CustomerTenantAccessCardPM");
var EntityArgs_1 = require("../../Infrastructure/DataContracts/EntityArgs");
var CommonDomainService_1 = require("../../Common/Services/CommonDomainService");
var PartnersDomainService_1 = require("../../Common/Services/PartnersDomainService");
var Tools_1 = require("../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var QueueMessageMoreDetailsListService_1 = require("../../Infrastructure/Services/StandardLists/QueueMessageMoreDetailsListService");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var APILogsListService_1 = require("../../Infrastructure/Services/StandardLists/APILogsListService");
var UserListService_1 = require("../../Common/Services/StandardLists/UserListService");
var RelatedCustomerComponent = /** @class */ (function (_super) {
    __extends(RelatedCustomerComponent, _super);
    function RelatedCustomerComponent(entityArgs, _entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObsList = [];
        _this.BatchObsList = [];
        _this.QueryObsList = [];
        _this.APILogsObsList = [];
        _this.QueueNoDataTextBlockVisibility = false;
        _this.QueriesList = [];
        _this.IsShowTipIcon = false;
        _this.IsShowTipArea = false;
        _this.IsTipsOpened = false;
        _this.IsFirstTipLoad = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isAddEnabled = true;
        _this.batchVisibility = false;
        _this.tipVisibility = false;
        _this.isEnabled = true;
        _this.CurrentSession.StartBusyIndicatorLoading();
        _this._entityResourceService.getEntityResourceByTableName("CustomerTenantAccess", 0).subscribe(function (response) {
            _this.EntityPM = entityArgs.EntityPM;
            _this.ObjectTableName = "CustomerTenantAccess";
            if (_this.EntityPM.CustomerTenantAccessCards.length == 0) {
                _this.IsShowTipArea = true;
                var table = window.ObjectTables.filter(function (d) { return d.Name == "CustomerTenantAccessCard" && (d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || d.Tenant == 0); })[0];
                if (table) {
                    _this._entityResourceService.getEntityResourceByTableName("CustomerTenantAccessCard", 0).subscribe(function (response) {
                        var Tip = window.Tips.filter(function (d) { return d.Code == "NCDT" && d.ObjectTableId == table.Id; })[0];
                        var TipsVisibility = window.TipsVisibilities.filter(function (d) { return d.TipCode == Tip.Code && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId; })[0];
                        if (TipsVisibility != null)
                            _this.IsTipsOpened = TipsVisibility.IsVisible;
                        else
                            _this.IsTipsOpened = false;
                    });
                }
            }
            _this.BuildData();
            _this.FillQueriesList();
        });
        return _this;
    }
    Object.defineProperty(RelatedCustomerComponent.prototype, "SelectedLogItem", {
        get: function () { return this.selectedLogItem; },
        set: function (value) {
            if (this.selectedLogItem != value)
                this.selectedLogItem = value;
        },
        enumerable: true,
        configurable: true
    });
    RelatedCustomerComponent.prototype.ViewLog = function (itemComponent) {
        if (itemComponent.Id != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: itemComponent.Id, ObjectTableName: 'APILogs', BackButtonLabel: "Back" });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                });
            });
        }
    };
    RelatedCustomerComponent.prototype.FillQueriesList = function () {
        var TodayBatch = new BatchQueriesData(0, "Today");
        var LastWeekBatch = new BatchQueriesData(1, "Last Week");
        var LastMonthBatch = new BatchQueriesData(2, "Last Month");
        this.QueriesList.push(TodayBatch);
        this.QueriesList.push(LastWeekBatch);
        this.QueriesList.push(LastMonthBatch);
        this.SelectedQueryItem = this.QueriesList[1];
        this.SelectedLogItem = this.QueriesList[1];
    };
    RelatedCustomerComponent.prototype.TipVisibilityChanged = function () {
        this.IsTipsOpened = !this.IsTipsOpened;
    };
    RelatedCustomerComponent.prototype.EditRelatedCustomer = function (item) {
        var _this = this;
        this.TenantAccessCard = item.EntityPM;
        var service = new CommonDomainService_1.CommonDomainService();
        this.CurrentSession.StartBusyIndicatorLoading();
        service.GetSingleCustomerTenantAccess(this.EntityPM.Id).subscribe(function (res) {
            if (!res.HasError) {
                _this.RealCustomerTenantAccessPM = res.Result;
                var entityService = new EntityResourceService_1.EntityResourceService();
                entityService.getEntityResourceByTableName("CustomerTenantAccessCard", 0).subscribe(function (p) {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.WindowArgs = { EntityPM: _this.EntityPM.CustomerTenantAccessCards.filter(function (a) { return a.CustomerId == _this.TenantAccessCard.CustomerId; })[0], Parent: _this };
                    logitudeWindow.Title = "Edit Card" + " - " + _this.TenantAccessCard.CustomerCode + " - " + _this.TenantAccessCard.CustomerName;
                    logitudeWindow.Show('./SharedLogistics/Components/EditRelatedCustomerComponent');
                    logitudeWindow.ComponentLoaded.subscribe(function (p) {
                        _this.CurrentSession.StopBusyIndicator();
                    });
                    logitudeWindow.WindowClosed.subscribe(function (p) {
                        if (p == "OK") {
                            _this.BuildData();
                        }
                    });
                });
            }
        });
    };
    RelatedCustomerComponent.prototype.LogsSelectedChange = function ($event) {
        var _this = this;
        this.SelectedLogItem = $event;
        var service = new APILogsListService_1.APILogsListService();
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        var TodayDate = Tools_1.DateTool.TruncateTime(new Date());
        var YesterdayDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetDateParts(new Date()).DateObject, -1);
        var LastSevenDaysDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetDateParts(new Date()).DateObject, -7);
        var LastThirtyDaysDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetDateParts(new Date()).DateObject, -30);
        var value;
        if ($event.Name == "Today") {
            value = TodayDate;
        }
        else if ($event.Name == "Last Week") {
            value = LastSevenDaysDate;
        }
        else if ($event.Name == "Last Month") {
            value = LastThirtyDaysDate;
        }
        filters.addAdditionalFilter("CreateDate", value, null, null, "GreaterThanOrEqual", false, true, false, "datetime");
        filters.PageSize = 100;
        filters.PageIndex = 0;
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";
        service.getByFilters(filters).subscribe(function (result) {
            _this.APILogsObsList = result.Result.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.CreateDate) === Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.CreateDate) > Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1; });
        });
    };
    RelatedCustomerComponent.prototype.QueriesSelectedChange = function ($event) {
        var _this = this;
        var service = new QueueMessageMoreDetailsListService_1.QueueMessageMoreDetailsListService();
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageSize = 100;
        filters.PageIndex = 0;
        filters.SortBy = "CreateDateTime";
        filters.SortDirection = "Descending";
        var TodayDate = Tools_1.DateTool.TruncateTime(new Date());
        var YesterdayDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetDateParts(new Date()).DateObject, -1);
        var LastSevenDaysDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetDateParts(new Date()).DateObject, -7);
        var LastThirtyDaysDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetDateParts(new Date()).DateObject, -30);
        var value;
        if ($event.Name == "Today") {
            value = TodayDate;
        }
        else if ($event.Name == "Last Week") {
            value = LastSevenDaysDate;
        }
        else if ($event.Name == "Last Month") {
            value = LastThirtyDaysDate;
        }
        filters.addAdditionalFilter("CreateDateTime", value, null, null, "GreaterThanOrEqual", false, true, false, "datetime");
        service.getByFilters(filters).subscribe(function (result) {
            _this.QueryObsList = result.Result;
        });
    };
    RelatedCustomerComponent.prototype.AddRelatedCustomer = function () {
        var _this = this;
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetSingleCustomerTenantAccess(this.EntityPM.Id).subscribe(function (res) {
            if (!res.HasError) {
                _this.RealCustomerTenantAccessPM = res.Result;
                var customerTenantAccessCardPM = new CustomerTenantAccessCardPM_1.CustomerTenantAccessCardPM(_this.EntityPM);
                customerTenantAccessCardPM.CustomerTenantAccessId = _this.RealCustomerTenantAccessPM.Id;
                customerTenantAccessCardPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                customerTenantAccessCardPM.UpdateDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                customerTenantAccessCardPM.CreateByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                var viewModel = new AddEditCustomerTenantAccessCardViewModel(_this.RealCustomerTenantAccessPM, customerTenantAccessCardPM, true, _this);
                viewModel.DataLoaded.subscribe(function (output) {
                    if (output) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.WindowArgs = viewModel;
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerTenantAccess.O.RelatedCustomers.AddRelatedCustomer");
                        logitudeWindow.Show('./SharedLogistics/Components/AddEditCustomerTenantAccessCardComponent');
                    }
                });
            }
        });
    };
    Object.defineProperty(RelatedCustomerComponent.prototype, "IsAddEnabled", {
        get: function () { return this.isAddEnabled; },
        set: function (value) { if (this.isAddEnabled != value)
            this.isAddEnabled = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedCustomerComponent.prototype, "BatchVisibility", {
        get: function () { return this.batchVisibility; },
        set: function (value) { if (this.batchVisibility != value)
            this.batchVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedCustomerComponent.prototype, "SelectedItemBatch", {
        get: function () { return this.selectedItemBatch; },
        set: function (value) { if (this.selectedItemBatch != value)
            this.selectedItemBatch = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedCustomerComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (value) {
            if (this.selectedTabCode != value) {
                this.selectedTabCode = value;
                this.SetBatchTitle();
            }
        },
        enumerable: true,
        configurable: true
    });
    RelatedCustomerComponent.prototype.RefreshRelatedCustomers = function () {
        this.BuildData();
    };
    RelatedCustomerComponent.prototype.RefreshBatch = function () {
        this.getBatchData();
    };
    RelatedCustomerComponent.prototype.RefreshLogs = function () {
        this.LogsSelectedChange(this.SelectedLogItem);
    };
    RelatedCustomerComponent.prototype.RefreshQueues = function () {
        this.QueriesSelectedChange(this.SelectedQueryItem);
    };
    RelatedCustomerComponent.prototype.AddBatch = function () {
        var _this = this;
        if (this.SelectedCardPM != null) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.WindowArgs = { AccessCardPM: this.SelectedCardPM, Parent: this };
            logitudeWindow.Title = "Add New Batch";
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Show('./SharedLogistics/Components/AddCustomerBatchComponent');
            logitudeWindow.WindowClosed.subscribe(function (p) {
                if (p == "OK") {
                    _this.BuildData();
                }
            });
        }
    };
    RelatedCustomerComponent.prototype.SetBatchTitle = function () {
        if (this.SelectedTabCode == "B") {
            this.BatchTitle = "Card " + this.SelectedItem.EntityPM.CustomerCode + " - " + this.SelectedItem.CustomerName + " Batch Build History (last 100)";
        }
        else if (this.SelectedTabCode == "L") {
            this.BatchTitle = "Card " + this.SelectedItem.EntityPM.CustomerCode + " - " + this.SelectedItem.CustomerName + " Logs History (last 100)";
            this.LogsSelectedChange(this.SelectedLogItem);
        }
        else if (this.SelectedTabCode == "Q") {
            this.BatchTitle = "Card " + this.SelectedItem.EntityPM.CustomerCode + " - " + this.SelectedItem.CustomerName + " Queues History (last 100) ";
            this.QueriesSelectedChange(this.SelectedQueryItem);
        }
    };
    Object.defineProperty(RelatedCustomerComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
                this.SelectedCardPM = value.EntityPM;
                this.getBatchData();
            }
        },
        enumerable: true,
        configurable: true
    });
    RelatedCustomerComponent.prototype.getBatchData = function () {
        var _this = this;
        this.BatchVisibility = true;
        var service = new CommonDomainService_1.CommonDomainService();
        this.BatchObsList = [];
        service.GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(this.SelectedItem.EntityPM.CustomerId, this.SelectedItem.EntityPM.CustomerTenantAccessId).subscribe(function (res) {
            if (!res.HasError) {
                var CustomerTenantAccessCardsBatchpms = res.Result;
                CustomerTenantAccessCardsBatchpms.forEach(function (item) {
                    _this.BatchObsList.push(new CustomerTenantAccessCardsBatchDataViewModel(item));
                });
            }
            _this.SelectedTabCode = "B";
            _this.SetBatchTitle();
        });
    };
    RelatedCustomerComponent.prototype.BuildData = function () {
        var _this = this;
        this.IsEnabled = false;
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetSingleCustomerTenantAccess(this.EntityPM.Id).subscribe(function (res) {
            _this.ObsList = [];
            if (!res.HasError) {
                var list = res.Result.CustomerTenantAccessCards;
                var customerTenantAccesspm = res.Result;
                list.forEach(function (item) {
                    _this.ObsList.push(new AddEditCustomerTenantAccessCardViewModel(res.Result, item, false, _this.DataContext));
                });
                if (_this.ObsList.length > 0)
                    _this.SelectedItem = _this.ObsList[0];
                var temp = _this.ObsList.filter(function (a) { return a.StatusTypeCode != "IA"; });
                if (customerTenantAccesspm.IsPrivateLabelCustomer == true) {
                    if (temp.length > 0) {
                        _this.IsAddEnabled = false;
                    }
                    else {
                        _this.IsAddEnabled = true;
                    }
                }
                _this.IsEnabled = true;
                if (_this.ObsList == null || _this.ObsList.length == 0) {
                    _this.TipVisibility = true;
                }
                else {
                    _this.TipVisibility = false;
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Object.defineProperty(RelatedCustomerComponent.prototype, "TipVisibility", {
        get: function () { return this.tipVisibility; },
        set: function (value) { if (this.tipVisibility != value)
            this.tipVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedCustomerComponent.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) { if (this.isEnabled != value)
            this.isEnabled = value; },
        enumerable: true,
        configurable: true
    });
    RelatedCustomerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'RelatedCustomerComponent',
            templateUrl: './RelatedCustomerComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], RelatedCustomerComponent);
    return RelatedCustomerComponent;
}(BaseComponent_1.BaseComponent));
exports.RelatedCustomerComponent = RelatedCustomerComponent;
var AddEditCustomerTenantAccessCardViewModel = /** @class */ (function (_super) {
    __extends(AddEditCustomerTenantAccessCardViewModel, _super);
    function AddEditCustomerTenantAccessCardViewModel(customertenantAccessPM, entityPM, isNew, Parent) {
        var _this = _super.call(this) || this;
        _this.CardObsList = [];
        _this.isNew = false;
        _this.DataLoaded = new core_1.EventEmitter();
        _this.DataContext = _this;
        _this.changeHybridStartDateEnable = false;
        _this.EntityPM = entityPM;
        _this.customertenantAccessPM = customertenantAccessPM;
        _this.isNew = isNew;
        _this.Parent = Parent;
        _this.setCreatedByName();
        if (isNew) {
            _this.LoadCardList();
        }
        return _this;
    }
    AddEditCustomerTenantAccessCardViewModel.prototype.setCreatedByName = function () {
        var _this = this;
        var service = new UserListService_1.UserListService();
        service.getSingleFromCache(this.EntityPM.CreateByUserId).subscribe(function (resp) {
            if (!resp.HasError) {
                var result = resp;
                var list = result.Result;
                if (list != null) {
                    _this.createByUserId = list.EnglishName;
                }
            }
        });
    };
    AddEditCustomerTenantAccessCardViewModel.prototype.LoadCardList = function () {
        var _this = this;
        var service = new PartnersDomainService_1.PartnersDomainService();
        this.CardObsList = [];
        service.GetCustomerCardListByTenantVatNumber(this.customertenantAccessPM.CompanyVat).subscribe(function (res) {
            if (!res.HasError) {
                var tempList = [];
                var list = res.Result;
                list.forEach(function (item) {
                    var AccessCardPM = _this.customertenantAccessPM.CustomerTenantAccessCards.filter(function (a) { return a.Tenant == item.Tenant && a.CustomerId == item.Id; })[0];
                    _this.CardObsList.push(new CardListDataViewModel(item, _this.customertenantAccessPM, _this, AccessCardPM));
                });
                _this.DataLoaded.emit(true);
            }
        });
    };
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (value) { if (value != this.EntityPM.CustomerId)
            this.EntityPM.CustomerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "UpdateDateTime", {
        get: function () { return this.EntityPM.UpdateDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "LastShipmentDateInQueue", {
        get: function () { return this.EntityPM.LastShipmentDateInQueue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "StatusType", {
        get: function () { return this.EntityPM.StatusType; },
        set: function (value) { if (this.EntityPM.StatusType != value)
            this.EntityPM.StatusType = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "StatusTypeCode", {
        get: function () { return this.EntityPM.StatusTypeCode; },
        set: function (value) { if (this.EntityPM.StatusTypeCode != value)
            this.EntityPM.StatusTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "CreateByUserId", {
        get: function () {
            return this.createByUserId;
        },
        set: function (value) {
            if (this.createByUserId != value)
                this.createByUserId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "CustomerCode", {
        get: function () { return this.EntityPM.CustomerCode; },
        set: function (value) { if (this.EntityPM.CustomerCode != value)
            this.EntityPM.CustomerCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "CustomerName", {
        get: function () { return this.EntityPM.CustomerName; },
        set: function (value) { if (this.CustomerName != value)
            this.EntityPM.CustomerName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "HybridStartDate", {
        get: function () {
            if (this.EntityPM.HybridStartDate == null || this.EntityPM.HybridStartDate.getFullYear() == 1 || this.EntityPM.HybridStartDate == Tools_1.DateTool.GetDateFormats(new Date()).DateParts.DateObject) {
                var date = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                date.setDate(date.getDate() - 14);
                this.EntityPM.HybridStartDate = date;
            }
            return this.EntityPM.HybridStartDate;
        },
        set: function (value) { if (this.EntityPM.HybridStartDate != value)
            this.EntityPM.HybridStartDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerTenantAccessCardViewModel.prototype, "ChangeHybridStartDateEnable", {
        get: function () {
            if (this.EntityPM.StatusType == "Accepted") {
                return true;
            }
            else {
                return false;
            }
        },
        set: function (value) {
            if (this.changeHybridStartDateEnable != value)
                this.changeHybridStartDateEnable = value;
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], AddEditCustomerTenantAccessCardViewModel.prototype, "DataLoaded", void 0);
    return AddEditCustomerTenantAccessCardViewModel;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomerTenantAccessCardViewModel = AddEditCustomerTenantAccessCardViewModel;
var CardListDataViewModel = /** @class */ (function () {
    function CardListDataViewModel(entityList, customertenantAccessPM, Parent, AccessCard) {
        var _this = this;
        this.isSelected = false;
        this.entityList = entityList;
        this.CustomerTenantAccessPM = customertenantAccessPM;
        this.Parent = Parent;
        this.AccessCardsPms = AccessCard;
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetCustomerTenantAccessCard(this.entityList.Id).subscribe(function (res) {
            if (!res.HasError) {
                var AccessCards = res.Result;
                if (AccessCards != null) {
                    _this.publicAccessCard = AccessCards;
                }
            }
        });
    }
    Object.defineProperty(CardListDataViewModel.prototype, "IsCustomerCanChecked", {
        get: function () {
            var isCustomerCanChecked = true;
            if (this.publicAccessCard != null) {
                isCustomerCanChecked = false;
            }
            return isCustomerCanChecked;
        },
        set: function (value) { if (value != this.isCustomerCanChecked)
            this.isCustomerCanChecked = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "IsCustomerCanCheckSubmiting", {
        get: function () { return this.isCustomerCanChecked; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "IsCardsHasSelectedOpacity", {
        get: function () {
            var _this = this;
            var isCardsHasSelectedOpacity = 1;
            if (this.CustomerTenantAccessPM.CustomerTenantAccessCards.filter(function (s) { return s.CustomerId == _this.Id; })[0]) {
                isCardsHasSelectedOpacity = 0.5;
            }
            return isCardsHasSelectedOpacity;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "EnglishName", {
        get: function () { return this.entityList.EnglishName; },
        set: function (value) { if (this.entityList.EnglishName != value)
            this.entityList.EnglishName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "VatNumber", {
        get: function () { return this.entityList.VatNumber; },
        set: function (value) { if (this.entityList.VatNumber != value)
            this.entityList.VatNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "Code", {
        get: function () { return this.entityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "IsSelected", {
        get: function () {
            if (this.AccessCardsPms != null || !this.IsCustomerCanChecked) {
                return true;
            }
            return this.isSelected;
        },
        set: function (value) {
            this.isSelected = value;
            if (value) {
                this.UnckechOthers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CardListDataViewModel.prototype, "IsSelectedSubmited", {
        get: function () { return this.isSelected; },
        enumerable: true,
        configurable: true
    });
    CardListDataViewModel.prototype.UnckechOthers = function () {
        var _this = this;
        this.Parent.CardObsList.forEach(function (item) {
            if (item.IsCustomerCanChecked) {
                if (_this.Id != item.Id) {
                    item.IsSelected = false;
                }
            }
        });
    };
    return CardListDataViewModel;
}());
exports.CardListDataViewModel = CardListDataViewModel;
var BatchQueriesData = /** @class */ (function () {
    function BatchQueriesData(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(BatchQueriesData.prototype, "Code", {
        get: function () {
            return this.code;
        },
        set: function (value) { if (this.code != value)
            this.code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchQueriesData.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) { if (this.name != value)
            this.name = value; },
        enumerable: true,
        configurable: true
    });
    return BatchQueriesData;
}());
exports.BatchQueriesData = BatchQueriesData;
var CustomerTenantAccessCardsBatchDataViewModel = /** @class */ (function () {
    function CustomerTenantAccessCardsBatchDataViewModel(entityPM) {
        this.EntityPM = entityPM;
    }
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "BatchNumber", {
        get: function () { return this.EntityPM.BatchNumber; },
        set: function (value) { if (this.EntityPM.BatchNumber != value)
            this.EntityPM.BatchNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "DoneDate", {
        get: function () { return this.EntityPM.DoneDate; },
        set: function (value) { if (this.EntityPM.DoneDate != value)
            this.EntityPM.DoneDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "CreateDateTime", {
        get: function () { return this.EntityPM.CreateDateTime; },
        set: function (value) { if (this.EntityPM.CreateDateTime != value)
            this.EntityPM.CreateDateTime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "FromDatetime", {
        get: function () { return this.EntityPM.FromDatetime; },
        set: function (value) { if (this.EntityPM.FromDatetime != value)
            this.EntityPM.FromDatetime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "ToDatetime", {
        get: function () { return this.EntityPM.ToDatetime; },
        set: function (value) { if (this.EntityPM.ToDatetime != value)
            this.EntityPM.ToDatetime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "Status", {
        get: function () { return this.EntityPM.Status; },
        set: function (value) { if (this.EntityPM.Status != value)
            this.EntityPM.Status = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "TotalFailed", {
        get: function () { return this.EntityPM.TotalFailed; },
        set: function (value) { if (this.EntityPM.TotalFailed != value)
            this.EntityPM.TotalFailed = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "TotalShipment", {
        get: function () { return this.EntityPM.TotalShipment; },
        set: function (value) { if (this.EntityPM.TotalShipment != value)
            this.EntityPM.TotalShipment = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerTenantAccessCardsBatchDataViewModel.prototype, "Totalsucceeded", {
        get: function () { return this.EntityPM.Totalsucceeded; },
        set: function (value) { if (this.EntityPM.Totalsucceeded != value)
            this.EntityPM.Totalsucceeded = value; },
        enumerable: true,
        configurable: true
    });
    return CustomerTenantAccessCardsBatchDataViewModel;
}());
exports.CustomerTenantAccessCardsBatchDataViewModel = CustomerTenantAccessCardsBatchDataViewModel;
//# sourceMappingURL=RelatedCustomerComponent.js.map