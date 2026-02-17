import {Component} from '@angular/core';
import {BaseComponent} from '../../LogitudeComponents/BaseComponent';
import {AppTool} from '../../../Tools';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {StateListService} from '../../../../Common/Services/StandardLists/StateListService';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTimeZone, TimeZoneInfoClass, DateTimeFormat} from '../../../Utilities/DateTimeZone';
import {Validator} from '../../../Validators/Validator';
import {TextCodeTranslator} from '../../../Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './WizardAddressCompnent.html',
})

export class WizardAddressCompnent extends BaseComponent {
    public EntityPM: AddressPM = null;
    public DataContext = this;
    public ObjectTableName: string = "Address";
    public AgentPM: AgentPM = null;
    public TenantPM: TenantPM = null;
    public TimeZonesList: TimeZoneInfoClass[] = [];
    public IsEditingEnabled: boolean = true;
    constructor() {
        super();
        this.TimeZonesList = DateTimeZone.GetTimeZonesList();
        this.InitializeServices();
    }

    private myStateListService: StateListService;
    private myCountryListService: CountryListService;
    InitializeServices() {
        this.myStateListService = new StateListService();
        this.myCountryListService = new CountryListService();
    }
    InitializeComponent(tenantPM: TenantPM, addressPM: AddressPM, agentPM: AgentPM) {

        this.TenantPM = tenantPM;
        this.EntityPM = addressPM;
        this.AgentPM = agentPM;

        this.selectedTimeZone = this.TimeZonesList.filter(f => f.BaseUtcOffset == this.TenantPM.TimeZoneOffset)[0];

        if (AppTool.IsNullOrEmpty(addressPM.Description)) {
            this.EntityPM.Description = addressPM.Description = "Main Address";
        }

        if (tenantPM.DayLightOffset != 0) {
            this.ShowDayLightSettings = false;
        }

        this.SetUIProperties();
    }
    Validate(errors: string[]) {
        
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        Validator.TryValidateObject(this.EntityPM, "Address", errors);

        if (AppTool.IsNullOrEmpty(this.City)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Address.F.City")));
        }

        if (AppTool.IsNullOrEmpty(this.CountryId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Address.F.CountryId")));
        }

        if (AppTool.IsNullOrEmpty(this.StateId)) {
            if (this.IsStateRequired) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Address.F.State")));
            }
        }

        if (AppTool.IsNullOrEmpty(this.TimeZoneOffset)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Tenant.F.TimeZoneOffset")));
        }

        return errors;
    }

