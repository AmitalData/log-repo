import { TariffModuleWorkspaceComponent } from './Components/Workspaces/TariffModuleWorkspaceComponent';
import { NewAirFreightCostComponent } from './Components/NewEntity/NewAirFreightCostComponent';
import { FieldTemplateComponent } from './Components/Templates/FieldTemplateComponent';


// Tabs
import { TariffGeneralTabComponent } from './Components/EditTabs/Tariff/TariffGeneralTabComponent';
import { AddEditTariffLineComponent } from './Components/EditTabs/Tariff/AddEditTariffLineComponent';


export const Components =
    [
        TariffModuleWorkspaceComponent,
        NewAirFreightCostComponent,
        FieldTemplateComponent,
        TariffGeneralTabComponent,
        AddEditTariffLineComponent
    ];

export const ControlsComponents =
    [
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "TariffModuleWorkspaceComponent": { myResult = TariffModuleWorkspaceComponent; break; }
            case "NewAirFreightCostComponent": { myResult = NewAirFreightCostComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "TariffGeneralTabComponent": { myResult = TariffGeneralTabComponent; break; }
            case "AddEditTariffLineComponent": { myResult = AddEditTariffLineComponent; break; }
        }

        return myResult;
    }
}
