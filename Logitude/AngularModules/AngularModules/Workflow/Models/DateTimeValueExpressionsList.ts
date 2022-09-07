import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { ListItem } from "./ListItem";

export class DateTimeValueExpressionsList {
    public Items: ListItem[] = [];

    constructor() {
        this.setDateTimeValueExpressions();
    }

    private setDateTimeValueExpressions() {
        this.Items = [
            new ListItem(DateTimeValueExpressions.Date),
            new ListItem(DateTimeValueExpressions.PlusMinusToday),
        ];
    }
}