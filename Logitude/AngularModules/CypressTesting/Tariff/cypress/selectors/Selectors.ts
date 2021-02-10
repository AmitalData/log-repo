export class TariffSelectors
{
	public static readonly TariffMenu = "#GeneralMHTariffModule";
    public static readonly NewFreightCostToggleButton = "#NEWTARIFF";
    public static readonly NewSurchargeCostToggleButton = "#NEWTARIFF_1";
    public static readonly TariffName = "#Tariff_Name";
    public static readonly TariffSeller = "#Tariff_SellerId";
    public static readonly TariffStartDate = "#date_Tariff_StartDate";
    public static readonly TariffProduct = "#Tariff_TariffProductId";
    public static readonly TariffVersionAllInChargeType = "input[id^='TariffVersionAllInCharge_ChargesTypeId']:last";
    public static readonly EditTariffAllInChargesButton = "newairfreightcostcomponent button[id^='Edit']:last";

    public static TariffSurcharge(number: number): string{
        return "Tariff_Surcharge" + number.toString() + "Id";
    }

    public static TariffSurchargeMeasurement(number: number): string{
        return "Tariff_Surcharge" + number.toString() + "UOM";
    }
}