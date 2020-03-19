import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {ShipmentPackageItem} from './PackagesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAirPackageComponent.html',
})

export class AddEditAirPackageComponent {
    public EntityPM: ShipmentPackagePM;
    public DataContext: ShipmentPackageItem;
    public ObjectTableName: string = "ShipmentPackage";
    public OkBtnId: string;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.OkBtnId = "OkBtn_-1_-1"; 
        }

        else {
            this.OkBtnId = "OkBtn_" + this.CurrentSession.GetNewId("OkBtn"); 
        }
    }

    SetDataContext(dataContext: ShipmentPackageItem) {
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
        this.DimensionsLabel = TextCodeTranslator.Translate('Shipment.O.Packages.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
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
        //this.EntityPM.ShipmentPackageItems.forEach(packageItem => {
        //    Validator.TryValidateObject(packageItem, this.ObjectTableName, errors);
        //});

        this.DataContext.PackageItemsList.Collection.forEach(item => {
            if (item != null) {
                Validator.TryValidateObject(item, "ShipmentPackageItem", errors);
            }
        });

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

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
                this.DataContext.IsNewEntity = false;
                this.DataContext.ShipmentPM.AddPackage(this.EntityPM);
                this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                //if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                //    this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                //}
                //this.DataContext.fatherComponent.SetGenerateData();
                //this.DataContext.fatherComponent.ComputeTotals();
            }

            this.DataContext.fatherComponent.SetGenerateData();
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
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('CommodityNumber');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Reference3');
        this.myCloner.AddField('Reference4');        
        
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        this.DataContext.ResetPackageItems();
        this.myCloner.RejectChanges();
    }

    onWeightLostFocus($event) {
        if (this.TabKeypressed == true) {
            var OkBtnElement = document.getElementById(this.OkBtnId);
            if (OkBtnElement) {
                OkBtnElement.focus();
            }
            this.TabKeypressed = false;
        }
    }
    TabKeypressed: boolean = false;
    onWeightkeyDown($event) {
        if ($event.keyCode == 9) {
            this.TabKeypressed = true;
        }
    } 
}
