import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class ScreenLayoutMainMenuItem extends CustomizationMainMenuItem {

    constructor(private mainArgs: any) {
        super("screenLayout");
        this.TextCode = "Screen Layout";
        this.Code = "SCREENLAYOUT";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/ScreenLayoutComponent";
        this.args = this.BuildScreenArgs(mainArgs);
        this.IsVisible = this.GetFeaturePermission(mainArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled,
            IsTabsCustomizationEnabled: this.GetTabsFeaturePermission(args),
        }
    }
    GetTabsFeaturePermission(args: any) {
        let IsShowTabs = FeatureLocator.HasFeaturePermession("General", "TabsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowTabs) && !args.IsCustomFieldsMenue;
    }
    GetFeaturePermission(args: any): boolean {
        let IsShowScreensLayout = FeatureLocator.HasFeaturePermession("General", "ScreenLayoutCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowScreensLayout) && !args.IsCustomFieldsMenue;
    }

}
