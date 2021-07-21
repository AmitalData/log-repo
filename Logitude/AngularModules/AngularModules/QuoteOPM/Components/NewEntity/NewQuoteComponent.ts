import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPPM} from '../../EntityPMs/QuoteOPPM';
import {QuoteOPPMService} from '../../Services/StandardPMs/QuoteOPPMService';
import {NewQuoteComponentArgs} from '../../Args';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteUtilities} from '../../Utilities/QuoteUtilities';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AddressPM} from '../../../Common/EntityPMs/AddressPM';
import {PortList} from '../../../Common/EntityLists/PortList';
import {CardList} from '../../../Common/EntityLists/CardList';
import {AddressList} from '../../../Common/EntityLists/AddressList';
import {PortListService} from '../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../Common/Services/StandardLists/AddressListService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../Common/Args';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {QuoteOPValidator} from '../../Validators/QuoteOPValidator';
import {QuoteOPPMInitService} from '../../EntityPMInitServices/QuoteOPPMInitService';
import {QuoteOPSettingPM} from '../../EntityPMs/QuoteOPSettingPM';
import {QuoteDomainService} from '../../Services/QuoteDomainService';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ChildDirective } from '../../../Infrastructure/Directives/ChildDirective';
import { ContactInputTemplateArgs } from '../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import { ShipmentSubTypeListService } from '../../../Shipment/services/standardlists/shipmentsubtypelistservice';
import { ShipmentSubTypeList } from '../../../Shipment/EntityLists/ShipmentSubTypeList';
import { FeatureToggleList } from '../../../Infrastructure/EntityLists/FeatureToggleList';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';

@Component({
    templateUrl: './NewQuoteComponent.html',
})

export class NewQuoteComponent extends BaseComponent implements OnInit, AfterViewInit {
    public EntityPM: QuoteOPPM;
    public DataContext: NewQuoteComponent = this;
    public ObjectTableName: string = "Quote";
    public LabelColumnWidth: number = 120;
    public ControlColumnWidth: number = 220;
    public CardDependencyProperty1: string = "CS,PO";
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsInlandDomestic: boolean = false;
    public SessionIndex: number;
    public QuoteSetting: QuoteOPSettingPM = null;
    public ValidationErrorsList: string[];
    public IsAddAgentVisible: boolean = false;
    private isConfirmCloseClicked: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChild(ChildDirective) Child: ChildDirective;
    constructor() {
        super();
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.InitializeServices();        
        this.InitializeAllowAgentInCustomersLOVFilters();
    }

    private myPortListService: PortListService;
    private myQuoteOPPMService: QuoteOPPMService;
    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    private myPartnersDomainService: PartnersDomainService;
    private myShipmentSubTypeListService: ShipmentSubTypeListService;
    private InitializeServices() {
        this.myPortListService = new PortListService();
        this.myQuoteOPPMService = new QuoteOPPMService();
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.myShipmentSubTypeListService = new ShipmentSubTypeListService();
    }

