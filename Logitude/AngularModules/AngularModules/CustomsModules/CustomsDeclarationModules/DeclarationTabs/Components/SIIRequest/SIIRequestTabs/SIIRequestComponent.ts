import { Component, QueryList, ViewChildren, OnInit, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { SIIRequestPMService } from 'Customs/Services/StandardPMs/SIIRequestPMService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { SiiRequestMode } from '../SIIRequestTabComponent';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { SupplierInvoiceItemExtendedListService } from 'Customs/Services/ExtendedLists/SupplierInvoiceItemExtendedListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from 'Infrastructure/Tools';
import { UserPM } from 'Common/EntityPMs/UserPM';
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SupplierInvoiceItemsForSIIRequest } from 'Customs/Services/WebServices/SIIRequestWebService';
import { UserPMService } from 'Common/Services/StandardPMs/UserPMService';

@Component({
    selector: 'SIIRequestComponent',
    templateUrl: './SIIRequestComponent.html',
    styleUrls: ['./SIIRequestComponent.scss'],
    providers: [EntityArgs]
})


export class SIIRequestComponent extends BaseComponent implements OnInit {
    private declarationWebService: DeclarationWebService = new DeclarationWebService; // TODO: Delete after test
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public DataContext = this;
    public ObjectTableName: string = "Customs.Declaration";
    public ObjectTableNameSiiRequest: string = "Customs.SIIRequests";
    siiRequestPMService: SIIRequestPMService = new SIIRequestPMService();
    userPmService: UserPMService = new UserPMService();
    supplierinvoiceitemsWebService: SupplierInvoiceItemExtendedListService = new SupplierInvoiceItemExtendedListService();
    public entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DecalarationData: DeclarationPM;
    public IsNewOrEdit: SiiRequestMode;
    public IsDisplayOnly: boolean = false;
    public isAllowChange: boolean = false;
    public entityPM: SIIRequestPM = new SIIRequestPM();
    public filterAgrs: ApiQueryFilters;
    private userData: UserPM = new UserPM();
    public IsLoaded: boolean = false;
    public supplierInvoiceItemsForSIIRequest: SupplierInvoiceItemsForSIIRequest[] = [];
    public supplierInvoiceItemsCollection: ObservableCollection;
    public IsCheckBoxVisible: boolean = false;

    constructor(public entityArgs: EntityArgs, public CD: ChangeDetectorRef) {
        super();
        this.SelectedInvoiceItemsReqList = new ObservableCollection([]);
    }

