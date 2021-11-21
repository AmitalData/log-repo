import { RegexSelectors } from '../selectors/RegexSelectors';

export class BankAccountSelectors extends RegexSelectors {

    public static readonly BankAccountAccountNumber = "#BankAccountLite_AccountNumber"
    public static readonly BankAccountBankCode = "#BankAccountLite_BankCode"
    public static readonly BankAccountBranchNumber = "#BankAccountLite_BranchNumber"
    public static readonly BankAccountCurrency = "#BankAccountLite_CurrencyId"
    public static readonly BankAccountName = "#BankAccountLite_EnglishName"
    public static readonly BankAccountLocalName = "#BankAccountLite_LocalName"
    public static readonly BankAccountSaveButton = "#BankAccountLite-Save"
    public static readonly InActiveBankAccountCheckBox = "#BankAccountLite_Inactive"
    public static readonly BankAccountSaveCloseButton= "#BankAccountLite-SaveClose"
    public static readonly BankAccountEventsTab = "#BankAccountLiteTHEvents"
    public static readonly MaintenanceItemBankAccount = "#MaintenanceItemMTBL"
    public static readonly CodeDigitCount = 10
}