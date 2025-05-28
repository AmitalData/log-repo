import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SIIRequestWebService, SupplierInvoiceItemsForSIIRequest } from 'Customs/Services/WebServices/SIIRequestWebService';
import { SupplierInvoiceItemsReqListWebService } from 'Customs/Services/WebServices/SupplierInvoiceItemsReqListWebService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SupplierInvoiceItemsReqListPMService } from 'Customs/Services/StandardPMs/SupplierInvoiceItemsReqListPMService';
import { SupplierInvoiceItemsForSIIRequestLine } from '../SIIRequestTabs/SIIRequestComponent';


@Component({
    selector: 'SIIRequestCopmleteDataItemComponent',
    templateUrl: './SIIRequestCopmleteDataItemComponent.html',
    styleUrls: ['./SIIRequestCopmleteDataItemComponent.scss'],
    providers: [EntityArgs],
})


export class SIIRequestCopmleteDataItemComponent extends BaseComponent implements OnInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public siiRequestWebService: SIIRequestWebService;
    public supplierInvoiceItemsReqListWebService: SupplierInvoiceItemsReqListWebService;
    public supplierInvoiceItemsReqListPMService: SupplierInvoiceItemsReqListPMService = new SupplierInvoiceItemsReqListPMService();
    public currentSiiRequest: SIIRequestPM = new SIIRequestPM();
    public entityPM: SupplierInvoiceItemsReqListPM;
    public DecalarationData: DeclarationPM;
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
        this.siiRequestWebService = new SIIRequestWebService();
        this.supplierInvoiceItemsReqListWebService = new SupplierInvoiceItemsReqListWebService();
        // this.supplierInvoiceItemsReqListPMService = new SupplierInvoiceItemsReqListPMService();
        this.entityPM = new SupplierInvoiceItemsReqListPM();

    }

    ngOnInit(): void {
        this.initiallizeComponent();
    }

    // #region initialization data:
    initiallizeComponent() {
        this.IsLoaded = true;
        this.SetPropertiesEnabled();
    }

    invoiceItemReq: SupplierInvoiceItemsForSIIRequestLine;
    SetWindowArgs(args: any) {
        this.currentSiiRequest = args.SIIRequest;
        this.DecalarationData = args.Decalaration;
        this.invoiceItemReq = args.invoiceItemReq;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.filterAgrs = args.filterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.SupplierInvoiceItemsReqList";
        this.entityPM = args.entityPMSupplierInvoiceItemsReqListPM;
        console.log(this.entityPM);
    }

    SetPropertiesEnabled() {
        let enabled: boolean = !this.IsDisplayOnly;
        this.UIProperties.SetEnabled("ManufactureCountryCode", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ItemNo", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ItemName", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("InvoiceQuantity", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("InvoiceQuantityType", this.ObjectTableNameSiiRequest, !enabled);
    }
    // #endregion initialization data

    //#region search product file number by API request:
    SearchProductFileNumber(ProductFileNumber: string = '') {
        let productFileExists: boolean = false;
        this.SaveSupplierInvoiceItemsReqList();

        // this.supplierInvoiceItemsReqListWebService.GetProductFileExists(ProductFileNumber, this.currentSiiRequest.ImporterId, this.entityPM.OriginCountryCode).subscribe(myResult => {
        //     let myResponse: ServiceResponse = myResult;
        //     if (!myResponse?.HasError && myResponse?.Result) {
        //         // TODO:
        //         // 1. Handle the response data:
        //         // if exist propduct file - return true
        //         //  else false and reset this.ProductFileNumber = '' (cannot save product file are not exist)


        //         // TODO: 2. check mandatory fields and display error alert:
        //         this.checkMandatoryFields();

        //         //TODO: 3.change to real response check
        //         // update productFileExists based on the response
        //         if (productFileExists) {
        //             this.SaveSupplierInvoiceItemsReqList();
        //         }
        //         else {
        //             this.entityPM.ProductFileNumber = '';
        //             // TODO: Display a warning alert and ask the user if they still want to save the data even though the product file does not exist
        //             // if yes- call to this.SaveSupplierInvoiceItemsReqList();
        //             // else- close or stay in the screen

        //         }
        //     }
        // });
    }

    //#region acations methods:
    SaveAndSearchSupplierInvoiceItemsReqList(ProductFileNumber: string = '') {
        this.SearchProductFileNumber(ProductFileNumber);
    }
    SaveSupplierInvoiceItemsReqList() {
        // TODO 4. Update RequestRequiredStatus to 0/1/2 base on the missing mandatory fields
        // TODO: save entityPM data to the server
        this.entityPM.DeclarationId = this.DecalarationData?.Id;
        this.entityPM.InvoiceCounterKey = this.invoiceItemReq.CounterKey;
        this.entityPM.InvoiceItemLineNumber = this.invoiceItemReq.LineNumber;

        this.supplierInvoiceItemsReqListPMService.insert(this.entityPM).subscribe(myResult => {
            let myResponse: ServiceResponse = myResult;
            console.log(myResponse);
            debugger
            if (!myResponse?.HasError && myResponse?.Result) {
                console.log(myResponse?.Result);

            }
        }, error => {
            console.error('Error saving SupplierInvoiceItemsReqList:', error);
        });


    }

    CancelSupplierInvoiceItemsReqList() {
        // TODO: cancel the operation and reset the form
    }

    checkMandatoryFields() {
        // TODO: check if all mandatory fields are filled
    }
    //#endregion acations methods

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
    public get ItemNo(): string {
        return this.entityPM?.ItemNo;
    }
    public set ItemNo(newValue: string) {
        this.entityPM.ItemNo = newValue;
    }
    public get ItemName(): string {
        return this.entityPM?.ItemName;
    }
    public set ItemName(newValue: string) {
        this.entityPM.ItemName = newValue;
    }
    public get InvoiceQuantity(): number {
        return this.entityPM?.InvoiceQuantity;
    }
    public set InvoiceQuantity(newValue: number) {
        this.entityPM.InvoiceQuantity = newValue;
    }
    public get InvoiceQuantityType(): string {
        return this.entityPM?.InvoiceQuantityType;
    }
    public set InvoiceQuantityType(newValue: string) {
        this.entityPM.InvoiceQuantityType = newValue;
    }
    public get StatisticQuantity(): number {
        return this.entityPM?.StatisticQuantity;
    }
    public set StatisticQuantity(newValue: number) {
        this.entityPM.StatisticQuantity = newValue;
    }
    public get StatisticQuantityType(): string {
        return this.entityPM?.StatisticQuantityType;
    }
    public set StatisticQuantityType(newValue: string) {
        this.entityPM.StatisticQuantityType = newValue;
    }
    public get DutchRequested(): boolean {
        // this.entityPM.EntityParentPM.DisableMarkAsDirty = true;
        return this.entityPM?.DutchRequested;
    }
    public set DutchRequested(newValue: boolean) {
        this.entityPM.DutchRequested = newValue;
        // this.entityPM.EntityParentPM.DisableMarkAsDirty = true;
    }
    // public get DutchGroupItem(): boolean {
    //     return this.entityPM?.DutchGroupItem;
    // }
    // public set DutchGroupItem(newValue: boolean) {
    //     this.entityPM.DutchGroupItem = newValue;
    // }

    //#endregion SiiRequest properties


}
