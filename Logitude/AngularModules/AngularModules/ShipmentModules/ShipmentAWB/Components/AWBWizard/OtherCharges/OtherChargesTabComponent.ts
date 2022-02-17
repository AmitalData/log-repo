import {Component} from '@angular/core';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, ArrayTool, DateTool, FontTool} from '../../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPayablePM} from '../../../../../Shipment/EntityPMs/ShipmentPayablePM';
import {ShipmentReceivablePM} from '../../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ShipmentAWBPrintOnlyPM} from '../../../../../Shipment/EntityPMs/ShipmentAWBPrintOnlyPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {IATACodeList} from '../../../../../Infrastructure/EntityLists/IATACodeList';
import {CurrencyList} from '../../../../../Common/EntityLists/CurrencyList';
import {DueTypeList} from '../../../../../Common/EntityLists/DueTypeList';
import {ChargesTypeList} from '../../../../../Common/EntityLists/ChargesTypeList';
import {MeasurementList} from '../../../../../Common/EntityLists/MeasurementList';
import {IATACodeListService} from '../../../../../Infrastructure/Services/StandardLists/IATACodeListService';
import {CurrencyListService} from '../../../../../Common/Services/StandardLists/CurrencyListService';
import {DueTypeListService} from '../../../../../Common/Services/StandardLists/DueTypeListService';
import {ChargesTypeListService} from '../../../../../Common/Services/StandardLists/ChargesTypeListService';
import {MeasurementListService} from '../../../../../Common/Services/StandardLists/MeasurementListService';
import {ShipmentTool, ByPckageType} from '../../../../../Shipment/Tools';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({    
    selector: 'OtherChargesTabComponent',
    templateUrl: './OtherChargesTabComponent.html',
})

