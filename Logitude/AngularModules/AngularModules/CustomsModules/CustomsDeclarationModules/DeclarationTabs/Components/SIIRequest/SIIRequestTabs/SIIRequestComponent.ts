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
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SIIRequestWebService, SupplierInvoiceItemsForSIIRequest } from 'Customs/Services/WebServices/SIIRequestWebService';
import { SupplierInvoiceItemLine } from 'CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent';
import { SupplierInvoiceItemsReqListWebService } from 'Customs/Services/WebServices/SupplierInvoiceItemsReqListWebService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ContactPMService } from 'Common/Services/StandardPMs/ContactPMService';
import { ContactPM } from 'Common/EntityPMs/ContactPM';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize, map } from 'rxjs/operators';

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
    public supplierInvoiceItemsReqListWebService: SupplierInvoiceItemsReqListWebService;
    public contactPmService: ContactPMService = new ContactPMService();
    public supplierinvoiceitemsWebService: SupplierInvoiceItemExtendedListService = new SupplierInvoiceItemExtendedListService();
    public siiRequestWebService: SIIRequestWebService;
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
    private contactData: ContactPM = new ContactPM();
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
    public isRequestNumberViewMode: boolean = false;
    private _ignoreLocalNameEvents = true;

    constructor(public entityArgs: EntityArgs, public CD: ChangeDetectorRef) {
        super();
        this.SelectedInvoiceItemsReqList = new ObservableCollection([]);
        this.supplierInvoiceItemsReqListWebService = new SupplierInvoiceItemsReqListWebService();
        this.siiRequestWebService = new SIIRequestWebService();
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
        this.originalSupplierInvoiceItemsCollection.Clear();


        this.supplierInvoiceItemsForSIIRequest.forEach((item, index) => {
            const supplierInvoiceItemLine = new SupplierInvoiceItemsForSIIRequestLine(item, this);
            supplierInvoiceItemLine.Counter = index + 1;
            this.supplierInvoiceItemsCollection.Insert(supplierInvoiceItemLine);
            this.originalSupplierInvoiceItemsCollection.Insert(supplierInvoiceItemLine);
        });
        this.IsCheckBoxVisible = this.supplierInvoiceItemsCollection?.Collection?.length > 0 ? true : false;
    }

    RefreshEntity() {
        this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
        this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
    }

    oldEntityPM: SIIRequestPM = new SIIRequestPM();
    ErrorsList: string[] = [];

    SetWindowArgs(args: any) {
        this._ignoreLocalNameEvents = true;
        this.entityPM = args.SIIRequest;
        this.oldEntityPM = args.SIIRequest;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.ErrorsList = args.errorMassage ? args.errorMassage : [];
        this.initfilterAgrs = args.filterAgrs;
        this.filterAgrs = this.initfilterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.SIIRequest";
        this.supplierInvoiceItemsForSIIRequest = args.supplierInvoiceItemsForSIIRequest;
        this.supplierInvoiceItemsCollection = new ObservableCollection([]);
        this.originalSupplierInvoiceItemsCollection = new ObservableCollection([]);

        if (!AppTool.IsNullOrEmpty(this.entityPM?.RequestNo)) {
            this.isAllowChange = false;
            this.IsDisplayOnly = true;
        }

        this.initFullData();
        setTimeout(() => {
            this._ignoreLocalNameEvents = false;
            this.entityPM.IsDirty = false;
        }, 0);;
    }



    initFullData() {
        const hasRequest = !AppTool.IsNullOrEmpty(this.entityPM?.RequestNo);
        const children = this.entityPM.SupplierInvoiceItemsReqLists || [];
        let invoices = this.supplierInvoiceItemsForSIIRequest || [];

        if (hasRequest && children.length > 0 && invoices.length > 0) {

            const sentChildren = children.filter(ch => ch.LineNumber && ch.LineNumber > 0);

            if (sentChildren.length > 0) {
                const linkedKeys = new Set(
                    sentChildren.map(l =>
                        `${l.DeclarationId}|${l.InvoiceCounterKey}|${l.InvoiceItemLineNumber}|${l.Tenant}`
                    )
                );

                invoices = invoices.filter(i => {
                    const key = `${this.DecalarationData.Id}|${i.InvoiceCounterKey}|${i.InvoiceLineNumber}|${this.entityPM.Tenant}`;
                    return linkedKeys.has(key);
                });

                const childByKey = new Map<string, any>();
                sentChildren.forEach(ch => {
                    const key = `${ch.DeclarationId}|${ch.InvoiceCounterKey}|${ch.InvoiceItemLineNumber}|${ch.Tenant}`;
                    childByKey.set(key, ch);
                });

                invoices.forEach(i => {
                    const key = `${this.DecalarationData.Id}|${i.InvoiceCounterKey}|${i.InvoiceLineNumber}|${this.entityPM.Tenant}`;
                    const child = childByKey.get(key);
                    if (!child) return;

                    i.StatusName = child.StatusName;
                    i.DistApprovalAttachmentPath = child.DistApprovalAttachmentPath;
                });

                this.supplierInvoiceItemsForSIIRequest = invoices;
            } else {
                this.supplierInvoiceItemsForSIIRequest = [];
            }
        }

        this.buildSupplierInvoiceItemsCollection();

        const defaultFilter = hasRequest
            ? this.filterOptionsAll
            : this.filterOptionsWithResponse;
        this.DemandStateFilterItemClicked(defaultFilter);

        if (!AppTool.IsNullOrEmpty(this.entityPM?.ContactId)) {
            this.getContactData(this.entityPM.ContactId);
        }
    }



    // #region Actions:
    CancelSiiRequest() {
        if (this.entityPM.IsDirty && !this.IsDisplayOnly) {
            const confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.SIIRequest.O.UnSavedChanges"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.SaveSiiRequest();
                    this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    this.entityPM = this.oldEntityPM;
                    this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
        else {
            this.RefreshEntity();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private closeAfterSave = false;
    onSaveClick() {
        this.closeAfterSave = true;
        this.SaveSiiRequest();
    }

    //#region SaveSiiRequest
    SaveSiiRequest() {
        this.saveSiiRequestCore()
            .then(() => {
                if (this.closeAfterSave) {
                    this.CurrentSession.CloseCurrentWindow();
                    this.closeAfterSave = false;
                }
            })
            .catch(() => {
            });
    }


    private saveSiiRequestCore(): Promise<SIIRequestPM> {
        return new Promise((resolve, reject) => {

            if (this.IsNewOrEdit === SiiRequestMode.IsNew) {
                this.siiRequestPMService.insert(this.entityPM).subscribe((response: ServiceResponse) => {
                    if (response?.ErrorsArray?.length === 0 && response?.Result) {
                        this.entityPM = response.Result as SIIRequestPM;
                        this.IsNewOrEdit = SiiRequestMode.IsEdit;
                        this.IsDisplayOnly = false;
                        this.isAllowChange = true;
                        this.RefreshEntity();
                        resolve(this.entityPM);
                    } else if (response?.ErrorsArray?.length > 0) {
                        this.validationErrors = response.ErrorsArray;
                        this.checkMandatoryCustomsFields(this.validationErrors);
                        reject(response.ErrorsArray);
                    } else {
                        const err = ['Unknown error'];
                        this.validationErrors = err;
                        reject(err);
                    }
                }, err => {
                    const msg = err?.message || 'Unknown error';
                    this.validationErrors = [msg];
                    reject([msg]);
                });
            }
            else {
                this.siiRequestPMService.update(this.entityPM).subscribe((response: ServiceResponse) => {
                    if (response?.Result) {
                        this.entityPM = response.Result as SIIRequestPM;
                        this.RefreshEntity();
                        resolve(this.entityPM);
                    } else if (response?.ErrorsArray?.length > 0) {
                        this.validationErrors = response.ErrorsArray;
                        this.checkMandatoryCustomsFields(this.validationErrors);
                        reject(response.ErrorsArray);
                    } else {
                        const err = ['Unknown error'];
                        this.validationErrors = err;
                        reject(err);
                    }
                }, err => {
                    const msg = err?.message || 'Unknown error';
                    this.validationErrors = [msg];
                    reject([msg]);
                });
            }

        });
    }

    ViewDocumentsComponent(): void {
        const windowArgs = {
            EntityPM: this.DecalarationData,
            ObjectTableName: 'Customs.Declaration',
            EntityParentPM: 'Declaration',
            IsFromStandAloneScreen: false,
            IsClose: true,
            FromSIIRequest: true,
            SkipCtor: true,
            SIIRequestPM: this.entityPM,
        };

        const logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = 'Customs.Declaration.TH.Documents';
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;

        logWindow.WindowClosed.subscribe((event: any) => this.onDocumentsWindowClosed(event));

        this.entityArgs.SkipCtor = true;
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    }

    onDocumentsWindowClosed(event: any): void {
        this.entityArgs.SkipCtor = false;
    }


    checkMandatoryCustomsFields(ValidationErrors: any[]) {
        let windowArgs: any = {};
        windowArgs.Errors = ValidationErrors;
        windowArgs.Warning = null;
        windowArgs.NoButtonVisibility = false;
        windowArgs.CancelButtonVisibility = true;
        windowArgs.SaveButtonText = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.Confirm");
        windowArgs.CancelButtonText = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.Cancel");
        windowArgs.ComponentHeight = '328px';
        let windowTitle = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ErrorsFound");
        let logWindow = new LogitudeWindow(this.CurrentSession);
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            return this.taxationWindowClosed($event) ? true : false;
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
    SendSiiRequest() {
        if (!AppTool.IsNullOrEmpty(this.entityPM?.RequestNo) || this.IsDisplayOnly || !this.isAllowChange) {
            return;
        }
        const selectedItems = (this.supplierInvoiceItemsCollection.Collection || [])
            .filter(i => i.IsSelected);

        if (selectedItems.length === 0) {
            const confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate('General.B.Close');
            confirm.ShowNoButton = false;
            confirm.Show(TextCodeTranslator.Translate('Customs.SIIRequest.O.NoRowSelected'));
            return;
        }

        const selectedRows = selectedItems.map(item => ({
            DeclarationId: this.DeclarationId,
            UiIndex: item.Counter,
            SIIRequestID: this.entityPM.Id,
            InvoiceCounterKey: item.InvoiceCounterKey,
            InvoiceItemLineNumber: item.InvoiceLineNumber,
        }));

        const afterSave = () => {
            this.siiRequestWebService
                .postSendSIIRequest(
                    this.entityPM.Id,
                    this.DeclarationId,
                    this.entityPM.Tenant,
                    selectedRows
                )
                .pipe(
                    map((resp: any) => normalizeReleasePayload(resp)),
                    finalize(() => this.CurrentSession.StopBusyIndicator())
                )
                .subscribe(
                    (payload: ReleaseRequestApiResponseDto) => {
                        const code = toNumber(payload?.ResponseCode);

                        if (code === 0) {
                            this.entityPM.IsDirty = false;

                            const reqNo = payload?.RequestNumber;
                            const reqNoStr = reqNo == null ? '' : String(reqNo);

                            const successMsg = reqNoStr !== ''
                                ? (TextCodeTranslator.Translate('Customs.SIIRequest.O.SendSuccessWithRequestNumber') || 'Request succeeded. Request number: {0}')
                                    .replace(/\{0\}/g, reqNoStr)
                                : ('Request sent successfully.');

                            const dlg = new ConfirmWindow();
                            dlg.Title = TextCodeTranslator.Translate('Customs.General.B.OK') || 'Success';
                            dlg.YesButtonText = TextCodeTranslator.Translate('Customs.General.B.OK') || 'OK';
                            dlg.ShowNoButton = false;
                            dlg.ShowInfoImage = true;
                            dlg.Show(successMsg);

                            dlg.WindowClosed.subscribe(() => {
                                if (dlg.Yes) {
                                    this.CurrentSession.CloseCurrentWindow();
                                } else {
                                    this.RefreshEntity();
                                }
                            });
                            return;
                        }

                        const errDlg = new ConfirmWindow();
                        errDlg.YesButtonText = TextCodeTranslator.Translate('General.B.Close');
                        errDlg.ShowNoButton = false;
                        errDlg.Title = TextCodeTranslator.Translate('Customs.General.B.OK');
                        errDlg.IsMultipleMessages = true;
                        errDlg.ShowErorImage = true;
                        errDlg.Show(
                            (payload && (payload as any).ValidationMessages) ||
                            TextCodeTranslator.Translate('General.B.Error')
                        );
                    },
                    (err: HttpErrorResponse) => {
                        const dlg = new ConfirmWindow();
                        dlg.YesButtonText = TextCodeTranslator.Translate('General.B.Close');
                        dlg.Title = TextCodeTranslator.Translate('Customs.General.B.OK');
                        dlg.ShowNoButton = false;
                        dlg.IsMultipleMessages = true;
                        dlg.ShowErorImage = true;
                        dlg.Show(extractMessage(err));
                        dlg.WindowClosed.subscribe(() => {
                            const body: any = err?.error;
                            if (body && body.IsFinal === true) {
                                this.CurrentSession.CloseCurrentWindow();
                            } else {
                                this.RefreshEntity();
                            }
                        });
                    }
                );
        };


        if (this.entityPM.IsDirty || this.entityPM.Id == null) {
            const confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate('General.B.Yes');
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate('Customs.SIIRequest.O.UnSavedChanges'));
            confirm.WindowClosed.subscribe(() => {
                if (confirm.Yes) {
                    this.CurrentSession.StartBusyIndicator(
                        TextCodeTranslator.Translate('Customs.SIIRequest.O.SendingRequest')
                    );
                    this.SaveSiiRequest();
                    this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
                    afterSave();
                } else {
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        } else {
            this.CurrentSession.StartBusyIndicator(
                TextCodeTranslator.Translate('Customs.SIIRequest.O.SendingRequest')
            );
            afterSave();
        }
    }


    //#endregion SendSiiRequest
    //#endregion Actions

    //#region complete data reqItem:
    onEditSupplierInvoiceItemRequest(item: SupplierInvoiceItemsForSIIRequestLine) {
        this.SelectedRow = item;
        this.getSupplierInvoiceItemsReqListPMDataAndopenLogWindow()
    }

    supplierInvoiceItemsReqListPM: SupplierInvoiceItemsReqListPM;
    async getSupplierInvoiceItemsReqListPMDataAndopenLogWindow() {
        if (AppTool.IsNullOrEmpty(this.entityPM?.Id)) {
            try {
                await this.saveSiiRequestCore();
            } catch (e) {
                console.error('Cannot open Complete Data window because SIIRequest failed to save:', e);
                return;
            }
        }
        if (AppTool.IsNullOrEmpty(this.entityPM.DeclarationId) && this.DecalarationData?.Id) {
            this.entityPM.DeclarationId = this.DecalarationData.Id;
        }

        let existingChild: SupplierInvoiceItemsReqListPM | undefined;

        if (this.entityPM && this.entityPM.SupplierInvoiceItemsReqLists) {
            existingChild = this.entityPM.SupplierInvoiceItemsReqLists.find(x =>
                x.DeclarationId === this.DecalarationData.Id &&
                x.InvoiceCounterKey === this.SelectedRow.InvoiceCounterKey &&
                x.InvoiceItemLineNumber === this.SelectedRow.InvoiceLineNumber &&
                x.Tenant === this.entityPM.Tenant
            );
        }

        if (existingChild) {
            this.supplierInvoiceItemsReqListPM = existingChild;

            const args: any = {
                Decalaration: this.DecalarationData,
                SIIRequest: this.entityPM,
                invoiceItemReq: this.SelectedRow,
                entityPMSupplierInvoiceItemsReqListPM: this.supplierInvoiceItemsReqListPM,
                IsNewOrEdit: SiiRequestMode.IsEdit,
                filterAgrs: this.initfilterAgrs,
                isAllowChange: this.isAllowChange,
                errorMassage: []
            };

            this.openLogWindow(args);
            return;
        }

        this.supplierInvoiceItemsReqListWebService
            .getBySiiRequest(
                this.DecalarationData.Id,
                this.SelectedRow.InvoiceCounterKey,
                this.SelectedRow.InvoiceLineNumber,
                this.entityPM?.Id
            )
            .subscribe(myResult => {
                const myResponse: ServiceResponse = myResult as ServiceResponse;

                let linePM: SupplierInvoiceItemsReqListPM;

                if (!myResponse?.HasError && myResponse?.Result) {
                    linePM = myResponse.Result as SupplierInvoiceItemsReqListPM;
                } else {
                    linePM = new SupplierInvoiceItemsReqListPM(this.entityPM);
                    linePM.DeclarationId = this.DecalarationData.Id;
                    linePM.SIIRequestID = this.entityPM.Id;
                    linePM.Tenant = this.entityPM.Tenant;
                    linePM.InvoiceCounterKey = this.SelectedRow.InvoiceCounterKey;
                    linePM.InvoiceItemLineNumber = this.SelectedRow.InvoiceLineNumber;
                    linePM.LineNumber = this.SelectedRow.LineNumber;
                }

                linePM.EntityParentPM = this.entityPM;

                if (!this.entityPM.SupplierInvoiceItemsReqLists) {
                    this.entityPM.SupplierInvoiceItemsReqLists = [];
                }

                const alreadyInParent = this.entityPM.SupplierInvoiceItemsReqLists.find(x =>
                    x.DeclarationId === linePM.DeclarationId &&
                    x.InvoiceCounterKey === linePM.InvoiceCounterKey &&
                    x.InvoiceItemLineNumber === linePM.InvoiceItemLineNumber &&
                    x.Tenant === linePM.Tenant
                );

                if (!alreadyInParent) {
                    this.entityPM.AddSupplierInvoiceItemsReqList(linePM);
                } else {
                    linePM = alreadyInParent;
                }

                this.supplierInvoiceItemsReqListPM = linePM;

                const args: any = {
                    Decalaration: this.DecalarationData,
                    SIIRequest: this.entityPM,
                    invoiceItemReq: this.SelectedRow,
                    entityPMSupplierInvoiceItemsReqListPM: this.supplierInvoiceItemsReqListPM,
                    IsNewOrEdit: SiiRequestMode.IsEdit,
                    filterAgrs: this.initfilterAgrs,
                    isAllowChange: this.isAllowChange,
                    errorMassage: []
                };

                this.openLogWindow(args);
            });
    }

    openLogWindow(args) {
        if (this.isOpen) return;
        this.isOpen = true;
        let logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 515;
        logWindow.Title = TextCodeTranslator.Translate("Customs.SIIRequest.O.CompletData");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestCopmleteDataItem/SIIRequestCopmleteDataItemComponent');
        args.logWindow = logWindow;
        logWindow.WindowClosed.subscribe((entityPM: SupplierInvoiceItemsReqListPM) => {
            if (!AppTool.IsNullOrEmpty(entityPM)) this.SelectedRow.entityPM.RequestRequiredStatus = entityPM.RequestRequiredStatus;

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
    Search(searchText: string): void {

        this.SearchText = AppTool.IsNullOrEmpty(searchText)
            ? ""
            : searchText.toLowerCase();

        let filtered: SupplierInvoiceItemsForSIIRequestLine[] =
            this.originalSupplierInvoiceItemsCollection.Collection.slice();


        if (this.DemandStateFilterSelectedValue !== this.filterOptionsAll) {
            filtered = filtered.filter(i => i.HasDemandState === true);
        }

        if (this.LevelSelectionFilterSelectedValue === this.filterOptionsInvoice &&
            !AppTool.IsNullOrEmpty(this.SelectedInvoiceNumber)) {

            filtered = filtered.filter(i => i.InvoiceNumber === this.SelectedInvoiceNumber);
        }

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            const term = this.SearchText;
            filtered = filtered.filter(i =>
                (i.ClassificationCode || "").toLowerCase().indexOf(term) > -1 ||
                (i.ItemCode || "").toLowerCase().indexOf(term) > -1
            );
        }

        this.supplierInvoiceItemsCollection.Clear();
        for (let idx = 0; idx < filtered.length; idx++) {
            filtered[idx].Counter = idx + 1;
            this.supplierInvoiceItemsCollection.Insert(
                new SupplierInvoiceItemsForSIIRequestLine(filtered[idx], this)
            );
        }
    }
    //#endregion Properties Filter Methods

    //#region DemandState Filter Methods
    public DemandStateFilterSelectedValue: string = this.filterOptionsAll;
    DemandStateFilterItemClicked(itemValue: string, isSearched: boolean = false) {
        if (this.DemandStateFilterSelectedValue !== itemValue || isSearched) {
            this.DemandStateFilterSelectedValue = itemValue;
            const original: SupplierInvoiceItemsForSIIRequestLine[] = this.originalSupplierInvoiceItemsCollection.Collection;
            let filtered: SupplierInvoiceItemsForSIIRequestLine[] = [];
            filtered = itemValue === this.filterOptionsAll ? original : original.filter(i => i.HasDemandState === true);

            if (this.LevelSelectionFilterSelectedValue === this.filterOptionsInvoice) {
                this.InvoicesSelectionChanged(this.currentSelectedItem);
                return;
            }

            this.supplierInvoiceItemsCollection.Clear();
            if (filtered.length > 0) {
                filtered.forEach((row, idx) => {
                    row.Counter = idx + 1; // <-- reset numbering
                    this.supplierInvoiceItemsCollection.Insert(
                        new SupplierInvoiceItemsForSIIRequestLine(row, this)
                    );
                });
            }

            if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                this.Search(this.SearchText);
            }
        }
    }

    //#endregion DemandState Filter Methods

    //#region Invoice ComboBox
    SelectedInvoiceHasDemandState: boolean = false;
    filterSupplierInvoiceItemsForSIIRequestLineByInvoiceNumber: SupplierInvoiceItemsForSIIRequestLine[] = [];
    currentSelectedItem: SupplierInvoiceItemLine;

    InvoicesSelectionChanged(selectedItem) {
        this.currentSelectedItem = selectedItem;
        if (!selectedItem) return;
        this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
        this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
        this.SelectedInvoiceHasDemandState = selectedItem.HasDemandState;
        let items = this.originalSupplierInvoiceItemsCollection.Collection.filter(i => i.InvoiceNumber === selectedItem.InvoiceNumber);
        // Filter by DemandState and InvoiceNumber:
        const original: SupplierInvoiceItemsForSIIRequestLine[] = items;
        items = this.DemandStateFilterSelectedValue === this.filterOptionsAll ? original : original.filter(i => i.HasDemandState === true);
        if (!items.length) return;
        this.supplierInvoiceItemsCollection.Clear();
        items.forEach((item, index) => {
            item.Counter = index + 1;
            this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(item, this));
        });
        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            this.Search(this.SearchText);
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
                if (this.LevelSelectionFilterSelectedValue === this.filterOptionsDeclarationConect) {
                    const original: SupplierInvoiceItemsForSIIRequestLine[] = items;
                    items = this.DemandStateFilterSelectedValue === this.filterOptionsAll ? original : original.filter(i => i.HasDemandState === true);
                }
                this.supplierInvoiceItemsCollection.Clear();
                items.forEach((item, index) => {
                    item.Counter = index + 1;
                    this.supplierInvoiceItemsCollection.Insert(new SupplierInvoiceItemsForSIIRequestLine(item, this));
                });
                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    this.Search(this.SearchText);
                }
            }
        }
    }

    openDistApprovalAttachment(item: any): void {
        const remoteUrl = item?.DistApprovalAttachmentPath;
        if (!remoteUrl) return;

        this.siiRequestWebService.getApprovalReportBlob(remoteUrl).subscribe({
            next: (blob: Blob) => {
                if (!blob || blob.size === 0) return;

                const objectUrl = URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = objectUrl;
                a.download = 'DeclarationApprovalReport.pdf';
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);

                setTimeout(() => URL.revokeObjectURL(objectUrl), 1000);
            },
            error: (err) => {
                console.error('getApprovalReportBlob failed', err);
            }
        });
    }

    //#endregion LevelSelection Filter Methods   

    //#region contact data
    getContactData(contactId: string) {
        this.contactPmService.get(contactId).subscribe((response: ServiceResponse) => {
            if (response?.Result !== null) {
                this.contactData = response.Result;
                this.setContactData();
            }
        });
    }

    setContactData() {
        if (!this.contactData) {
            return;
        }

        this.ContactEmail = this.contactData.Email ?? this.ContactEmail;
        this.ContactTel = this.contactData.BusinessPhone ?? this.ContactTel;
        this.ContactCellPhone = this.contactData.Mobile ?? this.ContactCellPhone;
        this.ContactFax = this.contactData.Fax ?? this.ContactFax;
    }


    //#endregion contact data

    //#region  SiiRequest properties
    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
        if (this.entityPM.Id != newValue) {
            this.entityPM.Id = newValue;
        }
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
        if (this.entityPM.WareHouseAddress != newValue) {
            this.entityPM.WareHouseAddress = newValue;
        }
    }

    public get WareHouseCity(): string {
        return this.entityPM?.WareHouseCity;
    }
    public set WareHouseCity(newValue: string) {
        if (this.entityPM.WareHouseCity != newValue) {
            this.entityPM.WareHouseCity = newValue;
        }
    }

    public get WareHouseCityName(): string {
        return this.entityPM?.WareHouseCityName;
    }
    public set WareHouseCityName(newValue: string) {
        if (this.entityPM.WareHouseCityName != newValue) {
            this.entityPM.WareHouseCityName = newValue;
        }
    }

    SetLocalName(entity, fieldName) {
        if (this._ignoreLocalNameEvents) return;

        const newVal = !AppTool.IsNullOrEmpty(entity) ? (entity?.LocalName ?? null) : null;
        const curVal = (this.entityPM[fieldName] ?? null);

        if (curVal !== newVal) {
            this.entityPM[fieldName] = newVal;
        }
    }

    public get IsClosed(): boolean {
        return this.entityPM.IsClosed ?? false;
    }
    public set IsClosed(newValue: boolean) {
        this.entityPM.IsClosed = newValue;
    }

    public get Remarks(): string {
        return this.entityPM?.Remarks;
    }
    public set Remarks(newValue: string) {
        if (this.entityPM.Remarks != newValue) {
            this.entityPM.Remarks = newValue;
        }
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
        return this.entityPM?.UnloadDate ?? null;
    }
    public set UnloadDate(newValue: Date) {
        if (this.entityPM.UnloadDate !== newValue) {
            this.entityPM.UnloadDate = newValue;
        }
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
        if (this.entityPM.ContactEmail != newValue) {
            this.entityPM.ContactEmail = newValue;
        }
    }

    public get ContactTel(): string {
        return this.entityPM?.ContactTel;
    }
    public set ContactTel(newValue: string) {
        if (this.entityPM.ContactTel != newValue) {
            this.entityPM.ContactTel = newValue;
        }
    }

    public get ContactCellPhone(): string {
        return this.entityPM?.ContactCellPhone;
    }
    public set ContactCellPhone(newValue: string) {
        if (this.entityPM.ContactCellPhone != newValue) {
            this.entityPM.ContactCellPhone = newValue;
        }
    }

    public get ContactFax(): string {
        return this.entityPM?.ContactFax;
    }
    public set ContactFax(newValue: string) {
        if (this.entityPM.ContactFax !== newValue) {
            this.entityPM.ContactFax = newValue;
        }
    }

    public get ContactId(): string {
        return this.entityPM?.ContactId;
    }
    public set ContactId(newValue: string) {
        if (this.entityPM.ContactId != newValue) {
            let oldValue = this.entityPM.ContactId;
            this.entityPM.ContactId = newValue;
            if (!AppTool.IsNullOrEmpty(newValue) && oldValue !== newValue) this.getContactData(newValue);
        }
    }
    //#endregion SiiRequest properties
}

