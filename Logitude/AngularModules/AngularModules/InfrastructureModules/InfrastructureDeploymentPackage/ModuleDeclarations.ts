import { AddCustomFieldsComponent } from "./Component/EditTabs/AddCustomFieldsComponent";
import { CustomFieldsTabComponent } from "./Component/EditTabs/CustomFieldsTabComponent";
import { DeploymentPackageGeneralTabComponent } from "./Component/EditTabs/DeploymentPackageGeneralTabComponent";
import { AddNewDeploymentPackageComponent } from "./Component/NewEntity/AddNewDeploymentPackageComponent";
import { NewExportDeploymentPackageComponent } from "./Component/NewEntity/NewExportDeploymentPackageComponent";
import { NewImportDeploymentPackageComponent } from "./Component/NewEntity/NewImportDeploymentPackageComponent";
import { ExportMenuButtonComponent } from "./Component/MenuButtons/ExportMenuButtonComponent";


export const Components =
    [
        AddNewDeploymentPackageComponent,
        NewExportDeploymentPackageComponent,
        NewImportDeploymentPackageComponent,
        AddCustomFieldsComponent,
        CustomFieldsTabComponent,
        DeploymentPackageGeneralTabComponent,
        ExportMenuButtonComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddNewDeploymentPackageComponent": { myResult = AddNewDeploymentPackageComponent; break; }
            case "NewExportDeploymentPackageComponent": { myResult = NewExportDeploymentPackageComponent; break; }
            case "NewImportDeploymentPackageComponent": { myResult = NewImportDeploymentPackageComponent; break; }
            case "CustomFieldsTabComponent": { myResult = CustomFieldsTabComponent; break; }
            case "AddCustomFieldsComponent": { myResult = AddCustomFieldsComponent; break; }
            case "DeploymentPackageGeneralTabComponent": { myResult = DeploymentPackageGeneralTabComponent; break; }
            case "ExportMenuButtonComponent": { myResult = ExportMenuButtonComponent; break; }
        }

        return myResult;
    }
}
