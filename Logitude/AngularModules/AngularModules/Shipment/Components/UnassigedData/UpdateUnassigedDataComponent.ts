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
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: ShipmentPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "Shipment";
        this.IsInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        this.SetUIProperties();
        this.SetCardDependency();
        this.InitializeServices();
        this.LoadAddress(this.EntityPM.ShipperAddressId, "S");
        this.LoadAddress(this.EntityPM.ConsigneeAddressId, "C");  
        this.Clone();
    }

    SetUIProperties() {
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

    //UnassigedShipperName
    //UnassigedConsigneeName

    //UnassigedShipperAddressList
    //UnassigedConsigneeAddressList

    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
            this.OnShipperChanged();
        }
    }

    private OnShipperChanged() {
        this.SetCustomer();

        if (AppTool.IsNullOrEmpty(this.ShipperId)) {
            this.SetShipperFields();
        }

        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myCardList: CardList = myResponse.Result;
                    if (myCardList) {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticFromTypeCode == "PART") {
                            this.EntityPM.MainCarriageFromPartnerId = this.ShipperId;
                            this.EntityPM.MainCarriageFromAddressId = myCardList.MainAddressId;
                        }

                        this.EntityPM.ShipperContactId = myCardList.PrimaryContactId;
                        this.EntityPM.ShipperName = myCardList.EnglishName;
                        this.EntityPM.ShipperNote = myCardList.Notes;
                        this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                        this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                        this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                        this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                        this.ShipperAddressId = myCardList.MainAddressId;
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
        this.ShipperAddressId = null;
        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticFromTypeCode == "PART") {
            this.EntityPM.MainCarriageFromPartnerId = null;
            this.EntityPM.MainCarriageFromAddressId = null;
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
                this.LoadAddress(newValue, "S");
            }
        }
    }

    private myShipperAddressList: AddressList;
    get ShipperAddressList() { return this.myShipperAddressList; }
    set ShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;
    }

    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;
            this.OnConsigneeChanged();
        }
    }

    private OnConsigneeChanged() {
        this.SetCustomer();
       
        if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            this.SetConsigneeFields();            
        }

        else {
            this.myCardListService.getSingle(this.ConsigneeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myCardList: CardList = myResponse.Result;
                    if (myCardList) {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticToTypeCode == "PART") {
                            this.EntityPM.MainCarriageToPartnerId = this.ConsigneeId;
                            this.EntityPM.MainCarriageToAddressId = myCardList.MainAddressId;
                        }

                        this.EntityPM.ConsigneeContactId = myCardList.PrimaryContactId;
                        this.EntityPM.ConsigneeName = myCardList.EnglishName;
                        this.EntityPM.ConsigneeNote = myCardList.Notes;
                        this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                        this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                        this.ConsigneeAddressId = myCardList.MainAddressId;
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
        this.ConsigneeAddressId = null;
        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticToTypeCode == "PART") {
            this.EntityPM.MainCarriageToPartnerId = null;
            this.EntityPM.MainCarriageToAddressId = null;
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
                this.LoadAddress(newValue, "C");                
            }
        }
    }

    private myConsigneeAddressList: AddressList;
    get ConsigneeAddressList() { return this.myConsigneeAddressList; }
    set ConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;
    }   

    private SetCustomer() {
        if (this.EntityPM.ShipmentCustomerTypeCode == "SHI") {
            this.EntityPM.CustomerId = this.ShipperId;
        }

        else if (this.EntityPM.ShipmentCustomerTypeCode == "CON") {
            this.EntityPM.CustomerId = this.ConsigneeId;
        }
    }

    LoadAddress(addressId: string, partner: string) {
        this.myAddressListService.getSingle(addressId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (partner == "S") {
                    this.ShipperAddressList = myResponse.Result;
                }

                else if (partner == "C") {
                    this.ConsigneeAddressList = myResponse.Result;
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
                        this.ShipperId = comp.EntityPM.Id;
                    }

                    else if (myPartnerCode == "Consignee") {
                        this.ConsigneeId = comp.EntityPM.Id;
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
