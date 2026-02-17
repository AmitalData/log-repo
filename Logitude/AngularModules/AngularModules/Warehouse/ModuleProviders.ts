


import {WarehouseEntryListService} from './Services/StandardLists/WarehouseEntryListService';
import {WarehouseEntryPackagesReleaseListService} from './Services/StandardLists/WarehouseEntryPackagesReleaseListService';
import {WarehouseEntryStatusListService} from './Services/StandardLists/WarehouseEntryStatusListService';
import {WarehouseReleaseListService} from './Services/StandardLists/WarehouseReleaseListService';
import {WarehouseReleaseStatusListService} from './Services/StandardLists/WarehouseReleaseStatusListService';

import {WarehouseEntryPackagesReleasePMService} from './Services/StandardPMs/WarehouseEntryPackagesReleasePMService';

import {WarehouseEntryPMService} from './Services/StandardPMs/WarehouseEntryPMService';
import {WarehouseReleasePMService} from './Services/StandardPMs/WarehouseReleasePMService';

// Menu Buttons 
import {WarehouseReleaseMenuButtonsHandler} from './Components/MenuButtons/WarehouseReleaseMenuButtonsHandler';


export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            case "WarehouseEntryPackagesReleasePMService": { myResult = new WarehouseEntryPackagesReleasePMService(); break; }
            case "WarehouseEntryPMService": { myResult = new WarehouseEntryPMService(); break; }
            case "WarehouseReleasePMService": { myResult = new WarehouseReleasePMService(); break; }

            case "WarehouseEntryListService": { myResult = new WarehouseEntryListService(); break; }
            case "WarehouseEntryPackagesReleaseListService": { myResult = new WarehouseEntryPackagesReleaseListService(); break; }
            case "WarehouseEntryStatusListService": { myResult = new WarehouseEntryStatusListService(); break; }

            case "WarehouseReleaseListService": { myResult = new WarehouseReleaseListService(); break; }
            case "WarehouseReleaseStatusListService": { myResult = new WarehouseReleaseStatusListService(); break; }

            case "WarehouseReleaseMenuButtonsHandler": { myResult = new WarehouseReleaseMenuButtonsHandler(); break; }
        }

        return myResult;
    }
}