import { Entities } from "Workflow/Utilities/Entities";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { EntitiesType } from "Workflow/Types";

export class EntitiesTreeList {
    public Items: TreeSelectItem[] = [];

    private EntitiesType: EntitiesType;

    constructor(entitiesType: EntitiesType = "all") {
        this.EntitiesType = entitiesType;
        this.setEntitiesTreeItems();
    }

    private setEntitiesTreeItems() {
        if (this.EntitiesType === "child") {
            this.setChildEntitiesTreeItems();
        } else {
            this.setParentEntitiesTreeItems();
        }
    }

    private setChildEntitiesTreeItems() {
        Entities.getChildren().forEach(childEntity => {
            let entityItem = new TreeSelectItem(childEntity.Code, childEntity.Name, true, true, false, false, []);
            this.Items.push(entityItem);
        });
    }

    private setParentEntitiesTreeItems() {
        Entities.getParents().forEach(entity => {
            let onlyParentEntities = this.EntitiesType === "parent";
            let childrenItems = onlyParentEntities ? [] : this.getChildrenItems(entity.Code);
            let entityItem = new TreeSelectItem(entity.Code, entity.Name, onlyParentEntities, true, !onlyParentEntities, false, childrenItems);
            this.Items.push(entityItem);
        });
    }

    private getChildrenItems(parentEntityCode: string) {
        let childrenItems = [];
        let childEntities = Entities.getChildren().filter(e => e.ParentEntityCode === parentEntityCode);
        childEntities.forEach(childEntity => {
            let childrenItem = new TreeSelectItem(childEntity.Code, childEntity.Name, true, true, false, false, []);
            childrenItems.push(childrenItem);
        });
        return childrenItems;
    }
}