import { AppTool } from '../../../Infrastructure/Tools';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class SubEntitiesService implements ICustomizationService {

    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {

        if (args.IsCustomFieldsMenue) return null;
        if (!this.GetFeaturePermission(args)) return null;

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
    GetFeaturePermission(args: any): boolean {
        let IsShowSubEntities = true; //to be updated later
        return !args.IsObjectTableFilterEnabled || IsShowSubEntities;
    }
}