    private InitializeAllowAgentInCustomersLOVFilters() {
        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,PO,AG";
            this.IsAddAgentVisible = true;
        }
    }

    public ScreenIsReady: boolean = false;
    public SubTypeFeatureToggle: FeatureToggleList;
    ngOnInit() {
        var listservice: EntityListService = new EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then((res: any) => {
            res.subscribe((resp: any) => {
                this.CreateNewQuote();
                this.SubTypeFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SUB")[0]; 
                this.ScreenIsReady = true;
                this.BuildFiltersLists();
                this.OnFiltersChanged();
                this.LoadAllowedAirline();
                this.LoadShipmentSubTypes();
                this.GetQuoteSetting();
                this.SetCreateButtonText();
            });
        });
        this.InitalizeFeatureOfClosedAutomatically();
    }

    private GeneratedComponent: any;
    ngAfterViewInit() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.Child.Location)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = "NewQuote";
                cmpRef.instance.LabelWidth = 110;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
            });

    }

    private CreateNewQuote() {
        if (this.EntityPM == null) {
            this.EntityPM = this.myQuoteOPPMService.GetNewEntityPM();
            QuoteOPPMInitService.InitValues(this.EntityPM, true);
        }
    }

    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsScreenEnabled);
        }
    }

    LoadAllowedAirline() {
        if (SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (this.EntityPM.TransportModeId == "A") {
                    this.myPartnersDomainService.GetAllowedAirlineId().subscribe((myResponse: ServiceResponse) => {
                        if (myResponse != null) {
                            if (myResponse.HasError) {
                                this.ValidationErrorsList = myResponse.ErrorsArray;
                            }

                            else {
                                var allowedAirlineId: string = myResponse.Result;
                                if (!AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                    this.MainCarriageCarrierId = allowedAirlineId;
                                }
                            }
                        }
                    });
                }
            }
        }
    }

    private allShipmentSubTypes: ShipmentSubTypeList[] = [];
    LoadShipmentSubTypes() {
        this.allShipmentSubTypes = [];

        this.myShipmentSubTypeListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allShipmentSubTypes = myResponse.Result;
                this.allShipmentSubTypes = this.allShipmentSubTypes.filter(d => !d.Inactive);
            }
        });
    }

    public sourceEntityPM: QuoteOPPM;
    public IsCopyFromQuote: boolean = false;
    public DefaultCustomerId: string = null;
    SetWindowArgs(args: NewQuoteComponentArgs) {
        if (args.ConvertTransportMode) {
            this.EntityPM = args.Quote;
            this.ConvertTransportMode = args.ConvertTransportMode; 
        }

        else {
            if (args.OpportunityId && !this.EntityPM) {
                this.CreateNewQuote();
            }

            this.sourceEntityPM = args.Quote;
            this.IsCopyFromQuote = args.IsCopyFromQuote;
            this.DefaultCustomerId = args.DefaultCustomerId;
            this.OpportunityId = args.OpportunityId;
            this.IsCreatedFromTicket = args.IsCreatedFromTicket;
            this.TicketCreateDate = args.TicketCreateDate;
        }

        this.BuildFiltersLists();
        this.SetUIProperties();
        this.Clone();
    }

    private GetQuoteSetting() {
        var quoteDomainService = new QuoteDomainService();
        quoteDomainService.GetQuoteSettings().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError == false) {
                if (myResponse.Result) {
                    if (myResponse.Result.Id) {
                        this.QuoteSetting = myResponse.Result;
                        this.ExpirationDays = this.QuoteSetting.QuoteExpirationDays;
                    }

                    if (this.IsCopyFromQuote) {
                        this.InitializeCopy(this.sourceEntityPM);
                    }
                }

                this.IsAutomaticallyClosed = true;                
            }
        });
    }

    public ScreenOpacity: number = 0.7;
    public IsScreenEnabled: boolean = false;
    public IsAddConsigneeEnabled: boolean;
    public IsAddShipperEnabled: boolean;
    public QuoteTypeIsEnabled: boolean = false;
    public IsFillDimensionsEnabled: boolean = false;
    SetScreenEnabled() {
        var isScreenEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.DirectionId) && !AppTool.IsNullOrEmpty(this.TransportModeId)) {
            if (this.TransportModeId == "A") {
                isScreenEnabled = true;
            }

            else if (!AppTool.IsNullOrEmpty(this.ShipmentTypeId)) {
                isScreenEnabled = true;
            }
        }

        this.ScreenOpacity = isScreenEnabled ? 1 : 0.7;

        if (isScreenEnabled) {
            if (this.IsCopyFromQuote) {
                this.QuoteTypeIsEnabled = false;
            }
            else {
                this.QuoteTypeIsEnabled = true;
            }
        }
        this.IsScreenEnabled = isScreenEnabled;
        this.IsAddConsigneeEnabled = this.IsScreenEnabled;
        this.IsAddShipperEnabled = this.IsScreenEnabled;

        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);

        if (!AppTool.IsNullOrEmpty(this.DefaultCustomerId)) {
            if (!AppTool.IsNullOrEmpty(this.ShipperId) && isScreenEnabled) {
                this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, false);
                this.IsAddShipperEnabled = false;
            }
            if (!AppTool.IsNullOrEmpty(this.ConsigneeId) && isScreenEnabled) {
                this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, false);
                this.IsAddConsigneeEnabled = false;
            }
        }

        // Shipper
        this.UIProperties.SetEnabled("ShipperAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference2", this.ObjectTableName, isScreenEnabled);

        // Consignee
        this.UIProperties.SetEnabled("ConsigneeAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference2", this.ObjectTableName, isScreenEnabled);

        //Customer
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("QuoteCustomerTypeCode", this.ObjectTableName, isScreenEnabled);

        // General
        this.UIProperties.SetEnabled("IncotermId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MoveTypeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpirationDays", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpirationDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("StartDate", this.ObjectTableName, isScreenEnabled);

        this.UIProperties.SetEnabled("IsAutomaticallyClosed", this.ObjectTableName, isScreenEnabled && this.IsQuoteClosedAutomaticallyEnabled);

        // Pickup
        this.UIProperties.SetEnabled("IncludePickUp", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PickUpAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressZipCode", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, isScreenEnabled);

        // Routing
        var isPortsEnabled = false;
        if (isScreenEnabled) {

            if (this.IsCopyFromQuote) {
                if (this.QuoteSetting) {
                    if (this.QuoteSetting.EditMainCarriage) {
                        isPortsEnabled = true;
                    }
                }
            }

            else {
                isPortsEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isScreenEnabled);

        // Delivery
        this.UIProperties.SetEnabled("IncludeDelivery", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("DeliveryAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressZipCode", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, isScreenEnabled);

        // Expected Order
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType1Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType2Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType3Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType4Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType5Quantity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isScreenEnabled);

        // Additional Fields
        this.SetUIProperties_GeneratedComponent();
        this.SetUIProperties_Dimentions();
    }
    OnFiltersChanged() {
        this.SetScreenEnabled();
        this.SetUIProperties();
        this.SetLabels();
        this.SetUnits();
        this.SetPartners();
    }
    SetUIProperties() {
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Customer();
        this.SetUIProperties_Domestic();
        this.SetUIProperties_Containers();
        this.SetUIProperties_NumberOfPackages();
        this.SetUIProperties_AutomaticallyClosed();
        this.SetUIProperties_Dimentions();
    }
    private SetUIProperties_Shipper() {
        var isShipperFieldsVisible = !AppTool.IsNullOrEmpty(this.ShipperId);
        this.UIProperties.SetVisibility("ShipperContactId", this.ObjectTableName, isShipperFieldsVisible);
        this.UIProperties.SetVisibility("ShipperReference1", this.ObjectTableName, isShipperFieldsVisible);
        this.UIProperties.SetVisibility("ShipperReference2", this.ObjectTableName, isShipperFieldsVisible);
    }
    private SetUIProperties_Consignee() {
        var isConsigneeFieldsVisible = !AppTool.IsNullOrEmpty(this.ConsigneeId);
        this.UIProperties.SetVisibility("ConsigneeContactId", this.ObjectTableName, isConsigneeFieldsVisible);
        this.UIProperties.SetVisibility("ConsigneeReference1", this.ObjectTableName, isConsigneeFieldsVisible);
        this.UIProperties.SetVisibility("ConsigneeReference2", this.ObjectTableName, isConsigneeFieldsVisible);
    }
    SetUIProperties_Customer() {
        //var isFieldEnabled: boolean = true;

        //if (this.ShipmentCustomerTypeCode == "SHI") {
        //    if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
        //        isFieldEnabled = false;
        //    }
        //}

        //else if (this.ShipmentCustomerTypeCode == "CON") {
        //    if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
        //        isFieldEnabled = false;
        //    }
        //}

        //this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isFieldEnabled);
    }
    private SetUIProperties_Domestic() {
        this.IsInlandDomestic = QuoteUtilities.IsInlandDomestic(this.EntityPM);

        var fromPortIsrequired = !this.IsInlandDomestic && AppTool.IsNullOrEmpty(this.FromPortId);
        var toPortIsrequired = !this.IsInlandDomestic && AppTool.IsNullOrEmpty(this.ToPortId);

        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, fromPortIsrequired);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, toPortIsrequired);

        if (this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.ShipperId)) {
                this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, false);
            }

            if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, false);
            }

            this.EntityPM.FromPortId = null;
            this.EntityPM.ToPortId = null;
            this.CardDependencyProperty1 = this.CardDependencyProperty1 + ",WH";
            this.CustomerDependencyProperty1 = ['SHI', 'CON'].includes(this.QuoteCustomerTypeCode) ? this.CustomerDependencyProperty1 + ",WH" : this.CustomerDependencyProperty1;
            this.CustomerDependencyProperty1IsList = ['SHI', 'CON'].includes(this.QuoteCustomerTypeCode) ? true : this.CustomerDependencyProperty1IsList ; 

        }

        else {
            this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_Containers() {
        if (this.EntityPM.QuoteTypeCode == "P") {
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, true);

            this.EntityPM.PackageType1Quantity = null;
            this.EntityPM.PackageType2Quantity = null;
            this.EntityPM.PackageType3Quantity = null;
            this.EntityPM.PackageType4Quantity = null;
            this.EntityPM.PackageType5Quantity = null;
        }

        else {
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, this.PackageType1Quantity > 0);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, this.PackageType2Quantity > 0);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, this.PackageType3Quantity > 0);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, this.PackageType4Quantity > 0);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, this.PackageType5Quantity > 0);

            if (AppTool.IsNullOrZero(this.PackageType1Quantity)) {
                this.PackageType1Id = null;
            }

            if (AppTool.IsNullOrZero(this.PackageType2Quantity)) {
                this.PackageType2Id = null;
            }

            if (AppTool.IsNullOrZero(this.PackageType3Quantity)) {
                this.PackageType3Id = null;
            }

            if (AppTool.IsNullOrZero(this.PackageType4Quantity)) {
                this.PackageType4Id = null;
            }

            if (AppTool.IsNullOrZero(this.PackageType5Quantity)) {
                this.PackageType5Id = null;
            }
        }
    }
    private SetUIProperties_AutomaticallyClosed() {
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, this.IsAutomaticallyClosed && this.IsQuoteClosedAutomaticallyEnabled);
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, this.IsAutomaticallyClosed && this.IsQuoteClosedAutomaticallyEnabled);
    }

    public IsQuoteClosedAutomaticallyEnabled: boolean;
    private InitalizeFeatureOfClosedAutomatically() {
        this.IsQuoteClosedAutomaticallyEnabled = FeatureLocator.HasFeaturePermession("Quote", "QuoteClosedAutomatically");
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, this.IsQuoteClosedAutomaticallyEnabled);
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, this.IsQuoteClosedAutomaticallyEnabled);
        this.UIProperties.SetEnabled("IsAutomaticallyClosed", this.ObjectTableName, this.IsQuoteClosedAutomaticallyEnabled);
        this.SetUIProperties_AutomaticallyClosed();
    }

    private SetUIProperties_Dimentions() {
        var fillDimEnabled: boolean = false;

        if (this.IsAdhoc) {
            fillDimEnabled = this.IsScreenEnabled;

            var isFieldsEnabled = this.EntityPM.QuotePackages.length == 0;
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldsEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldsEnabled);
            this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isFieldsEnabled);
            this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isFieldsEnabled);
        }

        else {

            fillDimEnabled = false;

            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, false);
        }

        this.IsFillDimensionsEnabled = fillDimEnabled;
    }
    private SetUIProperties_NumberOfPackages() {
        if (this.EntityPM.TransportModeId != null) {
            if (this.EntityPM.TransportModeId.toUpperCase() == "A") {
                this.UIProperties.SetVisibility("NumberOfPackages", this.ObjectTableName, true);
            }

            else {
                if (this.EntityPM.ShipmentTypeId != null) {
                    if (this.EntityPM.TransportModeId.toUpperCase() == "A"
                        ||
                        (this.EntityPM.TransportModeId.toUpperCase() == "O" && this.EntityPM.ShipmentTypeId.toUpperCase() == "LCLD")
                        ||
                        (this.EntityPM.TransportModeId.toUpperCase() == "I" && this.EntityPM.ShipmentTypeId.toUpperCase() == "LTL")) {
                        this.UIProperties.SetVisibility("NumberOfPackages", this.ObjectTableName, true);
                    }
                }
            }
        }
    }

    // Filters
    public DirectionsList: FilterClass[] = [];
    public TransportModesList: FilterClass[] = [];
    public ShipmentTypesList: FilterClass[] = [];
    public ShipmentSubTypesList: FilterClass[] = [];
    BuildFiltersLists() {
        this.DirectionsList = [];
        this.TransportModesList = [];

        var direct_E = new FilterClass("E", "Export");
        var direct_I = new FilterClass("I", "Import");
        var direct_D = new FilterClass("D", "Domestic");
        var direct_R = new FilterClass("R", "Drop");

        var transport_A = new FilterClass("A", "Air");
        var transport_O = new FilterClass("O", "Ocean");
        var transport_I = new FilterClass("I", "Inland");

        this.DirectionsList.push(direct_E);
        this.DirectionsList.push(direct_I);
        this.DirectionsList.push(direct_D);
        this.DirectionsList.push(direct_R);

        this.TransportModesList.push(transport_A);
        this.TransportModesList.push(transport_O);
        this.TransportModesList.push(transport_I);

        this.BuildShipmentTypes();
    }
    BuildShipmentTypes() {
        this.ShipmentTypesList = [];

        if (this.DirectionId && this.TransportModeId) {
            switch (this.TransportModeId) {
                case "O": {
                    this.ShipmentTypesList.push(new FilterClass("FCLD", "FCL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LCLD", "LCL", "./Images/CellIcons/Package.png"));
                    break;
                }

                case "I": {
                    this.ShipmentTypesList.push(new FilterClass("FTL", "FTL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LTL", "LTL", "./Images/CellIcons/Package.png"));
                    break;
                }
            }
        }

        if (this.EntityPM.IsCopy) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.ShipmentTypeId)) {
                this.ShipmentTypeName = null;
            }

            else {
                var item = this.ShipmentTypesList.filter(f => f.Code == this.EntityPM.ShipmentTypeId)[0];
                if (item) {
                    this.ShipmentTypeName = item.Name;
                }
            }
        }
    }
    BuildShipmentSubTypes() {
        this.ShipmentSubTypesList = [];

        this.allShipmentSubTypes.filter(d => d.ShipmentTypeCode == this.ShipmentTypeId).forEach(item => {
            this.ShipmentSubTypesList.push(new FilterClass(item.Id, item.Name));
        });
        this.SetDefaultSubType();
    }
    private SetDefaultSubType() {
        var subTypeCode: string = null;
        switch (this.ShipmentTypeId) {
            case "Air":
                {
                    subTypeCode = "Air";
                    break;
                }

            case "FCLD":
                {
                    subTypeCode = "FCL";
                    break;
                }

            case "LCLD":
                {
                    subTypeCode = "LCL";
                    break;
                }

            case "FTL":
                {
                    subTypeCode = "FTL";
                    break;
                }

            case "LTL":
                {
                    subTypeCode = "LTL";
                    break;
                }
        }

        var subType: ShipmentSubTypeList = this.allShipmentSubTypes.filter(d => d.Code == subTypeCode)[0];
        if (subType) {
            this.ShipmentSubTypeId = subType.Id;
        }

        else {
            var defaultSubType: ShipmentSubTypeList = this.allShipmentSubTypes.filter(d => d.ShipmentTypeCode == this.ShipmentTypeId)[0];             
            if (defaultSubType) {
                this.ShipmentSubTypeId = defaultSubType.Id;
            }
        }
    }

    get TicketCreateDate() { return this.EntityPM.TicketCreateDate; }
    set TicketCreateDate(value: Date) {
        if (this.EntityPM.TicketCreateDate != value)
            this.EntityPM.TicketCreateDate = value;
    }

    get IsCreatedFromTicket() { return this.EntityPM.IsCreatedFromTicket; }
    set IsCreatedFromTicket(value: boolean) {
        if (this.EntityPM.IsCreatedFromTicket != value)
            this.EntityPM.IsCreatedFromTicket = value;
    }

    get OpportunityId() { return this.EntityPM.OpportunityId; }
    set OpportunityId(value: string) {
        if (this.EntityPM.OpportunityId != value)
            this.EntityPM.OpportunityId = value;
    }
    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId != newValue) {
            this.EntityPM.DirectionId = newValue;

            this.OnFiltersChanged();
        }
    }

    private convertTransportMode: boolean = false;
    get ConvertTransportMode() { return this.convertTransportMode; }
    set ConvertTransportMode(newValue: boolean) {
        if (this.convertTransportMode != newValue) {
            this.convertTransportMode = newValue;
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;

            this.FromPortId = null;
            this.ToPortId = null;
            this.MainCarriageCarrierId = null;

            if (newValue == "A") {
                this.ShipmentTypeId = "Air";
            }

            else {
                this.ShipmentTypeId = null;
            }

            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = !this.IsLCLEntity;

            //this.DelOrderDetails();
            this.OnFiltersChanged();
            this.BuildShipmentTypes();
            this.BuildShipmentSubTypes();
            this.LoadAllowedAirline();
        }
    }

    public ShipmentTypeName: string = null;
    get ShipmentTypeId() { return this.EntityPM.ShipmentTypeId; }
    set ShipmentTypeId(newValue: string) {
        if (this.EntityPM.ShipmentTypeId != newValue) {
            this.EntityPM.ShipmentTypeId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipmentTypeName = null;
            }

            else {
                var item = this.ShipmentTypesList.filter(f => f.Code == newValue)[0];
                if (item) {
                    this.ShipmentTypeName = item.Name;
                }
            }

            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = !this.IsLCLEntity;

            this.BuildShipmentSubTypes();
            this.SetupOrderDetails();
            this.OnFiltersChanged();
        }
    }

    get ShipmentSubTypeId() { return this.EntityPM.ShipmentSubTypeId; }
    set ShipmentSubTypeId(newValue: string) {
        if (this.EntityPM.ShipmentSubTypeId != newValue) {
            this.EntityPM.ShipmentSubTypeId = newValue;
        }
    }

    // Shipper
    private isShipperMyCustomer: boolean = false;
    get IsShipperMyCustomer() { return this.isShipperMyCustomer; }
    set IsShipperMyCustomer(value: boolean) {
        if (this.isShipperMyCustomer != value) {
            this.isShipperMyCustomer = value;

            if (value) {
                if (this.QuoteCustomerTypeCode == "SHI") {
                    this.CustomerId = this.ShipperId;
                }
            }

            this.SetUIProperties_Shipper();
            this.SetUIProperties_Consignee();
        }
    }

    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;

            if (this.QuoteCustomerTypeCode == "SHI") {
                this.CustomerId = newValue;
            }

            if (this.IsCopyFromQuote) {
                if (AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipperCopyIsChecked = false;
                }
            }

            this.SetUIProperties_Shipper();
            this.SetUIProperties_Customer();
            this.SetUIProperties_Domestic();
            this.GetShipperCardData();
        }
    }

    private myShipperAddressList: AddressList;
    get ShipperAddressList() { return this.myShipperAddressList; }
    set ShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;

        if (newValue == null) {
            this.EntityPM.ShipperMainAddressId = null;
        }

        else {
            this.EntityPM.ShipperMainAddressId = newValue.Id;
        }

        this.UpdatePickUpAddressFields();
    }

    get ShipperNote() { return this.EntityPM.ShipperNote; }
    set ShipperNote(newValue: string) {
        if (this.EntityPM.ShipperNote != newValue) {
            this.EntityPM.ShipperNote = newValue;
        }
    }

    get ShipperContactId() { return this.EntityPM.ShipperContactId; }
    set ShipperContactId(newValue: string) {
        if (this.EntityPM.ShipperContactId != newValue) {
            this.EntityPM.ShipperContactId = newValue;

            if (this.QuoteCustomerTypeCode == "SHI") {
                this.EntityPM.CustomerContactId = newValue;
            }
        }
    }

    get ShipperReference1() { return this.EntityPM.ShipperReference1; }
    set ShipperReference1(newValue: string) {
        if (this.EntityPM.ShipperReference1 != newValue) {
            this.EntityPM.ShipperReference1 = newValue;
        }
    }

    get ShipperReference2() { return this.EntityPM.ShipperReference2; }
    set ShipperReference2(newValue: string) {
        if (this.EntityPM.ShipperReference2 != newValue) {
            this.EntityPM.ShipperReference2 = newValue;
        }
    }

    private GetShipperCardData() {
        if (AppTool.IsNullOrEmpty(this.ShipperId)) {
            this.ShipperNote = null;
            this.EntityPM.ShipperName = null;
            this.ShipperAddressList = null;
            this.EntityPM.ShipperContactId = null;
            this.EntityPM.ShipperMainAddressId = null;
            this.EntityPM.ShipperPickAddressId = null;
        }

        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe((myResult: any) => {

                var myCardList: CardList = myResult.Result;
                if (myCardList) {
                    this.EntityPM.ShipperName = myCardList.EnglishName;
                    this.ShipperNote = myCardList.Notes;
                    this.EntityPM.ShipperContactId = myCardList.PrimaryContactId;
                    this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                    this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;

                    this.myAddressListService.getSingle(myCardList.MainAddressId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            this.ShipperAddressList = myResponse.Result;
                        }
                    });
                }
            });
        }

        this.SetUIProperties_Shipper();
    }

    // Consignee
    private isConsigneeMyCustomer: boolean = false;
    get IsConsigneeMyCustomer() { return this.isConsigneeMyCustomer; }
    set IsConsigneeMyCustomer(value: boolean) {
        if (this.isConsigneeMyCustomer != value) {
            this.isConsigneeMyCustomer = value;

            if (value) {
                if (this.QuoteCustomerTypeCode == "CON") {
                    this.CustomerId = this.ConsigneeId;
                }
            }

            this.SetUIProperties_Shipper();
            this.SetUIProperties_Consignee();
        }
    }

    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;

            if (this.QuoteCustomerTypeCode == "CON") {
                this.CustomerId = newValue;
            }

            if (this.IsCopyFromQuote) {
                if (AppTool.IsNullOrEmpty(newValue)) {
                    this.ConsigneeCopyIsChecked = false;
                }
            }

            this.SetUIProperties_Consignee();
            this.SetUIProperties_Customer();
            this.SetUIProperties_Domestic();
            this.GetConsigneeCardData();
        }
    }

    private myConsigneeAddressList: AddressList;
    get ConsigneeAddressList() { return this.myConsigneeAddressList; }
    set ConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;

        if (newValue == null) {
            this.EntityPM.ConsigneeMainAddressId = null;
        }

        else {
            this.EntityPM.ConsigneeMainAddressId = newValue.Id;
        }

        this.UpdateDeliveryAddressFields();
    }

    get ConsigneeNote() { return this.EntityPM.ConsigneeNote; }
    set ConsigneeNote(newValue: string) {
        if (this.EntityPM.ConsigneeNote != newValue) {
            this.EntityPM.ConsigneeNote = newValue;
        }
    }

    get ConsigneeContactId() { return this.EntityPM.ConsigneeContactId; }
    set ConsigneeContactId(newValue: string) {
        if (this.EntityPM.ConsigneeContactId != newValue) {
            this.EntityPM.ConsigneeContactId = newValue;

            if (this.QuoteCustomerTypeCode == "CON") {
                this.EntityPM.CustomerContactId = newValue;
            }
        }
    }

    get ConsigneeReference1() { return this.EntityPM.ConsigneeReference1; }
    set ConsigneeReference1(newValue: string) {
        if (this.EntityPM.ConsigneeReference1 != newValue) {
            this.EntityPM.ConsigneeReference1 = newValue;
        }
    }

    get ConsigneeReference2() { return this.EntityPM.ConsigneeReference2; }
    set ConsigneeReference2(newValue: string) {
        if (this.EntityPM.ConsigneeReference2 != newValue) {
            this.EntityPM.ConsigneeReference2 = newValue;
        }
    }

    private GetConsigneeCardData() {
        if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            this.ConsigneeNote = null;
            this.EntityPM.ConsigneeName = null;
            this.ConsigneeAddressList = null;
            this.EntityPM.ConsigneeContactId = null;
            this.EntityPM.ConsigneeMainAddressId = null;
            this.EntityPM.ConsigneePickAddressId = null;
        }

        else {
            this.myCardListService.getSingle(this.ConsigneeId).subscribe((myResult: any) => {

                var myCardList: CardList = myResult.Result;
                if (myCardList) {
                    this.EntityPM.ConsigneeName = myCardList.EnglishName;
                    this.ConsigneeNote = myCardList.Notes;
                    this.EntityPM.ConsigneeContactId = myCardList.PrimaryContactId;
                    this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                    this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;

                    this.myAddressListService.getSingle(myCardList.MainAddressId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            this.ConsigneeAddressList = myResponse.Result;
                        }
                    });
                }
            });
        }

        this.SetUIProperties_Consignee();
    }

    //Customer
    public CustomerDependencyProperty1: string = "CS";
    public CustomerDependencyProperty1IsList: boolean = false;
    private ComputeCustomerDependency() {
        switch (this.QuoteCustomerTypeCode) {
            case "SHI":
                this.CustomerDependencyProperty1 = this.IsInlandDomestic ? "CS,WH" : "CS";
                this.CustomerDependencyProperty1IsList = this.IsInlandDomestic ? true : false;
                break;
            case "CON":
                {
                    this.CustomerDependencyProperty1 = this.IsInlandDomestic ? "CS,PO,WH":"CS,PO";
                    this.CustomerDependencyProperty1IsList = true;

                    if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                        this.CustomerDependencyProperty1 = this.IsInlandDomestic ? "CS,PO,AG,WH" : "CS,PO,AG";
                        this.CustomerDependencyProperty1IsList = true;
                    }

                    break;
                }

            case "AGT":
                {
                    if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                        this.CustomerDependencyProperty1 = "CS,AG";
                        this.CustomerDependencyProperty1IsList = true;
                    }

                    else {
                        this.CustomerDependencyProperty1 = "AG";
                        this.CustomerDependencyProperty1IsList = false;
                    }

                    break;
                }

            case "NOT":
                {
                    this.CustomerDependencyProperty1 = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }

            case "OTH": {
                this.CustomerDependencyProperty1 = "CS";
                this.CustomerDependencyProperty1IsList = false;
                break;
            }
        }
    }

    get QuoteCustomerTypeCode() { return this.EntityPM.QuoteCustomerTypeCode; }
    set QuoteCustomerTypeCode(newValue: string) {
        if (this.EntityPM.QuoteCustomerTypeCode != newValue) {
            this.EntityPM.QuoteCustomerTypeCode = newValue;

            this.CustomerId = null;

            this.SetCustomer(newValue);
            this.SetCustomerRequired();
            this.ComputeCustomerDependency();
            this.SetUIProperties_Customer();
        }
    }

    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;

            this.SetCustomerRequired();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.CustomerContactId = null;
                this.EntityPM.CustomerName = null;
                this.EntityPM.CustomerNote = null;
                this.CustomerAddressId = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.CustomerContactId = myCardList.PrimaryContactId;
                            this.EntityPM.CustomerName = myCardList.EnglishName;
                            this.EntityPM.CustomerNote = myCardList.Notes;
                            this.CustomerAddressId = myCardList.MainAddressId;
                            this.SetCustomePartner();
                        }
                    }
                });
            }
        }
    }

    private customerAddressId: string;
    get CustomerAddressId() { return this.customerAddressId; }
    set CustomerAddressId(newValue: string) {
        if (this.customerAddressId != newValue) {
            this.customerAddressId = newValue;
        }
    }

    get CustomerContactId() { return this.EntityPM.CustomerContactId; }
    set CustomerContactId(newValue: string) {
        if (this.EntityPM.CustomerContactId != newValue) {
            this.EntityPM.CustomerContactId = newValue;
        }
    }

    private SetCustomePartner() {
        //if (!this.IsCopyFromQuote) {
        switch (this.QuoteCustomerTypeCode) {
            case "SHI": {
                this.ShipperId = this.CustomerId;
                this.EntityPM.ShipperMainAddressId = this.CustomerAddressId;
                this.EntityPM.ShipperContactId = this.CustomerContactId;
                break;
            }

            case "CON": {
                this.ConsigneeId = this.CustomerId;
                this.EntityPM.ConsigneeMainAddressId = this.CustomerAddressId;
                this.EntityPM.ConsigneeContactId = this.CustomerContactId;
                break;
            }

            case "AGT": {
                this.EntityPM.AgentId = this.CustomerId;
                this.EntityPM.AgentAddressId = this.CustomerAddressId;
                this.EntityPM.AgentContactId = this.CustomerContactId;
                break;
            }

            case "NOT": {
                this.EntityPM.NotifyId = this.CustomerId;
                this.EntityPM.NotifyAddressId = this.CustomerAddressId;
                this.EntityPM.NotifyContactId = this.CustomerContactId;
                break;
            }

            case "OTH": {

                break;
            }
        }
        //}
    }

    public IsCustomerRequired: boolean = false;
    private SetCustomerRequired() {
        var isRequired: boolean = false

        if (AppTool.IsNullOrEmpty(this.CustomerId) || AppTool.IsNullOrEmpty(this.QuoteCustomerTypeCode)) {
            isRequired = true;
        }

        this.IsCustomerRequired = isRequired;
    }

    // General
    public RoutingHelpHeader: string = TextCodeTranslator.Translate("Quote.F.QuoteTypeCode");
    public RoutingHelpText: string = TextCodeTranslator.Translate("Quote.QuoteTypeCodeHelpText");

    SetQuoteType(newValue: string) {
        if (newValue == "A") {
            this.EntityPM.QuoteTypeCode = "A";
            this.IsAdhoc = true;
        }
        else {
            this.EntityPM.QuoteTypeCode = "P";
            this.IsRoutingRates = true;

            this.Volume = null;
            this.GrossWeight = null;
            this.EntityPM.VolumetricWeight = null;
            this.ChargeableWeight = null;
            this.NumberOfPackages = null;
        }

        this.SetUIProperties_Containers();
        this.SetUIProperties_Dimentions();
    }

    get QuoteTypeCode() { return this.EntityPM.QuoteTypeCode; }
    set QuoteTypeCode(newValue: string) {
        if (this.EntityPM.QuoteTypeCode != newValue) {
            this.EntityPM.QuoteTypeCode = newValue;
        }
    }

    get IsAdhoc() { return (this.EntityPM.QuoteTypeCode == "A" ? true : false); }
    set IsAdhoc(newValue) {
        if (newValue) {
            this.EntityPM.QuoteTypeCode = "A";
        }

        else {
            this.EntityPM.QuoteTypeCode = "P";
        }
    }

    get IsRoutingRates() { return (this.EntityPM.QuoteTypeCode == "P" ? true : false); }
    set IsRoutingRates(newValue) {
        if (newValue) {
            this.EntityPM.QuoteTypeCode = "P";
        }

        else {
            this.EntityPM.QuoteTypeCode = "A";
        }
    }

    get IncotermId() { return this.EntityPM.IncotermId; }
    set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;
        }
    }

    get MoveTypeId() { return this.EntityPM.MoveTypeId; }
    set MoveTypeId(newValue: string) {
        if (this.EntityPM.MoveTypeId != newValue) {
            this.EntityPM.MoveTypeId = newValue;
        }
    }

    get ExpirationDays() { return this.EntityPM.ExpirationDays; }
    set ExpirationDays(newValue: number) {
        if (this.EntityPM.ExpirationDays != newValue) {
            this.EntityPM.ExpirationDays = newValue;

            if (newValue == null) {
                this.ExpirationDate = null;
            }

            else {
                this.SetExpirationDate();
            }
        }
    }

    private SetExpirationDate() {
        if (this.EntityPM.ExpirationDays == null && this.EntityPM.ExpirationDate == null) { }
        else {
            var date = DateTool.AddDays(this.StartDate, this.ExpirationDays);
            if (date == null) {
                this.EntityPM.ExpirationDays = null;
            }
            else {
                if (this.ExpirationDate == null || (this.ExpirationDate != null && this.ExpirationDate.valueOf() != date.valueOf())) {
                    this.ExpirationDate = date;
                }
            }
        }
    }

    get ExpirationDate() { return this.EntityPM.ExpirationDate; }
    set ExpirationDate(newValue: Date) {
        if (this.EntityPM.ExpirationDate != newValue) {
            this.EntityPM.ExpirationDate = newValue;

            if (newValue == null) {
                this.EntityPM.ExpirationDays = null;
            }

            else {
                var days = this.GetDaysBetweenDates(newValue, this.StartDate);
                if (this.ExpirationDays != days) {
                    this.EntityPM.ExpirationDays = days;
                }
            }
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(newValue: Date) {
        if (this.EntityPM.StartDate != newValue) {
            this.EntityPM.StartDate = newValue;

            if (newValue == null) {
                this.EntityPM.ExpirationDays = null;
            }
            else {
                this.SetExpirationDate();
            }
        }
    }

    get IsAutomaticallyClosed() { return this.EntityPM.IsAutomaticallyClosed; }
    set IsAutomaticallyClosed(newValue: boolean) {
        if (this.EntityPM.IsAutomaticallyClosed != newValue) {
            this.EntityPM.IsAutomaticallyClosed = newValue;

            if (newValue) {
                var todayDate = DateTool.GetCurrentDateAsUtc();
                var closeDays = this.QuoteSetting != null ? this.QuoteSetting.AutomaticallyCloseDays : 30;
                this.EntityPM.AutomaticallyCloseDays = closeDays;
                this.EntityPM.AutomaticallyCloseDate = DateTool.AddDays(todayDate, closeDays);
            }

            else {
                this.EntityPM.AutomaticallyCloseDays = null;
                this.EntityPM.AutomaticallyCloseDate = null;
                this.EntityPM.QuoteClosingReasonCode = null;
                this.EntityPM.QuoteClosingReasonId = null;
            }

            this.SetUIProperties_AutomaticallyClosed();
        }
    }

    get AutomaticallyCloseDays() { return this.EntityPM.AutomaticallyCloseDays; }
    set AutomaticallyCloseDays(newValue: number) {
        if (this.EntityPM.AutomaticallyCloseDays != newValue) {
            this.EntityPM.AutomaticallyCloseDays = newValue;

            if (newValue == null) {
                this.EntityPM.AutomaticallyCloseDate = null;
            }

            else {
                var date = DateTool.GetDateByDay(newValue);

                if (this.AutomaticallyCloseDate == null) {
                    this.EntityPM.AutomaticallyCloseDate = date;
                }

                else {
                    if (this.AutomaticallyCloseDate.valueOf() != date.valueOf()) {
                        this.EntityPM.AutomaticallyCloseDate = date;
                    }
                }
            }
        }
    }

    get AutomaticallyCloseDate() { return this.EntityPM.AutomaticallyCloseDate; }
    set AutomaticallyCloseDate(newValue: Date) {
        if (this.EntityPM.AutomaticallyCloseDate != newValue) {
            this.EntityPM.AutomaticallyCloseDate = newValue;

            if (newValue == null) {
                this.EntityPM.AutomaticallyCloseDays = null;
            }

            else {
                var todayDate = DateTool.GetCurrentDateAsUtc();
                var days = this.GetDaysBetweenDates(newValue, todayDate);

                if (this.AutomaticallyCloseDays != days) {
                    this.EntityPM.AutomaticallyCloseDays = days;
                }
            }
        }
    }

    private GetDaysBetweenDates(date1: Date, date2: Date) {
        var myResult: number = 0;

        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {

                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);

                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = d1.getTime() - d2.getTime();
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                myResult = Daysdiff;
            }
        }

        return myResult;
    }
    private TruncateTime(date: Date) {
        var myResult: Date = null;

        if (date != null) {
            var myResult: Date = new Date(date.toString());
            myResult.setUTCHours(0);
            myResult.setUTCMinutes(0);
            myResult.setUTCSeconds(0);
        }

        return myResult;
    }

    // Pick up
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

        else if (this.IsCopyFromQuote && this.CopyPickUpIsChecked) {
            if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                this.PickUpAddressId = this.sourceEntityPM.PickUpAddressId;
            }

            if (AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = this.sourceEntityPM.FromAddressCity;
                this.EntityPM.FromAddressZipCode = this.sourceEntityPM.FromAddressZipCode;
                this.EntityPM.FromAddressCountryId = this.sourceEntityPM.FromAddressCountryId;
            }
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
    private SetUIProperties_PickupFields() {
        var isCityRequired: boolean = false;
        var isCountryRequired: boolean = false;

        if (this.IncludePickUp) {

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

        this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, isCountryRequired);
    }
    get IsEditPickUpAddressEnabled() {
        var myResult = false;

        if (this.IncludePickUp) {
            if (!AppTool.IsNullOrEmpty(this.ShipperId) && !AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                myResult = true;
            }
        }

        return myResult;
    }
    get IsAddPickUpAddressEnabled() {
        var myResult = false;

        if (this.IncludePickUp) {
            if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                myResult = true;
            }
        }

        return myResult;
    }

    // Main Carriage
    public FromPortList: PortList = null;
    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(value: string) {
        if (this.EntityPM.FromPortId != value) {
            this.EntityPM.FromPortId = value;
            this.SetUIProperties_Domestic();

            if (AppTool.IsNullOrEmpty(value)) {
                this.FromPortList = null;
            }

            else {
                this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.FromPortList = myResponse.Result;
                    }
                });
            }
        }
    }

    public ToPortList: PortList = null;
    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.EntityPM.ToPortId != value) {
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_Domestic();

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToPortList = null;
            }

            else {
                this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.ToPortList = myResponse.Result;
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

        else if (this.IsCopyFromQuote && this.CopyDeliveryIsChecked) {
            if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                this.DeliveryAddressId = this.sourceEntityPM.DeliveryAddressId;
            }

            if (AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = this.sourceEntityPM.ToAddressCity;
                this.EntityPM.ToAddressZipCode = this.sourceEntityPM.ToAddressZipCode;
                this.EntityPM.ToAddressCountryId = this.sourceEntityPM.ToAddressCountryId;
            }
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
    private SetUIProperties_DeliveryFields() {
        var isCityRequired: boolean = false;
        var isCountryRequired: boolean = false;

        if (this.IncludeDelivery) {

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

        this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, isCountryRequired);
    }

    get IsEditDeliveryAddressEnabled() {
        var myResult = false;

        if (this.IncludeDelivery) {
            if (!AppTool.IsNullOrEmpty(this.ConsigneeId) && !AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                myResult = true;
            }
        }

        return myResult;
    }
    get IsAddDeliveryAddressEnabled() {
        var myResult = false;

        if (this.IncludeDelivery) {
            if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                myResult = true;
            }
        }

        return myResult;
    }

    // Expected Order Details  
    get PackageType1Quantity() { return this.EntityPM.PackageType1Quantity; }
    set PackageType1Quantity(newValue: number) {
        if (this.EntityPM.PackageType1Quantity != newValue) {
            this.EntityPM.PackageType1Quantity = newValue;

            this.SetUIProperties_Containers();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType2Quantity() { return this.EntityPM.PackageType2Quantity; }
    set PackageType2Quantity(newValue: number) {
        if (this.EntityPM.PackageType2Quantity != newValue) {
            this.EntityPM.PackageType2Quantity = newValue;

            this.SetUIProperties_Containers();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType3Quantity() { return this.EntityPM.PackageType3Quantity; }
    set PackageType3Quantity(newValue: number) {
        if (this.EntityPM.PackageType3Quantity != newValue) {
            this.EntityPM.PackageType3Quantity = newValue;

            this.SetUIProperties_Containers();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType4Quantity() { return this.EntityPM.PackageType4Quantity; }
    set PackageType4Quantity(newValue: number) {
        if (this.EntityPM.PackageType4Quantity != newValue) {
            this.EntityPM.PackageType4Quantity = newValue;

            this.SetUIProperties_Containers();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType5Quantity() { return this.EntityPM.PackageType5Quantity; }
    set PackageType5Quantity(newValue: number) {
        if (this.EntityPM.PackageType5Quantity != newValue) {
            this.EntityPM.PackageType5Quantity = newValue;

            this.SetUIProperties_Containers();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType1Id() { return this.EntityPM.PackageType1Id; }
    set PackageType1Id(newValue: string) {
        if (this.EntityPM.PackageType1Id != newValue) {
            this.EntityPM.PackageType1Id = newValue;
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType2Id() { return this.EntityPM.PackageType2Id; }
    set PackageType2Id(newValue: string) {
        if (this.EntityPM.PackageType2Id != newValue) {
            this.EntityPM.PackageType2Id = newValue;
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType3Id() { return this.EntityPM.PackageType3Id; }
    set PackageType3Id(newValue: string) {
        if (this.EntityPM.PackageType3Id != newValue) {
            this.EntityPM.PackageType3Id = newValue;
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType4Id() { return this.EntityPM.PackageType4Id; }
    set PackageType4Id(newValue: string) {
        if (this.EntityPM.PackageType4Id != newValue) {
            this.EntityPM.PackageType4Id = newValue;
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType5Id() { return this.EntityPM.PackageType5Id; }
    set PackageType5Id(newValue: string) {
        if (this.EntityPM.PackageType5Id != newValue) {
            this.EntityPM.PackageType5Id = newValue;
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 2);
            this.ComputeChargeableWeight();
            //this.EntityPM.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 2);
            this.ComputeVolumetricWeight();
            //this.EntityPM.VolumetricWeight = QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(value: number) {
        if (this.EntityPM.VolumetricWeight != value) {
            this.EntityPM.VolumetricWeight = AppTool.Round(value, 2);
            this.ComputeChargeableWeight();
        }
    }

    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? null : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            var result = AppTool.Round(newValue, 2);
            this.EntityPM.ChargeableWeight = result;

            if (this.GrossWeight == null && this.EntityPM.VolumetricWeight == null) {
                this.EntityPM.VolumetricWeight = result;
                this.EntityPM.GrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.Ratio);
            }
        }
    }

    get PickupDeliveryVolumetricWeight() { return this.EntityPM.PickupDeliveryVolumetricWeight == null ? 0 : this.EntityPM.PickupDeliveryVolumetricWeight; }
    set PickupDeliveryVolumetricWeight(newValue: number) {
        if (this.EntityPM.PickupDeliveryVolumetricWeight != newValue) {
            this.EntityPM.PickupDeliveryVolumetricWeight = AppTool.Round(newValue, 3);

            if (this.EntityPM.QuotePackages.length == 0) {
                this.EntityPM.PickupDeliveryChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.EntityPM.PickupDeliveryVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }
    }

    get PickupDeliveryChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.PickupDeliveryChargeableWeight) ? null : this.EntityPM.PickupDeliveryChargeableWeight; }
    set PickupDeliveryChargeableWeight(newValue: number) {
        if (this.EntityPM.PickupDeliveryChargeableWeight != newValue) {
            var result = AppTool.Round(newValue, 2);
            this.EntityPM.PickupDeliveryChargeableWeight = result;

            if (this.GrossWeight == null && this.EntityPM.PickupDeliveryVolumetricWeight == null) {
                this.EntityPM.PickupDeliveryVolumetricWeight = result;
                this.EntityPM.GrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.Ratio);
            }
        }
    }


    ComputeChargeableWeight() {
        this.ChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        this.ComputePickupDeliveryChargeableWeight();
    }

    ComputePickupDeliveryChargeableWeight() {
        this.PickupDeliveryChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.PickupDeliveryVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }
    ComputeVolumetricWeight() {
        var myResult = null;

        if (this.Volume != null) {
            myResult = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.Volume, this.EntityPM.Ratio);
        }

        else if (this.GrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
        }

        this.VolumetricWeight = myResult;
        this.ComputePickupDeliveryVolumetricWeight();
    }

    ComputePickupDeliveryVolumetricWeight() {
        var myResult = null;

        if (this.EntityPM.Volume != null) {
            myResult = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.Volume, this.EntityPM.PickupDeliveryRatio);
        }

        else if (this.GrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
        }

        this.PickupDeliveryVolumetricWeight = myResult;
    }

    get NumberOfPackages() { return this.EntityPM.NumberOfPackages; }
    set NumberOfPackages(newValue: number) {
        if (this.EntityPM.NumberOfPackages != newValue) {
            this.EntityPM.NumberOfPackages = newValue;
        }
    }

    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
        }
    }

    FillDimensionsClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Quote.S.NewQuote.FillDimensions");
        logitudeWindow.Width = 850;
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show('./Quote/Components/NewEntity/QuoteDimensionsComponent');
        logitudeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.SetUIProperties_Dimentions();
            }
        });
    }

    // General Methods
    SetCustomer(myCode: string) {
        if (myCode == 'SHI') {
            this.IsShipperMyCustomer = true;
            this.IsConsigneeMyCustomer = false;
        }

        else if (myCode == 'CON') {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = true;
        }

        else {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = false;
        }
    }
    SelectCityClicked(selectCityTypeCode: string) {

        var mySourceCountryId: string = selectCityTypeCode == "P" ? this.FromAddressCountryId : this.ToAddressCountryId;

        var args = new CitySelectionArgs(mySourceCountryId);
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
                    myAddressId = this.PickUpAddressId;
                    myPartnerId = this.ShipperId;
                }

                break;
            }

            case "D": {
                if (this.IncludeDelivery) {
                    myAddressId = this.DeliveryAddressId;
                    myPartnerId = this.ConsigneeId;
                }

                break;
            }
        }

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId };
            logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            if (this.PickUpAddressId == this.EntityPM.ShipperMainAddressId) {
                                this.myAddressListService.getSingle(this.PickUpAddressId).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {
                                        this.ShipperAddressList = myResponse.Result;
                                    }
                                });
                            }

                            else {
                                this.PickUpAddressId = null;
                                this.PickUpAddressId = myAddressId;
                            }

                            break;
                        }

                        case "D": {
                            if (this.DeliveryAddressId == this.EntityPM.ConsigneeMainAddressId) {
                                this.myAddressListService.getSingle(this.DeliveryAddressId).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {
                                        this.ConsigneeAddressList = myResponse.Result;
                                    }
                                });
                            }

                            else {
                                this.DeliveryAddressId = null;
                                this.DeliveryAddressId = myAddressId;
                            }

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
                        entityPM.AddressTypeId = "P";
                        entityPM.CardId = this.ShipperId;
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
                        entityPM.AddressTypeId = "P";
                        entityPM.CardId = this.ConsigneeId;
                    }
                }

                break;
            }
        }

        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM };
            logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");
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

    public FromLabel: string;
    public ToLabel: string;
    public CarrierLabel: string;
    public CarrierDependencyProperty1: string;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    private SetLabels() {
        switch (this.TransportModeId) {
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

            default: {
                this.FromLabel = "Quote.S.NewQuote.From";
                this.ToLabel = "Quote.S.NewQuote.To";
                this.CarrierLabel = "Quote.S.NewQuote.Carrier";
                break;
            }
        }

        this.VolumeLabel = TextCodeTranslator.Translate("Quote.F.Volume").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Quote.F.GrossWeight").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }

        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    }
    private SetUnits() {
        if (!this.ConvertTransportMode) {
            var myDimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
            var myVolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
            var myGrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
            var myChargeableWeightUnitCode = AppTool.GetChargeableWeightUnitCode(this.EntityPM.TransportModeId);
            var myPickupDeliveryChargeableWeightUnitCode = AppTool.GetChargeableWeightUnitCode("O");
            if (this.EntityPM.DirectionId == "D") {
                if (!AppTool.IsNullOrEmpty(SessionLocator.TenantPM.CountryCode)) {
                    if (SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
                        myDimensionsUnitCode = "Inc";
                        myVolumeUnitCode = "CBI";
                        myGrossWeightUnitCode = "LB";
                        myChargeableWeightUnitCode = "LB";
                    }
                }
            }

            if (this.IsCopyFromQuote) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
                    this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                    this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.GrossWeightUnitCode)) {
                    this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
                }

                this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
                this.EntityPM.PickupDeliveryCWeightUnitCode = myPickupDeliveryChargeableWeightUnitCode;

                if (this.EntityPM.Ratio == null) {
                    this.EntityPM.Ratio = AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
                }


                if (this.EntityPM.PickupDeliveryRatio == null) {
                    this.EntityPM.PickupDeliveryRatio = AppTool.GetPickupDeliveryRatio(this.EntityPM.ShipmentTypeId);
                }

                if (this.EntityPM.DimFactor == null) {
                    this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
                }
            }

            else {
                this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
                this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
                this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
                this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
                this.EntityPM.Ratio = AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
                this.EntityPM.PickupDeliveryRatio = AppTool.GetPickupDeliveryRatio(this.EntityPM.ShipmentTypeId);
                this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
                this.EntityPM.VolumetricWeight = QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
                this.EntityPM.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
                this.EntityPM.PickupDeliveryCWeightUnitCode = myPickupDeliveryChargeableWeightUnitCode;
            }
        }
    }
    private SetPartners() {
        if (!this.ConvertTransportMode) {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = false;
            var myCRMCustomerId = null;

            var isTakenFromSourceEntity: boolean = false;
            if (this.IsCopyFromQuote) {
                if (this.sourceEntityPM) {
                    this.EntityPM.QuoteCustomerTypeCode = this.sourceEntityPM.QuoteCustomerTypeCode;
                    isTakenFromSourceEntity = true;
                }
            }

            if (isTakenFromSourceEntity) {
                if (this.EntityPM.QuoteCustomerTypeCode == "SHI") {
                    this.IsShipperMyCustomer = true;
                }

                if (this.EntityPM.QuoteCustomerTypeCode == "CON") {
                    this.IsConsigneeMyCustomer = true;
                }
            }

            else {
                switch (this.DirectionId) {
                    case "I": {
                        this.QuoteCustomerTypeCode = "CON";
                        this.IsConsigneeMyCustomer = true;

                        if (!AppTool.IsNullOrEmpty(this.DefaultCustomerId)) {
                            this.ConsigneeId = this.DefaultCustomerId;
                            if (this.ShipperId == this.DefaultCustomerId) {
                                this.ShipperId = null;
                            }
                        }

                        break;
                    }

                    default: {
                        this.QuoteCustomerTypeCode = "SHI";
                        this.IsShipperMyCustomer = true;

                        if (!AppTool.IsNullOrEmpty(this.DefaultCustomerId)) {
                            this.ShipperId = this.DefaultCustomerId;
                            if (this.ConsigneeId == this.DefaultCustomerId) {
                                this.ConsigneeId = null;
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
    private SetupOrderDetails() {
        this.PackageType1Quantity = null;
        this.PackageType2Quantity = null;
        this.PackageType3Quantity = null;
        this.PackageType4Quantity = null;
        this.PackageType5Quantity = null;
        this.PackageType1Id = null;
        this.PackageType2Id = null;
        this.PackageType3Id = null;
        this.PackageType4Id = null;
        this.PackageType5Id = null;
        this.Volume = null;
        this.GrossWeight = null;
        this.EntityPM.VolumetricWeight = null;
        this.ChargeableWeight = null;

        this.EntityPM.QuotePackages.forEach(item => {
            if (this.EntityPM.QuotePackages.indexOf(item) > -1) {
                this.EntityPM.RemoveQuoteOPPackage/*  RemoveQuotePackagePM*/(item);
            }
        });

        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = !this.IsLCLEntity;

        this.SetUIProperties_NumberOfPackages();
        this.SetUnits();
    }
    AddCustomerClicked(customerType: string) {
        var myComponentPath: string = null;
        var title = "";

        if (this.QuoteCustomerTypeCode == "AGT") {
            myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
        }

        else {
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        }

        if (customerType == "Customer") {
            title = "New " + this.ComputeAddCustomerTitle();
        }

        else {
            title = "New " + customerType;
        }

        var args = new NewEntityArgs();

        if (customerType == "Shipper") {
            if (!this.IsShipperMyCustomer) {
                args.Perspective = "ShippersAndConsignees";
            }
        }

        else if (customerType == "Consignee") {
            if (!this.IsConsigneeMyCustomer) {
                args.Perspective = "ShippersAndConsignees";
            }
        }

        else if (customerType == "Customer") {
            if (this.QuoteCustomerTypeCode == "SHI") {
                if (!this.IsShipperMyCustomer) {
                    args.Perspective = "ShippersAndConsignees";
                }
            }

            else if (this.QuoteCustomerTypeCode == "CON") {
                if (!this.IsConsigneeMyCustomer) {
                    args.Perspective = "ShippersAndConsignees";
                }
            }
        }

        var logWindow = new LogitudeWindow();
        logWindow.Title = title;
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Show(myComponentPath);

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (customerType == "Shipper") {
                        this.ShipperId = comp.EntityPM.Id;
                    }

                    else if (customerType == "Consignee") {
                        this.ConsigneeId = comp.EntityPM.Id;
                    }

                    else if (customerType == "Customer") {
                        this.CustomerId = comp.EntityPM.Id;
                    }
                }
            });
        });
    }
    private ComputeAddCustomerTitle(): string {
        var myResult: string = "";

        switch (this.QuoteCustomerTypeCode) {
            case "AGT":
                {
                    myResult = "Agent";
                    break
                }

            case "CON":
                {
                    myResult = "Consignee";
                    break
                }

            case "NOT":
                {
                    myResult = "Notify";
                    break
                }


            case "SHI":
                {
                    myResult = "Shipper";
                    break
                }
        }

        return myResult;
    }

    AddPotentialCustomerClicked(customerType: string) {
        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("Customer").subscribe(response2 => {
            _entityResourceService.getEntityResourceByTableName("Contact").subscribe(response2 => {
                var args = new NewEntityArgs();

                if (customerType == "Shipper") {
                    if (!this.IsShipperMyCustomer) {
                        args.Perspective = "ShippersAndConsignees";
                    }
                }

                else {
                    if (!this.IsConsigneeMyCustomer) {
                        args.Perspective = "ShippersAndConsignees";
                    }
                }

                var logWindow = new LogitudeWindow();
                logWindow.Title = "New Potential " + customerType;
                logWindow.Width = 990;
                logWindow.Height = 600;
                logWindow.WindowArgs = args;
                logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");

                logWindow.ComponentLoaded.subscribe(comp => {
                    logWindow.WindowClosed.subscribe(s => {
                        if (s) {

                            if (customerType == "Shipper") {
                                this.ShipperId = comp.EntityPM.Id;
                            }

                            else {
                                this.ConsigneeId = comp.EntityPM.Id;
                            }
                        }
                    });
                });
            });
        });
    }
    AddAgentClicked(customerType: string) {
        var args = new NewEntityArgs();

        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Agent";
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent");

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {

                    if (customerType == "Shipper") {
                        this.ShipperId = comp.EntityPM.Id;
                    }

                    else {
                        this.ConsigneeId = comp.EntityPM.Id;
                    }
                }
            });
        });
    }

    CancelButtonClicked() {
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = false;
            confirmWindow.YesButtonText = "Don't Save";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show("You are about to cancel Quote and all data will be lost");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CloseWizardWindow();
                }
                else if (confirmWindow.No) {
                    this.isConfirmCloseClicked = true;
                }
            });
        }

        else {
            this.CloseWizardWindow();
        }
    }
    private CloseWizardWindow() {
        if (this.ConvertTransportMode) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if (this.ConvertTransportMode) {
            this.ConvertQuoteTransportModeProcess();
        }
        else {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.SetDataOnFinish();
            var entityValidator: QuoteOPValidator = new QuoteOPValidator();
            this.ValidationErrorsList = entityValidator.Validate(this.EntityPM);

            if (this.ValidationErrorsList.length == 0) {
                this.InitializeCopy_Charges();
                this.SubmitCreatingNewQuote();
            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        }
    }
    private SetDataOnFinish() {

        this.EntityPM.IsCopyExchangeRates = this.CopyRatesIsChecked;

        if (this.EntityPM.ExchangeRate == null) {
            this.EntityPM.ExchangeRate = 1;
        }

        var computeNumberOfPackages = false;

        if (this.EntityPM.TransportModeId.toUpperCase() == "O" && this.EntityPM.ShipmentTypeId.toUpperCase() == "FCLD") {
            computeNumberOfPackages = true;
        }

        else if (this.EntityPM.TransportModeId.toUpperCase() == "I" && this.EntityPM.ShipmentTypeId.toUpperCase() == "FTL") {
            computeNumberOfPackages = true;
        }

        if (computeNumberOfPackages) {
            var sum = 0;
            if (this.EntityPM.PackageType1Quantity > 0) { sum = sum + (AppTool.IsNullOrEmpty(this.PackageType1Quantity) ? 0 : this.PackageType1Quantity); }
            if (this.EntityPM.PackageType2Quantity > 0) { sum = sum + (AppTool.IsNullOrEmpty(this.PackageType2Quantity) ? 0 : this.PackageType2Quantity) }
            if (this.EntityPM.PackageType3Quantity > 0) { sum = sum + (AppTool.IsNullOrEmpty(this.PackageType3Quantity) ? 0 : this.PackageType3Quantity) }
            if (this.EntityPM.PackageType4Quantity > 0) { sum = sum + (AppTool.IsNullOrEmpty(this.PackageType4Quantity) ? 0 : this.PackageType4Quantity) }
            if (this.EntityPM.PackageType5Quantity > 0) { sum = sum + (AppTool.IsNullOrEmpty(this.PackageType5Quantity) ? 0 : this.PackageType5Quantity) }

            if (this.IsLCLEntity) {
                this.EntityPM.NumberOfPackages = sum;
            }

            else {
                this.EntityPM.NumberOfContainers = sum;
            }
        }

        this.SetCustomerDataOnFinish();
        this.SetInlandDomesticOnFinish();
        this.SetPickupDeliveryOnFinish();
        this.SetCountryOnFinish();
    }
    private SetCustomerDataOnFinish() {
        this.SetCustomePartner();

        //if (AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
        //    this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.customerSalesmanId) ? this.EntityPM.CreatedByUserId : this.customerSalesmanId;
        //}

        //if (AppTool.IsNullOrEmpty(this.EntityPM.BusinessUnitId)) {
        //    this.EntityPM.BusinessUnitId = AppTool.IsNullOrEmpty(this.customerSalesmanBusinessUnitId) ? SessionLocator.LoggedUserPM.BusinessUnitId : this.customerSalesmanBusinessUnitId;
        //}

        //this.EntityPM.CustomerId = null;
        //this.EntityPM.CustomerName = null;
        //this.EntityPM.CustomerNote = null;
        //this.EntityPM.CustomerContactId = null;
        //this.EntityPM.CustomerReference1 = null;
        //this.EntityPM.CustomerReference2 = null;
        //this.EntityPM.QuoteCustomerTypeCode = null;

        //if (this.IsShipperMyCustomer) {
        //    this.EntityPM.QuoteCustomerTypeCode = "SHI";
        //    this.EntityPM.CustomerId = this.EntityPM.ShipperId;
        //    this.EntityPM.CustomerName = this.EntityPM.ShipperName;
        //    this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
        //    this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
        //    this.EntityPM.CustomerReference1 = this.EntityPM.ShipperReference1;
        //    this.EntityPM.CustomerReference2 = this.EntityPM.ShipperReference2;
        //    this.EntityPM.SalesmanUserId = this.shipperSalesmanId;
        //    this.EntityPM.BusinessUnitId = this.shipperSalesmanBusinessUnitId;
        //}

        //else {
        //    this.EntityPM.QuoteCustomerTypeCode = "CON";
        //    this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
        //    this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
        //    this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
        //    this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
        //    this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
        //    this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;
        //    this.EntityPM.SalesmanUserId = this.consigneeSalesmanId;
        //    this.EntityPM.BusinessUnitId = this.consigneeSalesmanBusinessUnitId;
        //}
    }
    private SetInlandDomesticOnFinish() {
        if (this.IsInlandDomestic) {
            this.IncludePickUp = false;
            this.IncludeDelivery = false;
            this.FromPortId = null;
            this.ToPortId = null;
            this.CardDependencyProperty1 = this.CardDependencyProperty1 + ",WH";
        }
    }
    private SetPickupDeliveryOnFinish() {
        if (this.IncludePickUp) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }
        }

        else {
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.EntityPM.PickUpAddressId = null;
        }


        if (this.IncludeDelivery) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }
        }

        else {
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.EntityPM.DeliveryAddressId = null;
        }
    }
    private SetCountryOnFinish() {
        if (this.IsInlandDomestic) {
            if (this.ShipperAddressList != null) {
                this.EntityPM.FromCountryId = this.ShipperAddressList.CountryId;
                this.EntityPM.FromCountryIsEC = this.ShipperAddressList.CountryEC;
            }

            if (this.ConsigneeAddressList != null) {
                this.EntityPM.ToCountryId = this.ConsigneeAddressList.CountryId;
                this.EntityPM.ToCountryIsEC = this.ConsigneeAddressList.CountryEC;
            }
        }

        else {
            if (this.FromPortList != null) {
                this.EntityPM.FromCountryId = this.FromPortList.CountryId;
                this.EntityPM.FromCountryIsEC = this.FromPortList.CountryEC;
            }

            if (this.ToPortList != null) {
                this.EntityPM.ToCountryId = this.ToPortList.CountryId;
                this.EntityPM.ToCountryIsEC = this.ToPortList.CountryEC;
            }
        }
    }

    private ConvertQuoteTransportModeProcess() {
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = false;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Ok");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
            confirmWindow.Title = "Convert Quote Transport Mode";
            confirmWindow.Show("Changing the quote transport mode will result in deleting all the quote packages & charges.");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    this.SubmitUpdateQuote();
                }
            });
        }
        else {
            this.CloseWizardWindow();
        }
    }

    private SubmitUpdateQuote() {
        var myService: QuoteOPPMService = new QuoteOPPMService();
        this.EntityPM.ConvertTransportMode = this.ConvertTransportMode;
        myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
        });
    }

    private SubmitCreatingNewQuote() {
        var myService: QuoteOPPMService = new QuoteOPPMService();
        myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.CurrentSession.CloseCurrentWindowEmit('OK');

                if (this.IsCopyFromQuote) {
                    this.RunInEditMode();
                }
            }
        });
    }
    private RunInEditMode() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.EntityPM.Id, ObjectTableName: "Quote", BackButtonLabel: "Quote: " + this.sourceEntityPM.QuoteNumber });

                let isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {
                        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                });

                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
    }

    AddContact(partnerId: string, type: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplateArgs();
        args.CustomerId = partnerId;
        args.CardDependencyProperty1 = this.CardDependencyProperty1;
        args.CustomerLable = type == "SH" ? "Shipper" : "Consignee";
        args.ComponentName = "Partners";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewContactWindowClosed($event, type));
    }

    OnNewContactWindowClosed(arg: any, type: string) {
        if (arg != 'cancel') {
            if (type == "SH") {
                this.ShipperContactId = arg;
            }
            else {
                this.ConsigneeContactId = arg;
            }
           
        }
    }
    //Copy Mode
    public ButtonContent: string = TextCodeTranslator.Translate("Quote.B.Create");
    public DirectionImageSRC: string;
    public TransportModeImageSRC: string;

    public IsCopyOtherPartnersVisible: boolean = false;
    ShipperCopyIsEnabled: boolean = false;
    ConsigneeCopyIsEnabled: boolean = false;
    AgentCopyIsEnabled: boolean = false;
    NotifyCopyIsEnabled: boolean = false;
    MainCarriageCopyIsEnabled: boolean = false;
    ChargesTypesCopyIsEnabled: boolean = false;
    CopyCostIsChecked: boolean = false;
    CopySaleIsChecked: boolean = false;
    CopyRatesIsChecked: boolean = false;
    InitializeCopy(myQuote: QuoteOPPM) {
        if (myQuote != null) {
            if (this.IsCopyFromQuote) {
                this.EntityPM.QuoteTemplateId = myQuote.QuoteTemplateId;

                if (!AppTool.IsNullOrEmpty(this.sourceEntityPM.AgentId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.sourceEntityPM.NotifyId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                this.EntityPM.IsCopy = true;
                this.sourceEntityPM = myQuote;
                this.ButtonContent = TextCodeTranslator.Translate("Quote.B.Copy");

                if (!AppTool.IsNullOrEmpty(myQuote.OpportunityId)) {
                    this.EntityPM.OpportunityId = myQuote.OpportunityId;
                }

                //if (myQuote.IsCustomerSet) {
                this.QuoteCustomerTypeCode = myQuote.QuoteCustomerTypeCode;
                this.EntityPM.IsCustomerSet = myQuote.IsCustomerSet;
                this.EntityPM.CustomerId = myQuote.CustomerId;
                this.EntityPM.CustomerName = myQuote.CustomerName;
                this.EntityPM.CustomerNote = myQuote.CustomerNote;
                this.EntityPM.CustomerRankName = myQuote.CustomerRankName;
                this.EntityPM.CustomerReference1 = myQuote.CustomerReference1;
                this.EntityPM.CustomerReference2 = myQuote.CustomerReference2;
                this.EntityPM.CustomerContactId = myQuote.CustomerContactId;
                this.ComputeCustomerDependency();
                this.ComputeCustomerAddressForCopy(myQuote);

                QuoteUtilities.CopyQuote(this.EntityPM, myQuote);
                QuoteUtilities.CopyQuotePackages(this.EntityPM, myQuote);

                this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = !this.IsLCLEntity;

                this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
                this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";

                this.InitializeCopy_Objects();
                this.OnFiltersChanged();
            }
        }
    }
    InitializeCopy_Objects() {

        // Partners
        if (!AppTool.IsNullOrEmpty(this.sourceEntityPM.ShipperId)) {
            this.ShipperCopyIsEnabled = true;

            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyShipper) {
                    this.ShipperCopyIsChecked = true;
                }
            }

            else {
                this.ShipperCopyIsChecked = true;
            }

            if (this.sourceEntityPM.CustomerId == this.sourceEntityPM.ShipperId) {
                this.IsShipperMyCustomer = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.sourceEntityPM.ConsigneeId)) {
            this.ConsigneeCopyIsEnabled = true;

            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyConsignee) {
                    this.ConsigneeCopyIsChecked = true;
                }
            }

            else {
                this.ConsigneeCopyIsChecked = true;
            }

            if (this.sourceEntityPM.CustomerId == this.sourceEntityPM.ConsigneeId) {
                this.IsConsigneeMyCustomer = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.sourceEntityPM.AgentId)) {
            this.AgentCopyIsEnabled = true;

            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyAgent) {
                    this.AgentCopyIsChecked = true;
                }
            }

            else {
                this.AgentCopyIsChecked = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.sourceEntityPM.NotifyId)) {
            this.NotifyCopyIsEnabled = true;

            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyNotify) {
                    this.NotifyCopyIsChecked = true;
                }
            }

            else {
                this.NotifyCopyIsChecked = true;
            }
        }

        // Routing
        if (this.QuoteSetting) {
            this.MainCarriageCopyIsEnabled = true;

            if (this.QuoteSetting.CopyMainCarriage) {
                this.MainCarriageCopyIsChecked = true;
            }

            if (this.QuoteSetting.CopyPickup) {
                this.CopyPickUpIsChecked = true;
            }

            if (this.QuoteSetting.CopyDelivery) {
                this.CopyDeliveryIsChecked = true;
            }
        }

        else {
            this.MainCarriageCopyIsEnabled = false;
            this.MainCarriageCopyIsChecked = true;
            this.CopyPickUpIsChecked = true;
            this.CopyDeliveryIsChecked = true;
        }

        // Charges
        if (this.sourceEntityPM.QuoteCharges.length > 0) {
            this.ChargesTypesCopyIsEnabled = true;

            if (this.QuoteSetting) {
                if (this.QuoteSetting.CopyChargesTypes) {
                    this.ChargesTypesCopyIsChecked = true;
                }
            }

            else {
                this.ChargesTypesCopyIsChecked = false;
            }
        }
    }
    InitializeCopy_Charges() {
        if (this.IsCopyFromQuote) {
            if (this.ChargesTypesCopyIsChecked) {

                if (this.EntityPM.QuoteCharges.length > 0) {
                    this.EntityPM.QuoteCharges.forEach((item) => {

                        item.QuoteOPChargePriceSteps.forEach((priceItem) => {
                            item.RemoveQuoteOPPriceSteps(priceItem);
                        });

                        this.EntityPM.RemoveQuoteOPCharge(item);
                    });
                }

                QuoteUtilities.CopyQuoteCharges(this.EntityPM, this.sourceEntityPM, this.CopySaleIsChecked, this.CopyCostIsChecked);

                this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);

                if (this.EntityPM.ExchangeRate == null) {
                    if (!AppTool.IsNullOrZero(this.sourceEntityPM.EstimateProfit) && !AppTool.IsNullOrZero(this.sourceEntityPM.ExchangeRate)) {
                        this.EntityPM.EstimateProfit = Math.abs((this.sourceEntityPM.EstimateProfit * this.sourceEntityPM.ExchangeRate));
                    }

                    else {
                        this.EntityPM.EstimateProfit = 0;
                    }
                }

                else {
                    if (this.sourceEntityPM.EstimateProfit == 0 || this.sourceEntityPM.EstimateProfit == null) {
                        this.EntityPM.EstimateProfit = 0;
                    }

                    else {
                        this.EntityPM.EstimateProfit = Math.abs(this.sourceEntityPM.EstimateProfit);
                    }
                }
            }
        }
    }

    SetCreateButtonText() {
        this.ButtonContent = TextCodeTranslator.Translate("Quote.B.Create");
        if (this.EntityPM.IsCopy) {
            this.ButtonContent = TextCodeTranslator.Translate("Quote.B.Copy");
        }
        if (this.ConvertTransportMode) {
            this.ButtonContent = TextCodeTranslator.Translate("General.B.Ok");
        }
    }

    private ComputeCustomerAddressForCopy(myQuote: QuoteOPPM) {
        switch (this.QuoteCustomerTypeCode) {
            case "SHI": {
                this.CustomerAddressId = myQuote.ShipperMainAddressId;
                break;
            }

            case "CON": {
                this.CustomerAddressId = myQuote.ConsigneeMainAddressId;
                break;
            }

            case "AGT": {
                this.CustomerAddressId = myQuote.AgentAddressId;
                break;
            }

            case "NOT": {
                this.CustomerAddressId = myQuote.NotifyAddressId;
                break;
            }

            case "OTH": {

                break;
            }
        }
    }

    private shipperCopyIsChecked: boolean = false;
    get ShipperCopyIsChecked() { return this.shipperCopyIsChecked; }
    set ShipperCopyIsChecked(newValue: boolean) {
        if (this.shipperCopyIsChecked != newValue) {
            this.shipperCopyIsChecked = newValue;

            if (newValue) {
                this.EntityPM.ShipperId = this.sourceEntityPM.ShipperId;
                this.EntityPM.ShipperName = this.sourceEntityPM.ShipperName;
                this.ShipperContactId = this.sourceEntityPM.ShipperContactId;
                this.ShipperReference1 = this.sourceEntityPM.ShipperReference1;
                this.ShipperReference2 = this.sourceEntityPM.ShipperReference2;
                this.ShipperNote = this.sourceEntityPM.ShipperNote;
                this.GetShipperCardData();

                if (this.EntityPM.QuoteCustomerTypeCode == "SHI") {
                    this.EntityPM.CustomerId = this.sourceEntityPM.ShipperId;
                }
            }

            else {
                this.ShipperId = null;
                this.EntityPM.ShipperName = null;
                this.ShipperContactId = null;
                this.ShipperReference1 = null;
                this.ShipperReference2 = null;
                this.ShipperNote = null;
            }
        }
    }

    private consigneeCopyIsChecked: boolean = false;
    get ConsigneeCopyIsChecked() { return this.consigneeCopyIsChecked; }
    set ConsigneeCopyIsChecked(newValue: boolean) {
        if (this.consigneeCopyIsChecked != newValue) {
            this.consigneeCopyIsChecked = newValue;

            if (newValue) {
                this.EntityPM.ConsigneeId = this.sourceEntityPM.ConsigneeId;
                this.EntityPM.ConsigneeName = this.sourceEntityPM.ConsigneeName;
                this.EntityPM.ConsigneeContactId = this.sourceEntityPM.ConsigneeContactId;
                this.EntityPM.ConsigneeReference1 = this.sourceEntityPM.ConsigneeReference1;
                this.EntityPM.ConsigneeReference2 = this.sourceEntityPM.ConsigneeReference2;
                this.EntityPM.ConsigneeNote = this.sourceEntityPM.ConsigneeNote;
                this.GetConsigneeCardData();

                if (this.EntityPM.QuoteCustomerTypeCode == "CON") {
                    this.EntityPM.CustomerId = this.sourceEntityPM.ConsigneeId;
                }
            }

            else {
                this.ConsigneeId = null;
                this.ConsigneeContactId = null;
                this.ConsigneeReference1 = null;
                this.ConsigneeReference2 = null;
                this.ConsigneeNote = null;
            }
        }
    }

    private agentCopyIsChecked: boolean = false;
    get AgentCopyIsChecked() { return this.agentCopyIsChecked; }
    set AgentCopyIsChecked(newValue: boolean) {
        if (this.agentCopyIsChecked != newValue) {
            this.agentCopyIsChecked = newValue;

            this.EntityPM.AgentId = !newValue ? null : this.sourceEntityPM.AgentId;
            this.EntityPM.AgentName = !newValue ? null : this.sourceEntityPM.AgentName;
            this.EntityPM.AgentAddressId = !newValue ? null : this.sourceEntityPM.AgentAddressId;
            this.EntityPM.AgentContactId = !newValue ? null : this.sourceEntityPM.AgentContactId;

            if (this.EntityPM.QuoteCustomerTypeCode == "AGT") {
                this.EntityPM.CustomerId = this.sourceEntityPM.AgentId;
            }
        }
    }

    private notifyCopyIsChecked: boolean = false;
    get NotifyCopyIsChecked() { return this.notifyCopyIsChecked; }
    set NotifyCopyIsChecked(newValue: boolean) {
        if (this.notifyCopyIsChecked != newValue) {
            this.notifyCopyIsChecked = newValue;

            this.EntityPM.NotifyId = !newValue ? null : this.sourceEntityPM.NotifyId;
            this.EntityPM.NotifyName = !newValue ? null : this.sourceEntityPM.NotifyName;
            this.EntityPM.NotifyAddressId = !newValue ? null : this.sourceEntityPM.NotifyAddressId;
            this.EntityPM.NotifyContactId = !newValue ? null : this.sourceEntityPM.NotifyContactId;
            this.EntityPM.NotifyNote = !newValue ? null : this.sourceEntityPM.NotifyNote;

            if (this.EntityPM.QuoteCustomerTypeCode == "NOT") {
                this.EntityPM.CustomerId = this.sourceEntityPM.NotifyId;
            }
        }
    }

    private mainCarriageCopyIsChecked: boolean = false;
    get MainCarriageCopyIsChecked() { return this.mainCarriageCopyIsChecked; }
    set MainCarriageCopyIsChecked(value: boolean) {
        if (this.mainCarriageCopyIsChecked != value) {
            this.mainCarriageCopyIsChecked = value;

            if (value) {
                this.EntityPM.FromPort = this.sourceEntityPM.FromPort;
                this.EntityPM.FromPortCountry = this.sourceEntityPM.FromPortCountry;
                this.EntityPM.FromPortId = this.sourceEntityPM.FromPortId;
                this.EntityPM.FromPortName = this.sourceEntityPM.FromPortName;
                this.EntityPM.ToPort = this.sourceEntityPM.ToPort;
                this.EntityPM.ToPortCountry = this.sourceEntityPM.ToPortCountry;
                this.EntityPM.ToPortId = this.sourceEntityPM.ToPortId;
                this.EntityPM.ToPortName = this.sourceEntityPM.ToPortName;
            }

            else {
                this.EntityPM.FromPort = null;
                this.EntityPM.FromPortCountry = null;
                this.EntityPM.FromPortId = null;
                this.EntityPM.FromPortName = null;
                this.EntityPM.ToPort = null;
                this.EntityPM.ToPortCountry = null;
                this.EntityPM.ToPortId = null;
                this.EntityPM.ToPortName = null;
            }

            this.SetUIProperties_Domestic();
        }
    }

    private copyPickUpIsChecked: boolean = false;
    get CopyPickUpIsChecked() { return this.copyPickUpIsChecked; }
    set CopyPickUpIsChecked(newValue: boolean) {
        if (this.copyPickUpIsChecked != newValue) {
            this.copyPickUpIsChecked = newValue;

            this.IncludePickUp = newValue;
        }
    }

    private copyDeliveryIsChecked: boolean = false;
    get CopyDeliveryIsChecked() { return this.copyDeliveryIsChecked; }
    set CopyDeliveryIsChecked(newValue: boolean) {
        if (this.copyDeliveryIsChecked != newValue) {
            this.copyDeliveryIsChecked = newValue;

            this.IncludeDelivery = newValue;
        }
    }

    private chargesTypesCopyIsChecked: boolean = false;
    get ChargesTypesCopyIsChecked() { return this.chargesTypesCopyIsChecked; }
    set ChargesTypesCopyIsChecked(value: boolean) {
        if (this.chargesTypesCopyIsChecked != value) {
            this.chargesTypesCopyIsChecked = value;

            if (value) {
                if (this.QuoteSetting) {
                    if (this.QuoteSetting.CopyChargesCost) {
                        this.CopyCostIsChecked = true;
                    }

                    if (this.QuoteSetting.CopyChargesSale) {
                        this.CopySaleIsChecked = true;
                    }

                    if (this.QuoteSetting.CopyExchangeRates) {
                        this.CopyRatesIsChecked = true;
                    }
                }
            }

            else {
                this.CopyCostIsChecked = false;
                this.CopySaleIsChecked = false;
                this.CopyRatesIsChecked = false;
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('DirectionId');
        this.myCloner.AddField('TransportModeId');
        this.myCloner.AddField('ShipmentTypeId');
        this.myCloner.AddField('ShipmentSubTypeId');
        this.myCloner.AddField('ShipmentTypeName');
        this.myCloner.AddField('ShipperId');
        this.myCloner.AddField('ShipperReference1');
        this.myCloner.AddField('ShipperContactId');
        this.myCloner.AddField('ShipperReference2');
        this.myCloner.AddField('ConsigneeId');
        this.myCloner.AddField('ConsigneeReference1');
        this.myCloner.AddField('ConsigneeContactId');
        this.myCloner.AddField('ConsigneeReference2');
        this.myCloner.AddField('QuoteCustomerTypeCode');
        this.myCloner.AddField('CustomerId');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('ExpirationDays');
        this.myCloner.AddField('ExpirationDate');
        this.myCloner.AddField('MainCarriageCarrierId');
        this.myCloner.AddField('IncotermId');
        this.myCloner.AddField('MoveTypeId');
        this.myCloner.AddField('IsAutomaticallyClosed');
        this.myCloner.AddField('AutomaticallyCloseDays');
        this.myCloner.AddField('AutomaticallyCloseDate');
        this.myCloner.AddField('IncludePickUp');
        this.myCloner.AddField('PickUpAddressId');
        this.myCloner.AddField('FromAddressZipCode');
        this.myCloner.AddField('FromAddressCity');
        this.myCloner.AddField('FromAddressCountryId');
        this.myCloner.AddField('FromPortId');
        this.myCloner.AddField('ToPortId');
        this.myCloner.AddField('MainCarriageCarrierId');
        this.myCloner.AddField('IncludeDelivery');
        this.myCloner.AddField('DeliveryAddressId');
        this.myCloner.AddField('ToAddressZipCode');
        this.myCloner.AddField('ToAddressCity');
        this.myCloner.AddField('ToAddressCity');
        this.myCloner.AddField('ToAddressCountryId');
        this.myCloner.AddField('GrossWeight');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('ChargeableWeight');
        this.myCloner.AddField('NumberOfPackages');
        this.myCloner.AddField('PackageType1Quantity');
        this.myCloner.AddField('PackageType1Id');
        this.myCloner.AddField('PackageType2Quantity');
        this.myCloner.AddField('PackageType2Id');
        this.myCloner.AddField('PackageType3Quantity');
        this.myCloner.AddField('PackageType3Id');
        this.myCloner.AddField('PackageType4Quantity');
        this.myCloner.AddField('PackageType4Id');
        this.myCloner.AddField('IsDangerous');
        this.myCloner.AddField('DescriptionOfGoods');
        this.myCloner.AddField('ConvertTransportMode');
        this.myCloner.AddField('ExchangeRate');
        this.myCloner.AddField('EventNote');
        this.myCloner.AddField('NumberOfContainers');
        this.myCloner.AddField('ShipperMainAddressId');

        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

class FilterClass {
    public Code: string;
    public Name: string;
    public SRC: string;
    constructor(code: string, name: string, src: string = null) {
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
}
