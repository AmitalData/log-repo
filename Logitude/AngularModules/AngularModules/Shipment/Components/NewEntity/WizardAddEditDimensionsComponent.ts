import {Component} from '@angular/core';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentOrderPackagePM} from '../../EntityPMs/ShipmentOrderPackagePM';
import {WizardDimensionItem} from './WizardDimensionsComponent';

@Component({
    moduleId: module.id,
    templateUrl: './WizardAddEditDimensionsComponent.html',
})

export class WizardAddEditDimensionsComponent {
    public EntityPM: ShipmentOrderPackagePM;
    public DataContext: WizardDimensionItem;
    public ObjectTableName: string = "ShipmentOrderPackage";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }
    
    public DimensionsLabel: string;
    public VolumeLabel: string;
    public VolumetricWeightLabel: string;
    public GrossWeightLabel: string;
    SetWindowArgs(item: WizardDimensionItem) {
        this.DataContext = item;
        this.EntityPM = item.EntityPM;
        this.DataContext.IsWindowMode = true;

        this.DimensionsLabel = item.fatherComponent.DimensionsColumnHeader;
        this.VolumeLabel = item.fatherComponent.VolumeColumnHeader;
        this.VolumetricWeightLabel = item.fatherComponent.VolumetricWeightColumnHeader;
        this.GrossWeightLabel = item.fatherComponent.WeightColumnHeader;
    }

    CancelButtonClicked() {
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DataContext.IsPackageTypeVisible) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
                errors.push("Package Type is required");
            }

            if (AppTool.IsNullOrZero(this.EntityPM.GrossWeight)) {
                errors.push("Gross Weight is required");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.DataContext.fatherComponent.EntityPM.AddOrderPackage(this.DataContext.EntityPM);

            if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
            }

            this.DataContext.IsWindowMode = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
}
