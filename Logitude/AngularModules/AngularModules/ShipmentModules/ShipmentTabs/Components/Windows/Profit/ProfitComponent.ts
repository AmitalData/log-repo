import { Component } from '@angular/core';
import { AppTool, FontTool } from '../../../../../Infrastructure/Tools';
import { NumbersPipe } from '../../../../../Infrastructure/Pipes/NumbersPipe';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { ShipmentPM } from '../../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPayablePM } from '../../../../../Shipment/EntityPMs/ShipmentPayablePM';
import { ShipmentReceivablePM } from '../../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import { ShipmentDomainService } from '../../../../../Shipment/Services/ShipmentDomainService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ChargesTypeListService } from '../../../../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../../../../Common/EntityLists/ChargesTypeList';

@Component({

    templateUrl: './ProfitComponent.html',
})

export class ProfitComponent {
    public EntityPM: ShipmentPM;
    public IsByLocalCurrency: boolean = false;
    public IsCurrencyFilterVisible: boolean = false;
    public IsAccrualsApprovingVisible: boolean = false;
    public LocalCurrencyCode: string = null;
    public ProfitCurrencyCode: string = null;
    public SelectedCurrencyCode: string = null;
    public ProfitsCollection: ProfitClass[] = [];
    private myDomainService: ShipmentDomainService;
    private chargesTypeListService: ChargesTypeListService;
    private CurrentSession = SessionLocator.SelectedSession;
    accrualsApprovementToggle: any;
    constructor() {
        this.myDomainService = new ShipmentDomainService();
        this.chargesTypeListService = new ChargesTypeListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['ShipmentPM'];
        this.IsByLocalCurrency = args['IsByLocalCurrency'];
        this.IsCurrencyFilterVisible = args['IsCurrencyFilterVisible'];
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ProfitCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.SelectedCurrencyCode = this.IsByLocalCurrency ? this.LocalCurrencyCode : this.ProfitCurrencyCode;
        this.accrualsApprovementToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "ACP" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.AccrualsApprovement") && this.accrualsApprovementToggle ) {
            this.IsAccrualsApprovingVisible = true;
        }

