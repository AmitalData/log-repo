import { AppTool } from './../../../../Infrastructure/Tools';
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';

@Component({
    moduleId: module.id,
    templateUrl: './APInvoiceDocsInTabComponent.html',
})

export class APInvoiceDocsInTabComponent implements OnInit {
    public EntityPM: APInvoicePM = null;
    public ObjectTableName = "APInvoice";
    public DataContext = this;
    public EntityId: string;
    public EntityReference: string
    public EntityObjectTableId: string;
    public ChildEntityId: string;
    public ChildEntityReference: string;
    public ChildEntityObjectTableId: string;
    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        var invoiceObjectTable = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];

        if (this.EntityPM.IsMultipleEntities) {
            this.EntityId = this.EntityPM.Id;
            this.EntityReference = this.EntityPM.InvoiceNumber;
            this.EntityObjectTableId = invoiceObjectTable.Id;
        }

        else {
            this.EntityId = this.EntityPM.MainEntityId;
            this.EntityReference = this.EntityPM.MainEntityReference;

            var invoiceEntitiy = this.EntityPM.InvoiceEntities.filter(a => a.EntityId == this.EntityPM.MainEntityId)[0];
            if (invoiceEntitiy) {
                this.EntityObjectTableId = invoiceEntitiy.ObjectTableId;
            }

            if(AppTool.IsNullOrEmpty(this.EntityObjectTableId)){
                this.EntityObjectTableId=invoiceObjectTable.Id;
            }
            this.ChildEntityId = this.EntityPM.Id;
            this.ChildEntityReference = this.EntityPM.InvoiceNumber;
            this.ChildEntityObjectTableId = invoiceObjectTable.Id;
        }
    }
}