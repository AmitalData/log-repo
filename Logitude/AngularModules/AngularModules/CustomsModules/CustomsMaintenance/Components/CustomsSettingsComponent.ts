declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from       '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { ApiQueryFilters } from  '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from  '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from   '../../../Infrastructure/Services/EntityListService';





import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomsSettingPM } from '../../../Customs/EntityPMs/CustomsSettingPM';
import { CustomsSettingList } from '../../../Customs/EntityLists/CustomsSettingList';

//C: \LW\Customs\AngularModules\AngularModules\Customs\Services\StandardPMs\CustomsSettingPMService.ts
import { CustomsSettingPMService } from '../../../Customs/Services/StandardPMs/CustomsSettingPMService';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { CustomsSettingExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { IIGGeneralMessagesService } from 'Customs/Services/WebServices/IIGGeneralMessagesService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';


@Component({
    
    templateUrl: './CustomsSettingsComponent.html',
})





export class CustomsSettingsComponent
    extends BaseComponent
    implements OnInit {
    

    public DataContext: CustomsSettingsComponent = this;
    public ObjectTableName: string = "Customs.CustomsSetting";
    public columns: any[] = null;

    

    private _CustomsSettingPMService: CustomsSettingPMService = new CustomsSettingPMService();
    private _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public customsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    ///public ComponentRef: ComponentRef<CustomsSettingsComponent>;

    _TenantCustomsSettingList: CustomsSettingList;
    entityPM: CustomsSettingPM;

    ValidationErrorsList: string[] = [];
    interval: any;
    _IIGGeneralMessagesService : IIGGeneralMessagesService= new IIGGeneralMessagesService();
    constructor() {
        super();
    }
    Loaded: boolean = false;
    public CompanyTypeList: CodeNameClass[];
    ngOnInit() {
        //ערכים C - דיפולטיבי (בסקריפט), םו B == בלדרות - םסור ריק יםותחל עם הפצה רםשונה + DEFAULT == C

        this.CompanyTypeList = [];
        this.UIProperties.SetEnabled("LastRunningDCAWS", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LastNumOfMessagesDCAWS", this.ObjectTableName, false);

        this.CompanyTypeList.push(new CodeNameClass("C", "עמילות"));
        this.CompanyTypeList.push(new CodeNameClass("B", "בלדרות"));
        this._SelectedCompanyType= this.CompanyTypeList[0];

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {

            var filters = new ApiQueryFilters(true);
            filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "string");

            this._CustomsSettingListService.getByFilters(filters)
                .subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var listCustomsSetting = myResponse.Result;

                            if (!AppTool.IsNullOrEmpty(listCustomsSetting)) {
                                this._TenantCustomsSettingList = listCustomsSetting[0];
                                this.SelectedCompanyType = this.CompanyTypeList.filter(r => r.Code == this._TenantCustomsSettingList.CompanyType)[0];
                                this._CustomsSettingPMService.get(this._TenantCustomsSettingList.Id)
                                    .subscribe((myResponse: ServiceResponse) => {
                                        this.entityPM = myResponse.Result;
                                        if (AppTool.IsNullOrEmpty(this.entityPM.QtyFeedbackInPendingMessage)) this.entityPM.QtyFeedbackInPendingMessage = 100;
                                        this.Loaded = true;
                                    
                                        this.LastRunningDCAWS = this.entityPM.LastRunningDCAWS.toString();

                                        this.interval = setInterval(() => {
               
                                            this._CustomsSettingPMService.get(this._TenantCustomsSettingList.Id).subscribe((response: ServiceResponse) => {

                                                this.LastRunningDCAWS = response.Result.LastRunningDCAWS;
                                                this.LastNumOfMessagesDCAWS = response.Result.LastNumOfMessagesDCAWS;

                                            });
                                        }, 30000);
                                        this.ValidScreen()
                                    });


                            }
                        }
                    }
                });


            //this.RefreshBtnClick()
        });

    }
    

    ValidScreen() {
        if (!this.IsConnectedToUniFreight) {
            this.IsUnifreightCertificateActivatedEnabled = false;
            this.UnifreightCertificateActivated = false;
        }
        else {
            this.IsUnifreightCertificateActivatedEnabled = true;
        }
    }

    ///#region Properties

    //IsUnifreightCertificateActivatedEnabled: boolean = true;

    _SelectedCompanyType: CodeNameClass;
    get SelectedCompanyType() { return this._SelectedCompanyType; }
    set SelectedCompanyType(val) {
        this._SelectedCompanyType = val;
        if (this._SelectedCompanyType != null && this.entityPM !=null) {
            this.entityPM.CompanyType = this._SelectedCompanyType.Code;
        }
    }

    get UnifreightCertificateActivated() { return this.entityPM != null ? this.entityPM.UnifreightCertificateActivated : false; }
    set UnifreightCertificateActivated(value: boolean) { this.entityPM.UnifreightCertificateActivated = value; }

    get AutoFillAccountType() { return this.entityPM != null ? this.entityPM.AutoFillAccountType : false; }
    set AutoFillAccountType(value: boolean) { this.entityPM.AutoFillAccountType = value; }

    get AutoUnitMeasurement() { return this.entityPM != null ? this.entityPM.AutoUnitMeasurement : false; }
    set AutoUnitMeasurement(value: boolean) { this.entityPM.AutoUnitMeasurement = value; }

    get StandAlone() { return this.entityPM != null ? this.entityPM.StandAlone : false; }
    set StandAlone(value: boolean) { this.entityPM.StandAlone = value; }

    get PaymentOrderAccCard() { return this.entityPM != null ? this.entityPM.PaymentOrderAccCard : null; }
    set PaymentOrderAccCard(value: string) { this.entityPM.PaymentOrderAccCard = value; }

    get QtyFeedbackInPendingMessage() { return this.entityPM != null ? this.entityPM.QtyFeedbackInPendingMessage : null; }
    set QtyFeedbackInPendingMessage(value: number) {this.entityPM.QtyFeedbackInPendingMessage = value }
        
    get MaxItemsSendInteractive() { return this.entityPM != null ? this.entityPM.MaxItemsSendInteractive : null; }
    set MaxItemsSendInteractive(value: number) {this.entityPM.MaxItemsSendInteractive = value }

    get MaxSISendInteractive() { return this.entityPM != null ? this.entityPM.MaxSISendInteractive : null; }
    set MaxSISendInteractive(value: number) {this.entityPM.MaxSISendInteractive = value }
    
    //get TotalInvoiceAmountInUSD() { return this.entityPM != null ? this.entityPM.TotalInvoiceAmountInUSD: null; }
    //set TotalInvoiceAmountInUSD(value: number) { this.entityPM.TotalInvoiceAmountInUSD = value }


    get IsMessagesPending() { return this.entityPM != null ? this.entityPM.IsMessagesPending : null; }
    set IsMessagesPending(value: boolean) { this.entityPM.IsMessagesPending = value; }



    get IsConnectedToUniFreight() { return this.entityPM != null ? this.entityPM.IsConnectedToUniFreight : false; }
    set IsConnectedToUniFreight(value: boolean) {
        this.entityPM.IsConnectedToUniFreight = value;
        if (!value) {
            this.IsUnifreightCertificateActivatedEnabled = false;
            this.UnifreightCertificateActivated = false;
        }
        else {
            this.IsUnifreightCertificateActivatedEnabled = true;
        }


    }

    set IsUnifreightCertificateActivatedEnabled(val: boolean) {
        this.UIProperties.SetEnabled("UnifreightCertificateActivated", this.ObjectTableName, val);
    }


    get BlockAgentBankForMasab() { return this.entityPM != null ? this.entityPM.BlockAgentBankForMasab : false; }
    set BlockAgentBankForMasab(value) { this.entityPM.BlockAgentBankForMasab = value; }




    get TehilaDca() { return this.entityPM != null ? this.entityPM.TehilaDca : false; }
    set TehilaDca(value: boolean) { this.entityPM.TehilaDca = value; }



    get CustomsAgentId() { return this.entityPM != null ? this.entityPM.CustomsAgentId : null; }
    set CustomsAgentId(value) { this.entityPM.CustomsAgentId = value; }


    get SignServiceAddress() { return this.entityPM != null ? this.entityPM.SignServiceAddress : null; }
    set SignServiceAddress(value) { this.entityPM.SignServiceAddress = value; }





    get IIGServiceAddress() { return this.entityPM != null ? this.entityPM.IIGServiceAddress : null; }
    set IIGServiceAddress(value) { this.entityPM.IIGServiceAddress = value; }



    get DCAServiceAddress() { return this.entityPM != null ? this.entityPM.DCAServiceAddress : null; }
    set DCAServiceAddress(value) { this.entityPM.DCAServiceAddress = value; }



    get DCAPartnerVault() { return this.entityPM != null ? this.entityPM.DCAPartnerVault : null; }
    set DCAPartnerVault(value) { this.entityPM.DCAPartnerVault = value; }




    get UServerServiceAddress() { return this.entityPM != null ? this.entityPM.UServerServiceAddress : null; }
    set UServerServiceAddress(value) { this.entityPM.UServerServiceAddress = value; }




    get DefaultNotificationAssignee() { return this.entityPM != null ? this.entityPM.DefaultNotificationAssignee : null; }
    set DefaultNotificationAssignee(value) { this.entityPM.DefaultNotificationAssignee = value; }




    get CustomsEnvoirmentTypeCode() { return this.entityPM != null ? this.entityPM.CustomsEnvoirmentTypeCode : null; }
    set CustomsEnvoirmentTypeCode(value) { this.entityPM.CustomsEnvoirmentTypeCode = value; }





    get UnfConnectionString() { return this.entityPM != null ? this.entityPM.UnfConnectionString : null; }
    set UnfConnectionString(value) { this.entityPM.UnfConnectionString = value; }

    get LastNumOfMessagesDCAWS() { return this.entityPM != null ? this.entityPM.LastNumOfMessagesDCAWS : null; }
    set LastNumOfMessagesDCAWS(value) { this.entityPM.LastNumOfMessagesDCAWS = value; }


    _LastRunningDCAWS: Date;
    get LastRunningDCAWS() {
       // if (this.entityPM != null) {
          //  if (this.entityPM.LastRunningDCAWS != null) {
        if (this._LastRunningDCAWS != null) {
            var myFormats = DateTool.GetDateFormats(this._LastRunningDCAWS);
                return myFormats.DateString + " " + myFormats.ShortTimeString;

        }
        //    }
      //  }
       return null;


       //return this.entityPM != null ? this.entityPM.LastRunningDCAWS : null;
    }
    set LastRunningDCAWS(value: any) { this._LastRunningDCAWS =  value ; }


    get SuppressIIGMessageFromDate() { return this.entityPM != null ? this.entityPM.SuppressIIGMessageFromDate : null; }
    set SuppressIIGMessageFromDate(value) { this.entityPM.SuppressIIGMessageFromDate = value; }



    get SuppressIIGMessageToDate() { return this.entityPM != null ? this.entityPM.SuppressIIGMessageToDate : null; }
    set SuppressIIGMessageToDate(value) { this.entityPM.SuppressIIGMessageToDate = value; }



    get HSMCompanyId() { return this.entityPM != null ? this.entityPM.HSMCompanyId : null; }
    set HSMCompanyId(value) { this.entityPM.HSMCompanyId = value; }




    
    get HSMToken() { return this.entityPM != null ? this.entityPM.HSMToken : null; }
    set HSMToken(value) { this.entityPM.HSMToken = value; }

    //#endregion
    ClearCache(){

        var myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Title="כתב ויתור"
        myConfirmWindow.Show(`ניקוי מטמון יבוצע בשרת הנ"ל בלבד 
        לא יבוצע ניקוי מטמון לשירותים ברקע ובשרתי ההיבריד
        ניקוי מטמון מביא להאטה בביצועים
        האם להמשיך?
        `);
        myConfirmWindow.WindowClosed.subscribe(event => {
            if (myConfirmWindow.Yes) {
                this._IIGGeneralMessagesService.GetClearCacheItems().subscribe(a=>{

                    var msg = new MessageWindow();
                    msg.RTL = true;
                    msg.Show("...אנא שקול אתחול שירותי רקע ");
                });
            }
     
        });
        
    }

    ShowRestartServiceScript(){


        const script= //this.entityPM.ServiceScript.replace(/(?:\r\n|\r|\n)/g, '<br>');
                        this.entityPM.ServiceScript;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = "אתחול סרוויסים";//TextCodeTranslator.Translate("CommunicationLogSteps.O.Log");
        logitudeWindow.WindowArgs = { Log: script , UseTextarea:true };
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
    }
    

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    EventsButtonClicked(){
        var windowArgs: EntityArgs = new EntityArgs();
        windowArgs.ObjectTableName =this.ObjectTableName;
        windowArgs.EntityPM = this.entityPM;
      
        var logWindow = new LogitudeWindow();
        logWindow.Width = 950;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator.Translate("General.O.Events");
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsMaintenance/Components/CustomsSettingsEventsComponent');
    }
    
    OkButtonClicked() {
        let IsNew: boolean = false;//itzik : there is a row that come with defualt DB !!!
        if (IsNew) {
            console.error("New Setting Record !!!!!?!?!?!?")
            return;
        }
        if (AppTool.IsNullOrEmpty(this.entityPM.QtyFeedbackInPendingMessage)) this.entityPM.QtyFeedbackInPendingMessage = 100;
        
        this.ValidationErrorsList = [];
        if (this.SuppressIIGMessageFromDate && this.SuppressIIGMessageToDate) {
            

            if (new Date(this.SuppressIIGMessageFromDate) >= new Date(this.SuppressIIGMessageToDate)) {
                this.ValidationErrorsList.push("המסרים למכס מושבתים -מתאריך חייב להיות גדול מעד תאריך");
            }
        } else if ((this.SuppressIIGMessageFromDate || this.SuppressIIGMessageToDate)) {///קיים םחד לפחות
            this.ValidationErrorsList.push("המסרים למכס מושבתים -מתאריך חייב להיות גדול מעד תאריך");
        }
        if (this.ValidationErrorsList.length > 0) {
            return
        }


        //let msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //List < ValidationResult > errors = new List<ValidationResult>();
        //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);
        this._CustomsSettingPMService.update(this.entityPM)
            .subscribe((resp:any) => {
                if (resp.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();

            });
    }
}
