import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { UIProperty, UIProperties } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentUnassignedFieldPM } from '../../../Shipment/EntityPMs/ShipmentUnassignedFieldPM';
import { ShipmentPM } from '../../../Shipment/EntityPMs/ShipmentPM';
import { AppTool } from '../../../Infrastructure/Tools';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { NewEntityArgs } from '../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { AddressList } from '../../../Common/EntityLists/AddressList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CardListService } from '../../../Common/Services/StandardLists/CardListService';
import { AddressListService } from '../../../Common/Services/StandardLists/AddressListService';
import { CardList } from '../../../Common/EntityLists/CardList';
import { ShipmentDomainService } from '../../Services/ShipmentDomainService';

@Component({
    templateUrl: './UpdateUnassigedDataComponent.html',
})

export class UpdateUnassigedDataComponent extends BaseComponent {    
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext: UpdateUnassigedDataComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
    public IsInlandDomestic: boolean = false;
    public IsShipperVisible: boolean = false;
    public IsConsigneeVisible: boolean = false;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: ShipmentPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "Shipment";
        this.IsInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;

        this.LoadAddressesFromUnassignedXML();

        this.SetUIProperties();
        this.SetCardDependency();
        this.InitializeServices();        
        this.Clone();
    }

    private LoadAddressesFromUnassignedXML() {
        var shipmentDomainService: ShipmentDomainService = new ShipmentDomainService();
        shipmentDomainService.LoadAddressesFromUnassignedXML(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                
            }
        });
    }

    SetUIProperties() {
        this.IsShipperVisible = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Shipper").length > 0;
        this.IsConsigneeVisible = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Consignee").length > 0;

        this.UIProperties.SetEnabled("ShipperName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ConsigneeName", this.ObjectTableName, false);
    }

    private SetCardDependency() {
        this.CardDependencyProperty1 = "CS";
        this.CardDependencyProperty1IsList = false;

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }

        if (this.IsInlandDomestic) {
            this.CardDependencyProperty1 = this.CardDependencyProperty1 + ",WH";
            this.CardDependencyProperty1IsList = true;
        }
    }

    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
    }

    //UnassigedShipperAddressList
    //UnassigedConsigneeAddressList

    get ShipperName() { return this.EntityPM.ShipperName; }
    get UpdatedShipperId() { return this.EntityPM.ShipperId; }
    set UpdatedShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
            this.OnShipperChanged();
        }
    }

    private OnShipperChanged() {
        this.SetCustomer();

        if (AppTool.IsNullOrEmpty(this.UpdatedShipperId)) {
            this.SetShipperFields();
        }

        else {
            this.myCardListService.getSingle(this.UpdatedShipperId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myCardList: CardList = myResponse.Result;
                    if (myCardList) {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticFromTypeCode == "PART") {
                            this.EntityPM.MainCarriageFromPartnerId = this.UpdatedShipperId;
                            this.EntityPM.MainCarriageFromAddressId = myCardList.MainAddressId;
                        }

                        var unassignedShipper: ShipmentUnassignedFieldPM = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Shipper")[0];
                        if (unassignedShipper) {
                            unassignedShipper.ReplacedDataId = myCardList.Id;
                        }

                        this.EntityPM.ShipperId = myCardList.Id;
                        this.EntityPM.ShipperContactId = myCardList.PrimaryContactId;
                        this.EntityPM.ShipperName = myCardList.EnglishName;
                        this.EntityPM.ShipperNote = myCardList.Notes;
                        this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                        this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                        this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                        this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                        this.UpdatedShipperAddressId = myCardList.MainAddressId;
                    }
                }
            });
        }
    }

    private SetShipperFields() {
        this.EntityPM.ShipperContactId = null;
        this.EntityPM.ShipperName = null;
        this.EntityPM.ShipperNote = null;
        this.EntityPM.ShipperReference1 = null;
        this.EntityPM.ShipperReference2 = null;
        this.EntityPM.ShipperMainAddressId = null;
        this.EntityPM.ShipperPickAddressId = null;
        this.EntityPM.KnownConsignorNumber = null;
        this.EntityPM.KCExpirationDate = null;
        this.UpdatedShipperAddressId = null;
        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticFromTypeCode == "PART") {
            this.EntityPM.MainCarriageFromPartnerId = null;
            this.EntityPM.MainCarriageFromAddressId = null;
        }
    }

    get UpdatedShipperAddressId() { return this.EntityPM.ShipperAddressId; }
    set UpdatedShipperAddressId(newValue: string) {
        if (this.EntityPM.ShipperAddressId != newValue) {
            this.EntityPM.ShipperAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.UpdatedShipperAddressList = null;
            }

            else {
                this.LoadAddress(newValue, "S");
            }
        }
    }

    private myShipperAddressList: AddressList;
    get UpdatedShipperAddressList() { return this.myShipperAddressList; }
    set UpdatedShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;
    }

    get ConsigneeName() { return this.EntityPM.ConsigneeName; }
    get UpdatedConsigneeId() { return this.EntityPM.ConsigneeId; }
    set UpdatedConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;
            this.OnConsigneeChanged();
        }
    }

    private OnConsigneeChanged() {
        this.SetCustomer();
       
        if (AppTool.IsNullOrEmpty(this.UpdatedConsigneeId)) {
            this.SetConsigneeFields();            
        }

        else {
            this.myCardListService.getSingle(this.UpdatedConsigneeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myCardList: CardList = myResponse.Result;
                    if (myCardList) {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticToTypeCode == "PART") {
                            this.EntityPM.MainCarriageToPartnerId = this.UpdatedConsigneeId;
                            this.EntityPM.MainCarriageToAddressId = myCardList.MainAddressId;
                        }

                        var unassignedConsignee: ShipmentUnassignedFieldPM = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Consignee")[0];
                        if (unassignedConsignee) {
                            unassignedConsignee.ReplacedDataId = myCardList.Id;
                        }

                        this.EntityPM.ConsigneeId = myCardList.Id;
                        this.EntityPM.ConsigneeContactId = myCardList.PrimaryContactId;
                        this.EntityPM.ConsigneeName = myCardList.EnglishName;
                        this.EntityPM.ConsigneeNote = myCardList.Notes;
                        this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                        this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                        this.UpdatedConsigneeAddressId = myCardList.MainAddressId;
                    }
                }
            });
        }
    }

    private SetConsigneeFields() {
        this.EntityPM.ConsigneeContactId = null;
        this.EntityPM.ConsigneeName = null;
        this.EntityPM.ConsigneeNote = null;
        this.EntityPM.ConsigneeReference1 = null;
        this.EntityPM.ConsigneeReference2 = null;
        this.EntityPM.ConsigneeMainAddressId = null;
        this.EntityPM.ConsigneePickAddressId = null;
        this.UpdatedConsigneeAddressId = null;
        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticToTypeCode == "PART") {
            this.EntityPM.MainCarriageToPartnerId = null;
            this.EntityPM.MainCarriageToAddressId = null;
        }
    }

    get UpdatedConsigneeAddressId() { return this.EntityPM.ConsigneeAddressId; }
    set UpdatedConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeAddressId != newValue) {
            this.EntityPM.ConsigneeAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.UpdatedConsigneeAddressList = null;
            }

            else {
                this.LoadAddress(newValue, "C");                
            }
        }
    }

    private myConsigneeAddressList: AddressList;
    get UpdatedConsigneeAddressList() { return this.myConsigneeAddressList; }
    set UpdatedConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;
    }   

    private SetCustomer() {
        if (this.EntityPM.ShipmentCustomerTypeCode == "SHI") {
            this.EntityPM.CustomerId = this.UpdatedShipperId;
        }

        else if (this.EntityPM.ShipmentCustomerTypeCode == "CON") {
            this.EntityPM.CustomerId = this.UpdatedConsigneeId;
        }
    }

    LoadAddress(addressId: string, partner: string) {
        this.myAddressListService.getSingle(addressId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (partner == "S") {
                    this.UpdatedShipperAddressList = myResponse.Result;
                }

                else if (partner == "C") {
                    this.UpdatedConsigneeAddressList = myResponse.Result;
                }
            }
        });
    }

    AddPartnerClicked(myPartnerCode: string) {
        var myComponentPath: string = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        var title = "New " + myPartnerCode;
        var isShipperMyCustomer: boolean = this.EntityPM.ShipmentCustomerTypeCode == "SHI" ? true : false;
        var isConsigneeMyCustomer: boolean = this.EntityPM.ShipmentCustomerTypeCode == "CON" ? true : false;
        var isCustomer = isShipperMyCustomer || isConsigneeMyCustomer;

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
                    if (myPartnerCode == "Shipper") {
                        this.UpdatedShipperId = comp.EntityPM.Id;
                    }

                    else if (myPartnerCode == "Consignee") {
                        this.UpdatedConsigneeId = comp.EntityPM.Id;
                    }
                }
            });
        });
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    UpdateButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        //this.myCloner.AddField('CeficClass');
        //this.myCloner.AddField('KelmerCode');
        //this.myCloner.AddField('EMS');
        //this.myCloner.AddField('ProperShippingName');
        //this.myCloner.AddField('MarinePollutant');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
