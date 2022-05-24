export class CommonSelectors {
    //#region Reset password
    public static readonly ErrorList = '#errorsList';
    public static readonly SubmitButton = '#cmdSubmit';
    public static readonly CurrentPassword = '#CurrentPassword';
    public static readonly Password = '#Password';
    public static readonly ConfirmPassword = '#ConfirmPassword';
    //#endregion
    //#region Customer
    public static readonly NewCustomer = '#NewButton_Customer';
    public static readonly CustomerCompanyName = '#Address_Name';
    public static readonly CustomerCity = '#Address_City';
    public static readonly CustomerCountry = '#Address_CountryId';
    public static readonly CustomerState = '#Address_StateId';
    public static readonly CustomerPhoneNumber ="#Address_PhoneNumber"
    public static readonly CustomerFaxNumber="#Address_FaxNumber"
    public static readonly AddCustomer = '#Ok-AddCustomer';
    public static readonly CustomerBillingTab = '#CustomerTHBilling';
    public static readonly EnableConsolidationInvoices = '[for="Customer_EnableConsolidationInvoices"]';
    public static readonly CustomerSave = '#Customer-Save';
    public static readonly CustomerVatNumber = '#Address_VatNumber';
    //#endregion
    //#region Login 
    public static readonly Email="#Email"
    public static readonly LoginButton="#cmdLogin"
    public static readonly ForgotYourPasswordLink="Forgot your password?"
    public static readonly  ResetPasswordLinkSentMessage="#message";
    //#endregion
}