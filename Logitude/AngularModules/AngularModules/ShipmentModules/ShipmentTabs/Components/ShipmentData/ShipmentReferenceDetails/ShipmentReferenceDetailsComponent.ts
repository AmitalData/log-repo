import {Component}  from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ReferenceTypePM } from 'Shipment/EntityPMs/ReferenceTypePM';
import { ShipmentPM } from 'Shipment/EntityPMs/ShipmentPM';
import { ShipmentReferancePM } from 'Shipment/EntityPMs/ShipmentReferancePM';
import { ReferenceTypeListService } from 'Shipment/Services/StandardLists/ReferenceTypeListService';

@Component({
    templateUrl: './ShipmentReferenceDetailsComponent.html',
    selector :'ShipmentReferenceDetailsComponent',
})
export class ShipmentReferenceDetailsComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public DataContext: any = this;
    public ObjectTableName: string;
    public IsDisplayOnly: boolean = false;
    public ShipmentReferances: ShipmentReferancePM[];
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public referenceTypeListService: ReferenceTypeListService = new ReferenceTypeListService();
    public IsResourcesReady: boolean = false;
    public referenceTypesTranslations = {};

    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.ObjectTableName = "ShipmentReferance"; 

        this.entityResourceService.getEntityResourceByTableName("ShipmentReferance").subscribe((response: any) => {
            this.referenceTypeListService.getAll().subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    const referenceTypePMList: ReferenceTypePM[] = response.Result;
                    referenceTypePMList.forEach(referenceTypePM => this.referenceTypesTranslations[referenceTypePM.Code] = referenceTypePM.LocalName);
                }
                
                this.IsResourcesReady = true;
            });
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;

            // clone the ShipmentReferances from entity
            this.ShipmentReferances = [];
            for (var i = 0; i < this.EntityPM.ShipmentReferances.length; i++) {
                if (this.EntityPM.ShipmentReferances[i].ChangeSetOp != "Delete") {
                    this.EntityPM.ShipmentReferances[i].CloneMe();
                    this.ShipmentReferances.push(this.EntityPM.ShipmentReferances[i].MyClone);
                }
            }

            if (this.ShipmentReferances.length == 0) {
                // setTimeout(() => this.Add(), 1000);
                // this.Add();
            }
            else {
                this.BuildItemsList();
            }
        }
    }

    Add() {
        var line: number = 0;
        var sequence: number = 0;
        if (this.ShipmentReferances.length > 0) {
            line = this.ShipmentReferances.length + 1;
        }
        else {
            line = 0;
        }

        line += 1;
        sequence += 1;

        var item: ShipmentReferancePM = new ShipmentReferancePM();
        item.ShipmentId = this.EntityPM.Id;
        item.Tenant = this.EntityPM.Tenant;
        // item.LineNumber = line;
        item.ChangeSetOp = "Insert";

        if (!this.ShipmentReferances.includes(item)) {
             this.AddShipmentReferance(item);
             this.ItemsSource.Insert(new ShipmentReferenceLine(item, this));
        }
    }

    public AddShipmentReferance(item: ShipmentReferancePM) {
        if (item != null) {
            var index = this.ShipmentReferances.indexOf(item);
            if (index == -1) {
                // item.EntityParentPM = this;
                this.ShipmentReferances.push(item);
            }
        }
    }
    public RemoveShipmentItem(item: ShipmentReferancePM) {
        if (item != null) {
            var index = this.ShipmentReferances.indexOf(item);
            if (index > -1) {

                if (item.ChangeSetOp == "Insert") {
                    this.ShipmentReferances.splice(index, 1);
                }
                else {
                    this.ShipmentReferances[index].ChangeSetOp = "Delete";
                }
            }
        }
    }

    BuildItemsList() {
        if (this.ItemsSource != null) {
            this.ItemsSource.Clear();
        }
        var TempItemSource: ShipmentReferenceLine[] = [];
        for (var i = 0; i < this.ShipmentReferances.length; i++) {
            TempItemSource.push(new ShipmentReferenceLine(this.ShipmentReferances[i], this));
        }
        if (this.ItemsSource != null) {
            this.ItemsSource.InsertCollection(TempItemSource);
        }
    }

    //#region Remark tooltip
    onCellSelected($event, Item: ShipmentReferenceLine) {
    }

    public ItemsSource: ObservableCollection;

    OnRowEnded($event) {
        if (($event) == this.ItemsSource.Length) {
            //setTimeout(() => this.Add(), 1);
            this.Add();

        }
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit(null);
    }

    Validate() {
        var errors: string[] = [];

        let emptyReferenceType = false;
        let emptyReferenceValue = false;

        for (var i = 0; i < this.ShipmentReferances.length; i++) {
            let shipmentReferance = this.ShipmentReferances[i];

            if (shipmentReferance.ChangeSetOp != "Delete") {
                if (AppTool.IsNullOrEmpty(shipmentReferance.ReferenceType) && !AppTool.IsNullOrEmpty(shipmentReferance.ReferenceValue)) {
                    emptyReferenceType = true;
                }
                else if (!AppTool.IsNullOrEmpty(shipmentReferance.ReferenceType) && AppTool.IsNullOrEmpty(shipmentReferance.ReferenceValue)) {
                    emptyReferenceValue = true;
                }
            }
            if (AppTool.IsNullOrEmpty(shipmentReferance.ReferenceType) && AppTool.IsNullOrEmpty(shipmentReferance.ReferenceValue)
                && shipmentReferance.ChangeSetOp == "Update") {
                emptyReferenceType = true;
                emptyReferenceValue = true;
            }
        }

        if (emptyReferenceType && emptyReferenceValue) {
            errors.push("אין להשאיר שורה ריקה אלא למחוק אותה");
        }
        else if (emptyReferenceType) {
            errors.push(TextCodeTranslator.Translate("ShipmentReferance.O.MissingReferenceType"));
        }
        else if (emptyReferenceValue) {
            errors.push(TextCodeTranslator.Translate("ShipmentReferance.O.MissingReferenceValue"));
        }
        // if (emptyReferenceType || emptyReferenceValue) {
        //     errors.push("Reference type / value can not be empty");
        // }
        this.ValidationErrorsList = errors;
    }

    public ValidationErrorsList: string[] = [];

    OkButtonClicked() {

        this.Validate();

        if (this.ValidationErrorsList.length == 0) {

            let shipmentReferances = this.ShipmentReferances;
            const lineNumbers = this.ShipmentReferances.map(item => item.LineNumber);

            for (var i = 0; i < shipmentReferances.length; i++) {

                let shipmentReferance = shipmentReferances[i];

                if (shipmentReferance.ChangeSetOp == "Insert") {

                    // remove rows that have been added but with empty values
                    if (AppTool.IsNullOrEmpty(shipmentReferance.ReferenceType) && AppTool.IsNullOrEmpty(shipmentReferance.ReferenceValue)) {
                        this.ShipmentReferances.splice(i, 1);
                    }
                    else if (!this.ShipmentReferances[i].LineNumber) {
                        this.ShipmentReferances[i].LineNumber = this.GetNextLineNumber(lineNumbers);
                    }
                }
            }

            console.log("SAVE", this.ShipmentReferances);

            // set the changes on the entity
            this.EntityPM.ShipmentReferances = this.ShipmentReferances;

            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        }
    }

    GetNextLineNumber(lineNumbers) {
        // const lineNumbers = this.ShipmentReferances.map(item => item.LineNumber);
        let lineNumber = 1;
        while (lineNumbers.indexOf(lineNumber) > -1) {
            lineNumber++;
        }
        console.log("GetNextLineNumber", lineNumbers, lineNumber)
        return lineNumber;
    }
}


