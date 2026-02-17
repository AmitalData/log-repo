import {Component} from '@angular/core';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuotePackagePM} from '../../EntityPMs/QuotePackagePM';
import {DimensionsPackageItem} from './QuoteDimensionsComponent';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './NewQuoteAddEditDimensionsComponent.html',
})

export class NewQuoteAddEditDimensionsComponent {
    public EntityPM: QuotePackagePM;
    public DataContext: DimensionsPackageItem;
    public ObjectTableName: string = "QuotePackage";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    public DimensionsLabel: string;
    public VolumeLabel: string;
    public VolumetricWeightLabel: string;
    public GrossWeightLabel: string;
    SetWindowArgs(item: DimensionsPackageItem) {
        this.DataContext = item;
        this.EntityPM = item.EntityPM;
        this.DataContext.IsWindowMode = true;

        this.DimensionsLabel = TextCodeTranslator.Translate("QuotePackage.F.Dimensions").replace('%UnitCode', this.DataContext.QuotePM.DimensionsUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate("QuotePackage.F.Volume").replace('%VolumeCode', this.DataContext.QuotePM.VolumeUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("QuotePackage.F.VolumetricWeight").replace('%WeightCode', this.DataContext.QuotePM.ChargeableWeightUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("QuotePackage.F.GrossWeight").replace('%WeightCode', this.DataContext.QuotePM.GrossWeightUnitCode);
    }

    CancelButtonClicked() {
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length == 0) {

            this.DataContext.fatherComponent.EntityPM.AddQuotePackagePM(this.DataContext.EntityPM);

            if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
            }

            this.DataContext.IsWindowMode = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
}
