export class TariffSelectors
{
	public static readonly TariffMenu = "#GeneralMHTariffModule";
    public static readonly NewFreightCostToggleButton = "div[id^='NEWTARIFF']:first";
    public static readonly NewSurchargeCostToggleButton = "div[id^='NEWTARIFF']:last";
    public static readonly NewFreightCostToggleMenuButton = "div[id^='NEWTARIFF']:first .ToggleButtonMenu button";
    public static readonly NewSurchargeCostToggleMenuButton = "div[id^='NEWTARIFF']:last .ToggleButtonMenu button";
    public static readonly TariffName = "#Tariff_Name";
    public static readonly TariffSeller = "#Tariff_SellerId";
    public static readonly TariffStartDate = "#date_Tariff_StartDate";
    public static readonly TariffProduct = "#Tariff_TariffProductId";
    public static readonly TariffVersionAllInChargeType = "input[id^='TariffVersionAllInCharge_ChargesTypeId']:last";
    public static readonly EditTariffAllInChargesButton = "newairfreightcostcomponent button[id^='Edit']:last";
    public static readonly TariffNotes = "#Tariff_Notes";
    public static readonly TariffLineFromPort = "#TariffLine_OriginPortId";
    public static readonly TariffLineToPort = "#TariffLine_DestinationPortId";
    public static readonly TariffLineTransitTime = "#TariffLine_TransitTime";
    public static readonly TariffLineNotes = "#TariffLine_Notes";
    public static readonly TariffLineMinPrice = "#TariffLine_MinPrice";
    public static readonly SaveTariff = "#Tariff-Save";
    public static readonly TariffContractNumber = "#Tariff_ContractNumber";
    public static readonly TariffCurrency = "#Tariff_CurrencyId";


    public static readonly TariffActionsToggleButton = ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ToggleButton img";
    public static readonly TariffActionsToggleButtonItem = ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ToggleButton button";
    public static readonly ContainsCopyIntoNewVersion = "Copy into new Version";


    public static TariffSurcharge(number: number): string{
        return "#Tariff_Surcharge" + number.toString() + "Id";
    }

    public static TariffLineSurchargePrice(number: number): string{
        return "#TariffLine_Surcharge" + number.toString() + "Price";
    }

    public static TariffLineStepPrice(number: number): string{
        return "#TariffLine_Step" + number.toString() + "Price";
    }

    public static TariffLineEditButton(lineNumber: number): string{
        return ".MediaFillAbsolute:visible #row" + lineNumber.toString() + " button[id^='Edit']";
    }
}