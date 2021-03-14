export class QuoteSelectors {
     //#region Create
     public static readonly CreateQuote = '#CreateQuote';
     public static readonly NewQuote = '#NewQuote';
     public static readonly QuoteShipper = '#Quote_ShipperId';
     public static readonly QuoteConsignee = '#Quote_ConsigneeId';
     public static readonly QuoteFromPort = '#Quote_FromPortId';
     public static readonly QuoteToPort = '#Quote_ToPortId';
     public static readonly QuoteCustomerType = '#Quote_QuoteCustomerTypeCode';
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
     //#region Quotation
     public static readonly Quotation = '#QuoteBQuotation';
     public static readonly QuoteEventsTab = '#QuoteTHEvents';
     public static readonly EditQuotationIntroduction = '#EditSection1';
     public static readonly AddDataField = '#AddDataField';
     public static readonly QuotationEditOkButton = '#OkButton';
     public static readonly SaveQuotation = '#Savee';
     public static readonly SendOption = '#SendOption';
     public static readonly SendToCustomer = '#SendToCustomer';
     public static readonly EmailSearchTextBox = 'input[id^=EmailSearchTextBox_TextArea]';
     public static readonly SendMessageButton = '#SendMessagebtn';
     public static readonly QuoteEventNote = '#Quote_EventNote';
     //#endregion

     //#region Copied page
     public static readonly CopiedQuoteEventsTab = "li[id^='QuoteTHEvents_']";
     public static readonly CopiedQuotePartnersTab = "li[id^='QuoteTHPartners_']";
     public static readonly CopiedQuotePackagesTab = "li[id^='QuoteTHPackages_']";
     public static readonly CopiedQuoteRoutingTab = "li[id^='QuoteTHRoutings_']";

     //#endregion
     //#region Regex selectors
     public static PackageLineSelector(Selector: string, lineNumber: number): string {
          return lineNumber > 0 ? Selector + '_' + lineNumber : Selector;
     }

     public static QuotationDataFields(dataField: string): string {
          return "td[data-cy^=Quote_" + dataField + "]";
     }

     public static QuotationActionsButton(action: string): string {
          action = action.replace(/\s/g, "");
          return "#QuoteB" + action;
     }
     //#endregion
}