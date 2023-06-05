import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { TenantPMService } from '../../../Common/Services/StandardPMs/TenantPMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { AppTool } from '../../../Infrastructure/Tools';
import { QuestionnaireList } from '../../../CRM/EntityLists/QuestionnaireList';
import { QuestionnaireListService } from '../../../CRM/Services/StandardLists/QuestionnaireListService';
import { VatFormatTypeListService } from '../../../Common/Services/StandardLists/VatFormatTypeListService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SupplierInvioceExportDefaultPMService } from 'Customs/Services/StandardPMs/SupplierInvioceExportDefaultPMService';
import { SupplierInvioceExportDefaultListService } from 'Customs/Services/StandardLists/SupplierInvioceExportDefaultListService';
import { SupplierInvioceExportDefaultPM } from 'Customs/EntityPMs/SupplierInvioceExportDefaultPM';
import { SupplierInvioceExportDefaultExtendedPMService } from 'Customs/Services/ExtendedPMs/SupplierInvioceExportDefaultExtendedPMService';

@Component({
    selector: 'OcrDefaultsSettingsComponent',

    templateUrl: './OcrDefaultsSettingsComponent.html',
})

export class OcrDefaultsSettingsComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.SupplierInvioceExportDefault";
    private SupplierInvioceExportDefaultPM: SupplierInvioceExportDefaultPM;
    public ValidationErrorsList: string[];
    public IsVisibile = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.LoadDefaults();

    }

    private LoadDefaults() {

        this.SupplierInvioceExportDefaultPM = new SupplierInvioceExportDefaultPM();
        var myService: SupplierInvioceExportDefaultExtendedPMService = new SupplierInvioceExportDefaultExtendedPMService();
        
        myService.getByTenat(SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            this.SupplierInvioceExportDefaultPM = response.Result;
            
            if (this.SupplierInvioceExportDefaultPM == null)
                this.SupplierInvioceExportDefaultPM = new SupplierInvioceExportDefaultPM();
            this.IsVisibile = true;

        });

    }


    get AccountTypeCode() { return this.SupplierInvioceExportDefaultPM?.AccountTypeCode ?? "380"; }
    set AccountTypeCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.AccountTypeCode != value) {
            this.SupplierInvioceExportDefaultPM.AccountTypeCode = value;

        }
    }

    get PartyRelationshipCode() { return this.SupplierInvioceExportDefaultPM?.PartyRelationshipCode ?? "3" }
    set PartyRelationshipCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.PartyRelationshipCode != value) {
            this.SupplierInvioceExportDefaultPM.PartyRelationshipCode = value;

        }
    }

    get ClaimReasonCode() { return this.SupplierInvioceExportDefaultPM?.ClaimReasonCode ?? "6" }
    set ClaimReasonCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.ClaimReasonCode != value) {
            this.SupplierInvioceExportDefaultPM.ClaimReasonCode = value;

        }
    }

    get TransactionNatureCode() { return this.SupplierInvioceExportDefaultPM?.TransactionNatureCode ?? "2" }
    set TransactionNatureCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.TransactionNatureCode != value) {
            this.SupplierInvioceExportDefaultPM.TransactionNatureCode = value;

        }
    }
    get ProcessTypeCode() { return this.SupplierInvioceExportDefaultPM?.ProcessTypeCode ?? "1100105" }
    set ProcessTypeCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.ProcessTypeCode != value) {
            this.SupplierInvioceExportDefaultPM.ProcessTypeCode = value;

        }
    }

    get BuyerRoleCode() { return this.SupplierInvioceExportDefaultPM?.BuyerRoleCode ?? "9" }
    set BuyerRoleCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.BuyerRoleCode != value) {
            this.SupplierInvioceExportDefaultPM.BuyerRoleCode = value;

        }
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.SupplierInvioceExportDefaultPM, this.DataContext.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();
        }

    }
    SubmitTenantChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: SupplierInvioceExportDefaultPMService = new SupplierInvioceExportDefaultPMService();
        this.SupplierInvioceExportDefaultPM.Tenant = SessionLocator.Tenant;
        if (this.SupplierInvioceExportDefaultPM?.Id == null) {
            myService.insert(this.SupplierInvioceExportDefaultPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                       
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {

                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
        else{
            myService.update(this.SupplierInvioceExportDefaultPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                       
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {

                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            }); 
        }

    }
}
