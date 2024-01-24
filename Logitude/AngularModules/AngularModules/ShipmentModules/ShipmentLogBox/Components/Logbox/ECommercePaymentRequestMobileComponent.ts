declare var System: any, window: any;
import { ShipmentArchiveFilter } from '../../../../Controls/ShipmentArchiveFilter';
import { TransportsFilter } from '../../../../Controls/TransportsFilter';
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SearchTextBox } from '../../../../Controls/SearchTextBox';
import { IconButton } from '../../../../Controls/IconButton';
import { LogGridComponent } from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogBoxDocumentsComponent } from './LogBoxDocumentsComponent';
import { ShipmentDomainService, ImporterQueriesDataCounts } from '../../../../Shipment/Services/ShipmentDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { CustomNumbersPipe } from '../../../../Infrastructure/Pipes/CustomNumbersPipe';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityStatusListService } from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import { BranchListService } from '../../../../Common/Services/StandardLists/BranchListService';
import { DepartmentListService } from '../../../../Common/Services/StandardLists/DepartmentListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ShipmentPMService, UrlAndLogo } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
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
import { TenantManagementPM } from 'Infrastructure/EntityPMs/TenantManagementPM';


@Component({    
    templateUrl: './ECommercePaymentRequestMobileComponent.html',
    styleUrls: ['./mobilePayments.scss', './detailsMobile.scss']
})

export class ECommercePaymentRequestMobileComponent extends BaseComponent implements OnInit, AfterViewInit {
    public DimDenyButton: boolean = false;
    public DimApproveButton: boolean = false;
    public orianStyle: boolean = false;
    public dsvStyle: boolean = false;

    DataContext: ECommercePaymentRequestMobileComponent = this;
    //private messageWindow: MessageWindow = new MessageWindow();
    EntityPm: ShipmentPM = new ShipmentPM();
    AdditionalData: any = {
        RequestPaymentData: {}, PaymentData: {}
    };
    externalDocs: any[] = [];

    public _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;

    public _ShipmentPMService: ShipmentPMService;
    RefreshTimer: any;
    private datePipe: DatePipe;
    _ImageLibraryService: ImageLibraryService;
    tenantManagement: TenantManagementPM;
    logoImg: string = '';
    logoUrl: string = '';
    serviceAgreementURL: string = '';

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

    ScreenWidth: number;
    private MaxScreenWidth: number = 600;
    FontTitleSize: number;

    IsAcceptedChanged($event) {
        this.IsAccepted = $event;
    }
    ngOnInit() {

        this.ScreenWidth = window.innerWidth > this.MaxScreenWidth ? this.MaxScreenWidth : window.innerWidth;;
        this.FontTitleSize = this.ScreenWidth > 436 ? 20 : this.ScreenWidth > 390 ? 17 : 15;
    }
    ngAfterViewInit() {

    }
    ShowFinalMessage: boolean = false;
    ShowErrorMessage: boolean = false;
    SecurityKey: string = "";
    Tenant: number = null;
    WhatsAppMessagingNumber: string = "00";
    ShowWhatsAppIcon: boolean = false;

