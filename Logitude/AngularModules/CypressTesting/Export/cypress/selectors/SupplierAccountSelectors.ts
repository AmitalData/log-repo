export class SupplierAccountSelectors {
   
    public static readonly SearchField = "searchtextbox input[id*=SearchFieldsId]";
    public static readonly FirstDeclaration = "[id*='LogGrid'][id*='row0']:first";
    // Supplier Accounts tab - can be in navigation sidebar or tab header
    public static readonly SupplierAccounts = "#CustomsDeclarationTHSupplierAccounts, a:contains('חשבונות ספק'), li:contains('חשבונות ספק'), [id*='SupplierAccount']";
    // Add button - based on actual HTML: button#Add_3 with class LogitudeIconButton
    // Use declarationsupplierinvoicetabcomponent to target the correct Add button for supplier accounts
    public static readonly AddButtonAccount = "declarationsupplierinvoicetabcomponent img[src='./Images/Buttons/Add.png'], declarationsupplierinvoicetabcomponent button[id^='Add_'].LogitudeIconButton, declarationsupplierinvoicetabcomponent button[id^='Add_'], button[id^='Add_'].LogitudeIconButton, button[id^='Add_']";
    
    // Popup/Modal dialog that appears after clicking Add button
    // Look for window, popup, or the "חשבונית חדשה" (New Invoice) title
    public static readonly SupplierInvoicePopup = ".LogitudeWindow, .MessageWindow, [id*='SupplierInvoice'][class*='Window'], ng-component:contains('חשבונית חדשה'), div:contains('חשבונית חדשה'), [class*='Window']:visible, [class*='popup']:visible, [class*='modal']:visible";

    public static readonly AccountTypeCode = '#Customs\\.SupplierInvoice_AccountTypeCode';
    public static readonly IssueDate = '#date_Customs\\.SupplierInvoice_IssueDate';
    public static readonly InvoiceCurrencyTypeCode = '#Customs\\.SupplierInvoice_InvoiceCurrencyTypeCode';
    public static readonly IncotermCode = '#Customs\\.SupplierInvoice_IncotermCode';
    public static readonly InvoiceNumber = '#Customs\\.SupplierInvoice_InvoiceNumber';
    public static readonly InvoiceAmount = '#Customs\\.SupplierInvoice_InvoiceAmount';
    public static readonly IsPreference = '#Customs\\.SupplierInvoice_IsPreference';
    public static readonly PreferenceDocumentTypeCode = '#Customs\\.SupplierInvoice_PreferenceDocumentTypeCode';
    public static readonly VendorId = '#Customs\\.SupplierInvoice_VendorId';

    // Transport data fields
    public static readonly TransportCurrencyTypeCode = '#edit-log-grid_0_30_0_0';
    public static readonly TransportAmount = '#edit-log-grid_0_30_1_0';

    public static readonly ButtonSaveSupplierInvoice = '#SaveSupplierInvoice';
    public static readonly ButtonDeleteSupplierInvoice = 'button[id^="Delete_"]';
    public static readonly Yes = '.RedButton';
    public static readonly SupplierAccountFirstRow = "#row0";

    // Customs detail row fields - based on actual HTML structure
    // index="1": edit-log-grid_0_20_0_0 (row number)
    // index="2": edit-log-grid_0_20_1_0 (ItemNo)
    // index="3": edit-log-grid_0_20_2_0 (ItemDescription)
    // index="4": edit-log-grid_0_20_3_0 (Item)
    // index="5": edit-log-grid_0_20_4_0 (TradeAgreementCode - dropdown)
    // index="6": edit-log-grid_0_20_5_0 (UnitsQuantity)
    // index="7": edit-log-grid_0_20_6_0 (UnitType - dropdown)
    // index="8": edit-log-grid_0_20_7_0 (ValueInForeignCurrency)
    // index="0": edit-log-grid_0_20_8_0 (skipped/other field)
    // index="1": edit-log-grid_0_20_9_0 (action buttons column - not a field)
    public static readonly AddItemButton = "button[id^=Add]:visible:last";
    
    public static readonly ItemNo = '#edit-log-grid_0_20_1_0';
    public static readonly ItemDescription = '#edit-log-grid_0_20_2_0';
    public static readonly Item = '#edit-log-grid_0_20_3_0';
    public static readonly TradeAgreementCode = '#edit-log-grid_0_20_4_0';
    public static readonly UnitsQuantity = '#edit-log-grid_0_20_5_0';
    public static readonly UnitType = '#edit-log-grid_0_20_6_0';
    public static readonly ValueInForeignCurrency = '#edit-log-grid_0_20_7_0';
    // index="0": edit-log-grid_0_20_8_0 (OriginCountryCode - dropdown)
    public static readonly OriginCountryCode = '#edit-log-grid_0_20_8_0';

}
