import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class SubEntitiesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainArgs: any) {
        super("subEntities");
        this.TextCode = "Sub Entities";
        this.Code = "SUBENTITIES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/SubEntitiesComponent";
        this.args = this.BuildScreenArgs(customizationMainArgs);
        this.IsVisible = this.GetFeaturePermission(customizationMainArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled
        }
    }
    GetFeaturePermission(args: any): boolean {
        let IsShowSubEntities = FeatureLocator.HasFeaturePermession("General", "SubEntitiesCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowSubEntities) && !args.IsCustomFieldsMenue && !args.IsSubEntity;
    }

}
