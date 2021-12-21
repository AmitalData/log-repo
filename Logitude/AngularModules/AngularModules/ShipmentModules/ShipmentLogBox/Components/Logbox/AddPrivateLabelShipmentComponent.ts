import { Component } from '@angular/core';
import { IncotermList } from '../../../../Common/EntityLists/IncotermList';
import { PackageTypeList } from '../../../../Common/EntityLists/PackageTypeList';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { BranchListService } from '../../../../Common/Services/StandardLists/BranchListService';
import { DepartmentListService } from '../../../../Common/Services/StandardLists/DepartmentListService';
import { IncotermListService } from '../../../../Common/Services/StandardLists/IncotermListService';
import { PortListService } from '../../../../Common/Services/StandardLists/PortListService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { EntityStatusListService } from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import { AppTool, DateTool, FormatTool } from '../../../../Infrastructure/Tools';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { FilterClass } from '../../../../Shipment/Components/NewEntity/NewShipmentComponent';
import { ShipmentSubTypeList } from '../../../../shipment/EntityLists/ShipmentSubTypeList';
import { ShipmentOrderPackagePM } from '../../../../Shipment/EntityPMs/ShipmentOrderPackagePM';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { AddEditPrivateLabelShipmentComponent } from './AddEditPrivateLabelShipmentComponent';



@Component({

    templateUrl: './AddPrivateLabelShipmentComponent.html',
})

export class AddPrivateLabelShipmentComponent extends AddEditPrivateLabelShipmentComponent {

