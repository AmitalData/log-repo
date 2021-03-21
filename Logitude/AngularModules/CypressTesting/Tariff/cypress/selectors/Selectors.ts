export class TariffSelectors
{
    //#region Buttons Without Id 
    public static readonly NewFreightCostToggleButton = "div[id^='NEWTARIFF']:first";
    public static readonly NewSurchargeCostToggleButton = "div[id^='NEWTARIFF']:last";
    public static readonly NewFreightCostToggleMenuButton = "div[id^='NEWTARIFF']:first .ToggleButtonMenu button";
    public static readonly NewSurchargeCostToggleMenuButton = "div[id^='NEWTARIFF']:last .ToggleButtonMenu button";
    public static readonly EditTariffAllInChargesButton = "newairfreightcostcomponent button[id^='Edit']:last";
    public static readonly TariffActionsToggleButton = ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ToggleButton img";
    public static readonly TariffActionsToggleButtonItem = ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ToggleButton button";
    public static readonly TariffVersionHistoryComboBox = ".TabControlBody[id!='ApplicationSession'] .MediaFillAbsolute:visible .ComboBox";
    public static readonly TariffUpdateSurchargeCheckBox = "checkbox[id^='IsSurchargeChecked']"
    public static readonly PriceCheckResultTableRow = ".LogitudeScrollViewer.LogitudeSmallScrollViewer > table > tr";
    public static readonly TariffNumberShortTitleDiv = ".LogitudeWindow:last .ShortTitleDiv:first";
    public static readonly TariffNumberShortTitle = ".ShortTitleMainControl:last .ShortTitleDiv:first";
    public static readonly PriceCheckFreightResult = "[data-cy='FreightPriceValue']";
    public static readonly PriceCheckSurchargeResult = "[data-cy='SurchargePriceValue']";
    public static readonly FreightTariffLink = "[data-cy='FreightTariffLink']";
    public static readonly SurchargeTariffLink = "[data-cy='SurchargeTariffLink']";
    public static readonly PriceCheckWholePrice = "[data-cy='WholePrice']";

    public static readonly PriceCheckSearch = ".SearchButton";
    public static readonly TariffActionsMenu = ".ToggleButton"
    public static readonly ToggleButtonMenu = ".ToggleButtonMenu"
    public static readonly PriceCheckQuery = ".QueryLink"
    public static readonly SearchButton = ".SearchButton"
    public static readonly InputUpload = "input.upload"
    //#endregion

    //#region TextBox
    public static readonly TariffName = "#Tariff_Name";
    public static readonly TariffNotes = "#Tariff_Notes";
    public static readonly TariffLineTransitTime = "#TariffLine_TransitTime";
    public static readonly TariffLineNotes = "#TariffLine_Notes";
    public static readonly TariffLineMinPrice = "#TariffLine_MinPrice";
    public static readonly TariffContractNumber = "#Tariff_ContractNumber";
    public static readonly ShippingLineSCACCode="#ShippingLine_SCACCode"
    public static readonly ShippingLineCode="#ShippingLine_Code"
    public static readonly ShippingLineName = "#ShippingLine_EnglishName"
    public static readonly TariffChargeableWeight = "#TariffLine_Weight";
    //#endregion

    //#region LogLov
    public static readonly TariffSeller = "#Tariff_SellerId";
    public static readonly TariffProduct = "#Tariff_TariffProductId";
    public static readonly TariffLineFromPort = "#TariffLine_OriginPortId";
    public static readonly TariffLineToPort = "#TariffLine_DestinationPortId";
    public static readonly TariffCurrency = "#Tariff_CurrencyId";
    public static readonly TariffUpdatePortSelector = "#Tariff_PortId"
    public static readonly TariffVersionAllInChargeType = "input[id^='TariffVersionAllInCharge_ChargesTypeId']:last";
    //#endregion

    //#region Date
    public static readonly TariffStartDate = "#date_Tariff_StartDate";
    public static readonly TariffLineStartDate = "#date_TariffLine_StartDate"
    public static readonly TarifflLineStartDate = "#date_TarifflLine_StartDate"
    public static readonly TariffUpdateStartDate = "#StartDate"
    public static readonly PriceCheckDate = "#Date";
    //#endregion

    //#region Button
    public static readonly SaveTariff = "#Tariff-Save";
    public static readonly AddButton  = "#Add" ; 
    public static readonly FromPort = "#FromPort"
    public static readonly ToPort = "#ToPort"
    public static readonly TariffSearchIcon = "#searchicon_Tariff_SellerId"
    //#endregion

    //#region Contains
    public static readonly ContainsAir = "Air"
    public static readonly ContainsOceanFCL ="Ocean FCL"
    public static readonly ContainsOceanLCL="Ocean LCL"
    public static readonly ContainsTariff="Tariff"
    public static readonly ContainsCopyIntoNewVersion = "Copy into new Version";
    public static readonly ContainsVersionHistory = "Version History"
    public static readonly ContainsApproveVersion ="Approve Version"
    public static readonly ContainsUpdateSurcharges = "Update Surcharges"
    public static readonly ContainsAdd = "Add"
    public static readonly ContainsClose = "Close"
    public static readonly ContainsSearch="Search"
    public static readonly ContainsGeneral = "General"
    public static readonly ContainsUpdate = "Update"
    public static readonly ContainsCancel = "Cancel"
    public static readonly ContainsTariffs = "Tariffs"
    public static readonly ContainsActions = "Actions"
    public static readonly ContainsNewShippingLine = "New Shipping Line"
    public static readonly ContainsTariffFailedError = "Create Tariff Failed"
    public static readonly ContainsUniqueSellerError = "Tariff surcharge seller should be unique"
    public static readonly ContainsCode = "Code:"
    public static readonly Span = 'span'
    public static readonly Last = ":last"
    public static readonly Binary = "binary"
    public static readonly Disabled = ":disabled"
    public static readonly DownArrow = "{downarrow}"
    public static readonly ExcelType = "application/vnd.ms-excel"
    public static readonly ContainsViewTariff = "View Tariff";
    //#endregion

    
    
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

    public static QueriesSurcharge(SurchargeName: string): string{
        return "hyperlinkquery[data-cy^=Surcharge_" + SurchargeName + "]";
    }

    public static GridFitstRow(): string{
        return "div[id^='LogGrid_'][id$='row0']";
    }

    public static ContainsBackButton(mode:string): string{
        return mode + " Surcharges Cost"
    }
}