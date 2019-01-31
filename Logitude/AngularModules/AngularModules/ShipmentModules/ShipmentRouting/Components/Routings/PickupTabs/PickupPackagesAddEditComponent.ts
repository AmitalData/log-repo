import {Component} from '@angular/core';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {PickupPackageItem} from './PickupPackagesTabComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './PickupPackagesAddEditComponent.html',
})

export class PickupPackagesAddEditComponent extends BaseComponent {
    public EntityPM: ShipmentPickUpDeliveryPackagePM;
    public ObjectTableName: string = "ShipmentPickUpDeliveryPackage";
    public DataContext: PickupPackageItem;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();
    }

    SetDataContext(dataContext: PickupPackageItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNewEntity = dataContext.IsNewEntity;
        this.SetLabels();
        this.Clone();
    }

    public DimensionsLabel: string;
    SetLabels() {
        this.DimensionsLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.fatherComponent.ShipmentPM.DimensionsUnitCode);
    }

    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.IsNewEntity) {
                this.DataContext.fatherComponent.EntityPM.AddPackage(this.EntityPM);
                this.DataContext.fatherComponent.BuildItemsSource();
            }

            SessionLocator.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('ContainerNumber');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('Seal');
        this.myCloner.AddField('Harmonize');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Description');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}