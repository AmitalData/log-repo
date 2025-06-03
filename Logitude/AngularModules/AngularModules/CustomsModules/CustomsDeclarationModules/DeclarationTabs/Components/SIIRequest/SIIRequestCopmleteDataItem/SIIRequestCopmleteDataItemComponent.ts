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
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';


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
    private CurrentSession = SessionLocator.SelectedSession;
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
        this.SaveSupplierInvoiceItemsReqList();
    }

    //#region acations methods:
    SaveAndSearchSupplierInvoiceItemsReqList(ProductFileNumber: string = '') {
        this.SearchProductFileNumber(ProductFileNumber);
    }
    SaveSupplierInvoiceItemsReqList() {
        this.entityPM.DeclarationId = this.DecalarationData?.Id;
        this.entityPM.SIIRequestID = this.currentSiiRequest.Id;
        this.entityPM.InvoiceCounterKey = this.invoiceItemReq.InvoiceCounterKey;
        this.entityPM.InvoiceItemLineNumber = this.invoiceItemReq.InvoiceLineNumber;
        this.entityPM.LineNumber = this.invoiceItemReq.LineNumber;

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
        this.RefreshEntity();
        this.CurrentSession.CloseCurrentWindow();
    }

    RefreshEntity() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
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
        return this.entityPM?.DutchRequested;
    }
    public set DutchRequested(newValue: boolean) {
        this.entityPM.DutchRequested = newValue;
    }
    public get DutchGroupItem(): number {
        return this.entityPM?.DutchGroupItem;
    }
    public set DutchGroupItem(newValue: number) {
        this.entityPM.DutchGroupItem = newValue;
    }
    //#endregion SiiRequest properties
}
