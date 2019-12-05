import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {CustomerSalesNotePM} from '../../../../Common/EntityPMs/CustomerSalesNotePM';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {VatNumberValidator, VATValidatorArgs} from '../../../../Infrastructure/Validators/VatNumberValidator';
import {CitySelectionArgs} from '../../../../Common/Args';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './NewPotentialCustomerComponent.html',
})

export class NewPotentialCustomerComponent extends BaseComponent {
    public EntityPM: CustomerPM;
    public CardId: string = null;
    public PartnerTypeId: string = "PO";
    public ObjectTableName: string = "Customer";
    public Contact: ContactPM;
    public ValidationErrorsList: string[] = [];
    public DomainService: PartnersDomainService;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private args: NewEntityArgs;
    public DataContext: NewPotentialCustomerComponent = this;  
    public ContactDataContext: ContactItem; 
    public IsResourcesReady: boolean = false;
    public IsRadioButtonsVisible: boolean = false;
    public ShowContactPart: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new CustomerPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.PartnerTypeId = this.PartnerTypeId;
        this.EntityPM.CustomerStatusCode = "POT";
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.IsCustomer = true;
        this.EntityPM.Code = "new";

        this.Contact = new ContactPM();
        this.Contact.Tenant = SessionLocator.Tenant;
        this.Contact.CardId = "newCard";
        this.Contact.IsCreatedWithPartner = true;
        this.Contact.SetAsPrimaryForCard = true;

        this.DomainService = new PartnersDomainService();
        this.ContactDataContext = new ContactItem(this.Contact, this.EntityPM, this);

        if (FeatureLocator.HasFeaturePermession("General", "SHIPPERSANDCONSIGNEES")) {
            this.IsRadioButtonsVisible = true;
        }

