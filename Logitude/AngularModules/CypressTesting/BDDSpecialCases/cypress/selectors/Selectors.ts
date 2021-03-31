export class BDDSpecialCasesSelectors {


    public static readonly HAWBDate = "#date_Shipment_HAWBDate"
    public static readonly TimeZoneComboBox = "[data-cy='ComboBox_TimeZone']"
    public static readonly DateTimeFormatComboBox = "[data-cy='ComboBox_DateTimeFormat']"
    public static readonly ComboBoxItem = "[id^='ComboBoxItem']"
    
    public static EventDateTime(eventName: string) {
        return "td[data-cy='Time_"+eventName+"']"
    }
}

