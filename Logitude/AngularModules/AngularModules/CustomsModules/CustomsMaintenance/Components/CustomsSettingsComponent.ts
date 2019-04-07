declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from       '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
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


@Component({
    moduleId: module.id,
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
    ///public ComponentRef: ComponentRef<CustomsSettingsComponent>;

    _TenantCustomsSettingList: CustomsSettingList;
    entityPM: CustomsSettingPM;

    ValidationErrorsList: string[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    Loaded: boolean = false;
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {

            var filters = new ApiQueryFilters(true);
            filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "string");

            this._CustomsSettingListService.getByFilters(filters)
                .subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var listCustomsSetting = myResponse.Result;

                            if (!AppTool.IsNullOrEmpty(listCustomsSetting)) {
                                this._TenantCustomsSettingList = listCustomsSetting[0];

                                this._CustomsSettingPMService.get(this._TenantCustomsSettingList.Id)
                                    .subscribe((myResponse: ServiceResponse) => {
                                        this.entityPM = myResponse.Result;
                                        this.Loaded = true;

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


    get UnifreightCertificateActivated() { return this.entityPM != null ? this.entityPM.UnifreightCertificateActivated : false; }
    set UnifreightCertificateActivated(value: boolean) { this.entityPM.UnifreightCertificateActivated = value; }

    get AutoFillAccountType() { return this.entityPM != null ? this.entityPM.AutoFillAccountType : false; }
    set AutoFillAccountType(value: boolean) { this.entityPM.AutoFillAccountType = value; }

    get AutoUnitMeasurement() { return this.entityPM != null ? this.entityPM.AutoUnitMeasurement : false; }
    set AutoUnitMeasurement(value: boolean) { this.entityPM.AutoUnitMeasurement = value; }

    get PaymentOrderAccCard() { return this.entityPM != null ? this.entityPM.PaymentOrderAccCard : null; }
    set PaymentOrderAccCard(value: string) { this.entityPM.PaymentOrderAccCard = value; }



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




    
    //#endregion


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
    OkButtonClicked() {
        let IsNew: boolean = false;//itzik : there is a row that come with defualt DB !!!
        if (IsNew) {
            console.error("New Setting Record !!!!!?!?!?!?")
            return;
        }
        //let msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //List < ValidationResult > errors = new List<ValidationResult>();
        //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);
        this._CustomsSettingPMService.update(this.entityPM)
            .subscribe(resp => {
                if (resp.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();

            });
    }
}
