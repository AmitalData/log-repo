import { AppTool } from '../../../Infrastructure/Tools';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class SubEntitiesService implements ICustomizationService {

    private IsShowSubEntities: boolean;
    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {
        this.GetFeaturePermission();
        if (!((!args.IsObjectTableFilterEnabled || this.IsShowSubEntities) && !args.IsCustomFieldsMenue)) return null;
        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("subEntities");
        this.customizationMainMenuItem.TextCode = "Sub Entities";
        this.customizationMainMenuItem.Code = "SUBENTITIES";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/SubEntitiesComponent";
        let objectTable = window.ObjectTables.filter(d => d.Id === this.objectTableId)[0];

        this.customizationMainMenuItem.args = {

        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission() {
        this.IsShowSubEntities = true; //to be updated later
    }
}
