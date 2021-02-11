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

    public static TariffSurcharge(number: number): string{
        return "#Tariff_Surcharge" + number.toString() + "Id";
    }
}