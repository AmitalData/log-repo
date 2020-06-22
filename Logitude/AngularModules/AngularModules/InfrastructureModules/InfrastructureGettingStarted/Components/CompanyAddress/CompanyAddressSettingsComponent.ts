import {Component,OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {CitySelectionArgs} from '../../../../Common/Args';
import {AddressPMService} from '../../../../Common/Services/StandardPMs/AddressPMService';
import {AgentPMService} from '../../../../Common/Services/StandardPMs/AgentPMService';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'CompanyAddressSettingsComponent',
    
    templateUrl: './CompanyAddressSettingsComponent.html',
})

export class CompanyAddressSettingsComponent extends BaseComponent implements OnInit  {
    public TenantPm: TenantPM = new TenantPM();
    public TenantAgent: AgentPM = new AgentPM();
    public TenantAddress: AddressPM = new AddressPM();
    public LocalTenantAddress: AddressPM = new AddressPM();
    public DataContext: CompanyAddressSettingsComponent = this;
    public AddressObjectTableName: string = "Address";
    public AgentObjectTableName: string = "Agent";
    public TenantObjectTableName: string = "Tenant";
    public IsVisibile: boolean = false;
    public IsLocalAddressTabVisible: boolean = false;
    public SelectedTabCode: string = "0";
    public LocalAddressDataContext: AddressItem; 
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public IsDemoTenant = false;

