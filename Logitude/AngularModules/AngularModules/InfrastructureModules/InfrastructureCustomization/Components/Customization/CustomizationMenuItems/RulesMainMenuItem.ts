import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class RulesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("rules");
        this.TextCode = "Rules";
        this.Code = "RULES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowRules = FeatureLocator.HasFeaturePermession("General", "RulesCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowRules) && !args.IsCustomFieldsMenue;
    }

}
