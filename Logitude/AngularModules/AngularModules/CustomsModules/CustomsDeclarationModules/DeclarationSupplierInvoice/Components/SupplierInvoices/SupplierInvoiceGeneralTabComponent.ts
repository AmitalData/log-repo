
declare var window: any;
declare var document: Document;
import { Component, ChangeDetectorRef, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { CustomsVendorPMService } from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsVendorPM } from '../../../../../Customs/EntityPMs/CustomsVendorPM';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { SupplierInvoiceItemPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { TradeAgreementPM } from '../../../../../Customs/EntityPMs/TradeAgreementPM';
import { MeasurmentUnitPM } from '../../../../../Customs/EntityPMs/MeasurmentUnitPM';
import { CustomsCountryPM } from '../../../../../Customs/EntityPMs/CustomsCountryPM';
import { AppTool, FontTool } from '../../../../../Infrastructure/Tools';
import { AddEditSupplierInvoiceComponent } from './AddEditSupplierInvoiceComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomsDocumentPointerPM } from '../../../../../Customs/EntityPMs/CustomsDocumentPointerPM';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { SupplierInvoiceFreightAmountPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceFreightAmountPM';
import { CurrencyTypePM } from '../../../../../Customs/EntityPMs/CurrencyTypePM';
import { CustomsExchangeRateExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { CustomsExchangeRatePM } from '../../../../../Customs/EntityPMs/CustomsExchangeRatePM';
import { SupplierInvoiceService } from '../../../../../Customs/Services/Others/SupplierInvoiceService';
import { TermsOfSaleTypeListService } from '../../../../../Customs/Services/StandardLists/TermsOfSaleTypeListService';
import { TermsOfSaleTypeList } from '../../../../../Customs/EntityLists/TermsOfSaleTypeList';
import { LuhnAlgorithm } from '../../../../../Customs/Utilities/LuhnAlgorithm';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { QuantityTypeMessageService } from '../../../../../Customs/Services/WebServices/QuantityTypeMessageService';
import { LogCellTemplateComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/EditableLogGridComponent/LogCellTemplateComponent';
import { CustomsRequiredFieldListService } from '../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
import { FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { CustomsVendorListService } from '../../../../../Customs/Services/StandardLists/CustomsVendorListService';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { SupplierInvoicePMService } from '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import { ImporterDespositionClass } from '../../../../../Customs/DataContract/ImporterDespositionClass';
import { DateTimeToDatePipe } from '../../../../../Controls/Pipes/DateTimeToDatePipe';
import { SupplierInvoiceItemProcesTypePM } from '../../../../../Customs/EntityPMs/SupplierInvoiceItemProcesTypePM';
import { ItemGovernmentProcedureTypeListService } from '../../../../../Customs/Services/StandardLists/ItemGovernmentProcedureTypeListService';
import { ItemGovernmentProcedureTypeList } from '../../../../../Customs/EntityLists/ItemGovernmentProcedureTypeList';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsCountryListService } from '../../../../../Customs/Services/StandardLists/CustomsCountryListService';
import { GITITEMCacheService } from '../../../../../Customs/Services/Others/GITITEMCacheService';
import { DecimalPipe } from '@angular/common';
import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import { CustomsRequiredFieldExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsRequiredFieldExtendedListService';
import { SupplierInvoiceModificationPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceModificationPM';
import { ModificationAndDiscountTypeList } from '../../../../../Customs/EntityLists/ModificationAndDiscountTypeList';
import { ModificationAndDiscountTypeListService } from '../../../../../Customs/Services/StandardLists/ModificationAndDiscountTypeListService';
import { any } from 'cypress/types/bluebird';
import { CurrencyTypeListService } from '../../../../../Customs/Services/StandardLists/CurrencyTypeListService';
import { GITITEMCR } from 'Customs/EntityPMs/Extended/GITITEMCR';
import { SupplierInvioceItemCertificatPM } from '../../../../../Customs/EntityPMs/SupplierInvioceItemCertificatPM';
import { MultiCertificateUpdateComponent } from '../../../../../CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/MultiCertificateUpdate/MultiCertificateUpdateComponent';
import { TradeAgreementListService } from 'Customs/Services/StandardLists/TradeAgreementListService';
import { TradeAgreementProtocolListService } from 'Customs/Services/StandardLists/TradeAgreementProtocolListService';
import { IncotemrsFileValidationListService } from 'Customs/Services/StandardLists/IncotemrsFileValidationListService';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { IncotemrsFileValidationList } from 'Customs/EntityLists/IncotemrsFileValidationList';
import { customsItemsService } from 'QuoteOPM/Utilities/customsItems.service';
import { SupplierInvoiceSharedService } from './Services/SupplierInvoiceSharedService';

import { VendorCurrencyService } from 'Customs/Services/WebServices/VendorCurrencyService';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { CustomsItemDetailsQueryRequestParams } from 'Customs/DataContract/RequestParams/CustomsItemDetailsQueryRequestParams';
import { SendRequestVIA } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { IIGGeneralMessagesService } from 'Customs/Services/WebServices/IIGGeneralMessagesService';
import { ClientIndicationPM } from 'Customs/EntityPMs/ClientIndicationPM';
import { ClientIndicationListService } from 'Customs/Services/StandardLists/ClientIndicationListService';
import { ClientItemExtendedPMService } from 'Customs/Services/ExtendedPMs/ClientItemExtendedPMService';
import { ClientItemPM } from 'Customs/EntityPMs/ClientItemPM';


@Component({

    templateUrl: './SupplierInvoiceGeneralTabComponent.html',
})


export class SupplierInvoiceGeneralTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public CurrencyTypeCode: any;
    public OriginCountryCode: any;
    hasOcr = false;
    public InvoiceTypeFocus: boolean;
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.SupplierInvoice";
    public EntityPM: SupplierInvoicePM;
    public customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();
    vendor: CustomsVendorPM;
    public ItemsSource: ObservableCollection;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public Parent: AddEditSupplierInvoiceComponent;
    Pointers: CustomsDocumentPointerPM[];
    public AmountList: ObservableCollection;
    public AdjustmentsList: ObservableCollection;
    private _entityListService: EntityListService;
    public declarationPM: DeclarationPM;
    isNewEntity: boolean;
    FreightCopyList: SupplierInvoiceFreightAmountLine[];
    AdjustmentsCopyList: ObservableCollection;
    public VendorFilterItems: ApiQueryFilters;
    ChangeScrollPosition: EventEmitter<any> = new EventEmitter();
    public vendorNumber: string = "";
    AccumulatedMessageVisibility: boolean;
    AccumulatedMessageText: string;
    public supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
    public termsOfSaleTypeListService: TermsOfSaleTypeListService = new TermsOfSaleTypeListService();
    public customsExchangeRateExtendedPMService: CustomsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService();
    supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
    incotemrsFileValidationListService: IncotemrsFileValidationListService = new IncotemrsFileValidationListService();
    public allowExport: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsReadOnly: boolean = false;
    //public IsCountryPURForItems: boolean = false;

    quantityTypeMessageService: QuantityTypeMessageService = new QuantityTypeMessageService();
    ClasificationQtyTypes: { [code: string]: any; } = {};
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    itemGovernmentProcedureTypeListService: ItemGovernmentProcedureTypeListService = new ItemGovernmentProcedureTypeListService();
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    private modificationAndDiscountTypeListService: ModificationAndDiscountTypeListService = new ModificationAndDiscountTypeListService();
    vendorCurrencyService: VendorCurrencyService = new VendorCurrencyService();
    IsActionButtonsEnabled: boolean = true;

    ikeaFeature: any;
    IFritz_feature: any;
    @Output() ReloadEntityEvent: EventEmitter<any> = new EventEmitter();
    isMasterInvoic: boolean = false;
    IsValueForCustomsOnlyVisible: boolean = false;
    isInsurance: boolean = false;
    isFreightCharge: boolean = false;
    TooltipCopy: string
    TooltipCertificate: string;
    TooltipCar: string;
    TooltipEdit: string;
    SumDifference: number = 0;
    ConUNF = true;
    PratMehesUNF = [];

    old_currency;
    old_amount;
    old_vendor;
    updateOptionsMap = new Map<UpdateOptions, UpdateGeneralParams>();
    public addedVehicles: any[] = [];
    clientIndicationListService: ClientIndicationListService = new ClientIndicationListService();
    clientIndicationList: Array<ClientIndicationPM> = new Array<ClientIndicationPM>();
    private CurrentSession = SessionLocator.SelectedSession;
    public tradeAgreementFilter: ApiQueryFilters = null as any;
    public ModificationAndDiscountTypeList = new Map<string, string>();
    constructor(
        private supplierInvoiceSharedService: SupplierInvoiceSharedService,
        private cd: ChangeDetectorRef,
        private logtuideTableDataService: LogtuideTableDataService,
    ) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.AmountList = new ObservableCollection([]);
        this._entityListService = new EntityListService();
        this.FreightCopyList = [];
        this.VendorFilterItems = new ApiQueryFilters();

        this.VendorFilterItems.addAdditionalFilter("StatusCode", "1", "NULL", null, "Equals", false, false, false, "string", false, true);
        //  this.VendorFilterItems.addAdditionalFilter("StatusCode", "NULL", null, null, "Equals", false, false, false, "string", false, true);
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];



        //this.accumulationFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCUMULATION") && f.ObjectTableId == table.Id)[0];
        this.accumulationFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "ACCUMULATION")
        if (this.accumulationFeature) {
            this.IsAccumulationStateVisibile = true;
            this.IsNotForAccumaltionVisibile = true;
        }
        else {
            this.IsNotForAccumaltionVisibile = false;
        }
        //this.CurrentSession.SubscriptionAdd(
        //this.CurrentSession.SelectInvoiceItemEvent.subscribe((res) => {
        //    var item: SupplierInvoiceItemLine = this.ItemsSource.Collection.filter(d => d.SequenceNumeric == res.filter)[0];
        //    this.SelectedRow = item;
        //    var index = this.ItemsSource.Collection.indexOf(item);
        //    this.ChangeScrollPosition.emit({ RowIndex: index});
        //    })
        //);


        this.ikeaFeature = FeatureLocator.Features.filter(f => (f.Code == "IKEA") && f.ObjectTableId == table.Id)[0];

        //FRITZ
        this.IFritz_feature = FeatureLocator.Features.filter(d => d.Code == "IFRITZ")[0];
        console.log("IFritz feature: ", this.IFritz_feature);

        this.initTradeAgreementFilter();
    }

    private buildQuantityTypeGeneralParams(){
        let params= new UpdateGeneralParams();
        params.Title=TextCodeTranslator.Translate("Customs.Declaration.O.CopyNow");
        params.Arguments=new UpdateGeneralArgsParams();
        params.Arguments.UpdateField ='InvoiceQuantityType';
        params.Arguments.Title=TextCodeTranslator.Translate("Customs.Declaration.O.MultiQantityType");
        params.Arguments.IsItemsWithNoValue= true;
        params.Arguments.LookUpTableName ='Customs.MeasurmentUnit';
        params.Arguments.ObjectTableName ='Customs.SupplierInvoiceItem';
        params.Arguments.ItemsWithNoValueTitle=TextCodeTranslator.Translate('Customs.Declaration.O.ItemsWithNoQuantityType');
        params.Arguments.SelectionCompletedMethod=  (comp) => {
            this.SelectionOriginCompleted(comp);
         };   
        this.updateOptionsMap[UpdateOptions.QuantityType]= params;
    }

    private buildProcessCodeGeneralParams(){
        let params= new UpdateGeneralParams();
        params.Title=TextCodeTranslator.Translate("Customs.Declaration.O.UpdateProcessCode");
        params.Arguments=new UpdateGeneralArgsParams();
        params.Arguments.UpdateField ='ProcessTypeCode';
        params.Arguments.LookUpTableName = 'Customs.ItemGovernmentProcedureType';
        params.Arguments.ObjectTableName ='Customs.SupplierInvoiceItemProcesType';
        params.Arguments.Title=TextCodeTranslator.Translate("Customs.Declaration.O.MultiProcessCode");
        params.Arguments.IsItemsWithNoValue= false;
        params.Arguments.SelectionCompletedMethod= (comp) => {
            this.SelectionCompleted(comp);
         }; ;   
        this.updateOptionsMap[UpdateOptions.ProcessCode]= params;
    }

    private buildCountryOfOriginGeneralParams(){
        let params= new UpdateGeneralParams();
        params.Title=TextCodeTranslator.Translate("Customs.Declaration.O.UpdateCountryOfOrigin");
        params.Arguments=new UpdateGeneralArgsParams();
        params.Arguments.UpdateField ='OriginCountryCode';
        params.Arguments.Title=TextCodeTranslator.Translate("Customs.Declaration.O.MultiCountryOfOrigin");
        params.Arguments.ItemsWithNoValueTitle=TextCodeTranslator.Translate("Customs.Declaration.O.ItemsWithNoCountrOfOrigin");
        params.Arguments.IsItemsWithNoValue= true;
        params.Arguments.LookUpTableName='Customs.CustomsCountry';
        params.Arguments.ObjectTableName ='Customs.SupplierInvoiceItem';
        params.Arguments.SelectionCompletedMethod= (comp) => {
            this.SelectionOriginCompleted(comp);
         };   
        this.updateOptionsMap[UpdateOptions.CountryOfOrigin]= params;
    }

    private buildClassificationCodeGeneralParams(){
        let params= new UpdateGeneralParams();
        params.Title=TextCodeTranslator.Translate("Customs.Declaration.O.UpdateClassificationCode");
        params.Arguments=new UpdateGeneralArgsParams();
        params.Arguments.UpdateField ='ClassificationCode';
        params.Arguments.Title=TextCodeTranslator.Translate("Customs.Declaration.O.MultiClassificationCode");
        params.Arguments.Validate=SupplierInvoiceItemLine.validateClassificationCode;
        params.Arguments.ItemsWithNoValueTitle=TextCodeTranslator.Translate("Customs.Declaration.O.ItemsWithNoClassificationCode");
        params.Arguments.IsItemsWithNoValue= true;
        params.Arguments.SelectionCompletedMethod= (comp) => {
            this.SelectionOriginCompleted(comp);
         }; 
        params.Arguments.ObjectTableName ='Customs.SupplierInvoiceItem'; 
        this.updateOptionsMap[UpdateOptions.ClassificationCode]= params;	
    }

    private buildProtocolCodeGeneralParams(){
        let params= new UpdateGeneralParams();
        params.Title=TextCodeTranslator.Translate("Customs.Declaration.O.UpdateProtocolCode");
        params.Arguments=new UpdateGeneralArgsParams();
        params.Arguments.UpdateField ='DutyRegimeProtocolCode';
        params.Arguments.Title=TextCodeTranslator.Translate("Customs.Declaration.O.MultiProtocolCode");
        params.Arguments.Validate=SupplierInvoiceItemLine.validateClassificationCode;
        params.Arguments.ItemsWithNoValueTitle=TextCodeTranslator.Translate("Customs.Declaration.O.ItemsWithNoProtocolCode");
        params.Arguments.IsItemsWithNoValue= true;
        params.Arguments.LookUpTableName='Customs.TradeAgreementProtocol';
        params.Arguments.SelectionCompletedMethod= (comp) => {
            this.SelectionOriginCompleted(comp);
         }; 
        params.Arguments.ObjectTableName ='Customs.SupplierInvoiceItem';   
        this.updateOptionsMap[UpdateOptions.ProtocolCode]= params;	
    }

    private buildTradeAgreementGeneralParams(){
        let params= new UpdateGeneralParams();
        params.Title=TextCodeTranslator.Translate("Customs.Declaration.O.UpdateTradeAgreement");
        params.Arguments=new UpdateGeneralArgsParams();
        params.Arguments.UpdateField ='TradeAgreementCode';
        params.Arguments.Title=TextCodeTranslator.Translate("Customs.Declaration.O.MultiTradeAgreement");
        params.Arguments.ItemsWithNoValueTitle=TextCodeTranslator.Translate("Customs.Declaration.O.ItemsWithNoTradeAgreement");
        params.Arguments.IsItemsWithNoValue= true;
        params.Arguments.LookUpTableName='Customs.TradeAgreement';
        params.Arguments.SelectionCompletedMethod= (comp) => {
            this.SelectionOriginCompleted(comp);
         };    
        params.Arguments.ObjectTableName ='Customs.SupplierInvoiceItem'; 
        this.updateOptionsMap[UpdateOptions.TradeAgreement]= params;	
    }

    ngOnInit() {
        this.buildQuantityTypeGeneralParams();
        this.buildProcessCodeGeneralParams();
        this.buildCountryOfOriginGeneralParams();
        this.buildClassificationCodeGeneralParams();
        this.buildProtocolCodeGeneralParams();
        this.buildTradeAgreementGeneralParams();

        this.hasOcr = FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR");
        if (this.allowExport) {
            this.TooltipCopy = "שכפל שורה";
            this.TooltipCertificate = "םישורים"
            this.TooltipCar = "נתוני רכב";
            this.TooltipEdit = "עריכת פריט";
            this.setAdjustmentsWarning(this.IncotermCode)
        }
        if (this.declarationPM.Direction == 'E') {

            this.modificationAndDiscountTypeListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                if (response.Result) {

                    response.Result.forEach(item => {

                        this.ModificationAndDiscountTypeList.set(item.Code, item.NetoValuesModificationAffectID)

                    });
                }
            });

        }
        this.supplierInvoiceSharedService.DifferenceAndTotalForeignCurrency$.subscribe(() => this.GetDifferenceAndTotalForeignCurrency())
    }


    initTradeAgreementFilter() {
        this.tradeAgreementFilter = new ApiQueryFilters();
        this.tradeAgreementFilter.addAdditionalFilter("CustomsBookTypeID", 2, null, null, "Equal", false, false, false, "number");
    }


    public SelectInvoiceItemMethod(res) {
        var item: SupplierInvoiceItemLine = this.ItemsSource.Collection.filter(d => d.SequenceNumeric == res.filter)[0];
        //this.SelectedRow = item;
        this.OnSelectedItemChanged(item); // set this.SelectedRow
        var index = this.ItemsSource.Collection.indexOf(item);
        this.ChangeScrollPosition.emit({ RowIndex: index });

    }

    ngOnDestroy() {
        console.log("SupplierInvoiceGeneralTabComponent:ngOnDestroy");
        this.cd = null;
        this.Parent = null
        if (this.ItemsSource) {
            this.ItemsSource.Collection.forEach(item => {
                var siil: SupplierInvoiceItemLine = item;
                (siil as any).Dispose();
                siil.Parent = null;
                siil.DataContext = null;


            });
            this.ItemsSource.Clear();
            this.ItemsSource = null;
        }
        if (this.AmountList) {
            this.AmountList.Collection.forEach(item => {
                var amm: SupplierInvoiceFreightAmountLine = item;
                amm.Parent = null;
                amm.DataContext = null;

            });

            this.AmountList.Clear();
            this.AmountList = null;
        }
        this.FreightCopyList = null;
        this.Dispose();
    }

    accumulationFeature: any;
    IsNotForAccumaltionVisibile: boolean = true;
    IsAccumulationStateVisibile: boolean = false;
    ParentItems: SupplierInvoiceItemPM[];
    ChildrenItems: SupplierInvoiceItemPM[];
    ParentsCount: string;
    ChildrenCount: string;
    SelectedRow: SupplierInvoiceItemLine;
    BuildItemsList() {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        if (this.ItemsSource != null) {
            this.ItemsSource.Clear();
        }
        this.ParentItems = [];
        this.ChildrenItems = [];
        var TempItemSource: SupplierInvoiceItemLine[] = [];
        if (this.IsAccumulated) {
            this.AccumulatedMessageVisibility = true;


            this.ParentItems = this.EntityPM.SupplierInvoiceItems.filter(d => d.IsParent);

            this.ChildrenItems = this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent);

            if (this.IsFromCustomsAnswer && !this.IsInvoiceAnswer) {
                this.ParentsCount = "(" + this.ParentItems.length.toString() + ")";
                this.ChildrenCount = "(" + this.ChildrenItems.length.toString() + ")";

            }
            if (this.AccumulatedFilterSelectedValue == 'Accumulated') {

                this.IsReadOnly = true;
                this.IsNotForAccumaltionVisibile = false;
                for (var i = 0; i < this.ParentItems.length; i++) {
                    TempItemSource.push(new SupplierInvoiceItemLine(this.ParentItems[i], this, this.allowExport));
                }

            }
            else if (this.AccumulatedFilterSelectedValue == 'NotAccumulated') {
                this.ChildrenItems = this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent);
                if (!this.IsDisplayOnly) {
                    this.IsReadOnly = false;
                }
                if (this.accumulationFeature) {
                    this.IsNotForAccumaltionVisibile = true;
                }
                for (var i = 0; i < this.ChildrenItems.length; i++) {
                    TempItemSource.push(new SupplierInvoiceItemLine(this.ChildrenItems[i], this, this.allowExport));
                }

            }

        }
        else {
            this.AccumulatedMessageVisibility = false;
            for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
                TempItemSource.push(new SupplierInvoiceItemLine(this.EntityPM.SupplierInvoiceItems[i], this, this.allowExport));
            }
        }
        if (this.EntityPM.IsAccumalated) {
            //    this.CurrentSession.AccumulatedFilterChangedEvent.emit({ filter: this.AccumulatedFilterSelectedValue, ParentCount: this.ParentItems.length, childrenCount: this.ChildrenItems.length });
        }
        if (this.ItemsSource != null) {
            this.ItemsSource.InsertCollection(TempItemSource);
        }
        this.CurrentSession.StopBusyIndicator();

        this.originalItemSource.InsertCollection(TempItemSource);
        if (!AppTool.IsNullOrEmpty(this.FromClassificationJumpToSII)) {
            this.SelectInvoiceItemMethod({ filter: this.FromClassificationJumpToSII });
        }


    }




    OpenExporterInvoiceWindow() {
        var windowArgs: any = {};
        windowArgs.SupplierInvoice = this.EntityPM;
        windowArgs.Declaration = this.declarationPM;

        windowArgs.Parent = this;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        var windowTitle = "נתונים נוספים ליצום - חטיבת חשבון יצוםן";

        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/ExporterInvoiceComponent');


    }

    originalItemSource: ObservableCollection = new ObservableCollection([]);
    Search(text: string) {
        var itemsSource: any = this.originalItemSource;
        var itemSourceByItemPrice: any = this.originalItemSource;
        if (AppTool.IsNullOrEmpty(text)) {
            this.BuildItemsList();
        }
        else {
            itemsSource = itemsSource.Collection.filter(f => f.ClassificationCode != null || f.ItemCode != null);
            itemSourceByItemPrice = itemSourceByItemPrice.Collection.filter(f => f.ItemPrice != null);
            var TempItemSource: SupplierInvoiceItemLine[] = [];

            TempItemSource = itemsSource.filter(f => (!AppTool.IsNullOrEmpty(f.ClassificationCode) ? f.ClassificationCode.toUpperCase().includes(text.toUpperCase()) : null) || (!AppTool.IsNullOrEmpty(f.ItemCode) ? f.ItemCode.toUpperCase().includes(text.toUpperCase()) : null));
            this.ItemsSource.InsertCollection(TempItemSource);
            if (itemSourceByItemPrice) {
                itemSourceByItemPrice = this.originalItemSource.Collection.filter(f => f.ItemPrice == (text));
                if (itemSourceByItemPrice.length > 0) {
                    if (this.ItemsSource.Length > 0) {
                        for (let item of itemSourceByItemPrice) {
                            var exist = TempItemSource.filter(d => d.LineNumber == item.LineNumber)[0];
                            if (!exist) {
                                TempItemSource.push(item);
                            }

                        } this.ItemsSource.InsertCollection(TempItemSource);
                    }
                    else {
                        this.ItemsSource.InsertCollection(itemSourceByItemPrice);
                    }
                }
            }


        }
        if (!AppTool.IsNullOrEmpty(text)) {
            this.SearchFilterChangedEvent = this.CurrentSession.SearchFilterChangedEvent.emit({ count: this.ItemsSource.Length });
        }
        else {
            this.SearchFilterChangedEvent = this.CurrentSession.SearchFilterChangedEvent.emit({ count: null });
        }
    }


    AccumulatedFilter: string;
    public AccumulatedFilterSelectedValue: string = 'Accumulated';
    AccumulatedFilterItemClicked(itemValue: string) {
        if (this.AccumulatedFilterChangedEvent) {
            this.AccumulatedFilterChangedEvent.unsubscribe();
            this.AccumulatedFilterChangedEvent = null;
        }
        if (this.AccumulatedFilterSelectedValue != itemValue) {
            this.AccumulatedFilterSelectedValue = itemValue;
            if (this.AccumulatedFilterSelectedValue == 'Accumulated') {
                this.AccumulatedFilter = "parent";
                this.AccumulatedMessageText = "חשבון צבור - פרטי מכס ניתנים לעריכה רק במצב לם צבור";

                this.IsActionButtonsEnabled = false;
            }
            else if (this.AccumulatedFilterSelectedValue == 'NotAccumulated') {
                this.AccumulatedFilter = "child";
                this.AccumulatedMessageText = "חשבון צבור";
                if (!this.IsDisplayOnly) {
                    this.IsActionButtonsEnabled = true;
                }
            }
            if (!this.IsFromCustomsAnswer || this.IsInvoiceAnswer) {
                this.CurrentSession.StartBusyIndicator("");

                this.Parent.EntityPM = this.EntityPM;
                if (this.Parent.EntityPM.IsDirty) {
                    this.Parent.SaveChangesSync();
                }
                this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey, 0, 500, this.AccumulatedFilter).subscribe((response: any) => {
                    var entityPM: SupplierInvoicePM = this.EntityPM;
                    this.EntityPM = response.Result;
                    this.BuildItemsList();
                    this.Parent.EntityPM.FullChildrenCount = this.EntityPM.FullChildrenCount;
                    this.Parent.EntityPM.FullParentsCount = this.EntityPM.FullParentsCount;

                    this.AccumulatedFilterChangedEvent = this.CurrentSession.AccumulatedFilterChangedEvent.emit({ filter: this.AccumulatedFilterSelectedValue, ParentCount: this.ParentItems.length, childrenCount: this.ChildrenItems.length, entityPM: entityPM });
                    this.Parent.EntityPM = this.EntityPM;

                    this.CurrentSession.StopBusyIndicator();

                });
            }
            else {

                this.BuildItemsList();
            }

        }
    }

    calculateTotals(deleteItem: boolean, deletedItemPrice: number) {
        if (deleteItem) {
            if (deletedItemPrice == null) deletedItemPrice = 0;
            if (this.Parent.TotalForeignCurrency == null) this.Parent.TotalForeignCurrency = 0;

            this.Parent.TotalForeignCurrency = this.Parent.TotalForeignCurrency - deletedItemPrice;

            this.declarationPM.Direction != "E" ? this.Parent.Difference = this.Parent.TotalForeignCurrency - (this.InvoiceAmount) : this.GetDifferenceAndTotalForeignCurrency();


        }
    }
    IsAccumulated: boolean;
    IsFromCustomsAnswer: boolean;
    IsInvoiceAnswer: boolean;
    private focusTimerToken: any;
    public AccumulatedFilterChangedEvent: any;
    public SearchFilterChangedEvent: any;

    OrangeVisibility: boolean;
    GreenVisibility: boolean;
    RedVisibility: boolean;

    opacity: number = 1;
    FromClassificationJumpToSII;
    DocumentFilingId: string;
    InitTab(entityPM: SupplierInvoicePM, parent: AddEditSupplierInvoiceComponent, isDisplayOnly: boolean, getFreightTotals: boolean = true, IsNewEntity, IsFromCustomsAnswer, IsInvoiceAnswer, documentFilingId) {

        this.InvoiceTypeFocus = false;
        this.focusTimerToken = setTimeout(() => {
            this.InvoiceTypeFocus = true;
        }, 1);

        this.EntityPM = entityPM;
        this.Parent = parent;
        this.IsDisplayOnly = isDisplayOnly;
        this.IsReadOnly = isDisplayOnly;
        this.IsActionButtonsEnabled = !isDisplayOnly;
        this.DocumentFilingId = documentFilingId;
        this.Pointers = this.Parent.pointers;
        this.declarationPM = parent.declarationPM;
        this.IsFromCustomsAnswer = IsFromCustomsAnswer;
        this.IsInvoiceAnswer = IsInvoiceAnswer;
        this.oldIncoterm = this.EntityPM.IncotermCode;
        //this.InvoiceNumber = entityPM.InvoiceNumber;
        this.IsChecked = false;
        if (this.declarationPM.Direction == "E" && FeatureLocator.HasFeaturePermession("Customs.Declaration", "EXPORTDECLARATIONPSCREEN")) {
            this.allowExport = true;
            this.FillGridData();
        }

        this.CheckRequrierdFieldsForSend();


        this.isNewEntity = IsNewEntity;
        if (isDisplayOnly) {
            this.opacity = 0.5;
        }
        if (!this.IsFromCustomsAnswer || this.IsInvoiceAnswer) {
            this.ParentsCount = "(" + this.EntityPM.FullParentsCount + ")";
            this.ChildrenCount = "(" + this.EntityPM.FullChildrenCount + ")";
        }
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForDate(this.declarationPM.TaxationDateTime).subscribe((responseRate: any) => {

            if (responseRate) {
                if (!responseRate.HasError) {
                    this.ExchangeRates = responseRate.Result;
                }
            }
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.CustomsPartnersItem").subscribe((response: any) => {

                    if (this.EntityPM.IsAccumalated) {
                        this.IsAccumulated = true;
                        this.AccumulatedMessageText = "חשבון צבור - פרטי מכס ניתנים לעריכה רק במצב לם צבור";
                        if (this.AccumulatedFilterSelectedValue == 'Accumulated') {
                            this.IsActionButtonsEnabled = false;

                        }
                        else if (!this.IsDisplayOnly) {
                            this.IsActionButtonsEnabled = true;;
                        }
                    }
                    else {
                        this.IsAccumulated = false;
                        this.AccumulatedMessageText = null;

                    }


                    this.BuildFreightAmountsList();
                    this.BuildItemsList();
                    //  this.CurrentSession.AccumulatedFilterChangedEvent.emit({ filter: this.AccumulatedFilterSelectedValue, ParentCount: this.ParentItems.length, childrenCount: this.ChildrenItems.length });

                    if (getFreightTotals) {
                        this.declarationPM.Direction != 'E' ? this.GetFreightTotals() : this.GetDifferenceAndTotalForeignCurrency();
                    }
                    //if (entityPM.InsruancePercentage) {
                    //    this.CalculateInsuranceAmount(entityPM.InsruancePercentage);

                    //}

                    if (this.InsurancePercentage == null) {

                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);


                        if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        }
                        this.GetInsurancePercentDefault();
                    }
                    else {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

                    }

                    if (this.EntityPM.InsruancePercentage) {
                        this.CalculateInsuranceAmount(this.EntityPM.InsruancePercentage);
                    }

                    if (entityPM.SequenceNumeric != null && entityPM.SequenceNumeric != 1) {

                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                    else {

                    }
                    this.IncotermLogic(this.EntityPM.IncotermCode);
                    this.SetScreenFieldsEditability();
                    //this.cd.detectChanges();

                    if (!AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
                        this.customsVendorPMService.get(this.EntityPM.VendorId).subscribe((myResponse: ServiceResponse) => {
                            this.vendor = myResponse.Result;
                            if (this.vendor != null) {
                                this.vendorNumber = this.vendor.VendorNumber;
                            }
                        });
                    }
                    if (this.declarationPM.Direction == "E")
                        this.GetDifferenceAndTotalForeignCurrency()

                    this.SetDepositionStatus();
                    //this.GetExchagneRates();


                });
            });
        });

        //FRITZ
        if (this.IFritz_feature) {
            if (this.declarationPM.PrimaryInvoiceCounterKey && this.EntityPM.InvoiceCounterKey) {
                if (this.EntityPM.InvoiceCounterKey.toString() == this.declarationPM.PrimaryInvoiceCounterKey) {
                    //yes, master invoice
                    this.isMasterInvoic = true;
                    this.IsValueForCustomsOnlyVisible = true;
                } else {
                    this.isMasterInvoic = false;
                    this.IsValueForCustomsOnlyVisible = false;
                }
            } else {
                // if it is the first created invoice, then its master 
                if (this.Parent.SaveAndNew) { // comes after click inittab
                    this.isMasterInvoic = false;
                    this.IsValueForCustomsOnlyVisible = false;
                } else if (this.declarationPM.SupplierInvoices.length == 0) {
                    this.isMasterInvoic = true;
                    this.IsValueForCustomsOnlyVisible = true;
                } else {
                    this.isMasterInvoic = false;
                    this.IsValueForCustomsOnlyVisible = false;
                }
            }

        }
        else {
            this.IsValueForCustomsOnlyVisible = false;
        }

        //after save n new
        if (this.Parent.SaveAndNew) {
            //open frieght
            this.AddAmountEnabled = true;
            this.FreightAmountGridEnabled = true;
            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);

            //resset is preference chck
            this.IsPreference = false;
        }

        //if (this.Parent.IsNewEntity) {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
            .subscribe((customsSettingList: ServiceResponse) => {
                if (customsSettingList) {
                    this.CurrentSession.StopBusyIndicator();
                    if (this.Parent.IsNewEntity && this.declarationPM.Direction != "E") {
                        let autoFillAccountType = customsSettingList.Result ? customsSettingList.Result.AutoFillAccountType : false;
                        if (autoFillAccountType) {
                            this.AccountTypeCode = "380";
                        }
                    }
                    let autoUnitMeasurement = customsSettingList.Result ? customsSettingList.Result.AutoUnitMeasurement : false;
                    if (autoUnitMeasurement) {
                        this.IsChecked = true;
                    }
                }
            });
        //}

        //this.GetCountryPURForItems();


    }

    //GetExchagneRates() {

    //    this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForDate(this.declarationPM.TaxationDateTime).subscribe((response:any) => {

    //        if (response) {
    //            if (!response.HasError) {
    //                this.ExchangeRates = response.Result;
    //            }
    //        }
    //            });
    //}

    FillGridData() {
        var paymentCounter = 0;
        if (this.EntityPM.SupplierInvoiceModifications.length > 0) {
            paymentCounter = this.getMax(this.EntityPM.SupplierInvoiceModifications, "SequenceNumeric");
        }

        let Insurance67: any = this.FindModificationByCode("67");
        let Freight104: any = this.FindModificationByCode("104");
        let ExtraPayments160: any = this.FindModificationByCode("160");

        this.AdjustmentsList = new ObservableCollection([]);
        if (Insurance67 == null) {
            paymentCounter += 1;
            this.AddModification("67", paymentCounter, "ביטוח");
        } else {
            Insurance67.TypeName = "ביטוח"
            this.AdjustmentsList.Insert(new ModificationItemModel(Insurance67, this, "67"));
        }

        if (Freight104 == null) {
            paymentCounter += 1;
            this.AddModification("104", paymentCounter, "הובלה בפועל");
        } else {
            Freight104.TypeName = "הובלה בפועל"
            this.AdjustmentsList.Insert(new ModificationItemModel(Freight104, this, "104"));
        }

        if (ExtraPayments160 == null) {
            paymentCounter += 1;
            this.AddModification("160", paymentCounter, "הוצםות נוספות");
        } else {
            ExtraPayments160.TypeName = "הוצםות נוספות"
            this.AdjustmentsList.Insert(new ModificationItemModel(ExtraPayments160, this, "160"));
        }


        this.ExportModificationCurrency = this.EntityPM.InvoiceCurrencyTypeCode;
        this.EntityPM.IsDirty = false;
    }

    GetInsuranceAsModificationItemModel() {
        /*var item = new SupplierInvoiceModificationPM(this.EntityPM);
        item.TypeName = "ביטוח";
        item.Amount = this.EntityPM.InsuranceAmount;
        item.CurrencyTypeCode = this.EntityPM.InsruanceCurrencyTypeCode;
        item.CurrencyTypeName = this.EntityPM.InsruanceCurrencyTypeCodeName;
        item.IsDirty = false;
        return item;*/
    }

    GetFreightAsModificationItemModel() {
        var item = new SupplierInvoiceModificationPM(this.EntityPM);
        if (this.EntityPM.SupplierInvoiceFreightAmounts.length != 0) {
            if (this.EntityPM.SupplierInvoiceFreightAmounts[0].Amount > 0) {
                item.Amount = this.EntityPM.SupplierInvoiceFreightAmounts[0].Amount;
            }
            item.CurrencyTypeCode = this.EntityPM.SupplierInvoiceFreightAmounts[0].CurrencyTypeCode;
            item.CurrencyTypeName = this.EntityPM.SupplierInvoiceFreightAmounts[0].CurrencyTypeName;
        }
        item.IsDirty = false;
        item.TypeName = "הובלה";
        return item;
    }

    FindModificationByCode(code: string) {
        for (let item of this.EntityPM.SupplierInvoiceModifications) {
            if (item.TypeCode == code) {
                return item;
            }
        }
        return null;
    }

    getMax(list: any[], propertyName: string) {
        var max = -99999;
        var maxObj = list && list.length > 0 ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current }) : null;
        if (maxObj != null)
            if (max <= maxObj[propertyName])
                max = maxObj[propertyName];
        return max;
    }

    AddModification(code: string, modificationCounter: number, typeName: string) {
        var item = new SupplierInvoiceModificationPM(this.EntityPM);
        item.TypeName = typeName;
        item.TypeCode = code;
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
        item.Tenant = SessionLocator.Tenant;
        item.ModificationCounterKey = modificationCounter;
        item.IsDirty = false;
        this.EntityPM.AddSupplierInvoiceModification(item);
        this.AdjustmentsList.Insert(new ModificationItemModel(item, this, code));
    }

    SetScreenFieldsEditability() {



        this.UIProperties.SetEnabled("AccountTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ActualPayedAmount", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SequenceNumeric", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IssueDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IssueCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InvoiceCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PreferenceDocumentTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsPreference", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PaymentTermsCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PaymentTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ActualPayedCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AccumalationStateCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("FreightAmountGridEnabled", this.ObjectTableName, !this.IsDisplayOnly);
        //this.UIProperties.SetEnabled("AddAmountEnabled", this.ObjectTableName, !this.IsDisplayOnly);
        //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IncotermCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InvoiceAmount", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TotalFreightAmountInInvoiceCurrency", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TotalFreightInFreightCurrency", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InsurancePercentage", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TotalFreightAmountInNIS", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PartyRelationshipCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("BuyerName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("BuyerCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("BuyerAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DutyRegimeProtocolCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("BuyerRoleCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportModificationCurrency", this.ObjectTableName, !this.IsDisplayOnly);


        if (this.IsDisplayOnly) {
            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
            this.UIProperties.SetEnabled("InsuranceAmount", this.ObjectTableName, !this.IsDisplayOnly);
            this.UIProperties.SetEnabled("InsruancePercentage", this.ObjectTableName, !this.IsDisplayOnly);
            this.AddAmountEnabled = !this.IsDisplayOnly;
            this.FreightAmountGridEnabled = !this.IsDisplayOnly;
        }

    }
    importer: ImporterDespositionClass;
    tootltip: string;
    SetDepositionStatus() {
        this.supplierInvoiceExtendedPMService.GetImporterDespositionStatus(this.VendorId, this.declarationPM.ImporterId).subscribe((response: any) => {
            if (response) {
                if (response.Result) {

                    this.importer = response.Result;
                    this.tootltip = new DateTimeToDatePipe().transform(this.importer.EndDate) + "," + this.importer.ImporterDespositionNumber;
                    if (this.importer.Status == "GreenTick") {
                        this.GreenVisibility = true;

                        this.RedVisibility = false;
                        this.OrangeVisibility = false;
                    }
                    else if (this.importer.Status == "RedX") {
                        this.RedVisibility = true;
                        this.GreenVisibility = false;

                        this.OrangeVisibility = false;
                    }

                    else if (this.importer.Status == "OrangeTick") {
                        this.OrangeVisibility = true;
                        this.GreenVisibility = false;
                        this.RedVisibility = false;

                    }
                    else {
                        this.GreenVisibility = false;
                        this.RedVisibility = false;
                        this.OrangeVisibility = false;
                    }
                }


            }
        });

    }

    CheckRequrierdFieldsForSend() {
        var isExport = false;
        if (this.allowExport) {
            isExport = true;
        }

        var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.SupplierInvoice')[0];
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        var customsRequiredFieldExtendedListService: CustomsRequiredFieldExtendedListService = new CustomsRequiredFieldExtendedListService();
        filters = customsRequiredFieldExtendedListService.GetFilter(filters, isExport)

        customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
            var requiredFields = response.Result;
            requiredFields.forEach((field) => {
                var objectField = window.ObjectFields.filter(d => d.FieldCode == field.ObjectfieldCode)[0];
                this.UIProperties.SetWarning(objectField.FieldName, 'Customs.SupplierInvoice', true);
            });
        });
    }

    //#region Properties

    private selectedFilter: string;
    get SelectedFilter() { return this.selectedFilter; }
    set SelectedFilter(value: string) {
        if (this.selectedFilter != value) {
            this.selectedFilter = value;

        }
    }
    public FiltersList: string[] = ["Copy Now"];
    public get AccountTypeCode() { return this.EntityPM.AccountTypeCode; }
    public set AccountTypeCode(newValue: string) {
        this.EntityPM.AccountTypeCode = newValue;
    }

    public get InvoiceNumber() { return this.EntityPM.InvoiceNumber; }
    public set InvoiceNumber(newValue: string) { this.EntityPM.InvoiceNumber = newValue; }

    public get ActualPayedAmount() { return this.EntityPM.ActualPayedAmount; }
    public set ActualPayedAmount(newValue: number) { this.EntityPM.ActualPayedAmount = newValue; }

    public get SequenceNumeric() { return this.EntityPM.SequenceNumeric; }
    public set SequenceNumeric(newValue: number) { this.EntityPM.SequenceNumeric = newValue; }

    public get IssueDate() { return this.EntityPM.IssueDate; }
    public set IssueDate(newValue: Date) { this.EntityPM.IssueDate = newValue; }

    public get ChangeInSupplierInvoice() { return this.EntityPM.ChangeInSupplierInvoice; }
    public set ChangeInSupplierInvoice(newValue: string) { this.EntityPM.ChangeInSupplierInvoice = newValue; }


    public get BuyerName() { return this.EntityPM ? this.EntityPM.BuyerName : null; }
    public set BuyerName(newValue: string) { this.EntityPM.BuyerName = newValue; }

    public get BuyerAddress() { return this.EntityPM ? this.EntityPM.BuyerAddress : null; }
    public set BuyerAddress(newValue: string) {
        this.EntityPM.BuyerAddress = newValue;
    }

    public get BuyerCountryCode() { return this.EntityPM ? this.EntityPM.BuyerCountryCode : null; }
    public set BuyerCountryCode(newValue: string) {

        this.EntityPM.BuyerCountryCode = newValue;
    }

    public get DutyRegimeProtocolCode() { return this.EntityPM ? this.EntityPM.DutyRegimeProtocolCode : null; }
    public set DutyRegimeProtocolCode(newValue: string) {

        this.EntityPM.DutyRegimeProtocolCode = newValue;
    }

    public get BuyerRoleCode() { return this.EntityPM ? this.EntityPM.BuyerRoleCode : null; }
    public set BuyerRoleCode(newValue: string) {

        this.EntityPM.BuyerRoleCode = newValue;
    }

    public get PartyRelationshipCode() { return this.EntityPM ? this.EntityPM.PartyRelationshipCode : null; }
    public set PartyRelationshipCode(newValue: string) {
        //if (AppTool.IsNullOrEmpty(newValue))
        //    this.UIProperties.SetRequired("PartyRelationshipCode", this.ObjectTableName, true);
        //else
        //    this.UIProperties.SetRequired("PartyRelationshipCode", this.ObjectTableName, false);

        this.EntityPM.PartyRelationshipCode = newValue;
    }

    public get VendorId() { return this.EntityPM.VendorId; }
    public set VendorId(newValue: string) {

        if (this.EntityPM.VendorId != newValue) {
            //this.Parent.calculateCommission = true; //old code
            //this.Parent.CalculateCommissionPercentage();
        }
        this.EntityPM.VendorId = newValue;
        this.customsVendorPMService.get(newValue).subscribe((myResponse: ServiceResponse) => {

            this.vendor = myResponse.Result;
            if (this.vendor != null) {
                this.EntityPM.IssueCountryCode = this.vendor.CountryCode;
                this.EntityPM.IssueCountryName = "טםיווםן";
                this.vendorNumber = this.vendor.VendorNumber;
                this.SetDepositionStatus();
            }

            else {
                this.GreenVisibility = false;
                this.OrangeVisibility = false;
                this.RedVisibility = false;
            }
        });
    }


    public get AccumalationStateCode() { return this.EntityPM.AccumalationStateCode; }
    public set AccumalationStateCode(newValue: string) {
        this.EntityPM.AccumalationStateCode = newValue;
    }

    public get IssueCountryCode() { return this.EntityPM.IssueCountryCode; }
    public set IssueCountryCode(newValue: string) { this.EntityPM.IssueCountryCode = newValue; }


    invoiceCurrency: any;
    public get InvoiceCurrency() { return this.invoiceCurrency; }
    public set InvoiceCurrency(newValue: any) { this.invoiceCurrency = newValue; }

    InvoiceCurrencyChanged(currency) {
        this.InvoiceCurrency = currency;
        if (this.InvoiceCurrency) {
            this.Parent.invoiceCurrencyName = this.InvoiceCurrency.LocalName;
            if (this.declarationPM.Direction == 'E') {
                this.GetDifferenceAndTotalForeignCurrency();
            }

        }

        //this.Parent.CalculateCommissionPercentage();
    }

    public get InvoiceCurrencyTypeCode() { return this.EntityPM.InvoiceCurrencyTypeCode; }
    public set InvoiceCurrencyTypeCode(newValue: string) {
        if (this.EntityPM.InvoiceCurrencyTypeCode != newValue) {
            //this.Parent.calculateCommission = true; //old code

        }

        this.EntityPM.InvoiceCurrencyTypeCode = newValue;


        var percentage;

        //if (this.declarationPM.SupplierInvoices.length > 0) {
        //    percentage = this.declarationPM.SupplierInvoices[0].InsruancePercentage;
        //}
        //else {
        //    percentage = this.InsurancePercentage;
        //}

        var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
        percentage = firstInvoice.InsruancePercentage;
        if (percentage != null) {

            this.CalculateInsuranceAmount(percentage);
        }
    }


    public get PreferenceDocumentTypeCode() { return this.EntityPM.PreferenceDocumentTypeCode; }
    public set PreferenceDocumentTypeCode(newValue: string) { this.EntityPM.PreferenceDocumentTypeCode = newValue; }


    public get IsPreference() { return this.EntityPM.IsPreference; }
    public set IsPreference(newValue: boolean) { this.EntityPM.IsPreference = newValue; }


    public get PaymentTermsCode() { return this.EntityPM.PaymentTermsCode; }
    public set PaymentTermsCode(newValue: string) { this.EntityPM.PaymentTermsCode = newValue; }

    public get PaymentTypeCode() { return this.EntityPM.PaymentTypeCode; }
    public set PaymentTypeCode(newValue: string) { this.EntityPM.PaymentTypeCode = newValue; }

    public get ActualPayedCurrencyTypeCode() { return this.EntityPM.ActualPayedCurrencyTypeCode; }
    public set ActualPayedCurrencyTypeCode(newValue: string) { this.EntityPM.ActualPayedCurrencyTypeCode = newValue; }

    public get InsruanceCurrencyTypeCode() { return this.EntityPM.InsruanceCurrencyTypeCode; }
    public set InsruanceCurrencyTypeCode(newValue: string) { this.EntityPM.InsruanceCurrencyTypeCode = newValue; }

    public get InsuranceAmount() { return this.EntityPM.InsuranceAmount; }
    public set InsuranceAmount(newValue: number) { this.EntityPM.InsuranceAmount = newValue; }

    private insuranceAmountEnabled: boolean;
    public get InsuranceAmountEnabled() { return this.insuranceAmountEnabled; }
    public set InsuranceAmountEnabled(newValue: boolean) { this.insuranceAmountEnabled = newValue; }

    private insuranceCurrencyEnabled: boolean;
    public get InsuranceCurrencyEnabled() { return this.insuranceCurrencyEnabled; }
    public set InsuranceCurrencyEnabled(newValue: boolean) { this.insuranceCurrencyEnabled = newValue; }

    private insurancePercentageEnabled: boolean;
    public get InsurancePercentageEnabled() { return this.insurancePercentageEnabled; }
    public set InsurancePercentageEnabled(newValue: boolean) { this.insurancePercentageEnabled = newValue; }

    public get InsruancePercentage() { return this.EntityPM.InsruancePercentage; }
    public set InsruancePercentage(newValue: number) {
        this.EntityPM.InsruancePercentage = newValue;
        this.IncotermLogic(this.IncotermCode);// this code replaced all the comments below.
        //if (!newValue) {
        //    this.InsuranceAmount = null;
        //    if (this.IncotermCode) {
        //        if (!(this.IncotermCode.startsWith("D") || this.IncotermCode == "CIF" || this.IncotermCode == "CIP")) {
        //            this.InsuranceAmount = null;

        //            this.InsurancePercentage = null;
        //            this.InsuranceAmount = null;
        //            this.InsruanceCurrencyTypeCode = null;


        //            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
        //            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);

        //        }
        //    }
        //}

        //else {
        //    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
        //    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

        //}

        if (newValue) {
            this.CalculateInsuranceAmount(newValue);
        }
    }




    //private isFreightCurrencyTypeCodeEnabled: boolean;
    //public get IsFreightCurrencyTypeCodeEnabled() { return this.isFreightCurrencyTypeCodeEnabled; }
    //public set IsFreightCurrencyTypeCodeEnabled(newValue: boolean) { this.isFreightCurrencyTypeCodeEnabled = newValue; }



    private freightAmountGridEnabled: boolean = true;
    public get FreightAmountGridEnabled() { return this.freightAmountGridEnabled; }
    public set FreightAmountGridEnabled(newValue: boolean) {
        if (this.IsDisplayOnly) {
            this.freightAmountGridEnabled = false;
        }
        else {
            this.freightAmountGridEnabled = newValue;
        }
    }


    private addAmountEnabled: boolean = true;
    public get AddAmountEnabled() { return this.addAmountEnabled; }
    public set AddAmountEnabled(newValue: boolean) {
        if (this.IsDisplayOnly) {
            this.addAmountEnabled = false;
        }
        else {
            this.addAmountEnabled = newValue;
        }
    }

    public get FreightCurrencyTypeCode() { return this.EntityPM.FreightCurrencyTypeCode; }
    public set FreightCurrencyTypeCode(newValue: string) {
        this.EntityPM.FreightCurrencyTypeCode = newValue;

        this.LoadCurrenciesExchangeRates(true);

    }

    incotermChanged: boolean = false;
    allowToDelete: boolean = true;
    oldIncoterm: string;
    public get IncotermCode() { return this.EntityPM.IncotermCode; }
    public set IncotermCode(newValue: string) {
        if (this.EntityPM.IncotermCode != null) {
            this.oldIncoterm = this.EntityPM.IncotermCode;
        }

        this.EntityPM.IncotermCode = newValue;
        this.incotermChanged = true;
        if (this.allowToDelete) {
            this.IncotermLogic(this.EntityPM.IncotermCode);
        }

        this.incotermChanged = false;
        this.allowToDelete = true;

        this.GetInsurancePercentDefault();

        this.setAdjustmentsWarning(newValue);
    }

    private async setAdjustmentsWarning(incotermCode: string) {
        /*if (incotermCode) {
            const filters: ApiQueryFilters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 50;
            filters.addAdditionalFilter("ENGLISHNAME", incotermCode, null, null, "Contains", false, false, false, "Text", false, false);
            filters.addAdditionalFilter("LeadDocumentTypeID", '2', null, null, "Contains", false, false, false, "Text", false, false);
            const incotemrsFileValidationList: IncotemrsFileValidationList[] = await this.logtuideTableDataService.getDataFromService(this.incotemrsFileValidationListService.getByFilters(filters))

            this.isInsurance = incotemrsFileValidationList.some(x => x.IsInsurance)
            this.isFreightCharge = incotemrsFileValidationList.some(x => x.IsFreightCharge)
        } else
            this.isInsurance = this.isFreightCharge = false;*/
    }

    private GetInsurancePercentDefault() {

        if (this.declarationPM.IsReleaseFile) return;
        if (this.IsDisplayOnly) return;
        //let goToInsuranceInUNF = false;

        //if (this.declarationPM.IsConnectedToUnifreight) {
        //    if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && !AppTool.IsNullOrEmpty(this.declarationPM.CustomFileNo)) {
        //        goToInsuranceInUNF = true;
        //    }
        //}
        //}

        //if (!goToInsuranceInUNF || (this.EntityPM.InvoiceCounterKey.toString() != this.declarationPM.PrimaryInvoiceCounterKey && this.declarationPM.SupplierInvoices.length > 0) ||
        //    this.InsruancePercentage != null || this.InsuranceAmount != null || (!this.IncotermCode.startsWith("E") && !this.IncotermCode.startsWith("F"))) {
        //    return;
        //}
        //if (_OpInsurancePercent != null) {
        //    if (!_OpInsurancePercent.IsComplete) {
        //        _OpInsurancePercent.Cancel();
        //    }
        //}


        /////////////////////////
        //In case(IsconnectedToUnifreight = True) & (ICIM_INSUR_PERCncotermCode statrs with “E” or “F”) & (InsruancePercent + InsruanceAmount = Null)  - WI 26677
        //Check customer default “CIM_INSUR_PERC” , if has data , fill that value in InsruancePercent & calc the InsuranceValue
        //This process will be done only on first supplier invoice 
        /////////////////////////
        var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
        if (!this.IsFirstInvoice()) {
            return;
        }

        if (!this.declarationPM.IsConnectedToUnifreight) {
            return;
        }
        let inco: string = this.IncotermCode || "";
        if (inco.startsWith("E") || inco.startsWith("F") || inco.startsWith("CPT") || inco.startsWith("CFR")) {
        } else {
            return;
        }
        if (AppTool.IsNullOrZero(this.InsruancePercentage) && AppTool.IsNullOrZero(this.InsuranceAmount)) {
            //only if user not insert 
        } else {
            return;
        }
        if (this.declarationPM.SupplierInvoices.length > 0) {
            if (this.declarationPM.SupplierInvoices[0].SequenceNumeric == this.EntityPM.SequenceNumeric) {
                ///first
            } else {
                return;
            }
        } else {
            ///first 1
        }


        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetInsurancePercentDefault(this.declarationPM.CustomerCode, this.declarationPM.Tenant)
            .subscribe((res: any) => {
                this._OpInsurancePercent_Completed(res.Result);// += _OpInsurancePercent_Completed;
            });
    }

    _OpInsurancePercent_Completed(res): void {
        //if (!_OpInsurancePercent.IsCanceled) {
        //    if (!_OpInsurancePercent.HasError) {
        //        _OpInsurancePercent.Completed -= _OpInsurancePercent_Completed;
        //        if (_OpInsurancePercent.Value != null) {
        let stringInsurancePercentage: string = //(string)_OpInsurancePercent.Value;
            res.insurancePercent;
        let decimalInsurancePercentage: number;
        //decimal.TryParse(stringInsurancePercentage, out decimalInsurancePercentage);
        var insurancePerc: number = Number(stringInsurancePercentage);
        if (insurancePerc > 0) {
            this.InsruancePercentage = Number(stringInsurancePercentage);  //decimal.TryParse(stringInsurancePercentage, out decimalInsurancePercentage) ? decimalInsurancePercentage : (decimal ?)null;
        }
        //FirePropertyChanged("InsurancePercentage");
        //        }
        //    }
        //}
    }


    //private GetCountryPURForItems() {
    //    this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
    //    var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    //    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", this.declarationPM.Tenant)
    //        .subscribe((response:any) => {
    //            this.CurrentSession.StopBusyIndicator();
    //            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
    //                this.IsCountryPURForItems = true;
    //            }
    //        });
    //}


    public get InvoiceAmount() { return this.EntityPM.InvoiceAmount; }
    public set InvoiceAmount(newValue: number) {

        if (this.EntityPM.InvoiceAmount != newValue) {
            //this.Parent.calculateCommission = true; //old
        }
        this.EntityPM.InvoiceAmount = newValue;
        if (this.Parent.TotalForeignCurrency == null) this.Parent.TotalForeignCurrency = 0;
        if (this.declarationPM.Direction != "E")
            this.Parent.Difference = this.Parent.TotalForeignCurrency - (newValue);
        else
            this.GetDifferenceAndTotalForeignCurrency()
        var percentage;
        //if (this.declarationPM.SupplierInvoices.length > 0) {
        //    percentage = this.declarationPM.SupplierInvoices[0].InsruancePercentage;
        //}
        //else {
        //    percentage = this.InsurancePercentage;
        //}
        var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
        percentage = firstInvoice.InsruancePercentage;
        if (percentage != null) {

            this.CalculateInsuranceAmount(percentage);
        }

    }

    private totalFreightAmountInInvoiceCurrency: number = 0;
    public get TotalFreightAmountInInvoiceCurrency() { return this.totalFreightAmountInInvoiceCurrency; }
    public set TotalFreightAmountInInvoiceCurrency(newValue: number) { this.totalFreightAmountInInvoiceCurrency = newValue; }

    private totalFreightInFreightCurrency: number;
    public get TotalFreightInFreightCurrency() { return this.EntityPM.TotalFreightInFreightCurrency; }
    public set TotalFreightInFreightCurrency(newValue: number) { this.EntityPM.TotalFreightInFreightCurrency = newValue; }

    totalExportModificationInInvoiceCurrency: number;
    public get TotalExportModificationInInvoiceCurrency() { return this.totalExportModificationInInvoiceCurrency; }
    public set TotalExportModificationInInvoiceCurrency(newValue: number) { this.totalExportModificationInInvoiceCurrency = newValue; }

    exportModificationCurrency: string;
    public get ExportModificationCurrency() { return this.exportModificationCurrency; }
    public set ExportModificationCurrency(newValue: string) {
        this.exportModificationCurrency = newValue;
        this.CalculateExportModificationAmount();
    }

    public get InsurancePercentage() { return this.EntityPM.InsruancePercentage; }
    public set InsurancePercentage(newValue: number) {
        this.EntityPM.InsruancePercentage = newValue;
        if (newValue) {
            this.CalculateInsuranceAmount(newValue);
        }
        else {
            this.InsuranceAmount = null;
        }


    }

    public ReCalculateInsuranceAmount() { //call from     AddEditSupplierInvoiceComponent.OkButtonClicked()

        if (this.EntityPM.InsruancePercentage) {
            this.CalculateInsuranceAmount(this.EntityPM.InsruancePercentage);
        }
    }
    private totalFreightAmountInNIS: number;
    public get TotalFreightAmountInNIS() { return this.EntityPM.TotalFreightInNIS; }
    public set TotalFreightAmountInNIS(newValue: number) { this.EntityPM.TotalFreightInNIS = newValue; }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(newValue: boolean) { this.isChecked = newValue; }

    public get IsValueForCustomsOnly() { return this.EntityPM.IsValueForCustomsOnly; }
    public set IsValueForCustomsOnly(newValue: boolean) { this.EntityPM.IsValueForCustomsOnly = newValue; }

    //#endregion

    OnInvoiceNumberLostFocus(invoiceNumberTextBox: any) {

        if (this.declarationPM != null && this.declarationPM.SupplierInvoices.length >= 0) {
            if (this.EntityPM.InvoiceNumber) {

                //server method
                //var supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
                //supplierInvoiceService.GetCheckIfInvoiceNumberExists(this.EntityPM.DeclarationId, this.EntityPM.InvoiceNumber, this.EntityPM.InvoiceCounterKey).subscribe((resp: ServiceResponse) => {
                //    if (!resp.HasError) {
                //        if (resp.Result) {
                //            var newValue = this.InvoiceNumber;
                //            var confirm = new ConfirmWindow();
                //            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                //            confirm.ShowNoButton = true;
                //            confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + resp.Result.SequenceNumeric + "- הםם להמשיך ?");
                //            confirm.WindowClosed.subscribe((event: any) => {
                //                confirm.Close();
                //                this.InvoiceNumber = newValue;
                //                if (confirm.Yes) {
                //                    SessionLocator.SustainFocusOnCell = false;
                //                }
                //                else {
                //                    SessionLocator.SustainFocusOnCell = true;
                //                    console.log(invoiceNumberTextBox.InputId);
                //                    var element = document.getElementById(invoiceNumberTextBox.InputId);
                //                    if (element) {
                //                        element.focus();
                //                    }
                //                }
                //            });
                //        }
                //    }
                //});

                //client method
                var invoices: SupplierInvoicePM[] = [];
                invoices = this.declarationPM.SupplierInvoices;
                invoices = invoices.concat(this.Parent.NewInvoices);
                var exist = invoices.find(d => d.InvoiceNumber == this.EntityPM.InvoiceNumber);
                if (exist) {
                    //show confirm window
                    var newValue = this.InvoiceNumber;
                    var confirm = new ConfirmWindow();
                    confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                    confirm.ShowNoButton = true;
                    confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + exist.SequenceNumeric + "- הםם להמשיך ?");
                    confirm.WindowClosed.subscribe((event: any) => {
                        confirm.Close();
                        this.InvoiceNumber = newValue;
                        if (confirm.Yes) {
                            SessionLocator.SustainFocusOnCell = false;
                        }
                        else {
                            SessionLocator.SustainFocusOnCell = true;
                            console.log(invoiceNumberTextBox.InputId);
                            var element = document.getElementById(invoiceNumberTextBox.InputId);
                            if (element) {
                                element.focus();
                            }
                        }
                    });

                }

            }

        }
    }

    CopyAmountList() {
        this.FreightCopyList.length = this.AmountList.Length;
        this.FreightCopyList = this.AmountList.Collection.concat();

    }


    CopyNowClicked(direction: string) {
        if (direction == 'E') {
            this.UpdateClicked(UpdateOptions.QuantityType);
        }
        else
        {
            var confirm = new ConfirmWindow();

            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.UpdateAndOverride");
            confirm.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Update");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.UpdateOrOverride"));
            confirm.WindowClosed.subscribe((event: any) => {

                if (confirm.Yes) {
                    confirm.Close();
                    for (let item of this.ItemsSource.Collection) {

                        if (item.QunatityTypeCode != null) {
                            var s = item.QunatityTypeCode.slice(1, item.QunatityTypeCode.length - 1);
                            item.InvoiceQuantityType = s;
                        }
                    }

                }
                else {
                    confirm.Close();
                    for (let item of this.ItemsSource.Collection) {
                        if (item.InvoiceQuantityType == null) {

                            if (item.QunatityTypeCode != null) {
                                var s = item.QunatityTypeCode.slice(1, item.QunatityTypeCode.length - 1);
                                //var s = item.QunatityTypeCode.split('(');
                                //var st = s[1].split(')');

                                item.InvoiceQuantityType = s;
                            }

                        }
                    }


                }

            });

            if (this.ItemsSource.Collection.length == 500) {

                var msg = new MessageWindow();

                msg.Show(" עודכנו רק 500 הפריטים המוצגים");

            }
        }
    
    }

    UpdateGroupingAccountLines(){
        debugger
        let GroupedItemsSource = new ObservableCollection([]);
        let GroupedItems: SupplierInvoiceItemPM[] = []; 
        let sequenceNumeric = 0;
        
        this.ItemsSource.Collection.forEach(item => {
            if(GroupedItemsSource.Collection.filter(j=> j.ClassificationCode == item.entityPM.ClassificationCode).length == 0){
                let filterItems: SupplierInvoiceItemLine[] = this.ItemsSource.Collection.filter(i => i.ClassificationCode == item.ClassificationCode && item.OriginCountryCode == i.OriginCountryCode);
                if(filterItems && filterItems.length > 1){
                    let GroupedItem:SupplierInvoiceItemPM = item.entityPM
                    let invoiceQuantity = 0;
                    let itemPrice = 0;

                    filterItems.forEach(i => {
                        invoiceQuantity += AppTool.IsNullOrEmpty(i.entityPM.InvoiceQuantity) ? 0 : i.entityPM.InvoiceQuantity;
                        itemPrice += AppTool.IsNullOrEmpty(i.entityPM.ItemPrice)? 0 : i.entityPM.ItemPrice;
                    });

                    GroupedItem.InvoiceQuantity = invoiceQuantity;
                    GroupedItem.ItemPrice = itemPrice;
                    GroupedItem.ItemDescription = "";
                    GroupedItem.ItemCode = "";
                    GroupedItem.SequenceNumeric = sequenceNumeric += 1;
                    GroupedItemsSource.Insert(new SupplierInvoiceItemLine(GroupedItem, this, this.allowExport)); 
                    GroupedItems.push(GroupedItem);  
                }
                else{
                    item.SequenceNumeric = sequenceNumeric += 1;
                    GroupedItemsSource.Insert(item);  
                    GroupedItems.push(item.entityPM);   
                }
               
            }
        });
        
        this.ItemsSource = GroupedItemsSource;
        this.EntityPM.SupplierInvoiceItems = GroupedItems;
    }
  

    UpdateClicked(type:UpdateOptions){
        let selectedOptionsSettings=this.updateOptionsMap[type];
        let title = selectedOptionsSettings.Title;
        var args=selectedOptionsSettings.Arguments;
        this.UpdateSupplierInvoiceGeneralField(args, title);
    }

    UpdateSupplierInvoiceGeneralField(args:UpdateGeneralArgsParams, title){
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.ShowCloseButton = true;
        windowArgs.SupplierInvoicePM = this.EntityPM;
        windowArgs = Object.assign(windowArgs,args);
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = title;
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    args.SelectionCompletedMethod(comp);
                }
            });
        });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/UpdateSupplierInvoiceGeneralFieldComponent');
    }

    private _CustomsCountryListService: CustomsCountryListService = new CustomsCountryListService();
    SelectionOriginCompleted(args) {
        if (args.ItemsSource != null) {
            if (args.UpdateAll) {
                for (let item of this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent)) {
                    if (item[args.UpdateField] != args) {
                        this.updateProcess(item, args);

                    }
                }
            } else {
                if (args.UpdateItemsWithNoValue) {
                    for (let item of this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent)) {
                        if (item[args.UpdateField] == "" || item[args.UpdateField] == null) {
                            this.updateProcess(item, args);
                        }
                    }
                } else {
                    if (args.ItemsSource) {
                        for (let item of this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent)) {
                            var number = args.ItemsSource.Collection.filter(d => d.Number == item.SequenceNumeric)[0];                            
                            if (number) {
                                this.updateProcess(item, args);
                            }
                        }
                    }
                }
            }
        }
    }
    updateProcess(item:SupplierInvoiceItemPM, args) {
        var updateField=args.UpdateField;
        var value= args[args.UpdateField];
        switch (updateField) {
            case 'OriginCountryCode': {
                item.OriginCountryCode = args.FinalValue.Code;
                item.OriginCountryName = args.FinalValue.LocalName;
                break;
            }
            case 'ClassificationCode': {
                item.ClassificationCode = value;
                var lineItem = this.ItemsSource.Collection[item.SequenceNumeric - 1];
                if (lineItem) {
                    lineItem.GetQuantityType();
                }
                break;
            }
            case 'InvoiceQuantityType': {
                item.InvoiceQuantityType = args.FinalValue.Code;
                item.InvoiceQuantityTypeName = args.FinalValue.LocalName;
                break;
            }
            case 'DutyRegimeProtocolCode': {
                item.DutyRegimeProtocolCode = args.FinalValue.Code;
                item.DutyRegimeProtocolLocalName = args.FinalValue.LocalName;
                break;
            }
            case 'TradeAgreementCode': {
                item.TradeAgreementCode = args.FinalValue.Code;
                item.TradeAgreementName = args.FinalValue.LocalName;
                break;
            }
        }
    }

    SelectionCompleted(args) {

        if (args.UpdateAll) {
            for (let item of this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent)) {
                var exist = item.SupplierInvoiceItemProcesTypes.filter(d => d.ProcessTypeCode == args.ProcessTypeCode)[0];
                if (!exist) {
                    var processType: SupplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM(item);
                    processType.DeclarationId = this.declarationPM.Id;
                    processType.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
                    processType.InvoiceItemLineNumber = item.LineNumber;
                    processType.Tenant = this.EntityPM.Tenant;
                    processType.ProcessTypeCode = args.ProcessTypeCode;

                    this.itemGovernmentProcedureTypeListService.getSingleFromCache(args.ProcessTypeCode).subscribe((response: any) => {

                        var result: ItemGovernmentProcedureTypeList = response.Result;
                        processType.ProcessTypeName = result.LocalName;
                    });
                    item.ItemAdditionalStatus = true;
                    item.AddSupplierInvoiceItemProcesType(processType);



                }
            }
            for (let item of this.ItemsSource.Collection) {
                item.ItemAdditionalStatusVisibility = true;
            }
            if (this.EntityPM.IsAccumalated) {
                if (this.AccumulatedFilterSelectedValue != 'Accumulated') {
                    var msg = new MessageWindow();
                    msg.RTL = true;
                    var count = this.EntityPM.SupplierInvoiceItems.length;
                    msg.Show("קוד תהליך נשמר בהצלחה ב-" + count + " שורות ");
                }
            }
            else {
                var msg = new MessageWindow();
                msg.RTL = true;

                var count = this.EntityPM.SupplierInvoiceItems.length;
                msg.Show("קוד תהליך נשמר בהצלחה ב-" + count + " שורות ");
            }

        }
        else {
            if (args.ItemsSource) {
                var items = args.ItemsSource.Collection;
                for (let item of this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent)) {
                    var number = args.ItemsSource.Collection.filter(d => d.Number == item.SequenceNumeric)[0];
                    if (number) {
                        var exist = item.SupplierInvoiceItemProcesTypes.filter(d => d.ProcessTypeCode == args.ProcessTypeCode)[0];
                        if (!exist) {
                            var processType: SupplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM(item);
                            processType.DeclarationId = this.declarationPM.Id;
                            processType.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
                            processType.InvoiceItemLineNumber = item.LineNumber;
                            processType.Tenant = this.EntityPM.Tenant;
                            processType.ProcessTypeCode = args.ProcessTypeCode;
                            this.itemGovernmentProcedureTypeListService.getSingleFromCache(args.ProcessTypeCode).subscribe((response: any) => {

                                var result: ItemGovernmentProcedureTypeList = response.Result;
                                processType.ProcessTypeName = result.LocalName;
                            });
                            item.ItemAdditionalStatus = true;
                            item.AddSupplierInvoiceItemProcesType(processType);




                        }
                    }
                }

                for (let item of this.ItemsSource.Collection) {
                    var number = args.ItemsSource.Collection.filter(d => d.Number == item.SequenceNumeric)[0];
                    if (number) {
                        item.ItemAdditionalStatusVisibility = true;
                    }
                }
                if (this.EntityPM.IsAccumalated) {
                    if (this.AccumulatedFilterSelectedValue != 'Accumulated') {

                        var msg = new MessageWindow();
                        var ItemSourceCount = args.ItemsSource.Length;
                        msg.RTL = true;
                        msg.Show("קוד תהליך נשמר בהצלחה ב-" + ItemSourceCount + " שורות ");
                        // msg.Show("קוד תהליך נשמר בהצלחה בשורות " + ItemSourceCount );
                    }
                }
                else {
                    var msg = new MessageWindow();
                    var ItemSourceCount = args.ItemsSource.Length;
                    msg.RTL = true;
                    msg.Show("קוד תהליך נשמר בהצלחה ב-" + ItemSourceCount + " שורות ");
                    //  msg.Show("קוד תהליך נשמר בהצלחה בשורות " + ItemSourceCount );
                }

            }
        }


    }

    GetDifferenceAndTotalForeignCurrency() {

        this.SumDifference = 0;
        var SumTotalForeignCurrency = 0;

        if (this.ModificationAndDiscountTypeList) {

            this.EntityPM.SupplierInvoiceModifications.forEach(item => {

                var ModificationAffectType = !AppTool.IsNullOrEmpty(this.ModificationAndDiscountTypeList.get(item.TypeCode)) ? this.ModificationAndDiscountTypeList.get(item.TypeCode) : ''



                if (item.Amount != null && (ModificationAffectType == '1' || ModificationAffectType == '2')) {

                    if (item.CurrencyTypeCode != this.EntityPM.InvoiceCurrencyTypeCode) {
                        this.TotalExportModificationInInvoiceCurrency = 0;
                        var ratePM: any;
                        var InvocieCurrencyRate: number = 0;
                        var rate: number = 0;
                        var total: number = 0;
                        if (this.ExchangeRates) {// this is a bug in errorslog filter of undefined, solution: if no exchange rate try to load them if not it will not be calculated----mohammad.
                            ratePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == this.EntityPM.InvoiceCurrencyTypeCode)[0];//.ExchangeRate;
                        }
                        else {
                            this.LoadExchangeRatesForModification(item);
                            return;
                        }
                        if (ratePM) {
                            InvocieCurrencyRate = ratePM.ExchangeRate;
                        }
                        total = (isNaN(item.Amount)) ? 0 : item.Amount;
                        var ModificationCurrencyRate = this.ExchangeRates.filter(d => d.CurrencyTypeCode == item.CurrencyTypeCode)[0];
                        if (ModificationCurrencyRate) {
                            rate = ModificationCurrencyRate.ExchangeRate;
                            if (InvocieCurrencyRate > 0) {
                                total = total * (rate / InvocieCurrencyRate);
                            }
                            else {
                                total = total * rate;
                            }
                        }
                        this.TotalExportModificationInInvoiceCurrency = (this.TotalExportModificationInInvoiceCurrency + total);

                        if (ModificationAffectType == '1') {
                            this.SumDifference += this.TotalExportModificationInInvoiceCurrency;
                            SumTotalForeignCurrency += this.TotalExportModificationInInvoiceCurrency;

                        }
                        else {
                            this.SumDifference -= this.TotalExportModificationInInvoiceCurrency;
                            SumTotalForeignCurrency -= this.TotalExportModificationInInvoiceCurrency;
                        }


                    }

                    else {
                        if (ModificationAffectType == '1') {
                            this.SumDifference += item.Amount;
                            SumTotalForeignCurrency += item.Amount;

                        }
                        else {
                            this.SumDifference -= item.Amount;
                            SumTotalForeignCurrency -= item.Amount;
                        }


                    }


                }


            });
            if (this.EntityPM.SupplierInvoiceItems.length > 0) {
                this.EntityPM.SupplierInvoiceItems.forEach(item => {

                    if (item.SupplierInvoiceItemsMods.length > 0) {

                        item.SupplierInvoiceItemsMods.forEach(Modifications => {
                            var ModificationAffectType = !AppTool.IsNullOrEmpty(this.ModificationAndDiscountTypeList.get(Modifications.TypeCode)) ? this.ModificationAndDiscountTypeList.get(Modifications.TypeCode) : ''


                            if (Modifications.Amount != null && (ModificationAffectType == '1' || ModificationAffectType == '2')) {

                                if (Modifications.CurrencyTypeCode != this.EntityPM.InvoiceCurrencyTypeCode) {
                                    this.TotalExportModificationInInvoiceCurrency = 0;
                                    var ratePM: any;
                                    var firstRatePM: any;
                                    var InvocieCurrencyRate: number = 0;
                                    var rate: number = 0;
                                    var total: number = 0;
                                    if (this.ExchangeRates) {// this is a bug in errorslog filter of undefined, solution: if no exchange rate try to load them if not it will not be calculated----mohammad.
                                        ratePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == this.EntityPM.InvoiceCurrencyTypeCode)[0];//.ExchangeRate;
                                        //firstRatePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == firstInvoice.InvoiceCurrencyTypeCode)[0];//.FirstExchangeRate;
                                    }
                                    else {
                                        this.LoadExchangeRatesForModification(Modifications);
                                        return;
                                    }
                                    if (ratePM) {
                                        InvocieCurrencyRate = ratePM.ExchangeRate;
                                    }
                                    total = (isNaN(Modifications.Amount)) ? 0 : Modifications.Amount;
                                    var ModificationCurrencyRate = this.ExchangeRates.filter(d => d.CurrencyTypeCode == Modifications.CurrencyTypeCode)[0];
                                    if (ModificationCurrencyRate) {
                                        rate = ModificationCurrencyRate.ExchangeRate;
                                        if (InvocieCurrencyRate > 0) {
                                            total = total * (rate / InvocieCurrencyRate);
                                        }
                                        else {
                                            total = total * rate;
                                        }
                                    }
                                    this.TotalExportModificationInInvoiceCurrency = (this.TotalExportModificationInInvoiceCurrency + total);

                                    if (ModificationAffectType == '1') {
                                        this.SumDifference += this.TotalExportModificationInInvoiceCurrency;
                                        SumTotalForeignCurrency += this.TotalExportModificationInInvoiceCurrency;

                                    }
                                    else {
                                        this.SumDifference -= this.TotalExportModificationInInvoiceCurrency;
                                        SumTotalForeignCurrency -= this.TotalExportModificationInInvoiceCurrency;
                                    }


                                }

                                else {


                                    if (ModificationAffectType == '1') {
                                        this.SumDifference += Modifications.Amount;
                                        SumTotalForeignCurrency += Modifications.Amount;

                                    }
                                    else {
                                        this.SumDifference -= Modifications.Amount;
                                        SumTotalForeignCurrency -= Modifications.Amount;
                                    }



                                }


                            }


                        });

                    }
                });


            }

            this.SumDifference += this.EntityPM.SupplierInvoiceItems.reduce((acc, cur) => acc + cur.ItemPrice, 0);


            this.Parent.Difference = Math.round(Math.abs(this.InvoiceAmount - this.SumDifference) * 100) / 100;


            SumTotalForeignCurrency += this.EntityPM.SupplierInvoiceItems.reduce((acc, cur) => acc + cur.ItemPrice, 0);

            this.Parent.TotalForeignCurrency = SumTotalForeignCurrency;

            if (isNaN(this.Parent.TotalForeignCurrency)) this.Parent.TotalForeignCurrency = 0;

            if (this.Parent.TotalForeignCurrency != 0) {
                if (this.Parent.Difference != null) {
                    if (this.Parent.Difference != 0) {
                        this.Parent.DifferenceColor = FontTool.Red; //red
                    }
                    else {
                        this.Parent.DifferenceColor = FontTool.Green; //green
                    }
                }
            }
        }
    }



    GetFreightTotals() {

        if (this.declarationPM.Direction != "E") {
            this.supplierInvoiceService.GetTotalForeignCurrencyForInvoice(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey).subscribe((response: any) => {
                if (response != null) {
                    this.Parent.TotalForeignCurrency = response.Result;
                    //for (let item of items)// this.entitypm(d=> d. SupplierInvoiceItemViewModel item in InvoiceItemsObslist.Where(d => d.entityPM.CounterKey == 0))
                    //{
                    //    if (item.ItemPrice != null)
                    //        this.TotalForeignCurrency = (TotalForeignCurrency != null ? TotalForeignCurrency : 0) + item.ItemPrice;
                    //}
                    if (isNaN(this.Parent.TotalForeignCurrency)) this.Parent.TotalForeignCurrency = 0;
                    var amount: number = this.InvoiceAmount;

                    if (isNaN(this.InvoiceAmount)) amount = 0;
                    if (this.declarationPM.Direction != "E")
                        this.Parent.Difference = this.Parent.TotalForeignCurrency - amount;


                    if (this.Parent.TotalForeignCurrency != 0) {
                        if (this.Parent.Difference != null) {
                            if (this.Parent.Difference != 0) {
                                this.Parent.DifferenceColor = FontTool.Red; //red
                            }
                            else {
                                this.Parent.DifferenceColor = FontTool.Green; //green
                            }
                        }
                    }
                    else {
                        this.Parent.DifferenceColor = FontTool.Black;
                    }
                }
            });
        }

    }

    private timerToken: any;
    public IncotermLogic(IncotermCode: string) {

        var _CustomsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        _CustomsSettingExtendedListService.GetDefault("ISRAEL", "NGG_INS_IMPORT", "NON", this.declarationPM.CustomerCode, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            if (response != null) {
                let obj = response.Result;
                var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();

                if (obj) {
                    let DefaultValue = obj['DefaultValue'];
                    if (!AppTool.IsNullOrEmpty(DefaultValue) && DefaultValue == "Y" && this.IncotermCode == 'CIF' && this.IsFirstInvoice()) {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                        return;

                    }
                }


                if (this.EntityPM != null && this.EntityPM.IncotermCode != null) {
                    //#region for insurance
                    if (!this.IsFirstInvoice()) {

                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                    }
                    else {

                        if (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP") {

                            if (this.incotermChanged) {
                                if (this.InsuranceAmount == null && this.InsurancePercentage == null && this.InsruanceCurrencyTypeCode == null) {

                                    if (this.InsuranceAmount != null) {
                                        this.InsuranceAmount = null;
                                    }
                                    if (this.InsruanceCurrencyTypeCode != null) {
                                        this.InsruanceCurrencyTypeCode = null;
                                    }

                                    if (this.InsurancePercentage != null) {
                                        this.InsurancePercentage = null;
                                    }

                                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                                }
                                else {

                                    this.timerToken = setTimeout(() => {
                                        var confirm = new ConfirmWindow();
                                        confirm.Cancel = true;
                                        confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                        confirm.ShowNoButton = true;
                                        confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
                                        confirm.WindowClosed.subscribe((event: any) => {
                                            if (confirm.Yes) {
                                                if (this.InsuranceAmount != null) {
                                                    this.InsuranceAmount = null;
                                                }
                                                if (this.InsruanceCurrencyTypeCode != null) {
                                                    this.InsruanceCurrencyTypeCode = null;
                                                }

                                                if (this.InsurancePercentage != null) {
                                                    this.InsurancePercentage = null;
                                                }

                                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                                                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                                            }

                                            else {
                                                this.allowToDelete = false;
                                                this.IncotermCode = this.oldIncoterm;
                                            }


                                        });
                                    }, 200);

                                }
                            }

                            else {
                                if (this.InsuranceAmount != null) {
                                    this.InsuranceAmount = null;
                                }
                                if (this.InsruanceCurrencyTypeCode != null) {
                                    this.InsruanceCurrencyTypeCode = null;
                                }

                                if (this.InsurancePercentage != null) {
                                    this.InsurancePercentage = null;
                                }


                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                            }
                        }
                        else if (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR") {
                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                            if (this.InsurancePercentage == null) {

                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);

                                if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                                }
                            }
                            else {

                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

                            }
                        }
                        else if (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F")) {

                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                            if (this.InsurancePercentage == null) {

                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);


                                if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                                }
                            }
                            else {
                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

                            }
                        }
                        else {
                            if (this.InsurancePercentage != null) {
                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                            }
                            else if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                            }
                            else {
                                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                            }
                        }
                    }

                }
                else {
                    if (!this.IsFirstInvoice()) {
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                    else if (!(this.EntityPM.SequenceNumeric != 1 && this.EntityPM.SequenceNumeric != null) || (this.isNewEntity && this.declarationPM.SupplierInvoices.length > 0)) {
                        this.AddAmountEnabled = true;
                        this.FreightAmountGridEnabled = true;
                        this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);

                        if (this.InsurancePercentage == null) {
                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                            if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                            }
                            else {
                                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                            }
                        }
                        else {
                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                        }
                    }
                }


            }
        });
    }

    //public IncotermLogic(IncotermCode: string) {


    //    var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();

    //    if (this.EntityPM != null && this.EntityPM.IncotermCode != null) {

    //        //#region For freight 
    //        if (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP") {
    //            if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0 && this.incotermChanged) {

    //                this.timerToken = setTimeout(() => {
    //                    var confirm = new ConfirmWindow();
    //                    confirm.Cancel = true;
    //                    confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
    //                    confirm.ShowNoButton = true;
    //                    confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
    //                    confirm.WindowClosed.subscribe((event: any) => {
    //                        if (confirm.Yes) {
    //                            //for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
    //                            //    this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
    //                            //}


    //                            //for (let item of this.FreightCopyList) {
    //                            //    this.AmountList.Remove(item);
    //                            //}

    //                            //this.FreightCurrencyTypeCode = null;
    //                            //this.EntityPM.TotalFreightInFreightCurrency = 0;


    //                            //this.AddAmountEnabled = false;

    //                            //this.FreightAmountGridEnabled = false;
    //                            //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);

    //                            if (this.InsuranceAmount != null) {
    //                                this.InsuranceAmount = null;
    //                            }
    //                            if (this.InsruanceCurrencyTypeCode != null) {
    //                                this.InsruanceCurrencyTypeCode = null;
    //                            }

    //                            if (this.InsurancePercentage != null) {
    //                                this.InsurancePercentage = null;
    //                            }


    //                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);

    //                        }

    //                        else {
    //                            this.allowToDelete = false;
    //                            this.IncotermCode = this.oldIncoterm;
    //                            //this.termsOfSaleTypeListService.getSingle(IncotermCode).subscribe((response:any) => {
    //                            //    var incoterm: TermsOfSaleTypeList = response.Result;
    //                            //    this.EntityPM.IncotermName = incoterm.LocalName;
    //                            //});

    //                        }



    //                    });
    //                }, 1);


    //            }


    //            else {

    //                //this.AddAmountEnabled = false;

    //                //this.FreightAmountGridEnabled = false;
    //                //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //            }



    //        }
    //        //else if (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR") {
    //        //    if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0 && this.incotermChanged) {
    //        //        this.timerToken = setTimeout(() => {
    //        //            var confirm = new ConfirmWindow();


    //        //            confirm.Cancel = true;
    //        //            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
    //        //            confirm.ShowNoButton = true;
    //        //            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
    //        //            confirm.WindowClosed.subscribe((event: any) => {
    //        //                if (confirm.Yes) {


    //        //                    for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
    //        //                        this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
    //        //                    }

    //        //                    this.CopyAmountList();

    //        //                    for (let item of this.FreightCopyList) {
    //        //                        this.AmountList.Remove(item);
    //        //                    }

    //        //                    this.FreightCurrencyTypeCode = null;
    //        //                    this.EntityPM.TotalFreightInFreightCurrency = 0;


    //        //                    this.AddAmountEnabled = false;

    //        //                    this.FreightAmountGridEnabled = false;
    //        //                    this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);

    //        //                }

    //        //                else {
    //        //                    this.allowToDelete = false;
    //        //                    IncotermCode = this.oldIncoterm;
    //        //                }

    //        //            });
    //        //        }, 200);

    //        //    }


    //        //    else {
    //        //        this.AddAmountEnabled = false;

    //        //        this.FreightAmountGridEnabled = false;
    //        //        this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //        //    }

    //        //}
    //        else if (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F")) {


    //            this.AddAmountEnabled = true;

    //            this.FreightAmountGridEnabled = true;
    //            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //        }
    //        else {

    //            this.AddAmountEnabled = true;

    //            this.FreightAmountGridEnabled = true;
    //            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //        }
    //        //#endregion

    //        //#region for insurance


    //        if (!this.IsFirstInvoice()) {//if ((this.EntityPM.SequenceNumeric != 1 && this.EntityPM.SequenceNumeric != null) || (this.isNewEntity && this.declarationPM.SupplierInvoices.length > 0)) {

    //            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);


    //        }
    //        else {

    //            if (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP") {

    //                if (this.incotermChanged) {
    //                    if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0) {
    //                        if (this.InsuranceAmount == null && this.InsurancePercentage == null && this.InsruanceCurrencyTypeCode == null) {

    //                            if (this.InsuranceAmount != null) {
    //                                this.InsuranceAmount = null;
    //                            }
    //                            if (this.InsruanceCurrencyTypeCode != null) {
    //                                this.InsruanceCurrencyTypeCode = null;
    //                            }

    //                            if (this.InsurancePercentage != null) {
    //                                this.InsurancePercentage = null;
    //                            }

    //                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                        }
    //                        else {

    //                            this.timerToken = setTimeout(() => {
    //                                var confirm = new ConfirmWindow();
    //                                confirm.Cancel = true;
    //                                confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
    //                                confirm.ShowNoButton = true;
    //                                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
    //                                confirm.WindowClosed.subscribe((event: any) => {
    //                                    if (confirm.Yes) {
    //                                        //for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
    //                                        //    this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
    //                                        //}


    //                                        //for (let item of this.FreightCopyList) {
    //                                        //    this.AmountList.Remove(item);
    //                                        //}

    //                                        //this.FreightCurrencyTypeCode = null;
    //                                        //this.EntityPM.TotalFreightInFreightCurrency = 0;


    //                                        //this.AddAmountEnabled = false;

    //                                        //this.FreightAmountGridEnabled = false;
    //                                        //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);

    //                                        if (this.InsuranceAmount != null) {
    //                                            this.InsuranceAmount = null;
    //                                        }
    //                                        if (this.InsruanceCurrencyTypeCode != null) {
    //                                            this.InsruanceCurrencyTypeCode = null;
    //                                        }

    //                                        if (this.InsurancePercentage != null) {
    //                                            this.InsurancePercentage = null;
    //                                        }


    //                                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);

    //                                    }

    //                                    else {
    //                                        this.allowToDelete = false;
    //                                        IncotermCode = this.oldIncoterm;
    //                                    }


    //                                });
    //                            }, 200);

    //                        }


    //                    }
    //                }

    //                else {
    //                    if (this.InsuranceAmount != null) {
    //                        this.InsuranceAmount = null;
    //                    }
    //                    if (this.InsruanceCurrencyTypeCode != null) {
    //                        this.InsruanceCurrencyTypeCode = null;
    //                    }

    //                    if (this.InsurancePercentage != null) {
    //                        this.InsurancePercentage = null;
    //                    }


    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                }
    //            }

    //            else if (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR") {
    //                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                if (this.InsurancePercentage == null) {

    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);

    //                    if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                    }
    //                }
    //                else {

    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

    //                }
    //                //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);

    //            }
    //            else if (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F")) {

    //                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                if (this.InsurancePercentage == null) {

    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);


    //                    if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                    }
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

    //                }
    //                //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);


    //            }
    //            else {
    //                if (this.InsurancePercentage != null) {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                }
    //                else if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                }



    //            }



    //        }
    //        // #endregion

    //    }


    //    else {


    //        if (!this.IsFirstInvoice()) {
    //            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //        }
    //        else if (!(this.EntityPM.SequenceNumeric != 1 && this.EntityPM.SequenceNumeric != null) || (this.isNewEntity && this.declarationPM.SupplierInvoices.length > 0)) {


    //            this.AddAmountEnabled = true;
    //            this.FreightAmountGridEnabled = true;
    //            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);

    //            if (this.InsurancePercentage == null) {

    //                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);


    //                if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                }
    //            }
    //            else {
    //                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

    //            }

    //            //else{

    //            //    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //            //    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //            //    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //            //}
    //        }
    //    }

    //}

    AddFreightAmount() {
        if (this.IsDisplayOnly)
            return; // go back -_-

        if (this.AddAmountEnabled) {
            var item: SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(this.EntityPM);
            item.DeclarationId = this.EntityPM.DeclarationId;
            item.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
            item.Tenant = this.EntityPM.Tenant;
            item.ChangeSetOp = "Insert";

            this.EntityPM.AddSupplierInvoiceFreightAmount(item);
            this.BuildFreightAmountsList();
        }
    }

    Add() {
        if (this.IsDisplayOnly)
            return; // go back -_-

        var line: number = 0;
        var sequence: number = 0;
        if (this.EntityPM.SupplierInvoiceItems.length > 0) {
            if (isNaN(this.EntityPM.InvoiceItemLastLineNumber)) this.EntityPM.InvoiceItemLastLineNumber = 0;
            line = this.EntityPM.InvoiceItemLastLineNumber;
            this.EntityPM.InvoiceItemLastLineNumber = this.EntityPM.InvoiceItemLastLineNumber + 1;
        }
        else {
            this.EntityPM.InvoiceItemLastLineNumber = 1;
            line = 0;
        }


        //if (this.EntityPM.FullItemsCount < 500 || AppTool.IsNullOrEmpty(this.EntityPM.FullItemsCount)) {

        // var items = this.EntityPM.SupplierInvoiceItems..sort(d => d.SequenceNumeric);
        var items = this.EntityPM.SupplierInvoiceItems.sort((a, b) => { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1 });
        if (items.length == 0) sequence = 0;
        else {
            sequence = items[this.EntityPM.SupplierInvoiceItems.length - 1].SequenceNumeric;
        }


        //}
        //else {
        //    sequence = this.EntityPM.FullItemsCount;

        //}

        line += 1;
        sequence += 1;

        var item: SupplierInvoiceItemPM = new SupplierInvoiceItemPM(this.EntityPM);
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.CounterKey = this.EntityPM.InvoiceCounterKey;
        item.Tenant = this.EntityPM.Tenant;
        item.LineNumber = line;
        item.OrderByLineNo = line.toString();
        item.SequenceNumeric = sequence;
        item.LastCopyFromOrderNo = "10000";
        this.EntityPM.FullChildrenCount++;
        this.ChildrenCount = "(" + this.EntityPM.FullChildrenCount + ")";


        if (!this.EntityPM.SupplierInvoiceItems.includes(item)) {
            this.EntityPM.AddSupplierInvoiceItem(item);
            this.ItemsSource.Insert(new SupplierInvoiceItemLine(item, this, this.allowExport));
            //this.CurrentSession.ResetRowIndex();
            if (isNaN(this.EntityPM.FullItemsCount)) this.EntityPM.FullItemsCount = 0;

            this.EntityPM.FullItemsCount = this.EntityPM.FullItemsCount + 1;
            this.EntityPM.MaxSequence = this.EntityPM.MaxSequence + 1;
        }
        this.calculateTotals(false, null);


    }

    Add___() {
        if (this.IsDisplayOnly)
            return; // go back -_-

        var line: number = 0;
        var sequence: number = 0;
        if (this.EntityPM.SupplierInvoiceItems.length > 0) {
            if (isNaN(this.EntityPM.InvoiceItemLastLineNumber)) this.EntityPM.InvoiceItemLastLineNumber = 0;
            line = this.EntityPM.InvoiceItemLastLineNumber;
        }

        if (this.EntityPM.FullItemsCount < 500 || AppTool.IsNullOrEmpty(this.EntityPM.FullItemsCount)) {

            // var items = this.EntityPM.SupplierInvoiceItems..sort(d => d.SequenceNumeric);
            var items = this.EntityPM.SupplierInvoiceItems.sort((a, b) => { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1 });
            if (items.length == 0) sequence = 0;
            else {
                sequence = items[this.EntityPM.SupplierInvoiceItems.length - 1].SequenceNumeric;
            }


        }
        else {
            sequence = this.EntityPM.FullItemsCount;

        }

        line += 1;
        sequence += 1;

        var item: SupplierInvoiceItemPM = new SupplierInvoiceItemPM(this.EntityPM);
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.CounterKey = this.EntityPM.InvoiceCounterKey;
        item.Tenant = this.EntityPM.Tenant;
        item.LineNumber = line;
        item.OrderByLineNo = line.toString();
        item.SequenceNumeric = sequence;
        this.EntityPM.FullChildrenCount++;
        this.ChildrenCount = "(" + this.EntityPM.FullChildrenCount + ")";


        if (!this.EntityPM.SupplierInvoiceItems.includes(item)) {
            this.EntityPM.AddSupplierInvoiceItem(item);
            this.ItemsSource.Insert(new SupplierInvoiceItemLine(item, this, this.allowExport));
            //this.CurrentSession.ResetRowIndex();
            if (isNaN(this.EntityPM.FullItemsCount)) this.EntityPM.FullItemsCount = 0;

            this.EntityPM.FullItemsCount = this.EntityPM.FullItemsCount + 1;
            this.EntityPM.MaxSequence = this.EntityPM.MaxSequence + 1;
        }
        this.calculateTotals(false, null);



    }

    public filters: ApiQueryFilters = null;
    BuildFreightAmountsList() {
        if (this.AmountList != null) {
            this.AmountList.Clear();

            for (var i = 0; i < this.EntityPM.SupplierInvoiceFreightAmounts.length; i++) {

                this.AmountList.Insert(new SupplierInvoiceFreightAmountLine(this.EntityPM.SupplierInvoiceFreightAmounts[i], this));

            }

            if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0) {
                this.FreightCurrencyTypeCode = null;

            }

            if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0) {
                var item: SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(this.EntityPM);
                item.DeclarationId = this.EntityPM.DeclarationId;
                item.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
                item.Tenant = this.EntityPM.Tenant;
                item.ChangeSetOp = "Insert";
                this.EntityPM.IsDirty = false;
                this.AmountList.Insert(new SupplierInvoiceFreightAmountLine(item, this), false);

            }

            this.CopyAmountList();
        }
    }

    LoadCurrenciesExchangeRates(recalculateTotals: boolean) {


        var currencyRates: string = "";

        if (this.AmountList != null && this.AmountList.Length > 0) {
            for (let line of this.AmountList.Collection) {
                currencyRates = currencyRates + "," + line.CurrencyTypeCode;

            }

            if (!AppTool.IsNullOrEmpty(this.FreightCurrencyTypeCode)) {
                if (!currencyRates.includes(this.FreightCurrencyTypeCode)) {
                    currencyRates = currencyRates + "," + this.FreightCurrencyTypeCode;
                }
            }


            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(currencyRates, this.declarationPM.TaxationDateTime).subscribe((response: any) => {

                var result = response.Result;
                if (result) {
                    this.customsExchangeRates = result;
                    if (recalculateTotals) {

                        this.UpdateTotalFreightInInvoiceCurrencyAndInNIS();
                        this.ReCalculateInsuranceAmount();
                    }
                }



            });
        }
    }


    public customsExchangeRates: CustomsExchangeRatePM[];
    UpdateTotalFreightInInvoiceCurrencyAndInNIS() {

        var totalFreightInNIS: number = 0;
        var totalFreightInInvoice: number = 0;

        for (let item of this.AmountList.Collection) {
            var amountInNIS: number;
            var rate: CustomsExchangeRatePM;
            if (item.CurrencyTypeCode == "ILS") {
                amountInNIS = AppTool.ToNumber(item.Amount);
                totalFreightInNIS = +totalFreightInNIS + +amountInNIS;
                //if (totalFreightInNIS != null) {
                //    totalFreightInNIS = Math.round(totalFreightInNIS);
                //}
            }

            else {
                if (this.customsExchangeRates) {

                    rate = this.customsExchangeRates.filter(d => d.CurrencyTypeCode == item.CurrencyTypeCode)[0];
                    if (rate != null) {

                        amountInNIS = AppTool.ToNumber(item.Amount) * rate.ExchangeRate;
                        totalFreightInNIS = totalFreightInNIS + amountInNIS;
                        //if (totalFreightInNIS != null) {
                        //    totalFreightInNIS = Math.round(totalFreightInNIS);
                        //}

                    }
                }
            }
        }

        if (this.FreightCurrencyTypeCode == "ILS") {
            totalFreightInInvoice = totalFreightInNIS;
        }

        else {
            var invoiceRate: CustomsExchangeRatePM = this.customsExchangeRates.filter(d => d.CurrencyTypeCode == this.FreightCurrencyTypeCode)[0];
            if (invoiceRate != null) {

                totalFreightInInvoice = totalFreightInNIS / invoiceRate.ExchangeRate;

            }

        }

        //if (totalFreightInInvoice != null) {
        //    totalFreightInInvoice = Math.round(totalFreightInInvoice);
        //}

        if (this.EntityPM.TotalFreightInFreightCurrency != totalFreightInInvoice) {
            this.TotalFreightAmountInInvoiceCurrency = totalFreightInInvoice;
            this.TotalFreightInFreightCurrency = totalFreightInInvoice;
        }
        if (this.EntityPM.TotalFreightInNIS != totalFreightInNIS) {
            this.TotalFreightAmountInNIS = totalFreightInNIS;
        }

        var percentage;
        //if (this.declarationPM.SupplierInvoices.length > 0) {
        //    percentage = this.declarationPM.SupplierInvoices[0].InsruancePercentage;
        //}
        //else {
        //    percentage = this.InsurancePercentage;
        //}
        var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
        percentage = firstInvoice.InsruancePercentage;
        if (percentage != null) {

            this.CalculateInsuranceAmount(percentage);
        }

    }

    CalculateExportModificationAmount() {
        this.TotalExportModificationInInvoiceCurrency = 0;
        this.AdjustmentsList.Collection.forEach((entityPM) => {
            var ratePM: any;
            var firstRatePM: any;
            var InvocieCurrencyRate: number = 0;
            var rate: number = 0;
            var total: number = 0;
            if (this.ExchangeRates) {// this is a bug in errorslog filter of undefined, solution: if no exchange rate try to load them if not it will not be calculated----mohammad.
                ratePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == this.ExportModificationCurrency)[0];//.ExchangeRate;
                //firstRatePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == firstInvoice.InvoiceCurrencyTypeCode)[0];//.FirstExchangeRate;
            }
            else {
                this.LoadExchangeRatesForModification(entityPM);
                return;
            }
            if (ratePM) {
                InvocieCurrencyRate = ratePM.ExchangeRate;
            }
            total = (isNaN(entityPM.Amount)) ? 0 : entityPM.Amount;
            var ModificationCurrencyRate = this.ExchangeRates.filter(d => d.CurrencyTypeCode == entityPM.CurrencyTypeCode)[0];
            if (ModificationCurrencyRate) {
                rate = ModificationCurrencyRate.ExchangeRate;
                if (InvocieCurrencyRate > 0) {
                    total = total * (rate / InvocieCurrencyRate);
                }
                else {
                    total = total * rate;
                }
            }
            this.TotalExportModificationInInvoiceCurrency = (this.TotalExportModificationInInvoiceCurrency + total);
        });

    }

    LoadExchangeRatesForModification(entityPM: any) {
        var currencyRates: string = "";
        currencyRates = currencyRates + "," + entityPM.CurrencyTypeCode;
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(currencyRates, this.declarationPM.TaxationDateTime).subscribe((response: any) => {

            var result = response.Result;
            if (result) {
                this.customsExchangeRates = result;
                this.CalculateExportModificationAmount();
            }
        });

    }
    ExchangeRates: CustomsExchangeRatePM[];
    CalculateInsuranceAmount(value: number) {

        var amount: number = (isNaN(this.InvoiceAmount)) ? 0 : this.InvoiceAmount;
        var total = (isNaN(this.TotalFreightAmountInInvoiceCurrency)) ? 0 : this.TotalFreightAmountInInvoiceCurrency;
        var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
        if (this.IsFirstInvoice()) {// if (this.SequenceNumeric == 1 || this.declarationPM.SupplierInvoices.length == 0 && !this.SequenceNumeric) {
            if (this.InsurancePercentage == null || this.InsurancePercentage == 0) {
                return;
            }
            // this.InsuranceAmount = (amount + total) * (this.InsurancePercentage / 100);
            this.InsruanceCurrencyTypeCode = this.InvoiceCurrencyTypeCode;
        }


        //if (value != null) {
        //    this.InsuranceAmount = Math.round(value);
        //}
        //else {
        //    this.InsuranceAmount = value;
        //}
        //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
        //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);

        //this.InsruanceCurrencyTypeCode = this.InvoiceCurrencyTypeCode;
        var firstInvoiceRate: number = 0;
        var totalFreight: number = 0;
        var totalAmount: number = 0;
        var InvocieCurrencyRate: number = 0;
        var rate: number = 0;

        var amount: number = 0;
        var total: number = 0;
        var ratePM: any;
        var firstRatePM: any;
        if (this.ExchangeRates) {// this is a bug in errorslog filter of undefined, solution: if no exchange rate try to load them if not it will not be calculated----mohammad.
            ratePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == this.InvoiceCurrencyTypeCode)[0];//.ExchangeRate;
            firstRatePM = this.ExchangeRates.filter(d => d.CurrencyTypeCode == firstInvoice.InvoiceCurrencyTypeCode)[0];//.FirstExchangeRate;
        }
        else {
            this.LoadCurrenciesExchangeRates(true);
            return;
        }
        if (firstRatePM) {
            firstInvoiceRate = firstRatePM.ExchangeRate;
        }
        if (ratePM) {
            InvocieCurrencyRate = ratePM.ExchangeRate;
        }
        var invoice: SupplierInvoicePM;
        //if (this.SequenceNumeric == 1 || this.declarationPM.SupplierInvoices.length == 0 && !this.SequenceNumeric) {
        //    invoice = this.EntityPM;
        //}
        //else {
        //    invoice = this.declarationPM.SupplierInvoices[0];
        //}
        invoice = this.Parent.Get1SupplierInvoice();// instead of doing the above code.

        amount = (isNaN(this.InvoiceAmount)) ? 0 : this.InvoiceAmount;
        total = (isNaN(this.TotalFreightInFreightCurrency)) ? 0 : this.TotalFreightInFreightCurrency;
        var FreightCurrencyRate = this.ExchangeRates.filter(d => d.CurrencyTypeCode == this.FreightCurrencyTypeCode)[0];
        if (FreightCurrencyRate) {
            rate = FreightCurrencyRate.ExchangeRate;
            if (InvocieCurrencyRate > 0) {
                total = total * (rate / InvocieCurrencyRate);
            }
            else {
                total = total * rate;
            }
        }



        for (let item of this.declarationPM.SupplierInvoices.filter(d => d.SequenceNumeric != this.EntityPM.SequenceNumeric)) {// loops the invoices

            if (item.FreightCurrencyTypeCode != this.InvoiceCurrencyTypeCode) {//calculate the freightamount
                if (!AppTool.IsNullOrEmpty(item.TotalFreightInFreightCurrency)) {
                    var result = this.ExchangeRates.filter(d => d.CurrencyTypeCode == item.FreightCurrencyTypeCode)[0];//.ExchangeRate;
                    if (result && InvocieCurrencyRate != 0) {
                        rate = result.ExchangeRate;

                        totalFreight = totalFreight + (item.TotalFreightInFreightCurrency * (rate / InvocieCurrencyRate));
                    }
                }
            }
            else {
                totalFreight = totalFreight + item.TotalFreightInFreightCurrency;
            }

            // calculate insurance
            if (invoice.InsruancePercentage) {// if no percentage then no automatic insurance.
                if (item.InvoiceCurrencyTypeCode != this.InvoiceCurrencyTypeCode) {
                    if (!AppTool.IsNullOrEmpty(item.InvoiceCurrencyTypeCode)) {
                        var x = this.ExchangeRates.filter(d => d.CurrencyTypeCode == item.InvoiceCurrencyTypeCode)[0];
                        if (x && InvocieCurrencyRate != 0) {
                            rate = x.ExchangeRate;


                            totalAmount = totalAmount + ((isNaN(item.InvoiceAmount)) ? 0 : item.InvoiceAmount * (rate / InvocieCurrencyRate));
                        }
                    }
                }
                else {
                    totalAmount = totalAmount + ((isNaN(item.InvoiceAmount)) ? 0 : item.InvoiceAmount);
                }

            }


        }
        var amountInCurrentInvoiceCurrency = (amount + total + totalAmount + totalFreight) * (invoice.InsruancePercentage / 100);
        var amountInFirstInoviceCurrency = 0;
        if (this.IsFirstInvoice()) {
            amountInFirstInoviceCurrency = amountInCurrentInvoiceCurrency;
        }
        else {
            amountInFirstInoviceCurrency = amountInCurrentInvoiceCurrency * InvocieCurrencyRate / firstInvoiceRate;
        }

        if (isNaN(amountInFirstInoviceCurrency)) {
            amountInFirstInoviceCurrency = 0;
        }
        var insAmt = amountInFirstInoviceCurrency == 0 ? null : amountInFirstInoviceCurrency;//(amount + total + totalAmount + totalFreight) * (invoice.InsruancePercentage / 100);
        if (insAmt == null) {
            invoice.InsuranceAmount = 0;
        }
        else {
            invoice.InsuranceAmount = Number(insAmt.toFixed(2));
        }
        if (this.IsFirstInvoice()) {
            this.InsuranceAmount = invoice.InsuranceAmount;
        }
        this.IncotermLogic(this.IncotermCode);// this is instead of just closing the two fields. below 
        //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
        //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);



    }


    OnInsuranceAmountLostFocus(event) {

        this.IncotermLogic(this.IncotermCode);// insteaad of doing the code below run the original code.
        //if (AppTool.IsNullOrEmpty(this.InsuranceAmount)) {

        //    if (this.InsurancePercentage == null && this.InsruanceCurrencyTypeCode == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);

        //        }

        //    }

        //    else {

        //    if (this.InsurancePercentage == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);


        //        }
        //    }
    }

    OnInsruanceCurrencyLostFocus(event) {
        this.IncotermLogic(this.IncotermCode);// insteaad of doing the code below run the original code.
        //if (this.InsruanceCurrencyTypeCode != null) {
        //    if (this.InsurancePercentage == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);



        //        }
        //    }
        //    else {
        //    if (this.InsurancePercentage == null && this.InsuranceAmount == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);


        //        }
        //    }
    }

    OnRowEnded($event) {
        //console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            //setTimeout(() => this.Add(), 1);
            this.Add();

        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    }

    Dispose() {
        if (this.ItemsSource) {
            this.ItemsSource.Collection.forEach((item) => {
                item.Dispose();
            });
        }

        if (this.AccumulatedFilterChangedEvent) {
            this.AccumulatedFilterChangedEvent.unsubscribe();
            this.AccumulatedFilterChangedEvent = null;
        }

        if (this.SearchFilterChangedEvent) {
            this.SearchFilterChangedEvent.unsubscribe();
            this.SearchFilterChangedEvent = null;
        }
    }

    SearchMethod() {


        if (!this.IsDisplayOnly) {
            var windowArgs: any = {};

            var logWindow = new LogitudeWindow();
            logWindow.Width = 700;
            logWindow.Height = 500;
            windowArgs.ImporterId = this.declarationPM.ImporterId;
            logWindow.ShowCloseButton = true;
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = "חיפוש ספקים מורחב";

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.SetVendorId(comp);
                    }
                });
            });

            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/VendorExtendedSearchComponent');
        }

    }

    SetVendorId(args) {
        if (args.SelectedRow) {

            if (!AppTool.IsNullOrEmpty(args.SelectedRow.DBVendorID)) {
                this.VendorId = args.SelectedRow.DBVendorID;
            } else {
                this.VendorId = args.SelectedRow.VendorId;
            }

        }
    }

    // Action Buttons
    UpdateCertificatesButtonClicked() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.MultiCertificateUpdate");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 666;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event == 'ok') {

                //client method
                if (this.ItemsSource.Collection) {
                    this.ItemsSource.Collection.forEach((item: SupplierInvoiceItemLine) => {
                        item.SetCertificateStatusVisibility();
                    });
                }

                //server method - replaced with client method
                //this.ReloadEntity(); 
            }

        });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/MultiCertificateUpdate/MultiCertificateUpdateComponent');
    }

    ReloadEntity() {
        this.ReloadEntityEvent.emit();
    }

    RefreshChangeInSupplierInvoice() {
        this.ChangeInSupplierInvoice = "1";
    }

    IsFirstInvoice() {
        var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
        return (this.EntityPM.InvoiceCounterKey == firstInvoice.InvoiceCounterKey);
    }

    InvoiceAmountBlur(text) {

        if (this.old_amount != this.EntityPM.InvoiceAmount ||
            this.old_currency != this.EntityPM.InvoiceCurrencyTypeCode ||
            this.old_vendor != this.EntityPM.VendorId) {
            this.Parent.CalculateCommissionPercentage();
            this.CalculateCurrencyTypeCode();
        }

        this.old_amount = this.EntityPM.InvoiceAmount;
        this.old_currency = this.EntityPM.InvoiceCurrencyTypeCode;
        this.old_vendor = this.EntityPM.VendorId;
    }
    CalculateCurrencyTypeCode() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.VendorId) && AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyTypeCode)) {
            this.vendorCurrencyService.GenListVendorCurrencyByVendorId(this.EntityPM.VendorId).subscribe(res => {

                if (!res.HasError) {
                    if (res.Result?.length == 1) {
                        this.EntityPM.InvoiceCurrencyTypeCode = res.Result[0].Currency;
                    }
                }
            })

        }
    }
    //#region Remark tooltip
    onCellSelected($event, Item: SupplierInvoiceItemLine) {
        if (this.SelectedRow != Item) {
            this.OnSelectedItemChanged(Item);
        }
    }
    OnSelectedItemChanged(selectedRow: SupplierInvoiceItemLine) {
        console.log("OnSelectedItemChanged > ", selectedRow);
        selectedRow.entityPM.DocumentFilingId = this.DocumentFilingId;

        if (selectedRow.entityPM.OcrHeight != 0 && !AppTool.IsNullOrEmpty(selectedRow.entityPM.OcrHeight) && selectedRow.entityPM.OcrPageNumber != 0 && !AppTool.IsNullOrEmpty(selectedRow.entityPM.OcrPageNumber) && selectedRow.entityPM.OcrTop != 0 && !AppTool.IsNullOrEmpty(selectedRow.entityPM.OcrTop)) {
            DeclarationEventManager.DeclarationSplitDocumentItemSelection.emit(selectedRow.entityPM);

        }
        else {
            DeclarationEventManager.DeclarationSplitDocumentItemSelection.emit("remove");
        }
        if (selectedRow) {

            if (this.SelectedRow != selectedRow) {
                if (this.SelectedRow) {
                    this.SelectedRow.ShowClassifierRemarkTooltip = false; // hide CR tooltip on prev selected row
                    selectedRow.closedManullay = false;

                    //this.SelectedRow.ShowTariffErrorTooltip = false; // hide Tariff tooltip on prev selected row

                }
            }
            if (!selectedRow.closedManullay) {
                selectedRow.ShowClassifierRemarkTooltip = true;
                selectedRow.CheckTariff();
                this.SelectedRow = selectedRow;
            }

        } else {
            if (this.SelectedRow) {
                this.SelectedRow.ShowClassifierRemarkTooltip = false;

                this.SelectedRow.closedManullay = false;
            }
            this.SelectedRow = null;
        }

    }



    InspectionRequest() {

        // var IsActivateInsurance = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "AIN")[0]? true : false;               
        //  if(!IsActivateInsurance) return;
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

        var IsAutoInsuranceExportSubmitFeature = FeatureLocator.Features.filter(f => (f.Code == "IsAutoInsuranceExportSubmit") && f.ObjectTableId == table.Id)[0];

        if (AppTool.IsNullOrEmpty(IsAutoInsuranceExportSubmitFeature)) {

            if (!this.EntityPM.IsDirty) return;

            if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse) return;

            if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyTypeCode) || AppTool.IsNullOrEmpty(this.EntityPM.InvoiceAmount) || AppTool.IsNullOrEmpty(this.EntityPM.IncotermCode)) return;
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();

            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == this.EntityPM.DeclarationId &&
                            mess.LogitudeViewModel == "SupplierInvoiceGeneralTabComponent.ts-ApprovalToInsurance");
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let ApprovalToInsure = ""; ApprovalToInsure = UnifreightMessageM.GetStringValue(mess, "ApprovalToInsure");
                            this.ActivateInsurance(ApprovalToInsure)
                        }
                    }
                );


            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(this.declarationPM.CustomFileNo, this.EntityPM.DeclarationId, "SupplierInvoiceGeneralTabComponent.ts-ApprovalToInsurance", "BFIFILE");
            unifreightMessageM.Requset.push(["AmountToInsure", this.EntityPM.InvoiceAmount.toString()]);
            unifreightMessageM.Requset.push(["Currency", this.EntityPM.InvoiceCurrencyTypeCode]);
            unifreightMessageM.Requset.push(["Incoterms", this.EntityPM.IncotermCode]);
            unifreightMessageM.Requset.push(["CustomerUNF", ""]);
            unifreightMessageM.Requset.push(["DefineToInsure", "Yes"]);
            unifreightMessageM.Requset.push(["ApprovalToInsure", ""]);
            unifreightMessageM.Requset.push(["InsuranceAmount", ""]);
            unifreightMessageM.Requset.push(["InsuranceCurrency", ""]);
            var PossibleOpenInsurance = TextCodeTranslator.Translate("Customs.Declaration.O.PossibleOpenInsurance");
            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "AmitalGatewayUtil.CustomExportApprovalToInsurance",
                "BFIHMAIN.LogitudeTask",
                "CustomExportApprovalToInsurance",
                unifreightMessageM,
                PossibleOpenInsurance);

        }
    }




    ActivateInsurance(ApprovalToInsure) {

        if (ApprovalToInsure == "Yes") {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.IsItForInsurance");

            var confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirm.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
            confirm.Width = 400;
            confirm.Show(msg);
            confirm.WindowClosed.subscribe((event: any) => {

                if (confirm.Yes == true) {
                    SessionLocator.SelectedSession.StartBusyIndicatorLoading();

                    let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                        .subscribe(
                            (mess: UnifreightMessageM) => {

                                var IsMatchUnifreightCallbackCommand = (
                                    mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                                    mess.LogitudeEntityNumber == this.EntityPM.DeclarationId &&
                                    mess.LogitudeViewModel == "SupplierInvoiceGeneralTabComponent.ts-ActivateInsurance");
                                if (IsMatchUnifreightCallbackCommand) {
                                    sub.unsubscribe();
                                    SessionLocator.SelectedSession.StopBusyIndicator();

                                    let InsuranceAmount = ""; InsuranceAmount = UnifreightMessageM.GetStringValue(mess, "InsuranceAmount");
                                    let InsuranceCurrency = ""; InsuranceCurrency = UnifreightMessageM.GetStringValue(mess, "InsuranceCurrency");
                                    let InvoiceNumber = ""; InvoiceNumber = UnifreightMessageM.GetStringValue(mess, "InvoiceNumber");

                                    let confirmWindow = new ConfirmWindow();
                                    confirmWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.CreatingInsurance");
                                    confirmWindow.Width = 350;
                                    confirmWindow.Height = 200;
                                    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                    confirmWindow.ShowNoButton = false;
                                    if (!AppTool.IsNullOrEmpty(InsuranceAmount) && !AppTool.IsNullOrEmpty(InsuranceCurrency)) {

                                        (new CurrencyTypeListService()).getSingleFromCache(InsuranceCurrency).subscribe(x => {
                                            let Insurance67: SupplierInvoiceModificationPM = this.FindModificationByCode("67");
                                            var index = this.AdjustmentsList.FindIndex("67");

                                            Insurance67.CurrencyTypeCode = InsuranceCurrency;
                                            Insurance67.Amount = Number(InsuranceAmount);
                                            Insurance67.CurrencyTypeName = x.Result.LocalName;
                                            this.AdjustmentsList.UpdateWithIndex(index, new ModificationItemModel(Insurance67, this, "67"));
                                        })
                                    }
                                    else if (!AppTool.IsNullOrEmpty(InvoiceNumber)) {

                                        var InsuranceOpenNum = TextCodeTranslator.Translate("Customs.Declaration.O.InsuranceOpenNum");
                                        var CompletedUnifreight = TextCodeTranslator.Translate("Customs.Declaration.O.CompletedUnifreight");
                                        confirmWindow.Show(InsuranceOpenNum + `' ` + InvoiceNumber + `, ` + CompletedUnifreight);

                                    }
                                    else {

                                        var OpenInsuranceFailed = TextCodeTranslator.Translate("Customs.Declaration.O.OpenInsuranceFailed");
                                        confirmWindow.Show(OpenInsuranceFailed);

                                    }
                                }
                            }
                        );


                    var unifreightMessageM =
                        AmitalGatewayUtil.Instance.
                            DeclarationMessaging.GetMessage(this.declarationPM.CustomFileNo, this.EntityPM.DeclarationId, "SupplierInvoiceGeneralTabComponent.ts-ActivateInsurance", "BFIFILE");
                    unifreightMessageM.Requset.push(["AmountToInsure", this.EntityPM.InvoiceAmount.toString()]);
                    unifreightMessageM.Requset.push(["Currency", this.EntityPM.InvoiceCurrencyTypeCode]);
                    unifreightMessageM.Requset.push(["Incoterms", this.EntityPM.IncotermCode]);
                    unifreightMessageM.Requset.push(["CustomerUNF", ""]);
                    unifreightMessageM.Requset.push(["DefineToInsure", "Yes"]);
                    unifreightMessageM.Requset.push(["ApprovalToInsure", "Yes"]);
                    unifreightMessageM.Requset.push(["InsuranceAmount", ""]);
                    unifreightMessageM.Requset.push(["InsuranceCurrency", ""]);

                    var OpeningInsuranceCase = TextCodeTranslator.Translate("Customs.Declaration.O.OpeningInsuranceCase");
                    AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                        "AmitalGatewayUtil.CustomExportActivateInsurance",
                        "BFIHMAIN.LogitudeTask",
                        "CustomExportActivateInsurance",
                        unifreightMessageM,
                        OpeningInsuranceCase);


                }

            });


        }

    }

    ShowIndication() {


        this.entityResourceService.getEntityResourceByTableName("Customs.ClientIndication").subscribe((response: any) => {
            var table = window.ObjectTables.filter(d => d.Name === 'Customs.ClientIndication')[0];

            var IsIndicationsClientFeature = FeatureLocator.Features.filter(f => (f.Code == "IndicationsClient") && f.ObjectTableId == table.Id)[0];
            if (IsIndicationsClientFeature) {

                if (this.IsPreference && this.allowExport) {

                    if (!AppTool.IsNullOrEmpty(this.declarationPM.ImporterId)) {
                        var msg = new MessageWindow();
                        msg.RTL = true;

                        let filters = new ApiQueryFilters();


                        filters.PageSize = 200;
                        filters.PageIndex = 0;
                        filters.GetAll = false;
                        filters.GetCount = true;

                        filters.addAdditionalFilter("ClientId", this.declarationPM.ImporterId, null, null, "Equals", false, false, false, "string", false);
                        filters.addAdditionalFilter("Tenant", this.declarationPM.Tenant, null, null, "Equals", true, false, false, "string");


                        return this.clientIndicationListService.getByFilters(filters)
                            .subscribe(r => {
                                this.clientIndicationList = r.Result

                                if (this.clientIndicationList.length > 0) {

                                    if (this.clientIndicationList.filter(x => x.CustomerIndicationTypeID == "2").length > 0) {
                                        var d = new Date();
                                        var date = Date.parse(d.toString())
                                        var IsExporterExpire = this.clientIndicationList.find(x => x.CustomerIndicationTypeID == "2" && Date.parse(x.StartDate.toString()) < date && Date.parse(x.EndDate.toString()) > date);

                                        if (!IsExporterExpire) {
                                            msg.Show(TextCodeTranslator.Translate("Customs.ClientIndication.O.DeclareInCustomsExpired"));
                                        }
                                    }
                                    else {
                                        msg.Show(TextCodeTranslator.Translate("Customs.ClientIndication.O.NotDeclareInCustoms"));

                                    }
                                }
                                else {
                                    msg.Show(TextCodeTranslator.Translate("Customs.ClientIndication.O.UpdatDataImpExp"));

                                }
                            });
                    }

                }


            }
        });
    }
    public get UpdateOptions() {
        return UpdateOptions;
    }
}

