declare var System: any, window: any;
import { ShipmentArchiveFilter } from '../../../../Controls/ShipmentArchiveFilter';
import { TransportsFilter } from '../../../../Controls/TransportsFilter';
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef, isDevMode } from '@angular/core';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SearchTextBox } from '../../../../Controls/SearchTextBox';
import { IconButton } from '../../../../Controls/IconButton';
import { LogGridComponent } from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogBoxDocumentsComponent } from './LogBoxDocumentsComponent';
import { ShipmentDomainService, ImporterQueriesDataCounts } from '../../../../Shipment/Services/ShipmentDomainService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { CustomNumbersPipe } from '../../../../Infrastructure/Pipes/CustomNumbersPipe';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityStatusListService } from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import { BranchListService } from '../../../../Common/Services/StandardLists/BranchListService';
import { DepartmentListService } from '../../../../Common/Services/StandardLists/DepartmentListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { ShipmentAdditionalCloudDataService } from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { GroupByPipe } from '../../../../Infrastructure/Pipes/GroupByPipe';
import { ImageLibraryService } from '../../../../Common/Services/Others/ImageLibraryService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
//import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentTypeMetaDataExtendedService } from '../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService'
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { TenantPMService } from '../../../../Common/Services/StandardPMs/TenantPMService';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { DatePipe } from '@angular/common';
import { Subject } from 'rxjs';



@Component({

    templateUrl: './UserIdNumberMobileComponent.html',
    styleUrls: [
        './mobilePayments.scss',
        './UserIdNumberMobileComponent.scss',
    ]
})

export class UserIdNumberMobileComponent extends BaseComponent implements OnInit, AfterViewInit {

    DataContext: UserIdNumberMobileComponent = this;
    // private messageWindow: MessageWindow = new MessageWindow();
    EntityPm: ShipmentPM = new ShipmentPM();
    AdditionalData: any = {};
    externalDocs: any[] = [];

    public _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    public qaIndicator: number = 0;

    public _ShipmentPMService: ShipmentPMService;
    private datePipe: DatePipe;
    RefreshTimer: any;
    _ImageLibraryService: ImageLibraryService;
    orianStyle: boolean = false;
    showDialogUserAcceptSave: boolean = false;
    $userAcceptSave: Subject<boolean> = new Subject<boolean>();

    constructor(private cd: ChangeDetectorRef) {
        super();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        this._ImageLibraryService = new ImageLibraryService();
        this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
        this._ShipmentPMService = new ShipmentPMService();
        this.datePipe = new DatePipe("en-US");
        //this.AdditionalData.RequestPaymentData = {};
        //this.AdditionalData.PaymentData = {};

        //this.AdditionalData.RequestPaymentData.ServiceTypes = [];
    }

    private isAccepted: boolean = false;
    public get IsAccepted() { return this.isAccepted }
    public set IsAccepted(newValue: boolean) { this.isAccepted = newValue; }

    IsAcceptedChanged($event) {
        this.IsAccepted = $event;
    }

    public ScreenWidth: number;
    private MaxScreenWidth: number = 600;

