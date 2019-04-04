import {Component, EventEmitter, Output, ComponentRef} from '@angular/core';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../EntityPMs/ShipmentPackagePM';
import {DateTool,AppTool} from '../../../Infrastructure/Tools';
import {ShipmentTool, ByPckageType} from '../../Tools';
import {ShipmentValidator} from '../../Validators/ShipmentValidator';
import {EntityWarningsValidator} from '../../../Infrastructure/Validators/EntityWarningsValidator';
import {RulesValidator} from '../../../Infrastructure/Validators/RulesValidator';
import {ObjectFieldPM} from '../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentMenuButtonsHandler} from './ShipmentMenuButtonsHandler';
import {PackageTypeListService} from '../../../Common/Services/StandardLists/PackageTypeListService';
import {PackageTypeList} from '../../../Common/EntityLists/PackageTypeList';
import {ConsoleShipmentPM} from '../../EntityPMs/ConsoleShipmentPM';
import {ShipmentDomainService} from '../../Services/ShipmentDomainService';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {DateTimePipe} from '../../../Controls/Pipes/DateTimePipe';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './MasterActionConfirmationComponent.html',
})

export class MasterActionConfirmationComponent extends BaseComponent {

    private FatherComponent: ShipmentMenuButtonsHandler = null;
    private MasterViewModel: MasterActionConfirmationViewModel;
    public SelectedItem: any;
    public shipmentService: ShipmentDomainService;
    public MasterLists = [];
    public FCLVisibility: boolean = false;
    public LCLVisibility: boolean = false;
    public GroupageVisibility: boolean = false;
    public ErrorList = [];
    private CurrentSession = SessionLocator.SelectedSession;
    SetWindowArgs(args: ShipmentMenuButtonsHandler) {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.shipmentService = new ShipmentDomainService();
        this.shipmentService.GetConnectedShipmentsByMasterIdAndTenant(args.EntityPM.Id, args.EntityPM.Tenant).subscribe(response => {
            if (!response.HasError && response.Result) {                                
                this.FatherComponent = args;
    
                this.shipmentService.GetShipmentConsolidationPackages(this.FatherComponent.EntityPM.Id).subscribe(result => {
                    this.FatherComponent.EntityPM.IsOperationalClosed = true;                  
                    this.MasterViewModel = new MasterActionConfirmationViewModel(this.FatherComponent.EntityPM, result.Result);
                    this.MasterViewModel.IsMaster = true;
                    this.MasterViewModel.ValidateShipment(this.FatherComponent.EntityPM);
                    var connectedShipments: Array<ShipmentPM> = response.Result;
                    this.MasterLists.push(this.MasterViewModel);
                    connectedShipments.forEach(shipment => {
                        shipment.IsOperationalClosed = true;
                        var viewmodel: MasterActionConfirmationViewModel = new MasterActionConfirmationViewModel(shipment);
                        this.MasterViewModel.ConnectedShipments.push(viewmodel);
                        viewmodel.ValidateShipment(shipment);
                        this.MasterLists.push(viewmodel);
                    });          

                    var actionSucceeded: boolean = false;
                    actionSucceeded = !this.MasterViewModel.HasErrors();

                    this.LCL_ObsList = this.MasterViewModel.LCL_ObsList;
                    this.FCL_ObsList1 = this.MasterViewModel.FCL_ObsList1;
                    this.FCL_ObsList2 = this.MasterViewModel.FCL_ObsList2;
                    this.GRO_ObsList = this.MasterViewModel.GRO_ObsList;
                    this.EnabledOkButton = this.ConfirmIsEnabled(this.MasterViewModel.HasErrors());
                    this.CurrentSession.StopBusyIndicator();
                    this.FCLVisibility = this.MasterViewModel.FCLVisibility && this.MasterViewModel.MasterVSHousesVisibility;
                    this.LCLVisibility = this.MasterViewModel.LCLVisibility && this.MasterViewModel.MasterVSHousesVisibility;
                    this.GroupageVisibility = this.MasterViewModel.GroupageVisibility && this.MasterViewModel.MasterVSHousesVisibility;
                    this.MasterVSHousesVisibility = this.MasterViewModel.MasterVSHousesVisibility;
                    this.ErrorList = this.MasterViewModel.ErrorList;
                    if (this.MasterViewModel.ErrorList.length > 0)
                        this.EnabledOkButton = false;

                });

            
            }

    
        });
    }

    
   



