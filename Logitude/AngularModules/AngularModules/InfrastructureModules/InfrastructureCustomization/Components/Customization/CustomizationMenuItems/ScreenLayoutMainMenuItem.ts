import { FeatureLocator } from "../../../../../Infrastructure/Utilities/FeatureLocator";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class ScreenLayoutMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("screenLayout");
        this.TextCode = "Screen Layout";
        this.Code = "SCREENLAYOUT";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/ScreenLayoutComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled,
            IsTabsCustomizationEnabled: this.CheckTabsFeaturePermission(args),
        }
    }
    CheckTabsFeaturePermission(args: any) {
        let IsShowTabs = FeatureLocator.HasFeaturePermession("General", "TabsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowTabs) && !args.IsCustomFieldsMenue;
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowScreensLayout = FeatureLocator.HasFeaturePermession("General", "ScreenLayoutCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowScreensLayout) && !args.IsCustomFieldsMenue;
    }

}
