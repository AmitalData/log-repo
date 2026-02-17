import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CitySelectionArgs} from '../../../../Common/Args';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CountryFlagPipe} from '../../../../Controls/Pipes/CountryFlagPipe';

@Component({
    moduleId: module.id,
    templateUrl: './AddressesTabComponent.html',
})

export class AddressesTabComponent {
    public ItemsSource: AddressItemClass[];
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTableName: string;    
    public Customer: CustomerPM = null;
    public PartnerTypeId: string = null;
    public IsCustomerPartner: boolean = false;
    public DomainService: PartnersDomainService;
    public IsVisibile: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(response=> {
            this.IsVisibile = true;
            this.ItemsSource = [];
            this.EntityPM = entityArgs.EntityPM;
            this.EntityId = entityArgs.EntityPM == null ? null : entityArgs.EntityPM.Id;
            this.ObjectTableName = entityArgs.ObjectTableName;
            this.PartnerTypeId = this.EntityPM.PartnerTypeId;

            if (this.EntityPM instanceof CustomerPM) {
                this.Customer = this.EntityPM;
                this.IsCustomerPartner = true;
            }

            if (this.DomainService == null) {
                this.DomainService = new PartnersDomainService();
            }

            this.Listen();
            this.SetUIProperties();
            this.LoadData();           
        });
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "EntityActivated") {
                    if (this.ObjectTableName == "Customer") {
                        this.SetUIProperties();
                    }
                }
            });
        }
    }
    
    public IsEditingEnabled: boolean = false;
    public IsBlockingUnifreightCustomer: boolean = false;
    private SetUIProperties() {
        var isBlockingUnifreightCustomer = false;

        if (this.Customer != null) {
            if (SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
                isBlockingUnifreightCustomer = true;
            }
        }

        this.IsBlockingUnifreightCustomer = isBlockingUnifreightCustomer;
        this.IsEditingEnabled = !isBlockingUnifreightCustomer;

        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }

    public AllAddresses: AddressPM[] = [];
    private LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllAddressesPMsbyCardId(this.EntityId).subscribe((myResult:any) => {
            this.AllAddresses = myResult;
            this.BuildItemsSource();
            this.CurrentSession.StopBusyIndicator();
        });
    }
    public BuildItemsSource() {
        this.ItemsSource = [];

        var items: AddressPM[] = [];
        this.AllAddresses.forEach(item => {

            if (item.AddressTypeId != "O") {
                items.push(item);
            }

            else if (this.ShowInactive) {
                items.push(item);
            }

            else {
                if (item.InActive == false) {
                    items.push(item);
                }
            }
        });

        if (items != null) {
            var isNewAddress_M = false;
            var isNewAddress_B = false;
            var isNewAddress_P = false;
            var myAddress_M = items.filter(f => f.AddressTypeId == "M")[0];
            var myAddress_B = items.filter(f => f.AddressTypeId == "B")[0];
            var myAddress_P = items.filter(f => f.AddressTypeId == "P")[0];

            if (myAddress_M == null) {
                isNewAddress_M = true;
                myAddress_M = new AddressPM();
                myAddress_M.Tenant = this.EntityPM.Tenant;
                myAddress_M.AddressTypeId = "M";
                myAddress_M.Description = "Main Address";
                myAddress_M.CardId = this.EntityId;
                myAddress_M.InActive = false;
                this.AllAddresses.push(myAddress_M);
            }

            if (myAddress_B == null) {
                isNewAddress_B = true;
                myAddress_B = new AddressPM();
                myAddress_B.Tenant = this.EntityPM.Tenant;
                myAddress_B.AddressTypeId = "B";
                myAddress_B.Description = "Billing Address";
                myAddress_B.CardId = this.EntityId;
                myAddress_B.InActive = false;
                this.AllAddresses.push(myAddress_B);
            }

            if (myAddress_P == null) {
                isNewAddress_P = true;
                myAddress_P = new AddressPM();
                myAddress_P.Tenant = this.EntityPM.Tenant;
                myAddress_P.AddressTypeId = "P";
                myAddress_P.Description = "Pickup / Delivery Address";
                myAddress_P.CardId = this.EntityId;
                myAddress_P.InActive = false;
                this.AllAddresses.push(myAddress_P);
            }

            this.ItemsSource.push(new AddressItemClass(myAddress_M, isNewAddress_M, this));
            this.ItemsSource.push(new AddressItemClass(myAddress_B, isNewAddress_B, this));
            this.ItemsSource.push(new AddressItemClass(myAddress_P, isNewAddress_P, this));

            items.filter(f => f.AddressTypeId == "O" || f.AddressTypeId == "L").forEach(item => {
                this.ItemsSource.push(new AddressItemClass(item, false, this));
            });
        }
    }

    private showInactive = false;
    get ShowInactive() { return this.showInactive; }
    set ShowInactive(newValue: boolean) {
        if (this.showInactive != newValue) {
            this.showInactive = newValue;
            this.BuildItemsSource();
        }
    }

    AddAddressClicked() {

        var item = new AddressPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CardId = this.EntityId;
        item.Name = this.EntityPM.EnglishName;
        item.AddressTypeId = 'O';
        item.InActive = false;

        var itemViewModel = new AddressItemClass(item, true, this);
        this.RunAddEditWindow(itemViewModel, TextCodeTranslator.Translate("Address.O.AddAddress"));
    }
    EditAddressClicked(itemViewModel: AddressItemClass) {
        var textCode: string = itemViewModel.IsNewEntity ? "Address.O.AddAddress" : "Address.O.EditAddress";
        this.RunAddEditWindow(itemViewModel, TextCodeTranslator.Translate(textCode));
    }
    private RunAddEditWindow(itemViewModel: AddressItemClass, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemViewModel;
        logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditAddressComponent');
    }
}