    RunComponent() {

        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq") {

                    let me: any = SessionLocator.ExternalParams;
                    if (me.SecurityKey) {
                        this.SecurityKey = me.SecurityKey;
                    }
                    if (me.Tenant) {
                        this.Tenant = me.Tenant;
                    }

                    this.orianStyle = +this.Tenant === 126 || +this.Tenant === 1153;
                    this.dsvStyle = +this.Tenant === 49 || +this.Tenant === 1062;

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
        this._ShipmentPMService.getSingleBySecurityKeyTenantWithoutToken(this.SecurityKey, this.Tenant).subscribe((MyResult:any) => {
            this.MapFieldsFromResponseData(MyResult);
            if (MyResult.Result) {
                //this.EntityPm = MyResult.Result;
                //this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.EntityPm.Id,Tenant).subscribe((AdditionalResult:any) => {

                this.AdditionalData = MyResult.Result;//AdditionalResult.Result
                if (this.AdditionalData.IsPaymentRequired) {
                    if (this.EntityPm) {
                        
                        this.SetTotalAmountInNIS();
                    }
                }
                else {
                    var myMessage = "משלוח זה כבר שולם בתאריך";
                    if (this.AdditionalData.PaymentDateTime != null) {
                        var formatedPaymentDateTime = this.datePipe.transform(this.AdditionalData.PaymentDateTime, 'dd/MM/yyyy');
                        myMessage = myMessage + " " + formatedPaymentDateTime;
                    }
                    this.FinalMessage = myMessage;
                    this.ShowFinalMessage = true;
                }


                //});
                var service = new CommonDomainService();
                service.GetTenantLogoUriByShipmentSecurityKey(this.Tenant, this.SecurityKey).subscribe((myLogoResult: any) => {

                    this.CompanyLogo = myLogoResult.Result;
                    this.StopBusyIndicator();

                });
                //GetTenantEcommerceSupportEmail
                service.GetTenantEcommerceSupportEmailByShipmentSecurityKey(this.Tenant, this.SecurityKey).subscribe((myTenant: any) => {
                    if (myTenant.Result) {
                        this.EcommerceSupportEmail = myTenant.Result;
                    }
                });
                if (this.RefreshTimer) {
                    clearTimeout(this.RefreshTimer);
                }
                //this.StopBusyIndicator();
                this.RefreshTimer = setInterval(() => this.ReloadPage(), 1200000);//1200000
            }
            else {
                this.FinalMessage = "התיק לא קיים בסביבה הזו";
                this.ShowFinalMessage = true;
                this.ShowErrorMessage = true;
                this.StopBusyIndicator();
            }
        });

        if(this.dsvStyle)
            this.initTenantManagements(this.SecurityKey)
    }

    MapFieldsFromResponseData(responseResult) {
        if (responseResult.Data) {
            this.WhatsAppMessagingNumber = responseResult.Data;
            this.SetShowWatsAppIcon();
        }
    }

    private SetShowWatsAppIcon() {
        if (this.WhatsAppMessagingNumber != null) {
            this.ShowWhatsAppIcon = true;
        }
    }

    private SetTotalAmountInNIS() {
        let ammount = 0;
        let maxDigitsAfterPoint = 0;
        this.AdditionalData.RequestPaymentData?.ServiceTypes.forEach((item, key) => {
            let splitItemAmount = item.AmountInNIS?.toString()?.split('.');
            if (splitItemAmount != null && splitItemAmount.length > 1 && splitItemAmount[1].length > maxDigitsAfterPoint) {
                maxDigitsAfterPoint = splitItemAmount[1].length;
            }
            ammount += +(item.AmountInNIS);
        });

        this.TotalAmount = +ammount.toFixed(maxDigitsAfterPoint);
    }

