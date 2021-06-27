export class AirLineSelectors {

    public static readonly MaintenanceItem = "#MaintenanceItemMTAL"
    public static readonly SaveCloseButton = "#Airline-SaveClose";
    public static readonly SaveButton = "#Airline-Save";
    public static readonly ImportAirLine_AddButton = "[data-cy='Import_Airline']"
    public static readonly AddNewAirLine = "[data-cy='Add_Airline']"

    public static readonly Code = "#Airline_Code";
    public static readonly ICAO = "#Airline_ICAO";
    public static readonly Name = "#Airline_EnglishName";
    public static readonly Prefix = "#Airline_Prefix";
    public static readonly LocalName = "#Airline_LocalName";
    public static readonly Notes = "#Airline_Remark";

    public static readonly AddressesTab = "#AirlineTHAddresses"
    public static readonly EditAddressButton = "#Edit"
    public static readonly AddressName = "#Address_Name"
    public static readonly AddressCity = "#Address_City"
    public static readonly AddressCountryId = "#Address_CountryId"
    public static readonly AddressStateId = "#Address_StateId"

    public static readonly TariffTranslationsTab = "#AirlineTHTariffTranslations"
    public static readonly AddTranslation = "#addTranslation"
    public static readonly TariffPartnerCode = "#TariffCarrierTranslation_PartnerCode"
    public static readonly TariffPort = "#TariffCarrierTranslation_PortId"

    public static readonly SurchargeTariffTab = "#AirlineTHSurchargeTariff";
    public static readonly AddSurchargeTariff = "[data-cy='AddSurchargeTariff']"
    public static readonly SurchargeTarrifFromDate = "#date_TarrifHeader_FromDate"
    public static readonly SurchargeTarrifToDate = "#date_TarrifHeader_ToDate"
    public static readonly AddTariffCharge = "[data-cy='AddTariffCharge']"
    public static readonly TarrifCharge_ChargesTypeId = "#TarrifCharge_ChargesTypeId"
    public static readonly TarrifCharge_UnitPrice = "#TarrifCharge_UnitPrice"

    public static readonly AdaptationsTab = "#AirlineTHAdaptations"
    public static readonly AddSpecialHandlingCode = "[data-cy='AddSpecialHandlingCode']"
    public static readonly AWBSpecialHandlingCode_Code = "#AWBSpecialHandlingCode_Code"
    public static readonly AWBSpecialHandlingCode_Name = "#AWBSpecialHandlingCode_Name"

    public static readonly GeneralTab = "#AirlineTHGeneral"
    public static readonly InactiveAirline = "#Airline_InActive"

    public static readonly EventsTab = "#AirlineTHEvents"

    public static readonly CodeDigitCount = 2
    public static readonly ICAODigitCount = 3
    public static readonly TariffCodeDigitCount = 10
}
