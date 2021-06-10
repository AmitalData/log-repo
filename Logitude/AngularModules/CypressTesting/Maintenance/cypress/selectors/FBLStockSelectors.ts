import { RegexSelectors } from '../selectors/RegexSelectors';

export class FBLStockSelectors extends RegexSelectors {

  public static readonly FBLStockMaintenanceItem = "#MaintenanceItemMTFS"


  public static readonly FBLStockStartNumber = "#StartNumber";
  public static readonly FBLStockEndNumber = "#EndNumber";
  public static readonly FBLStockByEndNumber = "#ByEndNumberRadio";
  public static readonly FBLStockByAmount = "#ByAmountRadio";
  public static readonly FBLStockAmount = "#Amount";
  public static readonly FBLStockGridBody = ".SimpleGridViewBody";
  public static readonly FBLStockGridRow = "tr.SimpleGridViewRow";
  public static readonly InActiveFBLStockAmount = "#textboxdiv_Amount"
  public static readonly Button = ".Button"
  public static readonly Remove = "Remove"
  public static readonly Add = "Add"
  public static readonly ConfirmRemove = "#ConfirmWindow_Yes_0"
  public static readonly Delete = "Delete"
  public static readonly RemoveSeries = "Remove Series"
  public static readonly MinRandomNumber = 1
  public static readonly MaxRandomNumber = 10000
  public static readonly MaxAmountRandomNumber = 1000
}
