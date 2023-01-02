import { Entities } from "./Entities";
import { TreeSelectItem } from "./TreeSelectItem";

type EntitiesType = "all" | "parent" | "child";

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
        Entities.Children.forEach(childEntity => {
            let entityItem = new TreeSelectItem(childEntity.ParentEntityCode + "." + childEntity.Code, childEntity.Name, true, true, false, false, []);
            this.Items.push(entityItem);
        });
    }

    private setParentEntitiesTreeItems() {
        Entities.Parents.forEach(entity => {
            let onlyParentEntities = this.EntitiesType === "parent";
            let childrenItems = onlyParentEntities ? [] : this.getChildrenItems(entity.Code);
            let entityItem = new TreeSelectItem(entity.Code, entity.Name, onlyParentEntities, true, !onlyParentEntities, false, childrenItems);
            this.Items.push(entityItem);
        });
    }

    private getChildrenItems(parentEntityCode: string) {
        let childrenItems = [];
        let childEntities = Entities.Children.filter(e => e.ParentEntityCode === parentEntityCode);
        childEntities.forEach(childEntity => {
            let childrenItem = new TreeSelectItem(childEntity.ParentEntityCode + "." + childEntity.Code, childEntity.Name, true, true, false, false, []);
            childrenItems.push(childrenItem);
        });
        return childrenItems;
    }
}