import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { ListItem } from "./ListItem";

export class DateTimeValueExpressionsList {
    public DateTimeValueExpressions: ListItem[] = [];

    constructor() {
        this.setDateTimeValueExpressions();
    }

    private setDateTimeValueExpressions() {
        this.DateTimeValueExpressions = [
            new ListItem(DateTimeValueExpressions.Date),
            new ListItem(DateTimeValueExpressions.PlusMinusToday),
        ];
    }
}