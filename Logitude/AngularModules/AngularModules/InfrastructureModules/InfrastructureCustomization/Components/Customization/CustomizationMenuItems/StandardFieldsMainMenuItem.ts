import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class StandardFieldsMainMenuItem extends CustomizationMainMenuItem {

    constructor(private mainArgs: any) {
        super("Fields");
        this.TextCode = "Standard Fields";
        this.Code = "STANDARDFIELD";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent";
        this.args = this.BuildScreenArgs(mainArgs);
        this.IsVisible = this.GetFeaturePermission(mainArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId
        }
    }
    GetFeaturePermission(args: any): boolean {
        let IsShowStandardFields = FeatureLocator.HasFeaturePermession("General", "StandardFieldsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowStandardFields) && !args.IsCustomFieldsMenue;
    }

}
