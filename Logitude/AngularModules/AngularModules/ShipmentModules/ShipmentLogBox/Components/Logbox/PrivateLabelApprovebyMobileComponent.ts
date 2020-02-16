declare var System: any, window: any;
import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {Http, Response} from '@angular/http';
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
import {ShipmentAdditionalCloudDataService} from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
//import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {DocumentTypeMetaDataExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService' 
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
@Component({
    moduleId: module.id,
    templateUrl: './PrivateLabelApprovebyMobileComponent.html'
})

export class PrivateLabelApprovebyMobileComponent extends BaseComponent implements OnInit, AfterViewInit {

    DataContext: PrivateLabelApprovebyMobileComponent = this;
    //private messageWindow: MessageWindow = new MessageWindow();
    EntityPm: ShipmentPM = new ShipmentPM();
    AdditionalData: any = {};
    externalDocs: any[] = [];
    public DimApproveButton: boolean = false;
    public DimDenyButton: boolean = false;
    public _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    public _ShipmentPMService: ShipmentPMService;
    
    _ImageLibraryService: ImageLibraryService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        super();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        this._ImageLibraryService = new ImageLibraryService();
        this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
        this._ShipmentPMService = new ShipmentPMService();
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
    ShowFinalMessage: boolean = false;
    RunComponent() {
        var ForwarderShipmentNumber = "";
        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp") {

                    let me: any = SessionLocator.ExternalParams;
                    if (me.ForwarderShipmentNumber) {
                        ForwarderShipmentNumber = me.ForwarderShipmentNumber; 
                    }
                    //SessionLocator.ExternalParams.Args.forEach(arg => {
                    //    if (arg.FieldName == 'ShipmentId') {
                    //        ShipmentId = arg.FieldValue; 
                    //    }
                    //});
                    
                    SessionLocator.ClearExternalParams();
                }
            }
        }
        this._ShipmentPMService.getSingleByForwarderShipmentNumber(ForwarderShipmentNumber).subscribe(MyResult => {
            if (MyResult.Result) {
                this.EntityPm = MyResult.Result;
                this._ShipmentAdditionalCloudDataService.get(this.EntityPm.Id).subscribe(AdditionalResult => {
                    
                        this.AdditionalData = AdditionalResult.Result
                        if (this.AdditionalData.IsImporterApprovalRequried){
                            if (this.EntityPm) {
                                //this.EntityPm = args.EntityPm;
                                //this.AdditionalData = args.AdditionalData;
                                if (!AppTool.IsNullOrEmpty(this.AdditionalData.DenyReason)) {
                                    this.DimDenyButton = true;
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
                                    this.FinalMessage = "גרסה זו כבר אושרה על ידי משתמש אחר";

                                    this.DimApproveButton = true;
                                }
                                var ObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
                                //this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
                                this._documentsFilingExtendedPMService.getAllDocumentsFilingsByEntityIdAndObjectTable(this.EntityPm.Id, ObjectTable.Id, "I", SessionLocator.Tenant).subscribe(res => {
                                    var Result = [];//DocumentTypeMetaDataExtendedService

                                    Result = res.Result.filter(a => a.IsDeleted == false);


                                    this.externalDocs = [];

                                    var SupplierInvoice = Result.filter(a => a.DocumentTypeCode == "380" || a.DocumentTypeCode == "721");
                                    var Others = Result.filter(a => a.DocumentTypeCode != "721" && a.DocumentTypeCode != "380");
                                    var tempSupplierInvoice = [];
                                    var tempOthers = [];
                                    var DRELID = "";
                                    this._DocumentTypeMetaDataExtendedService.GetDocumentsMetaDataTypeByCode("DREL").subscribe(myResult => {
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
                                    //SupplierInvoice.forEach((mydoc) => {
                                    //    //var DRELTypes = mydoc.DocumentsFilingMetaDataValues.filter(a => a.DocumentsMetaDataTypeId == DRELID);
                                    //    //if (DRELTypes != null && DRELTypes.length > 0) {
                                    //    tempSupplierInvoice.push(mydoc);
                                    //    //}
                                    //});
                                    //Others.forEach((docin) => {
                                    //    //var DRELTypes = docin.DocumentsFilingMetaDataValues.filter(a => a.DocumentsMetaDataTypeId == DRELID);
                                    //    //if (DRELTypes != null && DRELTypes.length > 0) {
                                    //    tempOthers.push(docin);
                                    //    //}
                                    //});

                                    //else {
                                    //    tempSupplierInvoice = SupplierInvoice;
                                    //    tempOthers = Others;
                                    //}
                                    this.externalDocs.push({ key: "חשבונות ספק ורשימות אריזה", value: tempSupplierInvoice });
                                    this.externalDocs.push({ key: "מסמכים נוספים", value: tempOthers });
                                    //this.CurrentSession.CurrentWindow.StopBusyIndicator();

                                }, error => {
                                    var dd: Response = error;
                                });
                                var ammount = 0;

                                this.AdditionalData.TaxesDetails.forEach((item, key) => {
                                    ammount += +(item.TaxAmount);
                                });

                                this.TotalAmount = ammount;
                            }
                        }
                        else {
                            this.FinalMessage = "התיק הנל אינו נדרש לאישור";
                            this.ShowFinalMessage = true;
                        }
                   

                });
            }
            else {
                this.FinalMessage = "התיק לא קיים בסביבה הזו";
                this.ShowFinalMessage = true;
            }
        });
       
    }
    private totalAmount: number = 0;
    public get TotalAmount() { return this.totalAmount }
    public set TotalAmount(newValue: number) { this.totalAmount = newValue; }
    public ValidationWarningsList: string = null;
    public FinalMessage: string = "גרסה זו אושרה";
    ApproveButtonClicked() {
        //this.CurrentSession.CurrentWindow.StartBusyIndicator("Approving ...");
        this.ValidationWarningsList = null;
        this._ShipmentAdditionalCloudDataService.getsingledata(this.EntityPm.Id).subscribe(AdditionalResult => {
            var entity = AdditionalResult.Result
            if (!AppTool.IsNullOrEmpty(entity.ApprovedByUserName) || !AppTool.IsNullOrEmpty(entity.DenyReason)) {
                //this.messageWindow.RTL = true;
                //this.messageWindow.Width = 300;
                //this.messageWindow.Height = 150;
                //this.messageWindow.Title = "אזהרה!";
                //this.messageWindow.Message = "גרסה זו כבר אושרה על ידי משתמש אחר";
                //this.messageWindow.Show(this.messageWindow.Message);
                this.FinalMessage == "גרסה זו כבר אושרה על ידי משתמש אחר";
                //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                //this.CurrentSession.CurrentWindow.StopBusyIndicator(); 
            }
            else {
                entity.ApprovedByUserName = SessionLocator.LoggedUserPM.EnglishName;
                this._ShipmentAdditionalCloudDataService.update(entity).subscribe(AdditionalResult => {
                    ServiceLocator.SendTotangoUserActivity("LogBox", "Approve Declaration");
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
                    //this.messageWindow.RTL = true;
                    //this.messageWindow.Width = 300;
                    //this.messageWindow.Height = 150;
                    //this.messageWindow.Title = "הצהרה אושרה";
                    //this.messageWindow.Message = "אישור הצהרה נשלח ל -" + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                    //this.messageWindow.Show(this.messageWindow.Message);
                    this.FinalMessage == "אישור הצהרה נשלח ל -" + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                    //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                    //this.CurrentSession.CurrentWindow.StopBusyIndicator();
                });
            }
        });

    }
   

    public get DenyReason() { return this.AdditionalData.DenyReason }
    public set DenyReason(newValue: string) { this.AdditionalData.DenyReason = newValue; }

    DownloadDocumentFile(item) {
        //this._ImageLibraryService.DownloadFile(item.DocumentId, item.FileExtension, item.Folder, SessionLocator.Tenant).subscribe(res => {
            var EntityNumber = "";
            if (this.EntityPm != null) {
                EntityNumber = this.EntityPm.ShipmentNumber;
            }
            var documentName = item.DocumentId + "*" + item.DocumentTypeCode + "-" + (!AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : item.EntityId) + "-" + item.Code;// +"." + CurrentDocument.Extension;


            DownloadManager.DownloadPage(documentName);

        //});

    }

    CloseButtonClicked() {
        //this.CurrentSession.CloseCurrentWindow();
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
    ShowGoodsScreen: boolean = false;
    ShowTaxesScreen: boolean = false;
    ShowDenyScreen: boolean = false;

    GoodsValueClick() {
        this.ShowGoodsScreen = true;
        //var newWindow = new LogitudeWindow();
        //newWindow.Width = 722;
        //newWindow.Height = 230;
        //newWindow.RTL = true;
        //newWindow.Title = "פרטי חשבון ספק";
        //var windowArgs: any = {};
        //windowArgs.IsNew = false;

        //windowArgs.AdditionalData = this.AdditionalData;
        //newWindow.WindowArgs = windowArgs;
        ////newWindow.Add(control); 
        //newWindow.Show('./Shipment/Components/Logbox/GoodsValueComponent');

    }

    TotalTaxClick() {
        this.ShowTaxesScreen = true;
        //var newWindow = new LogitudeWindow();
        //newWindow.Width = 550;
        //newWindow.Height = 230;
        //newWindow.RTL = true;
        //newWindow.Title = "פרטי מס";
        //var windowArgs: any = {};
        ////windowArgs.IsNew = false;

        //windowArgs.AdditionalData = this.AdditionalData;
        //newWindow.WindowArgs = windowArgs;
        ////newWindow.Add(control); 
        //newWindow.Show('./Shipment/Components/Logbox/TaxScreenComponent');

    }

    CloseGoodsButtonClicked() {
        this.ShowGoodsScreen = false;
    }

    CloseTaxesButtonClicked() {
        this.ShowTaxesScreen = false;
    }

    ValidationErrorsList: any[];
    MyAdditionalData: any = null;
    DenyButtonClicked() {
        //this.CurrentSession.CurrentWindow.StartBusyIndicator("...");
        //var newWindow = new LogitudeWindow();
        //newWindow.Width = 350;
        //newWindow.Height = 220;
        //newWindow.RTL = true;

        this._ShipmentAdditionalCloudDataService.getsingledata(this.EntityPm.Id).subscribe(AdditionalResult => {
            var entity = AdditionalResult.Result
            this.MyAdditionalData = AdditionalResult.Result;
            if (!AppTool.IsNullOrEmpty(entity.DenyReason) || !AppTool.IsNullOrEmpty(entity.ApprovedByUserName)) {
                //this.messageWindow.RTL = true;
                //this.messageWindow.Width = 300;
                //this.messageWindow.Height = 150;
                //this.messageWindow.Title = "אזהרה!";
                this.FinalMessage = "גרסה זו כבר נדחתה על ידי משתמש אחר";
                //this.messageWindow.Show(this.messageWindow.Message);
                //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                //this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
            else {
                this.ShowDenyScreen = true;
                //newWindow.Title = "הסבר לדחיית הצהרה";
                ////this.CurrentSession.CurrentWindow.StopBusyIndicator();
                //var windowArgs: any = {};
                //windowArgs.AdditionalData = entity;
                //newWindow.WindowArgs = windowArgs;
                ////newWindow.Add(control); 
                //newWindow.Show('./Shipment/Components/Logbox/DenyReasonComponent');
                //newWindow.WindowClosed.subscribe(($event: any) => {
                //    if ($event == "Denied") {
                //        ServiceLocator.SendTotangoUserActivity("LogBox", "Deny Declaration");
                //        this.DimDenyButton = true;
                //        //this.CurrentSession.CloseCurrentWindow();
                //        this.messageWindow.RTL = true;
                //        this.messageWindow.Width = 300;
                //        this.messageWindow.Height = 150;
                //        this.messageWindow.Title = "הצהרה נדחתה";
                //        this.messageWindow.Message = "דחיית הצהרה נשלח ל -" + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                //        this.messageWindow.Show(this.messageWindow.Message);
                //    }
                //});
            }
        });
    }

    SendButtonClicked() {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.DenyReason)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "DenyReason"));
        }
        if (this.ValidationErrorsList.length == 0) {
            //this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.MyAdditionalData.IsImporterApprovalRequried = false;
            this.MyAdditionalData.DenyReason = SessionLocator.LoggedUserPM.EnglishName + ", " + SessionLocator.LoggedUserPM.LocalName + ", " + SessionLocator.LoggedUserPM.Email + ", " + this.DenyReason + ", " + this.MyAdditionalData.VersionApproved;
            this._ShipmentAdditionalCloudDataService.update(this.MyAdditionalData).subscribe(AdditionalResult => {
                //this.CurrentSession.CurrentWindow.StopBusyIndicator();
                //this.CurrentSession.CloseCurrentWindowEmit("Denied");
                this.FinalMessage = "דחיית הצהרה נשלח ל -" + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                this.DimDenyButton = true;
                this.ShowDenyScreen = false;
            });
        }
    }

    CloseDenyButtonClicked() {
        this.ShowDenyScreen = false;
    }
}
