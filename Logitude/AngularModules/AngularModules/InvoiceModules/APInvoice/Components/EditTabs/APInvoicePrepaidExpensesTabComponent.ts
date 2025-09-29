declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';

@Component({
    
    templateUrl: './APInvoicePrepaidExpensesTabComponent.html',
})

export class APInvoicePrepaidExpensesTabComponent implements OnInit {
    public EntityPM: APInvoicePM = null;
    public ObjectTableName = "APInvoice";
    public DataContext = this;
    
    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;     
    }

    ngOnInit() {
       
    }
}