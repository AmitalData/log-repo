export class AccountingPeriodSelectors {
    public static readonly MiscellaneousMenu = "#GeneralMHOthers";
    public static readonly DefineAccountingPeriod = "הגדרת תקופות";
    public static readonly AccountingPeriods = "תקופות חשבונאיות";
    public static readonly YearInput = "#AccountingPeriod_Year";
    public static readonly ApproveYearButton = ".RedButton:contains('אישור')";
    public static readonly AccountingPeriodsTable = ".SimpleGridView";
    public static readonly ClosedMonthColumn = "td:contains('חודש סגור')";
    public static readonly PeriodTypeColumn = "td:contains('סוג תקופה')";
    public static readonly EditButton = "[title*='עריכה'], .EditIcon, button[title*='עריכה']";
    public static readonly OpenMonthDialog = ".LogitudeWindow";
    public static readonly OpenMonthButton = "button:contains('פתח'), button:contains('פתח חודש')";
    public static readonly ConfirmOpenMonthButton = ".RedButton:contains('אישור')";
}

