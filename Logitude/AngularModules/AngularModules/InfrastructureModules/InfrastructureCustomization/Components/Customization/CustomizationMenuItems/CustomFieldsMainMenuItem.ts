import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;

export class CustomFieldsMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainArgs: any) {
        super("Fields");
        this.TextCode = "Custom Fields";
        this.Code = "CUSTOMFIELD";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomFieldsComponent";
        this.args = this.BuildScreenArgs(customizationMainArgs);
        this.IsVisible = this.GetFeaturePermission(customizationMainArgs);


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
    GetFeaturePermission(args: any): boolean {
        let IsShowCustomFields = FeatureLocator.HasFeaturePermession("General", "CustomFieldsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowCustomFields || args.IsCustomFieldsMenue);
    }

}
