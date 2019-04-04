import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentOrderPackagePM} from '../../../../Shipment/EntityPMs/ShipmentOrderPackagePM';
import {ShipmentOrderPackageItem} from './OrdersTabComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditOrderPackageComponent.html',
})

export class AddEditOrderPackageComponent {
    public EntityPM: ShipmentOrderPackagePM;
    public ObjectTableName = "ShipmentOrderPackage";
    public DataContext: ShipmentOrderPackageItem;
    public ValidationErrorsList: string[] = [];
    public TransportModeId: string = null;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(args: any) {
        this.DataContext = args;
        this.EntityPM = args.EntityPM;
        this.TransportModeId = this.DataContext.fatherComponent.TransportModeId;
        this.IsLCLEntity = this.DataContext.fatherComponent.IsLCLEntity;
        this.IsFCLEntity = this.DataContext.fatherComponent.IsFCLEntity;
        this.SetLabels();
        this.Clone();
    }

    public TypeLabel: string;
    public VolumeLabel: string;
    public DimensionsLabel: string;
    public GrossWeightLabel: string;
    public VolumetricWeightLabel: string;
    SetLabels() {

        if (this.IsLCLEntity) {
            this.TypeLabel = TextCodeTranslator.Translate("ShipmentOrderPackage.F.PackageTypeId");
        }

        else {
            this.TypeLabel = TextCodeTranslator.Translate("ShipmentOrderPackage.F.ContainerTypeId");
        }

        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentOrderPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator.Translate('ShipmentOrderPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentOrderPackage.F.GrossWeight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('ShipmentOrderPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.TransportModeId != "A") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
                errors.push(this.TypeLabel + " is required");
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.GrossWeight)) {
                errors.push("Gross Weight is required");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewEntity) {

                this.DataContext.IsNewEntity = false;

                if (this.DataContext.ShipmentPM.ShipmentOrderPackages.indexOf(this.EntityPM) == -1) {
                    this.DataContext.ShipmentPM.AddOrderPackage(this.EntityPM);
                    this.DataContext.fatherComponent.BuildItemsSource();
                    this.DataContext.fatherComponent.ComputeTotals();
                }
            }

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('GrossWeight');        
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
