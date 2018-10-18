import {Component, OnDestroy} from '@angular/core';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteDomainService, QuoteSubjectArgs} from '../../../../Quote/Services/QuoteDomainService';
import {QuoteUtilities} from '../../../../Quote/Utilities/QuoteUtilities';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {PortList} from '../../../../Common/EntityLists/PortList';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../../Common/Args';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'OrdinaryRoutingsComponent',
    moduleId: module.id,
    templateUrl: './OrdinaryRoutingsComponent.html',
})

export class OrdinaryRoutingsComponent extends BaseComponent implements OnDestroy {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quote";
    public DataContext = this;
    public IsSubjectVisible: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.IsSubjectVisible = SessionLocator.TenantPM.IsQuoteSubjectEdited ? true : false;
        this.InitializeServices();
        this.Listen();
    }

    InitTab(entityPM: QuotePM, tableName: string) {
        this.EntityPM = entityPM;
        this.LoadPickupDeliveryData();
        this.SetLabels();
        this.SetUIProperties();
    }

    private myAddressListService: AddressListService;
    InitializeServices() {
        this.myAddressListService = new AddressListService();
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTRT") {
                    this.LoadPickupDeliveryData();
                    this.SetUIProperties();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsQuoteEditEnabled: boolean = false;
    SetUIProperties() {
        this.IsQuoteEditEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        this.SetUIProperties_PickupFields();
        this.SetUIProperties_DeliveryFields();
        this.SetUIProperties_EntityClosed();
    }
    SetUIProperties_EntityClosed() {
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsQuoteEditEnabled);

        this.UIProperties.SetEnabled("IncludePickUp", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromAddressZipCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("PickUpAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);

        this.UIProperties.SetEnabled("IncludeDelivery", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToAddressZipCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("DeliveryAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, this.IsQuoteEditEnabled);
    }

    private LoadPickupDeliveryData() {
        if (this.IncludePickUp) {
            if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                if (!AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                    this.LoadPickupAddress();
                }
            }
        }

        if (this.IncludeDelivery) {
            if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                if (!AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                    this.LoadDeliveryAddress();
                }
            }
        }
    }

    // Subject
    get IsSubjectEdited() { return this.EntityPM.IsSubjectEdited; }
    set IsSubjectEdited(value: boolean) {
        if (this.EntityPM.IsSubjectEdited != value) {
            this.EntityPM.IsSubjectEdited = value;
        }
    }

    get Subject() { return this.EntityPM.Subject; }
    set Subject(value: string) {
        if (this.EntityPM.Subject != value) {
            this.EntityPM.Subject = value;
            this.IsSubjectEdited = true;
        }
    }

    ResetSubjectEdited() {
        this.IsSubjectEdited = false;
        this.GetSubjectField();
    }

    GetSubjectField() {
        if (SessionLocator.TenantPM.IsQuoteSubjectEdited) {
            var myQuoteDomainService = new QuoteDomainService();

            myQuoteDomainService.ComputeQuoteAutomaticSubject(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myArgs: QuoteSubjectArgs = myResponse.Result;

                    if (myArgs != null) {
                        this.EntityPM.Subject = myArgs.Subject;
                    }
                }
            });
        }
    }

    get ETD() { return this.EntityPM.ETD; }
    set ETD(newValue: Date) {
        if (this.EntityPM.ETD != newValue) {
            this.EntityPM.ETD = newValue;
        }
    }

    get ETA() { return this.EntityPM.ETA; }
    set ETA(newValue: Date) {
        if (this.EntityPM.ETA != newValue) {
            this.EntityPM.ETA = newValue;
        }
    }

    // PickUp
    get IncludePickUp() { return this.EntityPM.IncludePickUp; }
    set IncludePickUp(newValue: boolean) {
        if (this.EntityPM.IncludePickUp != newValue) {
            this.EntityPM.IncludePickUp = newValue;
            this.UpdatePickUpAddressFields();
        }
    }

    get PickUpAddressId() { return this.EntityPM.PickUpAddressId; }
    set PickUpAddressId(newValue: string) {
        if (this.EntityPM.PickUpAddressId != newValue) {
            this.EntityPM.PickUpAddressId = newValue;

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.FromAddressCity = null;
                this.FromAddressZipCode = null;
                this.FromAddressCountryId = null;
            }

            this.LoadPickupAddress();
            this.SetUIProperties_PickupFields();
        }
    }

    get FromAddressZipCode() { return this.EntityPM.FromAddressZipCode; }
    set FromAddressZipCode(newValue: string) {
        if (this.EntityPM.FromAddressZipCode != newValue) {
            this.EntityPM.FromAddressZipCode = newValue;
            this.SetUIProperties_PickupFields();
        }
    }

    get FromAddressCity() { return this.EntityPM.FromAddressCity; }
    set FromAddressCity(newValue: string) {
        if (this.EntityPM.FromAddressCity != newValue) {
            this.EntityPM.FromAddressCity = newValue;
            this.SetUIProperties_PickupFields();
        }
    }

    get FromAddressCountryId() { return this.EntityPM.FromAddressCountryId; }
    set FromAddressCountryId(newValue: string) {
        if (this.EntityPM.FromAddressCountryId != newValue) {
            this.EntityPM.FromAddressCountryId = newValue;
            this.SetUIProperties_PickupFields();
        }
    }

    get ShipperId() { return this.EntityPM.ShipperId; }

    public PickupAddressList: AddressList;
    private LoadPickupAddress() {
        if (this.PickUpAddressId == null) {
            this.PickupAddressList = null;
        }

        else {
            this.myAddressListService.getSingle(this.PickUpAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.PickupAddressList = myResponse.Result;
                }
            });
        }
    }
    private UpdatePickUpAddressFields() {
        if (!this.IncludePickUp) {
            this.EntityPM.PickUpAddressId = null;
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.PickupAddressList = null;
        }

        else {
            this.PickupAddressList = null;
            this.EntityPM.PickUpAddressId = null;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipperPickAddressId)) {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperPickAddressId;
            }

            else {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperMainAddressId;
            }

            if (!AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }

            this.LoadPickupAddress();
        }

        this.SetUIProperties_PickupFields();
    }

    get IsEditPickUpAddressEnabled() {
        var myResult = false;

        if (this.IsQuoteEditEnabled) {
            if (this.IncludePickUp) {
                if (!AppTool.IsNullOrEmpty(this.ShipperId) && !AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }

    get IsAddPickUpAddressEnabled() {
        var myResult = false;

        if (this.IsQuoteEditEnabled) {
            if (this.IncludePickUp) {
                if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }

    public IsPickupAddressSubFieldsVisibile: boolean = false;
    private SetUIProperties_PickupFields() {
        var isCityRequired: boolean = false;
        var isCountryRequired: boolean = false;
        var isPickupAddressSubFieldsVisibile: boolean = false;

        if (this.IncludePickUp) {
            if (AppTool.IsNullOrEmpty(this.ShipperId)) {
                isPickupAddressSubFieldsVisibile = true;
            }

            else if (AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                isPickupAddressSubFieldsVisibile = true;
            }

            var validateFields: boolean = false;

            if (AppTool.IsNullOrEmpty(this.ShipperId)) {
                validateFields = true;
            }

            else if (!AppTool.IsNullOrEmpty(this.ShipperId) && AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                validateFields = true;
            }

            if (validateFields) {
                if (AppTool.IsNullOrEmpty(this.FromAddressCity) && AppTool.IsNullOrEmpty(this.FromAddressZipCode)) {
                    isCityRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.FromAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }

        this.IsPickupAddressSubFieldsVisibile = isPickupAddressSubFieldsVisibile;
        this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, isCountryRequired);
    }

    // Main Carriage
    public FromLabel: string;
    public ToLabel: string;
    public CarrierLabel: string;
    public CarrierDependencyProperty1: string;
    private SetLabels() {
        this.FromLabel = "Quote.S.NewQuote.From";
        this.ToLabel = "Quote.S.NewQuote.To";
        this.CarrierLabel = "Quote.S.NewQuote.Carrier";

        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.FromLabel = "Quote.S.NewQuote.Gateway";
                this.ToLabel = "Quote.S.NewQuote.Destination";
                this.CarrierLabel = "Quote.S.NewQuote.Airline";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }

            case "O": {
                this.FromLabel = "Quote.S.NewQuote.LoadingPort";
                this.ToLabel = "Quote.S.NewQuote.DischargePort";
                this.CarrierLabel = "Quote.S.NewQuote.Shippingline";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }

            case "I": {
                this.FromLabel = "Quote.S.NewQuote.From";
                this.ToLabel = "Quote.S.NewQuote.To";
                this.CarrierLabel = "Quote.S.NewQuote.Trucker";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }

    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(newValue: string) {
        if (this.EntityPM.FromPortId != newValue) {
            this.EntityPM.FromPortId = newValue;

            if (newValue == null) {
                this.EntityPM.FromCountryId = null;
            }

            else {
                var myService: PortListService = new PortListService();
                myService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var port: PortList = myResponse.Result;
                        if (port != null) {
                            this.EntityPM.FromCountryId = port.CountryId;
                        }                        
                    }
                });                
            }
        }
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(newValue: string) {
        if (this.EntityPM.ToPortId != newValue) {
            this.EntityPM.ToPortId = newValue;

            if (newValue == null) {
                this.EntityPM.ToCountryId = null;
            }

            else {
                var myService: PortListService = new PortListService();
                myService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var port: PortList = myResponse.Result;
                        if (port != null) {
                            this.EntityPM.ToCountryId = port.CountryId;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;
        }
    }
    
    // Delivery
    get IncludeDelivery() { return this.EntityPM.IncludeDelivery; }
    set IncludeDelivery(newValue: boolean) {
        if (this.EntityPM.IncludeDelivery != newValue) {
            this.EntityPM.IncludeDelivery = newValue;
            this.UpdateDeliveryAddressFields();
        }
    }

    get DeliveryAddressId() { return this.EntityPM.DeliveryAddressId; }
    set DeliveryAddressId(newValue: string) {
        if (this.EntityPM.DeliveryAddressId != newValue) {
            this.EntityPM.DeliveryAddressId = newValue;

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.ToAddressCity = null;
                this.ToAddressZipCode = null;
                this.ToAddressCountryId = null;
            }

            this.LoadDeliveryAddress();
            this.SetUIProperties_DeliveryFields();
        }
    }

    get ToAddressZipCode() { return this.EntityPM.ToAddressZipCode; }
    set ToAddressZipCode(newValue: string) {
        if (this.EntityPM.ToAddressZipCode != newValue) {
            this.EntityPM.ToAddressZipCode = newValue;
            this.SetUIProperties_DeliveryFields();
        }
    }

    get ToAddressCity() { return this.EntityPM.ToAddressCity; }
    set ToAddressCity(newValue: string) {
        if (this.EntityPM.ToAddressCity != newValue) {
            this.EntityPM.ToAddressCity = newValue;
            this.SetUIProperties_DeliveryFields();
        }
    }

    get ToAddressCountryId() { return this.EntityPM.ToAddressCountryId; }
    set ToAddressCountryId(newValue: string) {
        if (this.EntityPM.ToAddressCountryId != newValue) {
            this.EntityPM.ToAddressCountryId = newValue;
            this.SetUIProperties_DeliveryFields();
        }
    }

    get ConsigneeId() { return this.EntityPM.ConsigneeId; }

    public DeliveryAddressList: AddressList;
    private LoadDeliveryAddress() {
        if (this.DeliveryAddressId == null) {
            this.DeliveryAddressList = null;
        }

        else {
            this.myAddressListService.getSingle(this.DeliveryAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.DeliveryAddressList = myResponse.Result;
                }
            });
        }
    }
    private UpdateDeliveryAddressFields() {
        if (!this.IncludeDelivery) {
            this.EntityPM.DeliveryAddressId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.DeliveryAddressList = null;
        }

        else {
            this.DeliveryAddressList = null;
            this.EntityPM.DeliveryAddressId = null;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePickAddressId)) {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneePickAddressId;
            }

            else {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneeMainAddressId;
            }

            if (!AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }

            this.LoadDeliveryAddress();
        }

        this.SetUIProperties_DeliveryFields();
    }

    get IsEditDeliveryAddressEnabled() {
        var myResult = false;

        if (this.IsQuoteEditEnabled) {
            if (this.IncludeDelivery) {
                if (!AppTool.IsNullOrEmpty(this.ConsigneeId) && !AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }

    get IsAddDeliveryAddressEnabled() {
        var myResult = false;

        if (this.IsQuoteEditEnabled) {
            if (this.IncludeDelivery) {
                if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }

    public IsDeliveryAddressSubFieldsVisibile: boolean = false;
    private SetUIProperties_DeliveryFields() {
        var isCityRequired: boolean = false;
        var isCountryRequired: boolean = false;
        var isDeliveryAddressSubFieldsVisibile: boolean = false;

        if (this.IncludeDelivery) {
            if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isDeliveryAddressSubFieldsVisibile = true;
            }

            else if (AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                isDeliveryAddressSubFieldsVisibile = true;
            }

            var validateFields: boolean = false;

            if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                validateFields = true;
            }

            else if (!AppTool.IsNullOrEmpty(this.ConsigneeId) && AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                validateFields = true;
            }

            if (validateFields) {
                if (AppTool.IsNullOrEmpty(this.ToAddressCity) && AppTool.IsNullOrEmpty(this.ToAddressZipCode)) {
                    isCityRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.ToAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }

        this.IsDeliveryAddressSubFieldsVisibile = isDeliveryAddressSubFieldsVisibile;
        this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, isCountryRequired);
    }

    // Commands
    SelectCityClicked(selectCityTypeCode: string) {

        var mySourceCountryId: string = selectCityTypeCode == "P" ? this.FromAddressCountryId : this.ToAddressCountryId;

        var args = new CitySelectionArgs(this.ToAddressCountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                if (selectCityTypeCode == "P") {
                    this.FromAddressCity = args.CityName;
                    this.FromAddressCountryId = args.CountryId;
                }

                else {
                    this.ToAddressCity = args.CityName;
                    this.ToAddressCountryId = args.CountryId;
                }
            }
        });
    }

    EditAddressClicked(myAddressCode: string) {
        var myAddressId: string = null;
        var myPartnerId: string = null;

        switch (myAddressCode) {
            case "P": {
                if (this.IncludePickUp) {
                    myPartnerId = this.ShipperId;
                    myAddressId = this.PickUpAddressId;                    
                }

                break;
            }

            case "D": {
                if (this.IncludeDelivery) {
                    myPartnerId = this.ConsigneeId;
                    myAddressId = this.DeliveryAddressId;                    
                }

                break;
            }
        }

        if (!AppTool.IsNullOrEmpty(myAddressId) && !AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = TextCodeTranslator.Translate("Quote.S.Routings.EditAddress");
            logeWindow.WindowArgs = { EntityId: myAddressId, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            this.LoadPickupAddress();
                            break;
                        }

                        case "D": {
                            this.LoadDeliveryAddress();
                            break;
                        }
                    }
                }
            });
        }
    }
    AddAddressClicked(myAddressCode: string) {
        var entityPM: AddressPM = null;
        var myPartnerId: string = null;

        switch (myAddressCode) {
            case "P": {
                if (this.IncludePickUp) {
                    myPartnerId = this.ShipperId;

                    if (!AppTool.IsNullOrEmpty(myPartnerId)) {
                        entityPM = new AddressPM();
                        entityPM.Tenant = SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = myPartnerId;
                    }
                }

                break;
            }

            case "D": {
                if (this.IncludeDelivery) {
                    myPartnerId = this.ConsigneeId;

                    if (!AppTool.IsNullOrEmpty(myPartnerId)) {
                        entityPM = new AddressPM();
                        entityPM.Tenant = SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = myPartnerId;
                    }
                }

                break;
            }
        }

        if (entityPM != null && !AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = TextCodeTranslator.Translate("Quote.S.Routings.AddAddress");
            logeWindow.WindowArgs = { EntityPM: entityPM, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            this.PickUpAddressId = null;
                            this.PickUpAddressId = entityPM.Id;
                            break;
                        }

                        case "D": {
                            this.DeliveryAddressId = null;
                            this.DeliveryAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    }
}