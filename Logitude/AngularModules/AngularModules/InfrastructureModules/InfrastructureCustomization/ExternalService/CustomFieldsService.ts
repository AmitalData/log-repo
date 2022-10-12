import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';
declare var window: any;

export class CustomFieldsService implements ICustomizationService {

    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {
        
        if (!this.GetFeaturePermission(args)) return null;

        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("Fields");
        this.customizationMainMenuItem.TextCode = "Custom Fields";
        this.customizationMainMenuItem.Code = "CUSTOMFIELD";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomFieldsComponent";
        let objectTable = window.ObjectTables.filter(d => d.Id === this.objectTableId)[0];

        this.customizationMainMenuItem.args = {
            ObjectTableId: objectTable.Id,
            ObjectTableName: objectTable.Name,
            MaxNumberOfCustomFields: objectTable.MaxNumberOfCustomFields,
            IsCustomFieldsMenue: args.IsCustomFieldsMenue,
            AllowCustomFields: objectTable.AllowCustomFields
        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission(args: any): boolean {
        let IsShowCustomFields = FeatureLocator.HasFeaturePermession("General", "CustomFieldsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowCustomFields || args.IsCustomFieldsMenue);
    }
}
