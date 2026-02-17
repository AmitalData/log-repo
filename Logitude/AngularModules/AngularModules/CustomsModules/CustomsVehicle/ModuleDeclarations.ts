import { VehicleEditComponent } from './Components/EditTabs/VehicleEditComponent';
import { VehicleGeneralComponent } from './Components/EditTabs/VehicleGeneralComponent';
import { VehicleMoreDetailsTabComponent } from './Components/EditTabs/VehicleMoreDetailsTabComponent';
import { VehiclesOwnersAndSafetyTabComponent } from './Components/EditTabs/VehiclesOwnersAndSafetyTabComponent';
import { VehiclesSelectionComponent } from './Components/EditTabs/VehiclesSelectionComponent';
import { SendVehicleComponent } from './Components/SendVehicle/SendVehicleComponent';
import { DeleteVehicleComponent } from './Components/SendVehicle/DeleteVehicleComponent';
import {  CopyRichbitComponent } from './Components/EditTabs/CopyRichbitComponent';

export const Components =
    [
        VehicleEditComponent,
        VehicleGeneralComponent,
        VehicleMoreDetailsTabComponent,
        VehiclesOwnersAndSafetyTabComponent,
        VehiclesSelectionComponent,
        SendVehicleComponent,
        DeleteVehicleComponent,
        CopyRichbitComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "VehicleEditComponent": { myResult = VehicleEditComponent; break; }
            case "VehicleGeneralComponent": { myResult = VehicleGeneralComponent; break; }
            case "VehicleMoreDetailsTabComponent": { myResult = VehicleMoreDetailsTabComponent; break; }
            case "CopyRichbitComponent": { myResult = CopyRichbitComponent; break; }
            case "VehiclesOwnersAndSafetyTabComponent": { myResult = VehiclesOwnersAndSafetyTabComponent; break; }
            case "VehiclesSelectionComponent": { myResult = VehiclesSelectionComponent; break; }
            case "SendVehicleComponent": { myResult = SendVehicleComponent; break; }
            case "DeleteVehicleComponent": { myResult = DeleteVehicleComponent; break; }
        }

        
        return myResult;
    }
}