import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;
export class RulesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("rules");
        this.TextCode = "Rules";
        this.Code = "RULES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        let objectTable = window.ObjectTables.filter(d => d.Id === args.ObjectTableId)[0];
        return {
            ObjectTableId: args.ObjectTableId,
            ObjectTableName: objectTable.Name,
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowRules = CustomizationPermissionService.HasFeaturePermession("General", "RulesCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowRules) && !args.IsCustomFieldsMenue && this.screenArgs.ObjectTableName != "Card";
    }

}
