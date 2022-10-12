import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class StandardFieldsService implements ICustomizationService {

    private IsShowStandardFields: boolean;
    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {
        this.GetFeaturePermission();
        if (!((!args.IsObjectTableFilterEnabled || this.IsShowStandardFields) && !args.IsCustomFieldsMenue)) return null;
        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("Fields");
        this.customizationMainMenuItem.TextCode = "Standard Fields";
        this.customizationMainMenuItem.Code = "STANDARDFIELD";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent";
        let objectTable = window.ObjectTables.filter(d => d.Id === this.objectTableId)[0];

        this.customizationMainMenuItem.args = {

        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission() {
        this.IsShowStandardFields = FeatureLocator.HasFeaturePermession("General", "StandardFieldsCustomization");
    }
}
