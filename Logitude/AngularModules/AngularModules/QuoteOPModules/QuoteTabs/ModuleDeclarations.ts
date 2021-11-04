import { QuoteOPAdditionalTabComponent } from './Components/Additional/QuoteOPAdditionalTabComponent';
import { QuoteAuditTabComponent } from './Components/Audit/QuoteAuditTabComponent';
import {ConnectionsTabComponent} from './Components/Connections/ConnectionsTabComponent';
import { QuoteOPDataTabComponent } from './Components/Data/QuoteOPDataTabComponent';
import {QuoteDocsInTabComponent} from './Components/DocsIn/QuoteDocsInTabComponent';
import {QuoteDocsOutTabComponent} from './Components/DocsOut/QuoteDocsOutTabComponent';
import {OrdersTabComponent} from './Components/Orders/OrdersTabComponent';
import {OverviewTabComponent} from './Components/Overview/OverviewTabComponent';
import {AddEditPackageComponent} from './Components/Packages/AddEditPackageComponent';
import {PackagesTabComponent} from './Components/Packages/PackagesTabComponent';
import {AddEditPartnerComponent} from './Components/Partners/AddEditPartnerComponent';
import {PartnersTabComponent} from './Components/Partners/PartnersTabComponent';
import {InlandDomesticRoutingsComponent} from './Components/Routings/InlandDomesticRoutingsComponent';
import {OrdinaryRoutingsComponent} from './Components/Routings/OrdinaryRoutingsComponent';
import {RoutingsTabComponent} from './Components/Routings/RoutingsTabComponent';
import {TariffsComponent} from './Components/Tariffs/TariffsComponent';

export const Components =
    [
        ConnectionsTabComponent,
        QuoteDocsInTabComponent,
        QuoteDocsOutTabComponent,
        OrdersTabComponent,
        OverviewTabComponent,
        AddEditPackageComponent,
        PackagesTabComponent,
        AddEditPartnerComponent,
        PartnersTabComponent,
        InlandDomesticRoutingsComponent,
        OrdinaryRoutingsComponent,
        RoutingsTabComponent,
        TariffsComponent,
        QuoteAuditTabComponent,
        QuoteOPAdditionalTabComponent,
        QuoteOPDataTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ConnectionsTabComponent": { myResult = ConnectionsTabComponent; break; }
            case "QuoteDocsInTabComponent": { myResult = QuoteDocsInTabComponent; break; }
            case "QuoteDocsOutTabComponent": { myResult = QuoteDocsOutTabComponent; break; }
            case "OrdersTabComponent": { myResult = OrdersTabComponent; break; }
            case "OverviewTabComponent": { myResult = OverviewTabComponent; break; }
            case "AddEditPackageComponent": { myResult = AddEditPackageComponent; break; }
            case "PackagesTabComponent": { myResult = PackagesTabComponent; break; }
            case "AddEditPartnerComponent": { myResult = AddEditPartnerComponent; break; }
            case "PartnersTabComponent": { myResult = PartnersTabComponent; break; }
            case "InlandDomesticRoutingsComponent": { myResult = InlandDomesticRoutingsComponent; break; }
            case "OrdinaryRoutingsComponent": { myResult = OrdinaryRoutingsComponent; break; }
            case "RoutingsTabComponent": { myResult = RoutingsTabComponent; break; }
            case "TariffsComponent": { myResult = TariffsComponent; break; }
            case "QuoteAuditTabComponent": { myResult = QuoteAuditTabComponent; break; }  
            case "QuoteOPAdditionalTabComponent": { myResult = QuoteOPAdditionalTabComponent; break; }  
            case "QuoteOPDataTabComponent": { myResult = QuoteOPDataTabComponent; break; }  

        }

        return myResult;
    }
}
