import {Component} from '@angular/core';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {ShipmentPackageItem} from './PackagesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ShipmentDeliveryPM} from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PickUpDeliveryPackageHarmonizePM } from '../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditOceanPackageComponent.html',
})

export class AddEditOceanPackageComponent {
    public EntityPM: ShipmentPackagePM;
    public DataContext: ShipmentPackageItem;
    public ObjectTableName: string = "ShipmentPackage";
    public SelectedTabCode: string = "0";
    public IsFCLEntity: boolean = false;
    public IsLCLEntity: boolean = false; 
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(dataContext: ShipmentPackageItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.FillMethodsList();
        this.IsFCLEntity = dataContext.IsFCLEntity;
        this.IsLCLEntity = dataContext.IsLCLEntity;
        this.SetLabels();
        this.Clone();
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

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var isValid: boolean = this.Validate();

        if (isValid) {

            this.DataContext.PackageItemsList.Collection.forEach(item => {
                if (item != null) {
                    if (item.IsNewEntity) {
                        if (this.DataContext.EntityPM.ShipmentPackageItems.indexOf(item.EntityPM) == -1) {
                            item.IsNewEntity = false;
                            this.DataContext.EntityPM.AddShipmentPackageItemPM(item.EntityPM);
                        }
                    }
                }
            });

            if (this.DataContext.IsNewEntity) {
                this.DataContext.ShipmentPM.AddPackage(this.EntityPM);
                this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);

                this.DataContext.fatherComponent.SetGenerateData();
                this.DataContext.fatherComponent.ComputeTotals();
            }

            this.DataContext.IsNewEntity = false;

            this.UpdateDeliveryPackage();

            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private Validate() {
        var myResult: boolean = true;

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.DataContext.PackageItemsList.Collection.forEach(item => {
            if (item != null) {
                Validator.TryValidateObject(item, "ShipmentPackageItem", errors);
            }
        });

        if (AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {

            if (this.DataContext.IsLCLEntity) {
                errors.push("Package Type is required");
            }

            else {
                errors.push("Container Type is required");
            }
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.Weight)) {
            errors.push("Gross Weight is required");
        }

        this.ValidationErrorsList = errors;

        if (errors.length > 0) {
            myResult = false;
        }

        return myResult;
    }
    private UpdateDeliveryPackage() {
        if (this.EntityPM.DeliveryId) {
            var Delivery: ShipmentDeliveryPM = this.DataContext.ShipmentPM.ShipmentDeliveries.filter(f => f.Id == this.EntityPM.DeliveryId)[0];
            if (Delivery) {
                var DeliveryPackagePM: ShipmentPickUpDeliveryPackagePM = Delivery.ShipmentPickUpDeliveryPackages.filter(f => f.OriginalShipmentPackageId == this.EntityPM.Id)[0];
                if (DeliveryPackagePM) {

                    if (DeliveryPackagePM.ContainerNumber != this.EntityPM.ContainerNumber) {
                        DeliveryPackagePM.ContainerNumber = this.EntityPM.ContainerNumber;
                    }

                    if (DeliveryPackagePM.Description != this.EntityPM.Description) {
                        DeliveryPackagePM.Description = this.EntityPM.Description;
                    }

                    if (DeliveryPackagePM.PackageTypeId != this.EntityPM.PackageTypeId) {
                        DeliveryPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
                    }

                    if (DeliveryPackagePM.PackageTypeName != this.EntityPM.PackageTypeName) {
                        DeliveryPackagePM.PackageTypeName = this.EntityPM.PackageTypeName;
                    }

                    if (DeliveryPackagePM.Quantity != this.EntityPM.Quantity) {
                        DeliveryPackagePM.Quantity = this.EntityPM.Quantity;
                    }

                    if (DeliveryPackagePM.Volume != this.EntityPM.Volume) {
                        DeliveryPackagePM.Volume = this.EntityPM.Volume;
                    }

                    if (DeliveryPackagePM.Weight != this.EntityPM.Weight) {
                        DeliveryPackagePM.Weight = this.EntityPM.Weight;
                    }

                    if (DeliveryPackagePM.ShipperSeal != this.EntityPM.ShipperSeal) {
                        DeliveryPackagePM.ShipperSeal = this.EntityPM.ShipperSeal;
                    }

                    if (DeliveryPackagePM.Width != this.EntityPM.Width) {
                        DeliveryPackagePM.Width = this.EntityPM.Width;
                    }

                    if (DeliveryPackagePM.Height != this.EntityPM.Height) {
                        DeliveryPackagePM.Height = this.EntityPM.Height;
                    }

                    if (DeliveryPackagePM.Length != this.EntityPM.Length) {
                        DeliveryPackagePM.Length = this.EntityPM.Length;
                    }

                    if (DeliveryPackagePM.Harmonize != this.EntityPM.Harmonize) {
                        DeliveryPackagePM.Harmonize = this.EntityPM.Harmonize;
                    }

                    if (DeliveryPackagePM.IsMultiHarmonize != this.EntityPM.IsMultiHarmonize) {
                        DeliveryPackagePM.IsMultiHarmonize = this.EntityPM.IsMultiHarmonize;
                    }

                    if (DeliveryPackagePM.PickUpDeliveryPackageHarmonizes != null && DeliveryPackagePM.PickUpDeliveryPackageHarmonizes.length > 0) {
                        for (var i = DeliveryPackagePM.PickUpDeliveryPackageHarmonizes.length - 1; i >= 0; i--) {
                            var item = DeliveryPackagePM.PickUpDeliveryPackageHarmonizes[i];
                            DeliveryPackagePM.RemovePickUpDeliveryPackageHarmonizePM(item);
                        }
                    }

                    if (this.EntityPM.ShipmentPackageHarmonizes.length > 0) {
                        this.EntityPM.ShipmentPackageHarmonizes.forEach(harmonizeItem => {
                            if (harmonizeItem != null) {
                                var harmonize = new PickUpDeliveryPackageHarmonizePM(null);
                                harmonize.Harmonize = harmonizeItem.Harmonize;
                                harmonize.Tenant = harmonizeItem.Tenant;
                                DeliveryPackagePM.AddPickUpDeliveryPackageHarmonizePM(harmonize);
                            }
                        });
                    }
                }
            }
        }
    }

    MultiHarmonizeClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { PackagePM: this.EntityPM, ShipmentPM: this.DataContext.ShipmentPM, IsEditingEnabled: this.DataContext.IsEditingEnabled };
        logWindow.Title = "Container Multi-Harmonize";
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditPackageHarmonizeComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.DataContext.SetUIProperties_Harmonize();
            }
        });
    }
    ChooseHarmonizeClicked() {
        if (this.DataContext) {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("HarmonizeCode") + " Search";
            logitudeWindow.WindowArgs = { Entity: this.DataContext, FieldName: 'Harmonize' };
            logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Harmonizes/HarmonizesComponent");
            logitudeWindow.WindowClosed.subscribe(s => {

            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('ContainerNumber');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('Tare');
        this.myCloner.AddField('ShipperSeal');
        this.myCloner.AddField('Notes');       
        this.myCloner.AddField('IsDangerous');
        this.myCloner.AddField('ClassNumber');
        this.myCloner.AddField('UnNumber');
        this.myCloner.AddField('PackagingGroup');
        this.myCloner.AddField('IMDGCode');
        this.myCloner.AddField('FlashPoint');
        this.myCloner.AddField('MaterialDescription');
        this.myCloner.AddField('Harmonize');
        this.myCloner.AddField('CarrierSeal');
        this.myCloner.AddField('Temperature');
        this.myCloner.AddField('Ventilation');
        this.myCloner.AddField('MarksAndNumbers');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('SOC');
        this.myCloner.AddField('VGM');
        this.myCloner.AddField('MethodUsed');
        this.myCloner.AddField('CommodityNumber');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Reference3');
        this.myCloner.AddField('Reference4');
        this.myCloner.AddField('Make');
        this.myCloner.AddField('Model');
        this.myCloner.AddField('Year');
        this.myCloner.AddField('Color');
        this.myCloner.AddField('ChassisNumber');
        this.myCloner.AddField('RegistrationNumber');
        this.myCloner.AddField('CountryId');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
