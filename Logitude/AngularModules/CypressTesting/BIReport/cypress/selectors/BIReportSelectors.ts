import { RegexSelectors } from './RegexSelectors';

export class BIReportSelectors extends RegexSelectors {

 public static readonly BI = '#BI';
 //#region Bi Folder
 public static readonly NewBIReportFolder = '#NewBIReportFolder';
 public static readonly BIReportFolderDescription = '#BIReportFolder_Description';
 public static readonly BIReportFolderName = '#BIReportFolder_Name';
 public static readonly FolderSearch = '#null_Search_1';
 public static readonly SearchBIReportFolder = '#BIReportFolder';
 //#region Bi Report
 public static readonly NewButtonBIReport = '#NewButton_BIReport';
 public static readonly BIReportName = '#BIReport_Name';
 public static readonly BIReportDescription = '#BIReport_Description';
 public static readonly BIReportFact = '#ComboBox_0_1 img';
 public static readonly BIReportFactType ='.ComboBoxDropdown';

 //#region QueryBuilder
 public static readonly SearchDWQueryBuilderSearchFields = '#DWQueryBuilderSearchFields';
 //#region add shipment number
 public static readonly AddQBRootColumnShipmentNumber = '#AddQBRootColumnShipmentNumber';
 public static readonly AddQBRootFilterShipmentNumber_1 = '#AddQBRootFilterShipmentNumber';
//#region add customer
public static readonly AddQBRootColumnCustomer = '#AddQBRootColumnCustomer';
 public static readonly AddQBRootFilterCustomer = '#AddQBRootFilterCustomer';

 //#region add Shipper
 public static readonly AddQBRootColumnShipper = '#AddQBRootColumnShipper';
 public static readonly AddQBRootFilterShipper = '#AddQBRootFilterShipper';






}