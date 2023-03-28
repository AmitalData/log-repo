import {Component} from '@angular/core';
import {NumbersPipe} from '../../../../../Infrastructure/Pipes/NumbersPipe';
import {AppTool, DateTool, ArrayTool, FontTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPayablePM} from '../../../../../Shipment/EntityPMs/ShipmentPayablePM';
import {ShipmentReceivablePM} from '../../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ConsoleShipmentPM} from '../../../../../Shipment/EntityPMs/ConsoleShipmentPM';
import {QuotePM} from '../../../../../Quote/EntityPMs/QuotePM';
import {ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ChargesTypeList} from '../../../../../Common/EntityLists/ChargesTypeList';
import {PackageTypeList} from '../../../../../Common/EntityLists/PackageTypeList';
import {ChargesTypeListService} from '../../../../../Common/Services/StandardLists/ChargesTypeListService';
import {PackageTypeListService} from '../../../../../Common/Services/StandardLists/PackageTypeListService';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    
    templateUrl: './PayablesComponent.html',
})

export class PayablesComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public BaseQuote: QuotePM;
    public ItemsSource: ObservableCollection;
    public DataContext: PayablesComponent = this;
    public LocalCurrencyCode: string;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsUsingVirtuallization: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.SetIsUsingVirtuallization();
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ItemsSource = new ObservableCollection([]);
        this.InitializeServices();
    }

    SetIsUsingVirtuallization() {
        var hasGridVirtuallizationToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EVG")[0]
        if (hasGridVirtuallizationToggleFeature) {
            this.IsUsingVirtuallization = true;
        }
    }

    public AllChargesTypes: ChargesTypeList[] = [];
    public AllPackageTypes: PackageTypeList[] = [];
    myChargesTypeListService: ChargesTypeListService;
    myPackageTypeListService: PackageTypeListService;
    InitializeServices() {
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myPackageTypeListService = new PackageTypeListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.BaseQuote = args['BaseQuote'];

        this.entityResourceService.getEntityResourceByTableName("ShipmentReceivable").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("ShipmentPayable").subscribe((res2: any) => {                
                this.SetLabels();                

                this.myChargesTypeListService.getAllFromCache().subscribe((myResponse1: ServiceResponse) => {
                    if (!myResponse1.HasError) {
                        this.AllChargesTypes = myResponse1.Result;

                        this.myPackageTypeListService.getAllFromCache().subscribe((myResponse2: ServiceResponse) => {
                            if (!myResponse2.HasError) {
                                this.AllPackageTypes = myResponse2.Result;

                                this.BuildObsList();
                                this.ComputeTotals();
                                this.ComputeWidth();
                                this.Clone();
                                this.IsResourcesReady = true;
                            }
                        });
                    }
                });
            });
        });
    }

    public PayableText: string = null;
    public ReceivableText: string = null;
    public QuantityText: string = null;
    public UnitPriceText: string = null;
    public AmountText: string = null;
    public AmountLocalText: string = null;
    SetLabels() {
        this.PayableText = TextCodeTranslator.Translate('Shipment.S.Receivables.Payable');
        this.ReceivableText = TextCodeTranslator.Translate('Shipment.S.Receivables.Receivable');       
        this.QuantityText = TextCodeTranslator.Translate("ShipmentReceivable.F.Quantity");
        this.UnitPriceText = TextCodeTranslator.Translate("ShipmentReceivable.F.UnitPrice");
        this.AmountText = TextCodeTranslator.Translate("ShipmentReceivable.F.TotalAmount.Short");
        this.AmountLocalText = TextCodeTranslator.Translate("Shipment.O.Receivables.AmountLocal").replace("%LocalCurrencyCode", SessionLocator.LocalCurrencyCode);
    }

    BuildObsList() {
        var itemsCollection: GenerateFromPayablesModelData[] = [];

        this.EntityPM.ShipmentPayables.forEach(item => {
            itemsCollection.push(new GenerateFromPayablesModelData(item, this));
        });

        this.ItemsSource.InsertCollection(itemsCollection);
    }

    private payables: number = null;
    public get Payables() { return this.payables; }
    public set Payables(value: number) {
        if (this.payables != value) {
            this.payables = AppTool.Round(value, 2);
        }
    }

    private receivables: number = null;
    public get Receivables() { return this.receivables; }
    public set Receivables(value: number) {
        if (this.receivables != value) {
            this.receivables = AppTool.Round(value, 2);
        }
    }

    public get Profit() { return this.EntityPM.ProfitInLocalCurrency; }
    public set Profit(value: number) {
        if (this.EntityPM.ProfitInLocalCurrency != value) {
            this.EntityPM.ProfitInLocalCurrency = AppTool.Round(value,2);
        }
    }

    public get EstimateProfit() { return this.EntityPM.EstimateProfitInLocalCurrency; }
    public set EstimateProfit(value: number) {
        if (this.EntityPM.EstimateProfitInLocalCurrency != value) {
            this.EntityPM.EstimateProfitInLocalCurrency = AppTool.Round(value, 2);            
        }
    }

    get ProfitColor() {
        var myResult: string = FontTool.Black;

        if (this.Profit < 0) {
            myResult = FontTool.Red;
        }

        else if (this.Profit > 0) {
            myResult = FontTool.Green;
        }

        return myResult;
    }

    public IsOkButtonEnabled: boolean = false;
    ComputeTotals() {
        var myPayables: number = 0;
        var myReceivables: number = null;

        if (this.ItemsSource.Collection.length > 0) {
            myPayables = ArrayTool.Sum(this.ItemsSource.Collection, "PayableAmountLocal");

            if (this.ItemsSource.Collection.filter(f => f.IsAdded == true).length > 0) {
                myReceivables = ArrayTool.Sum(this.ItemsSource.Collection.filter(f => f.IsAdded == true), "ReceivableAmountLocal");
            }
        }

        this.Payables = myPayables;
        this.Receivables = myReceivables;
    }
    ComputeAmounts() {
        this.ComputeTotals();

        var myProfit: number = 0;
        var isOkButtonEnabled: boolean = false;

        if (this.ItemsSource.Collection.length > 0) {
            if (this.ItemsSource.Collection.filter(f => f.IsAdded == true).length > 0) {
                isOkButtonEnabled = true;

                var myPayables: number = ArrayTool.Sum(this.ItemsSource.Collection, "PayableAmountLocal");
                var myReceivables: number = ArrayTool.Sum(this.ItemsSource.Collection.filter(f => f.IsAdded == true), "ReceivableAmountLocal");
                myProfit = myReceivables - myPayables;
            }
        }

        this.Profit = myProfit == null ? 0 : myProfit;
        this.IsOkButtonEnabled = isOkButtonEnabled;
        this.ComputeWidth();
    }

    public SummaryWidth: number = 250;
    ComputeWidth() {
        var PayablesWidth = 0;
        var RecevableWidth = 0;
        var ProfitWidth = 0;
        var mySummaryWidth = 0;
        var myRightSideWidth = 0;
        var myPipe = new NumbersPipe();

        if (!AppTool.IsNullOrEmpty(this.Payables)) {
            PayablesWidth = AppTool.GetTextWidth(myPipe.transform(this.Payables, 'N2'));
        }

        if (!AppTool.IsNullOrEmpty(this.Receivables)) {
            RecevableWidth = AppTool.GetTextWidth(myPipe.transform(this.Receivables, 'N2'));
        }

        if (!AppTool.IsNullOrEmpty(this.Profit)) {
            ProfitWidth = AppTool.GetTextWidth(myPipe.transform(this.Profit, 'N2'));
        }

        if (PayablesWidth > myRightSideWidth) {
            myRightSideWidth = PayablesWidth;
        }

        if (RecevableWidth > myRightSideWidth) {
            myRightSideWidth = RecevableWidth;
        }

        if (ProfitWidth > myRightSideWidth) {
            myRightSideWidth = ProfitWidth;
        }

        mySummaryWidth = myRightSideWidth + 120 + 50;
        if (mySummaryWidth < 250) {
            mySummaryWidth = 250;
        }

        this.SummaryWidth = mySummaryWidth;
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ItemsSource.Collection.forEach(p => {
            if (p.IsAdded) {
                if (p.ReceivablePM.MeasurementCode == "PFCL") {
                    p.ReceivablePM.Quantity = ArrayTool.Sum(this.ItemsSource.Collection.filter(d => d.IsAdded && d.CurrencyCode != SessionLocator.LocalCurrencyCode && d.MeasurementCode != "PFCL"), "ReceivableAmountLocal");
                    p.ComputeReceivableAmount();
                }
                this.EntityPM.AddReceivable(p.ReceivablePM);
            }
        });

        this.BuildObsList();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    private myCloner: Cloner;
    private linesCloners: Cloner[] = [];
    private linesChildsCloners: Cloner[] = [];
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('Profit');
        this.myCloner.AddField('EstimateProfit');
        this.myCloner.AddEntity(this.EntityPM);

        var lines = this.EntityPM.ShipmentPayables;
        lines.forEach(itemPayablePM => {
            var lineCloner = new Cloner(itemPayablePM);
            lineCloner.AddField('Quantity');
            lineCloner.AddField('UnitPrice');
            lineCloner.AddField('ExpectedAmount');
            lineCloner.AddField('ExpectedAmountLocal');
            lineCloner.AddField('ExpectedAmountInProfitCurrency');
            lineCloner.AddField('OpenAmount');
            lineCloner.AddField('OpenAmountInLocalCurrency');
            lineCloner.AddField('OpenAmountInProfitCurrency');
            lineCloner.AddField('ShipmentPayableLineStatusCode');
            lineCloner.AddField('CorrectionAmount');
            lineCloner.AddField('CorrectionByUserId');
            lineCloner.AddField('CorrectionDate');
            lineCloner.AddEntity(itemPayablePM);
            this.linesCloners.push(lineCloner);

            if (this.EntityPM.ShipmentLevelCode == "C") {
                itemPayablePM.ChildShipmentPayables.forEach(itemChild => {
                    var lineChildCloner = new Cloner(itemChild);
                    lineChildCloner.AddField('Quantity');
                    lineChildCloner.AddField('UnitPrice');
                    lineChildCloner.AddField('ExpectedAmount');
                    lineChildCloner.AddField('ExpectedAmountLocal');
                    lineChildCloner.AddField('ExpectedAmountInProfitCurrency');
                    lineChildCloner.AddField('OpenAmount');
                    lineChildCloner.AddField('OpenAmountInLocalCurrency');
                    lineChildCloner.AddField('OpenAmountInProfitCurrency');
                    lineChildCloner.AddField('AccountedAmount');
                    lineChildCloner.AddField('AccountedAmountInLocalCurrency');
                    lineChildCloner.AddField('AccountedAmountInProfitCurrency');
                    lineChildCloner.AddEntity(itemChild);
                    this.linesChildsCloners.push(lineChildCloner);
                });
            }
        });
    }
    RejectChanges() {
        this.linesCloners.forEach(lineCloner => {
            lineCloner.RejectChanges();
        });

        this.linesChildsCloners.forEach(lineChildCloner => {
            lineChildCloner.RejectChanges();
        });

        this.myCloner.RejectChanges();
    }
}
export class GenerateFromPayablesModelData extends BaseComponent {
    public ShipmentPM: ShipmentPM;
    public PayablePM: ShipmentPayablePM;
    public ReceivablePM: ShipmentReceivablePM;
    constructor(myPayablePM: ShipmentPayablePM, private FatherComponent: PayablesComponent) {
        super();
        this.ShipmentPM = FatherComponent.EntityPM;
        this.PayablePM = myPayablePM;
        this.CreateReceivable();
    }

