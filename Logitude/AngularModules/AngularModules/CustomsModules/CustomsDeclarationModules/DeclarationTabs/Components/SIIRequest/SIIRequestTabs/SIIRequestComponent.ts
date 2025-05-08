import { Component, QueryList, ViewChildren, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { SIIRequestPMService } from 'Customs/Services/StandardPMs/SIIRequestPMService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { SiiRequestMode } from '../SIIRequestTabComponent';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { SupplierInvoiceItemList } from 'Customs/EntityLists/Extended/SupplierInvoiceItemList';
import { SupplierInvoiceItemExtendedListService } from 'Customs/Services/ExtendedLists/SupplierInvoiceItemExtendedListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from 'Infrastructure/Tools';
import { UserPM } from 'Common/EntityPMs/UserPM';
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'SIIRequestComponent',
    templateUrl: './SIIRequestComponent.html',
    styleUrls: ['./SIIRequestComponent.scss'],
    providers: [EntityArgs],
})


export class SIIRequestComponent extends BaseComponent implements OnInit {
    private declarationWebService: DeclarationWebService = new DeclarationWebService; // TODO: Delete after test
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public DataContext = this;
    public ObjectTableName: string = "Customs.Declaration";
    siiRequestPMService: SIIRequestPMService = new SIIRequestPMService();
    supplierinvoiceitemsWebService: SupplierInvoiceItemExtendedListService = new SupplierInvoiceItemExtendedListService();
    public entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DecalarationData: DeclarationPM;
    public IsNewOrEdit: SiiRequestMode;
    public IsDisplayOnly: boolean = false;
    public isAllowChange: boolean = false;
    public entityPM: SIIRequestPM = new SIIRequestPM();
    public supplierInvoiceItemsList: SupplierInvoiceItemList[] = [];
    public filterAgrs: ApiQueryFilters;
    private userData: UserPM = new UserPM();

    constructor(public entityArgs: EntityArgs, public CD: ChangeDetectorRef) {
        super();
    }

    ngOnInit(): void {
        this.initiallizeComponent();
    }

    initiallizeComponent() {

        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
            this.BuildColumns();
            this.userData = SessionLocator.LoggedUserPM;
            this.getAllSupplierinvoiceItemsByDeclarationId(this.DecalarationData.Id, this.DecalarationData.Tenant);
            this.FillInvoiceNumbersList();
        });

    }

    getAllSupplierinvoiceItemsByDeclarationId(declarationId: string, tenant: number) {
        debugger
        let filterAgrs: ApiQueryFilters = this.filterAgrs;
        filterAgrs.addAdditionalFilter("DeclarationId", declarationId, null, null, "Equals", false, false, false, "string", false);
        filterAgrs.addAdditionalFilter("Tenant", tenant, null, null, "Equals", true, false, false, "string");
        this.supplierinvoiceitemsWebService.getByFilters(this.filterAgrs).subscribe((response: ServiceResponse) => {
            if (response?.Result) {

                debugger
                this.supplierInvoiceItemsList = response?.Result;
                console.log(this.supplierInvoiceItemsList);
            }
        });
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



    // #region table row mangment:   
    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.InvoiceNumber"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'OriginCountryCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryCode"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'OriginCountryName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryName"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TradeAgreementCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementCode"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TradeAgreementName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementName"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
    }

    DataSource = {
        pageSize: 10,
        rowCount: null,
        sortingCol: "InvoiceNumber",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            return this.supplierInvoiceItemsList;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        return new Promise((resolve, reject) => {
            resolve(this.supplierInvoiceItemsList);
        });
    }
    // # endregion table row mangment:

    SearchText: string = "";
    Search(SearchText: string) {
        this.SearchText = !AppTool.IsNullOrEmpty(SearchText) ? SearchText.toLowerCase() : SearchText;
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }
    public SearchFilterChangedEvent: any;

    dataCount: number;

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
                // this.GetCertificates($event);
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
            // this.GetCertificates($event);
            this.CD.reattach();
            this.RefreshEntity();
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestCopmleteDataItemComponent');
    }

    ViewInitCompleted($event) { }
    public preventSelect: boolean = false;
    public SelectedRow: any = null;
    OnRowSelected(CurrentRow) {
        if (this.preventSelect == false) {
            this.SelectedRow = CurrentRow.rowData;
            this.CurrentSession.StartBusyIndicatorLoading();
            if (!AppTool.IsNullOrEmpty(this.supplierInvoiceItemsList)) {
                this.CurrentSession.StartBusyIndicatorLoading();
                var windowArgs: any = {};
                windowArgs.EntityPM = this.entityPM;
                windowArgs.declarationPM = this.DecalarationData;
                var logWindow = new LogitudeWindow();
                logWindow.Width = 400;
                logWindow.Height = 800;
                logWindow.Title = 'השלמת נתוני בקשה' // TODO: Change to Hebrew text code
                windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                logWindow.ShowCloseButton = false;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => {
                    // this.LoadConnectedItems($event);
                    this.CD.reattach();
                    //  this.RefreshEntity();
                });
                this.CD.detach();
                logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestCopmleteDataItemComponent');
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                var window = new MessageWindow();
                window.Show("There is no invoice with such key in this declaration!!");
                this.preventSelect = false;
            }
            this.CurrentSession.StopBusyIndicator();
        }
    }

    public connectedItems: ObservableCollection;
    public ExcludedItems: ObservableCollection;
    ignoreCount: boolean = false;
    IsCheckBoxVisible: boolean = false;
    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        if (this.isSelected) {
            this.dataCount = this.DataSource.rowCount;
        }
    }

    onCheckBoxChecked($event) { }

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
            // this.ReloadCertificateTickets(false);
        }
    }
    //#region Invoice ComboBox
    SelectedInvoiceReqConfirmation: string;
    InvoicesSelectionChanged(selectedItem) {
        if (selectedItem != null) {
            debugger
            this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
            this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
            this.SelectedInvoiceReqConfirmation = selectedItem.ReqConfirmationTypeCode;
            // this.ReloadCertificateTickets(false);
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
                this.ReloadCertificateTickets(false);
            }
        }
    }
    IsFirstTime: boolean = true;
    ReloadCertificateTickets(isFirstTime: boolean) {
        this.IsFirstTime = isFirstTime;
        // this.GetCertificates(null); מסמסכים קשורים
    }

    //#endregion


    //#region  SiiRequest properties
    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
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

    // updateIsDirty() {
    //     this.entityPM.IsDirty = true;
    //     this.Parent.entityPM.IsDirty = true;
    // }
}
