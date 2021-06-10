import { RegexSelectors } from '../selectors/RegexSelectors';

export class FBLStockSelectors extends RegexSelectors {

  public static readonly FBLStockMaintenanceItem = "#MaintenanceItemMTFS"


  public static readonly FBLStockStartNumber = "#StartNumber";
  public static readonly FBLStockEndNumber = "#EndNumber";
  public static readonly FBLStockByEndNumber = "#ByEndNumberRadio";
  public static readonly FBLStockByAmount = "#ByAmountRadio";
  public static readonly FBLStockFirstRow = "div[id$='row0']"
  public static readonly InActiveFBLStockAmount = "#textboxdiv_Amount"
  public static readonly NewWizardButton = "button[class$='Button']"
  public static readonly MinCodeRandomNumber = 1
  public static readonly MaxCodeRandomNumber = 10000000000
  public static readonly MinCounterCodeRandomNumber = 1
  public static readonly MaxCounterCodeRandomNumber = 10000
  public static readonly CodeDigitCount = 10
}
