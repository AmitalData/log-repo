export class QuoteSelectors {
     public static readonly QuotesTab = '#GeneralMHQuotes';
     public static readonly QuoteSave = '#Quote-Save';
     //#region  Create
     public static readonly CreateQuote = '#CreateQuote';
     public static readonly NewQuote = '#NewQuote';
     public static readonly QuoteShipper = '#Quote_ShipperId';
     public static readonly QuoteConsignee = '#Quote_ConsigneeId';
     public static readonly QuoteFromPort = '#Quote_FromPortId';
     public static readonly QuoteToPort = '#Quote_ToPortId';
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
     //#region Quick Search
     public static readonly QuoteSearch = '#Quote_Search';
     public static readonly QuoteSearchParent = 'quicksearchtextbox';
     public static readonly QuoteSearchParentClass = '.LogitudeQuickSearchTextBox';
     //#endregion
}