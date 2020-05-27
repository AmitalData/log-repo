//import {Component, ViewChildren, QueryList,ChangeDetectorRef}  from '@angular/core';
//import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
//import {LocationDirective} from '../../../../../Infrastructure/Utilities/LocationDirective';
//import {AppTool, FontTool} from '../../../../../Infrastructure/Tools';
//import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
//import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
//import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
//import {SupplierInvoicePMService} from '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
//import {CustomsDocumentPointersExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsDocumentPointersExtendedPMService';
//import {CustomsDocumentPointerPM} from '../../../../../Customs/EntityPMs/CustomsDocumentPointerPM';
//import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
//import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
//import {Validator} from '../../../../../Infrastructure/Validators/Validator';

//import { PaymentOrderPM } from '../../../../../Customs/EntityPMs/PaymentOrderPM';
//import { PaymentOrderList } from '../../../../../Customs/EntityLists/PaymentOrderList';
//import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
//import { AmitalGatewayUtil, UnifreightMessageM} from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
//import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';

//@Component({
//    
//    templateUrl: './EditPaymentOrderComponent.html',
//})
//export class EditPaymentOrderComponent extends BaseComponent {
//    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
//    LayoutDirection: string = 'ltr';
//    public DataContext: any = this;
//    public ObjectTableName: string = "Customs.PaymentOrder";
//    public ValidationErrorsList: string[] = [];
//    public EntityPM: PaymentOrderPM;
//    public declarationPM: DeclarationPM;
//    public TabsItemsSource: TabItem[] = [];
//    public IsDisplayOnly: boolean = false;
//    public IsNewEntity: boolean = false;
//    FIELD_IS_REQUIERD: string;

//    // services
//    public entityResourceService: EntityResourceService = new EntityResourceService();
//    supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
//    public customsDocumentPointersExtendedPMService: CustomsDocumentPointersExtendedPMService = new CustomsDocumentPointersExtendedPMService();


//    constructor(private cd: ChangeDetectorRef) {
//        super();

//        this.LayoutDirection = SessionLocatorrrr.GlobalSetting == undefined ? "ltr" : SessionLocatorrrr.GlobalSetting.LayoutDirection;
//        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
//    }

//    SetWindowArgs(args: any) {

//        if (!AppTool.IsNullOrEmpty(args)) {
//            this.EntityPM = args.EntityPM;
//            this.declarationPM = args.declarationPM;
//            this.IsDisplayOnly = args.IsDisplayOnly;
//            this.IsNewEntity = args.IsNewEntity;
//            this.SetScreenFieldsEditability();

//            if (!AppTool.IsNullOrEmpty(args.DeclarationError)) {
//                this.ShowXMLErrors(args.DeclarationError);
//            }

//        }
//        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
//            this.entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => {
//                this.BuildTabs();
//                this.RunComponent();
//            });
//        });


//    }

//    SetScreenFieldsEditability() {

//    }

//    BuildTabs() {
//        this.TabsItemsSource = [];
//        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.PaymentOrder.TH.General"));
//        this.TabsItemsSource.push(new TabItem("EVENTS", "Customs.PaymentOrder.TH.Events"));
//        this.TabsItemsSource.push(new TabItem("COMMUNICATIONS", "Customs.PaymentOrder.TH.Communications"));
//        this.TabsItemsSource.push(new TabItem("REQUEST", "Customs.PaymentOrder.TH.RequestSheets"));
//        this.selectedTabCode = "GENERAL";
//    }

//    RunComponent() {
//        if (this.AllLocations) {

//            if (this.AllLocations.length == 0) {
//                this.RunComponentTimer();
//            }

//            else {
//                this.isViewInited = true;
//                this.InitializeComponent();
//            }
//        }

//        else {
//            this.RunComponentTimer();
//        }
//    }

//    private Retries: number = 0;
//    private timerToken: any;
//    private RunComponentTimer() {
//        this.Retries++;

//        if (this.timerToken) {
//            clearTimeout(this.timerToken);
//        }

//        if (this.Retries < 20) {
//            this.timerToken = setTimeout(() => this.RunComponent(), 1);
//        }
//    }