        this.entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Contact").subscribe(response2 => {
                this.entityResourceService.getEntityResourceByTableName("Customer").subscribe(response3 => {
                    this.IsResourcesReady = true;
                    if (this.ShowContactPart) {
                        this.IsAddContactChecked = false;
                    }
                    else {
                        this.IsAddContactChecked = true;
                    }
                    this.SetUIProperties();
                    this.RunComponent();
                });
            });
        });
    }
    
    SetWindowArgs(args: NewEntityArgs) {
        if (args != null) {
            this.args = args;
            this.ShowContactPart = args.ShowContactPart;
            if (this.ShowContactPart) {
                this.IsAddContactChecked = false;
            }
            else {
                this.IsAddContactChecked = true;
            }
            if (args.Perspective == "ShippersAndConsignees") {
                this.IsCustomer = false;
            }
        }
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, "Customer.AdditionalFields");
            });
    }

    private SetUIProperties() {
        var isCountryRequired: boolean = false;
        if (AppTool.IsNullOrEmpty(this.CountryId_Potential)) {
            isCountryRequired = true;
        }

        this.UIProperties.SetRequired("CountryId_Potential", this.ObjectTableName, isCountryRequired);

        this.SetUIProperties_LocalName();
        this.SetUIProperties_PhonFax();
        this.SetUIProperties_State();
        this.SetUIProperties_VAT();       
    }
    private SetUIProperties_LocalName() {
        var isLocalNameRequired = false;

        if (this.IsLocalLanguage) {
            if (AppTool.IsNullOrEmpty(this.LocalName)) {
                isLocalNameRequired = true;
            }            
        }
        
        this.UIProperties.SetRequired("LocalName", this.ObjectTableName, isLocalNameRequired);
    }
    private SetUIProperties_PhonFax() {
        var isTelRequired = false;
        var isFaxRequired = false;

        if (this.IsCustomer) {
            if (SessionLocator.TenantPM.IsPotentialTelRequired) {
                if (AppTool.IsNullOrEmpty(this.PhoneNumber_Potential)) {
                    isTelRequired = true;
                }
            }

            if (SessionLocator.TenantPM.IsPotentialFaxRequired) {
                if (AppTool.IsNullOrEmpty(this.FaxNumber_Potential)) {
                    isFaxRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("PhoneNumber_Potential", this.ObjectTableName, isTelRequired);
        this.UIProperties.SetRequired("FaxNumber_Potential", this.ObjectTableName, isFaxRequired);
    }
    private SetUIProperties_VAT() {
        if (this.IsCustomer) {
            var args = new VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.IsCustomer;
            args.PartnerTypeId = this.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.EntityPM.CountryName;
            args.SetReady = false;

            VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator.ValidateVatMandatory(args);

            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
        }

        else {
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_State() {
        //var isEnabled = false;

        //if (this.Country != null) {
        //    if (this.Country.HasStates) {
        //        isEnabled = true;
        //    }
        //}

        //this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, isEnabled);

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

        this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, isEnabled);
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

        this.UIProperties.SetRequired("StateId_Potential", this.ObjectTableName, isRequired);
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }  

    SetIsCustomer(isCustomer: boolean) {
        this.IsCustomer = isCustomer;
    }
    get IsCustomer() { return this.EntityPM.IsCustomer; }
    set IsCustomer(newValue: boolean) {
        if (this.EntityPM.IsCustomer != newValue) {
            this.EntityPM.IsCustomer = newValue;
            this.SetUIProperties_VAT();
            this.SetUIProperties_PhonFax();
        }
    }

    get IsLocalLanguage() { return this.EntityPM.IsLocalLanguage; }
    set IsLocalLanguage(newValue) {
        if (this.EntityPM.IsLocalLanguage != newValue) {
            this.EntityPM.IsLocalLanguage = newValue;

            this.EnableLocalLanguage();
        }
    }

    EnableLocalLanguage() {
        if (!this.IsLocalLanguage) {
            var pattern = /^a-zA-Z0-9\s/;

            if (!AppTool.IsNullOrEmpty(this.EnglishName)) {
                this.EnglishName = this.EnglishName.replace(pattern, "");
            }

            if (!AppTool.IsNullOrEmpty(this.Address1_Potential)) {
                this.Address1_Potential = this.Address1_Potential.replace(pattern, "");
            }

            if (!AppTool.IsNullOrEmpty(this.Address2_Potential)) {
                this.Address2_Potential = this.Address2_Potential.replace(pattern, "");
            }

            if (!AppTool.IsNullOrEmpty(this.City_Potential)) {
                this.City_Potential = this.City_Potential.replace(pattern, "");
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.ATTN_Potential)) {
                this.EntityPM.ATTN_Potential = this.EntityPM.ATTN_Potential.replace(pattern, "");
            }           
        }

        this.SetUIProperties_LocalName();
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
            this.SearchText = newValue;
        }
    }

    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM.VatNumber != newValue) {
            this.EntityPM.VatNumber = newValue;

            this.SetUIProperties_VAT();
        }
    }

    get SalesmanUserId() { return this.EntityPM.SalesmanUserId; }
    set SalesmanUserId(newValue: string) {
        if (this.EntityPM.SalesmanUserId != newValue) {
            this.EntityPM.SalesmanUserId = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;

            this.SetUIProperties_LocalName();
        }
    }

    get Address1_Potential() { return this.EntityPM.Address1_Potential; }
    set Address1_Potential(newValue: string) {
        if (this.EntityPM.Address1_Potential != newValue) {
            this.EntityPM.Address1_Potential = newValue;
        }
    }

    get Address2_Potential() { return this.EntityPM.Address2_Potential; }
    set Address2_Potential(newValue: string) {
        if (this.EntityPM.Address2_Potential != newValue) {
            this.EntityPM.Address2_Potential = newValue;
        }
    }

    get ZipCode_Potential() { return this.EntityPM.ZipCode_Potential; }
    set ZipCode_Potential(newValue: string) {
        if (this.EntityPM.ZipCode_Potential != newValue) {
            this.EntityPM.ZipCode_Potential = newValue;
        }
    }

    get City_Potential() { return this.EntityPM.City_Potential; }
    set City_Potential(newValue: string) {
        if (this.EntityPM.City_Potential != newValue) {
            this.EntityPM.City_Potential = newValue;
        }
    }

    get CountryId_Potential() { return this.EntityPM.CountryId_Potential; }
    set CountryId_Potential(newValue: string) {
        if (this.EntityPM.CountryId_Potential != newValue) {
            this.EntityPM.CountryId_Potential = newValue;
            this.StateId_Potential = null;
            this.SetUIProperties();
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

    private OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.EntityPM.CountryCode = null;
            this.EntityPM.CountryName = null;
        }

        else {
            this.EntityPM.CountryCode = list.Code;
            this.EntityPM.CountryName = list.EnglishName;
        }

        this.SetUIProperties();
    }

    get StateId_Potential() { return this.EntityPM.StateId_Potential; }
    set StateId_Potential(newValue: string) {
        if (this.EntityPM.StateId_Potential != newValue) {
            this.EntityPM.StateId_Potential = newValue;
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

    private OnStateChanged(list: StateList) {
        if (list == null) {
            //this.StateCode = null;
            //this.StateEnglishName = null;
        }

        else {
            //this.StateCode = list.Code;
            //this.StateEnglishName = list.EnglishName;
        }

        this.SetUIProperties_StateRequired();
    }

    get PhoneNumber_Potential() { return this.EntityPM.PhoneNumber_Potential; }
    set PhoneNumber_Potential(newValue: string) {
        if (this.EntityPM.PhoneNumber_Potential != newValue) {
            this.EntityPM.PhoneNumber_Potential = newValue;

            this.SetUIProperties_PhonFax();
        }
    }

    get FaxNumber_Potential() { return this.EntityPM.FaxNumber_Potential; }
    set FaxNumber_Potential(newValue: string) {
        if (this.EntityPM.FaxNumber_Potential != newValue) {
            this.EntityPM.FaxNumber_Potential = newValue;

            this.SetUIProperties_PhonFax();
        }
    }

    SelectCityCommand() {
        var args = new CitySelectionArgs(this.CountryId_Potential);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");

        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                var mySelectedCity: string = args.CityName;
                if (this.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }

                this.City_Potential = mySelectedCity;
                this.CountryId_Potential = args.CountryId;
                this.StateId_Potential = args.StateId;
            }
        });
    }

    //Contact
    private isAddContactChecked: boolean = false;
    get IsAddContactChecked() { return this.isAddContactChecked; }
    set IsAddContactChecked(value: boolean) {
        if (this.isAddContactChecked != value) {
            this.isAddContactChecked = value;

            if (value) {
                if (this.EntityPM.Contacts.length == 0) {
                    this.EntityPM.Contacts.push(this.Contact);
                }
            }

            else {
                this.EntityPM.Contacts = [];
                this.ContactDataContext.Email = null;
                this.ContactDataContext.EnglishName = null;
                this.ContactDataContext.LocalName = null;
                this.ContactDataContext.BusinessPhone = null;
                this.ContactDataContext.Mobile = null;
                this.ContactDataContext.Fax = null;
                this.ContactDataContext.Position = null;
                this.ContactDataContext.LocalName = null;
                this.ContactDataContext.Info2Text = null;                
            }

            this.ContactDataContext.CloseContactFields(!value);
        }
    }

    private entityNotes: string;
    get EntityNotes() { return this.entityNotes; }
    set EntityNotes(newValue: string) {
        if (this.entityNotes != newValue) {
            this.entityNotes = newValue;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.IsLocalLanguage) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.LocalName)) {
                errors.push("Local Name is Required");
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityNotes)) {
            if (this.EntityNotes.length > 250) {
                errors.push("Sales Notes must be less than 250 char");
            }
        }

        var isLanguageValid = this.ValidateLocalLanguage(this.EntityPM);
        if (!isLanguageValid) {
            errors.push("Main address does not allow non-english characters");
        }

        if (AppTool.IsNullOrEmpty(this.CountryId_Potential)) {
            errors.push("Country is required");
        }

        if (this.IsCustomer) {
            if (SessionLocator.TenantPM.IsPotentialTelRequired) {
                if (AppTool.IsNullOrEmpty(this.PhoneNumber_Potential)) {
                    errors.push("Phone Number is required");
                }
            }

            if (SessionLocator.TenantPM.IsPotentialFaxRequired) {
                if (AppTool.IsNullOrEmpty(this.FaxNumber_Potential)) {
                    errors.push("Fax Number is required");
                }
            }

            var args = new VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.IsCustomer;
            args.PartnerTypeId = this.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.EntityPM.CountryName;
            args.SetReady = false;

            VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator.ValidateVatMandatory(args);

            args.Errors.forEach(item => {
                errors.push(item);
            });
        }

        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    errors.push("State is required");
                }
            }
        }
        
        if (this.IsAddContactChecked) {
            var index = this.EntityPM.Contacts.indexOf(this.Contact);
            if (index > -1) {
                if (!this.ShowContactPart) {
                    Validator.TryValidateObject(this.Contact, this.ContactDataContext.ObjectTableName, errors);
                }

                if (this.ContactDataContext.ContactAlreadyExist) {
                    errors.push("This Contact is Already added for you");
                }
            }

            if (!FormatTool.IsEmail(this.ContactDataContext.Contact.Email)) {
                errors.push("Invalid email format!");
            }
        }

        else {
            this.EntityPM.Contacts = [];
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            if (!AppTool.IsNullOrEmpty(this.EntityNotes)) {
                var note: CustomerSalesNotePM = new CustomerSalesNotePM(this.EntityPM);
                note.Tenant = SessionLocator.Tenant;
                note.CustomerId = this.EntityPM.Id;
                note.Notes = this.EntityNotes;

                this.EntityPM.AddCustomerSalesNotePM(note);
            }

            var partnerArgs = new PartnerServicePM();
            partnerArgs.Tenant = this.EntityPM.Tenant;
            partnerArgs.PartnerTypeId = this.PartnerTypeId;
            partnerArgs.Customer = this.EntityPM;
            //partnerArgs.Address = this.PartnerTamplate.Address;
            if (this.IsAddContactChecked) {
                this.ContactDataContext.Contact.SetAsPrimaryForCard = true;
                partnerArgs.Contact = this.ContactDataContext.Contact;
            }

            this.DomainService.PostPartnerAddress(partnerArgs).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result.Customer;
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }

    private ValidateLocalLanguage(entityPM: CustomerPM ) {
        var isValid = true;

        if (!entityPM.IsLocalLanguage) {            
            if (!AppTool.IsNullOrEmpty(entityPM.EnglishName) && !FormatTool.IsEnglishText(entityPM.EnglishName)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Address1_Potential) && !FormatTool.IsEnglishText(entityPM.Address1_Potential)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Address2_Potential) && !FormatTool.IsEnglishText(entityPM.Address2_Potential)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.City_Potential) && !FormatTool.IsEnglishText(entityPM.City_Potential)) {
                isValid = false;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.ATTN_Potential) && !FormatTool.IsEnglishText(entityPM.ATTN_Potential)) {
                isValid = false;
            }
        }

        return isValid;
    }
}

