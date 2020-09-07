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

    constructor() {
        super();
    }
    Loaded: boolean = false;
    public CompanyTypeList: CodeNameClass[];
    ngOnInit() {
        //ערכים C - דיפולטיבי (בסקריפט), או B == בלדרות - אסור ריק יאותחל עם הפצה ראשונה + DEFAULT == C

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

    get PaymentOrderAccCard() { return this.entityPM != null ? this.entityPM.PaymentOrderAccCard : null; }
    set PaymentOrderAccCard(value: string) { this.entityPM.PaymentOrderAccCard = value; }

    get QtyFeedbackInPendingMessage() { return this.entityPM != null ? this.entityPM.QtyFeedbackInPendingMessage : null; }
    set QtyFeedbackInPendingMessage(value: number) {this.entityPM.QtyFeedbackInPendingMessage = value }
        
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


    

    
    //#endregion


    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
    
    OkButtonClicked() {
        let IsNew: boolean = false;//itzik : there is a row that come with defualt DB !!!
        if (IsNew) {
            console.error("New Setting Record !!!!!?!?!?!?")
            return;
        }
        if (AppTool.IsNullOrEmpty(this.entityPM.QtyFeedbackInPendingMessage)) this.entityPM.QtyFeedbackInPendingMessage = 100;

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
