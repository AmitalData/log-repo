import {Component}  from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { FreightForwarderReferencePM } from 'Shipment/EntityPMs/FreightForwarderReferencePM';
import { ReferenceTypePM } from 'Shipment/EntityPMs/ReferenceTypePM';
import { ShipmentPM } from 'Shipment/EntityPMs/ShipmentPM';
import { ReferenceTypeListService } from 'Shipment/Services/StandardLists/ReferenceTypeListService';

@Component({
    templateUrl: './FreightForwarderReferenceDetailsComponent.html',
    selector :'FreightForwarderReferenceDetailsComponent',
})
export class FreightForwarderReferenceDetailsComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public DataContext: any = this;
    public ObjectTableName: string;
    public IsDisplayOnly: boolean = false;
    public FreightForwarderReferences: FreightForwarderReferencePM[];
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public referenceTypeListService: ReferenceTypeListService = new ReferenceTypeListService();
    public IsResourcesReady: boolean = false;
    public referenceTypesTranslations = {};
    public ItemsSource: ObservableCollection;

    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.ObjectTableName = "FreightForwarderReference"; 

        this.entityResourceService.getEntityResourceByTableName("FreightForwarderReference").subscribe((response: any) => {   
                 this.IsResourcesReady = true;
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;

            // clone the FreightForwarderReferences from entity
            this.FreightForwarderReferences = [];
            for (var i = 0; i < this.EntityPM.FreightForwarderReferences.length; i++) {
                if (this.EntityPM.FreightForwarderReferences[i].ChangeSetOp != "Delete") {
                    const myClone = ServiceHelper.CloneEntityPM(this.EntityPM.FreightForwarderReferences[i]);
                    this.FreightForwarderReferences.push(myClone);
                }
            }

            if (this.FreightForwarderReferences.length != 0) {
                this.BuildItemsList();
            }          
        }
    }

    Add() {
         var line: number = 0;
         var sequence: number = 0;
         if (this.FreightForwarderReferences.length > 0) {
             line = this.FreightForwarderReferences.reduce((max, obj) => (obj.LineNumber > max ? obj.LineNumber : max), this.FreightForwarderReferences[0].LineNumber);
         }        

         line += 1;
         sequence += 1;

        var item: FreightForwarderReferencePM = new FreightForwarderReferencePM();
        item.ShipmentId = this.EntityPM.Id;
        item.Tenant = this.EntityPM.Tenant;
        item.LineNumber = line;
        item.ChangeSetOp = "Insert";
        item.ForwarderFileConnect = false;

        if (!this.FreightForwarderReferences.includes(item)) {
             this.AddFreightForwarderReference(item);
             this.ItemsSource.Insert(new FreightForwarderReferenceLine(item, this));
        }
    }

    public AddFreightForwarderReference(item: FreightForwarderReferencePM) {
        if (item != null) {
            var index = this.FreightForwarderReferences.indexOf(item);
            if (index == -1) {
                this.FreightForwarderReferences.push(item);
            }
        }
    }
    public RemoveShipmentItem(item: FreightForwarderReferencePM) {
        if (item != null) {
            var index = this.FreightForwarderReferences.indexOf(item);
            if (index > -1) {

                if (item.ChangeSetOp == "Insert") {
                    this.FreightForwarderReferences.splice(index, 1);
                }
                else {
                    this.FreightForwarderReferences[index].ChangeSetOp = "Delete";
                }
            }
        }
    }

    BuildItemsList() {
        if (this.ItemsSource != null) {
            this.ItemsSource.Clear();
        }
        var TempItemSource: FreightForwarderReferenceLine[] = [];
        for (var i = 0; i < this.FreightForwarderReferences.length; i++) {
            TempItemSource.push(new FreightForwarderReferenceLine(this.FreightForwarderReferences[i], this));
        }
        if (this.ItemsSource != null) {
            this.ItemsSource.InsertCollection(TempItemSource);
        }
    }

    //#region Remark tooltip
    onCellSelected($event, Item: FreightForwarderReferenceLine) {
    }


    OnRowEnded($event) {
        if (($event) == this.ItemsSource.Length) {
            this.Add();

        }
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit(null);
    }

    Validate() {
        var errors: string[] = [];

        let emptyForwarderShipmentNumber = false;

        for (var i = 0; i < this.FreightForwarderReferences.length; i++) {
            let freightForwarderReference = this.FreightForwarderReferences[i];

            if (freightForwarderReference.ChangeSetOp != "Delete") {
                if (AppTool.IsNullOrEmpty(freightForwarderReference.ForwarderShipmentNumber)) {
                    emptyForwarderShipmentNumber = true;
                }
               
            }         
        }

        if (emptyForwarderShipmentNumber) {
            errors.push("אין להשאיר שורה ריקה אלא למחוק אותה");
        }
        else if (emptyForwarderShipmentNumber) {       
            errors.push(TextCodeTranslator.Translate("FreightForwarderReference.O.MissingForwarderShipmentNum"));
        }
      
        this.ValidationErrorsList = errors;
    }

    public ValidationErrorsList: string[] = [];

    OkButtonClicked() {

        this.Validate();

        if (this.ValidationErrorsList.length == 0) {

            let freightForwarderReferences = this.FreightForwarderReferences;
            const lineNumbers = this.FreightForwarderReferences.map(item => item.LineNumber);

            for (var i = 0; i < freightForwarderReferences.length; i++) {

                let freightForwarderReference = freightForwarderReferences[i];

                if (freightForwarderReference.ChangeSetOp == "Insert") {

                    // remove rows that have been added but with empty values
                    if (AppTool.IsNullOrEmpty(freightForwarderReference.ForwarderShipmentNumber) ) {
                        this.FreightForwarderReferences.splice(i, 1);
                    }
                    else if (!this.FreightForwarderReferences[i].LineNumber) {
                        this.FreightForwarderReferences[i].LineNumber = this.GetNextLineNumber(lineNumbers);
                    }
                }
            }

            console.log("SAVE", this.FreightForwarderReferences);

            // set the changes on the entity
            this.EntityPM.FreightForwarderReferences = this.FreightForwarderReferences;

            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        }
    }

       GetNextLineNumber(lineNumbers) {
           let lineNumber = 1;
           while (lineNumbers.indexOf(lineNumber) > -1) {
               lineNumber++;
           }
           console.log("GetNextLineNumber", lineNumbers, lineNumber)
           return lineNumber;
       }
}


