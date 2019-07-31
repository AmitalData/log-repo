declare var System: any;
declare var window: any;

import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {WarehouseReleasePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService';

import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';

import {AppTool, DateTool, FormatTool, DateParts} from '../../Infrastructure/Tools';

import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {TraceEventExtendedPMService } from '../../Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService';
import {EventTypeClass} from '../../Infrastructure/DataContracts/EventTypeArgs';

import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';

import {DateAgeHelper} from '../../Infrastructure/Utilities/DateAgeHelper';

import {WarehouseHelper} from '../Helpers/WarehouseHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
@Component({
    moduleId: module.id,
    selector: 'NewWarehouseReleaseComponent',
    templateUrl: './NewWarehouseReleaseComponent.html',
    providers: [WarehouseReleasePMExtendedService, TraceEventExtendedPMService, WarehouseEntryPackagePMExtendedService],

})
export class NewWarehouseReleaseComponent extends BaseComponent implements OnInit {


    WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
    public ValidationErrorsList: string[];
    ShipmentPM: any;
    warehouseReleasePM: WarehouseReleasePM = new WarehouseReleasePM();
    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public DimensionsColumnHeader: string;
    public VolumetricWeightColumnHeader: string;
    public PackageTypeColumnHeader: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    validator: ClassLevelValidator;
    EventTypeCodeList: EventTypeClass[];
    ObjectTableId: string;
   
    ActualReleaseDateOldValue: Date;
    ExpectedReleaseDateOldValue: Date;
    SelectedWarehouseReleasePackage: WarehouseReleasePackagePM;
    AllWarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];
    CustomWarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];
    IsChangeWarehouseIdOrCustomerId: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    ChargeableWeightLabel: string;

    DataContext: any = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService, public _traceEventExtendedPMService: TraceEventExtendedPMService, private warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        super();
        this.EventTypeCodeList = [];
        this.GetNewInstance();
        this.validator = new ClassLevelValidator();
   
        var table = window.ObjectTables.filter(d=> d.Name == "WarehouseRelease")[0];
        if (table) {
            this.ObjectTableId = table.Id;
        }
  
    }

    ngOnInit() {


    }





    GetNewInstance() {

        this.warehouseReleasePM.Tenant = SessionLocator.TenantPM.Id;
        this.warehouseReleasePM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.warehouseReleasePM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.warehouseReleasePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.warehouseReleasePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.warehouseReleasePM.StatusCode = "CREA";
        this.warehouseReleasePM.Id = "1-1";
        this.warehouseReleasePM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
        this.warehouseReleasePM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
        this.warehouseReleasePM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;

        this.warehouseReleasePM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;
        this.warehouseReleasePM.TotalVolume = 0;
        this.warehouseReleasePM.TotalGrossWeight = 0;
        this.warehouseReleasePM.TotalPieces = 0;
        this.warehouseReleasePM.ReleaseNumber = "123";
    }



    IsLoadPage: boolean = false;
    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease").subscribe(response => {
            this.Start(args);
    
        });

    }
    IsLCLEntity: boolean = true;

    Start(args: any) {
    
        this.ShipmentPM = args.ShipmentPM;

       
        this.SetLabel();
        this.SetValue(args);

        this.IsLoadPage = true;



    }
    IsLoadWarehouse: boolean = false;
    SetValue(args: any) {
        if (this.warehouseReleasePM) {

            if (this.ShipmentPM) {
                if (this.ShipmentPM.ShipmentLevelCode == "D") this.warehouseReleasePM.CustomerId = this.ShipmentPM.CustomerId;
                    this.warehouseReleasePM.ShipmentId = this.ShipmentPM.Id;
                    this.warehouseReleasePM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
                    this.warehouseReleasePM.HouseNumber = this.ShipmentPM.House;
                    this.warehouseReleasePM.MasterNumber = this.ShipmentPM.LongMaster;
                    this.warehouseReleasePM.ShipmentLevelCode = this.ShipmentPM.ShipmentLevelCode;
                    this.warehouseReleasePM.TransportModeId = this.ShipmentPM.TransportModeId;
                    this.warehouseReleasePM.ShipmentTypeId = this.ShipmentPM.ShipmentTypeId;
                    this.warehouseReleasePM.DirectionId = this.ShipmentPM.DirectionId;

            }


        this.TransportModeId = this.warehouseReleasePM.TransportModeId;
        this.DirectionId = this.warehouseReleasePM.DirectionId;
    
        this.FromPortId = this.ShipmentPM ? this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId :"";

        this.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;

        this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);

            var myCommonDomain = new CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(warehouseId) && AppTool.IsNullOrEmpty(args.WarehouseId) ) {
                        this.warehouseReleasePM.WarehouseId = warehouseId;
                    }
                    else {
                        this.warehouseReleasePM.WarehouseId = args.WarehouseId;
                    }
                } else {
                    this.warehouseReleasePM.WarehouseId = args.WarehouseId;
   
                }

           

            });


            this.warehouseReleasePM.ExpectedReleaseDate = args.ExpectedReleaseDate;
            this.warehouseReleasePM.ActualReleaseDate = args.ActualReleaseDate;
            this.ActualReleaseDateOldValue = this.warehouseReleasePM.ActualReleaseDate;
            this.ExpectedReleaseDateOldValue = this.warehouseReleasePM.ExpectedReleaseDate;

        }
    }

    SetLabel() {

        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.ChargeableWeightLabel = "ChargeableWeight (" + SessionLocator.TenantPM.ChargeableWeightUnitCode + ")";
        
        
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.ShipmentPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.ShipmentPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.ShipmentPM.ChargeableWeightUnitCode);
        this.PackageTypeColumnHeader = TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
    }


    LoadAllWarehouseEntryPackagesLists() {

        if (this.ShipmentPM) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(this.ShipmentPM.Id, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.ShipmentPM.Tenant).subscribe((res: any) => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    this.AllWarehouseEntryPackagesLists = pmResponse.Result;
 
                }

            });
        }
    } 
     
    CustomerValueChange(item) {
        if (item) {
            this.RefreshWarehouseEntryPackagesLists("CustomerId", item);
        }
    }
    WarehouseValueChange(item) {
        
        if (item) {
            this.RefreshWarehouseEntryPackagesLists("WarehouseId", item);
        }
    }



    RefreshWarehouseEntryPackagesLists(fieldName:string, item:any)
    {
        if (this.CustomWarehouseEntryPackagesLists && this.CustomWarehouseEntryPackagesLists.length > 0) {
            var lists = [];
            if (fieldName == "WarehouseId") {
                lists = this.CustomWarehouseEntryPackagesLists.filter(d=> d.WarehouseId == item.Id && d.CustomerId == this.warehouseReleasePM.CustomerId);
            }
            else {
                 lists = this.CustomWarehouseEntryPackagesLists.filter(d=> d.CustomerId == item.Id && d.WarehouseId == this.warehouseReleasePM.WarehouseId);
            }

            if (!lists || (lists && lists.length == 0)) {
                this.IsChangeWarehouseIdOrCustomerId = true;
                this.WarehouseReleasePackagesLists = [];
            }
        }
        else {
            this.IsChangeWarehouseIdOrCustomerId = true;
            this.WarehouseReleasePackagesLists = [];
        }

    }


 


    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {

        this.ValidationErrorsList = [];


        var errorsArray = this.validator.Validate("WarehouseRelease", this.warehouseReleasePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }

        if (this.WarehouseReleasePackagesLists.length == 0) {

            this.ValidationErrorsList.push("You should at least choose one package");
        }
        else {
            if (this.warehouseReleasePM.ActualReleaseDate != null) {
                var releasePackagesLists = this.WarehouseReleasePackagesLists.filter(d => d.ActualReleaseDate != null);
                var isValidReleasePackages: boolean = true;
                if (releasePackagesLists.length > 0) {
                    releasePackagesLists.forEach((item) => {
                        if (DateTool.IsDateBigger(item.ActualReleaseDate, this.warehouseReleasePM.ActualReleaseDate )) {
                            isValidReleasePackages = false;
                            return;
                        }
                    });

                     if (!isValidReleasePackages) {
                         this.ValidationErrorsList.push("Actual Release Date must be greater or equal to Actual Entry Date.");
                    }

                }
            }
        }


        var errors: string[] = [];


        
        // Actual Dates
        if (!DateTool.IsActualDateValid(this.warehouseReleasePM.ActualReleaseDate)) {
            this.ValidationErrorsList.push(DateTool.ActualDateMessage.replace("Field", "Actual Release Date"));
        }

        if (this.ValidationErrorsList.length == 0) {
            var message = "Can't set Field to future date";
            var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();

            if (this.warehouseReleasePM.ActualReleaseDate) {
                if (this.warehouseReleasePM.ActualReleaseDate.valueOf() > todayDateTime.valueOf()) {
                    this.ValidationErrorsList.push(message.replace("Field", "Actual Release Date"));
                }
            }
        }


        if (this.ValidationErrorsList.length == 0) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

      
                if (this.WarehouseReleasePackagesLists.length > 0) {
                    this.WarehouseReleasePackagesLists.forEach((item) => {
                        this.warehouseReleasePM.AddWarehouseReleasePackage(item);
                    });

                }
                
                this.ComputeAndFullTotalPackage();
                this.EventTypeCodeList.push(new EventTypeClass("CRRE",null));
                if (this.warehouseReleasePM.ExpectedReleaseDate) this.EventTypeCodeList.push(new EventTypeClass("EXRE", this.warehouseReleasePM.ExpectedReleaseDate));
                if (this.warehouseReleasePM.ActualReleaseDate) this.EventTypeCodeList.push(new EventTypeClass("ENRE", this.warehouseReleasePM.ActualReleaseDate));


                if (this.EventTypeCodeList.filter(d => d.Code == "ENRE")[0]) this.warehouseReleasePM.StatusCode = "RELE";


                this._warehouseReleasePMExtendedService.Insert(this.warehouseReleasePM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                  
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();

                    if (!pmResponse.HasError) {
                        ServiceLocator.SendTotangoUserActivity("Cross Docs", "Create Release");
                        this.warehouseReleasePM = pmResponse.Result;
                        this.CurrentSession.FireEvent("CrossDockReleases");
                        var myResult = pmResponse.Result;
                        if (myResult) {

                            var warehouseHelper: WarehouseHelper = new WarehouseHelper();
                            warehouseHelper.SetShipmentWarehouseLeg(this.ShipmentPM, this.warehouseReleasePM, "Release");

                            this.UpdateEventType();
                        }
                    } else {
                        pmResponse.ErrorsArray.forEach((item) => {
                            this.ValidationErrorsList.push(item);
                        });


                    }

                });
       

        }


    }


  

    ComputeAndFullTotalPackage() {
        var totalPieces: number = 0;
        var totalVolume: number = 0;
        var totalGrossWeight: number = 0;

        if (this.WarehouseReleasePackagesLists && this.WarehouseReleasePackagesLists.length > 0) {
            this.WarehouseReleasePackagesLists.forEach((item) => {
          if (item.Quantity) totalPieces += item.Quantity;
          if (item.Volume) totalVolume += item.Volume;
          if (item.Weight) totalGrossWeight += item.Weight;

            });
        }

        this.warehouseReleasePM.TotalPieces = totalPieces;
        this.warehouseReleasePM.TotalVolume = totalVolume;
        this.warehouseReleasePM.TotalGrossWeight = totalGrossWeight;
        this.warehouseReleasePM.GrossWeightUnitCode = "KG";
        this.warehouseReleasePM.VolumeUnitCode = "CBM";
    }



    IsChoosePackageOpen: boolean = false;

    IsPackageOpen: boolean = false;
    WarehouseId: string = "";

    ChoosePackage(packageType:string) {
        this.IsChoosePackageOpen = true;

        if (!this.IsPackageOpen) {
            this.IsPackageOpen = true;
            this.IsChoosePackageOpen = true;
            if (this.CustomerId != this.warehouseReleasePM.CustomerId || this.WarehouseId != this.warehouseReleasePM.WarehouseId) {

                var shipmentId: string = this.ShipmentPM ? this.ShipmentPM.Id : "";
                this.AllWarehouseEntryPackagesLists = [];
                this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(shipmentId, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.ShipmentPM.Tenant).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        this.AllWarehouseEntryPackagesLists = pmResponse.Result;
                        this.OpenChoosePackage(packageType);
                    }

                });

            } else {
                this.OpenChoosePackage(packageType);
            }
        }
   
    }


    TransportModeId: string;
    DirectionId: string;
    CustomerId: string;
    FromPortId: string;
    ToPortId: string;
    OpenChoosePackage(packageType: string) {

        this.WarehouseId = this.warehouseReleasePM.WarehouseId;
        this.CustomerId = this.warehouseReleasePM.CustomerId;

        this.IsPackageOpen = false;

        if (this.IsChangeWarehouseIdOrCustomerId) {
            this.AllWarehouseEntryPackagesLists.forEach((item) => {
                item.ReleaseQTY = 0;
            });
            this.IsChangeWarehouseIdOrCustomerId = false;
        }
        var windowArgs: any = {};
        windowArgs.WarehouseReleasePM = this.warehouseReleasePM;

        windowArgs.WarehouseEntryPackagesLists = this.AllWarehouseEntryPackagesLists;
        windowArgs.ViewModelTrigger = this;

    

        windowArgs.PackageType = packageType;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1150;
        logWindow.Height = 550;
        logWindow.Title = "Choose Packages";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/WarehouseReleaseChoosePackagesComponent");


        logWindow.WindowClosed.subscribe(($event: any) => {
            if (this.WarehouseReleasePackagesLists.length > 0 && this.IsChoosePackageOpen) {
                this.IsChoosePackageOpen = false;
                this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
                this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
            }

        });


    }

    OnActualReleaseDateDatePickerChange(value) {

        this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", true, null);

        if (!DateTool.IsActualDateValid(value)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Release Date");
            this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", false, errorMessage);
        }
    }

    get ActualReleaseDate() {
        var actualReleaseDate: Date = null;
        if (this.warehouseReleasePM) actualReleaseDate = this.warehouseReleasePM.ActualReleaseDate;
        return actualReleaseDate;
    }
    set ActualReleaseDate(value: Date) {
        if (this.warehouseReleasePM != null) {
            if (value != this.warehouseReleasePM.ActualReleaseDate) {
                this.warehouseReleasePM.ActualReleaseDate = value;
                this.OnActualReleaseDateDatePickerChange(value);
            }
        }
    }

    SetActualDateClicked(fieldName: string) {
        this.ActualReleaseDate = DateTool.GetDateParts(this.warehouseReleasePM.ExpectedReleaseDate).DateObject;
    }

    EditPackage(warehouseReleasePackagePM: WarehouseReleasePackagePM) {

    

    }


    DeletePackage(item: WarehouseReleasePackagePM) {



        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this package");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var index = this.WarehouseReleasePackagesLists.indexOf(item);
                if (index != -1) this.WarehouseReleasePackagesLists.splice(index, 1);

                var entry = this.AllWarehouseEntryPackagesLists.filter(d=> d.Id == item.EntryPackageId)[0];
                if (entry) {
                    entry.ReleaseQTY = 0;
                    entry.IsSelected = false;
                }

                if (this.WarehouseReleasePackagesLists.length == 0) {
                    this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", true);
                    this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", true);
                }
            }
        });




    }

    UpdateEventType() {

        if (this.EventTypeCodeList && this.EventTypeCodeList.length != 0) {

            var traceEventArgs: EventTypeArgs = new EventTypeArgs();

            traceEventArgs.EventTypeList = this.EventTypeCodeList;
            traceEventArgs.Tenant = SessionLocator.Tenant;
            traceEventArgs.ObjectTableId = this.ObjectTableId;
            traceEventArgs.EntityId = this.warehouseReleasePM.Id;
            traceEventArgs.LoggedContactId = SessionLocator.LoggedUserId;

            this._traceEventExtendedPMService.PutTraceEventGroup(traceEventArgs).subscribe(res => {

                this.CurrentSession.CurrentWindow.Close("Refresh");

            });

        }
        else {
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }

    }


}