    CreateReceivable() {
        var chargesType: ChargesTypeList = this.FatherComponent.AllChargesTypes.filter(f => f.Id == this.PayablePM.ChargesTypeId)[0];
        if (chargesType) {
            this.ReceivablePM = new ShipmentReceivablePM(null);
            this.ReceivablePM.Tenant = this.PayablePM.Tenant;
            this.ReceivablePM.ShipmentId = this.ShipmentPM.Id;
            this.ReceivablePM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
            this.ReceivablePM.ChargesTypeId = chargesType.Id;
            this.ReceivablePM.ChargesTypeCode = chargesType.Code;
            this.ReceivablePM.ChargesTypeName = chargesType.EnglishName
            this.ReceivablePM.ChargesGroupCode = chargesType.ChargesGroupCode;
            this.ReceivablePM.CurrencyId = this.PayablePM.CurrencyId;
            this.ReceivablePM.CurrencyCode = this.PayablePM.CurrencyCode;
            this.ReceivablePM.DueTypeCode = chargesType.DueTypeCode;
            this.ReceivablePM.IATACodeId = chargesType.IATACodeId;
            this.ReceivablePM.MeasurementId = this.PayablePM.MeasurementId;
            this.ReceivablePM.MeasurementCode = this.PayablePM.MeasurementCode;
            this.ReceivablePM.MeasurementShortName = this.PayablePM.MeasurementShortName;
            this.ReceivablePM.Quantity = this.PayablePM.Quantity;
            this.ReceivablePM.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? this.ShipmentPM.FreightPrepaidCollectId : this.ShipmentPM.OtherPrepaidCollectId;
            this.ReceivablePM.Rate = this.PayablePM.Rate;
            this.ReceivablePM.TotalAmount = this.PayablePM.ExpectedAmount;
            this.ReceivablePM.TotalAmountLocal = this.PayablePM.ExpectedAmountLocal;
            this.ReceivablePM.UnitPrice = this.PayablePM.UnitPrice;
            this.ReceivablePM.UpdateByUserId = SessionLocator.LoggedUserPM.Id;
            this.ReceivablePM.CreateDate = DateTool.GetCurrentDateAsUtc();
            this.ReceivablePM.CreatedByUserId = SessionLocator.LoggedUserPM.Id;
            this.ReceivablePM.VatTypeId = chargesType.VatTypeId;
            this.ReceivablePM.ViewOrder = chargesType.ViewOrder;
            this.ReceivablePM.ShipmentReceivableLineStatusCode = "OAMT";
            this.ReceivablePM.AmountInProfitCurrency = this.PayablePM.ExpectedAmountInProfitCurrency;
            this.ReceivablePM.ProfitCurrencyExchangeRate = this.PayablePM.ProfitCurrencyExchangeRate;
            this.ReceivablePM.DueTypeName = this.PayablePM.DueTypeName;
            this.ReceivablePM.IsExpense = chargesType.IsExpense;
        }
    }

