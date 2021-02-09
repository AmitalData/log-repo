import { RegexSelectors } from './RegexSelectors';

export class BaseSelectors extends RegexSelectors {
    //#region General menus
    public static readonly OperationsMenu = '#GeneralMHOperations';
    public static readonly AccountingMenu = '#GeneralMHAccounting';
    public static readonly CustomersMenu = '#GeneralMHCustomers';
    //#endregion
    //#region Buttons
    public static readonly RedButton = '.RedButton';
    public static readonly Button = '.Button';
    //#endregion
    public static readonly Row0 = '#row0';
    public static readonly UploadDocumentdbtn = '#UploadDocumentdbtn';
}