//    private isViewInited = false;
//    InitializeComponent() {
//        if (this.isViewInited) {
//            this.SelectionChanged();
//        }
//    }
//    private selectedTabCode: string;
//    get SelectedTabCode() { return this.selectedTabCode; }
//    set SelectedTabCode(newValue: string) {
//        if (this.selectedTabCode != newValue) {
//            this.selectedTabCode = newValue;
//            this.SelectionChanged();
//        }
//    }

//    private GENERAL: any = null;
//    public SelectedTab: TabItem;
//    SelectionChanged() {
//        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
//            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
//            if (myLocation != null) {
//                switch (this.SelectedTabCode) {

//                    case "GENERAL": {
//                        if (this.GENERAL == null) {
//                            SessionLocator.DynamicLoader.Load('./Customs/Components/PaymentOrder/EditTabs/General/PaymentOrdersGeneralTabComponent', myLocation.viewContainerRef)
//                                .then(cmpRef => {
//                                    this.GENERAL = cmpRef.instance;
//                                    this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, true);
//                                });
//                        }
//                        break;
//                    }

//                    case "EVENTS": {

//                        break;

//                    }
//                    case "COMMUNICATIONS": {

//                        break;

//                    }
//                    case "REQUEST": {
//                        SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent', myLocation.viewContainerRef)
//                            .then(cmpRef => {
//                                //this.REQUEST = cmpRef.instance;
//                                //this.REQUEST.InitTab(this.EntityPM, this, this.IsDisplayOnly, true);
//                            });
//                        break;
//                    }
//                }


//            }
//        }
//    }

//    //#region properties

//    //#endregion

//    // Buttons Handlers
//    OkButtonClicked() {
//        this.SaveAndNew = false;

//        this.SaveButtonClicked();

//    }
//    CancelButtonClicked() {
//        this.EntityPM.RejectChanges();
//        this.CurrentSession.CloseCurrentWindowEmit('cancel');
//    }

//    //#region Save Code
//    SaveAndNew: boolean = false;
//    _IsInitiateNewInstance: boolean = false;
//    loadingNextItems: boolean = false;
//    closeWindow: boolean;

//    sendMode: boolean;
//    public get SendMode() { return this.sendMode; }
//    public set SendMode(value: boolean) {
//        this.sendMode = value;
//    }

//    SaveButtonClicked() {
//        this.CurrentSession.StartBusyIndicatorSaving();

//        var valid = this.PreSaveAndNewValidate();
//        var checkFreightValues = true;


//        if (this.declarationPM.SupplierInvoices.length > 1) {
//            for (var item of this.declarationPM.SupplierInvoices) {
//                if (item.IncotermCode != null && (item.IncotermCode.startsWith("E") || item.IncotermCode.startsWith("F"))) {
//                    if (item.SupplierInvoiceFreightAmounts.length > 0 && item.InsuranceAmount != null) {
//                        checkFreightValues = false;
//                        break;
//                    }
//                }

//                else if (item.IncotermCode != null && (item.IncotermCode == "CPT" || item.IncotermCode == "CFR")) {
//                    if (item.InsuranceAmount != null) {
//                        checkFreightValues = false;
//                        break;
//                    }
//                }
//            }
//        }

//        if (valid) {
//            if (checkFreightValues && !this.loadingNextItems) {
//                if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F"))) {
//                    if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0 || this.EntityPM.InsuranceAmount == null) {

//                        this.CurrentSession.StopBusyIndicator();

//                        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
//                        var confirmWindow = new ConfirmWindow();
//                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
//                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
//                        confirmWindow.Show(msg);
//                        confirmWindow.WindowClosed.subscribe((event: any) => {

//                            if (confirmWindow.Yes) {
//                                this.ConfirmWindowYesButton();
//                            }
//                        });

//                        this.closeWindow = false;
//                    }
//                    else {
//                        this.closeWindow = true;
//                        //this.SaveChanges();
//                        this.SaveChangesSync();
//                    }

//                }

//                else if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
//                    if (this.EntityPM.InsuranceAmount == null) {
//                        this.CurrentSession.StopBusyIndicator();

//                        this.CurrentSession.StopBusyIndicator();

//                        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
//                        var confirmWindow = new ConfirmWindow();
//                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
//                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
//                        confirmWindow.Show(msg);
//                        confirmWindow.WindowClosed.subscribe((event: any) => {

