export class ShippingLineSelectors {

  public static readonly MaintenanceItem = "#MaintenanceItemMTSL"
  public static readonly SaveCloseButton = "#ShippingLine-SaveClose";
  public static readonly SaveButton = "#ShippingLine-Save";
  public static readonly Import = "[data-cy='Import']"
  public static readonly AddNewShippingLine = "[data-cy='Add_ShippingLine']"

  public static readonly Name = "#ShippingLine_EnglishName";
  public static readonly Code = "#ShippingLine_Code";
  public static readonly Notes = "#ShippingLine_Remark";
  public static readonly SCACCode = "#ShippingLine_SCACCode"

  public static readonly INTTRATab = "#ShippingLineTHINTTRA"
  public static readonly INTTRARegistrationNotes = "#ShippingLine_INTTRARegistrationNotes"
  public static readonly ImportShippingLine_AddButton = "[data-cy='Import_ShippingLine']"

  public static readonly AreasTab = "#ShippingLineTHAreas"
  public static readonly AddArea = "#addArea"
  public static readonly AreaName = "#CarrierArea_Name"
  public static readonly AreaDescription = "#CarrierArea_Description"

  public static readonly ChoosePortButton = "[data-cy='ChoosePort_CarrierArea']"
  public static readonly CarrierAreasPort_PortId = "#CarrierAreasPort_PortId"
  public static readonly Add_CarrierAreasPort = "[data-cy='Add_CarrierAreasPort']"
  public static readonly Close_CarrierAreasPort = "[data-cy='Close_CarrierAreasPort']"

  public static readonly ChooseCountryPortButton = "[data-cy='ChooseCountry_CarrierArea']"
  public static readonly CarrierAreasPort_CountryId = "#CarrierAreasPort_CountryId"
  public static readonly AddCountry_CarrierAreasPort = "[data-cy='AddCountry_CarrierAreasPort']"
  public static readonly CloseCountry_CarrierAreasPort = "[data-cy='CloseCountry_CarrierAreasPort']"

  public static readonly TariffTranslationsTab = "#ShippingLineTHTariffTranslations"
  public static readonly AddTranslation = "#addTranslation"
  public static readonly TariffPartnerCode = "#TariffCarrierTranslation_PartnerCode"
  public static readonly TariffPort = "#TariffCarrierTranslation_PortId"

  public static readonly GeneralTab = "#ShippingLineTHGeneral"
  public static readonly EventsTab = "#ShippingLineTHEvents"
  public static readonly AddressesTab = "#ShippingLineTHAddresses"
  public static readonly InActiveShippingLineCheckBox = "#ShippingLine_InActive"

  public static readonly IsINTTRACheckBox = '#ShippingLine_IsINTTRARegistered'

  public static readonly CodeDigitCount = 4
  public static readonly TariffCodeDigitCount = 10
}
