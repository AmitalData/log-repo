


import { SendDeclarationService } from './Components/SendDeclaration/SendDeclarationComponent';
import { SendManifestService } from './Components/SendDeclaration/SendManifestComponent';




export class ModuleProviders {

  public static GetInstance(name: string) {

    var myResult: any = null;

    switch (name) {

      case "SendDeclarationService": { myResult = new SendDeclarationService(); break; }
      case "SendManifestService": { myResult = new SendManifestService(); break; }

 
    }

    return myResult;
  }


}
