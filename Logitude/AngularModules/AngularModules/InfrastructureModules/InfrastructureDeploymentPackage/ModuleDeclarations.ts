import { CustomFieldsTabComponent } from "./Component/EditTabs/CustomFieldsTabComponent";
import { GeneralTabComponent } from "./Component/EditTabs/GeneralTabComponent";
import { AddNewDeploymentPackageComponent } from "./Component/NewEntity/AddNewDeploymentPackageComponent";
import { NewExportDeploymentPackageComponent } from "./Component/NewEntity/NewExportDeploymentPackageComponent";
import { NewImportDeploymentPackageComponent } from "./Component/NewEntity/NewImportDeploymentPackageComponent";


export const Components =
    [
        AddNewDeploymentPackageComponent,
        NewExportDeploymentPackageComponent,
        NewImportDeploymentPackageComponent,
        CustomFieldsTabComponent,
        GeneralTabComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddNewDeploymentPackageComponent": { myResult = AddNewDeploymentPackageComponent; break; }
            case "NewExportDeploymentPackageComponent": { myResult = NewExportDeploymentPackageComponent; break; }
            case "NewImportDeploymentPackageComponent": { myResult = NewImportDeploymentPackageComponent; break; }
            case "CustomFieldsTabComponent": { myResult = CustomFieldsTabComponent; break; }
            case "GeneralTabComponent": { myResult = GeneralTabComponent; break; }
        }

        return myResult;
    }
}