        this.SetLabels();
        this.BuildBaseData();
    }

    public ReceivablesHeader: string;
    public PayablesHeader: string;
    public ProfitHeader: string;
    SetLabels() {
        this.ReceivablesHeader = TextCodeTranslator.Translate('Shipment.S.Profit.Receivables').replace('%LocalCurrencyCode', this.SelectedCurrencyCode);
        this.PayablesHeader = TextCodeTranslator.Translate('Shipment.S.Profit.Payables').replace('%LocalCurrencyCode', this.SelectedCurrencyCode);
        this.ProfitHeader = TextCodeTranslator.Translate('Shipment.S.Profit.Profit').replace('%LocalCurrencyCode', this.SelectedCurrencyCode);
    }

    private isPayablesExists: boolean = false;
    private isReceivablesExists: boolean = false;
    private allHousesIdsString: string = null;
    private AllPayables: ShipmentPayablePM[] = [];
    private AllRecievables: ShipmentReceivablePM[] = [];
    BuildBaseData() {

        if (this.EntityPM.ShipmentLevelCode != "C" || this.EntityPM.ShipmentConsoleShipments.length == 0) {
            if (this.EntityPM.ShipmentPayables.length > 0) {
                this.isPayablesExists = true;
            }

            if (this.EntityPM.ShipmentReceivables.length > 0) {
                this.isReceivablesExists = true;
            }

            this.AllPayables = this.EntityPM.ShipmentPayables;
            this.AllRecievables = this.EntityPM.ShipmentReceivables;
            this.BuildProfitData();
        }

        else {

            if (this.EntityPM.ConnectedShipmentsPayablesCount > 0) {
                this.isPayablesExists = true;
            }

            if (this.EntityPM.ProrateReceivables) {
                if (this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                    this.isReceivablesExists = true;
                }
            }

            else {
                if (this.EntityPM.ShipmentReceivables.length > 0 || this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                    this.isReceivablesExists = true;
                }
            }

            var housesIds: string[] = [];
            this.EntityPM.ShipmentConsoleShipments.forEach(item => {
                housesIds.push(item.Id);
            });

            this.allHousesIdsString = AppTool.GetIdsArrayText(housesIds);

            if (this.EntityPM.ProrateReceivables == false) {
                this.AllRecievables = this.EntityPM.ShipmentReceivables;
            }

            if (this.EntityPM.ProrateReceivables && this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                this.LoadMasterHousesReceivables();
            }

            else if (this.EntityPM.ConnectedShipmentsPayablesCount > 0) {
                this.LoadMasterHousesPayables();
            }

            else {
                this.BuildProfitData();
            }
        }
    }

    LoadMasterHousesPayables() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetAllMasterHousesPayables(this.allHousesIdsString).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllPayables = myResponse.Result;

                this.BuildProfitData();
            }
        });
    }
    LoadMasterHousesReceivables() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetAllMasterHousesReceivables(this.allHousesIdsString).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                var loadedList: ShipmentReceivablePM[] = myResponse.Result;
                loadedList.forEach((item: ShipmentReceivablePM) => {
                    this.AllRecievables.push(item);
                });

                this.LoadMasterHousesPayables();
            }
        });
    }

    BuildProfitData() {
        this.CurrentSession.StopBusyIndicator();

        this.ProfitsCollection = [];

        if (this.IsByLocalCurrency) {
            this.BuildDataInLocalCurrency();
        }

        else {
            this.BuildDataInProfitCurrency();
        }

        this.FilterExpense();
        this.ComputeTotals();
    }
    BuildDataInLocalCurrency() {
        // Receivables Group by
        var myReceivables: ProfitClass[] = [];
        this.AllRecievables.forEach(itemReceivable => {
            if (!AppTool.IsNullOrEmpty(itemReceivable.TotalAmountLocal)) {
                var itemReceivableOpenedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "OAMT" || itemReceivable.ShipmentReceivableLineStatusCode == "DRFT") {
                    if (!AppTool.IsNullOrEmpty(itemReceivable.TotalAmountLocal)) {
                        itemReceivableOpenedAmount = itemReceivable.TotalAmountLocal;
                    }
                }

                var itemReceivableAcountedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "ACCT") {
                    if (!AppTool.IsNullOrEmpty(itemReceivable.TotalAmountLocal)) {
                        itemReceivableAcountedAmount = itemReceivable.TotalAmountLocal;
                    }
                }

                var itemGrouped: ProfitClass = myReceivables.filter(f => f.ChargeTypeId == itemReceivable.ChargesTypeId)[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemReceivable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemReceivable.ChargesTypeName;
                    itemGrouped.ReceivableOpenedAmount = itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount = itemReceivableAcountedAmount;
                    myReceivables.push(itemGrouped);
                }

                else {
                    itemGrouped.ReceivableOpenedAmount += itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount += itemReceivableAcountedAmount;
                }
            }
        });

        // Payables Group by
        var myPayables: ProfitClass[] = [];
        this.AllPayables.forEach(itemPayable => {
            if (!AppTool.IsNullOrEmpty(itemPayable.OpenAmountInLocalCurrency) || !AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInLocalCurrency)) {
                var itemPayableOpenedAmount = 0;
                if (!AppTool.IsNullOrEmpty(itemPayable.OpenAmountInLocalCurrency)) {
                    itemPayableOpenedAmount = itemPayable.OpenAmountInLocalCurrency;
                }

                var itemPayableAcountedAmount = 0;
                if (!AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInLocalCurrency)) {
                    itemPayableAcountedAmount = itemPayable.AccountedAmountInLocalCurrency;
                }

                var itemGrouped: ProfitClass = myPayables.filter(f => f.ChargeTypeId == itemPayable.ChargesTypeId)[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemPayable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemPayable.ChargesTypeName;
                    itemGrouped.PayableOpenedAmount = itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount = itemPayableAcountedAmount;
                    myPayables.push(itemGrouped);
                }

                else {
                    itemGrouped.PayableOpenedAmount += itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount += itemPayableAcountedAmount;
                }
            }
        });

        // Push Payables
        myPayables.forEach(item => {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.PayableOpenedAmount = this.Fixed(item.PayableOpenedAmount);
            record.PayableAcountedAmount = this.Fixed(item.PayableAcountedAmount);
            record.PayableAmount = this.Fixed(item.PayableOpenedAmount + item.PayableAcountedAmount);

            var rec: ProfitClass = myReceivables.filter(f => f.ChargeTypeId == item.ChargeTypeId)[0];
            if (rec != null) {
                myReceivables.splice(myReceivables.indexOf(rec), 1);
                record.ReceivableOpenedAmount = this.Fixed(rec.ReceivableOpenedAmount);
                record.ReceivableAcountedAmount = this.Fixed(rec.ReceivableAcountedAmount);
                record.ReceivableAmount = this.Fixed(rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount);
                record.Profit = (rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount) - (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }

            else {
                record.Profit = -1 * (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }

            this.ProfitsCollection.push(record);
        });

        // Push Receivables
        myReceivables.forEach(item => {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.ReceivableOpenedAmount = this.Fixed(item.ReceivableOpenedAmount);
            record.ReceivableAcountedAmount = this.Fixed(item.ReceivableAcountedAmount);
            record.ReceivableAmount = this.Fixed(item.ReceivableOpenedAmount + item.ReceivableAcountedAmount);
            record.Profit = item.ReceivableOpenedAmount + item.ReceivableAcountedAmount;

            this.ProfitsCollection.push(record);
        });
    }
    BuildDataInProfitCurrency() {

        // Receivables Group by
        var myReceivables: ProfitClass[] = [];
        this.AllRecievables.forEach(itemReceivable => {
            if (!AppTool.IsNullOrEmpty(itemReceivable.AmountInProfitCurrency)) {
                var itemReceivableOpenedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "OAMT" || itemReceivable.ShipmentReceivableLineStatusCode == "DRFT") {
                    if (!AppTool.IsNullOrEmpty(itemReceivable.AmountInProfitCurrency)) {
                        itemReceivableOpenedAmount = itemReceivable.AmountInProfitCurrency;
                    }
                }

                var itemReceivableAcountedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "ACCT") {
                    if (!AppTool.IsNullOrEmpty(itemReceivable.AmountInProfitCurrency)) {
                        itemReceivableAcountedAmount = itemReceivable.AmountInProfitCurrency;
                    }
                }

                var itemGrouped: ProfitClass = myReceivables.filter(f => f.ChargeTypeId == itemReceivable.ChargesTypeId)[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemReceivable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemReceivable.ChargesTypeName;
                    itemGrouped.ReceivableOpenedAmount = itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount = itemReceivableAcountedAmount;
                    myReceivables.push(itemGrouped);
                }

                else {
                    itemGrouped.ReceivableOpenedAmount += itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount += itemReceivableAcountedAmount;
                }
            }
        });

        // Payables Group by
        var myPayables: ProfitClass[] = [];
        this.AllPayables.forEach(itemPayable => {
            if (!AppTool.IsNullOrEmpty(itemPayable.OpenAmountInProfitCurrency) || !AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInProfitCurrency)) {
                var itemPayableOpenedAmount = 0;
                if (!AppTool.IsNullOrEmpty(itemPayable.OpenAmountInProfitCurrency)) {
                    itemPayableOpenedAmount = itemPayable.OpenAmountInProfitCurrency;
                }

                var itemPayableAcountedAmount = 0;
                if (!AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInProfitCurrency)) {
                    itemPayableAcountedAmount = itemPayable.AccountedAmountInProfitCurrency;
                }

                var itemGrouped: ProfitClass = myPayables.filter(f => f.ChargeTypeId == itemPayable.ChargesTypeId)[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemPayable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemPayable.ChargesTypeName;
                    itemGrouped.PayableOpenedAmount = itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount = itemPayableAcountedAmount;
                    myPayables.push(itemGrouped);
                }

                else {
                    itemGrouped.PayableOpenedAmount += itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount += itemPayableAcountedAmount;
                }
            }
        });

        // Push Payables
        myPayables.forEach(item => {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.PayableOpenedAmount = this.Fixed(item.PayableOpenedAmount);
            record.PayableAcountedAmount = this.Fixed(item.PayableAcountedAmount);
            record.PayableAmount = this.Fixed(item.PayableOpenedAmount + item.PayableAcountedAmount);

            var rec: ProfitClass = myReceivables.filter(f => f.ChargeTypeId == item.ChargeTypeId)[0];
            if (rec != null) {
                myReceivables.splice(myReceivables.indexOf(rec), 1);
                record.ReceivableOpenedAmount = this.Fixed(rec.ReceivableOpenedAmount);
                record.ReceivableAcountedAmount = this.Fixed(rec.ReceivableAcountedAmount);
                record.ReceivableAmount = this.Fixed(rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount);
                record.Profit = (rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount) - (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }

            else {
                record.Profit = -1 * (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }

            this.ProfitsCollection.push(record);
        });

        // Push Receivables
        myReceivables.forEach(item => {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.ReceivableOpenedAmount = this.Fixed(item.ReceivableOpenedAmount);
            record.ReceivableAcountedAmount = this.Fixed(item.ReceivableAcountedAmount);
            record.ReceivableAmount = this.Fixed(item.ReceivableOpenedAmount + item.ReceivableAcountedAmount);
            record.Profit = item.ReceivableOpenedAmount + item.ReceivableAcountedAmount;

            this.ProfitsCollection.push(record);
        });
    }

    public TotalProfit: number = null;
    public TotalProfitText: string = "N/A";
    public TotalPayablesText: string = "N/A";
    public TotalReceivablesText: string = "N/A";
    public EstimateProfit: number = null;
    public Difference: number = null;
    FilterExpense() {

        if (!this.ExpenseChargesIncluded) {

            this.chargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var allChargesTypes: ChargesTypeList[] = myResponse.Result;

                    this.ProfitsCollection.forEach(item => {
                        var list: ChargesTypeList = allChargesTypes.filter(f => f.Id == item.ChargeTypeId)[0];
                        if (list) {
                            item.ChargeTypeIsExpense = list.IsExpense;
                        }
                    });

                    this.ProfitsCollection = this.ProfitsCollection.filter(f => f.ChargeTypeIsExpense == false);
                }
            });
        }
    }
    ComputeTotals() {
        var myTotalProfitText = "N/A";
        var myTotalPayablesText = "N/A";
        var myTotalReceivablesText = "N/A";

        var myProfit: number = null;
        var myEstimateProfit: number = null;
        var myDifference: number = null;

        if (this.ProfitsCollection.length > 0) {
            let myTotalProfit: number = 0;
            let myTotalPayables: number = 0;
            let myTotalReceivables: number = 0;

            this.ProfitsCollection.forEach(item => {

                if (this.isPayablesExists) {
                    if (item.PayableOpenedAmount != null) {
                        myTotalPayables += item.PayableOpenedAmount;
                    }

                    if (item.PayableAcountedAmount != null) {
                        myTotalPayables += item.PayableAcountedAmount;
                    }
                }

                if (this.isReceivablesExists) {
                    if (item.ReceivableOpenedAmount != null) {
                        myTotalReceivables += item.ReceivableOpenedAmount;
                    }

                    if (item.ReceivableAcountedAmount != null) {
                        myTotalReceivables += item.ReceivableAcountedAmount;
                    }
                }

                if (item.Profit != null) {
                    myTotalProfit += item.Profit;
                }
            });

            var myPipe = new NumbersPipe();

            if (this.isPayablesExists) {
                myTotalPayablesText = myPipe.transform(myTotalPayables, 'N2');
            }

            if (this.isReceivablesExists) {
                myTotalReceivablesText = myPipe.transform(myTotalReceivables, 'N2');
            }

            if (this.isPayablesExists || this.isReceivablesExists) {
                myProfit = myTotalProfit;
                myTotalProfitText = myPipe.transform(myTotalProfit, 'N2');
            }
        }

        this.TotalProfit = myProfit;
        this.TotalProfitText = myTotalProfitText;
        this.TotalPayablesText = myTotalPayablesText;
        this.TotalReceivablesText = myTotalReceivablesText;

        if (this.IsByLocalCurrency) {
            myEstimateProfit = this.EntityPM.EstimateProfitInLocalCurrency;
        }

        else {
            myEstimateProfit = this.EntityPM.EstimateProfitInProfitCurrency;
        }

        if (!AppTool.IsNullOrEmpty(myProfit) && !AppTool.IsNullOrEmpty(myEstimateProfit)) {
            myDifference = myProfit - myEstimateProfit;
        }

        this.EstimateProfit = myEstimateProfit;
        this.Difference = myDifference;
    }
    Fixed(value: number) {
        return value == 0 ? null : value;
    }
    OnSelectCurrency(myCurrencyCode: string) {
        this.SelectedCurrencyCode = myCurrencyCode;

        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }

        else {
            this.IsByLocalCurrency = false;

        }

        this.SetLabels();
        this.BuildProfitData();
    }

    private SaveCompletedEvent: any = null;
    OnAccrualsApprovingClicked() {

        if (!this.EntityPM.IsAccrualsApproved) {

            this.EntityPM.IsAccrualsApproved = true;

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = null;
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.SetUIProperties_MainCarriage();
                        //this.myCloner.AddField('Master');
                        //this.myCloner.AddField('MAWBOBLDate');
                        //this.myCloner.AddField('MAWBStackNumber');
                        //this.myCloner.AddField('MAWBTakenFromStack');
                        //this.myCloner.AddField('MAWBReturnedToStack');
                        //this.myCloner.AddField('MainCarriageIsFromStack');
                    }

                    else {
                        //this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                    }

                    AppTool.KillEventEmitter(this.SaveCompletedEvent);
                    this.SaveCompletedEvent = null;
                });

                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        }
    }

    private expenseChargesIncluded: boolean = true;
    get ExpenseChargesIncluded() { return this.expenseChargesIncluded; }
    set ExpenseChargesIncluded(value: boolean) {
        if (this.expenseChargesIncluded != value) {
            this.expenseChargesIncluded = value;
            this.BuildProfitData();
        }
    }
}

class ProfitClass {
    public ChargeTypeId: string = null;
    public ChargeTypeName: string = null;
    public ChargeTypeIsExpense: boolean = false;
    public ReceivableOpenedAmount: number = null;
    public ReceivableAcountedAmount: number = null;
    public PayableOpenedAmount: number = null;
    public PayableAcountedAmount: number = null;
    public ReceivableAmount: number = null;
    public PayableAmount: number = null;
    public Profit: number = null;
}
