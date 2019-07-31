import { Component } from '@angular/core';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { ShipmentPackageHarmonizePM } from '../../../../Shipment/EntityPMs/ShipmentPackageHarmonizePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPackageHarmonizeComponent.html',
})

export class AddEditPackageHarmonizeComponent {
    public ShipmentPM: ShipmentPM = null;
    public EntityPM: ShipmentPackagePM;
    public ItemsSource: HarmonizeItemClass[] = [];
    public ObjectTableName: string = "ShipmentPackageHarmonize";
    public IsEditingEnabled: boolean = true;
    public ValidationErrorsList: string[];
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {

    }

    private isPackageDirty: boolean = false;
    private isShipmentDirty: boolean = false;
    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            if (args) {
                this.IsEditingEnabled = args['IsEditingEnabled'];
                this.EntityPM = args['PackagePM'];
                this.ShipmentPM = args['ShipmentPM'];

                this.isPackageDirty = this.EntityPM.IsDirty;
                this.isShipmentDirty = this.ShipmentPM.IsDirty;

                this.EntityPM.ShipmentPackageHarmonizes.forEach((item: ShipmentPackageHarmonizePM) => {
                    this.ItemsSource.push(new HarmonizeItemClass(item));
                });

                this.Clone();
            }

            this.IsVisibile = true;
        });
    }

    AddButtonClicked() {
        var item: ShipmentPackageHarmonizePM = new ShipmentPackageHarmonizePM(this.EntityPM);
        this.ItemsSource.push(new HarmonizeItemClass(item));
    }
    DeleteItem(item: HarmonizeItemClass) {
        if (item) {
            var index = this.ItemsSource.indexOf(item);
            if (index > -1) {
                this.ItemsSource.splice(index, 1);
            }
        }
    }

    CancelButtonClicked() {

        this.ItemsSource.forEach((item: HarmonizeItemClass) => {
            if (item.Harmonize != item.OldValue) {
                item.Harmonize = item.OldValue;
            }          
        });

        this.EntityPM.IsDirty = this.isPackageDirty;
        this.ShipmentPM.IsDirty = this.isShipmentDirty;

        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];

        this.ItemsSource.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, this.ObjectTableName, errors);
        });

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var allItemsPM: ShipmentPackageHarmonizePM[] = [];
            
            this.ItemsSource.forEach((item: HarmonizeItemClass) => {
                var index = this.EntityPM.ShipmentPackageHarmonizes.indexOf(item.EntityPM);

                if (index > -1) {
                    var itemPM = this.EntityPM.ShipmentPackageHarmonizes[index];
                    if (itemPM) {
                        if (itemPM.Harmonize != item.Harmonize) {
                            itemPM.Harmonize = item.Harmonize;
                        }
                    }
                }

                else {
                    this.EntityPM.ShipmentPackageHarmonizes.push(item.EntityPM);
                }

                allItemsPM.push(item.EntityPM);
            });

            for (var i = this.EntityPM.ShipmentPackageHarmonizes.length - 1; i >= 0; i--) {

                var index = allItemsPM.indexOf(this.EntityPM.ShipmentPackageHarmonizes[i]);

                if (index == -1) {
                    var item = this.EntityPM.ShipmentPackageHarmonizes[i];
                    this.EntityPM.RemoveShipmentPackageHarmonizePM(item);
                } 
            }
            
            this.EntityPM.IsMultiHarmonize = this.EntityPM.ShipmentPackageHarmonizes.length > 0 ? true : false;
            if (this.EntityPM.IsMultiHarmonize) {
                if (this.EntityPM.Harmonize) {
                    this.EntityPM.Harmonize = null;
                }
            }

            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {

    }
    private RejectChanges() {
        //this.DataContext.ResetPackageItems();
        //this.myCloner.RejectChanges();
    }

    ChooseHarmonizeClicked(item: HarmonizeItemClass) {
        if (item) {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("HarmonizeCode") + " Search";
            logitudeWindow.WindowArgs = { Entity: item, FieldName: 'Harmonize' };
            logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Harmonizes/HarmonizesComponent");
            logitudeWindow.WindowClosed.subscribe(s => {

            });
        }
    }
}

class HarmonizeItemClass extends BaseComponent {
    public Id: string = null;
    public EntityPM: ShipmentPackageHarmonizePM;
    public ObjectTableName: string = "ShipmentPackageHarmonize";
    public OldValue: string = null;
    constructor(item: ShipmentPackageHarmonizePM) {
        super();
        this.Id = item.Id;
        this.EntityPM = item;
        this.OldValue = item.Harmonize;
    }

    public get Harmonize() { return this.EntityPM.Harmonize; }
    public set Harmonize(value: string) {
        if (this.EntityPM.Harmonize != value) {
            this.EntityPM.Harmonize = value;
        }
    }
}
