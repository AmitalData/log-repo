

import {NewWarehouseEntryComponent} from './Components/NewWarehouseEntryComponent';
import {NewWarehouseReleaseComponent} from './Components/NewWarehouseReleaseComponent';
import {AddEditWarehouseEntryPackagesAndContainers} from './Components/AddEditWarehouseEntryPackagesAndContainers';

import {WarehouseReleaseChoosePackagesComponent} from './Components/WarehouseReleaseChoosePackagesComponent';

import {EditWarehouseEntryComponent} from './Components/EditWarehouseEntryComponent';
import {EditWarehouseReleaseComponent} from './Components/EditWarehouseReleaseComponent';

import {WarehouseEntryShortTitleComponent} from './Components/ShortTitles/WarehouseEntryShortTitleComponent';
import {WarehouseReleaseShortTitleComponent} from './Components/ShortTitles/WarehouseReleaseShortTitleComponent';

import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {CopyFromReleasesPackagesComponent} from './Components/CopyFromReleasesPackagesComponent';
import {WarehouseDocsOutTabComponent} from './Components/EditTabs/DocsOut/WarehouseDocsOutTabComponent';
import {WarehouseDocsInTabComponent} from './Components/EditTabs/DocsIn/WarehouseDocsInTabComponent';
import {WarehouseWorkspaceComponent} from './Components/Workspaces/WarehouseWorkspaceComponent';
import { NewFullWarehouseEntryComponent } from './Components/NewEntity/NewFullWarehouseEntryComponent';
import { NewFullWarehouseReleaseComponent } from './Components/NewEntity/NewFullWarehouseReleaseComponent';


import {WarehouseEntryPackagesDetailsComponent} from './Components/WarehouseEntryPackagesDetailsComponent';
import {WarehouseEntryFiltersMenuComponent} from './Components/FiltersMenu/WarehouseEntryFiltersMenuComponent';
import {WarehouseEntryPartnersTabComponent} from './Components/EditTabs/PartnersTab/WarehouseEntryPartnersTabComponent';
import {AddEditPartnerComponent} from './Components/EditTabs/PartnersTab/AddEditPartnerComponent';
import {WarehouseEntryRoutingsTabComponent} from './Components/EditTabs/RoutingsTab/WarehouseEntryRoutingsTabComponent'; 
import {WarehouseConnectionsTabComponent} from './Components/EditTabs/ConnectionsTab/WarehouseConnectionsTabComponent'; 
import {WarehouseEntryPackagesTabComponent} from './Components/EditTabs/PackagesTab/WarehouseEntryPackagesTabComponent'; 
import {WarehouseReleaseFiltersMenuComponent} from './Components/FiltersMenu/WarehouseReleaseFiltersMenuComponent';

import {WarehouseReleasePackagesDetailsComponent} from './Components/WarehouseReleasePackagesDetailsComponent';


import {WarehouseReleaseRoutingsTabComponent} from './Components/EditTabs/RoutingsTab/WarehouseReleaseRoutingsTabComponent'; 
import {ChoosePackagesFromWarehousePackageReleasesComponent} from './Components/ChoosePackagesFromWarehousePackageReleasesComponent';



export const Components =
    [
        NewWarehouseEntryComponent,
        NewWarehouseReleaseComponent,
        AddEditWarehouseEntryPackagesAndContainers,
        WarehouseReleaseChoosePackagesComponent,
        CopyFromReleasesPackagesComponent,
        EditWarehouseEntryComponent,
        EditWarehouseReleaseComponent,
        FieldTemplateComponent,
        WarehouseEntryShortTitleComponent,
        WarehouseReleaseShortTitleComponent,
        WarehouseDocsOutTabComponent,
        WarehouseDocsInTabComponent,
        WarehouseWorkspaceComponent,
        NewFullWarehouseEntryComponent,
        WarehouseEntryPackagesDetailsComponent,
        WarehouseEntryFiltersMenuComponent,
        WarehouseEntryPartnersTabComponent,
        AddEditPartnerComponent,
        WarehouseEntryRoutingsTabComponent,
        WarehouseEntryPackagesTabComponent,
        WarehouseReleaseFiltersMenuComponent,
        WarehouseConnectionsTabComponent,
        WarehouseReleasePackagesDetailsComponent,
        NewFullWarehouseReleaseComponent,
        WarehouseReleaseRoutingsTabComponent,
        ChoosePackagesFromWarehousePackageReleasesComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {

            case "NewWarehouseEntryComponent": { myResult = NewWarehouseEntryComponent; break; }
            case "NewWarehouseReleaseComponent": { myResult = NewWarehouseReleaseComponent; break; }
            case "AddEditWarehouseEntryPackagesAndContainers": { myResult = AddEditWarehouseEntryPackagesAndContainers; break; }
            case "WarehouseReleaseChoosePackagesComponent": { myResult = WarehouseReleaseChoosePackagesComponent; break; }
            case "EditWarehouseEntryComponent": { myResult = EditWarehouseEntryComponent; break; }
            case "EditWarehouseReleaseComponent": { myResult = EditWarehouseReleaseComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "WarehouseEntryShortTitleComponent": { myResult = WarehouseEntryShortTitleComponent; break; }
            case "WarehouseReleaseShortTitleComponent": { myResult = WarehouseReleaseShortTitleComponent; break; } 
            case "CopyFromReleasesPackagesComponent": { myResult = CopyFromReleasesPackagesComponent; break; } 
            case "WarehouseDocsOutTabComponent": { myResult = WarehouseDocsOutTabComponent; break; } 
            case "WarehouseDocsInTabComponent": { myResult = WarehouseDocsInTabComponent; break; } 
            case "WarehouseWorkspaceComponent": { myResult = WarehouseWorkspaceComponent; break; } 
            case "NewFullWarehouseEntryComponent": { myResult = NewFullWarehouseEntryComponent; break; } 
            case "WarehouseEntryPackagesDetailsComponent": { myResult = WarehouseEntryPackagesDetailsComponent; break; } 
            case "WarehouseEntryFiltersMenuComponent": { myResult = WarehouseEntryFiltersMenuComponent; break; } 
            case "WarehouseEntryPartnersTabComponent": { myResult = WarehouseEntryPartnersTabComponent; break; } 
            case "AddEditPartnerComponent": { myResult = AddEditPartnerComponent; break; } 
            case "WarehouseEntryRoutingsTabComponent": { myResult = WarehouseEntryRoutingsTabComponent; break; } 
            case "WarehouseEntryPackagesTabComponent": { myResult = WarehouseEntryPackagesTabComponent; break; } 
            case "WarehouseReleaseFiltersMenuComponent": { myResult = WarehouseReleaseFiltersMenuComponent; break; }
            case "WarehouseConnectionsTabComponent": { myResult = WarehouseConnectionsTabComponent; break; }
            case "WarehouseReleasePackagesDetailsComponent": { myResult = WarehouseReleasePackagesDetailsComponent; break; } 
            case "NewFullWarehouseReleaseComponent": { myResult = NewFullWarehouseReleaseComponent; break; }
            case "WarehouseReleaseRoutingsTabComponent": { myResult = WarehouseReleaseRoutingsTabComponent; break; }
            case "ChoosePackagesFromWarehousePackageReleasesComponent": { myResult = ChoosePackagesFromWarehousePackageReleasesComponent; break; }
   
        }


        return myResult;
    }
}