class UpdateGeneralArgsParams {
    public UpdateField: string;
    public LookUpTableName: string;
    public ObjectTableName: string;
    public Validate: any;
    public Title: string;
    public IsItemsWithNoValue: boolean;
    public ItemsWithNoValueTitle: string;
    public SelectionCompletedMethod: any;
}

class UpdateGeneralParams {
    public Title: string;
    public Arguments: UpdateGeneralArgsParams;
}

 enum UpdateOptions {
    QuantityType,
    ProcessCode,
    CountryOfOrigin,
    ClassificationCode,
    ProtocolCode,
    TradeAgreement
}

export class SupplierInvoiceItemLine extends BaseComponent {
    public entityPM: SupplierInvoiceItemPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItem";
    public DataContext = this;
    Parent: SupplierInvoiceGeneralTabComponent;
    public QuantityTypeCodeLoaded: any;
    IsBlueBorderVisibile: boolean = false;
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private customsVendorListService: CustomsVendorListService = new CustomsVendorListService();
    private _CustomsCountryListService: CustomsCountryListService = new CustomsCountryListService();
    private tradeAgreementProtocolListService: TradeAgreementProtocolListService = new TradeAgreementProtocolListService();

    public ShowClassefierRemarkInfo: boolean = false;
    public ShowClassifierRemarkTooltip: boolean = false;
    public ShowTariffErrorInfo: boolean = false;
    public ShowTariffErrorTooltip: boolean = false;
    public ShowValidatioIcon: boolean = false;