export class OtherChargesTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: OtherChargesTabComponent = this;
    public ObjectTableName: string;
    public ItemsSource: AWBWizardOtherChargeItem[];
    public FreightItemsSource: AWBWizardOtherChargeItem[];
    public SelectedItem: AWBWizardOtherChargeItem = null;
    public _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsLCLEntity: boolean = false;
    constructor() {
        super();
        this.ItemsSource = [];
        this.InitServices();
    }

    public AllDueTypes: DueTypeList[] = [];
    public AllIATACodes: IATACodeList[] = [];
    public AllCurrencies: CurrencyList[] = [];    
    public AllChargesTypes: ChargesTypeList[] = [];
    public AllMeasurements: MeasurementList[] = [];
    public myDueTypeListService: DueTypeListService
    public myIATACodeListService: IATACodeListService
    public myCurrencyListService: CurrencyListService
    public myChargesTypeListService: ChargesTypeListService
    public myMeasurementListService: MeasurementListService
    InitServices() {
        if (this.myDueTypeListService == null) {
            this.myDueTypeListService = new DueTypeListService();
        }

        if (this.myIATACodeListService == null) {
            this.myIATACodeListService = new IATACodeListService();
        }

        if (this.myCurrencyListService == null) {
            this.myCurrencyListService = new CurrencyListService();
        }

        if (this.myChargesTypeListService == null) {
            this.myChargesTypeListService = new ChargesTypeListService();
        }

        if (this.myMeasurementListService == null) {
            this.myMeasurementListService = new MeasurementListService();
        }
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.SetWarningInfo()
        this.BuildItemsSource();
        this.BuildFreightItemsSource();
        this.GenerateDefaultList();
        this.SetGenerateButton();
        this.Listen();
        this.SetUIProperties();
        this.CheckUpdateQuantities();
    }

    RefreshTab() {
        this.SetGenerateButton();
        this.ComputeTotals();
        this.CheckUpdateQuantities();
    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.BuildItemsSource();
                    this.SetUIProperties();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.BuildItemsSource();
                    this.SetUIProperties();
                }
            });
        }
    }

    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        
        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }

    public IsFillDataWarningVisible: boolean = false;
    private SetWarningInfo() {
        if (!this.Wizard.IsImportWizard) {
            var myResult = false;

            var myCodes: string[] = [];
            myCodes.push("EAWB");
            myCodes.push("BUBK");

            if (FeatureLocator.IsPackageOneOf(myCodes)) {
                myResult = false;
            }

            else {
                if (this.ItemsSource.length == 0) {
                    myResult = true;
                }
            }

            this.IsFillDataWarningVisible = myResult;
        }
    }

    private FireWizardEvent() {
        this.Wizard.ValidateScreen_OTC();
    }

    get TenantZeroAirlineId() { return this.EntityPM.TenantZeroAirlineId; }
    get AsAgreedOtherCharges() { return this.EntityPM.AsAgreedOtherCharges; }
    set AsAgreedOtherCharges(newValue: boolean) {
        if (this.EntityPM.AsAgreedOtherCharges != newValue) {
            this.EntityPM.AsAgreedOtherCharges = newValue;
        }
    }

    get OtherPrepaidCollectId() { return this.EntityPM.OtherPrepaidCollectId; }
    set OtherPrepaidCollectId(newValue: string) {
        if (this.EntityPM.OtherPrepaidCollectId != newValue) {
            this.EntityPM.OtherPrepaidCollectId = newValue;
            ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
        }
    }

    get IsPrepaidSelected() {
        var myResult = false;

        if (this.OtherPrepaidCollectId == "P") {
            myResult = true;
        }

        return myResult;
    }
    SetIsPrepaidSelected(newValue: boolean) {
        if (newValue) {
            this.OtherPrepaidCollectId = "P";
        }

        else {
            this.OtherPrepaidCollectId = "C";
        }
    }

    ApplyToAllClicked() {
        this.ItemsSource.forEach(item => {
            item.PrepaidCollectId = this.OtherPrepaidCollectId;
        });
    }

    // Manage Defaults
    ManageDefaultsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Manage defaults";
        logWindow.WindowArgs = this.EntityPM.ShipmentLevelCode;
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherCharges/ManageDefaultsComponent");
    }

    // Generate
    public IsGeneratingVisible: boolean;
    private SetGenerateButton() {
        var myResult = false;

        //if (!FeatureLocator.IsPackage_EAWB()) {
        //    if (this.ItemsSource.length == 0) {
        //        myResult = true;
        //    }
        //}

        if (this.ItemsSource.length == 0) {
            myResult = true;
        }

        this.IsGeneratingVisible = myResult;
        this.SetWarningInfo();
    }

    GenerateClicked() {
        this.myChargesTypeListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AllChargesTypes.concat(myResponse.Result);

                    var ChargesTypes: ChargesTypeList[] = myResponse.Result;
                    if (ChargesTypes != null) {

                        ChargesTypes = ChargesTypes.filter(d => d.IsAir == true && d.InActive == false && d.ChargesGroupCode != "FRT");

                        if (this.EntityPM.ShipmentLevelCode == "C") {
                            ChargesTypes = ChargesTypes.filter(d => d.IsAutoDisplayInConsolidation);
                        }

                        else {
                            ChargesTypes = ChargesTypes.filter(d => d.IsAutoDisplayInShipment == true);
                        }

                        ChargesTypes.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {

                            var newAWBPrintOnly: ShipmentAWBPrintOnlyPM = new ShipmentAWBPrintOnlyPM(this.EntityPM);
                            newAWBPrintOnly.Tenant = this.EntityPM.Tenant;
                            newAWBPrintOnly.ShipmentId = this.EntityPM.Id;
                            newAWBPrintOnly.MeasurementId = item.MeasurementId;
                            newAWBPrintOnly.DueTypeCode = item.DueTypeCode;
                            newAWBPrintOnly.DueTypeName = item.DueTypeName;
                            newAWBPrintOnly.IATACodeId = item.IATACodeId;
                            newAWBPrintOnly.CurrencyId = this.EntityPM.AWBCurrencyId;
                            newAWBPrintOnly.CurrencyCode = this.EntityPM.AWBCurrencyCode;
                            newAWBPrintOnly.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;

                            if (newAWBPrintOnly.CurrencyId != null) {
                                var myCurrencyList: CurrencyList = this.AllCurrencies.filter(d => d.Id == newAWBPrintOnly.CurrencyId)[0];
                                if (myCurrencyList != null) {
                                    newAWBPrintOnly.CurrencyCode = myCurrencyList.Code;
                                }

                                else {
                                    this.myCurrencyListService.getSingle(newAWBPrintOnly.CurrencyId).subscribe((myResponse: ServiceResponse) => {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                this.AllCurrencies.push(myResponse.Result);
                                                newAWBPrintOnly.CurrencyCode = myResponse.Result.Code;
                                            }
                                        }
                                    });
                                }
                            }

                            if (newAWBPrintOnly.IATACodeId != null) {
                                var myIATACodeList: IATACodeList = this.AllIATACodes.filter(d => d.Id == newAWBPrintOnly.IATACodeId)[0];
                                if (myIATACodeList != null) {
                                    newAWBPrintOnly.IATACodeName = myIATACodeList.Name;
                                }

                                else {
                                    this.myIATACodeListService.getSingle(newAWBPrintOnly.IATACodeId).subscribe((myResponse: ServiceResponse) => {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                this.AllIATACodes.push(myResponse.Result);
                                                newAWBPrintOnly.IATACodeName = myResponse.Result.Name;
                                            }
                                        }
                                    });
                                }
                            }

                            if (newAWBPrintOnly.MeasurementId != null) {
                                var myMeasurementList: MeasurementList = this.AllMeasurements.filter(d => d.Id == newAWBPrintOnly.MeasurementId)[0];
                                if (myMeasurementList != null) {
                                    newAWBPrintOnly.MeasurementCode = myMeasurementList.Code;

                                    switch (newAWBPrintOnly.MeasurementCode) {
                                        case "GRWT": { newAWBPrintOnly.Quantity = this.EntityPM.GrossWeight; break; }
                                        case "CHWT": { newAWBPrintOnly.Quantity = this.EntityPM.ChargeableWeight; break; }
                                        case "VOLU": { newAWBPrintOnly.Quantity = this.EntityPM.Volume; break; }
                                        case "BTEU": { newAWBPrintOnly.Quantity = this.EntityPM.TEU; break; }
                                        case "FIXD": { newAWBPrintOnly.Quantity = 1; break; }
                                        case "PRVL": { newAWBPrintOnly.Quantity = this.EntityPM.ValueOfGoods; break; }
                                        case "GWTN": { newAWBPrintOnly.Quantity = this.EntityPM.GrossWeightPerTon; break; }
                                        case "QTY": { newAWBPrintOnly.Quantity = this.EntityPM.NumberOfPackages; break; }
                                        default: { break; }
                                    }
                                }

                                else {
                                    this.myMeasurementListService.getSingle(newAWBPrintOnly.MeasurementId).subscribe((myResponse: ServiceResponse) => {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                this.AllMeasurements.push(myResponse.Result);
                                                newAWBPrintOnly.MeasurementCode = myResponse.Result.Code;

                                                switch (newAWBPrintOnly.MeasurementCode) {
                                                    case "GRWT": { newAWBPrintOnly.Quantity = this.EntityPM.GrossWeight; break; }
                                                    case "CHWT": { newAWBPrintOnly.Quantity = this.EntityPM.ChargeableWeight; break; }
                                                    case "VOLU": { newAWBPrintOnly.Quantity = this.EntityPM.Volume; break; }
                                                    case "BTEU": { newAWBPrintOnly.Quantity = this.EntityPM.TEU; break; }
                                                    case "FIXD": { newAWBPrintOnly.Quantity = 1; break; }
                                                    case "PRVL": { newAWBPrintOnly.Quantity = this.EntityPM.ValueOfGoods; break; }
                                                    case "GWTN": { newAWBPrintOnly.Quantity = this.EntityPM.GrossWeightPerTon; break; }
                                                    case "QTY": { newAWBPrintOnly.Quantity = this.EntityPM.NumberOfPackages; break; }
                                                    default: { break; }
                                                }
                                            }
                                        }
                                    });
                                }
                            }

                            this.EntityPM.ShipmentAWBPrintOnlies.push(newAWBPrintOnly);
                        });

                        this.BuildItemsSource();
                    }
                }
            }
        });
    }
    private BuildItemsSource() {
        this.ItemsSource = [];

        this.EntityPM.ShipmentPayables.filter(f => f.ChargesGroupCode != "FRT").forEach(item => {
            this.ItemsSource.push(new AWBWizardOtherChargeItem(item, this, false));
        });

        this.EntityPM.ShipmentReceivables.filter(f => f.ChargesGroupCode != "FRT").forEach(item => {
            this.ItemsSource.push(new AWBWizardOtherChargeItem(item, this, false));
        });

        this.EntityPM.ShipmentAWBPrintOnlies.forEach(item => {
            this.ItemsSource.push(new AWBWizardOtherChargeItem(item, this, false));
        });

        this.ComputeTotals();
        this.SetGenerateButton();
    }
    private BuildFreightItemsSource() {
        this.FreightItemsSource = [];

        this.EntityPM.ShipmentPayables.filter(f => f.ChargesGroupCode == "FRT").forEach(item => {
            this.FreightItemsSource.push(new AWBWizardOtherChargeItem(item, this, false));
        });

        this.EntityPM.ShipmentReceivables.filter(f => f.ChargesGroupCode == "FRT").forEach(item => {
            this.FreightItemsSource.push(new AWBWizardOtherChargeItem(item, this, false));
        });
    }
    private GenerateDefaultList() {
        if (FeatureLocator.IsPackage_EAWB()) {
            if (this.ItemsSource.length < 5) {
                var list: ShipmentAWBPrintOnlyPM[] = [];

                for (var i = this.ItemsSource.length; i < 5; i++) {
                    var item = new ShipmentAWBPrintOnlyPM(null);
                    item.Tenant = SessionLocator.Tenant;
                    item.ShipmentId = this.EntityPM.Id;
                    item.CurrencyId = this.EntityPM.AWBCurrencyId;
                    item.CurrencyCode = this.EntityPM.AWBCurrencyCode;
                    item.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;
                    item.ExchangeRate = this.Wizard.GetCurrencyRate(this.EntityPM.AWBCurrencyId);
                    list.push(item);
                }

                list.forEach(item => {
                    this.ItemsSource.push(new AWBWizardOtherChargeItem(item, this, false));
                });

                this.SetGenerateButton();
            }
        }
    }

    // Summary Totals
    public Total_P: number = 0;
    public Total_C: number = 0;
    public TaxAmount_P: number = 0;
    public TaxAmount_C: number = 0;
    public AgentAmount_P: number = 0;
    public AgentAmount_C: number = 0;
    public CarrierAmount_P: number = 0;
    public CarrierAmount_C: number = 0;
    public ValuationAmount_P: number = 0;
    public ValuationAmount_C: number = 0;
    get AWBFreightAmountPrepaid() { return AppTool.IsNullOrEmpty(this.EntityPM.AWBFreightAmountPrepaid) ? 0 : this.EntityPM.AWBFreightAmountPrepaid; }
    get AWBFreightAmountCollect() { return AppTool.IsNullOrEmpty(this.EntityPM.AWBFreightAmountCollect) ? 0 : this.EntityPM.AWBFreightAmountCollect; }    
    public ComputeTotals() {

        this.Total_P = 0;
        this.Total_C = 0;
        this.TaxAmount_P = 0;
        this.TaxAmount_C = 0;
        this.AgentAmount_P = 0;
        this.AgentAmount_C = 0;
        this.CarrierAmount_P = 0;
        this.CarrierAmount_C = 0;
        this.ValuationAmount_P = 0;
        this.ValuationAmount_C = 0;

        this.ItemsSource.filter(d => d.AWBPrint && d.CurrencyId == this.EntityPM.AWBCurrencyId).forEach(item => {
            if (!AppTool.IsNullOrEmpty(item.Amount)) {
                if (item.PrepaidCollectId == "P") {

                    this.Total_P += item.Amount;

                    switch (item.DueTypeCode) {
                        case "TX": {
                            this.TaxAmount_P += item.Amount;
                            break;
                        }

                        case "AG": {
                            this.AgentAmount_P += item.Amount;
                            break;
                        }

                        case "CA": {
                            this.CarrierAmount_P += item.Amount;
                            break;
                        }

                        case "VL": {
                            this.ValuationAmount_P += item.Amount;
                            break;
                        }
                    }
                }

                else if (item.PrepaidCollectId == "C") {

                    this.Total_C += item.Amount;

                    switch (item.DueTypeCode) {
                        case "TX": {
                            this.TaxAmount_C += item.Amount;
                            break;
                        }

                        case "AG": {
                            this.AgentAmount_C += item.Amount;
                            break;
                        }

                        case "CA": {
                            this.CarrierAmount_C += item.Amount;
                            break;
                        }

                        case "VL": {
                            this.ValuationAmount_C += item.Amount;
                            break;
                        }
                    }
                }
            }
        });

        if (!AppTool.IsNullOrEmpty(this.AWBFreightAmountPrepaid)) {
            this.Total_P += this.AWBFreightAmountPrepaid;
        }

        if (!AppTool.IsNullOrEmpty(this.AWBFreightAmountCollect)) {
            this.Total_C += this.AWBFreightAmountCollect;
        }

        this.FireWizardEvent();
        this.SetWarningInfo();
        this.SetGenerateButton();
    }

    // Commands
    public Add() {
        var itemPM: ShipmentAWBPrintOnlyPM = new ShipmentAWBPrintOnlyPM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.CurrencyId = this.EntityPM.AWBCurrencyId;
        itemPM.CurrencyCode = this.EntityPM.AWBCurrencyCode;
        itemPM.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;
        itemPM.ExchangeRate = this.Wizard.GetCurrencyRate(this.EntityPM.AWBCurrencyId);

        var itemViewModel = new AWBWizardOtherChargeItem(itemPM, this, true);
        itemViewModel.OnCurrencyIdChanged();

        var title = "Add " + itemViewModel.TypeName.replace("AWB Print Only", "Charge");
        this.RunWindow(itemViewModel, title);
    }
    public Edit(itemViewModel: AWBWizardOtherChargeItem) {
        var title = "Edit " + itemViewModel.TypeName.replace("AWB Print Only", "Charge");
        this.RunWindow(itemViewModel, title);
    }
    public Delete(item: AWBWizardOtherChargeItem) {
        // Dont Show Confirm: Task 21883

        if (item != null) {
            if (item.PayablePM != null) {
                this.EntityPM.RemovePayable(item.PayablePM);
            }

            else if (item.ReceivablePM != null) {
                this.EntityPM.RemoveReceivable(item.ReceivablePM);            
            }

            else if (item.AWBPrintOnlyPM != null) {
                this.EntityPM.RemoveAWBPrintOnly(item.AWBPrintOnlyPM);
            }

            var index = this.ItemsSource.indexOf(item);
            if (index > -1) {
                this.ItemsSource.splice(index, 1);
            }

            this.ComputeTotals();
            this.SetGenerateButton();
        }
    }
    RunWindow(item: AWBWizardOtherChargeItem, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.DataContext = item;
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherCharges/AddEditOtherChargeComponent");
    }

    // Update Quantities
    public UpdateQuantitiesMessage: string;
    public UpdateQuantitiesMessageWidth: number = 0;
    public IsUpdateQuantitiesVisible: boolean = false;
    CheckUpdateQuantities() {
        var updateMessage = null;

        var activeLines: AWBWizardOtherChargeItem[] = [];
        activeLines = this.ItemsSource;
        this.FilterPayablesLines(activeLines);
        this.FilterReceivablesLines(activeLines);        
        activeLines = activeLines.filter(d => d.UnitPrice != null);

        if (activeLines.length > 0) {

            var isDifferentOrders: boolean = false;
            var isDifferentPRVL: boolean = false;
            var isDifferentPRFR: boolean = false;

            activeLines.forEach(item => {
                switch (item.MeasurementCode) {
                    case "PFCL": {
                        var quantity_Payable: number = AppTool.Round(ArrayTool.Sum(this.ItemsSource.filter(d => d.TypeName == "Payable" && d.CurrencyId != SessionLocator.LocalCurrencyId && d.MeasurementCode != "PFCL"), "AmountLocal"), 3);
                        var quantity_Receivable: number = AppTool.Round(ArrayTool.Sum(this.ItemsSource.filter(d => d.TypeName == "Receivable" && d.CurrencyId != SessionLocator.LocalCurrencyId && d.MeasurementCode != "PFCL"), "AmountLocal"), 3);

                        if ((item.TypeName == "Payable" && item.Quantity != quantity_Payable)
                            || (item.TypeName == "Receivable" && item.Quantity != quantity_Receivable)) {
                            isDifferentPRVL = true;
                        }

                        break;
                    }

                    case "SCGW": {
                        if (item.Quantity != this.EntityPM.GrossWeightPerStorageDays) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "CWKG": {
                        if (item.Quantity != this.EntityPM.ChargeableWeightInKG) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "GWKG": {
                        if (item.Quantity != this.EntityPM.GrossWeightInKG) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "GRWT": {
                        if (item.Quantity != this.EntityPM.GrossWeight) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "GWTN": {
                        if (item.Quantity != this.EntityPM.GrossWeightPerTon) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "QTY": {
                        if (this.IsLCLEntity) {
                            if (item.Quantity != this.EntityPM.NumberOfPackages) {
                                isDifferentOrders = true;
                            }
                        }

                        else {
                            if (item.Quantity != this.EntityPM.NumberOfContainers) {
                                isDifferentOrders = true;
                            }
                        }

                        break;
                    }

                    case "CHWT": {
                        if (item.Quantity != this.EntityPM.ChargeableWeight) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "VOLU": {
                        if (item.Quantity != this.EntityPM.Volume) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "BTEU": {
                        if (item.Quantity != this.EntityPM.TEU) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "PRVL": {
                        if (item.Quantity != this.EntityPM.ValueOfGoods) {
                            isDifferentPRVL = true;
                        }
                        break;
                    }

                    case "PRFR": {
                        if (this.FreightItemsSource.length > 0) {
                            var FRT_Quantity_Payable = ArrayTool.Sum(this.FreightItemsSource.filter(d => d.TypeName == "Payable" && AppTool.IsNullOrEmpty(d.EntityParentId)), "Amount");
                            var FRT_Quantity_Receivable = ArrayTool.Sum(this.FreightItemsSource.filter(d => d.TypeName == "Receivable" && AppTool.IsNullOrEmpty(d.EntityParentId)), "Amount");

                            if ((item.TypeName == "Payable" && item.Quantity != FRT_Quantity_Payable)
                                || (item.TypeName == "Receivable" && item.Quantity != FRT_Quantity_Receivable)) {
                                isDifferentPRVL = true;
                            }

                            if (this.ItemsSource.filter(f => f.MeasurementCode == "PRFR"
                                && ((f.TypeName == "Payable" && f.Quantity != FRT_Quantity_Payable)
                                || (f.TypeName == "Receivable" && f.Quantity != FRT_Quantity_Receivable))).length > 0) {
                                isDifferentPRFR = true;
                            }
                        }                        

                        break;
                    }

                    case "VCBM": {

                        if (item.Quantity != this.EntityPM.VolumeInCBM) {
                            isDifferentOrders = true;
                        }

                        break;
                    }
                }
            });

            if (isDifferentOrders) {
                updateMessage = "You have updated the packages details, apply the new values?";
            }

            else if (isDifferentPRVL) {
                updateMessage = "You have updated the value of goods, apply the new values?";
            }

            else if (isDifferentPRFR) {
                updateMessage = "You have updated the value of freight charge, apply the new values?";
            }
        }

        this.UpdateQuantitiesMessage = updateMessage;
        this.UpdateQuantitiesMessageWidth = AppTool.GetTextWidth(updateMessage, 11);
        this.IsUpdateQuantitiesVisible = AppTool.IsNullOrEmpty(updateMessage) ? false : true;
    }    
    FilterPayablesLines(activeLines: AWBWizardOtherChargeItem[]) {
        activeLines = activeLines.filter(d => d.TypeName == "Payable");
        activeLines = activeLines.filter(d => d.PayablePM.ShipmentPayableParentId == null);
        activeLines = activeLines.filter(d => d.PayablePM.ShipmentPayableAmountTypeCode != "NEXP");
        activeLines = activeLines.filter(d => d.PayablePM.ShipmentPayableLineStatusCode != "ACCT");
        activeLines = activeLines.filter(d => d.PayablePM.ShipmentPayableLineStatusCode != "PACC");        
    }
    FilterReceivablesLines(activeLines: AWBWizardOtherChargeItem[]) {
        activeLines = activeLines.filter(d => d.TypeName == "Receivable");
        activeLines = activeLines.filter(d => d.ReceivablePM.ShipmentReceivableParentId == null);
        activeLines = activeLines.filter(d => d.ReceivablePM.ARInvoiceId == null);        
    }
    
    UpdateQuantitiesClicked() {
        var activeLines: AWBWizardOtherChargeItem[] = [];
        activeLines = this.ItemsSource;
        this.FilterPayablesLines(activeLines);
        this.FilterReceivablesLines(activeLines);        

        activeLines.forEach((item: AWBWizardOtherChargeItem) => {
            item.SetQuantity();
        });

        this.CheckUpdateQuantities();
    }
}

export class AWBWizardOtherChargeItem extends BaseComponent {
    public DataContext: AWBWizardOtherChargeItem = this;
    public ShipmentPM: ShipmentPM;
    public PayablePM: ShipmentPayablePM;
    public ReceivablePM: ShipmentReceivablePM;
    public AWBPrintOnlyPM: ShipmentAWBPrintOnlyPM;    
    public IsNewEntity: boolean;
    public IsWindowMode: boolean;
    public TypeName: string;
    public TypeImageSource: string;
    public AmountFieldName: string;
    public RateFieldName: string;
    public ObjectTableName: string;
    public EntityParentId: string;
    constructor(item: any, public fatherComponent: OtherChargesTabComponent, isnew: boolean) {
        super();

        if (item != null) {

            this.IsNewEntity = isnew;
            this.ShipmentPM = fatherComponent.EntityPM;

            if (item instanceof ShipmentPayablePM) {
                this.PayablePM = item;
                this.TypeName = "Payable";
                this.TypeImageSource = "./Images/Letter_P.png";
                this.AmountFieldName = "TotalAmount";
                this.RateFieldName = "Rate";
                this.ObjectTableName = "ShipmentPayable";
                this.EntityParentId = this.PayablePM.ShipmentPayableParentId;
            }

            else if (item instanceof ShipmentReceivablePM) {
                this.ReceivablePM = item;
                this.TypeName = "Receivable";
                this.TypeImageSource = "./Images/Letter_R.png";
                this.AmountFieldName = "TotalAmount";
                this.RateFieldName = "Rate";
                this.ObjectTableName = "ShipmentReceivable";
                this.EntityParentId = this.ReceivablePM.ShipmentReceivableParentId;
            }

            else if (item instanceof ShipmentAWBPrintOnlyPM) {
                this.AWBPrintOnlyPM = item;
                this.TypeName = "AWB Print Only";
                this.TypeImageSource = "./Images/Letter_A.png";
                this.AmountFieldName = "Amount";
                this.RateFieldName = "ExchangeRate";
                this.ObjectTableName = "ShipmentAWBPrintOnly";
            }
            this.fatherComponent._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response=> {
                this.SetUIProperties();
                this.BuildQueryFilters();
            });       
        }
    }

    public IsEditingItemEnabled: boolean = false;
    public IsChargesTypeVisible: boolean = false;
    public SetUIProperties() {
        this.SetCheckBox();
        this.SetEditingItemEnabled();
        this.ValidateAWBPrintRequiredFields();
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled(this.AmountFieldName, this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, this.IsEditingItemEnabled);

        var isEditingFieldEnabled = this.IsEditingItemEnabled;
        if (isEditingFieldEnabled) {
            if (this.AWBPrintOnlyPM != null) {
                if (FeatureLocator.IsPackage_EAWB()) {
                    if (AppTool.IsNullOrEmpty(this.IATACodeId)) {
                        isEditingFieldEnabled = false;
                    }
                }
            }
        }

        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("MeasurementId", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("PrepaidCollectId", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, isEditingFieldEnabled);
    }
    private SetEditingItemEnabled() {

        var isEditingItemEnabled = this.fatherComponent.IsEditingEnabled;

        if (isEditingItemEnabled) {
            if (this.ReceivablePM != null) {
                if (this.ReceivablePM.ShipmentReceivableLineStatusCode == "ACCT" || this.ReceivablePM.ShipmentReceivableLineStatusCode == "DRFT") {
                    isEditingItemEnabled = false;
                }

                if (this.ReceivablePM.IsChargeBySteps) {
                    isEditingItemEnabled = false;
                }

                if (!AppTool.IsNullOrEmpty(this.ReceivablePM.ShipmentReceivableParentId)) {
                    isEditingItemEnabled = false;
                }
            }

            else if (this.PayablePM != null) {
                if (this.PayablePM.ShipmentPayableLineStatusCode == "ACCT" || this.PayablePM.ShipmentPayableLineStatusCode == "PACC") {
                    isEditingItemEnabled = false;
                }

                if (this.PayablePM.IsChargeBySteps) {
                    isEditingItemEnabled = false;
                }

                if (!AppTool.IsNullOrEmpty(this.PayablePM.ShipmentPayableParentId)) {
                    isEditingItemEnabled = false;
                }
            }
        }

        this.IsEditingItemEnabled = isEditingItemEnabled;
    }
    private ValidateAWBPrintRequiredFields() {
        var isChargesTypeVisible = true;
        if (this.AWBPrintOnlyPM != null) {
            isChargesTypeVisible = false;
            this.UIProperties.SetVisibility("ChargesTypeId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("IATACodeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.IATACodeId));
            this.UIProperties.SetRequired("PrepaidCollectId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PrepaidCollectId));
        }

        this.IsChargesTypeVisible = isChargesTypeVisible;
    }

    private BuildQueryFilters() {

    }

    // AWBPrint
    public NotMatchedToolTip: string;
    public IsAWBPrintIconVisible: boolean = false;
    public IsAWBPrintCheckBoxVisible: boolean = false;
    public IsAWBPrintCheckBoxEnabled: boolean = false;
    get AWBPrint() {
        var myResult = false;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.AWBPrint;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.AWBPrint;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = true;

            if (AppTool.IsNullOrEmpty(this.IATACodeId) || AppTool.IsNullOrEmpty(this.PrepaidCollectId) || AppTool.IsNullOrEmpty(this.DueTypeCode) || this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
                myResult = false;
            }
        }

        return myResult;
    }
    set AWBPrint(newValue: boolean) {
        if (this.PayablePM != null) {
            if (this.PayablePM.AWBPrint != newValue) {
                this.PayablePM.AWBPrint = newValue;
            }

            this.fatherComponent.ComputeTotals();
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.AWBPrint != newValue) {
                this.ReceivablePM.AWBPrint = newValue;
            }

            this.fatherComponent.ComputeTotals();
        }        
    }
    private SetAWBPrintField() {
        var myResult = true;

        if (AppTool.IsNullOrEmpty(this.IATACodeId) || AppTool.IsNullOrEmpty(this.PrepaidCollectId) || AppTool.IsNullOrEmpty(this.DueTypeCode) || this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
            myResult = false;
        }

        this.AWBPrint = myResult;

        this.SetCheckBox();
    }
    private SetCheckBox() {

        var isMatched = true;
        var error = "";

        if (this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
            isMatched = false;
            error = "Currency is not equal to the AWB currency";
        }

        if (AppTool.IsNullOrEmpty(this.IATACodeId)) {
            isMatched = false;

            if (AppTool.IsNullOrEmpty(error)) {
                error = "IATA code field is required";
            }

            else {
                error += "\nIATA code field is required";
            }
        }

        if (AppTool.IsNullOrEmpty(this.DueTypeCode)) {
            isMatched = false;

            if (AppTool.IsNullOrEmpty(error)) {
                error = "Due type field is required";
            }

            else {
                error += "\nDue type field is required";
            }
        }

        if (AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            isMatched = false;

            if (AppTool.IsNullOrEmpty(error)) {
                error = "P / C field is required";
            }

            else {
                error += "\nP / C field is required";
            }
        }

        this.NotMatchedToolTip = error;
        this.IsAWBPrintIconVisible = !isMatched;
        this.IsAWBPrintCheckBoxVisible = isMatched;
        this.IsAWBPrintCheckBoxEnabled = this.AWBPrintOnlyPM == null ? true : false;
    }

    // IATACodeId
    get IATACodeId() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.IATACodeId;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.IATACodeId;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.IATACodeId;
        }

        return myResult;
    }
    set IATACodeId(newValue: string) {

        var isExecuting = false;

        if (this.PayablePM != null) {
            if (this.PayablePM.IATACodeId != newValue) {
                this.PayablePM.IATACodeId = newValue;
                isExecuting = true;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.IATACodeId != newValue) {
                this.ReceivablePM.IATACodeId = newValue;
                isExecuting = true;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.IATACodeId != newValue) {
                this.AWBPrintOnlyPM.IATACodeId = newValue;
                isExecuting = true;

                if (FeatureLocator.IsPackage_EAWB()) {

                    var itemIndex = this.ShipmentPM.ShipmentAWBPrintOnlies.indexOf(this.AWBPrintOnlyPM);

                    if (newValue == null) {
                        this.MeasurementId = null;
                        this.DueTypeCode = null;
                        this.UnitPrice = null;
                        this.Quantity = null;

                        if (!this.IsWindowMode) {
                            if (itemIndex > -1) {
                                this.ShipmentPM.RemoveAWBPrintOnly(this.AWBPrintOnlyPM);
                            }
                        }
                    }

                    else {

                        var list: IATACodeList = this.fatherComponent.AllIATACodes.filter(d => d.Id == this.AWBPrintOnlyPM.IATACodeId)[0];
                        if (list != null) {
                            this.AWBPrintOnlyPM.IATACodeName = list.Name;
                        }

                        else {
                            this.fatherComponent.myIATACodeListService.getSingle(this.AWBPrintOnlyPM.IATACodeId).subscribe((myResponse: ServiceResponse) => {
                                if (myResponse != null) {
                                    if (!myResponse.HasError) {
                                        this.fatherComponent.AllIATACodes.push(myResponse.Result);
                                        this.AWBPrintOnlyPM.IATACodeName = myResponse.Result.Name;
                                    }
                                }
                            });
                        }

                        if (!this.IsWindowMode) {
                            if (itemIndex == -1) {
                                this.ShipmentPM.AddAWBPrintOnly(this.AWBPrintOnlyPM);
                            }
                        }
                    }
                }

                this.ValidateAWBPrintRequiredFields();
            }
        }

        if (isExecuting) {
            this.OnIATACodeIdChanged();
            this.SetAWBPrintField();
            this.SetUIProperties();
        }
    }
    get IATACodeName() {
        var myResult = null;

        if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.IATACodeName;
        }

        return myResult;
    }
    set IATACodeName(newValue: string) {
        if (this.AWBPrintOnlyPM != null) {
            this.AWBPrintOnlyPM.IATACodeName = newValue;
        }
    }
    private OnIATACodeIdChanged() {
        if (AppTool.IsNullOrEmpty(this.IATACodeId)) {
            this.IATACodeName = null;
        }

        else {
            var list: IATACodeList = this.fatherComponent.AllIATACodes.filter(d => d.Id == this.IATACodeId)[0];
            if (list != null) {
                this.IATACodeName = list.Name;
            }

            else {
                this.fatherComponent.myIATACodeListService.getSingle(this.IATACodeId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.fatherComponent.AllIATACodes.push(myResponse.Result);
                            this.IATACodeName = myResponse.Result.Name;
                        }
                    }
                });
            }           
        }
    }

    // ChargesTypeId
    get ChargesTypeId() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.ChargesTypeId;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.ChargesTypeId;
        }

        return myResult;
    }
    set ChargesTypeId(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.ChargesTypeId != newValue) {
                this.PayablePM.ChargesTypeId = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.ChargesTypeId != newValue) {
                this.ReceivablePM.ChargesTypeId = newValue;
            }
        }

        this.OnChargesTypeIdChanged();
    }
    get ChargesTypeCode() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.ChargesTypeCode;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.ChargesTypeCode;
        }

        return myResult;
    }
    set ChargesTypeCode(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.ChargesTypeCode != newValue) {
                this.PayablePM.ChargesTypeCode = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.ChargesTypeCode != newValue) {
                this.ReceivablePM.ChargesTypeCode = newValue;
            }
        }
    }
    get ChargesTypeName() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.ChargesTypeName;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.ChargesTypeName;
        }

        return myResult;
    }
    set ChargesTypeName(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.ChargesTypeName != newValue) {
                this.PayablePM.ChargesTypeName = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.ChargesTypeName != newValue) {
                this.ReceivablePM.ChargesTypeName = newValue;
            }
        }
    }
    get ChargesGroupCode() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.ChargesGroupCode;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.ChargesGroupCode;
        }

        return myResult;
    }
    set ChargesGroupCode(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.ChargesGroupCode != newValue) {
                this.PayablePM.ChargesGroupCode = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.ChargesGroupCode != newValue) {
                this.ReceivablePM.ChargesGroupCode = newValue;
            }
        }
    }
    private OnChargesTypeIdChanged() {
        if (AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.DueTypeCode = null;
            this.DueTypeName = null;
            this.PrepaidCollectId = null;
            this.CurrencyId = null;
            this.ChargesGroupCode = null;
            this.IATACodeId = null;
            this.MeasurementId = null;
        }

        else {
            var list: ChargesTypeList = this.fatherComponent.AllChargesTypes.filter(d => d.Id == this.ChargesTypeId)[0];
            if (list != null) {
                this.ChargesTypeCode = list.Code;
                this.ChargesTypeName = list.EnglishName;
                this.DueTypeCode = list.DueTypeCode;
                this.DueTypeName = list.DueTypeName;
                this.ChargesGroupCode = list.ChargesGroupCode;
                this.IATACodeId = list.IATACodeId;
                this.MeasurementId = list.MeasurementId;

                this.SetPrepaidCollectId();

                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    this.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                }

                else {
                    this.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }

            else {
                this.fatherComponent.myChargesTypeListService.getSingle(this.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var list: ChargesTypeList = myResponse.Result;

                            this.fatherComponent.AllChargesTypes.push(list);

                            this.ChargesTypeCode = list.Code;
                            this.ChargesTypeName = list.EnglishName;
                            this.DueTypeCode = list.DueTypeCode;
                            this.DueTypeName = list.DueTypeName;
                            this.ChargesGroupCode = list.ChargesGroupCode;
                            this.IATACodeId = list.IATACodeId;
                            this.MeasurementId = list.MeasurementId;

                            this.SetPrepaidCollectId();

                            if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                                this.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                            }

                            else {
                                this.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                            }
                        }
                    }
                });
            } 
        }
    }

    // PrepaidCollectId
    get PrepaidCollectId() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.PrepaidCollectId;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.PrepaidCollectId;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.PrepaidCollectId;
        }

        return myResult;
    }
    set PrepaidCollectId(newValue: string) {

        var isExecuting = false;

        if (this.PayablePM != null) {
            if (this.PayablePM.PrepaidCollectId != newValue) {
                this.PayablePM.PrepaidCollectId = newValue;
                isExecuting = true;
            } 
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.PrepaidCollectId != newValue) {
                this.ReceivablePM.PrepaidCollectId = newValue;
                isExecuting = true;
            } 
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.PrepaidCollectId != newValue) {
                this.AWBPrintOnlyPM.PrepaidCollectId = newValue;
                isExecuting = true;

                this.ValidateAWBPrintRequiredFields();
            } 
        }

        if (isExecuting) {
            this.SetAWBPrintField();
            this.SetUIProperties();
            this.fatherComponent.ComputeTotals();
        }
    }
    private SetPrepaidCollectId() {
        if (AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            if (this.ChargesGroupCode == "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.FreightPrepaidCollectId;
            }

            else if (this.ChargesGroupCode != "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.OtherPrepaidCollectId;
            }
        }
    }

    // DueTypeCode
    get DueTypeCode() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.DueTypeCode;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.DueTypeCode;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.DueTypeCode;
        }

        return myResult;
    }
    set DueTypeCode(newValue: string) {

        var isExecuting = false;

        if (this.PayablePM != null) {
            if (this.PayablePM.DueTypeCode != newValue) {
                this.PayablePM.DueTypeCode = newValue;
                isExecuting = true;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.DueTypeCode != newValue) {
                this.ReceivablePM.DueTypeCode = newValue;
                isExecuting = true;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.DueTypeCode != newValue) {
                this.AWBPrintOnlyPM.DueTypeCode = newValue;
                isExecuting = true;
            }
        }

        if (isExecuting) {
            this.SetAWBPrintField();
            this.SetUIProperties();
            this.OnDueTypeCodeChanged();
            this.fatherComponent.ComputeTotals();
        }
    }
    get DueTypeName() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.DueTypeName;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.DueTypeName;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.DueTypeName;
        }

        return myResult;
    }
    set DueTypeName(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.DueTypeName != newValue) {
                this.PayablePM.DueTypeName = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.DueTypeName != newValue) {
                this.ReceivablePM.DueTypeName = newValue;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.DueTypeName != newValue) {
                this.AWBPrintOnlyPM.DueTypeName = newValue;
            }
        }
    }
    private OnDueTypeCodeChanged() {
        if (this.DueTypeCode == null) {
            this.DueTypeName = null;
        }

        else {
            var list: DueTypeList = this.fatherComponent.AllDueTypes.filter(d => d.Code == this.DueTypeCode)[0];
            if (list != null) {
                this.DueTypeName = list.Name;
            }

            else {
                this.fatherComponent.myDueTypeListService.getSingle(this.DueTypeCode).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.fatherComponent.AllDueTypes.push(myResponse.Result);
                            this.DueTypeName = myResponse.Result.Name;
                        }
                    }
                });
            }
        }
    }

    // CurrencyId
    get CurrencyId() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.CurrencyId;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.CurrencyId;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.CurrencyId;
        }

        return myResult;
    }
    set CurrencyId(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.CurrencyId != newValue) {
                this.PayablePM.CurrencyId = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.CurrencyId != newValue) {
                this.ReceivablePM.CurrencyId = newValue;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.CurrencyId != newValue) {
                this.AWBPrintOnlyPM.CurrencyId = newValue;
            }
        }

        this.SetAWBPrintField();
        this.OnCurrencyIdChanged();
        this.fatherComponent.ComputeTotals();
    }
    get CurrencyCode() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.CurrencyCode;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.CurrencyCode;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.CurrencyCode;
        }

        return myResult;
    }
    set CurrencyCode(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.CurrencyCode != newValue) {
                this.PayablePM.CurrencyCode = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.CurrencyCode != newValue) {
                this.ReceivablePM.CurrencyCode = newValue;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.CurrencyCode != newValue) {
                this.AWBPrintOnlyPM.CurrencyCode = newValue;
            }
        }
    }
    get ExchangeRate() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.Rate;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.Rate;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.ExchangeRate;
        }

        return myResult;
    }
    set ExchangeRate(newValue: number) {

        if (this.PayablePM != null) {
            if (this.PayablePM.Rate != newValue) {
                this.PayablePM.Rate = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.Rate != newValue) {
                this.ReceivablePM.Rate = newValue;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.ExchangeRate != newValue) {
                this.AWBPrintOnlyPM.ExchangeRate = newValue;
            }
        }
    }
    public OnCurrencyIdChanged() {
        this.ExchangeRate = this.fatherComponent.Wizard.GetCurrencyRate(this.CurrencyId);

        if (this.CurrencyId == null) {
            this.CurrencyCode = null;            
        }

        else {
            var list: CurrencyList = this.fatherComponent.AllCurrencies.filter(d => d.Id == this.CurrencyId)[0];
            if (list != null) {
                this.CurrencyCode = list.Code;                
            }

            else {
                this.fatherComponent.myCurrencyListService.getSingle(this.CurrencyId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.fatherComponent.AllCurrencies.push(myResponse.Result);
                            this.CurrencyCode = myResponse.Result.Code;
                        }
                    }
                });
            }
        }
    }

    // MeasurementId
    get MeasurementId() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.MeasurementId;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.MeasurementId;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.MeasurementId;
        }

        return myResult;
    }
    set MeasurementId(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.MeasurementId != newValue) {
                this.PayablePM.MeasurementId = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.MeasurementId != newValue) {
                this.ReceivablePM.MeasurementId = newValue;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.MeasurementId != newValue) {
                this.AWBPrintOnlyPM.MeasurementId = newValue;
            }
        }

        this.OnMeasurementIdChanged();
    }
    get MeasurementCode() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.MeasurementCode;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.MeasurementCode;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.MeasurementCode;
        }

        return myResult;
    }
    set MeasurementCode(newValue: string) {

        if (this.PayablePM != null) {
            if (this.PayablePM.MeasurementCode != newValue) {
                this.PayablePM.MeasurementCode = newValue;
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.MeasurementCode != newValue) {
                this.ReceivablePM.MeasurementCode = newValue;
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.MeasurementCode != newValue) {
                this.AWBPrintOnlyPM.MeasurementCode = newValue;
            }
        }
    }
    private OnMeasurementIdChanged() {
        if (this.MeasurementId == null) {
            this.MeasurementCode = null;
        }

        else {
            var list: MeasurementList = this.fatherComponent.AllMeasurements.filter(d => d.Id == this.MeasurementId)[0];
            if (list != null) {
                this.MeasurementCode = list.Code;

                switch (this.MeasurementCode) {
                    case "GRWT": { this.Quantity = this.ShipmentPM.GrossWeight; break; }
                    case "CHWT": { this.Quantity = this.ShipmentPM.ChargeableWeight; break; }
                    case "VOLU": { this.Quantity = this.ShipmentPM.Volume; break; }
                    case "BTEU": { this.Quantity = this.ShipmentPM.TEU; break; }
                    case "FIXD": { this.Quantity = 1; break; }
                    case "PRVL": { this.Quantity = this.ShipmentPM.ValueOfGoods; break; }
                    case "GWTN": { this.Quantity = this.ShipmentPM.GrossWeightPerTon; break; }
                    case "QTY": { this.Quantity = this.ShipmentPM.NumberOfPackages; break; }
                    default: { break; }
                }
            }

            else {
                this.fatherComponent.myMeasurementListService.getSingle(this.MeasurementId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.fatherComponent.AllMeasurements.push(myResponse.Result);
                            this.MeasurementCode = myResponse.Result.Code;

                            switch (this.MeasurementCode) {
                                case "GRWT": { this.Quantity = this.ShipmentPM.GrossWeight; break; }
                                case "CHWT": { this.Quantity = this.ShipmentPM.ChargeableWeight; break; }
                                case "VOLU": { this.Quantity = this.ShipmentPM.Volume; break; }
                                case "BTEU": { this.Quantity = this.ShipmentPM.TEU; break; }
                                case "FIXD": { this.Quantity = 1; break; }
                                case "PRVL": { this.Quantity = this.ShipmentPM.ValueOfGoods; break; }
                                case "GWTN": { this.Quantity = this.ShipmentPM.GrossWeightPerTon; break; }
                                case "QTY": { this.Quantity = this.ShipmentPM.NumberOfPackages; break; }
                                default: { break; }
                            }
                        }
                    }
                });
            }
        }
    }

    get Name() {
        var myResult = "";

        if (this.AWBPrintOnlyPM != null) {
            myResult = this.IATACodeName;
        }

        else {
            myResult = this.ChargesTypeName;
        }

        return myResult;
    }

    get Quantity() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.Quantity;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.Quantity;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.Quantity;
        }

        return myResult;
    }
    set Quantity(newValue: number) {

        if (this.PayablePM != null) {
            if (this.PayablePM.Quantity != newValue) {
                this.PayablePM.Quantity = AppTool.Round(newValue, 3);
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.Quantity != newValue) {
                this.ReceivablePM.Quantity = AppTool.Round(newValue, 3);
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.Quantity != newValue) {
                this.AWBPrintOnlyPM.Quantity = AppTool.Round(newValue, 3);
            }
        }

        this.OnQuantityChanged();
    }

    get UnitPrice() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.UnitPrice;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.UnitPrice;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.UnitPrice;
        }

        return myResult;
    }
    set UnitPrice(newValue: number) {

        if (this.PayablePM != null) {
            if (this.PayablePM.UnitPrice != newValue) {
                this.PayablePM.UnitPrice = AppTool.Round(newValue, 3);
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.UnitPrice != newValue) {
                this.ReceivablePM.UnitPrice = AppTool.Round(newValue, 3);
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.UnitPrice != newValue) {
                this.AWBPrintOnlyPM.UnitPrice = AppTool.Round(newValue, 3);
            }
        }

        this.ComputeAmount();
    }

    get TotalAmount() {
        return this.Amount;
    }
    get Amount() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.ExpectedAmount;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.TotalAmount;
        }

        else if (this.AWBPrintOnlyPM != null) {
            myResult = this.AWBPrintOnlyPM.Amount;
        }

        return myResult;
    }
    set Amount(newValue: number) {

        if (this.PayablePM != null) {
            if (this.PayablePM.ExpectedAmount != newValue) {
                this.PayablePM.ExpectedAmount = AppTool.Round(newValue, 2);
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.TotalAmount != newValue) {
                this.ReceivablePM.TotalAmount = AppTool.Round(newValue, 2);
            }
        }

        else if (this.AWBPrintOnlyPM != null) {
            if (this.AWBPrintOnlyPM.Amount != newValue) {
                this.AWBPrintOnlyPM.Amount = AppTool.Round(newValue, 2);
            }
        }

        this.fatherComponent.ComputeTotals();
    }
    get AmountColor() {
        var myResult = FontTool.Black;

        if (this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
            myResult = FontTool.Red;
        }

        return myResult;
    }
    get AmountLocal() {
        var myResult = null;

        if (this.PayablePM != null) {
            myResult = this.PayablePM.ExpectedAmountLocal;
        }

        else if (this.ReceivablePM != null) {
            myResult = this.ReceivablePM.TotalAmountLocal;
        }

        return myResult;
    }

    private SetLineStatus() {
        if (this.PayablePM != null) {
            ShipmentTool.SetPayableLineStatus(this.PayablePM);
        }

        else if (this.ReceivablePM != null) {
            ShipmentTool.SetReceivableLineStatus(this.ReceivablePM);
        }
    }
    private ComputeAmount() {
        var amount = this.Quantity * this.UnitPrice;
        var _AmountLocal = null;
        var _AmountProfit = null;

        if (this.AWBPrintOnlyPM != null) {
            this.Amount = amount;
        }

        else {
            this.SetLineStatus();

            if (this.PayablePM != null) {
                if (this.PayablePM.QuoteCostMinAmount != null || this.PayablePM.QuoteCostMaxAmount != null) {
                    if (amount == null) {
                        amount = this.PayablePM.QuoteCostMinAmount;
                    }

                    if (amount != null) {
                        if (amount < this.PayablePM.QuoteCostMinAmount) {
                            amount = this.PayablePM.QuoteCostMinAmount;
                        }

                        else if (amount > this.PayablePM.QuoteCostMaxAmount) {
                            amount = this.PayablePM.QuoteCostMaxAmount;
                        }
                    }
                }
               

                /* From Tariff */
                if (this.PayablePM.MinAmount != null || this.PayablePM.MaxAmount != null) {
                    if (amount != null) {
                        if (amount < this.PayablePM.MinAmount) {
                            amount = this.PayablePM.MinAmount;
                        }

                        else if (amount > this.PayablePM.MaxAmount) {
                            amount = this.PayablePM.MaxAmount;
                        }
                    }
                }

                _AmountLocal = amount * this.ExchangeRate;

                if (this.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
                    _AmountProfit = amount;
                }

                else {
                    _AmountProfit = _AmountLocal / this.PayablePM.ProfitCurrencyExchangeRate;
                }


                this.Amount = amount;
                this.PayablePM.ExpectedAmountLocal = _AmountLocal;
                this.PayablePM.ExpectedAmountInProfitCurrency = _AmountProfit;
                this.ComputePayableOtherAmounts();
                ShipmentTool.ComputeTotals(this.ShipmentPM);
            }

            else if (this.ReceivablePM != null) {

                if (this.ReceivablePM.QuoteSaleMinAmount != null || this.ReceivablePM.QuoteSaleMaxAmount != null) {
                    if (amount == null) {
                        amount = this.ReceivablePM.QuoteSaleMinAmount;
                    }

                    if (amount != null) {
                        if (amount < this.ReceivablePM.QuoteSaleMinAmount) {
                            amount = this.ReceivablePM.QuoteSaleMinAmount;
                        }

                        else if (amount > this.ReceivablePM.QuoteSaleMaxAmount) {
                            amount = this.ReceivablePM.QuoteSaleMaxAmount;
                        }
                    }
                }

                _AmountLocal = amount * this.ExchangeRate;

                if (this.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
                    _AmountProfit = amount;
                }

                else {
                    _AmountProfit = _AmountLocal / this.ReceivablePM.ProfitCurrencyExchangeRate;
                }

                this.Amount = amount;
                this.ReceivablePM.TotalAmountLocal = _AmountLocal;
                this.ReceivablePM.AmountInProfitCurrency = _AmountProfit;
                ShipmentTool.ComputeTotals(this.ShipmentPM);
            }
        }
    }
    private OnQuantityChanged() {

        var baseQuote: any = null;

        if (this.PayablePM != null) {
            if (this.PayablePM.IsChargeBySteps) {                
                ShipmentTool.SetPayableUnitPriceBySteps(this.PayablePM, baseQuote);                
            }
        }

        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.IsChargeBySteps) {
                ShipmentTool.SetReceivableUnitPriceBySteps(this.ReceivablePM, baseQuote);               
            }
        }

        this.ComputeAmount();
    }
    private ComputePayableOtherAmounts() {
        if (this.PayablePM != null) {
            if (this.PayablePM.ShipmentPayableLineStatusCode == "EMPT" || this.PayablePM.ShipmentPayableLineStatusCode == "OAMT") {
                this.PayablePM.AccountedAmount = 0;
                this.PayablePM.CorrectionAmount = 0;
                this.PayablePM.OpenAmount = this.PayablePM.ExpectedAmount;
            }

            else {
                var expe = this.PayablePM.ExpectedAmount == null ? 0 : this.PayablePM.ExpectedAmount;
                var acct = this.PayablePM.AccountedAmount == null ? 0 : this.PayablePM.AccountedAmount;
                var open = this.PayablePM.OpenAmount == null ? 0 : this.PayablePM.OpenAmount;
                this.PayablePM.CorrectionAmount = expe - acct - open;

                if (!AppTool.IsNullOrEmpty(this.PayablePM.CorrectionByUserId)) {
                    this.SetLineStatus();
                }
            }

            this.PayablePM.AccountedAmountInLocalCurrency = this.PayablePM.AccountedAmount * this.PayablePM.Rate;;
            this.PayablePM.AccountedAmountInProfitCurrency = this.PayablePM.AccountedAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;;
            this.PayablePM.OpenAmountInLocalCurrency = this.PayablePM.OpenAmount * this.PayablePM.Rate;
            this.PayablePM.OpenAmountInProfitCurrency = this.PayablePM.OpenAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;
            this.PayablePM.CorrectionByUserId = SessionLocator.LoggedUserId;
            this.PayablePM.CorrectionDate = DateTool.GetCurrentDateAsUtc();
        }
    }

    SetQuantity() {
        var result = null;

        switch (this.MeasurementCode) {
            case "GRWT": { result = this.ShipmentPM.GrossWeight; break; }
            case "CHWT": { result = this.ShipmentPM.ChargeableWeight; break; }
            case "VOLU": { result = this.ShipmentPM.Volume; break; }
            case "BTEU": { result = this.ShipmentPM.TEU; break; }
            case "FIXD": { result = 1; break; }
            case "PRVL": { result = this.ShipmentPM.ValueOfGoods; break; }           
            case "GWTN": { result = this.ShipmentPM.GrossWeightPerTon; break; }
            case "QTY": { result = this.fatherComponent.IsLCLEntity ? this.ShipmentPM.NumberOfPackages : this.ShipmentPM.NumberOfContainers; break; }
            case "CWKG": { result = this.ShipmentPM.ChargeableWeightInKG; break; }
            case "GWKG": { result = this.ShipmentPM.GrossWeightInKG; break; }
            case "VCBM": { result = this.ShipmentPM.VolumeInCBM; break; }
            case "BCNT": { break; }
            case "SCGW": { result = this.ShipmentPM.GrossWeightPerStorageDays; break; }
            case "PRFR": { result = ArrayTool.Sum(this.fatherComponent.FreightItemsSource.filter(d => d.TypeName == this.TypeName && AppTool.IsNullOrEmpty(d.EntityParentId)), "Amount"); break; }
            case "PFCL": { result = AppTool.Round(ArrayTool.Sum(this.fatherComponent.ItemsSource.filter(d => d.TypeName == this.TypeName && d.CurrencyId != SessionLocator.LocalCurrencyId && d.MeasurementCode != "PFCL"), "AmountLocal"), 3); break;}

            default: {
                if (!AppTool.IsNullOrEmpty(this.MeasurementId)) {
                    var allBCNTGrouped: ByPckageType[] = ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);

                    var itemGrouped = allBCNTGrouped.filter(f => f.MeasurementId == this.MeasurementId)[0];
                    if (itemGrouped != null) {
                        result = itemGrouped.Quantity;
                    }
                }

                break;
            }
        }

        this.Quantity = result;
    }
}