    public get ChargesTypeCode() { return this.PayablePM.ChargesTypeCode;    }
    public get ChargesTypeName() { return this.PayablePM.ChargesTypeName; }
    public get MeasurementCode() { return this.PayablePM.MeasurementCode; }
    public get CurrencyCode() { return this.PayablePM.CurrencyCode; }

    // Payable
    public get PayableQuantity() { return this.PayablePM.Quantity; }
    public set PayableQuantity(value: number) {
        if (this.PayablePM.Quantity != value) {
            this.PayablePM.Quantity = AppTool.Round(value, 2);

            if (this.PayablePM.IsChargeBySteps) {
                ShipmentTool.SetPayableUnitPriceBySteps(this.PayablePM, this.FatherComponent.BaseQuote);
            }

            this.ComputePayableTotalAmount();
        }
    }

    public get PayableUnitPrice() { return this.PayablePM.UnitPrice; }
    public set PayableUnitPrice(value: number) {
        if (this.PayablePM.UnitPrice != value) {
            this.PayablePM.UnitPrice = AppTool.Round(value, 3);
            this.ComputePayableTotalAmount();
        }
    }

    public get PayableAmount() { return this.PayablePM.ExpectedAmount;    }
    public set PayableAmount(value: number) {
        if (this.PayablePM.ExpectedAmount != value) {
            this.PayablePM.ExpectedAmount = AppTool.Round(value, 2);
            this.SetLineStatus();
            this.ComputeUnitPrice();
            this.ComputePayableTotalAmountLocal();
        }
    }

