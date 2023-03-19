import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;
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
        let objectTable = window.ObjectTables.filter(d => d.Id === args.ObjectTableId)[0];
        return {
            ObjectTableId: args.ObjectTableId,
            ObjectTableName: objectTable.Name,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled,
            IsTabsCustomizationEnabled: this.CheckTabsFeaturePermission(args),
        }
    }
    CheckTabsFeaturePermission(args: any) {
        let IsShowTabs = CustomizationPermissionService.HasFeaturePermession("General", "TabsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowTabs) && !args.IsCustomFieldsMenue;
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowScreensLayout = CustomizationPermissionService.HasFeaturePermession("General", "ScreenLayoutCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowScreensLayout) && !args.IsCustomFieldsMenue && this.screenArgs.ObjectTableName != "Card";
    }

}