    public allowExport = false;
    public closedManullay: boolean = false;

    ClassefierRemarkToolTipWrapper: string = "ClassefierRemarkToolTipWrapper";
    ClassefierRemarkToolTip: string = "ClassefierRemarkToolTip";
    TariffErrorToolTipWrapper: string = "TariffErrorToolTipWrapper";
    TariffErrorToolTip: string = "TariffErrorToolTip";
    private CurrentSession = SessionLocator.SelectedSession;
    //public GITITEMCRs: GITITEMCR[];
    constructor(EntityPM: SupplierInvoiceItemPM, parent: SupplierInvoiceGeneralTabComponent, allowExport: boolean = false) {
        super();
        this.entityPM = EntityPM;
        this.allowExport = allowExport;
        //calculate ids
        this.ClassefierRemarkToolTipWrapper += EntityPM.SequenceNumeric;
        this.ClassefierRemarkToolTip += EntityPM.SequenceNumeric;
        //this.TariffErrorToolTipWrapper += EntityPM.SequenceNumeric;
        //this.TariffErrorToolTip += EntityPM.SequenceNumeric;

        ////Fill tariff error text
        //this.TariffErrorText = "מדינה לם תוםמת לקוד התעריף"; //"Tarrif doesnt match country” 

        ////get currenct customs country
        if (this.OriginCountryCode) {
            this._CustomsCountryListService.getSingle(this.OriginCountryCode).subscribe((res) => {
                var entity = res.Result;
                if (entity) {
                    this.CustomsCountry = entity;
                    this.OriginCountryName = this.CustomsCountry.LocalName;
                }
            });
        }

        if (this.entityPM.ClasifiedRemarks) {
            this.ShowClassefierRemarkInfo = true;
        }


        this.Parent = parent;
        this.oldvalue = this.entityPM.ItemPrice;

        if (this.Parent.accumulationFeature == null) {
            this.Parent.IsNotForAccumaltionVisibile = false;
        }

        if (this.Parent.IsReadOnly) {
            this.UIProperties.SetEnabled("NotForAccumaltion", "Customs.SupplierInvoiceItem", false);
        }


        if (this.entityPM.SupplierInvioceItemCertificats?.length == 0) {
            this.WarningVisiblity = false;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = false;


        }

        else if (this.entityPM.CertificatesStatusCode == "1") {
            this.OkVisiblity = true;
        }


        else if (this.entityPM.CertificatesStatusCode == "2") {
            this.WarningVisiblity = true;
        }
        else if (this.entityPM.CertificatesStatusCode == "3") {
            this.IsBlueBorderVisibile = true;
            this.OkVisiblity = true;
        }

        else if (this.entityPM.CertificatesStatusCode == "4") {
            this.WarningVisiblity = true;
            this.IsBlueBorderVisibile = true;
        }

        if (this.entityPM.ItemAdditionalStatus) {
            this.ItemAdditionalStatusVisibility = true;
        }

        else {

            this.ItemAdditionalStatusVisibility = false;
        }

        if (this.entityPM.SupplierInvoiceItemVehicles.length > 0) {
            this.VehicleOkVisiblity = true;
        }



        this.GetQuantityType(false, true);
        this.QuantityTypeCodeLoaded =
            //this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.QuantityTypeCodeLoadedEvent.subscribe((res) => {
                if (this.ClassificationCode == res.ClassificationCode) {
                    this.QunatityTypeCode = res.QuantityTypeCode;

                }
            })
            //)
            ;
    }


    //#region Properties

    private editButtonName;
    get EditButtonName() { return this.editButtonName; }
    set EditButtonName(value: string) { this.editButtonName = value; }

    private itemAdditionalStatusVisibility = false;
    get ItemAdditionalStatusVisibility() { return this.itemAdditionalStatusVisibility; }
    set ItemAdditionalStatusVisibility(value: boolean) { this.itemAdditionalStatusVisibility = value; }

    private okVisiblity = false;
    get OkVisiblity() { return this.okVisiblity; }
    set OkVisiblity(value: boolean) { this.okVisiblity = value; }

    private vehicleOkVisiblity = false;
    get VehicleOkVisiblity() { return this.vehicleOkVisiblity; }
    set VehicleOkVisiblity(value: boolean) { this.vehicleOkVisiblity = value; }


    private wrningVisiblity = false;
    get WarningVisiblity() { return this.wrningVisiblity; }
    set WarningVisiblity(value: boolean) { this.wrningVisiblity = value; }

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

        if (!AppTool.IsNullOrEmpty(this.ItemCode) && GITITEMCacheService.Instance.IsUnitPURForItems) {
            var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);// .ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            if (itemCodeDetails != null) {
                itemCodeDetails.InvoiceQuantityType = this.InvoiceQuantityType;
                itemCodeDetails.IsNew = true;
            }
        }
    }

    gITITEMCRs: GITITEMCR[];
    get GITITEMCRs() { return this.gITITEMCRs; }
    set GITITEMCRs(value: GITITEMCR[]) {

        if (!AppTool.IsNullOrEmpty(value)) {
            /*
            for (GITITEMCR k in value) {
                // var mySupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(k);
                // SupplierInvioceItemCertificatPM newSupplierInvioceItemCertificatPM = mySupplierInvioceItemCertificatPM{ declara };
                // newSupplierInvoiceItemPM.SupplierInvioceItemCertificats.push(newSupplierInvioceItemCertificatPM);
                
                for (let item of this.EntityPM.SupplierInvoiceItems.filter(d => !d.IsParent)) {
                    var exist = item.SupplierInvioceItemCertificats.filter(d => d.certificateNumber == k.REQCERT.replace(/^0+/, ''))[0];
                    if (!exist) {
                        var SupplierInvioceItemCertificat: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(this.entityPM);
                        SupplierInvioceItemCertificat.DeclarationId = this.entityPM.DeclarationId;
                        SupplierInvioceItemCertificat.InvoiceCounterKey = this.entityPM.CounterKey;
                        SupplierInvioceItemCertificat.LineNumber = this.entityPM.LineNumber;
                        SupplierInvioceItemCertificat.Tenant = this.entityPM.Tenant;
                        SupplierInvioceItemCertificat.CertificateNumber = "k.REQCERT";


                        item.AddSupplierInvioceItemCertificat(SupplierInvioceItemCertificat);



                    }
                }
            }
            */
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

    CustomsCountryChanged($event) {
        this.CustomsCountry = $event;
    }

    customsCountry: CustomsCountryPM;
    get CustomsCountry() { return this.customsCountry; }
    set CustomsCountry(value: CustomsCountryPM) {

        if (this.customsCountry != value) {
            this.customsCountry = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginCountryName = value.LocalName;
            if (!AppTool.IsNullOrEmpty(this.ItemCode) && GITITEMCacheService.Instance.IsCountryPURForItems) {
                //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);// .ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];

                if (itemCodeDetails != null) {
                    itemCodeDetails.OriginCountryCode = value.Code;
                    itemCodeDetails.OriginCountryName = value.LocalName;
                    itemCodeDetails.IsNew = true;
                }
            }
            this.CheckTariff();
        } else {
            this.OriginCountryName = null;
            this.OriginCountryCode = null;
            if (!AppTool.IsNullOrEmpty(this.ItemCode) && GITITEMCacheService.Instance.IsCountryPURForItems) {
                //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode)//.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails != null) {
                    itemCodeDetails.OriginCountryCode = null;
                    itemCodeDetails.OriginCountryName = null;
                    itemCodeDetails.IsNew = true;
                }
            }
            this.CheckTariff();
        }
    }

    public get OrderByLineNo() { return this.entityPM.OrderByLineNo; }
    public set OrderByLineNo(newValue: string) { this.entityPM.OrderByLineNo = newValue; }

    public get SequenceNumeric() { return this.entityPM.SequenceNumeric; }
    public set SequenceNumeric(newValue: number) { this.entityPM.SequenceNumeric = newValue; }

    public get LineNumber() { return this.entityPM.LineNumber; }
    public set LineNumber(newValue: number) { this.entityPM.LineNumber = newValue; }

    public get ItemCode() { return this.entityPM.ItemCode; }
    public set ItemCode(newValue: string) { this.entityPM.ItemCode = newValue; }

    public get ItemDescription() { return this.entityPM.ItemDescription; }
    public set ItemDescription(newValue: string) { this.entityPM.ItemDescription = newValue; }

    public get TariffID() { return this.entityPM.TradeAgreementCode; }
    public set TariffID(newValue: string) { this.entityPM.TradeAgreementCode = newValue; }

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
    public get ClassificationCode() { return this.entityPM.ClassificationCode; }
    public set ClassificationCode(newValue: string) {

        this.entityPM.ClassificationCode = newValue;

        if (newValue == null) {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");

        }

    }

    public get TradeAgreementCode() { return this.entityPM.TradeAgreementCode; }
    public set TradeAgreementCode(newValue: string) {
        this.entityPM.TradeAgreementCode = newValue;
        this.CheckTariff();
    }

    public get TradeAgreementName() { return this.entityPM.TradeAgreementName; }
    public set TradeAgreementName(newValue: string) { this.entityPM.TradeAgreementName = newValue; }

    public get DutyRegimeProtocolCode() { return this.entityPM.DutyRegimeProtocolCode; }
    public set DutyRegimeProtocolCode(newValue: string) {
        this.entityPM.DutyRegimeProtocolCode = newValue;
        this.tradeAgreementProtocolListService.getSingle(newValue).subscribe((res) =>
            this.DutyRegimeProtocolLocalName = res?.Result?.LocalName);
    }

    public get DutyRegimeProtocolLocalName() { return this.entityPM.DutyRegimeProtocolLocalName; }
    public set DutyRegimeProtocolLocalName(newValue: string) { this.entityPM.DutyRegimeProtocolLocalName = newValue; }

    public get InvoiceQuantity() { return this.entityPM.InvoiceQuantity; }
    public set InvoiceQuantity(newValue: number) { this.entityPM.InvoiceQuantity = newValue; }


    public get InvoiceQuantityType() { return this.entityPM.InvoiceQuantityType; }
    public set InvoiceQuantityType(newValue: string) {
        this.entityPM.InvoiceQuantityType = newValue;
    }

    public get InvoiceQuantityTypeName() { return this.entityPM.InvoiceQuantityTypeName; }
    public set InvoiceQuantityTypeName(newValue: string) { this.entityPM.InvoiceQuantityTypeName = newValue; }
    oldvalue: number = 0;
    doCalculate: boolean = false;
    public get ItemPrice() { return this.entityPM.ItemPrice; }
    public set ItemPrice(newValue: number) {

        if (newValue != this.entityPM.ItemPrice) {
            this.doCalculate = true;
            this.entityPM.ItemPrice = newValue;
        }


    }

    public get OriginCountryCode() { return this.entityPM.OriginCountryCode; }
    public set OriginCountryCode(newValue: string) {
        if (newValue) {
            this._CustomsCountryListService.getSingle(newValue).subscribe((res) => {
                var entity = res.Result;
                if (entity) {
                    this.CustomsCountry = entity;
                    this.OriginCountryName = this.CustomsCountry.LocalName;
                }
            });
        }
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

    OnItemPriceLostFocus(ItemPriceTextBox: any) {

        if (this.doCalculate) {
            if (isNaN(this.ItemPrice)) this.ItemPrice = 0;
            if (this.Parent.Parent.TotalForeignCurrency == null) this.Parent.Parent.TotalForeignCurrency = 0;
            if (isNaN(this.oldvalue)) this.oldvalue = 0;
            var totalFCurr: number = this.Parent.Parent.TotalForeignCurrency;
            this.Parent.Parent.TotalForeignCurrency = totalFCurr - this.oldvalue + this.ItemPrice;
            this.Parent.declarationPM.Direction != "E" ? this.Parent.Parent.Difference = this.Parent.Parent.TotalForeignCurrency - (this.Parent.InvoiceAmount) : this.Parent.GetDifferenceAndTotalForeignCurrency();


        }
        this.oldvalue = this.entityPM.ItemPrice;
        this.doCalculate = false;
        ItemPriceTextBox.TextValue = this.oldvalue;
    }

    GetQuantityType(isChangeInvoiceQuantityType: boolean = true, calcInvoiceQuantityType: boolean = false) {

        if (this.ClassificationCode != null) {
            var keys = Object.keys(this.Parent.ClasificationQtyTypes);
            if (keys.indexOf(this.ClassificationCode) > -1 && !calcInvoiceQuantityType) {
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
                this.Parent.quantityTypeMessageService.GetQuantityType(code, this.Parent.declarationPM.Direction === 'E').subscribe((myServiceResponse: ServiceResponse) => {
                    if (!myServiceResponse.HasError) {
                        this.Parent.declarationPM.ProcedureCurrentCode;

                        if (!myServiceResponse.HasError) {
                            if (myServiceResponse.Result) {
                                this.QunatityTypeCode = "(" + myServiceResponse.Result + ")";
                            }
                            else {
                                if (!calcInvoiceQuantityType) {
                                    this.Send8314()
                                    this.QunatityTypeCode = null;
                                }

                            }


                            if (this.Parent.IsChecked && isChangeInvoiceQuantityType) {
                                if (this.InvoiceQuantityType == null && myServiceResponse.Result != null) {
                                    this.InvoiceQuantityType = myServiceResponse.Result;
                                }
                            }
                            if (!(keys.indexOf(this.ClassificationCode) > -1) || calcInvoiceQuantityType) {
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

    NotForAccumaltionChecked(checked: boolean, item: SupplierInvoiceItemPM) {

        if (checked) {

            this.NotForAccumaltion = true;
        }
        else {
            this.NotForAccumaltion = false;
        }

    }
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    Send8314() {

        if (AppTool.IsNullOrEmpty(this.ClassificationCode)) return
        var currRequestParams = new CustomsItemDetailsQueryRequestParams();///Force new GUID On Each Send !!
        currRequestParams.ValidToDate = new Date();
        currRequestParams.Classification = this.ClassificationCode;
        currRequestParams.CustomsBookType = this.Parent.declarationPM.Direction == 'I' ? 1 : 2;

        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        currRequestParams.ForcePersonalSign = false;
        var ResponseData: any
        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession, currRequestParams.PBId, "שםילתם לנתוני פרט מכס", true)
            .then((res) => {
                if (res) {
                    ResponseData = res;

                    if (!ResponseData.HasError)
                        this.GetQuantityType(true, true)
                }

            }


            ).catch((err) => {


            });

        this._IIGGeneralMessagesService.PostCustomsItemDetailsQuery(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {

            });


    }


    public pointers: CustomsDocumentPointerPM[];
    DeleteButtonClicked() {
        this.pointers = this.Parent.Pointers;

        var pointer = this.pointers.filter(d => d.Child2EntityId === this.entityPM.LineNumber.toString())[0];
        if (pointer == null) {

            this.Delete();
        }

        else {

            var confirmWindow = new ConfirmWindow();

            //     confirmWindow.DisplayWariningIconImage();
            confirmWindow.Width = 450;

            confirmWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            confirmWindow.Height = 190;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.ShowWarningImage = true;
            confirmWindow.ShowNoButton
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.General.O.InvoiceRelatedPoiner"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.Delete();
                } else if (confirmWindow.No) {

                }
            });
        }
    }

    Delete() {
        if (!this.Parent.IsReadOnly) {
            var deletedItemPrice: number = this.entityPM.ItemPrice;
            if (this.Parent.EntityPM.SupplierInvoiceItems.includes(this.entityPM)) {
                if (this.ShowValidatioIcon == true) {
                    this.Parent.Parent.tariffErrorItems -= 1;
                }
                this.Parent.EntityPM.RemoveSupplierInvoiceItem(this.entityPM);
                var sequence;
                if (this.Parent.ItemsSource.Collection[0].SequenceNumeric == this.entityPM.SequenceNumeric) {
                    sequence = this.entityPM.SequenceNumeric;
                    this.Parent.ItemsSource.Remove(this, false); //the position of this line is important :after sequence is taken

                }
                else {
                    this.Parent.ItemsSource.Remove(this, false);//the position of this line is important before sequence is taken
                    sequence = this.Parent.ItemsSource.Collection[0].SequenceNumeric;
                }


                //var sortedCollection = this.Parent.ItemsSource.Collection.sort((a, b) => { return a.SequenceNumeric - b.SequenceNumeric });
                if (!this.Parent.declarationPM.IsAmendment) {
                    this.Parent.ItemsSource.Collection.forEach((item: SupplierInvoiceItemLine) => {
                        item.SequenceNumeric = sequence;
                        sequence++;
                    });
                }
                this.Parent.EntityPM.FullItemsCount = this.Parent.EntityPM.FullItemsCount - 1;
                this.Parent.EntityPM.FullChildrenCount--;
                this.Parent.ChildrenCount = "(" + this.Parent.EntityPM.FullChildrenCount + ")";
            }
            //this.Parent.ItemsSource.Remove(this.entityPM); 
            this.Parent.calculateTotals(true, deletedItemPrice);

        }
    }

    CertificateButtonClicked(item: SupplierInvoiceItemLine) {
        if (!AppTool.IsNullOrEmpty(item)) {
            var windowArgs: any = {};
            windowArgs.SupplierInvoiceItemPM = item.entityPM;
            windowArgs.IsDisplayOnly = this.Parent.IsReadOnly;
            var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
            windowArgs.allowExport = this.allowExport;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            if (item.ClassificationCode != null) {
                logWindow.Title = "םישורים לפרט מכס" + " " + item.ClassificationCode;
            }
            else {
                logWindow.Title = "םישורים לפרט מכס";
            }
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.SetCertificateStatusVisibility());
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
        }
    }

    EditItem(item: SupplierInvoiceItemLine) {
        if (!AppTool.IsNullOrEmpty(item)) {

            if (this.Parent.declarationPM.Direction == "E")
                this.allowExport = true;

            var windowArgs: any = {};
            windowArgs.SupplierInvoiceItemPM = item.entityPM;
            windowArgs.Parent = item;
            windowArgs.IsDisplayOnly = this.Parent.IsReadOnly;
            var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1010;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            windowArgs.allowExport = this.allowExport;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.SetStatusVisibility());
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/EditSupplierInvoiceItem');


        } else {
            console.log("[!] There is no item to open", item);
        }
    }


    SetStatusVisibility() {


        if (this.entityPM.ItemAdditionalStatus) {
            this.ItemAdditionalStatusVisibility = true;
        }

        else {

            this.ItemAdditionalStatusVisibility = false;
        }
    }

    public SetCertificateStatusVisibility() {


        if (this.entityPM.CertificatesStatusCode == "1") {
            this.OkVisiblity = true;
            this.WarningVisiblity = false;
            this.IsBlueBorderVisibile = false;
        }

        else if (this.entityPM.CertificatesStatusCode == "2") {
            this.WarningVisiblity = true;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = false;
        }



        else if (this.entityPM.CertificatesStatusCode == "3") {
            this.OkVisiblity = true;
            this.WarningVisiblity = false;
            this.IsBlueBorderVisibile = true;
        }
        else if (this.entityPM.CertificatesStatusCode == "4") {
            this.WarningVisiblity = true;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = true;
        }

        else {
            this.WarningVisiblity = false;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = false;
        }

    }

    ClassificationKeyUp(event, logCellTemplate: any, classificationTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnClassificationLostFocus(logCellTemplate, classificationTextBox);
        }
    }

    public static validateClassificationCode(classificationCode: any) {
        var checkDigit, digit;
        var newValue = classificationCode;
        let result = {
            valid: true,
            errorDescription: '',
            ClassificationCode: classificationCode
        };

        if (!AppTool.IsNullOrEmpty(classificationCode)) {
            if (newValue.toString().length > 11) {
                result.valid = false;
                result.errorDescription = TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong");
            }
            else if (newValue.toString().length < 8) {
                result.valid = false;
                result.errorDescription = TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort");

            }
            else if (newValue.toString().length == 8) {
                newValue = newValue + "00";
                checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
                newValue = newValue + checkDigit;
                result.valid = true;;

            }
            else if (newValue.toString().length == 9) {
                digit = newValue.toString().substring(8);

                newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
                checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));

                if (digit != checkDigit.toString()) {
                    result.valid = false;
                    result.errorDescription = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString();

                }
            }
            else if (newValue.toString().length == 10) {
                checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
                newValue = newValue + "" + checkDigit;
            }
            else if (newValue.toString().length == 11) {
                digit = newValue.toString().substring(10);
                checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
                if (digit != checkDigit.toString()) {
                    result.valid = false;
                    result.errorDescription = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString();

                }
            }
        }
        result.ClassificationCode = newValue;
        return result;
    }


    async ClassificationCodeChanged(logCellTemplate: any, classificationTextBox: any) {
        if (this.valid) {
            SessionLocator.SustainFocusOnCell = false;

            if (!AppTool.IsNullOrEmpty(this.ItemCode)) {

                //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponentByDirection(this.ItemCode, this.Parent.declarationPM.Direction);//.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails == null) {
                    this.AdditemCodeDetail();

                }
                else {

                    if (itemCodeDetails.ClassificationCode != this.ClassificationCode || itemCodeDetails.ItemDescription != this.ItemDescription) {
                        itemCodeDetails.ClassificationCode = this.ClassificationCode;
                        itemCodeDetails.ItemDescription = this.ItemDescription;
                        itemCodeDetails.VendorNumber = this.Parent.vendorNumber;
                        if (GITITEMCacheService.Instance.IsUnitPURForItems) {

                            itemCodeDetails.InvoiceQuantityType = this.InvoiceQuantityType;
                        }
                        if (GITITEMCacheService.Instance.IsCountryPURForItems) {
                            itemCodeDetails.OriginCountryCode = this.OriginCountryCode;
                            itemCodeDetails.OriginCountryName = this.OriginCountryName;
                            itemCodeDetails.TariffID = this.TradeAgreementCode;
                        }
                        for (let inner of this.GITITEMCRs) {
                            itemCodeDetails.GITITEMCRs.push(new GITITEMCR(inner.COUNTER, inner.REQCERT, inner.REMARKS));
                            var exist = this.entityPM.SupplierInvioceItemCertificats.filter(d => d.CertificateNumber == inner.REQCERT.replace(/^0+/, ''))[0];
                            if (!exist) {
                                var SupplierInvioceItemCertificat: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(this.entityPM);
                                SupplierInvioceItemCertificat.DeclarationId = this.entityPM.DeclarationId;
                                SupplierInvioceItemCertificat.InvoiceCounterKey = this.entityPM.CounterKey;
                                SupplierInvioceItemCertificat.LineNumber = this.entityPM.LineNumber;
                                SupplierInvioceItemCertificat.Tenant = this.entityPM.Tenant;
                                //SupplierInvioceItemCertificat.CertificateNumber = item.REQCERT;
                                SupplierInvioceItemCertificat.ReqConfirmationTypeCode = inner.REQCERT.replace(/^0+/, '');
                                this.entityPM.AddSupplierInvioceItemCertificat(SupplierInvioceItemCertificat);
                                var multiCertificateUpdateComponent: MultiCertificateUpdateComponent = new MultiCertificateUpdateComponent();
                                multiCertificateUpdateComponent.UpdateCertStatusAlaaMethod(SupplierInvioceItemCertificat, this.entityPM);
                            }
                        }
                        this.SetCertificateStatusVisibility();
                        itemCodeDetails.IsNew = true;
                        itemCodeDetails.GITITEMCRs = (this.GITITEMCRs);
                    }
                }
            }
            this.GetQuantityType();
            logCellTemplate.IsDisplayMode = true;
            //logCellTemplate.IsEditMode = false;          
        }
        else {
            //var element = document.getElementById(logCellTemplate.OuterDivId);
            // element.focus();
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: classificationTextBox.InputId });

        }
        if (this.Parent.IsChecked) {

            if (this.InvoiceQuantityType == null && this.QunatityTypeCode != null) {
                var s = this.QunatityTypeCode.slice(1, this.QunatityTypeCode.length - 1);


                this.InvoiceQuantityType = s;

            }



        }
    }

    async OnClassificationLostFocus(logCellTemplate: any, classificationTextBox: any) {
        var res = SupplierInvoiceItemLine.validateClassificationCode(this.ClassificationCode);
        if (res.valid) {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }
        else {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, res.errorDescription);
        }
        this.ClassificationCode = res.ClassificationCode;
        classificationTextBox.TextValue = this.ClassificationCode;
        this.valid = res.valid;
        this.ClassificationCodeChanged(logCellTemplate, classificationTextBox);
    }

    private AdditemCodeDetail() {
        var originCountryCode: string = null;
        var originCountryName: string = null;
        var tariffID: string = null;
        var invoiceQuantityType: string = null;
        if ( /*this.Parent*/GITITEMCacheService.Instance.IsCountryPURForItems) {
            originCountryCode = this.OriginCountryCode;
            originCountryName = this.OriginCountryName;
            //tariffID = this.TradeAgreementCode;
        }
        if (GITITEMCacheService.Instance.IsUnitPURForItems) {

            invoiceQuantityType = this.InvoiceQuantityType;
        }
        tariffID = this.TradeAgreementCode
        //this.Parent.Parent.ItemCode_LocalCache.push(new ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, this.InvoiceQuantityType));
        GITITEMCacheService.Instance. /*ItemCode_LocalCache.push*/AddItemCodeComponent(
            new ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, invoiceQuantityType, this.Parent.declarationPM.CustomerCode, tariffID, this.GITITEMCRs, this.Parent.declarationPM.Direction));
    }

    OnOriginCountryCodeLostFocus(logCellTemplate: any, originCountryCodeLov: any) {
        if (!/*this.Parent*/GITITEMCacheService.Instance.IsCountryPURForItems) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.ItemCode)) {
            //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode);//.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];

            if (itemCodeDetails != null) {
                itemCodeDetails.OriginCountryCode = this.OriginCountryCode != null ? this.OriginCountryCode : this.customsCountry != null ? this.customsCountry.Code : null;
                itemCodeDetails.OriginCountryName = this.OriginCountryName != null ? this.OriginCountryName : this.customsCountry != null ? this.customsCountry.LocalName : null;
                itemCodeDetails.IsNew = true;
            }
        }
    }

    ItemCodeLostFocus(logCellTemplate: LogCellTemplateComponent) {
        if (!this.entityPM.ClassificationCode) this.entityPM.ClassificationCode = "";
        if (!this.entityPM.ItemDescription) this.entityPM.ItemDescription = "";

        if (AppTool.IsNullOrEmpty(this.ItemCode)) {
            return;
        }
        //if (this.Parent.Parent.ItemCode_LocalCache != null && this.Parent.Parent.ItemCode_LocalCache.length > 0) {
        //if (GITITEMCacheService.Instance.ItemCode_LocalCache != null && GITITEMCacheService.Instance.ItemCode_LocalCache.length > 0)
        {
            if (this.Parent.declarationPM.Direction != "E") {
                //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService.Instance.FirstItemCodeComponentByDirection(this.ItemCode, this.Parent.declarationPM.Direction);//.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails != null) {
                    let b = true;
                    if (b) {
                        GITITEMCacheService.Instance.OnItemCodeAdd(this.entityPM, itemCodeDetails)
                            .then(myOnItemCodeAddResult => {
                                switch (myOnItemCodeAddResult) {

                                    case 2/*OnItemCodeAddResult.AddTaskToUpdateDB*/:
                                        {
                                            GITITEMCacheService.Instance./*ItemCode_LocalCache.push*/AddItemCodeComponent(
                                                new ItemCodeComponent(
                                                    this.ItemCode,
                                                    this.ClassificationCode,
                                                    this.ItemDescription,
                                                    this.Parent.vendorNumber,
                                                    this.OriginCountryCode,
                                                    this.OriginCountryName, true,
                                                    this.InvoiceQuantityType,
                                                    this.Parent.declarationPM.CustomerCode,
                                                    this.TariffID,
                                                    this.GITITEMCRs,
                                                    this.Parent.declarationPM.Direction
                                                )
                                            );
                                        }
                                        break;
                                    //case OnItemCodeAddResult.OverwriteRowFromDB:
                                    //case OnItemCodeAddResult.voidDoNothing:
                                    default:
                                        {
                                            itemCodeDetails.IsNew = true;
                                            this.ClassificationCode = itemCodeDetails.ClassificationCode;
                                            this.ItemDescription = itemCodeDetails.ItemDescription;
                                            if (GITITEMCacheService.Instance.IsUnitPURForItems) {

                                                this.InvoiceQuantityType = itemCodeDetails.InvoiceQuantityType;
                                            }
                                            if (/*this.Parent*/GITITEMCacheService.Instance.IsCountryPURForItems) {
                                                this.OriginCountryCode = itemCodeDetails.OriginCountryCode;
                                                this.OriginCountryName = itemCodeDetails.OriginCountryName;
                                                this.TradeAgreementCode = itemCodeDetails.TariffID;
                                            }
                                            //this.entityPM.AddSupplierInvioceItemCertificat()

                                            for (let item of itemCodeDetails.GITITEMCRs) {
                                                var exist = this.entityPM.SupplierInvioceItemCertificats.filter(d => d.CertificateNumber == item.REQCERT.replace(/^0+/, ''))[0];
                                                if (!exist) {
                                                    var SupplierInvioceItemCertificat: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(this.entityPM);
                                                    SupplierInvioceItemCertificat.DeclarationId = this.entityPM.DeclarationId;
                                                    SupplierInvioceItemCertificat.InvoiceCounterKey = this.entityPM.CounterKey;
                                                    SupplierInvioceItemCertificat.LineNumber = this.entityPM.LineNumber;
                                                    SupplierInvioceItemCertificat.Tenant = this.entityPM.Tenant;
                                                    //SupplierInvioceItemCertificat.CertificateNumber = item.REQCERT;
                                                    SupplierInvioceItemCertificat.ReqConfirmationTypeCode = item.REQCERT.replace(/^0+/, '');
                                                    this.entityPM.AddSupplierInvioceItemCertificat(SupplierInvioceItemCertificat);
                                                    var multiCertificateUpdateComponent: MultiCertificateUpdateComponent = new MultiCertificateUpdateComponent();
                                                    multiCertificateUpdateComponent.UpdateCertStatusAlaaMethod(SupplierInvioceItemCertificat, this.entityPM);
                                                }
                                            }
                                            this.GITITEMCRs = itemCodeDetails.GITITEMCRs;
                                            this.SetCertificateStatusVisibility();
                                            this.GetQuantityType();
                                        }
                                        break;
                                }
                                return;
                            });
                    }




                    return;
                }
            }
            else if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR")) {
                if (AppTool.IsNullOrEmpty(this.entityPM.ClassificationCode) && !AppTool.IsNullOrEmpty(this.Parent.declarationPM.ExporterImporterCode) && !AppTool.IsNullOrEmpty(this.ItemCode)) {
                    var clientItemExtendedPMService: ClientItemExtendedPMService = new ClientItemExtendedPMService();
                    clientItemExtendedPMService.GetClientItemPM(this.ItemCode, this.Parent.declarationPM.ExporterImporterCode, SessionLocator.Tenant)
                        .subscribe((response: ServiceResponse) => {
                            if (response?.Result) {
                                var clientItemPM: ClientItemPM = response.Result;
                                this.entityPM.ClassificationCode = clientItemPM?.ClassificationCode;
                                if (AppTool.IsNullOrEmpty(this.entityPM.ItemDescription))
                                    this.entityPM.ItemDescription = clientItemPM?.ItemDescription;
                                if (AppTool.IsNullOrEmpty(this.entityPM.OriginCountryCode)) {
                                    this.entityPM.OriginCountryCode = clientItemPM?.OriginCountryCode;
                                    this.entityPM.OriginCountryName = clientItemPM?.OriginCountryName;
                                }
                                if(!AppTool.IsNullOrEmpty(this.entityPM.ClassificationCode))
                                {
                                    this.Parent.quantityTypeMessageService.GetQuantityType(this.entityPM.ClassificationCode, this.Parent.declarationPM.Direction === 'E').subscribe((myServiceResponse: ServiceResponse) => {
                                        if (!myServiceResponse.HasError && myServiceResponse.Result != null) {
                                            this.entityPM.InvoiceQuantityType = myServiceResponse.Result;
                                            this.QunatityTypeCode = "(" + myServiceResponse.Result + ")";
                                           
                                        }
                    
                    
                    
                                    });
                                }


                            }

                        })
                }
            }

        }

        this.declarationWebService.GetGITITEMPartnersItemListByItemCode(this.Parent.vendorNumber, this.Parent.declarationPM.CustomerCode, this.ItemCode, 30, false)
            .subscribe((response: ServiceResponse) => {
                var res = response.Result;
                if (!AppTool.IsNullOrEmpty(res) && res.length == 1) {
                    this.PartnerItemsSelectionCompleted(this.entityPM, res[0]);
                    return;
                }
                else {
                    this.declarationWebService.GetGITITEMPartnersItemListByItemCode(this.Parent.vendorNumber, this.Parent.declarationPM.CustomerCode, this.ItemCode, 30, true)
                        .subscribe((response: ServiceResponse) => {
                            var res = response.Result;
                            if (!AppTool.IsNullOrEmpty(res) && res.length == 1) {
                                this.PartnerItemsSelectionCompleted(this.entityPM, res[0]);
                                return;
                            }
                            else {
                                this.declarationWebService.GetGITITEMPartnersItemListByName(this.Parent.vendorNumber, this.Parent.declarationPM.CustomerCode, this.ItemCode, 30)
                                    .subscribe((response: ServiceResponse) => {
                                        var res = response.Result;
                                        if (!AppTool.IsNullOrEmpty(res) && res.length == 1) {
                                            this.PartnerItemsSelectionCompleted(this.entityPM, res[0]);
                                            return;
                                        } else {
                                            //Eitancommented 15 minutes ago
                                            //@odelia devashi @itzik M סיכום:
                                            //גם כםשר מזינים קודם פרט מכס וםח"כ קוד פריט (מקט), עדיין צריך ליצור TASK של לימוד עצמי + שימוש ב-CACHE ברמת SESSION
                                            if (!AppTool.IsNullOrEmpty(this.ClassificationCode)) {
                                                this.AdditemCodeDetail();//Task 43218: שיפור במנגנון לימוד עצמי
                                            }
                                        }

                                    });
                            }
                        });
                }
            });
    }

    ItemCodeDblClick(logCellTemplate: LogCellTemplateComponent) {
        if (!this.Parent.IsReadOnly) {
            console.log("[Double Click] ", this.entityPM);

            if (this.Parent.IsDisplayOnly)
                return;

            //close the cell before showing window; to avoid [true] to [false] problem
            logCellTemplate.IsDisplayMode = true;
            logCellTemplate.IsEditMode = false;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 650;
            logWindow.Title = TextCodeTranslator.Translate("Customs.CustomsPartnersItem.Q.ItemQuery");
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = {
                invoicePM: this.Parent.EntityPM,
                customerCode: this.Parent.declarationPM.CustomerCode,
                searchText: this.ItemCode,
            };
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.PartnerItemsSelectionCompleted(this.entityPM, $event);
            });

            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/PartnersItemsSelectionComponent');
        }
    }
    ClassificationCodeDblClick(logCellTemplate: LogCellTemplateComponent, ClassificationTextBox) {

        if (this.Parent.declarationPM.Direction != "E" || !AmitalGatewayUtil.Instance.AmitalBrowserInUse || this.Parent.declarationPM.IsConnectedToUnifreight) return;

        if (!this.Parent.IsReadOnly) {
            console.log("[Double Click] ", this.entityPM);

            if (this.Parent.IsDisplayOnly)
                return;

            //close the cell before showing window; to avoid [true] to [false] problem
            logCellTemplate.IsDisplayMode = true;
            logCellTemplate.IsEditMode = false;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 650;
            logWindow.Title = TextCodeTranslator.Translate("Customs.CustomsPartnersItem.Q.ItemQuery");
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = {

                searchText: this.ClassificationCode,
                customFileNo: this.Parent.declarationPM.CustomFileNo,
                declarationId: this.Parent.declarationPM.Id,

            };
            logWindow.WindowClosed.subscribe(($event: any) => {

                this.PartnerItemsDescreptionSelectionCompleted(this.entityPM, $event, logCellTemplate, ClassificationTextBox);

            });

            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/PartnersItemsDescreptionSelectionComponent');
        }
    }
    PartnerItemsDescreptionSelectionCompleted(item, partnersItem, logcelltemplate, ClassificationTextBox) {
        if (!AppTool.IsNullOrEmpty(partnersItem)) {
            console.log("Response returned: ", partnersItem);
            if (!AppTool.IsNullOrEmpty(partnersItem.ItemCode) || !AppTool.IsNullOrEmpty(partnersItem.ClassificationCode)) {
                item.ClassificationCode = partnersItem.ClassificationCode;
                item.ItemDescription = partnersItem.ItemDescription;
                logcelltemplate.IsDisplayMode = false;
                logcelltemplate.IsEditMode = true;
                this.OnClassificationLostFocus(logcelltemplate, ClassificationTextBox)
            }
        }
    }
    PartnerItemsSelectionCompleted(item, partnersItem) {

        if (!AppTool.IsNullOrEmpty(partnersItem)) {
            console.log("Response returned: ", partnersItem);
            if (!AppTool.IsNullOrEmpty(partnersItem.ItemCode) || !AppTool.IsNullOrEmpty(partnersItem.ClassificationCode)) {
                let b = true;
                if (b) {
                    GITITEMCacheService.Instance.OnItemCodeAdd(this.entityPM, partnersItem)
                        .then(myOnItemCodeAddResult => {
                            switch (myOnItemCodeAddResult) {

                                case 2: //OnItemCodeAddResult.AddTaskToUpdateDB:
                                    {
                                        GITITEMCacheService.Instance./*ItemCode_LocalCache.push*/AddItemCodeComponent(
                                            new ItemCodeComponent(
                                                item.ItemCode,
                                                item.ClassificationCode,
                                                item.ItemDescription,
                                                this.Parent.vendorNumber,
                                                item.OriginCountryCode,
                                                item.OriginCountryName, true,
                                                item.InvoiceQuantityType,
                                                this.Parent.declarationPM.CustomerCode,
                                                item.TariffID,
                                                item.GITITEMCRs,
                                                this.Parent.declarationPM.Direction

                                            )
                                        );
                                        this.GetQuantityType();

                                    }
                                    break;
                                //case OnItemCodeAddResult.OverwriteRowFromDB:
                                //case OnItemCodeAddResult.voidDoNothing:
                                default:
                                    {
                                        item.ItemCode = partnersItem.ItemCode;
                                        item.ClassificationCode = partnersItem.ClassificationCode;
                                        item.ItemDescription = partnersItem.Name;
                                        if (/*this.Parent*/GITITEMCacheService.Instance.IsCountryPURForItems) {
                                            item.OriginCountryCode = partnersItem.OriginCountryCode;
                                            item.OriginCountryName = partnersItem.OriginCountryName;
                                        }
                                        if (GITITEMCacheService.Instance.IsUnitPURForItems) {

                                            item.InvoiceQuantityType = partnersItem.InvoiceQuantityType;
                                        }
                                        partnersItem.GITITEMCRs.
                                            forEach((itm: GITITEMCR) => {
                                                //item.GITITEMCRs.push(itm);
                                                // item.GITITEMCRs.push(new GITITEMCR(itm.COUNTER, itm.REQCERT, itm.REMARKS));
                                            });
                                        for (let itm of partnersItem.GITITEMCRs) {
                                            // item.GITITEMCRs.push(new GITITEMCR(itm.COUNTER, itm.REQCERT, itm.REMARKS));
                                            var exist = this.entityPM.SupplierInvioceItemCertificats.filter(d => d.CertificateNumber == itm.REQCERT.replace(/^0+/, ''))[0];
                                            if (!exist) {
                                                var SupplierInvioceItemCertificat: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(this.entityPM);
                                                SupplierInvioceItemCertificat.DeclarationId = this.entityPM.DeclarationId;
                                                SupplierInvioceItemCertificat.InvoiceCounterKey = this.entityPM.CounterKey;
                                                SupplierInvioceItemCertificat.LineNumber = this.entityPM.LineNumber;
                                                SupplierInvioceItemCertificat.Tenant = this.entityPM.Tenant;
                                                //SupplierInvioceItemCertificat.CertificateNumber = item.REQCERT;
                                                SupplierInvioceItemCertificat.ReqConfirmationTypeCode = itm.REQCERT.replace(/^0+/, '');
                                                this.entityPM.AddSupplierInvioceItemCertificat(SupplierInvioceItemCertificat);
                                                var multiCertificateUpdateComponent: MultiCertificateUpdateComponent = new MultiCertificateUpdateComponent();
                                                multiCertificateUpdateComponent.UpdateCertStatusAlaaMethod(SupplierInvioceItemCertificat, this.entityPM);
                                            }
                                        }
                                        item.TariffID = partnersItem.TariffID;
                                        item.GITITEMCRs = partnersItem.GITITEMCRs;
                                        this.SetCertificateStatusVisibility();
                                        //this.Parent.Parent.ItemCode_LocalCache.push(new ItemCodeComponent(partnersItem.ItemCode, partnersItem.ClassificationCode, partnersItem.Name, this.Parent.vendorNumber, item.OriginCountryCode, item.OriginCountryName, false, item.InvoiceQuantityType));
                                        GITITEMCacheService.Instance./*ItemCode_LocalCache.push*/AddItemCodeComponent(new ItemCodeComponent(partnersItem.ItemCode, partnersItem.ClassificationCode, partnersItem.Name, this.Parent.vendorNumber, item.OriginCountryCode, item.OriginCountryName, false, item.InvoiceQuantityType, this.Parent.declarationPM.CustomerCode, item.TariffID, item.GITITEMCRs, this.Parent.declarationPM.Direction));
                                        this.GetQuantityType();

                                        //item.IsNew = true;
                                        //item.GITITEMCRs.push.apply(item.GITITEMCRs, partnersItem.GITITEMCRs) ;
                                    }
                                    break;
                            }
                            return;
                        });
                }
            }
        }
    }

    VehicleButtonClicked(item: SupplierInvoiceItemLine) {
        if (!AppTool.IsNullOrEmpty(item)) {
            var windowArgs: any = {};
            windowArgs.SupplierInvoiceItemPM = item.entityPM;
            windowArgs.IsDisplayOnly = this.Parent.IsReadOnly;
            windowArgs.declarationPM = this.Parent.declarationPM;
            windowArgs.parent = this.Parent;
            windowArgs.SupplierInvoicePM = this.Parent.EntityPM;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 500;
            logWindow.Title = TextCodeTranslator.Translate("General.MH.Vehicles");
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.SetVehicleStatusVisibility());
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemVehicleComponent');
        }
    }

    SetVehicleStatusVisibility() {

        if (this.entityPM.VehicleStatus) {
            this.VehicleOkVisiblity = true;
        }
        else {
            this.VehicleOkVisiblity = false;
        }

    }

    CopyLineClicked(copieditem: SupplierInvoiceItemLine, ItemsSource: ObservableCollection) {

        var entityPM: SupplierInvoicePM = this.Parent.EntityPM;
        if (this.Parent.IsDisplayOnly)
            return; // go back -_-

        var line: number = 0;
        var sequence: number = 0;
        var orderByLineNo: string;
        if (entityPM.SupplierInvoiceItems.length > 0) {
            if (isNaN(entityPM.InvoiceItemLastLineNumber)) entityPM.InvoiceItemLastLineNumber = 0;
            line = entityPM.InvoiceItemLastLineNumber;
            entityPM.InvoiceItemLastLineNumber = entityPM.InvoiceItemLastLineNumber + 1;
        }

        var copiedFromOrderByLineNo = copieditem.entityPM.OrderByLineNo;
        var lastCopyOrderByLineNo = copieditem.entityPM.LastCopyFromOrderNo;
        var lastCopyItem = this.Parent.ItemsSource.Collection.filter(d => d.OrderByLineNo == lastCopyOrderByLineNo)[0];
        orderByLineNo = this.GetNextOrderByLineNumber(copiedFromOrderByLineNo, lastCopyOrderByLineNo);

        line += 1;

        sequence = copieditem.SequenceNumeric;// + 1;

        var item: SupplierInvoiceItemPM = new SupplierInvoiceItemPM(entityPM);
        item.DeclarationId = entityPM.DeclarationId;
        item.CounterKey = entityPM.InvoiceCounterKey;
        item.Tenant = entityPM.Tenant;
        item.LineNumber = line;//entityPM.InvoiceItemLastLineNumber;
        item.SequenceNumeric = sequence;
        item.OrderByLineNo = orderByLineNo;
        item.ClassificationCode = copieditem.ClassificationCode;
        item.InvoiceQuantityType = copieditem.InvoiceQuantityType;
        item.OriginCountryCode = copieditem.OriginCountryCode;
        item.OriginCountryName = copieditem.OriginCountryName;
        item.TradeAgreementCode = copieditem.TradeAgreementCode;
        item.TradeAgreementName = copieditem.TradeAgreementName;
        item.IsCopy = true;
        entityPM.FullChildrenCount++;
        copieditem.entityPM.LastCopyFromOrderNo = orderByLineNo;
        this.Parent.ChildrenCount = "(" + entityPM.FullChildrenCount + ")";


        if (!entityPM.SupplierInvoiceItems.includes(item)) {
            var copiedItemIndex: number;
            copiedItemIndex = this.Parent.ItemsSource.GetIndex(copieditem);
            //if (lastCopyItem) {
            //    copiedItemIndex = this.Parent.ItemsSource.GetIndex(lastCopyItem);
            //}
            //else {
            //    copiedItemIndex = this.Parent.ItemsSource.GetIndex(copieditem);
            //}
            entityPM.AddSupplierInvoiceItem(item);
            this.Parent.ItemsSource.InsertAtIndex(copiedItemIndex + 1, new SupplierInvoiceItemLine(item, this.Parent, false));
            //this.Parent.ItemsSource.Collection.push(new SupplierInvoiceItemLine(item, this.Parent));//Insert(new SupplierInvoiceItemLine(item, this.Parent), false);
            if (isNaN(entityPM.FullItemsCount)) entityPM.FullItemsCount = 0;

            entityPM.FullItemsCount = entityPM.FullItemsCount + 1;
            entityPM.MaxSequence = entityPM.MaxSequence + 1;
        }
        var sequenceNo = this.Parent.ItemsSource.Collection[0].SequenceNumeric;
        var sortedCollection = this.Parent.ItemsSource.Collection.sort((a, b) => { return a.SequenceNumeric - b.SequenceNumeric || a.LineNumber - b.LineNumber });
        sortedCollection.forEach((item: SupplierInvoiceItemLine) => {
            this.Parent.ItemsSource.Collection.filter(d => d.LineNumber == item.LineNumber)[0].SequenceNumeric = sequenceNo;
            sequenceNo++;
        });
        //this.Parent.ItemsSource.Collection.forEach((item: SupplierInvoiceItemLine) => {
        //    item.SequenceNumeric = sequence;
        //    sequence++;
        //});
        //this.Parent.ItemsSource.InsertCollection(sortedCollection);
        this.Parent.calculateTotals(false, null);

    }

    GetNextOrderByLineNumber(copiedOrderByLineNo: string, lastCopyOrderByLineNo: string) {

        var newCopyOrderbyNo: string = "";

        if (AppTool.IsNullOrEmpty(lastCopyOrderByLineNo)) {
            newCopyOrderbyNo = copiedOrderByLineNo + ".1";
        }
        else {
            var lastcopyLineNoparts: string[] = lastCopyOrderByLineNo.split('.');
            var incremetedPart: number;
            var compinedLastPart: number;
            var lastpartLastNo: string = lastcopyLineNoparts[lastcopyLineNoparts.length - 1];
            var lastPartLastNoLastNo: string = lastpartLastNo.substring(lastpartLastNo.length - 1);

            incremetedPart = Number(lastPartLastNoLastNo);
            compinedLastPart = Number(lastpartLastNo);
            var addedResult: string = "";
            if (incremetedPart != 9) {
                addedResult = (compinedLastPart + 1).toString();

            }
            else {
                addedResult = lastpartLastNo + "1";
            }

            for (var i = 0; i < lastcopyLineNoparts.length - 1; i++) {
                newCopyOrderbyNo = newCopyOrderbyNo + "." + lastcopyLineNoparts[i];
            }
            newCopyOrderbyNo = newCopyOrderbyNo + "." + addedResult;
            newCopyOrderbyNo = newCopyOrderbyNo.substr(1, newCopyOrderbyNo.length - 1);
        }

        return newCopyOrderbyNo;
    }

    Dispose() {
        this.Parent = null;
        this.DataContext = null;


        if (this.QuantityTypeCodeLoaded) {
            this.QuantityTypeCodeLoaded.unsubscribe();
        }

    }


    // timer?? because function OnSelectedItemChanged() hit before after these functions
    CloseButtonClicked() {
        var t = setTimeout(() => {
            this.closedManullay = true;
            this.ShowClassifierRemarkTooltip = false;
        }, 20);
    }

    ExcButtonClicked() {
        var t = setTimeout(() => {
            this.closedManullay = false;
            this.ShowClassifierRemarkTooltip = true;
        }, 20);
    }

    TariffErrorButtonClicked() {
        this.ShowTariffErrorTooltip = !this.ShowTariffErrorTooltip;
    }

    CheckTariff() {
        /*if (this.TradeAgreementCode && this.OriginCountryCode) {
            this.ShowTariffErrorInfo = (this.CustomsCountry.TarriffCode != this.TradeAgreementCode);
        } else {
            this.ShowTariffErrorInfo = false;
        }*/
        if (!AppTool.IsNullOrEmpty(this.CustomsCountry) &&
            this.OriginCountryName && this.CustomsCountry.TarriffCode && this.CustomsCountry.TarriffCode != this.TradeAgreementCode) {
            if (this.ShowValidatioIcon != true) {
                this.ShowValidatioIcon = true;
                this.Parent.Parent.tariffErrorItems += 1;
            }
            var agreementCode = !AppTool.IsNullOrEmpty(this.TradeAgreementCode) ? this.TradeAgreementCode : "לם מוזן";
            this.TariffErrorText = "קוד הסכם " + agreementCode + ", לם מתםים למדינה " + this.OriginCountryName + " (" + " הסכם " + this.CustomsCountry.TarriffCode + " )";

        }
        else {
            if (this.ShowValidatioIcon == true) {
                this.ShowValidatioIcon = false;
                this.Parent.Parent.tariffErrorItems -= 1;
            }
        }
    }
}

