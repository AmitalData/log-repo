import { TariffModuleWorkspaceComponent } from './Components/Workspaces/TariffModuleWorkspaceComponent';
import { TariffSettingComponent } from './Components/Workspaces/TariffSettingComponent';
import { NewAirFreightCostComponent } from './Components/NewEntity/NewAirFreightCostComponent';
import { FieldTemplateComponent } from './Components/Templates/FieldTemplateComponent';
import { TariffSearchAirFreightPricesComponent } from './Components/Workspaces/TariffSearchAirFreightPricesComponent';

// Tabs
import { TariffDetailsTabComponent } from './Components/EditTabs/Tariff/TariffDetailsTabComponent';
import { VersionTabComponent } from './Components/EditTabs/Tariff/VersionTabComponent';
import { TariffGeneralTabComponent } from './Components/EditTabs/Tariff/TariffGeneralTabComponent';
import { VersionHistoryTabComponent } from './Components/EditTabs/Tariff/VersionHistoryTabComponent';
import { AddEditTariffLineComponent } from './Components/EditTabs/Tariff/AddEditTariffLineComponent';
import { TariffTabsContentComponent } from './Components/EditTabs/Tariff/TariffTabsContentComponent';
import { SurchargeVersionTabComponent } from './Components/EditTabs/Tariff/SurchargeVersionTabComponent';
import { TariffDatesValidationComponent } from './Components/EditTabs/Tariff/TariffDatesValidationComponent';

export const Components =
    [
        TariffModuleWorkspaceComponent,
        TariffSettingComponent,
        NewAirFreightCostComponent,
        FieldTemplateComponent,
        TariffDetailsTabComponent,
        VersionTabComponent,
        VersionHistoryTabComponent,
        TariffGeneralTabComponent,
        AddEditTariffLineComponent,
        TariffTabsContentComponent,
        SurchargeVersionTabComponent,
        TariffSearchAirFreightPricesComponent,
        TariffDatesValidationComponent,
    ];

export const ControlsComponents =
    [
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "TariffModuleWorkspaceComponent": { myResult = TariffModuleWorkspaceComponent; break; }
            case "TariffSettingComponent": { myResult = TariffSettingComponent; break; }                
            case "NewAirFreightCostComponent": { myResult = NewAirFreightCostComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "TariffDetailsTabComponent": { myResult = TariffDetailsTabComponent; break; }
            case "VersionTabComponent": { myResult = VersionTabComponent; break; }                
            case "VersionHistoryTabComponent": { myResult = VersionHistoryTabComponent; break; }                
            case "TariffGeneralTabComponent": { myResult = TariffGeneralTabComponent; break; }
            case "AddEditTariffLineComponent": { myResult = AddEditTariffLineComponent; break; }
            case "TariffTabsContentComponent": { myResult = TariffTabsContentComponent; break; }
            case "SurchargeVersionTabComponent": { myResult = SurchargeVersionTabComponent; break; }
            case "TariffSearchAirFreightPricesComponent": { myResult = TariffSearchAirFreightPricesComponent; break; }
            case "TariffDatesValidationComponent": { myResult = TariffDatesValidationComponent; break; }
        }

        return myResult;
    }
}
