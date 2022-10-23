import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class SubEntitiesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("subEntities");
        this.TextCode = "Sub Entities";
        this.Code = "SUBENTITIES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/SubEntitiesComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowSubEntities = FeatureLocator.HasFeaturePermession("General", "SubEntitiesCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowSubEntities) && !args.IsCustomFieldsMenue && !args.IsSubEntity;
    }

}
