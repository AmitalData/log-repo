
import { VendorGeneralTabComponent } from './Components/EditTabs/General/VendorGeneralTabComponent';
import { VendorEditComponent } from './Components/EditTabs/VendorEditComponent';
import { AddVendorCommunicationComponent } from './Components/EditTabs/General/AddVendorCommunicationComponent';
import { NewVendorComponent } from './Components/NewEntity/NewVendorComponent';
import { VendorCurrencyTabComponent } from './Components/EditTabs/VendorCurrency/VendorCurrencyTabComponent';

export const Components =
  [
    VendorGeneralTabComponent,
    VendorEditComponent,
    AddVendorCommunicationComponent,



    NewVendorComponent,
    VendorCurrencyTabComponent
  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

    switch (name) {
      case "VendorGeneralTabComponent": { myResult = VendorGeneralTabComponent; break; }
      case "VendorEditComponent": { myResult = VendorEditComponent; break; }
      case "AddVendorCommunicationComponent": { myResult = AddVendorCommunicationComponent; break; }

      case "NewVendorComponent": { myResult = NewVendorComponent; break; }

      case "VendorCurrencyTabComponent": { myResult = VendorCurrencyTabComponent; break; }


    }

    return myResult;
  }
}