    SetUIProperties() {
        this.IsEditingEnabled = true;
        this.UIProperties.SetRequired("City", "Address", AppTool.IsNullOrEmpty(this.City) ? true : false);
        this.UIProperties.SetRequired("CountryId", "Address", AppTool.IsNullOrEmpty(this.CountryId) ? true : false);
        this.SetUIProperties_State();
        this.SetUIProperties_TimeZone();
    }
    SetUIProperties_State() {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.IsEditingEnabled) {
            isEnabled = this.HasStates;
        }

        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.IsStateRequired) {
            if (AppTool.IsNullOrEmpty(this.StateId)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    }
    SetUIProperties_TimeZone() {
        var isRequired: boolean = false;

        if (AppTool.IsNullOrEmpty(this.TimeZoneOffset)) {
            isRequired = true;
        }

        this.UIProperties.SetRequired("TimeZoneOffset", "Tenant", isRequired);
    }

    // Address 
    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
            this.TenantPM.Company = value;
            this.AgentPM.EnglishName = value;
        }
    }

    get Address1() { return this.EntityPM.Address1; }
    set Address1(value: string) {
        if (this.EntityPM.Address1 != value) {
            this.EntityPM.Address1 = value;
        }
    }

    get Address2() { return this.EntityPM.Address2; }
    set Address2(value: string) {
        if (this.EntityPM.Address2 != value) {
            this.EntityPM.Address2 = value;
        }
    }

    get Signature() { return this.EntityPM.Signature; }
    set Signature(value: string) {
        if (this.EntityPM.Signature != value) {
            this.EntityPM.Signature = value;
            this.TenantPM.Signature = value;
        }
    }

    get City() { return this.EntityPM.City; }
    set City(value: string) {
        if (this.EntityPM.City != value) {
            this.EntityPM.City = value;
            this.UIProperties.SetRequired("City", "Address", AppTool.IsNullOrEmpty(value) ? true : false);
        }
    }

    get FaxNumber() { return this.EntityPM.FaxNumber; }
    set FaxNumber(value: string) {
        if (this.EntityPM.FaxNumber != value) {
            this.EntityPM.FaxNumber = value;
        }
    }

    get ZipCode() { return this.EntityPM.ZipCode; }
    set ZipCode(value: string) {
        if (this.EntityPM.ZipCode != value) {
            this.EntityPM.ZipCode = value;
        }
    }

    get PhoneNumber() { return this.EntityPM.PhoneNumber; }
    set PhoneNumber(value: string) {
        if (this.EntityPM.PhoneNumber != value) {
            this.EntityPM.PhoneNumber = value;
            SessionLocator.LoggedUserPM.BusinessPhone = value;
        }
    }

    get StateId() { return this.EntityPM.StateId; }
    set StateId(value: string) {
        if (this.EntityPM.StateId != value) {
            this.EntityPM.StateId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.StateCode = null;
                this.StateEnglishName = null;
                this.SetUIProperties_StateRequired();
            }

            else {
                this.myStateListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: StateList = myResponse.Result;
                        if (list) {
                            this.StateCode = list.Code;
                            this.StateEnglishName = list.EnglishName;
                            this.SetUIProperties_StateRequired();
                        }
                    }
                });
            }
        }
    }

    get StateCode() { return this.EntityPM.StateCode; }
    set StateCode(value: string) {
        if (this.EntityPM.StateCode != value) {
            this.EntityPM.StateCode = value;
        }
    }

    get StateEnglishName() { return this.EntityPM.StateEnglishName; }
    set StateEnglishName(value: string) {
        if (this.EntityPM.StateEnglishName != value) {
            this.EntityPM.StateEnglishName = value;
        }
    }

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(value: string) {
        if (this.EntityPM.CountryId != value) {
            this.EntityPM.CountryId = value;
            this.StateId = null;
            this.UIProperties.SetRequired("CountryId", "Address", AppTool.IsNullOrEmpty(value) ? true : false);
                       
            if (AppTool.IsNullOrEmpty(value)) {
                this.TenantPM.CountryCode = null;
                this.TenantPM.CountryName = null;
                this.CountryCode = null;
                this.CountryName = null;
                this.CountryEnglishName = null;
                this.HasStates = false;
                this.IsStateRequired = false;
                this.SetUIProperties_State();
            }

            else {
                this.myCountryListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CountryList = myResponse.Result;
                        if (list) {
                            this.TenantPM.CountryCode = list.Code;
                            this.TenantPM.CountryName = list.EnglishName;
                            this.CountryCode = list.Code;
                            this.CountryName = list.EnglishName;
                            this.CountryEnglishName = list.EnglishName;
                            this.HasStates = list.HasStates;
                            this.IsStateRequired = list.IsStateRequired;
                            this.SetUIProperties_State();
                        }
                    }
                });
            }
        }
    }

    get CountryCode() { return this.EntityPM.CountryCode; }
    set CountryCode(value: string) {
        if (this.EntityPM.CountryCode != value) {
            this.EntityPM.CountryCode = value;
        }
    }

    get CountryName() { return this.EntityPM.CountryName; }
    set CountryName(value: string) {
        if (this.EntityPM.CountryName != value) {
            this.EntityPM.CountryName = value;
        }
    }

    get CountryEnglishName() { return this.EntityPM.CountryEnglishName; }
    set CountryEnglishName(value: string) {
        if (this.EntityPM.CountryEnglishName != value) {
            this.EntityPM.CountryEnglishName = value;
        }
    }

    get HasStates() { return this.EntityPM.HasStates; }
    set HasStates(value: boolean) {
        if (this.EntityPM.HasStates != value) {
            this.EntityPM.HasStates = value;
        }
    }

    get IsStateRequired() { return this.EntityPM.IsStateRequired; }
    set IsStateRequired(value: boolean) {
        if (this.EntityPM.IsStateRequired != value) {
            this.EntityPM.IsStateRequired = value;
        }
    }

    // Tenant
    get TimeZoneOffset() { return this.TenantPM.TimeZoneOffset; }
    set TimeZoneOffset(value: number) {
        if (this.TenantPM.TimeZoneOffset != value) {
            this.TenantPM.TimeZoneOffset = value;
        }
    }

    get DayLightStartDate() { return this.TenantPM.DayLightStartDate; }
    set DayLightStartDate(value: Date) {
        if (this.TenantPM.DayLightStartDate != value) {
            this.TenantPM.DayLightStartDate = value;
        }
    }

    get DayLightEndDate() { return this.TenantPM.DayLightEndDate; }
    set DayLightEndDate(value: Date) {
        if (this.TenantPM.DayLightEndDate != value) {
            this.TenantPM.DayLightEndDate = value;
        }
    }

    get DayLightOffset() { return this.TenantPM.DayLightOffset; }
    set DayLightOffset(value: number) {
        if (this.TenantPM.DayLightOffset != value) {
            this.TenantPM.DayLightOffset = value;
        }
    }

    private showDayLightSettings: boolean;
    get ShowDayLightSettings() { return this.showDayLightSettings; }
    set ShowDayLightSettings(value: boolean) {
        if (this.showDayLightSettings != value) {
            this.showDayLightSettings = value;

            if (!value) {
                this.DayLightOffset = 0;
                this.DayLightStartDate = null;
                this.DayLightEndDate = null;
            }
        }
    }

    private selectedTimeZone: TimeZoneInfoClass;
    get SelectedTimeZone() { return this.selectedTimeZone; }
    set SelectedTimeZone(value: TimeZoneInfoClass) {
        if (this.selectedTimeZone != value) {
            this.selectedTimeZone = value;

            if (value) {
                this.TenantPM.TimeZoneOffset = value.BaseUtcOffset;
            }

            else {
                this.TenantPM.TimeZoneOffset = 0;
            }

            this.SetUIProperties_TimeZone();
        }
    }
}

  
