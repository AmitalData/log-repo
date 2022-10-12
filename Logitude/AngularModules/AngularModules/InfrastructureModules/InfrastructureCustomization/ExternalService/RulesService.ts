import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class RulesService implements ICustomizationService {

    private IsShowRules: boolean;
    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {
        this.GetFeaturePermission();
        if (!((!args.IsObjectTableFilterEnabled || this.IsShowRules) && !args.IsCustomFieldsMenue)) return null;
        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("rules");
        this.customizationMainMenuItem.TextCode = "Rules";
        this.customizationMainMenuItem.Code = "RULES";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent";
        let objectTable = window.ObjectTables.filter(d => d.Id === this.objectTableId)[0];

        this.customizationMainMenuItem.args = {

        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission() {
        this.IsShowRules = FeatureLocator.HasFeaturePermession("General", "RulesCustomization");
    }
}
