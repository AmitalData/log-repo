import {NewOpportunityComponent} from './Components/NewEntity/NewOpportunityComponent';
import {OpportunityOverviewTabComponent} from './Components/EditTabs/OpportunityOverviewTabComponent';
import {QuotesWindowComponent} from './Components/EditTabs/QuotesWindowComponent';
import {OpportunityDocsOutTabComponent} from './Components/EditTabs/OpportunityDocsOutTabComponent';
import {OpportunityDocsInTabComponent} from './Components/EditTabs/OpportunityDocsInTabComponent';
import {OpportunityProductsTabComponent} from './Components/EditTabs/OpportunityProductsTabComponent';
import {EditProductComponent} from './Components/EditTabs/EditProductComponent';
import {ProductHistoryDetailsComponent} from './Components/EditTabs/ProductHistoryDetailsComponent';
import {OpportunityGeneralTabComponent} from './Components/EditTabs/OpportunityGeneralTabComponent';

export const Components =
    [
        NewOpportunityComponent,
        OpportunityGeneralTabComponent,
        OpportunityOverviewTabComponent,
        QuotesWindowComponent,
        OpportunityDocsOutTabComponent,
        OpportunityDocsInTabComponent,
        OpportunityProductsTabComponent,
        EditProductComponent,
        ProductHistoryDetailsComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewOpportunityComponent": { myResult = NewOpportunityComponent; break; } 
            case "OpportunityGeneralTabComponent": { myResult = OpportunityGeneralTabComponent; break; } 
            case "OpportunityOverviewTabComponent": { myResult = OpportunityOverviewTabComponent; break; } 
            case "QuotesWindowComponent": { myResult = QuotesWindowComponent; break; }
            case "OpportunityDocsOutTabComponent": { myResult = OpportunityDocsOutTabComponent; break; }
            case "OpportunityDocsInTabComponent": { myResult = OpportunityDocsInTabComponent; break; } 
            case "OpportunityProductsTabComponent": { myResult = OpportunityProductsTabComponent; break; }
            case "EditProductComponent": { myResult = EditProductComponent; break; }
            case "ProductHistoryDetailsComponent": { myResult = ProductHistoryDetailsComponent; break; }

        }

        return myResult;
    }
}