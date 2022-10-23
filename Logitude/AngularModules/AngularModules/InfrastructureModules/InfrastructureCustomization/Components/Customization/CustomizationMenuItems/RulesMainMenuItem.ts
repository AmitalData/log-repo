import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class RulesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainArgs: any) {
        super("rules");
        this.TextCode = "Rules";
        this.Code = "RULES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent";
        this.args = this.BuildScreenArgs(customizationMainArgs);
        this.IsVisible = this.GetFeaturePermission(customizationMainArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId
        }
    }
    GetFeaturePermission(args: any): boolean {
        let IsShowRules = FeatureLocator.HasFeaturePermession("General", "RulesCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowRules) && !args.IsCustomFieldsMenue;
    }

}
