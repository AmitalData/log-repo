
import {PackagesTabComponent} from './Components/Packages/PackagesTabComponent';
import {AddEditAirPackageComponent} from './Components/Packages/AddEditAirPackageComponent';
import {AddEditOceanPackageComponent} from './Components/Packages/AddEditOceanPackageComponent';
import {AddEditInsidePackageComponent} from './Components/Packages/AddEditInsidePackageComponent';
import {AdvancedDangerousGoodsComponent} from './Components/Packages/AdvancedDangerousGoodsComponent';
import {ContainerFollowupActionsComponent} from './Components/Packages/ContainerFU/ContainerFollowupActionsComponent';
import {ContainerFollowupWindowComponent} from './Components/Packages/ContainerFU/ContainerFollowupWindowComponent';
import {ContainerFollowupWindowTemplate} from './Components/Packages/ContainerFU/ContainerFollowupWindowTemplate';
import {ContainerFollowupWizardComponent} from './Components/Packages/ContainerFU/ContainerFollowupWizardComponent';
import {ContainerFollowupWizardTemplate} from './Components/Packages/ContainerFU/ContainerFollowupWizardTemplate';
import { LastStatusComponent } from './Components/Packages/LastStatusComponent';
import { AddEditPackageHarmonizeComponent } from './Components/Packages/AddEditPackageHarmonizeComponent';
import { DownloadPackagesFileComponent } from './Components/Packages/DownloadPackagesFileComponent';

export const Components =
    [
        PackagesTabComponent,
        AddEditAirPackageComponent,
        AddEditOceanPackageComponent,
        AddEditInsidePackageComponent,
        AdvancedDangerousGoodsComponent,
        ContainerFollowupActionsComponent,
        ContainerFollowupWindowComponent,
        ContainerFollowupWindowTemplate,
        ContainerFollowupWizardComponent,
        ContainerFollowupWizardTemplate,
        LastStatusComponent,
        AddEditPackageHarmonizeComponent,
        DownloadPackagesFileComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "PackagesTabComponent": { myResult = PackagesTabComponent; break; }
            case "AddEditAirPackageComponent": { myResult = AddEditAirPackageComponent; break; }
            case "AddEditOceanPackageComponent": { myResult = AddEditOceanPackageComponent; break; }
            case "AddEditInsidePackageComponent": { myResult = AddEditInsidePackageComponent; break; }
            case "AdvancedDangerousGoodsComponent": { myResult = AdvancedDangerousGoodsComponent; break; }
            case "ContainerFollowupActionsComponent": { myResult = ContainerFollowupActionsComponent; break; }
            case "ContainerFollowupWindowComponent": { myResult = ContainerFollowupWindowComponent; break; }
            case "ContainerFollowupWindowTemplate": { myResult = ContainerFollowupWindowTemplate; break; }
            case "ContainerFollowupWizardComponent": { myResult = ContainerFollowupWizardComponent; break; }
            case "ContainerFollowupWizardTemplate": { myResult = ContainerFollowupWizardTemplate; break; }  
            case "LastStatusComponent": { myResult = LastStatusComponent; break; }
            case "AddEditPackageHarmonizeComponent": { myResult = AddEditPackageHarmonizeComponent; break; }
            case "DownloadPackagesFileComponent": { myResult = DownloadPackagesFileComponent; break; }
        }

        return myResult;
    }
}
