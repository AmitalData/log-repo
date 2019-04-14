
import { TariffModuleWorkspaceComponent } from './Components/Workspaces/TariffModuleWorkspaceComponent';
import { NewAirFreightCostComponent } from './Components/NewEntity/NewAirFreightCostComponent';



export const Components =
    [
        TariffModuleWorkspaceComponent,
        NewAirFreightCostComponent,
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
        }

        return myResult;
    }
}
