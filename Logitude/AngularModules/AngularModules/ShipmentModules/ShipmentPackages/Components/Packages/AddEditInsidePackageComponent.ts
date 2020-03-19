import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {InsideShipmentPackagePM} from '../../../../Shipment/EntityPMs/InsideShipmentPackagePM';
import {InsideShipmentPackageItem} from './PackagesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditInsidePackageComponent.html',
})

export class AddEditInsidePackageComponent {
    public EntityPM: InsideShipmentPackagePM;
    public DataContext: InsideShipmentPackageItem;
    public ObjectTableName: string = "InsideShipmentPackage";
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(dataContext: InsideShipmentPackageItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
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
        this.DimensionsLabel = TextCodeTranslator.Translate('ShipmentPackage.O.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewEntity) {
                this.DataContext.ShipmentPackagePM.AddInsideShipmentPackagePM(this.EntityPM);
                this.DataContext.fatherComponent.BuildInsideItemsSource();
                this.DataContext.fatherComponent.ComputeFromInsidePackages();

                if (this.DataContext.fatherComponent.Row) {
                    var isExpandaple = false;

                    if (this.DataContext.fatherComponent.InsideItemsSource.length > 0) {
                        isExpandaple = true;
                    }

                    this.DataContext.fatherComponent.Row.SetExpandaple(isExpandaple);
                }

                if (this.DataContext.fatherComponent.InsideItemsSource.length == 0) {
                    this.DataContext.fatherComponent.fatherComponent.BuildItemsSource();
                }
            }

            this.DataContext.IsNewEntity = false;
            this.DataContext.fatherComponent.ComputeFromInsidePackages();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
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
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('PackageTypeId');
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
        this.myCloner.AddEntity(this.DataContext.ShipmentPackagePM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