export class ModificationItemModel extends BaseComponent {
    public EntityPM: SupplierInvoiceModificationPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceModification";
    public DataContext = this;
    isValid: boolean;
    public customsExchangeRateExtendedPMService: CustomsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService();
    public code: string;
    constructor(private modificationPM: SupplierInvoiceModificationPM, private parent: SupplierInvoiceGeneralTabComponent, code: string) {
        super();
        this.EntityPM = modificationPM;
        this.code = code;
        this.isValid = true;
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.parent.ExportModificationCurrency, this.parent.declarationPM.TaxationDateTime).subscribe((response: any) => {
            if (response) {
                if (response.Result) {
                    var rate = response.Result[0];
                    if (rate) {
                        this.InvoiceCurrencyExchangeRtae = rate.ExchangeRate;
                    }
                }
            }
        });
        if (this.EntityPM.Amount != null) {
            this.parent.CalculateExportModificationAmount();
            this.DeleteButton = true;
        }
        this.EntityPM.IsDirty = false;
        this.UIProperties.SetEnabled("CurrencyTypeCode", this.ObjectTableName, !this.parent.IsDisplayOnly);
        this.UIProperties.SetEnabled("Amount", this.ObjectTableName, !this.parent.IsDisplayOnly);

    }

    setSupplierInvoiceFreightAmountPM() {
        var item: SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(this.parent.EntityPM);
        item.DeclarationId = this.parent.EntityPM.DeclarationId;
        item.InvoiceCounterKey = this.parent.EntityPM.InvoiceCounterKey;
        item.Tenant = this.parent.EntityPM.Tenant;
        item.ChangeSetOp = "Insert";
        this.parent.EntityPM.AddSupplierInvoiceFreightAmount(item);
        this.parent.EntityPM.IsDirty = false;
    }

    setSupplierInvoiceFreightAmountPMWithCode(code: string) {
        if (code != null) {
            var item: SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(this.parent.EntityPM);
            item.DeclarationId = this.parent.EntityPM.DeclarationId;
            item.InvoiceCounterKey = this.parent.EntityPM.InvoiceCounterKey;
            item.Tenant = this.parent.EntityPM.Tenant;
            item.CurrencyTypeCode = code;
            item.ChangeSetOp = "Insert";
            item.Amount = this.Amount;
            this.parent.EntityPM.AddSupplierInvoiceFreightAmount(item);
            this.parent.AmountList.Insert(new SupplierInvoiceFreightAmountLine(item, this.parent));
        }
    }

    deletSupplierInvoiceFreightAmountPMWithCode(code: string) {
        if (code != null) {
            var OldCurrency = this.parent.EntityPM.SupplierInvoiceFreightAmounts.find(d => d.DeclarationId == this.parent.EntityPM.DeclarationId).CurrencyTypeCode;
            var OldPM = this.parent.EntityPM.SupplierInvoiceFreightAmounts.filter(item => item.CurrencyTypeCode == code);
            if (OldPM[0] != null) {
                this.parent.EntityPM.RemoveSupplierInvoiceFreightAmount(OldPM[0]);
                this.parent.AmountList.Clear();
            }
        }
    }

    AddFreightToLists() {
        if (this.parent.EntityPM.SupplierInvoiceFreightAmounts[0] == null) {
            this.setSupplierInvoiceFreightAmountPM();
            if (this.parent.AmountList.Length == 0) {
                this.parent.AmountList.Insert(new SupplierInvoiceFreightAmountLine(this.parent.EntityPM.SupplierInvoiceFreightAmounts[0], this.parent));
            }
        }
    }
    //#region Properties
    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            //this.ModificationPM.TypeCode = value;

            if (value == "I02") {
                this.EntityPM.TypeCode = value;
                this.isValid = false;
                var errors = [];
                errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee"));
            } else {
                var exists;
                if (this.parent.AdjustmentsList.Length != 0) {
                    exists = this.parent.EntityPM.SupplierInvoiceModifications.find(d => d.TypeCode == value);
                }
                if (exists) {
                    this.EntityPM.TypeCode = value;
                    this.isValid = false;
                    var errors = [];
                    errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
                } else {
                    this.EntityPM.TypeCode = value;
                    //FirePropertyChanged("TypeCode");
                    this.isValid = true;
                    var errors = [];
                }

            }
            this.EntityPM.IsDirty = false;

        }
    }

    currencyType: CurrencyTypePM;
    get CurrencyType() { return this.currencyType; }
    set CurrencyType(value: CurrencyTypePM) {
        if (this.currencyType != value && !AppTool.IsNullOrEmpty(value)) {
            this.DeleteButton = true;
            this.currencyType = value;
            /* if (this.code == "67") {
                this.parent.InsruanceCurrencyTypeCode = value.Code;
            }
            if (this.code == "144") {
                this.AddFreightToLists();
                var OldCurrency = this.parent.EntityPM.SupplierInvoiceFreightAmounts.find(d => d.DeclarationId == this.parent.EntityPM.DeclarationId).CurrencyTypeCode;
                if (OldCurrency != null && OldCurrency != value.Code) {
                    this.deletSupplierInvoiceFreightAmountPMWithCode(OldCurrency);
                    this.setSupplierInvoiceFreightAmountPMWithCode(value.Code);
                }
                if (OldCurrency == null) {
                    this.parent.EntityPM.SupplierInvoiceFreightAmounts.find(d => d.DeclarationId == this.parent.EntityPM.DeclarationId).CurrencyTypeCode = value.Code;
                }
                this.parent.FreightCurrencyTypeCode = value.Code;
            }*/
            this.CurrencyTypeCode = value.Code;
            this.CurrencyTypeName = value.LocalName;
            this.parent.CalculateExportModificationAmount();
        }
        if (AppTool.IsNullOrEmpty(value)) {
            this.CurrencyTypeName = null;
            this.CurrencyTypeCode = null;
            this.parent.FreightCurrencyTypeCode = null;
        }
    }

    get TypeName() { return this.EntityPM.TypeName; }
    set TypeName(value: string) {
        if (this.EntityPM.TypeName != value) {
            this.EntityPM.TypeName = value;
        }
    }

    get CurrencyTypeCode() { return this.EntityPM.CurrencyTypeCode; }
    set CurrencyTypeCode(value: string) {
        if (this.EntityPM.CurrencyTypeCode != value) {
            this.EntityPM.CurrencyTypeCode = value;

        }
    }

    get CurrencyTypeName() { return this.EntityPM.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.EntityPM.CurrencyTypeName != value) {
            this.EntityPM.CurrencyTypeName = value;

        }
    }

    get Amount() { return this.EntityPM.Amount; }
    set Amount(value: number) {
        if (this.EntityPM.Amount != value && !AppTool.IsNullOrEmpty(value)) {
            this.DeleteButton = true;
            this.EntityPM.Amount = value;
            this.parent.CalculateExportModificationAmount();
            /*if (this.code == "67") {
                this.parent.InsuranceAmount = value;
            }
            if (this.code == "144") {
                this.AddFreightToLists();
                this.parent.EntityPM.TotalFreightInFreightCurrency = value;
                this.parent.TotalFreightInFreightCurrency = value;
                this.parent.EntityPM.IsDirty = true;
                if (this.parent.EntityPM.SupplierInvoiceFreightAmounts[0] != null) {
                    this.parent.EntityPM.SupplierInvoiceFreightAmounts[0].Amount = value;
                    this.parent.AmountList.Collection[0].Amount = value;
                }
            }*/
        }
        if (AppTool.IsNullOrEmpty(value)) {
            this.EntityPM.Amount = null;
        }

    }
    doCalculate: boolean = false;

    OriginalText: string;
    AmountOriginalText(originalText: string) {
        this.OriginalText = originalText;
        if (this.OriginalText) {
            if ((this.OriginalText + "").indexOf('%') > -1) {
                this.doCalculate = true;
            }
        }

        this.OriginalText = null;
    }

    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
    DiscountInNIS: number = 0;
    InvoiceCurrencyExchangeRtae: number = 0;
    DiscountInDsicCurrency: number = 0;

    OnCurrentTypeLostFocus() {

        if (this.parent.declarationPM.Direction == 'E' && !AppTool.IsNullOrEmpty(this.EntityPM.Amount)) {
            this.parent.GetDifferenceAndTotalForeignCurrency()
        }
    }


    OnAmountLostFocus() {
        if (this.parent.declarationPM.Direction == 'E' && !AppTool.IsNullOrEmpty(this.EntityPM.Amount)) {

            this.parent.GetDifferenceAndTotalForeignCurrency()
        }
        if (this.doCalculate) {
            var value = this.Amount;


            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.CurrencyTypeCode, this.parent.declarationPM.TaxationDateTime).subscribe((response: any) => {
                if (this.parent.EntityPM.InvoiceAmount) {
                    this.DiscountInNIS = this.parent.EntityPM.InvoiceAmount * this.InvoiceCurrencyExchangeRtae * value;
                    //  value = value * this.parent.InvoicePM.InvoiceAmount;
                    if (response) {
                        if (response.Result) {
                            var result = response.Result[0];
                            if (result) {

                                var amount = this.DiscountInNIS / result.ExchangeRate;
                                if (amount) {
                                    this.Amount = amount;
                                }


                            }
                        }
                    }
                }
                this.doCalculate = false;


            });
        }

    }

    public DeleteButton = false;
    DeleteButtonClicked(item: ModificationItemModel) {
        if (item != null) {
            this.Amount = null;
            this.CurrencyType = null;
            if (this.parent.declarationPM.Direction == "E") {
                this.parent.GetDifferenceAndTotalForeignCurrency();
            }


            switch (item.code) {
                /*case "67":
                    this.parent.InsuranceAmount = null;
                    this.parent.InsruanceCurrencyTypeCode = null;
                    break;
                case "144":
                    this.parent.EntityPM.FreightCurrencyTypeCode = null;
                    this.parent.EntityPM.TotalFreightInFreightCurrency = null;
                    var SupplierInvoiceFreightAmount = this.parent.EntityPM.SupplierInvoiceFreightAmounts.find(d => d.DeclarationId == this.parent.EntityPM.DeclarationId);
                    this.deletSupplierInvoiceFreightAmountPMWithCode(SupplierInvoiceFreightAmount.CurrencyTypeCode);
                    break;*/
            }
            this.DeleteButton = false;
        }
    }
}

