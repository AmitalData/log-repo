import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';


@Component({
    selector: 'SIIRequestCopmleteDataItemComponent',
    templateUrl: './SIIRequestCopmleteDataItemComponent.html',
    styleUrls: ['./SIIRequestCopmleteDataItemComponent.scss'],
    providers: [EntityArgs],
})


export class SIIRequestCopmleteDataItemComponent extends BaseComponent implements OnInit {

    public currentSiiRequest: SIIRequestPM = new SIIRequestPM();
    public entityPM: SupplierInvoiceItemsReqListPM = new SupplierInvoiceItemsReqListPM(this.currentSiiRequest);
    public DecalarationData: DeclarationPM;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public DataContext = this;
    public ObjectTableName: string = "Customs.SupplierInvoiceItemsReqList";
    public ObjectTableNameDeclaration: string = "Customs.Declaration";
    public ObjectTableNameSiiRequest: string = "Customs.SIIRequests";
    public IsDisplayOnly: boolean = false;
    public isAllowChange: boolean = false;
    public filterAgrs: ApiQueryFilters;
    public IsNewOrEdit: boolean = false;
    


    constructor(public entityArgs: EntityArgs) {
        super();
    }

    ngOnInit(): void {
    }
    SetWindowArgs(args: any) {
        this.currentSiiRequest = args.SIIRequest;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.filterAgrs = args.filterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.SupplierInvoiceItemsReqList";
        this.entityPM = new SupplierInvoiceItemsReqListPM(this.currentSiiRequest);
        console.log(this.entityPM);
    }


}
