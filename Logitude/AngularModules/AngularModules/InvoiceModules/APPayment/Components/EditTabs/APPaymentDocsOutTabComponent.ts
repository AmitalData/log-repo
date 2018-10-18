declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APPaymentPM} from '../../../../Invoice/EntityPMs/APPaymentPM';

@Component({
    moduleId: module.id,
    templateUrl: './APPaymentDocsOutTabComponent.html',
})

export class APPaymentDocsOutTabComponent implements OnInit {
    public EntityPM: APPaymentPM = null;
    public ObjectTableName = "APPayment";
    public DataContext = this;
    public ObjectTableId: string;
    public EntityId: string;
    public EntityReference: string

    constructor(private entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.EntityReference = this.EntityPM.PaymentNo;
        }

    }
}