import {Component, ViewChildren, EventEmitter, Output, QueryList, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LocationDirective} from '../../../../../Infrastructure/Utilities/LocationDirective';
import {AppTool, FontTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {SupplierInvoicePMService} from '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import {CustomsDocumentPointersExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsDocumentPointersExtendedPMService';
import { CustomsDocumentPointerPM } from '../../../../../Customs/EntityPMs/CustomsDocumentPointerPM';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';

import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { SupplierInvoiceFreightAmountPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceFreightAmountPM';
import { SupplierInvoiceModificationPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceModificationPM';

import { SupplierInvoiceList } from '../../../../../Customs/EntityLists/SupplierInvoiceList';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { VendorCommissionPM } from '../../../../../Customs/EntityPMs/VendorCommissionPM';
import { VendorCommissionList } from '../../../../../Customs/EntityLists/VendorCommissionList';
import { AmitalGatewayUtil, UnifreightMessageM} from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';

import { ItemCodeComponent } from './SupplierInvoiceGeneralTabComponent';
import { GITITEMExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/GITITEMExtendedPMService';
import { GITITEMDto } from '../../../../../Customs/EntityPMs/Extended/GITITEMDto';
import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';
import {ObjectsLocator} from '../../../../../Infrastructure/Locators/ObjectsLocator';
import {SupplierInvoiceService} from '../../../../../Customs/Services/Others/SupplierInvoiceService';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
declare var window: any;
import {VendorCommissionPMService} from  '../../../../../Customs/Services/StandardPMs/VendorCommissionPMService';

import {ModificationAndDiscountTypeListService} from '../../../../../Customs/Services/StandardLists/ModificationAndDiscountTypeListService';
import { ModificationAndDiscountTypeList } from '../../../../../Customs/EntityLists/ModificationAndDiscountTypeList';
import { SupplierInvoiceItemPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';

import {VendorCommissionService} from '../../../../../Customs/Services/WebServices/VendorCommissionService';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { GITITEMCacheService } from '../../../../../Customs/Services/Others/GITITEMCacheService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditSupplierInvoiceComponent.html',
})
export class AddEditSupplierInvoiceComponent extends BaseComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    LayoutDirection: string = 'ltr';
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.SupplierInvoice";
    public ValidationErrorsList: string[] = [];
    public EntityPM: SupplierInvoicePM;
    public declarationPM: DeclarationPM;
    public TabsItemsSource: TabItem[] = [];
    public IsDisplayOnly: boolean = false;
    public IsNewEntity: boolean = false;
    public IsSaveAndNewVisible: boolean = true;
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    FIELD_IS_REQUIERD: string;
    NumberOfLoadedItems: number;
    VendorCommissionPMService: VendorCommissionPMService = new VendorCommissionPMService();
    vendorCommissionService: VendorCommissionService = new VendorCommissionService(); // extended
    // services
    public entityResourceService: EntityResourceService = new EntityResourceService();
    supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
    private _ModificationAndDiscountTypeListService: ModificationAndDiscountTypeListService = new ModificationAndDiscountTypeListService();

    public customsDocumentPointersExtendedPMService: CustomsDocumentPointersExtendedPMService = new CustomsDocumentPointersExtendedPMService();
    FirstCurrentLine: number = 1;
    skipedItems: number = 0;
    InvoiceItemsMessage: string;
    takenItems: number;
    IsNextButtonEnabled: boolean = true;
    IsPreviousButtonEnabled: boolean = false;
    WindowTitle: string;
    NextPreviousVisible: boolean = false;
    IsFromCustomsAnswer: boolean = false;
    IsInvoiceAnswer: boolean;
    AccumulatedFilter: string;
    IsSelectedRowTextBoxVisibile: boolean = false;
    //public ItemCode_LocalCache: ItemCodeComponent[];
    public GITITEMExtendedPMService: GITITEMExtendedPMService = new GITITEMExtendedPMService();

    public calculateCommission: boolean;//itzik put i due AOT error  (Alla set in SupplierInvoiceGeneralTabComponent.ts  "this.Parent.calculateCommission = true;")

    public NewInvoices: SupplierInvoicePM[] = [];
   
    _SkipAutoInsurance: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor//(private cd: ChangeDetectorRef) {
        () {
        super();

        //this.ItemCode_LocalCache = [];
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
        this.ikeaFeature = FeatureLocator.Features.filter(f => (f.Code == "IKEA") && f.ObjectTableId == table.Id)[0];
        this.screenHeight= this.GetScreenHeight();
        //this.GetCurrencies();
        this.ToggleButtonTopPosition = (this.screenHeight > 768) ? (21) : -81;

    }
    GetScreenHeight()
    {
        return self.innerHeight;
    }
    screenHeight: number;
    ToggleButtonTopPosition: number;




    ikeaFeature: any;
    SearchItemsFound: boolean = false;
    SearchItemsMessage: string = null;
    OldEntityPM: SupplierInvoicePM;
    FromClassificationJumpToSII;
    SetWindowArgs(args: any) {

        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OldEntityPM = this.EntityPM;
            this.declarationPM = args.declarationPM;
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.IsNewEntity = args.IsNewEntity;
            this.NumberOfLoadedItems = args.NumberOfLoadedItems;
            this.takenItems = this.NumberOfLoadedItems;
            this.SetScreenFieldsEditability();
            this.WindowTitle = args.WindowTitle;
            this.IsFromCustomsAnswer = args.IsFromCustomsAnswer;
            this.IsInvoiceAnswer = args.IsInvoiceAnswer;
            this.FromClassificationJumpToSII = args.FromClassificationJumpToSII;
            if (!AppTool.IsNullOrEmpty(this.FromClassificationJumpToSII)) {
                this.IsSaveAndNewVisible = false;
            }

            if (!this.IsNewEntity && this.EntityPM.FullItemsCount <= 500) {//bug 35255 yarons comment about leting every one see this box.
                this.IsSelectedRowTextBoxVisibile = true;
            }
            else if (this.IsNewEntity) {//bug 35255 yarons comment about leting every one see this box.
                this.IsSelectedRowTextBoxVisibile = true; 
            }
            // Document Filing
            this.GetDocumentFilingId();

            if (this.EntityPM.IsAccumalated) {
                this.AccumulatedFilter = "parent";
            }
            this.CurrentSession.SubscriptionAdd(
                this.CurrentSession.AccumulatedFilterChangedEvent.subscribe((res) => {
                    // this.EntityPM = res.entityPM;

                    this.skipedItems = 0;
                    this.takenItems = 500;
                    this.FirstCurrentLine = null;
                    if (res.filter == "Accumulated") {
                        this.AccumulatedFilter = "parent";
                        if (this.EntityPM.FullParentsCount <= 500) {
                            this.NextPreviousVisible = false;
                            //if (this.ikeaFeature) { bug 35255 yarons comment about leting every one see this box.
                            this.IsSelectedRowTextBoxVisibile = true;
                            // }
                        }
                        else {
                            this.InvoiceItemsMessage = "לחשבון זה קיימות  " + this.EntityPM.FullParentsCount + " שורות , מציג שורות  " + 1 + " עד " + this.NumberOfLoadedItems;

                            this.NextPreviousVisible = true;
                            this.IsSelectedRowTextBoxVisibile = false;

                        }
                        this.IsNextButtonEnabled = true;
                        this.IsPreviousButtonEnabled = false;
                        //  this.ReloadSupplierInvoiceWithItems(0, 500);
                    }
                    else if (res.filter == "NotAccumulated") {
                        this.AccumulatedFilter = "child";

                        if (this.EntityPM.FullChildrenCount <= 500) {
                            this.NextPreviousVisible = false;
                            // if (this.ikeaFeature) { bug 35255 yarons comment about leting every one see this box.
                            this.IsSelectedRowTextBoxVisibile = true;
                            //}
                        }
                        else {
                            this.InvoiceItemsMessage = "לחשבון זה קיימות  " + this.EntityPM.FullChildrenCount + " שורות , מציג שורות  " + 1 + " עד " + this.NumberOfLoadedItems;

                            this.NextPreviousVisible = true;
                            this.IsSelectedRowTextBoxVisibile = false;
                        }
                        this.IsNextButtonEnabled = true;
                        this.IsPreviousButtonEnabled = false;
                        //  this.ReloadSupplierInvoiceWithItems(0, 500);
                    }
                    else {
                        //this.AccumulatedFilter = null;
                        this.NextPreviousVisible = true;
                        this.IsSelectedRowTextBoxVisibile = false;
                        //   this.ReloadSupplierInvoiceWithItems(0, 500);
                    }
                })
            );
                this.CurrentSession.SearchFilterChangedEvent.subscribe((res) => {
                    if (res.count != null && res.count != 0) {
                        this.SearchItemsFound = true;
                        this.SearchItemsMessage = "נמצאו  " + res.count + " תוצאות שתואמות לחיפוש";
                    }
                    else {
                        this.SearchItemsFound = false;
                        this.SearchItemsMessage = null;
                    }
                });
            if (!AppTool.IsNullOrEmpty(args.DeclarationError)) {
                this.IsSaveAndNewVisible = false;
                this.ShowXMLErrors(args.DeclarationError);
            }
            if (!AppTool.IsNullOrEmpty(args.AmendmentView)) {
                this.IsSaveAndNewVisible = false;
                this.ShowXMLCorrections(args.AmendmentView);
            }
            this.EntityPM.IsValueForCustomsOnly = this.declarationPM.IsValueForCustomsOnly;

            this.getModTypeName();

            //Calculate commission percentage value
            //this.CalculateCommissionPercentage();
            this.GetCustomerCommissions();
        }

        var customsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();

        
        customsSettingExtendedListService.GetSkipAutoInsurancePromise(this.declarationPM.CustomerCode, this.declarationPM.Tenant).subscribe(myResult => {
            var res: ServiceResponse = myResult;
            if (res.Result.SkipAutoInsurance === true) {
                this._SkipAutoInsurance = true;
            }
            if (FeatureLocator.IsFeatureGrantedByCode("IFRITZ")) { // If FRITZ always check insurance- Task 37656
                this._SkipAutoInsurance = false;
            }
            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceFreightAmount").subscribe(response => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceModification").subscribe(response => {
                        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(response => {

                            this.BuildTabs();
                            this.RunComponent();
                            for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
                                this.itemsLineNumbers = this.itemsLineNumbers + "," + this.EntityPM.SupplierInvoiceItems[i].LineNumber;
                            }



                            //    this.itemsLineNumbers = this.itemsLineNumbers.substring(0);

                            this.GetPointers();

                        });
                    });
                });
            });
        });

        this.IsNextButtonEnabled = true;
        this.IsPreviousButtonEnabled = false;
        if ((this.EntityPM.FullItemsCount > this.NumberOfLoadedItems) && !this.EntityPM.IsAccumalated) {

            //MessageBorderVisibility = Visibility.Visible;
            this.InvoiceItemsMessage = "לחשבון זה קיימות  " + this.EntityPM.FullItemsCount + " שורות , מציג שורות  " + 1 + " עד " + this.NumberOfLoadedItems;
            this.NextPreviousVisible = true;
            this.IsSelectedRowTextBoxVisibile = false;
        }

        else if (this.EntityPM.IsAccumalated && this.EntityPM.FullParentsCount > 500) {
            this.InvoiceItemsMessage = "לחשבון זה קיימות  " + this.EntityPM.FullParentsCount + " שורות , מציג שורות  " + 1 + " עד " + this.NumberOfLoadedItems;

            this.NextPreviousVisible = true;
            this.IsSelectedRowTextBoxVisibile = false;
        }
        else if (this.EntityPM.FullItemsCount < 500) {//&& this.ikeaFeature) {bug 35255 yarons comment about leting every one see this box.

            this.IsSelectedRowTextBoxVisibile = true;
        }


    }

    SetScreenFieldsEditability() {

    }

    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        //this.TabsItemsSource.push(new TabItem("DOCUMENT", "Customs.Declaration.TH.Documents"));
        this.TabsItemsSource.push(new TabItem("MORE", "Customs.Declaration.TH.More"));
        this.selectedTabCode = "GENERAL";
    }
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    private isViewInited = false;
    InitializeComponent() {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    }
    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }
    private GENERAL: any = null;
    private MORE: any = null;
    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "GENERAL": {
                        if (this.GENERAL == null) {
                          SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.GENERAL = cmpRef.instance;
                                    this.GENERAL.FromClassificationJumpToSII = this.FromClassificationJumpToSII;
                                    this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, true, this.IsNewEntity, this.IsFromCustomsAnswer, this.IsInvoiceAnswer);
                                    this.GENERAL.ReloadEntityEvent.subscribe((response: any) => {
                                        this.ReloadPromise().then(() => {
                                            this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, true, this.IsNewEntity, this.IsFromCustomsAnswer, this.IsInvoiceAnswer);
                                        });
                                    });
                                });
                        }

                        break;
                    }

                    case "DOCUMENTS": {

                        break;
                    }

                    case "MORE": {
                        if (this.MORE == null) {
                          SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceMoreTabComponent',
                                myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.MORE = cmpRef.instance;
                                    this.MORE.SetTabArgs({ InvoicePM: this.EntityPM, IsDisplayOnly: this.IsDisplayOnly, DeclarationPM: this.declarationPM, Parent: this});
                                    this.MORE.FillValidationErrorList.subscribe((response: any) => {
                                        this.ValidationErrorsList = response;
                                    });
                                });
                        }

                        break;
                    }

                }


            }
        }
    }

    //#region properties

    public DifferenceColor: string = "#282E30";

    private difference: number = 0;
    get Difference() { return this.difference; }
    set Difference(newValue: number) {
        if (this.difference != newValue) {
            this.difference = newValue;

            if (this.TotalForeignCurrency != 0) {
                if (this.difference != null) {
                    if (this.difference != 0) {
                        this.DifferenceColor = FontTool.Red; //red
                    }
                    else {
                        this.DifferenceColor = FontTool.Green; //green
                    }
                }
            }
            else {
                this.DifferenceColor = FontTool.Black;
            }
        }
    }

    private totalForeignCurrency: number = 0;
    get TotalForeignCurrency() { return this.totalForeignCurrency; }
    set TotalForeignCurrency(newValue: number) {
        if (this.totalForeignCurrency != newValue) {
            this.totalForeignCurrency = newValue;
        }
    }

    //#endregion

    itemsLineNumbers: string;
    public pointers: Array<CustomsDocumentPointerPM> = [];
    GetPointers() {
        this.customsDocumentPointersExtendedPMService.GetCustomDocumentPointersForItems(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey.toString(), this.itemsLineNumbers).subscribe(response => {
            var result = response.Result;
            this.pointers = result;


        });
    }

    // Buttons Handlers
    OkButtonClicked() {
        this.SaveAndNew = false;

        //if (this.EntityPM.InvoiceNumber) {
        //    var supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
        //    supplierInvoiceService.GetCheckIfInvoiceNumberExists(this.EntityPM.DeclarationId, this.EntityPM.InvoiceNumber, this.EntityPM.InvoiceCounterKey).subscribe((resp: ServiceResponse) => {
        //        if (!resp.HasError) {
        //            if (resp.Result) {
        //                var confirm = new ConfirmWindow();

        //                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

        //                confirm.ShowNoButton = true; 
        //                confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה"+ resp.Result.SequenceNumeric + "- האם להמשיך ?");
        //                confirm.WindowClosed.subscribe((event: any) => {
        //                    if (confirm.Yes) {
        //                        confirm.Close();
        //                        this.SaveButtonClicked();
        //                    }
        //                    else {
        //                        confirm.Close();
        //                    }
        //                });
        //            }
        //            else {
        //                this.SaveButtonClicked();
        //            }
        //        }
        //    });
        //}
        //else {
        //    this.SaveButtonClicked();
        //}
        
        this.SaveButtonClicked();
        
    }
    CancelButtonClicked() {
        if ((this.EntityPM.IsDirty || this.ForceSave) && !this.IsDisplayOnly && !this.IsFromCustomsAnswer) {
            var confirm = new ConfirmWindow();

            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.OkButtonClicked();
                    
                }
                else {
                    this.EntityPM.RejectChanges();
                    this.GENERAL.Dispose();
                    if (this.SaveAndNew) {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindowEmit('cancel');
                    }
                }

            });


        }

        else {
            //this.EntityPM.RejectChanges();
            this.GENERAL.Dispose();
            if (this.SaveAndNew) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit('cancel');
            }
        }

    }

    //#region Save Code
    SaveAndNew: boolean = false;
    _IsInitiateNewInstance: boolean = false;
    loadingNextItems: boolean = false;
    closeWindow: boolean;

    sendMode: boolean;
    public get SendMode() { return this.sendMode; }
    public set SendMode(value: boolean) {
        this.sendMode = value;
    }

    SaveAndNewButtonClicked() {
        //if (this.EntityPM.InvoiceNumber) {
        //    var supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
        //    supplierInvoiceService.GetCheckIfInvoiceNumberExists(this.EntityPM.DeclarationId, this.EntityPM.InvoiceNumber, this.EntityPM.InvoiceCounterKey).subscribe((resp: ServiceResponse) => {
        //        if (!resp.HasError) {
        //            if (resp.Result) {
        //                var confirm = new ConfirmWindow();

        //                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

        //                confirm.ShowNoButton = true;
        //                confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + resp.Result.SequenceNumeric + "- האם להמשיך ?");
        //                confirm.WindowClosed.subscribe((event: any) => {
        //                    if (confirm.Yes) {
        //                        confirm.Close();
        //                        this.ApplySaveAndNew();
        //                    }
        //                    else {
        //                        confirm.Close();
        //                    }
        //                });
        //            }
        //            else {
        //                this.ApplySaveAndNew();
        //            }
        //        }
        //    });
        //}
        //else {
        //    this.ApplySaveAndNew();
        //}

        this.NewInvoices.push(this.EntityPM);;

        //remove LineDoesNotExist msg
        this.LineDoesNotExist = false;

        this.ApplySaveAndNew();
    }

    ApplySaveAndNew() {

        var errors = this.ValidateModifications();
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            return;
        }


        var valid = this.PreSaveAndNewValidate();
        var checkFreightValues = true;

        this._IsInitiateNewInstance = true;
        if (this.declarationPM.SupplierInvoices.length > 1) {
            for (let item of this.declarationPM.SupplierInvoices) {
                if (!AppTool.IsNullOrEmpty(item.IncotermCode)) {

                    if (item.IncotermCode.startsWith("E") || item.IncotermCode.startsWith("F")) {
                        //if (item.SupplierInvoiceFreightAmounts.length > 0 && !AppTool.IsNullOrEmpty(item.InsuranceAmount)) {
                        if (!AppTool.IsNullOrEmpty(item.InsuranceAmount)) {
                            checkFreightValues = false;
                        }
                    }
                    else if (item.IncotermCode == "CPT" || item.IncotermCode == "CFR") {
                        if (!AppTool.IsNullOrEmpty(item.InsuranceAmount)) {
                            checkFreightValues = false;
                        }
                    }

                }
            }
        }

        if (valid) {
            if (checkFreightValues) {

                this.SaveAndNew = true;
                if (!AppTool.IsNullOrEmpty(this.EntityPM.IncotermCode) && (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F"))) {

                    if ((this.EntityPM.SupplierInvoiceFreightAmounts.length == 0 && !this.declarationPM.InvoiceHasFreight) || ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null))) {

                        this.CurrentSession.StopBusyIndicator();

                        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
                        confirmWindow.Show(msg);
                        confirmWindow.WindowClosed.subscribe((event: any) => {

                            if (confirmWindow.Yes) {
                                this.ConfirmWindowYesButton();
                            }
                        });
                        this.closeWindow = false;


                    }
                    else {
                        this.closeWindow = false;

                        //this.SaveChanges();

                        //this._IsInitiateNewInstance = true;
                        //this.InitiateNewInstance();
                        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
                        this.SaveChangesSync();
                    }

                }
                else if (!AppTool.IsNullOrEmpty(this.EntityPM.IncotermCode) && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {

                    if ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null)) {


                        this.CurrentSession.StopBusyIndicator();

                        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
                        confirmWindow.Show(msg);
                        confirmWindow.WindowClosed.subscribe((event: any) => {

                            if (confirmWindow.Yes) {
                                this.ConfirmWindowYesButton();
                            }
                        });

                        this.closeWindow = false;
                    }
                    else {
                        this.closeWindow = false;
                        //this.SaveChanges();
                        //this._IsInitiateNewInstance = true;
                        //this.InitiateNewInstance();
                        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
                        this.SaveChangesSync();
                    }

                }
                else {
                    this.closeWindow = false;
                    //this.SaveChanges()//;
                    //    .then(
                    //    (a) => {
                    //        this._IsInitiateNewInstance = true;
                    //        this.InitiateNewInstance();
                    //    });
                    this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
                    this.SaveChangesSync();
                }

            }
            else {
                this.closeWindow = false;
                //this.SaveChanges();
                //this._IsInitiateNewInstance = true;
                //this.InitiateNewInstance();
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
                this.SaveChangesSync();
            }
        }
    }

    SaveButtonClicked() {
        // return;

        var errors = this.ValidateModifications();
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            return;
        }

        this.CurrentSession.StartBusyIndicatorSaving();

        //if (InvoiceModificationsObslist.Count > 0) {
        //    bool exist = (from a in InvoiceModificationsObslist
        //    where !a.isValid
        //    select a).Any();

        //    if (exist) {
        //        return;
        //    }
        //}

        var valid = this.PreSaveAndNewValidate();
        var checkFreightValues = true;


        if (this.declarationPM.SupplierInvoices.length > 1) {
            for (var item of this.declarationPM.SupplierInvoices) {
                if (item.IncotermCode != null && (item.IncotermCode.startsWith("E") || item.IncotermCode.startsWith("F"))) {
                    //if (item.SupplierInvoiceFreightAmounts.length > 0 && item.InsuranceAmount != null) {
                    if (item.InsuranceAmount != null) {
                        checkFreightValues = false;
                        break;
                    }
                }

                else if (item.IncotermCode != null && (item.IncotermCode == "CPT" || item.IncotermCode == "CFR")) {
                    if (item.InsuranceAmount != null) {
                        checkFreightValues = false;
                        break;
                    }
                }
            }
        }

        if (valid) {


            this.ContinueSaving(checkFreightValues); // if not use old calclate commestion code

            //if (this.calculateCommission) {
            //    this.VendorCommissionPMService.get(this.EntityPM.VendorId, this.declarationPM.CustomerId).subscribe(myResult => {

            //        if (myResult) {
            //            if (!myResult.HasError) {
            //                if (myResult.Result) {
            //                    this.EntityPM.VendorComissionPercentage = myResult.Result.CommisionPercentage;
            //                    if (this.EntityPM.SupplierInvoiceModifications.length > 0) {
            //                        for (let item of this.EntityPM.SupplierInvoiceModifications) {

            //                            if (item.CurrencyTypeCode == this.EntityPM.InvoiceCurrencyTypeCode && item.TypeCode == "I10" && item.Amount == (this.EntityPM.InvoiceAmount * this.EntityPM.VendorComissionPercentage)) {

            //                                this.ContinueSaving(checkFreightValues);

            //                            }
            //                            else if (item.CurrencyTypeCode != this.EntityPM.InvoiceCurrencyTypeCode || item.Amount != (this.EntityPM.InvoiceAmount * this.EntityPM.VendorComissionPercentage)) {
            //                                this.CurrentSession.StopBusyIndicator();
            //                                var confirm = new ConfirmWindow;
            //                                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

            //                                confirm.ShowNoButton = true;
            //                                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.UpdateCommision"));
            //                                confirm.WindowClosed.subscribe((event: any) => {
            //                                    if (confirm.Yes) {
            //                                        item.CurrencyTypeCode = this.EntityPM.InvoiceCurrencyTypeCode;
            //                                        item.Amount = (this.EntityPM.VendorComissionPercentage / 100) * this.EntityPM.InvoiceAmount;
            //                                        this.ContinueSaving(checkFreightValues);
            //                                        confirm.Close();

            //                                    }
            //                                    else {
            //                                        this.ContinueSaving(checkFreightValues);
            //                                        confirm.Close();
            //                                    }
            //                                });
            //                            }
            //                        }

            //                    }

            //                    else {
            //                        if (myResult.Result.CommisionPercentage && myResult.Result.CommisionPercentage > 0) {
            //                            var supplierInvoiceModificationPM = new SupplierInvoiceModificationPM(this.EntityPM);
            //                            supplierInvoiceModificationPM.DeclarationId = this.EntityPM.DeclarationId;
            //                            supplierInvoiceModificationPM.Tenant = this.EntityPM.Tenant;
            //                            supplierInvoiceModificationPM.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
            //                            supplierInvoiceModificationPM.Amount = this.EntityPM.InvoiceAmount * (this.EntityPM.VendorComissionPercentage / 100);
            //                            supplierInvoiceModificationPM.CurrencyTypeCode = this.EntityPM.InvoiceCurrencyTypeCode;
            //                            supplierInvoiceModificationPM.TypeCode = "I10";
            //                            this.EntityPM.SupplierInvoiceModifications.push(supplierInvoiceModificationPM);
            //                        }
            //                        this.ContinueSaving(checkFreightValues);
            //                    }
            //                }
            //                else {
            //                    this.ContinueSaving(checkFreightValues);
            //                }
            //            }

            //            else {
            //                this.ValidationErrorsList = myResult.ErrorsArray;
            //                this.CurrentSession.StopBusyIndicator();

            //            }

            //        }
            //        else {
            //            this.ContinueSaving(checkFreightValues);
            //        }



            //    });
            //}
            //else {
            //    this.ContinueSaving(checkFreightValues);
            //}




        }
        else {
            this.CurrentSession.StopBusyIndicator();

        }
    }

    ContinueSaving(checkFreightValues: boolean) {
        if (checkFreightValues /*&& !this.loadingNextItems*/) {// maybe this should be done because i'm saving the invoice in case next and previous.
            if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F"))) {
                if ((this.EntityPM.SupplierInvoiceFreightAmounts.length == 0 && !this.declarationPM.InvoiceHasFreight) || ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null))) {

                    this.CurrentSession.StopBusyIndicator();

                    var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                    confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe((event: any) => {

                        if (confirmWindow.Yes) {
                            this.ConfirmWindowYesButton();
                        }
                    });

                    this.closeWindow = false;
                }
                else {
                    this.closeWindow = true;
                    //this.SaveChanges();
                    this.SaveChangesSync();
                }

            }

            else if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
                if ((this.declarationPM.SupplierInvoices.length > 0 && this.EntityPM.SequenceNumeric == 1 && this.EntityPM.InsuranceAmount == null) || (this.declarationPM.SupplierInvoices.length == 0 && this.EntityPM.SequenceNumeric == null && this.EntityPM.InsuranceAmount == null)) {
                    this.CurrentSession.StopBusyIndicator();

                    this.CurrentSession.StopBusyIndicator();

                    var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AmountsNotCompatableToIncoterm");
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                    confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe((event: any) => {

                        if (confirmWindow.Yes) {
                            this.ConfirmWindowYesButton();
                        }
                    });

                    this.closeWindow = false;
                }
                else {
                    if (!this.loadingNextItems) {
                        this.closeWindow = true;
                    }
                    else {
                        this.closeWindow = true;
                    }
                    //this.SaveChanges();
                    this.SaveChangesSync();
                }
            }

            else {
                if (this.SendMode) {
                    this.closeWindow = false;
                }
                else if (!this.loadingNextItems) {
                    this.closeWindow = true;
                }
                //this.SaveChanges();
                this.SaveChangesSync();
            }

        }

        else {
            if (this.SendMode) {
                this.closeWindow = false;
            }
            else if (!this.loadingNextItems) {
                this.closeWindow = true;
            }
            //this.SaveChanges();
            this.SaveChangesSync();
        }
    }

    //ValidateModifications() {
    //    if (this.EntityPM.SupplierInvoiceModifications) {

    //        var validationErrors = [];

    //        this.EntityPM.SupplierInvoiceModifications.forEach((mod) => {


    //            var typeCode = mod.TypeCode;

    //            if (typeCode == "I02") {
    //                //     validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee"));
    //            } else {

    //                var exists = [];
    //                if (this.EntityPM.SupplierInvoiceModifications.length != 0) {
    //                    exists = this.EntityPM.SupplierInvoiceModifications.filter(d => d.TypeCode == typeCode);
    //                }
    //                if (exists.length > 1) {
    //                    validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
    //                }

    //            }
    //        });

    //        return validationErrors;


    //    }
    //}

    private SaveChanges() {

        //var errors = [];


        //if (errors.length == 0) {

        if (this.IsNewEntity) {

            this.supplierInvoicePMService.insert(this.EntityPM).subscribe(myResult => {

                var res: ServiceResponse = myResult;
                if (!res.HasError) {
                    this.entity = res.Result;

                    console.log("..Saved Successfully ", this.entity);
                    return true;
                }
                else {
                    this.ValidationErrorsList = res.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
                return false;
            });


        } else {

            this.supplierInvoicePMService.update(this.EntityPM).subscribe(myResult => {

                var res: ServiceResponse = myResult;
                if (!res.HasError) {
                    this.entity = res.Result;

                    console.log("..Saved Successfully ", this.entity);
                    return true;


                }
                else {
                    this.ValidationErrorsList = res.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
                return false;
            });

        }
        //}
    }

    



    ReloadPromise(): Promise<boolean> {
        return new Promise((resolve) => {
            if (this.loadingNextItems || this.NextPreviousVisible) {
                resolve(true);
            } else {

                this.supplierInvoicePMService
                    .get(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey)
                    .subscribe(myResult => {
                        var res: ServiceResponse = myResult;
                        var entity = res.Result;
                        this.EntityPM = entity;// now we have the Sequence from server (and can update )
                        resolve(true);
                    });

            }
        });

    }


    Reload1stSIPromise(): Promise<boolean> {
        return new Promise((resolve) => {
            if (this.loadingNextItems || this.NextPreviousVisible) {
                resolve(true);
            } else {
                ///reload the 1st all the time !! <<<<<
                var siFirst = this.Get1SupplierInvoice();
                if (siFirst.InvoiceCounterKey == this.EntityPM.InvoiceCounterKey) {
                    resolve(true);
                } else {
                    this.supplierInvoicePMService
                        .get(siFirst.DeclarationId, siFirst.InvoiceCounterKey)
                        .subscribe(my1stResult => {
                            let res1: ServiceResponse = my1stResult;
                            this.declarationPM.SupplierInvoices[0] = this.supplierInvoicePMService.MapJsonToEntityPM(res1.Result);
                            //this.EntityPM = this.declarationPM.SupplierInvoices[0];// now we have the Sequence from server (and can update )
                            resolve(true);
                        });
                }
            }
        });

    }


    SavingPromise(isChromeMode: boolean): Promise<boolean> {


        return new Promise((resolve) => {
            if (this.IsNewEntity) {
                this.supplierInvoicePMService.insert(this.EntityPM).subscribe(myResult => {
                    var res: ServiceResponse = myResult;
                    if (!res.HasError) {
                        this.entity = res.Result;
                        this.EntityPM = this.entity;// now we have the Sequence from server (and can update )
                        console.log("..Saved Successfully ", this.entity);
                        resolve(true);
                        if (isChromeMode) {
                            if (this.closeWindow) {
                                this.CurrentSession.CloseCurrentWindow();
                            }
                            else if (this._IsInitiateNewInstance) {
                                if (this.copyInvoiceWithItem && this.entity.FullItemsCount > 500) {
                                    if (this.OldEntityPM.FullItemsCount > 500) {
                                        var msg = new MessageWindow();

                                        msg.Show("ההצהרה מכילה יותר מ-500 פריטים, לא ניתן להעתיק אותה");
                                        this.NextPreviousVisible = true;

                                    }


                                }
                                else {
                                    this.GENERAL.Dispose();
                                    this.InitiateNewInstance();
                                    this._IsInitiateNewInstance = this.closeWindow = false;
                                }
                                //return Promise.reject(new Error('Finish InitiateNewInstance'));

                            }
                            else if (this.loadingNextItems) {
                                this.GENERAL.Dispose();
                                this.ReloadSupplierInvoiceWithItems(this.skipedItems, this.takenItems);
                            }
                        }
                    }
                    else {
                        this.ValidationErrorsList = res.ErrorsArray;
                    }
                    this.CurrentSession.StopBusyIndicator();
                    resolve(false);
                });

            }
            else {
                this.supplierInvoicePMService.update(this.EntityPM).subscribe(myResult => {

                    var res: ServiceResponse = myResult;
                    if (!res.HasError) {
                         this.entity = res.Result;
                        this.CurrentSession.StopBusyIndicator();
                        console.log("..Saved Successfully ", this.entity);
                        resolve(true);
                        if (isChromeMode) {
                            if (this.closeWindow) {
                                this.CurrentSession.CloseCurrentWindow();
                            }
                            else if (this._IsInitiateNewInstance) {
                                if (this.copyInvoiceWithItem && this.entity.FullItemsCount > 500) {
                                    if (this.OldEntityPM.FullItemsCount > 500) {
                                        var msg = new MessageWindow();

                                        msg.Show("ההצהרה מכילה יותר מ-500 פריטים, לא ניתן להעתיק אותה");
                                        this.NextPreviousVisible = true;
                                        
                                    }
                                    
                                   
                                }
                                else {
                                    this.GENERAL.Dispose();
                                    this.InitiateNewInstance();
                                    this._IsInitiateNewInstance = this.closeWindow = false;
                                }
                            
                            }
                            else if (this.loadingNextItems) {
                                this.GENERAL.Dispose();

                                this.ReloadSupplierInvoiceWithItems(this.skipedItems, this.takenItems);
                            }
                        }
                    }
                    else {
                        this.ValidationErrorsList = res.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }

                    resolve(false);
                });


            }



        });
    }
    entity: SupplierInvoicePM;
    _My1stSupplierInvoicePM: SupplierInvoicePM = null;
    public Get1SupplierInvoice() {
        //let supplierInvoice = this.EntityPM;
        let supplierInvoice: SupplierInvoicePM = null;
        if (this.declarationPM.SupplierInvoices && this.declarationPM.SupplierInvoices.length > 0) {
            supplierInvoice = this.declarationPM.SupplierInvoices[0];
        }
        else {
            if (this._My1stSupplierInvoicePM != null) {
                supplierInvoice = this._My1stSupplierInvoicePM;
            } else {
                supplierInvoice = this.EntityPM;
                this._My1stSupplierInvoicePM = supplierInvoice;
            }
        }


        //itzik : the this.declarationPM.SupplierInvoices[0] is not the same reference so old will be data update 
        if (//supplierInvoice.DeclarationId == this.EntityPM.DeclarationId
            supplierInvoice.InvoiceCounterKey == this.EntityPM.InvoiceCounterKey &&
            supplierInvoice.DeclarationId == this.EntityPM.DeclarationId) {
            supplierInvoice = this.EntityPM;
        }
        return supplierInvoice;
    }


    _LastUnifreightMessageM: UnifreightMessageM;
    _FinishPromiseDoWhatPlannedDone: boolean;
    SaveChangesSync() {

        //this.supplierInvoiceExtendedPMService.PutSupplierInvoicePercentage(this.EntityPM).subscribe(myResult => {

        //    if (myResult) {
        //        if (!myResult.HasError) {

        //        }
        //    }
        //});


        this.ValidationErrorsList = [];
        this._FinishPromiseDoWhatPlannedDone = false;
        this._LastUnifreightMessageM = null;

        if (!this.EntityPM.IsDirty && !this.ForceSave) {
            this.FinishPromiseDoWhatPlanned(true);
            return; 
        }

      //this.SaveItemCodeLocalCache();
      GITITEMCacheService.Instance.SaveItemCodeLocalCache();

        if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            if (this.EntityPM.IsDirty || this.ForceSave) {
                return this.SavingPromise(true);
            }
            else {
                this.FinishPromiseDoWhatPlanned(true);
            }

        }
        Promise.resolve("lets go")
            .then((res) => {
                ///Just Saving  .....
                this.LogMe("this.SavingPromise();")
                if (this.EntityPM.IsDirty || this.ForceSave) {
                    return this.SavingPromise(false);
                }
                else {
                    this.FinishPromiseDoWhatPlanned(true);
                }


            })
            .then(saveWithoutError => {
                if (this.ValidationErrorsList.length > 0) {
                    //return Promise.reject(new Error('bad Save '));
                    throw new Error('bad Save ');
                }
                let goToInsuranceInUNF = false;
                if (!this._SkipAutoInsurance){
                    if (this.declarationPM.IsConnectedToUnifreight) {
                        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.declarationPM.CustomFileNo, this.declarationPM.IsConvertedDeclaration, this.declarationPM.IsConnectedToUnifreight)) {//if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && !AppTool.IsNullOrEmpty(this.declarationPM.CustomFileNo)) {
                            goToInsuranceInUNF = true;
                        }
                    }
                }
                if (!goToInsuranceInUNF) {
                    this.LogMe("!goToInsuranceInUNF");
                    this.FinishPromiseDoWhatPlanned();
                    //return Promise.reject(new Error('Finish '));
                    throw new Error('Finish');
                }

                return "Go Onn";

            })
            .then(goON => {

                this.LogMe("ReloadPromise()");
                return this.ReloadPromise()


            })
            .then(goON => {

                this.LogMe("ReloadPromise()");
                return this.Reload1stSIPromise()

            })

            .then(goON => {
                if (this.loadingNextItems) {
                    throw new Error('Next\prev =>Finish (Check Insurance only on save)');
                }
                this.LogMe("goToInsuranceInUNF");
                this.CurrentSession.StartBusyIndicator("Check Insurance ...");
                var toPromise = true;
                return this.SendUnifaceRequestAndWaitPromise();

            })


            .then((res) => {
                return this.UnifreightInsuranceCallbackAction(this._LastUnifreightMessageM);
            })
            //.catch(getInsuranceError => {
            //})
            .then(saveInsurance => {
                return new Promise((resolve, reject) => {

                    if (saveInsurance == "Save1stSupplierInvoice") {
                        let supplierInvoice = this.Get1SupplierInvoice(); //this.declarationPM.SupplierInvoices[0];

                        ////itzik : the this.declarationPM.SupplierInvoices[0] is not the same reference so old will be data update 
                        //if (//supplierInvoice.DeclarationId == this.EntityPM.DeclarationId
                        //    supplierInvoice.InvoiceCounterKey == this.EntityPM.InvoiceCounterKey &&
                        //    supplierInvoice.DeclarationId == this.EntityPM.DeclarationId) {
                        //    supplierInvoice = this.EntityPM;
                        //}

                        this.supplierInvoicePMService.update(supplierInvoice).subscribe(myResult => {

                            var res: ServiceResponse = myResult;
                            if (!res.HasError) {
                                this.entity = res.Result;

                                console.log("..Saved Successfully ", this.entity);


                                //resolveInsuranceCallback("ok")
                                this.CurrentSession.StopBusyIndicator();
                                resolve(true);


                            }
                            else {
                                this.ValidationErrorsList = res.ErrorsArray;
                                this.CurrentSession.StopBusyIndicator();
                                console.warn("Error Saving UnifreightInsurance");
                                resolve(false);
                            }

                        });
                    }
                    else {
                        this.FinishPromiseDoWhatPlanned();
                        //throw new Error("go to end");
                    }
                });
            })
            //.catch(saveInsuranceError => {
            //})
            .then(InsuranceSaved => {
                if (!InsuranceSaved) {
                    this.closeWindow = false;
                    this._IsInitiateNewInstance = false;
                    var closeWidowsWhenSaveInsuranceFailed = true;
                    if (closeWidowsWhenSaveInsuranceFailed) {
                        this.closeWindow = true;
                        this.loadingNextItems = false;
                        console.warn("Due Error in Saving UnifreightInsurance ...closeWindow");
                        this._FinishPromiseDoWhatPlannedDone = false;//i didnt finsh  - pls do !!
                        this.FinishPromiseDoWhatPlanned();
                    }
                }
                else {
                    this.FinishPromiseDoWhatPlanned();
                }
            })
            //.catch(saveInsuranceError => {
            //})

            .catch(finish => {
                this.FinishPromiseDoWhatPlanned(true);
                this.LogMe("catch(finish !!")
                this.CurrentSession.StopBusyIndicator();
                this._IsInitiateNewInstance = this.closeWindow = false;
            });


    }


    FinishPromiseDoWhatPlanned(inCatchBlock?: boolean) {
        if (this.ValidationErrorsList && this.ValidationErrorsList.length > 0) return;//Show Err Validation !!
        if (this._FinishPromiseDoWhatPlannedDone) return;


        this._FinishPromiseDoWhatPlannedDone = true;
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

        this.LogMe("FinishPromiseDoWhatPlanned");
        if (this.closeWindow) {
            this.GENERAL.Dispose();
            this.CurrentSession.CloseCurrentWindow();
            this._IsInitiateNewInstance = this.closeWindow = false;
            //return Promise.reject(new Error('Finish CloseCurrentWindow'));
            if (inCatchBlock != true) {
                throw new Error('Finish CloseCurrentWindow');
            }

        }
        if (this._IsInitiateNewInstance) {
            if (this.copyInvoiceWithItem && this.OldEntityPM.FullItemsCount > 500) {
                if (this.entity.FullItemsCount > 500) {
                    var msg = new MessageWindow();

                    msg.Show("ההצהרה מכילה יותר מ-500 פריטים, לא ניתן להעתיק אותה");
                    this.NextPreviousVisible = true;

                }


            }
            else {
                this.GENERAL.Dispose();
                this.InitiateNewInstance();
                this._IsInitiateNewInstance = this.closeWindow = false;
                //return Promise.reject(new Error('Finish InitiateNewInstance'));
                if (inCatchBlock != true) {
                    throw new Error('Finish InitiateNewInstance');
                }
            }
        }
        if (this.loadingNextItems) {
            this.GENERAL.Dispose();
            this.ReloadSupplierInvoiceWithItems(this.skipedItems, this.takenItems);
        }
    }


    SendUnifaceRequestAndWaitPromise() {
        return new Promise((resolve, reject) => {
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                (mess: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (
                        mess.UnifreightEntityNumber == this.declarationPM.CustomFileNo &&
                        mess.LogitudeViewModel == "AddEditSupplierInvoiceComponent");
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        this.LogMe("UnifaceRequestArrived");
                        this._LastUnifreightMessageM = mess;
                        resolve("Arrived mess from unifrieght");
                        //Promise.resolve("Arrived mess from unifrieght")

                    }
                });

            AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseCheckInsuranseReturnIsNeededAmount(
                this.declarationPM.CustomFileNo, this.declarationPM.Id + this.EntityPM.InvoiceCounterKey.toString()
                , "AddEditSupplierInvoiceComponent", "OPEN"
            );
        });
    }
    PreSaveAndNewValidate() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);

        // FreightAmounts
        for (let frAmount of this.EntityPM.SupplierInvoiceFreightAmounts) {

            Validator.TryValidateObject(frAmount, "Customs.SupplierInvoiceFreightAmount", errors);

            if (!AppTool.IsNullOrEmpty(frAmount.CurrencyTypeCode) && AppTool.IsNullOrEmpty(frAmount.Amount)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceFreightAmount.F.Amount"));
            }

            else if (AppTool.IsNullOrEmpty(frAmount.CurrencyTypeCode) && !AppTool.IsNullOrEmpty(frAmount.Amount)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceFreightAmount.F.CurrencyTypeCode"));

            }

        }

        // SupplierInvoiceItem
        for (let item of this.EntityPM.SupplierInvoiceItems) {
            Validator.TryValidateObject(item, "Customs.SupplierInvoiceItem", errors);
            //Validator.TryValidateObject(item, new ValidationContext(item, null, null), errors);

            for (let itemModification of item.SupplierInvoiceItemsMods) {
                itemModification.ChangeSetOp = "None";
            }

        }

        // IncotermCode
        //if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP")) {

        //    if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
        //        for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
        //            this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
        //        }
        //    }

        //}
        //else if (this.EntityPM.IncotermCode != null && (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR")) {
        //    if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
        //        for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
        //            this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
        //        }
        //    }
        //}

        // InsruanceCurrencyTypeCode
        if (this.EntityPM.InsruanceCurrencyTypeCode != null && this.EntityPM.InsuranceAmount == null) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoice.F.InsuranceAmount"));
        }
        else if (this.EntityPM.InsruanceCurrencyTypeCode == null && this.EntityPM.InsuranceAmount != null) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoice.F.InsruanceCurrencyTypeCode"));
        }

        // SupplierInvoiceModifications
        for (let item of this.EntityPM.SupplierInvoiceModifications) {
            //Validator.TryValidateObject(item, new ValidationContext(item, null, null), errors);

            if (AppTool.IsNullOrEmpty(item.CurrencyTypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.CurrencyTypeCode") + " - מסך נוספים");
            }
            if (AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.TypeCode") + " - מסך נוספים");
            }
            if (AppTool.IsNullOrEmpty(item.Amount)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceModification.F.Amount") + " - מסך נוספים");
            }

        }

        if (errors.length == 0) {

            //if (isNewEntity) {
            //    //supplier invoice is removed from composition. mohammad
            //    if (!context.SupplierInvoicePMs.Contains(entityPM)) {
            //        context.SupplierInvoicePMs.Add(entityPM);
            //    }
            //}

            for (let item of this.EntityPM.SupplierInvoiceModifications) {
                item.ChangeSetOp = "None";
            }
            return true;
        } else {

            this.ValidationErrorsList = errors;
            return false;
        }
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }




    private UnifreightInsuranceCallbackAction(unifreightMessageM: UnifreightMessageM) {
        this.LogMe("UnifreightInsuranceCallbackAction");
        return new Promise((resolveInsuranceCallback, reject) => {
            this.LogMe("UnifreightInsuranceCallbackAction Promise ");
            let sAction = ""; sAction = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.Action");
            let sInsuranseIsNeeded = ""; sInsuranseIsNeeded = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsNeeded");
            let sInsuranseIsSucceeded = ""; sInsuranseIsSucceeded = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsSucceeded");
            let sInsuranceHasOpen = ""; sInsuranceHasOpen = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseHasOpen");
            let sInsuranceMessage = ""; sInsuranceMessage = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranceMessage");
            let sInsuranseAmount = ""; sInsuranseAmount = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseAmount");
            let sInsuranseCurrency = ""; sInsuranseCurrency = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseCurrency");
            let sExpensesAmount = ""; sExpensesAmount = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmount");
            let sExpensesAmountCurr = ""; sExpensesAmountCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmountCurr");
            let sFreightAmount = ""; sFreightAmount = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount");
            let sFreightAmountCurr = ""; sFreightAmountCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr");
            //Task 40723:
            let sFreightAmount2 = ""; sFreightAmount2 = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount2");
            let sFreightAmountCurr2 = ""; sFreightAmountCurr2 = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr2");
            let sTotalFreightInFreightCurr = ""; sTotalFreightInFreightCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.TotalFreightInFreightCurr");

            let sForceMessage = ""; sFreightAmountCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ForceMessage");

            switch (sAction) {
                case "OPEN":
                    {
                        if (this.declarationPM.SupplierInvoices != null
                            //&& this.declarationPM.SupplierInvoices.length > 0
                        ) {

                            let supplierInvoice: SupplierInvoicePM = null;

                            supplierInvoice = this.Get1SupplierInvoice(); //this.declarationPM.SupplierInvoices[0];
                            var execAsService = true;
                            if (execAsService) {
                                var service: AnalyzeUnifreightInsuranceService = new AnalyzeUnifreightInsuranceService();
                                service.Open(unifreightMessageM, this.declarationPM, supplierInvoice);

                            }


                            if (!AppTool.IsNullOrEmpty(sForceMessage) || 
                                (!AppTool.IsNullOrEmpty(sInsuranceMessage) && !this.declarationPM.IsChanged)
                            ) {
                                if (sInsuranseIsNeeded.toString().toLowerCase() == "true" &&
                                    sInsuranseIsSucceeded === "false") {
                                    /*this.CurrentSession.StopBusyIndicator(); */this._IsInitiateNewInstance = this.closeWindow = false;
                                    this.closeWindow = true;
                                    ///setTimeout(() => {
                                        this.CurrentSession.StopBusyIndicator();
                                        var messageWindow = new MessageWindow();
                                        messageWindow.Width = 400;
                                        messageWindow.Height = 150;
                                        messageWindow.Title = "";
                                        messageWindow.Show(sInsuranceMessage);
                                    //}, 500);
                                    } else {
                                    console.log(sInsuranceMessage);
                                }
                                resolveInsuranceCallback("Show Error Message - Stop");

                            }
                            if (this.declarationPM.IsChanged == true) {
                                resolveInsuranceCallback("Save1stSupplierInvoice");


                            } else {
                                this.CurrentSession.StopBusyIndicator();
                                resolveInsuranceCallback("nothing done ");
                            }



                        }
                        else {
                            resolveInsuranceCallback("nothing done ");
                        }
                    }
                    break;
                case "CHECK":
                    {
                        if (sInsuranseIsNeeded == "true") {
                            //VisibilityInsurance = "Visible";
                        }
                        else {
                            //VisibilityInsurance = "Collapsed";
                        }
                        resolveInsuranceCallback("nothing done ");
                        break;
                    }
                default:
                    {
                        resolveInsuranceCallback("nothing done ");
                        break;
                    }
            }
        });
    }
    copyInvoice: boolean;
    copyInvoiceWithItem: boolean;
    accumulationFeature: any;
    CopyInvoiceClicked() {
        this.copyInvoice = true;
        this.SaveAndNewButtonClicked();
        this.DropdownDisplayClose();
    }
    CopyInvoiceWithItemsClicked() {
        this.copyInvoice = true;
        this.copyInvoiceWithItem = true;

        this.SaveAndNewButtonClicked();
        this.DropdownDisplayClose();

    }
    ForceSave: boolean=false;
    InitiateNewInstance() {
        this.ForceSave = true;
        var itemPM = new SupplierInvoicePM();
        this.TextValue = null;
        if (this.EntityPM) {
            if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
                this.declarationPM.InvoiceHasFreight = true;
            }
        }

        if (this.copyInvoice) {
            itemPM.VendorId = this.EntityPM.VendorId;
            itemPM.IssueCountryCode = this.EntityPM.IssueCountryCode;
            itemPM.AccountTypeCode = this.EntityPM.AccountTypeCode;
            itemPM.IssueDate = this.EntityPM.IssueDate;
            itemPM.InvoiceCurrencyTypeCode = this.EntityPM.InvoiceCurrencyTypeCode;
            itemPM.IncotermCode = this.EntityPM.IncotermCode;
            itemPM.InvoiceAmount = null;
            itemPM.InsruancePercentage = null;
            itemPM.InsuranceAmount = null;
            itemPM.InsruanceCurrencyTypeCode = null;

        }
        itemPM.DeclarationId = this.declarationPM.Id;
        itemPM.IsValueForCustomsOnly = this.EntityPM.IsValueForCustomsOnly;//this.declarationPM.IsValueForCustomsOnly;
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.InvoiceCounterKey = 0;
        itemPM.SequenceNumeric = 0;
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
        this.NextPreviousVisible = false;
        this.Difference = 0;
        this.TotalForeignCurrency = 0;
        this.accumulationFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCUMULATION") && f.ObjectTableId == table.Id)[0];
        if (this.accumulationFeature == null) {
            itemPM.AccumalationStateCode = "3";
        }
        else {
            itemPM.AccumalationStateCode = "1";
        }
        itemPM.IsAccumalated = false;
        this.TotalForeignCurrency = 0;
        this.IsNewEntity = true;
        var counterKey = this.EntityPM.InvoiceCounterKey;
        this.EntityPM = itemPM;
        this.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.NewInvoice");

        if (this.copyInvoiceWithItem) {
           
            
                this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.DeclarationId, counterKey, 0, this.NumberOfLoadedItems, "child").subscribe(response => {
                    if (!response.HasError) {
                        this.OldEntityPM = response.Result;
                        var supplierinvoiceitems: SupplierInvoiceItemPM[] = [];
                        var line: number = 1;
                        var sequence: number = 1;
                       
                      

                      
                       

                        
                       // itemPM.SupplierInvoiceItems = this.OldEntityPM.SupplierInvoiceItems;
                        for (let item of this.OldEntityPM.SupplierInvoiceItems) {
                         var  newItem: SupplierInvoiceItemPM = new SupplierInvoiceItemPM(itemPM);
                         newItem.ClassificationCode = item.ClassificationCode;
                         newItem.TradeAgreementCode = item.TradeAgreementCode;
                         newItem.InvoiceQuantityType = item.InvoiceQuantityType;
                         newItem.OriginCountryCode = item.OriginCountryCode;
                         newItem.OriginCountryName = item.OriginCountryName;
                         newItem.TradeAgreementName = item.TradeAgreementName;
                         newItem.ChangeSetOp = "Insert";
                         newItem.DeclarationId = item.DeclarationId;
                         newItem.LineNumber = line;
                         newItem.CounterKey = itemPM.InvoiceCounterKey;
                         newItem.SequenceNumeric = sequence;
                         newItem.Tenant = this.OldEntityPM.Tenant;
                         this.EntityPM.AddSupplierInvoiceItem(newItem);
                         line += 1;
                         sequence += 1;
                        }


                        this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, false,true);

                    }
                    else {
                        //this.ValidationErrorsList = response.ErrorArray;
                    }
                });
            
        }

        else {
            this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, false,true);
        }
        //select general tab
        this.SelectedTabCode = "GENERAL";
        this.copyInvoice = false;
        this.copyInvoiceWithItem = false;
        //distroy old more tab content
        if (this.MORE)
            this.DistroyMoreTab();

        // this.cd.detectChanges();
        //newViewModel.isNewEntity = true;

        //if (this.OnInitiateNewInvoice != null) {
        //    this.OnInitiateNewInvoice(newViewModel, new EventArgs());
        //}

        //newViewModel.SetEntityPM(itemPM, true, this._IsEnabled, this._ViewDisableMessage);
        //newViewModel.selectedIndex = 0;
        //newViewModel.OnSelectedIndexChanged();
    }

    ConfirmWindowYesButton() {

        if (this.SaveAndNew || this.loadingNextItems) {
            this.closeWindow = false;

            //this.SaveChanges()//;
            //    .then(
            //    (a) => {
            //        this.InitiateNewInstance();
            //    });
            this.SaveChangesSync();

        }
        else {
            if (this.SendMode) {
                this.closeWindow = false;
            }
            else {
                this.closeWindow = true;
            }
            //this.SaveChanges().then(res => { this.CloseWindowAfterSaveIfNeeded() });
            this.SaveChangesSync();

        }

    }

    //SaveItemCodeLocalCache() {
    //    if (this.ItemCode_LocalCache != null && this.ItemCode_LocalCache.length > 0) {
    //        for (let item of this.ItemCode_LocalCache) {
    //            if (item.IsNew) {
    //                var myGITITEMPM = new GITITEMDto();
    //                myGITITEMPM.PARTNERID = this.declarationPM.CustomerCode;
    //                if (AppTool.IsNullOrEmpty(item.VendorNumber)) {
    //                    item.VendorNumber = "NULL";
    //                }
    //                myGITITEMPM.SAPAKID = item.VendorNumber;
    //                myGITITEMPM.ITEMNO = item.ItemCode;
    //                myGITITEMPM.PRATID = item.ClassificationCode;
    //                myGITITEMPM.NAMEENG = item.ItemDescription;
    //                myGITITEMPM.ORIGINCOUNTRY = item.OriginCountryCode;
    //                myGITITEMPM.UNITID = item.InvoiceQuantityType;

    //                this.GITITEMExtendedPMService.insert(myGITITEMPM).subscribe(myResult => {
    //                    var mm: ServiceResponse = myResult;
    //                    if (!mm.HasError) {
    //                        this.entity = mm.Result;
    //                    }
    //                });
    //            }
    //        }
    //    }
    //}

    //#endregion

    DistroyMoreTab() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "MORE")[0];
        myLocation.viewContainerRef.clear();
        this.MORE = null;
    }

    ShowXMLErrors(error) {
        if (!AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
        }

        var errors = [];
        if (!AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors: any[] = error.Description.split(/,|:/);
            for (var xmlError of xmlErrors) {
                errors.push(xmlError);
            }
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
                //if (OnShowXMLErrors != null) {
                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
                //}
            }
        }
    }
    ShowXMLCorrections(amendment) {
        if (amendment.EntityName.toLowerCase() == "supplierinvoiceitem") {
            //if (OnShowXMLErrors != null) {
            //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { 
            //        SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == amendment.Line).FirstOrDefault(),
            //    });
            //}
        } else {

            if (!AppTool.IsNullOrEmpty(amendment.Field)) {
                this.UIProperties.SetValidity(amendment.Field, "Customs.SupplierInvoice", false, amendment.ErrorType);
            }

            var errors = [];
            errors.push(amendment.ErrorType);
            this.ValidationErrorsList = errors;
        }
    }
    CloseWindowAfterSaveIfNeeded() {
        if (this.closeWindow === true) {
            this.CurrentSession.CloseCurrentWindow();
        }
    }
    LogMe(mess) {
        var alertIt = false;
        if (alertIt) {
            alert(mess);
        } else {
            console.log(mess);
        }

    }

    Previous() {
        var fullCount = 0;
        if (this.AccumulatedFilter == "parent") {
            fullCount = this.EntityPM.FullParentsCount;
        }
        else if (this.AccumulatedFilter == "child") {
            fullCount = this.EntityPM.FullChildrenCount;
        }
        else {
            fullCount = this.EntityPM.FullItemsCount;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        var Z: number;
        if (this.FirstCurrentLine == null) {
            this.FirstCurrentLine = 1;
        }
        if (this.FirstCurrentLine <= this.NumberOfLoadedItems) {
            this.skipedItems = 0;
            this.FirstCurrentLine = 1;
            Z = this.NumberOfLoadedItems;
            this.takenItems = this.NumberOfLoadedItems;
        }
        else {

            this.skipedItems = this.FirstCurrentLine - 1 - this.NumberOfLoadedItems;//this.skipedItems - this.NumberOfLoadedItems;
            if (this.skipedItems < 0) {
                this.skipedItems = 0;
            }

            this.FirstCurrentLine = this.skipedItems + 1;
            Z = this.FirstCurrentLine + this.NumberOfLoadedItems - 1;
        }
        this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;


        this.loadingNextItems = true;

        if (this.skipedItems == 0) {
            this.IsPreviousButtonEnabled = false;
        }

        else {
            this.IsPreviousButtonEnabled = true;
        }

        if (!this.IsNextButtonEnabled) {
            this.IsNextButtonEnabled = true;
        }

        this.SaveChangesSync();
    }


    Next() {
        var fullCount = 0;
        if (this.AccumulatedFilter == "parent") {
            fullCount = this.EntityPM.FullParentsCount;
        }
        else if (this.AccumulatedFilter == "child") {
            fullCount = this.EntityPM.FullChildrenCount;
        }
        else {
            fullCount = this.EntityPM.FullItemsCount;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        var Z = 0;
        if (this.FirstCurrentLine == null) {
            this.FirstCurrentLine = 1;

        }
        if (this.FirstCurrentLine == 1) {
            this.IsPreviousButtonEnabled = false;
        }
        if (this.skipedItems + 1 != this.FirstCurrentLine) {
            if (this.FirstCurrentLine >= fullCount) {
                this.skipedItems = fullCount - this.NumberOfLoadedItems + 1;
                this.takenItems = this.NumberOfLoadedItems + 1;
                Z = fullCount;
                if (Z > fullCount) {
                    Z = fullCount;
                }
                this.FirstCurrentLine = this.skipedItems + 1;
                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;
            }
            else {
                this.skipedItems = this.FirstCurrentLine - 1;
                this.takenItems = this.NumberOfLoadedItems + 1;//501;

                Z = this.FirstCurrentLine + this.NumberOfLoadedItems;

                if (Z > fullCount) {
                    Z = fullCount;
                }

                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;

            }
        }
        else {

            if (this.FirstCurrentLine >= fullCount) {
                this.skipedItems = fullCount - this.NumberOfLoadedItems + 1;
                this.takenItems = this.NumberOfLoadedItems + 1;
                Z = fullCount;

                this.FirstCurrentLine = this.skipedItems + 1;
                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;

            }
            else {
                this.skipedItems = this.skipedItems + this.NumberOfLoadedItems;
                this.FirstCurrentLine = this.skipedItems + 1;
                if (this.FirstCurrentLine >= fullCount) {
                    Z = fullCount;
                }
                else {
                    if (this.takenItems > 1) {
                        Z = this.FirstCurrentLine + this.NumberOfLoadedItems - 1;
                    }
                    else {
                        Z = this.FirstCurrentLine + this.NumberOfLoadedItems;
                    }
                    if (Z > fullCount) {
                        Z = fullCount;
                    }
                }

                this.InvoiceItemsMessage = "לחשבון זה קיימות  " + fullCount + " שורות , מציג שורות  " + this.FirstCurrentLine + " עד " + Z;

            }
        }
        if (this.AccumulatedFilter == "parent") {
            var diff = this.EntityPM.FullParentsCount - this.FirstCurrentLine;
            if (diff <= 500) {
                this.IsNextButtonEnabled = false;

            }
        }
        else if (this.AccumulatedFilter == "child") {
            var diff = this.EntityPM.FullChildrenCount - this.FirstCurrentLine;
            if (diff <= 500) {
                this.IsNextButtonEnabled = false;

            }
        }
        else {

            if (Z == fullCount) {
                this.IsNextButtonEnabled = false;

            }
            else {
                this.IsNextButtonEnabled = true;
            }
        }


        this.loadingNextItems = true;
        if (!this.IsPreviousButtonEnabled && this.FirstCurrentLine != 1) {
            this.IsPreviousButtonEnabled = true;
        }

        this.SaveChangesSync();
    }

    TextBoxKeyUp(event) {
        if (this.FirstCurrentLine == null) {
            this.FirstCurrentLine = 1;
        }
        if (!this.IsNextButtonEnabled && (this.FirstCurrentLine < this.EntityPM.FullItemsCount)) {
            this.IsNextButtonEnabled = true;
        }
        if (this.FirstCurrentLine == 1) {
            this.IsPreviousButtonEnabled = false;
        }
    }
    SelectInvoiceItemEvent: any;
    TextValue: number;
    LineDoesNotExist: boolean = false;
    LineDoesNotExistMessage: string = null;
    SelectedTextBoxKeyUp(event) {
        if (event) {

            //this.SelectInvoiceItemEvent = this.CurrentSession.SelectInvoiceItemEvent.emit({ filter: this.TextValue });
            this.GENERAL.SelectInvoiceItemMethod({ filter: this.TextValue });
        }
        if (!this.TextValue) {
            this.LineDoesNotExist = false;
        }

        else {
            var value = this.EntityPM.SupplierInvoiceItems.filter(d => d.SequenceNumeric == this.TextValue)[0];
            if (!value) {
                this.LineDoesNotExist = true;
                this.LineDoesNotExistMessage = "מספר שורה לא נמצא";
            }
            else {
                this.LineDoesNotExist = false;
            }
        }
    }

    ReloadSupplierInvoiceWithItems(skippedItems, takenItems) {
        this.loadingNextItems = false;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey, skippedItems, takenItems, this.AccumulatedFilter).subscribe(response => {
            this.EntityPM = response.Result;
            this.selectedTabCode = "GENERAL";
            this.GENERAL.InitTab(this.EntityPM, this, this.IsDisplayOnly, false, false);
            this.CurrentSession.StopBusyIndicator();
        });
    }

    ValidateModifications() {
        if (this.EntityPM.SupplierInvoiceModifications) {

            var validationErrors = [];

            this.EntityPM.SupplierInvoiceModifications.forEach((mod) => {


                var typeCode = mod.TypeCode;
                if (typeCode == "I02") {
                    //itzik+yaron20180201 validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee") + " - מסך נוספים");
                } else {

                    var exists = [];
                    if (this.EntityPM.SupplierInvoiceModifications.length != 0) {
                        exists = this.EntityPM.SupplierInvoiceModifications.filter(d => d.TypeCode == typeCode);
                    }
                    if (exists.length > 1) {
                        validationErrors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
                    }

                }
            });

            return validationErrors;


        }
    }

    //#region DocumentFiling
    DocumentFilingId: string;
    GetDocumentFilingId() {
        console.log(" --->> Getting related document filing ...");
        this.supplierInvoiceExtendedPMService.GetDocumentFilingIdForForInvoice(this.declarationPM.Id, this.EntityPM.InvoiceCounterKey).subscribe(response => {
            console.log("[Reponse] GetDocumentFilingIdForForInvoice: ", response);
            var result = response.Result;
            if (result) {
                this.DocumentFilingId = response.Result;
                console.log("sending document filing document filing ...");
                DeclarationEventManager.DeclarationSplitDocumentSelection.emit(this.DocumentFilingId);

                //(new MessageWindow()).Show("document filing found: " + this.DocumentFilingId);
            }
            else
                console.log("[!] No related document filing found!!");


        });
    }
    //#endregion

    //#region Commission Code
    @Output() ReloadModificationEvent: EventEmitter<any> = new EventEmitter();
    invoiceCurrencyName: string;
    typeNameForI10: string;
    CustomerCommissionsList: VendorCommissionPM[] = [];

    GetCustomerCommissions() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.vendorCommissionService.GetCommissionsForCustomer(this.declarationPM.CustomerId).subscribe(response => {
            console.log("[Reponse] GetCommissionsForCustomer: ", response);
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var result = response.Result;
            if (result)
            {
                this.CustomerCommissionsList = response.Result;
                this.CalculateCommissionPercentage();
            }
        });
    }

    CalculateCommissionPercentage() { // NEW CODE

        if (this.EntityPM.VendorId && this.declarationPM.CustomerId && this.EntityPM.InvoiceAmount && this.EntityPM.InvoiceCurrencyTypeCode) {
            // GET percentage for vendor, customer from server
            //this.VendorCommissionPMService.get(this.EntityPM.VendorId, this.declarationPM.CustomerId).subscribe((response: ServiceResponse) => {
            //if (response && !response.HasError) {

            //var commission: VendorCommissionPM = response.Result;

            var commission: VendorCommissionList = this.CustomerCommissionsList.filter(d => d.VendorId == this.EntityPM.VendorId)[0];
            if (commission) {
                if (commission.CommisionPercentage) {

                    //init new mod values
                    var newCurrency = this.EntityPM.InvoiceCurrencyTypeCode;
                    var newAmount = this.precisionRound((commission.CommisionPercentage / 100) * this.EntityPM.InvoiceAmount, 2);

                    // if commission found:
                    // 1- update Field VendorCommisionPercentage.SupplierInvoice
                    this.EntityPM.VendorComissionPercentage = commission.CommisionPercentage;
                    console.log("[!] invoice commission changed to:", this.EntityPM.VendorComissionPercentage);

                    // 2- In case there’s mod record , update it
                    var modTypeI10 = this.EntityPM.SupplierInvoiceModifications.filter(d => d.TypeCode == "I10")[0];
                    if (modTypeI10) {
                        // 3- In case there’s record with same type (I10) 
                        //    and it's with different currency OR value ask user
                        if (modTypeI10.Amount != newAmount || modTypeI10.CurrencyTypeCode != newCurrency) {
                            //somthing changed, amount or currency
                            //ask user to change it
                            var confirm = new ConfirmWindow;
                            var msgTxt = TextCodeTranslator.Translate("Customs.Declaration.O.CommissionChangedFromTo");
                            msgTxt = msgTxt.replace("#oldValue", modTypeI10.Amount.toFixed(2).toString() + " " + modTypeI10.CurrencyTypeCode);
                            msgTxt = msgTxt.replace("#newValue", newAmount.toFixed(2).toString() + " " + newCurrency); // new values
                            confirm.Show(msgTxt);

                            confirm.WindowClosed.subscribe((event: any) => {
                                if (confirm.Yes) {
                                    //update record
                                    modTypeI10.CurrencyTypeCode = newCurrency;
                                    modTypeI10.CurrencyTypeName = this.invoiceCurrencyName;

                                    modTypeI10.Amount = newAmount;

                                    this.UpdateModificationsList();

                                    confirm.Close();
                                }
                                else {
                                    //don't update
                                    confirm.Close();
                                }
                            });

                        }
                        else {
                            // same currency and amount
                            if (modTypeI10.Amount == newAmount && modTypeI10.CurrencyTypeCode == newCurrency) {
                                // no changes
                            }
                        }

                    }
                    else {
                        //In case there’s no mod record I10, create a record in SupplierInvoiceModification 
                        var newMod = new SupplierInvoiceModificationPM(this.EntityPM);
                        newMod.Tenant = this.EntityPM.Tenant;
                        newMod.DeclarationId = this.EntityPM.DeclarationId;
                        newMod.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;

                        newMod.TypeCode = "I10";
                        newMod.TypeName = this.typeNameForI10;

                        newMod.CurrencyTypeCode = newCurrency;
                        newMod.CurrencyTypeName = this.invoiceCurrencyName;

                        newMod.Amount = newAmount;

                        this.EntityPM.SupplierInvoiceModifications.push(newMod);

                        this.UpdateModificationsList();
                    }


                }
            } else {
                //No commission for this vendor , delete existing commission ?
                var msg = "לא קיימים נתוני עמלה לספק זה , האם למחוק נתוני עמלה קיימים ?";
                if (this.EntityPM.VendorComissionPercentage) {
                    var confirm = new ConfirmWindow();
                    confirm.Show(msg);
                    confirm.WindowClosed.subscribe((event: any) => {
                        if (confirm.Yes) {
                            //delete commission

                            this.EntityPM.VendorComissionPercentage = null;
                            console.log("[!] invoice commission deleted");

                            var modTypeI10 = this.EntityPM.SupplierInvoiceModifications.filter(d => d.TypeCode == "I10")[0];
                            this.EntityPM.RemoveSupplierInvoiceModification(modTypeI10);

                            this.UpdateModificationsList();

                            confirm.Close();
                        }
                        else {
                            //don't
                            confirm.Close();
                        }
                    });
                }
            }

            //}
            //});

        } else {
            console.log("[!] Cannot calculate commession percentage, some fields are required!");
        }
    }

    UpdateModificationsList() {
        this.ReloadModificationEvent.emit("");
    }

    precisionRound(number, digitsAfterPoint) {
        var factor = Math.pow(10, digitsAfterPoint);
        return Math.round(number * factor) / factor;
    }

    getModTypeName() {
        this._ModificationAndDiscountTypeListService.getSingleFromCache("I10").subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var type = myResponse.Result;
                if (type)
                    this.typeNameForI10 = type.LocalName;
            }
        });

    }
    DropdownDisplayClose() {
        this._DropdownDisplay = 'none';
    }
    getScreenHeight() { return self.innerHeight; }
    _DropdownDisplay: string = 'none';
    dropdowndisplayToggle() {
    
        var item = document.getElementById("savebutton");
        var itemRect = item.getBoundingClientRect();

        //document.getElementById(this._CustomSendOptionsComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
        //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
        document.getElementById("dropdowmenu").style.top =
            itemRect.top + 'px';

        let DDLHeight = 67;//    height: 22px; * 3 +30 
        let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
        if (itemRect.bottom + DDLHeight > this.getScreenHeight()) {//this.PaintTop = true                
            document.getElementById("dropdowmenu").style.top =
                (itemRect.bottom - DDLHeight - Extra) + 'px';
        }
        //document.getElementById("dropdowmenu").style.left =
        //    (itemRect.left + 100) + 'px';
        if (this._DropdownDisplay == 'none') {
           
            this._DropdownDisplay = 'block';
        } else {
            this._DropdownDisplay = 'none';
        }
 }
    //#endregion
}


