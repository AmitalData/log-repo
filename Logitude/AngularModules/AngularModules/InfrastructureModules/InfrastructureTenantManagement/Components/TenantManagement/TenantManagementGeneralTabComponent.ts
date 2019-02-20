declare var window: any;
import {Component, OnInit, OnDestroy}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {AppTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {TenantAddOnPM} from '../../../../Infrastructure/EntityPMs/TenantAddOnPM';
import {TenantManagementLicensePM} from '../../../../Infrastructure/EntityPMs/TenantManagementLicensePM';
import {PackageList} from '../../../../Common/EntityLists/PackageList';
import {PackageListService} from '../../../../Common/Services/StandardLists/PackageListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {GlobalDomainService} from '../../../../Common/Services/GlobalDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantManagementList} from '../../../../Infrastructure/EntityLists/TenantManagementList';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
@Component({
    moduleId: module.id,
    selector: 'TenantManagementGeneralTabComponent',
    templateUrl: './TenantManagementGeneralTabComponent.html',
})

export class TenantManagementGeneralTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public DataContext: TenantManagementGeneralTabComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    private iGlobalDomainService: GlobalDomainService;
    public BluesnapContractIdFilterItems: ApiQueryFilters;
    public BluesnapEAWBContractIdFilterItems: ApiQueryFilters;
    public BluesnapEAWBSContractIdFilterItems: ApiQueryFilters;
    public BluesnapOneTimeContractIdFilterItems: ApiQueryFilters;
    public BluesnapCRMContractIdFilterItems: ApiQueryFilters;

    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.iGlobalDomainService = new GlobalDomainService();
        this.LoadParentTenants();
        this.Listen();

        this.BluesnapContractIdFilterItems = new ApiQueryFilters();
        this.BluesnapContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "BA", null, null, "Equals", false, false, false, "string", false, true);

        this.BluesnapCRMContractIdFilterItems = new ApiQueryFilters();
        this.BluesnapCRMContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "CRM", null, null, "Equals", false, false, false, "string", false, true);

        this.BluesnapEAWBContractIdFilterItems = new ApiQueryFilters();
        this.BluesnapEAWBContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "EAWB", null, null, "Equals", false, false, false, "string", false, true);


        this.BluesnapEAWBSContractIdFilterItems = new ApiQueryFilters();
        this.BluesnapEAWBSContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "EABS", null, null, "Equals", false, false, false, "string", false, true);


        this.BluesnapOneTimeContractIdFilterItems = new ApiQueryFilters();
        this.BluesnapOneTimeContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "OT", null, null, "Equals", false, false, false, "string", false, true);


    }

    private SessionEvent: any = null;
    private TabChangedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.iGlobalDomainService.UpdateTenantManagementJS(this.EntityPM);
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.iGlobalDomainService.UpdateTenantManagementJS(this.EntityPM);
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.BuildPackagesList();
            this.BuildAddOnsList();
            this.SetUIProperties();
            this.FillTechnologiesList();
        }
    }

    private isTenantManagementEditable: boolean;
    SetUIProperties() {
        this.isTenantManagementEditable = this.IsTenantManagementEditable();

        this.SetUIProperties_Distributor();
        this.SetUIProperties_NumberOfUsers();
        this.SetUIProperties_ManageLicencesPerUser();

        if (this.isTenantManagementEditable) {
            this.SetUIProperties_PaymentFailure();
            this.SetUIProperties_IsTrial();
            this.SetUIProperties_IsRecurring();
            this.SetUIProperties_Plimus();
            this.SetUIProperties_TemporalPackage();
            this.SetUIProperties_TenantType();
            this.SetUIProperties_ParentTenant();         
        }

        else {
            this.ParentComboEnabled = false;

            this.UIProperties.SetEnabled("Name", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FreeUsers", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PackageCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TTY", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAWBStockPrepaid", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsINTTRAStockPrepaid", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsActive", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsSystemSupportEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsDistributorSupportEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DistributorCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Notes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentFailure", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("SuspendDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InternalNotes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsTrial", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsRecurring", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RecurringPeriodCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FirstPaymentDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaidUntilDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentChannelCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentMethodCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PlimusAccount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("MainContract", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LicensePrice", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalPackageCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TenantTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsParentTenant", this.ObjectTableName, false);
        }

        if (SessionLocator.Tenant == 0) {
            this.UIProperties.SetEnabled("IsSystemSupportEnabled", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_NumberOfUsers() {
        var isEditable = false;

        if (this.isTenantManagementEditable) {
            if (!this.IsMultiPackage) {
                isEditable = true;
            }
        }

        this.UIProperties.SetEnabled("NumberOfUsers", this.ObjectTableName, isEditable);
    }
    private SetUIProperties_Distributor() {
        if (SessionLocator.LoggedUserPM.IsCustomerCare) {
            var objectTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == "TenantManagement")[0];
            var enableSysDisChoices = FeatureLocator.Features.filter(f => f.Code == "SYSDISENABLED" && f.ObjectTableId == objectTable.Id)[0];

            if (enableSysDisChoices) {
                this.UIProperties.SetVisibility("IsSystemSupportEnabled", "TenantManagement", true);
                this.UIProperties.SetVisibility("IsDistributorSupportEnabled", "TenantManagement", true);
                this.UIProperties.SetVisibility("DistributorCode", "TenantManagement", true);
            }

            else {
                this.UIProperties.SetVisibility("IsSystemSupportEnabled", "TenantManagement", false);
                this.UIProperties.SetVisibility("IsDistributorSupportEnabled", "TenantManagement", false);
                this.UIProperties.SetVisibility("DistributorCode", "TenantManagement", false);
            }
        }
    }
    private SetUIProperties_PaymentFailure() {
        if (this.EntityPM.PaymentFailure) {
            this.UIProperties.SetEnabled("SuspendDate", "TenantManagement", true);
            this.UIProperties.SetEnabled("InternalNotes", "TenantManagement", true);
        }
        else {
            this.UIProperties.SetEnabled("SuspendDate", "TenantManagement", false);
            this.UIProperties.SetEnabled("InternalNotes", "TenantManagement", false);
        }
    }
    private SetUIProperties_IsTrial() {
        if (this.EntityPM.IsTrial) {
            if (this.TrialStartDate == null) {
                this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, true);
                this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, true);
            }

            this.UIProperties.SetEnabled("TrialStartDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("TrialEndDate", this.ObjectTableName, true);
        }

        else {
            this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, false);
            this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialEndDate", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_IsRecurring() {
        if (this.EntityPM.IsRecurring) {
            this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", AppTool.IsNullOrEmpty(this.RecurringPeriodCode));
            this.UIProperties.SetEnabled("RecurringPeriodCode", "TenantManagement", true);
        }
        else {
            this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", false);
            this.UIProperties.SetEnabled("RecurringPeriodCode", "TenantManagement", false);
        }        
    }
    private SetUIProperties_TemporalPackage() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.TemporalPackageCode)) {
            if (this.TemporalStartDate == null && this.TemporalEndDate == null) {
                this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, true);
                this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, true);
            }

            this.UIProperties.SetEnabled("TemporalStartDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("TemporalEndDate", this.ObjectTableName, true);
        }

        else {
            this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, false);
            this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalEndDate", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_Plimus() {
        if (!AppTool.IsNullOrEmpty(this.PaymentChannelCode)) {
            if (this.PaymentChannelCode == "PL") {
                this.UIProperties.SetEnabled("PlimusAccount", "TenantManagement", true);
            }
            else {
                this.UIProperties.SetEnabled("PlimusAccount", "TenantManagement", false);
            }
        }
        else {
            this.UIProperties.SetEnabled("PlimusAccount", "TenantManagement", false);
        }
    }
    private SetUIProperties_ManageLicencesPerUser() {
        var isFieldEnabled = false;

        if (this.isTenantManagementEditable) {
            if (!this.IsMultiPackage) {
                isFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("ManageLicencesPerUser", this.ObjectTableName, isFieldEnabled);
    }
    private SetUIProperties_TenantType() {
        if (AppTool.IsNullOrEmpty(this.TenantTypeCode)) {
            this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, false);
        }
        else {
            if (this.TenantTypeCode == "AIR") {
                this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, true);
            }

            else {
                this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, false);
            }
        }
    }

    public ParentComboEnabled: boolean = true;
    private SetUIProperties_ParentTenant() {
        if (this.IsParentTenant) {
            this.ParentComboEnabled = false;
        }
        else {
            this.ParentComboEnabled = true;
        }

        if (this.ParentTenantId != null) {
            this.UIProperties.SetEnabled("IsParentTenant", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("IsParentTenant", this.ObjectTableName, true);
        }
    }    

    private CloseBillingFields(close: boolean) {
        this.UIProperties.SetEnabled("IsRecurring", "TenantManagement", !close);
        this.UIProperties.SetEnabled("RecurringPeriodCode", "TenantManagement", !close);
        this.UIProperties.SetEnabled("FirstPaymentDate", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaidUntilDate", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaymentChannelCode", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaymentMethodCode", "TenantManagement", !close);
        this.UIProperties.SetEnabled("BluesnapAccount", "TenantManagement", !close);
        this.UIProperties.SetEnabled("MainContract", "TenantManagement", !close);
        this.UIProperties.SetEnabled("LicensePrice", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaymentCurrencyCode", "TenantManagement", !close);
    }

    private IsTenantManagementEditable() {
        var myResult = false;

        if (FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }

        return myResult;
    }

    private LoadParentTenants() {
        this.iGlobalDomainService.GetParentTenants().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var myList: TenantManagementList[] = myResponse.Result

                this.BuildParentTenantsList(myList);
            }
        });
    }

    //Parent Tenant
    public ParentTenantsList: CodeNameClass[] = [];
    private BuildParentTenantsList(myList: TenantManagementList[]) {
        this.ParentTenantsList = [];

        if (myList) {
            if (myList.length > 0) {

                var noneItem: CodeNameClass = new CodeNameClass();
                noneItem.Code = "None";
                noneItem.Name = "None";
                noneItem.DisplyText = "None";
                this.ParentTenantsList.push(noneItem);

                myList.forEach(item => {
                    var newItem: CodeNameClass = new CodeNameClass();
                    newItem.Code_Int = item.Id;
                    newItem.Name = item.Name;
                    newItem.DisplyText = item.Name + " (" + item.Id + ")";

                    this.ParentTenantsList.push(newItem);
                });

                if (!AppTool.IsNullOrEmpty(this.EntityPM.ParentTenantId)) {
                    this.selectedParentTenant = this.ParentTenantsList.filter(d => d.Code_Int == this.EntityPM.ParentTenantId)[0];
                }

                else {
                    this.selectedParentTenant = this.ParentTenantsList.filter(d => d.Code == "None")[0];
                }
            }
        }
    }

    private selectedParentTenant: CodeNameClass;
    get SelectedParentTenant() { return this.selectedParentTenant; }
    set SelectedParentTenant(value: CodeNameClass) {
        if (this.selectedParentTenant != value) {
            this.selectedParentTenant = value;

            if (value) {
                if (value.Code == "None") {
                    this.ParentTenantId = null;
                }

                else {
                    this.ParentTenantId = value.Code_Int;
                }
            }

            else {
                this.ParentTenantId = null;
            }
        }
    }
    
    get IsParentTenant() { return this.EntityPM.IsParentTenant; }
    set IsParentTenant(newValue: boolean) {
        if (this.EntityPM.IsParentTenant != newValue) {
            this.EntityPM.IsParentTenant = newValue;

            this.SetUIProperties_ParentTenant();

        }
    }

    get ParentTenantId() { return this.EntityPM.ParentTenantId; }
    set ParentTenantId(newValue: number) {
        if (this.EntityPM.ParentTenantId != newValue) {
            this.EntityPM.ParentTenantId = newValue;

            this.SetUIProperties_ParentTenant();
        }
    }

    //General Details
    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get TenantTypeCode() { return this.EntityPM.TenantTypeCode; }
    set TenantTypeCode(newValue: string) {
        if (this.EntityPM.TenantTypeCode != newValue) {
            this.EntityPM.TenantTypeCode = newValue;

            this.SetUIProperties_TenantType();
        }
    }

    get TenantConnectedToAirlineCode() { return this.EntityPM.TenantConnectedToAirlineCode; }
    set TenantConnectedToAirlineCode(newValue: string) {
        if (this.EntityPM.TenantConnectedToAirlineCode != newValue) {
            this.EntityPM.TenantConnectedToAirlineCode = newValue;
        }
    }

    get IsAWBStockPrepaid() { return this.EntityPM.IsAWBStockPrepaid; }
    set IsAWBStockPrepaid(newValue: boolean) {
        if (this.EntityPM.IsAWBStockPrepaid != newValue) {
            this.EntityPM.IsAWBStockPrepaid = newValue;
        }
    }

    get IsINTTRAStockPrepaid() { return this.EntityPM.IsINTTRAStockPrepaid; }
    set IsINTTRAStockPrepaid(newValue: boolean) {
        if (this.EntityPM.IsINTTRAStockPrepaid != newValue) {
            this.EntityPM.IsINTTRAStockPrepaid = newValue;
        }
    }

    get ManageLicencesPerUser() { return this.EntityPM.ManageLicencesPerUser; }
    set ManageLicencesPerUser(newValue: boolean) {
        if (this.EntityPM.ManageLicencesPerUser != newValue) {
            this.EntityPM.ManageLicencesPerUser = newValue;
        }
    }
    
    get ManagesRegisteredAgent() { return this.EntityPM.ManagesRegisteredAgent; }
    set ManagesRegisteredAgent(newValue: boolean) {
        if (this.EntityPM.ManagesRegisteredAgent != newValue) {
            this.EntityPM.ManagesRegisteredAgent = newValue;
        }
    }

    get IsActive() { return this.EntityPM.IsActive; }
    set IsActive(newValue: boolean) {
        if (this.EntityPM.IsActive != newValue) {
            this.EntityPM.IsActive = newValue;
        }
    }

    get IsSystemSupportEnabled() { return this.EntityPM.IsSystemSupportEnabled; }
    set IsSystemSupportEnabled(newValue: boolean) {
        if (this.EntityPM.IsSystemSupportEnabled != newValue) {
            this.EntityPM.IsSystemSupportEnabled = newValue;
        }
    }

    get IsDistributorSupportEnabled() { return this.EntityPM.IsDistributorSupportEnabled; }
    set IsDistributorSupportEnabled(newValue: boolean) {
        if (this.EntityPM.IsDistributorSupportEnabled != newValue) {
            this.EntityPM.IsDistributorSupportEnabled = newValue;
        }
    }

    get DistributorCode() { return this.EntityPM.DistributorCode; }
    set DistributorCode(newValue: string) {
        if (this.EntityPM.DistributorCode != newValue) {
            this.EntityPM.DistributorCode = newValue;
        }
    }

    get PrivateLabelId() { return this.EntityPM.PrivateLabelId; }
    set PrivateLabelId(newValue: string) {
        if (this.EntityPM.PrivateLabelId != newValue) {
            this.EntityPM.PrivateLabelId = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get Technology() { return this.EntityPM.Technology; }
    set Technology(newValue: string) {
        if (this.EntityPM.Technology != newValue) {
            this.EntityPM.Technology = newValue;
        }
    }

    public TechnologiesList: CodeNameClass[];
    private FillTechnologiesList() {
        this.TechnologiesList = [];
        this.TechnologiesList.push(new CodeNameClass("AG", "Angular"));
        this.TechnologiesList.push(new CodeNameClass("SL", "SliverLight"));
        this.TechnologiesList.push(new CodeNameClass("PR", "Prompt"));

        this.selectedTechnology = this.TechnologiesList.filter(d => d.Code == this.EntityPM.Technology)[0];
    }

    private selectedTechnology: CodeNameClass;
    get SelectedTechnology() { return this.selectedTechnology; }
    set SelectedTechnology(value: CodeNameClass) {
        if (this.selectedTechnology != value) {
            this.selectedTechnology = value;

            if (!AppTool.IsNullOrEmpty(value)) {
                this.Technology = value.Code;
            }
        }
    }

    //Trial Details
    get IsTrial() { return this.EntityPM.IsTrial; }
    set IsTrial(newValue: boolean) {
        if (this.EntityPM.IsTrial != newValue) {
            this.EntityPM.IsTrial = newValue;

            if (newValue) {
                this.IsRecurring = !newValue;
            }

            this.CloseBillingFields(newValue);
            this.SetUIProperties_IsTrial();
        }
    }

    get TrialStartDate() { return this.EntityPM.TrialStartDate; }
    set TrialStartDate(newValue: Date) {
        if (this.EntityPM.TrialStartDate != newValue) {
            this.EntityPM.TrialStartDate = newValue;

            if (newValue == null) {
                this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, false);
            }
        }
    }

    get TrialEndDate() { return this.EntityPM.TrialEndDate; }
    set TrialEndDate(newValue: Date) {
        if (this.EntityPM.TrialEndDate != newValue) {
            this.EntityPM.TrialEndDate = newValue;

            if (newValue == null) {
                this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, false);
            }
        }
    }
    
    get SilverlightEndDate() { return this.EntityPM.SilverlightEndDate; }
    set SilverlightEndDate(newValue: Date) {
        if (this.EntityPM.SilverlightEndDate != newValue) {
            this.EntityPM.SilverlightEndDate = newValue;

        }
    }

    get ChangeHeaderColor() { return this.EntityPM.ChangeHeaderColor; }
    set ChangeHeaderColor(newValue: boolean) {
        if (this.EntityPM.ChangeHeaderColor != newValue) {
            this.EntityPM.ChangeHeaderColor = newValue;
        }
    }

    get DocumentShareAsDefault() { return this.EntityPM.DocumentShareAsDefault; }
    set DocumentShareAsDefault(newValue: boolean) {
        if (this.EntityPM.DocumentShareAsDefault != newValue) {
            this.EntityPM.DocumentShareAsDefault = newValue;
        }
    }

    get TrialBoxIsEnabled() {
        var result: boolean = true;

        if (this.PaidUntilDate != null || this.IsRecurring) {
            result = false;
        }

        return result;
    }

    //Payment Failure
    get PaymentFailure() { return this.EntityPM.PaymentFailure; }
    set PaymentFailure(newValue: boolean) {
        if (this.EntityPM.PaymentFailure != newValue) {
            this.EntityPM.PaymentFailure = newValue;

            if (!newValue) {
                this.SuspendDate = null;
                this.InternalNotes = null;
            }

            this.SetUIProperties_PaymentFailure();
        }
    }

    get SuspendDate() { return this.EntityPM.SuspendDate; }
    set SuspendDate(newValue: Date) {
        if (this.EntityPM.SuspendDate != newValue) {
            this.EntityPM.SuspendDate = newValue;
        }
    }

    get InternalNotes() { return this.EntityPM.InternalNotes; }
    set InternalNotes(newValue: string) {
        if (this.EntityPM.InternalNotes != newValue) {
            this.EntityPM.InternalNotes = newValue;
        }
    }

    get PaymentFailureBoxIsEnabled() {
        var result: boolean = false;

        if (this.IsRecurring) {
            result = true;
        }

        return result;
    }

    //Subscription Details
    get IsMultiPackage() { return this.EntityPM.IsMultiPackage; }
    set IsMultiPackage(newValue: boolean) {
        if (this.EntityPM.IsMultiPackage != newValue) {
            this.EntityPM.IsMultiPackage = newValue;

            this.SetUIProperties_NumberOfUsers();
            this.SetUIProperties_ManageLicencesPerUser();
        }
    }

    get NumberOfUsers() { return this.EntityPM.NumberOfUsers; }
    set NumberOfUsers(newValue: number) {
        if (this.EntityPM.NumberOfUsers != newValue) {
            this.EntityPM.NumberOfUsers = newValue;
        }
    }

    get FreeUsers() { return this.EntityPM.FreeUsers; }
    set FreeUsers(newValue: number) {
        if (this.EntityPM.FreeUsers != newValue) {
            this.EntityPM.FreeUsers = newValue;
        }
    }


    get BluesnapContractQTYs() { return this.EntityPM.BluesnapContractQTY; }
    set BluesnapContractQTYs(newValue: number) {
        if (this.EntityPM.BluesnapContractQTY != newValue) {
            if (newValue == null)
                newValue = 0;
            this.EntityPM.BluesnapContractQTY = newValue;
        }

    }


    get BluesnapCRMContractQTYs() { return this.EntityPM.BluesnapCRMContractQTY; }
    set BluesnapCRMContractQTYs(newValue: number) {
        if (this.EntityPM.BluesnapCRMContractQTY != newValue) {
            if (newValue == null)
                newValue = 0;
            this.EntityPM.BluesnapCRMContractQTY = newValue;
        }
    }


    get BluesnapEAWBContractQTYs() { return this.EntityPM.BluesnapEAWBContractQTY; }
    set BluesnapEAWBContractQTYs(newValue: number) {
        if (this.EntityPM.BluesnapEAWBContractQTY != newValue) {
            if (newValue == null)
                newValue = 0;
            this.EntityPM.BluesnapEAWBContractQTY = newValue;
        }
    }


    get BluesnapEAWBSContractQTYs() { return this.EntityPM.BluesnapEAWBSContractQTY; }
    set BluesnapEAWBSContractQTYs(newValue: number) {
        if (this.EntityPM.BluesnapEAWBSContractQTY != newValue) {
            if (newValue == null)
                newValue = 0;
            this.EntityPM.BluesnapEAWBSContractQTY = newValue;
        }
    }


    get BluesnapOneTimeContractQTYs() { return this.EntityPM.BluesnapOneTimeContractQTY; }
    set BluesnapOneTimeContractQTYs(newValue: number) {
        if (this.EntityPM.BluesnapOneTimeContractQTY != newValue) {
            if (newValue == null)
                newValue = 0;
            this.EntityPM.BluesnapOneTimeContractQTY = newValue;
        }
    }

    

    get PackageCode() { return this.EntityPM.PackageCode; }
    set PackageCode(newValue: string) {
        if (this.EntityPM.PackageCode != newValue) {
            this.EntityPM.PackageCode = newValue;

            if (newValue == "EAWB") {
                this.IsAWBStockPrepaid = true;
            }
        }
    }

    AddPackageClicked(packageType: string) {
        if (packageType == "AD") {
            this.entityResourceService.getEntityResourceByTableName("TenantAddOn").subscribe((res1: any) => {
                var newAddOnPM: TenantAddOnPM = new TenantAddOnPM(this.EntityPM);
                newAddOnPM.Tenant = this.EntityPM.Id;

                var logeWindow = new LogitudeWindow();
                logeWindow.Title = "Add Add-On";
                logeWindow.DataContext = new AddOnItem(newAddOnPM, this, true);
                logeWindow.Show("../InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditAddOnComponent");
                logeWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.BuildAddOnsList();
                    }
                });
            });
        }

        else {
            this.entityResourceService.getEntityResourceByTableName("TenantManagementLicense").subscribe((res1: any) => {
                var newLicencePM: TenantManagementLicensePM = new TenantManagementLicensePM(this.EntityPM);
                newLicencePM.Tenant = this.EntityPM.Id;

                var logeWindow = new LogitudeWindow();
                logeWindow.Title = "Add Package";
                logeWindow.DataContext = new PackageItem(newLicencePM, this, true);
                logeWindow.Show("./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditLicenceComponent");
                logeWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.BuildPackagesList();
                    }
                });
            });
        } 
    }

    public PackagesList: PackageItem[];
    private BuildPackagesList() {
        if (this.PackagesList == null) {
            this.PackagesList = new Array<PackageItem>();
        }

        else {
            this.PackagesList = [];
        }

        var service: PackageListService = new PackageListService();
        service.getAllFromCache().subscribe(result => {
            var allPackages = result.Result;
            
            this.EntityPM.TenantManagementLicenses.forEach(item => {
                var list: PackageList = allPackages.filter(d => d.Code == item.PackageCode)[0];
                if (list != null) {
                    this.PackagesList.push(new PackageItem(item, this, false));
                }
            });
        });        
    } 

    public AddOnsList: AddOnItem[];
    private BuildAddOnsList() {
        if (this.AddOnsList == null) {
            this.AddOnsList = new Array<AddOnItem>();
        }

        else {
            this.AddOnsList = [];
        }

        var service: PackageListService = new PackageListService();
        service.getAllFromCache().subscribe(result => {
            var allPackages = result.Result;

            this.EntityPM.AddOns.forEach(item => {
                var list: PackageList = allPackages.filter(d => d.Code == item.PackageCode)[0];
                if (list != null) {
                    this.AddOnsList.push(new AddOnItem(item, this, false));
                }
            });
        });
    }

    EditAddOn(itemViewModel: AddOnItem) {
        this.entityResourceService.getEntityResourceByTableName("TenantAddOn").subscribe((res1: any) => {
            itemViewModel.SetOldData();

            var logeWindow = new LogitudeWindow();
            logeWindow.Title = "Edit Add-On";
            logeWindow.DataContext = itemViewModel;
            logeWindow.Show("./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditAddOnComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.BuildAddOnsList();
                }
            });
        });
    }
    DeleteAddOn(itemViewModel: AddOnItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Add-On?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var itemIndex = this.AddOnsList.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    this.AddOnsList.splice(itemIndex, 1);
                }

                this.EntityPM.RemoveTenantAddOnPM(itemViewModel.EntityPM);

                this.BuildAddOnsList();
            }
        });
    } 

    EditPackage(itemViewModel: PackageItem) {
        this.entityResourceService.getEntityResourceByTableName("TenantManagementLicense").subscribe((res1: any) => {
            itemViewModel.SetOldData();

            var logeWindow = new LogitudeWindow();
            logeWindow.Title = "Edit Package";
            logeWindow.DataContext = itemViewModel;
            logeWindow.Show("./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditLicenceComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.BuildPackagesList();
                }
            });
        });
    }
    DeletePackage(itemViewModel: PackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Package?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var itemIndex = this.PackagesList.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    this.PackagesList.splice(itemIndex, 1);
                }

                this.EntityPM.RemoveTenantManagementLicensePM(itemViewModel.EntityPM);

                this.BuildPackagesList();
            }
        });
    } 

    get IsEditable() {
        var isEditable: boolean = false;

        if (this.isTenantManagementEditable) {
            if (this.IsMultiPackage) {
                isEditable = true;
            }
        }

        return isEditable;
    }

    //Billing Details
    get IsRecurring() { return this.EntityPM.IsRecurring; }
    set IsRecurring(newValue: boolean) {
        if (this.EntityPM.IsRecurring != newValue) {
            this.EntityPM.IsRecurring = newValue;
            //if (newValue)
            //    this.IsTrial = !newValue;
        }

        this.SetUIProperties_IsRecurring();
        this.SetUIProperties_IsTrial();
        this.SetUIProperties_PaymentFailure();
    }
    
    get RecurringPeriodCode() { return this.EntityPM.RecurringPeriodCode; }
    set RecurringPeriodCode(newValue: string) {
        if (this.EntityPM.RecurringPeriodCode != newValue) {
            this.EntityPM.RecurringPeriodCode = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", true);
            }

            else {
                this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", false);

                if (newValue == "MO") {
                    this.UIProperties.SetEnabled("IsRecurring", "TenantManagement", false);
                }

                else {
                    this.UIProperties.SetEnabled("IsRecurring", "TenantManagement", true);
                }
            }
        }
    }

    get FirstPaymentDate() { return this.EntityPM.FirstPaymentDate; }
    set FirstPaymentDate(newValue: Date) {
        if (this.EntityPM.FirstPaymentDate != newValue) {
            this.EntityPM.FirstPaymentDate = newValue;
        }
    }

    get PaidUntilDate() { return this.EntityPM.PaidUntilDate; }
    set PaidUntilDate(newValue: Date) {
        if (this.EntityPM.PaidUntilDate != newValue) {
            this.EntityPM.PaidUntilDate = newValue;

            this.SetUIProperties_IsTrial();
        }
    }

    get PaymentCurrencyCode() { return this.EntityPM.PaymentCurrencyCode; }
    set PaymentCurrencyCode(newValue: string) {
        if (this.EntityPM.PaymentCurrencyCode != newValue) {
            this.EntityPM.PaymentCurrencyCode = newValue;
        }
    }

    get PaymentChannelCode() { return this.EntityPM.PaymentChannelCode; }
    set PaymentChannelCode(newValue: string) {
        if (this.EntityPM.PaymentChannelCode != newValue) {
            this.EntityPM.PaymentChannelCode = newValue;

            this.SetUIProperties_Plimus(); 
        }
    }

    get PaymentMethodCode() { return this.EntityPM.PaymentMethodCode; }
    set PaymentMethodCode(newValue: string) {
        if (this.EntityPM.PaymentMethodCode != newValue) {
            this.EntityPM.PaymentMethodCode = newValue;
        }
    }

    get BluesnapAccount() { return this.EntityPM.BluesnapAccount; }
    set BluesnapAccount(newValue: string) {
        if (this.EntityPM.BluesnapAccount != newValue) {
            this.EntityPM.BluesnapAccount = newValue;
        }
    }

    get MainContract() { return this.EntityPM.MainContract; }
    set MainContract(newValue: string) {
        if (this.EntityPM.MainContract != newValue) {
            this.EntityPM.MainContract = newValue;
        }
    }

    get LicensePrice() { return this.EntityPM.LicensePrice; }
    set LicensePrice(newValue: number) {
        if (this.EntityPM.LicensePrice != newValue) {
            this.EntityPM.LicensePrice = newValue;
        }
    }

    get BluesnapContractId() {
        return this.EntityPM.BluesnapContractId;
    }
    set BluesnapContractId(newValue: string) {
        if (this.EntityPM.BluesnapContractId != newValue) {
            this.EntityPM.BluesnapContractId = newValue;
        }
    }



    get BluesnapCRMContractId() {
        return this.EntityPM.BluesnapCRMContractId;
    }
    set BluesnapCRMContractId(newValue: string) {
        if (this.EntityPM.BluesnapCRMContractId != newValue) {
            this.EntityPM.BluesnapCRMContractId = newValue;
        }
    }




    get BluesnapEAWBContractId() {
        return this.EntityPM.BluesnapEAWBContractId;
    }
    set BluesnapEAWBContractId(newValue: string) {
        if (this.EntityPM.BluesnapEAWBContractId != newValue) {
            this.EntityPM.BluesnapEAWBContractId = newValue;
        }
    }



    get BluesnapEAWBSContractId() {
        return this.EntityPM.BluesnapEAWBSContractId;
    }
    set BluesnapEAWBSContractId(newValue: string) {
        if (this.EntityPM.BluesnapEAWBSContractId != newValue) {
            this.EntityPM.BluesnapEAWBSContractId = newValue;
        }
    }



    get BluesnapOneTimeContractId() {
        return this.EntityPM.BluesnapOneTimeContractId;
    }
    set BluesnapOneTimeContractId(newValue: string) {
        if (this.EntityPM.BluesnapOneTimeContractId != newValue) {
            this.EntityPM.BluesnapOneTimeContractId = newValue;
        }
    }

    get BillingByLogitude() { return this.EntityPM.BillingByLogitude; }
    set BillingByLogitude(newValue: boolean) {
        if (this.EntityPM.BillingByLogitude != newValue) {
            this.EntityPM.BillingByLogitude = newValue;
        }
    }

    SetBillingByLogitude(billingByLogitude: boolean) {
        this.BillingByLogitude = billingByLogitude;
    }

    get ResellerCommission() { return this.EntityPM.ResellerCommission; }
    set ResellerCommission(newValue: number) {
        if (this.EntityPM.ResellerCommission != newValue) {
            this.EntityPM.ResellerCommission = newValue;
        }
    }

    //Temporal Package Details
    get TemporalPackageCode() { return this.EntityPM.TemporalPackageCode; }
    set TemporalPackageCode(newValue: string) {
        if (this.EntityPM.TemporalPackageCode != newValue) {
            this.EntityPM.TemporalPackageCode = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.TemporalStartDate = null;
                this.TemporalEndDate = null;
            }

            this.SetUIProperties_TemporalPackage();
        }
    }

    get TemporalStartDate() { return this.EntityPM.TemporalStartDate; }
    set TemporalStartDate(newValue: Date) {
        if (this.EntityPM.TemporalStartDate != newValue) {
            this.EntityPM.TemporalStartDate = newValue;

            if (newValue == null) {
                this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, false);
            }
        }
    }

    get TemporalEndDate() { return this.EntityPM.TemporalEndDate; }
    set TemporalEndDate(newValue: Date) {
        if (this.EntityPM.TemporalEndDate != newValue) {
            this.EntityPM.TemporalEndDate = newValue;

            if (newValue == null) {
                this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, false);
            }
        }
    }
}

