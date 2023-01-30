import { FieldTypes } from "Workflow/Constants/FieldTypes";

export class FieldType {

    private static PrimitiveTypes: string[] = [
        FieldTypes.Text,
        FieldTypes.Date,
        FieldTypes.Decimal,
        FieldTypes.Boolean
    ];

    static isPrimitive(type: string) {
        if (type) {
            type = type.replace("[]", "");
            return this.PrimitiveTypes.includes(type);
        }
        return false;
    }

}