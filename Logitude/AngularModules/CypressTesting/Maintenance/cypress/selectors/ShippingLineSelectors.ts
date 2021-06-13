import { RegexSelectors } from '../selectors/RegexSelectors';

export class ShippingLineSelectors extends RegexSelectors {

  public static readonly ShippingLineMaintenanceItem = "#MaintenanceItemMTSL"
  public static readonly ShippingLineSaveCloseButton = "#ShippingLine-SaveClose";
  public static readonly ShippingLineSaveButton = "#ShippingLine-Save";
  public static readonly ShippingLineName = "#ShippingLine_EnglishName";
  public static readonly ShippingLineLocalName = "#ShippingLine_LocalName";
  public static readonly ShippingLineCode = "#ShippingLine_Code";
  public static readonly ShippingLineNotes = "#ShippingLine_Remark";
  public static readonly ShippingLineSCACCode = "#ShippingLine_SCACCode"
  public static readonly InActiveShippingLineCheckBox = "#ShippingLine_InActive"
  public static readonly ShippingLineEventsTab = "#ShippingLineTHEvents"
  public static readonly AddressesTab = "#ShippingLineTHAddresses"
  public static readonly AccountingTab = "#ShippingLineTHAccounting"
  public static readonly INTTRA = "#ShippingLineTHINTTRA"
  public static readonly AreasTab = "#ShippingLineTHAreas"
  public static readonly TariffTranslations = "#ShippingLineTHTariffTranslations"
  public static readonly GeneralTab = "#ShippingLineTHGeneral"
  public static readonly ShippingLine_IsINTTRA = '#ShippingLine_IsINTTRARegistered'
  public static readonly ShippingLine_INTTRANotes= '#ShippingLine_INTTRARegistrationNotes'
  public static readonly MinCodeRandomNumber = 1
  public static readonly MaxCodeRandomNumber = 10000000000
  public static readonly MinCounterCodeRandomNumber = 1
  public static readonly MaxCounterCodeRandomNumber = 10000
  public static readonly CodeDigitCount = 4
  public static readonly TariffCodeDigitCount = 10
}