export class SupplierInvoiceFreightAmountLine extends BaseComponent {
    public entityPM: SupplierInvoiceFreightAmountPM;
    public ObjectTableName: string = "Customs.SupplierInvoiceFreightAmount";
    public DataContext = this;
    Parent: SupplierInvoiceGeneralTabComponent;
    constructor(EntityPM: SupplierInvoiceFreightAmountPM, parent: SupplierInvoiceGeneralTabComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
        if (EntityPM.ChangeSetOp == "Insert") {
            this.UIProperties.SetEnabled("CurrencyTypeCode", "Customs.SupplierInvoiceFreightAmount", true);
        }
        else {
            this.UIProperties.SetEnabled("CurrencyTypeCode", "Customs.SupplierInvoiceFreightAmount", false);
        }
    }

    currencyType: CurrencyTypePM;
    get CurrencyType() { return this.currencyType; }
    set CurrencyType(value: CurrencyTypePM) {

        if (this.currencyType != value) {
            this.currencyType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CurrencyTypeName = value.LocalName;


        } else {
            this.CurrencyTypeName = null;
            this.CurrencyTypeCode = null;
        }
    }


    public get CurrencyTypeCode() { return this.entityPM.CurrencyTypeCode; }
    public set CurrencyTypeCode(newValue: string) {
        if (this.Parent.AmountList.Length > 0) {
            if (newValue != null) {
                var exist = this.Parent.EntityPM.SupplierInvoiceFreightAmounts.find(d => d.CurrencyTypeCode === newValue);
                this.entityPM.CurrencyTypeCode = newValue
                if (exist) {


                    var confirmWindow = new ConfirmWindow();


                    confirmWindow.Width = 400;

                    confirmWindow.Height = 200;
                    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    confirmWindow.ShowNoButton = false;
                    confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType") + " - מסך נוספים");
                    this.entityPM.CurrencyTypeCode = newValue;
                    this.entityPM.CurrencyTypeCode = null;
                    // this.entityPM.CurrencyTypeName = null;
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            //this.entityPM.CurrencyTypeCode = newValue;
                            this.entityPM.CurrencyTypeCode = null;
                            this.CurrencyTypeName = null;
                            confirmWindow.Close();
                        }

                    });
                }
                else {
                    this.entityPM.CurrencyTypeCode = newValue;
                    if (this.Parent.AmountList.Length == 1) {
                        this.Parent.EntityPM.AddSupplierInvoiceFreightAmount(this.entityPM);
                    }


                }
            }
            else {
                this.entityPM.CurrencyTypeCode = newValue

            }

        }

        if (this.Parent.EntityPM.SupplierInvoiceFreightAmounts.length == 1) {

            this.Parent.FreightCurrencyTypeCode = newValue;

        }

        if (this.Amount != null) {
            this.Parent.LoadCurrenciesExchangeRates(true);
        }





    }

    public get CurrencyTypeName() { return this.entityPM.CurrencyTypeName; }
    public set CurrencyTypeName(newValue: string) { this.entityPM.CurrencyTypeName = newValue; }

    public get Amount() { return this.entityPM.Amount; }
    public set Amount(newValue: number) {
        this.entityPM.Amount = newValue;
    }

    OnAmountLostFocus(logCellTemplate: any, amountItemTextBox: any) {

        this.Amount = amountItemTextBox.textValue
        if (this.CurrencyTypeCode != null) {
            this.Parent.LoadCurrenciesExchangeRates(true);
        }
    }

    DeleteButtonClicked() {


        if (this.Parent.EntityPM.SupplierInvoiceFreightAmounts.includes(this.entityPM)) {
            this.Parent.EntityPM.RemoveSupplierInvoiceFreightAmount(this.entityPM);
            if (this.CurrencyTypeCode == this.Parent.FreightCurrencyTypeCode) {
                if (this.Parent.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
                    this.Parent.FreightCurrencyTypeCode = this.Parent.EntityPM.SupplierInvoiceFreightAmounts[0].CurrencyTypeCode;
                }
            }
        }

        this.Parent.LoadCurrenciesExchangeRates(true);

        this.Parent.BuildFreightAmountsList();

    }

}

