import { Component } from '@angular/core';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ShipmentCommodityItem } from './AWBPackagesTabComponent';
import { ShipmentCommodityPM } from '../../../../../Shipment/EntityPMs/ShipmentCommodityPM';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { Cloner } from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    templateUrl: './AWBAddEditCommodityComponent.html',
})

export class AWBAddEditCommodityComponent {
    public EntityPM: ShipmentCommodityPM;
    public ObjectTableName: string;
    public DataContext: ShipmentCommodityItem;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(dataContext: ShipmentCommodityItem) {
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
        this.TareLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator.Translate('Shipment.O.Packages.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
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

                this.DataContext.ShipmentPM.AddCommodity(this.EntityPM);

                this.DataContext.fatherComponent.BuildData();
                this.DataContext.fatherComponent.SetRebuildButton();
                this.DataContext.fatherComponent.SetGenerateButton();
                this.DataContext.fatherComponent.ComputeTotals();
            }

            this.DataContext.IsNewEntity = false;
            this.DataContext.IsWindowMode = false;
            this.DataContext.SetUIProperties();
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
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