    public MasterVSHousesVisibility: boolean = false;

    public LCL_ObsList: Array<LineData>=[];
    public FCL_ObsList1: Array<LineData>=[];
    public FCL_ObsList2: Array<LineData>=[];
    public GRO_ObsList: Array<LineData>=[];
  
    public EnabledOkButton: boolean = false;
    ConfirmIsEnabled(prop: boolean) { return !prop; }


    OkButtonClicked() {      
            this.CurrentSession.CloseCurrentWindowEmit("confirm");        
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
        
    }



}


export class MasterActionConfirmationViewModel {
    public CurrentShipment: ShipmentPM;
    private masterPM: ShipmentPM;
    private isMaster: boolean;
    public get IsMaster() { return this.isMaster; }
    public DatePipe: DateTimePipe;

    public set IsMaster(value: boolean) { this.isMaster = value; }
    public houseShipmentsPackaes: Array<ShipmentPackagePM>;
    public LCL_ObsList: Array<LineData>;
    public FCL_ObsList1: Array<LineData>;
    public FCL_ObsList2: Array<LineData>;
    public GRO_ObsList: Array<LineData>;
    public get MasterVSHousesVisibility() {

        if (this.masterPM.ShipmentConsoleShipments.length > 0) {
            return true;
        }

        return false;


    }

    public get FCLVisibility() {
        if (!ShipmentTool.IsLCL(this.masterPM)) {
            if (!this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                return true;
            }
        }
        return false;        
    }
    public shipmentService: ShipmentDomainService;
    constructor(shipment: ShipmentPM, houseShipmentsPackaes: Array<ShipmentPackagePM> = null) {
        this.DatePipe = new DateTimePipe();
        this.shipmentService = new ShipmentDomainService();
        this.CurrentShipment = shipment;
        this.masterPM = shipment;
        if (houseShipmentsPackaes == null)
            this.houseShipmentsPackaes = new Array<ShipmentPackagePM>();
        else
            this.houseShipmentsPackaes = houseShipmentsPackaes;

        this.LCL_ObsList = new Array<LineData>();
        this.FCL_ObsList1 = new Array<LineData>();
        this.FCL_ObsList2 = new Array<LineData>();
        this.GRO_ObsList = new Array<LineData>();
        this.LoadHousePackages();        
    }

    public get GroupageVisibility() {    
        if (!ShipmentTool.IsLCL(this.masterPM)) {
            if (this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                return true;
            }
        }
        return false;
    }

    public get LCLVisibility() {return ShipmentTool.IsLCL(this.masterPM) ? true : false; }


