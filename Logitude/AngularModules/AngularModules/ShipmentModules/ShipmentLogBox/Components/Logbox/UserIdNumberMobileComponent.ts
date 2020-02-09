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
    templateUrl: './UserIdNumberMobileComponent.html'
})

export class UserIdNumberMobileComponent extends BaseComponent implements OnInit, AfterViewInit {

    DataContext: UserIdNumberMobileComponent = this;
    private messageWindow: MessageWindow = new MessageWindow();
    EntityPm: ShipmentPM = new ShipmentPM();
    AdditionalData: any = {};
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
    SecurityKey: string = "";
    Tenant: number = null;
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
                    //SessionLocator.ExternalParams.Args.forEach(arg => {
                    //    if (arg.FieldName == 'ShipmentId') {
                    //        ShipmentId = arg.FieldValue; 
                    //    }
                    //});

                    SessionLocator.ClearExternalParams();
                }
            }
        }
        this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.SecurityKey, this.Tenant).subscribe(myAdditionalResult => {

            var entity = myAdditionalResult.Result;//AdditionalResult.Result
            if (entity.IsUserIDNumberRequired == false) {
                var myMessage = "תעודת זהות כבר הוזנה למשלוח זה";
                if (entity.UserIdNumberUpdateDateTime != null) {
                    myMessage = myMessage + " " + entity.UserIdNumberUpdateDateTime;
                }
                this.FinalMessage == myMessage;
                this.ShowFinalMessage = true;

            }
            else {
                this._ShipmentPMService.getUserIdDetailsByShipmentSecurityKeyWithoutToken(this.SecurityKey, this.Tenant).subscribe(MyResult => {
                    if (MyResult.Result) {
                       
                        this.AdditionalData = MyResult.Result;//AdditionalResult.Result
 
                        var service = new CommonDomainService();
                        service.GetTenantLogoUri(this.Tenant).subscribe((myLogoResult: any) => {

                            this.CompanyLogo = myLogoResult.Result;

                        });
                 
                    }
                    else {
                        this.FinalMessage = "התיק לם קיים בסביבה הזו";
                        this.ShowFinalMessage = true;
                    }
                });
            }
        });
        

    }

    private companyLogo: string = "";
    public get CompanyLogo() { return this.companyLogo }
    public set CompanyLogo(newValue: string) { this.companyLogo = newValue; }

    public ValidationWarningsList: string = null;
    public FinalMessage: string = "תעודת זהות כבר הוזנה למשלוח זה";



    public get CustomerName() { return this.AdditionalData.CustomerName }
    public set CustomerName(newValue: string) { this.AdditionalData.CustomerName = newValue; }

    public get CustomerAddress() { return this.AdditionalData.CustomerAddress }
    public set CustomerAddress(newValue: string) { this.AdditionalData.CustomerAddress = newValue; }



    public get Hawb() { return this.AdditionalData.Hawb }
    public set Hawb(newValue: string) { this.AdditionalData.Hawb = newValue; }

    public get ShipmentValueInNIS() { return new CustomNumbersPipe().transform(this.AdditionalData.ShipmentValueInNIS, 0) }
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
        this.ValidationWarningsList = null;
        this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.SecurityKey, this.Tenant).subscribe(myAdditionalResult => {

            var entity = myAdditionalResult.Result;//AdditionalResult.Result
            if (entity.IsUserIDNumberRequired == false) {
                var myMessage = "תעודת זהות כבר הוזנה למשלוח זה";
                if (entity.UserIdNumberUpdateDateTime != null) {
                    myMessage = myMessage + " " + entity.UserIdNumberUpdateDateTime;
                }
                this.FinalMessage == myMessage;
                this.ShowFinalMessage = true;
               
            }
            else {
                if (!AppTool.IsNullOrEmpty(this.UserIdNumber)) {
                    entity.UserIdNumberUpdateDateTime = new Date();
                    entity.IsUserIDNumberRequired = false;
                    entity.UserIdNumber = this.UserIdNumber;

                    this._ShipmentAdditionalCloudDataService.updateUserID(entity).subscribe(AdditionalResult => {

                        this.FinalMessage == "זיהוי משתמש נשלח בהצלחה ";
                        this.ShowFinalMessage = true;

                    });
                }
                else {
                }
            }


        });
      

    }


}