    ngOnInit(): void {
        this.initiallizeComponent();
    }
    initiallizeComponent() {
        this.entityResourceService.getEntityResourceByTableName("Customs.SIIRequest").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsReqList").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
                    this.IsLoaded = true;
                    this.userData = SessionLocator.LoggedUserPM;
                    this.FillInvoiceNumbersList();
                });
            });
        });
    }

    buildSupplierInvoiceItemsCollection(): void {
        this.supplierInvoiceItemsCollection.Clear();
        this.supplierInvoiceItemsForSIIRequest.forEach((item) => {
            let siiRequestComponent: SIIRequestComponent;
            const supplierInvoiceItemLine = new SiiRequestSupplierInvoiceItemsLine(item, siiRequestComponent);
            this.supplierInvoiceItemsCollection.Insert(supplierInvoiceItemLine);
        });

        this.IsCheckBoxVisible = this.supplierInvoiceItemsCollection?.Collection?.length > 0 ? true : false;
    }

    RefreshEntity() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }

    SetWindowArgs(args: any) {
        this.entityPM = args.SiiRequest;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.filterAgrs = args.filterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.SIIRequest";
        this.supplierInvoiceItemsForSIIRequest = args.supplierInvoiceItemsForSIIRequest;
        this.supplierInvoiceItemsCollection = new ObservableCollection([]);
        this.buildSupplierInvoiceItemsCollection();
        if (args.SiiRequest != null) this.entityPM = args.SiiRequest;
    }

    CancelSaveSiiRequest() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }
    SaveSiiRequest() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }
    SendSiiRequest(event: any) {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }

    SearchText: string = "";
    Search(SearchText: string) {
        this.SearchText = !AppTool.IsNullOrEmpty(SearchText) ? SearchText.toLowerCase() : SearchText;
        //TODO: add search filter to LIST
    }

    public SearchFilterChangedEvent: any;

    CreateMethod() {
        var windowArgs: any = {};
        windowArgs.Parent = this;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "SIIRequestCopmleteDataItemComponent";
        logWindow.Width = 400;
        logWindow.Height = 800;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.CD.reattach();
                this.RefreshEntity();
            }
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestCopmleteDataItemComponent');
    }

    MoveMethod() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.SubmitForApproval");
        logWindow.Width = 900;
        logWindow.Height = 600;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CD.reattach();
            this.RefreshEntity();
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestCopmleteDataItemComponent');
    }


    SelectedRow: SupplierInvoiceItemsReqListLine;
    SelectedRows: SupplierInvoiceItemsReqListLine[];
    preventSelect: boolean;
    deletedInvItems: string = "";

    public SelectedInvoiceItemsReqList: ObservableCollection;
    OnRowSelected(items: SupplierInvoiceItemsReqListLine[]) {
        this.SelectedRows = items;
        if (items.length === 0) this.SelectedInvoiceItemsReqList.Clear();
        else this.SelectedInvoiceItemsReqList.Collection = items;
    }

    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        this.OnRowSelected(value ? this.supplierInvoiceItemsCollection?.Collection : []);
    }

    //#region Properties Filter Methods
    private SelectedCounterKey: number = null;
    private DemandState: string = null;
    private selectedInvoiceNumber: string = null;
    public get SelectedInvoiceNumber() { return this.selectedInvoiceNumber }
    public set SelectedInvoiceNumber(newValue: string) {
        this.selectedInvoiceNumber = newValue;
    }
    //#endregion

    //#region DemandState Filter Methods
    public DemandStateFilterSelectedValue: string = 'All';
    DemandStateFilterItemClicked(itemValue: string) {
        if (this.DemandStateFilterSelectedValue != itemValue) {
            this.DemandStateFilterSelectedValue = itemValue;
            this.DemandState = itemValue == 'All' ? null : itemValue;
        }
    }
    //#region Invoice ComboBox
    SelectedInvoiceReqConfirmation: string;
    InvoicesSelectionChanged(selectedItem) {
        if (selectedItem != null) {
            this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
            this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
            this.SelectedInvoiceReqConfirmation = selectedItem.ReqConfirmationTypeCode;
        }
    }

    InvoicesNumbersList: any[];
    FillInvoiceNumbersList() {
        this.declarationWebService.GetDeclarationInvoicesNumbers(this.DecalarationData.Id)
            .subscribe((response: ServiceResponse) => {
                if (!AppTool.IsNullOrEmpty(response?.Result))
                    this.InvoicesNumbersList = response?.Result;
            });
    }
    //#endregion

    //#region LevelSelection Filter Methods
    public LevelSelectionFilterSelectedValue: string = 'DeclarationConect';
    LevelSelectionFilterItemClicked(itemValue: string) {
        if (this.LevelSelectionFilterSelectedValue != itemValue) {
            this.LevelSelectionFilterSelectedValue = itemValue;
            if (itemValue != "Invoice") {
                this.SelectedInvoiceNumber = null;
                this.SelectedCounterKey = null;
            }
        }
    }
    //#endregion

    //#region  SiiRequest properties
    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
        this.entityPM.Id = newValue;
    }
    public get ContactId(): string {
        return this.entityPM.Id;
    }
    public set ContactId(newValue: string) {
        this.entityPM.Id = newValue;
    }
    public get Tenant(): number {
        return this.entityPM.Tenant;
    }
    public set Tenant(newValue: number) {
        this.entityPM.Tenant = newValue;
    }

    public get RequestNo(): string {
        return this.entityPM.RequestNo;
    }
    public set RequestNo(newValue: string) {
        this.entityPM.RequestNo = newValue;
    }

    public get DeclarationId(): string {
        return this.entityPM.DeclarationId;
    }
    public set DeclarationId(newValue: string) {
        this.entityPM.DeclarationId = newValue;
    }

    public get Status(): string {
        return this.entityPM.Status;
    }
    public set Status(newValue: string) {
        this.entityPM.Status = newValue;
    }

    public get WareHouseAddress(): string {
        return this.entityPM.WareHouseAddress;
    }
    public set WareHouseAddress(newValue: string) {
        this.entityPM.WareHouseAddress = newValue;
    }

    public get WareHouseCity(): string {
        return this.entityPM.WareHouseCity;
    }
    public set WareHouseCity(newValue: string) {
        this.entityPM.WareHouseCity = newValue;
    }

    public get IsClosed(): boolean {
        return this.entityPM.IsClosed;
    }
    public set IsClosed(newValue: boolean) {
        this.entityPM.IsClosed = newValue;
    }

    public get Remarks(): string {
        return this.entityPM?.Remarks ? this.entityPM?.Remarks : "";
    }
    public set Remarks(newValue: string) {
        this.entityPM.Remarks = newValue;
    }
    // TODO: change to Real data map
    public get ShipName(): string {
        // return this.entityPM.ShipName;
        return this.ShipName;
    }
    public set ShipName(newValue: string) {
        // this.entityPM.ShipName = newValue;
        this.ShipName = newValue;
    }
    //#endregion SiiRequest properties

    //#region  Declaration properties
    public get CustomFileNo(): string {
        return this.DecalarationData.CustomFileNo;
    }
    public set CustomFileNo(newValue: string) {
        this.DecalarationData.CustomFileNo = newValue;
    }

    public get ImporterId(): string {
        return !AppTool.IsNullOrEmpty(this.DecalarationData.ImporterId) ? this.DecalarationData.ImporterId : this.ImporterCode;
    }
    public set ImporterId(newValue: string) {
        this.DecalarationData.ImporterId = newValue;
    }
    public get ImporterCode(): string {
        return this.DecalarationData.ImporterCode;
    }
    public set ImporterCode(newValue: string) {
        this.DecalarationData.ImporterCode = newValue;
    }

    // TODO: change to Real data (from apa date decalration referantdata) 
    public get ArrivelDate(): string {
        return this.DecalarationData.ImporterCode;
    }
    public set ArrivelDate(newValue: string) {
        this.DecalarationData.ImporterCode = newValue;
    }
    //#endregion declaration properties

    //#region user data properties
    public get Email(): string {
        return this.userData.Email;
    }
    public set Email(newValue: string) {
        this.userData.Email = newValue;
    }
    public get BusinessPhone(): string {
        return this.userData.BusinessPhone;
    }
    public set BusinessPhone(newValue: string) {
        this.userData.BusinessPhone = newValue;
    }
    public get Mobile(): string {
        return this.userData.Mobile;
    }
    public set Mobile(newValue: string) {
        this.userData.Mobile = newValue;
    }
    public get Fax(): string {
        return this.userData.Fax;
    }
    public set Fax(newValue: string) {
        this.userData.Fax = newValue;
    }
    //#endregion user data properties

}



