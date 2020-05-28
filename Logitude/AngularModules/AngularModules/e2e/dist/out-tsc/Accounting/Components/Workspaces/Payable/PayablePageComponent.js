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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AccountingSummery_1 = require("../../../DataContracts/AccountingSummery");
var Tools_1 = require("../../../../Infrastructure/Tools");
var APPaymentPM_1 = require("../../../../Invoice/EntityPMs/APPaymentPM");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ModulesService_1 = require("../../../Services/ModulesService");
var PayablePageComponent = /** @class */ (function () {
    function PayablePageComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        this.glAccountSummary = new AccountingSummery_1.GLAccountSummary();
        // Queries
        this.collectorsGLAVisibility = false;
        this.debetorsGLAVisibility = false;
        this.activeVendorsGLAVisibility = false;
        this.inactiveVendorsGlaVisibility = false;
        this.CLIENTGLACCOUNTSGlaVisibility = false;
        this.RecentGLAccountsCount = 0;
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("APPayment").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("APInvoice").subscribe(function (response) { });
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.LoadAllScreenData();
    }
    PayablePageComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    };
    PayablePageComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    PayablePageComponent.prototype.LoadAllScreenData = function () {
        this.LoadRecentGLAccounts();
        this.LoadQueriesCounts();
    };
    PayablePageComponent.prototype.SetQueriesVisibility = function () {
        //this.collectorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "collectorsGLA") ? true : false;
        //this.debetorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "debetorsGLA") ? true : false;
        this.activeVendorsGLAVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "activeVendorsGLA") ? true : false;
        this.inactiveVendorsGlaVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "inactiveVendorsGla") ? true : false;
        this.CLIENTGLACCOUNTSGlaVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "VENDORGLACCOUNTS") ? true : false;
    };
    // Vendors
    PayablePageComponent.prototype.RunNewVendorWizard = function () {
        //var windowTitle = "New Vendor";
        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 700;
        //logWindow.Height = 500;
        //logWindow.Title = windowTitle;
        ////logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        //logWindow.Show('./Accounting/Components/NewEntity/NewCashBookComponent');
    };
    PayablePageComponent.prototype.ViewVendorQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "Vendor Accounts";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                // MyVendorsAsCollectors
                // DebetorsVendors
                // ActiveVendorsGLAccounts
                // InactiveVendorsGLAccount
                //case "MyVendorsAsCollectors":
                //    {
                //        displayTitle = "My Vendors (As Collectors)";
                //        break;
                //    }
                //case "DebetorsVendors":
                //    {
                //        displayTitle = "Debtors Vendors";
                //        break;
                //    }
                case "ActiveVendorsGLAccounts":
                    {
                        displayTitle = "Active Vendors";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.ActiveVendors");
                        break;
                    }
                case "InactiveVendorsGLAccount":
                    {
                        displayTitle = "Inactive Vendors";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.InactiveVendors");
                        break;
                    }
                case "All Vendors":
                    {
                        displayTitle = "All Vendors";
                        myQueryCode = "Vendor Accounts";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.AllVendors");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Payables");
            listArgs.Perspective = "GLAccountPayables";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    // APPayments
    PayablePageComponent.prototype.NewAPPaymentMethod = function () {
        var _this = this;
        var newApPaymentPM = new APPaymentPM_1.APPaymentPM();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, ObjectTableName: 'APPayment' });
            cmpRef.instance.BackCompleted.subscribe(function ($event1) {
                _this.LoadAllScreenData();
            });
        });
    };
    PayablePageComponent.prototype.ViewInvoiceQuery = function (args) {
        var _this = this;
        if (args != null) {
            var displayTitle = "";
            var queryCode = args;
            queryCode = "APPayment";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (args) {
                case "Draft Payments":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.Q.DraftAPPayments");
                        break;
                    }
                case "Open Payments":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.Q.OpenAPPayments");
                        break;
                    }
                case "All Payments":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.Q.AllAPPayments");
                        break;
                    }
                default: {
                    break;
                }
            }
            var backButtonTitle = "Accounting";
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
            var query = window.Queries.filter(function (q) { return q.ObjectTableId == ObjectTable.Id && q.Code == queryCode; })[0];
            if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; }) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; });
                predefinedFilters.forEach(function (filter, key) {
                    var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    _this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
                });
            }
            this.filterAgrs.ObjectTableName = query.ObjectTableName;
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Payables");
            listArgs.DisplayTitle = displayTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    PayablePageComponent.prototype.LoadRecentGLAccounts = function () {
        var _this = this;
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;
        this._GLAccountExtendedListService.GetRecentGLAccounts("3").subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    _this.RecentGLAccountsList = myResult;
                    _this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    };
    PayablePageComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this._GLAccountExtendedListService.GetGLAccountsSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.glAccountSummary.ActiveVendorsCount = myResult.ActiveVendorsCount > 1000 ? "1000+" : myResult.ActiveVendorsCount.toString();
                _this.glAccountSummary.InactiveVendorsCount = myResult.InactiveVendorsCount > 1000 ? "1000+" : myResult.InactiveVendorsCount.toString();
                //this.glAccountSummary.CollectorsCount = myResult.CollectorsCount > 1000 ? "1000+" : myResult.CollectorsCount.toString();
                //this.glAccountSummary.DebitorsCount = myResult.DebitorsCount > 1000 ? "1000+" : myResult.DebitorsCount.toString();
                _this.glAccountSummary.AllVendorsCount = myResult.AllVendorsCount > 1000 ? "1000+" : myResult.AllVendorsCount.toString();
            }
        });
        // APPayments
        var myService = new ModulesService_1.ModulesService();
        myService.GetAccountPayablesSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.APPaymentsDraftsCount = myResult.APPaymentsDraftsCount > 1000 ? "1000+" : myResult.APPaymentsDraftsCount.toString();
                _this.APPaymentsOpenedCount = myResult.APPaymentsOpenedCount > 1000 ? "1000+" : myResult.APPaymentsOpenedCount.toString();
                _this.APInvoicesDraftsCount = myResult.APInvoicesDraftsCount > 1000 ? "1000+" : myResult.APInvoicesDraftsCount.toString();
                _this.APInvoicesUnpaidCount = myResult.APInvoicesUnpaidCount > 1000 ? "1000+" : myResult.APInvoicesUnpaidCount.toString();
            }
        });
    };
    PayablePageComponent.prototype.EditGLAccount = function (entity) {
        var _this = this;
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Payables") });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    // General Invoice 
    PayablePageComponent.prototype.NewGeneralAPInvoice = function () {
        var _this = this;
        var str = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewGeneralInvoice");
        var windowTitle = str;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 900;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = "";
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'APInvoice' });
                    });
                }
            });
        });
        logWindow.Show("./InvoiceModules/APInvoice/Components/NewEntity/NewGeneralAPInvoiceComponent");
    };
    PayablePageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PayablePageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PayablePageComponent);
    return PayablePageComponent;
}());
exports.PayablePageComponent = PayablePageComponent;
//# sourceMappingURL=PayablePageComponent.js.map