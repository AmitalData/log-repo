/// <reference path="../../../tools.ts" />
import {Component, OnInit, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';      
import {WarehouseEntryPM} from '../../../EntityPMs/WarehouseEntryPM';
import {PortList} from '../../../../Common/EntityLists/PortList';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';

@Component({
    selector: 'WarehouseEntryRoutingsTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseEntryRoutingsTabComponent.html',
})

export class WarehouseEntryRoutingsTabComponent extends BaseComponent {

    public EntityPM: WarehouseEntryPM;
    public ObjectTableName: string = null;
    public DataContext = this;
    public IsInlandDomestic: boolean = false;
    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
    ShipperPartnerTypeId: string;
    ConsigneePartnerTypeId: string;
    public FromPortText = ""; 
    public ToPortText = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.Listen();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }
        this.Initialize();

        this.LoadFromAddress();
        this.LoadToAddress();

       


    }
    IswarehouseEntryConnectedToShipment: boolean = false;
    ngOnInit() {
        if (this.EntityPM != null) {
            this.IsInlandDomestic = this.IsInlandDomesticWarehouse(this.EntityPM);


            if (this.EntityPM.ConnectedToShipment) {
                this.IswarehouseEntryConnectedToShipment = true;
                if (this.IsInlandDomestic) {
                    this.UIProperties.SetEnabled("FromPartnerId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("ToPartnerId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);

                    this.UIProperties.SetEnabled("FromAddressId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("ToAddressId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);

                }
                else {
                    this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                }
            }

        }
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                  
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });
        }
    }

    IsInlandDomesticWarehouse(entityPM: WarehouseEntryPM) {
        var isInlandDomestic = false;

        if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I") {
            isInlandDomestic = true;
        }

        return isInlandDomestic;
    }


    private myPortListService: PortListService;
    private myAddressListService: AddressListService;
    private myCardListService: CardListService;
    Initialize() {
        this.myPortListService = new PortListService();
        this.myAddressListService = new AddressListService();
        this.myCardListService = new CardListService();
    }

    public FromPortList: PortList = null;
    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(value: string) {
        if (this.EntityPM.FromPortId != value) {
            this.EntityPM.FromPortId = value;
            this.SetUIProperties_Ports();
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


    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;
        }
    }

    get TruckerReference() { return this.EntityPM.TruckerReference; }
    set TruckerReference(newValue: string) {
        if (this.EntityPM.TruckerReference != newValue) {
            this.EntityPM.TruckerReference = newValue;

        }
    }
    get TruckerId() { return this.EntityPM.TruckerId; }
    set TruckerId(newValue: string) {
        if (this.EntityPM.TruckerId != newValue) {
            this.EntityPM.TruckerId = newValue;

        }
    }

    get FromPartnerId() {

        return !AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerId) ? this.EntityPM.FromPartnerId : this.EntityPM.ShipperId;


    }
    set FromPartnerId(value: string) {
        if (this.EntityPM.FromPartnerId != value) {
            this.EntityPM.FromPartnerId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.FromAddressId = null;
                this.ShipperPartnerTypeId = null;
            }
            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.FromAddressId = list.MainAddressId;
                            this.ShipperPartnerTypeId = list.PartnerTypeId;
                        }
                    }
                });
            }
        }
    }

    get  FromAddressId() { return this.EntityPM. FromAddressId; }
    set  FromAddressId(value: string) {
        if (this.EntityPM.FromAddressId != value) {
            this.EntityPM.FromAddressId = value;
            this.LoadFromAddress();
        }
    }

    get ToPartnerId() { return !AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerId) ? this.EntityPM.ToPartnerId : this.EntityPM.ConsigneeId; }
    set ToPartnerId(value: string) {
        if (this.EntityPM.ToPartnerId != value) {
            this.EntityPM.ToPartnerId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddressId = null;
                this.ConsigneePartnerTypeId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.ToAddressId = list.MainAddressId;
                            this.ConsigneePartnerTypeId = list.PartnerTypeId;
                        }
                    }
                });
            }
        }
    }

    get  ToAddressId() { return this.EntityPM. ToAddressId; }
    set  ToAddressId(value: string) {
        if (this.EntityPM. ToAddressId != value) {
            this.EntityPM. ToAddressId = value;
            this.LoadToAddress();
        }
    }

    public ToPortList: PortList = null;
    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.EntityPM.ToPortId != value) {
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_Ports();

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
    SetUIProperties_Ports() {
        var isFromRequired: boolean = false;
        var isToRequired: boolean = false;

        var isShipperIdRequired: boolean = false;
        var isConsigneeIdRequired: boolean = false;
        var isCustomerIdRequired: boolean = false;

        if (!this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.FromPortId)) {
                isFromRequired = true;
            }

            if (AppTool.IsNullOrEmpty(this.ToPortId)) {
                isToRequired = true;
            }

        }

        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isToRequired);
    }
    
    public ToAddressList: AddressList;
    public FromAddressList: AddressList;
    private LoadToAddress() {
        if (AppTool.IsNullOrEmpty(this. ToAddressId)) {
            this.ToAddressList = null;
        }

        else {
            this.myAddressListService.getSingle(this. ToAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AddressList = myResponse.Result;
                    if (list) {
                        this.ToAddressList = list;
                    }
                }
            });
        }
    }
    private LoadFromAddress() {
        if (AppTool.IsNullOrEmpty(this. FromAddressId)) {
            this.FromAddressList = null;
        }

        else {
            this.myAddressListService.getSingle(this. FromAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AddressList = myResponse.Result;
                    if (list) {
                        this.FromAddressList = list;
                    }
                }
            });
        }
    }

    AddAddressClicked(myAddressCode: string) {
        var entityPM: AddressPM = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;
        var IsShipperMyCustomer;
        if (this.EntityPM.DirectionId == "I") {
            IsShipperMyCustomer = false;
        }
        else {
            IsShipperMyCustomer = true;
        }

        switch (myAddressCode) {
            case "F": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.FromPartnerId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer =IsShipperMyCustomer;
                break;
            }

            case "T": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ToPartnerId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !IsShipperMyCustomer;
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
                        case "F": {
                            this.FromAddressId = null;
                            this.FromAddressId = entityPM.Id;
                            break;
                        }

                        case "T": {
                            this.ToAddressId = null;
                            this.ToAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    }
    EditAddressClicked(myAddressCode: string) {

        var myAddressId: string = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;
        var isShipperMyCustomer;
        if (this.EntityPM.DirectionId == "I") {
            isShipperMyCustomer = false;
        }
        else {
            isShipperMyCustomer = true;
        }

        switch (myAddressCode) {
            case "F": {
                myAddressId = this.FromAddressId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = isShipperMyCustomer;
                break;
            }

            case "T": {
                myAddressId = this.ToAddressId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !isShipperMyCustomer;
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
                        case "F": {
                            this.FromAddressId = null;
                            this.FromAddressId = myAddressId;
                            break;
                        }

                        case "T": {
                            this.ToAddressId = null;
                            this.ToAddressId = myAddressId;
                            break;
                        }
                    }
                }
            });
        }
    }
}
