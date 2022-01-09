import {RoutingsTabComponent} from './Components/Routings/RoutingsTabComponent';
import {AddEditPreCarriageComponent} from './Components/Routings/AddEditPreCarriageComponent';
import {AddEditOnCarriageComponent} from './Components/Routings/AddEditOnCarriageComponent';
import {AddEditHouseRoutingComponent} from './Components/Routings/AddEditHouseRoutingComponent';
import {AddEditMainCarriageComponent} from './Components/Routings/AddEditMainCarriageComponent';
import {AddEditPickupComponent} from './Components/Routings/AddEditPickupComponent';
import {AddEditWarehouseLegComponent} from './Components/Routings/AddEditWarehouseLegComponent';
import {PickupMainTabComponent} from './Components/Routings/PickupTabs/PickupMainTabComponent';
import {PickupPackagesTabComponent} from './Components/Routings/PickupTabs/PickupPackagesTabComponent';
import {PickupDocsOutTabComponent} from './Components/Routings/PickupTabs/PickupDocsOutTabComponent';
import {PickupDocsInTabComponent} from './Components/Routings/PickupTabs/PickupDocsInTabComponent';
import {PickupPackagesAddEditComponent} from './Components/Routings/PickupTabs/PickupPackagesAddEditComponent';
import {PickupPackagesChooseComponent} from './Components/Routings/PickupTabs/PickupPackagesChooseComponent';
import {AddEditDeliveryComponent} from './Components/Routings/AddEditDeliveryComponent';
import {DeliveryMainTabComponent} from './Components/Routings/DeliveryTabs/DeliveryMainTabComponent';
import {DeliveryPackagesTabComponent} from './Components/Routings/DeliveryTabs/DeliveryPackagesTabComponent';
import {DeliveryDocsOutTabComponent} from './Components/Routings/DeliveryTabs/DeliveryDocsOutTabComponent';
import {DeliveryDocsInTabComponent} from './Components/Routings/DeliveryTabs/DeliveryDocsInTabComponent';
import {DeliveryPackagesAddEditComponent} from './Components/Routings/DeliveryTabs/DeliveryPackagesAddEditComponent';
import {DeliveryPackagesChooseComponent} from './Components/Routings/DeliveryTabs/DeliveryPackagesChooseComponent';
import {DeliveryPackagesConnectComponent} from './Components/Routings/DeliveryTabs/DeliveryPackagesConnectComponent';
import {OnCarriageDateComponent} from './Components/Routings/OnCarriageDateComponent';
import { AddEditPackageHarmonizeComponent } from './Components/Routings/AddEditPackageHarmonizeComponent';
import { WarehouseStoragePricingComponent } from './Components/Routings/WarehouseStoragePricingComponent';
import { ChooseStandaloneShipmentComponent } from './Components/Routings/ChooseStandaloneShipmentComponent';
import { SelectStandalonePackagesComponent } from './Components/Routings/SelectStandalonePackagesComponent';
import { StandAlonePickupDeilveryActionsComponent } from './Components/Routings/StandAlonePickupDeilveryActionsComponent';
import { ChooseVesselComponent } from './Components/Routings/ChooseVesselComponent';

export const Components =
    [
        RoutingsTabComponent,
        AddEditPreCarriageComponent,
        AddEditOnCarriageComponent,
        AddEditHouseRoutingComponent,
        AddEditMainCarriageComponent,
        AddEditPickupComponent,
        AddEditWarehouseLegComponent,
        PickupMainTabComponent,
        PickupPackagesTabComponent,
        PickupDocsOutTabComponent,
        PickupDocsInTabComponent,
        PickupPackagesAddEditComponent,
        PickupPackagesChooseComponent,
        AddEditDeliveryComponent,
        DeliveryMainTabComponent,
        DeliveryPackagesTabComponent,
        DeliveryDocsOutTabComponent,
        DeliveryDocsInTabComponent,
        DeliveryPackagesAddEditComponent,
        DeliveryPackagesChooseComponent,       
        DeliveryPackagesConnectComponent,
        AddEditPackageHarmonizeComponent,
        WarehouseStoragePricingComponent,
        ChooseStandaloneShipmentComponent,
        SelectStandalonePackagesComponent,
        StandAlonePickupDeilveryActionsComponent,
        ChooseVesselComponent,
    ];

export const ControlsComponents =
    [
        OnCarriageDateComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {

            case "RoutingsTabComponent": { myResult = RoutingsTabComponent; break; }
            case "AddEditPreCarriageComponent": { myResult = AddEditPreCarriageComponent; break; }
            case "AddEditOnCarriageComponent": { myResult = AddEditOnCarriageComponent; break; }
            case "AddEditHouseRoutingComponent": { myResult = AddEditHouseRoutingComponent; break; }
            case "AddEditMainCarriageComponent": { myResult = AddEditMainCarriageComponent; break; }
            case "AddEditPickupComponent": { myResult = AddEditPickupComponent; break; }
            case "AddEditWarehouseLegComponent": { myResult = AddEditWarehouseLegComponent; break; }
            case "PickupMainTabComponent": { myResult = PickupMainTabComponent; break; }
            case "PickupPackagesTabComponent": { myResult = PickupPackagesTabComponent; break; }
            case "PickupDocsOutTabComponent": { myResult = PickupDocsOutTabComponent; break; }
            case "PickupDocsInTabComponent": { myResult = PickupDocsInTabComponent; break; }
            case "PickupPackagesAddEditComponent": { myResult = PickupPackagesAddEditComponent; break; }
            case "PickupPackagesChooseComponent": { myResult = PickupPackagesChooseComponent; break; }
            case "AddEditDeliveryComponent": { myResult = AddEditDeliveryComponent; break; }
            case "DeliveryMainTabComponent": { myResult = DeliveryMainTabComponent; break; }
            case "DeliveryPackagesTabComponent": { myResult = DeliveryPackagesTabComponent; break; }
            case "DeliveryDocsOutTabComponent": { myResult = DeliveryDocsOutTabComponent; break; }
            case "DeliveryDocsInTabComponent": { myResult = DeliveryDocsInTabComponent; break; }
            case "DeliveryPackagesAddEditComponent": { myResult = DeliveryPackagesAddEditComponent; break; }
            case "DeliveryPackagesChooseComponent": { myResult = DeliveryPackagesChooseComponent; break; }
            case "DeliveryPackagesConnectComponent": { myResult = DeliveryPackagesConnectComponent; break; }
            case "AddEditPackageHarmonizeComponent": { myResult = AddEditPackageHarmonizeComponent; break; }
            case "WarehouseStoragePricingComponent": { myResult = WarehouseStoragePricingComponent; break; }
            case "ChooseStandaloneShipmentComponent": { myResult = ChooseStandaloneShipmentComponent; break; }
            case "SelectStandalonePackagesComponent": { myResult = SelectStandalonePackagesComponent; break; }
            case "StandAlonePickupDeilveryActionsComponent": { myResult = StandAlonePickupDeilveryActionsComponent; break; }
            case "ChooseVesselComponent": { myResult = ChooseVesselComponent; break; }
        }

        return myResult;
    }
}