    public get PayableAmountLocal() { return this.PayablePM.ExpectedAmountLocal; }
    public set PayableAmountLocal(value: number) {
        if (this.PayablePM.ExpectedAmountLocal != value) {
            this.PayablePM.ExpectedAmountLocal = AppTool.Round(value, 2);

            if (this.ShipmentPM.ShipmentLevelCode == "C") {
                this.ComputeInsidePayablesData();
            }
        }
    }

    public get PayableAmountInProfitCurrency() { return this.PayablePM.ExpectedAmountInProfitCurrency; }
    public set PayableAmountInProfitCurrency(value: number) {
        if (this.PayablePM.ExpectedAmountInProfitCurrency != value) {
            this.PayablePM.ExpectedAmountInProfitCurrency = AppTool.Round(value, 2);
        }
    }

    public get PayableOpenAmount() { return this.PayablePM.OpenAmount; }
    public set PayableOpenAmount(value: number) {
        if (this.PayablePM.OpenAmount != value) {
            this.PayablePM.OpenAmount = AppTool.Round(value, 2);

            this.OpenAmountInLocalCurrency = value * this.PayablePM.Rate;
            this.OpenAmountInProfitCurrency = this.OpenAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;

            var expe: number = this.PayablePM.ExpectedAmount == null ? 0 : this.PayablePM.ExpectedAmount;
            var acct: number = this.PayablePM.AccountedAmount == null ? 0 : this.PayablePM.AccountedAmount;
            var open: number = value == null ? 0 : value;
            this.CorrectionAmount = expe - acct - open;

            if (!AppTool.IsNullOrEmpty(this.PayablePM.CorrectionByUserId)) {
                this.SetLineStatus();
            }
        }
    }

