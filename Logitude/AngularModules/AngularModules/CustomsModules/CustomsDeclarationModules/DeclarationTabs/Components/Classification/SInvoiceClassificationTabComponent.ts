

declare var window;
import { Component, AfterViewInit, ChangeDetectorRef, OnDestroy, ViewChild, ViewContainerRef, ElementRef } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, FontTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { SupplierInvoiceItemPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { TradeAgreementPM } from '../../../../../Customs/EntityPMs/TradeAgreementPM';
import { MeasurmentUnitPM } from '../../../../../Customs/EntityPMs/MeasurmentUnitPM';
import { CustomsCountryPM } from '../../../../../Customs/EntityPMs/CustomsCountryPM';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';

import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';

import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { CardPMService } from '../../../../../Common/Services/StandardPMs/CardPMService';
import { CustomsHouseTypeExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';

import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { CustomsVendorListService } from '../../../../../Customs/Services/StandardLists/CustomsVendorListService';
import { CustomsCountryListService } from '../../../../../Customs/Services/StandardLists/CustomsCountryListService';

import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import { CustomsRequiredFieldListService } from '../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
import { ApiQueryFilters, FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';

import { CustomsRequestMenuService } from '../../../../../Customs/Services/Others/CustomsRequestMenuService';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ItemCodeComponent } from '../../../../../Customsmodules/Customsdeclarationmodules/Declarationsupplierinvoice/Components/Supplierinvoices/SupplierInvoiceGeneralTabComponent';
import { LogCellTemplateComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/EditableLogGridComponent/LogCellTemplateComponent';
import { LuhnAlgorithm } from '../../../../../Customs/Utilities/LuhnAlgorithm';
import { CustomsVendorPMService } from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';
import { CustomsVendorPM } from '../../../../../Customs/EntityPMs/CustomsVendorPM';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { AddEditSupplierInvoiceDUMMY } from './DeclarationClassificationComponent';
import { SupplierInvoiceService } from '../../../../../Customs/Services/Others/SupplierInvoiceService';

import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { QuantityTypeMessageService } from '../../../../../Customs/Services/WebServices/QuantityTypeMessageService';
import { GITITEMCacheService } from '../../../../../Customs/Services/Others/GITITEMCacheService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';


@Component({
    selector: 'SInvoiceClassificationTabContent',
    moduleId: module.id,
    templateUrl: './SInvoiceClassificationTabComponent.html',
    providers: [DeclarationExtendedListService]
})

export class SInvoiceClassificationTabComponent
    extends BaseComponent
    implements OnDestroy, AfterViewInit {
    public EntityPM: SupplierInvoicePM;
    public declarationPM: DeclarationPM;

    public ObjectTableName: string = "Customs.SupplierInvoice";
    public DataContext: any = this;
    public Tab: LogTab;

    public IsDisplayOnly: boolean = false;
    public ParentIsDisplayOnly: boolean = false;
    ShowExcludeConsignmentBoolean: boolean = false;
    IsCourierDeclaration: boolean = false;

    public customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();
    vendor: CustomsVendorPM;
    public vendorNumber: string = "";
    
    public IsCountryPURForItems: boolean = false;
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService();

    public AddEditSupplierInvoiceDUMMYManager: AddEditSupplierInvoiceDUMMY;
    public declarationPMService: DeclarationPMService = new DeclarationPMService();
    ClasificationQtyTypes: { [code: string]: any; } = {};
    quantityTypeMessageService: QuantityTypeMessageService = new QuantityTypeMessageService();
    _viewContainerRefOfClassificationCode: LogCellTemplateComponent;


    //@ViewChild('logcelltemplateOfClassificationCode', { read: ViewContainerRef })
    //set(val: any) {
    //    this._viewContainerRefOfClassificationCode = val as LogCellTemplateComponent;
    //    this._viewContainerRefOfClassificationCode.IsEditMode = true;
    //}
    @ViewChild('logcelltemplateOfClassificationCode') myDiv: ElementRef;
    //myDiv: ElementRef;
    //@ViewChild('logcelltemplateOfClassificationCode')
    //set(val: any) {
    //    this.myDiv = val;
    //    let logCell = (this.myDiv as any);
    //    logCell.IsEditMode = true;
    //}
    public CurrentSession = SessionLocator.SelectedSession;
    IsDisplayMessage: boolean;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, public declarationExtendedListService: DeclarationExtendedListService) {
        super();
        //this.ConsimentPackages = new ObservableCollection([]);
        this.ItemsSource = new ObservableCollection([]);
        this.AddEditSupplierInvoiceDUMMYManager = new AddEditSupplierInvoiceDUMMY("");
        
        this.Listen();
    }
    private _SubDisplayModeChanged;
    private _SubConsignmentsChanged;
    ngAfterViewInit() {
        console.log('SInvoiceClassificationTabComponent:ngAfterViewInit():');
        let token = setTimeout(() => {
            console.log("viewContainerRefOfClassificationCode:", this.myDiv);
            let LogCellTemplateComponent = this.myDiv as any;
            if (LogCellTemplateComponent) {
                LogCellTemplateComponent.IsEditMode = true;
            }
            
            clearTimeout(token);
        }, 700);
    }
    ngOnDestroy() {
        console.log("SInvoiceClassificationTabComponent:ngOnDestroy");
        //if (this.Tab.ComponentReference && this.Tab.ComponentReference.ngOnDestroy) {
        //    this.Tab.ComponentReference.ngOnDestroy();
        //}
        if (this.Tab) {
            this.Tab.ComponentReference = null;
        }
        this.Tab = null;
        if (this._SubDisplayModeChanged) {
            this._SubDisplayModeChanged.unsubscribe();
            this._SubDisplayModeChanged = null;
        }
        if (this._SubConsignmentsChanged) {
            this._SubConsignmentsChanged.unsubscribe();
            this._SubConsignmentsChanged = null;
        }
    }
    private Listen() {
        this._SubDisplayModeChanged =
            DeclarationEventManager.DisplayModeChanged.subscribe((IsDisplayOnly: any) => {

                
                this.ParentIsDisplayOnly = IsDisplayOnly;

                this.SetScreenFieldsEditability();
                

            });
        this._SubConsignmentsChanged =
            DeclarationEventManager.ConsignmentsChanged.subscribe((e) => {
                console.log("ConsignmentsChanged", this.declarationPM, e);
                

            });
    }

    private isChecked: boolean = true;/// יש לשים לב ללוגיקות שקיימות במסך העבודה הרגיל. למשל:     בשינוי פרט מכס יש להקפיץ יחידת מידה (במסך הרגיל זה תלוי בסימון V, פה זה ההתנהגות הרגילה).
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(newValue: boolean) { this.isChecked = newValue; }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    public DisplayOnlyMessage: string = "";
    DisplayOnlyCheck() {
        if (this.CurrentSession.CurrentEditComponent.EditComponentController) {
            this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
                this.SetScreenFieldsEditability();
                DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
                return;
            }
            else {
            this.InitDisplayOnlyMessage();
        }

        }
        
    
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.CurrentSession.CurrentEditComponent.EntityPM).subscribe((response: any) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else {
                this.InitDisplayOnlyMessage();
            }
            
            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
        });
    }


    InitDisplayOnlyMessage() {
        if (this.declarationPM.IsAmendment && (this.declarationPM.AmendmentStatus == "2" || this.declarationPM.AmendmentStatus == null)) {
            this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.IsAmendment") + ' ' + this.declarationPM.AmendmentStatusName;
            this.IsDisplayMessage = true;
        }
        else if (this.declarationPM.IsAmendment == false) {
            this.declarationExtendedListService.GetDeclarationAmendmentsById(this.declarationPM.Id).subscribe
                (data => {
                    if (data.Result == null || data.Result.length <= 0) return;

                    data.Result = data.Result.sort((obj1, obj2) => {
                        if (obj1.amendmentissueDate > obj2.amendmentissueDate) {
                            return 1;
                        }

                        if (obj1.amendmentissueDate < obj2.amendmentissueDate) {
                            return -1;
                        }

                        return 0;
                    });

                    data.Result.forEach((item) => {
                        if (item.AmendmentStatus == "3" || item.AmendmentStatus == "1" || item.AmendmentStatus == "6") {
                            this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ExistsAmendments") + ' ' + item.AmendmentStatusName;
                            this.IsDisplayMessage = true;

                            return;
                        }

                    });
                    this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ExistsAmendments") + ' ' + data.Result[0].AmendmentStatusName;
                    this.IsDisplayMessage = true;

                }


                );

        }
    }

    SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        this.ParentIsDisplayOnly = args.Disabled;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            this.customsVendorPMService.get(this.EntityPM.VendorId).subscribe((myResponse: ServiceResponse) => {
                this.vendor = myResponse.Result;
                if (this.vendor != null) {
                    this.vendorNumber = this.vendor.VendorNumber;
                }
            });
        }
        this.AddEditSupplierInvoiceDUMMYManager = args.Parent.AddEditSupplierInvoiceDUMMYManager;
        this.GetFreightTotals();
        if (!this.declarationPM)
            this.declarationPM = args.Parent.DeclarationPM;

        this.IsCourierDeclaration = this.declarationPM.IsCourierDeclaration;
        this.ShowExcludeConsignmentBoolean = this.declarationPM.Consignments.length == 1;


        //this.EntityPM.PropertyChanged.subscribe((event) => { console.log("PropertyChanged: ", event); });


        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }




        // show xml errors
        if (!AppTool.IsNullOrEmpty(args.DecErrors)) {
            if (!AppTool.IsNullOrEmpty(args.DecErrors.Field)) {
                this.UIProperties.SetValidity(args.DecErrors.Field, "Customs.Consignment", false, args.DecErrors.Description);
            }
        }

        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
            .subscribe((customsSettingList: ServiceResponse) => {
                if (customsSettingList) {
                    this.CurrentSession.StopBusyIndicator();
                    if (this.AddEditSupplierInvoiceDUMMYManager.IsNewEntity) {
                        let autoFillAccountType = customsSettingList.Result ? customsSettingList.Result.AutoFillAccountType : false;
                        if (autoFillAccountType) {
                            //this.AccountTypeCode = "380";
                        }
                    }
                    let autoUnitMeasurement = customsSettingList.Result ? customsSettingList.Result.AutoUnitMeasurement : false;
                    if (autoUnitMeasurement) {
                        this.IsChecked = true;
                    }
                }
                this.GetCountryPURForItems();
            });
        //}


        var TempItemSource: SInvoiceItemClassificationLine[] = [];
        for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
            TempItemSource.push(new SInvoiceItemClassificationLine(this.EntityPM.SupplierInvoiceItems[i], this));
        }



        this.ItemsSource.InsertCollection(TempItemSource);
        this.CurrentSession.StopBusyIndicator();

        console.log("Tabs Args: ", args);
    }
    public supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
    GetFreightTotals() {
        this.supplierInvoiceService.GetTotalForeignCurrencyForInvoice(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey).subscribe(response => {



            this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = response.Result;
            //for (let item of items)// this.entitypm(d=> d. SupplierInvoiceItemViewModel item in InvoiceItemsObslist.Where(d => d.entityPM.CounterKey == 0))
            //{
            //    if (item.ItemPrice != null)
            //        this.TotalForeignCurrency = (TotalForeignCurrency != null ? TotalForeignCurrency : 0) + item.ItemPrice;
            //}
            if (isNaN(this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency)) this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
            var amount: number = this.InvoiceAmount;

            if (isNaN(this.InvoiceAmount)) amount = 0;
            this.AddEditSupplierInvoiceDUMMYManager.Difference = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - amount;

            if (this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency != 0) {
                if (this.AddEditSupplierInvoiceDUMMYManager.Difference != null) {
                    if (this.AddEditSupplierInvoiceDUMMYManager.Difference != 0) {
                        this.AddEditSupplierInvoiceDUMMYManager.DifferenceColor = FontTool.Red; //red
                    }
                    else {
                        this.AddEditSupplierInvoiceDUMMYManager.DifferenceColor = FontTool.Green; //green
                    }
                }
            }
            else {
                this.AddEditSupplierInvoiceDUMMYManager.DifferenceColor = FontTool.Black;
            }

        });
    }
    private GetCountryPURForItems() {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", this.declarationPM.Tenant)
            .subscribe(response => {
                this.CurrentSession.StopBusyIndicator();
                if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                    this.IsCountryPURForItems = true;
                }
            });
    }

  
     
    SelectedRow: SInvoiceItemClassificationLine;
    
    
    public ItemsSource: ObservableCollection;

    BuildItemsList() {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.ItemsSource.Clear();
        //this.ParentItems = [];
        //this.ChildrenItems = [];
        //var TempItemSource: SInvoiceItemClassificationLine[] = [];

        //this.AccumulatedMessageVisibility = false;
        for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
            //TempItemSource.push(new SInvoiceItemClassificationLine(this.EntityPM.SupplierInvoiceItems[i], this));
            this.ItemsSource.Insert(new SInvoiceItemClassificationLine(this.EntityPM.SupplierInvoiceItems[i], this));
        }


        //this.ItemsSource.InsertCollection(TempItemSource);
        this.CurrentSession.StopBusyIndicator();

        //  this.originalItemSource.InsertCollection(TempItemSource);


    }
    SetScreenFieldsEditability() {
        console.log("SetScreenFieldsEditability: " + this.EntityPM);
        this.UIProperties.SetEnabled("CargoDescription", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OriginCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ReceiverWarehouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsLastReleaseFromWarehous", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeliveryPlaceName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValue", this.ObjectTableName, !this.IsDisplayOnly);
    }

    calculateTotals(deleteItem: boolean, deletedItemPrice: number) {
        if (deleteItem) {
            if (deletedItemPrice == null) deletedItemPrice = 0;
            if (this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency == null) this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;

            this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - deletedItemPrice;
            this.AddEditSupplierInvoiceDUMMYManager.Difference = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - (this.InvoiceAmount);


        }
    }


    public get InvoiceAmount() { return this.EntityPM.InvoiceAmount; }
    public set InvoiceAmount(newValue: number) {

        if (this.EntityPM.InvoiceAmount != newValue) {
            //this.Parent.calculateCommission = true; //old

        }
        this.EntityPM.InvoiceAmount = newValue;
        if (this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency == null) this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
        this.AddEditSupplierInvoiceDUMMYManager.Difference = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - (newValue);
        

    }

    OnSelectedItemChanged(selectedRow: SInvoiceItemClassificationLine) {
        console.log("OnSelectedItemChanged > ", selectedRow);
        if (selectedRow) {

            if (this.SelectedRow != selectedRow) {
                if (this.SelectedRow) {
                    this.SelectedRow.ShowClassifierRemarkTooltip = false; // hide CR tooltip on prev selected row
                    selectedRow.closedManullay = false;

                    //this.SelectedRow.ShowTariffErrorTooltip = false; // hide Tariff tooltip on prev selected row
                }
            }
            if (!selectedRow.closedManullay)
                selectedRow.ShowClassifierRemarkTooltip = true;

            this.SelectedRow = selectedRow;

        } else {
            if (this.SelectedRow) {
                this.SelectedRow.ShowClassifierRemarkTooltip = false;
                this.SelectedRow.closedManullay = false;
            }
            this.SelectedRow = null;
        }

    }



    NumberOfLoadedItems: number = 50;// COURIER HAVE TO BE NO MORE THEN 10 
    EditButtonClicked(item: SInvoiceItemClassificationLine) {



        var errors = [];
        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);

        for (let item of this.CurrentSession.CurrentEditComponent.EntityPM.Consignments) {
            for (let line of item.ConsignmentPackages) {
                if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                    var errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                    if (!AppTool.IsNullOrEmpty(errorMessage)) {
                        errors.push(errorMessage);

                    }
                }

            }
        }


        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }


        else {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicator("");
                this.declarationPMService.update(this.CurrentSession.CurrentEditComponent.EntityPM).subscribe((response: ServiceResponse) => {
                    var declaration = response.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (!AppTool.IsNullOrEmpty(declaration)) {
                        if (!AppTool.IsNullOrEmpty(item)) {
                            this.EditInvoice(item);

                        }
                    }

                });
            }
            else {
                this.EditInvoice(item);
            }


        }



    }
    EditInvoice(mySInvoiceItemClassificationLine: SInvoiceItemClassificationLine) {
        this.CurrentSession.StartBusyIndicator("");
        
        var supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
        var decPM: DeclarationPM = this.CurrentSession.CurrentEditComponent.EntityPM;// this component 
        supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(decPM/*this.EntityPM*/.Id, this.EntityPM.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe(response => {

            var windowArgs: any = {};
            windowArgs.EntityPM = response.Result;
            windowArgs.declarationPM = decPM/*this.EntityPM*/;
            windowArgs.NumberOfLoadedItems = this.NumberOfLoadedItems;
            var windowTitle = "Supplier Invoice";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1017;// this changed By Rabaia for Task No. 54930; Dont change it back before calling me. //995; // don't change this width!
            logWindow.Height = 600;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) && !AppTool.IsNullOrEmpty(decPM/*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.InvoiceNumber + "-" + decPM/*this.EntityPM*/.DeclarationNumber;

            }
            else if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) && !AppTool.IsNullOrEmpty(decPM/*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + decPM/*this.EntityPM*/.DeclarationNumber;

            }
            else if (!AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) && AppTool.IsNullOrEmpty(decPM/*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.InvoiceNumber;

            }
            else if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) && AppTool.IsNullOrEmpty(decPM/*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

            }
            windowArgs.IsDisplayOnly = false;// //this.IsDisplayOnly;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            windowArgs.FromClassificationJumpToSII = mySInvoiceItemClassificationLine.entityPM.SequenceNumeric;
            ///this.CD.detach();
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event != 'cancel') {

                    this.RefreshEntity();
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    this.ReloadMyScreen();
                }
                ///this.CD.reattach();

            });
            logWindow.IsHideHeader = true;
          logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
          
            this.CurrentSession.StopBusyIndicator();
        });

    }
    ReloadMyScreen() { ///DSV - After Sending to Customs - Enter SUpplierInvoice and Getting Optimistic Concurancy error"
        ///this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        //this.getSupplierInvoices();
        //this.DisplayOnlyCheck();
    }

   

}




export class SInvoiceItemClassificationLine extends BaseComponent {
    public entityPM: SupplierInvoiceItemPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItem";
    public DataContext = this;
    Parent: SInvoiceClassificationTabComponent;
    public QuantityTypeCodeLoaded: any;
    IsBlueBorderVisibile: boolean = false;
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private customsVendorListService: CustomsVendorListService = new CustomsVendorListService();
    private _CustomsCountryListService: CustomsCountryListService = new CustomsCountryListService();

    public ShowClassefierRemarkInfo: boolean = false;
    public ShowClassifierRemarkTooltip: boolean = false;
    public ShowTariffErrorInfo: boolean = false;
    public ShowTariffErrorTooltip: boolean = false;

    public closedManullay: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(EntityPM: SupplierInvoiceItemPM, parent: SInvoiceClassificationTabComponent) {
        super();
        this.entityPM = EntityPM;

        //calculate ids
        if (this.entityPM.ClasifiedRemarks) {
            this.ShowClassefierRemarkInfo = true;
        }


        this.Parent = parent;
        this.oldvalue = this.entityPM.ItemPrice;
        
    }


    //#region Properties

    private editButtonName;
    get EditButtonName() { return this.editButtonName; }
    set EditButtonName(value: string) { this.editButtonName = value; }

    private itemAdditionalStatusVisibility = false;
    get ItemAdditionalStatusVisibility() { return this.itemAdditionalStatusVisibility; }
    set ItemAdditionalStatusVisibility(value: boolean) { this.itemAdditionalStatusVisibility = value; }

    
    measurmentUnit: MeasurmentUnitPM;
    get MeasurmentUnit() { return this.measurmentUnit; }
    set MeasurmentUnit(value: MeasurmentUnitPM) {

        if (this.measurmentUnit != value) {
            this.measurmentUnit = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.InvoiceQuantityTypeName = value.LocalName;


        } else {
            this.InvoiceQuantityTypeName = null;
            this.InvoiceQuantityType = null;
        }
    }


    tradeAgreement: TradeAgreementPM;
    get TradeAgreement() { return this.tradeAgreement; }
    set TradeAgreement(value: TradeAgreementPM) {

        if (this.tradeAgreement != value) {
            this.tradeAgreement = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.TradeAgreementName = value.LocalName;


        } else {
            this.TradeAgreementName = null;
            this.TradeAgreementCode = null;
        }
    }


    customsCountry: CustomsCountryPM;
    get CustomsCountry() { return this.customsCountry; }
    set CustomsCountry(value: CustomsCountryPM) {

        if (this.customsCountry != value) {
            this.customsCountry = value;
            //this.CheckTariff();
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginCountryName = value.LocalName;
            if (!AppTool.IsNullOrEmpty(this.ItemCode) && this.Parent.IsCountryPURForItems) {
                //var itemCodeDetails = this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);/*ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0]*/;
                if (itemCodeDetails != null) {
                    itemCodeDetails.OriginCountryCode = value.Code;
                    itemCodeDetails.OriginCountryName = value.LocalName;
                    itemCodeDetails.IsNew = true;
                }
            }

        } else {
            this.OriginCountryName = null;
            this.OriginCountryCode = null;
            if (!AppTool.IsNullOrEmpty(this.ItemCode) && this.Parent.IsCountryPURForItems) {
                //var itemCodeDetails = this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);//ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails != null) {
                    itemCodeDetails.OriginCountryCode = null;
                    itemCodeDetails.OriginCountryName = null;
                    itemCodeDetails.IsNew = true;
                }
            }
        }
    }

    public get OrderByLineNo() { return this.entityPM.OrderByLineNo; }
    public set OrderByLineNo(newValue: string) { this.entityPM.OrderByLineNo = newValue; }

    public get SequenceNumeric() { return this.entityPM.SequenceNumeric; }
    public set SequenceNumeric(newValue: number) { this.entityPM.SequenceNumeric = newValue; }

    
    public get ItemCode() { return this.entityPM.ItemCode; }
    //public set ItemCode(newValue: string) { this.entityPM.ItemCode = newValue; }

    public get ItemDescription() { return this.entityPM.ItemDescription; }
    //public set ItemDescription(newValue: string) { this.entityPM.ItemDescription = newValue; }

    public get NotForAccumaltion() { return this.entityPM.NotForAccumaltion; }
    public set NotForAccumaltion(value: boolean) {
        this.entityPM.NotForAccumaltion = value;
    }

    public get SearchFields() { return this.entityPM.SearchFields; }
    public set SearchFields(value: string) {
        this.entityPM.SearchFields = value;
    }


    valid: boolean = true;
    digit: string = null;
    checkDigit: number = 0;
    public SetDirty() {
        if (this.CurrentSession.CurrentEditComponent == null) return;
        var declarationPM = this.CurrentSession.CurrentEditComponent.EntityPM as DeclarationPM;
        declarationPM.IsDirty = true;
        this.entityPM.ChangeSetOp = "Update";
        let si: SupplierInvoicePM = declarationPM.SupplierInvoices.filter(si =>
            //si.DeclarationId == this.entityPM.DeclarationId &&
            si.InvoiceCounterKey == this.entityPM.CounterKey)[0];
        si.IsDirty = true;
        (si as any).ChangeSetOp = "Update";  //ca sera sera
    }
    public get ClassificationCode() { return this.entityPM.ClassificationCode; }
    public set ClassificationCode(newValue: string) {

        this.entityPM.ClassificationCode = newValue;
        this.SetDirty();
        if (newValue == null) {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");

        }

    }

    public get TradeAgreementCode() { return this.entityPM.TradeAgreementCode; }
    public set TradeAgreementCode(newValue: string) {
        this.SetDirty();
        this.entityPM.TradeAgreementCode = newValue;
        //this.CheckTariff();
    }

    public get TradeAgreementName() { return this.entityPM.TradeAgreementName; }
    public set TradeAgreementName(newValue: string) {
        this.entityPM.TradeAgreementName = newValue;
    }


    public get InvoiceQuantity() { return this.entityPM.InvoiceQuantity; }
    public set InvoiceQuantity(newValue: number) {
        this.SetDirty();
        this.entityPM.InvoiceQuantity = newValue;
    }


    public get InvoiceQuantityType() { return this.entityPM.InvoiceQuantityType; }
    public set InvoiceQuantityType(newValue: string) {
        this.SetDirty();
        this.entityPM.InvoiceQuantityType = newValue;
    }

    public get InvoiceQuantityTypeName() { return this.entityPM.InvoiceQuantityTypeName; }
    public set InvoiceQuantityTypeName(newValue: string) { this.entityPM.InvoiceQuantityTypeName = newValue; }
    oldvalue: number = 0;
    doCalculate: boolean = false;
    public get ItemPrice() { return this.entityPM.ItemPrice; }
    public set ItemPrice(newValue: number) {
        this.SetDirty();
        if (newValue != this.entityPM.ItemPrice) {
            this.doCalculate = true;
            this.entityPM.ItemPrice = newValue;
        }


    }

    public get OriginCountryCode()
    { return this.entityPM.OriginCountryCode; }
    public set OriginCountryCode(newValue: string) {
        this.SetDirty();
        this.entityPM.OriginCountryCode = newValue;
    }

    public get OriginCountryName() { return this.entityPM.OriginCountryName; }
    public set OriginCountryName(newValue: string) { this.entityPM.OriginCountryName = newValue; }

    private qunatityTypeCode: string;
    public get QunatityTypeCode() { return this.qunatityTypeCode; }
    public set QunatityTypeCode(newValue: string) { this.qunatityTypeCode = newValue; }


    public get IsParent() { return this.entityPM.IsParent; }
    public set IsParent(newValue: boolean) { this.entityPM.IsParent = newValue; }

    public get ClasifiedRemarks() { return this.entityPM.ClasifiedRemarks; }
    public set ClasifiedRemarks(newValue: string) { this.entityPM.ClasifiedRemarks = newValue; }

    _TariffErrorText: string;
    public get TariffErrorText() { return this._TariffErrorText; }
    public set TariffErrorText(newValue: string) { this._TariffErrorText = newValue; }



    //#endregion

    OnItemPriceLostFocus(value: number) {

        if (this.doCalculate) {
            if (isNaN(this.ItemPrice)) this.ItemPrice = 0;
            if (this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency == null) this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
            if (isNaN(this.oldvalue)) this.oldvalue = 0;
            var totalFCurr: number = this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency;
            this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = totalFCurr - this.oldvalue + this.ItemPrice;
            this.Parent.AddEditSupplierInvoiceDUMMYManager.Difference = this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - (this.Parent.InvoiceAmount);
        }
        this.oldvalue = this.entityPM.ItemPrice;
        this.doCalculate = false;
    }

    
    
    

 
    ClassificationKeyUp(event, logCellTemplate: any, classificationTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnClassificationLostFocus(logCellTemplate, classificationTextBox);
        }
    }
    OnClassificationLostFocus(logCellTemplate: any, classificationTextBox: any) {
        var newValue = this.ClassificationCode;
        this.valid = true;
        logCellTemplate.IsEditMode = false;
        this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }
        else if (newValue.toString().length > 11) {
            this.valid = false;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));

        }
        else if (newValue.toString().length < 8) {
            this.valid = false;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));

        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + this.checkDigit;
            this.valid = true;;

        }
        else if (newValue.toString().length == 9) {
            this.digit = newValue.toString().substring(8);

            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));

            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());

            }
            else {
                this.valid = true;
            }

        }
        else if (newValue.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + this.checkDigit;
            this.valid = true;

        }
        else if (newValue.toString().length == 11) {
            this.digit = newValue.toString().substring(10);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());

            }
            else {
                this.valid = true;
            }


        }

        else {
            this.valid = true;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }

        this.ClassificationCode = newValue;
        classificationTextBox.TextValue = newValue;
        if (this.valid) {
            SessionLocator.SustainFocusOnCell = false;

            if (!AppTool.IsNullOrEmpty(this.ItemCode)) {
              //var itemCodeDetails = this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);//.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
              
                if (itemCodeDetails == null) {
                    var originCountryCode: string = null;
                    var originCountryName: string = null;
                    if (this.Parent.IsCountryPURForItems) {
                        originCountryCode = this.OriginCountryCode;
                        originCountryName = this.OriginCountryName;
                    }
                  //this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.push(new ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, null));
                  GITITEMCacheService.Instance.AddItemCodeComponent(/*ItemCode_LocalCache.push(*/
                    new ItemCodeComponent(
                      this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, null,
                      this.Parent.declarationPM.CustomerCode));
                  
                }
                else {
                    if (itemCodeDetails.ClassificationCode != this.ClassificationCode || itemCodeDetails.ItemDescription != this.ItemDescription) {
                        itemCodeDetails.ClassificationCode = this.ClassificationCode;
                        itemCodeDetails.ItemDescription = this.ItemDescription;
                        itemCodeDetails.VendorNumber = this.Parent.vendorNumber;
                        if (this.Parent.IsCountryPURForItems) {
                            itemCodeDetails.OriginCountryCode = this.OriginCountryCode;
                            itemCodeDetails.OriginCountryName = this.OriginCountryName;
                        }
                        itemCodeDetails.IsNew = true;
                    }
                }
                
            }
            this.GetQuantityType();
        }
        else {
            //var element = document.getElementById(logCellTemplate.OuterDivId);
            // element.focus();
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: classificationTextBox.InputId });

        }
        if (this.Parent.IsChecked) { //private isChecked: boolean = true;/// יש לשים לב ללוגיקות שקיימות במסך העבודה הרגיל. למשל:     בשינוי פרט מכס יש להקפיץ יחידת מידה (במסך הרגיל זה תלוי בסימון V, פה זה ההתנהגות הרגילה).

            if (this.InvoiceQuantityType == null && this.QunatityTypeCode != null) {
                var s = this.QunatityTypeCode.slice(1, this.QunatityTypeCode.length - 1);


                this.InvoiceQuantityType = s;

            }

        }
    }

    GetQuantityType(isChangeInvoiceQuantityType: boolean = true) {
        if (this.ClassificationCode != null) {
            var keys = Object.keys(this.Parent.ClasificationQtyTypes);
            if (keys.indexOf(this.ClassificationCode) > -1) {
                var result: string = this.Parent.ClasificationQtyTypes[this.ClassificationCode];
                if (result) {
                    this.QunatityTypeCode = "(" + result + ")";
                    if (this.Parent.IsChecked && isChangeInvoiceQuantityType) {
                        if (this.InvoiceQuantityType == null && result != null) {
                            this.InvoiceQuantityType = result;
                        }
                    }
                }
            }
            else {
                this.Parent.ClasificationQtyTypes[this.ClassificationCode] = null;
                var code = this.ClassificationCode.toString().slice(0, this.ClassificationCode.toString().length - 1);
                this.Parent.quantityTypeMessageService.GetQuantityType(code).subscribe((myServiceResponse: ServiceResponse) => {
                    if (!myServiceResponse.HasError) {


                        if (!myServiceResponse.HasError) {
                            if (myServiceResponse.Result) {
                                this.QunatityTypeCode = "(" + myServiceResponse.Result + ")";
                            }
                            else {
                                this.QunatityTypeCode = null;
                            }


                            if (this.Parent.IsChecked && isChangeInvoiceQuantityType) {
                                if (this.InvoiceQuantityType == null && myServiceResponse.Result != null) {
                                    this.InvoiceQuantityType = myServiceResponse.Result;
                                }
                            }
                            if (!(keys.indexOf(this.ClassificationCode) > -1)) {
                                this.Parent.ClasificationQtyTypes[this.ClassificationCode] = myServiceResponse.Result;
                            }
                            this.CurrentSession.QuantityTypeCodeLoadedEvent.emit({ ClassificationCode: this.ClassificationCode, QuantityTypeCode: this.QunatityTypeCode });
                            //QuantityTypeCodeLoadedEvent quantityLoadedEvent = currentAssemlyLocator.EventAggregator.GetEvent<QuantityTypeCodeLoadedEvent>();
                            //quantityLoadedEvent.Publish(new QuantityTypeCodeLoadedEventArgs() { ClassificationCode = ClassificationCode, QuantityTypeCode = this.QunatityTypeCode });
                        }
                    }



                });
            }
        }

        else {

            this.QunatityTypeCode = null;
        }
    }
    OnOriginCountryCodeLostFocus(logCellTemplate: any, originCountryCodeLov: any) {
        if (!this.Parent.IsCountryPURForItems) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.ItemCode)) {
            var itemCodeDetails = /*this.Parent.AddEditSupplierInvoiceDUMMYManager*/GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);//.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
          
            if (itemCodeDetails != null) {
                itemCodeDetails.OriginCountryCode = this.OriginCountryCode != null ? this.OriginCountryCode : this.customsCountry != null ? this.customsCountry.Code : null;
                itemCodeDetails.OriginCountryName = this.OriginCountryName != null ? this.OriginCountryName : this.customsCountry != null ? this.customsCountry.LocalName : null;
                itemCodeDetails.IsNew = true;
            }
        }
    }
    

   
    Dispose() {
        this.Parent = null;
        this.DataContext = null;


        if (this.QuantityTypeCodeLoaded) {
            this.QuantityTypeCodeLoaded.unsubscribe();
        }

    }



   



}
