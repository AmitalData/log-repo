import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class TabsMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainArgs: any) {
        super("tabs");
        this.TextCode = "Tabs";
        this.Code = "TABS";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationTabsComponent";
        this.args = this.BuildScreenArgs(customizationMainArgs);
        this.IsVisible = this.GetFeaturePermission(customizationMainArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId
        }
    }
    GetFeaturePermission(args: any): boolean {
        let IsShowTabs = FeatureLocator.HasFeaturePermession("General", "TabsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowTabs) && !args.IsCustomFieldsMenue;
    }

}
