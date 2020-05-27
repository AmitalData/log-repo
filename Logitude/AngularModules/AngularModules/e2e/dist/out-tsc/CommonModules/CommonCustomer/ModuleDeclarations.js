"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewCustomerComponent_1 = require("./Components/NewEntity/NewCustomerComponent");
var CustomerAWBStockTabComponent_1 = require("./Components/EditTabs/CustomerAWBStockTabComponent");
var CustomerCommitmentsTabComponent_1 = require("./Components/EditTabs/CustomerCommitmentsTabComponent");
var EditProductCommitmentComponent_1 = require("./Components/EditTabs/EditProductCommitmentComponent");
var EditProductPotentialComponent_1 = require("./Components/EditTabs/EditProductPotentialComponent");
var CustomerDocsInTabComponent_1 = require("./Components/EditTabs/CustomerDocsInTabComponent");
var CustomerDocsOutTabComponent_1 = require("./Components/EditTabs/CustomerDocsOutTabComponent");
var CustomerGeneralTabComponent_1 = require("./Components/EditTabs/CustomerGeneralTabComponent");
var CustomerOverviewTabComponent_1 = require("./Components/EditTabs/CustomerOverviewTabComponent");
var CustomerProductsTabComponent_1 = require("./Components/EditTabs/CustomerProductsTabComponent");
var CustomerSalesTabComponent_1 = require("./Components/EditTabs/CustomerSalesTabComponent");
var CustomerStatisticsTabComponent_1 = require("./Components/EditTabs/CustomerStatisticsTabComponent");
var CustomerBillingTabComponent_1 = require("./Components/EditTabs/CustomerBillingTabComponent");
var QuickBooksComponent_1 = require("./Components/EditTabs/QuickBooksComponent");
var CustomerOverviewTabDetailsComponent_1 = require("./Components/EditTabs/CustomerOverviewTabDetailsComponent");
var CustomerActivationComponent_1 = require("./Components/CustomerActivation/CustomerActivationComponent");
var ReadyForActivationComponent_1 = require("./Components/CustomerActivation/ReadyForActivationComponent");
var EditCustomerAdditionalServiceComponent_1 = require("./Components/EditTabs/EditCustomerAdditionalServiceComponent");
var CustomerAccountManagerByProductSplitComponent_1 = require("./Components/EditTabs/MoreButtons/CustomerAccountManagerByProductSplitComponent");
var CustomerSalesmanByProductSplitComponent_1 = require("./Components/EditTabs/MoreButtons/CustomerSalesmanByProductSplitComponent");
var CustomerForwarderByProductSplitComponent_1 = require("./Components/EditTabs/MoreButtons/CustomerForwarderByProductSplitComponent");
var CustomerCustomsAgentByProductSplitComponent_1 = require("./Components/EditTabs/MoreButtons/CustomerCustomsAgentByProductSplitComponent");
var CustomerMediatorByProductSplitComponent_1 = require("./Components/EditTabs/MoreButtons/CustomerMediatorByProductSplitComponent");
exports.Components = [
    NewCustomerComponent_1.NewCustomerComponent,
    CustomerAWBStockTabComponent_1.CustomerAWBStockTabComponent,
    CustomerCommitmentsTabComponent_1.CustomerCommitmentsTabComponent,
    EditProductCommitmentComponent_1.EditProductCommitmentComponent,
    EditProductPotentialComponent_1.EditProductPotentialComponent,
    CustomerDocsInTabComponent_1.CustomerDocsInTabComponent,
    CustomerDocsOutTabComponent_1.CustomerDocsOutTabComponent,
    CustomerGeneralTabComponent_1.CustomerGeneralTabComponent,
    CustomerOverviewTabComponent_1.CustomerOverviewTabComponent,
    CustomerProductsTabComponent_1.CustomerProductsTabComponent,
    CustomerSalesTabComponent_1.CustomerSalesTabComponent,
    CustomerStatisticsTabComponent_1.CustomerStatisticsTabComponent,
    CustomerBillingTabComponent_1.CustomerBillingTabComponent,
    QuickBooksComponent_1.QuickBooksComponent,
    CustomerOverviewTabDetailsComponent_1.CustomerOverviewTabDetailsComponent,
    CustomerActivationComponent_1.CustomerActivationComponent,
    ReadyForActivationComponent_1.ReadyForActivationComponent,
    EditCustomerAdditionalServiceComponent_1.EditCustomerAdditionalServiceComponent,
    CustomerAccountManagerByProductSplitComponent_1.CustomerAccountManagerByProductSplitComponent,
    CustomerSalesmanByProductSplitComponent_1.CustomerSalesmanByProductSplitComponent,
    CustomerForwarderByProductSplitComponent_1.CustomerForwarderByProductSplitComponent,
    CustomerCustomsAgentByProductSplitComponent_1.CustomerCustomsAgentByProductSplitComponent,
    CustomerMediatorByProductSplitComponent_1.CustomerMediatorByProductSplitComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewCustomerComponent": {
                myResult = NewCustomerComponent_1.NewCustomerComponent;
                break;
            }
            case "CustomerAWBStockTabComponent": {
                myResult = CustomerAWBStockTabComponent_1.CustomerAWBStockTabComponent;
                break;
            }
            case "CustomerCommitmentsTabComponent": {
                myResult = CustomerCommitmentsTabComponent_1.CustomerCommitmentsTabComponent;
                break;
            }
            case "EditProductCommitmentComponent": {
                myResult = EditProductCommitmentComponent_1.EditProductCommitmentComponent;
                break;
            }
            case "EditProductPotentialComponent": {
                myResult = EditProductPotentialComponent_1.EditProductPotentialComponent;
                break;
            }
            case "CustomerDocsInTabComponent": {
                myResult = CustomerDocsInTabComponent_1.CustomerDocsInTabComponent;
                break;
            }
            case "CustomerDocsOutTabComponent": {
                myResult = CustomerDocsOutTabComponent_1.CustomerDocsOutTabComponent;
                break;
            }
            case "CustomerGeneralTabComponent": {
                myResult = CustomerGeneralTabComponent_1.CustomerGeneralTabComponent;
                break;
            }
            case "CustomerOverviewTabComponent": {
                myResult = CustomerOverviewTabComponent_1.CustomerOverviewTabComponent;
                break;
            }
            case "CustomerProductsTabComponent": {
                myResult = CustomerProductsTabComponent_1.CustomerProductsTabComponent;
                break;
            }
            case "CustomerSalesTabComponent": {
                myResult = CustomerSalesTabComponent_1.CustomerSalesTabComponent;
                break;
            }
            case "CustomerStatisticsTabComponent": {
                myResult = CustomerStatisticsTabComponent_1.CustomerStatisticsTabComponent;
                break;
            }
            case "CustomerBillingTabComponent": {
                myResult = CustomerBillingTabComponent_1.CustomerBillingTabComponent;
                break;
            }
            case "QuickBooksComponent": {
                myResult = QuickBooksComponent_1.QuickBooksComponent;
                break;
            }
            case "CustomerOverviewTabDetailsComponent": {
                myResult = CustomerOverviewTabDetailsComponent_1.CustomerOverviewTabDetailsComponent;
                break;
            }
            case "CustomerActivationComponent": {
                myResult = CustomerActivationComponent_1.CustomerActivationComponent;
                break;
            }
            case "ReadyForActivationComponent": {
                myResult = ReadyForActivationComponent_1.ReadyForActivationComponent;
                break;
            }
            case "EditCustomerAdditionalServiceComponent": {
                myResult = EditCustomerAdditionalServiceComponent_1.EditCustomerAdditionalServiceComponent;
                break;
            }
            case "CustomerAccountManagerByProductSplitComponent": {
                myResult = CustomerAccountManagerByProductSplitComponent_1.CustomerAccountManagerByProductSplitComponent;
                break;
            }
            case "CustomerSalesmanByProductSplitComponent": {
                myResult = CustomerSalesmanByProductSplitComponent_1.CustomerSalesmanByProductSplitComponent;
                break;
            }
            case "CustomerForwarderByProductSplitComponent": {
                myResult = CustomerForwarderByProductSplitComponent_1.CustomerForwarderByProductSplitComponent;
                break;
            }
            case "CustomerCustomsAgentByProductSplitComponent": {
                myResult = CustomerCustomsAgentByProductSplitComponent_1.CustomerCustomsAgentByProductSplitComponent;
                break;
            }
            case "CustomerMediatorByProductSplitComponent": {
                myResult = CustomerMediatorByProductSplitComponent_1.CustomerMediatorByProductSplitComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map