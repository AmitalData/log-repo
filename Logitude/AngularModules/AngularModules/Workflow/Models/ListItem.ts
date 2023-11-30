import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";

export class ListItem {
    public Code: string;
    public Name: string;

    constructor(code: string, name: string | null = null) {
        this.Code = code;
        this.Name = name ? name : this.getItemNameFromCode(code);
    }

    private getItemNameFromCode(itemCode: string) {
        let customItemNameFromCode = this.getCustomItemNameFromCode(itemCode);
        if (customItemNameFromCode) {
            return customItemNameFromCode;
        }
        return (itemCode ? itemCode.split(/(?=[A-Z])/).join(" ") : null);
    }

    private getCustomItemNameFromCode(itemCode: string) {
        switch (itemCode) {
            case DateTimeValueExpressions.PlusMinusToday:
                return "Today +/-";
            default:
                return null;
        }
    }
}