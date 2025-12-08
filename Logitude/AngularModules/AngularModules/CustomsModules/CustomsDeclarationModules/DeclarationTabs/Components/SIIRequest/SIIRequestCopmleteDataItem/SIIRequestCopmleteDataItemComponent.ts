import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SupplierInvoiceItemsReqListPM } from 'Customs/EntityPMs/SupplierInvoiceItemsReqListPM';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SIIRequestWebService } from 'Customs/Services/WebServices/SIIRequestWebService';
import { SupplierInvoiceItemsReqListWebService } from 'Customs/Services/WebServices/SupplierInvoiceItemsReqListWebService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { CompleteStatuses, SupplierInvoiceItemsForSIIRequestLine } from '../SIIRequestTabs/SIIRequestComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { AppTool } from 'Infrastructure/Tools';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { SIIRequestPMService } from 'Customs/Services/StandardPMs/SIIRequestPMService';


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
    public siiRequestPMService: SIIRequestPMService = new SIIRequestPMService();
    public supplierInvoiceItemsReqListWebService: SupplierInvoiceItemsReqListWebService;
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
        this.entityPM = new SupplierInvoiceItemsReqListPM(this.currentSiiRequest);
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
    oldRequestRequiredStatus: string;

    SetWindowArgs(args: any) {
        this.currentSiiRequest = args.SIIRequest;
        this.DecalarationData = args.Decalaration;
        this.invoiceItemReq = args.invoiceItemReq;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.IsDisplayOnly = !this.isAllowChange;
        this.filterAgrs = args.filterAgrs;
        this.entityPM = args.entityPMSupplierInvoiceItemsReqListPM;
        this.oldRequestRequiredStatus = this.entityPM?.RequestRequiredStatus;
        this.SetPropertiesEnabled();
    }

    SetPropertiesEnabled() {
    const editable = this.isAllowChange; // false in display-only

    // Editable fields (locked when display-only)
    this.UIProperties.SetEnabled("ItemNo", this.ObjectTableName, editable);
    this.UIProperties.SetEnabled("ProductFileNumber", this.ObjectTableName, editable); // even though textbox has ForceDisabled, this keeps it consistent
    this.UIProperties.SetEnabled("ItemName", this.ObjectTableName, editable);
    this.UIProperties.SetEnabled("ManufactureCountryCode", this.ObjectTableName, editable);
    this.UIProperties.SetEnabled("ManufacturerName", this.ObjectTableName, editable);
    this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, editable);
    this.UIProperties.SetEnabled("DutchRequested", this.ObjectTableName, editable);

    // Always read-only (as in your HTML: ForceDisabled / just for info)
    this.UIProperties.SetEnabled("InvoiceQuantityType", this.ObjectTableName, false);
    this.UIProperties.SetEnabled("InvoiceQuantity", this.ObjectTableName, false);
    this.UIProperties.SetEnabled("StatisticQuantity", this.ObjectTableName, false);
    this.UIProperties.SetEnabled("StatisticQuantityType", this.ObjectTableName, false);
}

    // #endregion initialization data


    //region mandatory fields check:
    errorsList: string[] = [];
    checkMandatoryFields() {
        this.errorsList = [];
        let missingField: string = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ValueMustBeEntered");
        let fieldName = 'Customs.SupplierInvoiceItemsReqList.F.';
        const mandatoryFields = [
            { field: this.ManufactureCountryCode, name: TextCodeTranslator.Translate(fieldName + "ManufactureCountryCode") },
            { field: this.ItemNo, name: TextCodeTranslator.Translate(fieldName + "ItemNo") },
            { field: this.ItemName, name: TextCodeTranslator.Translate(fieldName + "ItemName") },
            { field: this.InvoiceQuantity, name: TextCodeTranslator.Translate(fieldName + "InvoiceQuantity") },
            { field: this.InvoiceQuantityType, name: TextCodeTranslator.Translate(fieldName + "InvoiceQuantityType") },
        ];
        this.errorsList = mandatoryFields.filter(({ field }) => AppTool.IsNullOrEmpty(field))?.map(({ name }) => `${missingField} ${name}`);
        this.RequestRequiredStatus = this.errorsList?.length === 0 && !AppTool.IsNullOrEmpty(this.ProductFileNumber) ? CompleteStatuses.FullyCompleted : this.errorsList?.length > 0 || AppTool.IsNullOrEmpty(this.ProductFileNumber) ? CompleteStatuses.PartiallyCompleted : CompleteStatuses.UnCompleted;
    }


    displayErrorsMsg() {
        this.openErrorsMsgWindow();
    }

    openErrorsMsgWindow() {
        if (this.errorsList?.length > 0) {
            this.errorsList.push(TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ContinueSave") + "?");
        }
        let windowArgs: any = {};
        windowArgs.Errors = this.errorsList;
        windowArgs.Warning = this.validationErrors;
        windowArgs.NoButtonVisibility = false;
        windowArgs.CancelButtonVisibility = true;
        windowArgs.SaveButtonText = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.Confirm");
        windowArgs.CancelButtonText = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.Cancel");
        windowArgs.ComponentHeight = '328px';
        let windowTitle = TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ErrorsFound");
        let logWindow = new LogitudeWindow(this.CurrentSession);
        logWindow.Width = 440;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;

        logWindow.WindowClosed.subscribe(($event: any) => {
            return this.taxationWindowClosed($event) ? this.SaveSupplierInvoiceItemsReqList() : false;
        });
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }

    validationErrors: string[] = [];
    taxationWindowClosed(event) {
        this.validationErrors = [];
        this.errorsList = [];
        switch (event) {
            case "ok": {
                return true;
            }
            case "cancel": {
                return false;
            }
        }
    }
    //#endregion mandatory fields check

    //#region acations methods:
    async SaveAndSearchSupplierInvoiceItemsReqList(): Promise<void> {
        if (!this.isAllowChange) {
            return;
        }

        this.validationErrors = [];
        this.checkMandatoryFields();
        if (this.errorsList.length > 0) {
            this.displayErrorsMsg();
            return;
        }

        const modelCode = this.ItemNo;
        const importerNumber = this.currentSiiRequest?.ImporterId || '';
        const originCountry = this.entityPM?.OriginCountryCode || '';
        const declarationId = this.entityPM?.DeclarationId || this.DecalarationData?.Id || '';

        if (!modelCode) {
            this.generalErrors = [
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ValueMustBeEntered") + " " +
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.F.ItemNo")
            ];
            this.openErrorsMsgWindow();
            return;
        }

        try {
            this.CurrentSession.StartBusyIndicator(
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.SendingRequest")
            );

            const sr = await this.supplierInvoiceItemsReqListWebService
                .LookupProductFileByModel(modelCode, importerNumber, originCountry, declarationId)
                .toPromise();

            this.CurrentSession.StopBusyIndicator();

            const productFileId = sr && sr.Result as any;

            if (productFileId) {
                this.ProductFileNumber = productFileId;
                this.checkMandatoryFields();

                if (this.errorsList.length === 0 && !AppTool.IsNullOrEmpty(this.ProductFileNumber)) {
                    this.RequestRequiredStatus = CompleteStatuses.FullyCompleted;
                }

                this.generalErrors = [];
                this.SaveSupplierInvoiceItemsReqList();
                return;
            }

            this.validationErrors = [
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ProductNotFound") + ".\n " +
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ContinueSave") + "?"
            ];
            this.displayErrorsMsg();

        } catch (error) {
            this.CurrentSession.StopBusyIndicator();
            const errorBody =
                (error && error.error && (error.error.error || error.error.ErrorMessage))
                || (error && error.message)
                || JSON.stringify(error);

            console.error('Product file lookup error:', errorBody);

            this.validationErrors = [
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ProductNotFound") + ".\n " +
                TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.ContinueSave") + "?"
            ];
            this.displayErrorsMsg();
        }
    }

    private async handleProductFileNotFound(modelCode: string, errorBody: string | null): Promise<void> {
        if (errorBody) {
            console.error('Product file lookup error:', { modelCode, errorBody });
            this.generalErrors = [errorBody];
        }

        const confirm = new ConfirmWindow();
        confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        confirm.NoButtonText = TextCodeTranslator.Translate("General.B.No");
        confirm.Show(
            TextCodeTranslator.Translate("Customs.ProductFile.NotFound")
            || "Product File not found in the Standards Institute. Do you still want to save the entered data?"
        );

        await new Promise<void>(resolve => {
            confirm.WindowClosed.subscribe(() => {
                const proceed = confirm.Yes === true;
                console.warn('ProductFile not found – user decision:', { modelCode, proceed });
                if (proceed) {
                    this.SaveSupplierInvoiceItemsReqList();
                }
                resolve();
            });
        });
    }

    generalErrors: string[] = [];
    SaveSupplierInvoiceItemsReqList() {
        const siiRequestId = this.currentSiiRequest?.Id;
        if (AppTool.IsNullOrEmpty(siiRequestId)) {
            this.siiRequestPMService.insert(this.currentSiiRequest).subscribe({
                next: (response: ServiceResponse) => {
                    if (!response?.HasError && response?.Result) {
                        this.currentSiiRequest = response.Result;
                        this.entityPM.SIIRequestID = this.currentSiiRequest?.Id;
                        this.saveItemCompletionData();
                    }
                    else if (response?.ErrorsArray.length > 0) {
                        console.error('Error saving SII request:', response?.ErrorsArray);
                        this.generalErrors = response?.ErrorsArray || ['Unknown error'];
                    }
                },
                error: (err: any) => {
                    console.error('Error saving SII request:', err);
                    this.generalErrors = [err?.message || 'Unknown error'];
                }
            });
        } else {
            this.entityPM.SIIRequestID = siiRequestId;
            this.saveItemCompletionData();
        }
    }

    private saveItemCompletionData() {



        this.siiRequestPMService.update(this.currentSiiRequest).subscribe({
            next: (myResult: ServiceResponse) => {
                if (!myResult?.HasError && myResult?.Result) {
                    this.generalErrors = [];
                } else {
                    this.generalErrors = myResult?.ErrorsArray || ['Unknown error'];
                }

                this.RefreshEntity();
                this.CurrentSession.CloseCurrentWindowData(this.entityPM);
            },
            error: (error: any) => {
                this.generalErrors = [error?.message || 'Unknown error'];
                console.error('Error saving SIIRequest with SupplierInvoiceItemsReqList:', error);
            }
        });
    }

    oldEntityPM: SupplierInvoiceItemsReqListPM = new SupplierInvoiceItemsReqListPM(this.currentSiiRequest);
    CancelSupplierInvoiceItemsReqList() {
        if (!this.isAllowChange || this.IsDisplayOnly) {
            this.CurrentSession.CloseCurrentWindow();
            return;
        }

        if (this.entityPM.IsDirty) {
            const confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsReqList.O.UnSavedChanges"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.SaveAndSearchSupplierInvoiceItemsReqList();
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
        this.entityPM.IsDirty = true;
    }
    public get ManufactureCountryCode(): string {
        return this.entityPM?.ManufactureCountryCode;
    }
    public set ManufactureCountryCode(newValue: string) {
        this.entityPM.ManufactureCountryCode = newValue;
        this.entityPM.IsDirty = true;
    }
    public get ManufactureCountryName(): string {
        return this.entityPM?.ManufactureCountryName;
    }
    public set ManufactureCountryName(newValue: string) {
        this.entityPM.ManufactureCountryName = newValue;
        this.entityPM.IsDirty = true;
    }
    public get ManufacturerName(): string {
        return this.entityPM?.ManufacturerName;
    }
    public set ManufacturerName(newValue: string) {
        this.entityPM.ManufacturerName = newValue;
        this.entityPM.IsDirty = true;
    }
    public get Remarks(): string {
        return this.entityPM?.Remarks;
    }
    public set Remarks(newValue: string) {
        this.entityPM.Remarks = newValue;
        this.entityPM.IsDirty = true;
    }
    public get ItemNo(): string {
        return this.entityPM?.ItemNo;
    }
    public set ItemNo(newValue: string) {
        this.entityPM.ItemNo = newValue;
        this.entityPM.IsDirty = true;
    }
    public get ItemName(): string {
        return this.entityPM?.ItemName;
    }
    public set ItemName(newValue: string) {
        this.entityPM.ItemName = newValue;
        this.entityPM.IsDirty = true;
    }
    public get InvoiceQuantity(): number {
        return this.entityPM?.InvoiceQuantity;
    }
    public set InvoiceQuantity(newValue: number) {
        this.entityPM.InvoiceQuantity = newValue;
        this.entityPM.IsDirty = true;
    }
    public get InvoiceQuantityType(): string {
        return this.entityPM?.InvoiceQuantityType;
    }
    public set InvoiceQuantityType(newValue: string) {
        this.entityPM.InvoiceQuantityType = newValue;
        this.entityPM.IsDirty = true;
    }
    public get StatisticQuantity(): number {
        return this.entityPM?.StatisticQuantity;
    }
    public set StatisticQuantity(newValue: number) {
        this.entityPM.StatisticQuantity = newValue;
        this.entityPM.IsDirty = true;
    }
    public get StatisticQuantityType(): string {
        return this.entityPM?.StatisticQuantityType;
    }
    public set StatisticQuantityType(newValue: string) {
        this.entityPM.StatisticQuantityType = newValue;
        this.entityPM.IsDirty = true;
    }
    public get DutchRequested(): boolean {
        return this.entityPM?.DutchRequested;
    }
    public set DutchRequested(newValue: boolean) {
        this.entityPM.DutchRequested = newValue;
        this.entityPM.IsDirty = true;
    }
    public get DutchGroupItem(): number {
        return this.entityPM?.DutchGroupItem;
    }
    public set DutchGroupItem(newValue: number) {
        this.entityPM.DutchGroupItem = newValue;
        this.entityPM.IsDirty = true;
    }
    public get RequestRequiredStatus(): string {
        return this.entityPM?.RequestRequiredStatus;
    }
    public set RequestRequiredStatus(newValue: string) {
        this.entityPM.RequestRequiredStatus = newValue;
        this.entityPM.IsDirty = true;
    }
    //#endregion SiiRequest properties
}
