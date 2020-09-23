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
import { ShipmentTool } from '../../../../Shipment/Tools';

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
    public MaxLineNumber = 0;
    public IsResourcesReady: boolean = false;
    public PricesChanged: boolean = false;
    public WeightLabel: string;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];

        this.BuildPricingItems();
        this.CopyPricings();
        this.MaxLineNumber = ArrayTool.Max(this.PricingItemsList.Collection, "LineNumber");
        this.ComputeWeightLabel();
        this.Clone();
    }

    BuildPricingItems() {
        var itemsCollection: PricingItem[] = [];

        var freeItem: ShipmentStoragePricingPM = new ShipmentStoragePricingPM(null);
        freeItem.Days = this.EntityPM.WarehouseStorageFreeDays;
        freeItem.SalePrice = 0;
        freeItem.LineNumber = 0;

        itemsCollection.push(new PricingItem(freeItem, this, true, false));

        this.EntityPM.ShipmentStoragePricings.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 }).forEach(item => {
            itemsCollection.push(new PricingItem(item, this, false, false));
        });

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

    private ComputeWeightLabel() {
        var weight: number = ShipmentTool.ComputeStorageWeight(this.EntityPM);

        if (weight == null) {
            weight = 0;
        }

        this.WeightLabel = "Weight = " + weight;
    }

    get ChargeStorageCurrencyId() { return this.EntityPM.ChargeStorageCurrencyId; }
    set ChargeStorageCurrencyId(newValue: string) {
        if (this.EntityPM.ChargeStorageCurrencyId != newValue) {
            this.EntityPM.ChargeStorageCurrencyId = newValue;
            this.PricesChanged = true;
        }
    }

    get WeightMeasurementCode() { return this.EntityPM.WeightMeasurementCode; }
    set WeightMeasurementCode(newValue: string) {
        if (this.EntityPM.WeightMeasurementCode != newValue) {
            this.EntityPM.WeightMeasurementCode = newValue;
            this.PricesChanged = true;

            this.ComputeWeightLabel();
        }
    }

    get WeightRoundingCode() { return this.EntityPM.WeightRoundingCode; }
    set WeightRoundingCode(newValue: string) {
        if (this.EntityPM.WeightRoundingCode != newValue) {
            this.EntityPM.WeightRoundingCode = newValue;
            this.PricesChanged = true;

            this.ComputeWeightLabel();
        }
    }

    OnRowEnded($event) {
        if (($event) == this.PricingItemsList.Length) {
            this.AddPricingItemMethod();
        }
    }

    AddPricingItemMethod() {
        var previousLine: PricingItem = this.PricingItemsList.Collection.filter(d => !d.IsFreeLine && d.LineNumber == this.MaxLineNumber)[0];

        var from: number = null;
        if (previousLine != null) {
            if (!AppTool.IsNullOrZero(previousLine.StepTo)) {
                from = previousLine.StepTo + 1;
            }
        }

        this.MaxLineNumber += 1;
        var item: ShipmentStoragePricingPM = new ShipmentStoragePricingPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        item.WarehouseId = this.EntityPM.WarehouseLegWarehouseId;
        item.LineNumber = this.MaxLineNumber;
        item.StepFrom = from;
        this.PricingItemsList.Insert(new PricingItem(item, this, false, true));
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        var myItems: PricingItem[] = this.PricingItemsList.Collection.filter(d => !d.IsFreeLine);

        myItems.forEach((item: PricingItem) => {
            if (item != null) {
                Validator.TryValidateObject(item.EntityPM, "ShipmentStoragePricing", errors);
            }
        });

        if (myItems.filter(d => AppTool.IsNullOrZero(d.Days) || (AppTool.IsNullOrZero(d.StepTo))).length == 0) {
            var hasError: boolean = this.ValidateSteps(myItems);
            if (hasError) {
                errors.push("Invalid Steps");
            }
        }

        else {
            var hasError: boolean = this.ValidateLastStep(myItems);
            if (hasError) {
                errors.push("Only last step can have empty Days or Step To");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            myItems.forEach((item: PricingItem) => {
                if (item != null) {
                    if (item.IsNewEntity) {
                        if (this.EntityPM.ShipmentStoragePricings.indexOf(item.EntityPM) == -1) {
                            item.IsNewEntity = false;
                            this.EntityPM.AddShipmentStoragePricing(item.EntityPM);
                        }
                    }
                }
            });

            var emitMessage: string = "ok";
            if (this.PricesChanged) {
                emitMessage = "PricesChanged";
            }

            this.CurrentSession.CloseCurrentWindowEmit(emitMessage);
        }
    }

    private ValidateSteps(myItems: PricingItem[]) {
        var hasError: boolean = false;

        var minLineNumber: number = this.Min(myItems, "LineNumber");
        myItems.forEach((item: PricingItem) => {
            if (item != null) {
                var isFirstStep: boolean = item.LineNumber == minLineNumber ? true : false;

                if (!isFirstStep) {
                    var previousLine: PricingItem = myItems.filter(d => d.StepTo + 1 == item.StepFrom)[0];
                    if (previousLine != null) {
                        if (item.StepFrom != previousLine.StepTo + 1) {
                            hasError = true;
                        }
                    }

                    else {
                        hasError = true;
                    }
                }
            }
        });


        return hasError;
    }

    private ValidateLastStep(myItems: PricingItem[]) {
        var hasError: boolean = false;
        myItems.filter(d => d.LineNumber != this.MaxLineNumber).forEach((item: PricingItem) => {
            if (AppTool.IsNullOrZero(item.Days) || AppTool.IsNullOrZero(item.StepTo)) {
                hasError = true;
            }
        });
        
        return hasError;
    }

    public Min(array: any[], fieldname: string): number {
        var myResult: number = 1;

        if (array && fieldname) {
            array.forEach(item => {
                var itemValue = item[fieldname];

                if (!AppTool.IsNullOrEmpty(itemValue)) {
                    if (typeof (itemValue) == "number") {
                        if (myResult > itemValue) {
                            myResult = itemValue;
                        }
                    }
                }
            });
        }

        if (AppTool.IsNullOrEmpty(myResult)) {
            myResult = 0;
        }

        return myResult;
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this);
        this.myCloner.AddField('ChargeStorageCurrencyId');
        this.myCloner.AddField('WeightMeasurementCode');
        this.myCloner.AddField('WeightRoundingCode');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.ResetPricings();
        this.myCloner.RejectChanges();
    }

    public savedItems: ShipmentStoragePricingPM[] = [];
    public CopyPricings() {
        this.savedItems = [];
        if (this.EntityPM.ShipmentStoragePricings.length > 0) {
            this.EntityPM.ShipmentStoragePricings.forEach(item => {
                this.MaxLineNumber = 0;
                if (item.LineNumber > this.MaxLineNumber) {
                    this.MaxLineNumber = item.LineNumber;
                }
                var pricingItem = new ShipmentStoragePricingPM(null);
                pricingItem.ShipmentId = item.ShipmentId;
                pricingItem.Tenant = item.Tenant;
                pricingItem.WarehouseId = item.WarehouseId;
                pricingItem.LineNumber = item.LineNumber;
                pricingItem.StepFrom = item.StepFrom;
                pricingItem.StepTo = item.StepTo;
                pricingItem.Days = item.Days;
                pricingItem.SalePrice = item.SalePrice;
                pricingItem.Amount = item.Amount;
                this.savedItems.push(pricingItem);
            });
        }
    }

    public ResetPricings() {
        if (this.savedItems != null) {
            var items: ShipmentStoragePricingPM[] = this.EntityPM.ShipmentStoragePricings;
            items.forEach(item => {
                var savedItem: ShipmentStoragePricingPM = this.savedItems.filter(d => d.LineNumber == item.LineNumber)[0];
                if (savedItem == null) {
                    if (this.EntityPM.ShipmentStoragePricings.indexOf(item) != -1) {
                        this.EntityPM.RemoveShipmentStoragePricing(item);
                    }
                }

                else {
                    item.StepFrom = savedItem.StepFrom;
                    item.StepTo = savedItem.StepTo;
                    item.Days = savedItem.Days;
                    item.SalePrice = savedItem.SalePrice;
                    item.Amount = savedItem.Amount;
                }
            });

            this.savedItems.forEach(item => {
                var list = this.EntityPM.ShipmentStoragePricings.filter(d => d.LineNumber == item.LineNumber);
                if (list == null) {
                    this.EntityPM.ShipmentStoragePricings.push(item);
                }
            });
        }
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

        this.SetCellColor();
    }

    public CellColor: string = "transparent";
    private SetCellColor() {
        if (this.IsFreeLine) {
            this.CellColor = "#DFF9EB";
        }

        else {
            this.CellColor = "transparent";
        }
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
            this.EntityPM.StepFrom = AppTool.Round(newValue, 0);
            this.fatherComponent.PricesChanged = true;

            this.ComputeStepTo();
            this.ComputeDays();
        }
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

            this.fatherComponent.PricesChanged = true;
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

            this.fatherComponent.PricesChanged = true;
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
            this.fatherComponent.PricesChanged = true;
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

    get ChargeableDays() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.ChargeableDays;
        }
        return myResult;
    }
    set ChargeableDays(newValue: number) {
        if (this.EntityPM.ChargeableDays != newValue) {
            this.EntityPM.ChargeableDays = newValue;
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
        if (this.StepFrom != null && this.StepTo != null) {
            this.EntityPM.Days = (this.StepTo - this.StepFrom) + 1;
        }

        else {
            this.EntityPM.Days = null;
        }
    }
    private ComputeStepTo() {
        if (this.StepFrom != null && this.Days != null) {
            this.EntityPM.StepTo = (this.StepFrom + this.Days) - 1;
        }

        else {
            this.EntityPM.StepTo = null;
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

                this.fatherComponent.MaxLineNumber = ArrayTool.Max(this.fatherComponent.PricingItemsList.Collection, "LineNumber");
            }
        });
    }
}

