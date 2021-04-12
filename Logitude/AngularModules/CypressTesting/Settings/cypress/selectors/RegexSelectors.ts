export class RegexSelectors {
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