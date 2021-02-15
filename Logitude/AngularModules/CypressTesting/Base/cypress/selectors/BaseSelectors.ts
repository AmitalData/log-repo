import { RegexSelectors } from './RegexSelectors';

export class BaseSelectors extends RegexSelectors {
    //#region General menus
    public static readonly OperationsMenu = '#GeneralMHOperations';
    public static readonly AccountingMenu = '#GeneralMHAccounting';
    public static readonly CustomersMenu = '#GeneralMHCustomers';
    public static readonly TicketsMenu = '#GeneralMHTicket';
    //#endregion
    //#region Buttons
    public static readonly RedButton = '.RedButton';
    public static readonly Button = '.Button';
    public static readonly Row0 = '#row0';
    public static readonly UploadDocumentdbtn = "#UploadDocumentdbtn";
    public static readonly OKBtn = "#OKBtn";
    public static readonly SaveWizard = "#SaveWizard";
    public static readonly Backbutton = '#EditBackbutton';
    //#region Contains
    public static readonly ContainsApplytoall = 'Apply to all';
    public static readonly ContainsOK = 'OK';
    //#endregion
    //#region general
    public static readonly ToggleButtonClass = '.ToggleButton';
    public static readonly label = "label"
    public static readonly button = "button"
    //#endregion
    //#endregion

    public static readonly FirstElementInList = 'ul > li';
}