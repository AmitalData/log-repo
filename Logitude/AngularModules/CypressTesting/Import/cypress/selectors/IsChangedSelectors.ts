export class IsChangedSelectors {

    public static readonly GeneralMHDeclarationsTab = "#GeneralMHDeclarations"
    public static readonly IsChangedFilterOpen ="td img[src*=FiltersOpen]" //'[src="./Images/FiltersOpen.png"]';
    public static readonly IsChangedDeclarationStatus = '#Customs\\.Declaration_TextValue';
    //public static readonly IsChangedDeclarationStatus = '#Customs\\.Declaration_TextValue_1'; - תקין לפני התיקון של ה - URL
    public static readonly IsChangedFilterClose ="td img[src*=FiltersClose]" //'[src="./Images/FiltersClose.png"]';
    //public static readonly IsChangedFilterCust = '.InputDiv.eq(2)'
    public static readonly IsChangedFilterCust = '#Customs\\.Declaration_TextValue_1'
    public static readonly IsChangedDeclaration1 ="logitude-grid table tr:eq(1)";
    //public static readonly IsChangedDeclaration1 = '#LogGrid_0_0row0';
    public static readonly IsChangedGeneral = '#CustomsDeclarationTHGeneral';
    public static readonly IsChangedDeclarationOfficeCode = '#Customs\\.Declaration_DeclarationOfficeCode';
    public static readonly IsChangedSaveButton = '#CustomsDeclaration-Save';
    public static readonly IsChangedDeclarationPaymentButton = '#CustomsDeclarationBDeclarationPayment';
    //public static readonly IsChangedSendButton = '#CustomSendOptionsComponent_1';
    public static readonly IsChangedCargoDescription = '#Customs\\.Consignment_CargoDescription';
    //public static readonly IsChangedQuantity = '.TextTrimming';
    public static readonly IsChangedSendButtonD = '#CustomSendOptionsComponent_1';
    public static readonly DisbleBox = '\.DisbleBox';
    public static readonly SearchField = "searchtextbox input[id*=SearchFieldsId]"




    //public static readonly IsChangedquantity = '#edit-log-gridRows_0_0';
    //public static readonly IsChangedquantity = '#edit-log-grid_0_30_2_0';
    //public static readonly IsChangedquantity = 'div#edit-log-grid_0_10_2_0.LogCellTemplate';
    public static readonly IsChangedquantity = '.LogCellTemplate:eq(2)'



    public static readonly Scen = 'combobox table tr:eq(3)';
    //public static readonly Scen1 ="td img[src*=ToggleIcon]"
    public static readonly Scen1 ='senddeclarationtastcasecomponent combobox div table td img';
    public static readonly Save = '.RedButton'
    public static readonly ErrorsMsg = '.ErrorsDiv'

    public static readonly Approve = '.LogitudeWindow table Button'//תפריט תשובה לתיק
    public static readonly Approve7 = '.Button.eq(7)'//תפריט תשובה לתיק


 
    
   
}