    ReloadPage() {
        var ConfirmResult = confirm("The page has expired. Do you want to refresh it ?");
        if (ConfirmResult == true || ConfirmResult == false) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            this._ShipmentPMService.getSingleBySecurityKeyTenantWithoutToken(this.SecurityKey, this.Tenant).subscribe((MyResult:any) => {
                this.MapFieldsFromResponseData(MyResult);
                if (MyResult.Result) {
                    //this.EntityPm = MyResult.Result;
                    //this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.EntityPm.Id,Tenant).subscribe((AdditionalResult:any) => {

                    this.AdditionalData = MyResult.Result;//AdditionalResult.Result
                    if (this.AdditionalData.IsPaymentRequired) {
                        if (this.EntityPm) {

                            this.SetTotalAmountInNIS();
                        }
                    }
                    else {
                        var myMessage = "משלוח זה כבר שולם בתםריך";
                        if (this.AdditionalData.PaymentDateTime != null) {
                            var formatedPaymentDateTime = this.datePipe.transform(this.AdditionalData.PaymentDateTime, 'dd/MM/yyyy');
                            myMessage = myMessage + " " + formatedPaymentDateTime;
                        }
                        this.FinalMessage = myMessage;
                        this.ShowFinalMessage = true;
                    }

                    this.RefreshTimer = setInterval(() => this.ReloadPage(), 1200000);//1200000
                }
                else {
                    this.FinalMessage = "התיק לא קיים בסביבה הזו";
                    this.ShowFinalMessage = true;
                    this.ShowErrorMessage = true;
                }
            });
        }
    }
    private companyLogo: string = "";
    public get CompanyLogo() { return this.companyLogo }
    public set CompanyLogo(newValue: string) { this.companyLogo = newValue; }
    private totalAmount: number = 0;
    public get TotalAmount() { return this.totalAmount }
    public set TotalAmount(newValue: number) { this.totalAmount = newValue; }
    public ValidationWarningsList: string = null;
    public FinalMessage: string = "גרסה זו םושרה";

    private ecommerceSupportEmail: string = "";
    public get EcommerceSupportEmail() { return this.ecommerceSupportEmail }
    public set EcommerceSupportEmail(newValue: string) { this.ecommerceSupportEmail = newValue; }

    public get CustomerName() { return this.AdditionalData.RequestPaymentData?.CustomerName }
    public set CustomerName(newValue: string) { this.AdditionalData.RequestPaymentData.CustomerName = newValue; }

    public get CustomerAddress() { return this.AdditionalData.RequestPaymentData?.CustomerAddress }
    public set CustomerAddress(newValue: string) { this.AdditionalData.RequestPaymentData.CustomerAddress = newValue; }

    public get Master() { return this.AdditionalData.RequestPaymentData?.Master }
    public set Master(newValue: string) { this.AdditionalData.RequestPaymentData.Master = newValue; }

    public get Hawb() { return this.AdditionalData.RequestPaymentData?.Hawb }
    public set Hawb(newValue: string) { this.AdditionalData.RequestPaymentData.Hawb = newValue; }

    public get DeclarationNumber() { return new CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData?.DeclarationNumber, 0) }
    public set DeclarationNumber(newValue: string) { this.AdditionalData.RequestPaymentData.DeclarationNumber = newValue; }

    public get ShipmentValueInNIS() { return new CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData?.ShipmentValueInNIS, 0) }
    public set ShipmentValueInNIS(newValue: string) { this.AdditionalData.RequestPaymentData.ShipmentValueInNIS = newValue; }

    public get ForeignCurrencyValue() { return this.orianStyle ? this.AdditionalData.RequestPaymentData?.ForeignCurrencyValue : new CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData.ForeignCurrencyValue, 0) }
    public set ForeignCurrencyValue(newValue: string) { this.AdditionalData.RequestPaymentData.ForeignCurrencyValue = newValue; }

    public get ForeignCurrency() { return this.AdditionalData.RequestPaymentData?.ForeignCurrency }
    public set ForeignCurrency(newValue: string) { this.AdditionalData.RequestPaymentData.ForeignCurrency = newValue; }

    public get SenderDetails() { return this.AdditionalData.RequestPaymentData?.SenderDetails }
    public set SenderDetails(newValue: string) { this.AdditionalData.RequestPaymentData.SenderDetails = newValue; }

    public get GoodsDescritpion() { return this.AdditionalData.RequestPaymentData?.GoodsDescritpion }
    public set GoodsDescritpion(newValue: string) { this.AdditionalData.RequestPaymentData.GoodsDescritpion = newValue; }

    public get IsImporterApprovalRequried() { return this.AdditionalData.RequestPaymentData?.IsImporterApprovalRequried }
    public set IsImporterApprovalRequried(newValue: boolean) { this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried = newValue; }

    public get Quantity() { return this.AdditionalData.RequestPaymentData?.Quantity }
    public set Quantity(newValue: string) { this.AdditionalData.RequestPaymentData.Quantity = newValue; }

    public get Weight() { return this.AdditionalData.RequestPaymentData?.Weight }
    public set Weight(newValue: string) { this.AdditionalData.RequestPaymentData.Weight = newValue; }

    public get TotalChargesInNIS() { return this.AdditionalData.RequestPaymentData?.TotalChargesInNIS }
    public set TotalChargesInNIS(newValue: string) { this.AdditionalData.RequestPaymentData.TotalChargesInNIS = newValue; }

    public get sum() { return this.AdditionalData.PaymentData?.sum }
    public set sum(newValue: string) { this.AdditionalData.PaymentData.sum = newValue; }

    public get currency() { return this.AdditionalData.PaymentData?.currency }
    public set currency(newValue: string) { this.AdditionalData.PaymentData.currency = newValue; }

    public get op() { return this.AdditionalData.PaymentData?.op }
    public set op(newValue: string) { this.AdditionalData.PaymentData.op = newValue; }

    public get DCdisable() { return this.AdditionalData.PaymentData?.DCdisable }
    public set DCdisable(newValue: string) { this.AdditionalData.PaymentData.DCdisable = newValue; }

    public get DclickTK() { return this.AdditionalData.PaymentData?.DclickTK }
    public set DclickTK(newValue: string) { this.AdditionalData.PaymentData.DclickTK = newValue; }

    public get thtk() { return this.AdditionalData.PaymentData?.thtk }
    public set thtk(newValue: string) { this.AdditionalData.PaymentData.thtk = newValue; }

    public get TargetEnv() {
        //todo:liron add bit parameter
        let directTranzilaLink = this.GetDirectTranzilaLink();
        var Env = directTranzilaLink + this.AdditionalData.PaymentData?.TargetEnv + "/";//amitaltest
        return Env;
    }
    private GetDirectTranzilaLink() {
        if (false) {
            return "https://direct2.tranzila.com/";
        }
        return "https://direct.tranzila.com/";
    }

    public set TargetEnv(newValue: string) { this.AdditionalData.PaymentData.TargetEnv = newValue; }

    public get TermsOfUseDocumentId() { return this.AdditionalData.RequestPaymentData?.TermsOfUseDocumentId }
    public set TermsOfUseDocumentId(newValue: string) { this.AdditionalData.RequestPaymentData.TermsOfUseDocumentId = newValue; }

    public get u71() { return this.AdditionalData.PaymentData?.u71 }
    public set u71(newValue: string) { this.AdditionalData.PaymentData.u71 = newValue; }

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

    OnPayClick() {
        //alert("Yes");
        document.forms["form"].action = this.TargetEnv
        document.forms["form"].submit();
    }
    IsAgreed: boolean = false;
    IsAggreeChicked(isAgreed) {
        this.IsAgreed = isAgreed;
    }

    ViewAggreement() {
        //this._documentsFilingExtendedPMService.getDocumentsFilingsByCode(this.TermsOfUseDocumentId).subscribe((myResult:any) => {

        //if (myResult.Result) { 
        //var securityId = myResult.Result.SecurityId;

        if(this.serviceAgreementURL)
            open(this.serviceAgreementURL)
        else
            DownloadManager.DownloadExternalPage(null, this.Tenant, this.TermsOfUseDocumentId);
        //  }
        //});

    }
    
    async initTenantManagements(securityKey: string) {
        const data: UrlAndLogo = await this._ShipmentPMService.getLogoAndUrlWithoutToken(securityKey)
        this.logoImg = data.logo;
        this.logoUrl = data.url;
        this.serviceAgreementURL = data.serviceAgreementURL;
    }

    openLogoUrl() {
        if(this.logoUrl)
            window.open(this.logoUrl)
    }
}
