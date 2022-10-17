import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class ScreenLayoutService implements ICustomizationService {

    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {

        if (args.IsCustomFieldsMenue) return null;
        if (!this.GetFeaturePermission(args)) return null;

        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("screenLayout");
        this.customizationMainMenuItem.TextCode = "Screen Layout";
        this.customizationMainMenuItem.Code = "SCREENLAYOUT";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/ScreenLayoutComponent";

        this.customizationMainMenuItem.args = {
            ObjectTableId: this.objectTableId,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled,
            IsTabsCustomizationEnabled: this.GetTabsFeaturePermission(args),
        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission(args: any): boolean {
        let IsShowScreensLayout = FeatureLocator.HasFeaturePermession("General", "ScreenLayoutCustomization");
        return !args.IsObjectTableFilterEnabled || IsShowScreensLayout;
    }
    GetTabsFeaturePermission(args: any): boolean {
        let IsShowTabs = FeatureLocator.HasFeaturePermession("General", "TabsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowTabs) && !args.IsCustomFieldsMenue;
    }
}
