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
import { AddEditPrivateLabelCustomsShipmentComponent } from './AddEditPrivateLabelCustomsShipmentComponent';
import { CustomerTenantAccessRequestExtendedPMService } from '../../../../Common/Services/ExtendedPMs/CustomerTenantAccessRequestExtendedPMService';



@Component({

    templateUrl: './AddEditPrivateLabelShipmentComponent.html',
})

export class AddEditPrivateLabelShipmentComponent extends AddEditPrivateLabelCustomsShipmentComponent {

    public TenantPM: TenantPM;
    public DirectionsList: FilterClass[] = [];
    public TransportModesList: FilterClass[] = [];
    public ShipmentTypesList: FilterClass[] = [];
    public ShipmentLevelsList: FilterClass[] = [];
    public ShipmentSubTypesList: FilterClass[] = [];
    public WarningErrorsList: any[];
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
    public customerTenantAccessRequestExtendedPMService: CustomerTenantAccessRequestExtendedPMService;
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
    public AllowCreateAirShipmentsWithoutDocuments: boolean = false;
    public AllowCreateOceanShipmentsWithoutDocuments: boolean = false;
    public Order: string = "Reference";
    public RequestedDateLabel: string = "Requested Flight Date";
    public FromTextCode: string;
    public ToTextCode: string;
    public ToPortTextCode: string;
    public isRTL: boolean = false;
    public IsNew: boolean = false;
    public HideDocumentSection: boolean = false;

    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.WarningErrorsList = [];
        this.InitializeServices();
        this.SetUIProperties();
        this.SetUIProperties_Filters();
        this.TenantPM = SessionLocator.TenantPM;
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.IsDSVTenant = SessionLocator.PrivateLableSettings.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1;
        
    }



    SetFromPort() {
        if (!this.IsNew) {
            this.FromPort = this.EntityPM.FromPortId;
            return;
        }

        this._PortExtendedPMService.getSinglePort("TLV", "IL", SessionLocator.Tenant).subscribe((Result: any) => {
            this.FromPort = Result.Result.Id;
        })
    }


    SetUnits() {
        if (!this.IsNew) return;
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

        this.SetExportShipmentLabels();

        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }

    }



    private SetExportShipmentLabels() {

        switch (this.TransportModeId) {
            case "A": {
                this.SetAirExportLabels();
                break;
            }

            case "O": {
                this.SetOceanExportLabel();
                break;
            }

            case "I":
            default: {
                this.SetDefaultExportLabel();
                break;
            }
        }
    }

    private SetDefaultExportLabel() {
        this.FromTextCode = "Shipment.S.NewShipment.From";
        this.ToTextCode = "Shipment.S.NewShipment.To";
    }

    private SetOceanExportLabel() {
        this.FromTextCode = "Shipment.S.NewShipment.LoadingPort";
        this.ToTextCode = "Shipment.S.NewShipment.DischargePort";
        this.Order = "Order Number";
        this.RequestedDateLabel = "Expected Sealing Date";
    }

    private SetAirExportLabels() {
        this.FromTextCode = "Shipment.S.NewShipment.Gateway";
        this.ToTextCode = "Shipment.S.NewShipment.Destination";
        this.Order = "Reference";
        this.RequestedDateLabel = "Requested Flight Date";
    }

    InitializeServices() {
        this.portListService = new PortListService();
        this.incotermListService = new IncotermListService();
        this.shipmentPMService = new ShipmentPMService();
        this.entityStatusListService = new EntityStatusListService();
        this.customerTenantAccessRequestExtendedPMService = new CustomerTenantAccessRequestExtendedPMService();
    }

    SetWindowArgs(args: any) {
        this.SetArgs(args);
        this.SetUnits();
        this.SetLabels();
        this.SetFromPort();
        this.SetExportShipmentArgs(args);
    }

    private SetExportShipmentArgs(args: any) {
        if (this.IsNew) this.SetNewExportShipmentArgs();
        else this.SetEditExportShipmentArgs();
        this.SetUIProperties();
    }

    private SetNewExportShipmentArgs() {
        this.GetBranch();
        this.GetDepartment();
        this.SetUIProperties_Filters();
    }

    private SetEditExportShipmentArgs() {
        this.BuildShipmentTypes();
        this.SetShipmentOrderPackages();
        this.SetUIProperties_OrderDetails();
    }

    private SetArgs(args: any) {
        this.IsNew = args.IsNew;
        if (args.HideDocumentSection) this.HideDocumentSection = true;
        this.SetIsCustomsActivated(args);
        this.args = args;
        if (args.EntityPM) {
            this.EntityPM = args.EntityPM;
        }
        this.AllowCreateAirShipmentsWithoutDocuments = SessionLocator.PrivateLableSettings.CreateShipmentsWithoutDocs;
        this.AllowCreateOceanShipmentsWithoutDocuments = SessionLocator.PrivateLableSettings.CreateOShipmentsWithoutDocs;
    }

    private SetIsCustomsActivated(args: any) {
        if (this.IsNew) {
            this.IsCustomsActivated = args.IsCustomsActivated;
            this.BuildFiltersLists();
            return;
        }
        this.customerTenantAccessRequestExtendedPMService.getByForwarderId(SessionLocator.Tenant, SessionLocator.PrivateLableSettings.HybridPartnerId).subscribe((res: any) => {
            if (!res.HasError) {
                this.IsCustomsActivated = res.Result.IsCustoms;
                this.BuildFiltersLists();
            }
        });
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
        this.UIProperties.SetRequired("ConsigneeName", this.ObjectTableName, false);
        this.UIProperties.SetRequired("HSCode", this.ObjectTableName, false);
        this.SetUIProperties_OceanTransportMode();
        this.SetUIProperties_Containers();
    }

    SetUIProperties_OceanTransportMode() {
        if (this.TransportModeId != "O") return;
        this.UIProperties.SetRequired("ShippingAgent", this.ObjectTableName, false);
        this.UIProperties.SetRequired("ShippingLine", this.ObjectTableName, false);
    }

    SetUIProperties_Filters() {
        if (!this.IsNew) return;
        this.DirectionId = 'E';
        this.TransportModeId = 'A';
    }

    BuildFiltersLists() {
        this.BuildDirectionsList();
        this.BuildTransportModesList();
        // this.BuildShipmentTypes();
    }

    ChangeTransportMode(code) {

        this.TransportModeId = code;
        if (this.TransportModeId == 'O') {
            this.ShipmentTypeId = 'FCLD'
        }
    }
    BuildShipmentTypes() {
        this.ShipmentTypesList = [];

        if (!(this.DirectionId && this.TransportModeId)) return;

        switch (this.TransportModeId) {
            case "O": {
                this.BuildOceanShipmentTypeList();
                break;
            }

            case "I": {
                this.BuildInlandShipmentTYpeList();
                break;
            }
        }

    }

    private BuildInlandShipmentTYpeList() {
        this.ShipmentTypesList.push(new FilterClass("FTL", "FTL", "./Images/CellIcons/Container.png"));
        this.ShipmentTypesList.push(new FilterClass("LTL", "LTL", "./Images/CellIcons/Package.png"));
    }

    private BuildOceanShipmentTypeList() {
        this.ShipmentTypesList.push(new FilterClass("FCLD", "FCL", "./Images/CellIcons/Container.png"));
        this.ShipmentTypesList.push(new FilterClass("LCLD", "LCL", "./Images/CellIcons/Package.png"));
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
        if (this.ShipmentTypeId != 'FCLD' || this.TransportModeId == 'A') return;
        this.AddShipmentOrderPackages();
        this.EntityPM.BookingNumberOfPackages = this.CalculateNumberOfPackages();

    }

    private AddShipmentOrderPackages() {

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
    }

    private hasPackage4() {
        return this.PackageTypeList4 != null && this.Quantity4 > 0 && this.Weight4 > 0;
    }

    private hasPackage3() {
        return this.PackageTypeList3 != null && this.Quantity3 > 0 && this.Weight3 > 0;
    }

    private hasPackage2() {
        return this.PackageTypeList2 != null && this.Quantity2 > 0 && this.Weight2 > 0;
    }

    private hasPackage1() {
        return this.PackageTypeList1 != null && this.Quantity1 > 0 && this.Weight1 > 0;
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
        item.GrossWeight = this.EntityPM.Weight4;
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
        item.GrossWeight = this.EntityPM.Weight3;
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
        item.GrossWeight = this.EntityPM.Weight2;
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
        item.GrossWeight = this.EntityPM.Weight1;
        this.EntityPM.AddOrderPackage(item);
        return item;
    }

    get ContainerNumber1() { return this.EntityPM.ContainerNumber1; }
    set ContainerNumber1(newValue: string) {
        if (this.EntityPM.ContainerNumber1 == newValue) return;

        this.EntityPM.ContainerNumber1 = newValue;
        this.SetUIProperties_Containers();
    }

    get ContainerNumber2() { return this.EntityPM.ContainerNumber2; }
    set ContainerNumber2(newValue: string) {
        if (this.EntityPM.ContainerNumber2 == newValue) return;

        this.EntityPM.ContainerNumber2 = newValue;
        this.SetUIProperties_Containers();
    }

    get ContainerNumber3() { return this.EntityPM.ContainerNumber3; }
    set ContainerNumber3(newValue: string) {
        if (this.EntityPM.ContainerNumber3 == newValue) return;

        this.EntityPM.ContainerNumber3 = newValue;
        this.SetUIProperties_Containers();

    }

    get ContainerNumber4() { return this.EntityPM.ContainerNumber4; }
    set ContainerNumber4(newValue: string) {
        if (this.EntityPM.ContainerNumber4 == newValue) return;

        this.EntityPM.ContainerNumber4 = newValue;
        this.SetUIProperties_Containers();

    }

    get Quantity1() { return this.EntityPM.Quantity1; }
    set Quantity1(newValue: number) {
        if (this.EntityPM.Quantity1 == newValue) return;

        this.EntityPM.Quantity1 = newValue;
        this.SetUIProperties_Containers();
    }


    SetUIProperties_Containers() {

        this.UIProperties.SetEnabled("PackageTypeId1", this.ObjectTableName, this.Quantity1 > 0);
        this.UIProperties.SetEnabled("PackageTypeId2", this.ObjectTableName, this.Quantity2 > 0);
        this.UIProperties.SetEnabled("PackageTypeId3", this.ObjectTableName, this.Quantity3 > 0);
        this.UIProperties.SetEnabled("PackageTypeId4", this.ObjectTableName, this.Quantity4 > 0);
        this.UIProperties.SetEnabled("Weight1", this.ObjectTableName, this.Quantity1 > 0);
        this.UIProperties.SetEnabled("Weight2", this.ObjectTableName, this.Quantity2 > 0);
        this.UIProperties.SetEnabled("Weight3", this.ObjectTableName, this.Quantity3 > 0);
        this.UIProperties.SetEnabled("Weight4", this.ObjectTableName, this.Quantity4 > 0);

        if (AppTool.IsNullOrZero(this.Quantity1)) {
            this.PackageTypeId1 = null;
            this.Weight1 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity2)) {
            this.PackageTypeId2 = null;
            this.Weight2 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity3)) {
            this.PackageTypeId3 = null;
            this.Weight3 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity4)) {
            this.PackageTypeId4 = null;
            this.Weight4 = null;
        }

    }

    get Quantity2() { return this.EntityPM.Quantity2; }
    set Quantity2(newValue: number) {
        if (this.EntityPM.Quantity2 == newValue) return;

        this.EntityPM.Quantity2 = newValue;
        this.SetUIProperties_Containers();

    }

    get Quantity3() { return this.EntityPM.Quantity3; }
    set Quantity3(newValue: number) {
        if (this.EntityPM.Quantity3 == newValue) return;

        this.EntityPM.Quantity3 = newValue;
        this.SetUIProperties_Containers();
    }

    get Quantity4() { return this.EntityPM.Quantity4; }
    set Quantity4(newValue: number) {
        if (this.EntityPM.Quantity4 == newValue) return;

        this.EntityPM.Quantity4 = newValue;
        this.SetUIProperties_Containers();
    }

    get Weight1() { return this.EntityPM.Weight1; }
    set Weight1(newValue: number) {
        if (this.EntityPM.Weight1 == newValue) return;
        this.EntityPM.Weight1 = newValue;
        this.SetUIProperties_Containers();
    }

    get Weight2() { return this.EntityPM.Weight2; }
    set Weight2(newValue: number) {
        if (this.EntityPM.Weight2 == newValue) return;
        this.EntityPM.Weight2 = newValue;
        this.SetUIProperties_Containers();
    }

    get Weight3() { return this.EntityPM.Weight3; }
    set Weight3(newValue: number) {
        if (this.EntityPM.Weight3 == newValue) return;
        this.EntityPM.Weight3 = newValue;
        this.SetUIProperties_Containers();
    }

    get Weight4() { return this.EntityPM.Weight4; }
    set Weight4(newValue: number) {
        if (this.EntityPM.Weight4 == newValue) return;
        this.EntityPM.Weight4 = newValue;
        this.SetUIProperties_Containers();
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
            this.SetPrivateLabelTranssportMode(newValue);
        }
    }



    private SetPrivateLabelTranssportMode(newValue: string) {
        this.EntityPM.TransportModeId = newValue;
        if (newValue == "A") {
            this.EntityPM.ShipmentTypeId = "Air";
            this.ContainerNumber = "";
        }
        this.SetExportShipmentType();
        this.BuildShipmentTypes();
        this.SetExportShipmentLabels();
    }

    private SetExportShipmentType() {
        if (this.EntityPM.DirectionId == "E" && this.TransportModeId == "O") {
            this.EntityPM.ShipmentTypeId = "FCLD";
        } else {
            this.EntityPM.ShipmentTypeId = null;
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
        if (this.DirectionId == 'C') this.SaveChanges();
        else this.UpsertExportShipment();
    }

    private UpsertExportShipment() {
        this.ValidateReferenceNumber();
        this.ValidateConsigneeNameField();
        this.ValidatePickUpDeliveryFields();
        this.ValidateHSCodeField();
        this.ValidateSealNumberField();
        this.ValidateRequiredFields();

        if (this.ValidationErrorsList.length != 0) {
            return;
        }
        if (this.IsNew) this.InitializeNewExportShipmentFields();
        else this.InitializeEditExportShipmentFields();
    }

    ValidateReferenceNumber() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.CustomerReference3) && AppTool.IsNullOrEmpty(this.PrivateLabelInvoiceNumber)) return;
        if (this.CustomerReference3?.toLowerCase().trim() == this.PrivateLabelInvoiceNumber?.toLowerCase().trim())
            this.ValidationErrorsList.push("The Reference and Invoice number should be different");
        if (!AppTool.IsNullOrEmpty(this.CustomerReference3) && this.CustomerReference3.length >= 30) {
            this.ValidationErrorsList.push("Reference Field must be less than 30");
        }
        if (!AppTool.IsNullOrEmpty(this.PrivateLabelInvoiceNumber) && this.PrivateLabelInvoiceNumber.length >= 30) {
            this.ValidationErrorsList.push("Invoice Number Field must be less than 30");
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
                this.NewShipmentMixPanelLocator(this.EntityPM);
                this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");

            }
        });
    }

    private UpdateShipment() {
        this.shipmentPMService.update(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (serviceResponse.HasError) {
                this.ValidationErrorsList = serviceResponse.ErrorsArray;
                return;
            }
            this.EntityPM = serviceResponse.Result;
            this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
            this.CurrentSession.CloseCurrentWindow();
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

    private InitializeNewExportShipmentFields() {

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
        this.EntityPM.IsShipmentOrder = this.TransportModeId == 'O';
        this.EntityPM.ShipmentOrderPackages = this.GetShipmentOrderPackages();
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

    private InitializeEditExportShipmentFields() {

        this.CurrentSession.StartBusyIndicator("Updating...");
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;
        this.EntityPM.IsImporterShipment = true;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.NewConcurrencyGUID = Guid.newGuid();
        this.EntityPM.ShipmentCustomerTypeCode = "SHI";
        this.EntityPM.MainCarriageFromPortId = this.TransportModeId == 'A' ? this.FromPort : this.MainCarriageFromPortId;
        this.EntityPM.FromPortId = this.FromPort;
        this.EntityPM.ShipmentOrderPackages = this.GetShipmentOrderPackages();
        this.SetOrderPackagesOnFinish();
        this.SetDocumentFilingIds();
        this.UpdateShipment();
    }

    GetShipmentOrderPackages(): ShipmentOrderPackagePM[] {
        if (this.EntityPM.ShipmentOrderPackages && this.EntityPM.ShipmentOrderPackages.length != 0) return this.EntityPM.ShipmentOrderPackages;
        var shipmentOrderPackage = new ShipmentOrderPackagePM(null);
        shipmentOrderPackage.Tenant = this.EntityPM.Tenant;
        shipmentOrderPackage.IsContainer = false;
        shipmentOrderPackage.Quantity = this.BookingNumberOfPackages;
        shipmentOrderPackage.GrossWeight = this.OrderGrossWeight;
        shipmentOrderPackage = this.CalculateShipmentOrderPackageVolume(shipmentOrderPackage);

        var shipmentOrderPackages = [];
        shipmentOrderPackages.push(shipmentOrderPackage);
        return shipmentOrderPackages;
    }

    SetShipmentOrderPackages(): ShipmentOrderPackagePM[] {
        if (!this.EntityPM.ShipmentOrderPackages || this.EntityPM.ShipmentOrderPackages.length == 0) return;
        if (this.ShipmentTypeId != 'FCLD' || this.TransportModeId == 'A') return;

        this.EntityPM.ShipmentOrderPackages.forEach((shipmentOrderPackage, index) => {
            this.FillShipmentOrderPackage(index, shipmentOrderPackage);
        });
    }

    private FillShipmentOrderPackage(index: number, shipmentOrderPackage: ShipmentOrderPackagePM) {
        let fieldNumber = index + 1;
        this['ContainerNumber' + fieldNumber] = shipmentOrderPackage.ContainerNumber;
        this['Quantity' + fieldNumber] = shipmentOrderPackage.Quantity;
        this['PackageTypeId' + fieldNumber] = shipmentOrderPackage.PackageTypeId;
        this['Weight' + fieldNumber] = shipmentOrderPackage.GrossWeight;
    }

    CalculateShipmentOrderPackageVolume(shipmentOrderPackage: ShipmentOrderPackagePM): ShipmentOrderPackagePM {
        if (this.BookingVolume) return shipmentOrderPackage;
        shipmentOrderPackage.VolumetricWeight = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, shipmentOrderPackage.GrossWeight);
        shipmentOrderPackage.Volume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, shipmentOrderPackage.VolumetricWeight, this.EntityPM.Ratio)
        return shipmentOrderPackage;
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

    private ValidateConsigneeNameField() {
        if (AppTool.IsNullOrEmpty(this.ConsigneeName)) {
            this.PushErrorMessage("Consignee/Importer Name");
        }
        if (!AppTool.IsNullOrEmpty(this.ConsigneeName) && this.ConsigneeName.length >= 30) {
            this.ValidationErrorsList.push("Consignee/Importer Name Field Length must be less than 30");
        }
    }

    private ValidatePickUpDeliveryFields() {
        if (!AppTool.IsNullOrEmpty(this.PlaceOfDelivery) && this.PlaceOfDelivery.length >= 30) {
            this.ValidationErrorsList.push("Place Of Delivery Field Length must be less than 30");
        }
        if (!AppTool.IsNullOrEmpty(this.PickupPlace) && this.PickupPlace.length >= 30) {
            this.ValidationErrorsList.push("Pickup Place Field Length must be less than 30");
        }
    }

    private ValidateHSCodeField() {
        if (!this.OrderIsDangerouseGoods) return;
        if (AppTool.IsNullOrEmpty(this.HSCode)) {
            this.PushErrorMessage("HS/UN Code");
        }
        if (!AppTool.IsNullOrEmpty(this.HSCode) && this.HSCode.length >= 30) {
            this.ValidationErrorsList.push("HS/UN Code Name Field Length must be less than 30");
        }
    }

    private ValidateSealNumberField() {
        if (this.TransportModeId != "O") return;
        if (!AppTool.IsNullOrEmpty(this.SealNo) && this.SealNo.length >= 30) {
            this.ValidationErrorsList.push("Seal No. Field Length must be less than 30");
        }
    }
    public ValidateContainerNumber(input: string) {
        this.WarningErrorsList = [];
        let result = FormatTool.ValidateContainerNumber(input);

        if (result != null) {
            this.WarningErrorsList.push(result);
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

        if (AppTool.IsNullOrEmpty(this.ShippingAgent)) {
            this.PushErrorMessage("Shipping Agent");
        }

        if (AppTool.IsNullOrEmpty(this.ShippingLine)) {
            this.PushErrorMessage("Shipping Line");
        }

        if (!AppTool.IsNullOrEmpty(this.ShippingAgent) && this.ShippingAgent.length >= 30) {
            this.ValidationErrorsList.push("Shipping Agent Field Length must be less than 30");
        }

        if (!AppTool.IsNullOrEmpty(this.ShippingLine) && this.ShippingLine.length >= 30) {
            this.ValidationErrorsList.push("Shipping Line Field Length must be less than 30");
        }

        if (!AppTool.IsNullOrEmpty(this.RequestedFlightDate)) {
            this.ValidateRequestedFlightDate();
        }


        if (this.ValidateOceanShipmentDocuments()) {
            this.ValidationErrorsList.push("You should have at least one document shared with agent");
        }

        this.ValidateLCLDShipmentType();

        this.ValidateFCLDShipmentType();

    }

    private ValidateOceanShipmentDocuments() {
        if (this.HideDocumentSection) return false;
        let AllowCreateOceanExportShipmentsWithoutDocuments = this.AllowCreateOceanShipmentsWithoutDocuments && this.DirectionId == "E" && this.TransportModeId == "O";
        return !this.IsDSVTenant && !AllowCreateOceanExportShipmentsWithoutDocuments && (!this.documentsFilings || this.documentsFilings.filter(d => d.IsSharedWithForwarder == true).length == 0);
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
        this.validateWeight();
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

        this.WarningErrorsList = [];
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
    private validateWeight() {
        if (AppTool.IsNullOrEmpty(this.Weight1) && !AppTool.IsNullOrEmpty(this.Quantity1)) {
            this.PushErrorMessage("Weight");
        }
        if (AppTool.IsNullOrEmpty(this.Weight2) && !AppTool.IsNullOrEmpty(this.Quantity2)) {
            this.PushErrorMessage("Weight");
        }
        if (AppTool.IsNullOrEmpty(this.Weight3) && !AppTool.IsNullOrEmpty(this.Quantity3)) {
            this.PushErrorMessage("Weight");
        }
        if (AppTool.IsNullOrEmpty(this.Weight4) && !AppTool.IsNullOrEmpty(this.Quantity4)) {
            this.PushErrorMessage("Weight");
        }
    }

    ShowContainerValidationMessage(ContainerNumber: string) {
        let validateContainerNumber = FormatTool.ValidateContainerNumber(ContainerNumber);
        if (validateContainerNumber != null)
            this.WarningErrorsList.push(validateContainerNumber);
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

        if (this.ValidateAirShipmentDocuments()) {
            this.ValidationErrorsList.push("You should have at least one document shared with agent");
        }
    }

    private ValidateAirShipmentDocuments() {
        if (this.HideDocumentSection) return false;
        let AllowCreateAirExportShipmentsWithoutDocuments = this.AllowCreateAirShipmentsWithoutDocuments && this.DirectionId == "E" && this.TransportModeId == "A";
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
        let requestedFlightDate = this.GetRequestedFlightDate();
        if (requestedFlightDate < new Date().getTime()) {
            this.ValidationErrorsList.push(this.RequestedDateLabel + " must be for a future date");
        }
    }

    GetRequestedFlightDate() {
        if (!AppTool.IsNullOrEmpty(this.RequestedFlightDate) && typeof (this.RequestedFlightDate) == "string") {
            return new Date(this.RequestedFlightDate);
        }

        return this.RequestedFlightDate.getTime();
    }



    private PushErrorMessage(fieldName: string) {
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
        this.EntityPM.CustomerReference1 = !AppTool.IsNullOrEmpty(newValue) ? newValue.substring(0, 50) : null;
        this.EntityPM.CustomerReference3 = newValue;
        this.ValidateReferenceNumber();
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

    public get ShippingAgent() { return this.EntityPM.ShippingAgent }
    public set ShippingAgent(newValue: string) { this.EntityPM.ShippingAgent = newValue; }

    public get ShippingLine() { return this.EntityPM.ShippingLine }
    public set ShippingLine(newValue: string) { this.EntityPM.ShippingLine = newValue; }

    public get PlaceOfDelivery() { return this.EntityPM.PlaceOfDelivery }
    public set PlaceOfDelivery(newValue: string) { this.EntityPM.PlaceOfDelivery = newValue; }

    public get PickupPlace() { return this.EntityPM.PickupPlace }
    public set PickupPlace(newValue: string) { this.EntityPM.PickupPlace = newValue; }

    public get SealNo() { return this.EntityPM.SealNo }
    public set SealNo(newValue: string) { this.EntityPM.SealNo = newValue; }

    public get HSCode() { return this.EntityPM.HSCode }
    public set HSCode(newValue: string) { this.EntityPM.HSCode = newValue; }


    get PrivateLabelInvoiceNumber() { return this.EntityPM.PrivateLabelInvoiceNumber; }
    set PrivateLabelInvoiceNumber(newValue: string) {
        if (this.EntityPM.PrivateLabelInvoiceNumber != newValue) {
            this.EntityPM.PrivateLabelInvoiceNumber = newValue;
            this.ValidateReferenceNumber();
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

    GetTableHeight() {
        if (this.TransportModeId == "O" && !this.PrivateLabelIncludePickup && !this.PrivateLabelIncludeDelivery) return 590;
        if (this.TransportModeId == "O" && (this.PrivateLabelIncludePickup || this.PrivateLabelIncludeDelivery)) return 622;
        return 555;
    }

}
