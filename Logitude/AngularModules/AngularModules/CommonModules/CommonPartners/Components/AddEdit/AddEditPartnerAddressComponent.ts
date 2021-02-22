import { Component } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AddressPM } from '../../../../Common/EntityPMs/AddressPM';
import { AddressPMService } from '../../../../Common/Services/StandardPMs/AddressPMService';
import { CitySelectionArgs } from '../../../../Common/Args';
import { StateList } from '../../../../Common/EntityLists/StateList';
import { CountryList } from '../../../../Common/EntityLists/CountryList';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { AddressTypeList } from '../../../../Common/EntityLists/AddressTypeList';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';

@Component({
    templateUrl: './AddEditPartnerAddressComponent.html',
})

export class AddEditPartnerAddressComponent extends BaseComponent {
    public EntityPM: AddressPM;
    public DataContext = this;
    public ObjectTableName: string = "Address";
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsNewEntity: boolean = false;
    public IsCustomer: boolean = false;
    public CardId: string = null;
    public PartnerTypeId: string = null;
    public AddressTypeDependencyProperty1: string = "O";
    private CurrentSession = SessionLocator.SelectedSession;
    private entityPMService: AddressPMService;
    public PartnersDomainService: PartnersDomainService;

    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new AddressPM();
        this.entityPMService = new AddressPMService();
        this.PartnersDomainService = new PartnersDomainService();
    }

    SetWindowArgs(args: any) {
        var entityId = args['EntityId'];
        var entityPM = args['EntityPM'];

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;

            this.CurrentSession.StartBusyIndicatorLoading();

            if (entityPM) {
                this.IsNewEntity = true;
                this.EntityPM = entityPM;
                this.CardId = this.EntityPM.CardId;
                this.AddressTypeDependencyProperty1 = "O";
                this.LoadCard();
            }

            else if (entityId) {
                this.IsNewEntity = false;

                this.entityPMService.get(entityId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        this.EntityPM = myResponse.Result;
                        this.CardId = this.EntityPM.CardId;
                        this.AddressTypeDependencyProperty1 = this.EntityPM.AddressTypeId;
                        this.LoadCard();
                    }
                });
            }
        });
    }
    LoadCard() {
        var listService = new CardListService();

        listService.getSingle(this.CardId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                var list: CardList = myResponse.Result;
                this.IsCustomer = list.IsCustomer;
                this.PartnerTypeId = list.PartnerTypeId;

                if (this.IsNewEntity) {
                    if (this.AddressTypeId == "M" && list.MainAddressId) {
                        this.AddressTypeId = "O";
                    }

                    else if (this.AddressTypeId == "B" && list.BillingAddressId) {
                        this.AddressTypeId = "O";
                    }

                    else if (this.AddressTypeId == "P" && list.PickAddressId) {
                        this.AddressTypeId = "O";
                    }

                    if (!list.BillingAddressId) {
                        this.AddressTypeDependencyProperty1 += ",B";
                    }

                    if (!list.PickAddressId) {
                        this.AddressTypeDependencyProperty1 += ",P";
                    }
                }
            }

            this.SetUIProperties();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    SetUIProperties() {
        this.SetUIProperties_PartnerType();
        this.SetUIProperties_Description();
        this.SetUIProperties_City();
        this.SetUIProperties_State();
        this.SetUIProperties_TelFax();
    }
    SetUIProperties_PartnerType() {
        var isEnabled = false;

        if (this.IsNewEntity) {
            isEnabled = true;
        }

        this.UIProperties.SetEnabled("AddressTypeId", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_Description() {
        var IsEditingEnabled = true;
        var isDescriptionEnabled = false;

        if (IsEditingEnabled) {
            if (this.AddressTypeId == "O") {
                isDescriptionEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("Description", this.ObjectTableName, isDescriptionEnabled);
    }
    SetUIProperties_State() {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_StateRequired() {
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
    SetUIProperties_TelFax() {
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

    SetUIProperties_City() {
        var isRequired = false;
        
        if (AppTool.IsNullOrEmpty(this.City) && this.PartnerTypeId != "PO") {
            isRequired = true;
        }
        this.UIProperties.SetRequired("City", this.ObjectTableName, isRequired);
    }

    private addressTypeList: AddressTypeList = null;
    get AddressTypeList() { return this.addressTypeList; }
    set AddressTypeList(value: AddressTypeList) {
        if (this.addressTypeList != value) {
            this.addressTypeList = value;

            if (this.IsNewEntity) {
                if (value) {
                    this.Description = value.Name;
                }

                else {
                    this.Description = null;
                }
            }
        }
    }

    get AddressTypeId() { return this.EntityPM.AddressTypeId; }
    set AddressTypeId(newValue: string) {
        if (this.EntityPM.AddressTypeId != newValue) {
            this.EntityPM.AddressTypeId = newValue;

            this.SetUIProperties_Description();
        }
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
            this.SetUIProperties_City();
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

        this.ValidateAddress(errors);

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
                this.entityPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {

                this.PartnersDomainService.PutAddress(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    this.CurrentSession.StopBusyIndicator();
                });

                //this.entityPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                //    if (myResponse.HasError) {
                //        this.ValidationErrorsList = myResponse.ErrorsArray;
                //    }

                //    else {
                //        this.CurrentSession.CloseCurrentWindowEmit("OK");
                //    }

                //    this.CurrentSession.StopBusyIndicator();
                //});
            }
        }
    }

    private ValidateAddress(errors: string[]) {
        var newPotentialAddressCity = this.EntityPM.City;
        if (AppTool.IsNullOrEmpty(this.EntityPM.City) && this.PartnerTypeId == "PO") {
            this.EntityPM.City = (AppTool.IsNullOrEmpty(this.EntityPM.City) ? " Potential city " : this.EntityPM.City);
        }

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.PartnerTypeId == "PO") {
            this.EntityPM.City = newPotentialAddressCity;
        }
    }

}
