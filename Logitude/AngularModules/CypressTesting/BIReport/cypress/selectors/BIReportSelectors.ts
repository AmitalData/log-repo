import { RegexSelectors } from './RegexSelectors';

export class BIReportSelectors extends RegexSelectors {

 public static readonly BI = '#BI';
 //#region Bi Folder
 public static readonly NewBIReportFolder = '#NewBIReportFolder';
 public static readonly BIReportFolderDescription = '#BIReportFolder_Description';
 public static readonly BIReportFolderName = '#BIReportFolder_Name';
 public static readonly FolderSearch = '[id^=null_Search_]'
 public static readonly SearchBIReportFolder = '#BIReportFolder';
 //#region Bi Report
 public static readonly NewButtonBIReport = '#NewButton_BIReport';
 public static readonly BIReportName = '#BIReport_Name';
 public static readonly BIReportDescription = '#BIReport_Description';
 public static readonly BIReportFact = '.ComboBox';
 public static readonly BIReportFactType ='.ComboBoxDropdown';
 public static readonly EditQueryBuilder ='#EditQueryBuilder';
 public static readonly EditBackbutton ='#EditBackbutton_1';
 public static readonly MasterEditBackbutton ='#EditBackbutton_2';
 public static readonly ARInvoicesEditBackbutton ='#EditBackbutton_3';
 public static readonly QuoteEditBackbutton ='#EditBackbutton_4';

 
 //#region QueryBuilder
 public static readonly SearchDWQueryBuilderSearchFields = '#DWQueryBuilderSearchFields';
 //#region add shipment number
 
 public static readonly GridViewRowRowHover = '#SimpleGridViewRowRowHover';
 public static readonly AddQBRootColumnShipmentNumber = '#AddQBRootColumnShipmentNumber';
 public static readonly AddQBRootFilterShipmentNumber = '#AddQBRootFilterShipmentNumber';
//#region add customer
public static readonly AddQBRootColumnCustomer = '#AddQBRootColumnCustomer';
 public static readonly AddQBRootFilterCustomer = '#AddQBRootFilterCustomer';

 //#region add Shipper
 public static readonly AddQBRootColumnShipper = '#AddQBRootColumnShipper';
 public static readonly AddQBRootFilterShipper = '#AddQBRootFilterShipper';

 //#region add Branch
 public static readonly AddQBRootColumnBranch = '#AddQBRootColumnBranch';
 public static readonly AddQBRootFilterBranch = '#AddQBRootFilterBranch';

 //#region add Master
 public static readonly AddQBRootColumnMaster = '#AddQBRootColumnMaster';
 public static readonly AAddQBRootFilterMaster = '#AddQBRootFilterMaster';

 //#region add MasterShipmentNumber
 public static readonly AddQBRootColumnMasterShipmentNumber = '#AddQBRootColumnMasterShipmentNumber';
 public static readonly AddQBRootFilterMasterShipmentNumber = '#AddQBRootFilterMasterShipmentNumber';

 
  //#region add ARInvoiceType
  public static readonly AddQBRootColumnARInvoiceType = '#AddQBRootColumnARInvoiceType';
  public static readonly AddQBRootFilterARInvoiceType = '#AddQBRootFilterARInvoiceType';

   //#region add InvoiceBranch
 public static readonly AddQBRootColumnInvoiceBranch = '#AddQBRootColumnInvoiceBranch';
 public static readonly AddQBRootFilterInvoiceBranch = '#AddQBRootFilterInvoiceBranch';

  //#region add QuoteNumber
  public static readonly AddQBRootColumnQuoteNumber = '#AddQBRootColumnQuoteNumber';
  public static readonly AddQBRootFilterQuoteNumber = '#AddQBRootFilterQuoteNumber';

    //#region add Is Quote Data External
 public static readonly AddQBRootColumnIsQuoteDataExternal = '#AddQBRootColumnIsQuoteDataExternal';
 public static readonly AddQBRootFilterIsQuoteDataExternal = '#AddQBRootFilterIsQuoteDataExternal';
}