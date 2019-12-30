import { CostWorkspaceComponent } from './Components/Workspaces/CostWorkspaceComponent';
import { TariffSettingComponent } from './Components/Workspaces/TariffSettingComponent';
import { NewAirFreightCostComponent } from './Components/NewEntity/NewAirFreightCostComponent';
import { FieldTemplateComponent } from './Components/Templates/FieldTemplateComponent';
import { TariffSearchAirFreightPricesComponent } from './Components/Workspaces/TariffSearchAirFreightPricesComponent';
import { TariffPriceStepsComponent } from './Components/NewEntity/TariffPriceStepsComponent';
import { WizardDimensionsComponent } from './Components/Workspaces/WizardDimensionsComponent';
import { TariffHelperComponent } from './Components/Helpers/TariffHelperComponent';
import { TariffWorkspaceComponent } from './Components/Workspaces/TariffWorkspaceComponent';
import { SettingsWorkspaceComponent } from './Components/Workspaces/SettingsWorkspaceComponent';
// Tabs
import { TariffDetailsTabComponent } from './Components/EditTabs/Tariff/TariffDetailsTabComponent';
import { VersionTabComponent } from './Components/EditTabs/Tariff/VersionTabComponent';
import { TariffGeneralTabComponent } from './Components/EditTabs/Tariff/TariffGeneralTabComponent';
import { VersionHistoryTabComponent } from './Components/EditTabs/Tariff/VersionHistoryTabComponent';
import { AddEditTariffLineComponent } from './Components/EditTabs/Tariff/AddEditTariffLineComponent';
import { TariffTabsContentComponent } from './Components/EditTabs/Tariff/TariffTabsContentComponent';
import { SurchargeVersionTabComponent } from './Components/EditTabs/Tariff/SurchargeVersionTabComponent';
import { TariffDatesValidationComponent } from './Components/EditTabs/Tariff/TariffDatesValidationComponent';
import { UpdateSurchargesComponent } from './Components/EditTabs/Tariff/UpdateSurchargesComponent';
import { ChoosePortComponent } from './Components/EditTabs/Tariff/ChoosePortComponent';
import { AddEditAllInChargesComponent } from './Components/EditTabs/Tariff/AddEditAllInChargesComponent';
import { OceanFCLVersionTabComponent } from './Components/EditTabs/Tariff/OceanFCLVersionTabComponent';

export const Components =
    [
        CostWorkspaceComponent,
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
        UpdateSurchargesComponent,
        ChoosePortComponent,
        AddEditAllInChargesComponent,
        TariffPriceStepsComponent,
        WizardDimensionsComponent,
        TariffHelperComponent,
        OceanFCLVersionTabComponent,
        TariffWorkspaceComponent,
        SettingsWorkspaceComponent,
    ];

export const ControlsComponents =
    [
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CostWorkspaceComponent": { myResult = CostWorkspaceComponent; break; }
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
            case "UpdateSurchargesComponent": { myResult = UpdateSurchargesComponent; break; }
            case "ChoosePortComponent": { myResult = ChoosePortComponent; break; }
            case "AddEditAllInChargesComponent": { myResult = AddEditAllInChargesComponent; break; }
            case "TariffPriceStepsComponent": { myResult = TariffPriceStepsComponent; break; }
            case "WizardDimensionsComponent": { myResult = WizardDimensionsComponent; break; }
            case "TariffHelperComponent": { myResult = TariffHelperComponent; break; }
            case "OceanFCLVersionTabComponent": { myResult = OceanFCLVersionTabComponent; break; }
            case "TariffWorkspaceComponent": { myResult = TariffWorkspaceComponent; break; }
            case "SettingsWorkspaceComponent": { myResult = SettingsWorkspaceComponent; break; }
        }

        return myResult;
    } 
}
