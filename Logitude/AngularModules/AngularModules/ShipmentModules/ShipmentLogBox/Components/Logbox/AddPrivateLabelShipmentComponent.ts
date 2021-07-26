import { Component } from '@angular/core';
import { IncotermList } from '../../../../Common/EntityLists/IncotermList';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { BranchListService } from '../../../../Common/Services/StandardLists/BranchListService';
import { DepartmentListService } from '../../../../Common/Services/StandardLists/DepartmentListService';
import { IncotermListService } from '../../../../Common/Services/StandardLists/IncotermListService';
import { PortListService } from '../../../../Common/Services/StandardLists/PortListService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { EntityStatusListService } from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { FilterClass } from '../../../../Shipment/Components/NewEntity/NewShipmentComponent';
import { ShipmentSubTypeList } from '../../../../shipment/EntityLists/ShipmentSubTypeList';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { AddEditPrivateLabelShipmentComponent } from './AddEditPrivateLabelShipmentComponent';



@Component({

    templateUrl: './AddPrivateLabelShipmentComponent.html',
})

export class AddPrivateLabelShipmentComponent extends AddEditPrivateLabelShipmentComponent {


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
    private entityResourceService: EntityResourceService;
    public ScreenOpacity: number = 1;  

    constructor() {
        super();
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.InitializeServices();
        this.BuildFiltersLists();
        this.LoadEntityResource(this.ObjectTableName);
    }

    LoadEntityResource(objectTableName: string) { 
 
        this.entityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response: any) => { 

        });


    }
    InitializeServices() {
        this.portListService = new PortListService();
        this.incotermListService = new IncotermListService();
        this.shipmentPMService = new ShipmentPMService();  
        this.entityStatusListService = new EntityStatusListService();
        this.entityResourceService= new EntityResourceService();
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
    }

    private SetArgs(args: any) {
        this.args = args;
        if (args.EntityPM) {
            this.EntityPM = args.EntityPM;
        }
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
        this.SetUIProperties_Filters();
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, true);

    }

    SetUIProperties_Filters() {
        this.DirectionId = 'E';
        this.TransportModeId = 'A';  
    }

    BuildFiltersLists() { 
        this.BuildDirectionsList(); 
        this.BuildTransportModesList();  
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
        this.DirectionsList.push(new FilterClass("C", "Customs"));
    }

    public get StatusId() { return this.EntityPM.StatusId }
    public set StatusId(newValue: string) { this.EntityPM.StatusId = newValue; }

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
            else {
                this.EntityPM.ShipmentTypeId = null;
            } 
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
            this.ValidateRequiredFields();

            if (this.ValidationErrorsList.length == 0) { 
                this.InitializeExportShipmentFields(); 
                this.SetForwarderPartner(); 
                this.InsertShipment();
            }
        }

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
        this.EntityPM.MainCarriageFromPortId = this.EntityPM.MainCarriageToPortId;
        this.EntityPM.OtherPrepaidCollectId = "C";
        this.EntityPM.FreightPrepaidCollectId = "C";
        this.EntityPM.ShipmentLevelCode = "A";
    }

    private ValidateRequiredFields() { 

        if (!AppTool.IsNullOrEmpty(this.RequestedFlightDate)) {
            this.ValidateRequestedFlightDate();
        }
        if (AppTool.IsNullOrEmpty(this.CustomerReference2)) {
            this.PushErrorMessage("Reference");
        }

        if (AppTool.IsNullOrEmpty(this.IncotermId)) {
            this.PushErrorMessage("Incoterm");  
        }

        this.ValidatBookingNumberOfPackagese();

        if (AppTool.IsNullOrEmpty(this.OrderGrossWeight)) {
            this.PushErrorMessage("Gross Weight (MT)");  
        }

        if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
            this.PushErrorMessage("Destination");  
        }
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
            this.ValidationErrorsList.push("Requested flight date must be for a future date");
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
          } 
    }


    public get ForwarderPartnerId() { return this.EntityPM.ForwarderPartnerId }
    public set ForwarderPartnerId(newValue: string) { this.EntityPM.ForwarderPartnerId = newValue; }


    public get DepartmentId() { return this.EntityPM.DepartmentId }
    public set DepartmentId(newValue: string) { this.EntityPM.DepartmentId = newValue; }

    public get ShipmentLevelCode() { return this.EntityPM.ShipmentLevelCode }
    public set ShipmentLevelCode(newValue: string) { this.EntityPM.ShipmentLevelCode = newValue; }


    public get CustomerReference1() { return this.EntityPM.CustomerReference1 }
    public set CustomerReference1(newValue: string) { this.EntityPM.CustomerReference1 = newValue; }


    get CustomerReference2() { return this.EntityPM.CustomerReference2; }
    set CustomerReference2(newValue: string) {
        if (this.EntityPM.CustomerReference2 != newValue) {
            this.EntityPM.CustomerReference2 = newValue;
        }
    }

    get ConsigneeName() { return this.EntityPM.ConsigneeName; }
    set ConsigneeName(newValue: string) {
        if (this.EntityPM.ConsigneeName != newValue) {
            this.EntityPM.ConsigneeName = newValue;
        }
    }


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

    MainCarriageFromPortId = null;
    SetUIProperties_Ports() { 
        var isToRequired: boolean = false; 
        if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
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
