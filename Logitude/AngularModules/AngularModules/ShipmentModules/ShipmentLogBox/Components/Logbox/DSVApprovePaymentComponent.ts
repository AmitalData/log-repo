declare var System: any, window: any;
import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'

import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogBoxDocumentsComponent} from './LogBoxDocumentsComponent';
import {ShipmentDomainService, ImporterQueriesDataCounts} from '../../../../Shipment/Services/ShipmentDomainService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CustomNumbersPipe} from '../../../../Infrastructure/Pipes/CustomNumbersPipe';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityStatusListService} from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import {BranchListService} from '../../../../Common/Services/StandardLists/BranchListService';
import {DepartmentListService} from '../../../../Common/Services/StandardLists/DepartmentListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ShipmentAdditionalCloudDataService} from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {DocumentTypeMetaDataExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService' 
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
@Component({
    
    templateUrl: './DSVApprovePaymentComponent.html'
})

export class DSVApprovePaymentComponent extends BaseComponent implements OnInit, AfterViewInit {
  public SearchText: string = null;
  public DeleteDocumentClicked(item: any) { }

    DataContext: DSVApprovePaymentComponent = this;
    private messageWindow: MessageWindow = new MessageWindow();
    EntityPm: ShipmentPM = new ShipmentPM();
    Language: string = 'HB';
    public RTL: boolean = true;
    AdditionalData: any;
    externalDocs: any[];
    public DimApproveButton: boolean = false;
    public DimDenyButton: boolean = false;
    public _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    _ImageLibraryService: ImageLibraryService;
    private CurrentSession = SessionLocator.SelectedSession;
    private tax1Amount: number = 0;
    private tax16Amount: number = 0;
    constructor() {
        super();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        this._ImageLibraryService = new ImageLibraryService();
        this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
        this.Language = SessionLocator.TenantPM.Language;
        this.RTL = (this.Language == 'HB');
    }

    private isAccepted: boolean = false;
    public get IsAccepted() { return this.isAccepted }
    public set IsAccepted(newValue: boolean) { this.isAccepted = newValue; }