export class ShipmentReferenceLine extends BaseComponent {
    public entityPM: ShipmentReferancePM = null;
    public DataContext = this;
    Parent: ShipmentReferenceDetailsComponent;

    public ShowClassifierRemarkTooltip: boolean = false;
    public ShowValidatioIcon: boolean = false;
    public closedManullay: boolean = false;

    constructor(EntityPM: ShipmentReferancePM, parent: ShipmentReferenceDetailsComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }

    //#region Properties
    public get ReferenceValue() { return this.entityPM.ReferenceValue; }
    public set ReferenceValue(newValue: string) { 
        if (this.entityPM.ReferenceValue != newValue) {
            this.entityPM.ReferenceValue = newValue;
            if (!this.entityPM.ChangeSetOp) {
                this.entityPM.ChangeSetOp = "Update";
            }
            // newValue && this.Parent.Validate();
        }
    }

    public get ReferenceType() { return this.Parent.referenceTypesTranslations[this.entityPM.ReferenceType] || this.entityPM.ReferenceType; }
    public set ReferenceType(newValue: string) { 
        if (this.entityPM.ReferenceType != newValue && typeof(newValue) == "string") {
            this.entityPM.ReferenceType = newValue; 
            if (!this.entityPM.ChangeSetOp) {
                this.entityPM.ChangeSetOp = "Update";
            }
            // newValue && this.Parent.Validate();
        }
    }

    DeleteButtonClicked(item: ShipmentReferenceLine) {
        if (this.Parent.ShipmentReferances.includes(this.entityPM)) {
            this.Parent.RemoveShipmentItem(this.entityPM);
        }
        this.Parent.ItemsSource.Remove(item); 
    }
}
