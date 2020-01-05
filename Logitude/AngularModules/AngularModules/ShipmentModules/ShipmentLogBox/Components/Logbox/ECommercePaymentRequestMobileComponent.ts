declare var System: any, window: any;
import { ShipmentArchiveFilter } from '../../../../Controls/ShipmentArchiveFilter';
import { TransportsFilter } from '../../../../Controls/TransportsFilter';
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SearchTextBox } from '../../../../Controls/SearchTextBox';
import { IconButton } from '../../../../Controls/IconButton';
import { LogGridComponent } from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import { Http, Response } from '@angular/http';
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
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { ShipmentAdditionalCloudDataService } from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { GroupByPipe } from '../../../../Infrastructure/Pipes/GroupByPipe';
import { ImageLibraryService } from '../../../../Common/Services/Others/ImageLibraryService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentTypeMetaDataExtendedService } from '../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService'
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { TenantPMService } from '../../../../Common/Services/StandardPMs/TenantPMService';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager'; 


@Component({
    moduleId: module.id,
    templateUrl: './ECommercePaymentRequestMobileComponent.html'
})

export class ECommercePaymentRequestMobileComponent extends BaseComponent implements OnInit, AfterViewInit {

    DataContext: ECommercePaymentRequestMobileComponent = this;
    private messageWindow: MessageWindow = new MessageWindow();
    EntityPm: ShipmentPM = new ShipmentPM();
    AdditionalData: any = {
        RequestPaymentData: {}, PaymentData: {}};
    externalDocs: any[] = [];

    public _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService; 

