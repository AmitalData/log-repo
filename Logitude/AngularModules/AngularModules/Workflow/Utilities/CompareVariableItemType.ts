import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ObjectTables } from "./ObjectTables";

export class CompareVariableItemType {

    static compare(item: TreeSelectItem, compareWithType: string, compareWithLookupOrPickListType: string) {
        switch (compareWithType) {
            case FieldTypes.LookUp:
                return this.compareItemLookupType(item, compareWithLookupOrPickListType);
            case FieldTypes.PickList:
                return this.compareItemPickListType(item, compareWithLookupOrPickListType);
            case FieldTypes.Text:
            case FieldTypes.NText:
                return this.compareItemTextType(item);
            case FieldTypes.Boolean:
                return this.compareItemBooleanType(item);
            case FieldTypes.Date:
            case FieldTypes.DateTime:
                return this.compareItemDateType(item);
            case FieldTypes.BigInteger:
            case FieldTypes.Decimal:
            case FieldTypes.Double:
            case FieldTypes.Integer:
            case FieldTypes.SigDouble:
            case FieldTypes.UnsDecimal:
            case FieldTypes.UnsInteger:
                return this.compareItemNumberType(item);
            default:
                return this.compareItemDefaultType(item, compareWithType);
        }
    }

    private static compareItemLookupType(item: TreeSelectItem, compareWithLookupOrPickListType: string) {
        let fieldItemType = item.data["type"];
        let fieldItemLookupType = item.data["lookupType"];
        let fieldItemCode = item.data["fieldCode"];
        if (compareWithLookupOrPickListType) {
            let lookupField = compareWithLookupOrPickListType + "." + ObjectTables.getKeyPropertyPathByName(compareWithLookupOrPickListType);
            if (fieldItemCode === lookupField) {
                return true;
            }
        }
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.LookUp)) {
            return false;
        }
        if (fieldItemLookupType !== undefined && (fieldItemLookupType === null || fieldItemLookupType !== compareWithLookupOrPickListType)) {
            return false;
        }
        return true;
    }

    private static compareItemPickListType(item: TreeSelectItem, compareWithLookupOrPickListType: string) {
        let fieldItemType = item.data["type"];
        let fieldItemPickListType = item.data["picklistType"];
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.PickList)) {
            return false;
        }
        if (fieldItemPickListType !== undefined && (fieldItemPickListType === null || fieldItemPickListType !== compareWithLookupOrPickListType)) {
            return false;
        }
        return true;
    }

    private static compareItemTextType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        let validTypes = [FieldTypes.Text, FieldTypes.NText, FieldTypes.LookUp, FieldTypes.PickList];
        if (fieldItemType !== undefined && (fieldItemType === null || validTypes.indexOf(fieldItemType) === -1)) {
            return false;
        }
        return true;
    }

    private static compareItemBooleanType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.Boolean)) {
            return false;
        }
        return true;
    }

    private static compareItemDateType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        let validTypes = [FieldTypes.Date, FieldTypes.DateTime];
        if (fieldItemType !== undefined && (fieldItemType === null || validTypes.indexOf(fieldItemType) === -1)) {
            return false;
        }
        return true;
    }

    private static compareItemNumberType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        let validTypes = [FieldTypes.BigInteger, FieldTypes.Decimal, FieldTypes.Double, FieldTypes.Integer, FieldTypes.SigDouble, FieldTypes.UnsDecimal, FieldTypes.UnsInteger];
        if (fieldItemType !== undefined && (fieldItemType === null || validTypes.indexOf(fieldItemType) === -1)) {
            return false;
        }
        return true;
    }

    private static compareItemDefaultType(item: TreeSelectItem, compareWithType: string) {
        let fieldItemType = item.data["type"];
        let fieldItemTypeToCompare = fieldItemType;
        let compareWithTypeToCompare = compareWithType;
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemTypeToCompare !== compareWithTypeToCompare)) {
            return false;
        }
        return true;
    }

}