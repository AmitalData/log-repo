export class AddItemSelectors {
   
    public static readonly SearchField = "searchtextbox input[id*=SearchFieldsId]";
    public static readonly FirstDeclaration ="#LogGrid_0_0row0";
    public static readonly AddButtonInvoice = "declarationsupplierinvoicetabcomponent img[src='./Images/Buttons/Add.png']";
    public static readonly EditButtonInvoice = "declarationsupplierinvoicetabcomponent img[src='./Images/Buttons/Edit.png']";
    //public static readonly AddItemButton = "img[src='./Images/Buttons/Add.png']:visible:last";
    public static readonly AddItemButton = "button[id^=Add]:visible:last";
    public static readonly DeleteButton = "button[id^=Delete]:visible:last";


    //public static readonly ItemNo = 'div.MediaFill div.MediaStretch ng-component table tr td logitude-edit-grid:eq(1) div.LogCellTemplateInnerChild.TextTrimming:eq(1)
    public static readonly ItemNo = '#textboxdiv_Customs\\.SupplierInvoice_ItemCode';
    public static readonly ItemDescription = '#edit-log-grid_0_20_2_0';
    public static readonly Item = '#edit-log-grid_0_20_3_0';
    //public static readonly TradeAgreementCode = '#LogLov_Customs\\.SupplierInvoice_TradeAgreementCode';
    public static readonly TradeAgreementCode = '.LogCellTemplate:eq(38)';
    public static readonly ProtocolCode  = '#edit-log-grid_0_20_5_0';
    public static readonly UnitsQuantity  = '#edit-log-grid_0_20_6_0';
    public static readonly UnitType  = '#edit-log-grid_0_20_7_0';
    //public static readonly UnitType  = '#LogLov_Customs\\.SupplierInvoice_InvoiceQuantityType';
    public static readonly ValueInForeignCurrency  = '#edit-log-grid_0_20_8_0';
    //public static readonly OriginCountry  = '#Customs\\.SupplierInvoice_OriginCountryCode';
    public static readonly OriginCountry  = '#edit-log-grid_0_20_9_0';


    public static readonly Edit = '#Edit'
    public static readonly TypeCode  = '#LogLov_Customs\\.SupplierInvoiceItemsMod_TypeCode';
    public static readonly CurrencyType = '.LogCellTemplate:eq(46)';
    public static readonly Amount  = '.LogCellTemplate:eq(47)';
    public static readonly Save = '#SaveSupplierInvoice'


    public static readonly ButtonSaveSupplierInvoice = '#SaveSupplierInvoice'
    public static readonly ButtonDeleteSupplierInvoice = '#Delete_3.LogitudeIconButton'
    public static readonly Yes = '.RedButton'
    public static readonly SupplierInvoiceFirstRow ="#row0";


    
    
}
