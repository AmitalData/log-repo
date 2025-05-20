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
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    public siiRequestPMService: SIIRequestPMService = new SIIRequestPMService();
    public userPmService: UserPMService = new UserPMService();
    public supplierinvoiceitemsWebService: SupplierInvoiceItemExtendedListService = new SupplierInvoiceItemExtendedListService();
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext = this;
    public ObjectTableName: string = "Customs.Declaration";
    public ObjectTableNameSiiRequest: string = "Customs.SIIRequests";
    private CurrentSession = SessionLocator.SelectedSession;
    public DecalarationData: DeclarationPM;
    public IsNewOrEdit: SiiRequestMode;
    public IsDisplayOnly: boolean = false;
    public isAllowChange: boolean = false;
    public entityPM: SIIRequestPM = new SIIRequestPM();
    public initfilterAgrs: ApiQueryFilters;
    public filterAgrs: ApiQueryFilters;
    private userData: UserPM = new UserPM();
    public IsLoaded: boolean = false;
    public supplierInvoiceItemsForSIIRequest: SupplierInvoiceItemsForSIIRequest[] = [];
    public supplierInvoiceItemsCollection: ObservableCollection;
    public originalSupplierInvoiceItemsCollection: ObservableCollection;
    public IsCheckBoxVisible: boolean = false;
    public filterOptionsAll: FilterOptions = FilterOptions.All;
    public filterOptionsWithResponse: FilterOptions = FilterOptions.WithResponse;
    public filterOptionsInvoice: FilterOptions = FilterOptions.Invoice;
    public filterOptionsDeclarationConect: FilterOptions = FilterOptions.DeclarationConect;
    public isOpen: boolean;
    public isUnCompleted: CompleteStatuses = CompleteStatuses.UnCompleted;
    public isPartiallyCompleted: CompleteStatuses = CompleteStatuses.PartiallyCompleted;
    public isFullyCompleted: CompleteStatuses = CompleteStatuses.FullyCompleted;

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
        this.initfilterAgrs = args.filterAgrs;
        this.filterAgrs = this.initfilterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.SIIRequest";
        this.supplierInvoiceItemsForSIIRequest = args.supplierInvoiceItemsForSIIRequest;
        this.supplierInvoiceItemsCollection = new ObservableCollection([]);
        this.originalSupplierInvoiceItemsCollection = new ObservableCollection([]);
        this.initFullData();
    }

    initFullData() {
        this.buildSupplierInvoiceItemsCollection();
        this.DemandStateFilterItemClicked(this.filterOptionsWithResponse);
        if (!AppTool.IsNullOrEmpty(this.entityPM?.ContactId)) this.getUserData(this.entityPM.ContactId);
    }

    // #region Actions:
    CancelSaveSiiRequest() {
        this.RefreshEntity();
        this.CurrentSession.CloseCurrentWindow();
        console.log(this.entityPM);
        console.log(this.SelectedRowsCheckBox.length);
    }

    //#region SaveSiiRequest
    SaveSiiRequest() {
        if (this.IsNewOrEdit === SiiRequestMode.IsNew) {
            this.siiRequestPMService.insert(this.entityPM).subscribe((response: ServiceResponse) => {
                if (response.ErrorsArray.length === 0 && response?.Result) {
                    this.entityPM = response.Result;
                    this.IsNewOrEdit = SiiRequestMode.IsEdit;
                    this.IsDisplayOnly = false;
                    this.isAllowChange = true;
                    this.RefreshEntity();
                    this.CurrentSession.CloseCurrentWindow();
                }
                else if (response.ErrorsArray.length > 0) {
                    this.validationErrors = response.ErrorsArray;
                    this.checkMandatoryCustomsFields(this.validationErrors);
                }
            });
        }
        else if (this.IsNewOrEdit === SiiRequestMode.IsEdit) {
            this.siiRequestPMService.update(this.entityPM).subscribe((response: ServiceResponse) => {
                if (response?.Result) {
                    this.entityPM = response.Result;
                    this.RefreshEntity();
                    this.CurrentSession.CloseCurrentWindow();
                }
                else if (response.ErrorsArray.length > 0) {
                    this.validationErrors = response.ErrorsArray;
                    this.checkMandatoryCustomsFields(this.validationErrors);
                }
            });
        }
    }

    checkMandatoryCustomsFields(ValidationErrors: any[]) {
        let windowArgs: any = {};
        windowArgs.Errors = ValidationErrors;
        windowArgs.Warning = null;
        windowArgs.NoButtonVisibility = false;
        windowArgs.CancelButtonVisibility = true;
        windowArgs.SaveButtonText = "אשר";
        windowArgs.CancelButtonText = "בטל";
        windowArgs.ComponentHeight = '328px';
        let windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.Errors");
        let logWindow = new LogitudeWindow(this.CurrentSession);
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            return this.taxationWindowClosed($event) ? true : false; // TODO: change to return action like save function when true
        });
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
        this.CurrentSession.StopBusyIndicator();
    }

    validationErrors: string[] = [];
    taxationWindowClosed(event) {
        this.validationErrors = [];
        switch (event) {
            case "ok": {
                return true;
            }
            case "cancel": {
                return false;
            }
        }
    }
    //#endregion SaveSiiRequest

    //#region SendSiiRequest
    SendSiiRequest(event: any) {
        this.RefreshEntity();
        this.CurrentSession.CloseCurrentWindow();
        console.log(this.entityPM);
        console.log(this.SelectedRowsCheckBox.length);
    }
    //#endregion SendSiiRequest
    //#endregion Actions

    //#region complete data reqItem:
    onEditSupplierInvoiceItemRequest(item: SupplierInvoiceItemsForSIIRequestLine) {
        this.SelectedRow = item;
        this.openLogWindow()
    }

    openLogWindow() {
        let args: any = {
            Decalaration: this.DecalarationData,
            SIIRequest: this.entityPM,
            invoiceItemReq: this.SelectedRow,
            IsNewOrEdit: SiiRequestMode.IsEdit,
            filterAgrs: this.initfilterAgrs,
            isAllowChange: this.isAllowChange,
        };

        if (this.isOpen) return;
        this.isOpen = true;
        let logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 650;
        logWindow.Title = TextCodeTranslator.Translate("Customs.SIIRequest.O.CompletData");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestCopmleteDataItem/SIIRequestCopmleteDataItemComponent');
        args.logWindow = logWindow;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.RefreshEntity();
            this.isOpen = false;
        });
    }
    //#endregion complete data reqItem

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

    //#region  SelectedRow/SelectedRows: 
    SelectedRow: SupplierInvoiceItemsForSIIRequestLine = new SupplierInvoiceItemsForSIIRequestLine(new SupplierInvoiceItemsForSIIRequest(), this);
    SelectedRowsCheckBox: SupplierInvoiceItemsForSIIRequestLine[] = [];
    public SelectedInvoiceItemsReqList: ObservableCollection;

    OnRowSelected(item: SupplierInvoiceItemsForSIIRequestLine) {
        this.SelectedRow = item;
    }

    OnRowSelectedRowsCheckBox(items: SupplierInvoiceItemsForSIIRequestLine[]) {
        if (this.SelectedRowsCheckBox.length === 0 || this.SelectedRowsCheckBox.length === this.supplierInvoiceItemsCollection?.Collection.length) {
            this.supplierInvoiceItemsCollection?.Collection.forEach((item: SupplierInvoiceItemsForSIIRequestLine) => {
                item.IsSelected = this.IsSelected;
            });
        }
        this.SelectedRowsCheckBox = items;
    }

    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        this.OnRowSelectedRowsCheckBox(value ? this.supplierInvoiceItemsCollection?.Collection : []);
    }
    //#endregion SelectedRow/SelectedRows

    //#region Properties Filter Methods
    private SelectedCounterKey: number = null;
    private selectedInvoiceNumber: string = null;
    public get SelectedInvoiceNumber() { return this.selectedInvoiceNumber }
    public set SelectedInvoiceNumber(newValue: string) {
        this.selectedInvoiceNumber = newValue;
    }

    public SearchFilterChangedEvent: any;
    SearchText: string = "";
    Search(SearchText: string) {
        this.SearchText = AppTool.IsNullOrEmpty(SearchText) ? "" : SearchText.toLowerCase();
        const original: SupplierInvoiceItemsForSIIRequestLine[] = this.originalSupplierInvoiceItemsCollection.Collection;
        let filtered: SupplierInvoiceItemsForSIIRequestLine[] = [];
        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filtered = original.filter(i => i.ClassificationCode.toLowerCase().includes(this.SearchText) || i.ItemCode.toLowerCase().includes(this.SearchText));
            this.supplierInvoiceItemsCollection.Clear();
            if (filtered.length > 0) {
                filtered.forEach(i => this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(i, this)));
            }
        }
        else this.DemandStateFilterItemClicked(this.DemandStateFilterSelectedValue, true);
    }
    //#endregion Properties Filter Methods

    //#region DemandState Filter Methods
    public DemandStateFilterSelectedValue: string = this.filterOptionsAll;
    DemandStateFilterItemClicked(itemValue: string, isSearched: boolean = false) {
        if (this.DemandStateFilterSelectedValue !== itemValue || isSearched) {
            this.DemandStateFilterSelectedValue = itemValue;
            const original: SupplierInvoiceItemsForSIIRequestLine[] = this.originalSupplierInvoiceItemsCollection.Collection;
            let filtered: SupplierInvoiceItemsForSIIRequestLine[] = [];
            filtered = itemValue === this.filterOptionsAll ? original : original.filter(i => this.isValidDemandState(i.ReqConfirmationTypeCode));

            if (this.LevelSelectionFilterSelectedValue === this.filterOptionsInvoice) {
                this.InvoicesSelectionChanged(this.currentSelectedItem);
                return;
            }
            this.supplierInvoiceItemsCollection.Clear();
            if (filtered.length > 0)
                filtered.forEach(i => this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(i, this)));
        }
    }

    private isValidDemandState(value: string): boolean {
        return (Object.values(DemandStateFilterOptions) as string[]).includes(value);
    }
    //#endregion DemandState Filter Methods

    //#region Invoice ComboBox
    SelectedInvoiceReqConfirmation: string;
    filterSupplierInvoiceItemsForSIIRequestLineByInvoiceNumber: SupplierInvoiceItemsForSIIRequestLine[] = [];
    currentSelectedItem: SupplierInvoiceItemLine;

    InvoicesSelectionChanged(selectedItem) {
        this.currentSelectedItem = selectedItem;
        if (!selectedItem) return;
        this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
        this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
        this.SelectedInvoiceReqConfirmation = selectedItem.ReqConfirmationTypeCode;
        let items = this.originalSupplierInvoiceItemsCollection.Collection.filter(i => i.InvoiceNumber === selectedItem.InvoiceNumber);
        // Filter by DemandState and InvoiceNumber:
        const original: SupplierInvoiceItemsForSIIRequestLine[] = items;
        items = this.DemandStateFilterSelectedValue === this.filterOptionsAll ? original : original.filter(i => this.isValidDemandState(i.ReqConfirmationTypeCode));
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
    //#endregion Invoice ComboBox

    //#region LevelSelection Filter Methods
    public LevelSelectionFilterSelectedValue: string = this.filterOptionsDeclarationConect;
    LevelSelectionFilterItemClicked(itemValue: string) {
        if (this.LevelSelectionFilterSelectedValue !== itemValue) {
            this.LevelSelectionFilterSelectedValue = itemValue;
            if (itemValue !== this.filterOptionsInvoice) {
                this.SelectedInvoiceNumber = null;
                this.SelectedCounterKey = null;
                let items: SupplierInvoiceItemsForSIIRequestLine[] = this.originalSupplierInvoiceItemsCollection.Collection;
                // Filter by DemandState and InvoiceNumber:
                if (this.LevelSelectionFilterSelectedValue === this.filterOptionsDeclarationConect) {
                    const original: SupplierInvoiceItemsForSIIRequestLine[] = items;
                    items = this.DemandStateFilterSelectedValue === this.filterOptionsAll ? original : original.filter(i => this.isValidDemandState(i.ReqConfirmationTypeCode));
                }
                this.supplierInvoiceItemsCollection.Clear();
                items.forEach((item) => {
                    this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(item, this));
                });
            }
        }
    }
    //#endregion LevelSelection Filter Methods   

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
        this.ContactEmail = this.userData.Email || '';
        this.ContactTel = this.userData.BusinessPhone || '';
        this.ContactCellPhone = this.userData.Mobile || '';
        this.ContactFax = this.userData.Fax || '';
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

    public get WareHouseCityName(): string {
        return this.entityPM?.WareHouseCityName;
    }
    public set WareHouseCityName(newValue: string) {

        this.entityPM.WareHouseCityName = newValue;
    }

    SetLocalName(entity, fieldName) {
        this.entityPM[fieldName] = !AppTool.IsNullOrEmpty(entity) ? entity?.LocalName : null;
    }

    public get IsClosed(): boolean {
        return this.entityPM.IsClosed ?? false;
    }
    public set IsClosed(newValue: boolean) {
        this.entityPM.IsClosed = newValue;
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
        let oldValue = this.entityPM.ContactId;
        this.entityPM.ContactId = newValue;
        if (!AppTool.IsNullOrEmpty(newValue) && oldValue !== newValue) this.getUserData(newValue);
        this.entityPM.IsDirty = true;
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
        this.IsCompletedStatus = CompleteStatuses.UnCompleted;
    }
    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
    }

    OnRowSelected(item: SupplierInvoiceItemsForSIIRequestLine, isSelected: boolean) {
        item.IsSelected = isSelected;
        this.Parent.SelectedRowsCheckBox = this.Parent.supplierInvoiceItemsCollection?.Collection?.filter(i => i.IsSelected === true);
        this.Parent.IsSelected = !(this.Parent.SelectedRowsCheckBox?.length !== this.Parent.supplierInvoiceItemsCollection.Collection?.length);
    }

    public get InvoiceNumber(): string {
        return this.entityPM.InvoiceNumber;
    }
    public set InvoiceNumber(newValue: string) {
        this.entityPM.InvoiceNumber = newValue;
    }
    public get IsCompletedStatus(): number {
        return this.entityPM.IsCompletedStatus;
    }
    public set IsCompletedStatus(newValue: number) {
        this.entityPM.IsCompletedStatus = newValue;
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

    public get ReqConfirmationTypeCode(): string {
        return this.entityPM.ReqConfirmationTypeCode;
    }
    public set ReqConfirmationTypeCode(newValue: string) {
        this.entityPM.ReqConfirmationTypeCode = newValue;
    }

}

export enum FilterOptions {
    All = "All",
    WithResponse = "withResponse",
    Invoice = "Invoice",
    DeclarationConect = "DeclarationConect"
}
export enum DemandStateFilterOptions {
    FirstCertificate = "401",
    SecondCertificate = "402",
    ThirdCertificate = "403",
}
export enum CompleteStatuses {
    UnCompleted = 0,
    PartiallyCompleted = 1,
    FullyCompleted = 2
}
