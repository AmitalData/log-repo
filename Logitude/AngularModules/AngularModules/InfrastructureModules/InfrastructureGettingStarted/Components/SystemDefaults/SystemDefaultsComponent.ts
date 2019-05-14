import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'SystemDefaultsComponent',
    moduleId: module.id,
    templateUrl: './SystemDefaultsComponent.html',
})

export class SystemDefaultsComponent extends BaseComponent{

    public DataContext: SystemDefaultsComponent = this;
    public ObjectTableName: string = "Tenant";
    public TenantPm: TenantPM = new TenantPM();
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response => {
            this.LoadTenantPMMethod();
        });
    }


    // Load Tenant 
    public IsTenantUS: boolean = false;
    private LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.TenantPm = response.Result;
            this.LoadCachedLists();
            this.IsVisibile = true;

            if (this.TenantPm.CountryCode.toUpperCase() == "US") {
                this.IsTenantUS = true;
            }
        });
    }

    SetUIProperties() {
        this.SetUIProperties_VatUnique();
        this.SetUIProperties_VatMandatory();
        this.SetUIProperties_DemoAgent();
        this.SetUIProperties_DimensionsUnitCode();

        this.UIProperties.SetEnabled("RegulatedAgentNumber", "Tenant", this.RegulatedAgentRegimeActivated);

        if (this.TenantPm.IsDocumentsArchive) {
            this.UIProperties.SetVisibility("CustomerId", "Tenant",  true);
            this.UIProperties.SetVisibility("AgentId", "Tenant",  false);
        }

        else {
            this.UIProperties.SetVisibility("CustomerId", "Tenant", false);
            this.UIProperties.SetVisibility("AgentId", "Tenant",  true);
        }

        if (this.TenantPm.Id == 65 && SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
            this.SetUIPropertiesHitVisible();
        }
    }

    SetUIPropertiesHitVisible() {
        this.UIProperties.SetEnabled("ExportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("ImportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("ExportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("ImportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterExportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterImportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterExportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterImportOtherPrepaidCollectId", "Tenant", false);

        this.UIProperties.SetEnabled("VatMandatoryTypeCode", "Tenant", false);
        this.UIProperties.SetEnabled("VatMandatoryCountryId", "Tenant", false);
        this.UIProperties.SetEnabled("VatMandatoryForPotentialCustomers", "Tenant", false);
        this.IsVatMandatoryForPotentialCustomers = false;
        this.UIProperties.SetEnabled("VatUniqueTypeCode", "Tenant", false);
        this.UIProperties.SetEnabled("VatUniqueCountryId", "Tenant", false);

        this.UIProperties.SetEnabled("VolumeUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("WeightMeasurementUnitCode", "Tenant", false);

        this.UIProperties.SetEnabled("IATA", "Tenant", false);
        this.UIProperties.SetEnabled("CASSCode", "Tenant", false);
        this.UIProperties.SetEnabled("LocalCustomsCode", "Tenant", false);
        this.UIProperties.SetEnabled("AgentId", "Tenant", false);
        this.UIProperties.SetEnabled("CustomerId", "Tenant", false);
        this.UIProperties.SetEnabled("IsCustomerTenantShare", "Tenant", false);
        this.UIProperties.SetEnabled("CustomerTenantShareImportFile", "Tenant", false);
        this.UIProperties.SetEnabled("CustomerTenantShareExportFile", "Tenant", false);

        this.UIProperties.SetEnabled("RegulatedAgentRegimeActivated", "Tenant", false);
        this.UIProperties.SetEnabled("RegulatedAgentNumber", "Tenant", false);
    }
    SetUIProperties_DemoAgent() {
        if (this.TenantPm.Id == 65) {
            this.UIProperties.SetEnabled("AgentId", "Tenant", false);
        }
        else {
            this.UIProperties.SetEnabled("AgentId", "Tenant", true);
        }
    }

    public DimensionsDependencyProperty1: string = null;
    public DimensionsDependencyProperty1IsList: boolean = false;
    private SetUIProperties_DimensionsUnitCode() {
        var isEditingEnabled: boolean = true;
        var isFieldEnabled: boolean = false;

        if (this.TenantPm.Id == 65 && SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
            isEditingEnabled = false;
        }

        if (isEditingEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }

        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }

        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }

        //this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    }

    LoadCachedLists() {
        this.LoadPaymentTermListMethod();
    }

    // Cach Lists 
    public PaymentTermList: PaymentTermList[] = [];
    private LoadPaymentTermListMethod() {
        var myService: PaymentTermListService = new PaymentTermListService();
        var filters = new ApiQueryFilters();
        filters.GetAll = true;
        myService.getAllFromCache(filters).subscribe((myResult: any) => {
            this.PaymentTermList = myResult;
            this.SetUIProperties();
            this.UIProperties.SetVisibility("IsCustomerTenantShare", "Tenant", FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES"));
            this.UIProperties.SetVisibility("CustomerTenantShareImportFile", "Tenant", FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES"));
            this.UIProperties.SetVisibility("CustomerTenantShareExportFile", "Tenant", FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES"));

        });
    }

    // region Tenant 65
    get DemoMessageVisibility() {
        var result = false;
        if (this.TenantPm.Id == 65) {
            result = true;

            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                result = false;
            }
        }

        return result;
    }

    // Prepaid | Collect
    get ExportFreightPrepaidCollectId() { return this.TenantPm.ExportFreightPrepaidCollectId; }
    set ExportFreightPrepaidCollectId(value: string) {
        if (this.TenantPm.ExportFreightPrepaidCollectId != value) {
            this.TenantPm.ExportFreightPrepaidCollectId = value;
        }
    }

    get ImportFreightPrepaidCollectId() { return this.TenantPm.ImportFreightPrepaidCollectId; }
    set ImportFreightPrepaidCollectId(value: string) {
        if (this.TenantPm.ImportFreightPrepaidCollectId != value) {
            this.TenantPm.ImportFreightPrepaidCollectId = value;
        }
    }

    get ExportOtherPrepaidCollectId() { return this.TenantPm.ExportOtherPrepaidCollectId; }
    set ExportOtherPrepaidCollectId(value: string) {
        if (this.TenantPm.ExportOtherPrepaidCollectId != value) {
            this.TenantPm.ExportOtherPrepaidCollectId = value;
        }
    }

    get ImportOtherPrepaidCollectId() { return this.TenantPm.ImportOtherPrepaidCollectId; }
    set ImportOtherPrepaidCollectId(value: string) {
        if (this.TenantPm.ImportOtherPrepaidCollectId != value) {
            this.TenantPm.ImportOtherPrepaidCollectId = value;
        }
    }

    get MasterImportOtherPrepaidCollectId() { return this.TenantPm.MasterImportOtherPrepaidCollectId; }
    set MasterImportOtherPrepaidCollectId(value: string) {
        if (this.TenantPm.MasterImportOtherPrepaidCollectId != value) {
            this.TenantPm.MasterImportOtherPrepaidCollectId = value;
        }
    }

    get MasterExportFreightPrepaidCollectId() { return this.TenantPm.MasterExportFreightPrepaidCollectId; }
    set MasterExportFreightPrepaidCollectId(value: string) {
        if (this.TenantPm.MasterExportFreightPrepaidCollectId != value) {
            this.TenantPm.MasterExportFreightPrepaidCollectId = value;
        }
    }

    get MasterImportFreightPrepaidCollectId() { return this.TenantPm.MasterImportFreightPrepaidCollectId; }
    set MasterImportFreightPrepaidCollectId(value: string) {
        if (this.TenantPm.MasterImportFreightPrepaidCollectId != value) {
            this.TenantPm.MasterImportFreightPrepaidCollectId = value;
        }
    }

    get MasterExportOtherPrepaidCollectId() { return this.TenantPm.MasterExportOtherPrepaidCollectId; }
    set MasterExportOtherPrepaidCollectId(value: string) {
        if (this.TenantPm.MasterExportOtherPrepaidCollectId != value) {
            this.TenantPm.MasterExportOtherPrepaidCollectId = value;
        }
    }

    // VAT# is unique by country
    get VatUniqueTypeCode() { return this.TenantPm.VatUniqueTypeCode; }
    set VatUniqueTypeCode(value: string) {
        if (this.TenantPm.VatUniqueTypeCode != value) {
            this.TenantPm.VatUniqueTypeCode = value;
            this.SetUIProperties_VatUnique();
            this.VatUniqueCountryId = null;
        }
    }

    get VatUniqueCountryId() { return this.TenantPm.VatUniqueCountryId; }
    set VatUniqueCountryId(value: string) {
        if (this.TenantPm.VatUniqueCountryId != value) {
            this.TenantPm.VatUniqueCountryId = value;
            this.SetUIProperties_VatUnique();
        }
    }

    SetUIProperties_VatUnique() {
        var isEnabled: boolean = false;
        var isRequired: boolean = false;

        if (this.VatUniqueTypeCode == "USC") {
            isEnabled = true;

            if (AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetEnabled("VatUniqueCountryId", "Tenant", isEnabled);
        this.UIProperties.SetRequired("VatUniqueCountryId", "Tenant", isRequired);
    }

    // VAT# is mandatory for customers
    get VatMandatoryTypeCode() { return this.TenantPm.VatMandatoryTypeCode; }
    set VatMandatoryTypeCode(value: string) {
        if (this.TenantPm.VatMandatoryTypeCode != value) {
            this.TenantPm.VatMandatoryTypeCode = value;
            this.SetUIProperties_VatMandatory();
            this.VatMandatoryCountryId = null;
            if (value == "MNT") {
                this.VatMandatoryForPotentialCustomers = false;
            }
        }
    }

    get VatMandatoryForPotentialCustomers() { return this.TenantPm.VatMandatoryForPotentialCustomers; }
    set VatMandatoryForPotentialCustomers(value: boolean) {
        if (this.TenantPm.VatMandatoryForPotentialCustomers != value) {
            this.TenantPm.VatMandatoryForPotentialCustomers = value;
        }
    }

    get VatMandatoryCountryId() { return this.TenantPm.VatMandatoryCountryId; }
    set VatMandatoryCountryId(value: string) {
        if (this.TenantPm.VatMandatoryCountryId != value) {
            this.TenantPm.VatMandatoryCountryId = value;
            this.SetUIProperties_VatMandatory();
        }
    }

    public iVatMandatoryForPotentialCustomers = false;
    get IsVatMandatoryForPotentialCustomers() {
        return this.iVatMandatoryForPotentialCustomers;
    }

    set IsVatMandatoryForPotentialCustomers(value: boolean){
        this.iVatMandatoryForPotentialCustomers = value;
    }

    SetUIProperties_VatMandatory() {
        var isEnabled: boolean = false;
        var isRequired: boolean = false;

        if (this.VatMandatoryTypeCode == "MSC") {
            isEnabled = true;

            if (AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetEnabled("VatMandatoryCountryId", "Tenant", isEnabled);
        this.UIProperties.SetEnabled("VatMandatoryForPotentialCustomers", "Tenant", this.VatMandatoryTypeCode != "MNT");
        this.IsVatMandatoryForPotentialCustomers = this.VatMandatoryTypeCode != "MNT";
        this.UIProperties.SetRequired("VatMandatoryCountryId", "Tenant", isRequired);
    }

    // Default Units
    get VolumeUnitCode() { return this.TenantPm.VolumeUnitCode; }
    set VolumeUnitCode(value: string) {
        if (this.TenantPm.VolumeUnitCode != value) {
            this.TenantPm.VolumeUnitCode = value;
            this.SetUIProperties_DimensionsUnitCode();
            this.TenantPm.DimensionsUnitCode = AppTool.GetDimentionsCodeFromVolumeCode(value);
        }
    }

    get DimensionsUnitCode() { return this.TenantPm.DimensionsUnitCode; }
    set DimensionsUnitCode(value: string) {
        if (this.TenantPm.DimensionsUnitCode != value) {
            this.TenantPm.DimensionsUnitCode = value;
        }
    }

    get GrossWeightUnitCode() { return this.TenantPm.GrossWeightUnitCode; }
    set GrossWeightUnitCode(value: string) {
        if (this.TenantPm.GrossWeightUnitCode != value) {
            this.TenantPm.GrossWeightUnitCode = value;
        }
    }

    get ChargeableWeightUnitCode() { return this.TenantPm.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(value: string) {
        if (this.TenantPm.ChargeableWeightUnitCode != value) {
            this.TenantPm.ChargeableWeightUnitCode = value;
        }
    }

    get WeightMeasurementUnitCode() { return this.TenantPm.WeightMeasurementUnitCode; }
    set WeightMeasurementUnitCode(value: string) {
        if (this.TenantPm.WeightMeasurementUnitCode != value) {
            this.TenantPm.WeightMeasurementUnitCode = value;
        }
    }

    get TemperatureUnitCode() { return this.TenantPm.TemperatureUnitCode; }
    set TemperatureUnitCode(value: string) {
        if (this.TenantPm.TemperatureUnitCode != value) {
            this.TenantPm.TemperatureUnitCode = value;
        }
    }

    // Others 
    get PaymentTermId() {
        if (this.TenantPm.PaymentTermId == null) {
            var list: PaymentTermList = this.PaymentTermList.filter(d => d.EnglishName == "Net 30" && d.Tenant == this.TenantPm.Id)[0];
            if (list != null) {
                this.TenantPm.PaymentTermId = list.Id;
            }
        }
        return this.TenantPm.CASSCode;
    }

    set PaymentTermId(value: string) {
        if (this.TenantPm.PaymentTermId != value) {
            this.TenantPm.PaymentTermId = value;
        }
    }

    get IATA() { return this.TenantPm.IATA; }
    set IATA(value: string) {
        if (this.TenantPm.IATA != value) {
            this.TenantPm.IATA = value;
        }
    }

    get CASSCode() { return this.TenantPm.CASSCode; }
    set CASSCode(value: string) {
        if (this.TenantPm.CASSCode != value) {
            this.TenantPm.CASSCode = value;
        }
    }

    get SCACCode() { return this.TenantPm.SCACCode; }
    set SCACCode(value: string) {
        if (this.TenantPm.SCACCode != value) {
            this.TenantPm.SCACCode = value;
        }
    }

    get FMCNumber() { return this.TenantPm.FMCNumber; }
    set FMCNumber(value: string) {
        if (this.TenantPm.FMCNumber != value) {
            this.TenantPm.FMCNumber = value;
        }
    }

    get LocalCustomsCode() { return this.TenantPm.LocalCustomsCode; }
    set LocalCustomsCode(value: string) {
        if (this.TenantPm.LocalCustomsCode != value) {
            this.TenantPm.LocalCustomsCode = value;
        }
    }

    get VatNumber() { return this.TenantPm.VatNumber; }
    set VatNumber(value: string) {
        if (this.TenantPm.VatNumber != value) {
            this.TenantPm.VatNumber = value;
        }
    }

    get AgentId() { return this.TenantPm.AgentId; }
    set AgentId(value: string) {
        if (this.TenantPm.AgentId != value) {
            this.TenantPm.AgentId = value;
        }
    }

    get CustomerId() { return this.TenantPm.CustomerId; }
    set CustomerId(value: string) {
        if (this.TenantPm.CustomerId != value) {
            this.TenantPm.CustomerId = value;
        }
    }

    get IsCustomerTenantShare() { return this.TenantPm.IsCustomerTenantShare; }
    set IsCustomerTenantShare(value: boolean) {
        if (this.TenantPm.IsCustomerTenantShare != value) {
            this.TenantPm.IsCustomerTenantShare = value;
        }
    }

    get CustomerTenantShareImportFile() { return this.TenantPm.CustomerTenantShareImportFile; }
    set CustomerTenantShareImportFile(value: boolean) {
        if (this.TenantPm.CustomerTenantShareImportFile != value) {
            this.TenantPm.CustomerTenantShareImportFile = value;
        }
    }

    get CustomerTenantShareExportFile() { return this.TenantPm.CustomerTenantShareExportFile; }
    set CustomerTenantShareExportFile(value: boolean) {
        if (this.TenantPm.CustomerTenantShareExportFile != value) {
            this.TenantPm.CustomerTenantShareExportFile = value;
        }
    }

    get IsQuoteSubjectEdited() { return this.TenantPm.IsQuoteSubjectEdited; }
    set IsQuoteSubjectEdited(value: boolean) {
        if (this.TenantPm.IsQuoteSubjectEdited != value) {
            this.TenantPm.IsQuoteSubjectEdited = value;
        }
    }

    get AllowAgentInCustomersLOV() { return this.TenantPm.AllowAgentInCustomersLOV; }
    set AllowAgentInCustomersLOV(value: boolean) {
        if (this.TenantPm.AllowAgentInCustomersLOV != value) {
            this.TenantPm.AllowAgentInCustomersLOV = value;
        }
    }

    get IsCorrespondenceRightToLeftEnabled() { return this.TenantPm.IsCorrespondenceRightToLeftEnabled; }
    set IsCorrespondenceRightToLeftEnabled(value: boolean)
    {
        this.TenantPm.IsCorrespondenceRightToLeftEnabled = value;
    }

    get RightToLeftVisible()
    {
        var result = false;
        if (FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            result = true;
        }
        return result;
    }

    get IsNotesRightToLeftEnabled() { return this.TenantPm.IsNotesRightToLeftEnabled; }
    set IsNotesRightToLeftEnabled(value: boolean)
    {
        this.TenantPm.IsNotesRightToLeftEnabled = value;
    }

    get NotesRightToLeftVisible()
    {
        var result = false;
        if (FeatureLocator.HasFeaturePermession("General", "NotesRightToLeftEnabled")) {
            result = true;
        }
        return result;
    }

    get AllowAgentInCustomersLOVVisible() {
        var result = false;
        if (FeatureLocator.HasFeaturePermession("General", "AllowAgentInCustomersLOV")) {
            result = true;
        }
        return result;
    }

    // Regulated Agent
    get RegulatedAgentRegimeActivated() { return this.TenantPm.RegulatedAgentRegimeActivated; }
    set RegulatedAgentRegimeActivated(value: boolean) {
        if (this.TenantPm.RegulatedAgentRegimeActivated != value) {
            this.TenantPm.RegulatedAgentRegimeActivated = value;
            this.UIProperties.SetEnabled("RegulatedAgentNumber", "Tenant", value);
            if (!value) {
                this.RegulatedAgentNumber = null;
            }
        }
    }

    get RegulatedAgentNumber() { return this.TenantPm.RegulatedAgentNumber; }
    set RegulatedAgentNumber(value: string) {
        if (this.TenantPm.RegulatedAgentNumber != value) {
            this.TenantPm.RegulatedAgentNumber = value;
        }
    }

    get CustomerIdVisibility() {
        var customerIdVisibility = false;
        if (this.TenantPm.IsDocumentsArchive) {
            customerIdVisibility =true;
        }

        return customerIdVisibility;
    }

    get RegulatedAgentVisibility() {
        var myResult = false;
        if (SessionLocator.TenantManagementJS.ManagesRegisteredAgent) {
            myResult = true;
        }

        return myResult;
    }

    get AgentIdVisibility() {
        var agentIdVisibility = false;
        if (!this.TenantPm.IsDocumentsArchive) {
            agentIdVisibility = true;
        }

        return agentIdVisibility;
    }

    get CustomerTenantShareImportFileVisible() {
        var result = false;
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            result = true;
        }

        return result;
    }

    get CustomerTenantShareExportFileVisible() {
        var result = false;
        //var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LEX" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            result = true;
        }

        return result;
    }

    get IsCustomerTenantShareVisible() {
        var result = false;
       
    
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            result = true;
        }

        return result;
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);


        if (this.VatUniqueTypeCode == "USC") {
            if (AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                errors.push(TextCodeTranslator.Translate("Tenant.F.VatUniqueCountryId"));
            }
        }

        if (this.VatMandatoryTypeCode == "MSC") {
            if (AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                errors.push(TextCodeTranslator.Translate("Tenant.F.VatMandatoryCountryId"));
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
        myService.update(this.TenantPm).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.TenantPm;
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
