import { RegexSelectors } from '../selectors/RegexSelectors';

export class ShipmentSelectors extends RegexSelectors {
  //#region Shared
  public static readonly ShipmentMoreList = '#MenuButtons';
  public static readonly ShipmentTab = '#SHIP';
  public static readonly ShipmentSearchBar = '#Shipment_Search';
  public static readonly ShipmentSearchParent = 'searchbox';
  public static readonly ShipmentSearchParentClass = '.SearchBox';
  public static readonly ShipmentEventNote = '#EventNotes';
  //public static readonly InvoiceMoreList = '#MenuButtons_1';
  public static readonly ConfirmWindowYes = '#ConfirmWindow_Yes_0';
  public static readonly SaveClose = '#SaveClose';
  public static readonly ShipmentSaveButton = '#Shipment-Save';
  public static readonly ShipmentShipper = '#Shipment_ShipperId';
  public static readonly ShipmentConsignee = '#Shipment_ConsigneeId';
  //#endregion
  //#region Create shipment
  public static readonly NewShipmentToggleButton = '#NEWSHIP .LogitudeToggleButtonContainer';
  public static readonly NewShipmentToggleButtonItem = '#NEWSHIP .LogitudeToggleButtonItem';
  public static readonly ShipmentMainCarriageFromPort = '#Shipment_MainCarriageFromPortId';
  public static readonly ShipmentMainCarriageToPort = '#Shipment_MainCarriageToPortId';
  public static readonly MasterMainCarriageFromPort = '#Master_MainCarriageFromPortId';
  public static readonly MasterMainCarriageToPort = '#Master_MainCarriageToPortId';
  public static readonly MasterAgent = '#Master_AgentId';
  public static readonly CreateShipmentButton = '#ShipmentCreatebtn';
  public static readonly CreateMasterShipmentButton = '#MasterCreatebtn';
  //#endregion
  //#region Shipment tabs
  public static readonly GeneralTab = '#ShipmentTHGeneral';
  public static readonly OrdersTab = '#ShipmentTHOrders';
  public static readonly PartnersTab = '#ShipmentTHPartners';
  public static readonly ShipmentsTab = '#ShipmentTHConsolidation';
  public static readonly PackagesTab = '#ShipmentTHPackages';
  public static readonly ReceivablesTab = '#ShipmentTHReceivables';
  public static readonly RoutingsTab = '#ShipmentTHRoutings';
  public static readonly PayablesTab = '#ShipmentTHPayables';
  public static readonly DocsOutTab = '#ShipmentTHDocsOut';
  public static readonly DocsInTabb = '#ShipmentTHDocsIn';
  public static readonly Events = '#ShipmentTHEvents';
  //#endregion
  //#region General tab
  public static readonly ShipmentGrossWeight = '#Shipment_GrossWeightInKG';
  public static readonly ShipmentMoveType = '#Shipment_MoveTypeId';
  public static readonly ShipmentValueOfGoods = '#Shipment_ValueOfGoods';
  //#endregion
  //#region Shipment tab
  public static readonly ShipmentCustomer = '#Shipment_CustomerId';
  //#endregion
  //#region Order tab
  public static readonly OrdersAddPackage = '#Orders-AddPackage';
  public static readonly OrderPackageQuantity = '#ShipmentOrderPackage_Quantity';
  public static readonly OrderPackageType = '#ShipmentOrderPackage_PackageTypeId';
  public static readonly OrderPackageGrossWeight = '#ShipmentOrderPackage_GrossWeight';
  public static readonly OrderPackageLength = '#ShipmentOrderPackage_Length';
  public static readonly OrderPackageWidth = '#ShipmentOrderPackage_Width';
  public static readonly OrderPackageHeight = '#ShipmentOrderPackage_Height';
  public static readonly OrderOKButton = '#OrderOKbtn';
  //#endregion
  //#region Partner tab
  public static readonly PartnerToggle = '#PartnerToggle';
  public static readonly AddShipperButton = '#SHIPR';
  public static readonly AddConsigneeButton = '#CONSI';
  public static readonly AddAgentButton = '#AGENT';
  public static readonly AddIssuingCarrierAgentButton = '#ISSAG';
  public static readonly AddCustomsAgentExportButton = '#CSAEX';
  public static readonly AddCustomsAgentImportButton = '#CSAIM';
  public static readonly AddNotify1Button = '#NOTF1';
  public static readonly AddNotify2Button = '#NOTF2';
  public static readonly AddShipperNotExporterButton = '#SHPNT';
  public static readonly AddConsigneeNotImporterButton = '#CONNT';
  public static readonly AddFreightForwarderButton = '#FRTFR';
  public static readonly AddColoaderButton = '#COLOD';
  public static readonly AddCustomClearancePointButton = '#CLERN';
  public static readonly AddConsolidatorButton = '#CONSL';
  public static readonly AddReleasingAgentButton = '#REAGT';
  public static readonly ShipmentAgent = '#Shipment_AgentId';
  public static readonly ShipmentIssuingCarrierAgent = '#Shipment_IssuingCarrierAgentId';
  public static readonly ShipmentCustomAgentExport = '#Shipment_CustomAgentExportId';
  public static readonly ShipmentCustomAgentImport = '#Shipment_CustomAgentImportId';
  public static readonly ShipmentNotify1 = '#Shipment_Notify1Id';
  public static readonly ShipmentNotify2 = '#Shipment_Notify2Id';
  public static readonly ShipmentShipperNotExporter = '#Shipment_ShipperNotExporterId';
  public static readonly ShipmentConsigneeNotImporter = '#Shipment_ConsigneeNotImporterId';
  public static readonly ShipmentFreightForwarder = '#Shipment_FreightForwarderId';
  public static readonly ShipmentColoader = '#Shipment_ColoaderId';
  public static readonly ShipmentCustomClearancePoint = '#Shipment_CustomClearancePointId';
  public static readonly ShipmentConsolidator = '#Shipment_ConsolidatorId';
  public static readonly ShipmentReleasingAgent = '#Shipment_ReleasingAgentId';
  public static readonly PartnerOKButton = '#PartnerOKbtn';
  public static readonly PartnerEditShipper = '#Edit-Shipper';
  //#endregion 
  //#region Package tab fields
  public static readonly AddPackage = '#AddPackage';
  public static readonly PackageQuantity = '#ShipmentPackage_Quantity';
  public static readonly PackageLength = '#ShipmentPackage_Length';
  public static readonly PackageWidth = '#ShipmentPackage_Width';
  public static readonly PackageHeight = '#ShipmentPackage_Height';
  public static readonly PackageType = '#ShipmentPackage_PackageTypeId';
  public static readonly PackageWeight = '#ShipmentPackage_Weight';
  public static readonly AirPackageOKButton = '#OkAirPackage';
  public static readonly OceanPackageOKButton = '#OkOceanPackage';
  //#endregion
  //#region Routing tab fields
  public static readonly RoutingToggle = '#RoutingToggle';
  public static readonly PickUp = '#PickUp';
  public static readonly Delivery = '#Delivery';
  public static readonly PreCarriage = '#PreCarriage';
  public static readonly EditRoutingMainCarriage = '#Edit-MainCarriage';
  public static readonly ShipmentMainCarriageCarrierId = '#Shipment_MainCarriageCarrierId';
  public static readonly ShipmentBookingNumberOfPackages = '#Shipment_BookingNumberOfPackages';
  public static readonly ShipmentFlightNumber = '#Shipment_MainCarriageCarrierNumber';
  public static readonly ShipmentMAWB = '#Shipment_Master';
  public static readonly ShipmentDateMaincarriageATD = '#calendarbutton_date_Shipment_MainCarriageATD';
  public static readonly MainCarriageOKBtn = '#MainCarriageOKBtn';
  public static readonly ShipmentPickUpDeliveryToPartnerCard = '#ShipmentPickUpDelivery_ToPartnerCardId';
  public static readonly ShipmentPreCarriageTransportMode = '#Shipment_PreCarriageTransportModeId';
  public static readonly ShipmentPreCarriageFromPort = '#Shipment_PreCarriageFromPortId';
  public static readonly ShipmentPreCarriageToPort = '#Shipment_PreCarriageToPortId';
  public static readonly PreCarriageOKBtn = '#PreCarriageOKBtn';
  public static readonly ShipmentOnCarriageTransportMode = '#Shipment_OnCarriageTransportModeId';
  public static readonly ShipmentOnCarriageFromPort = '#Shipment_OnCarriageFromPortId';
  public static readonly OnCarriage = '#OnCarriage';
  public static readonly ShipmentOnCarriageToPort = '#Shipment_OnCarriageToPortId';
  public static readonly OnCarriageOKBtn = '#OnCarriageOKBtn';
  //#endregion
  //#region Receivable tab fields
  public static readonly AddNewReceivableLine = '#AddReceivable button';
  public static readonly ReceivableChargesType = '#ShipmentReceivable_ChargesTypeId';
  public static readonly ReceivableUnitPrice = '#ShipmentReceivable_UnitPrice';
  public static readonly ReceivableQuantity = '#ShipmentReceivable_Quantity'
  public static readonly ReceivableCurrency = '#ShipmentReceivable_CurrencyId'
  public static readonly ReceivableMeasurement = '#ShipmentReceivable_MeasurementId'
  public static readonly ReceivableTotalAmount = '#ShipmentReceivable_TotalAmount';
  public static readonly AddReceivableOkButton = '#Ok-AddReceivableBtn';
  public static readonly ShipmentReceivableRate = '#ShipmentReceivable_Rate'
  public static readonly ReceivableFromPayables="#PAYB-Receivable"
  //#endregion 
  //#region Payable tab fields
  public static readonly AddNewPayableLine = '#AddPayable button';
  public static readonly AddPayableOkButton = '#Ok-AddPayableBtn';
  public static readonly ShipmentPayableChargesType = '#ShipmentPayable_ChargesTypeId';
  public static readonly ShipmentPayableMeasurement = '#ShipmentPayable_MeasurementId';
  public static readonly ShipmentPayableCurrency = '#ShipmentPayable_CurrencyId';
  public static readonly ShipmentPayableRate ="#ShipmentPayable_Rate"
  public static readonly ShipmentPayableUnitPrice = '#ShipmentPayable_UnitPrice';
  public static readonly ShipmentPayableQuantity = '#ShipmentPayable_Quantity';
  public static readonly ShipmentPayableVendor = '#ShipmentPayable_VendorId';
  //#endregion
  //#region operations and actions
  public static readonly NewAttachedHouse = '#NewHouseBtn';
  public static readonly CopyShipmentButton = '#ShipmentBCopyShipment';
  public static readonly CancelShipmentButton = '#ShipmentBCancelShipment';
  public static readonly ReactivateShipmentButton = '#ShipmentBReactivateShipment';
  public static readonly OperationalCloseButton = '#ShipmentBOperationalClose';
  public static readonly AccountllyCloseButton = '#ShipmentBAccountingClose';
  public static readonly OperationalReopenButton = '#ShipmentBOperationalReopen';
  public static readonly AccountllyReopenButton = '#ShipmentBAccountedReopen';
  public static readonly ConfirmActionButton = '#ConfirmAction';
  //#endregion

  //#region Send docs tab
  public static readonly SendMessageButton = '#SendMessagebtn';
  public static readonly EmailSearchInput = '.LogitudeEmailSearchInput';
  //#endregion
  //#region AWB Wizard
  public static readonly AddPackageLineInAWBWizard = "#AddPackageBtn";
  public static readonly PackageQuantityInAWBWizard = "input[id^='ShipmentPackage_Quantity']:last";
  public static readonly PackageLengthInAWBWizard = "input[id^='ShipmentPackage_Length']:last";
  public static readonly PackageWidthInAWBWizard = "input[id^='ShipmentPackage_Width']:last";
  public static readonly PackageHeightInAWBWizard = "input[id^='ShipmentPackage_Height']:last";
  public static readonly PackageWeightInAWBWizard = "input[id^='ShipmentPackage_Weight']:last";
  public static readonly OverviewTabComponentInAWBWizard = "overviewtabcomponent";
  public static readonly OverviewTabInAWBWizard = "#OVE";
  public static readonly PackagesTabInAWBWizard = "#PAC";
  //#endregion

  public static readonly ContainsToggleButtonDisabled = "ToggleButtonDisabled"
  public static readonly ShortTitleControl = ".ShortTitleControl"

}