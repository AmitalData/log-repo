import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class StandardFieldsService implements ICustomizationService {

    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {

        if (args.IsCustomFieldsMenue) return null;
        if (!this.GetFeaturePermission(args)) return null;

        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("Fields");
        this.customizationMainMenuItem.TextCode = "Standard Fields";
        this.customizationMainMenuItem.Code = "STANDARDFIELD";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent";

        this.customizationMainMenuItem.args = {
            ObjectTableId: this.objectTableId
        }
        return this.customizationMainMenuItem;
    }


    GetFeaturePermission(args: any): boolean {
        let IsShowStandardFields = FeatureLocator.HasFeaturePermession("General", "StandardFieldsCustomization");
        return !args.IsObjectTableFilterEnabled || IsShowStandardFields;
    }
}
