declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARPaymentPM} from '../../../../Invoice/EntityPMs/ARPaymentPM';

@Component({
    moduleId: module.id,
    templateUrl: './ARPaymentDocsInTabComponent.html',
})

export class ARPaymentDocsInTabComponent implements OnInit {
    public EntityPM: ARPaymentPM = null;
    public ObjectTableName = "ARPayment";
    public DataContext: this;
    public ObjectTableId: string;
    public EntityId: string;
    public IsVisible = false; 

    constructor(private entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            if (this.EntityPM.StatusCode != "VD") {
                this.IsVisible = true;
            }

            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
        }
    }
}