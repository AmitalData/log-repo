import { Component} from '@angular/core';
import { ShipmentPM } from '../../EntityPMs/ShipmentPM';
import { ShipmentPMService } from '../../Services/StandardPMs/ShipmentPMService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { AddressList } from '../../../Common/EntityLists/AddressList';
import { PortList } from '../../../Common/EntityLists/PortList';
import { CardList } from '../../../Common/EntityLists/CardList';      
import { CardListService } from '../../../Common/Services/StandardLists/CardListService';
import { AddressListService } from '../../../Common/Services/StandardLists/AddressListService';
import { PortListService } from '../../../Common/Services/StandardLists/PortListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AddEditPartnerArgs } from '../../Args';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { NewEntityArgs } from '../../../Infrastructure/Args';
import { AddressPM } from '../../../Common/EntityPMs/AddressPM';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ShipmentTool, RoutingHelper } from '../../Tools';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './ShipmenDirectionConvertComponent.html',
})

export class ShipmenDirectionConvertComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext: ShipmenDirectionConvertComponent = this;
    public ValidationErrorsList: string[] = [];
    public EnabledOkButton: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
    private IsCurrentInlandDomestic: boolean = false;
    public IsOldInlandDomestic: boolean = false;
    public TransportModeId: string;
    public DirectionsList: DirectionFilterItem[] = [];
    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    private myPortListService: PortListService;
    public SessionIndex: number;
    constructor() {
        super();

        this.SessionIndex = SessionLocator.Index;
        
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myPortListService = new PortListService();
    }

    private oldShipmentDirection: string;
    SetWindowArgs(args: ConvertDirectionArgs) {
        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.EnabledOkButton = args.EnabledOkButton;
        this.ValidationErrorsList = args.ValidationErrorsList;
        this.oldShipmentDirection = this.EntityPM.DirectionId;
        this.TransportModeId = this.EntityPM.TransportModeId;

        this.IsOldInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        this.IsCurrentInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.CardDependencyProperty1 = "AG";
            this.CardDependencyProperty1IsList = false;
        }

        else {
            if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                this.CardDependencyProperty1 = "CS,AG";
                this.CardDependencyProperty1IsList = true;
            }
        }

        this.SetLabels();
        this.SetUIProperties();
        this.SetScreenEnabled();
        this.BuildDirectionsFilterList();
        this.Clone();

        if (this.IsCurrentInlandDomestic) {
            this.LoadFromAddress();
            this.LoadToAddress();
        }

        else {
            this.LoadAddress("S");
            this.LoadAddress("C");
        }
    }

    public FromTextCode: string;
    public ToTextCode: string;    
    SetLabels() {
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCode = "Shipment.S.NewShipment.Gateway";
                this.ToTextCode = "Shipment.S.NewShipment.Destination";                
                break;
            }

            case "O": {
                this.FromTextCode = "Shipment.S.NewShipment.LoadingPort";
                this.ToTextCode = "Shipment.S.NewShipment.DischargePort";                
                break;
            }

            case "I": {
                this.FromTextCode = "Shipment.S.NewShipment.From";
                this.ToTextCode = "Shipment.S.NewShipment.To";                
                break;
            }

            default: {
                this.FromTextCode = "Shipment.S.NewShipment.From";
                this.ToTextCode = "Shipment.S.NewShipment.To";               
                break;
            }
        }        
    }

    public ScreenOpacity: number = 0.7;
    public IsScreenEnabled: boolean = false;
    SetScreenEnabled() {
        var isScreenEnabled = false;

        if (this.oldShipmentDirection != this.DirectionId) {
            isScreenEnabled = true;
        }

        this.IsScreenEnabled = isScreenEnabled;
        this.ScreenOpacity = isScreenEnabled ? 1 : 0.7;

        // Shipper
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference2", this.ObjectTableName, isScreenEnabled);

        // Consignee
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference2", this.ObjectTableName, isScreenEnabled);

        //Customer
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipmentCustomerTypeCode", this.ObjectTableName, isScreenEnabled);

        // Ports        
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isScreenEnabled);
    }

    SetUIProperties() {
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Ports();        
    }
    SetUIProperties_Shipper() {
        var isFieldRequired: boolean = false;

        if (this.IsCurrentInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.FromPartnerId)) {
                isFieldRequired = true;
            }
        }

        this.UIProperties.SetRequired(this.FromPartnerIdProperty, this.ObjectTableName, isFieldRequired);
    }
    SetUIProperties_Consignee() {
        var isFieldRequired: boolean = false;

        if (this.IsCurrentInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.ToPartnerId)) {
                isFieldRequired = true;
            }
        }
        this.UIProperties.SetRequired(this.ToPartnerIdProperty, this.ObjectTableName, isFieldRequired);
    }
    SetUIProperties_Ports() {
        var isFromRequired: boolean = false;
        var isToRequired: boolean = false;

        if (this.IsOldInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                isFromRequired = true;
            }

            if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
                isToRequired = true;
            }
        }

        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, isToRequired);
    }

    get FromPartnerIdProperty() {
        if (this.IsCurrentInlandDomestic) {
            return "MainCarriageFromPartnerId";
        }

        else {
            return "ShipperId";
        }
    }

    get FromPartnerId() {
        if (this.IsCurrentInlandDomestic) {
            return this.MainCarriageFromPartnerId;
        }

        else {
            return this.ShipperId;
        }
    }
    set FromPartnerId(newValue: string) {
        if (this.IsCurrentInlandDomestic) {
            this.MainCarriageFromPartnerId = newValue;
        }
        this.ShipperId = newValue;
    }

    get FromAddressIdProperty() {
        if (this.IsCurrentInlandDomestic) {
            return "MainCarriageFromAddressId";
        }

        else {
            return "ShipperAddressId";
        }
    }

    get FromAddressId() {
        if (this.IsCurrentInlandDomestic) {
            return this.MainCarriageFromAddressId;
        }

        else {
            return this.ShipperAddressId;
        }
    }
    set FromAddressId(newValue: string) {
        if (this.IsCurrentInlandDomestic) {
            this.MainCarriageFromAddressId = newValue;
        }
        this.ShipperAddressId = newValue;
    }

    get ToPartnerIdProperty() {
        if (this.IsCurrentInlandDomestic) {
            return "MainCarriageToPartnerId";
        }

        else {
            return "ConsigneeId";
        }
    }

    get ToPartnerId() {
        if (this.IsCurrentInlandDomestic) {
            return this.MainCarriageToPartnerId;
        }

        else {
            return this.ConsigneeId;
        }
    }
    set ToPartnerId(newValue: string) {
        if (this.IsCurrentInlandDomestic) {
            this.MainCarriageToPartnerId = newValue;
        }
        this.ConsigneeId = newValue;
    }

    get ToAddressIdProperty() {
        if (this.IsCurrentInlandDomestic) {
            return "MainCarriageToAddressId";
        }

        else {
            return "ConsigneeAddressId";
        }
    }

    get ToAddressId() {
        if (this.IsCurrentInlandDomestic) {
            return this.MainCarriageToAddressId;
        }

        else {
            return this.ConsigneeAddressId;
        }
    }
    set ToAddressId(newValue: string) {
        if (this.IsCurrentInlandDomestic) {
            this.MainCarriageToAddressId = newValue;
        }
        this.ConsigneeAddressId = newValue;
    }

    // Direction
    BuildDirectionsFilterList() {
        this.DirectionsList = [];
        
        this.DirectionsList.push(new DirectionFilterItem("E", "Export"));
        this.DirectionsList.push(new DirectionFilterItem("I", "Import"));
        this.DirectionsList.push(new DirectionFilterItem("D", "Domestic"));
        this.DirectionsList.push(new DirectionFilterItem("R", "Drop"));
    }

    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId != newValue) {
            this.EntityPM.DirectionId = newValue;
            
            this.OnDirectionChanged();
        }
    }

    private OnDirectionChanged() {
        this.IsCurrentInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;

        this.SetScreenEnabled();
        this.SetUIProperties();

        if (this.IsCurrentInlandDomestic) {
            this.MainCarriageFromPartnerId = this.ShipperId;
            this.MainCarriageToPartnerId = this.ConsigneeId;
        }

        else {
            this.LoadAddress("S");
            this.LoadAddress("C");
        }
    }

    // Shipper
    private ShipperPartnerTypeId: string;
    private ShipperIsCustomer: boolean;
    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;

            if (this.ShipmentCustomerTypeCode == "SHI") {
                this.CustomerId = newValue;
            }

            this.SetUIProperties_Shipper();
            
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperPartnerTypeId = null;
                this.ShipperContactId = null;
                this.EntityPM.ShipperName = null;
                this.EntityPM.ShipperNote = null;
                this.EntityPM.ShipperReference1 = null;
                this.EntityPM.ShipperReference2 = null;
                this.EntityPM.ShipperMainAddressId = null;
                this.EntityPM.ShipperPickAddressId = null;
                this.EntityPM.KnownConsignorNumber = null;
                this.EntityPM.KCExpirationDate = null;
                this.ShipperAddressId = null;
            }

            else {
                if (this.IsCurrentInlandDomestic) {
                    this.MainCarriageFromPartnerId = newValue;
                }

                else {
                    this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var myCardList: CardList = myResponse.Result;
                            if (myCardList) {
                                this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                                this.ShipperContactId = myCardList.PrimaryContactId;
                                this.EntityPM.ShipperName = myCardList.EnglishName;
                                this.EntityPM.ShipperNote = myCardList.Notes;
                                this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                                this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                                this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                                this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                                this.ShipperAddressId = myCardList.MainAddressId;
                                this.ShipperIsCustomer = myCardList.IsCustomer;
                            }
                        }
                    });
                }
            }
        }
    }

    get ShipperAddressId() { return this.EntityPM.ShipperAddressId; }
    set ShipperAddressId(newValue: string) {
        if (this.EntityPM.ShipperAddressId != newValue) {
            this.EntityPM.ShipperAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperAddressList = null;
            }

            else {
                this.LoadAddress("S");
            }
        }
    }

    private myShipperAddressList: AddressList;
    get ShipperAddressList() { return this.myShipperAddressList; }
    set ShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;
    }

    get ShipperContactId() { return this.EntityPM.ShipperContactId; }
    set ShipperContactId(newValue: string) {
        if (this.EntityPM.ShipperContactId != newValue) {
            this.EntityPM.ShipperContactId = newValue;
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

    get ShipperName() { return this.EntityPM.ShipperName; }
    set ShipperName(newValue: string) {
        if (this.EntityPM.ShipperName != newValue) {
            this.EntityPM.ShipperName = newValue;
        }
    }

    // Consignee
    private ConsigneePartnerTypeId: string;
    private ConsigneeIsCustomer: boolean;
    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;

            if (this.ShipmentCustomerTypeCode == "CON") {
                this.CustomerId = newValue;
            }

            this.SetUIProperties_Consignee();
            
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ConsigneePartnerTypeId = null;
                this.ConsigneeContactId = null;
                this.EntityPM.ConsigneeName = null;
                this.EntityPM.ConsigneeNote = null;
                this.EntityPM.ConsigneeReference1 = null;
                this.EntityPM.ConsigneeReference2 = null;
                this.EntityPM.ConsigneeMainAddressId = null;
                this.EntityPM.ConsigneePickAddressId = null;
                this.ConsigneeAddressId = null;
            }

            else {
                if (this.IsCurrentInlandDomestic) {
                    this.MainCarriageToPartnerId = newValue;
                }

                else {
                    this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var myCardList: CardList = myResponse.Result;
                            if (myCardList) {
                                this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                                this.ConsigneeContactId = myCardList.PrimaryContactId;
                                this.EntityPM.ConsigneeName = myCardList.EnglishName;
                                this.EntityPM.ConsigneeNote = myCardList.Notes;
                                this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                                this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                                this.ConsigneeAddressId = myCardList.MainAddressId;
                                this.ConsigneeIsCustomer = myCardList.IsCustomer;
                            }
                        }
                    });
                }
            }
        }
    }

    get ConsigneeAddressId() { return this.EntityPM.ConsigneeAddressId; }
    set ConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeAddressId != newValue) {
            this.EntityPM.ConsigneeAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ConsigneeAddressList = null;
            }

            else {
                this.LoadAddress("C");
            }
        }
    }

    private myConsigneeAddressList: AddressList;
    get ConsigneeAddressList() { return this.myConsigneeAddressList; }
    set ConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;
    }

    get ConsigneeContactId() { return this.EntityPM.ConsigneeContactId; }
    set ConsigneeContactId(newValue: string) {
        if (this.EntityPM.ConsigneeContactId != newValue) {
            this.EntityPM.ConsigneeContactId = newValue;
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

    get ConsigneeName() { return this.EntityPM.ConsigneeName; }
    set ConsigneeName(newValue: string) {
        if (this.EntityPM.ConsigneeName != newValue) {
            this.EntityPM.ConsigneeName = newValue;
        }
    }

    private isShipperMyCustomer: boolean = false;
    get IsShipperMyCustomer() { return this.isShipperMyCustomer; }
    set IsShipperMyCustomer(value: boolean) {
        if (this.isShipperMyCustomer != value) {
            this.isShipperMyCustomer = value;

            if (value) {
                if (this.ShipmentCustomerTypeCode == "SHI") {
                    this.CustomerId = this.ShipperId;
                }
            }
            
            this.SetUIProperties_Shipper();
            this.SetUIProperties_Consignee();
        }
    }

    private isConsigneeMyCustomer: boolean = false;
    get IsConsigneeMyCustomer() { return this.isConsigneeMyCustomer; }
    set IsConsigneeMyCustomer(value: boolean) {
        if (this.isConsigneeMyCustomer != value) {
            this.isConsigneeMyCustomer = value;

            if (value) {
                if (this.ShipmentCustomerTypeCode == "CON") {
                    this.CustomerId = this.ConsigneeId;
                }
            }
            
            this.SetUIProperties_Shipper();
            this.SetUIProperties_Consignee();
        }
    }
    
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

    //Customer
    public CustomerDependencyProperty1: string = "CS";
    public CustomerDependencyProperty1IsList: boolean = false;
    private ComputeCustomerDependency() {
        switch (this.ShipmentCustomerTypeCode) {
            case "SHI":
            case "CON":
                {
                    this.CustomerDependencyProperty1 = "CS";
                    this.CustomerDependencyProperty1IsList = false;

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        this.CustomerDependencyProperty1 = "AG";
                        this.CustomerDependencyProperty1IsList = false;
                    }

                    else {
                        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            this.CustomerDependencyProperty1 = "CS,AG";
                            this.CustomerDependencyProperty1IsList = true;
                        }
                    }

                    break;
                }

            case "AGT":
            case "IGT":
            case "FOR":
            case "COL":
                {
                    this.CustomerDependencyProperty1 = "AG";
                    this.CustomerDependencyProperty1IsList = false;
                    break;
                }

            case "CAE":
            case "CAI":
                {
                    this.CustomerDependencyProperty1 = "CG";
                    this.CustomerDependencyProperty1IsList = false;
                    break;
                }

            case "NT1":
            case "NT2":
            case "REA":
                {
                    this.CustomerDependencyProperty1 = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }

            case "SNE":
            case "CNI":
            case "CSD":
                {
                    this.CustomerDependencyProperty1 = "AG,CS";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }

            case "CCP": {
                this.CustomerDependencyProperty1 = "WH";
                this.CustomerDependencyProperty1IsList = false;
                break;
            }

            case "OTH": {
                this.CustomerDependencyProperty1 = "CS";
                this.CustomerDependencyProperty1IsList = false;
                break;
            }
        }
    }

    get ShipmentCustomerTypeCode() { return this.EntityPM.ShipmentCustomerTypeCode; }
    set ShipmentCustomerTypeCode(newValue: string) {
        if (this.EntityPM.ShipmentCustomerTypeCode != newValue) {
            this.EntityPM.ShipmentCustomerTypeCode = newValue;

            this.CustomerId = null;

            this.SetCustomer(newValue);
            this.SetCustomerRequired();
            this.ComputeCustomerDependency();
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
                            this.SetCustomerPartner();
                        }
                    }
                });
            }
        }
    }

    get CustomerAddressId() { return this.EntityPM.CustomerAddressId; }
    set CustomerAddressId(newValue: string) {
        if (this.EntityPM.CustomerAddressId != newValue) {
            this.EntityPM.CustomerAddressId = newValue;
        }
    }

    get CustomerContactId() { return this.EntityPM.CustomerContactId; }
    set CustomerContactId(newValue: string) {
        if (this.EntityPM.CustomerContactId != newValue) {
            this.EntityPM.CustomerContactId = newValue;
        }
    }

    private SetCustomerPartner() {
        switch (this.ShipmentCustomerTypeCode) {
            case "SHI": {
                this.ShipperId = this.CustomerId;
                break;
            }

            case "CON": {
                this.ConsigneeId = this.CustomerId;
                break;
            }

            case "AGT": {
                this.EntityPM.AgentId = this.CustomerId;
                this.EntityPM.AgentAddressId = this.CustomerAddressId;
                this.EntityPM.AgentContactId = this.CustomerContactId;
                break;
            }

            case "IGT": {
                this.EntityPM.IssuingCarrierAgentId = this.CustomerId;
                this.EntityPM.IssuingCarrierAddressId = this.CustomerAddressId;
                break;
            }

            case "FOR": {
                this.EntityPM.FreightForwarderId = this.CustomerId;
                this.EntityPM.FreightForwarderAddressId = this.CustomerAddressId;
                this.EntityPM.FreightForwarderContactId = this.CustomerContactId;
                break;
            }

            case "COL": {
                this.EntityPM.ColoaderId = this.CustomerId;
                this.EntityPM.ColoaderAddressId = this.CustomerAddressId;
                this.EntityPM.ColoaderContactId = this.CustomerContactId;
                break;
            }

            case "CAE": {
                this.EntityPM.CustomAgentExportId = this.CustomerId;
                this.EntityPM.CustomAgentExportAddressId = this.CustomerAddressId;
                this.EntityPM.CustomAgentExportContactId = this.CustomerContactId;
                break;
            }

            case "CAI": {
                this.EntityPM.CustomAgentImportId = this.CustomerId;
                this.EntityPM.CustomAgentImportAddressId = this.CustomerAddressId;
                this.EntityPM.CustomAgentImportContactId = this.CustomerContactId;
                break;
            }

            case "NT1": {
                this.EntityPM.Notify1Id = this.CustomerId;
                this.EntityPM.Notify1AddressId = this.CustomerAddressId;
                this.EntityPM.Notify1ContactId = this.CustomerContactId;
                break;
            }

            case "NT2": {
                this.EntityPM.Notify2Id = this.CustomerId;
                this.EntityPM.Notify2AddressId = this.CustomerAddressId;
                this.EntityPM.Notify2ContactId = this.CustomerContactId;
                break;
            }

            case "REA": {
                this.EntityPM.ReleasingAgentId = this.CustomerId;
                this.EntityPM.ReleasingAgentAddressId = this.CustomerAddressId;
                this.EntityPM.ReleasingAgentContactId = this.CustomerContactId;
                break;
            }

            case "SNE": {
                this.EntityPM.ShipperNotExporterId = this.CustomerId;
                this.EntityPM.ShipperNotExporterAddressId = this.CustomerAddressId;
                this.EntityPM.ShipperNotExporterContactId = this.CustomerContactId;
                break;
            }

            case "CNI": {
                this.EntityPM.ConsigneeNotImporterId = this.CustomerId;
                this.EntityPM.ConsigneeNotImporterAddressId = this.CustomerAddressId;
                this.EntityPM.ConsigneeNotImporterContactId = this.CustomerContactId;
                break;
            }

            case "CSD": {
                this.EntityPM.ConsolidatorId = this.CustomerId;
                this.EntityPM.ConsolidatorAddressId = this.CustomerAddressId;
                this.EntityPM.ConsolidatorContactId = this.CustomerContactId;
                break;
            }

            case "CCP": {
                this.EntityPM.CustomClearancePointId = this.CustomerId;
                this.EntityPM.CustomClearancePointAddressId = this.CustomerAddressId;
                this.EntityPM.CustomClearancePointContactId = this.CustomerContactId;
                break;
            }

            case "OTH": {

                break;
            }
        }
    }

    public IsCustomerRequired: boolean = false;
    private SetCustomerRequired() {
        var isRequired: boolean = false

        if (AppTool.IsNullOrEmpty(this.CustomerId) || AppTool.IsNullOrEmpty(this.ShipmentCustomerTypeCode)) {
            isRequired = true;
        }

        this.IsCustomerRequired = isRequired;
    }

    //Ports
    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(value: string) {
        if (this.EntityPM.MainCarriageFromPortId != value) {
            this.EntityPM.MainCarriageFromPortId = value;

            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }
    
    get MainCarriageToPortId() { return this.EntityPM.MainCarriageToPortId; }
    set MainCarriageToPortId(value: string) {
        if (this.EntityPM.MainCarriageToPortId != value) {
            this.EntityPM.MainCarriageToPortId = value;
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    //Inland Domestic Partners
    get MainCarriageFromPartnerId() { return this.EntityPM.MainCarriageFromPartnerId; }
    set MainCarriageFromPartnerId(value: string) {
        if (this.EntityPM.MainCarriageFromPartnerId != value) {
            this.EntityPM.MainCarriageFromPartnerId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageFromAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.MainCarriageFromAddressId = list.MainAddressId;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageFromAddressId() { return this.EntityPM.MainCarriageFromAddressId; }
    set MainCarriageFromAddressId(value: string) {
        if (this.EntityPM.MainCarriageFromAddressId != value) {
            this.EntityPM.MainCarriageFromAddressId = value;
            this.LoadFromAddress();
        }
    }

    get MainCarriageToPartnerId() { return this.EntityPM.MainCarriageToPartnerId; }
    set MainCarriageToPartnerId(value: string) {
        if (this.EntityPM.MainCarriageToPartnerId != value) {
            this.EntityPM.MainCarriageToPartnerId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageToAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.MainCarriageToAddressId = list.MainAddressId;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageToAddressId() { return this.EntityPM.MainCarriageToAddressId; }
    set MainCarriageToAddressId(value: string) {
        if (this.EntityPM.MainCarriageToAddressId != value) {
            this.EntityPM.MainCarriageToAddressId = value;
            this.LoadToAddress();
        }
    }
       
    //LoadAddress
    private LoadToAddress() {
        if (AppTool.IsNullOrEmpty(this.MainCarriageToAddressId)) {
            this.ConsigneeAddressList = null;

            if (this.EntityPM.ToCountryId != null) {
                this.EntityPM.ToCountryId = null;
            }

            if (this.EntityPM.ToCountryIsEC != false) {
                this.EntityPM.ToCountryIsEC = false;
            }
        }

        else {
            this.myAddressListService.getSingle(this.MainCarriageToAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AddressList = myResponse.Result;
                    if (list) {
                        this.ConsigneeAddressList = list;

                        if (this.EntityPM.ToCountryId != list.CountryId) {
                            this.EntityPM.ToCountryId = list.CountryId;
                        }

                        if (this.EntityPM.ToCountryIsEC != list.CountryEC) {
                            this.EntityPM.ToCountryIsEC = list.CountryEC;
                        }
                    }
                }
            });
        }
    }
    private LoadFromAddress() {
        if (AppTool.IsNullOrEmpty(this.MainCarriageFromAddressId)) {
            this.ShipperAddressList = null;

            if (this.EntityPM.FromCountryId != null) {
                this.EntityPM.FromCountryId = null;
            }

            if (this.EntityPM.FromCountryIsEC != false) {
                this.EntityPM.FromCountryIsEC = false;
            }
        }

        else {
            this.myAddressListService.getSingle(this.MainCarriageFromAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AddressList = myResponse.Result;
                    if (list) {
                        this.ShipperAddressList = list;

                        if (this.EntityPM.FromCountryId != list.CountryId) {
                            this.EntityPM.FromCountryId = list.CountryId;
                        }

                        if (this.EntityPM.FromCountryIsEC != list.CountryEC) {
                            this.EntityPM.FromCountryIsEC = list.CountryEC;
                        }
                    }
                }
            });
        }
    }

    private LoadAddress(myAddressCode: string) {
        switch (myAddressCode) {
            case "S": {
                if (!AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
                    this.myAddressListService.getSingle(this.ShipperAddressId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            this.ShipperAddressList = myResponse.Result;
                        }
                    });
                }
                break;
            }

            case "C": {
                if (!AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
                    this.myAddressListService.getSingle(this.ConsigneeAddressId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            this.ConsigneeAddressList = myResponse.Result;
                        }
                    });
                }
                break;
            }
        }
    }

    // Add|Edit Address
    EditAddressClicked(myAddressCode: string) {
        var myAddressId: string = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;

        switch (myAddressCode) {
            case "S": {
                myAddressId = this.ShipperAddressId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.ShipperIsCustomer;
                break;
            }

            case "C": {
                myAddressId = this.ConsigneeAddressId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = this.ConsigneeIsCustomer;
                break;
            }
        }

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "S": {
                            this.ShipperAddressId = null;
                            this.ShipperAddressId = myAddressId;
                            break;
                        }

                        case "C": {
                            this.ConsigneeAddressId = null;
                            this.ConsigneeAddressId = myAddressId;
                            break;
                        }
                    }
                }
            });
        }
    }
    AddAddressClicked(myAddressCode: string) {
        var entityPM: AddressPM = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;

        switch (myAddressCode) {
            case "S": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ShipperId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.ShipperIsCustomer;
                break;
            }

            case "C": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ConsigneeId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = this.ConsigneeIsCustomer;
                break;
            }
        }

        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "S": {
                            this.ShipperAddressId = null;
                            this.ShipperAddressId = entityPM.Id;
                            break;
                        }

                        case "C": {
                            this.ConsigneeAddressId = null;
                            this.ConsigneeAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    }

    // Commands
    AddPartnerClicked(myPartnerCode: string) {
        var myComponentPath: string = null;
        var title = "";
        var isCustomer = false;

        if (myPartnerCode == "S") {
            title = "New Shipper";
            isCustomer = this.IsShipperMyCustomer;
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        }

        else if (myPartnerCode == "C") {
            title = "New Consignee";
            isCustomer = this.IsConsigneeMyCustomer;
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        }

        else if (myPartnerCode == "T") {
            if (this.ShipmentCustomerTypeCode == "IGT") {
                var myTitle = "Add Issuing Carrier's Agent";

                var windowArgs = new AddEditPartnerArgs();
                windowArgs.EntityPM = this.EntityPM;
                windowArgs.IsNewEntity = true;
                windowArgs.PartnerTypeCode = "AGT";

                var logWindow = new LogitudeWindow();
                logWindow.Title = myTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');

                logWindow.ComponentLoaded.subscribe(cmp => {
                    logWindow.WindowClosed.subscribe(($event: any) => {

                        if (cmp.IsUpdatingPartner) {

                            var myPartnerId = cmp.CurrentPartnerId;
                            var myAddressId = cmp.CurrentAddressId;

                            if (this.EntityPM.IssuingCarrierAgentId != myPartnerId) {
                                this.EntityPM.IssuingCarrierAgentId = myPartnerId;
                            }

                            else {
                                this.EntityPM.IssuingCarrierAddressId = myAddressId;
                            }
                        }
                    });
                });
            }

            else {
                title = "New " + this.ComputeAddCustomerTitle();

                if (this.CustomerDependencyProperty1 == "AG") {
                    myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
                }

                else if (this.CustomerDependencyProperty1 == "WH") {
                    myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewWarehouseComponent";
                }

                else if (this.ShipmentCustomerTypeCode == "CAE" || this.ShipmentCustomerTypeCode == "CAI") {
                    myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewCustomAgentComponent";
                }

                else {
                    myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";

                    if (this.ShipmentCustomerTypeCode == "SHI") {
                        isCustomer = this.IsShipperMyCustomer;
                    }

                    else if (this.ShipmentCustomerTypeCode == "CON") {
                        isCustomer = this.IsConsigneeMyCustomer;
                    }
                }
            }
        }

        var args = new NewEntityArgs();
        if (!isCustomer) {
            args.Perspective = "ShippersAndConsignees";
        }

        var logeWindow = new LogitudeWindow();
        logeWindow.Width = 960;
        logeWindow.Height = 600;
        logeWindow.Title = title;
        logeWindow.WindowArgs = args;
        logeWindow.Show(myComponentPath);

        logeWindow.ComponentLoaded.subscribe(comp => {
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {

                    if (myPartnerCode == "S") {
                        this.ShipperId = comp.EntityPM.Id;
                        this.ShipperName = comp.EntityPM.EnglishName;
                    }

                    else if (myPartnerCode == "C") {
                        this.ConsigneeId = comp.EntityPM.Id;
                        this.ConsigneeName = comp.EntityPM.EnglishName;
                    }

                    else if (myPartnerCode == "T") {
                        this.CustomerId = comp.EntityPM.Id;

                        if (this.ShipmentCustomerTypeCode == "SHI") {
                            this.ShipperId = comp.EntityPM.Id;
                            this.ShipperName = comp.EntityPM.EnglishName;
                        }

                        else if (this.ShipmentCustomerTypeCode == "CON") {
                            this.ConsigneeId = comp.EntityPM.Id;
                            this.ConsigneeName = comp.EntityPM.EnglishName;
                        }
                    }
                }
            });
        });
    }
    private ComputeAddCustomerTitle(): string {
        var myResult: string = "";

        switch (this.ShipmentCustomerTypeCode) {
            case "AGT":
                {
                    myResult = "Agent";
                    break
                }

            case "CAE":
                {
                    myResult = "Custom's Agent Export";
                    break
                }

            case "CAI":
                {
                    myResult = "Custom's Agent Import";
                    break
                }

            case "CCP":
                {
                    myResult = "Custom Clearance Point";
                    break
                }

            case "CNI":
                {
                    myResult = "Consignee Not Importer";
                    break
                }

            case "COL":
                {
                    myResult = "Coloader";
                    break
                }

            case "CON":
                {
                    myResult = "Consignee";
                    break
                }

            case "CSD":
                {
                    myResult = "Consolidator";
                    break
                }

            case "FOR":
                {
                    myResult = "Freight Forwarder";
                    break
                }

            case "IGT":
                {
                    myResult = "Issuing Carrier Agent";
                    break
                }

            case "NT1":
                {
                    myResult = "Notify 1";
                    break
                }

            case "NT2":
                {
                    myResult = "Notify 2";
                    break
                }

            case "REA":
                {
                    myResult = "Releasing Agent";
                    break
                }

            case "SHI":
                {
                    myResult = "Shipper";
                    break
                }

            case "SNE":
                {
                    myResult = "Shipper Not Exporter";
                    break
                }
        }

        return myResult;
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('DirectionId');
        this.myCloner.AddField('ShipperId');
        this.myCloner.AddField('ShipperName');
        this.myCloner.AddField('ShipperAddressId');
        this.myCloner.AddField('ShipperContactId');
        this.myCloner.AddField('ShipperReference1');
        this.myCloner.AddField('ShipperReference2');
        this.myCloner.AddField('ConsigneeId');
        this.myCloner.AddField('ConsigneeName');
        this.myCloner.AddField('ConsigneeAddressId');
        this.myCloner.AddField('ConsigneeContactId');
        this.myCloner.AddField('ConsigneeReference1');
        this.myCloner.AddField('ConsigneeReference2');
        this.myCloner.AddField('CustomerId');
        this.myCloner.AddField('CustomerName');
        this.myCloner.AddField('ShipmentCustomerTypeCode');
        this.myCloner.AddField('MainCarriageFromPortId');
        this.myCloner.AddField('MainCarriageFromToId');

        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var message: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors)

        if (this.IsCurrentInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
                errors.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.ShipperId)) {
                errors.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ShipperId")));
            }

            if (this.EntityPM.ShipmentLevelCode == "C") {
                errors.push("Master inland domestic are not allowed");
            }

            else if (this.EntityPM.ShipmentLevelCode == "H") {
                errors.push("House inland domestic shipments are not allowed");
            }

            if (this.EntityPM.ShipmentLevelCode != "C") {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipperId) && !AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
                    if (this.EntityPM.FromCountryId != this.EntityPM.ToCountryId) {
                        if (this.EntityPM.FromCountryIsEC == false || this.EntityPM.ToCountryIsEC == false) {
                            errors.push("Both Addresses must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }

        else {
            if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId)) {
                var textCode = ShipmentTool.GetFromPortTextCode(this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode);
                errors.push(message.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
                var textCode = ShipmentTool.GetToPortTextCode(this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode);
                errors.push(message.replace("%FieldName", TextCodeTranslator.Translate(textCode)));
            }

            if (this.EntityPM.DirectionId == "D") {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
                    if (this.EntityPM.FromCountryId != this.EntityPM.ToCountryId) {
                        if (this.EntityPM.FromCountryIsEC == false || this.EntityPM.ToCountryIsEC == false) {
                            errors.push("Both Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }

        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.CustomerId) || AppTool.IsNullOrEmpty(this.EntityPM.ShipmentCustomerTypeCode)) {
                errors.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.CustomerId")));
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.SetCustomerPartner();

            if (this.IsCurrentInlandDomestic) {
                if (this.ShipperAddressList != null) {
                    this.EntityPM.FromCountryId = this.ShipperAddressList.CountryId;
                    this.EntityPM.FromCountryIsEC = this.ShipperAddressList.CountryEC;
                }

                if (this.ConsigneeAddressList != null) {
                    this.EntityPM.ToCountryId = this.ConsigneeAddressList.CountryId;
                    this.EntityPM.ToCountryIsEC = this.ConsigneeAddressList.CountryEC;
                }

                this.EntityPM.IncludePickUp = false;
                this.EntityPM.IncludeDelivery = false;
                this.MainCarriageFromPortId = null;
                this.MainCarriageToPortId = null;
                this.EntityPM.MainCarriageFinalDestinationPortId = null;                
            
                var confirmWindow: ConfirmWindow = new ConfirmWindow();
                confirmWindow.Title = "Convert Shipment Direction";
                confirmWindow.Width = 400;
                confirmWindow.Show("Origin and Destination ports will be replaced by the Shipper/Consignee Addresses, proceed?");
                confirmWindow.YesButtonText = "Yes";
                confirmWindow.NoButtonText = "No";
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.CompleteConversion();
                    }
                });
            }

            else {
                this.CompleteConversion();
            }
        }
    }

    private CompleteConversion() {
        this.CurrentSession.StartBusyIndicatorSaving();

        var oldDirectionName: string = this.DirectionsList.filter(d => d.Code == this.oldShipmentDirection)[0].Name;
        var currentDirectionName: string = this.DirectionsList.filter(d => d.Code == this.DirectionId)[0].Name;

        this.EntityPM.EventNote = "Converted from [" + oldDirectionName + "] to [" + currentDirectionName + "]";
        this.EntityPM.ShipmentDirectionConverted = true;

        var service: ShipmentPMService = new ShipmentPMService();
        service.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}

export class ConvertDirectionArgs {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;    
    public EnabledOkButton: boolean = true;
    public ValidationErrorsList: Array<string> = [];
}

export class DirectionFilterItem {
    public Code: string;
    public Name: string;
    public SRC: string;
    constructor(code: string, name: string, src: string = null) {
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
}
