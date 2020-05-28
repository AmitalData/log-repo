declare var System: any;
declare var window: any;

import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoiceEntityPM} from '../../../../Invoice/EntityPMs/ARInvoiceEntityPM';

@Component({
    
    templateUrl: './ARInvoiceDocsOutTabComponent.html',
})

export class ARInvoiceDocsOutTabComponent implements OnInit {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public ObjectTableId: string;
    public EntityId: string;
    public EntityReference: string
    public currentInvoiceEntity: ARInvoiceEntityPM;
    public ChildObjectTableId: string;
    public IsVisible = false; 
    public CustomFilterOperation: string = "";
    public CustomFilterValue: string = "";
    public ChildEntityId: string = "";
    public ShowMessageDocument: boolean = false;
    constructor(private entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            this.ChildEntityId = this.EntityPM.Id;
            if (table) this.ObjectTableId = table.Id;

            if (this.EntityPM.IsConsolidationInvoice) {
                this.IsVisible = true;
                if (table) this.ObjectTableId = table.Id;
                this.EntityId = this.EntityPM.Id;
                var myReference: string = null;

                if (this.EntityPM.InvoiceNumber == null) {
                    myReference = "Draft: " + this.EntityPM.DraftNumber;
                }

                else {
                    myReference = this.EntityPM.InvoiceNumber;
                }
               
                this.EntityReference = myReference;
                this.CustomFilterOperation = "Equal";
                this.CustomFilterValue = "999C";
                this.ShowMessageDocument = true;

            }

            else if (this.EntityPM.IsGeneralInvoice) {
                this.IsVisible = true;
                if (table) this.ObjectTableId = table.Id;
                this.EntityId = this.EntityPM.Id;
                var myReference: string = null;

                if (this.EntityPM.InvoiceNumber == null) {
                    myReference = "Draft: " + this.EntityPM.DraftNumber;
                }

                else {
                    myReference = this.EntityPM.InvoiceNumber;
                }

                this.EntityReference = myReference;
                this.CustomFilterOperation = "Equal";
                this.CustomFilterValue = "999G";
            }

            else if (this.EntityPM.StatusCode != "VD") {
                this.IsVisible = true;
                var entityObjectTable = null;
                if (this.EntityPM.InvoiceEntities.length > 1) {
                    entityObjectTable = window.ObjectTables.filter(d => d.Name == "Master")[0];
                    this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(a => a.ObjectTableId == entityObjectTable.Id)[0];
                }

                else if (this.EntityPM.InvoiceEntities.length == 1) {
                    entityObjectTable = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
                    this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(a => a.ObjectTableId == entityObjectTable.Id)[0];
                    if (this.currentInvoiceEntity == null) {
                        entityObjectTable = window.ObjectTables.filter(d => d.Name == "Master")[0];
                        this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(a => a.ObjectTableId == entityObjectTable.Id)[0];
                    }
                }

                if (entityObjectTable != null) {
                    this.ObjectTableId = entityObjectTable.Id;
                }

                var invoiceEntityId = "";
                if (this.currentInvoiceEntity != null) {
                    invoiceEntityId = this.currentInvoiceEntity.EntityId;
                }
                else {
                    invoiceEntityId = this.EntityPM.MainEntityId;

                }

                this.EntityId = invoiceEntityId;
                if (table) this.ChildObjectTableId = table.Id;

                this.CustomFilterOperation = "NotEqual";
                this.CustomFilterValue = "999C";
            }
        }
    }

}
