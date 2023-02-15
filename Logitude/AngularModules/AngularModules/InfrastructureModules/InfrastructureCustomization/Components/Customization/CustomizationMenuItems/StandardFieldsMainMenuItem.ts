import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

export class StandardFieldsMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("Fields");
        this.TextCode = "Standard Fields";
        this.Code = "STANDARDFIELD";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowStandardFields = CustomizationPermissionService.HasFeaturePermession("General", "StandardFieldsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowStandardFields) && !args.IsCustomFieldsMenue;
    }

}
