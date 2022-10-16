import { AppTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { CustomizationMainMenuItem } from '../Components/Customization/CustomizationEditComponent';
import { ICustomizationService } from '../Interface/ICustomizationService';

declare var window: any;

export class RulesService implements ICustomizationService {

    private customizationMainMenuItem: CustomizationMainMenuItem;
    private objectTableId: string;

    LoadCustomizationMenuItem(args: any) {

        if (args.IsCustomFieldsMenue) return null;
        if (!this.GetFeaturePermission(args)) return null;

        this.objectTableId = args.ObjectTableId;
        this.customizationMainMenuItem = new CustomizationMainMenuItem("rules");
        this.customizationMainMenuItem.TextCode = "Rules";
        this.customizationMainMenuItem.Code = "RULES";
        this.customizationMainMenuItem.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent";

        this.customizationMainMenuItem.args = {
            ObjectTableId: this.objectTableId
        }
        return this.customizationMainMenuItem;
    }

    GetFeaturePermission(args: any): boolean {
        let IsShowRules = FeatureLocator.HasFeaturePermession("General", "RulesCustomization");
        return !args.IsObjectTableFilterEnabled || IsShowRules;
    }
}
