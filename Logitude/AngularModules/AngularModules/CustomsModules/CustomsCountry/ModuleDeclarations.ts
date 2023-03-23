import { AddressCurrencyTabComponent } from "./Components/EditTabs/AddressCurrency/AddressCurrencyTabComponent";


export const Components =
  [
    AddressCurrencyTabComponent,
    
  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

    switch (name) {
      case "AddressCurrencyTabComponent": { myResult = AddressCurrencyTabComponent; break; }
     

    }

    return myResult;
  }
}
