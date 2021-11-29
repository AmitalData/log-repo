import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
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
import { UnassignedEntityListService } from '../../../Common/Services/StandardLists/UnassignedEntityListService';
import { UnassignedEntityList } from '../../../Common/EntityLists/UnassignedEntityList';
declare var window: any;

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
    private customerObjecTableId: string;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: ShipmentPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "Shipment";
        this.IsInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        this.customerObjecTableId = window.ObjectTables.filter(d => d.Name == "Customer")[0].Id;
        this.InitializeServices();
        this.SetUIProperties();
        this.SetCardDependency();
        this.LoadUnassignedEntities();

        if (this.IsShipperVisible) {
            this.myAddressListService.getSingle(this.EntityPM.ShipperAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ShipperAddressList = myResponse.Result;
                }
            });
        }

        if (this.IsConsigneeVisible) {
            this.myAddressListService.getSingle(this.EntityPM.ConsigneeAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ConsigneeAddressList = myResponse.Result;
                }
            });
        }

        this.Clone();
    }

    SetUIProperties() {
        this.IsShipperVisible = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Shipper" && AppTool.IsNullOrEmpty(d.ReplacedDataId)).length > 0;
        this.IsConsigneeVisible = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Consignee" && AppTool.IsNullOrEmpty(d.ReplacedDataId)).length > 0;

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

    private unassignedEntities: UnassignedEntityList[];
    private LoadUnassignedEntities() {
        this.unassignedEntities = [];
        var service: UnassignedEntityListService = new UnassignedEntityListService();
        service.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.unassignedEntities = myResponse.Result;
            }
        });
    }
    get ShipperName() { return this.EntityPM.ShipperName; }
    public ShipperAddressList: AddressList;
    private updatedShipperId: string;
    get UpdatedShipperId() { return this.updatedShipperId; }
    set UpdatedShipperId(newValue: string) {
        if (this.updatedShipperId != newValue) {
            this.updatedShipperId = newValue;
            this.OnShipperChanged();
        }
    }

    private shipperCard: CardList;
    private OnShipperChanged() {
        if (AppTool.IsNullOrEmpty(this.UpdatedShipperId)) {
            this.SetShipperFields();
        }

        else {
            this.myCardListService.getSingle(this.UpdatedShipperId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.shipperCard = myResponse.Result;
                    if (this.shipperCard) {
                        this.UpdatedShipperAddressId = this.shipperCard.MainAddressId;
                        this.UpdatedShipperContactId = this.shipperCard.PrimaryContactId; 
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

    private updatedShipperAddressId: string;
    get UpdatedShipperAddressId() { return this.updatedShipperAddressId; }
    set UpdatedShipperAddressId(newValue: string) {
        if (this.updatedShipperAddressId != newValue) {
            this.updatedShipperAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.UpdatedShipperAddressList = null;
            }

            else {
                this.LoadAddress(newValue, "S");
            }
        }
    }

    private updatedShipperContactId: string;
    get UpdatedShipperContactId() { return this.updatedShipperContactId; }
    set UpdatedShipperContactId(newValue: string) {
        if (this.updatedShipperContactId != newValue) {
            this.updatedShipperContactId = newValue;
        }
    }

    private myShipperAddressList: AddressList;
    get UpdatedShipperAddressList() { return this.myShipperAddressList; }
    set UpdatedShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;
    }

    get ConsigneeName() { return this.EntityPM.ConsigneeName; }
    public ConsigneeAddressList: AddressList;
    private updatedConsigneeId: string;
    get UpdatedConsigneeId() { return this.updatedConsigneeId; }
    set UpdatedConsigneeId(newValue: string) {
        if (this.updatedConsigneeId != newValue) {
            this.updatedConsigneeId = newValue;
            this.OnConsigneeChanged();
        }
    }

    private consigneeCard: CardList;
    private OnConsigneeChanged() {       
        if (AppTool.IsNullOrEmpty(this.UpdatedConsigneeId)) {
            this.SetConsigneeFields();            
        }

        else {
            this.myCardListService.getSingle(this.UpdatedConsigneeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.consigneeCard = myResponse.Result;
                    if (this.consigneeCard) {
                        this.UpdatedConsigneeAddressId = this.consigneeCard.MainAddressId;
                        this.UpdatedConsigneeContactId = this.consigneeCard.PrimaryContactId;                        
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

    private updatedConsigneeAddressId: string;
    get UpdatedConsigneeAddressId() { return this.updatedConsigneeAddressId; }
    set UpdatedConsigneeAddressId(newValue: string) {
        if (this.updatedConsigneeAddressId != newValue) {
            this.updatedConsigneeAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.UpdatedConsigneeAddressList = null;
            }

            else {
                this.LoadAddress(newValue, "C");                
            }
        }
    }

    private updatedConsigneeContactId: string;
    get UpdatedConsigneeContactId() { return this.updatedConsigneeContactId; }
    set UpdatedConsigneeContactId(newValue: string) {
        if (this.updatedConsigneeContactId != newValue) {
            this.updatedConsigneeContactId = newValue;
        }
    }

    private myConsigneeAddressList: AddressList;
    get UpdatedConsigneeAddressList() { return this.myConsigneeAddressList; }
    set UpdatedConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;
    }   

    private SetCustomer() {
        if (this.EntityPM.ShipmentCustomerTypeCode == "SHI" && this.IsShipperVisible) {
            this.EntityPM.CustomerId = this.UpdatedShipperId;
        }

        else if (this.EntityPM.ShipmentCustomerTypeCode == "CON" && this.IsConsigneeVisible) {
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
        var shipmentDomainService: ShipmentDomainService = new ShipmentDomainService();
        shipmentDomainService.LoadAddresseFromUnassignedXML(this.EntityPM.Id, myPartnerCode).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var address: AddressList = myResponse.Result;
                var myComponentPath: string = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
                var title = "New " + myPartnerCode;
                var isShipperMyCustomer: boolean = this.EntityPM.ShipmentCustomerTypeCode == "SHI" ? true : false;
                var isConsigneeMyCustomer: boolean = this.EntityPM.ShipmentCustomerTypeCode == "CON" ? true : false;
                var isCustomer = isShipperMyCustomer || isConsigneeMyCustomer;

                var args = new NewEntityArgs();
                if (!isCustomer) {
                    args.Perspective = "ShippersAndConsignees";
                }

                args.Address = address;
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
            this.SetCustomer();

            if (this.IsShipperVisible && !AppTool.IsNullOrEmpty(this.UpdatedShipperId)) {
                this.UpdateShipper();                
            }

            if (this.IsConsigneeVisible && !AppTool.IsNullOrEmpty(this.UpdatedConsigneeId)) {
                this.UpdateConsignee();                
            }

            this.ComputeHasUnassignedField();
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }
    private UpdateShipper() {
        this.EntityPM.ShipperId = this.UpdatedShipperId;
        this.EntityPM.ShipperAddressId = this.UpdatedShipperAddressId;
        this.EntityPM.ShipperContactId = this.UpdatedShipperContactId;

        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticFromTypeCode == "PART") {
            this.EntityPM.MainCarriageFromPartnerId = this.EntityPM.ShipperId;
            this.EntityPM.MainCarriageFromAddressId = this.EntityPM.ShipperAddressId;
        }

        if (this.shipperCard) {
            var unassignedShipper: ShipmentUnassignedFieldPM = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Shipper")[0];
            if (unassignedShipper
                && unassignedShipper.ReplacedDataId != this.EntityPM.ShipperId
                && this.unassignedEntities.filter(d => d.ObjectTableId == this.customerObjecTableId && d.UnassignedCode == this.shipperCard.Code).length == 0) {
                unassignedShipper.ReplacedDataId = this.EntityPM.ShipperId;
            }

            this.EntityPM.ShipperName = this.shipperCard.EnglishName;
            this.EntityPM.ShipperNote = this.shipperCard.Notes;
            this.EntityPM.ShipperMainAddressId = this.shipperCard.MainAddressId;
            this.EntityPM.ShipperPickAddressId = this.shipperCard.PickAddressId;
        }
    }
    private UpdateConsignee() {
        this.EntityPM.ConsigneeId = this.UpdatedConsigneeId;
        this.EntityPM.ConsigneeAddressId = this.UpdatedConsigneeAddressId;
        this.EntityPM.ConsigneeContactId = this.UpdatedConsigneeContactId;

        if (AppTool.IsNullOrEmpty(this.EntityPM.StandalonePickupDeliveryId) && this.IsInlandDomestic && this.EntityPM.InlandDomesticToTypeCode == "PART") {
            this.EntityPM.MainCarriageToPartnerId = this.UpdatedConsigneeId;
            this.EntityPM.MainCarriageToAddressId = this.EntityPM.ConsigneeAddressId;
        }

        if (this.consigneeCard) {
            var unassignedConsignee: ShipmentUnassignedFieldPM = this.EntityPM.ShipmentUnassignedFields.filter(d => d.FieldName == "Consignee")[0];
            if (unassignedConsignee
                && unassignedConsignee.ReplacedDataId != this.EntityPM.ConsigneeId
                && this.unassignedEntities.filter(d => d.ObjectTableId == this.customerObjecTableId && d.UnassignedCode == this.consigneeCard.Code).length == 0) {
                unassignedConsignee.ReplacedDataId = this.EntityPM.ConsigneeId;
            }

            this.EntityPM.ConsigneeName = this.consigneeCard.EnglishName;
            this.EntityPM.ConsigneeNote = this.consigneeCard.Notes;
            this.EntityPM.ConsigneeMainAddressId = this.consigneeCard.MainAddressId;
            this.EntityPM.ConsigneePickAddressId = this.consigneeCard.PickAddressId;
        }
    }
    private ComputeHasUnassignedField() {
        this.EntityPM.HasUnassignedData = false;

        var myList: ShipmentUnassignedFieldPM[] = this.EntityPM.ShipmentUnassignedFields.filter(s => AppTool.IsNullOrEmpty(s.ReplacedDataId));
        if (myList.length > 0) {
            this.EntityPM.HasUnassignedData = true;
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