    LoadHousePackages() {
        if (this.masterPM.ShipmentConsoleShipments.length > 0) {
                this.BuildData();
        }
    }
   private BuildLCLData() {
       this.LCL_ObsList = [];
       var HouseValueSum = 0;
       var MasterValueSum = 0;
       var IsEqualsSum= false;
       var LineImageSum = "";
       this.houseShipmentsPackaes.forEach(item => { if (item.Quantity != null) HouseValueSum += item.Quantity; });
       this.masterPM.ShipmentPackages.forEach(item => { if (item.Quantity != null) MasterValueSum += item.Quantity; });
       if (HouseValueSum == MasterValueSum) { IsEqualsSum = true; LineImageSum = "/Images/SimplogIcons/GreenTick.png" }
       else LineImageSum = "/Images/SimplogIcons/Warning.png";
       var line1: LineData = new LineData();       
           line1.LineLabel = "Total packages";
           line1.HouseValue = HouseValueSum;
           line1.MasterValue = MasterValueSum;
           line1.IsEquals = IsEqualsSum;
           line1.LineImage = LineImageSum;
       ////////////////////////////////////////////////////
           HouseValueSum = 0;
           MasterValueSum = 0;
           IsEqualsSum = false;
           LineImageSum = "";
           this.masterPM.ShipmentConsoleShipments.forEach(item => { if (item.GrossWeight != null) HouseValueSum += item.GrossWeight; });
           MasterValueSum = this.masterPM.GrossWeight;
           if (HouseValueSum == MasterValueSum) { IsEqualsSum = true; LineImageSum = "/Images/SimplogIcons/GreenTick.png" }
           else LineImageSum = "/Images/SimplogIcons/Warning.png";
           var line2: LineData = new LineData();       
           line2.LineLabel = "Gross Weight";
           line2.HouseValue = HouseValueSum;
           line2.MasterValue = MasterValueSum;
           line2.IsEquals = IsEqualsSum;
           line2.LineImage = LineImageSum;

       ////////////////////////////////////////////////////
           HouseValueSum = 0;
           MasterValueSum = 0;
           IsEqualsSum = false;
           LineImageSum = "";
           this.masterPM.ShipmentConsoleShipments.forEach(item => { if (item.VolumetricWeight != null) HouseValueSum += item.VolumetricWeight; });
           MasterValueSum = this.masterPM.VolumetricWeight;
           if (HouseValueSum == MasterValueSum) { IsEqualsSum = true; LineImageSum = "/Images/SimplogIcons/GreenTick.png" }
           else LineImageSum = "/Images/SimplogIcons/Warning.png";

           var line3: LineData = new LineData();       
           line3.LineLabel = "Vol Weight";
           line3.HouseValue = HouseValueSum;
           line3.MasterValue = MasterValueSum;
           line3.IsEquals = IsEqualsSum;
           line3.LineImage = LineImageSum;
           this.LCL_ObsList.push(line1);
           this.LCL_ObsList.push(line2);
           this.LCL_ObsList.push(line3);

    }
   private BuildFCLData() {

       this.FCL_ObsList1 = [];
       this.FCL_ObsList2 = [];

       var fclHousesGroup: Array<ByPckageType> = [];
       var fclMasterGroup: Array<ByPckageType> = [];
       var PackListservice: PackageTypeListService = new PackageTypeListService();
       var CachedList: any;
       PackListservice.getAllFromCache().subscribe((result:any) => {
           if (!result.HasError) {
               CachedList = result.Result;
               this.houseShipmentsPackaes = this.houseShipmentsPackaes.filter(p => p.IsContainer == true);
               this.houseShipmentsPackaes.forEach(item => {
                   var existsedItem = fclHousesGroup.filter(f => f.PackageTypeId == item.PackageTypeId)[0];
                   if (existsedItem == null) {
                       existsedItem = new ByPckageType();
                       existsedItem.PackageTypeId = item.PackageTypeId;
                       existsedItem.Quantity = item.Quantity;
                       existsedItem.MeasurementId = (CachedList.filter(f => f.Id == item.PackageTypeId)[0]) != null ? (CachedList.filter(f => f.Id == item.PackageTypeId)[0]).MeasurementId : null;
                       fclHousesGroup.push(existsedItem);
                   }
                   else {
                       existsedItem.Quantity += item.Quantity;
                   }
               });

               this.masterPM.ShipmentPackages = this.masterPM.ShipmentPackages.filter(p => p.IsContainer == true);
               this.masterPM.ShipmentPackages.forEach(item => {
                   var existsedItem = fclMasterGroup.filter(f => f.PackageTypeId == item.PackageTypeId)[0];
                   if (existsedItem == null) {
                       existsedItem = new ByPckageType();
                       existsedItem.PackageTypeId = item.PackageTypeId;
                       existsedItem.Quantity = item.Quantity;
                       existsedItem.MeasurementId = (CachedList.filter(f => f.Id == item.PackageTypeId)[0]) != null ? (CachedList.filter(f => f.Id == item.PackageTypeId)[0]).MeasurementId : null;
                       fclMasterGroup.push(existsedItem);
                   }
                   else {
                       existsedItem.Quantity += item.Quantity;
                   }
               });

               fclHousesGroup.forEach(houseItem => {

                   var list: PackageTypeList = CachedList.filter(f => f.Id == houseItem.PackageTypeId)[0];
                   if (list != null) {
                       var line: LineData = new LineData();
                       line.LineLabel = list.EnglishName;
                       line.HouseValue = houseItem.Quantity;

                       var masterItem: ByPckageType = fclMasterGroup.filter(f => f.PackageTypeId == houseItem.PackageTypeId && f.MeasurementId == houseItem.MeasurementId)[0];
                       if (masterItem != null) {
                           line.MasterValue = masterItem.Quantity;
                           line.IsEquals = (houseItem.Quantity == masterItem.Quantity);
                           line.LineImage = (houseItem.Quantity == masterItem.Quantity) ? "/Images/SimplogIcons/GreenTick.png" : "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                           var temp: Array<ByPckageType> = [];
                           fclMasterGroup.forEach(p => {
                               if (p != masterItem)
                                   temp.push(p);
                           });
                           fclMasterGroup = temp;
                       }

                       else {
                           line.MasterValue = 0;
                           line.IsEquals = false;
                           line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                       }
                       this.FCL_ObsList1.push(line);


                   }
               });

               fclMasterGroup.forEach(masterItem => {
                   var list: PackageTypeList = CachedList.filter(f => f.Id == masterItem.PackageTypeId)[0];
                   if (list != null) {
                       var line: LineData = new LineData();
                       line.LineLabel = list.EnglishName;
                       line.HouseValue = 0;
                       line.MasterValue = masterItem.Quantity;
                       line.IsEquals = false;
                       line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                       this.FCL_ObsList1.push(line);
                   }
               });

               var masterPackages: Array<ShipmentPackagePM> = this.masterPM.ShipmentPackages;
               this.houseShipmentsPackaes.sort((a, b) => { return (a.ShipmentId === b.ShipmentId) ? 0 : (a.ShipmentId < b.ShipmentId) ? -1 : 1 });
               this.houseShipmentsPackaes.forEach(houseItem => {
                   var list: PackageTypeList = CachedList.filter(p => p.Id == houseItem.PackageTypeId)[0];
                   if (list != null) {

                       var line: LineData = new LineData()
                       line.ShipmentNumber = houseItem.ShipmentNumber;
                       line.LineLabel = list.EnglishName;
                       line.HouseStringValue = AppTool.IsNullOrEmpty(houseItem.ContainerNumber) ? "- - -" : houseItem.ContainerNumber;

                       if (AppTool.IsNullOrEmpty(line.ShipmentNumber)) {
                           var dd: ConsoleShipmentPM = this.masterPM.ShipmentConsoleShipments.filter(d => d.Id == houseItem.ShipmentId)[0];
                           if (dd != null) {
                               line.ShipmentNumber = dd.ShipmentNumber;
                           }
                       }


                       var masterItem: ShipmentPackagePM = masterPackages.filter(d => d.OriginalShipmentPackageId == houseItem.Id)[0];
                       if (masterItem != null) {
                           var temp: Array<ShipmentPackagePM> = [];
                           masterPackages.forEach(p => {
                               if (p != masterItem)
                                   temp.push(p);
                           });
                           masterPackages = temp;
                           line.MasterStringValue = AppTool.IsNullOrEmpty(masterItem.ContainerNumber) ? "- - -" : masterItem.ContainerNumber;
                           line.IsEquals = (line.HouseStringValue == line.MasterStringValue);
                           line.LineImage = (line.HouseStringValue == line.MasterStringValue) ? "/Images/SimplogIcons/GreenTick.png" : "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                       }
                       else {
                           line.MasterStringValue = "Not exists";
                           line.IsEquals = false;
                           line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png";
                       }

                       if (!line.IsEquals) {
                           this.FCL_ObsList2.push(line);
                       }

                   }


               });

               masterPackages.forEach(item => {
                   var list: PackageTypeList = CachedList.filter(d => d.Id == item.PackageTypeId)[0];
                   if (list != null) {
                       var line: LineData = new LineData()

                       line.ShipmentNumber = this.masterPM.ShipmentNumber,
                           line.LineLabel = list.EnglishName,
                           line.HouseStringValue = "Not exists",
                           line.MasterStringValue = AppTool.IsNullOrEmpty(item.ContainerNumber) ? "- - -" : item.ContainerNumber,
                           line.IsEquals = false,
                           line.LineImage = "/Logitude.ApplicationThemes;component/Images/SimplogIcons/deleteicon.png",
                           this.FCL_ObsList2.push(line);

                   }

               });


               if (this.FCL_ObsList1.filter(d => d.IsEquals == false)[0] || this.FCL_ObsList2.filter(d => d.IsEquals == false)[0]) {
                   this.ErrorInfoVisibility = true;
                   this.MismatchError = "Mismatch Quantities or Container numbers";
                   this.ErrorList.push(this.MismatchError);
               }

           }

       });
     

   }

    

   private errorInfoVisibility: boolean = false;
   public get ErrorInfoVisibility() { return this.errorInfoVisibility; }
   public set ErrorInfoVisibility(value: boolean) { this.errorInfoVisibility = value; }

   private mismatchError: string = "";
   public get MismatchError() { return this.mismatchError; }
   public set MismatchError(value: string) { this.mismatchError = value; }
   public ErrorList = [];

   private BuildGroupageData() {

       this.GRO_ObsList=[];

       var HouseValueSum = 0;
       var MasterValueSum = 0;
       var IsEqualsSum = false;
       var LineImageSum = "";
       this.houseShipmentsPackaes.forEach(item => { if (item.Quantity != null) HouseValueSum += item.Quantity; });
       this.masterPM.ShipmentPackages.forEach(item => {
           item.InsideShipmentPackages.forEach(d => {
               if (d.Quantity != null)
                   MasterValueSum += d.Quantity;
           });
       });

       if (HouseValueSum == MasterValueSum) { IsEqualsSum = true; LineImageSum = "/Images/SimplogIcons/GreenTick.png" }
       else LineImageSum = "/Images/SimplogIcons/Warning.png";
       var line1: LineData = new LineData();
       line1.LineLabel = "Total packages";
       line1.HouseValue = HouseValueSum;
       line1.MasterValue = MasterValueSum;
       line1.IsEquals = IsEqualsSum;
       line1.LineImage = LineImageSum;
       ////////////////////////////////////////////////////
       HouseValueSum = 0;
       MasterValueSum = 0;
       IsEqualsSum = false;
       LineImageSum = "";
       this.masterPM.ShipmentConsoleShipments.forEach(item => { if (item.GrossWeight != null) HouseValueSum += item.GrossWeight; });
       MasterValueSum = this.masterPM.GrossWeight;
       if (HouseValueSum == MasterValueSum) { IsEqualsSum = true; LineImageSum = "/Images/SimplogIcons/GreenTick.png" }
       else LineImageSum = "/Images/SimplogIcons/Warning.png";
       var line2: LineData = new LineData();
       line2.LineLabel = "Gross Weight";
       line2.HouseValue = HouseValueSum;
       line2.MasterValue = MasterValueSum;
       line2.IsEquals = IsEqualsSum;
       line2.LineImage = LineImageSum;

       ////////////////////////////////////////////////////
       HouseValueSum = 0;
       MasterValueSum = 0;
       IsEqualsSum = false;
       LineImageSum = "";
       this.masterPM.ShipmentConsoleShipments.forEach(item => { if (item.VolumetricWeight != null) HouseValueSum += item.VolumetricWeight; });
       MasterValueSum = this.masterPM.VolumetricWeight;
       if (HouseValueSum == MasterValueSum) { IsEqualsSum = true; LineImageSum = "/Images/SimplogIcons/GreenTick.png" }
       else LineImageSum = "/Images/SimplogIcons/Warning.png";

       var line3: LineData = new LineData();
       line3.LineLabel = "Vol Weight";
       line3.HouseValue = HouseValueSum;
       line3.MasterValue = MasterValueSum;
       line3.IsEquals = IsEqualsSum;
       line3.LineImage = LineImageSum;
       this.GRO_ObsList.push(line1);
       this.GRO_ObsList.push(line2);
       this.GRO_ObsList.push(line3);       
    }
    BuildData() {

        if (this.masterPM.ShipmentConsoleShipments.length > 0) {
            if (ShipmentTool.IsLCL(this.masterPM)){
                this.BuildLCLData();
            }
            else {
                if (!this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                    this.BuildFCLData();

                }
                else {
                    this.BuildGroupageData();                    
                }

            }

        }

    }
    public get ShipmentNo() { return this.CurrentShipment.ShipmentNumber; }

    public get FlightVoyage() { return this.CurrentShipment.MainCarriageCarrierNumber; }

    public get MasterOrHouseNo() {
        var  masterOrHouse:string = null;
        if (this.CurrentShipment.ShipmentLevelCode == "H") {
            masterOrHouse = this.CurrentShipment.House;
        }
        else {
            masterOrHouse = this.CurrentShipment.Master;
        }
        return masterOrHouse;

    }

    public get ATDATA() {
        var atAandAtd: string = null;
        atAandAtd = (this.CurrentShipment.MainCarriageATD != null ? this.DatePipe.transform(this.CurrentShipment.MainCarriageATD) : "") + " / " + (this.CurrentShipment.MainCarriageATA != null ? this.DatePipe.transform(this.CurrentShipment.MainCarriageATA, "SD") : "");
        if (atAandAtd == " / ") {
            atAandAtd = "";
        }
        return atAandAtd;


    }

    public get PPCC() { return this.CurrentShipment.FreightPrepaidCollectId; }
    //ErrorsList
    private warningsList: Array<string>;
    public get WarningsList() {
        if (this.warningsList == null) {
            this.warningsList = new Array<string>();
        }
        return this.warningsList;
        
    }
    public set WarningsList(value: Array<string>) {
        this.warningsList = value;
    }

    private errorsList: Array<string>;
    public get ErrorsList() {
        if (this.errorsList == null) {
            this.errorsList = new Array<string>();
        }
        return this.errorsList;

    }
    public set ErrorsList(value: Array<string>) {
        this.errorsList = value;
    }
   
    private connectedShipments: Array<MasterActionConfirmationViewModel>;
    public get ConnectedShipments() {

        if (this.connectedShipments == null) {
            this.connectedShipments = new Array<MasterActionConfirmationViewModel>();

        }
        return this.connectedShipments;

    }
    public set ConnectedShipments(value: Array<MasterActionConfirmationViewModel>) { this.connectedShipments = value; };

    public  HasErrors() {

        var hasErrors: boolean = false;

        if (!ShipmentTool.IsLCL(this.masterPM)) {
            if (!this.masterPM.ShipmentTypeId.toUpperCase().includes("MYG")) {
                if (this.FCL_ObsList1.filter(d => d.IsEquals == false)[0] || this.FCL_ObsList2.filter(d => d.IsEquals == false)[0]) {
                    hasErrors = true;
                }
            }
        }

        if (this.ErrorsList.length != 0) {
            hasErrors = true;
        }

        else {
            this.ConnectedShipments.forEach(model => {
                if (!model.IsMaster) {
                    if (model.HasErrors()) {
                        hasErrors = true;
                        return;
                    }
                }
            });
        }
        return hasErrors;


    }

    public HasWarnings() {

        var succeeded: boolean = true;
        if (this.WarningsList.length != 0) {
            succeeded = false;

        }
        else {

            this.ConnectedShipments.forEach(model => {
                {
                    if (model.HasWarnings()) {
                        succeeded = false;
                        return;
                    }
                }
            });
            return succeeded;


        }
    }

    ValidateShipment(entityPM: ShipmentPM) {
        var validationResults = [];        
        var masterObject = window.ObjectTables.filter(d => d.Name === 'Master')[0];
        var shipmentObject = window.ObjectTables.filter(d => d.Name === 'Shipment')[0];
        var validator = new ShipmentValidator();
        var requiredFields: Array<any> = [];
        validationResults = validator.Validate(entityPM);
        validationResults.forEach(error => { this.ErrorsList.push(error); });
        var succeeded: boolean = true;
        var  ruleValidator = new RulesValidator();
        var warningValidator: EntityWarningsValidator = new EntityWarningsValidator();
        
        if (entityPM.ShipmentLevelCode == "H") {
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_II", entityPM, requiredFields);

            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_IE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_II", entityPM, this.WarningsList);
        }

        if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C") {

            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II", entityPM, requiredFields);

            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);

            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II", entityPM, this.WarningsList);

            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE_D", entityPM, this.WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II_D", entityPM, this.WarningsList);
        }





        if (entityPM.ShipmentLevelCode == "C") {
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_War", entityPM, this.WarningsList);
        }

        var _tenantObjectFields = window.ObjectFields;

        if (requiredFields.length != 0) {
             succeeded = false;
            for (var k in requiredFields) {
                var field = requiredFields[k];
                var obField = _tenantObjectFields.filter(x => x.Id === field.ObjectFieldId)[0];//ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
                var requiredError = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                var fieldTrans = TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
                requiredError = requiredError.replace("%FieldName", fieldTrans);
                if (!this.ErrorsList.includes(requiredError))
                this.ErrorsList.push(requiredError);
            }
            


        }

        if (this.ErrorsList.length != 0) {
            succeeded = false;
        }       
        else {
            succeeded = true;
        }
       // this.WarningsList = validationResults;

        return succeeded;
        
    }

}



