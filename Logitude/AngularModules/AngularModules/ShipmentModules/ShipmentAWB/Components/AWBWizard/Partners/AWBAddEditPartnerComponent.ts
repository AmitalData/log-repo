import {Component, AfterViewInit} from '@angular/core';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {CitySelectionArgs} from '../../../../../Common/Args';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {StateList} from '../../../../../Common/EntityLists/StateList';
import {CountryList} from '../../../../../Common/EntityLists/CountryList';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {AddressPM} from '../../../../../Common/EntityPMs/AddressPM';
import {AgentPM} from '../../../../../Common/EntityPMs/AgentPM';
import {CustomerPM} from '../../../../../Common/EntityPMs/CustomerPM';
import {CustomAgentPM} from '../../../../../Common/EntityPMs/CustomAgentPM';
import {ShippingAgentPM} from '../../../../../Common/EntityPMs/ShippingAgentPM';
import {VendorPM} from '../../../../../Common/EntityPMs/VendorPM';
import {WarehousePM} from '../../../../../Common/EntityPMs/WarehousePM';
import {AirlinePM} from '../../../../../Common/EntityPMs/AirlinePM';
import {ShippingLinePM} from '../../../../../Common/EntityPMs/ShippingLinePM';
import {TruckerPM} from '../../../../../Common/EntityPMs/TruckerPM';
import {AddressPMService} from '../../../../../Common/Services/StandardPMs/AddressPMService';
import {AgentPMService} from '../../../../../Common/Services/StandardPMs/AgentPMService';
import {CustomerPMService} from '../../../../../Common/Services/StandardPMs/CustomerPMService';
import {CustomAgentPMService} from '../../../../../Common/Services/StandardPMs/CustomAgentPMService';
import {ShippingAgentPMService} from '../../../../../Common/Services/StandardPMs/ShippingAgentPMService';
import {VendorPMService} from '../../../../../Common/Services/StandardPMs/VendorPMService';
import {WarehousePMService} from '../../../../../Common/Services/StandardPMs/WarehousePMService';
import {AirlinePMService} from '../../../../../Common/Services/StandardPMs/AirlinePMService';
import {ShippingLinePMService} from '../../../../../Common/Services/StandardPMs/ShippingLinePMService';
import {TruckerPMService} from '../../../../../Common/Services/StandardPMs/TruckerPMService';
import {PartnersDomainService, PartnerServicePM} from '../../../../../Common/Services/PartnersDomainService';
import {AddressValidator} from '../../../../../Infrastructure/Validators/AddressValidator';
import {VatNumberValidator, VATValidatorArgs} from '../../../../../Infrastructure/Validators/VatNumberValidator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AddEditPartnerArgs} from '../../../../../Shipment/Args';
import {InfraSettings} from '../../../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';

@Component({
    moduleId: module.id,
    templateUrl: './AWBAddEditPartnerComponent.html',
})

