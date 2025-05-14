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
import { SupplierInvoiceItemLine } from 'CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent';

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
    public originalSupplierInvoiceItemsCollection: ObservableCollection;
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
                    this.FillInvoiceNumbersList();
                    this.SetPropertiesEnabled();
                });
            });
        });
    }

    buildSupplierInvoiceItemsCollection(): void {
        this.supplierInvoiceItemsCollection.Clear();
        this.supplierInvoiceItemsForSIIRequest.forEach((item) => {
            const supplierInvoiceItemLine = new SupplierInvoiceItemsForSIIRequestLine(item, this);
            this.supplierInvoiceItemsCollection.Insert(supplierInvoiceItemLine);
            this.originalSupplierInvoiceItemsCollection.Insert(supplierInvoiceItemLine);
        });
        this.IsCheckBoxVisible = this.supplierInvoiceItemsCollection?.Collection?.length > 0 ? true : false;
    }

    RefreshEntity() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }

    SetWindowArgs(args: any) {
        this.entityPM = args.SIIRequest;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.filterAgrs = args.filterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.SIIRequest";
        this.supplierInvoiceItemsForSIIRequest = args.supplierInvoiceItemsForSIIRequest;
        this.supplierInvoiceItemsCollection = new ObservableCollection([]);
        this.originalSupplierInvoiceItemsCollection = new ObservableCollection([]);
        this.buildSupplierInvoiceItemsCollection();
    }

    // #region Actions:
    CancelSaveSiiRequest() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
        this.CurrentSession.CloseCurrentWindow();
    }
    SaveSiiRequest() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
        this.CurrentSession.CloseCurrentWindow();
    }

    SendSiiRequest(event: any) {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }
    //#endregion Actions

    public SearchFilterChangedEvent: any;
    SearchText: string = "";
    Search(SearchText: string) {
        this.SearchText = AppTool.IsNullOrEmpty(SearchText) ? "" : SearchText.toLowerCase();
        const original = this.originalSupplierInvoiceItemsCollection.Collection;
        const filtered = original.filter(i =>
            i.ClassificationCode.toLowerCase().includes(this.SearchText) ||
            i.ItemCode.toLowerCase().includes(this.SearchText)
        );
        this.supplierInvoiceItemsCollection.Clear();
        (filtered.length ? filtered : original).forEach(i => this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(i, this)));
    }

    SetPropertiesEnabled() {
        let enabled: boolean = !this.IsDisplayOnly;
        this.UIProperties.SetEnabled("DateOfDeclaration", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("Id", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("RequestNo", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("DeclarationId", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("Status", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("WareHouseAddress", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("WareHouseCity", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("IsClosed", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("WareHouseCityName", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("SupplierInvoiceItemsReqLists", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("Remarks", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("ListCounter", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("ContactName", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("VesselName", this.ObjectTableNameSiiRequest, enabled);
        this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ImporterId", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ContactEmail", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ContactTel", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ContactCellPhone", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ContactFax", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("UnloadDate", this.ObjectTableNameSiiRequest, !enabled);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableNameSiiRequest, !enabled);
    }

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


    SelectedRow: SupplierInvoiceItemsForSIIRequestLine = new SupplierInvoiceItemsForSIIRequestLine(new SupplierInvoiceItemsForSIIRequest(), this);
    SelectedRows: SupplierInvoiceItemsForSIIRequestLine[] = [];
    public SelectedInvoiceItemsReqList: ObservableCollection;

    OnRowSelected(items: SupplierInvoiceItemsForSIIRequestLine[]) {
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
    filterSupplierInvoiceItemsForSIIRequestLineByInvoiceNumber: SupplierInvoiceItemsForSIIRequestLine[] = [];

    InvoicesSelectionChanged(selectedItem) {
        if (!selectedItem) return;
        this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
        this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
        this.SelectedInvoiceReqConfirmation = selectedItem.ReqConfirmationTypeCode;
        const items = this.originalSupplierInvoiceItemsCollection.Collection.filter(i => i.InvoiceNumber === selectedItem.InvoiceNumber);
        if (!items.length) return;
        this.supplierInvoiceItemsCollection.Clear();
        items.forEach(item => this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(item, this)));
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
                this.supplierInvoiceItemsCollection.Clear();
                this.originalSupplierInvoiceItemsCollection.Collection.forEach((item) => {
                    this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(item, this));
                });
            }
        }
    }
    //#endregion

    //#region user data
    getUserData(userId: string) {
        this.userPmService.get(userId).subscribe((response: ServiceResponse) => {
            if (response?.Result !== null) {
                this.userData = response.Result;
                this.setUserData();
            }
        });
    }
    setUserData() {
        console.log(this.userData?.Email);
        console.log(this.userData?.BusinessPhone);
        console.log(this.userData?.Mobile);
        console.log(this.userData?.Fax);
        this.ContactEmail = this.userData.Email;
        this.ContactTel = this.userData.BusinessPhone;
        this.ContactCellPhone = this.userData.Mobile;
        this.ContactFax = this.userData.Fax;
        this.entityPM = this.entityPM;
        this.RefreshEntity();
    }
    //#endregion user data

    //#region  SiiRequest properties
    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
        this.entityPM.Id = newValue;
    }

    public get RequestNo(): string {
        return this.entityPM?.RequestNo;
    }
    public set RequestNo(newValue: string) {
        this.entityPM.RequestNo = newValue;
    }

    public get DeclarationId(): string {
        return this.entityPM.DeclarationId ?? "";
    }
    public set DeclarationId(newValue: string) {
        this.entityPM.DeclarationId = newValue;
    }

    public get Status(): string {
        return this.entityPM?.Status;
    }
    public set Status(newValue: string) {
        this.entityPM.Status = newValue;
    }

    public get WareHouseAddress(): string {
        return this.entityPM?.WareHouseAddress;
    }
    public set WareHouseAddress(newValue: string) {
        this.entityPM.WareHouseAddress = newValue;
    }

    public get WareHouseCity(): string {
        return this.entityPM?.WareHouseCity;
    }
    public set WareHouseCity(newValue: string) {
        this.entityPM.WareHouseCity = newValue;
    }

    public get IsClosed(): boolean {
        return this.entityPM.IsClosed ?? false;
    }
    public set IsClosed(newValue: boolean) {
        this.entityPM.IsClosed = newValue;
    }

    public get WareHouseCityName(): string {
        return this.entityPM?.WareHouseCityName;
    }
    public set WareHouseCityName(newValue: string) {
        this.entityPM.WareHouseCityName = newValue;
    }

    public get SupplierInvoiceItemsReqLists(): SupplierInvoiceItemsReqListPM[] {
        return this.entityPM?.SupplierInvoiceItemsReqLists ?? [];
    }
    public set SupplierInvoiceItemsReqLists(newValue: SupplierInvoiceItemsReqListPM[]) {
        this.entityPM.SupplierInvoiceItemsReqLists = newValue;
    }

    public get Remarks(): string {
        return this.entityPM?.Remarks;
    }
    public set Remarks(newValue: string) {
        this.entityPM.Remarks = newValue;
    }

    public get ListCounter(): number {
        return this.entityPM.ListCounter ?? 0;
    }
    public set ListCounter(newValue: number) {
        this.entityPM.ListCounter = newValue;
    }

    public get ImporterId(): string {
        return this.entityPM.ImporterId ?? "";
    }
    public set ImporterId(newValue: string) {
        this.entityPM.ImporterId = newValue;
    }

    public get ContactName(): string {
        return this.entityPM?.ContactName;
    }
    public set ContactName(newValue: string) {
        this.entityPM.ContactName = newValue;
    }

    public get UnloadDate(): Date {
        return this.entityPM.UnloadDate ?? new Date();
    }
    public set UnloadDate(newValue: Date) {
        this.entityPM.UnloadDate = newValue;
    }

    public get ManifestNumber(): string {
        return this.entityPM.ManifestNumber ?? "";
    }
    public set ManifestNumber(newValue: string) {
        this.entityPM.ManifestNumber = newValue;
    }

    public get VesselName(): string {
        return this.entityPM.VesselName ?? "";
    }
    public set VesselName(newValue: string) {
        this.entityPM.VesselName = newValue;
    }

    public get ContactEmail(): string {
        return this.entityPM?.ContactEmail;
    }
    public set ContactEmail(newValue: string) {
        this.entityPM.ContactEmail = newValue;
    }

    public get ContactTel(): string {
        return this.entityPM?.ContactTel;
    }
    public set ContactTel(newValue: string) {
        this.entityPM.ContactTel = newValue;
    }

    public get ContactCellPhone(): string {
        return this.entityPM?.ContactCellPhone;
    }
    public set ContactCellPhone(newValue: string) {
        this.entityPM.ContactCellPhone = newValue;
    }

    public get ContactFax(): string {
        return this.entityPM?.ContactFax;
    }
    public set ContactFax(newValue: string) {
        this.entityPM.ContactFax = newValue;
    }

    public get ContactId(): string {
        return this.entityPM?.ContactId;
    }
    public set ContactId(newValue: string) {
        this.entityPM.ContactId = newValue;
        this.getUserData(newValue);
    }
    //#endregion SiiRequest properties

    //#region  Declaration properties
    public get CustomFileNo(): string {
        return this.DecalarationData.CustomFileNo;
    }
    public set CustomFileNo(newValue: string) {
        this.DecalarationData.CustomFileNo = newValue;
    }
    //#endregion declaration properties

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
//#endregion SupplierInvoiceItemsReqListLine properties

//#region SupplierInvoiceItemsForSIIRequestLine properties:
export class SupplierInvoiceItemsForSIIRequestLine extends BaseComponent {
    public entityPM: SupplierInvoiceItemsForSIIRequest;
    public ObjectTableName: string = "Customs.CertificateOfOriginItem";
    public DataContext = this;
    Parent: SIIRequestComponent;
    constructor(EntityPM: SupplierInvoiceItemsForSIIRequest, parent: SIIRequestComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }
    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
    }

    OnRowSelected(item: SupplierInvoiceItemsForSIIRequestLine, isSelected: boolean) {
        item.IsSelected = isSelected;
        if (item.IsSelected) {
            // remove item from selected rows:
            this.Parent?.SelectedRows?.push(item);
        }
        else {
            // remove item from selected rows:
            const index = this.Parent?.SelectedRows?.findIndex((x: SupplierInvoiceItemsForSIIRequestLine) => x.entityPM.LineNumber == item.entityPM.LineNumber);
            if (index != null && index > -1) {
                this.Parent?.SelectedRows.slice(index);
            }
        }
        this.Parent?.OnRowSelected(this.Parent.SelectedRows);
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