export class PackageItem extends BaseComponent{
    public EntityPM: TenantManagementLicensePM;
    public TenantManagementPM: TenantManagementPM;
    public ObjectTableName: string = "TenantManagementLicense";
    public IsNew: boolean;
    constructor(entity: TenantManagementLicensePM, public fatherComponent: TenantManagementGeneralTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNew = isNew;
        this.TenantManagementPM = fatherComponent.EntityPM;

        this.GetPackageName();
    }

    private old_PackageCode: string;
    private old_NumberOfUsers: number;
    public SetOldData() {
        this.old_PackageCode = this.PackageCode;
        this.old_NumberOfUsers = this.NumberOfUsers;
    }

    public ResetOldData() {
        this.PackageCode = this.old_PackageCode;
        this.NumberOfUsers = this.old_NumberOfUsers;
    }

    get PackageCode() { return this.EntityPM.PackageCode; }
    set PackageCode(newValue: string) {
        if (this.EntityPM.PackageCode != newValue) {
            this.EntityPM.PackageCode = newValue;

            this.GetPackageName();
        }
    }

    get NumberOfUsers() { return this.EntityPM.NumberOfUsers; }
    set NumberOfUsers(newValue: number) {
        if (this.EntityPM.NumberOfUsers != newValue) {
            this.EntityPM.NumberOfUsers = newValue;
        }
    }