//                            if (confirmWindow.Yes) {
//                                this.ConfirmWindowYesButton();
//                            }
//                        });

//                        this.closeWindow = false;
//                    }
//                    else {
//                        if (!this.loadingNextItems) {
//                            this.closeWindow = true;
//                        }
//                        else {
//                            this.closeWindow = true;
//                        }
//                        //this.SaveChanges();
//                        this.SaveChangesSync();
//                    }
//                }

//                else {
//                    if (this.SendMode) {
//                        this.closeWindow = false;
//                    }
//                    else if (!this.loadingNextItems) {
//                        this.closeWindow = true;
//                    }
//                    //this.SaveChanges();
//                    this.SaveChangesSync();
//                }

//            }

//            else {
//                if (this.SendMode) {
//                    this.closeWindow = false;
//                }
//                else if (!this.loadingNextItems) {
//                    this.closeWindow = true;
//                }
//                this.SaveChangesSync();
//            }

//        }
//        else {
//            this.CurrentSession.StopBusyIndicator();

//        }
//    }

//    private SaveChanges() {


//        if (this.IsNewEntity) {

//            this.supplierInvoicePMService.insert(this.EntityPM).subscribe((myResult:any) => {

//                var res: ServiceResponse = myResult;
//                if (!res.HasError) {
//                    var entity = res.Result;

//                    console.log("..Saved Successfully ", entity);
//                    return true;
//                }
//                else {
//                    this.ValidationErrorsList = res.ErrorsArray;
//                }
//                this.CurrentSession.StopBusyIndicator();
//                return false;
//            });


//        } else {

//            this.supplierInvoicePMService.update(this.EntityPM).subscribe((myResult:any) => {

//                var res: ServiceResponse = myResult;
//                if (!res.HasError) {
//                    var entity = res.Result;

//                    console.log("..Saved Successfully ", entity);
//                    return true;
//                }
//                else {
//                    this.ValidationErrorsList = res.ErrorsArray;
//                }
//                this.CurrentSession.StopBusyIndicator();
//                return false;
//            });

//        }
//    }

//    SavingPromise(): Promise<boolean> {
//        return new Promise((resolve) => {
//            if (this.IsNewEntity) {
//                this.supplierInvoicePMService.insert(this.EntityPM).subscribe((myResult:any) => {
//                    var res: ServiceResponse = myResult;
//                    if (!res.HasError) {
//                        var entity = res.Result;

//                        console.log("..Saved Successfully ", entity);
//                        resolve(true);
//                        if (this.closeWindow) {
//                            this.CurrentSession.CloseCurrentWindow();
//                        }
//                    }
//                    else {
//                        this.ValidationErrorsList = res.ErrorsArray;
//                    }
//                    this.CurrentSession.StopBusyIndicator();
//                    resolve(false);
//                });

//            } else {
//                this.supplierInvoicePMService.update(this.EntityPM).subscribe((myResult:any) => {

//                    var res: ServiceResponse = myResult;
//                    if (!res.HasError) {
//                        var entity = res.Result;

//                        console.log("..Saved Successfully ", entity);
//                        resolve(true);
//                        if (this.closeWindow) {
//                            this.CurrentSession.CloseCurrentWindow();
//                        }
//                    }
//                    else {
//                        this.ValidationErrorsList = res.ErrorsArray;
//                    }
//                    this.CurrentSession.StopBusyIndicator();
//                    resolve(false);
//                });
//            }
//        });
//    }

//    _LastUnifreightMessageM: UnifreightMessageM;
//    SaveChangesSync() {
//        this._LastUnifreightMessageM = null;

//        Promise.resolve("lets go")
//            .then((res) => {
//                ///Just Saving  .....
//                this.LogMe("this.SavingPromise();")
//                return this.SavingPromise();