    public TenantPM: TenantPM;
    public DirectionsList: FilterClass[] = [];
    public TransportModesList: FilterClass[] = [];
    public ShipmentTypesList: FilterClass[] = [];
    public ShipmentLevelsList: FilterClass[] = [];
    public ShipmentSubTypesList: FilterClass[] = [];
    EntityPM: ShipmentPM = new ShipmentPM();
    public ShipmentTypeName: string = null;
    public ShowShipmentLevels: boolean = true;
    public ObjectTableName: string = "Shipment";
    public LabelColumnWidth: number = 115;
    public ControlColumnWidth: number = 220;
    private portListService: PortListService;
    private incotermListService: IncotermListService;
    private shipmentPMService: ShipmentPMService;
    public departmentListService: DepartmentListService;
    public entityStatusListService: EntityStatusListService;
    public errorMessage = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    private args: any;
    EntityProgressStatusId: string;
    public SessionIndex: number;
    public ScreenOpacity: number = 1;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    public VolumetricWeightColumnHeader: string;
    public IsDSVTenant: boolean = false;
    public FromPort: string;
    public ShowAddDocument: boolean = false;
    public ChangePageButton: string = "Next";
    public IsCustomsActivated: boolean = false;
    public AllowCreateShipmentsWithoutDocuments: boolean = false;
    public Order: string = "Reference";
    public RequestedDateLabel: string = "Requested Flight Date";
    public AllowCreateOceanExport: boolean = false; 
    public FromTextCode: string;
    public ToTextCode: string; 
    public ToPortTextCode: string;
    public isRTL: boolean = false;

    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.InitializeServices();
        this.SetUIProperties();
        this.SetUIProperties_Filters(); 
        this.TenantPM = SessionLocator.TenantPM;
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.IsDSVTenant = SessionLocator.PrivateLableSettings.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1;
        this.BuildFiltersLists(); 
        this.SetUnits();
        this.SetLabels();
        this.SetFromPort();
        this.checkAirShipmentToggle();

    }
    checkAirShipmentToggle() { 
         let AirShipmentFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "PLE")[0];
        if (AirShipmentFeatureToggle) {
            this.AllowCreateOceanExport = true;

        }
    }

    SetFieldsLabel() {

        if (this.EntityPM.TransportModeId == "A") {
            this.SetAirExportLabels();
        }
        if (this.EntityPM.TransportModeId == "O") {
            this.SetOceanExportLabels();
        }

        
        this.SetPortsLabel();
    }
 

    private SetOceanExportLabels() {
        this.Order = "Order Number";
        this.RequestedDateLabel = "Expected Sealing Date";
    }

    private SetAirExportLabels() {
        this.Order = "Reference";
        this.RequestedDateLabel = "Requested Flight Date";
    }

    SetFromPort() {
        this._PortExtendedPMService.getSinglePort("TLV", "IL", SessionLocator.Tenant).subscribe((Result: any) => {
            this.FromPort =  Result.Result.Id;
        })

    }


    SetUnits() { 
        this.SetDimensionsUnitCode(); 
        this.SetVolumeUnitCode(); 
        this.SetGrossWeightUnitCode();
        this.SetChargeableWeightUnitCode();
        this.ComputeOrderVolumetricWeight();
        this.ComputeChargeableWeight(); 
    }
 
    private SetChargeableWeightUnitCode() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeightUnitCode)) {
            this.EntityPM.ChargeableWeightUnitCode = AppTool.GetChargeableWeightUnitCode(this.TransportModeId);
        }
    }

    private SetGrossWeightUnitCode() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.GrossWeightUnitCode)) {
            this.EntityPM.GrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        }
    }

    private SetVolumeUnitCode() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
            this.EntityPM.VolumeUnitCode = this.TenantPM.VolumeUnitCode;
        }
    }

    private SetDimensionsUnitCode() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
            this.EntityPM.DimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        }
    }

    private ComputeOrderVolumetricWeight() {
        let weight: number = null;

        if (this.BookingVolume != null) {
            weight = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.BookingVolume, this.EntityPM.Ratio);
        } 
        else if (this.OrderGrossWeight != null) {
            weight = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.OrderGrossWeight);
        }

        this.OrderVolumetricWeight = weight;
    }

    get OrderVolumetricWeight() { return this.EntityPM.OrderVolumetricWeight; }
    set OrderVolumetricWeight(newValue: number) {
        if (this.EntityPM.OrderVolumetricWeight != newValue) {
            this.EntityPM.OrderVolumetricWeight = AppTool.Round(newValue, 3);
            this.ComputeChargeableWeight();
        }
    }

    private ComputeChargeableWeight() {
        this.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.OrderGrossWeight, this.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }

    get OrderChargeableWeight() { return this.EntityPM.OrderChargeableWeight; }
    set OrderChargeableWeight(newValue: number) {
        if (this.EntityPM.OrderChargeableWeight != newValue) {
            let OrderVolume: number = AppTool.Round(newValue, 3);
            this.EntityPM.OrderChargeableWeight = OrderVolume;

            this.SetUIProperties_OrderDetails();

            if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                this.SetOrderDetails(OrderVolume);
            }
        }
    }

     

    private SetOrderDetails(orderVolume: number) {
        this.EntityPM.OrderVolumetricWeight = orderVolume;
        this.EntityPM.OrderGrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, orderVolume);
        this.EntityPM.BookingVolume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, orderVolume, this.EntityPM.Ratio);
    }

    SetLabels() { 

        this.SetPortsLabel();

        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        } 
        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
         
    }
    private SetPortsLabel() {
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

    InitializeServices() {
        this.portListService = new PortListService();
        this.incotermListService = new IncotermListService();
        this.shipmentPMService = new ShipmentPMService();  
        this.entityStatusListService = new EntityStatusListService();
    }

    SetWindowArgs(args: any) { 
        this.SetExportShipmentArgs(args);
    }

    private SetExportShipmentArgs(args: any) { 
        this.SetArgs(args);
        this.GetBranch();
        this.GetDepartment();
        this.SetUIProperties();
        this.BuildFiltersLists();
        this.SetUIProperties_Filters();
    }
    private SetArgs(args: any) {
        this.SetDirections(args);
        this.args = args;
        if (args.EntityPM) {
            this.EntityPM = args.EntityPM;
        }
        this.AllowCreateShipmentsWithoutDocuments = SessionLocator.PrivateLableSettings.CreateShipmentsWithoutDocs;
    }

    private SetDirections(args: any) {
        this.IsCustomsActivated = args.IsCustomsActivated;
    }


    private GetDepartment() {
        this._DepartmentListService.getAll().subscribe((result: any) => {
            if (!result.HasError) {
                this.DepartmentId = result.Result.filter(a => a.Tenant == SessionLocator.Tenant)[0].Id;
            }
            else {
                this.ValidationErrorsList = result.ErrorsArray;
            }
        });
    }

    private GetBranch() {
        this._BranchListService.getAll().subscribe((result: any) => {
            if (!result.HasError) {
                this.BranchId = result.Result.filter(a => a.Tenant == SessionLocator.Tenant)[0].Id;
            }
            else {
                this.ValidationErrorsList = result.ErrorsArray;
            }
        });
    }

    SetUIProperties() { 
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, true);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, false);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, false);

    }

    SetUIProperties_Filters() {
        this.DirectionId = 'E';
        this.TransportModeId = 'A';
        this.ShipmentTypeId = 'FCLD';
    }

    BuildFiltersLists() { 
        this.BuildDirectionsList(); 
        this.BuildTransportModesList();
       // this.BuildShipmentTypes();
    }

    p 
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

    private BuildTransportModesList() {
        this.TransportModesList = []; 
        this.TransportModesList.push(new FilterClass("A", "Air"));
        this.TransportModesList.push(new FilterClass("O", "Ocean"));
        this.TransportModesList.push(new FilterClass("I", "Inland"));
    }

    private BuildDirectionsList() {
        this.DirectionsList = [];
        this.DirectionsList.push(new FilterClass("E", "Export"));  

         this.SetCustomOption(); 
    } 
     
    private SetCustomOption() {
        if (SessionLocator.PrivateLableSettings.IsCustomsActivated && this.IsCustomsActivated) {
            this.DirectionsList.push(new FilterClass("C", "Customs"));
        }
    }

    SetOrderPackagesOnFinish() {
        if (this.ShipmentTypeId == 'FCLD') {

            this.EntityPM.ShipmentOrderPackages = [];

            if (this.hasPackage1()) {
                var item = this.AddOrderPackage1(); 
            }

            if (this.hasPackage2()) {
                var item = this.AddOrderPackage2(item); 
            }

            if (this.hasPackage3()) {
                var item = this.AddOrderPackage3(item); 
            }

            if (this.hasPackage4()) {
                var item = this.AddOrderPackage4(item); 
            }
             
            this.EntityPM.BookingNumberOfPackages = this.CalculateNumberOfPackages();
            
        }
    }
    private hasPackage4() {
        return this.PackageTypeList4 != null && this.Quantity4 > 0;
    }

    private hasPackage3() {
        return this.PackageTypeList3 != null && this.Quantity3 > 0;
    }

    private hasPackage2() {
        return this.PackageTypeList2 != null && this.Quantity2 > 0;
    }

    private hasPackage1() {
        return this.PackageTypeList1 != null && this.Quantity1 > 0;
    }

    CalculateNumberOfPackages(): number {
        var numberOfPackages = 0;

        if (this.hasPackage1()) { 
            numberOfPackages = numberOfPackages + this.Quantity1;
        }

        if (this.hasPackage2()) { 
            numberOfPackages = numberOfPackages + this.Quantity2;
        }

        if (this.hasPackage3()) { 
            numberOfPackages = numberOfPackages + this.Quantity3;
        }

        if (this.hasPackage4()) { 
            numberOfPackages = numberOfPackages + this.Quantity4;
        }
        return numberOfPackages;
 
    }

    private AddOrderPackage4(item: ShipmentOrderPackagePM) {
        var item = new ShipmentOrderPackagePM(this.EntityPM);
        item.IsContainer = this.PackageTypeList4.IsContainer;
        item.ContainerNumber = this.ContainerNumber4;
        item.Quantity = this.Quantity4;
        item.PackageTypeId = this.PackageTypeId4;
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        this.EntityPM.AddOrderPackage(item);
        return item;
    }

    private AddOrderPackage3(item: ShipmentOrderPackagePM) {
        var item = new ShipmentOrderPackagePM(this.EntityPM);
        item.IsContainer = this.PackageTypeList3.IsContainer;
        item.ContainerNumber = this.ContainerNumber3;
        item.Quantity = this.Quantity3;
        item.PackageTypeId = this.PackageTypeId3;
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        this.EntityPM.AddOrderPackage(item);
        return item;
    }

    private AddOrderPackage2(item: ShipmentOrderPackagePM) {
        var item = new ShipmentOrderPackagePM(this.EntityPM);
        item.IsContainer = this.PackageTypeList2.IsContainer;
        item.ContainerNumber = this.ContainerNumber2;
        item.Quantity = this.Quantity2;
        item.PackageTypeId = this.PackageTypeId2;
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        this.EntityPM.AddOrderPackage(item);
        return item;
    }

    private AddOrderPackage1() {
        var item = new ShipmentOrderPackagePM(this.EntityPM);
        item.IsContainer = this.PackageTypeList1.IsContainer;
        item.Quantity = this.Quantity1;
        item.ContainerNumber = this.ContainerNumber1;
        item.PackageTypeId = this.PackageTypeId1;
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        this.EntityPM.AddOrderPackage(item);
        return item;
    }

    get ContainerNumber1() { return this.EntityPM.ContainerNumber1; }
    set ContainerNumber1(newValue: string) {
        if (this.EntityPM.ContainerNumber1 != newValue) {
            this.EntityPM.ContainerNumber1 = newValue;

            this.SetUIProperties_Containers();
        }
    }

    get ContainerNumber2() { return this.EntityPM.ContainerNumber2; }
    set ContainerNumber2(newValue: string) {
        if (this.EntityPM.ContainerNumber2 != newValue) {
            this.EntityPM.ContainerNumber2 = newValue;

            this.SetUIProperties_Containers();
        }
    }

    get ContainerNumber3() { return this.EntityPM.ContainerNumber3; }
    set ContainerNumber3(newValue: string) {
        if (this.EntityPM.ContainerNumber3 != newValue) {
            this.EntityPM.ContainerNumber3 = newValue;

            this.SetUIProperties_Containers();
        }
    }

    get ContainerNumber4() { return this.EntityPM.ContainerNumber4; }
    set ContainerNumber4(newValue: string) {
        if (this.EntityPM.ContainerNumber4 != newValue) {
            this.EntityPM.ContainerNumber4 = newValue;

            this.SetUIProperties_Containers();
        }
    }

    get Quantity1() { return this.EntityPM.Quantity1; }
    set Quantity1(newValue: number) {
        if (this.EntityPM.Quantity1 != newValue) {
            this.EntityPM.Quantity1 = newValue;

            this.SetUIProperties_Containers();
        }
    }


    SetUIProperties_Containers() {

        this.UIProperties.SetEnabled("PackageTypeId1", this.ObjectTableName, this.Quantity1 > 0);
        this.UIProperties.SetEnabled("PackageTypeId2", this.ObjectTableName, this.Quantity2 > 0 );
        this.UIProperties.SetEnabled("PackageTypeId3", this.ObjectTableName, this.Quantity3 > 0);
        this.UIProperties.SetEnabled("PackageTypeId4", this.ObjectTableName, this.Quantity4 > 0);

        if (AppTool.IsNullOrZero(this.Quantity1)) {
            this.PackageTypeId1 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity2)) {
            this.PackageTypeId2 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity3)) {
            this.PackageTypeId3 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity4)) {
            this.PackageTypeId4 = null;
        }

    }

    get Quantity2() { return this.EntityPM.Quantity2; }
    set Quantity2(newValue: number) {
        if (this.EntityPM.Quantity2 != newValue) {
            this.EntityPM.Quantity2 = newValue;

            this.SetUIProperties_Containers();
        }
    }

    get Quantity3() { return this.EntityPM.Quantity3; }
    set Quantity3(newValue: number) {
        if (this.EntityPM.Quantity3 != newValue) {
            this.EntityPM.Quantity3 = newValue;

            this.SetUIProperties_Containers();
        }
    }

    get Quantity4() { return this.EntityPM.Quantity4; }
    set Quantity4(newValue: number) {
        if (this.EntityPM.Quantity4 != newValue) {
            this.EntityPM.Quantity4 = newValue;

            this.SetUIProperties_Containers();

        }
    }
    public PackageTypeList1: PackageTypeList = null;
    get PackageTypeId1() { return this.EntityPM.PackageTypeId1; }
    set PackageTypeId1(newValue: string) {
        if (this.EntityPM.PackageTypeId1 != newValue) {
            this.EntityPM.PackageTypeId1 = newValue;
        }
    }

    public PackageTypeList2: PackageTypeList = null;
    get PackageTypeId2() { return this.EntityPM.PackageTypeId2; }
    set PackageTypeId2(newValue: string) {
        if (this.EntityPM.PackageTypeId2 != newValue) {
            this.EntityPM.PackageTypeId2 = newValue;
        }
    }

    public PackageTypeList3: PackageTypeList = null;
    get PackageTypeId3() { return this.EntityPM.PackageTypeId3; }
    set PackageTypeId3(newValue: string) {
        if (this.EntityPM.PackageTypeId3 != newValue) {
            this.EntityPM.PackageTypeId3 = newValue;
        }
    }

    public PackageTypeList4: PackageTypeList = null;
    get PackageTypeId4() { return this.EntityPM.PackageTypeId4; }
    set PackageTypeId4(newValue: string) {
        if (this.EntityPM.PackageTypeId4 != newValue) {
            this.EntityPM.PackageTypeId4 = newValue;
        }
    }

    public get BranchId() { return this.EntityPM.BranchId }
    public set BranchId(newValue: string) { this.EntityPM.BranchId = newValue; }

    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId != newValue) {
            this.EntityPM.DirectionId = newValue;

        }
    }

    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
        }
    } 


    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue; 
            if (newValue == "A") {
                this.EntityPM.ShipmentTypeId = "Air";
            }
            else if (this.EntityPM.DirectionId == "E") {
                this.EntityPM.ShipmentTypeId = "FCLD";
            } else {
                this.EntityPM.ShipmentTypeId = null;
            }

            this.BuildShipmentTypes();
            this.SetFieldsLabel();
        }
    }

    

    changeDirection(code) {
        this.DirectionId = code;
        this.ValidationErrorsList = [];
       switch (code) {
            case 'C':
               { 
                    this.SetCustomsShipmentArgs(this.args);
                    break;
                }
            case 'E':
               { 
                    this.SetExportShipmentArgs(this.args);
                    break;
                }
       }
    }
 
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.DirectionId == 'C') {
            this.SaveChanges();
        } else { 
            this.CreateExportShipment();
        }

    }
     
    private CreateExportShipment() {
        this.ValidateRequiredFields();

        if (this.ValidationErrorsList.length == 0) {
            this.InitializeExportShipmentFields();
        }
    }

    NextButtonClicked() {
        this.ShowAddDocument = !this.ShowAddDocument;
        this.SetChangeButtonTitle();
    }

    private SetChangeButtonTitle() {
        if (this.ShowAddDocument)
            this.ChangePageButton = "Back";
        else
            this.ChangePageButton = "Next";
    }

    private InsertShipment() {
        this.shipmentPMService.insert(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (serviceResponse.HasError) {
                this.ValidationErrorsList = serviceResponse.ErrorsArray;
            }
            else {
                this.EntityPM = serviceResponse.Result;
                this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded"); 

            }
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    } 


    private SetForwarderPartner() {
        if (SessionLocator.PrivateLableSettings) {
            this.EntityPM.ForwarderPartnerId = SessionLocator.PrivateLableSettings.HybridPartnerId;
        }
    }

    public get StatusId() { return this.EntityPM.StatusId }
    public set StatusId(newValue: string) { this.EntityPM.StatusId = newValue; }

    private InitializeExportShipmentFields() {

        this.CurrentSession.StartBusyIndicator("Creating...");
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId; 
        this.EntityPM.StatusDate = DateTool.GetCurrentDateTimeAsUtc();  
        this.EntityPM.IsImporterShipment = true;
        this.EntityPM.CustomerId = SessionLocator.TenantPM.CustomerId;
        this.EntityPM.CustomerName = SessionLocator.TenantPM.CustomerId; 
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId; 
        this.EntityPM.NewConcurrencyGUID = Guid.newGuid();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.ShipmentCustomerTypeCode = "SHI";
        this.EntityPM.MainCarriageFromPortId = this.TransportModeId == 'A' ? this.FromPort : this.MainCarriageFromPortId;
        this.EntityPM.OtherPrepaidCollectId = "C";
        this.EntityPM.FreightPrepaidCollectId = "C";
        this.EntityPM.ShipmentLevelCode = "A";
        this.EntityPM.FromPortId = this.FromPort;
        this.SetOrderPackagesOnFinish();
        this._EntityStatusListService.getAll().subscribe((myResult: any) => {
            if (!myResult.HasError) {

                this.SetShipmentStatus(myResult);
                this.SetForwarderPartner(); 
                this.SetDocumentFilingIds(); 
                this.InsertShipment(); 
            }
            else {
                this.ValidationErrorsList = myResult.ErrorsArray;
            }
        });



    }
   
    private SetDocumentFilingIds() {
        this.EntityPM.DocumentFilingIds = "";
        this.documentsFilings.forEach((item) => {
            this.EntityPM.DocumentFilingIds += (item.Id + ",");
        });
    }

    private SetShipmentStatus(myResult: any) {
        this.StatusId = myResult.Result.filter(a => a.Code == "OPOP")[0]?.Id;
        this.EntityProgressStatusId = myResult.Result.filter(a => a.Code == "INPS")[0]?.Id;
        this.EntityPM.StatusId = !this.IsDSVTenant ? this.EntityProgressStatusId : this.EntityPM.StatusId;
    }

    private ValidateRequiredFields() { 

        if (this.TransportModeId == 'A') { 
            this.ValidateAirExportShipmentFields();
            return;
        }
        if (this.TransportModeId == 'O') {
            this.ValidateOceanExportShipmentFields();
            return;
        }
        return; 
    }

    public ValidateContainerNumber(input: string) {
        let result = FormatTool.ValidateContainerNumber(input);

        if (result != null) {
            this.PushErrorMessage(result);
        }
    }

    ValidateOceanExportShipmentFields() { 

        if (AppTool.IsNullOrEmpty(this.CustomerReference3)) {
            this.PushErrorMessage("Order Number");
        } 

        if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
            this.PushErrorMessage("Discharge Port");
        }

        if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
            this.PushErrorMessage("Loading Port");
        } 

        if (AppTool.IsNullOrEmpty(this.IncotermId)) {
            this.PushErrorMessage("Incoterm");
        }

        if (!AppTool.IsNullOrEmpty(this.RequestedFlightDate)) {
            this.ValidateRequestedFlightDate();
        }


        this.ValidateLCLDShipmentType();
         
        this.ValidateFCLDShipmentType();
 
    }
    ValidateFCLDShipmentType() {

        if (!(this.ShipmentTypeId == 'FCLD' && this.TransportModeId == 'O')) {
            return;
        }

        if (AppTool.IsNullOrEmpty(this.Quantity1)) {
            this.PushErrorMessage("Quantity");
        }


        this.ValidateContainerFCLDContainerNumber();

    }
    ValidateContainerFCLDContainerNumber() {

        this.ValidateContainers();
        this.ValidatePackateTypes();
    }

    ValidatePackateTypes() {
        if (AppTool.IsNullOrEmpty(this.PackageTypeId1) && !AppTool.IsNullOrEmpty(this.Quantity1)) {
            this.PushErrorMessage("Package Type");
        }
        if (AppTool.IsNullOrEmpty(this.PackageTypeId2) && !AppTool.IsNullOrEmpty(this.Quantity2)) {
            this.PushErrorMessage("Package Type");
        }
        if (AppTool.IsNullOrEmpty(this.PackageTypeId3) && !AppTool.IsNullOrEmpty(this.Quantity3)) {
            this.PushErrorMessage("Package Type");
        }
        if (AppTool.IsNullOrEmpty(this.PackageTypeId4) && !AppTool.IsNullOrEmpty(this.Quantity4)) {
            this.PushErrorMessage("Package Type");
        }
    }
    private ValidateContainers() {

        if (!AppTool.IsNullOrEmpty(this.ContainerNumber1) && AppTool.IsNullOrEmpty(this.Quantity1)) {
            this.PushErrorMessage("Quantity");
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber2) && AppTool.IsNullOrEmpty(this.Quantity2)) {
            this.PushErrorMessage("Quantity");
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber3) && AppTool.IsNullOrEmpty(this.Quantity3)) {
            this.PushErrorMessage("Quantity");
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber4) && AppTool.IsNullOrEmpty(this.Quantity4)) {
            this.PushErrorMessage("Quantity");
        } 

        if (!AppTool.IsNullOrEmpty(this.ContainerNumber1)) {
            this.ShowContainerValidationMessage(this.ContainerNumber1);
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber2)) {
            this.ShowContainerValidationMessage(this.ContainerNumber2);
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber3)) {
            this.ShowContainerValidationMessage(this.ContainerNumber3);
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber4)) {
            this.ShowContainerValidationMessage(this.ContainerNumber4);
        }
    }

    ShowContainerValidationMessage(ContainerNumber: string) {
        let validateContainerNumber = FormatTool.ValidateContainerNumber(ContainerNumber);
        if (validateContainerNumber != null)
            this.PushErrorMessage(validateContainerNumber);
    }
     

    ValidateLCLDShipmentType() {
        if (!(this.ShipmentTypeId == 'LCLD' && this.TransportModeId == 'O')) {
            return;
        }
        this.ValidatBookingNumberOfPackagese();

        if (AppTool.IsNullOrEmpty(this.OrderGrossWeight)) {
            this.PushErrorMessage("Gross Weight");
        }

        if (AppTool.IsNullOrEmpty(this.BookingVolume)) {
            this.PushErrorMessage("Volume");
        }
    }

    private ValidateAirExportShipmentFields() {
     
        if (!AppTool.IsNullOrEmpty(this.RequestedFlightDate)) {
            this.ValidateRequestedFlightDate();
        }
        if (AppTool.IsNullOrEmpty(this.CustomerReference3)) {
            this.PushErrorMessage("Reference");
        }

        if (AppTool.IsNullOrEmpty(this.IncotermId)) {
            this.PushErrorMessage("Incoterm");
        }

        this.ValidatBookingNumberOfPackagese();

        if (AppTool.IsNullOrEmpty(this.OrderGrossWeight)) {
            this.PushErrorMessage("Gross Weight");
        }

        if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
            this.PushErrorMessage("Destination");
        }

        if (this.ValidateDocuments()) {
            this.ValidationErrorsList.push("You should have at least one document shared with agent");
        }
    }

    private ValidateDocuments() {
        let AllowCreateAirExportShipmentsWithoutDocuments = this.AllowCreateShipmentsWithoutDocuments && this.DirectionId == "E" && this.TransportModeId == "A";
        return !this.IsDSVTenant && !AllowCreateAirExportShipmentsWithoutDocuments && (!this.documentsFilings || this.documentsFilings.filter(d => d.IsSharedWithForwarder == true).length == 0);
    }

    private ValidatBookingNumberOfPackagese() {
        if (AppTool.IsNullOrEmpty(this.BookingNumberOfPackages)) {
            this.PushErrorMessage("Number Of Packages");
        }

        if (!AppTool.IsNullOrEmpty(this.BookingNumberOfPackages)) {
            if (this.BookingNumberOfPackages > 999999999) {
                this.ValidationErrorsList.push("Number Of Packages must be less than ten digits");
            }
        }
    }

    ValidateRequestedFlightDate() { 
        if (this.RequestedFlightDate.getTime() < new Date().getTime()) { 
            this.ValidationErrorsList.push(this.RequestedDateLabel+ " must be for a future date");
        } 
    }



    private PushErrorMessage(  fieldName: string) { 
        this.ValidationErrorsList.push(this.errorMessage.replace("%FieldName", fieldName));
    }

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


            //this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            //this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

             //this.BuildShipmentSubTypes();
            //this.DelOrderDetails();
            //this.OnFiltersChanged();
          } 
    }

  

    public get ForwarderPartnerId() { return this.EntityPM.ForwarderPartnerId }
    public set ForwarderPartnerId(newValue: string) { this.EntityPM.ForwarderPartnerId = newValue; }


    public get DepartmentId() { return this.EntityPM.DepartmentId }
    public set DepartmentId(newValue: string) { this.EntityPM.DepartmentId = newValue; }

    public get ShipmentLevelCode() { return this.EntityPM.ShipmentLevelCode }
    public set ShipmentLevelCode(newValue: string) { this.EntityPM.ShipmentLevelCode = newValue; }


    public get CustomerReference3() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerReference3))
            return this.EntityPM.CustomerReference3;
        else
            return this.EntityPM.CustomerReference1;
    }
    public set CustomerReference3(newValue: string) {
        if (!AppTool.IsNullOrEmpty(newValue)) this.EntityPM.CustomerReference1 = newValue.substring(0, 50);
        this.EntityPM.CustomerReference3 = newValue;
    }


    get CustomerReference2() { return this.EntityPM.CustomerReference2; }
    set CustomerReference2(newValue: string) {
        if (this.EntityPM.CustomerReference2 != newValue) {
            this.EntityPM.CustomerReference2 = newValue;
        }
    }

    public get ShipperName() { return this.EntityPM.ShipperName }
    public set ShipperName(newValue: string) { this.EntityPM.ShipperName = newValue; }

    public get ConsigneeName() { return this.EntityPM.ConsigneeName }
    public set ConsigneeName(newValue: string) { this.EntityPM.ConsigneeName = newValue; }

    public get ShippingAgent() { return this.EntityPM.ShippingAgent  }
    public set ShippingAgent(newValue: string) { this.EntityPM.ShippingAgent  = newValue; }



    get PrivateLabelInvoiceNumber() { return this.EntityPM.PrivateLabelInvoiceNumber; }
    set PrivateLabelInvoiceNumber(newValue: string) {
        if (this.EntityPM.PrivateLabelInvoiceNumber != newValue) {
            this.EntityPM.PrivateLabelInvoiceNumber = newValue;
        }
    }

    public ToPortList: PortList = null;
    get MainCarriageToPortId() { return this.EntityPM.MainCarriageToPortId; }
    set MainCarriageToPortId(value: string) {
        if (this.EntityPM.MainCarriageToPortId != value) {
            this.EntityPM.MainCarriageToPortId = value;
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_Ports(); 
            if (AppTool.IsNullOrEmpty(value)) {
                this.ToPortList = null;
            }
            else {
                this.portListService.getSingle(value).subscribe((serviceResponse: ServiceResponse) => {
                    if (!serviceResponse.HasError) {
                        this.ToPortList = serviceResponse.Result;
                    }
                });
            }
        }
    }

    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(value: string) {
        if (this.EntityPM.MainCarriageFromPortId != value) {
            this.EntityPM.MainCarriageFromPortId = value;
            this.EntityPM.FromPortId = value;

            this.OnMainCarrigeFromPortChanged();
        }
    }
    public FromPortList: PortList = null;

    private OnMainCarrigeFromPortChanged() {
       
            //this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                this.FromPortList = null;
            }

            else {
                this.portListService.getSingle(this.MainCarriageFromPortId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.FromPortList = myResponse.Result;
                    }
                });
            }
       
    }

    SetUIProperties_Ports() { 
        var isToRequired: boolean = false; 
        if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
            isToRequired = true;
        } 
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, isToRequired);
    }
     
    AddPackageFirst() {
        var itemPM = new ShipmentPackagePM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.ShipmentId = this.EntityPM.Id; 
        this.RunAddEditPackage("Fill Dimensions");
    }
    RunAddEditPackage(windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        let windowArgs: any = {}; 
        logitudeWindow.Title = windowTitle;  
        windowArgs.CurrentEntityPM = this.EntityPM;
        windowArgs.DataViewModel = this;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelPackageComponent'); 
    }
      
    AddPackage() {
        var logeWindow = new LogitudeWindow();
        logeWindow.Width = 850;
        logeWindow.Title = "Fill Dimensions";
        logeWindow.WindowArgs = this.EntityPM;
        logeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelPackageComponent');
        logeWindow.WindowClosed.subscribe(s => {
            if (s) {
                   this.SetUIProperties_OrderDetails();
            }
        });
    }
    SetUIProperties_OrderDetails() {

        let isFieldsEnabled = this.EntityPM.ShipmentOrderPackages.length == 0 ? true : false; 
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isFieldsEnabled);  
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isFieldsEnabled);

    }

      
    private isShipmentLevelsListEnabled: boolean = false;
    get IsShipmentLevelsListEnabled() { return this.isShipmentLevelsListEnabled; }
    set IsShipmentLevelsListEnabled(value: boolean) {
        if (this.isShipmentLevelsListEnabled != value) {
            this.isShipmentLevelsListEnabled = value;
        }
    } 

    get OrderIsDangerouseGoods() { return this.EntityPM.OrderIsDangerouseGoods; }
    set OrderIsDangerouseGoods(newValue: boolean) {
        if (this.EntityPM.OrderIsDangerouseGoods != newValue) {
            this.EntityPM.OrderIsDangerouseGoods = newValue;
        }
    }

    get PrivateLabelIncludePickup() { return this.EntityPM.PrivateLabelIncludePickup; }
    set PrivateLabelIncludePickup(newValue: boolean) {
        if (this.EntityPM.PrivateLabelIncludePickup != newValue) {
            this.EntityPM.PrivateLabelIncludePickup = newValue;
        }
    } 

    get PrivateLabelIncludeDelivery() { return this.EntityPM.PrivateLabelIncludeDelivery; }
    set PrivateLabelIncludeDelivery(newValue: boolean) {
        if (this.EntityPM.PrivateLabelIncludeDelivery != newValue) {
            this.EntityPM.PrivateLabelIncludeDelivery = newValue;
        }
    }
     

    get BookingNumberOfPackages() { return this.EntityPM.BookingNumberOfPackages; }
    set BookingNumberOfPackages(newValue: number) {
        if (this.EntityPM.BookingNumberOfPackages != newValue) {
            this.EntityPM.BookingNumberOfPackages = newValue;
        }
    }

    get OrderGrossWeight() { return this.EntityPM.OrderGrossWeight; }
    set OrderGrossWeight(newValue: number) {
        if (this.EntityPM.OrderGrossWeight != newValue) {
            this.EntityPM.OrderGrossWeight = newValue;
        }
    }

    get BookingVolume() { return this.EntityPM.BookingVolume; }
    set BookingVolume(newValue: number) {
        if (this.EntityPM.BookingVolume != newValue) {
            this.EntityPM.BookingVolume = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    } 

    get RequestedFlightDate() { return this.EntityPM.RequestedFlightDate; }
    set RequestedFlightDate(newValue: Date) {
        if (this.EntityPM.RequestedFlightDate != newValue) {
            this.EntityPM.RequestedFlightDate = newValue;
        }
    }
      
    get IncotermId() { return this.EntityPM.IncotermId; }
    set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;

            if (newValue != null) {
                this.incotermListService.getSingle(newValue).subscribe((serviceResponse: ServiceResponse) => {
                    if (!serviceResponse.HasError) {
                        this.GetIncotermsFields(serviceResponse);
                    }
                });
            }
        }
    }


    private GetIncotermsFields(serviceResponse: ServiceResponse) {
        let incotermList: IncotermList = serviceResponse.Result;
        if (incotermList) {
            this.FreightPrepaidCollectId = incotermList.Freight;
            this.OtherPrepaidCollectId = incotermList.OtherCharges;
        }
    }
      
}