export class LineData {

    private lineLabel: string;
    public get LineLabel() { return this.lineLabel; }
    public set LineLabel(value: string) { this.lineLabel = value; }

    private houseValue: number;
    public get HouseValue() { return this.houseValue; }
    public set HouseValue(value: number) { this.houseValue = value; }


    private masterValue: number;
    public get MasterValue() { return this.masterValue; }
    public set MasterValue(value: number) { this.masterValue = value; }

    private lineImage: string;
    public get LineImage() { return this.lineImage; }
    public set LineImage(value: string) { this.lineImage = value; }


    private isEquals: boolean;
    public get IsEquals() { return this.isEquals; }
    public set IsEquals(value: boolean) { this.isEquals = value; }


    private shipmentNumber: string;
    public get ShipmentNumber() { return this.shipmentNumber; }
    public set ShipmentNumber(value: string) { this.shipmentNumber = value; }



    private houseStringValue: string;
    public get HouseStringValue() { return this.houseStringValue; }
    public set HouseStringValue(value: string) { this.houseStringValue = value; }


    private masterStringValue: string;
    public get MasterStringValue() { return this.masterStringValue; }
    public set MasterStringValue(value: string) { this.masterStringValue = value; }
    

}


export class ValidationErrorInfo {
    private messageType: string;
    private errorCode: number;
    private errorMessage: string;
    public get MessageType() { return this.messageType; }
    public set MessageType(value: string) { this.messageType = value; }
    public set ErrorCode(value: number) { this.errorCode = value; }
    public get ErrorCode() { return this.errorCode; }
    public get ErrorMessage() { return this.errorMessage; }
    public set ErrorMessage(value: string) { this.errorMessage = value; }
    public ToString() {
        return this.ErrorMessage;
    }
}
