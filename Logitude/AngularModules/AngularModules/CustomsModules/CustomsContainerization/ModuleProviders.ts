import { SendContainerizationService } from "./Components/SendContainerization/SendContainerization";




export class ModuleProviders {

  public static GetInstance(name: string) {

    var myResult: any = null;

    switch (name) {

        case "SendContainerizationService": { myResult = new SendContainerizationService(); break; }

 
    }

    return myResult;
  }


}
