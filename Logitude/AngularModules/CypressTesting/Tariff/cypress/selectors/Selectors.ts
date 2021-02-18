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
    public static readonly TariffVersionHistoryComboBox = ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ComboBox";
    public static readonly PriceCheckDate = "#Date";
    public static readonly PriceCheckSearch = ".SearchButton";
    
    public static readonly AddButton  = "#Add" ; 
    public static readonly TariffLineStartDate = "#date_TariffLine_StartDate"
    public static readonly TariffUpdateStartDate = "#StartDate"
    public static readonly TariffUpdatePortSelector = "#Tariff_PortId"
    public static readonly FromPort = "#FromPort"
    public static readonly ToPort = "#ToPort"
    public static readonly TariffUpdateSurchargeCheckBox = "checkbox[id^='IsSurchargeChecked']"
    public static readonly ShippingLineSCACCode="#ShippingLine_SCACCode"
    public static readonly ShippingLineCode="#ShippingLine_Code"
    public static readonly ShippingLineName = "#ShippingLine_EnglishName"
    public static readonly TariffActionsMenu = ".ToggleButton"
    public static readonly TariffChargeableWeight = "#TariffLine_Weight";
    public static readonly TariffEditBackbutton = "#EditBackbutton";
    public static readonly PriceCheckQuery = ".QueryLink"
    public static readonly SearchButton = ".SearchButton"
    // public static readonly

    
    public static TariffSurcharge(number: number): string{
        return "#Tariff_Surcharge" + number.toString() + "Id";
    }

    public static TariffLineSurchargePrice(number: number): string{
        return "#TariffLine_Surcharge" + number.toString() + "Price";
    }

    public static TariffLineStepPrice(number: number): string{
        return "#TariffLine_Step" + number.toString() + "Price";
    }

    public static TariffUpdatePrice(number: number): string{
        return "#Price" + number.toString();
    }

    public static TariffLineEditButton(lineNumber: number): string{
        return ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible #row" + lineNumber.toString() + " button[id^='Edit']";
    }

    public static TariffVersionHistoryComboBoxItem(versionNumber: number): string{
        return ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ComboBox .ComboBoxItem span[title^='Version " + versionNumber.toString() + "']";
    }

    public static PriceCheckQuantity(quantityNumber: number): string{
        return "#Quantity" + quantityNumber.toString();
    }
}