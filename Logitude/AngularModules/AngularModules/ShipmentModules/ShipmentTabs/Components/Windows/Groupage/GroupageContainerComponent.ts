import {Component} from '@angular/core';
import {AppTool, FormatTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {PackageTypeList} from '../../../../../Common/EntityLists/PackageTypeList';
import {PackageTypeListService} from '../../../../../Common/Services/StandardLists/PackageTypeListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {GroupageComponent, GroupageListItem} from './GroupageComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './GroupageContainerComponent.html',
})

export class GroupageContainerComponent extends BaseComponent {
    public EntityPM: ShipmentPackagePM = null;
    public ShipmentPM: ShipmentPM = null;
    public FatherComponent: GroupageComponent = null;
    public ShipmentListItem: GroupageListItem;
    public ObjectTableName: string = "ShipmentPackage";
    public DataContext = this;
    public ValidationErrorsList: string[];
    public WarningErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.FatherComponent = args['FatherComponent'];
        this.ShipmentListItem = args['ShipmentListItem'];
        this.ShipmentPM = this.FatherComponent.EntityPM;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ContainerNumber)) {
            this.isContainerNumberExists = true;
        }

        this.SetLabels();
        this.SetUIProperties();
    }

    public TareLabel: string;
    public VolumeLabel: string;
    public DimensionsLabel: string;
    public GrossWeightLabel: string;
    public VolumetricWeightLabel: string;
    SetLabels() {
        this.TareLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    }

    private isContainerNumberExists: boolean = false;
    SetUIProperties() {
        this.UIProperties.SetEnabled("ContainerNumber", this.ObjectTableName, !this.isContainerNumberExists);
        this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
        this.UIProperties.SetRequired("Weight", this.ObjectTableName, AppTool.IsNullOrZero(this.Weight) ? true : false);
    }

    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(value: string) {
        if (this.EntityPM.PackageTypeId != value) {
            this.EntityPM.PackageTypeId = value;

            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.PackageTypeName = null;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.PackageTypeName = list.EnglishName;                            
                        }
                    }
                });
            }
        }
    }

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(newValue: string) {
        if (this.EntityPM.PackageTypeName != newValue) {
            this.EntityPM.PackageTypeName = newValue;
        }
    }

    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    set ContainerNumber(value: string) {
        if (this.EntityPM.ContainerNumber != value) {

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.ContainerNumber = value;
            }

            else {
                this.EntityPM.ContainerNumber = value.toUpperCase();
            }

            this.ValidateContainerNumber(value);
        }
    }

    ContainerNumberLostFocus(input: string) {
        this.ValidateContainerNumber(input);
    }

    ValidateContainerNumber(input: string) {
        var warnings: string[] = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);
        }

        this.WarningErrorsList = warnings;
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(value: number) {
        if (this.EntityPM.Weight != value) {
            this.EntityPM.Weight = AppTool.Round(value, 3);

            this.SetUIProperties();
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = AppTool.Round(value, 3);
            this.ComputeVolumetricWeight();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(value: number) {
        if (this.EntityPM.VolumetricWeight != value) {
            this.EntityPM.VolumetricWeight = AppTool.Round(value, 3);
        }
    }

    get Tare() { return this.EntityPM.Tare; }
    set Tare(value: number) {
        if (this.EntityPM.Tare != value) {
            this.EntityPM.Tare = AppTool.Round(value, 3);
        }
    }

    get ShipperSeal() { return this.EntityPM.ShipperSeal; }
    set ShipperSeal(value: string) {
        if (this.EntityPM.ShipperSeal != value) {
            this.EntityPM.ShipperSeal = value;
        }
    }

    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.EntityPM.Quantity, this.EntityPM.Width, this.EntityPM.Height, this.EntityPM.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            errors.push("Container Type is required");
        }

        if (AppTool.IsNullOrZero(this.Weight)) {
            errors.push("Gross Weight is required");
        }

        if (!AppTool.IsNullOrEmpty(this.ContainerNumber)) {
            if (this.FatherComponent.MyGroupagePackages.filter(d => !AppTool.IsNullOrEmpty(d.ContainerNumber) && d.ContainerNumber.toUpperCase() == this.ContainerNumber.toUpperCase()).length > 0) {
                errors.push("This container number is already added");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            var indexOfItem = this.FatherComponent.ShipmentsPackages.indexOf(this.ShipmentListItem);
            if (indexOfItem > -1) {
                this.FatherComponent.ShipmentsPackages.splice(indexOfItem, 1);
            }

            var newItem = new GroupageListItem(this.EntityPM, this.FatherComponent, true);
            this.FatherComponent.MyGroupagePackages.push(newItem);
            this.FatherComponent.BuildToggleItems();
            newItem.UpdateItem();
            newItem.ComputeFromInsidePackages();
            this.CurrentSession.CloseCurrentWindow();
        }
    }
}
