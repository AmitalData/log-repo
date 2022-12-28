import { AddNewDeploymentPackageComponent } from "./Component/NewEntity/AddNewDeploymentPackageComponent";


export const Components =
    [
        AddNewDeploymentPackageComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddNewDeploymentPackageComponent": { myResult = AddNewDeploymentPackageComponent; break; }
        }

        return myResult;
    }
}
