
import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {DateTool, AppTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

import {CustomerDepositionListExtendedService} from '../../../../../Common/Services/ExtendedLists/CustomerDepositionListExtendedService';


@Component({
    selector: 'CustomsShipperGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './CustomsShipperGeneralTabComponent.html',
})

export class CustomsShipperGeneralTabComponent implements OnInit {
    public EntityPM: any;
    public ObjectTableName: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private customerDepositionListExtendedService: CustomerDepositionListExtendedService;
    
    public ItemsSource: any[] = [];
    IsReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.customerDepositionListExtendedService  = new CustomerDepositionListExtendedService();

        this._entityResourceService.getEntityResourceByTableName("CustomerDeposition").subscribe(response => {
            this.IsReady = true;
            this.LoadData();
        });



    }

    ngOnInit() {

    }



    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.customerDepositionListExtendedService.GetCustomerDepositionListsByCustomsShipperId(this.EntityPM.Id, this.EntityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
               
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }



}