//#region SupplierInvoiceItemsReqListLine properties
export class SupplierInvoiceItemsReqListLine extends BaseComponent {
    public entityPM: SupplierInvoiceItemsReqListPM;
    public ObjectTableName: string = "Customs.CertificateOfOriginItem";
    public DataContext = this;
    Parent: SIIRequestComponent;
    constructor(EntityPM: SupplierInvoiceItemsReqListPM, parent: SIIRequestComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }

    public get LineNumber(): number {
        return this.entityPM.LineNumber;
    }
    public set LineNumber(newValue: number) {
        this.entityPM.LineNumber = newValue;
    }

    public get SIIRequestID(): string {
        return this.entityPM.SIIRequestID;
    }
    public set SIIRequestID(newValue: string) {
        this.entityPM.SIIRequestID = newValue;
    }

    public get InvoiceCounterKey(): number {
        return this.entityPM.InvoiceCounterKey;
    }
    public set InvoiceCounterKey(newValue: number) {
        this.entityPM.InvoiceCounterKey = newValue;
    }

    public get InvoiceItemLineNumber(): number {
        return this.entityPM.InvoiceItemLineNumber;
    }
    public set InvoiceItemLineNumber(newValue: number) {
        this.entityPM.InvoiceItemLineNumber = newValue;
    }

    public get RequestType(): string {
        return this.entityPM.RequestType;
    }
    public set RequestType(newValue: string) {
        this.entityPM.RequestType = newValue;
    }

    public get ProductFileNumber(): string {
        return this.entityPM.ProductFileNumber;
    }
    public set ProductFileNumber(newValue: string) {
        this.entityPM.ProductFileNumber = newValue;
    }