//            })
//            .then(saveWithoutError => {
//                if (this.ValidationErrorsList.length > 0) {
//                    //return Promise.reject(new Error('bad Save '));
//                    throw new Error('bad Save ');
//                }
//                let goToInsuranceInUNF = false;
//                if (this.declarationPM.IsConnectedToUnifreight) {
//                    //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && !AppTool.IsNullOrEmpty(this.declarationPM.CustomFileNo)) {
//                    if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.declarationPM.CustomFileNo, this.declarationPM.IsConvertedDeclaration, this.declarationPM.IsConnectedToUnifreight)) {
//                        goToInsuranceInUNF = true;
//                    }
//                }
//                if (!goToInsuranceInUNF) {
//                    this.LogMe("!goToInsuranceInUNF");
//                    this.FinishPromiseDoWhatPlanned();
//                    //return Promise.reject(new Error('Finish '));
//                    throw new Error('Finish');
//                }

//                return "Go Onn";

//            })

//            //.then(saveInsurance => {
//            //    return new Promise((resolve, reject) => {

//            //        if (saveInsurance == "Save1stSupplierInvoice") {
//            //            let supplierInvoice = this.declarationPM.SupplierInvoices[0];
//            //            this.supplierInvoicePMService.update(supplierInvoice).subscribe((myResult:any) => {

//            //                var res: ServiceResponse = myResult;
//            //                if (!res.HasError) {
//            //                    var entity = res.Result;

//            //                    console.log("..Saved Successfully ", entity);


//            //                    //resolveInsuranceCallback("ok")
//            //                    this.CurrentSession.StopBusyIndicator();
//            //                    resolve(true);


//            //                }
//            //                else {
//            //                    this.ValidationErrorsList = res.ErrorsArray;
//            //                    this.CurrentSession.StopBusyIndicator();
//            //                    resolve(false);
//            //                }

//            //            });
//            //        }
//            //        else {
//            //            this.FinishPromiseDoWhatPlanned();
//            //            //throw new Error("go to end");
//            //        }
//            //    });
//            //})

//            //.then(InsuranceSaved => {
//            //    if (!InsuranceSaved) {
//            //        this.closeWindow = false;
//            //        this._IsInitiateNewInstance = false;
//            //    }
//            //    else {
//            //        this.FinishPromiseDoWhatPlanned();
//            //    }
//            //})

//            .catch(finish => {
//                this.LogMe("catch(finish !!")
//                this.CurrentSession.StopBusyIndicator();
//                this._IsInitiateNewInstance = this.closeWindow = false;
//            });


//    }


//    FinishPromiseDoWhatPlanned() {
//        this.LogMe("FinishPromiseDoWhatPlanned");
//        if (this.closeWindow) {
//            this.CurrentSession.CloseCurrentWindow();
//            this._IsInitiateNewInstance = this.closeWindow = false;
//            //return Promise.reject(new Error('Finish CloseCurrentWindow'));
//            throw new Error('Finish CloseCurrentWindow')

//        }
//        if (this._IsInitiateNewInstance) {
//            this.InitiateNewInstance();
//            this._IsInitiateNewInstance = this.closeWindow = false;
//            //return Promise.reject(new Error('Finish InitiateNewInstance'));
//            throw new Error('Finish InitiateNewInstance')
//        }
//    }


//    PreSaveAndNewValidate() {
//        var errors = [];
//        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
//        //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);

//        // FreightAmounts
//        for (let frAmount of this.EntityPM.SupplierInvoiceFreightAmounts) {

//            Validator.TryValidateObject(frAmount, "Customs.SupplierInvoiceFreightAmount", errors);

//            if (!AppTool.IsNullOrEmpty(frAmount.CurrencyTypeCode) && AppTool.IsNullOrEmpty(frAmount.Amount)) {
//                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceFreightAmount.F.Amount"));
//            }

//            else if (AppTool.IsNullOrEmpty(frAmount.CurrencyTypeCode) && !AppTool.IsNullOrEmpty(frAmount.Amount)) {
//                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceFreightAmount.F.CurrencyTypeCode"));

//            }

//        }

//        // SupplierInvoiceItem
//        for (let item of this.EntityPM.SupplierInvoiceItems) {
//            Validator.TryValidateObject(item, "Customs.SupplierInvoiceItem", errors);
//            //Validator.TryValidateObject(item, new ValidationContext(item, null, null), errors);

//            for (let itemModification of item.SupplierInvoiceItemsMods) {
//                itemModification.ChangeSetOp = "None";
//            }

//        }

//        // IncotermCode
//        if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP")) {

//            if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
//                for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
//                    this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
//                }
//            }

