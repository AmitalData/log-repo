import { AppTool } from "../../../../../Infrastructure/Tools";
import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;

export class QueriesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("queries");
        this.TextCode = "Views";
        this.Code = "QUERIES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationQueriesComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        return {
            ObjectTableId: args.ObjectTableId
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let isShowQueries = CustomizationPermissionService.HasFeaturePermession("General", "QueriesCustomization");
        let isMainCustomObjectTable = this.IsMainCustomObjectTable(args.ObjectTableId);
        return (!args.IsObjectTableFilterEnabled || isShowQueries) && !args.IsCustomFieldsMenue && !args.IsSubEntity && isMainCustomObjectTable;
    }

    IsMainCustomObjectTable(objectTableId: string) {
        if (AppTool.IsNullOrEmpty(objectTableId)) return false;
        let objectTable = window.ObjectTables.filter(d => d.Id === objectTableId)[0];
        return (objectTable.IsCustom && AppTool.IsNullOrEmpty(objectTable.ParentObjectTableId));
    }

}
