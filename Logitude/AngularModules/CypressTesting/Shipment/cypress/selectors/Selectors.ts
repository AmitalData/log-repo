import { RegexSelectors } from '../selectors/RegexSelectors';

export class ShipmentSelectors extends RegexSelectors {
  //#region Shared
  public static readonly ShipmentMoreList = '#MenuButtons';
  public static readonly ShipmentTab = '#SHIP';
  public static readonly ContainersTab = '#CNFU';
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
  public static readonly ShipmentCustomerType = '#Shipment_ShipmentCustomerTypeCode';
  public static readonly NewTapItem = '.NewTabItem';
  public static readonly ShipmentWorkspace = '#SHIP_1';
  public static readonly ShipmentSearch = '#Shipment_Search_1';
  public static readonly EditShipmentMaincarrige = '.LogitudeIconButton';

  

  
  
  
  //#endregion
  //#region Create shipment
  public static readonly NewShipmentToggleButton = '#NEWSHIP .LogitudeToggleButtonContainer';
  public static readonly NewShipmentToggleButtonItem = '#NEWSHIP .LogitudeToggleButtonItem';
  public static readonly ShipmentMainCarriageFromPort = '#Shipment_MainCarriageFromPortId';
  public static readonly ShipmentMainCarriageToPort = '#Shipment_MainCarriageFinalDestinationPortId';
  public static readonly MasterMainCarriageFromPort = '#Master_MainCarriageFromPortId';
  public static readonly MasterMainCarriageToPort = '#Master_MainCarriageToPortId';
  public static readonly MasterAgent = '#Master_AgentId';
  public static readonly CreateShipmentButton = '#ShipmentCreatebtn';
  public static readonly CreateMasterShipmentButton = '#MasterCreatebtn';
  public static readonly LogLovShipmentCustomer="#LogLov_Shipment_ShipmentCustomerTypeCode"
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
  public static readonly EventsTab = '#ShipmentTHEvents';
  public static readonly ConnectionsTab = '#ShipmentTHConnections';
  public static readonly CustomsTab = '#ShipmentTHCustoms';
  public static readonly RoutingsTabHouse = '#ShipmentTHRoutings_1';
  public static readonly EventsTabHouse = '#ShipmentTHEvents_1';
  public static readonly PackagesTabHouse = '#ShipmentTHPackages_1';
  public static readonly OrdersTabHouse = '#ShipmentTHOrders_1';
  public static readonly PartnersTabHouse = '#ShipmentTHPartners_1';
  public static readonly BacktoMasterShipment = '.BackBottonBody';

 
  
  
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
  public static readonly AddInsidePackage = '#AddInsidePackage';
  public static readonly PackageQuantity = '#ShipmentPackage_Quantity';
  public static readonly InsidePackageQuantity = '#InsideShipmentPackage_Quantity';
  public static readonly PackageLength = '#ShipmentPackage_Length';
  public static readonly PackageWidth = '#ShipmentPackage_Width';
  public static readonly PackageHeight = '#ShipmentPackage_Height';
  public static readonly PackageVolume = '#ShipmentPackage_Volume';
  public static readonly PackageType = '#ShipmentPackage_PackageTypeId';
  public static readonly InsidePackageType = '#InsideShipmentPackage_PackageTypeId';
  public static readonly ContainerNumber = '#ShipmentPackage_ContainerNumber';
  public static readonly PackageWeight = '#ShipmentPackage_Weight';
  public static readonly PackageChargeableWeight = '#Shipment_ChargeableWeight';
  public static readonly InsidePackageWeight = '#InsideShipmentPackage_Weight';
  public static readonly InsidePackageDescription = '#InsideShipmentPackage_Description';
  public static readonly PackageContainerNumber = '#ShipmentPackage_ContainerNumber';
  public static readonly AirPackageOKButton = '#OkAirPackage';
  public static readonly OceanPackageOKButton = '#OkOceanPackage';
  public static readonly EditPackage = '#Edit';
  public static readonly PackageGrossWeight = '#Shipment_GrossWeight';
  public static readonly PackagePartialSplit = '#PartialSplit';