export class FreightForwarderReferenceLine extends BaseComponent {
    public entityPM: FreightForwarderReferencePM = null;
    public DataContext = this;
    Parent: FreightForwarderReferenceDetailsComponent;

    public ShowClassifierRemarkTooltip: boolean = false;
    public ShowValidatioIcon: boolean = false;
    public closedManullay: boolean = false;

    constructor(EntityPM: FreightForwarderReferencePM, parent: FreightForwarderReferenceDetailsComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }

    //#region Properties
    public get ForwarderShipmentNumber() { return this.entityPM.ForwarderShipmentNumber; }
    public set ForwarderShipmentNumber(newValue: string) { 
        if (this.entityPM.ForwarderShipmentNumber != newValue) {
            this.entityPM.ForwarderShipmentNumber = newValue;
            if (!this.entityPM.ChangeSetOp) {
                this.entityPM.ChangeSetOp = "Update";
            }
        }
    }

    public get ForwarderFileConnect() { return this.entityPM.ForwarderFileConnect; }
 


    DeleteButtonClicked(item: FreightForwarderReferenceLine) {
        if (this.Parent.FreightForwarderReferences.includes(this.entityPM)) {
            this.Parent.RemoveShipmentItem(this.entityPM);
        }
        this.Parent.ItemsSource.Remove(item); 
    }
}
