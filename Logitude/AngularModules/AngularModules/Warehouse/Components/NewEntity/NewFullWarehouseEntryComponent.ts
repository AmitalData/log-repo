

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseEntryPM} from '../../../Warehouse/EntityPMs/WarehouseEntryPM';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {WarehouseEntryPackagePM} from '../../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {AddressList} from '../../../Common/EntityLists/AddressList'; 
import {AddressListService} from '../../../Common/Services/StandardLists/AddressListService';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../Common/EntityLists/CardList';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {AddressPM} from '../../../Common/EntityPMs/AddressPM';
import {PortList} from '../../../Common/EntityLists/PortList';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {WarehouseHelper} from '../../Helpers/WarehouseHelper';
import {FilterClass} from '../../../Shipment/Components/NewEntity/NewShipmentComponent';
import {PortListService} from '../../../Common/Services/StandardLists/PortListService';

@Component({
    
    selector: 'NewFullWarehouseEntryComponent',
    templateUrl: './NewFullWarehouseEntryComponent.html',


})

export class NewFullWarehouseEntryComponent extends BaseComponent implements OnInit {
    private myAddressListService: AddressListService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private myCardListService: CardListService;
    private myPortListService: PortListService;



    public DirectionsList: FilterClass[] = [];
    public TransportModesList: FilterClass[] = [];
    public ShipmentTypesList: FilterClass[] = [];
    public ShipmentLevelsList: FilterClass[] = [];
    IsLCLEntity: boolean = false;
    ShipmentLevelCode: string = "";
    public ShowShipmentLevels: boolean = true;
    public ControlColumnWidth: number = 220;

    ObjectTableName: string = "WarehouseEntry";
    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
    public DataContext = this;
    public LabelColumnWidth: number = 117;
    IsConsigneeMyCustomer: boolean = false;

