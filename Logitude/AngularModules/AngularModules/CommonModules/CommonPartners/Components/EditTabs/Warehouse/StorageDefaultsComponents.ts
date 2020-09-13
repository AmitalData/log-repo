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
    public MaxLineNumber = 0;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: WarehousePM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "Warehouse";
        this.SetUIProperties();
        this.BuildPricingItems();
        this.CopyPricings();
        this.MaxLineNumber = ArrayTool.Max(this.PricingItemsList.Collection, "LineNumber");
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

        this.EntityPM.WarehouseStoragePricings.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 }).forEach(item => {
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

    get StorageFreeDays() { return this.EntityPM.StorageFreeDays; }
    set StorageFreeDays(newValue: number) {
        if (this.EntityPM.StorageFreeDays != newValue) {
            this.EntityPM.StorageFreeDays = newValue;
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
        var previousLine: PricingItem = this.PricingItemsList.Collection.filter(d => d.LineNumber == this.MaxLineNumber)[0];

        var from: number = null;
        if (previousLine != null) {
            if (!AppTool.IsNullOrZero(previousLine.StepTo)) {
                from = previousLine.StepTo + 1;
            }
        }

        this.MaxLineNumber += 1;
        var item: WarehouseStoragePricingPM = new WarehouseStoragePricingPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.WarehouseId = this.EntityPM.Id;
        item.LineNumber = this.MaxLineNumber;
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

        if (this.PricingItemsList.Collection.filter(d => AppTool.IsNullOrZero(d.Days) || (AppTool.IsNullOrZero(d.StepTo))).length == 0) {
            var hasError: boolean = this.ValidateSteps();
            if (hasError) {
                errors.push("Invalid Steps");
            }
        }

        else {
            var hasError: boolean = this.ValidateLastStep();
            if (hasError) {
                errors.push("Only last step can have empty Days or Step To");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.PricingItemsList.Collection.forEach((item: PricingItem) => {
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

        var minLineNumber: number = this.Min(this.PricingItemsList.Collection, "LineNumber");
        this.PricingItemsList.Collection.forEach((item: PricingItem) => {
            if (item != null) {
                var isFirstStep: boolean = item.LineNumber == minLineNumber ? true : false;

                if (!isFirstStep) {
                    var previousLine: PricingItem = this.PricingItemsList.Collection.filter(d => d.StepTo + 1 == item.StepFrom)[0];
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

    private ValidateLastStep() {
        var hasError: boolean = false;
        this.PricingItemsList.Collection.filter(d => d.LineNumber != this.MaxLineNumber).forEach((item: PricingItem) => {
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
        this.ResetPricings();
        this.myCloner.RejectChanges();
    }

    public savedItems: WarehouseStoragePricingPM[] = [];
    public CopyPricings() {
        this.savedItems = [];
        if (this.EntityPM.WarehouseStoragePricings.length > 0) {
            this.EntityPM.WarehouseStoragePricings.forEach(item => {
                this.MaxLineNumber = 0;
                if (item.LineNumber > this.MaxLineNumber) {
                    this.MaxLineNumber = item.LineNumber;
                }
                var pricingItem = new WarehouseStoragePricingPM(null);
                pricingItem.Tenant = item.Tenant;
                pricingItem.WarehouseId = item.WarehouseId;
                pricingItem.LineNumber = item.LineNumber;
                pricingItem.StepFrom = item.StepFrom;
                pricingItem.StepTo = item.StepTo;
                pricingItem.Days = item.Days;
                pricingItem.SalePrice = item.SalePrice;
                this.savedItems.push(pricingItem);
            });
        }
    }

    public ResetPricings() {
        if (this.savedItems != null) {
            var items: WarehouseStoragePricingPM[] = this.EntityPM.WarehouseStoragePricings;
            items.forEach(item => {
                var savedItem: WarehouseStoragePricingPM = this.savedItems.filter(d => d.LineNumber == item.LineNumber)[0];
                if (savedItem == null) {
                    if (this.EntityPM.WarehouseStoragePricings.indexOf(item) != -1) {
                        this.EntityPM.RemoveWarehouseStoragePricingPM(item);
                    }
                }

                else {
                    item.StepFrom = savedItem.StepFrom;
                    item.StepTo = savedItem.StepTo;
                    item.Days = savedItem.Days;
                    item.SalePrice = savedItem.SalePrice;
                }
            });

            this.savedItems.forEach(item => {
                var list = this.EntityPM.WarehouseStoragePricings.filter(d => d.LineNumber == item.LineNumber);
                if (list == null) {
                    this.EntityPM.WarehouseStoragePricings.push(item);
                }
            });
        }
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

                this.fatherComponent.MaxLineNumber = ArrayTool.Max(this.fatherComponent.PricingItemsList.Collection, "LineNumber");
            }
        });
    }
}