export class AddressItemClass extends BaseComponent {
    public Header: string;
    public ObjectTableName = "Address";
    public EntityPM: AddressPM;
    public IsNewEntity: boolean = false;
    public Src = null;
    constructor(item: AddressPM, isNewEntity: boolean, public fatherComponent: AddressesTabComponent) {
        super();
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;
        this.SetHeader();
        this.SetUIProperties();
        var pipe = new CountryFlagPipe();
        this.Src = pipe.transform(this.CountryCode);
    }

    private SetHeader() {
        //if (this.AddressTypeId.toUpperCase() == "M") {
        //    this.Header = TextCodeTranslator.Translate("Address.O.MainAddress");
        //}

        //else if (this.AddressTypeId.toUpperCase() == "B") {
        //    this.Header = TextCodeTranslator.Translate("Address.O.BillingAddress");
        //}

        //else {
            this.Header = this.Description;
        //}
    }

    public IsInActiveVisible: boolean = false;
    public IsEditingEnabled: boolean = false;
    public IsBlockingUnifreightCustomer: boolean = false;
    public SetUIProperties() {
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.IsBlockingUnifreightCustomer = this.fatherComponent.IsBlockingUnifreightCustomer;

        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Address1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Address2", this.ObjectTableName, this.IsEditingEnabled);        
        this.UIProperties.SetEnabled("ZipCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("City", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PhoneNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("FaxNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ATTN", this.ObjectTableName, this.IsEditingEnabled);

        var isInActiveVisible = false;
        if (this.AddressTypeId != "M") {
            if (!this.IsNewEntity) {
                isInActiveVisible = true;
            }
        }

        this.IsInActiveVisible = isInActiveVisible;
        this.SetUIProperties_State();
        this.SetUIProperties_TelFax();
    }
    private SetUIProperties_State() {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    private SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.IsEditingEnabled) {
            isEnabled = this.HasStates;
        }

        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    }
    private SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.State == null) {
            if (this.IsStateRequired) {
                isRequired = true;
            }
        }

        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    }
    private SetUIProperties_TelFax() {
        var isTelRequired = false;
        var isFaxRequired = false;

        if (this.fatherComponent.Customer != null) {
            if (this.fatherComponent.Customer.IsCustomer) {
                if (this.fatherComponent.Customer.PartnerTypeId == "CS") {
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

                else if (this.fatherComponent.Customer.PartnerTypeId == "PO") {
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
        }

        this.UIProperties.SetRequired("PhoneNumber", this.ObjectTableName, isTelRequired);
        this.UIProperties.SetRequired("FaxNumber", this.ObjectTableName, isFaxRequired);
    }

    // Properties
    get AddressTypeId() { return this.EntityPM.AddressTypeId; }
    set AddressTypeId(newValue: string) {
        if (this.EntityPM.AddressTypeId != newValue) {
            this.EntityPM.AddressTypeId = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
            this.SetHeader();
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

    get City() { return this.EntityPM.City; }
    set City(newValue: string) {
        if (this.EntityPM.City != newValue) {
            this.EntityPM.City = newValue;
        }
    }

    get ZipCode() { return this.EntityPM.ZipCode; }
    set ZipCode(newValue: string) {
        if (this.EntityPM.ZipCode != newValue) {
            this.EntityPM.ZipCode = newValue;
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

    get CityLineText() {
        var myResult = this.City;

        if (!AppTool.IsNullOrEmpty(this.StateEnglishName)) {
            myResult += ", " + this.StateEnglishName;
        }

        if (!AppTool.IsNullOrEmpty(this.ZipCode)) {
            myResult += ", " + this.ZipCode;
        }

        return myResult;
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

    get HasStates() { return this.EntityPM.HasStates; }
    set HasStates(newValue: boolean) {
        if (this.EntityPM.HasStates != newValue) {
            this.EntityPM.HasStates = newValue;
        }
    }

    get IsStateRequired() { return this.EntityPM.IsStateRequired; }
    set IsStateRequired(newValue: boolean) {
        if (this.EntityPM.IsStateRequired != newValue) {
            this.EntityPM.IsStateRequired = newValue;
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
            this.HasStates = false;
            this.IsStateRequired = false;
        }

        else {
            this.CountryCode = list.Code;
            this.CountryName = this.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
            this.HasStates = list.HasStates;
            this.IsStateRequired = list.IsStateRequired;
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

    // Commands
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

    public IsCopyMainAddress: boolean = false;
    CopyMainAddressClicked(isFromMainTab: boolean) {
        if (this.AddressTypeId == "P") {
            var address = this.fatherComponent.AllAddresses.filter(f => f.AddressTypeId == 'M')[0];
            if (address != null) {

                if (AppTool.IsNullOrEmpty(address.City)) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("Please fill the main address city");
                }

                else {
                    if (isFromMainTab) {
                        this.IsCopyMainAddress = true;
                        this.fatherComponent.EditAddressClicked(this);
                    }

                    else {
                        this.CopyMainAddress();
                    }
                }
            }
        }
    }
    public CopyMainAddress() {
        var address = this.fatherComponent.AllAddresses.filter(f => f.AddressTypeId == 'M')[0];

        if (address != null) {
            this.Name = address.Name;
            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.City = address.City;
            this.ATTN = address.ATTN;
            this.CountryId = address.CountryId;
            this.CountryCode = address.CountryCode;
            this.CountryName = address.CountryName;
            this.CountryEnglishName = address.CountryEnglishName;
            this.StateId = address.StateId;
            this.StateCode = address.StateCode;
            this.StateEnglishName = address.StateEnglishName;
            this.ZipCode = address.ZipCode;
            this.PhoneNumber = address.PhoneNumber;
            this.FaxNumber = address.FaxNumber;
            this.IsLocalLanguage = address.IsLocalLanguage;
        }

        this.IsCopyMainAddress = false;
    }
}
