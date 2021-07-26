import { NewClientComponent } from './Components/NewClient/NewClientComponent';
import {ClientEditComponent} from './Components/EditTabs/ClientEditComponent';
import {ClientGeneralTabComponent} from  './Components/EditTabs/General/ClientGeneralTabComponent';
import {ClientAddressesTabComponent} from  './Components/EditTabs/Addresses/ClientAddressesTabComponent';
import {AddEditAddressComponent} from './Components/EditTabs/Addresses/AddEditAddressComponent';
import {ClientDrivingLicenseTabComponent} from './Components/EditTabs/License/ClientDrivingLicenseTabComponent';
import { ClientPoaTabComponent } from './Components/EditTabs/ClientPoa/ClientPoaTabComponent';



export const Components =
    [
        NewClientComponent,
        ClientEditComponent,
        ClientGeneralTabComponent,
        ClientAddressesTabComponent,
        AddEditAddressComponent,
        ClientDrivingLicenseTabComponent,
        ClientPoaTabComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewClientComponent": { myResult = NewClientComponent; break; }
            case "ClientEditComponent": { myResult = ClientEditComponent; break; }
            case "ClientGeneralTabComponent": { myResult = ClientGeneralTabComponent; break; }
            case "ClientAddressesTabComponent": { myResult = ClientAddressesTabComponent; break; }
            case "AddEditAddressComponent": { myResult = AddEditAddressComponent; break; }
            case "ClientDrivingLicenseTabComponent": { myResult = ClientDrivingLicenseTabComponent; break; } 
            case "ClientPoaTabComponent": { myResult = ClientPoaTabComponent; break; }
        }

        return myResult;
    }
}
