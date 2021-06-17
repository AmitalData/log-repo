import { RegexSelectors } from '../selectors/RegexSelectors';

export class CurrencySelectors extends RegexSelectors {

  public static readonly CurrencyMaintenanceItem = "#MaintenanceItemMTCR"
  public static readonly CurrencySaveCloseButton = "#Currency-SaveClose";
  public static readonly CurrencySaveButton = "#Currency-Save";
  public static readonly CurrencyName = "#Tenant_CurrencyId";
  public static readonly CurrencyLocalName = "#Currency_LocalName";
  public static readonly CurrencyExchangeRate = "#CurrencyRate";
  public static readonly CurrencyFirstRow = "div[id$='row0']"
  public static readonly InActiveCurrencyCheckBox = "#Currency_InActive"
  public static readonly CurrencyEventsTab = "#CurrencyTHEvents"
  public static readonly AccountingTab = "#CurrencyTHAccounting"
  public static readonly AccountingExternalID = "input[id^='Currency_AccountingExternalCode']"
  public static readonly MinRandomNumber = 0
  public static readonly MaxRandomNumber = 21
  public static readonly MinCounterCodeRandomNumber = 1
  public static readonly MaxCounterCodeRandomNumber = 10000
  public static readonly CodeDigitCount = 10
}