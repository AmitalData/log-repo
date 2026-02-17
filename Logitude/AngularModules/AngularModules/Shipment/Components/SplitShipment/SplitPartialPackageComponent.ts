import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../EntityPMs/ShipmentPackagePM';
import {SplitShipmentItem} from './SplitShipmentComponent';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './SplitPartialPackageComponent.html',
})

export class SplitPartialPackageComponent extends BaseComponent {
    public ItemPM: SplitShipmentItem = null;
    public EntityPM: ShipmentPackagePM;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "ShipmentPackage";
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string = null;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.ItemPM = args['Item'];

        if (this.ItemPM) {
            this.EntityPM = this.ItemPM.EntityPM;
            this.ShipmentPM = this.ItemPM.fatherComponent.EntityPM;
            this.IsLCLEntity = this.ItemPM.fatherComponent.IsLCLEntity;
            this.IsFCLEntity = this.ItemPM.fatherComponent.IsFCLEntity;
            this.TransportModeId = this.ShipmentPM.TransportModeId;
            this.Quantity = this.ItemPM.Quantity;
            this.Volume = this.ItemPM.Volume;
            this.Weight = this.ItemPM.Weight;
            this.SetLabels();
            this.SetUIProperties();
        }
    }

    public TareLabel: string;
    public VolumeLabel: string;
    public DimensionsLabel: string;
    public GrossWeightLabel: string;
    public VolumetricWeightLabel: string;
    public IsPackageItemsMessageVisible: boolean = false;
    SetLabels() {
        this.TareLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.ShipmentPM.ChargeableWeightUnitCode);
    }
    SetUIProperties() {
        //var isQuantityValid: boolean = true;

        if (this.TransportModeId != "A") {
            this.UIProperties.SetRequired('Weight', this.ObjectTableName, AppTool.IsNullOrEmpty(this.Weight) ? true : false);
        }

        if (this.EntityPM.ShipmentPackageItems.length > 0) {
            this.IsPackageItemsMessageVisible = true;
        }

        if (AppTool.IsNullOrEmpty(this.Volume)) {
            this.UIProperties.SetEnabled('Volume', this.ObjectTableName, false);
        }

        if (AppTool.IsNullOrEmpty(this.Weight)) {
            this.UIProperties.SetEnabled('Weight', this.ObjectTableName, false);
        }
    }

    private _quantity: number = null;
    public get Quantity() { return this._quantity; }
    public set Quantity(value: number) {
        if (this._quantity != value) {
            this._quantity = value;
        }
    }

    private _volume: number = null;
    public get Volume() { return this._volume; }
    public set Volume(value: number) {
        if (this._volume != value) {
            this._volume = value;
        }
    }

    private _weight: number = null;
    public get Weight() { return this._weight; }
    public set Weight(value: number) {
        if (this._weight != value) {
            this._weight = value;
            this.SetUIProperties();
        }
    }

    OnGrossWeightLostFocus(input1: number) {

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        // Quantity
        if (AppTool.IsNullOrEmpty(this.Quantity)) {
            errors.push("Quantity field is required");
        }

        else {
            if (this.Quantity >= this.ItemPM.Quantity) {
                errors.push("Quantity should be less than " + this.ItemPM.Quantity);
            }
        }

        // Volume
        if (this.ItemPM.Volume != null) {
            if (AppTool.IsNullOrEmpty(this.Volume)) {
                errors.push("Volume field is required");
            }

            else {
                if (this.Volume >= this.ItemPM.Volume) {
                    errors.push("Volume should be less than " + this.ItemPM.Volume);
                }
            }
        }

        // Weight
        if (this.ItemPM.Weight != null) {
            if (AppTool.IsNullOrEmpty(this.Weight)) {
                if (this.TransportModeId != "A") {
                    errors.push("Weight field is required");
                }
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.ItemPM.Weight)) {
                    if (this.Weight >= this.ItemPM.Weight) {
                        errors.push("Weight should be less than " + this.ItemPM.Weight);
                    }
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    }
}
