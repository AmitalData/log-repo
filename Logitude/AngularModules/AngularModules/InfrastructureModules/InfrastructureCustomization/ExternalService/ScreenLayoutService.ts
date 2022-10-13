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
        let objectTable = window.ObjectTables.filter(d => d.Id === this.objectTableId)[0];

        this.customizationMainMenuItem.args = {

        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission(args: any): boolean {
        let IsShowScreensLayout = FeatureLocator.HasFeaturePermession("General", "ScreenLayoutCustomization");
        return !args.IsObjectTableFilterEnabled || IsShowScreensLayout;
    }
}