    CustomerHelpText: string = "Indicates who the customer is, so that Logitude knows to refer to the relevant partner for statistics, billing and shared logistics. For Export, the Shipper is selected automatically. For Import, the Consignee is selected automatically.";
    WarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];
    public ValidationErrorsList: string[];

    warehouseEntryPM: WarehouseEntryPM = new WarehouseEntryPM();
    IsFromShipment: boolean = false;
    SelectedWarehouseEntryPackage: WarehouseEntryPackagePM;
    validator: ClassLevelValidator;
    ObjectTableId: string;

    IsLoadPage: boolean = false;
    warehouseHelper: WarehouseHelper = new WarehouseHelper();
    IsInlandDomestic: boolean = false;
    public SessionIndex: number;


    public FromPortText: string;
    public ToPortText: string;
    public CarrierTextCode: string;
    public CarrierNumberTextCode: string;
    public CarrierDependencyProperty1: string;
    public MasterTextCode: string;
    public HouseTextCode: string;
    public Volume: string;
    ShipperPartnerTypeId: string;
    ConsigneePartnerTypeId: string;
    public ScreenOpacity: number = 0.7;
    public IsScreenEnabled: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SessionIndex = SessionLocator.Index;
        this.warehouseEntryPM = this.warehouseHelper.GetNewWarehouseEntry(this);
        this.validator = new ClassLevelValidator();
   


        var table = window.ObjectTables.filter(d => d.Name == "WarehouseEntry")[0];
        if (table) this.ObjectTableId = table.Id;

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }

        this.InitializeServices();

      
    } ngOnInit() { }


    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myPortListService = new PortListService();
        
    }



    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry").subscribe((response:any) => {
            this.Start(args);
        });
    }

    Start(args: any) {

        if (args) {

            this.ShowShipmentLevels = false;
        
         
            this.WarehouseEntryPackagesLists = args.WarehouseEntryPackagesLists;
            if (!this.WarehouseEntryPackagesLists) this.WarehouseEntryPackagesLists = [];

            this.SetValue(args);
            this.BuildFiltersLists();
            this.OnFiltersChanged();

            this.IsLoadPage = true;

        }
    }


    private isAddShipperButtonDisabled: boolean = false;
    get IsAddShipperButtonDisabled() {
        if (!this.IsScreenEnabled || (this.IsShipperMyCustomer && !this.IsInlandDomestic )) {
            this.isAddShipperButtonDisabled = true;
        } else this.isAddShipperButtonDisabled = false;

        return this.isAddShipperButtonDisabled;


    }


    private isAddConsigneeButtonDisabled: boolean = false;
    get IsAddConsigneeButtonDisabled() {
        if (!this.IsScreenEnabled || (!this.IsShipperMyCustomer && !this.IsInlandDomestic)) {
            this.isAddConsigneeButtonDisabled = true;
        } else this.isAddConsigneeButtonDisabled = false;

        return this.isAddConsigneeButtonDisabled;


    }





    
    get DirectionId() { return this.warehouseEntryPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.warehouseEntryPM.DirectionId != newValue) {
            this.warehouseEntryPM.DirectionId = newValue;
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            this.OnFiltersChanged();
        }
    }

    get TransportModeId() { return this.warehouseEntryPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.warehouseEntryPM.TransportModeId != newValue) {
            this.warehouseEntryPM.TransportModeId = newValue;
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            this.FromPortId = null;
            this.ToPortId = null;
            this.ShipmentTypeId = null;
            this.OnFiltersChanged();
            this.BuildShipmentTypes();
            this.SetChargeableWeightUnit();
        }
    }

    SetChargeableWeightUnit() {
        this.warehouseEntryPM.ChargeableWeightUnitCode = AppTool.GetChargeableWeightUnitCode(this.TransportModeId);
    }

    public ShipmentTypeName: string = null;
    get ShipmentTypeId() { return this.warehouseEntryPM.ShipmentTypeId; }
    set ShipmentTypeId(newValue: string) {
        if (this.warehouseEntryPM.ShipmentTypeId != newValue) {
            this.warehouseEntryPM.ShipmentTypeId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipmentTypeName = null;
            }

            else {
                var item = this.ShipmentTypesList.filter(f => f.Code == newValue)[0];
                if (item) {
                    this.ShipmentTypeName = item.Name;
                }
            }

          
            this.OnFiltersChanged();
        }
    }


    private isTransportModesListEnabled: boolean = false;
    get IsTransportModesListEnabled() { return this.isTransportModesListEnabled; }
    set IsTransportModesListEnabled(value: boolean) {
        if (this.isTransportModesListEnabled != value) {
            this.isTransportModesListEnabled = value;
        }
    }


    get ShipperId() {

        return this.warehouseEntryPM.ShipperId;
    }
    set ShipperId(newValue: string) {
        if (this.warehouseEntryPM.ShipperId != newValue) {
            this.warehouseEntryPM.ShipperId = newValue;
            this.warehouseEntryPM.FromPartnerId = newValue;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperName = null;
                this.FromAddressId = null;
                this.ShipperPartnerTypeId = null;
                if (this.IsInlandDomestic && this.IsShipperMyCustomer) {
                    this.CustomerId = null;
                }
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        this.FromAddressId = myCardList.MainAddressId;
                        this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                        this.ShipperName = myCardList.EnglishName;

                        if (myCardList) {

                           // if (this.IsInlandDomestic) {
                                this.FullShipperConsignee();
                           // }

                           
                        }
                    }
                });
            }
        }
    }



    get ShipperName() { return this.warehouseEntryPM.ShipperName; }
    set ShipperName(newValue: string) {
        if (this.warehouseEntryPM.ShipperName != newValue) {
            this.warehouseEntryPM.ShipperName = newValue;

        }
    }





    get FromAddressId() { return this.warehouseEntryPM.FromAddressId; }
    set FromAddressId(newValue: string) {
        if (this.warehouseEntryPM.FromAddressId != newValue) {
            this.warehouseEntryPM.FromAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.ShipperAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    private myShipperAddressList: AddressList;
    get ShipperAddressList() { return this.myShipperAddressList; }
    set ShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;

    }


    get ConsigneeId() { return this.warehouseEntryPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.warehouseEntryPM.ConsigneeId != newValue) {
            this.warehouseEntryPM.ConsigneeId = newValue;
            this.warehouseEntryPM.ToPartnerId = newValue;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ToAddressId = null;
                this.ConsigneePartnerTypeId = null;
                this.ConsigneeName = null;
                if (this.IsInlandDomestic && !this.IsShipperMyCustomer) {
                    this.CustomerId = null;
                }
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.ToAddressId = myCardList.MainAddressId;
                            this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                            this.ConsigneeName = myCardList.EnglishName;
                          //  if (this.IsInlandDomestic) {
                                this.FullShipperConsignee();
                           // }
                        }
                    }
                });
            }
        }
    }


    get ConsigneeName() { return this.warehouseEntryPM.ConsigneeName; }
    set ConsigneeName(newValue: string) {
        if (this.warehouseEntryPM.ConsigneeName != newValue) {
            this.warehouseEntryPM.ConsigneeName = newValue;

        }
    }


    get ToAddressId() { return this.warehouseEntryPM.ToAddressId; }
    set ToAddressId(newValue: string) {
        if (this.warehouseEntryPM.ToAddressId != newValue) {
            this.warehouseEntryPM.ToAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ConsigneeAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.ConsigneeAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    private myConsigneeAddressList: AddressList;
    get ConsigneeAddressList() { return this.myConsigneeAddressList; }
    set ConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;

    }

    private CustomerPartnerTypeId: string;
    private CustomerIsCustomer: boolean;
    get CustomerId() { return this.warehouseEntryPM.CustomerId; }
    set CustomerId(newValue: string) {
        if (this.warehouseEntryPM.CustomerId != newValue) {
            this.warehouseEntryPM.CustomerId = newValue;
            this.FullShipperConsignee();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.CustomerPartnerTypeId = null;
                this.CustomerContactId = null;
                this.warehouseEntryPM.CustomerName = null;
                
                this.CustomerAddressId = null;

            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.CustomerPartnerTypeId = myCardList.PartnerTypeId;
                            this.CustomerContactId = myCardList.PrimaryContactId;
                            this.warehouseEntryPM.CustomerName = myCardList.EnglishName;
                            this.CustomerAddressId = myCardList.MainAddressId;
 
                        }
                    }
                });
            }
        }
    }

    private customerAddressId: string = null;
    get CustomerAddressId() { return this.customerAddressId; }
    set CustomerAddressId(newValue: string) {
        if (this.customerAddressId != newValue) {
            this.customerAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.CustomerAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.CustomerAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    private myCustomerAddressList: AddressList;
    get CustomerAddressList() { return this.myCustomerAddressList; }
    set CustomerAddressList(newValue: AddressList) {
        this.myCustomerAddressList = newValue;
       
    }


    private customerContactId: string = null;
    get CustomerContactId() { return this.customerContactId; }
    set CustomerContactId(newValue: string) {
        if (this.customerContactId != newValue) {
            this.customerContactId= newValue;
        }
    }

    get CustomerRef1() { return this.warehouseEntryPM.CustomerRef1; }
    set CustomerRef1(newValue: string) {
        if (this.warehouseEntryPM.CustomerRef1 != newValue) {
            this.warehouseEntryPM.CustomerRef1 = newValue;
        }
    }

    get CustomerRef2() { return this.warehouseEntryPM.CustomerRef2; }
    set CustomerRef2(newValue: string) {
        if (this.warehouseEntryPM.CustomerRef2 != newValue) {
            this.warehouseEntryPM.CustomerRef2 = newValue;

        }
    }

    
    private isShipperMyCustomer: boolean = true;
    get IsShipperMyCustomer() { return this.isShipperMyCustomer; }
    set IsShipperMyCustomer(newValue: boolean) {
        if (this.isShipperMyCustomer != newValue) {
            this.isShipperMyCustomer = newValue;
            this.FullShipperConsignee(true);

        }
    }
    
    get TruckerReference() { return this.warehouseEntryPM.TruckerReference; }
    set TruckerReference(newValue: string) {
        if (this.warehouseEntryPM.TruckerReference != newValue) {
            this.warehouseEntryPM.TruckerReference = newValue;

        }
    }
   
    get TruckerId() { return this.warehouseEntryPM.TruckerId; }
    set TruckerId(newValue: string) {
        if (this.warehouseEntryPM.TruckerId != newValue) {
            this.warehouseEntryPM.TruckerId = newValue;

        }
    }


    get Manufacturer() { return this.warehouseEntryPM.Manufacturer; }
    set Manufacturer(newValue: string) {
        if (this.warehouseEntryPM.Manufacturer != newValue) {
            this.warehouseEntryPM.Manufacturer = newValue;

        }
    }






    get ToCountryId() { return this.warehouseEntryPM.ToCountryId; }
    set ToCountryId(newValue: string) {
        if (this.warehouseEntryPM.ToCountryId != newValue) {
            this.warehouseEntryPM.ToCountryId = newValue;
        }
    }


    get FromCountryId() { return this.warehouseEntryPM.FromCountryId; }
    set FromCountryId(newValue: string) {
        if (this.warehouseEntryPM.FromCountryId != newValue) {
            this.warehouseEntryPM.FromCountryId = newValue;
        }
    }




    public FromPortList: PortList = null;
    get FromPortId() { return this.warehouseEntryPM.FromPortId; }
    set FromPortId(value: string) {
        if (this.warehouseEntryPM.FromPortId != value) {
            this.warehouseEntryPM.FromPortId = value;
            this.SetUIProperties();
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
    get ToPortId() { return this.warehouseEntryPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.warehouseEntryPM.ToPortId != value) {
            this.warehouseEntryPM.ToPortId = value;
            this.SetUIProperties();
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

    get ReceivedBy() { return this.warehouseEntryPM.ReceivedBy; }
    set ReceivedBy(newValue: string) {
        if (this.warehouseEntryPM.ReceivedBy != newValue) {
            this.warehouseEntryPM.ReceivedBy = newValue;

        }
    }

    get MasterNumber() { return this.warehouseEntryPM.MasterNumber; }
    set MasterNumber(newValue: string) {
        if (this.warehouseEntryPM.MasterNumber != newValue) {
            this.warehouseEntryPM.MasterNumber = newValue;

        }
    }

    get HouseNumber() { return this.warehouseEntryPM.HouseNumber; }
    set HouseNumber(newValue: string) {
        if (this.warehouseEntryPM.HouseNumber != newValue) {
            this.warehouseEntryPM.HouseNumber = newValue;

        }
    }
    get Notes() { return this.warehouseEntryPM.Notes; }
    set Notes(newValue: string) {
        if (this.warehouseEntryPM.Notes != newValue) {
            this.warehouseEntryPM.Notes = newValue;

        }
    }

    get WarehouseId() { return this.warehouseEntryPM.WarehouseId; }
    set WarehouseId(newValue: string) {
        if (this.warehouseEntryPM.WarehouseId != newValue) {
            this.warehouseEntryPM.WarehouseId = newValue;

        }
    }


    get EntryReference() { return this.warehouseEntryPM.EntryReference; }
    set EntryReference(newValue: string) {
        if (this.warehouseEntryPM.EntryReference != newValue) {
            this.warehouseEntryPM.EntryReference = newValue;

        }
    }

    get FromTypeCode() { return this.warehouseEntryPM.FromTypeCode; }
    set FromTypeCode(newValue: string) {
        if (this.warehouseEntryPM.FromTypeCode != newValue) {
            this.warehouseEntryPM.FromTypeCode = newValue;
            this.FromPortId = null;
            this.FromCountryId = null;

        }
    }

    get ToTypeCode() { return this.warehouseEntryPM.ToTypeCode; }
    set ToTypeCode(newValue: string) {
        if (this.warehouseEntryPM.ToTypeCode != newValue) {
            this.warehouseEntryPM.ToTypeCode = newValue;
            this.ToPortId = null;
            this.ToCountryId = null;
        }
    }


    







    get ActualEntryDate() { return this.warehouseEntryPM.ActualEntryDate; }
    set ActualEntryDate(newValue: Date) {
        if (this.warehouseEntryPM.ActualEntryDate != newValue) {
            this.warehouseEntryPM.ActualEntryDate = newValue;
            this.OnActualEntryDateDatePickerChange(newValue);

        }
    }

    get ExpectedEntryDate() { return this.warehouseEntryPM.ExpectedEntryDate; }
    set ExpectedEntryDate(newValue: Date) {
        if (this.warehouseEntryPM.ExpectedEntryDate != newValue) {
            this.warehouseEntryPM.ExpectedEntryDate = newValue;

        }
    }


    get SpecialInstruction() { return this.warehouseEntryPM.SpecialInstruction; }
    set SpecialInstruction(newValue: string) {
        if (this.warehouseEntryPM.SpecialInstruction != newValue) {
            this.warehouseEntryPM.SpecialInstruction = newValue;

        }
    }


    get TotalVolume() { return this.warehouseEntryPM.TotalVolume; }
    set TotalVolume(newValue: number) {
        if (this.warehouseEntryPM.TotalVolume != newValue) {
            this.warehouseEntryPM.TotalVolume = newValue;

        }
    }

    get TotalGrossWeight() { return this.warehouseEntryPM.TotalGrossWeight; }
    set TotalGrossWeight(newValue: number) {
        if (this.warehouseEntryPM.TotalGrossWeight != newValue) {
            this.warehouseEntryPM.TotalGrossWeight = newValue;

        }
    }

    get TotalPieces() { return this.warehouseEntryPM.TotalPieces; }
    set TotalPieces(newValue: number) {
        if (this.warehouseEntryPM.TotalPieces != newValue) {
            this.warehouseEntryPM.TotalPieces = newValue;

        }
    }



    get QuantityLabel() {
        var quantityLabel: string = "";
        if (this.IsLCLEntity) {
            quantityLabel = "Number of Packages";
        } else quantityLabel = "Number of Containers";

        return quantityLabel;

    }
   

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

        var isAirExportOnly: boolean = false;
    

            this.DirectionsList.push(direct_E);
            this.DirectionsList.push(direct_I);
            this.DirectionsList.push(direct_D);
            this.DirectionsList.push(direct_R);
            this.TransportModesList.push(transport_A);
            this.TransportModesList.push(transport_O);
            this.TransportModesList.push(transport_I);
        

        this.BuildShipmentTypes();
        this.BuildShipmentLevels();
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
    }

    BuildShipmentLevels() {
        this.ShipmentLevelsList = [];

        if (this.ShowShipmentLevels) {
            this.ShipmentLevelsList.push(new FilterClass("D", "Direct"));
            this.ShipmentLevelsList.push(new FilterClass("H", "House"));
        }
    }

    FullShipperConsignee(emptyShipmentConsignee: boolean =false) {

        //if (!this.IsInlandDomestic) {
        //    if (emptyShipmentConsignee) {
        //        this.ShipperId = null;
        //        this.ConsigneeId = null;
        //    }

        //    if (this.isShipperMyCustomer) {
        //        this.ShipperId = this.CustomerId;
        //    } else this.ConsigneeId = this.CustomerId;
        //}
       // else {
            if (this.isShipperMyCustomer) {
                this.CustomerId = this.ShipperId;
            } else this.CustomerId = this.ConsigneeId;

        //}
    }

    OnFiltersChanged() {
        this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
        this.IsTransportModesListEnabled = AppTool.IsNullOrEmpty(this.DirectionId) ? false : true;
        this.warehouseEntryPM.Ratio = AppTool.GetRatio(this.DirectionId, this.TransportModeId, this.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);


        this.SetScreenEnabled();
        this.SetPartners();
        this.SetUIProperties();
        this.SetLabels();
    

    }
    

    SetPartners() {

        switch (this.DirectionId) {
            case "I": {
                this.IsShipperMyCustomer = false;
                
                if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
                    this.ConsigneeId = this.CustomerId;
                    if (this.ShipperId == this.CustomerId) {
                        this.ShipperId = null;
                    }
                }

                break;
            }

            default: {
                this.IsShipperMyCustomer = true;
               // this.warehouseEntryPM.ShipmentCustomerTypeCode = "SHI";

                if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
                    this.ShipperId = this.CustomerId;
                    if (this.ConsigneeId == this.CustomerId) {
                        this.ConsigneeId = null;
                    }
                }

                break;
            }
        }

      
    }



     
    SetLabels() {
        this.CarrierDependencyProperty1 = "TR";
        this.FromPortText = "Origin";
        this.ToPortText = "Destination";


        switch (this.TransportModeId) {
            case "A": {
                //this.FromPortText = "Gateway";
               // this.ToPortText = "Destination";
                this.CarrierTextCode = "Shipment.S.NewShipment.Airline";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.FlightNo";
              //  this.CarrierDependencyProperty1 = "AL";
                this.MasterTextCode = "Shipment.S.NewShipment.MAWB";
                this.HouseTextCode = "Shipment.S.NewShipment.HAWB";
                break;
            }

            case "O": {
               // this.FromPortText = "Loading Port";
               // this.ToPortText = "Discharge Port";
                this.CarrierTextCode = "Shipment.S.NewShipment.Shippingline";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.VoyageNo";
              //  this.CarrierDependencyProperty1 = "SL";
                this.MasterTextCode = "Shipment.S.NewShipment.OBL";
                this.HouseTextCode = "Shipment.S.NewShipment.FBL";
                break;
            }

            case "I": {
               // this.FromPortText = "From";
               // this.ToPortText = "To";
                this.CarrierTextCode = "Shipment.S.NewShipment.Trucker";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.TruckerNo";
               // this.CarrierDependencyProperty1 = "TR";
                this.MasterTextCode = "Shipment.S.NewShipment.CMR/RWB#";
                this.HouseTextCode = "Shipment.F.House";
                break;
            }

            default: {
               // this.FromPortText = "From";
               // this.ToPortText = "To";
                this.CarrierTextCode = "Shipment.S.NewShipment.Carrier";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.No";
                this.MasterTextCode = "Shipment.S.NewShipment.MAWB";
                this.HouseTextCode = "Shipment.S.NewShipment.HAWB";
                break;
            }
        }


    }


  
 
    SetValue(args: any) {

       
        if (this.warehouseEntryPM) {
            this.FromTypeCode = 'PORT';
            this.ToTypeCode = 'PORT';

            var myCommonDomain = new CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(warehouseId)) {
                        this.WarehouseId = warehouseId;
                    }
                  
                }
               
            });

         

        }
    }

   
    ValidatePorts() {
        if (!this.IsInlandDomestic) {

            if (this.warehouseEntryPM.DirectionId == "D") {

                if (!AppTool.IsNullOrEmpty(this.warehouseEntryPM.FromPortId) && !AppTool.IsNullOrEmpty(this.warehouseEntryPM.ToPortId)) {
                    var fromCountryId = null;
                    var fromCountryIsEC = null;
                    var toCountryId = null;
                    var toCountryIsEC = null;

                    if (this.FromPortList != null) {
                        fromCountryId = this.FromPortList.CountryId;
                        fromCountryIsEC = this.FromPortList.CountryEC;
                    }

                    if (this.ToPortList != null) {
                        toCountryId = this.ToPortList.CountryId;
                        toCountryIsEC = this.ToPortList.CountryEC;
                    }

                    if (fromCountryId != toCountryId) {
                        if (fromCountryIsEC == false || toCountryIsEC == false) {
                            this.ValidationErrorsList.push("Both Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
    }   
    ValidateInlandDomestic() {

        if (this.IsInlandDomestic) {
            if (this.warehouseEntryPM.ShipmentLevelCode != "C") {
                var fromCountryId = null;
                var fromCountryIsEC = null;
                var toCountryId = null;
                var toCountryIsEC = null;

                if (this.ShipperAddressList != null) {
                    fromCountryId = this.ShipperAddressList.CountryId;
                    fromCountryIsEC = this.ShipperAddressList.CountryEC;
                }

                if (this.ConsigneeAddressList != null) {
                    toCountryId = this.ConsigneeAddressList.CountryId;
                    toCountryIsEC = this.ConsigneeAddressList.CountryEC;
                }
                if (!AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId) && !AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId)) {
                    if (fromCountryId != toCountryId) {
                        if (fromCountryIsEC == false || toCountryIsEC == false) {
                            this.ValidationErrorsList.push("Both Addresses must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }

    }

    SetActualDateClicked(fieldName: string) {
        this.ActualEntryDate = DateTool.GetDateParts(this.warehouseEntryPM.ExpectedEntryDate).DateObject;
    }
    SetCustomer(myCode: string) {
        if (myCode == 'S') {
            this.IsShipperMyCustomer = true;

        }

        else {
            this.IsShipperMyCustomer = false;

        }
    }
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

        this.IsScreenEnabled = isScreenEnabled;
        this.ScreenOpacity = isScreenEnabled ? 1 : 0.7;

        // Customer
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("CustomerReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("CustomerReference2", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TruckerReference", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TruckerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ReceivedBy", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MasterNumber", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("HouseNumber", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("WarehouseId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("EntryReference", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ActualEntryDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpectedEntryDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("SpecialInstruction", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalPieces", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalGrossWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalVolume", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Manufacturer", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToCountryId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromCountryId", this.ObjectTableName, isScreenEnabled);

    }

    FillMorePackagesDetails() {

        var isLCLEntity = AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);

        var logeWindow = new LogitudeWindow();
        logeWindow.Width =isLCLEntity ? 1010 :1090;
        logeWindow.Height = 500;
        logeWindow.Title = "Packages Details";

      

        logeWindow.WindowArgs = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, ShowPackageSummary: true, IsFromFullWarehouseEntryComponent:true};
        logeWindow.Show("./Warehouse/Components/WarehouseEntryPackagesDetailsComponent");

    }
    SetPackagesDetailsEnable() {


        var isEnablePackageArea: boolean = true;
        if (this.warehouseEntryPM.WarehouseEntryPackages && this.warehouseEntryPM.WarehouseEntryPackages.length > 0) {
            isEnablePackageArea = false;
        }

        this.UIProperties.SetEnabled("TotalPieces", this.ObjectTableName, isEnablePackageArea);
        this.UIProperties.SetEnabled("TotalGrossWeight", this.ObjectTableName, isEnablePackageArea);
        this.UIProperties.SetEnabled("TotalVolume", this.ObjectTableName, isEnablePackageArea);

    }


    SetUIProperties() {
        this.SetUIProperties_Port();
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
    }
    

    SetUIProperties_Port() {
        var isFromRequired: boolean = false;
        var isToRequired: boolean = false;

        if (!this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.FromPortId)) {
                isFromRequired = true;
            }
            if (AppTool.IsNullOrEmpty(this.ToPortId)) {
                isToRequired = true;
            }
        } 

        //this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isFromRequired);
        //this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isToRequired);


    }

    SetUIProperties_Shipper() {
        var isFieldRequired: boolean = false;

        if (AppTool.IsNullOrEmpty(this.ShipperId)) {
            if (this.IsShipperMyCustomer) {
                isFieldRequired = true;
            }

            else if (this.DirectionId == "E" || this.DirectionId == "D" || this.DirectionId == "R" || this.DirectionId == null || this.IsInlandDomestic) {
                isFieldRequired = true;
            }
        }

        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, isFieldRequired);

    }
    SetUIProperties_Consignee() {

        var isFieldRequired: boolean = false;
        if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            if (!this.IsShipperMyCustomer) {
                isFieldRequired = true;
            }

            else if (this.DirectionId == "I") {
                isFieldRequired = true;
            }

            else if (this.IsInlandDomestic) {
                isFieldRequired = true;
            }
        }
        this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, isFieldRequired);

    }




    OnActualEntryDateDatePickerChange(value) {
        this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", true, null);

        if (!DateTool.IsActualDateValid(value)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Entry Date");
            this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", false, errorMessage);
        }
    }
    // Add|Edit Partner
    AddPartnerClicked(myPartnerCode: string) {



        var title = "";
        var isCustomer = false;

        if (myPartnerCode == "Shipper") {
            title = "New Shipper";
            isCustomer = this.IsShipperMyCustomer;
        }

        else if (myPartnerCode == "Consignee") {
            title = "New Consignee";
            isCustomer = !this.IsShipperMyCustomer;
        }
        else if (myPartnerCode == "Customer") {
            title = "New Customer";
            isCustomer = true;
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
        logeWindow.Show("./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent");

        logeWindow.ComponentLoaded.subscribe(comp => {
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (myPartnerCode == "Shipper") this.ShipperId = comp.EntityPM.Id;
                    else if (myPartnerCode == "Consignee") this.ConsigneeId = comp.EntityPM.Id;
                    else if (myPartnerCode == "Customer") this.CustomerId = comp.EntityPM.Id;
                }
            });
        });

    }
    AddAddressClicked(myAddressCode: string) {

        var entityPM: AddressPM = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;

        switch (myAddressCode) {
            case "Shipper": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ShipperId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.IsShipperMyCustomer;
                break;
            }

            case "Consignee": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ConsigneeId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !this.IsShipperMyCustomer;
                break;
            }

            case "Customer": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.CustomerId;
                myPartnerTypeId = this.CustomerPartnerTypeId;
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
                        case "Shipper": {
                            this.FromAddressId = null;
                            this.FromAddressId = entityPM.Id;
                            break;
                        }

                        case "Consignee": {
                            this.ToAddressId = null;
                            this.ToAddressId = entityPM.Id;
                            break;
                        }

                        case "Customer": {
                            this.CustomerAddressId = null;
                            this.CustomerAddressId = entityPM.Id;
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

        switch (myAddressCode) {
            case "Shipper": {
                myAddressId = this.FromAddressId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.isShipperMyCustomer;
                break;
            }

            case "Consignee": {
                myAddressId = this.ToAddressId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !this.isShipperMyCustomer;
                break;
            }

            case "Customer": {
                myAddressId = this.CustomerAddressId;
                myPartnerTypeId = this.CustomerPartnerTypeId;
                isCustomer = this.isShipperMyCustomer;
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
                        case "Shipper": {
                            this.FromAddressId = null;
                            this.FromAddressId = myAddressId;
                            break;
                        }

                        case "Consignee": {
                            this.ToAddressId = null;
                            this.ToAddressId = myAddressId;
                            break;
                        }

                        case "Customer": {
                            this.CustomerAddressId = null;
                            this.CustomerAddressId = myAddressId;
                            break;
                        }
                    }

                }
            });
        }
    }

    OkButtonClicked() {

        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("WarehouseEntry", this.warehouseEntryPM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }



        if (this.IsInlandDomestic) {
            this.warehouseEntryPM.FromPortId = null;
            this.warehouseEntryPM.ToPortId = null;
        }

        if (!this.IsInlandDomestic) {
            //if (AppTool.IsNullOrEmpty(this.warehouseEntryPM.FromPortId)) {
            //    this.ValidationErrorsList.push(this.FromPortText + " field is required");
            //}
            //if (AppTool.IsNullOrEmpty(this.warehouseEntryPM.ToPortId)) {
            //    this.ValidationErrorsList.push(this.ToPortText + " field is required");
            //}

            if (this.IsShipperMyCustomer) {
                this.warehouseEntryPM.ShipperReference1 = this.CustomerRef1;
                this.warehouseEntryPM.ShipperReference2 = this.CustomerRef2;
            }
            else {
                this.warehouseEntryPM.ConsigneeReference1 = this.CustomerRef1;
                this.warehouseEntryPM.ConsigneeReference2 = this.CustomerRef2;
            }
        }

        this.ValidatePartners();


        this.ValidationErrorsList = this.ValidationErrorsList.filter(d => d != "Customer field is required");

        if (this.ValidationErrorsList.length == 0) {
            this.ValidateInlandDomestic();
            this.ValidatePorts();

        }

        if (this.ValidationErrorsList.length == 0) {
            if (!this.warehouseEntryPM.WarehouseEntryPackages) this.warehouseEntryPM.WarehouseEntryPackages = [];
            this.warehouseHelper.CreateWarehouseEntry(this.warehouseEntryPM, this);
        }



    }


    ValidatePartners() {
        if (this.warehouseEntryPM.ShipmentLevelCode != "C") {
            if ((this.warehouseEntryPM.DirectionId.toUpperCase() == "E" || this.warehouseEntryPM.DirectionId.toUpperCase() == "R") && AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId)) {

                this.ValidationErrorsList.push("Shipper" + " field is required");
            }

            else if (this.warehouseEntryPM.DirectionId.toUpperCase() == "I" && AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId)) {
                this.ValidationErrorsList.push("Consignee" + " field is required");
            }

            else if (this.warehouseEntryPM.DirectionId.toUpperCase() == "D" && (AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId) || AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId))) {
                if (AppTool.IsNullOrEmpty(this.warehouseEntryPM.ShipperId)) {
                    this.ValidationErrorsList.push("Shipper" + " field is required");
                }

                if (this.IsInlandDomestic) {
                    if (AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConsigneeId)) {
                        this.ValidationErrorsList.push("Consignee" + " field is required");
                    }
                }
            }
        }

    }



    CancelButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }
    
   


   



 






}