export class ItemCodeComponent extends BaseComponent {
    private _ItemCode: string;
    private _ClassificationCode: string;
    private _VendorNumber: string;
    private _ItemDescription: string;
    private _OriginCountryCode: string;
    private _OriginCountryName: string;
    private _TariffID: string;
    private _InvoiceQuantityType: string;
    private _IsNew: boolean;
    private _GITITEMCRs: GITITEMCR[] = [];
    private _Direction: string;

    constructor(itemCode: string, classificationCode: string, itemDescription: string, vendorNumber: string, originCountryCode: string, originCountryName: string, isNew: boolean, invoiceQuantityType: string, private _CustomerCode: string, tariffID: string, public gITITEMCRs: GITITEMCR[], direction: string) {
        super();

        this.ItemCode = itemCode;
        this.ClassificationCode = classificationCode;
        this.VendorNumber = vendorNumber;
        this.ItemDescription = itemDescription;
        this.OriginCountryCode = originCountryCode;
        this.OriginCountryName = originCountryName;
        this.InvoiceQuantityType = invoiceQuantityType;
        this.IsNew = isNew;
        this.GITITEMCRs = gITITEMCRs;
        this.TariffID = tariffID;
        this._Direction = direction;
    }
    public get GITITEMCRs() { return this._GITITEMCRs; }
    public set GITITEMCRs(newValue: GITITEMCR[]) { this._GITITEMCRs = newValue; }

