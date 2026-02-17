import {Component} from '@angular/core';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {AddressPMService} from '../../../../Common/Services/StandardPMs/AddressPMService';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CitySelectionArgs} from '../../../../Common/Args';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAddressComponent.html',
})

export class AddEditAddressComponent extends BaseComponent {
    public EntityPM: AddressPM;
    public DataContext = this;
    public ObjectTableName: string = "Address";
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsNewEntity: boolean = false;
    public PartnerTypeId: string = null;
    private myService: AddressPMService;
    private IsCustomer: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new AddressPM();
        this.myService = new AddressPMService();
    }

    SetWindowArgs(args: any) {
        var entityId = args['EntityId'];
        var entityPM = args['EntityPM'];
        this.PartnerTypeId = args['PartnerTypeId'];
        this.IsCustomer = args['IsCustomer'];

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;

            if (!AppTool.IsNullOrEmpty(entityId)) {

                this.CurrentSession.StartBusyIndicatorLoading();

                this.myService.get(entityId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.EntityPM = myResponse.Result;
                    }

                    this.CurrentSession.StopBusyIndicator();
                    this.SetUIProperties();
                });
            }

            else if (entityPM != null) {                
                this.IsNewEntity = true;
                this.EntityPM = entityPM;

                if (!this.EntityPM.InActive) {
                    this.EntityPM.InActive = false;
                }

                this.SetUIProperties();
            }
        });
    }

    public SetUIProperties() {
        this.SetUIProperties_State();
        this.SetUIProperties_TelFax();
    }
    private SetUIProperties_State() {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    private SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
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

        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    }
    private SetUIProperties_TelFax() {
        var isTelRequired = false;
        var isFaxRequired = false;

        if (this.IsCustomer) {
            if (this.PartnerTypeId == "CS") {
                if (SessionLocator.TenantPM.IsCustomerTelRequired) {
                    if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                        isTelRequired = true;
                    }
                }

                if (SessionLocator.TenantPM.IsCustomerFaxRequired) {
                    if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                        isFaxRequired = true;
                    }
                }
            }

            else if (this.PartnerTypeId == "PO") {
                if (SessionLocator.TenantPM.IsPotentialTelRequired) {
                    if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                        isTelRequired = true;
                    }
                }

                if (SessionLocator.TenantPM.IsPotentialFaxRequired) {
                    if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                        isFaxRequired = true;
                    }
                }
            }
        }

        this.UIProperties.SetRequired("PhoneNumber", this.ObjectTableName, isTelRequired);
        this.UIProperties.SetRequired("FaxNumber", this.ObjectTableName, isFaxRequired);
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Address1() { return this.EntityPM.Address1; }
    set Address1(newValue: string) {
        if (this.EntityPM.Address1 != newValue) {
            this.EntityPM.Address1 = newValue;
        }
    }

    get Address2() { return this.EntityPM.Address2; }
    set Address2(newValue: string) {
        if (this.EntityPM.Address2 != newValue) {
            this.EntityPM.Address2 = newValue;
        }
    }

    get ZipCode() { return this.EntityPM.ZipCode; }
    set ZipCode(newValue: string) {
        if (this.EntityPM.ZipCode != newValue) {
            this.EntityPM.ZipCode = newValue;
        }
    }

    get City() { return this.EntityPM.City; }
    set City(newValue: string) {
        if (this.EntityPM.City != newValue) {
            this.EntityPM.City = newValue;
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

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(newValue: string) {
        if (this.EntityPM.CountryId != newValue) {
            this.EntityPM.CountryId = newValue;
            this.StateId = null;
        }
    }

    get CountryCode() { return this.EntityPM.CountryCode; }
    set CountryCode(newValue: string) {
        if (this.EntityPM.CountryCode != newValue) {
            this.EntityPM.CountryCode = newValue;
        }
    }

    get CountryName() { return this.EntityPM.CountryName; }
    set CountryName(newValue: string) {
        if (this.EntityPM.CountryName != newValue) {
            this.EntityPM.CountryName = newValue;
        }
    }

    get CountryEnglishName() { return this.EntityPM.CountryEnglishName; }
    set CountryEnglishName(newValue: string) {
        if (this.EntityPM.CountryEnglishName != newValue) {
            this.EntityPM.CountryEnglishName = newValue;
        }
    }

    private state: StateList = null;
    get State() { return this.state; }
    set State(newValue: StateList) {
        if (this.state != newValue) {
            this.state = newValue;
            this.OnStateChanged(newValue);
        }
    }

    get StateId() { return this.EntityPM.StateId; }
    set StateId(newValue: string) {
        if (this.EntityPM.StateId != newValue) {
            this.EntityPM.StateId = newValue;
        }
    }

    get StateCode() { return this.EntityPM.StateCode; }
    set StateCode(newValue: string) {
        if (this.EntityPM.StateCode != newValue) {
            this.EntityPM.StateCode = newValue;
        }
    }

    get StateEnglishName() { return this.EntityPM.StateEnglishName; }
    set StateEnglishName(newValue: string) {
        if (this.EntityPM.StateEnglishName != newValue) {
            this.EntityPM.StateEnglishName = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    get PhoneNumber() { return this.EntityPM.PhoneNumber; }
    set PhoneNumber(newValue: string) {
        if (this.EntityPM.PhoneNumber != newValue) {
            this.EntityPM.PhoneNumber = newValue;
            this.SetUIProperties_TelFax();
        }
    }

    get FaxNumber() { return this.EntityPM.FaxNumber; }
    set FaxNumber(newValue: string) {
        if (this.EntityPM.FaxNumber != newValue) {
            this.EntityPM.FaxNumber = newValue;
            this.SetUIProperties_TelFax();
        }
    }

    get ATTN() { return this.EntityPM.ATTN; }
    set ATTN(newValue: string) {
        if (this.EntityPM.ATTN != newValue) {
            this.EntityPM.ATTN = newValue;
        }
    }

    get IsLocalLanguage() { return this.EntityPM.IsLocalLanguage; }
    set IsLocalLanguage(newValue: boolean) {
        if (this.EntityPM.IsLocalLanguage != newValue) {
            this.EntityPM.IsLocalLanguage = newValue;

            if (this.Country != null) {
                this.CountryName = this.EntityPM.IsLocalLanguage ? this.Country.LocalName : this.Country.EnglishName;
            }

            if (!newValue) {
                if (!AppTool.IsNullOrEmpty(this.Description)) {
                    this.Description = this.Description.replace(/[^\x20-\x7F]/g, "");
                }

                if (!AppTool.IsNullOrEmpty(this.Name)) {
                    this.Name = this.Name.replace(/[^\x20-\x7F]/g, "");
                }

                if (!AppTool.IsNullOrEmpty(this.Address1)) {
                    this.Address1 = this.Address1.replace(/[^\x20-\x7F]/g, "");
                }

                if (!AppTool.IsNullOrEmpty(this.Address2)) {
                    this.Address2 = this.Address2.replace(/[^\x20-\x7F]/g, "");
                }

                if (!AppTool.IsNullOrEmpty(this.City)) {
                    this.City = this.City.replace(/[^\x20-\x7F]/g, "");
                }

                if (!AppTool.IsNullOrEmpty(this.ATTN)) {
                    this.ATTN = this.ATTN.replace(/[^\x20-\x7F]/g, "");
                }
            }
        }
    }

    get AddressTypeId() { return this.EntityPM.AddressTypeId; }

    private OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
        }

        else {
            this.CountryCode = list.Code;
            this.CountryName = this.IsLocalLanguage ? list.LocalName : list.EnglishName;
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

    SelectCityCommand() {
        var args = new CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                var mySelectedCity: string = args.CityName;
                if (this.EntityPM.IsLocalLanguage && args.CityLocalName != null) {
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

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.IsCustomer) {
            if (this.PartnerTypeId == "CS") {
                if (SessionLocator.TenantPM.IsCustomerTelRequired) {
                    if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                        errors.push("Phone Number is required");
                    }
                }

                if (SessionLocator.TenantPM.IsCustomerFaxRequired) {
                    if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                        errors.push("Fax Number is required");
                    }
                }
            }

            else if (this.PartnerTypeId == "PO") {
                if (SessionLocator.TenantPM.IsPotentialTelRequired) {
                    if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                        errors.push("Phone Number is required");
                    }
                }

                if (SessionLocator.TenantPM.IsPotentialFaxRequired) {
                    if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                        errors.push("Fax Number is required");
                    }
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            if (this.IsNewEntity) {
                this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }

            else {
                this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    }
}
