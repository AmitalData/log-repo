import { AppTool } from "../../../../../Infrastructure/Tools";
import { CustomizationPermissionService } from "../../../ExternalService/CustomizationPermissionService";
import { CustomizationMainMenuItem } from "./CustomizationMainMenuItem";

declare var window: any;

export class SubEntitiesMainMenuItem extends CustomizationMainMenuItem {

    constructor(private customizationMainMenuArgs: any) {
        super("subEntities");
        this.TextCode = "Sub Objects";
        this.Code = "SUBENTITIES";
        this.ComponentPath = "./InfrastructureModules/InfrastructureCustomization/Components/Customization/SubEntitiesComponent";
        this.screenArgs = this.BuildScreenArgs(customizationMainMenuArgs);
        this.IsVisible = this.CheckFeaturePermission(customizationMainMenuArgs);


    }
    BuildScreenArgs(args: any): any {
        let objectTable = window.ObjectTables.filter(d => d.Id === args.ObjectTableId)[0];
        return {
            ObjectTableId: args.ObjectTableId,
            ObjectTableName: objectTable.Name,
            IsObjectTableFilterEnabled: args.IsObjectTableFilterEnabled
        }
    }
    CheckFeaturePermission(args: any): boolean {
        let IsShowSubEntities = CustomizationPermissionService.HasFeaturePermession("General", "SubEntitiesCustomization");
        let IsReferenceCustomObject = this.CheckReferenceCustomObjectType(args.ObjectTableId);
        return (!args.IsObjectTableFilterEnabled || IsShowSubEntities) && !args.IsCustomFieldsMenue && !args.IsSubEntity && !IsReferenceCustomObject && this.screenArgs.ObjectTableName != "Card";
    }
    CheckReferenceCustomObjectType(objectTableId: string) {
        if (AppTool.IsNullOrEmpty(objectTableId)) return false;
        let objectTable = window.ObjectTables.filter(d => d.Id === objectTableId)[0];
        return (objectTable.IsCustom && AppTool.IsNullOrEmpty(objectTable.ParentObjectTableId) && objectTable.ObjectTableTypeCode == "MD");
    }

}