    public get TariffID() { return this._TariffID; }
    public set TariffID(newValue: string) { this._TariffID = newValue; }

    public get ItemCode() { return this._ItemCode; }
    public set ItemCode(newValue: string) { this._ItemCode = newValue; }

    public get ClassificationCode() { return this._ClassificationCode; }
    public set ClassificationCode(newValue: string) { this._ClassificationCode = newValue; }

    public get VendorNumber() { return this._VendorNumber; }
    public set VendorNumber(newValue: string) { this._VendorNumber = newValue; }

    public get ItemDescription() { return this._ItemDescription; }
    public set ItemDescription(newValue: string) { this._ItemDescription = newValue; }

    public get OriginCountryCode() { return this._OriginCountryCode; }
    public set OriginCountryCode(newValue: string) { this._OriginCountryCode = newValue; }

    public get OriginCountryName() { return this._OriginCountryName; }
    public set OriginCountryName(newValue: string) { this._OriginCountryName = newValue; }

    public get InvoiceQuantityType() { return this._InvoiceQuantityType; }
    public set InvoiceQuantityType(newValue: string) { this._InvoiceQuantityType = newValue; }

    public get CustomerCode() { return this._CustomerCode; }
    public set CustomerCode(newValue: string) { this._CustomerCode = newValue; }

    public get IsNew() { return this._IsNew; }
    public set IsNew(newValue: boolean) { this._IsNew = newValue; }

    public get Direction() { return this._Direction; }
    public set Direction(newValue: string) { this._Direction = newValue; }
}

export class ItemCertificateComponent extends BaseComponent {
    private _COUNTER: number;
    private _REQCERT: string;
    private _REMARKS: string;


    constructor(COUNTER: number, REQCERT: string, REMARKS: string) {
        super();

        this.COUNTER = COUNTER;
        this.REQCERT = REQCERT;
        this.REMARKS = REMARKS;

    }

    public get COUNTER() { return this._COUNTER; }
    public set COUNTER(newValue: number) { this._COUNTER = newValue; }

    public get REQCERT() { return this._REQCERT; }
    public set REQCERT(newValue: string) { this._REQCERT = newValue; }

    public get REMARKS() { return this._REMARKS; }
    public set REMARKS(newValue: string) { this._REMARKS = newValue; }


}