    public get ManufactureCountryCode(): string {
        return this.entityPM.ManufactureCountryCode;
    }
    public set ManufactureCountryCode(newValue: string) {
        this.entityPM.ManufactureCountryCode = newValue;
    }

    public get ManufactureCountryName(): string {
        return this.entityPM.ManufactureCountryName;
    }
    public set ManufactureCountryName(newValue: string) {
        this.entityPM.ManufactureCountryName = newValue;
    }

    public get ManufacturerName(): string {
        return this.entityPM.ManufacturerName;
    }
    public set ManufacturerName(newValue: string) {
        this.entityPM.ManufacturerName = newValue;
    }

    public get Remarks(): string {
        return this.entityPM.Remarks;
    }
    public set Remarks(newValue: string) {
        this.entityPM.Remarks = newValue;
    }
}
//#region SiiRequestSupplierInvoiceItemsLine properties:
export class SiiRequestSupplierInvoiceItemsLine extends BaseComponent {
    public entityPM: SupplierInvoiceItemsForSIIRequest;
    public ObjectTableName: string = "Customs.CertificateOfOriginItem";
    public DataContext = this;
    Parent: SIIRequestComponent;
    constructor(EntityPM: SupplierInvoiceItemsForSIIRequest, parent: SIIRequestComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }

    public get InvoiceNumber(): string {
        return this.entityPM.InvoiceNumber;
    }
    public set InvoiceNumber(newValue: string) {
        this.entityPM.InvoiceNumber = newValue;
    }
    public get LineNumber(): number {
        return this.entityPM.LineNumber;
    }
    public set LineNumber(newValue: number) {
        this.entityPM.LineNumber = newValue;
    }
    public get ItemCode(): string {
        return this.entityPM.ItemCode;
    }
    public set ItemCode(newValue: string) {
        this.entityPM.ItemCode = newValue;
    }
    public get ItemName(): string {
        return this.entityPM.ItemName;
    }
    public set ItemName(newValue: string) {
        this.entityPM.ItemName = newValue;
    }
    public get ItemDescription(): string {
        return this.entityPM.ItemDescription;
    }
    public set ItemDescription(newValue: string) {
        this.entityPM.ItemDescription = newValue;
    }
    public get ClassificationCode(): string {
        return this.entityPM.ClassificationCode;
    }
    public set ClassificationCode(newValue: string) {
        this.entityPM.ClassificationCode = newValue;
    }

    public get TradeAgreementCode(): string {
        return this.entityPM.TradeAgreementCode;
    }
    public set TradeAgreementCode(newValue: string) {
        this.entityPM.TradeAgreementCode = newValue;
    }

    public get TradeAgreementName(): string {
        return this.entityPM.TradeAgreementName;
    }
    public set TradeAgreementName(newValue: string) {
        this.entityPM.TradeAgreementName = newValue;
    }

    public get InvoiceQuantityType(): string {
        return this.entityPM.InvoiceQuantityType;
    }
    public set InvoiceQuantityType(newValue: string) {
        this.entityPM.InvoiceQuantityType = newValue;
    }
    public get InvoiceQuantityTypeName(): string {
        return this.entityPM.InvoiceQuantityType;
    }
    public set InvoiceQuantityTypeName(newValue: string) {
        this.entityPM.InvoiceQuantityType = newValue;
    }
    public get InvoiceQuantity(): string {
        return this.entityPM.InvoiceQuantity;
    }
    public set InvoiceQuantity(newValue: string) {
        this.entityPM.InvoiceQuantity = newValue;
    }
    public get ItemPrice(): string {
        return this.entityPM.ItemPrice;
    }
    public set ItemPrice(newValue: string) {
        this.entityPM.ItemPrice = newValue;
    }
    public get ItemPriceCurrencyCode(): string {
        return this.entityPM.ItemPriceCurrencyCode;
    }
    public set ItemPriceCurrencyCode(newValue: string) {
        this.entityPM.ItemPriceCurrencyCode = newValue;
    }
    public get OriginCountryCode(): string {
        return this.entityPM.OriginCountryCode;
    }
    public set OriginCountryCode(newValue: string) {
        this.entityPM.OriginCountryCode = newValue;
    }

    public get OriginCountryName(): string {
        return this.entityPM.OriginCountryName;
    }
    public set OriginCountryName(newValue: string) {
        this.entityPM.OriginCountryName = newValue;
    }
}