    public _ShipmentPMService: ShipmentPMService;
    RefreshTimer: any;
    _ImageLibraryService: ImageLibraryService;
    constructor(private cd: ChangeDetectorRef) {
        super();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        this._ImageLibraryService = new ImageLibraryService();
        this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
        this._ShipmentPMService = new ShipmentPMService();
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
    ngOnInit() {

    }
    ngAfterViewInit() {

    }
    ShowFinalMessage: boolean = false;
    SecurityKey:string = "";
    Tenant : number = null;
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
                    //SessionLocator.ExternalParams.Args.forEach(arg => {
                    //    if (arg.FieldName == 'ShipmentId') {
                    //        ShipmentId = arg.FieldValue; 
                    //    }
                    //});

                    SessionLocator.ClearExternalParams();
                }
            }
        }
        this._ShipmentPMService.getSingleBySecurityKeyTenantWithoutToken(this.SecurityKey, this.Tenant).subscribe(MyResult => {
            if (MyResult.Result) {
                //this.EntityPm = MyResult.Result;
                //this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.EntityPm.Id,Tenant).subscribe(AdditionalResult => {

                this.AdditionalData = MyResult.Result;//AdditionalResult.Result
                if (this.AdditionalData.IsPaymentRequired) {
                    if (this.EntityPm) {

                        var ammount = 0;

                        this.AdditionalData.RequestPaymentData.ServiceTypes.forEach((item, key) => {
                            ammount += +(item.AmountInNIS);
                        });

                        this.TotalAmount = ammount;
                    }
                }
                else {
                    this.FinalMessage = "קובץ זה םינו נדרש לתשלום";
                    this.ShowFinalMessage = true;
                }


                //});
                var service = new CommonDomainService();
                service.GetTenantLogoUri(this.Tenant).subscribe((myLogoResult: any) => {

                    this.CompanyLogo = myLogoResult.Result;

                });
                //GetTenantEcommerceSupportEmail
                service.GetTenantEcommerceSupportEmail(this.Tenant).subscribe((myTenant: any) => {
                    if (myTenant.Result) {
                        this.EcommerceSupportEmail = myTenant.Result;
                    }
                });
                if (this.RefreshTimer) {
                    clearTimeout(this.RefreshTimer);
                }
                
                this.RefreshTimer = setInterval(() => this.ReloadPage(), 1200000);//1200000
            }
            else {
                this.FinalMessage = "התיק לם קיים בסביבה הזו";
                this.ShowFinalMessage = true;
            }
        });

    }
    ReloadPage() {
        var ConfirmResult = confirm("The page has expired. Do you want to refresh it ?");
        if (ConfirmResult == true || ConfirmResult == false) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            this._ShipmentPMService.getSingleBySecurityKeyTenantWithoutToken(this.SecurityKey, this.Tenant).subscribe(MyResult => {
                if (MyResult.Result) {
                    //this.EntityPm = MyResult.Result;
                    //this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.EntityPm.Id,Tenant).subscribe(AdditionalResult => {

                    this.AdditionalData = MyResult.Result;//AdditionalResult.Result
                    if (this.AdditionalData.IsPaymentRequired) {
                        if (this.EntityPm) {

                            var ammount = 0;

                            this.AdditionalData.RequestPaymentData.ServiceTypes.forEach((item, key) => {
                                ammount += +(item.AmountInNIS);
                            });

                            this.TotalAmount = ammount;
                        }
                    }
                    else {
                        this.FinalMessage = "קובץ זה םינו נדרש לתשלום";
                        this.ShowFinalMessage = true;
                    }

                    this.RefreshTimer = setInterval(() => this.ReloadPage(), 1200000);//1200000
                }
                else {
                    this.FinalMessage = "התיק לם קיים בסביבה הזו";
                    this.ShowFinalMessage = true;
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

    public get CustomerName() { return this.AdditionalData.RequestPaymentData.CustomerName }
    public set CustomerName(newValue: string) { this.AdditionalData.RequestPaymentData.CustomerName = newValue; }

    public get CustomerAddress() { return this.AdditionalData.RequestPaymentData.CustomerAddress }
    public set CustomerAddress(newValue: string) { this.AdditionalData.RequestPaymentData.CustomerAddress = newValue; }

    public get Master() { return this.AdditionalData.RequestPaymentData.Master }
    public set Master(newValue: string) { this.AdditionalData.RequestPaymentData.Master = newValue; }

    public get Hawb() { return this.AdditionalData.RequestPaymentData.Hawb }
    public set Hawb(newValue: string) { this.AdditionalData.RequestPaymentData.Hawb = newValue; }

    public get DeclarationNumber() { return new CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData.DeclarationNumber, 0) }
    public set DeclarationNumber(newValue: string) { this.AdditionalData.RequestPaymentData.DeclarationNumber = newValue; }

    public get ShipmentValueInNIS() { return new CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData.ShipmentValueInNIS, 0) }
    public set ShipmentValueInNIS(newValue: string) { this.AdditionalData.RequestPaymentData.ShipmentValueInNIS = newValue; }

    public get SenderDetails() { return this.AdditionalData.RequestPaymentData.SenderDetails }
    public set SenderDetails(newValue: string) { this.AdditionalData.RequestPaymentData.SenderDetails = newValue; }

    public get GoodsDescritpion() { return this.AdditionalData.RequestPaymentData.GoodsDescritpion }
    public set GoodsDescritpion(newValue: string) { this.AdditionalData.RequestPaymentData.GoodsDescritpion = newValue; }

    public get IsImporterApprovalRequried() { return this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried }
    public set IsImporterApprovalRequried(newValue: boolean) { this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried = newValue; }

    public get Quantity() { return this.AdditionalData.RequestPaymentData.Quantity }
    public set Quantity(newValue: string) { this.AdditionalData.RequestPaymentData.Quantity = newValue; }

    public get Weight() { return this.AdditionalData.RequestPaymentData.Weight }
    public set Weight(newValue: string) { this.AdditionalData.RequestPaymentData.Weight = newValue; }

    public get TotalChargesInNIS() { return this.AdditionalData.RequestPaymentData.TotalChargesInNIS }
    public set TotalChargesInNIS(newValue: string) { this.AdditionalData.RequestPaymentData.TotalChargesInNIS = newValue; }

    public get sum() { return this.AdditionalData.PaymentData.sum }
    public set sum(newValue: string) { this.AdditionalData.PaymentData.sum = newValue; }

    public get currency() { return this.AdditionalData.PaymentData.currency }
    public set currency(newValue: string) { this.AdditionalData.PaymentData.currency = newValue; }

    public get op() { return this.AdditionalData.PaymentData.op }
    public set op(newValue: string) { this.AdditionalData.PaymentData.op = newValue; }

    public get DCdisable() { return this.AdditionalData.PaymentData.DCdisable }
    public set DCdisable(newValue: string) { this.AdditionalData.PaymentData.DCdisable = newValue; }

    public get DclickTK() { return this.AdditionalData.PaymentData.DclickTK }
    public set DclickTK(newValue: string) { this.AdditionalData.PaymentData.DclickTK = newValue; }

    public get thtk() { return this.AdditionalData.PaymentData.thtk }
    public set thtk(newValue: string) { this.AdditionalData.PaymentData.thtk = newValue; }

    public get TargetEnv() {
        var Env = "https://direct.tranzila.com/" + this.AdditionalData.PaymentData.TargetEnv + "/";//amitaltest
        return Env;
    }
    public set TargetEnv(newValue: string) { this.AdditionalData.PaymentData.TargetEnv = newValue; }

    public get TermsOfUseDocumentId() { return this.AdditionalData.RequestPaymentData.TermsOfUseDocumentId }
    public set TermsOfUseDocumentId(newValue: string) { this.AdditionalData.RequestPaymentData.TermsOfUseDocumentId = newValue; }



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
        this._documentsFilingExtendedPMService.getDocumentsFilingsByCode(this.TermsOfUseDocumentId).subscribe(myResult => {
             
            if (myResult.Result) { 
                var securityId = myResult.Result.SecurityId;
                DownloadManager.DownloadPage(null, securityId);
            }
        });
      
    }


}
