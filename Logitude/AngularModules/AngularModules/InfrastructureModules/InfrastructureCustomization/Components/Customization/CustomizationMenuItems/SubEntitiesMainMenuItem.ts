import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class SubEntitiesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private mainArgs: any) {
        super("subEntities");
        this.TextCode = "Sub Entities";
        this.Code = "SUBENTITIES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/SubEntitiesComponent";
        this.args = this.BuildScreenArgs(mainArgs);
        this.IsVisible = this.GetFeaturePermission(mainArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
        }
    }
    GetFeaturePermission(args: any): boolean {
        let IsShowSubEntities = true; //to be updated later
        return (!args.IsObjectTableFilterEnabled || IsShowSubEntities) && !args.IsCustomFieldsMenue;
    }

}