  public static readonly DeliveryATDTime = '#time_ShipmentPackage_DeliveryATD';
  public static readonly DeliveryATDDate = '#date_ShipmentPackage_DeliveryATD';
  public static readonly DeliveryATATime = '#time_ShipmentPackage_DeliveryATA';
  public static readonly DeliveryATADate = '#date_ShipmentPackage_DeliveryATA';
  public static readonly EmptyContainerReturnATDTime = '#time_ShipmentPackage_EmptyContainerReturnATD';
  public static readonly EmptyContainerReturnATDDate = '#date_ShipmentPackage_EmptyContainerReturnATD';
  public static readonly EmptyContainerReturnATATime = '#time_ShipmentPackage_EmptyContainerReturnATA';
  public static readonly EmptyContainerReturnATADate = '#date_ShipmentPackage_EmptyContainerReturnATA';
  public static readonly PickUpDeliveryATDTime = '#time_ShipmentPickUpDelivery_ATD';
  public static readonly PickUpDeliveryATDDate = '#date_ShipmentPickUpDelivery_ATD';
  //#endregion
  //#region Routing tab fields
  public static readonly RoutingToggle = '#RoutingToggle';
  public static readonly StorageCalculationScreen = '.LogitudeSectionBody';
  public static readonly PickUp = '#PickUp';
  public static readonly Delivery = '#Delivery';
  public static readonly PreCarriage = '#PreCarriage';
  public static readonly AddWarehouse = '#Add-WarehouseLeg';
  public static readonly EditWarehouseLeg = '#Edit-WarehouseLeg';
  public static readonly EditWarehouseLegPickups="#Edit-WarehouseLeg_Pickups"
  public static readonly EditRoutingMainCarriage = '#Edit-MainCarriage';
  public static readonly ShipmentMainCarriageCarrierId = '#Shipment_MainCarriageCarrierId';
  public static readonly ShipmentBookingNumberOfPackages = '#Shipment_BookingNumberOfPackages';
  public static readonly ShipmentFlightNumber = '#Shipment_MainCarriageCarrierNumber';
  public static readonly ShipmentMAWB = '#Shipment_Master';
  public static readonly ShipmentDateMaincarriageATD = '#calendarbutton_date_Shipment_MainCarriageATD';
  public static readonly MainCarriageATDTime = '#time_Shipment_MainCarriageATD';
  public static readonly MainCarriageATDDate = '#date_Shipment_MainCarriageATD';
  public static readonly MainCarriageATATime = '#time_Shipment_MainCarriageATA';
  public static readonly MainCarriageATADate = '#date_Shipment_MainCarriageATA';
  public static readonly WarehouseLeg = "#Shipment_WarehouseLegWarehouseId";
  public static readonly WarehouseLegActualReleaseDate = '#date_Shipment_WarehouseLegActualReleaseDate';
  public static readonly WarehouseLegActualEntryDate = '#date_Shipment_WarehouseLegActualEntryDate';
  public static readonly WarehouseLegExpectedReleaseDate = '#date_Shipment_WarehouseLegExpectedReleaseDate';
  public static readonly ContainsCalculateStorage = 'Calculate Storage';
  public static readonly ContainsStoragePricing = 'Storage Pricing';
  public static readonly ContainsWeight = "Weight = ";
  public static readonly StorageFeeResult = "div[data-cy='StorageFee']";
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
  public static readonly MainCarrigeVessel = 'input[id^=Shipment_MainCarriageVesselId_]';
  public static readonly MainCarrigeVoyageNo = 'input[id^=Shipment_MainCarriageCarrierNumber_]';
//Warehouse
public static readonly ShipmentWarehouseLegExpectedEntryDate="#date_Shipment_WarehouseLegExpectedEntryDate"
public static readonly ShipmentWarehouseLegActualEntryDate="#date_Shipment_WarehouseLegActualEntryDate"
public static readonly  ShipmentWarehouseLegExpectedEntryTime="#time_Shipment_WarehouseLegExpectedEntryDate"
public static readonly ShipmentWarehouseLegActualEntryTime="#time_Shipment_WarehouseLegActualEntryDate"

public static readonly ShipmentWarehouseLegExpectedReleaseDate="#date_Shipment_WarehouseLegExpectedReleaseDate"
public static readonly ShipmentWarehouseLegActualReleaseDate="#date_Shipment_WarehouseLegActualReleaseDate"
public static readonly ShipmentWarehouseLegExpectedReleaseTime="#time_Shipment_WarehouseLegExpectedReleaseDate"
public static readonly ShipmentWarehouseLegActualReleaseTime="#time_Shipment_WarehouseLegActualReleaseDate"
public static readonly WarehouseOKBtn="#WarehouseOKBtn"
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
  public static readonly LogLovShipmentReceivableChargesTypeId="#LogLov_ShipmentReceivable_ChargesTypeId"
  public static readonly LogLovShipmentReceivableCurrencyId="#LogLov_ShipmentReceivable_CurrencyId"

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
  public static readonly SplitShipmentButton = '#ShipmentBSplitShipment';
  public static readonly OperationalCloseButton = '#ShipmentBOperationalClose';
  public static readonly AccountllyCloseButton = '#ShipmentBAccountingClose';
  public static readonly OperationalReopenButton = '#ShipmentBOperationalReopen';
  public static readonly AccountllyReopenButton = '#ShipmentBAccountedReopen';
  public static readonly ConfirmActionButton = '#ConfirmAction';
  public static readonly ShipmentExceptionResolved="#ShipmentBExceptionResolved"
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
  public static readonly ContainsSplit = "Split"
  public static readonly ShortTitleControl = ".ShortTitleControl"

