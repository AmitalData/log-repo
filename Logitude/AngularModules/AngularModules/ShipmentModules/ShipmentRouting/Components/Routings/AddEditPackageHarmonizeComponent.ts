import { Component } from '@angular/core';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { PickUpDeliveryPackageHarmonizePM } from '../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';
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
    public EntityPM: ShipmentPickUpDeliveryPackagePM;
    public ItemsSource: HarmonizeItemClass[] = [];
    public ObjectTableName: string = "PickUpDeliveryPackageHarmonize";
    public IsEditingEnabled: boolean = true;
    public ValidationErrorsList: string[];
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {

    }

    private isPackageDirty: boolean = false;
    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            if (args) {
                this.IsEditingEnabled = args['IsEditingEnabled'];
                this.EntityPM = args['PackagePM'];
                this.isPackageDirty = this.EntityPM.IsDirty;
                this.EntityPM.PickUpDeliveryPackageHarmonizes.forEach((item: PickUpDeliveryPackageHarmonizePM) => {
                    this.ItemsSource.push(new HarmonizeItemClass(item));
                });
                this.Clone();
            }
            this.IsVisibile = true;
        });
    }
    AddButtonClicked() {
        var item: PickUpDeliveryPackageHarmonizePM = new PickUpDeliveryPackageHarmonizePM(this.EntityPM);
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
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        this.ItemsSource.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, this.ObjectTableName, errors);
        });

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var allItemsPM: PickUpDeliveryPackageHarmonizePM[] = [];

            this.ItemsSource.forEach((item: HarmonizeItemClass) => {
                var index = this.EntityPM.PickUpDeliveryPackageHarmonizes.indexOf(item.EntityPM);

                if (index > -1) {
                    var itemPM = this.EntityPM.PickUpDeliveryPackageHarmonizes[index];
                    if (itemPM) {
                        if (itemPM.Harmonize != item.Harmonize) {
                            itemPM.Harmonize = item.Harmonize;
                        }
                    }
                }

                else {
                    this.EntityPM.PickUpDeliveryPackageHarmonizes.push(item.EntityPM);
                }

                allItemsPM.push(item.EntityPM);
            });

            for (var i = this.EntityPM.PickUpDeliveryPackageHarmonizes.length - 1; i >= 0; i--) {

                var index = allItemsPM.indexOf(this.EntityPM.PickUpDeliveryPackageHarmonizes[i]);

                if (index == -1) {
                    var item = this.EntityPM.PickUpDeliveryPackageHarmonizes[i];
                    this.EntityPM.RemovePickUpDeliveryPackageHarmonizePM(item);
                }
            }

            this.EntityPM.IsMultiHarmonize = this.EntityPM.PickUpDeliveryPackageHarmonizes.length > 0 ? true : false;
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
    public EntityPM: PickUpDeliveryPackageHarmonizePM;
    public ObjectTableName: string = "PickUpDeliveryPackageHarmonize";
    public OldValue: string = null;
    constructor(item: PickUpDeliveryPackageHarmonizePM) {
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