export class AWBAddEditPartnerComponent extends BaseComponent implements AfterViewInit {
    private shipmentPM: ShipmentPM;
    public EntityPM: AddressPM = null;
    public IsNewEntity: boolean = false;
    public IsCancelled: boolean = false;
    public PartnerTypeId: string;
    public PartnerTypeCode: string;
    public CurrentPartnerId: string;
    public CurrentAddressId: string;
    public IsUpdatingPartner: boolean = false;
    public DataContext: AWBAddEditPartnerComponent = this;
    public ObjectTableName: string = "Address";
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: AddEditPartnerArgs) {
        this.shipmentPM = windowArgs.EntityPM;
        this.IsNewEntity = windowArgs.IsNewEntity;
        this.PartnerTypeCode = windowArgs.PartnerTypeCode;
        this.InitializeComponent();
    }

    ngAfterViewInit() {
        this.SetUIProperties();
    }

    // Select Address

    private oldAddressId: string;
    private selectedAddressId: string;
    get SelectedAddressId() { return this.selectedAddressId; }
    set SelectedAddressId(newValue: string) {
        if (this.selectedAddressId != newValue) {
            this.oldAddressId = this.selectedAddressId;
            this.selectedAddressId = newValue;

            var isLoadingAddress = false;
            if (this.EntityPM == null) {
                isLoadingAddress = true;
            }

            else if (!this.EntityPM.IsDirty) {
                isLoadingAddress = true;
            }

            if (isLoadingAddress) {
                this.CurrentSession.StartBusyIndicatorLoading();
                this.CurrentAddressId = newValue;
                this.LoadAddress();
            }

            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", this.ObjectTableName));
                confirmWindow.WindowClosed.subscribe((event: any) => {

                    if (confirmWindow.Yes) {
                        var isValid = this.Validate();
                        if (isValid) {
                            this.CurrentSession.StartBusyIndicatorSaving();
                            this.Save(true);
                        }
                    }

                    else if (confirmWindow.Cancel) {
                        this.selectedAddressId = this.oldAddressId;
                    }

                    else if (confirmWindow.No) {
                        this.CurrentSession.StartBusyIndicatorLoading();
                        this.CurrentAddressId = newValue;
                        this.LoadAddress();
                    }
                });
            }
        }
    }

    // Load Data
    private myAgentPM: AgentPM;
    private myCustomerPM: CustomerPM;
    private myCustomAgentPM: CustomAgentPM;
    private myShippingAgentPM: ShippingAgentPM;
    private myVendorPM: VendorPM;
    private myWarehousePM: WarehousePM;
    private myAirlinePM: AirlinePM;
    private myShippingLinePM: ShippingLinePM;
    private myTruckerPM: TruckerPM;
    public HelpMessage: string = null;
    public ShowHelpMessage: boolean = false;
    public InitializeComponent() {

        if (this.IsNewEntity) {
            if (this.PartnerTypeCode == "AGT" || this.shipmentPM.ShipmentLevelCode == "C") {
                this.PartnerTypeId = "AG";

                this.myAgentPM = new AgentPM();
                this.myAgentPM.Tenant = this.shipmentPM.Tenant;
                this.myAgentPM.PartnerTypeId = this.PartnerTypeId;
                this.myAgentPM.Code = "new";

                this.EntityPM = new AddressPM();
                this.EntityPM.Tenant = this.shipmentPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";

                this.myAgentPM.Addresses.push(this.EntityPM);
            }

            else {
                this.PartnerTypeId = "CS";

                this.myCustomerPM = new CustomerPM();
                this.myCustomerPM.Tenant = this.shipmentPM.Tenant;
                this.myCustomerPM.PartnerTypeId = this.PartnerTypeId;
                this.myCustomerPM.CustomerStatusCode = "ACT";
                this.myCustomerPM.IsCustomer = this.PartnerTypeCode == "SHI" ? true : false;
                this.myCustomerPM.Code = "new";  

                this.EntityPM = new AddressPM();
                this.EntityPM.Tenant = this.shipmentPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";

                this.myCustomerPM.Addresses.push(this.EntityPM);
            }

            if (this.shipmentPM.ShipmentLevelCode == "C") {
                switch (this.PartnerTypeCode) {
                    case "SHI": {
                        this.HelpMessage = "Shipper will be added as Agent";
                        this.ShowHelpMessage = true;
                        break;
                    }

                    case "CON": {
                        this.HelpMessage = "Consignee will be added as Agent";
                        this.ShowHelpMessage = true;
                        break;
                    }

                    case "NTF": {
                        this.HelpMessage = "Notify will be added as Agent";
                        this.ShowHelpMessage = true;
                        break;
                    }
                }
            }

            this.SetUIProperties();
        }

        else {            

            switch (this.PartnerTypeCode) {
                case "SHI":
                    {
                        this.CurrentPartnerId = this.shipmentPM.ShipperId;
                        this.CurrentAddressId = this.shipmentPM.ShipperAddressId;
                        break;
                    }

                case "CON":
                    {
                        this.CurrentPartnerId = this.shipmentPM.ConsigneeId;
                        this.CurrentAddressId = this.shipmentPM.ConsigneeAddressId;
                        break;
                    }

                case "AGT":
                    {
                        this.CurrentPartnerId = this.shipmentPM.IssuingCarrierAgentId;
                        this.CurrentAddressId = this.shipmentPM.IssuingCarrierAddressId;
                        break;
                    }

                case "NTF":
                    {
                        this.CurrentPartnerId = this.shipmentPM.Notify1Id;
                        this.CurrentAddressId = this.shipmentPM.Notify1AddressId;
                        break;
                    }                    
            }

            this.selectedAddressId = this.CurrentAddressId;

            if (!AppTool.IsNullOrEmpty(this.CurrentPartnerId)) {

                this.CurrentSession.StartBusyIndicatorLoading();

                var myService: CardListService = new CardListService();
                

                myService.getSingle(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            this.CurrentSession.StopBusyIndicator();
                        }

                        else {
                            this.PartnerTypeId = myResponse.Result.PartnerTypeId;
                            this.LoadPartner();
                            this.LoadAddress();
                        }
                    }

                    else {
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    }

    private isPartnerLoaded: boolean = false;
    private isAddressLoaded: boolean = false;
    private LoadPartner() {
        this.isPartnerLoaded = false;

        switch (this.PartnerTypeId) {
            case "AG": {
                var myAgentService = new AgentPMService();
                myAgentService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myAgentPM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "CS": {
                var myCustomerService = new CustomerPMService();
                myCustomerService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myCustomerPM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "CG": {
                var myCustomAgentService = new CustomAgentPMService();
                myCustomAgentService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myCustomAgentPM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "SG": {
                var myShippingAgentService = new ShippingAgentPMService();
                myShippingAgentService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myShippingAgentPM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "VD": {
                var myVendorService = new VendorPMService();
                myVendorService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myVendorPM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "WH": {
                var myWarehouseService = new WarehousePMService();
                myWarehouseService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myWarehousePM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "AL": {
                var myAirlineService = new AirlinePMService();
                myAirlineService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myAirlinePM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "SL": {
                var myShippingLineService = new ShippingLinePMService();
                myShippingLineService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myShippingLinePM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }

            case "TR": {
                var myTruckerService = new TruckerPMService();
                myTruckerService.get(this.CurrentPartnerId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.myTruckerPM = myResponse.Result;
                            this.isPartnerLoaded = true;
                            this.OnLoadCompleted();
                        }
                    }
                });
                break;
            }
        }
    }
    private LoadAddress() {
        this.isAddressLoaded = false;

        if (AppTool.IsNullOrEmpty(this.CurrentAddressId)) {
            this.EntityPM = new AddressPM();
            this.EntityPM.Description = "Other Address";
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.CardId = this.CurrentPartnerId;
            this.EntityPM.AddressTypeId = 'O';
            this.EntityPM.InActive = false;

            this.isAddressLoaded = true;
            this.OnLoadCompleted();
        }

        else {
            var myService = new AddressPMService();
            myService.get(this.CurrentAddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.EntityPM = myResponse.Result;
                        this.isAddressLoaded = true;
                        this.OnLoadCompleted();
                    }
                }
            });
        }
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

                if (!AppTool.IsNullOrEmpty(newValue)) {
                    if (newValue.length > 70) {
                        this.EntityPM.Name = newValue.substr(0, 70);
                    }
                }

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

    get IsLocalLanguage() { return this.EntityPM == null ? false : this.EntityPM.IsLocalLanguage; }
    set IsLocalLanguage(newValue: boolean) {
        if (this.EntityPM != null) {
            if (this.EntityPM.IsLocalLanguage != newValue) {
                this.EntityPM.IsLocalLanguage = newValue;

                if (this.Country != null) {
                    this.CountryName = this.EntityPM.IsLocalLanguage ? this.Country.LocalName : this.Country.EnglishName;
                }

                if (!newValue) {
                    if (!AppTool.IsNullOrEmpty(this.Description)) {
                        this.Description = this.Description.replace(/[^\x20-\x7F]/g, "");
                    }

                    if (!AppTool.IsNullOrEmpty(this.CardEnglishName)) {
                        this.CardEnglishName = this.CardEnglishName.replace(/[^\x20-\x7F]/g, "");
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

    // SelectCity
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
        this.IsCancelled = true;
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
                this.IsUpdatingPartner = true;
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

                    case "CG":
                        {
                            if (this.myCustomAgentPM != null) {

                                if (this.myCustomAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myCustomAgentPM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myCustomAgentPM.VatNumber != this.VatNumber) {
                                    this.myCustomAgentPM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myCustomAgentPM.IsDirty;
                            }

                            break;
                        }

                    case "SG":
                        {
                            if (this.myShippingAgentPM != null) {

                                if (this.myShippingAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myShippingAgentPM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myShippingAgentPM.VatNumber != this.VatNumber) {
                                    this.myShippingAgentPM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myShippingAgentPM.IsDirty;
                            }

                            break;
                        }

                    case "VD":
                        {
                            if (this.myVendorPM != null) {

                                if (this.myVendorPM.EnglishName != this.CardEnglishName) {
                                    this.myVendorPM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myVendorPM.VatNumber != this.VatNumber) {
                                    this.myVendorPM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myVendorPM.IsDirty;
                            }

                            break;
                        }

                    case "WH":
                        {
                            if (this.myWarehousePM != null) {

                                if (this.myWarehousePM.EnglishName != this.CardEnglishName) {
                                    this.myWarehousePM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myWarehousePM.VatNumber != this.VatNumber) {
                                    this.myWarehousePM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myWarehousePM.IsDirty;
                            }

                            break;
                        }

                    case "AL":
                        {
                            if (this.myAirlinePM != null) {

                                if (this.myAirlinePM.EnglishName != this.CardEnglishName) {
                                    this.myAirlinePM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myAirlinePM.VatNumber != this.VatNumber) {
                                    this.myAirlinePM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myAirlinePM.IsDirty;
                            }

                            break;
                        }

                    case "SL":
                        {
                            if (this.myShippingLinePM != null) {

                                if (this.myShippingLinePM.EnglishName != this.CardEnglishName) {
                                    this.myShippingLinePM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myShippingLinePM.VatNumber != this.VatNumber) {
                                    this.myShippingLinePM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myShippingLinePM.IsDirty;
                            }

                            break;
                        }

                    case "TR":
                        {
                            if (this.myTruckerPM != null) {

                                if (this.myTruckerPM.EnglishName != this.CardEnglishName) {
                                    this.myTruckerPM.EnglishName = this.CardEnglishName;
                                }

                                if (this.myTruckerPM.VatNumber != this.VatNumber) {
                                    this.myTruckerPM.VatNumber = this.VatNumber;
                                }

                                this.isPartnerDirty = this.myTruckerPM.IsDirty;
                            }

                            break;
                        }
                }

                this.Save(false);
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
                var myCity:string = this.City;
                var myName:string = this.CardEnglishName;

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
    private Save(isSelectedAddressSaving: boolean) {
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
        args.CustomAgent = this.myCustomAgentPM;
        args.ShippingAgent = this.myShippingAgentPM;
        args.Vendor = this.myVendorPM;
        args.Warehouse = this.myWarehousePM;
        args.Airline = this.myAirlinePM;
        args.ShippingLine = this.myShippingLinePM;
        args.Trucker = this.myTruckerPM;

        this.myPartnersDomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (!myResponse.HasError) {

                if (isSelectedAddressSaving) {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.CurrentAddressId = this.selectedAddressId;
                    this.LoadAddress();
                }

                else {
                    this.CurrentAddressId = myResponse.Result.AddressId;
                    this.CurrentPartnerId = myResponse.Result.PartnerId;
                    this.IsUpdatingPartner = true;
                    this.CurrentSession.CloseCurrentWindow();
                }
            }

            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;

                if (isSelectedAddressSaving) {
                    this.selectedAddressId = this.oldAddressId;
                }
            }
        });
    }
}
