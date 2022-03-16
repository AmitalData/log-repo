import {NewCustomerComponent} from './Components/NewEntity/NewCustomerComponent';
import {CustomerAWBStockTabComponent} from './Components/EditTabs/CustomerAWBStockTabComponent';
import {CustomerCommitmentsTabComponent} from './Components/EditTabs/CustomerCommitmentsTabComponent';
import {EditProductCommitmentComponent} from './Components/EditTabs/EditProductCommitmentComponent';
import {EditProductPotentialComponent} from './Components/EditTabs/EditProductPotentialComponent';
import {CustomerDocsInTabComponent} from './Components/EditTabs/CustomerDocsInTabComponent';
import {CustomerDocsOutTabComponent} from './Components/EditTabs/CustomerDocsOutTabComponent';
import {CustomerGeneralTabComponent} from './Components/EditTabs/CustomerGeneralTabComponent';
import {CustomerOverviewTabComponent} from './Components/EditTabs/CustomerOverviewTabComponent';
import {CustomerProductsTabComponent} from './Components/EditTabs/CustomerProductsTabComponent';
import {CustomerSalesTabComponent} from './Components/EditTabs/CustomerSalesTabComponent';
import {CustomerStatisticsTabComponent} from './Components/EditTabs/CustomerStatisticsTabComponent';
import {CustomerBillingTabComponent} from './Components/EditTabs/CustomerBillingTabComponent';
import {QuickBooksComponent} from './Components/EditTabs/QuickBooksComponent';
import {CustomerOverviewTabDetailsComponent} from './Components/EditTabs/CustomerOverviewTabDetailsComponent';
import {CustomerActivationComponent} from './Components/CustomerActivation/CustomerActivationComponent';
import {ReadyForActivationComponent} from './Components/CustomerActivation/ReadyForActivationComponent';
import {EditCustomerAdditionalServiceComponent} from './Components/EditTabs/EditCustomerAdditionalServiceComponent';
import {CustomerAccountManagerByProductSplitComponent} from './Components/EditTabs/MoreButtons/CustomerAccountManagerByProductSplitComponent';
import {CustomerSalesmanByProductSplitComponent} from './Components/EditTabs/MoreButtons/CustomerSalesmanByProductSplitComponent';
import {CustomerForwarderByProductSplitComponent} from './Components/EditTabs/MoreButtons/CustomerForwarderByProductSplitComponent';
import {CustomerCustomsAgentByProductSplitComponent} from './Components/EditTabs/MoreButtons/CustomerCustomsAgentByProductSplitComponent';
import {CustomerMediatorByProductSplitComponent} from './Components/EditTabs/MoreButtons/CustomerMediatorByProductSplitComponent';
import { CustomerOccasionsTabComponent } from './Components/EditTabs/CustomerOccasionsTabComponent';
import { CustomerProductItemsTabComponent } from './Components/EditTabs/CustomerProductItemsTabComponent';
import { AddEditCustomerProductItemComponent } from './Components/AddEdit/AddEditCustomerProductItemComponent';
import { TariffsTabComponent } from './Components/EditTabs/TariffsTabComponent';

export const Components =
    [
        NewCustomerComponent,
        CustomerAWBStockTabComponent,
        CustomerCommitmentsTabComponent,
        EditProductCommitmentComponent,
        EditProductPotentialComponent,
        CustomerDocsInTabComponent,
        CustomerDocsOutTabComponent,
        CustomerGeneralTabComponent,
        CustomerOverviewTabComponent,
        CustomerProductsTabComponent,
        CustomerSalesTabComponent,
        CustomerStatisticsTabComponent,
        CustomerBillingTabComponent,
        QuickBooksComponent,
        CustomerOverviewTabDetailsComponent,
        CustomerActivationComponent,
        ReadyForActivationComponent,
        EditCustomerAdditionalServiceComponent,
        CustomerAccountManagerByProductSplitComponent,
        CustomerSalesmanByProductSplitComponent,
        CustomerForwarderByProductSplitComponent,
        CustomerCustomsAgentByProductSplitComponent,
        CustomerMediatorByProductSplitComponent,
        CustomerOccasionsTabComponent,
        CustomerProductItemsTabComponent,
        AddEditCustomerProductItemComponent,
        TariffsTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewCustomerComponent": { myResult = NewCustomerComponent; break; }
            case "CustomerAWBStockTabComponent": { myResult = CustomerAWBStockTabComponent; break; }
            case "CustomerCommitmentsTabComponent": { myResult = CustomerCommitmentsTabComponent; break; }
            case "EditProductCommitmentComponent": { myResult = EditProductCommitmentComponent; break; }
            case "EditProductPotentialComponent": { myResult = EditProductPotentialComponent; break; }
            case "CustomerDocsInTabComponent": { myResult = CustomerDocsInTabComponent; break; }
            case "CustomerDocsOutTabComponent": { myResult = CustomerDocsOutTabComponent; break; }
            case "CustomerGeneralTabComponent": { myResult = CustomerGeneralTabComponent; break; }
            case "CustomerOverviewTabComponent": { myResult = CustomerOverviewTabComponent; break; }
            case "CustomerProductsTabComponent": { myResult = CustomerProductsTabComponent; break; }
            case "CustomerSalesTabComponent": { myResult = CustomerSalesTabComponent; break; }
            case "CustomerStatisticsTabComponent": { myResult = CustomerStatisticsTabComponent; break; }
            case "CustomerBillingTabComponent": { myResult = CustomerBillingTabComponent; break; }
            case "QuickBooksComponent": { myResult = QuickBooksComponent; break; }
            case "CustomerOverviewTabDetailsComponent": { myResult = CustomerOverviewTabDetailsComponent; break; }
            case "CustomerActivationComponent": { myResult = CustomerActivationComponent; break; }
            case "ReadyForActivationComponent": { myResult = ReadyForActivationComponent; break; }
            case "EditCustomerAdditionalServiceComponent": { myResult = EditCustomerAdditionalServiceComponent; break }
            case "CustomerAccountManagerByProductSplitComponent": { myResult = CustomerAccountManagerByProductSplitComponent; break }
            case "CustomerSalesmanByProductSplitComponent": { myResult = CustomerSalesmanByProductSplitComponent; break }
            case "CustomerForwarderByProductSplitComponent": { myResult = CustomerForwarderByProductSplitComponent; break }
            case "CustomerCustomsAgentByProductSplitComponent": { myResult = CustomerCustomsAgentByProductSplitComponent; break }
            case "CustomerMediatorByProductSplitComponent": { myResult = CustomerMediatorByProductSplitComponent; break }
            case "CustomerOccasionsTabComponent": { myResult = CustomerOccasionsTabComponent; break }
            case "CustomerProductItemsTabComponent": { myResult = CustomerProductItemsTabComponent; break }
            case "AddEditCustomerProductItemComponent": { myResult = AddEditCustomerProductItemComponent; break }
            case "TariffsTabComponent": { myResult = TariffsTabComponent; break }
        }

        return myResult;
    }
}