    public get OpenAmountInLocalCurrency() { return this.PayablePM.OpenAmountInLocalCurrency; }
    public set OpenAmountInLocalCurrency(value: number) {
        if (this.PayablePM.OpenAmountInLocalCurrency != value) {
            this.PayablePM.OpenAmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    public get OpenAmountInProfitCurrency() { return this.PayablePM.OpenAmountInProfitCurrency; }
    public set OpenAmountInProfitCurrency(value: number) {
        if (this.PayablePM.OpenAmountInProfitCurrency != value) {
            this.PayablePM.OpenAmountInProfitCurrency = AppTool.Round(value, 2);
        }
    }

    public get CorrectionAmount() { return this.PayablePM.CorrectionAmount; }
    public set CorrectionAmount(value: number) {
        if (this.PayablePM.CorrectionAmount != value) {
            this.PayablePM.CorrectionAmount = AppTool.Round(value, 2);
            this.CorrectionByUserId = SessionLocator.LoggedUserId;
            this.CorrectionDate = DateTool.GetCurrentDateAsUtc();
        }
    }

    public get CorrectionByUserId() { return this.PayablePM.CorrectionByUserId; }
    public set CorrectionByUserId(value: string) {
        if (this.PayablePM.CorrectionByUserId != value) {
            this.PayablePM.CorrectionByUserId = value;
        }
    }

    public get CorrectionDate() { return this.PayablePM.CorrectionDate; }
    public set CorrectionDate(value: Date) {
        if (this.PayablePM.CorrectionDate != value) {
            this.PayablePM.CorrectionDate = value;
        }
    }

    SetLineStatus() {
        ShipmentTool.SetPayableLineStatus(this.PayablePM);
    }
    ComputeUnitPrice() {
        if (!this.PayablePM.IsChargeBySteps) {
            var myResult: number = null;

            if (this.PayablePM.ExpectedAmount != null && this.PayablePM.Quantity != null) {
                if (this.PayablePM.Quantity == 0) {
                    myResult = 0;
                }

                else {
                    myResult = this.PayablePM.ExpectedAmount / this.PayablePM.Quantity;
                }
            }

            this.PayablePM.UnitPrice = AppTool.Round(myResult, 3);
        }
    }
    ComputePayableTotalAmount() {
        var myResult: number = null;

        this.SetLineStatus();

        if (this.PayablePM.Quantity != null && this.PayablePM.UnitPrice != null) {
            myResult = this.PayablePM.Quantity * this.PayablePM.UnitPrice;
        }

        /* From Tarrifs */
        if (this.PayablePM.MinAmount != null || this.PayablePM.MaxAmount != null) {
            if (myResult != null) {
                if (myResult < this.PayablePM.MinAmount) {
                    myResult = this.PayablePM.MinAmount;
                }

                else if (myResult > this.PayablePM.MaxAmount) {
                    myResult = this.PayablePM.MaxAmount;
                }
            }
        }

        if (this.PayablePM.QuoteCostMinAmount != null) {
            if (myResult == null) {
                myResult = this.PayablePM.QuoteCostMinAmount;
            }

            else {
                if (myResult < this.PayablePM.QuoteCostMinAmount) {
                    myResult = this.PayablePM.QuoteCostMinAmount;
                }
            }
        }

        this.PayablePM.ExpectedAmount = AppTool.Round(myResult, 2);
        this.ComputePayableTotalAmountLocal();
    }
    ComputePayableTotalAmountLocal() {
        if (this.PayablePM.ExpectedAmount != null && this.PayablePM.Rate != null) {
            this.PayableAmountLocal = AppTool.Round(this.PayablePM.ExpectedAmount * this.PayablePM.Rate, 2);

        }

        else {
            this.PayableAmountLocal = null;
        }

        this.ComputePayableTotalAmountInProfitCurrency();
        this.ComputeInsidePayablesData();
        this.FatherComponent.ComputeAmounts();
    }
    ComputePayableTotalAmountInProfitCurrency() {
        if (this.PayablePM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.PayableAmountInProfitCurrency = this.PayablePM.ExpectedAmount;
        }

        else {
            this.PayableAmountInProfitCurrency = this.PayableAmountLocal / this.PayablePM.ProfitCurrencyExchangeRate;
        }

        this.ComputePayableOtherAmounts();
    }
    ComputePayableOtherAmounts() {
        if (this.PayablePM.ShipmentPayableLineStatusCode == "EMPT" || this.PayablePM.ShipmentPayableLineStatusCode == "OAMT") {
            this.PayablePM.CorrectionAmount = 0;
            this.PayablePM.AccountedAmount = 0;
            this.PayablePM.AccountedAmountInLocalCurrency = 0;
            this.PayablePM.AccountedAmountInProfitCurrency = 0;

            if (this.PayablePM.OpenAmount != this.PayablePM.ExpectedAmount) {
                this.PayableOpenAmount = this.PayablePM.ExpectedAmount;
            }

            else {
                this.OpenAmountInLocalCurrency = this.PayablePM.OpenAmount * this.PayablePM.Rate;
                this.OpenAmountInProfitCurrency = this.OpenAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;
            }
        }
    }
    ComputeInsidePayablesData() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            var _QuantityTotal = null;
            var _Ratio = null;
            var quantity = null;
            var unitPrice = null;

            this.PayablePM.ChildShipmentPayables.forEach(item => {
                switch (this.MeasurementCode) {
                    case "VOLU": {
                        _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "Volume");
                        _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].Volume;
                        break;
                    }

                    case "GRWT": {
                        _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "GrossWeight");
                        _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].GrossWeight;
                        break;
                    }

                    case "GWTN": {
                        _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "GrossWeightPerTon");
                        _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].GrossWeightPerTon;
                        break;
                    }

                    case "QTY": {
                        if (ShipmentTool.IsLCL(this.ShipmentPM)) {
                            _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "NumberOfPackages");
                            _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                            unitPrice = _Ratio * this.PayablePM.UnitPrice;
                            quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].NumberOfPackages;
                        }

                        else {
                            _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "NumberOfContainers");
                            _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                            unitPrice = _Ratio * this.PayablePM.UnitPrice;
                            quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].NumberOfContainers;
                        }
                        break;
                    }

                    case "CHWT": {
                        _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "ChargeableWeight");
                        _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].ChargeableWeight;
                        break;
                    }

                    case "BTEU": {
                        _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "TEU");
                        _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].TEU;
                        break;
                    }

                    case "FIXD": {
                        _Ratio = this.PayablePM.Quantity / this.ShipmentPM.ShipmentConsoleShipments.length;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].TEU;
                        break;
                    }

                    case "PFCL": {
                        _Ratio = this.PayablePM.Quantity / this.ShipmentPM.ShipmentConsoleShipments.length;
                        unitPrice = _Ratio * this.PayablePM.UnitPrice;
                        quantity = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(d => d.CurrencyId != SessionLocator.LocalCurrencyId && d.MeasurementCode != "PFCL"), "ExpectedAmountLocal");
                        break;
                    }

                    default: {
                        if ((this.ShipmentPM.TransportModeId == "O" && this.ShipmentPM.ShipmentTypeId == "FCLD") || (this.ShipmentPM.TransportModeId == "I" && this.ShipmentPM.ShipmentTypeId == "FTL")) {
                            var list: PackageTypeList = this.FatherComponent.AllPackageTypes.filter(d => d.MeasurementId == this.PayablePM.MeasurementId && d.Tenant == this.PayablePM.Tenant)[0];
                            if (list) {
                                var houseRecord: ConsoleShipmentPM = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0];
                                if (houseRecord != null) {
                                    var fclData = houseRecord.FCLDataList.filter(d => d.Id == list.Id)[0];
                                    if (fclData != null) {
                                        quantity = fclData.Quantity;
                                    }
                                }

                                unitPrice = this.PayablePM.UnitPrice;
                            }
                        }

                        else {
                            // Groupage by Chargeable
                            _QuantityTotal = ArrayTool.Sum(this.ShipmentPM.ShipmentConsoleShipments, "ChargeableWeight");
                            _Ratio = this.PayablePM.Quantity / _QuantityTotal;
                            unitPrice = _Ratio * this.PayablePM.UnitPrice;
                            quantity = this.ShipmentPM.ShipmentConsoleShipments.filter(d => d.Id == item.ShipmentId)[0].ChargeableWeight;
                        }

                        break;
                    }
                }

                item.Quantity = AppTool.Round(quantity, 2);
                item.UnitPrice = AppTool.Round(unitPrice, 3);
                
                var expectedAmount = quantity * unitPrice;
                var expectedAmountLocal = expectedAmount * this.PayablePM.Rate;
                var expectedAmountInProfitCurrency = expectedAmountLocal / this.PayablePM.ProfitCurrencyExchangeRate;

                item.ExpectedAmount = AppTool.Round(expectedAmount, 2);
                item.ExpectedAmountLocal = AppTool.Round(expectedAmountLocal, 2);
                item.ExpectedAmountInProfitCurrency = AppTool.Round(expectedAmountInProfitCurrency, 2);

                item.OpenAmount = item.ExpectedAmount;
                item.OpenAmountInLocalCurrency = item.ExpectedAmountLocal;
                item.OpenAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;

                item.AccountedAmount = 0;
                item.AccountedAmountInLocalCurrency = 0;
                item.AccountedAmountInProfitCurrency = 0;
            });
        }
    }

    // Receivable
    public get ReceivableQuantity() { return this.ReceivablePM.Quantity; }
    public set ReceivableQuantity(value: number) {
        if (this.ReceivablePM.Quantity != value) {
            this.ReceivablePM.Quantity = AppTool.Round(value, 2);
            this.ComputeReceivableAmount();
        }
    }

    public get ReceivableUnitPrice() { return this.ReceivablePM.UnitPrice; }
    public set ReceivableUnitPrice(value: number) {
        if (this.ReceivablePM.UnitPrice != value) {
            this.ReceivablePM.UnitPrice = AppTool.Round(value, 2);
            this.ComputeReceivableAmount();
        }
    }

    public get ReceivableAmount() { return this.ReceivablePM.TotalAmount; }
    public set ReceivableAmount(value: number) {
        if (this.ReceivablePM.TotalAmount != value) {
            this.ReceivablePM.TotalAmount = AppTool.Round(value, 2);
            this.ComputeReceivableAmountLocal();
        }
    }

    public get ReceivableAmountLocal() { return this.ReceivablePM.TotalAmountLocal; }
    public set ReceivableAmountLocal(value: number) {
        if (this.ReceivablePM.TotalAmountLocal != value) {
            this.ReceivablePM.TotalAmountLocal = AppTool.Round(value, 2);
        }
    }

    public ComputeReceivableAmount() {
        var amount: number = null;
        var amountLocal: number = null;
        var amountProfit: number = null;

        if (!AppTool.IsNullOrEmpty(this.ReceivableQuantity) && !AppTool.IsNullOrEmpty(this.ReceivableUnitPrice)) {
            amount = this.ReceivableQuantity * this.ReceivableUnitPrice;
            amountLocal = amount * this.ReceivablePM.Rate;
            amountProfit = amountLocal / this.ReceivablePM.ProfitCurrencyExchangeRate;
        }

        this.ReceivablePM.TotalAmount = AppTool.Round(amount, 2);
        this.ReceivablePM.TotalAmountLocal = AppTool.Round(amountLocal, 2);
        this.ReceivablePM.AmountInProfitCurrency = AppTool.Round(amountProfit, 2);
        this.ReceivablePM.ShipmentReceivableLineStatusCode = (this.ReceivablePM.Quantity != null && this.ReceivablePM.UnitPrice != null) ? "OAMT" : "EMPT";

        this.ComputeMarkp();
        this.FatherComponent.ComputeAmounts();
    }
    ComputeReceivableAmountLocal() {
        var amountLocal: number = this.ReceivableAmount * this.ReceivablePM.Rate;
        var amountProfit: number = amountLocal / this.ReceivablePM.ProfitCurrencyExchangeRate;
        this.ReceivableAmountLocal = amountLocal;
        this.ReceivablePM.AmountInProfitCurrency = AppTool.Round(amountProfit, 2);
    }


    private markup: string = "0";
    public get Markup() {
        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(this.markup)) {
            var str: string = this.markup.replace("%", "");

            var myPipe = new NumbersPipe();
            myResult = myPipe.transform(AppTool.Round(parseFloat(str), 3), "N3") + "";

            if (this.markup.includes("%")) {
                myResult = myResult + "%";
            }
        }
        
        return myResult;
    }
    public set Markup(value: string) {
        this.markup = value;

        if (this.IsAdded == false) {
            this. IsAdded = true;
        }

        this.OnMarkUpChanged();
    }

    OnMarkUpChanged() {
        var result: number = this.PayablePM.ExpectedAmount;

        if (this.PayablePM.UnitPrice != null && !AppTool.IsNullOrEmpty(this.markup)) {
            if (this.Markup.includes("%")) {
                var markupValue: number = parseFloat(this.markup.replace("%", ""));
                result = this.PayablePM.ExpectedAmount + (this.PayablePM.ExpectedAmount * (markupValue / 100));
            }

            else {
                var markupValue: number = parseFloat(this.markup);
                result = this.PayablePM.ExpectedAmount + markupValue;
            }
        }

        this.ReceivableAmount = result;

        if (this.ReceivablePM.Quantity != null && result != null) {
            this.ReceivablePM.UnitPrice = AppTool.Round(result / this.ReceivablePM.Quantity, 3);
        }

        this.FatherComponent.ComputeAmounts();
    }
    ComputeMarkp() {
        var result: number = 0;

        if (this.PayablePM.ExpectedAmount != null && this.ReceivablePM.TotalAmount != null) {

            if (this.markup.includes("%")) {
                result = ((this.ReceivablePM.TotalAmount - this.PayablePM.ExpectedAmount) * 100) / this.PayablePM.ExpectedAmount;
            }

            else {
                result = this.ReceivablePM.TotalAmount - this.PayablePM.ExpectedAmount;
            }
        }

        var str: string = result + "";
        if (this.markup.includes("%")) {
            str = str + "%";
        }

        this.markup = str;       
    }


    private isAdded: boolean = false;
    public get IsAdded() { return this.isAdded; }
    public set IsAdded(value: boolean) {
        if (this.isAdded != value) {
            this.isAdded = value;
            this.FatherComponent.ComputeAmounts();
        }
    }

}
