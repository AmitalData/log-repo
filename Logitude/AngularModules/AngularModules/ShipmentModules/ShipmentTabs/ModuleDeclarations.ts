import {CustomsTabComponent} from './Components/Customs/CustomsTabComponent';
import {ExportFileComponent} from './Components/Customs/ExportFileComponent';
import {OverviewTabComponent} from './Components/Overview/OverviewTabComponent';
import {OrdersTabComponent} from './Components/Orders/OrdersTabComponent';
import {AddEditOrderPackageComponent} from './Components/Orders/AddEditOrderPackageComponent';
import {PartnersTabComponent} from './Components/Partners/PartnersTabComponent';
import {AddEditPartnerComponent} from './Components/Partners/AddEditPartnerComponent';
import {ShipmentsTabComponent} from './Components/Shipments/ShipmentsTabComponent';
import {MasterTabComponent} from './Components/Master/MasterTabComponent';
import {CustomsFileTabComponent} from './Components/CustomsFile/CustomsFileTabComponent';
import {ConnectionsTabComponent} from './Components/Connections/ConnectionsTabComponent';
import {AddEditShipmentAssemblyComponent} from './Components/Connections/AddEditShipmentAssemblyComponent';
import {ShipmentDocsOutTabComponent} from './Components/DocsOut/ShipmentDocsOutTabComponent';
import {ShipmentDocsInTabComponent} from './Components/DocsIn/ShipmentDocsInTabComponent';
import {ShipmentAuditTabComponent} from './Components/Audit/ShipmentAuditTabComponent';
import {ReceivablesTabComponent} from './Components/Receivables/ReceivablesTabComponent';
import {AddEditReceivableComponent} from './Components/Receivables/AddEditReceivableComponent';
import {PayablesTabComponent} from './Components/Payables/PayablesTabComponent';
import {AddEditPayableComponent} from './Components/Payables/AddEditPayableComponent';
import {TariffsComponent} from './Components/Windows/Tariffs/TariffsComponent';
import {ProfitComponent} from './Components/Windows/Profit/ProfitComponent';
import {QuotesComponent} from './Components/Windows/Quotes/QuotesComponent';
import {PayablesComponent} from './Components/Windows/Payables/PayablesComponent';
import {GroupageComponent} from './Components/Windows/Groupage/GroupageComponent';
import {GroupageContainerComponent} from './Components/Windows/Groupage/GroupageContainerComponent';
import { HarmonizesComponent } from './Components/Windows/Harmonizes/HarmonizesComponent';
import { ProductItemsTabComponent } from './Components/ProductItems/ProductItemsTabComponent';
import { EditCustomerProductItemComponent } from './Components/ProductItems/EditCustomerProductItemComponent';
import { ShipmentPackagesTabComponent } from './Components/ShipmentPackages/ShipmentPackagesTabComponent';
import { ShipmentDataTabComponent } from './Components/ShipmentData/ShipmentDataTabComponent';
import { ShipmentReferenceDetailsComponent } from './Components/ShipmentData/ShipmentReferenceDetails/ShipmentReferenceDetailsComponent';
import { FreightForwarderReferenceDetailsComponent } from './Components/ShipmentData/FreightForwarderReferenceDetails/FreightForwarderReferenceDetailsComponent';
import { InlandTransportTabComponent } from './Components/InlandTransport/InlandTransportTabComponent';

export const Components =
    [
        ShipmentsTabComponent,
        MasterTabComponent,
        CustomsFileTabComponent,
        ConnectionsTabComponent,
        AddEditShipmentAssemblyComponent,
        ShipmentDocsOutTabComponent,
        ShipmentDocsInTabComponent,
        ShipmentAuditTabComponent,
        OverviewTabComponent,
        CustomsTabComponent,
        ShipmentPackagesTabComponent,
        ShipmentDataTabComponent,
        InlandTransportTabComponent,
        ShipmentReferenceDetailsComponent,
        ExportFileComponent,
        OrdersTabComponent,
        AddEditOrderPackageComponent,
        PartnersTabComponent,
        AddEditPartnerComponent,
        ReceivablesTabComponent,
        AddEditReceivableComponent,
        PayablesTabComponent,
        AddEditPayableComponent,
        TariffsComponent,
        ProfitComponent,
        QuotesComponent,
        PayablesComponent,
        GroupageComponent,
        GroupageContainerComponent,
        HarmonizesComponent,
        ProductItemsTabComponent,
        EditCustomerProductItemComponent,
        FreightForwarderReferenceDetailsComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "OverviewTabComponent": { myResult = OverviewTabComponent; break; }
            case "CustomsTabComponent": { myResult = CustomsTabComponent; break; }
            case "ExportFileComponent": { myResult = ExportFileComponent; break; }
            case "OrdersTabComponent": { myResult = OrdersTabComponent; break; }
            case "AddEditOrderPackageComponent": { myResult = AddEditOrderPackageComponent; break; }
            case "PartnersTabComponent": { myResult = PartnersTabComponent; break; }
            case "AddEditPartnerComponent": { myResult = AddEditPartnerComponent; break; }
            case "ShipmentsTabComponent": { myResult = ShipmentsTabComponent; break; }
            case "MasterTabComponent": { myResult = MasterTabComponent; break; }
            case "CustomsFileTabComponent": { myResult = CustomsFileTabComponent; break; }
            case "ConnectionsTabComponent": { myResult = ConnectionsTabComponent; break; }
            case "AddEditShipmentAssemblyComponent": { myResult = AddEditShipmentAssemblyComponent; break; }
            case "ShipmentDocsOutTabComponent": { myResult = ShipmentDocsOutTabComponent; break; }
            case "ShipmentAuditTabComponent": { myResult = ShipmentAuditTabComponent; break; }
            case "ShipmentDocsInTabComponent": { myResult = ShipmentDocsInTabComponent; break; }
            case "ReceivablesTabComponent": { myResult = ReceivablesTabComponent; break; }
            case "AddEditReceivableComponent": { myResult = AddEditReceivableComponent; break; }
            case "PayablesTabComponent": { myResult = PayablesTabComponent; break; }
            case "AddEditPayableComponent": { myResult = AddEditPayableComponent; break; }
            case "TariffsComponent": { myResult = TariffsComponent; break; }
            case "ProfitComponent": { myResult = ProfitComponent; break; }
            case "QuotesComponent": { myResult = QuotesComponent; break; }
            case "PayablesComponent": { myResult = PayablesComponent; break; }
            case "GroupageComponent": { myResult = GroupageComponent; break; }
            case "GroupageContainerComponent": { myResult = GroupageContainerComponent; break; }
            case "HarmonizesComponent": { myResult = HarmonizesComponent; break; }
            case "ProductItemsTabComponent": { myResult = ProductItemsTabComponent; break; }
            case "EditCustomerProductItemComponent": { myResult = EditCustomerProductItemComponent; break; }
            case "ShipmentPackagesTabComponent" : { myResult = ShipmentPackagesTabComponent; break; }
            case "ShipmentDataTabComponent" : { myResult = ShipmentDataTabComponent; break; }
            case "InlandTransportTabComponent" : { myResult = InlandTransportTabComponent; break; }
            case "ShipmentReferenceDetailsComponent" : { myResult = ShipmentReferenceDetailsComponent; break; }
            case "FreightForwarderReferenceDetailsComponent" : { myResult = FreightForwarderReferenceDetailsComponent; break; }

        }

        return myResult;
    }
}
