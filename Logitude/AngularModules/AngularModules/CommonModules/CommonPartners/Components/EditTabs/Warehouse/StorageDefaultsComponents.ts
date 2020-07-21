import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { WarehousePM } from '../../../../../Common/EntityPMs/WarehousePM';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { Cloner } from '../../../../../Infrastructure/Utilities/Cloner';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { WarehouseStoragePricingPM } from '../../../../../Common/EntityPMs/WarehouseStoragePricingPM';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';

@Component({
    templateUrl: './StorageDefaultsComponents.html',
})

export class StorageDefaultsComponents extends BaseComponent {
    public EntityPM: WarehousePM;
    public ObjectTableName: string;
    public DataContext: StorageDefaultsComponents = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public PricingItemsList: ObservableCollection;
    private maxPackageItemsLineNumber = 0;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: WarehousePM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "Warehouse";
        this.SetUIProperties();
        this.BuildPricingItems();
        this.maxPackageItemsLineNumber = ArrayTool.Max(this.PricingItemsList.Collection, "LineNumber");
        this.Clone();
    }

    SetUIProperties() {
       
    }

    BuildPricingItems() {
        if (this.PricingItemsList == null) {
            this.PricingItemsList = new ObservableCollection([]);
        }
        else {
            this.PricingItemsList.Collection.forEach(item => {
                this.PricingItemsList.Clear();
            });
        }

        var itemsCollection: PricingItem[] = [];

        this.EntityPM.WarehouseStoragePricings.forEach(item => {
            itemsCollection.push(new PricingItem(item, this, false));
        });

        this.PricingItemsList.InsertCollection(itemsCollection);
    }

    get ChargeStorage() { return this.EntityPM.ChargeStorage; }
    set ChargeStorage(newValue: boolean) {
        if (this.EntityPM.ChargeStorage != newValue) {
            this.EntityPM.ChargeStorage = newValue;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(newValue: string) {
        if (this.EntityPM.CurrencyId != newValue) {
            this.EntityPM.CurrencyId = newValue;
        }
    }

    get AirWeightMeasurementCode() { return this.EntityPM.AirWeightMeasurementCode; }
    set AirWeightMeasurementCode(newValue: string) {
        if (this.EntityPM.AirWeightMeasurementCode != newValue) {
            this.EntityPM.AirWeightMeasurementCode = newValue;
        }
    }

    get OceanWeightMeasurementCode() { return this.EntityPM.OceanWeightMeasurementCode; }
    set OceanWeightMeasurementCode(newValue: string) {
        if (this.EntityPM.OceanWeightMeasurementCode != newValue) {
            this.EntityPM.OceanWeightMeasurementCode = newValue;
        }
    }

    get InlandWeightMeasurementCode() { return this.EntityPM.InlandWeightMeasurementCode; }
    set InlandWeightMeasurementCode(newValue: string) {
        if (this.EntityPM.InlandWeightMeasurementCode != newValue) {
            this.EntityPM.InlandWeightMeasurementCode = newValue;
        }
    }

    get AirWeightRoundingCode() { return this.EntityPM.AirWeightRoundingCode; }
    set AirWeightRoundingCode(newValue: string) {
        if (this.EntityPM.AirWeightRoundingCode != newValue) {
            this.EntityPM.AirWeightRoundingCode = newValue;
        }
    }

    get OceanWeightRoundingCode() { return this.EntityPM.OceanWeightRoundingCode; }
    set OceanWeightRoundingCode(newValue: string) {
        if (this.EntityPM.OceanWeightRoundingCode != newValue) {
            this.EntityPM.OceanWeightRoundingCode = newValue;
        }
    }

    get InlandWeightRoundingCode() { return this.EntityPM.InlandWeightRoundingCode; }
    set InlandWeightRoundingCode(newValue: string) {
        if (this.EntityPM.InlandWeightRoundingCode != newValue) {
            this.EntityPM.InlandWeightRoundingCode = newValue;
        }
    }

    OnRowEnded($event) {
        if (($event) == this.PricingItemsList.Length) {
            this.AddPricingItemMethod();
        }
    }

    AddPricingItemMethod() {
        var previousLine: PricingItem = this.PricingItemsList.Collection.filter(d => d.LineNumber == this.maxPackageItemsLineNumber)[0];

        var from: number = null;
        if (previousLine != null) {
            from = previousLine.StepTo;
        }

        this.maxPackageItemsLineNumber += 1;
        var item: WarehouseStoragePricingPM = new WarehouseStoragePricingPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.WarehouseId = this.EntityPM.Id;
        item.LineNumber = this.maxPackageItemsLineNumber;
        item.StepFrom = from;
        this.PricingItemsList.Insert(new PricingItem(item, this, true));
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        this.PricingItemsList.Collection.forEach((item: PricingItem) => {
            if (item != null) {
                Validator.TryValidateObject(item.EntityPM, "WarehouseStoragePricing", errors);
            }
        });

        var hasError: boolean = this.ValidateSteps();
        if (hasError) {
            errors.push("Invalid Steps");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.DataContext.PricingItemsList.Collection.forEach((item: PricingItem) => {
                if (item != null) {
                    if (item.IsNewEntity) {
                        if (this.DataContext.EntityPM.WarehouseStoragePricings.indexOf(item.EntityPM) == -1) {
                            item.IsNewEntity = false;
                            this.DataContext.EntityPM.AddWarehouseStoragePricingPM(item.EntityPM);
                        }
                    }
                }
            });

            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private ValidateSteps() {
        var hasError: boolean = false;

        this.DataContext.PricingItemsList.Collection.forEach((item: PricingItem) => {
            if (item != null) {
                var previousLine: PricingItem = this.PricingItemsList.Collection.filter(d => d.LineNumber == item.LineNumber - 1)[0];
                if (previousLine != null) {
                    if (item.StepFrom != previousLine.StepTo) {
                        hasError = true;
                    }
                }
            }
        });


        return hasError;
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ChargeStorage');
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddField('AirWeightMeasurementCode');
        this.myCloner.AddField('OceanWeightMeasurementCode');
        this.myCloner.AddField('InlandWeightMeasurementCode');
        this.myCloner.AddField('AirWeightRoundingCode');
        this.myCloner.AddField('OceanWeightRoundingCode');
        this.myCloner.AddField('InlandWeightRoundingCode');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

export class PricingItem extends BaseComponent {
    public EntityPM: WarehouseStoragePricingPM;
    public ObjectTableName: string = "WarehouseStoragePricing";
    public IsNewEntity: boolean = false;

    constructor(entity: WarehouseStoragePricingPM, public fatherComponent: StorageDefaultsComponents, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
    }

    get StepFrom() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.StepFrom;
        }
        return myResult;
    }
    set StepFrom(newValue: number) {
        if (this.EntityPM.StepFrom != newValue) {
            this.EntityPM.StepFrom = newValue;
        }
    }

    get StepFromText() {
        var myResult = null;
        if (this.StepFrom != null) {
            myResult = "+ " + this.EntityPM.StepFrom + " days";
        }
        return myResult;
    }

    get StepTo() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.StepTo;
        }
        return myResult;
    }
    set StepTo(newValue: number) {
        if (this.EntityPM.StepTo != newValue) {
            this.EntityPM.StepTo = newValue;

            this.ComputeDays();
        }
    }

    get Days() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.Days;
        }
        return myResult;
    }
    set Days(newValue: number) {
        if (this.EntityPM.Days != newValue) {
            this.EntityPM.Days = newValue;

            this.ComputeStepTo();
        }
    }

    get SalePrice() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.SalePrice;
        }
        return myResult;
    }
    set SalePrice(newValue: number) {
        if (this.EntityPM.SalePrice != newValue) {
            this.EntityPM.SalePrice = newValue;
        }
    }

    get LineNumber() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.LineNumber;
        }
        return myResult;
    }
    set LineNumber(newValue: number) {
        if (this.EntityPM.LineNumber != newValue) {
            this.EntityPM.LineNumber = newValue;
        }
    }

    private ComputeDays() {
        if (this.StepFrom != null && !AppTool.IsNullOrZero(this.StepTo)) {
            this.Days = this.StepTo - this.StepFrom;
        }

        else {
            this.Days = null;
        }
    }
    private ComputeStepTo() {
        if (this.StepFrom != null && !AppTool.IsNullOrZero(this.Days)) {
            this.StepTo = this.StepFrom + this.Days;
        }

        else {
            this.StepTo = null;
        }
    }

    RemoveLine(item) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this item ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.fatherComponent.EntityPM.WarehouseStoragePricings.indexOf(this.EntityPM) != -1) {
                    this.fatherComponent.EntityPM.RemoveWarehouseStoragePricingPM(this.EntityPM);
                }

                if (this.fatherComponent.PricingItemsList.Collection.indexOf(this) != -1) {
                    this.fatherComponent.PricingItemsList.Remove(this);
                }
            }
        });
    }
}
