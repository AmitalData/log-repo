import { CountryCurrencyTabComponent } from "./Components/EditTabs/CountryCurrency/CountryCurrencyTabComponent";


export const Components =
  [
    CountryCurrencyTabComponent,
    
  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

    switch (name) {
      case "CountryCurrencyTabComponent": { myResult = CountryCurrencyTabComponent; break; }
     

    }

    return myResult;
  }
}