//#region SupplierInvoiceItemsForSIIRequestLine properties:
export class SupplierInvoiceItemsForSIIRequestLine extends BaseComponent {
    public entityPM: SupplierInvoiceItemsForSIIRequest;
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
        this.Parent.SelectedRowsCheckBox = this.Parent.supplierInvoiceItemsCollection?.Collection?.filter(i => i.IsSelected === true);
        this.Parent.IsSelected = !(this.Parent.SelectedRowsCheckBox?.length !== this.Parent.supplierInvoiceItemsCollection.Collection?.length);
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
    public get InvoiceLineNumber(): number {
        return this.entityPM.InvoiceLineNumber;
    }
    public set InvoiceLineNumber(newValue: number) {
        this.entityPM.InvoiceLineNumber = newValue;
    }
    public get InvoiceCounterKey(): number {
        return this.entityPM.InvoiceCounterKey;
    }
    public set InvoiceCounterKey(newValue: number) {
        this.entityPM.InvoiceCounterKey = newValue;
    }
    public get ItemCode(): string {
        return this.entityPM.ItemCode;
    }
    public set ItemCode(newValue: string) {
        this.entityPM.ItemCode = newValue;
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
    public get InvoiceQuantityType(): string {
        return this.entityPM.InvoiceQuantityType;
    }
    public set InvoiceQuantityType(newValue: string) {
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
    public get InvoiceQuantityTypeName(): string {
        return this.entityPM.InvoiceQuantityTypeName;
    }
    public set InvoiceQuantityTypeName(newValue: string) {
        this.entityPM.InvoiceQuantityTypeName = newValue;
    }
    public get TradeAgreementName(): string {
        return this.entityPM.TradeAgreementName;
    }
    public set TradeAgreementName(newValue: string) {
        this.entityPM.TradeAgreementName = newValue;
    }
    public get OriginCountryName(): string {
        return this.entityPM.OriginCountryName;
    }
    public set OriginCountryName(newValue: string) {
        this.entityPM.OriginCountryName = newValue;
    }
    public get HasDemandState(): boolean {
        return this.entityPM.HasDemandState;
    }
    public set HasDemandState(newValue: boolean) {
        this.entityPM.HasDemandState = newValue;
    }
    public get RequestRequiredStatus(): string {
        return this.entityPM.RequestRequiredStatus;
    }
    public set RequestRequiredStatus(newValue: string) {
        this.entityPM.RequestRequiredStatus = AppTool.IsNullOrEmpty(newValue) ? CompleteStatuses.UnCompleted : newValue;
    }
    public get Counter(): number {
        return this.entityPM.Counter;
    }
    public set Counter(newValue: number) {
        this.entityPM.Counter = newValue;
    }
    public get StatusName(): string {
        return this.entityPM.StatusName;
    }
    public set StatusName(newValue: string) {
        this.entityPM.StatusName = newValue;
    }
    public get DistApprovalAttachmentPath(): string {
        return this.entityPM.DistApprovalAttachmentPath;
    }
    public set DistApprovalAttachmentPath(newValue: string) {
        this.entityPM.DistApprovalAttachmentPath = newValue;
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
    UnCompleted = "0",
    PartiallyCompleted = "1",
    FullyCompleted = "2"
}
export interface ReleaseRequestApiResponseDto {
    RequestNumber: number;
    ResponseCode: number;
    ValidationMessages?: string;
}

function extractMessage(err: HttpErrorResponse): string {
    return (
        err.error?.Details?.ValidationMessages ||
        err.error?.ValidationMessages ||
        err.error?.Message ||
        err.message ||
        TextCodeTranslator.Translate('General.B.Error')
    );
}
function normalizeReleasePayload(resp: any): ReleaseRequestApiResponseDto {
    const p = (resp && resp.Result) ? resp.Result : resp;
    return p as ReleaseRequestApiResponseDto;
}
function toNumber(x: any): number {
    const n = Number(x);
    return isNaN(n) ? -1 : n;
}
