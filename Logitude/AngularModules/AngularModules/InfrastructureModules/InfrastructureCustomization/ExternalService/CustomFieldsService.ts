import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';
declare var window: any;

export class CustomFieldsService implements ICustomizationService {
    private IsShowCustomFields: boolean;
    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {
        this.GetFeaturePermission();
        if (!(!args.IsObjectTableFilterEnabled || this.IsShowCustomFields || args.IsCustomFieldsMenue)) return null;
        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("Fields");
        this.customizationMainMenuItem.TextCode = "Custom Fields";
        this.customizationMainMenuItem.Code = "CUSTOMFIELD";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomFieldsComponent";
        let objectTable = window.ObjectTables.filter(d => d.Id === this.objectTableId)[0];

        this.customizationMainMenuItem.args = {

        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission() {
        this.IsShowCustomFields = FeatureLocator.HasFeaturePermession("General", "CustomFieldsCustomization");
    }
}
