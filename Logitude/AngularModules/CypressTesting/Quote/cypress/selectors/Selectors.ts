export class QuoteSelectors {
     //#region Create
     public static readonly CreateQuote = '#CreateQuote';
     public static readonly QuoteCharges = '#QuoteTHCharges';
     public static readonly NewQuote = '#NewQuote';
     public static readonly QuoteShipper = '#Quote_ShipperId';
     public static readonly QuoteConsignee = '#Quote_ConsigneeId';
     public static readonly QuoteFromPort = '#Quote_FromPortId';
     public static readonly QuoteToPort = '#Quote_ToPortId';
     public static readonly QuoteCustomerType = '#Quote_QuoteCustomerTypeCode';
     public static readonly QuoteAddCharges = '#AddCharges';
     public static readonly LogLovQuoteCustomerType = "#LogLov_Quote_QuoteCustomerTypeCode"
     public static readonly RoutingRadioButton = "#RoutingRadio0"
     public static readonly TransitTime = "#Quote_TransitTime"
     public static readonly DetailsTab = "#QuoteTHDetails"
     public static readonly QuotePartnersTab = "#QuoteTHPartners"
     //#endregion
     //#region Quick Search
     public static readonly QuoteSearch = '#Quote_Search';
     public static readonly QuoteSearchParent = 'quicksearchtextbox';
     public static readonly QuoteSearchParentClass = '.LogitudeQuickSearchTextBox';
     //#endregion
     //#region Update
     public static readonly QuotesTab = '#GeneralMHQuotes';
     public static readonly QuoteSave = '#Quote-Save';
     //#endregion
     //#region Package
     public static readonly PackagesTab = '#QuoteTHPackages';
     public static readonly AddPackage = '#AddPackage';
     public static readonly PackageType = '#QuotePackage_PackageTypeId';
     public static readonly PackageQuantity = '#QuotePackage_Quantity';
     public static readonly PackageLength = '#QuotePackage_Length';
     public static readonly PackageWidth = '#QuotePackage_Width';
     public static readonly PackageHeight = '#QuotePackage_Height';
     public static readonly PackageVolume = '#QuotePackage_Volume';
     public static readonly PackageWeight = '#QuotePackage_GrossWeight';
     public static readonly OkAddPackage = '#OkAddPackage';
     public static readonly DeletePackage = "Button[id^='Delete']";
     //#endregion

     //#region 
     public static readonly ContainerType = '#Quote_PackageType1Id';
     public static readonly ContainerQuantity = '#Quote_PackageType1Quantity';
     //#endregion

     //#region Quotation
     public static readonly Quotation = '#QuoteBQuotation';
     public static readonly QuoteEventsTab = '#QuoteTHEvents';
     public static readonly EditQuotationIntroduction = '#EditSection1';
     public static readonly AddDataField = '#AddDataField';
     public static readonly QuotationEditOkButton = '#OkButton';
     public static readonly SaveQuotation = '#Savee';
     public static readonly SendOption = '#SendOption';
     public static readonly SendToCustomer = '#SendToCustomer';
     public static readonly SendMessageButton = '#SendMessagebtn';
     public static readonly QuoteEventNote = '#Quote_EventNote';
     //#endregion

     //#region Copied page
     public static readonly CopiedQuoteEventsTab = "li[id^='QuoteTHEvents_']";
     public static readonly CopiedQuotePartnersTab = "li[id^='QuoteTHPartners_']";
     public static readonly CopiedQuotePackagesTab = "li[id^='QuoteTHPackages_']";
     public static readonly CopiedQuoteRoutingTab = "li[id^='QuoteTHRoutings_']";
     //#endregion

     //#region Charges 
     public static readonly QuoteChargeCostCurrency = "#QuoteCharge_CostCurrencyId";
     public static readonly QuoteChargeCostExchangeRate = "#QuoteCharge_CostExchangeRate";
     public static readonly QuoteCancelAddCharges = "#CancelAddCharges";
     public static readonly ContaintsQuote = "Quotes";
     public static readonly DeleteAllChargesButton = "#DeleteAll";
     public static readonly AddChargesButton = "#AddCharges";
     public static readonly ChargesType = "#QuoteCharge_ChargesTypeId";
     public static readonly ChargeSaleCurrency = "#QuoteCharge_SaleCurrencyId";
     public static readonly SameAsCostCurrencyComboBox = "#ComboBoxItem_200";

     //#endregion

     //#region Routing 
     public static readonly QuoteRoutingsTab = "#QuoteTHRoutings"
     public static readonly IncludePickUpCheckBox = "#Quote_IncludePickUp"
     public static readonly IncludeDelivery = "#Quote_IncludeDelivery"
     public static readonly ToAddressCity = "#Quote_ToAddressCity"
     public static readonly ToAddressCountryId = "#Quote_ToAddressCountryId"
     //#endregion
     public static readonly BuildShipmentButton = "#QuoteBBuildShipment"
     public static readonly ShipmentLevelDirectRadio = "#ShipmentLevelRadio_0D"

     public static readonly CancelQuery = "[data-cy='CancelQuery']"
     public static readonly AcceptQuery = "[data-cy='AcceptQuery']"
     public static readonly AllQuoteQuery = "[data-cy='AllQuoteQuery']"

     //#region Regex selectors
     public static PackageLineSelector(Selector: string, lineNumber: number): string {
          return lineNumber > 0 ? Selector + '_' + lineNumber : Selector;
     }

     public static QuotationDataFields(dataField: string): string {
          return "td[data-cy^='Quote_" + dataField + "']";
     }

     public static QuotationActionsButton(action: string): string {
          action = action.replace(/\s/g, "");
          return "#QuoteB" + action;
     }
     //#endregion
}