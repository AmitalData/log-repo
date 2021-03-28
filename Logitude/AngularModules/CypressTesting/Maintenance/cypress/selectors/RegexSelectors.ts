export class RegexSelectors {

    public static NewWizardButton(name: string): string {
        return "#NewButton_" + name;
    }

    public static SaveButton(itemName: string): string {
        return "#" + itemName + "-Save"
    }

    public static SaveCloseButton(itemName: string): string {
        return "#" + itemName + "-SaveClose"
    }

    //#region Quote Template 
    public static QuoteTemplateSectionsButton(section: string): string {
        return "[data-cy='Setting_" + section + "']"
    }

    public static HeaderFooterColumnWidth(number: string): string {
        return "[data-cy='CoulmnWidth" + number + "']"
    }

    public static AvaliableColumnsFields(field: string): string {
        return "[data-cy='Column_ " + field + "']"
    }

    public static ColumnDropArea(coulmn: string): string {
        return "[data-cy='" + coulmn + "']"
    }

    public static LabelDiv(label: string): string {
        return "[data-cy='Label_" + label + "']"
    }

    public static LabelTextBox(label: string): string {
        return "[data-cy='LogTextBox_" + label + "']"
    }

    public static IntroductionDataField(field: string): string {
        return "[data-cy='Quote_" + field + "']"
    }

    public static PricingCheckBox(columnName: string): string {
        return "[data-cy='checkbox_" + columnName + "']"
    }
    //#endregion

    //#region Currency Settings 

    public static CurrencyDate(currencyType: string): string {
        return "[data-cy='Date_" + currencyType + "']"
    }

    public static CurrencyRate(currencyType: string): string {
        return "[data-cy='CurrentRate_" + currencyType + "']"
    }

    public static CurrencyEditButton(currencyType: string): string {
        return "[data-cy='EditButton_" + currencyType + "']"
    }

    public static CurrencyHistoryButton(currencyType: string): string {
        return "[data-cy='HistoryButton_" + currencyType + "']"
    }

    //#endregion
}