import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {TenantPMService} from '../../../Common/Services/StandardPMs/TenantPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {AppTool} from '../../../Infrastructure/Tools';
import {QuestionnaireList} from '../../../CRM/EntityLists/QuestionnaireList'; 
import {QuestionnaireListService} from '../../../CRM/Services/StandardLists/QuestionnaireListService'; 
import {VatFormatTypeList} from '../../../Common/EntityLists/VatFormatTypeList'; 
import {VatFormatTypeListService} from '../../../Common/Services/StandardLists/VatFormatTypeListService'; 
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'CustomerActivationSettingsComponent',
    moduleId: module.id,
    templateUrl: './CustomerActivationSettingsComponent.html',
})

export class CustomerActivationSettingsComponent extends BaseComponent {
    public DataContext: CustomerActivationSettingsComponent = this;
    public ObjectTableName: string = "Tenant";
    private tenantPM: TenantPM;
    public ValidationErrorsList: string[];
    public QuestionnaireList: QuestionnaireList []= [];
    public IsVisibile = false;
    IsShowAreaDefaultQuestionnaire: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.QuestionnaireList = [];
        this.tenantPM = new TenantPM();
        this.LoadTenantPMMethod();
        if (FeatureLocator.HasFeaturePermession("General","QUESTIONNAIRE")) {
            this.IsShowAreaDefaultQuestionnaire = true;
        }
     
    }

    // Load Tenant 
    private LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!response.HasError) {
                this.tenantPM = response.Result;
                if (AppTool.IsNullOrEmpty(this.VatFormatTypeCode)) {
                    this.getVatFormatList();
                }
                else {
                    this.isAlphaNumeric = !this.IsNumeric;
                    this.GetQuestionnairesByTenant();
                    this.SetUIProperties();
                    this.IsVisibile = true;
                }
            }
        });
    }
    private getVatFormatList() {
        var service: VatFormatTypeListService = new VatFormatTypeListService();
        service.getAllFromCache().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list = response.Result;
                if (list != null) {
                    var noFormatVat = list.filter(d => d.Code == "NOF")[0];
                    if (noFormatVat != null) {
                        this.VatFormatTypeCode = noFormatVat.Code;
                    }
                }

                this.isAlphaNumeric = !this.IsNumeric;
                this.GetQuestionnairesByTenant();
                this.SetUIProperties();
                this.IsVisibile = true;
            }
        });
    }

    //SetUIProperties
    private SetUIProperties() {
        this.SetUIProperties_VatUnique();
        this.SetUIProperties_VatMandatory();
        this.SetUIProperties_VatFormat();
        this.SetCustomerPotentialTelProperties();
        this.SetCustomerPotentialFaxProperties();
    }
    private SetCustomerPotentialTelProperties() {
        if (!this.IsCustomerTelRequired) {
            if (this.IsPotentialTelRequired) {
                this.IsCustomerTelRequired = true;
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, false);
            }
            else {
                this.IsCustomerTelRequired = false;
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, true);
            }
        }

        else {
            if (this.IsPotentialTelRequired) {
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, true);
            }
        }
    }
    private SetCustomerPotentialFaxProperties() {
        if (!this.IsCustomerFaxRequired) {
            if (this.IsPotentialFaxRequired) {
                this.IsCustomerFaxRequired = true;
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, false);
            }
            else {
                this.IsCustomerFaxRequired = false;
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, true);
            }

            //FirePropertyChanged("IsCustomerFaxRequired");
        }

        else {
            if (this.IsPotentialFaxRequired) {
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, true);
            }
        }
    }

    //Questionnaire
    private GetQuestionnairesByTenant() {
        this.QuestionnaireList = [];
        var service: QuestionnaireListService = new QuestionnaireListService();
        service.getAll().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list: QuestionnaireList[] = response.Result;
                if (list != null) {
                    list = list.filter(q => q.InActive == false);
                }
                list.forEach(item => {
                    this.QuestionnaireList.push(item);
                });

                var None: QuestionnaireList = new QuestionnaireList();
                None.Name = "None";
                this.QuestionnaireList.push(None);
            }
        });
    }

    private questionnaireSelected: QuestionnaireList;
    get QuestionnaireSelected()
    {
        if (!AppTool.IsNullOrEmpty(this.tenantPM.DefaultQuestionnaireId)) {
            if (QuestionnaireList != null) {
                 this.questionnaireSelected = this.QuestionnaireList.filter(d => d.Id == this.tenantPM.DefaultQuestionnaireId)[0];
            }
        }

        else if (this.tenantPM.DefaultQuestionnaireId == null) {
           this.questionnaireSelected = this.QuestionnaireList.filter(d => d.Name == "None")[0];
        }

        return  this.questionnaireSelected;
    }
    set QuestionnaireSelected(value:QuestionnaireList)
    {
         this.questionnaireSelected = value;

         if ( this.questionnaireSelected.Name == "None") {
             this.tenantPM.DefaultQuestionnaireId = null;
        }
        else {
              this.tenantPM.DefaultQuestionnaireId =  this.questionnaireSelected.Id;
        }
    }

    get DefaultQuestionnaireId() {
        return this.tenantPM.DefaultQuestionnaireId;
    }
    set DefaultQuestionnaireId(value: string) {
        this.tenantPM.DefaultQuestionnaireId = value;
    }

    //Settings
    get IsCustomerTelRequired() { return this.tenantPM.IsCustomerTelRequired; }
    set IsCustomerTelRequired(value: boolean) {
        if (this.tenantPM.IsCustomerTelRequired != value) {
            this.tenantPM.IsCustomerTelRequired = value;
            this.SetCustomerPotentialTelProperties();
        }
    }

    get IsCustomerFaxRequired() { return this.tenantPM.IsCustomerFaxRequired; }
    set IsCustomerFaxRequired(value: boolean) {
        if (this.tenantPM.IsCustomerFaxRequired != value) {
            this.tenantPM.IsCustomerFaxRequired = value;

            this.SetCustomerPotentialFaxProperties();
        }
    }

    get IsPickDelAdrsRequired() { return this.tenantPM.IsPickDelAdrsRequired; }
    set IsPickDelAdrsRequired(value: boolean) {
        if (this.tenantPM.IsPickDelAdrsRequired != value) {
            this.tenantPM.IsPickDelAdrsRequired = value;
        }
    }

    get IsCustomerAddress1Required() { return this.tenantPM.IsCustomerAddress1Required; }
    set IsCustomerAddress1Required(value: boolean) {
        if (this.tenantPM.IsCustomerAddress1Required != value) {
            this.tenantPM.IsCustomerAddress1Required = value;
        }
    }

    get HasPrimaryContact() { return this.tenantPM.HasPrimaryContact; }
    set HasPrimaryContact(value: boolean) {
        if (this.tenantPM.HasPrimaryContact != value) {
            this.tenantPM.HasPrimaryContact = value;
        }
    }

    get IsPotentialTelRequired() { return this.tenantPM.IsPotentialTelRequired; }
    set IsPotentialTelRequired(value: boolean) {
        if (this.tenantPM.IsPotentialTelRequired != value) {
            this.tenantPM.IsPotentialTelRequired = value;
            this.SetCustomerPotentialTelProperties();
        }
    }

    get IsPotentialFaxRequired() { return this.tenantPM.IsPotentialFaxRequired; }
    set IsPotentialFaxRequired(value: boolean) {
        if (this.tenantPM.IsPotentialFaxRequired != value) {
            this.tenantPM.IsPotentialFaxRequired = value;
            this.SetCustomerPotentialFaxProperties();
        }
    }

    //VAT Settings

    //VAT format by country
    get VatFormatTypeCode() { return this.tenantPM.VatFormatTypeCode; }
    set VatFormatTypeCode(value: string) {
        if (this.tenantPM.VatFormatTypeCode != value) {
            this.tenantPM.VatFormatTypeCode = value;
            this.SetUIProperties_VatFormat();
            this.tenantPM.VatFormatCountryId = null;
        }
    }

    get VatFormatCountryId() { return this.tenantPM.VatFormatCountryId; }
    set VatFormatCountryId(value: string) {
        if (this.tenantPM.VatFormatCountryId != value) {
            this.tenantPM.VatFormatCountryId = value;
            this.SetUIProperties_VatFormat();
        }
    }

    get IsNumeric() { return this.tenantPM.IsNumeric; }
    set IsNumeric(value: boolean) {
        if (this.tenantPM.IsNumeric != value) {
            this.isAlphaNumeric = !value;
            this.tenantPM.IsNumeric = value;
        }
    }

    private isAlphaNumeric;
    get IsAlphaNumeric() { return this.isAlphaNumeric; }
    set IsAlphaNumeric(value: boolean) {
        if (this.isAlphaNumeric != value) {
            this.isAlphaNumeric = value;
            this.tenantPM.IsNumeric = !value;
        }
    }

    get VatSize() { return this.tenantPM.VatSize; }
    set VatSize(value: number) {
        if (this.tenantPM.VatSize != value) {
            this.tenantPM.VatSize = value;

            this.SetUIProperties_VatFormat();
        }
    }

    private isNumericEnabled;
    get IsNumericEnabled() { return this.isNumericEnabled; }
    set IsNumericEnabled(value: boolean) {
        if (this.isNumericEnabled != value) {
            this.isNumericEnabled = value;
        }
    }

    private SetUIProperties_VatFormat() {
        if (this.VatFormatTypeCode == "FSC") {
            this.UIProperties.SetEnabled("VatFormatCountryId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VatSize", this.ObjectTableName, true);

            this.IsNumericEnabled = true;

            this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, false);
            if (AppTool.IsNullOrEmpty(this.VatFormatCountryId)) {
                this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, true);
            }
        }

        else if (this.VatFormatTypeCode == "FAC") {
            this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("VatFormatCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatSize", this.ObjectTableName, true);

            this.IsNumericEnabled = true;
        }

        else {
            this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("VatFormatCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatSize", this.ObjectTableName, false);

            this.IsNumericEnabled = false;
        }

        if (this.VatSize != null) {
            if (this.VatSize > 20) {
                this.UIProperties.SetValidity("VatSize", this.ObjectTableName, false, "VAT Size should not exceed 20");
            }
            else {
                this.UIProperties.SetValidity("VatSize", this.ObjectTableName, true, "");
            }
        }
    }

    //VAT# is unique by countr
    get VatUniqueTypeCode(){ return this.tenantPM.VatUniqueTypeCode; }
    set VatUniqueTypeCode(value:string)
    {
        if (this.tenantPM.VatUniqueTypeCode != value) {
            this.tenantPM.VatUniqueTypeCode = value;
            this.SetUIProperties_VatUnique();
            this.tenantPM.VatUniqueCountryId = null;
        }
    }

    get VatUniqueCountryId(){ return this.tenantPM.VatUniqueCountryId; }
    set VatUniqueCountryId(value:string)
    {
        if (this.tenantPM.VatUniqueCountryId != value) {
            this.tenantPM.VatUniqueCountryId = value;
            this.SetUIProperties_VatUnique();
        }
    }

    private SetUIProperties_VatUnique() {
        var isEnabled = false;
        var isRequired = false;

        if (this.VatUniqueTypeCode == "USC") {
            isEnabled = true;

            if (AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetEnabled("VatUniqueCountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetRequired("VatUniqueCountryId", this.ObjectTableName, isRequired);
    }

    //VAT# is mandatory for customers
    get VatMandatoryTypeCode() { return this.tenantPM.VatMandatoryTypeCode; }
    set VatMandatoryTypeCode(value:string)
    {
        if (this.tenantPM.VatMandatoryTypeCode != value) {
            this.tenantPM.VatMandatoryTypeCode = value;
            this.SetUIProperties_VatMandatory();
            this.tenantPM.VatMandatoryCountryId = null;
            if (value == "MNT") {
                this.VatMandatoryForPotentialCustomers = false;
            }
        }
    }

    get VatMandatoryCountryId() { return this.tenantPM.VatMandatoryCountryId; }
    set VatMandatoryCountryId(value:string)
    {
        if (this.tenantPM.VatMandatoryCountryId != value) {
            this.tenantPM.VatMandatoryCountryId = value;
            this.SetUIProperties_VatMandatory();
        }
    }

    get VatMandatoryForPotentialCustomers() { return this.tenantPM.VatMandatoryForPotentialCustomers; }
    set VatMandatoryForPotentialCustomers(value:boolean)
    {
        if (this.tenantPM.VatMandatoryForPotentialCustomers != value) {
            this.tenantPM.VatMandatoryForPotentialCustomers = value;
        }
    }
    private SetUIProperties_VatMandatory() {
        var isEnabled = false;
        var isRequired = false;

        if (this.VatMandatoryTypeCode == "MSC") {
            isEnabled = true;

            if (AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetEnabled("VatMandatoryCountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetRequired("VatMandatoryCountryId", this.ObjectTableName, isRequired);
        this.UIProperties.SetEnabled("VatMandatoryForPotentialCustomers", this.ObjectTableName, this.VatMandatoryTypeCode != "MNT");
    }

    // Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);

        if (this.VatFormatTypeCode == "FSC") {
            if (AppTool.IsNullOrEmpty(this.VatFormatCountryId)) {
                errors.push("VAT Format Country is required");
            }
        }

        if (this.VatUniqueTypeCode == "USC") {
            if (AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                errors.push("VAT Unique Country is required");
            }
        }

        if (this.VatMandatoryTypeCode == "MSC") {
            if (AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                errors.push("VAT Mandatory Country is required");
            }
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();

        }

    }
    SubmitTenantChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: TenantPMService = new TenantPMService();
        myService.update(this.tenantPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.tenantPM;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
    SetCustomerActivationRadio(code:string) {
        if (code == 'A') {
            this.IsNumeric = false;
        }
        else {
            this.IsAlphaNumeric = false;
        }
    }
}