    ngOnInit() {
        this.ScreenWidth = window.innerWidth > this.MaxScreenWidth ? this.MaxScreenWidth : window.innerWidth;
    }
    ngAfterViewInit() {

    }
    ShowFinalMessage: boolean = false;
    SecurityKey: string = "";
    Tenant: number = null;
    HandleTenant(){
        this.orianStyle = +this.Tenant === 126 || +this.Tenant === 1153;
        if (this.orianStyle)
            this.getEcommerceSupportEmail()
    }
    RunComponent() {

        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "uid") {

                    let me: any = SessionLocator.ExternalParams;
                    if (me.SecurityKey) {
                        this.SecurityKey = me.SecurityKey;
                    }
                    if (me.Tenant) {
                        this.Tenant = me.Tenant;
                    }
                    this.HandleTenant();
                    //SessionLocator.ExternalParams.Args.forEach(arg => {
                    //    if (arg.FieldName == 'ShipmentId') {
                    //        ShipmentId = arg.FieldValue; 
                    //    }
                    //});

                    SessionLocator.ClearExternalParams();
                }
            }
        }
        this.StartBusyIndicator("Loading ...");
        this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.SecurityKey, this.Tenant).subscribe((myAdditionalResult: any) => {
            var entity = myAdditionalResult.Result;//AdditionalResult.Result
            if (entity && entity.IsUserIDNumberRequired == false) {
                var myMessage = AppTool.IsNullOrEmpty(entity.UserIdNumber) ? "לא נדרשת השלמת תעודת זהות למשלוח זה" : "הפרטים נשמרו בהצלחה";
                if (entity.UserIdNumberUpdateDate != null) {

                    //var myDateParts = DateTool.GetDateParts(entity.UserIdNumberUpdateDate);
                    //var LocalDateString = myDateParts.DateObject.toLocaleDateString();

                    var formatedUpdateDate = this.datePipe.transform(entity.UserIdNumberUpdateDate, 'dd/MM/yyyy');
                    myMessage = myMessage + " " + formatedUpdateDate;
                }
                this.FinalMessage = myMessage;
                this.ShowFinalMessage = true;
                this.StopBusyIndicator();
            }
            else {
                this._ShipmentPMService.getUserIdDetailsByShipmentSecurityKeyWithoutToken(this.SecurityKey, this.Tenant).subscribe((MyResult: any) => {
                    if (MyResult.Result) {
                        this.AdditionalData = MyResult.Result;//AdditionalResult.Result
                        this.Tenant = this.AdditionalData.Tenant;
                        this.HandleTenant();
                        this.qaIndicator = 1;
                        var service = new CommonDomainService();
                        service.GetTenantLogoUri(this.Tenant).subscribe((myLogoResult: any) => {

                            this.CompanyLogo = myLogoResult.Result;
                            this.StopBusyIndicator();

                        });

                    }
                    else {
                        this.FinalMessage = "התיק לא קיים בסביבה הזו";
                        this.ShowFinalMessage = true;
                        this.StopBusyIndicator();
                    }
                });
            }
        });


    }

    private companyLogo: string = "";
    public get CompanyLogo() { return this.companyLogo }
    public set CompanyLogo(newValue: string) { this.companyLogo = newValue; }

    public ValidationList: string[] = [];
    public FinalMessage: string = "הפרטים נשמרו בהצלחה";



    public get CustomerName() { return this.AdditionalData.CustomerName }
    public set CustomerName(newValue: string) { this.AdditionalData.CustomerName = newValue; }

    public get CustomerAddress() { return this.AdditionalData.CustomerAddress }
    public set CustomerAddress(newValue: string) { this.AdditionalData.CustomerAddress = newValue; }



    public get Hawb() { return this.AdditionalData.Hawb }
    public set Hawb(newValue: string) { this.AdditionalData.Hawb = newValue; }

    public get ShipmentValueInNIS() { return this.orianStyle ? this.AdditionalData.ShipmentValueInNIS : new CustomNumbersPipe().transform(this.AdditionalData.ShipmentValueInNIS, 0) }
    public set ShipmentValueInNIS(newValue: string) { this.AdditionalData.ShipmentValueInNIS = newValue; }

    public get SenderDetails() { return this.AdditionalData.SenderDetails }
    public set SenderDetails(newValue: string) { this.AdditionalData.SenderDetails = newValue; }

    public get GoodsDescritpion() { return this.AdditionalData.GoodsDescritpion }
    public set GoodsDescritpion(newValue: string) { this.AdditionalData.GoodsDescritpion = newValue; }

    //public get IsImporterApprovalRequried() { return this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried }
    //public set IsImporterApprovalRequried(newValue: boolean) { this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried = newValue; }

    public get Quantity() { return this.AdditionalData.Quantity }
    public set Quantity(newValue: string) { this.AdditionalData.Quantity = newValue; }

    public get Weight() { return this.AdditionalData.Weight }
    public set Weight(newValue: string) { this.AdditionalData.Weight = newValue; }

    public get ShipmentId() { return this.AdditionalData.Id }
    public set ShipmentId(newValue: string) { this.AdditionalData.Id = newValue; }

    private userIdNumber;
    public get UserIdNumber() { return this.userIdNumber }
    public set UserIdNumber(newValue: string) { this.userIdNumber = newValue; }

    private ecommerceSupportEmail: string = "";
    public get EcommerceSupportEmail() { return this.ecommerceSupportEmail }
    public set EcommerceSupportEmail(newValue: string) { this.ecommerceSupportEmail = newValue; }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    ShowPaymentDetailsScreen: boolean = false;

    PaymentDetailsClick() {
        this.ShowPaymentDetailsScreen = true;

    }

    ClosePaymentDetailsButtonClicked() {
        this.ShowPaymentDetailsScreen = false;
    }

    ValidationErrorsList: any[];
    MyAdditionalData: any = null;



    SendButtonClicked() {
        this.ValidationList = [];
        this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.SecurityKey, this.Tenant).subscribe(async (myAdditionalResult: any) => {
            var entity = myAdditionalResult.Result;//AdditionalResult.Result
            if (entity.IsUserIDNumberRequired == false) {
                var myMessage = AppTool.IsNullOrEmpty(this.UserIdNumber) ? "לא נדרשת השלמת תעודת זהות למשלוח זה" : "הפרטים נשמרו בהצלחה";
                if (entity.UserIdNumberUpdateDate != null) {
                    var formatedUpdateDate = this.datePipe.transform(entity.UserIdNumberUpdateDate, 'dd/MM/yyyy');
                    myMessage = myMessage + " " + formatedUpdateDate;
                }
                this.FinalMessage == myMessage;
                this.ShowFinalMessage = true;

            }
            else {
                if (!AppTool.IsNullOrEmpty(this.UserIdNumber)) {
                    this.UserIdNumber = this.PadStringWithZerosUpTo9Digits(this.UserIdNumber);
                    entity.UserIdNumberUpdateDate = new Date();
                    entity.IsUserIDNumberRequired = false;
                    entity.UserIdNumber = this.UserIdNumber;
                    if (this.IsValidIsraeliID(this.UserIdNumber)) {

                        entity.UserAcceptSaveID = this.orianStyle ? await this.checkUserAcceptSave() : 0;

                        this._ShipmentAdditionalCloudDataService.updateUserID(entity).subscribe((AdditionalResult: any) => {
                            if (!AdditionalResult.HasError) {
                                this.FinalMessage == "זיהוי משתמש נשלח בהצלחה ";
                                this.ShowFinalMessage = true;
                            }
                            else {
                                this.ValidationList.push("Error");
                            }

                        });
                    }
                    else {
                        this.ValidationList.push("נם להקליד ת.ז תקנית בעלת 9 ספרות");
                    }
                }
                else {
                    this.ValidationList.push("נם להקליד ת.ז תקנית בעלת 9 ספרות");
                }
            }


        });


    }

    IsValidIsraeliID(userIdNumber) {
        var id = String(userIdNumber).trim();
        if (id.length > 9 || id.length < 5 || isNaN(parseInt(id))) return false;

        // Pad string with zeros up to 9 digits
        id = id.length < 9 ? ("00000000" + id).slice(-9) : id;

        return Array
            .from(id, Number)
            .reduce((counter, digit, i) => {
                const step = digit * ((i % 2) + 1);
                return counter + (step > 9 ? step - 9 : step);
            }) % 10 === 0;
    }

    PadStringWithZerosUpTo9Digits(value) {
        let userIdNumber = value;
        let id = String(userIdNumber).trim();
        if (id.length <= 9 && id.length >= 5) {
            userIdNumber = id.length < 9 ? ("00000000" + id).slice(-9) : id;
        }

        return userIdNumber;
    }

    private getEcommerceSupportEmail() {
        new CommonDomainService().GetTenantEcommerceSupportEmailByShipmentSecurityKey(this.Tenant, this.SecurityKey).subscribe((myTenant: any) => {
            if (myTenant.Result)
                this.EcommerceSupportEmail = myTenant.Result;
        });
    }

    async checkUserAcceptSave(): Promise<boolean> {
        this.showDialogUserAcceptSave = true;

        return await new Promise(res => {
            const subscribtion = this.$userAcceptSave.subscribe((userAcceptSave: boolean) => {
                subscribtion.unsubscribe();
                this.showDialogUserAcceptSave = false;
                res(userAcceptSave);
            })
        })
    }
}
