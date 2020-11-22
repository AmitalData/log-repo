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
import { InsideShipmentPackagePM } from '../../../../Shipment/EntityPMs/InsideShipmentPackagePM';

@Component({
    
    templateUrl: './AddEditPackageHarmonizeComponent.html',
})

export class AddEditPackageHarmonizeComponent {
    public ShipmentPM: ShipmentPM = null;
    public EntityPM: ShipmentPackagePM;
    public InsidePackagePM: InsideShipmentPackagePM;
    public ItemsSource: HarmonizeItemClass[] = [];
    public ObjectTableName: string = "ShipmentPackageHarmonize";
    public IsEditingEnabled: boolean = true;
    public ValidationErrorsList: string[];
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {

    }

    private isInsidePackageDirty: boolean = false;
    private isPackageDirty: boolean = false;
    private isShipmentDirty: boolean = false;
    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response:any) => {
            if (args) {
                this.IsEditingEnabled = args['IsEditingEnabled'];
                this.EntityPM = args['PackagePM'];
                this.ShipmentPM = args['ShipmentPM'];
                this.InsidePackagePM = args['InsidePackagePM'];

                this.isPackageDirty = this.EntityPM.IsDirty;
                this.isShipmentDirty = this.ShipmentPM.IsDirty;

                if (this.InsidePackagePM != null) {
                    this.isInsidePackageDirty = this.InsidePackagePM.IsDirty;

                    this.InsidePackagePM.InsidePackageHarmonizes.forEach((item: ShipmentPackageHarmonizePM) => {
                        this.ItemsSource.push(new HarmonizeItemClass(item));
                    });
                }

                else {
                    this.EntityPM.ShipmentPackageHarmonizes.forEach((item: ShipmentPackageHarmonizePM) => {
                        this.ItemsSource.push(new HarmonizeItemClass(item));
                    });
                }

                this.Clone();
            }

            this.IsVisibile = true;
        });
    }

    AddButtonClicked() {
        var item: ShipmentPackageHarmonizePM;

        if (this.InsidePackagePM != null) {
            item = new ShipmentPackageHarmonizePM(this.InsidePackagePM);
        }

        else {
            item = new ShipmentPackageHarmonizePM(this.EntityPM);
        }

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

        if (this.InsidePackagePM != null) {
            this.InsidePackagePM.IsDirty = this.isInsidePackageDirty;
        }

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
            if (this.InsidePackagePM != null) {
                this.FillHarmonizes_InsidePackage();
            }

            else {
                this.FillHarmonizes_ShipmentPackage();
            }

            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    }
    FillHarmonizes_InsidePackage() {
        var allItemsPM: ShipmentPackageHarmonizePM[] = [];

        this.ItemsSource.forEach((item: HarmonizeItemClass) => {
            var index = this.InsidePackagePM.InsidePackageHarmonizes.indexOf(item.EntityPM);

            if (index > -1) {
                var itemPM = this.InsidePackagePM.InsidePackageHarmonizes[index];
                if (itemPM) {
                    if (itemPM.Harmonize != item.Harmonize) {
                        itemPM.Harmonize = item.Harmonize;
                    }
                }
            }

            else {
                this.InsidePackagePM.InsidePackageHarmonizes.push(item.EntityPM);
            }

            allItemsPM.push(item.EntityPM);
        });

        for (var i = this.InsidePackagePM.InsidePackageHarmonizes.length - 1; i >= 0; i--) {

            var index = allItemsPM.indexOf(this.InsidePackagePM.InsidePackageHarmonizes[i]);

            if (index == -1) {
                var item = this.InsidePackagePM.InsidePackageHarmonizes[i];
                this.InsidePackagePM.RemoveShipmentPackageHarmonizePM(item);
            }
        }

        this.InsidePackagePM.IsMultiHarmonize = this.InsidePackagePM.InsidePackageHarmonizes.length > 0 ? true : false;
        if (this.InsidePackagePM.IsMultiHarmonize) {
            if (this.InsidePackagePM.Harmonize) {
                this.InsidePackagePM.Harmonize = null;
            }
        }
    }

    FillHarmonizes_ShipmentPackage() {
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
