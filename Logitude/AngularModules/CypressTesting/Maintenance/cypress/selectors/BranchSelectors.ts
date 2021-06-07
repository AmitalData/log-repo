import { RegexSelectors } from '../selectors/RegexSelectors';

export class BranchSelectors extends RegexSelectors {

  public static readonly BranchMaintenanceItem = "#MaintenanceItemMTBR"
  public static readonly BranchSaveCloseButton = "#Branch-SaveClose";
  public static readonly BranchSaveButton = "#Branch-Save";
  public static readonly BranchName = "#Branch_EnglishName";
  public static readonly BranchLocalName = "#Branch_LocalName";
  public static readonly BranchCode = "#Branch_Code";
  public static readonly BranchSignature = "#Branch_Signature";
  public static readonly BranchCounterCode = "#Branch_CounterCode"
  public static readonly BranchFirstRow = "div[id$='row0']"
  public static readonly InActiveBranchCheckBox = "#Branch_InActive"
  public static readonly BranchEventsTab = "#BranchTHEvents"
  public static readonly MinRandomNumber = 1
  public static readonly MaxRandomNumber = 1000
}

