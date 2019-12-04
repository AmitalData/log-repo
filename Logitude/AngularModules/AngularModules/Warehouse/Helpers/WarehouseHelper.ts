/// <reference path="../../infrastructure/locators/servicelocator.ts" />
/// <reference path="../../infrastructure/utilities/infragenericfilter.ts" />
/// <reference path="../../shipment/entitypms/shipmentpm.ts" />

declare var System: any;
declare var window: any;
import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {CardListService} from '../../Common/Services/StandardLists/CardListService';
import {AppTool, DateTool, FormatTool} from '../../Infrastructure/Tools';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ShipmentPickUpPM} from '../../Shipment/EntityPMs/ShipmentPickUpPM';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import { WarehouseEntryPM } from '../../Warehouse/EntityPMs/WarehouseEntryPM';
import { WarehouseReleasePM } from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import { WarehouseEntryPMService } from '../../Warehouse/Services/StandardPMs/WarehouseEntryPMService';
import { WarehouseReleasePMExtendedService } from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService';
import {EventTypeClass, EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
export class WarehouseHelper {
    validator: ClassLevelValidator;
    public _warehouseEntryPMService: WarehouseEntryPMService;
    public _warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
      
        
    }

    SetShipmentWarehouseLeg(shipmentPM: ShipmentPM, warehouseEntity: any, type: string) {

        if (shipmentPM && warehouseEntity && !AppTool.IsNullOrEmpty(type)) {
            var isShipmentDirty: boolean = shipmentPM.IsDirty;

            if (AppTool.IsNullOrEmpty(shipmentPM.WarehouseLegWarehouseId)) {
                if (shipmentPM.DirectionId != "C" && (shipmentPM.ShipmentLevelCode == "D" || shipmentPM.ShipmentLevelCode == "H")) {
                    var myService = new CardListService();
                    myService.getSingle(warehouseEntity.WarehouseId).subscribe((myResponse: ServiceResponse) => {
                        if (myResponse != null) {


                            shipmentPM.WarehouseLegWarehouseId = warehouseEntity.WarehouseId;

                            if (type == "Release") {
                                shipmentPM.WarehouseLegActualReleaseDate = warehouseEntity.ActualReleaseDate;
                                shipmentPM.WarehouseLegExpectedReleaseDate = warehouseEntity.ExpectedReleaseDate;
                            } else {
                                shipmentPM.WarehouseLegActualEntryDate = warehouseEntity.ActualEntryDate;
                                shipmentPM.WarehouseLegExpectedEntryDate = warehouseEntity.ExpectedEntryDate;

                            }

                            if (!myResponse.HasError) {
                                var result = myResponse.Result;
                                if (result) {
                                    var isFirmCodeVisible: boolean = false;
                                    if (!AppTool.IsNullOrEmpty(SessionLocator.TenantPM.CountryCode)) {
                                        isFirmCodeVisible = (SessionLocator.TenantPM.CountryCode.toUpperCase()) == "US" ? true : false;
                                    }

                                    shipmentPM.WarehouseLegAddressId = result.MainAddressId;
                                    shipmentPM.WarehouseLegTerminalName = result.EnglishName;
                                    shipmentPM.WarehouseLegTerminalCode = result.FirmCode;
                                    if (isFirmCodeVisible) {
                                        shipmentPM.WarehouseLegTerminalCode = result.FirmCode;
                                    }


                                    if (this.CurrentSession.CurrentEditComponent) {
                                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                                    }

                                    if (!isShipmentDirty) shipmentPM.IsDirty = false;

                              
                                    this.CurrentSession.FireEvent("RefreshWareHouseLeg");

                                }
                            }
                        }

                        


                    });
                }
            }


           
        }
    }
    

    ShowNewWarehouseEntryComponent(windowArgs: any) {

    var shipmentPackages: any[] = [];
    var entityPM: ShipmentPM = windowArgs.EntityPM;
    var entityChildPM: ShipmentPickUpPM = windowArgs.EntityChildPM;
    
    if (windowArgs.PageRequest == "ShipmentPickUp") {
        shipmentPackages = entityChildPM ? entityChildPM.ShipmentPickUpDeliveryPackages : [];
    }
    else shipmentPackages = entityPM ? entityPM.ShipmentPackages : [];
     
    if (shipmentPackages && shipmentPackages.length > 0) {
        windowArgs.WarehouseEntryPackagesLists = this.FullWarehouseEntryPackagePM(shipmentPackages, entityPM);
    }

        windowArgs.ShipmentPM = entityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = "New Cross Dock Entry";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/NewWarehouseEntryComponent");
    }


    FullWarehouseEntryPackagePM(shipmentPackages: any, entityPM: ShipmentPM, packageType: string = null ) {
        var warehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];

        if (shipmentPackages && shipmentPackages.length > 0) {
            
            shipmentPackages.forEach((item) => {
                var warehouseEntryPackagePM: WarehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
                warehouseEntryPackagePM.Width = item.Width;
                warehouseEntryPackagePM.Height = item.Height;
                warehouseEntryPackagePM.Length = item.Length;
                warehouseEntryPackagePM.Description = item.Description;
                warehouseEntryPackagePM.Quantity = item.Quantity;
                warehouseEntryPackagePM.Volume = item.Volume;
                warehouseEntryPackagePM.Weight = item.Weight;
                warehouseEntryPackagePM.ContainerNumber = item.ContainerNumber;
                warehouseEntryPackagePM.Seal = item.Seal;
                warehouseEntryPackagePM.Tenant = item.Tenant;
                warehouseEntryPackagePM.Harmonize = item.Harmonize;
                warehouseEntryPackagePM.Instock = item.Quantity;
                warehouseEntryPackagePM.PackageTypeId = item.PackageTypeId;
                warehouseEntryPackagePM.PackageTypeName = item.PackageTypeName;
                warehouseEntryPackagePM.Tenant = SessionLocator.TenantPM.Id;
                warehouseEntryPackagePM.CreatedByUserId = SessionLocator.LoggedUserId;
                warehouseEntryPackagePM.UpdatedByUserId = SessionLocator.LoggedUserId;
                warehouseEntryPackagePM.CreateDate = DateTool.GetCurrentDateAsUtc();
                warehouseEntryPackagePM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                warehouseEntryPackagePM.VolumetricWeight = item.VolumetricWeight;

                if (packageType == "ShipmentPackages") warehouseEntryPackagePM.IsContainer = item.IsContainer;
                else {

                    if (!AppTool.IsNullOrEmpty(item.PackageTypeId)) {
                        var myService: PackageTypeListService = new PackageTypeListService();
                        myService.getSingleFromCache(item.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var list: any = myResponse.Result;
                                if (list != null) {
                                    warehouseEntryPackagePM.IsContainer = list.IsContainer;
                                }
                            }
                        });
                    } else {
                        if (!AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                            warehouseEntryPackagePM.IsContainer = true;
                        }

                    }

                }
                
                warehouseEntryPackagePM.Id = "1-1";
                warehouseEntryPackagePM.WarehouseEntryId = "1-1";
                var height: string = warehouseEntryPackagePM.Height ? warehouseEntryPackagePM.Height.toString() : "";
                var width: string = warehouseEntryPackagePM.Width ? warehouseEntryPackagePM.Width.toString() : "";
                var length: string = warehouseEntryPackagePM.Length ? warehouseEntryPackagePM.Length.toString() : "";
                warehouseEntryPackagePM.Dimensions = length + "-" + width + "-" + length;

                if (!AppTool.IsNullOrEmpty(warehouseEntryPackagePM.ContainerNumber) && warehouseEntryPackagePM.IsContainer) {
                    var error = FormatTool.ValidateContainerNumber(warehouseEntryPackagePM.ContainerNumber);
                    warehouseEntryPackagePM.ContainerNumberWarning = error;

                }

                warehouseEntryPackagesLists.push(warehouseEntryPackagePM);


            });

        }
        return warehouseEntryPackagesLists;
    }


    CreateWarehouseEntry(entityPM: WarehouseEntryPM, viewModel: any) {

        if (entityPM != null && viewModel!=null) {
            viewModel.ValidationErrorsList = [];

            if (this.validator == null) {
                this.validator = new ClassLevelValidator();
            }

            var errorsArray = this.validator.Validate("WarehouseEntry", entityPM);
            if (errorsArray.length > 0) {
                errorsArray.forEach((item) => {
                    viewModel.ValidationErrorsList.push(item);
                });
            }


            entityPM.WarehouseEntryPackages = entityPM.WarehouseEntryPackages.filter(d => d.Quantity > 0);

            if (entityPM.WarehouseEntryPackages.length == 0) {
                viewModel.ValidationErrorsList.push("You should at least add one package");
            }
            


            // Actual Dates
            if (!DateTool.IsActualDateValid(entityPM.ActualEntryDate)) {
                viewModel.ValidationErrorsList.push(DateTool.ActualDateMessage.replace("Field", "Actual Entry Date"));
            }


            if (viewModel.ValidationErrorsList.length == 0) {


                if (entityPM.WarehouseEntryPackages.length > 0 && entityPM.ConnectedToShipment) {
                    entityPM.WarehouseEntryPackages.forEach((item) => {
                        item.IsConnectedToShipment = true;
                    });
                }


                this.CurrentSession.StartBusyIndicatorSaving();

                if (this._warehouseEntryPMService == null) this._warehouseEntryPMService = new WarehouseEntryPMService();
                if (entityPM.ActualEntryDate) entityPM.StatusCode = "ENTE";

                    this._warehouseEntryPMService.insert(entityPM).subscribe(res => {
                        var pmResponse: ServiceResponse = res;

                        if (!pmResponse.HasError) {
                            ServiceLocator.SendTotangoUserActivity("Cross Docs", "Create Entry");

                            entityPM = pmResponse.Result;
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                if (viewModel.IsFromShipment && !viewModel.IsNotSetWarehouseIdForWarehouseLegShipment) {
                                    this.SetShipmentWarehouseLeg(viewModel.ShipmentPM, entityPM, "Entry");
                                }
                                this.CurrentSession.StopBusyIndicator();
                                this.CurrentSession.CurrentWindow.Close("Refresh");
                            }
                            else this.CurrentSession.StopBusyIndicator();
                        } else {
                            pmResponse.ErrorsArray.forEach((item) => {
                                viewModel.ValidationErrorsList.push(item);
                            });

                            this.CurrentSession.StopBusyIndicator();
                        }


                    });
                
            }
        }
   
    }





    CreateWarehouseRelease(entityPM: WarehouseReleasePM, viewModel: any) {

        if (entityPM != null && viewModel != null) {
            viewModel.ValidationErrorsList = [];
 
            entityPM.FromPortId = entityPM.WarehouseId;
            entityPM.ToPortId = null;
            entityPM.ToTypeCode = null;

            if (this.validator == null) {
                this.validator = new ClassLevelValidator();
            }

            var errorsArray = this.validator.Validate("WarehouseRelease", entityPM);
            if (errorsArray.length > 0) {
                errorsArray.forEach((item) => {
                    viewModel.ValidationErrorsList.push(item);
                });
            }

            entityPM.WarehouseReleasePackages = entityPM.WarehouseReleasePackages.filter(d => d.Quantity > 0);



            if (entityPM.WarehouseReleasePackages.length == 0) {

                viewModel.ValidationErrorsList.push("You should at least choose one package");
            }
            else {
                if (entityPM.ActualReleaseDate != null) {
                    var releasePackagesLists = viewModel.WarehouseReleasePackagesLists.filter(d => d.ActualReleaseDate != null);
                    var isValidReleasePackages: boolean = true;
                    if (releasePackagesLists.length > 0) {
                        releasePackagesLists.forEach((item) => {
                            if (DateTool.IsDateBigger(item.ActualReleaseDate, entityPM.ActualReleaseDate)) {
                                isValidReleasePackages = false;
                                return;
                            }
                        });

                        if (!isValidReleasePackages) {
                            viewModel.ValidationErrorsList.push("Actual Release Date must be greater or equal to Actual Entry Date.");
                        }

                    }
                }
            }



            // Actual Dates
            if (!DateTool.IsActualDateValid(entityPM.ActualReleaseDate)) {
                viewModel.ValidationErrorsList.push(DateTool.ActualDateMessage.replace("Field", "Actual Release Date"));
            }

            if (viewModel.ValidationErrorsList.length == 0) {
                var message = "Can't set Field to future date";
                var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();

                if (entityPM.ActualReleaseDate) {
                    if (entityPM.ActualReleaseDate.valueOf() > todayDateTime.valueOf()) {
                        viewModel.ValidationErrorsList.push(message.replace("Field", "Actual Release Date"));
                    }
                }
            }



            if (viewModel.ValidationErrorsList.length == 0) {
                
                this.CurrentSession.StartBusyIndicatorSaving();

                if (this._warehouseReleasePMExtendedService == null) this._warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
                if (entityPM.ActualReleaseDate) entityPM.StatusCode = "RELE";

                this._warehouseReleasePMExtendedService.Insert(entityPM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;

                    this.CurrentSession.CurrentWindow.StopBusyIndicator();

                    if (!pmResponse.HasError) {
                        ServiceLocator.SendTotangoUserActivity("Cross Docs", "Create Release");
                        entityPM = pmResponse.Result;
                        this.CurrentSession.FireEvent("CrossDockReleases");
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            if (viewModel.IsFromShipment) {
                              
                                this.SetShipmentWarehouseLeg(viewModel.ShipmentPM, entityPM, "Release");
                            }
                            this.CurrentSession.CurrentWindow.Close("Refresh");
                        }
                    } else {
                        pmResponse.ErrorsArray.forEach((item) => {
                            viewModel.ValidationErrorsList.push(item);
                        });


                    }

                    this.CurrentSession.StopBusyIndicator();

                });



            }
        }

    }

















    SetLabel(viewModel: any) {
        if (viewModel != null) {
            viewModel.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
            viewModel.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
            viewModel.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator.TenantPM.DimensionsUnitCode + ")";

        }
    }


    GetNewWarehouseEntry(viewModel:any) {
        var warehouseEntryPM: WarehouseEntryPM = new WarehouseEntryPM() ;
        if (viewModel != null) {

            warehouseEntryPM.ReceivedBy = SessionLocator.LoggedUserPM.EnglishName;
            warehouseEntryPM.Tenant = SessionLocator.TenantPM.Id;
            warehouseEntryPM.CreatedByUserId = SessionLocator.LoggedUserId;
            warehouseEntryPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            warehouseEntryPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            warehouseEntryPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            warehouseEntryPM.StatusCode = "CREA";
            warehouseEntryPM.Id = "1-1";
            warehouseEntryPM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
            warehouseEntryPM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
            warehouseEntryPM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
            warehouseEntryPM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;

            warehouseEntryPM.EntryNumber = "123";
            //if (viewModel.IsFromShipment) {
                warehouseEntryPM.TotalVolume = 0;
                warehouseEntryPM.TotalGrossWeight = 0;
                warehouseEntryPM.TotalPieces = 0;

           // }
        }


        return warehouseEntryPM;

    }


    GetNewWarehouseRelease(viewModel: any) {
        var warehouseReleasePM: WarehouseReleasePM = new WarehouseReleasePM();
        if (viewModel != null) {

            warehouseReleasePM.ReleaseBy = SessionLocator.LoggedUserPM.EnglishName;
            warehouseReleasePM.Tenant = SessionLocator.TenantPM.Id;
            warehouseReleasePM.CreatedByUserId = SessionLocator.LoggedUserId;
            warehouseReleasePM.UpdatedByUserId = SessionLocator.LoggedUserId;
            warehouseReleasePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            warehouseReleasePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            warehouseReleasePM.StatusCode = "CREA";
            warehouseReleasePM.Id = "1-1";
            warehouseReleasePM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
            warehouseReleasePM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
            warehouseReleasePM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
            warehouseReleasePM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;

            warehouseReleasePM.ReleaseNumber = "123";
            //if (viewModel.IsFromShipment) {
            warehouseReleasePM.TotalVolume = 0;
            warehouseReleasePM.TotalGrossWeight = 0;
            warehouseReleasePM.TotalPieces = 0;

            // }
        }


        return warehouseReleasePM;

    }



}
