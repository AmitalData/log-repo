import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CitySelectionArgs} from '../../../../Common/Args';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {WarehousePM} from '../../../../Common/EntityPMs/WarehousePM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AddressValidator} from '../../../../Infrastructure/Validators/AddressValidator';
import {VatNumberValidator, VATValidatorArgs} from '../../../../Infrastructure/Validators/VatNumberValidator';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    
    templateUrl: './NewPartnerTamplate.html',    
})

export class NewPartnerTamplate extends BaseComponent implements OnInit {
    public CardId: string = null;
    public EntityPM: any = null;
    public CardTableName: string = null;
    public Address: AddressPM;
    public Contact: ContactPM;
    public Warehouse: WarehousePM;
    public ObjectTableName: string = "Address";    
    public DataContext: NewPartnerTamplate = this;
    public PartnerTypeId: string = null;
    public IsCustomer: boolean = false;
    public IsCustomerPartner: boolean = false;
    public IsCardCodeVisible: boolean = false;
    public IsAdditionalFieldsVisible: boolean = false;
    public SimilaryCardsHeader: string = "";
    public DefaultValues: string;
    public IsShowCountry: boolean;
    public IsWarehouseTypeCodeVisible: boolean = false;
    public IsWarehouseFirmCodeVisible: boolean = false;
    public DomainService: PartnersDomainService;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor() {
        super();

        this.Address = new AddressPM();
        this.Address.Tenant = SessionLocator.Tenant;
        this.Address.AddressTypeId = "M";
        this.Address.Description = "Main Address";
        this.Address.IsCreatedWithPartner = true;

        this.Contact = new ContactPM();
        this.Contact.CardId = "newCard";
        this.Contact.Tenant = SessionLocator.Tenant;
        this.Contact.IsCreatedWithPartner = true;
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    public InitTemplate() {
        if (this.PartnerTypeId == "CS" || this.PartnerTypeId == "PO") {
            this.IsCustomerPartner = true;
            this.IsAdditionalFieldsVisible = true;
        }

        switch (this.PartnerTypeId) {
            case "TR": {
                this.CardCode = null;
                this.IsCardCodeVisible = true;
                break;
            }
            case "WH": {
                this.Warehouse = this.EntityPM;
                this.CardCode = null;
                this.IsCardCodeVisible = true;
                this.IsWarehouseTypeCodeVisible = true;
                if (SessionLocator.TenantPM.CountryCode.toUpperCase() == "US"){
                    this.IsWarehouseFirmCodeVisible = true;
                }
                break;
            }

            default: {
                this.CardCode = "new";
                this.IsCardCodeVisible = false;
                break;
            }
        }
        

        //DefaultValues Abed Code

        if (!AppTool.IsNullOrEmpty(this.DefaultValues)) {

            var DefaultValueData: string[] = this.DefaultValues.split("^");

            if (DefaultValueData[0] == "Trucker") {
                //Trucker
                this.CardCode = !AppTool.IsNullOrEmpty(DefaultValueData[1]) ? DefaultValueData[1] : "";
                this.Name = !AppTool.IsNullOrEmpty(DefaultValueData[2]) ? DefaultValueData[2] : "";

            }
            else {
               //Partners
                this.Name = !AppTool.IsNullOrEmpty(DefaultValueData[0]) ? DefaultValueData[0] : "";
                this.Address1 = !AppTool.IsNullOrEmpty(DefaultValueData[1]) ? DefaultValueData[1] : "";
                this.Address2 = !AppTool.IsNullOrEmpty(DefaultValueData[2]) ? DefaultValueData[2] : "";
                this.City = !AppTool.IsNullOrEmpty(DefaultValueData[3]) ? DefaultValueData[3] : "";
                this.CountryId = !AppTool.IsNullOrEmpty(DefaultValueData[4]) ? DefaultValueData[4] : ""; 
            }
   

        }

        this.SetUIProperties();
        this.RunComponent();


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
        this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(response2 => {
                this.BuildAdditionalFields();
                this.SimilaryCardsHeader = "Similar " + this.CardTableName + " in the system";
            });
        });
    }

    private BuildAdditionalFields() {
        if (this.IsAdditionalFieldsVisible) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
                .then(cmpRef => {
                    var screenCode = this.CardTableName + ".AdditionalFields";
                    cmpRef.instance.LabelWidth = 110;
                    cmpRef.instance.Run(this.EntityPM, this.CardTableName, screenCode);
                });
        }
    }

    public IsSelectCityEnabled: boolean = true;
    public IsAddContactEnabled: boolean = true;
    public IsAdditionalEnabled: boolean = true;
    SetUIProperties() {
        if (this.PartnerTypeId == "CS") {
            if (!FeatureLocator.HasFeaturePermession("Customer", "CREATEACTIVECUSTOMER")) {

                var isFieldsEnabled = true;
                if (this.IsCustomer) {
                    isFieldsEnabled = false
                }

                this.UIProperties.SetEnabled("Name", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("Address1", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("Address2", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ZipCode", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("City", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("PhoneNumber", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("FaxNumber", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactEmail", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactName", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactPosition", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactBusinessPhone", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactMobile", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactFax", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("FirmCode", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, isFieldsEnabled);

                this.IsSelectCityEnabled = isFieldsEnabled;
                this.IsAddContactEnabled = isFieldsEnabled;
                this.IsAdditionalEnabled = isFieldsEnabled;
            }
        }
        

        //this.UIProperties.SetVisibility("VatNumber", this.ObjectTableName, this.IsCustomerPartner);
        this.UIProperties.SetVisibility("SalesmanUserId", this.ObjectTableName, this.IsCustomerPartner);

        this.SetUIProperties_Code();
        this.SetUIProperties_VAT();
        this.SetUIProperties_TelFax();
        this.SetUIProperties_State();
        this.SetUIProperties_Contact();
        this.SetUIProperties_City();
    }

    private SetUIProperties_Code() {
        if (this.PartnerTypeId == "TR" || this.PartnerTypeId == "WH") {

            var isRequired = false;                        
            if (AppTool.IsNullOrEmpty(this.CardCode)) {
                isRequired = true;
            }

            this.UIProperties.SetRequired("CardCode", this.ObjectTableName, isRequired);
                     
            if (!isRequired) {
                if (this.CardCode != null) {
                    if (this.CardTableName == "Trucker") {
                        if (this.CardCode.length >= 7) {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, false, "Code field must be less than 7 and more than 0");
                        }
                        else {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, true, "");
                        }
                    }
                    else {
                        if (this.CardCode.length >= 5) {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, false, "Code field must be less than 5 and more than 0");
                        }

                        else {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, true, "");
                        }
                    }
                }
            }
        }
    }
    private SetUIProperties_VAT() {
        if (this.IsCustomerPartner) {
            if (this.IsCustomer) {
                var args = new VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.EntityPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;

                VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator.ValidateVatMandatory(args);

                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
            }

            else {
                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, false);
            }
        }
    }

    private SetUIProperties_TelFax() {
        if (this.IsCustomerPartner) {
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
    private SetUIProperties_Contact() {

        var isFieldEnabled = false;
        if (this.IsAddContactChecked) {
            if (this.loadedContact == null) {
                isFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("ContactEmail", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactName", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactPosition", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactBusinessPhone", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactMobile", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactFax", this.ObjectTableName, isFieldEnabled);

        var isRequired = false;
        if (this.IsAddContactChecked) {
            if (this.ContactName == null) {
                isRequired = true;
            }
        }

        this.UIProperties.SetRequired("ContactName", this.ObjectTableName, isRequired);
    }

    SetUIProperties_City() {
        var isRequired = false;

        if (AppTool.IsNullOrEmpty(this.City) && this.PartnerTypeId != "PO") {
            isRequired = true;
        }
        this.UIProperties.SetRequired("City", this.ObjectTableName, isRequired);
    }

    // Address
    get CardCode() { return this.Address.CardCode; }
    set CardCode(newValue: string) {
        if (this.Address.CardCode != newValue) {
            this.Address.CardCode = newValue;
            this.SetUIProperties_Code();
        }
    }

    public CodeMessage: string = null;
    public IsCodeAlreadyExists: boolean = false;
    CodeLostFocus(code: string) {
        this.CodeMessage = null;
        this.IsCodeAlreadyExists = false;

        if (!AppTool.IsNullOrEmpty(code)) {
            if (code.length <= 4) {

                if (this.PartnerTypeId == "TR") {
                    this.DomainService.GetTruckerByCode(code, SessionLocator.Tenant).subscribe((myResult:any) => {
                        if (myResult != null) {
                            this.CodeMessage = "This trucker already exists";
                            this.IsCodeAlreadyExists = true;
                        }

                        else {
                            this.DomainService.GetTruckerByCode(code, 0).subscribe((myResult:any) => {
                                if (myResult != null) {
                                    this.Name = myResult.EnglishName;
                                    this.CodeMessage = "This trucker already exists in our database and on save it will be copied to your truckers list";
                                }
                            });
                        }
                    });
                }

                else if (this.PartnerTypeId == "WH") {
                    this.DomainService.GetWarehouseByCode(code, SessionLocator.Tenant).subscribe((myResult:any) => {
                        if (myResult != null) {
                            this.CodeMessage = "This Warehouse already exists";
                            this.IsCodeAlreadyExists = true;
                        }

                        else {
                            this.DomainService.GetWarehouseByCode(code, 0).subscribe((myResult:any) => {
                                if (myResult != null) {
                                    this.Name = myResult.EnglishName;
                                    this.CodeMessage = "This Warehouse already exists in our database and on save it will be copied to your Warehouses list";
                                }
                            });
                        }
                    });
                }
            }
        }
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    get Name() { return this.Address.Name; }
    set Name(value: string) {
        if (this.Address.Name != value) {
            this.Address.Name = value;
            this.LocalName = value;

            if (!this.DefaultValues) {
                this.SearchText = value;
            }

            else {
                this.DefaultValues = "";
            }
        }
    }

    private localName: string;
    get LocalName() { return this.localName; }
    set LocalName(newValue: string) {
        if (this.localName != newValue) {
            this.localName = newValue;           
        }
    }

    get Description() { return this.Address.Description; }
    set Description(newValue: string) {
        if (this.Address.Description != newValue) {
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

    get ZipCode() { return this.Address.ZipCode; }
    set ZipCode(newValue: string) {
        if (this.Address.ZipCode != newValue) {
            this.Address.ZipCode = newValue;
        }
    }

    get PhoneNumber() { return this.Address.PhoneNumber; }
    set PhoneNumber(newValue: string) {
        if (this.Address.PhoneNumber != newValue) {
            this.Address.PhoneNumber = newValue;
            this.SetUIProperties_TelFax();
        }
    }

    get FaxNumber() { return this.Address.FaxNumber; }
    set FaxNumber(newValue: string) {
        if (this.Address.FaxNumber != newValue) {
            this.Address.FaxNumber = newValue;
            this.SetUIProperties_TelFax();
        }
    }

    get FirmCode() { return this.Warehouse.FirmCode; }
    set FirmCode(newValue: string) {
        if (this.Warehouse.FirmCode != newValue) {
            this.Warehouse.FirmCode = newValue;
        }
    }

    get TypeCode() { return this.Warehouse.TypeCode; }
    set TypeCode(newValue: string) {
        if (this.Warehouse.TypeCode != newValue) {
            this.Warehouse.TypeCode = newValue;
        }
    }

    get ATTN() { return this.Address.ATTN; }
    set ATTN(newValue: string) {
        if (this.Address.ATTN != newValue) {
            this.Address.ATTN = newValue;
        }
    }

    get City() { return this.Address.City; }
    set City(newValue: string) {
        if (this.Address.City != newValue) {
            this.Address.City = newValue;
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

    get CountryId() { return this.Address.CountryId; }
    set CountryId(newValue: string) {
        if (this.Address.CountryId != newValue) {
            this.Address.CountryId = newValue;
            this.StateId = null;
        }
    }

    get CountryCode() { return this.Address.CountryCode; }
    set CountryCode(newValue: string) {
        if (this.Address.CountryCode != newValue) {
            this.Address.CountryCode = newValue;
        }
    }

    get CountryName() { return this.Address.CountryName; }
    set CountryName(newValue: string) {
        if (this.Address.CountryName != newValue) {
            this.Address.CountryName = newValue;
        }
    }

    get CountryEnglishName() { return this.Address.CountryEnglishName; }
    set CountryEnglishName(newValue: string) {
        if (this.Address.CountryEnglishName != newValue) {
            this.Address.CountryEnglishName = newValue;
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

    get StateId() { return this.Address.StateId; }
    set StateId(newValue: string) {
        if (this.Address.StateId != newValue) {
            this.Address.StateId = newValue;
        }
    }

    get StateCode() { return this.Address.StateCode; }
    set StateCode(newValue: string) {
        if (this.Address.StateCode != newValue) {
            this.Address.StateCode = newValue;
        }
    }

    get StateEnglishName() { return this.Address.StateEnglishName; }
    set StateEnglishName(newValue: string) {
        if (this.Address.StateEnglishName != newValue) {
            this.Address.StateEnglishName = newValue;
        }
    }

    get VatNumber() { return this.Address.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.Address.VatNumber != newValue) {
            this.Address.VatNumber = newValue;
            this.SetUIProperties_VAT();
        }
    }

    get SalesmanUserId() { return this.Address.SalesmanUserId; }
    set SalesmanUserId(newValue: string) {
        if (this.Address.SalesmanUserId != newValue) {
            this.Address.SalesmanUserId = newValue;
        }
    }

    get IsLocalLanguage() { return this.Address.IsLocalLanguage; }
    set IsLocalLanguage(newValue: boolean) {
        if (this.Address.IsLocalLanguage != newValue) {
            this.Address.IsLocalLanguage = newValue;
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

    // Select City
    SelectCityCommand() {
        var args = new CitySelectionArgs(this.CountryId);
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

                this.City = mySelectedCity;
                this.CountryId = args.CountryId;
                this.StateId = args.StateId;
            }
        });
    }

    // Contact
    private isAddContactChecked: boolean = false;
    get IsAddContactChecked() { return this.isAddContactChecked; }
    set IsAddContactChecked(newValue: boolean) {
        if (this.isAddContactChecked != newValue) {
            this.isAddContactChecked = newValue;

            if (!newValue) {
                this.ContactEmail = null;
                this.ContactName = null;
                this.ContactPosition = null;
                this.ContactBusinessPhone = null;
                this.ContactMobile = null;
                this.ContactFax = null;

                this.Info1Text = null;
                this.Info2Text = null;
                this.loadedContact = null;
                this.allCardContacts = [];
                this.ExistedContactId = null;
                this.ValidateContactExist();
            }

            this.SetUIProperties_Contact();
        }
    }

    get ContactEmail() { return this.Address.ContactEmail; }
    set ContactEmail(newValue: string) {
        if (this.Address.ContactEmail != newValue) {
            this.Address.ContactEmail = newValue;
        }
    }

    get ContactName() { return this.Address.ContactName; }
    set ContactName(newValue: string) {
        if (this.Address.ContactName != newValue) {
            this.Address.ContactName = newValue;

            var isRequired = false;
            if (this.IsAddContactChecked) {
                if (this.ContactName == null) {
                    isRequired = true;
                }
            }

            this.UIProperties.SetRequired("ContactName", this.ObjectTableName, isRequired);
        }
    }

    get ContactPosition() { return this.Address.ContactPosition; }
    set ContactPosition(newValue: string) {
        if (this.Address.ContactPosition != newValue) {
            this.Address.ContactPosition = newValue;
        }
    }

    get ContactBusinessPhone() { return this.Address.ContactBusinessPhone; }
    set ContactBusinessPhone(newValue: string) {
        if (this.Address.ContactBusinessPhone != newValue) {
            this.Address.ContactBusinessPhone = newValue;
        }
    }

    get ContactMobile() { return this.Address.ContactMobile; }
    set ContactMobile(newValue: string) {
        if (this.Address.ContactMobile != newValue) {
            this.Address.ContactMobile = newValue;
        }
    }

    get ContactFax() { return this.Address.ContactFax; }
    set ContactFax(newValue: string) {
        if (this.Address.ContactFax != newValue) {
            this.Address.ContactFax = newValue;
        }
    }

    public HasCardContact: boolean = false;
    public Info1Text: string = null;
    public Info2Text: string = null;
    public ExistedContactId: string = null;
    private loadedContact: ContactPM = null;
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
            this.ExistedContactId = null;
            this.SetUIProperties_Contact();
            this.ValidateContactExist();
        }

        else {
            this.DomainService.GetContactsByEmail(email).subscribe((myResult:any) => {
                if (myResult != null) {
                    this.loadedContact = myResult[0];
                    this.SetUIProperties_Contact();

                    if (this.loadedContact != null) {
                        this.ExistedContactId = this.loadedContact.Id;

                        if (this.loadedContact.InActive) {
                            this.Info1Text = "This Contact is InActive";
                        }

                        else {
                            this.ContactName = this.loadedContact.EnglishName;
                            this.ContactPosition = this.loadedContact.Position;
                            this.ContactBusinessPhone = this.loadedContact.BusinessPhone;
                            this.ContactMobile = this.loadedContact.Mobile;
                            this.ContactFax = this.loadedContact.Fax;
                            this.HasCardContact = this.loadedContact.HasCardContact;

                            this.DomainService.GetCardContactsByContact(this.loadedContact.Id).subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    this.allCardContacts = myResponse.Result;
                                    this.ValidateContactExist();

                                    var otherCardContacts = this.allCardContacts.filter(d => d.ContactId == this.loadedContact.Id && d.CardId != this.CardId);

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
                }
            });
        }
    }

    public Validate() {
        var errors: string[] = [];
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        this.ValidateCardCode(errors, msg);
        this.ValidateAddress(errors);

        var isLanguageValid = AddressValidator.IsMainAddressEnglishCharacters(this.Address);
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

        if (this.PartnerTypeId == "TR" || this.PartnerTypeId == "WH") {
            if (AppTool.IsNullOrEmpty(this.CardCode)) {
                errors.push(msg.replace("%FieldName", "Code"));
            }
        }

        if (this.IsCustomerPartner) {
            this.ValidateCustomerFields(errors);
        }

        if (this.IsAddContactChecked) {
            this.Contact.EnglishName = this.ContactName;
            this.Contact.LocalName = this.ContactName;
            this.Contact.Email = this.ContactEmail;
            this.Contact.Position = this.ContactPosition;
            this.Contact.BusinessPhone = this.ContactBusinessPhone;
            this.Contact.Mobile = this.ContactMobile;
            this.Contact.Fax = this.ContactFax;

            if (this.loadedContact != null) {
                this.Contact.Anniversary = this.loadedContact.Anniversary;
                this.Contact.Birthday = this.loadedContact.Birthday;
                this.Contact.InActive = this.loadedContact.InActive;
            }

            Validator.TryValidateObject(this.Contact, "Contact", errors);

            if (AppTool.IsNullOrEmpty(this.Contact.EnglishName)) {
                errors.push(msg.replace("%FieldName", "English Name"));
            }

            if (!FormatTool.IsEmail(this.ContactEmail)) {
                errors.push("Invalid email format!");
            }

            if (this.ValidateContactExist()) {
                errors.push("This Contact is Already added for you");
            }

            if (this.Contact.InActive) {
                errors.push("This Contact is InActive");
            }
        }

        return errors;
    }

    private ValidateAddress(errors: string[]) {
        var newPotentialAddressCity = this.Address.City;
        if (AppTool.IsNullOrEmpty(this.Address.City) && this.PartnerTypeId == "PO") {
            this.Address.City = (AppTool.IsNullOrEmpty(this.Address.City) ? " Potential city " : this.Address.City);
        }

        Validator.TryValidateObject(this.Address, this.ObjectTableName, errors);

        if (this.PartnerTypeId == "PO") {
            this.Address.City = newPotentialAddressCity;
        }
    }

    private ValidateCardCode(errors: string[], msg:string) {
        if (this.PartnerTypeId == "TR" || this.PartnerTypeId == "WH") {
            if (AppTool.IsNullOrEmpty(this.CardCode)) {
                errors.push(msg.replace("%FieldName", "Code"));
            }

            else {
                if (this.CardTableName == "Trucker") {
                    if (this.CardCode.length >= 7) {
                        errors.push("Code must be less than 7");
                    }
                }
                else {
                    if (this.CardCode.length >= 5) {
                        errors.push("Code must be less than 5");
                    }
                }
            }
        }
    }
    private ValidateCustomerFields(errors: string[]) {
        if (this.IsCustomerPartner) {
            if (this.IsCustomer) {
                var args = new VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.EntityPM.IsCustomer;
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
        }
    }
    private ValidateContactExist() {
        var isContactAlreadyExist = false;

        if (this.CardId != null) {
            if (this.loadedContact != null) {
                if (this.allCardContacts != null) {
                    if (this.allCardContacts.filter(d => d.ContactId == this.loadedContact.Id && d.CardId == this.CardId).length > 0) {
                        isContactAlreadyExist = true;
                    }
                }
            }
        }

        this.Info1Text = isContactAlreadyExist ? TextCodeTranslator.Translate("Contact.M.ContactAddedForYou") : null;
        return isContactAlreadyExist;
    }
}
