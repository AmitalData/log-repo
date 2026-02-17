declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoiceEntityPM} from '../../../../Invoice/EntityPMs/ARInvoiceEntityPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceDocsInTabComponent.html',
})

export class ARInvoiceDocsInTabComponent implements OnInit {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public ObjectTableId: string;
    public EntityId: string;
    public EntityReference: string
    public currentInvoiceEntity: ARInvoiceEntityPM;
    public ChildObjectTableId: string;
    public IsVisible = false; 
    public ChildEntityId: string;
    public ChildEntityReference: string;


    constructor(private entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            if (this.EntityPM.StatusCode != "VD") {
                this.IsVisible = true;
            }
            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            var entityObjectTable = null;
            if (this.EntityPM.InvoiceEntities.length > 1) {
                entityObjectTable = window.ObjectTables.filter(d => d.Name == "Master" && (d.Tenant == SessionLocator.TenantPM.Id || d.Tenant == 0))[0];
                this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(a => a.ObjectTableId == entityObjectTable.Id)[0];
            }

            else if (this.EntityPM.InvoiceEntities.length == 1) {
                entityObjectTable = window.ObjectTables.filter(d => d.Name == "Shipment" && (d.Tenant == SessionLocator.TenantPM.Id || d.Tenant == 0))[0];
                this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(a => a.ObjectTableId == entityObjectTable.Id)[0];
                if (this.currentInvoiceEntity == null) {
                    entityObjectTable = window.ObjectTables.filter(d => d.Name == "Master" && (d.Tenant == SessionLocator.TenantPM.Id || d.Tenant == 0))[0];
                    this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(a => a.ObjectTableId == entityObjectTable.Id)[0];
                }
            }

            if (AppTool.IsNullOrEmpty(this.ObjectTableId)) {
                var ObjectTable = window.ObjectTables.filter(d => d.Name == "ARInvoice" && (d.Tenant == SessionLocator.TenantPM.Id || d.Tenant == 0))[0];
                if (ObjectTable != null) {
                    this.ObjectTableId = ObjectTable.Id;
                }
            }

            if (entityObjectTable != null) {
                this.ObjectTableId = entityObjectTable.Id;
            }

            var invoiceEntityId = "";
            if (this.currentInvoiceEntity != null) {
                invoiceEntityId = this.currentInvoiceEntity.EntityId;
            }
            
            this.EntityId = invoiceEntityId;
            if (table) this.ChildObjectTableId = table.Id;
            this.ChildEntityId = this.EntityPM.Id;
            this.ChildEntityReference = this.EntityPM.InvoiceNumber;
        }
    }
}