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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var AccountingTabComponent = /** @class */ (function () {
    function AccountingTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.TabTitleTextCode = null;
        this.AccountingSystemPM = null;
        this.isFullAccounting = false;
        this.isQuickBooksOnline = false;
        this.isPartnerEntity = false;
        this.isQuickBooksOnlineEntity = false;
        this.IsAccountingActivated = false;
        this.IsExternalCodesFromAPI = false;
        this.IsExternalCodesFromTable = false;
        this.IsSingleCurrencyAccount = false;
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.TabTitleTextCode = this.ObjectTableName + ".TH.Accounting";
        this.InitializeComponent();
    }
    AccountingTabComponent.prototype.ngOnInit = function () {
        this.LoadComponent();
    };
    AccountingTabComponent.prototype.InitializeComponent = function () {
        this.IsAccountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        this.AccountingSystemPM = SessionLocator_1.SessionLocator.AccountingSystemPM;
        if (this.AccountingSystemPM) {
            this.IsExternalCodesFromAPI = this.AccountingSystemPM.IsExternalCodesFromAPI;
            this.IsExternalCodesFromTable = this.AccountingSystemPM.IsExternalCodesFromTable;
            this.IsSingleCurrencyAccount = this.AccountingSystemPM.IsSingleCurrencyAccount;
        }
        switch (this.ObjectTableName) {
            case "Agent":
            case "Airline":
            case "CustomAgent":
            case "Customer":
            case "ShippingAgent":
            case "ShippingLine":
            case "Trucker":
            case "Warehouse":
            case "Vendor":
                {
                    this.isPartnerEntity = true;
                    break;
                }
        }
        switch (this.ObjectTableName) {
            case "Customer":
            case "Currency":
            case "PaymentTerm":
            case "ARPaymentMethod":
            case "APPaymentMethod":
            case "ChargesType":
            case "Vendor":
            case "VatType":
            case "Warehouse":
            case "ShippingAgent":
            case "ShippingLine":
            case "Airline":
            case "CustomAgent":
            case "Agent":
            case "AccountingPaymentMethod":
            case "Trucker":
                {
                    this.isQuickBooksOnlineEntity = true;
                    break;
                }
        }
        if (this.IsAccountingActivated) {
            this.isFullAccounting = true;
        }
        if (this.IsExternalCodesFromAPI && this.isQuickBooksOnlineEntity) {
            this.isQuickBooksOnline = true;
        }
    };
    AccountingTabComponent.prototype.LoadComponent = function () {
        var myComponentPath = null;
        if (this.isFullAccounting
            && this.ObjectTableName != 'ChargesType'
            && this.ObjectTableName != 'Currency'
            && this.ObjectTableName != 'PaymentTerm'
            && this.ObjectTableName != 'VatType'
            && this.ObjectTableName != 'Branch'
            && this.ObjectTableName != 'AccountingPaymentMethod'
            && this.ObjectTableName != 'APPaymentMethod'
            && !this.isQuickBooksOnline) {
            myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Full";
        }
        else if (this.isQuickBooksOnline) {
            myComponentPath = "./Common/Components/AccountingTab/AccountingTab_QuickBooksOnline";
        }
        else if (this.isPartnerEntity) {
            myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Partners";
        }
        else {
            switch (this.ObjectTableName) {
                case "Currency": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Currency";
                    break;
                }
                case "PaymentTerm": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_PaymentTerm";
                    break;
                }
                case "VatType": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_VatType";
                    break;
                }
                case "ChargesType": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_ChargesType";
                    break;
                }
                case "Branch": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Branch";
                    break;
                }
                case "AccountingPaymentMethod": {
                    myComponentPath = "./Invoice/Components/AccountingTab/AccountingTab_AccountingPaymentMethod";
                    break;
                }
                case "APPaymentMethod": {
                    myComponentPath = "./Invoice/Components/AccountingTab/AccountingTab_APPaymentMethod";
                    break;
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myComponentPath)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                .then(function (cmpRef) {
                //cmpRef.instance
            });
        }
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AccountingTabComponent.prototype, "viewContainerRef", void 0);
    AccountingTabComponent = __decorate([
        core_1.Component({
            template: "\n    <div class=\"TabHolder\">\n        <table>\n            <tr class=\"TabTitleRow\">\n                <td>{{TabTitleTextCode | TextCodeTranslationPipe}}</td>\n            </tr>\n\n            <tr>\n                <td>\n                    <div class=\"MediaFill\">\n                        <div #Child></div>\n                    </div>\n                </td>\n            </tr>\n        </table>\n    </div>\n    ",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AccountingTabComponent);
    return AccountingTabComponent;
}());
exports.AccountingTabComponent = AccountingTabComponent;
//select
//ObjectTableTabs.Code, ObjectTables.Name, ObjectTableTabs.ControlPath
//from ObjectTableTabs
//join ObjectTables on ObjectTableTabs.ObjectTableId = ObjectTables.Id
//where ObjectTableTabs.HtmlComponentUrl = './Common/Components/AccountingTab/AccountingTabComponent'
//go
//CRAC	Currency	    Simplog.FreightLib.Views.Tabs.CurrencyAccountingTabControl
//CHAC	ChargesType	    Simplog.FreightLib.Views.ChargesTypes.AccountingTabControl
//VTAC	VatType	        Simplog.FreightLib.Views.VATTypeAccountingControl
//CLAC	Customer	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.CustomerAccountingTabControl
//AGAC	Agent	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.AgentAccountingTabControl
//CUAC	CustomAgent	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.CustomAgentAccountingTabControl
//SAAC	ShippingAgent	Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.ShippingAgentAccountingTabControl
//ALAC	Airline	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.AirlineAccountingTabControl
//SLAC	ShippingLine	Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.ShippingLineAccountingTabControl
//TRAC	Trucker	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.TruckerAccountingTabControl
//VDAC	Vendor	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.VendorAccountingTabControl
//WHAC	Warehouse	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.WarehouseAccountingTabControl
//PTAC	PaymentTerm	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.PaymentTermAccountingTabControl
//# sourceMappingURL=AccountingTabComponent.js.map