//        }
//        else if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
//            if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
//                for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
//                    this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
//                }
//            }
//        }

//        // InsruanceCurrencyTypeCode
//        if (this.EntityPM.InsruanceCurrencyTypeCode != null && this.EntityPM.InsuranceAmount == null) {
//            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoice.F.InsuranceAmount"));
//        }
//        else if (this.EntityPM.InsruanceCurrencyTypeCode == null && this.EntityPM.InsuranceAmount != null) {
//            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoice.F.InsruanceCurrencyTypeCode"));
//        }

//        // SupplierInvoiceModifications
//        for (let item of this.EntityPM.SupplierInvoiceModifications) {
//            //Validator.TryValidateObject(item, new ValidationContext(item, null, null), errors);

//            if (AppTool.IsNullOrEmpty(item.CurrencyTypeCode)) {
//                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.CurrencyTypeCode"));
//            }
//            if (AppTool.IsNullOrEmpty(item.TypeCode)) {
//                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.TypeCode"));
//            }
//            if (AppTool.IsNullOrEmpty(item.Amount)) {
//                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.Amount"));
//            }

//        }

//        if (errors.length == 0) {

//            //if (isNewEntity) {
//            //    //supplier invoice is removed from composition. mohammad
//            //    if (!context.SupplierInvoicePMs.Contains(entityPM)) {
//            //        context.SupplierInvoicePMs.Add(entityPM);
//            //    }
//            //}

//            for (let item of this.EntityPM.SupplierInvoiceModifications) {
//                item.ChangeSetOp = "None";
//            }
//            return true;
//        } else {
           
//            this.ValidationErrorsList = errors;
//            return false;
//        }
//    }

//    GetRequierdFieldErrorText(fieldName) {
//        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
//    }


//    InitiateNewInstance() {

//        var itemPM = new SupplierInvoicePM();
//        itemPM.DeclarationId = this.declarationPM.Id;
//        itemPM.Tenant = SessionLocator.Tenant;
//        itemPM.InvoiceCounterKey = 0;
//        itemPM.SequenceNumeric = 0;

//        this.IsNewEntity = true;
//        this.EntityPM = itemPM;
//        this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, false);

//        // this.cd.detectChanges();
//        //newViewModel.isNewEntity = true;

//        //if (this.OnInitiateNewInvoice != null) {
//        //    this.OnInitiateNewInvoice(newViewModel, new EventArgs());
//        //}

//        //newViewModel.SetEntityPM(itemPM, true, this._IsEnabled, this._ViewDisableMessage);
//        //newViewModel.selectedIndex = 0;
//        //newViewModel.OnSelectedIndexChanged();
//    }

//    ConfirmWindowYesButton() {

//        if (this.SaveAndNew) {
//            this.closeWindow = false;
//            this.SaveChangesSync();

//        }
//        else {
//            if (this.SendMode) {
//                this.closeWindow = false;
//            }
//            else {
//                this.closeWindow = true;
//            }
//            this.SaveChangesSync();
//        }

//    }

//    //#endregion

//    ShowXMLErrors(error) {
//        if (!AppTool.IsNullOrEmpty(error.Field)) {
//            this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
//        }

//        var errors = [];
//        if (!AppTool.IsNullOrEmpty(error.Description)) {
//            var xmlErrors: any[] = error.Description.split(/,|:/);
//            for (var xmlError of xmlErrors) {
//                errors.push(xmlError);
//            }
//            this.ValidationErrorsList = [];
//            this.ValidationErrorsList = errors;
//        }
//        if (error.EntityName != null) {
//            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
//                //if (OnShowXMLErrors != null) {
//                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
//                //}
//            }
//        }
//    }

//    CloseWindowAfterSaveIfNeeded() {
//        if (this.closeWindow === true) {
//            this.CurrentSession.CloseCurrentWindow();
//        }
//    }

//    LogMe(mess) {
//        var alertIt = false;
//        if (alertIt) {
//            alert(mess);
//        } else {
//            console.log(mess);
//        }

//    }
//}

//class TabItem {
//    public code: string;
//    public textCode: string;
//    constructor(Code: string, TextCode: string) {
//        this.code = Code;

//        this.textCode = TextCode;
//    }
//}
