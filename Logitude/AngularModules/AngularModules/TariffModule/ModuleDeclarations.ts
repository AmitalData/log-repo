
import { TariffModuleWorkspaceComponent } from './Components/Workspaces/TariffModuleWorkspaceComponent';



export const Components =
    [
        TariffModuleWorkspaceComponent,
    ];

export const ControlsComponents =
    [
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "TariffModuleWorkspaceComponent": { myResult = TariffModuleWorkspaceComponent; break; }

                
        }

        return myResult;
    }
}