export class ContactItem extends BaseComponent {
    public ObjectTableName: string = "Contact";
    public Contact: ContactPM;
    public Customer: CustomerPM;
    public IsChecked: boolean = false;
    constructor(contact: ContactPM, customer: CustomerPM, public father: NewPotentialCustomerComponent) {
        super();

        this.Contact = contact;
        this.Customer = customer;

        this.CloseContactFields(true);
        this.SetUIProperties();
    }

    public SetUIProperties() {
        var isRequired = false;

        if (this.IsChecked) {
            if (AppTool.IsNullOrEmpty(this.EnglishName)) {
                isRequired = true;
            }
        }

        if (this.father.ShowContactPart) {
            this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, isRequired);
        }
    }

    public CloseContactFields(close: boolean) {
        this.IsChecked = !close;
        this.SetUIProperties();
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("Position", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("BusinessPhone", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("Mobile", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("Fax", this.ObjectTableName, this.IsChecked);
    }

    public Info1Text: string = null;
    public Info2Text: string = null;

    get Email() { return this.Contact.Email; }
    set Email(newValue: string) {
        if (this.Contact.Email != newValue) {
            this.Contact.Email = newValue;
        }
    }

    get EnglishName() { return this.Contact.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.Contact.EnglishName != newValue) {
            this.Contact.EnglishName = newValue;
            this.LocalName = newValue;
            this.SetUIProperties();
        }
    }

    get LocalName() { return this.Contact.LocalName; }
    set LocalName(newValue: string) {
        if (this.Contact.LocalName != newValue) {
            this.Contact.LocalName = newValue;
        }
    }

    get Position() { return this.Contact.Position; }
    set Position(newValue: string) {
        if (this.Contact.Position != newValue) {
            this.Contact.Position = newValue;
        }
    }

    get Mobile() { return this.Contact.Mobile; }
    set Mobile(newValue: string) {
        if (this.Contact.Mobile != newValue) {
            this.Contact.Mobile = newValue;
        }
    }

    get Fax() { return this.Contact.Fax; }
    set Fax(newValue: string) {
        if (this.Contact.Fax != newValue) {
            this.Contact.Fax = newValue;
        }
    }

    get BusinessPhone() { return this.Contact.BusinessPhone; }
    set BusinessPhone(newValue: string) {
        if (this.Contact.BusinessPhone != newValue) {
            this.Contact.BusinessPhone = newValue;
        }
    }

    get Notes() { return this.Contact.Notes; }
    set Notes(newValue: string) {
        if (this.Contact.Notes != newValue) {
            this.Contact.Notes = newValue;
        }
    }

    private loadedContact: ContactPM = null;
    public ContactAlreadyExist: boolean = false;
    private allCardContacts: any[] = [];
    EmailLostFocus(email: string) {
        var isLoading = false;

        if (!AppTool.IsNullOrEmpty(email)) {
            if (email.indexOf('@') > -1 && email.indexOf('.') > -1) {
                isLoading = true;
            }
        }

        if (!isLoading) {
            this.Info1Text = null;
            this.Info2Text = null;
            this.loadedContact = null;
            this.allCardContacts = [];
            this.Customer.ExistedContactId = null;
            this.CloseContactFields(false);
            this.ValidateContactExist();
        }

        else {
            var domainService: PartnersDomainService = new PartnersDomainService();

            domainService.GetContactsByEmail(email).subscribe(myResult => {
                if (myResult != null) {
                    this.loadedContact = myResult[0];

                    if (this.loadedContact == null) {
                        this.CloseContactFields(false);
                        this.ContactAlreadyExist = false;

                        var index = this.Customer.Contacts.indexOf(this.Contact);
                        if (index == -1) {
                            this.Customer.ExistedContactId = null;
                        }
                    }

                    else {
                        this.Customer.ExistedContactId = this.loadedContact.Id;
                        this.Email = this.loadedContact.Email;
                        this.EnglishName = this.loadedContact.EnglishName;
                        this.LocalName = this.loadedContact.LocalName;
                        this.BusinessPhone = this.loadedContact.BusinessPhone;
                        this.Mobile = this.loadedContact.Mobile;
                        this.Fax = this.loadedContact.Fax;
                        this.Contact.Anniversary = this.loadedContact.Anniversary;
                        this.Contact.Birthday = this.loadedContact.Birthday;
                        this.Contact.InActive = this.loadedContact.InActive;
                        this.Position = this.loadedContact.Position;
                        this.CloseContactFields(true);

                        domainService.GetCardContactsByContact(this.loadedContact.Id).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.allCardContacts = myResponse.Result;
                                this.ValidateContactExist();

                                var otherCardContacts = this.allCardContacts.filter(d => d.ContactId == this.loadedContact.Id && d.CardId != this.Customer.Id);

                                if (otherCardContacts.length == 0) {
                                    this.Info2Text = null;
                                }

                                else {
                                    this.Info2Text = "This Contact is Already Added for " + otherCardContacts.length + " other Partners!";
                                }
                            }
                        });
                    }
                }
            });
        }

        this.SetUIProperties();
    }

    private ValidateContactExist() {
        this.ContactAlreadyExist = false;

        if (this.Customer.Id != null) {
            if (this.loadedContact != null) {
                if (this.allCardContacts != null) {
                    if (this.allCardContacts.filter(d => d.ContactId == this.loadedContact.Id && d.CardId == this.Customer.Id).length > 0) {
                        this.ContactAlreadyExist = true;
                    }
                }
            }
        }

        this.Info1Text = this.ContactAlreadyExist ? TextCodeTranslator.Translate("Contact.M.ContactAddedForYou") : null;
        return this.ContactAlreadyExist;
    }
}
