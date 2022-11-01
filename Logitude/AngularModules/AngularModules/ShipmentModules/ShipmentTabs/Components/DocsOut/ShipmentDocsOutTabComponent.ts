declare var System: any;
declare var window: any;
import {Component, OnInit, ElementRef, Output, EventEmitter}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeListService} from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    
    templateUrl: './ShipmentDocsOutTabComponent.html',
    providers: [DocumentTypeListService]
})


export class ShipmentDocsOutTabComponent implements OnInit {


    public ObjectTableId: string;
    public EntityId: string;
    public TransportModeId: string;
    public ShipmentlevelCode: string
    public ChildrenObjectTableIds: string;
    public EntityPM: ShipmentPM;
    @Output() InitializeDocsOutForAnotherObjectTable = new EventEmitter();
    constructor(
        public entityArgs: EntityArgs, public _documentTypeListService: DocumentTypeListService,


        public _elementRef: ElementRef
    ) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d=> d.Name == "Shipment")[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.ShipmentlevelCode = this.EntityPM.ShipmentLevelCode;
            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = this.EntityPM.Tenant;
            this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;

                    var childrenDocTypes_invoices = myResult.filter(d => d.Code == "999S" || d.Code == "999M" || d.Code == "999CI" || d.Code == "ARINV");

                    if (childrenDocTypes_invoices.length > 0) {
                        childrenDocTypes_invoices.forEach((typeList) => {
                            this.ChildrenObjectTableIds = this.ChildrenObjectTableIds + "," + typeList.ObjectTableId;
                        });
                        var x = this.ChildrenObjectTableIds.charAt(0);
                        if (x == ',') {
                            this.ChildrenObjectTableIds = this.ChildrenObjectTableIds.substr(1);
                        }
           

                    }
                }



            });


        }

    }

    LoadFirstObjectTablesDocsCompleted() {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            var masterObject = window.ObjectTables.filter(d=> d.Name == "Master")[0];
            if (masterObject) {

                this.InitializeDocsOutForAnotherObjectTable.emit(masterObject.Id);
            }
        }

       
    }








}