import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;
export class TabsMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("tabs");
        this.TextCode = "Tabs";
        this.Code = "TABS";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationTabsComponent";
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
        let IsShowTabs = CustomizationPermissionService.HasFeaturePermession("General", "TabsCustomization");
        return (!args.IsObjectTableFilterEnabled || IsShowTabs) && !args.IsCustomFieldsMenue && !args.IsSubEntity && this.screenArgs.ObjectTableName != "Card";
    }

}
