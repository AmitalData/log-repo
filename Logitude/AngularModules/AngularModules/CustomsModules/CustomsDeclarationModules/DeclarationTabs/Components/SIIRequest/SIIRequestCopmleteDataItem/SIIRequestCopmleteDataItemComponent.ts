import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';


@Component({
    selector: 'SIIRequestCopmleteDataItemComponent',
    templateUrl: './SIIRequestCopmleteDataItemComponent.html',
    styleUrls: ['./SIIRequestCopmleteDataItemComponent.scss'],
    providers: [EntityArgs],
})


export class SIIRequestCopmleteDataItemComponent extends BaseComponent implements OnInit {
    public entityResourceService: EntityResourceService = new EntityResourceService();
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
    public IsLoaded: boolean = false;



    constructor(public entityArgs: EntityArgs) {
        super();
    }

    ngOnInit(): void {
        this.initiallizeComponent();
    }

    initiallizeComponent() {
        this.IsLoaded = true;
        // this.entityResourceService.getEntityResourceByTableName("Customs.SIIRequest").subscribe((response: any) => {
        //     this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsReqList").subscribe((response: any) => {
        //         this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
        //         });
        //     });
        // });

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


    SetPropertiesEnabled() {
    }

    SaveSupplierInvoiceItemsReqList() {

    }

    CancelSupplierInvoiceItemsReqList() {

    }

    //#region  SiiRequest properties
    public get ProductFileNumber(): string {
        return this.entityPM?.ProductFileNumber;
    }
    public set ProductFileNumber(newValue: string) {
        this.entityPM.ProductFileNumber = newValue;
    }

    public get ManufactureCountryCode(): string {
        return this.entityPM?.ManufactureCountryCode;
    }
    public set ManufactureCountryCode(newValue: string) {
        this.entityPM.ManufactureCountryCode = newValue;
    }
    public get ManufactureCountryName(): string {
        return this.entityPM?.ManufactureCountryName;
    }
    public set ManufactureCountryName(newValue: string) {
        this.entityPM.ManufactureCountryName = newValue;
    }

    public get ManufacturerName(): string {
        return this.entityPM?.ManufacturerName;
    }
    public set ManufacturerName(newValue: string) {
        this.entityPM.ManufacturerName = newValue;
    }
    public get Remarks(): string {
        return this.entityPM?.Remarks;
    }
    public set Remarks(newValue: string) {
        this.entityPM.Remarks = newValue;
    }
    // TODO: Delete after adding the property to the entity
    private _IsAggravationGroup1Req: boolean = false;
    public get IsAggravationGroup1Req(): boolean {
        return this._IsAggravationGroup1Req;
    }
    public set IsAggravationGroup1Req(newValue: boolean) {
        debugger
        this._IsAggravationGroup1Req = newValue;
    }

    // TODO: Uncomment after adding the property to the entity 
    // public get ItemNo(): string {
    //     return this.entityPM?.ItemNo;
    // }
    // public set ItemNo(newValue: string) {
    //     this.entityPM.ItemNo = "newValue";
    // }

    // public get ItemName(): string {
    //     return this.entityPM?.ItemName;
    // }
    // public set ItemName(newValue: string) {
    //     this.entityPM.ItemName = newValue;
    // }

    // public get InvoiceQuantity(): number {
    //     return this.entityPM?.InvoiceQuantity;
    // }
    // public set InvoiceQuantity(newValue: number) {
    //     this.entityPM.InvoiceQuantity = newValue;
    // }

    // public get InvoiceQuantityType(): string {
    //     return this.entityPM?.InvoiceQuantityType;
    // }
    // public set InvoiceQuantityType(newValue: string) {
    //     this.entityPM.InvoiceQuantityType = newValue;
    // }

    // public get StatisticQuantity(): number {
    //     return this.entityPM?.StatisticQuantity;
    // }
    // public set StatisticQuantity(newValue: number) {
    //     this.entityPM.StatisticQuantity = newValue;
    // }

    // public get StatisticQuantityType(): string {
    //     return this.entityPM?.StatisticQuantityType;
    // }
    // public set StatisticQuantityType(newValue: string) {
    //     this.entityPM.StatisticQuantityType = newValue;
    // }
    // public get IsAggravationGroup1Req(): string {
    //     return this.entityPM?.IsAggravationGroup1Req;
    // }
    // public set IsAggravationGroup1Req(newValue: string) {
    //     this.entityPM.IsAggravationGroup1Req = newValue;
    // }

    //#endregion SiiRequest properties


}
