import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {QuotePackagePM} from '../../../../Quote/EntityPMs/QuotePackagePM';
import {QuotePackageItem} from './PackagesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './AddEditPackageComponent.html',
})

export class AddEditPackageComponent {
    public EntityPM: QuotePackagePM;
    public DataContext: QuotePackageItem;
    public ObjectTableName: string = "QuotePackage";
    public ValidationErrorsList: string[];
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.RunComponent();
    }
    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {
        let screenCode: string = "QuotePackage.AdditionalFields";
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.HideLastColumn = true;
                cmpRef.instance.LabelWidth = 120;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
            });
    }
    SetDataContext(dataContext: QuotePackageItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.SetLabels();
        this.Clone();
    }

    public DimensionsLabel: string;
    public VolumeLabel: string;
    public VolumetricWeightLabel: string;
    public GrossWeightLabel: string;    
    SetLabels() {
        this.DimensionsLabel = TextCodeTranslator.Translate('QuotePackage.F.Dimensions').replace('%UnitCode', this.DataContext.QuotePM.DimensionsUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate('QuotePackage.F.Volume').replace('%VolumeCode', this.DataContext.QuotePM.VolumeUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('QuotePackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.QuotePM.ChargeableWeightUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('QuotePackage.F.GrossWeight').replace('%WeightCode', this.DataContext.QuotePM.GrossWeightUnitCode);        
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DataContext.QuotePM.TransportModeId != "A") {
            if (AppTool.IsNullOrEmpty(this.DataContext.PackageTypeId)) {
                errors.push("Package Type Field is Required");
            }

            if (this.DataContext.GrossWeight == null) {
                errors.push("Gross Weight Field is Required");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewEntity) {

                this.DataContext.QuotePM.AddQuotePackagePM(this.EntityPM);
                this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                this.DataContext.fatherComponent.BuildItemsSource();   
            }

            this.DataContext.IsNewEntity = false;
            this.DataContext.fatherComponent.ResetTotalEditedValues();
            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('PickupDeliveryVolume');
        this.myCloner.AddField('PickupDeliveryVolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuotePM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