  //#region INTTRA
  public static readonly INTTRASettingId = "#INTTRASetting_INTTRAId";
  public static readonly INTTRASettingAlias = "#INTTRASetting_INTTRAAlias";
  public static readonly FTPDetailUserName = "#FTPDetail_UserName";
  public static readonly FTPDetailPassword = "#FTPDetail_Password";
  public static readonly FTPDetailHost = "#FTPDetail_Host";
  public static readonly FTPDetailFolder = "#FTPDetail_Folder";
  public static readonly SaveFTPDetailButton = ".RedButton:last";
  public static readonly ContainsSendEBooking = "Send e-booking";
  public static readonly ContainsSendShippingInstructions = "Send Shipping Instructions";
  public static readonly ShipmentINTTRAContractNumber = "#Shipment_INTTRAContractNumber";
  public static readonly ShipmentMainCarriageVessel = "input[id^='Shipment_MainCarriageVesselId']:last";
  public static readonly ShipmentMainCarriageETDDate = "#date_Shipment_MainCarriageETD";
  public static readonly ShipmentMainCarriageETDTime = "#time_Shipment_MainCarriageETD";
  public static readonly ShipmentDescriptionOfGoods = "#Shipment_DescriptionOfGoods";
  public static readonly ShipmentBookingConfirmationNumber = "#Shipment_BookingConfirmationNumber";
  public static readonly EditShipper = "#Edit-Shipper";
  public static readonly ShipmentShipperContact = "#Shipment_ShipperContactId";
  public static readonly ContactEnglishName = "#Contact_EnglishName";
  public static readonly ContactEmail = "#Contact_Email";
  public static readonly ShipmentBranch = "#Shipment_BranchId";
  public static readonly ContainsMessageHasBeenSentSuccessfully = "Message has been sent Successfully";
  public static readonly ContainsRequestBooking = "Request Booking";
  public static readonly SaveShipperContactButton = ".RedButton:last";
  public static readonly ContainsSend = "Send";
  public static readonly INTTRABookingStatus = "[data-cy='INTTRABookingStatus']";
  public static readonly AddShipperContactButton = "[data-cy='AddContact'] button";
  public static readonly ShippingInstructionsResultMessage = "td[data-cy='ResultMessage']";
  //#endregion
  //#region AMANAC
  public static readonly AMANACTab = '#AMANAC';
  public static readonly StatusValue = 'td[data-cy^=StatusValue]';
  public static readonly StatusDate = 'td[data-cy^=StatusDate]';
  public static readonly UserName = 'td[data-cy^=UserName]';
  public static readonly CloseAMANACView = 'button[data-cy^=CloseAMANACView]';
  public static readonly CloseExportingScreen = 'button[data-cy^=CloseExportingScreen]';
  public static readonly ValidationMsg = 'div[data-cy^=ValidationMsg]';
  public static readonly CustomsTransmissionsStatusValue = 'td[data-cy^=CustomsTransmissionsStatusValue]';
  public static readonly CustomsTransmissionsStatusDate = 'td[data-cy^=CustomsTransmissionsStatusDate]';
  public static readonly CustomsTransmissionsUserName = 'td[data-cy^=CustomsTransmissionsUserName]';
  public static readonly CloseCustomsTransmissions = 'button[data-cy^=CloseCustomsTransmissions]';
  public static readonly CustomsTransmissionsRetransfer = 'button[data-cy^=CustomsTransmissionsRetransfer]';
  public static readonly CheckAll = "CheckBox[data-cy^=CheckAll]";
  //#endregion
  //#region Event
  public static readonly ContainAddEvent= "Add Event"
  public static readonly EventType="#TraceEvent_EventTypeId"
  public static readonly EventDate="#date_TraceEvent_EventDateTime"
  public static readonly EventTime="#time_TraceEvent_EventDateTime"
  public static readonly TraceEventNotes= "#TraceEvent_Notes"
  public static readonly EventItemBox=".EventItemBox"
  public static readonly ContainHasException="Has Exception:"
  public static readonly EventNotes="#EventNotes"
  public static readonly ExceptionResolved="Exception Resolved"
  //#endregion


  //#region Shipment Conversions
  public static readonly ConvertToLCL = "#ShipmentBConverttoLCL";
  public static readonly ConvertToFCL = "#ShipmentBConverttoFCL";
  public static readonly ConvertToHouse = "#ShipmentBConvertShipmentFromDirectToHouse";
  public static readonly ConvertToDirect = "#ShipmentBConvertShipmentFromHouseToDirect";
  public static readonly ConvertShipmentDirection = "#ShipmentBConvertShipmentDirection";
  public static readonly ShipmentTypeValue = "[data-cy='ShipmentTypeValue']";
  public static readonly ShipmentEventsRefreshButton = "[data-cy='EventsRefresh_Shipment'] button";
  public static readonly PartnerName = "[data-cy='PartnerName']";
  //#endregion
  //#region Delivery Leg
  public static readonly EditDelivery="#Edit-Delivery"
  public static readonly DeliveryToPartnerName="#ShipmentPickUpDelivery_ToPartnerCardId"
  public static readonly PickUpDeliveryETDDate ="#date_ShipmentPickUpDelivery_ETD"
  public static readonly PickUpDeliveryETDTime="#time_ShipmentPickUpDelivery_ETD"
  //#endregion
  public static readonly ComponentBusyIndicator="#EditComponentBusyIndicator_0"



}