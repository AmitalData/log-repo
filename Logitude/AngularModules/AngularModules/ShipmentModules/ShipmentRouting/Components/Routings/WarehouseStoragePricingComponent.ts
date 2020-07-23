import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { WarehouseStoragePricingPM } from '../../../../Common/EntityPMs/WarehouseStoragePricingPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentStoragePricingPM } from '../../../../Shipment/EntityPMs/ShipmentStoragePricingPM';

@Component({
    templateUrl: './WarehouseStoragePricingComponent.html',
})

export class WarehouseStoragePricingComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext: WarehouseStoragePricingComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public PricingItemsList: ObservableCollection;
    private maxPackageItemsLineNumber = 0;
    constructor() {
        super();
    }

    private DefaultPricings: WarehouseStoragePricingPM[];
    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.DefaultPricings = args['DefaultPricings'];

        this.BuildPricingItems();
        this.maxPackageItemsLineNumber = ArrayTool.Max(this.PricingItemsList.Collection, "LineNumber");       
        this.Clone();
    }

    BuildPricingItems() {
        var itemsCollection: PricingItem[] = [];

        var freeItem: ShipmentStoragePricingPM = new ShipmentStoragePricingPM(null);
        freeItem.Days = this.EntityPM.WarehouseStorageFreeDays;
        freeItem.SalePrice = 0;
        freeItem.LineNumber = 0;

        itemsCollection.push(new PricingItem(freeItem, this, true, false));

        if (this.DefaultPricings != null && this.DefaultPricings.length > 0) {
            var count: number = 1;
            this.DefaultPricings.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 }).forEach(item => {                
                var defaultItem: ShipmentStoragePricingPM = new ShipmentStoragePricingPM(null);
                defaultItem.Tenant = SessionLocator.Tenant;
                defaultItem.ShipmentId = this.EntityPM.Id;
                defaultItem.WarehouseId = this.EntityPM.WarehouseLegWarehouseId;                
                defaultItem.StepFrom = item.StepFrom;
                defaultItem.StepTo = item.StepTo;
                defaultItem.Days = item.Days;
                defaultItem.SalePrice = item.SalePrice;
                defaultItem.LineNumber = count++;

                itemsCollection.push(new PricingItem(defaultItem, this, false, true));
            });
        }

        else {
            this.EntityPM.ShipmentStoragePricings.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 }).forEach(item => {
                itemsCollection.push(new PricingItem(item, this, false, false));
            });
        }

        if (this.PricingItemsList == null) {
            this.PricingItemsList = new ObservableCollection([]);
        }

        else {
            this.PricingItemsList.Collection.forEach(item => {
                this.PricingItemsList.Clear();
            });
        }

        this.PricingItemsList.InsertCollection(itemsCollection);
    }

    get ChargeStorageCurrencyId() { return this.EntityPM.ChargeStorageCurrencyId; }
    set ChargeStorageCurrencyId(newValue: string) {
        if (this.EntityPM.ChargeStorageCurrencyId != newValue) {
            this.EntityPM.ChargeStorageCurrencyId = newValue;
        }
    }

    get WeightMeasurementCode() { return this.EntityPM.WeightMeasurementCode; }
    set WeightMeasurementCode(newValue: string) {
        if (this.EntityPM.WeightMeasurementCode != newValue) {
            this.EntityPM.WeightMeasurementCode = newValue;
        }
    }

    get WeightRoundingCode() { return this.EntityPM.WeightRoundingCode; }
    set WeightRoundingCode(newValue: string) {
        if (this.EntityPM.WeightRoundingCode != newValue) {
            this.EntityPM.WeightRoundingCode = newValue;
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
        var item: ShipmentStoragePricingPM = new ShipmentStoragePricingPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        item.WarehouseId = this.EntityPM.WarehouseLegWarehouseId;
        item.LineNumber = this.maxPackageItemsLineNumber;
        item.StepFrom = from;
        this.PricingItemsList.Insert(new PricingItem(item, this, false, true));
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
                    if (item.IsNewEntity && !item.IsFreeLine) {
                        if (this.EntityPM.ShipmentStoragePricings.indexOf(item.EntityPM) == -1) {
                            item.IsNewEntity = false;
                            this.EntityPM.AddShipmentStoragePricing(item.EntityPM);
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
    public EntityPM: ShipmentStoragePricingPM;
    public ObjectTableName: string = "ShipmentStoragePricing";
    public IsNewEntity: boolean = false;
    public IsFreeLine: boolean = false;
    constructor(entity: ShipmentStoragePricingPM, public fatherComponent: WarehouseStoragePricingComponent, isFree: boolean, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.IsFreeLine = isFree;
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
        if (this.IsFreeLine) {
            myResult = "Free Days";
        }

        else {
            if (this.StepFrom != null) {
                myResult = "+ " + this.EntityPM.StepFrom + " days";
            }
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

    get Amount() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.Amount;
        }
        return myResult;
    }
    set Amount(newValue: number) {
        if (this.EntityPM.Amount != newValue) {
            this.EntityPM.Amount = newValue;
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

    RemoveLine() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this item ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.fatherComponent.EntityPM.ShipmentStoragePricings.indexOf(this.EntityPM) != -1) {
                    this.fatherComponent.EntityPM.RemoveShipmentStoragePricing(this.EntityPM);
                }

                if (this.fatherComponent.PricingItemsList.Collection.indexOf(this) != -1) {
                    this.fatherComponent.PricingItemsList.Remove(this);
                }
            }
        });
    }
}

