
import { TariffTypeListService } from './Services/StandardLists/TariffTypeListService';
import { TariffVersionListService } from './Services/StandardLists/TariffVersionListService';
import { TariffListService } from './Services/StandardLists/TariffListService';
import { TariffPMService } from './Services/StandardPMs/TariffPMService';
import { TariffVersionPMService } from './Services/StandardPMs/TariffVersionPMService';
import { TariffDomainService } from './Services/TariffDomainService';
import { TariffMenuButtonsHandler } from './Components/MenuButtons/TariffMenuButtonsHandler';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            // List
            case "TariffListService": { myResult = new TariffListService(); break; }
            case "TariffVersionListService": { myResult = new TariffVersionListService(); break; }
            case "TariffTypeListService": { myResult = new TariffTypeListService(); break; }
            
            // PM
            case "TariffVersionPMService": { myResult = new TariffVersionPMService(); break; }
            case "TariffPMService": { myResult = new TariffPMService(); break; }

           // DomainService
            case "TariffDomainService": { myResult = new TariffDomainService(); break; }


            // MenuButtons
            case "TariffMenuButtonsHandler": { myResult = new TariffMenuButtonsHandler(); break; }   

                
     
        }

        return myResult;
    }
}
