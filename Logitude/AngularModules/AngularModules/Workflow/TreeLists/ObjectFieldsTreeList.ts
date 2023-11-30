import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";

export class ObjectFieldsTreeList {
    private ObjectTableId: string;
    public Items: TreeSelectItem[] = [];

    constructor(objectTableId: string) {
        this.ObjectTableId = objectTableId;
        this.setObjectFieldsTreeItems();
    }

    private setObjectFieldsTreeItems() {
        ObjectFields.getByObjectTableId(this.ObjectTableId).forEach(objectField => {
            let data = {
                objectField: objectField,
                type: objectField.DataTypeCode,
                lookupType: (objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null),
                picklistType: (objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null),
                fieldCode: objectField.FieldCode,
                tooltip: objectField.FieldName
            };

            let itemKey = objectField.FieldCode;
            let itemTitle = objectField.FullNameTextCodeDefaultText.trim();

            let objectFieldItem = new TreeSelectItem(itemKey, itemTitle, true, true, false, false, [], data);
            this.Items.push(objectFieldItem);
        });
    }
}