    private packageName: string;
    get PackageName() { return this.packageName; }
    set PackageName(newValue: string) {
        if (this.packageName != newValue) {
            this.packageName = newValue;
        }
    }

    private GetPackageName() {
        var service: PackageListService = new PackageListService();
        service.getAllFromCache().subscribe(result => {
            var allPackages = result.Result;

            var list: PackageList = allPackages.filter(d => d.Code == this.PackageCode)[0];
            if (list != null) {
                this.PackageName = list.Name;
            }
        });
    }    
}

export class AddOnItem extends BaseComponent {
    public EntityPM: TenantAddOnPM;
    public TenantManagementPM: TenantManagementPM;
    public ObjectTableName: string = "TenantAddOn";
    public IsNew: boolean;
    constructor(entity: TenantAddOnPM, public fatherComponent: TenantManagementGeneralTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNew = isNew;
        this.TenantManagementPM = fatherComponent.EntityPM;

        this.GetPackageName();
    }

    private old_PackageCode: string;
    public SetOldData() {
        this.old_PackageCode = this.PackageCode;
    }

    public ResetOldData() {
        this.PackageCode = this.old_PackageCode;
    }

    get PackageCode() { return this.EntityPM.PackageCode; }
    set PackageCode(newValue: string) {
        if (this.EntityPM.PackageCode != newValue) {
            this.EntityPM.PackageCode = newValue;

            this.GetPackageName();
        }
    }

    private packageName: string;
    get PackageName() { return this.packageName; }
    set PackageName(newValue: string) {
        if (this.packageName != newValue) {
            this.packageName = newValue;
        }
    }

    private GetPackageName() {
        var service: PackageListService = new PackageListService();
        service.getAllFromCache().subscribe(result => {
            var allPackages = result.Result;

            var list: PackageList = allPackages.filter(d => d.Code == this.PackageCode)[0];
            if (list != null) {
                this.PackageName = list.Name;
            }
        });
    }
}