class TabItem {
    public code: string;
    public textCode: string;
    constructor(Code: string, TextCode: string) {
        this.code = Code;

        this.textCode = TextCode;
    }
}



export class AnalyzeUnifreightInsuranceService {
    //public IsFritzFeatureIsOn: boolean
    ///public SuppressUpdateFreight: boolean

    Open(unifreightMessageM: UnifreightMessageM, declarationPM: DeclarationPM, supplierInvoice: SupplierInvoicePM) {
        let sAction = ""; sAction = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.Action");
        let sInsuranseIsNeeded = ""; sInsuranseIsNeeded = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsNeeded");
        let sInsuranseIsSucceeded = ""; sInsuranseIsSucceeded = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseIsSucceeded");
        let sInsuranceHasOpen = ""; sInsuranceHasOpen = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseHasOpen");
        let sInsuranceMessage = ""; sInsuranceMessage = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranceMessage");
        let sInsuranseAmount = ""; sInsuranseAmount = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseAmount");
        let sInsuranseCurrency = ""; sInsuranseCurrency = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.InsuranseCurrency");
        let sExpensesAmount = ""; sExpensesAmount = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmount");
        let sExpensesAmountCurr = ""; sExpensesAmountCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExpensesAmountCurr");
        let sFreightAmount = ""; sFreightAmount = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount");
        let sFreightAmountCurr = ""; sFreightAmountCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr");
        //Task 40723:
        let sFreightAmount2 = ""; sFreightAmount2 = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmount2");
        let sFreightAmountCurr2 = ""; sFreightAmountCurr2 = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.FreightAmountCurr2");        
        let sTotalFreightInFreightCurr = ""; sTotalFreightInFreightCurr = UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.TotalFreightInFreightCurr");

        console.log("Fritz FreightAmount: sFreightAmount= " + sFreightAmount + " sFreightAmount2=" + sFreightAmount2 + " TotalFreightInFreightCurr=" + sTotalFreightInFreightCurr);

        if (sAction != "OPEN") {
            throw Error("sAction != OPEN  !!!!");
        }
        var toUpdateFreightAmount: boolean = true;
        //if (this.IsFritzFeatureIsOn) {
        //if (!this.SuppressUpdateFreight) {
        if (declarationPM.IsValueForCustomsOnly) {  //o	יש לבדוק את השדה החדש, ואם הערך true אז לא לעדכן הובלה. אחרת לעדכן הובלה, אם התקבל ערך
            toUpdateFreightAmount = false;;
            console.warn("FritzFeatureIsOn but SuppressUpdateFreight is true soo Suppress Update Freight ");
        }
        //}

        if (FeatureLocator.IsFeatureGrantedByCode("IFRITZ")) {//    o        לאחר שמירה ובדיקת שדות לשליחה, יש לבדוק Feature כפי שבודקים במסך חשבון ספק
            console.log("FritzFeatureIsON .. ");

            if (toUpdateFreightAmount && !AppTool.IsNullOrEmpty(sFreightAmount) && !AppTool.IsNullOrEmpty(sFreightAmountCurr)) {
                console.warn("Update Freight ");
                let FreightAmount: number = 0;
                let FreightAmount2: number = 0;//task 40723
                if (Number(sFreightAmount2) != NaN) {
                    FreightAmount2 = Number(sFreightAmount2);
                }
                let TotalFreightInFreightCurr: number = 0;
                if (Number(sTotalFreightInFreightCurr) != NaN) {
                    TotalFreightInFreightCurr = Number(sTotalFreightInFreightCurr);
                }
                if (Number(sFreightAmount) != NaN) { //(decimal.TryParse(sFreightAmount, out FreightAmount)) {
                    FreightAmount = Number(sFreightAmount);

                    if (FreightAmount2 == 0) {//task 40723
                        supplierInvoice.TotalFreightInFreightCurrency = FreightAmount;
                        supplierInvoice.FreightCurrencyTypeCode = sFreightAmountCurr;
                    }
                    else {//TODO: calc tot amount...
                        supplierInvoice.TotalFreightInFreightCurrency = TotalFreightInFreightCurr;
                        supplierInvoice.FreightCurrencyTypeCode = sFreightAmountCurr;
                    }
                    //supplierInvoice.TotalFreightInFreightCurrency = FreightAmount;
                    //supplierInvoice.FreightCurrencyTypeCode = sFreightAmountCurr;

                    let supplierInvoiceFreightAmountPM: SupplierInvoiceFreightAmountPM;
                    if (supplierInvoice.SupplierInvoiceFreightAmounts.length > 0) {
                        //for (let item of supplierInvoice.SupplierInvoiceFreightAmounts) {
                        //    supplierInvoice.RemoveSupplierInvoiceFreightAmount(item);
                        //}
                        let lengthCounter: number = supplierInvoice.SupplierInvoiceFreightAmounts.length;
                        for (var i = 0; i < lengthCounter; i++) {
                            supplierInvoice.RemoveSupplierInvoiceFreightAmount(supplierInvoice.SupplierInvoiceFreightAmounts[0]);
                        }
                    }

                    supplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(supplierInvoice);
                    supplierInvoice.AddSupplierInvoiceFreightAmount(supplierInvoiceFreightAmountPM);

                    supplierInvoiceFreightAmountPM.DeclarationId = supplierInvoice.DeclarationId;
                    supplierInvoiceFreightAmountPM.Tenant = supplierInvoice.Tenant;
                    supplierInvoiceFreightAmountPM.InvoiceCounterKey = supplierInvoice.InvoiceCounterKey;
                    supplierInvoiceFreightAmountPM.Amount = FreightAmount;
                    supplierInvoiceFreightAmountPM.CurrencyTypeCode = sFreightAmountCurr;

                    if (FreightAmount2 != 0) {//task 40723
                        let supplierInvoiceFreightAmount2PM: SupplierInvoiceFreightAmountPM;
                        supplierInvoiceFreightAmount2PM = new SupplierInvoiceFreightAmountPM(supplierInvoice);
                        supplierInvoice.AddSupplierInvoiceFreightAmount(supplierInvoiceFreightAmount2PM);
                        supplierInvoiceFreightAmount2PM.DeclarationId = supplierInvoice.DeclarationId;
                        supplierInvoiceFreightAmount2PM.Tenant = supplierInvoice.Tenant;
                        supplierInvoiceFreightAmount2PM.InvoiceCounterKey = supplierInvoice.InvoiceCounterKey;
                        supplierInvoiceFreightAmount2PM.Amount = FreightAmount2;
                        supplierInvoiceFreightAmount2PM.CurrencyTypeCode = sFreightAmountCurr2;
                    }

                    //supplierInvoice.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //this.declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    declarationPM.IsChanged = true;

                }
            }
            //o	בקבלת התשובה יש לעדכן הוצאות (160) אם התקבל ערך
            if (!AppTool.IsNullOrEmpty(sExpensesAmount) && !AppTool.IsNullOrEmpty(sExpensesAmountCurr)) {
                console.log("Update  sExpensesAmount");
                let ExpensesAmount = 0;
                if (Number(sExpensesAmount) != NaN) {//(decimal.TryParse(sExpensesAmount, out ExpensesAmount)) {
                    ExpensesAmount = Number(sExpensesAmount)

                    let supplierInvoiceModificationPM: SupplierInvoiceModificationPM;

                    let list160 = supplierInvoice.SupplierInvoiceModifications.filter(rec => rec.TypeCode == "160");
                    if (list160.length > 0) {
                        //supplierInvoiceModificationPM = list160[0];
                        supplierInvoice.RemoveSupplierInvoiceModification(list160[0]);
                    }
                    supplierInvoiceModificationPM = new SupplierInvoiceModificationPM(supplierInvoice);
                    supplierInvoice.AddSupplierInvoiceModification(supplierInvoiceModificationPM);

                    supplierInvoiceModificationPM.DeclarationId = supplierInvoice.DeclarationId;
                    supplierInvoiceModificationPM.Tenant = supplierInvoice.Tenant;
                    supplierInvoiceModificationPM.InvoiceCounterKey = supplierInvoice.InvoiceCounterKey;
                    supplierInvoiceModificationPM.Amount = ExpensesAmount;
                    supplierInvoiceModificationPM.CurrencyTypeCode = sExpensesAmountCurr;
                    //supplierInvoiceModificationPM.ModificationCounterKey = 1;
                    supplierInvoiceModificationPM.TypeCode = "160";
                    declarationPM.IsChanged = true;
                }

            }


        }
        //o	ערך ביטוח מעדכנים תמיד, בתנאי שחזק מבדיקת הביטוח
        if (!AppTool.IsNullOrEmpty(sInsuranseAmount) && !AppTool.IsNullOrEmpty(sInsuranseCurrency)) {
            let insuranseAmount = 0;
            if (Number(sInsuranseAmount) != NaN) {//if (decimal.TryParse(sInsuranseAmount, out insuranseAmount)) {
                insuranseAmount = Number(sInsuranseAmount);



                supplierInvoice.InsuranceAmount = insuranseAmount;
                supplierInvoice.InsruanceCurrencyTypeCode = sInsuranseCurrency;
                supplierInvoice.InsruancePercentage = null;
                //supplierInvoice.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                //declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                declarationPM.IsChanged = true;

                //VisibilityInsurance = "Collapsed";

                //var submit = context.SubmitChanges();
                //submit.Completed += submit_Completed;
            }

        }

    }


}
