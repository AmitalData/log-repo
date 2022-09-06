import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";

export class ListItem {
    public Code: string;
    public Name: string;

    constructor(code: string, name: string | null = null) {
        this.Code = code;
        this.Name = name ? name : this.getItemNameFromCode(code);
    }

    private getItemNameFromCode(itemCode: string) {
        if (itemCode === DateTimeValueExpressions.PlusMinusToday) {
            return "Today +/-";
        }

        return (itemCode ? itemCode.split(/(?=[A-Z])/).join(" ") : null);
    }
}