    constructor() {
        super();      
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response=> {
            this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(response2=> {              
                this.LoadTenantPMMethod();
            });
        });
    }

    LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();

        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            var pmResponse: ServiceResponse = response;
            if (!pmResponse.HasError) {
                this.TenantPm = response.Result;
                this.LoadAddressPM();
                this.GetDemoMessageVisibility();

                if (ObjectsLocator.IsDemoTenant(this.TenantPm.Id.toString()) && SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
                    this.IsDemoTenant = true;
                    this.SetUIPropertiesHitVisible();
                }

                if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLocalAddress")) {
                    this.LoadLocalAddress();
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                }
            }
        });
    }

    LoadAddressPM() {
        if (AppTool.IsNullOrEmpty(this.TenantPm.AddressId)) {
            this.TenantAddress = new AddressPM();
            this.TenantAddress.Tenant = this.TenantPm.Id;
            this.TenantAddress.AddressTypeId = "M";

            this.InitializeData();
            this.IsVisibile = true;
        }

        else {
            var myService: AddressPMService = new AddressPMService();
            myService.get(this.TenantPm.AddressId).subscribe((myResult: ServiceResponse) => {
                this.TenantAddress = myResult.Result;
                this.InitializeData();
                this.IsVisibile = true;
            });
        }
    }

    LoadLocalAddress() {
        if (AppTool.IsNullOrEmpty(this.TenantPm.LocalAddressId)) {
            this.LocalTenantAddress = new AddressPM();
            this.LocalTenantAddress.Tenant = this.TenantPm.Id;
            this.LocalTenantAddress.AddressTypeId = "L";

            this.LocalAddressDataContext = new AddressItem(this.LocalTenantAddress, this.TenantPm, this);
            this.IsLocalAddressTabVisible = true;
        }

        else {
            var myService: AddressPMService = new AddressPMService();
            myService.get(this.TenantPm.LocalAddressId).subscribe((myResult: ServiceResponse) => {
                this.LocalTenantAddress = myResult.Result;

                this.LocalAddressDataContext = new AddressItem(this.LocalTenantAddress, this.TenantPm, this);
                this.IsLocalAddressTabVisible = true;
            });
        }
    }

    InitializeData() {
        if (this.TenantAddress != null) {
            var countryListService: CountryListService = new CountryListService();
            countryListService.getAllFromCache().subscribe((result:any) => {                
                this.SetUIProperties_State();

                if (AppTool.IsNullOrEmpty(this.TenantAddress.Id)) {
                    this.TenantAddress.Name = this.TenantPm.Company;
                    this.TenantAddress.Description = this.TenantPm.Company;
                    this.TenantAddress.PhoneNumber = SessionLocator.LoggedUserPM.BusinessPhone;

                    this.TenantAgent = new AgentPM();
                    this.TenantAgent.Code = "new";
                    this.TenantAgent.PartnerTypeId = "AG";
                    this.TenantAgent.Tenant = 1;
                    this.TenantAgent.EnglishName = "new";
                }

                else {
                    if (AppTool.IsNullOrEmpty(this.TenantPm.AgentId)) {
                        this.TenantAgent = new AgentPM();

                        this.TenantAgent = new AgentPM();
                        this.TenantAgent.Code = "new";
                        this.TenantAgent.PartnerTypeId = "AG";
                        this.TenantAgent.Tenant = 1;
                        this.TenantAgent.EnglishName = "new";
                        this.TenantAgent.TenantAddressId = this.TenantAddress.Id;
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.TenantPm.AgentId)) {
                    this.LoadTenantAgentMethod();
                }
            });
        }
    }

    SetUIPropertiesHitVisible() {
        this.UIProperties.SetEnabled("Company", this.TenantObjectTableName, false);
        this.UIProperties.SetEnabled("Address1", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("Address2", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("City", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("CountryId", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("ZipCode", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("Signature", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("PhoneNumber", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("FaxNumber", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("StateId", this.AddressObjectTableName, false);       
    }

    // Cach Lists 
    private LoadTenantAgentMethod() {
        var myService = new AgentPMService();
        myService.get(this.TenantPm.AgentId).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.TenantAgent = myResult.Result;
            }
        });
    }

    //Tenant 65
    public DemoMessageVisibility: boolean = false;
    private GetDemoMessageVisibility() {
        var result = false;
        if (ObjectsLocator.IsDemoTenant(this.TenantPm.Id.toString())) {
            result = true;

            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                result = false;
            }
        }
        this.DemoMessageVisibility = result;
    }

    // Properties 
    get Company() { return this.TenantPm.Company; }
    set Company(value: string) {
        if (this.TenantPm.Company != value) {
            this.TenantPm.Company = value;
            this.TenantAddress.Name = value;
            this.TenantAgent.EnglishName = value;
            this.TenantAddress.Description = value;
            if (AppTool.IsNullOrEmpty(value)){

                this.UIProperties.SetRequired("Name", "Address", true);
            }
            else {

                this.UIProperties.SetRequired("Name", "Address", false);
            }
        }
    }

    get Name()
    {
        return this.TenantAddress.Name;
    }
    set Name(value: string) {
        if (this.TenantAddress.Name != value) {
            this.TenantAddress.Name = value;
        }
    }

    get Address1()
    {
        return this.TenantAddress.Address1;
    }
    set Address1(value: string) {
        if (this.TenantAddress.Address1 != value) {
            this.TenantAddress.Address1 = value;
        }
    }

    get Address2() {
        return this.TenantAddress.Address2;
    }
    set Address2(value: string) {
        if (this.TenantAddress.Address2 != value) {
            this.TenantAddress.Address2 = value;
        }
    }

    get Signature() {
        return this.TenantPm.Signature;
    }
    set Signature(value: string) {
        if (this.TenantPm.Signature != value) {
            this.TenantPm.Signature = value;
        }
    }

    get City() {
        return this.TenantAddress.City;
    }
    set City(value: string) {
        if (this.TenantAddress.City != value) {
            this.TenantAddress.City = value;
            if (AppTool.IsNullOrEmpty(value)) {

                this.UIProperties.SetRequired("City", "Address", true);
            }
            else {

                this.UIProperties.SetRequired("City", "Address", false);
            }
        }
    }

    get FaxNumber() {
        return this.TenantAddress.FaxNumber;
    }
    set FaxNumber(value: string) {
        if (this.TenantAddress.FaxNumber != value) {
            this.TenantAddress.FaxNumber = value;
           
        }
    }

    get ZipCode() {
        return this.TenantAddress.ZipCode;
    }
    set ZipCode(value: string) {
        if (this.TenantAddress.ZipCode != value) {
            this.TenantAddress.ZipCode = value;

        }
    }

    get PhoneNumber() {
        return this.TenantAddress.PhoneNumber;
    }
    set PhoneNumber(value: string) {
        if (this.TenantAddress.PhoneNumber != value) {
            this.TenantAddress.PhoneNumber = value;
            SessionLocator.LoggedUserPM.BusinessPhone = value;
        }
    }

    private state: StateList = null;
    get State() { return this.state; }
    set State(value: StateList) {
        if (this.state != value) {
            this.state = value;
            this.OnStateChanged(value);
        }
    }

    get StateId() { return this.TenantAddress.StateId; }
    set StateId(value: string) {
        if (this.TenantAddress.StateId != value) {
            this.TenantAddress.StateId = value;
        }
    }

    get StateCode() { return this.TenantAddress.StateCode; }
    set StateCode(newValue: string) {
        if (this.TenantAddress.StateCode != newValue) {
            this.TenantAddress.StateCode = newValue;
        }
    }

    get StateEnglishName() { return this.TenantAddress.StateEnglishName; }
    set StateEnglishName(newValue: string) {
        if (this.TenantAddress.StateEnglishName != newValue) {
            this.TenantAddress.StateEnglishName = newValue;
        }
    }

    private country: CountryList = null;
    get Country() { return this.country; }
    set Country(newValue: CountryList) {
        if (this.country != newValue) {
            this.country = newValue;
            this.OnCountryChanged(newValue);
        }
    }

    get CountryId() { return this.TenantAddress.CountryId; }
    set CountryId(value: string) {
        if (this.TenantAddress.CountryId != value) {
            this.TenantAddress.CountryId = value;
            this.StateId = null;
        }
    }

    get CountryCode() { return this.TenantAddress.CountryCode; }
    set CountryCode(value: string) {
        if (this.TenantAddress.CountryCode != value) {
            this.TenantAddress.CountryCode = value;
        }
    }

    get CountryName() { return this.TenantAddress.CountryName; }
    set CountryName(value: string) {
        if (this.TenantAddress.CountryName != value) {
            this.TenantAddress.CountryName = value;
        }
    }

    get CountryEnglishName() { return this.TenantAddress.CountryEnglishName; }
    set CountryEnglishName(value: string) {
        if (this.TenantAddress.CountryEnglishName != value) {
            this.TenantAddress.CountryEnglishName = value;
        }
    }

    private OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
        }

        else {
            this.CountryCode = list.Code;
            this.CountryName = this.TenantAddress.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }

        this.SetUIProperties_State();
    }
    private OnStateChanged(list: StateList) {
        if (list == null) {
            this.StateCode = null;
            this.StateEnglishName = null;
        }

        else {
            this.StateCode = list.Code;
            this.StateEnglishName = list.EnglishName;
        }

        this.SetUIProperties_StateRequired();
    }

    private SetUIProperties_State() {
        if (this.TenantAddress != null) {
            this.SetUIProperties_StateEnabled();
            this.SetUIProperties_StateRequired();
        }
    }
    private SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("StateId", "Address", isEnabled && !this.IsDemoTenant);
    }
    private SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("StateId", "Address", isRequired);
    }
    
    // Commands 
    SelectCityCommand() {

        var args = new CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                var mySelectedCity: string = args.CityName;
                if (this.TenantAddress.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }

                this.City = mySelectedCity;
                this.CountryId = args.CountryId;
                this.StateId = args.StateId;
            }
        });
    } 

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.TenantAddress, this.DataContext.AddressObjectTableName, errors);
        Validator.TryValidateObject(this.TenantAgent, this.DataContext.AgentObjectTableName, errors);
        Validator.TryValidateObject(this.TenantPm, this.DataContext.TenantObjectTableName, errors);

        if (this.LocalAddressDataContext != null) {
            Validator.TryValidateObject(this.LocalAddressDataContext.Address, this.LocalAddressDataContext.ObjectTableName, errors);
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (AppTool.IsNullOrEmpty(this.TenantAddress.Id)) {
                this.SubmitCreatingAgent();
            }

            else {
                this.SubmitUpdatingAgent();
            }
        }
    }

    SubmitCreatingAgent() {
        var myService: AgentPMService = new AgentPMService();
        myService.insert(this.TenantAgent).subscribe((myRespone: ServiceResponse) => {

            if (myRespone != null) {
                if (!myRespone.HasError) {
                    this.OnSaveAgentCompletedSuccessfully();
                }

                else {
                    this.ValidationErrorsList = myRespone.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    SubmitUpdatingAgent() {
        var myService: AgentPMService = new AgentPMService();
        myService.update(this.TenantAgent).subscribe((myRespone: ServiceResponse)  => {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    this.OnSaveAgentCompletedSuccessfully();
                }

                else {
                    this.ValidationErrorsList = myRespone.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    SubmitUpdatingAddress(type: string) {
        var address: AddressPM = null;
        if (type == "M") {
            address = this.TenantAddress;
        }
        else if (type == "L") {
            address = this.LocalTenantAddress;
        }

        if (address != null && address.IsDirty) {
            var myService: AddressPMService = new AddressPMService();
            myService.update(address).subscribe((myRespone: ServiceResponse) => {
                if (myRespone != null) {
                    if (!myRespone.HasError) {
                        this.SubmitTenantChanges();
                    }

                    else {
                        this.ValidationErrorsList = myRespone.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }

        else if (this.TenantPm.IsDirty) {
            this.SubmitTenantChanges();
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }

    SubmitCreatingAddress(type: string) {
        var address: AddressPM = null;
        if (type == "M") {
            address = this.TenantAddress;
        }
        else if (type == "L") {
            address = this.LocalTenantAddress;
        }

        if (address != null && address.IsDirty) {
            var myService: AddressPMService = new AddressPMService();
            myService.insert(address).subscribe((myRespone: ServiceResponse) => {
                if (myRespone != null) {
                    if (!myRespone.HasError) {
                        if (type == "L" && AppTool.IsNullOrEmpty(this.TenantPm.LocalAddressId)) {
                            this.TenantPm.LocalAddressId = myRespone.Result.Id;
                        }

                        this.SubmitTenantChanges();
                    }

                    else {
                        this.ValidationErrorsList = myRespone.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    }

    OnSaveAgentCompletedSuccessfully() {
        if (AppTool.IsNullOrEmpty(this.TenantAddress.Id)) {
            this.SubmitCreatingAddress("M");
        }
        else {
            this.SubmitUpdatingAddress("M");
        }

        if (AppTool.IsNullOrEmpty(this.LocalTenantAddress.Id)) {
            this.SubmitCreatingAddress("L");
        }
        else {
            this.SubmitUpdatingAddress("L");
        }
    }

    SubmitTenantChanges() {
        var myService: TenantPMService = new TenantPMService();
        myService.update(this.TenantPm).subscribe((myRespone: ServiceResponse) => {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    InfraSettings.TenantPM = this.TenantPm;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = myRespone.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}

export class AddressItem extends BaseComponent {
    public ObjectTableName: string = "Address";
    public Address: AddressPM;
    public Tenant: TenantPM;
    public DemoMessageVisibility: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(address: AddressPM, tenant: TenantPM, public father: CompanyAddressSettingsComponent) {
        super();

        this.Address = address;
        this.Tenant = tenant;
        this.DemoMessageVisibility = this.father.DemoMessageVisibility;
    }
    
    get Name() { return this.Address.Name; }
    set Name(newValue: string) {
        if (this.Address.Name != newValue) {
            this.Address.Name = newValue;
            this.Address.Description = newValue;
        }
    }

    get Address1() { return this.Address.Address1; }
    set Address1(newValue: string) {
        if (this.Address.Address1 != newValue) {
            this.Address.Address1 = newValue;            
        }
    }

    get Address2() { return this.Address.Address2; }
    set Address2(newValue: string) {
        if (this.Address.Address2 != newValue) {
            this.Address.Address2 = newValue;
        }
    }

    get City() { return this.Address.City; }
    set City(newValue: string) {
        if (this.Address.City != newValue) {
            this.Address.City = newValue;
        }
    }

    get CountryId() { return this.Address.CountryId; }
    set CountryId(newValue: string) {
        if (this.Address.CountryId != newValue) {
            this.Address.CountryId = newValue;
        }
    }

    get StateId() { return this.Address.StateId; }
    set StateId(newValue: string) {
        if (this.Address.StateId != newValue) {
            this.Address.StateId = newValue;
        }
    }
    
    get ZipCode() { return this.Address.ZipCode; }
    set ZipCode(newValue: string) {
        if (this.Address.ZipCode != newValue) {
            this.Address.ZipCode = newValue;
        }
    }

    get Signature() { return this.Address.Signature; }
    set Signature(newValue: string) {
        if (this.Address.Signature != newValue) {
            this.Address.Signature = newValue;
        }
    }

    get PhoneNumber() { return this.Address.PhoneNumber; }
    set PhoneNumber(newValue: string) {
        if (this.Address.PhoneNumber != newValue) {
            this.Address.PhoneNumber = newValue;
        }
    }

    get FaxNumber() { return this.Address.FaxNumber; }
    set FaxNumber(newValue: string) {
        if (this.Address.FaxNumber != newValue) {
            this.Address.FaxNumber = newValue;
        }
    }

    private country: CountryList = null;
    get Country() { return this.country; }
    set Country(newValue: CountryList) {
        if (this.country != newValue) {
            this.country = newValue;
            this.OnCountryChanged(newValue);
        }
    }

    private state: StateList = null;
    get State() { return this.state; }
    set State(value: StateList) {
        if (this.state != value) {
            this.state = value;
            this.OnStateChanged(value);
        }
    }

    private OnCountryChanged(list: CountryList) {
        this.SetUIProperties_State();
    }
    private OnStateChanged(list: StateList) {
        this.SetUIProperties_StateRequired();
    }

    private SetUIProperties_State() {
        if (this.Address != null) {
            this.SetUIProperties_StateEnabled();
            this.SetUIProperties_StateRequired();
        }
    }
    private SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("StateId", "Address", isEnabled && !this.father.IsDemoTenant);
    }
    private SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("StateId", "Address", isRequired);
    }

    SelectCityCommand() {
        var args = new CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                var mySelectedCity: string = args.CityName;
                if (this.Address.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }

                this.City = mySelectedCity;
                this.CountryId = args.CountryId;
                this.StateId = args.StateId;
            }
        });
    }   
}
