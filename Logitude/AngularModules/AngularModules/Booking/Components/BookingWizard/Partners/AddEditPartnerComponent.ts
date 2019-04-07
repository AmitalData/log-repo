import {Component, AfterViewInit} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {BookingPM} from '../../../EntityPMs/BookingPM';
import {PartnersTabComponent} from './PartnersTabComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../../Common/Args';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {AddressPMService} from '../../../../Common/Services/StandardPMs/AddressPMService';
import {AgentPMService} from '../../../../Common/Services/StandardPMs/AgentPMService';
import {CustomerPMService} from '../../../../Common/Services/StandardPMs/CustomerPMService';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {AddressValidator} from '../../../../Infrastructure/Validators/AddressValidator';
import {VatNumberValidator, VATValidatorArgs} from '../../../../Infrastructure/Validators/VatNumberValidator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AddEditPartnerArgs} from '../../../Args';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPartnerComponent.html',
})

export class AddEditPartnerComponent extends BaseComponent implements AfterViewInit {
    private bookingPM: BookingPM;
    public EntityPM: AddressPM = null;
    public IsNewEntity: boolean = false;
    public IsCancelled: boolean = false;
    public PartnerTypeId: string;
    public PartnerTypeCode: string;
    public CurrentPartnerId: string;
    public CurrentAddressId: string;
    public FatherComponent: PartnersTabComponent;
    public DataContext: AddEditPartnerComponent = this;
    public ObjectTableName: string = "Address";
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: AddEditPartnerArgs) {
        this.bookingPM = windowArgs.EntityPM;
        this.IsNewEntity = windowArgs.IsNewEntity;
        this.PartnerTypeCode = windowArgs.PartnerTypeCode;
        this.FatherComponent = windowArgs.FatherComponent;
        this.InitializeComponent();
    }

    ngAfterViewInit() {
        this.SetUIProperties();
    }
    
    // Load Data
    private myAgentPM: AgentPM;
    private myCustomerPM: CustomerPM;
    public InitializeComponent() {
        if (this.IsNewEntity) {
            if (this.PartnerTypeCode == "AGT" || this.bookingPM.BookingLevelCode == "C") {
                this.PartnerTypeId = "AG";

                this.myAgentPM = new AgentPM();
                this.myAgentPM.Tenant = this.bookingPM.Tenant;
                this.myAgentPM.PartnerTypeId = this.PartnerTypeId;
                this.myAgentPM.Code = "new";

                this.EntityPM = new AddressPM();
                this.EntityPM.Tenant = this.bookingPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";

                this.myAgentPM.Addresses.push(this.EntityPM);
            }

            else {
                this.PartnerTypeId = "CS";

                this.myCustomerPM = new CustomerPM();
                this.myCustomerPM.Tenant = this.bookingPM.Tenant;
                this.myCustomerPM.PartnerTypeId = this.PartnerTypeId;
                this.myCustomerPM.CustomerStatusCode = "ACT";
                this.myCustomerPM.IsCustomer = this.PartnerTypeCode == "SHI" ? true : false;
                this.myCustomerPM.Code = "new";

                this.EntityPM = new AddressPM();
                this.EntityPM.Tenant = this.bookingPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";

                this.myCustomerPM.Addresses.push(this.EntityPM);
            }

            this.SetUIProperties();
        }

        else {

            switch (this.PartnerTypeCode) {
                case "SHI":
                    {
                        this.CurrentPartnerId = this.bookingPM.ShipperId;
                        this.CurrentAddressId = this.bookingPM.ShipperAddressId;
                        break;
                    }

                case "CON":
                    {
                        this.CurrentPartnerId = this.bookingPM.ConsigneeId;
                        this.CurrentAddressId = this.bookingPM.ConsigneeAddressId;
                        break;
                    }

                case "AGT":
                    {
                        this.CurrentPartnerId = this.bookingPM.IssuingCarrierAgentId;
                        this.CurrentAddressId = this.bookingPM.IssuingCarrierAddressId;
                        break;
                    }
            }
            
            if (!AppTool.IsNullOrEmpty(this.CurrentPartnerId)) {
                this.CurrentSession.StartBusyIndicatorLoading();

                var myService: CardListService = new CardListService();

                myService.getSingle(this.CurrentPartnerId).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {

                        var card: CardList = myResponse.Result;
                        if (card != null) {
                            this.PartnerTypeId = card.PartnerTypeId;
                            this.LoadPartner();
                            this.LoadAddress();
                        }

                        else {
                            this.CurrentSession.StopBusyIndicator();
                        }
                    }

                    else {
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
                    , error => {
                        this.CurrentSession.StopBusyIndicator();
                    });
            }
        }
    }

    private isPartnerLoaded: boolean = false;
    private isAddressLoaded: boolean = false;
    private LoadPartner() {
        this.isPartnerLoaded = false;

        var myService: any = null;

        switch (this.PartnerTypeId) {
            case "AG": {
                myService = new AgentPMService();
                myService.get(this.CurrentPartnerId).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;

                    if (!myResponse.HasError) {
                        this.myAgentPM = myResponse.Result;

                        this.isPartnerLoaded = true;
                        this.OnLoadCompleted();
                    }
                });
                break;
            }

            case "CS": {
                myService = new CustomerPMService();
                myService.get(this.CurrentPartnerId).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;

                    if (!myResponse.HasError) {
                        this.myCustomerPM = myResponse.Result;

                        this.isPartnerLoaded = true;
                        this.OnLoadCompleted();
                    }
                });
                break;
            }
        }
    }
    private LoadAddress() {
        this.isAddressLoaded = false;

        var myService = new AddressPMService();
        myService.get(this.CurrentAddressId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;

                this.isAddressLoaded = true;
                this.OnLoadCompleted();
            }
        });
    }
    private OnLoadCompleted() {
        if (this.isPartnerLoaded && this.isAddressLoaded) {
            this.SetUIProperties();
            this.CurrentSession.StopBusyIndicator();
        }
    }

    // SetUIProperties
    private SetUIProperties() {
        if (this.EntityPM != null) {
            this.UIProperties.SetRequired("CardEnglishName", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CardEnglishName) ? true : false);

            if (this.EntityPM.AddressTypeId == "O") {
                this.UIProperties.SetVisibility("Description", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("InActive", this.ObjectTableName, true);
            }

            else {
                this.UIProperties.SetVisibility("Description", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("InActive", this.ObjectTableName, false);
            }

            this.SetUIProperties_VAT();
            this.SetUIProperties_State();
            this.SetUIProperties_TelFax();
        }
    }
    private SetUIProperties_VAT() {
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                var args = new VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.myCustomerPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;

                VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator.ValidateVatMandatory(args);

                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
            }
        }
    }
    private SetUIProperties_State() {
        if (this.EntityPM != null) {
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
        if (this.myCustomerPM != null) {
            var isTelRequired = false;
            var isFaxRequired = false;
            
            if (this.myCustomerPM != null) {
                if (this.myCustomerPM.IsCustomer) {
                    if (this.myCustomerPM.PartnerTypeId == "CS") {
                        if (InfraSettings.TenantPM.IsCustomerTelRequired) {
                            if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                                isTelRequired = true;
                            }
                        }

                        if (InfraSettings.TenantPM.IsCustomerFaxRequired) {
                            if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                                isFaxRequired = true;
                            }
                        }
                    }

                    else if (this.myCustomerPM.PartnerTypeId == "PO") {
                        if (InfraSettings.TenantPM.IsPotentialTelRequired) {
                            if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                                isTelRequired = true;
                            }
                        }

                        if (InfraSettings.TenantPM.IsPotentialFaxRequired) {
                            if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                                isFaxRequired = true;
                            }
                        }
                    }
                }
            }

            this.UIProperties.SetRequired("PhoneNumber", this.ObjectTableName, isTelRequired);
            this.UIProperties.SetRequired("FaxNumber", this.ObjectTableName, isFaxRequired);
        }
    }

    // Properties
    get Description() { return this.EntityPM == null ? null : this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        }
    }

    get CardEnglishName() { return this.EntityPM == null ? null : this.EntityPM.CardEnglishName; }
    set CardEnglishName(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.CardEnglishName != newValue) {
                this.EntityPM.Name = newValue;
                this.EntityPM.CardEnglishName = newValue;

                this.UIProperties.SetRequired("CardEnglishName", this.ObjectTableName, AppTool.IsNullOrEmpty(newValue) ? true : false);
            }
        }
    }

    get Address1() { return this.EntityPM == null ? null : this.EntityPM.Address1; }
    set Address1(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.Address1 != newValue) {
                this.EntityPM.Address1 = newValue;
            }
        }
    }

    get Address2() { return this.EntityPM == null ? null : this.EntityPM.Address2; }
    set Address2(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.Address2 != newValue) {
                this.EntityPM.Address2 = newValue;
            }
        }
    }

    get City() { return this.EntityPM == null ? null : this.EntityPM.City; }
    set City(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.City != newValue) {
                this.EntityPM.City = newValue;
            }
        }
    }

    get ATTN() { return this.EntityPM == null ? null : this.EntityPM.ATTN; }
    set ATTN(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.ATTN != newValue) {
                this.EntityPM.ATTN = newValue;
            }
        }
    }

    get ZipCode() { return this.EntityPM == null ? null : this.EntityPM.ZipCode; }
    set ZipCode(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.ZipCode != newValue) {
                this.EntityPM.ZipCode = newValue;
            }
        }
    }

    get PhoneNumber() { return this.EntityPM == null ? null : this.EntityPM.PhoneNumber; }
    set PhoneNumber(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.PhoneNumber != newValue) {
                this.EntityPM.PhoneNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        }
    }

    get FaxNumber() { return this.EntityPM == null ? null : this.EntityPM.FaxNumber; }
    set FaxNumber(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.FaxNumber != newValue) {
                this.EntityPM.FaxNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        }
    }

    get VatNumber() { return this.EntityPM == null ? null : this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.VatNumber != newValue) {
                this.EntityPM.VatNumber = newValue;
                this.SetUIProperties_VAT();
            }
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

    get CountryId() { return this.EntityPM == null ? null : this.EntityPM.CountryId; }
    set CountryId(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
                this.StateId = null;
            }
        }
    }

    get CountryCode() { return this.EntityPM == null ? null : this.EntityPM.CountryCode; }
    set CountryCode(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.CountryCode != newValue) {
                this.EntityPM.CountryCode = newValue;
            }
        }
    }

    get CountryName() { return this.EntityPM == null ? null : this.EntityPM.CountryName; }
    set CountryName(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.CountryName != newValue) {
                this.EntityPM.CountryName = newValue;
            }
        }
    }

    get CountryEnglishName() { return this.EntityPM == null ? null : this.EntityPM.CountryEnglishName; }
    set CountryEnglishName(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.CountryEnglishName != newValue) {
                this.EntityPM.CountryEnglishName = newValue;
            }
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

    get StateId() { return this.EntityPM == null ? null : this.EntityPM.StateId; }
    set StateId(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.StateId != newValue) {
                this.EntityPM.StateId = newValue;
            }
        }
    }

    get StateCode() { return this.EntityPM == null ? null : this.EntityPM.StateCode; }
    set StateCode(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.StateCode != newValue) {
                this.EntityPM.StateCode = newValue;
            }
        }
    }

    get StateEnglishName() { return this.EntityPM == null ? null : this.EntityPM.StateEnglishName; }
    set StateEnglishName(newValue: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.StateEnglishName != newValue) {
                this.EntityPM.StateEnglishName = newValue;
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
            this.CountryName = this.EntityPM.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }

        this.SetUIProperties_VAT();
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

    private isPartnerDirty = false;
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {        
        this.CurrentSession.StartBusyIndicatorSaving();

        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }

        else {
            if (!this.EntityPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindow();
            }

            else {
                switch (this.PartnerTypeId) {
                    case "AG":
                        {
                            if (this.myAgentPM != null) {

                                if (this.myAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myAgentPM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myAgentPM.VatNumber != this.VatNumber) {
                                    this.myAgentPM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myAgentPM.IsDirty;
                            }

                            break;
                        }

                    case "CS":
                        {
                            if (this.myCustomerPM != null) {

                                if (this.myCustomerPM.EnglishName != this.CardEnglishName) {
                                    this.myCustomerPM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myCustomerPM.VatNumber != this.VatNumber) {
                                    this.myCustomerPM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myCustomerPM.IsDirty;
                            }

                            break;
                        }
                }

                this.Save();
            }
        }
    }

    private Validate() {
        var isValid = true;
        var errors: string[] = [];

        if (this.EntityPM != null) {
            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            var isLanguageValid = AddressValidator.IsMainAddressEnglishCharacters(this.EntityPM);
            if (!isLanguageValid) {
                errors.push("Main address does not allow non-english characters");
            }

            if (this.Country != null) {
                if (this.State == null) {
                    if (this.Country.IsStateRequired) {
                        errors.push(msg.replace("%FieldName", "State"));
                    }
                }
            }

            this.ValidateCustomerFields(errors);

            if (errors.length == 0) {
                var myCity: string = this.City;
                var myName: string = this.CardEnglishName;

                if (!AppTool.IsNullOrEmpty(myCity)) {
                    myCity = myCity.trim();
                }

                if (!AppTool.IsNullOrEmpty(myName)) {
                    myName = myName.trim();
                }

                if (AppTool.IsNullOrEmpty(myCity)) {
                    errors.push(msg.replace("%FieldName", "City"));
                }

                if (AppTool.IsNullOrEmpty(myName)) {
                    errors.push(msg.replace("%FieldName", "Name"));
                }
            }
        }

        if (errors.length > 0) {
            isValid = false;
        }

        this.ValidationErrorsList = errors;
        return isValid;
    }
    private ValidateCustomerFields(errors: string[]) {
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                var args = new VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.myCustomerPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;

                VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator.ValidateVatMandatory(args);

                args.Errors.forEach(item => {
                    errors.push(item);
                });

                if (this.PartnerTypeId == "CS") {
                    if (InfraSettings.TenantPM.IsCustomerTelRequired) {
                        if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }

                    if (InfraSettings.TenantPM.IsCustomerFaxRequired) {
                        if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }

                else if (this.PartnerTypeId == "PO") {
                    if (InfraSettings.TenantPM.IsPotentialTelRequired) {
                        if (AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }

                    if (InfraSettings.TenantPM.IsPotentialFaxRequired) {
                        if (AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
            }
        }
    }

    private myPartnersDomainService: PartnersDomainService;
    private Save() {
        if (this.myPartnersDomainService == null) {
            this.myPartnersDomainService = new PartnersDomainService();
        }

        var args = new PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.AddressId = this.CurrentAddressId;
        args.PartnerId = this.CurrentPartnerId;
        args.PartnerTypeId = this.PartnerTypeId;
        args.IsAddressDirty = this.EntityPM.IsDirty;
        args.IsPartnerDirty = this.isPartnerDirty;
        args.Address = this.EntityPM;
        args.Agent = this.myAgentPM;
        args.Customer = this.myCustomerPM;

        this.myPartnersDomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentAddressId = myResponse.Result.AddressId;
                this.CurrentPartnerId = myResponse.Result.PartnerId;               
                this.FatherComponent.UpdatePartner(this.PartnerTypeCode, this.CurrentPartnerId, this.CurrentAddressId);
                this.CurrentSession.CloseCurrentWindow();
            }

            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        }
            , error => {
                this.ValidationErrorsList.push(error);
                this.CurrentSession.CloseCurrentWindow();
            });
    }
}
