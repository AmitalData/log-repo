

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseReleasePM} from '../../../Warehouse/EntityPMs/warehouseReleasePM';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {WarehouseReleasePackagePM} from '../../../Warehouse/EntityPMs/WarehouseReleasePackagePM';
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
    moduleId: module.id,
    selector: 'NewFullWarehouseReleaseComponent',
    templateUrl: './NewFullWarehouseReleaseComponent.html',


})

export class NewFullWarehouseReleaseComponent extends BaseComponent implements OnInit {
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

    ObjectTableName: string = "WarehouseRelease";
    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
    public DataContext = this;
    public LabelColumnWidth: number = 117;
    public WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
    public ValidationErrorsList: string[];

    warehouseReleasePM: WarehouseReleasePM = new WarehouseReleasePM();
    IsFromShipment: boolean = false;
    SelectedWarehouseReleasePackage: WarehouseReleasePackagePM;
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
        this.warehouseReleasePM = this.warehouseHelper.GetNewWarehouseRelease(this);
        this.validator = new ClassLevelValidator();
   


        var table = window.ObjectTables.filter(d => d.Name == "WarehouseRelease")[0];
        if (table) this.ObjectTableId = table.Id;

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }

        this.InitializeServices();

      
    }

   ngOnInit() { }


    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myPortListService = new PortListService();
        
    }



    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease").subscribe(response => {
            this.Start(args);
        });
    }

    Start(args: any) {

        if (args) {

            this.ShowShipmentLevels = false;
        
         
            this.WarehouseReleasePackagesLists = args.WarehouseReleasePackagesLists;
            if (!this.WarehouseReleasePackagesLists) this.WarehouseReleasePackagesLists = [];

            this.SetValue(args);
            this.BuildFiltersLists();
            this.OnFiltersChanged();

            this.IsLoadPage = true;

        }
    }


    ShipmentValueChange(shipment: any) {
        this.warehouseReleasePM.ShipmentNumber = null;
        if (shipment) {
            this.warehouseReleasePM.ShipmentNumber = shipment.ShipmentNumber;
        }

        this.Shipment = shipment;
    }




    
    get DirectionId() { return this.warehouseReleasePM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.warehouseReleasePM.DirectionId != newValue) {
            this.warehouseReleasePM.DirectionId = newValue;
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            this.OnFiltersChanged();
        }
    }

    get TransportModeId() { return this.warehouseReleasePM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.warehouseReleasePM.TransportModeId != newValue) {
            this.warehouseReleasePM.TransportModeId = newValue;
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            //this.FromPortId = null;
            //this.ToPortId = null;
           
            this.ShipmentTypeId = null;
            this.OnFiltersChanged();
            this.BuildShipmentTypes();
        }
    }

    
    public ShipmentTypeName: string = null;
    get ShipmentTypeId() { return this.warehouseReleasePM.ShipmentTypeId; }
    set ShipmentTypeId(newValue: string) {
        if (this.warehouseReleasePM.ShipmentTypeId != newValue) {
            this.warehouseReleasePM.ShipmentTypeId = newValue;

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


    
    
    //public FromPortList: PortList = null;
    //get FromPortId() { return this.warehouseReleasePM.FromPortId; }
    //set FromPortId(value: string) {
    //    if (this.warehouseReleasePM.FromPortId != value) {
    //        this.warehouseReleasePM.FromPortId = value;
    //        this.SetUIProperties();
    //        if (AppTool.IsNullOrEmpty(value)) {
    //            this.FromPortList = null;
    //        }
    //        else {
    //            this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
    //                if (!myResponse.HasError) {
    //                    this.FromPortList = myResponse.Result;
    //                }
    //            });
    //        }
    //    }
    //}

    //public ToPortList: PortList = null;
    //get ToPortId() { return this.warehouseReleasePM.ToPortId; }
    //set ToPortId(value: string) {
    //    if (this.warehouseReleasePM.ToPortId != value) {
    //        this.warehouseReleasePM.ToPortId = value;
    //        this.SetUIProperties();
    //        if (AppTool.IsNullOrEmpty(value)) {
    //            this.ToPortList = null;
    //        }
    //        else {
    //            this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
    //                if (!myResponse.HasError) {
    //                    this.ToPortList = myResponse.Result;
    //                }
    //            });
    //        }
    //    }
    //}



    get ReleaseBy() { return this.warehouseReleasePM.ReleaseBy; }
    set ReleaseBy(newValue: string) {
        if (this.warehouseReleasePM.ReleaseBy != newValue) {
            this.warehouseReleasePM.ReleaseBy = newValue;

        }
    }

    //get MasterNumber() { return this.warehouseReleasePM.MasterNumber; }
    //set MasterNumber(newValue: string) {
    //    if (this.warehouseReleasePM.MasterNumber != newValue) {
    //        this.warehouseReleasePM.MasterNumber = newValue;

    //    }
    //}

    //get HouseNumber() { return this.warehouseReleasePM.HouseNumber; }
    //set HouseNumber(newValue: string) {
    //    if (this.warehouseReleasePM.HouseNumber != newValue) {
    //        this.warehouseReleasePM.HouseNumber = newValue;

    //    }
    //}
    get Notes() { return this.warehouseReleasePM.Notes; }
    set Notes(newValue: string) {
        if (this.warehouseReleasePM.Notes != newValue) {
            this.warehouseReleasePM.Notes = newValue;

        }
    }

    get WarehouseId() { return this.warehouseReleasePM.WarehouseId; }
    set WarehouseId(newValue: string) {
        if (this.warehouseReleasePM.WarehouseId != newValue) {
            this.warehouseReleasePM.WarehouseId = newValue;
            this.warehouseReleasePM.FromPortId = newValue;

        }
    }
    get ShipmentId() { return this.warehouseReleasePM.ShipmentId; }
    set ShipmentId(newValue: string) {
        if (this.warehouseReleasePM.ShipmentId != newValue) {
            this.warehouseReleasePM.ShipmentId = newValue;

        }
    }

    get ActualReleaseDate() { return this.warehouseReleasePM.ActualReleaseDate; }
    set ActualReleaseDate(newValue: Date) {
        if (this.warehouseReleasePM.ActualReleaseDate != newValue) {
            this.warehouseReleasePM.ActualReleaseDate = newValue;
            this.OnActualReleaseDateDatePickerChange(newValue);

        }
    }

    get ExpectedReleaseDate() { return this.warehouseReleasePM.ExpectedReleaseDate; }
    set ExpectedReleaseDate(newValue: Date) {
        if (this.warehouseReleasePM.ExpectedReleaseDate != newValue) {
            this.warehouseReleasePM.ExpectedReleaseDate = newValue;

        }
    }


    get SpecialInstruction() { return this.warehouseReleasePM.SpecialInstruction; }
    set SpecialInstruction(newValue: string) {
        if (this.warehouseReleasePM.SpecialInstruction != newValue) {
            this.warehouseReleasePM.SpecialInstruction = newValue;

        }
    }


    get TotalVolume() { return this.warehouseReleasePM.TotalVolume; }
    set TotalVolume(newValue: number) {
        if (this.warehouseReleasePM.TotalVolume != newValue) {
            this.warehouseReleasePM.TotalVolume = newValue;

        }
    }

    get TotalGrossWeight() { return this.warehouseReleasePM.TotalGrossWeight; }
    set TotalGrossWeight(newValue: number) {
        if (this.warehouseReleasePM.TotalGrossWeight != newValue) {
            this.warehouseReleasePM.TotalGrossWeight = newValue;

        }
    }

    get TotalPieces() { return this.warehouseReleasePM.TotalPieces; }
    set TotalPieces(newValue: number) {
        if (this.warehouseReleasePM.TotalPieces != newValue) {
            this.warehouseReleasePM.TotalPieces = newValue;

        }
    }

    private customerContactId: string = null;
    get CustomerContactId() { return this.customerContactId; }
    set CustomerContactId(newValue: string) {
        if (this.customerContactId != newValue) {
            this.customerContactId = newValue;
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

    private CustomerPartnerTypeId: string;
    private CustomerIsCustomer: boolean;

    get CustomerId() { return this.warehouseReleasePM.CustomerId; }
    set CustomerId(newValue: string) {
        if (this.warehouseReleasePM.CustomerId != newValue) {
            this.warehouseReleasePM.CustomerId = newValue;
           

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.CustomerPartnerTypeId = null;
                this.CustomerContactId = null;
                this.warehouseReleasePM.CustomerName = null;

                this.CustomerAddressId = null;

            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.CustomerPartnerTypeId = myCardList.PartnerTypeId;
                            this.CustomerContactId = myCardList.PrimaryContactId;
                            this.warehouseReleasePM.CustomerName = myCardList.EnglishName;
                            this.CustomerAddressId = myCardList.MainAddressId;

                        }
                    }
                });
            }
        }
    }


    //private isAddShipperButtonDisabled: boolean = false;
    //get IsAddShipperButtonDisabled() {
    //    if (!this.IsScreenEnabled) {
    //        this.isAddShipperButtonDisabled = true;
    //    } else this.isAddShipperButtonDisabled = false;

    //    return this.isAddShipperButtonDisabled;


    //}

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

   

    OnFiltersChanged() {
        this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
        this.IsTransportModesListEnabled = AppTool.IsNullOrEmpty(this.DirectionId) ? false : true;
       // this.warehouseReleasePM.Ratio = AppTool.GetRatio(this.DirectionId, this.TransportModeId, this.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);


        this.SetScreenEnabled();
       // this.SetPartners();
        this.SetUIProperties();
        this.SetLabels();
    

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

       
        if (this.warehouseReleasePM) {

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

            if (this.warehouseReleasePM.DirectionId == "D") {

                //if (!AppTool.IsNullOrEmpty(this.warehouseReleasePM.FromPortId) && !AppTool.IsNullOrEmpty(this.warehouseReleasePM.ToPortId)) {
                //    var fromCountryId = null;
                //    var fromCountryIsEC = null;
                //    var toCountryId = null;
                //    var toCountryIsEC = null;

                //    if (this.FromPortList != null) {
                //        fromCountryId = this.FromPortList.CountryId;
                //        fromCountryIsEC = this.FromPortList.CountryEC;
                //    }

                //    if (this.ToPortList != null) {
                //        toCountryId = this.ToPortList.CountryId;
                //        toCountryIsEC = this.ToPortList.CountryEC;
                //    }

                //    if (fromCountryId != toCountryId) {
                //        if (fromCountryIsEC == false || toCountryIsEC == false) {
                //            this.ValidationErrorsList.push("Both Ports must be in the same country since the direction is Domestic");
                //        }
                //    }
                //}
            }
        }
    }   


    SetActualDateClicked(fieldName: string) {
        this.ActualReleaseDate = DateTool.GetDateParts(this.warehouseReleasePM.ExpectedReleaseDate).DateObject;
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
        
        //this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, isScreenEnabled);
        //this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ReleaseBy", this.ObjectTableName, isScreenEnabled);
     
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("WarehouseId", this.ObjectTableName, isScreenEnabled);
       
        this.UIProperties.SetEnabled("ActualReleaseDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ExpectedReleaseDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("SpecialInstruction", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalPieces", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("TotalGrossWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipmentId", this.ObjectTableName, isScreenEnabled);
        //this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isScreenEnabled);
        //this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isScreenEnabled);
        //this.UIProperties.SetRequired("CustomerId", this.ObjectTableName, isScreenEnabled);
        //this.UIProperties.SetRequired("WarehouseId", this.ObjectTableName, isScreenEnabled);
    }

    Shipment: any;
    FillMorePackagesDetails() {

        var logeWindow = new LogitudeWindow();
        logeWindow.Width =1000;
        logeWindow.Height = 500;
        logeWindow.Title = "Packages Details";

        logeWindow.WindowArgs = { warehouseReleasePM: this.warehouseReleasePM, ViewModelTrigger: this, ShowPackageSummary: true, IsFromFullWarehouseReleaseComponent: true, ShipmentPM: this.Shipment };
        logeWindow.Show("./Warehouse/Components/WarehouseReleasePackagesDetailsComponent");

    }
    SetPackagesDetailsEnable() {


        var isEnablePackageArea: boolean = true;
        if (this.warehouseReleasePM.WarehouseReleasePackages && this.warehouseReleasePM.WarehouseReleasePackages.length > 0) {
            isEnablePackageArea = false;
        }

        this.UIProperties.SetEnabled("TotalPieces", this.ObjectTableName, isEnablePackageArea);
        this.UIProperties.SetEnabled("TotalGrossWeight", this.ObjectTableName, isEnablePackageArea);
        this.UIProperties.SetEnabled("TotalVolume", this.ObjectTableName, isEnablePackageArea);

    }


    SetUIProperties() {
        this.SetUIProperties_Port();
        //var isWarehouseIdRequired: boolean = false;
        //var isCustomerIdRequired: boolean = false;
        //if (AppTool.IsNullOrEmpty(this.WarehouseId)) {
        //    isWarehouseIdRequired = true;
        //}
        //if (AppTool.IsNullOrEmpty(this.CustomerId)) {
        //    isCustomerIdRequired = true;

        //}
        //this.UIProperties.SetRequired("WarehouseId", this.ObjectTableName, isWarehouseIdRequired);
        //this.UIProperties.SetRequired("CustomerId", this.ObjectTableName, isCustomerIdRequired);
    }
    

    SetUIProperties_Port() {
        var isFromRequired: boolean = false;
        //var isToRequired: boolean = false;

        //if (!this.IsInlandDomestic) {
        //    if (AppTool.IsNullOrEmpty(this.FromPortId)) {
        //        isFromRequired = true;
        //    }
        //    if (AppTool.IsNullOrEmpty(this.ToPortId)) {
        //        isToRequired = true;
        //    }
        //} 

        //this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isFromRequired);
        //this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isToRequired);


    }

   


    OnActualReleaseDateDatePickerChange(value) {
        this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", true, null);

        if (!DateTool.IsActualDateValid(value)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Release Date");
            this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", false, errorMessage);
        }
    }
    // Add|Edit Partner
    AddCustomerClicked() {

        var args = new NewEntityArgs();
        var logeWindow = new LogitudeWindow();
        logeWindow.Width = 960;
        logeWindow.Height = 600;
        logeWindow.Title = "New Customer";
        logeWindow.WindowArgs = args;
        logeWindow.Show("./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent");

        logeWindow.ComponentLoaded.subscribe(comp => {
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.CustomerId = comp.EntityPM.Id;
                }
                
            });
        });

    }

    AddAddressClicked() {

        var entityPM: AddressPM = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean=true;

   
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.CustomerId;
                myPartnerTypeId = this.CustomerPartnerTypeId;
               

       


        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                 
                       
                            this.CustomerAddressId = null;
                            this.CustomerAddressId = entityPM.Id;
                   
                }
            });
        }
    }
    EditAddressClicked() {

        var myAddressId: string = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean = true;
    
                myAddressId = this.CustomerAddressId;
                myPartnerTypeId = this.CustomerPartnerTypeId;
               
            
           
  

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                   
                            this.CustomerAddressId = null;
                            this.CustomerAddressId = myAddressId;
                           

                }
            });
        }
    }

    OkButtonClicked() {

        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("WarehouseRelease", this.warehouseReleasePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }



        //if (this.IsInlandDomestic) {
        //    this.warehouseReleasePM.FromPortId = null;
        //    this.warehouseReleasePM.ToPortId = null;
        //}
  

        //if (!this.IsInlandDomestic) {
        //    if (AppTool.IsNullOrEmpty(this.warehouseReleasePM.FromPortId)) {
        //        this.ValidationErrorsList.push(this.FromPortText + " field is required");
        //    }
        //    if (AppTool.IsNullOrEmpty(this.warehouseReleasePM.ToPortId)) {
        //        this.ValidationErrorsList.push(this.ToPortText + " field is required");
        //    }

           
        //}

        //this.ValidatePartners();


        this.ValidationErrorsList = this.ValidationErrorsList.filter(d => d != "Customer field is required");

        if (this.ValidationErrorsList.length == 0) {
          //  this.ValidateInlandDomestic();
            this.ValidatePorts();

        }

        if (this.ValidationErrorsList.length == 0) {
            if (!this.warehouseReleasePM.WarehouseReleasePackages) this.warehouseReleasePM.WarehouseReleasePackages = [];
            this.warehouseHelper.CreateWarehouseRelease(this.warehouseReleasePM, this);
        }



    }


   




    CancelButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }
    
   


   



 






}
