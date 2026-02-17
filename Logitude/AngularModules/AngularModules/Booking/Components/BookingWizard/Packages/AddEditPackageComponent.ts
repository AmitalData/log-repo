import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BookingWizardPackageItem} from './PackagesTabComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPackageComponent.html',
})

export class AddEditPackageComponent extends BaseComponent {
    public ObjectTableName: string;
    public DataContext: BookingWizardPackageItem;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: BookingWizardPackageItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.SetLabels();
        this.Clone();
    }

    public TareLabel: string;
    public VolumeLabel: string;
    public DimensionsLabel: string;
    public GrossWeightLabel: string;
    public VolumetricWeightLabel: string;
    SetLabels() {
        this.TareLabel = TextCodeTranslator.Translate('BookingPackage.S.Packages.Tare').replace('%WeightCode', this.DataContext.BookingPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate('BookingPackage.S.Packages.Volume').replace('%VolumeCode', this.DataContext.BookingPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator.Translate('BookingPackage.S.Packages.Dimensions').replace('%UnitCode', this.DataContext.BookingPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('BookingPackage.S.Packages.Weight').replace('%WeightCode', this.DataContext.BookingPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('BookingPackage.S.Packages.VolumetricWeight').replace('%WeightCode', this.DataContext.BookingPM.ChargeableWeightUnitCode);
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {

                this.DataContext.IsNewEntity = false;

                if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                }

                if (this.DataContext.BookingPM.BookingPackages.indexOf(this.EntityPM) == -1) {
                    this.DataContext.BookingPM.AddBookingPackage(this.EntityPM);                    
                }               
            }

            this.DataContext.fatherComponent.BuildData();
            this.DataContext.fatherComponent.ComputeTotals();
            //this.DataContext.SetUIProperties();
            this.CurrentSession.CloseCurrentWindow();
            this.DataContext.IsWindowMode = false;
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.BookingPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
