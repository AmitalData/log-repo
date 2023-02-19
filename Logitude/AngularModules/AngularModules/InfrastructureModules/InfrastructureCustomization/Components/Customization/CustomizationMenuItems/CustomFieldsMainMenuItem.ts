import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;

export class CustomFieldsMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("Fields");
        this.TextCode = "Custom Fields";
        this.Code = "CUSTOMFIELD";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomFieldsComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        let objectTable = window.ObjectTables.filter(d => d.Id === args.ObjectTableId)[0];

        return {
            ObjectTableId: objectTable.Id,
            ObjectTableName: objectTable.Name,
            MaxNumberOfCustomFields: objectTable.MaxNumberOfCustomFields,
            IsCustomFieldsMenue: args.IsCustomFieldsMenue,
            AllowCustomFields: objectTable.AllowCustomFields
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowCustomFields = CustomizationPermissionService.HasFeaturePermession("General", "CustomFieldsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowCustomFields || args.IsCustomFieldsMenue);
    }

}
