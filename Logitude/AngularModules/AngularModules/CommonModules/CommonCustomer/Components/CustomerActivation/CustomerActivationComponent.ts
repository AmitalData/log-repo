import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import {CustomerPMService} from '../../../../Common/Services/StandardPMs/CustomerPMService';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {CustomerActivationArgs, CitySelectionArgs} from '../../../../Common/Args';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {AppTool} from '../../../../Infrastructure/Tools';
import {VatNumberValidator, VATValidatorArgs} from '../../../../Infrastructure/Validators/VatNumberValidator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AddressValidator} from '../../../../Infrastructure/Validators/AddressValidator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './CustomerActivationComponent.html',
})

export class CustomerActivationComponent extends BaseComponent {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public DataContext: CustomerActivationComponent = this;
    public ValidationErrorsList: string[] = [];
    private partnersDomainService: PartnersDomainService;
    private customerService: CustomerPMService;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.partnersDomainService = new PartnersDomainService();
        this.customerService = new CustomerPMService;
    }

    public Message: string;
    public ActivateFeatureON: boolean;
    SetWindowArgs(args: CustomerActivationArgs) {
        this.EntityPM = args.EntityPM;

        if (args.ActivatedFromQuoteSide) {
            if (!FeatureLocator.HasFeaturePermession("Customer", "ACTIVATE")) {
                this.Message = "You have no permession to activate this customer";
                this.ActivateFeatureON = false;
            }

            else {
                if (args.ActivatedPartnerType == "SH") {
                    this.Message = "The Shipper is a potential one and has to be activated in order to build a shipment";
                }
                else {
                    this.Message = "The Consignee is a potential one and has to be activated in order to build a shipment";
                }
                
                this.ActivateFeatureON = true;
            }
        }

        else {
            this.ActivateFeatureON = true;
        }

        this.SetUIProperties();
        this.LoadPickupDeliveryAddress();        
        this.CheckIfVatUnique();
        this.RunAdditionalFieldsComponent();
        this.Clone();
    }

    private SetUIProperties() {
        if (!this.ActivateFeatureON) {
            this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Address1_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Address2_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ZipCode_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("City_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CountryId_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PhoneNumber_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FaxNumber_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ATTN_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomerSizeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ATTN_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ATTN_Potential", this.ObjectTableName, false);

            this.SetUIProperties_GeneratedComponent();
        }

        else {
            this.SetUIProperties_Others();
            this.SetUIProperties_VAT();
            this.SetUIProperties_State();
            this.SetUIProperties_TelFax();
            this.SetUIProperties_GeneratedComponent();
        }
    }
    private SetUIProperties_Others() {
        this.UIProperties.SetRequired("City_Potential", this.ObjectTableName, AppTool.IsNullOrEmpty(this.City_Potential));

        if (this.EntityPM.IsCustomer) {
            if (SessionLocator.TenantPM.IsCustomerAddress1Required) {
                this.UIProperties.SetRequired("Address1_Potential", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Address1_Potential));
            }
        }
    }
    private SetUIProperties_VAT() {
        if (this.EntityPM.IsCustomer) {
            var args = new VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.EntityPM.IsCustomer;
            args.PartnerTypeId = this.EntityPM.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.CountryName;
            args.CountryEnglishName = this.CountryEnglishName;
            args.SetReady = false;

            VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator.ValidateVatMandatory(args);

            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
        }
    }
    private SetUIProperties_State() {
        var stateIsEnabled = true;

        if (AppTool.IsNullOrEmpty(this.CountryId_Potential)) {
            stateIsEnabled = false;
        }
        else {
            var countryListService: CountryListService = new CountryListService();
            countryListService.getSingleFromCache(this.CountryId_Potential).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: CountryList = myResponse.Result;
                    if (list != null) {
                        if (!list.HasStates) {
                            stateIsEnabled = false;
                        }
                    }
                    this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, stateIsEnabled);
                }
            });
        }
        
    }
    private SetUIProperties_TelFax() {
        if (this.EntityPM.IsCustomer) {
            if (SessionLocator.TenantPM.IsCustomerTelRequired) {
                this.UIProperties.SetRequired("PhoneNumber_Potential", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PhoneNumber_Potential));
            }

            if (SessionLocator.TenantPM.IsCustomerFaxRequired) {
                this.UIProperties.SetRequired("FaxNumber_Potential", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FaxNumber_Potential));
            }
        }
    }

    private PickupAddress: AddressList;
    private LoadPickupDeliveryAddress() {
        this.partnersDomainService.GetAddressByCardAndType(this.EntityPM.Id, "P").subscribe((myResponse: any) => {
            this.PickupAddress = myResponse;
        });
    }

    private vatTypeNotUnique: boolean;
    private CheckIfVatUnique() {
        if (this.EntityPM.IsCustomer) {
            if (!AppTool.IsNullOrEmpty(this.VatNumber)) {
                this.partnersDomainService.GetIsVATUniqueForCustomer(this.VatNumber, this.EntityPM.Id, this.CountryId_Potential).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.vatTypeNotUnique = myResponse.Result;
                    }
                });
            }
        }      
    }

    private RunAdditionalFieldsComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private GeneratedComponent: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunAdditionalFieldsComponent(), 1);
        }
    }
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                //cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, "Customer.AdditionalFields");
                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = "Customer.AdditionalFields";
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
            });
    }

    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.ActivateFeatureON);
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }    

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }    

    get Address1_Potential() { return this.EntityPM.Address1_Potential; }
    set Address1_Potential(newValue: string) {
        if (this.EntityPM.Address1_Potential != newValue) {
            this.EntityPM.Address1_Potential = newValue;

            this.SetUIProperties_Others();
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

            this.SetUIProperties_Others();
        }
    }    

    get CountryId_Potential() { return this.EntityPM.CountryId_Potential; }
    set CountryId_Potential(newValue: string) {
        if (this.EntityPM.CountryId_Potential != newValue) {
            this.EntityPM.CountryId_Potential = newValue;

            this.StateId_Potential = null;            
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
    
    get StateId_Potential() { return this.EntityPM.StateId_Potential; }
    set StateId_Potential(newValue: string) {
        if (this.EntityPM.StateId_Potential != newValue) {
            this.EntityPM.StateId_Potential = newValue;
        }
    }    
    
    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM.VatNumber != newValue) {
            this.EntityPM.VatNumber = newValue;

            this.SetUIProperties_VAT();
            this.CheckIfVatUnique();
        }
    }    

    get PhoneNumber_Potential() { return this.EntityPM.PhoneNumber_Potential; }
    set PhoneNumber_Potential(newValue: string) {
        if (this.EntityPM.PhoneNumber_Potential != newValue) {
            this.EntityPM.PhoneNumber_Potential = newValue;

            this.SetUIProperties_TelFax();
        }
    } 

    get FaxNumber_Potential() { return this.EntityPM.FaxNumber_Potential; }
    set FaxNumber_Potential(newValue: string) {
        if (this.EntityPM.FaxNumber_Potential != newValue) {
            this.EntityPM.FaxNumber_Potential = newValue;

            this.SetUIProperties_TelFax();
        }
    }    

    get ATTN_Potential() { return this.EntityPM.ATTN_Potential; }
    set ATTN_Potential(newValue: string) {
        if (this.EntityPM.ATTN_Potential != newValue) {
            this.EntityPM.ATTN_Potential = newValue;
        }
    }    

    get CustomerSizeId() { return this.EntityPM.CustomerSizeId; }
    set CustomerSizeId(newValue: string) {
        if (this.EntityPM.CustomerSizeId != newValue) {
            this.EntityPM.CustomerSizeId = newValue;
        }
    }    

    private CountryName: string;
    private CountryEnglishName: string;
    private OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.CountryName = null;
            this.CountryEnglishName = null;
        }

        else {
            this.CountryName = this.EntityPM.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }

        this.SetUIProperties_State();
        this.SetUIProperties_VAT();
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
                if (this.EntityPM.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }

                this.City_Potential = mySelectedCity;
                this.CountryId_Potential = args.CountryId;
                this.StateId_Potential = args.StateId;
            }
        });
    }
    
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
        var test = this.CurrentSession.CurrentEditComponent;

    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('EnglishName');
        this.myCloner.AddField('LocalName');
        this.myCloner.AddField('Address1_Potential');
        this.myCloner.AddField('Address2_Potential');
        this.myCloner.AddField('ZipCode_Potential');
        this.myCloner.AddField('City_Potential');
        this.myCloner.AddField('CountryId_Potential');
        this.myCloner.AddField('StateId_Potential');
        this.myCloner.AddField('VatNumber');
        this.myCloner.AddField('PhoneNumber_Potential');
        this.myCloner.AddField('FaxNumber_Potential');
        this.myCloner.AddField('ATTN_Potential');
        this.myCloner.AddField('CustomerSizeId');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    OkButtonClicked() {
        var screenErrors: string[] = [];
        var allErrors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, screenErrors);

        if (this.EntityPM.IsCustomer) {
            var args = new VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.EntityPM.IsCustomer;
            args.PartnerTypeId = this.EntityPM.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.CountryName;
            args.CountryEnglishName = this.CountryEnglishName;
            args.SetReady = false;

            VatNumberValidator.ValidateVatMandatory(args);
            VatNumberValidator.ValidateVatFormat(args);

            if (args.Errors.length > 0) {
                screenErrors = args.Errors;
            }

            if (SessionLocator.TenantPM.IsCustomerTelRequired) {
                if (AppTool.IsNullOrEmpty(this.PhoneNumber_Potential)) {
                    screenErrors.push("Phone Number is required");
                }
            }

            if (SessionLocator.TenantPM.IsCustomerFaxRequired) {
                if (AppTool.IsNullOrEmpty(this.FaxNumber_Potential)) {
                    screenErrors.push("Fax Number is required");
                }
            }

            if (SessionLocator.TenantPM.IsCustomerAddress1Required) {
                if (AppTool.IsNullOrEmpty(this.Address1_Potential)) {
                    screenErrors.push("Address1 is required");
                }
            }
        }

        if (this.vatTypeNotUnique) {
            if (SessionLocator.TenantPM.VatUniqueTypeCode == "UFA") {
                screenErrors.push("VAT Number already exists");
            }

            else if (SessionLocator.TenantPM.VatUniqueTypeCode == "USC") {
                screenErrors.push("VAT Number already exists for " + this.CountryEnglishName);
            }
        }

        if (AppTool.IsNullOrEmpty(this.City_Potential)) {
            screenErrors.push("City is required");
        }
        
        var isLanguageValid = AddressValidator.IsMainAddressEnglishCharacters_Potential(this.EntityPM);
        if (!isLanguageValid) {
            screenErrors.push("Main address does not allow non-english characters");
        }

        screenErrors.forEach((error) => {
            allErrors.push(error);
        });

        if (this.EntityPM.IsCustomer) {
            if (SessionLocator.TenantPM.IsPickDelAdrsRequired) {
                if (this.PickupAddress == null) {
                    allErrors.push("PickUp Delivery Address is required please fill it");
                }
            }

            if (SessionLocator.TenantPM.HasPrimaryContact) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.PrimaryContactId)) {
                    allErrors.push("There is no Primary Contact please add one");
                }
            }
        }

        this.ValidationErrorsList = allErrors;

        if (allErrors.length == 0) {
            ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.DoActivation();
        }

        else if (screenErrors.length == 0) {
            this.EntityPM.SavedForActivation = true;
            this.Save("Just Saved");
        }
    }

    private DoActivation() {
        this.EntityPM.SetActivated = true;
        this.EntityPM.SetReady = false;

        this.Save("Activated");
    }

    private Save(msg: string) {
        if (msg == "Activated") {
            this.CurrentSession.StartBusyIndicatorSaving();
        }

        this.customerService.update(this.EntityPM).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                if (msg == "Activated") {
                    this.CurrentSession.CloseCurrentWindowEmit(msg);
                    this.CurrentSession.FireEvent("EntityActivated");
                }
                
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }    
}
