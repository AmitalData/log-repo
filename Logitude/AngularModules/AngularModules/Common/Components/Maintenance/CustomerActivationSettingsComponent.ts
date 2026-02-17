import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {TenantPMService} from '../../../Common/Services/StandardPMs/TenantPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {AppTool} from '../../../Infrastructure/Tools';
import {QuestionnaireList} from '../../../CRM/EntityLists/QuestionnaireList'; 
import {QuestionnaireListService} from '../../../CRM/Services/StandardLists/QuestionnaireListService'; 
import {VatFormatTypeListService} from '../../../Common/Services/StandardLists/VatFormatTypeListService'; 
import {Validator} from '../../../Infrastructure/Validators/Validator';
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

                this.GetQuestionnairesByTenant();
                this.SetUIProperties();
                this.IsVisibile = true;
                
            }
        });
    }
    
    //SetUIProperties
    private SetUIProperties() {
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
    
    // Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);

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
}
