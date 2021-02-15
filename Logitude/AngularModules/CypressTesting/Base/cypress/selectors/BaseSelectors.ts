import { RegexSelectors } from './RegexSelectors';

export class BaseSelectors extends RegexSelectors {
    //#region General menus
    public static readonly OperationsMenu = '#GeneralMHOperations';
    public static readonly AccountingMenu = '#GeneralMHAccounting';
    public static readonly CustomersMenu = '#GeneralMHCustomers';
    //#endregion
    //#region Buttons
    public static readonly TicketsMenu = '#GeneralMHTicket';
    public static readonly RedButton = '.RedButton';
    public static readonly Button = '.Button';
    public static readonly Row0='#row0';
    public static readonly UploadDocumentdbtn="#UploadDocumentdbtn";
    public static readonly OKBtn = "#OKBtn";
    public static readonly SaveWizard = "#SaveWizard";
    public static readonly AddButton = "#Add";
    public static readonly SpanElement = "span";
    public static readonly DivElement = "div";
    public static readonly FirstRecentEntityItem = ".RecentEntityItem:first";
}