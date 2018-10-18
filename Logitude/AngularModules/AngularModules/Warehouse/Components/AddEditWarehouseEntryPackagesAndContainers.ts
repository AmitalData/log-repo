declare var System: any;
declare var window: any;
import {AppTool, FormatTool} from '../../Infrastructure/Tools';

import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {NewWarehouseEntryComponent} from '../../Warehouse/Components/NewWarehouseEntryComponent';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';


import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
@Component({
    moduleId: module.id,
    selector: 'AddEditWarehouseEntryPackagesAndContainers',
    templateUrl: './AddEditWarehouseEntryPackagesAndContainers.html',
    providers: [WarehouseEntryPackagePMExtendedService],
})

export class AddEditWarehouseEntryPackagesAndContainers implements OnInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[];
    public AllPackageTypes: PackageTypeList[] = [];
    warehouseEntryPM: any;
    warehouseEntryPackagePM: WarehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
    validator: ClassLevelValidator;

    public ContainerNumberWarning: string = null;
    IsVolumRefresh: boolean = false;
    IsNewEntity: boolean = false;
    IsDependencyFilter2Value: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    ObjectTableId: string;
    Type: string;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;

    constructor(private _warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        this.validator = new ClassLevelValidator();

        var table = window.ObjectTables.filter(d=> d.Name == "WarehouseEntryPackage")[0];
        if (table) this.ObjectTableId = table.Id;


    }

    ngOnInit(

    ) {


    }

    SetWindowArgs(args: any) { 
    
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntryPackage").subscribe(response => {

            this.Start(args);
        });

    }



    SetUIProperties() {

        if (this.warehouseEntryPackagePM.IsContainer) {

            if (this.IsNewEntity) {
                this.warehouseEntryPackagePM.Quantity = 1;
            }
            this.warehouseEntryPackagePM.UIProperties.SetEnabled("Quantity", "WarehouseEntryPackage", false)
        }
        else {
            if (!this.warehouseEntryPackagePM.Quantity) {
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Length", "WarehouseEntryPackage", false)
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Width", "WarehouseEntryPackage", false)
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Height", "WarehouseEntryPackage", false)
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Volume", "WarehouseEntryPackage", false)
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Weight", "WarehouseEntryPackage", false)
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Harmonize", "WarehouseEntryPackage", false)

            }

        }




    }

    SetLabel() {

        this.VolumeLabel = "Volume (" + this.warehouseEntryPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Weight (" + this.warehouseEntryPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dimensions(L-W-H) (" + this.warehouseEntryPM.DimensionsUnitCode + ")";
      

    }
    IsParentDirty: boolean = false;
    IsChildDirty: boolean = false;

    public savedItem: WarehouseEntryPackagePM;
    Start(args: any) {
        
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.warehouseEntryPackagePM = args.WarehouseEntryPackagePM;
        if (this.warehouseEntryPM && this.warehouseEntryPackagePM) {

            this.ViewModelTrigger = args.ViewModelTrigger;
            this.IsNewEntity = args.IsNewEntity;

            this.AllPackageTypes = args.AllPackageTypes;

            this.IsDependencyFilter2Value = this.warehouseEntryPackagePM.IsContainer;

            if (this.IsNewEntity) {
                this.warehouseEntryPackagePM.Instock = 0;
                this.warehouseEntryPackagePM.IsContainer = this.warehouseEntryPackagePM.IsContainer;
            }
            else {
                if (this.warehouseEntryPackagePM.IsContainer) {
                    if (this.warehouseEntryPackagePM.ContainerNumber) {
                        this.ValidateContainerNumber(this.warehouseEntryPackagePM.ContainerNumber);
                    }
                }

                if (this.warehouseEntryPackagePM) {
                    this.ContainerNumberLostFocus(this.warehouseEntryPackagePM.ContainerNumber);
                    this.IsParentDirty = this.warehouseEntryPM.IsDirty;
                    this.IsChildDirty = this.warehouseEntryPackagePM.IsDirty;

                    this.savedItem = new WarehouseEntryPackagePM(null);
                    this.savedItem.PackageTypeId = this.warehouseEntryPackagePM.PackageTypeId;
                    this.savedItem.ContainerNumber = this.warehouseEntryPackagePM.ContainerNumber;
                    this.savedItem.Length = this.warehouseEntryPackagePM.Length;
                    this.savedItem.Height = this.warehouseEntryPackagePM.Height;
                    this.savedItem.Width = this.warehouseEntryPackagePM.Width;
                    this.savedItem.Volume = this.warehouseEntryPackagePM.Volume;
                    this.savedItem.Weight = this.warehouseEntryPackagePM.Weight;
                    this.savedItem.Description = this.warehouseEntryPackagePM.Description;
                    this.savedItem.Seal = this.warehouseEntryPackagePM.Seal;
                    this.savedItem.Harmonize = this.warehouseEntryPackagePM.Harmonize;
                    this.savedItem.Location = this.warehouseEntryPackagePM.Location;
                    this.savedItem.Dimensions = this.warehouseEntryPackagePM.Dimensions;
                    this.savedItem.Instock = this.warehouseEntryPackagePM.Instock;
                    this.savedItem.Quantity = this.warehouseEntryPackagePM.Quantity;
                    this.savedItem.ContainerNumberWarning = this.warehouseEntryPackagePM.ContainerNumberWarning;
                }
            }

            this.SetLabel();
            this.SetUIProperties();

            this.IsLoadPage = true;
        }


    }


    ResetPackageItem() {


        if (!this.IsNewEntity) {

            if (this.warehouseEntryPackagePM && this.savedItem) {
                this.warehouseEntryPackagePM.PackageTypeId = this.savedItem.PackageTypeId;
                this.warehouseEntryPackagePM.ContainerNumber = this.savedItem.ContainerNumber;
                this.warehouseEntryPackagePM.Length = this.savedItem.Length;
                this.warehouseEntryPackagePM.Height = this.savedItem.Height;
                this.warehouseEntryPackagePM.Width = this.savedItem.Width;
                this.warehouseEntryPackagePM.Volume = this.savedItem.Volume;
                this.warehouseEntryPackagePM.Weight = this.savedItem.Weight;
                this.warehouseEntryPackagePM.Description = this.savedItem.Description;
                this.warehouseEntryPackagePM.Seal = this.savedItem.Seal;
                this.warehouseEntryPackagePM.Harmonize = this.savedItem.Harmonize;
                this.warehouseEntryPackagePM.Location = this.savedItem.Location;
                this.warehouseEntryPackagePM.Dimensions = this.savedItem.Dimensions;
                this.warehouseEntryPackagePM.Instock = this.savedItem.Instock;
                this.warehouseEntryPackagePM.Quantity = this.savedItem.Quantity;
                this.warehouseEntryPackagePM.ContainerNumberWarning = this.savedItem.ContainerNumberWarning;
            }

            if (this.warehouseEntryPM && !this.IsParentDirty && this.warehouseEntryPM.IsDirty) this.warehouseEntryPM.IsDirty = this.IsParentDirty;
            if (this.warehouseEntryPackagePM && !this.IsChildDirty && this.warehouseEntryPackagePM.IsDirty) this.warehouseEntryPackagePM.IsDirty = this.IsChildDirty;
        }
    }


    CanceluttonClicked() {
        this.ResetPackageItem();
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

   
    SaveButtonClicked() {

        this.ValidationErrorsList = [];


        var errorsArray = this.validator.Validate("WarehouseEntryPackage", this.warehouseEntryPackagePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }

        if (this.ValidationErrorsList.length == 0) {

           if (this.warehouseEntryPackagePM.IsDirty) {

                 this.warehouseEntryPackagePM.Height = this.warehouseEntryPackagePM.Height!=null ? AppTool.Round(this.warehouseEntryPackagePM.Height, 2) : null;
                 this.warehouseEntryPackagePM.Width = this.warehouseEntryPackagePM.Width != null ? AppTool.Round(this.warehouseEntryPackagePM.Width, 2) : null;
                 this.warehouseEntryPackagePM.Length = this.warehouseEntryPackagePM.Length != null ? AppTool.Round(this.warehouseEntryPackagePM.Length, 2) : null;
                 this.warehouseEntryPackagePM.Volume = this.warehouseEntryPackagePM.Volume != null ? AppTool.Round(this.warehouseEntryPackagePM.Volume, 3) : null;
                 this.warehouseEntryPackagePM.Weight = this.warehouseEntryPackagePM.Weight != null ? AppTool.Round(this.warehouseEntryPackagePM.Weight, 3) : null;

                if (this.warehouseEntryPackagePM.Quantity) this.warehouseEntryPackagePM.Quantity = AppTool.Round(this.warehouseEntryPackagePM.Quantity, 0);

                this.ComplateSave();
            }
            else {
               SessionLocator.CurrentSession.CloseCurrentWindow();
            }

        }


    }

    ComplateSave() {


        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

        if (!this.warehouseEntryPackagePM.IsContainer) {

            var height: string = this.warehouseEntryPackagePM.Height!=null ? this.warehouseEntryPackagePM.Height.toString() : "";
            var width: string = this.warehouseEntryPackagePM.Width != null ? this.warehouseEntryPackagePM.Width.toString() : "";
            var length: string = this.warehouseEntryPackagePM.Length!= null? this.warehouseEntryPackagePM.Length.toString() : "";

            this.warehouseEntryPackagePM.Dimensions = length + "-" + width + "-" + height;
        }

        this.warehouseEntryPackagePM.PackageTypeName = "";

        this.warehouseEntryPackagePM.Instock = this.warehouseEntryPackagePM.Quantity;
        
        if (this.AllPackageTypes) {
            var packageTypeList: PackageTypeList = this.AllPackageTypes.filter(d=> d.Id == this.warehouseEntryPackagePM.PackageTypeId)[0];
            if (packageTypeList) this.warehouseEntryPackagePM.PackageTypeName = packageTypeList.EnglishName;
        }

        if (this.IsNewEntity && this.ViewModelTrigger.WarehouseEntryPackagesLists) {
       
           this.ViewModelTrigger.WarehouseEntryPackagesLists.push(this.warehouseEntryPackagePM);
       }   

       SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
       SessionLocator.CurrentSession.CurrentWindow.Close("Refresh");
           
            
          

   

    }


    OnDimensionsChange(value: number, type: string) {
        
        this.Computed();
    }

    OnVolumChange(value: number) {
        var isDimEnable: boolean = false;
        if (this.warehouseEntryPackagePM.Volume == null) {
            isDimEnable = true;
        }

        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Length", "WarehouseEntryPackage", isDimEnable);
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Height", "WarehouseEntryPackage", isDimEnable);
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Width", "WarehouseEntryPackage", isDimEnable);
     

    }

    IsEnable: boolean = false;
    OnQuantityChange() {

        var isEnable: boolean = false;
        var isEnableVolume: boolean = false;
        var isEnableDim: boolean = false;
        if (this.warehouseEntryPackagePM.Quantity && this.warehouseEntryPackagePM.Quantity>0) {
            isEnable = true;
        }

        this.IsEnable = isEnable;

        if (isEnable) {
            if (this.warehouseEntryPackagePM.Height != null || this.warehouseEntryPackagePM.Width != null || this.warehouseEntryPackagePM.Length != null) isEnableDim = true;
            else if (this.warehouseEntryPackagePM.Volume != null) isEnableVolume = true;
            else {
                isEnableVolume = true;
                isEnableDim = true;
            }

        }
     
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Length", "WarehouseEntryPackage", isEnableDim);
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Height", "WarehouseEntryPackage", isEnableDim);
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Width", "WarehouseEntryPackage", isEnableDim);
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Volume", "WarehouseEntryPackage", isEnableVolume);

        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Weight", "WarehouseEntryPackage", isEnable);
        this.warehouseEntryPackagePM.UIProperties.SetEnabled("Harmonize", "WarehouseEntryPackage", isEnable);

        this.Computed();
  
    }


    Computed() {

        if (this.warehouseEntryPackagePM.Width == null && this.warehouseEntryPackagePM.Height == null && this.warehouseEntryPackagePM.Length == null && this.warehouseEntryPackagePM.Quantity == null) {
            this.warehouseEntryPackagePM.Volume = null;
            if (this.IsEnable) {
                this.warehouseEntryPackagePM.UIProperties.SetEnabled("Volume", "WarehouseEntryPackage", true);
            }
        } else {
            var volume: number = AppTool.GetVolumeFromDimentions(this.warehouseEntryPM.DimensionsUnitCode, this.warehouseEntryPM.VolumeUnitCode, this.warehouseEntryPackagePM.Width, this.warehouseEntryPackagePM.Height, this.warehouseEntryPackagePM.Length, this.warehouseEntryPackagePM.Quantity);
            this.warehouseEntryPackagePM.Volume = volume;
            this.warehouseEntryPackagePM.UIProperties.SetEnabled("Volume", "WarehouseEntryPackage", false);
        }

        this.IsVolumRefresh = !this.IsVolumRefresh;
    }


    ContainerNumberLostFocus(input: string) {
        this.ValidateContainerNumber(input);
    }


    ValidateContainerNumber(input: string) {
        var warnings: string[] = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);

            if (AppTool.IsNullOrEmpty(this.warehouseEntryPackagePM.ContainerNumberWarning)) {
                this.warehouseEntryPackagePM.ContainerNumberWarning = error;
            }
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.warehouseEntryPackagePM.ContainerNumberWarning)) {
                this.warehouseEntryPackagePM.ContainerNumberWarning = null;
            }
           
        }

        this.WarningErrorsList = warnings;
      
        //var isDirtywarehouseEntryPM = this.warehouseEntryPM.IsDirty;
        //var isDirty = this.warehouseEntryPackagePM.IsDirty;
        //this.warehouseEntryPackagePM.ContainerNumberWarning = error;

        //if (!this.IsNewEntity) {
        //    this.warehouseEntryPackagePM.IsDirty = isDirty;
        //    this.warehouseEntryPM.IsDirty = isDirtywarehouseEntryPM;
        //}
    }


}