    IsAcceptedChanged($event) {
        this.IsAccepted = $event;
    }
    ngOnInit() {

    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
        if (args.EntityPm) {
            this.EntityPm = args.EntityPm;
            this.AdditionalData = args.AdditionalData;
            
                if (!AppTool.IsNullOrEmpty(this.AdditionalData.DenyReason)) {
                    //this.DimDenyButton = true;
                }
                if (!AppTool.IsNullOrEmpty(this.AdditionalData.ApprovedByUserName) && !AppTool.IsNullOrEmpty(this.AdditionalData.VersionApproved) && (this.AdditionalData.VersionApproved == this.AdditionalData.VersionId)) {
                    var today = new Date(this.AdditionalData.ApproveDateTime);
                    var d = today.getDate();
                    var m = today.getMonth() + 1; //January is 0!
                    var dd = "";
                    var mm = "";
                    var yyyy = today.getFullYear().toString();
                    if (d < 10) {
                        dd = '0' + d;
                    }
                    else {
                        dd = d.toString();
                    }
                    if (m < 10) {
                        mm = '0' + m;
                    }
                    else {
                        mm = m.toString();
                    }
                    var to = dd + '/' + mm + '/' + yyyy;
                    //This Version (*VersionID*) was already Approved by *ApprovedByUserName* at *ApproveDateTime*
                    //גרסת הצהרה זו כבר אושרה על ידי *ApprovedByUserName* בתאריך *ApproveDateTime*
                    var tempMessage = TextCodeTranslator.Translate("Shipment.O.VersionApprovedBy");
                    tempMessage = tempMessage.replace("*VersionID*", this.AdditionalData.VersionId);
                    tempMessage = tempMessage.replace("*ApprovedByUserName*", this.AdditionalData.ApprovedByUserName);
                    tempMessage = tempMessage.replace("*ApproveDateTime*", to);
                    this.ValidationWarningsList = tempMessage;//TextCodeTranslator.Translate("Shipment.O.VersionApprovedBy") + " " + this.AdditionalData.ApprovedByUserName + " " + TextCodeTranslator.Translate("Shipment.O.OnDate") + " " + to;//"גרסת הצהרה זו כבר אושרה על ידי " + this.AdditionalData.ApprovedByUserName + " בתאריך " + to + "";

                    this.DimApproveButton = true;
                }
                var ObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
                this._documentsFilingExtendedPMService.getAllDocumentsFilingsByEntityIdAndObjectTable(this.EntityPm.Id, ObjectTable.Id, "I", SessionLocator.Tenant).subscribe((res:any) => {
                    var Result = [];//DocumentTypeMetaDataExtendedService

                    Result = res.Result.filter(a => a.IsDeleted == false);


                    this.externalDocs = [];

                    var SupplierInvoice = Result.filter(a => a.DocumentTypeCode == "380" || a.DocumentTypeCode == "721");
                    var Others = Result.filter(a => a.DocumentTypeCode != "721" && a.DocumentTypeCode != "380");
                    var tempSupplierInvoice = [];
                    var tempOthers = [];
                    var DRELID = "";
                    this._DocumentTypeMetaDataExtendedService.GetDocumentsMetaDataTypeByCode("DREL").subscribe((myResult:any) => {
                        if (myResult.Result) {
                            DRELID = myResult.Result.Id;
                            if (!AppTool.IsNullOrEmpty(DRELID)) {
                                SupplierInvoice.forEach((mydoc) => {
                                    var DRELTypes = mydoc.DocumentsFilingMetaDataValues.filter(a => a.DocumentsMetaDataTypeId == DRELID);
                                    if (DRELTypes != null && DRELTypes.length > 0) {
                                        tempSupplierInvoice.push(mydoc);
                                    }
                                });
                                Others.forEach((docin) => {
                                    var DRELTypes = docin.DocumentsFilingMetaDataValues.filter(a => a.DocumentsMetaDataTypeId == DRELID);
                                    if (DRELTypes != null && DRELTypes.length > 0) {
                                        tempOthers.push(docin);
                                    }
                                });
                            }
                        }
                    });

                    //else {
                    //    tempSupplierInvoice = SupplierInvoice;
                    //    tempOthers = Others;
                    //}
                    this.externalDocs.push({ key: TextCodeTranslator.Translate("Shipment.O.SupplierInvoiceAndPackingList"), value: tempSupplierInvoice });// "חשבונות ספק ורשימות אריזה"
                    this.externalDocs.push({ key: TextCodeTranslator.Translate("Shipment.O.AdditionalDocuments"), value: tempOthers });//"מסמכים נוספים"
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();

                }, error => {
                    var dd: Response = error;
                });
           
          
        }
    }
    public ValidationWarningsList: string = null;
    ApproveButtonClicked() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Approving ...");
        this.ValidationWarningsList = null;
        this._ShipmentAdditionalCloudDataService.getsingledata(this.EntityPm.Id).subscribe((AdditionalResult:any) => {
            var entity = AdditionalResult.Result;
            this.CurrentSession.CurrentWindow.StopBusyIndicator(); 
            if (!AppTool.IsNullOrEmpty(entity.ApprovedByUserName)) {//!AppTool.IsNullOrEmpty(entity.DenyReason) || 
                this.messageWindow.RTL = this.RTL;
                this.messageWindow.Width = 300;
                this.messageWindow.Height = 150;
                this.messageWindow.Title = TextCodeTranslator.Translate("Shipment.O.warning");//"אזהרה!";
                this.messageWindow.Message = TextCodeTranslator.Translate("Shipment.O.VersionAlreadyApproved");//"גרסה זו כבר אושרה על ידי משתמש אחר";
                this.messageWindow.Show(this.messageWindow.Message);
                //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
            }
            else {
                if (SessionLocator.TenantPM.ShowTaxAmountWarning) {
                    var warningCode: string;
                    if (this.AdditionalData.TaxesDetails) {
                        warningCode = this.GetWarningCodeBeforeApproval(this.AdditionalData.TaxesDetails);
                    }
                    if (warningCode && warningCode != '0') {
                        var warningWindow = new LogitudeWindow();
                        warningWindow.Width = 290;
                        warningWindow.Height = 180;
                        warningWindow.RTL = this.RTL;
                        //warningWindow.Title
                        var windowArgs: any = {};
                        windowArgs.WarningCode = warningCode;
                        windowArgs.tax1Amount = this.tax1Amount;
                        windowArgs.tax16Amount = this.tax16Amount;
                        windowArgs.RTL = this.RTL;
                        warningWindow.WindowArgs = windowArgs;
                        warningWindow.IsShowCloseButton = true;
                        warningWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/WarningApprovePaymentComponent');
                        warningWindow.WindowClosed.subscribe((event: any) => {
                            if (event == "Approved")
                                this.UpdateShipmentAdditionalCloudData(entity);
                            else if (event == "Deny")
                                this.DenyButtonClicked();
                        });
                    }
                    else {
                        this.UpdateShipmentAdditionalCloudData(entity);
                    }
                }
                else {
                    this.UpdateShipmentAdditionalCloudData(entity);
                }
            }
        });

    }

    private UpdateShipmentAdditionalCloudData(entity: any) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Approving ...");
        entity.ApprovedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        entity.DenyReason = "";
        this._ShipmentAdditionalCloudDataService.update(entity).subscribe((AdditionalResult: any) => {
            ServiceLocator.SendTotangoUserActivity("LogBox", "Approve Declaration");
            MixPanelLocator.Action({ ProjectName:"LogBox", ActionName: "Approve Declaration" });
            this.DimApproveButton = true;
            var today = new Date();
            var d = today.getDate();
            var m = today.getMonth() + 1; //January is 0!
            var dd = "";
            var mm = "";
            var yyyy = today.getFullYear().toString();
            if (d < 10) {
                dd = '0' + d;
            }
            else {
                dd = d.toString();
            }
            if (m < 10) {
                mm = '0' + m;
            }
            else {
                mm = m.toString();
            }
            var to = dd + '/' + mm + '/' + yyyy;
            this.messageWindow.RTL = this.RTL;
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = TextCodeTranslator.Translate("Shipment.O.StatementWasApproved"); //"הצהרה אושרה";
            this.messageWindow.Message = TextCodeTranslator.Translate("Shipment.O.ConfirmationSentTo") + SessionLocator.PrivateLableSettings.PrivateLabelShortName; //"אישור הצהרה נשלח ל -" + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.messageWindow.Show(this.messageWindow.Message);
            //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    }

    private GetWarningCodeBeforeApproval(TaxDetails) {
        this.AlertForTesting(TaxDetails);
        this.tax1Amount = 0, this.tax16Amount = 0;
        TaxDetails.forEach((tax) => {
            if (tax.TaxTypeCode == '1')
                this.tax1Amount = +tax.TaxAmount;
            else if (tax.TaxTypeCode == '16')
                this.tax16Amount = +tax.TaxAmount;
        });
        if (this.tax1Amount != 0 && this.tax16Amount != 0) {
            if (this.tax1Amount + this.tax16Amount > 100)
                return '17';
        }
        else if (this.tax1Amount > 100) {
            return '1';
        }
        else if (this.tax16Amount > 100) {
            return '16';
        }
        return '0';

    }

    AlertForTesting(TaxDetails) {
        if (SessionLocator.LoggedUserPM.Email == "ahmadb@logitudeworld.com") { //For testing.
            TaxDetails.forEach((tax) => {
                alert("tax type code:" + tax.TaxTypeCode + "\ntax amount:" + tax.TaxAmount);
            });
        }
    }

    DenyButtonClicked() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Sending ...");
        var newWindow = new LogitudeWindow();
        newWindow.Width = 350;
        newWindow.Height = 280;
        newWindow.RTL = true;
        
        this._ShipmentAdditionalCloudDataService.getsingledata(this.EntityPm.Id).subscribe((AdditionalResult:any) => {
            var entity = AdditionalResult.Result
            if (!AppTool.IsNullOrEmpty(entity.ApprovedByUserName)) {//!AppTool.IsNullOrEmpty(entity.DenyReason) || 
                this.messageWindow.RTL = this.RTL;
                this.messageWindow.Width = 300;
                this.messageWindow.Height = 150;
                this.messageWindow.Title = TextCodeTranslator.Translate("Shipment.O.warning");//"אזהרה!";
                this.messageWindow.Message = TextCodeTranslator.Translate("Shipment.O.Thisversionhasalreadybeenrejected");//"גרסה זו כבר נדחתה על ידי משתמש אחר";
                this.messageWindow.Show(this.messageWindow.Message);
                //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
            else {
                newWindow.Title = TextCodeTranslator.Translate("Shipment.O.ExplanationForRejectingAStatement");//"הסבר לדחיית הצהרה";//Shipment.O.ExplanationForRejectingAStatement
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var windowArgs: any = {};
                windowArgs.AdditionalData = entity;
                newWindow.WindowArgs = windowArgs;
                //newWindow.Add(control); 
              newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DenyReasonComponent');
                newWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event == "Denied") {
                        ServiceLocator.SendTotangoUserActivity("LogBox", "Deny Declaration");
                        this.DimDenyButton = true;
                        this.CurrentSession.CloseCurrentWindow();
                        this.messageWindow.RTL = this.RTL;
                        this.messageWindow.Width = 300;
                        this.messageWindow.Height = 150;
                        this.messageWindow.Title = TextCodeTranslator.Translate("Shipment.O.Astatementwasrejected");//"הצהרה נדחתה";
                        this.messageWindow.Message = TextCodeTranslator.Translate("Shipment.O.TheRejectionStatementWasSentTo") + SessionLocator.PrivateLableSettings.PrivateLabelShortName;//"דחיית הצהרה נשלח ל -" + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                        this.messageWindow.Show(this.messageWindow.Message);
                    }
                });
            }
        });
    }

    DownloadDocumentFile(item) {
        this._ImageLibraryService.DownloadFile(item.DocumentId, item.FileExtension, item.Folder, SessionLocator.Tenant).subscribe((res:any) => {
            var EntityNumber = "";
            if (this.EntityPm != null) {
                EntityNumber = this.EntityPm.ShipmentNumber;
            }
            var documentName = item.DocumentId + "*" + item.DocumentTypeCode + "-" + (!AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : item.EntityId) + "-" + item.Code;// +"." + CurrentDocument.Extension;


            DownloadManager.DownloadPage(documentName);

        });

    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public get ShipperReference1() { return this.EntityPm.ShipperReference1 }
    public set ShipperReference1(newValue: string) { this.EntityPm.ShipperReference1 = newValue; }

    public get CustomerReference1() { return this.EntityPm.CustomerReference1 }
    public set CustomerReference1(newValue: string) { this.EntityPm.CustomerReference1 = newValue; }

    public get FromPortId() { return this.EntityPm.FromPortId }
    public set FromPortId(newValue: string) { this.EntityPm.FromPortId = newValue; }

    public get CustomsFileNo() { return this.AdditionalData.CustomsFileNo }
    public set CustomsFileNo(newValue: string) { this.AdditionalData.CustomsFileNo = newValue; }

    public get DeclarationNo() { return this.AdditionalData.DeclarationNo }
    public set DeclarationNo(newValue: string) { this.AdditionalData.DeclarationNo = newValue; }

    public get MishgorDescOfGoods1() { return this.AdditionalData.MishgorDescOfGoods1 }
    public set MishgorDescOfGoods1(newValue: string) { this.AdditionalData.MishgorDescOfGoods1 = newValue; }

    public get GoodsValue() { return new CustomNumbersPipe().transform(this.AdditionalData.GoodsValue, 0) }
    public set GoodsValue(newValue: string) { this.AdditionalData.GoodsValue = newValue; }

    public get CifValue() { return new CustomNumbersPipe().transform(this.AdditionalData.CifValue, 0) }
    public set CifValue(newValue: string) { this.AdditionalData.CifValue = newValue; }

    public get TotalTax() { return new CustomNumbersPipe().transform(this.AdditionalData.TotalTax, 0) }
    public set TotalTax(newValue: string) { this.AdditionalData.TotalTax = newValue; }

    public get MishgorPackageQuantity() { return this.AdditionalData.MishgorPackageQuantity }
    public set MishgorPackageQuantity(newValue: string) { this.AdditionalData.MishgorPackageQuantity = newValue; }

    public get MishgorPackageWeight() { return this.AdditionalData.MishgorPackageWeight }
    public set MishgorPackageWeight(newValue: string) { this.AdditionalData.MishgorPackageWeight = newValue; }

    public get IsImporterApprovalRequried() { return this.AdditionalData.IsImporterApprovalRequried }
    public set IsImporterApprovalRequried(newValue: boolean) { this.AdditionalData.IsImporterApprovalRequried = newValue; }

    public get ApprovedByUserName() { return this.AdditionalData.ApprovedByUserName }
    public set ApprovedByUserName(newValue: string) { this.AdditionalData.ApprovedByUserName = newValue; }

    public get Taxtypename() { return this.AdditionalData.Taxtypename }
    public set Taxtypename(newValue: string) { this.AdditionalData.Taxtypename = newValue; }

    public get TaxBasis() { return this.AdditionalData.TaxBasis }
    public set TaxBasis(newValue: string) { this.AdditionalData.TaxBasis = newValue; }

    public get TaxToPay() { return this.AdditionalData.TaxToPay }
    public set TaxToPay(newValue: string) { this.AdditionalData.TaxToPay = newValue; }

    public get TaxAmount() { return this.AdditionalData.TaxAmount }
    public set TaxAmount(newValue: string) { this.AdditionalData.TaxAmount = newValue; }

    public get SupAccount() { return this.AdditionalData.SupAccount }
    public set SupAccount(newValue: string) { this.AdditionalData.SupAccount = newValue; }

    public get IncotermId() { return this.AdditionalData.IncotermId }
    public set IncotermId(newValue: string) { this.AdditionalData.IncotermId = newValue; }

    public get Value() { return this.AdditionalData.Value }
    public set Value(newValue: string) { this.AdditionalData.Value = newValue; }

    public get CurrencyName() { return this.AdditionalData.CurrencyName }
    public set CurrencyName(newValue: string) { this.AdditionalData.CurrencyName = newValue; }

    public get CountryName() { return this.AdditionalData.CountryName }
    public set CountryName(newValue: string) { this.AdditionalData.CountryName = newValue; }

    public get SupplierName() { return this.AdditionalData.SupplierName }
    public set SupplierName(newValue: string) { this.AdditionalData.SupplierName = newValue; }

    public get SupplierFreight() { return this.AdditionalData.SupplierFreight }
    public set SupplierFreight(newValue: string) { this.AdditionalData.SupplierFreight = newValue; }

    //public get ApprovedByUserName() { return this.AdditionalData.ApprovedByUserName }
    //public set ApprovedByUserName(newValue: string) { this.AdditionalData.ApprovedByUserName = newValue; }

    GoodsValueClick() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 722;
        newWindow.Height = 230;
        if (this.Language == 'HB') {
            newWindow.RTL = true;
        }
        else {
            newWindow.RTL = false;
        }
        newWindow.Title = TextCodeTranslator.Translate("Shipment.O.ProviderAccountInformation");//"פרטי חשבון ספק";
        var windowArgs: any = {};
        //windowArgs.IsNew = false;

        windowArgs.AdditionalData = this.AdditionalData;
        newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control); 
      newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/GoodsValueComponent');

    }

    TotalTaxClick() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 550;
        newWindow.Height = 230;
        //if (this.Language == 'HB') {
        //    newWindow.RTL = true;
        //}
        //else {
        newWindow.RTL = this.RTL;
        //}
        newWindow.Title = TextCodeTranslator.Translate("Shipment.O.TaxInformation");//"פרטי מס";
        var windowArgs: any = {};
        //windowArgs.IsNew = false;

        windowArgs.AdditionalData = this.AdditionalData;
        newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control); 
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/TaxScreenComponent');

    }
}
