import { Component } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { InterestReportLinesByDatePM } from '../../../../../EntityPMs/InterestReportLinesByDatePM';
import { InterestTransactionExtendedListService } from '../../../../../Services/ExtendedLists/InterestTransactionExtendedListService';
import { InterestTransactionList } from '../../../../../EntityLists/InterestTransactionList';
import { ServiceResponse } from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../../../Infrastructure/Locators/ObjectsLocator';
import { AccountingEntityHelper } from '../../../../../Utilities/AccountingEntityHelper';
import { AppTool } from '../../../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';


@Component({

    templateUrl: './InterestReportLineByDateDetailsComponent.html',
})

export class InterestReportLineByDateDetailsComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: InterestReportLinesByDatePM;
    public myService: InterestTransactionExtendedListService;
    public InterestTransactions: ObservableCollection;
    public InterestLineDataList: ObservableCollection;
    public TotalInterests: number;
    public TotalAmount: number;
    public isRTL: boolean = false;
    public IconCode: string = null;
    constructor() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.myService = new InterestTransactionExtendedListService();
        this.InterestTransactions = new ObservableCollection([]);
        this.InterestLineDataList = new ObservableCollection([]);
    }

    //Grid Header Label
    public InterestEntityTypeCodeHeader = TextCodeTranslator.Translate("InterestTransaction.F.InterestEntityTypeCode");
    public InterestValueDateHeader = TextCodeTranslator.Translate("InterestTransaction.F.InterestValueDate");
    public LocalAmountHeader = TextCodeTranslator.Translate("InterestTransaction.F.LocalAmount");
    public CurrencyCodeHeader = TextCodeTranslator.Translate("InterestTransaction.F.CurrencyCode");
    public ForiegnAmountHeader = TextCodeTranslator.Translate("InterestTransaction.F.ForeignAmount");
    public JournalNumberHeader = TextCodeTranslator.Translate("InterestTransaction.F.JournalNumber");
    public TotalAmountHeader = TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalAmount");
    public TotalInterestHeader = TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalInterest");
    public PercentageHeader = TextCodeTranslator.Translate("InterestReportLinesByDate.O.Percentage");
    public TotalHeader = TextCodeTranslator.Translate("InterestReportLinesByDate.O.Total");
    public InterestTransactionNotes = TextCodeTranslator.Translate("ARInvoice.F.InternalNotes");

    public interestTransactionsWithTotal: InterestTransactionsWithTotal;
    public TotalLocalAmount: number;
    public ReportIsLoading: boolean = false;
    GetAllInterestLinesByDate(InterestReportId: string, InterestCalculationDate: Date) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ReportIsLoading = true;
        this.myService.GetAllInterestTransactionByDate(InterestReportId, InterestCalculationDate).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            this.ReportIsLoading = false;
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.interestTransactionsWithTotal = mm.Result;
                this.TotalLocalAmount = this.interestTransactionsWithTotal.TotalLocalAmount;
                this.InterestTransactions.InsertCollection(this.interestTransactionsWithTotal.interestTransactionLists);
            }
        });
    }
    OpenSource(id: string, sourceTypeCode: string) {

        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = AccountingEntityHelper.getEntityObjectTableName(sourceTypeCode);


        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName,
                    BackButtonLabel: 'Back'
                });
            });

    }
    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    SetDataContext(entityPM: InterestReportLinesByDatePM) {
        this.EntityPM = entityPM;
        this.GetAllInterestLinesByDate(this.EntityPM.InterestReportId, this.EntityPM.FromDate);
        this.ReportIsLoading = true;
        this.BuildInterestLineData();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    InterestLineParameters: InterestLineParameters[] = [];
    BuildInterestLineData() {
        var interestLine1: InterestLineParameters = new InterestLineParameters(TextCodeTranslator.Translate("InterestReportLinesByDate.F.StandardInterestPercentage"), this.EntityPM.StandardInterestPercentage, this.EntityPM.StandardInterestAmount, this.EntityPM.CalculatedStandInterestAmount);
        var interestLine2: InterestLineParameters = new InterestLineParameters(TextCodeTranslator.Translate("InterestReportLinesByDate.F.ExceptionalInterestPercentage"), this.EntityPM.ExceptionalInterestPercentage, this.EntityPM.ExceptionalInterestAmount, this.EntityPM.CalculatedExcepInterestAmount);
        var interestLine3: InterestLineParameters = new InterestLineParameters(TextCodeTranslator.Translate("InterestReportLinesByDate.F.CreditInterestPercentage"), this.EntityPM.CreditInterestPercentage, this.EntityPM.CreditInterestAmount, this.EntityPM.CalculatedCreditInterestAmount);
        this.TotalInterests = this.EntityPM.CalculatedStandInterestAmount + this.EntityPM.CalculatedExcepInterestAmount + this.EntityPM.CalculatedCreditInterestAmount;
        this.TotalAmount = this.EntityPM.StandardInterestAmount + this.EntityPM.ExceptionalInterestAmount + this.EntityPM.CreditInterestAmount;
        this.InterestLineParameters.push(interestLine1);
        this.InterestLineParameters.push(interestLine2);
        this.InterestLineParameters.push(interestLine3);
        this.InterestLineDataList.InsertCollection(this.InterestLineParameters);
    }

    OpenInterestTransactionNote(line: any) {

        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("ARInvoice.F.InternalNotes");
        logWindow.WindowArgs = { interestTransaction: line };
        logWindow.Show('./Accounting/Components/Others/InterestTransactionNotesComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
        });

        
    }
}

export class InterestTransactionsWithTotal {
    public interestTransactionLists: InterestTransactionList[];
    public TotalLocalAmount: number;
}
export class InterestLineParameters {
    public Title: string;
    public Percentage: number;
    public TotalInterest: number;
    public TotalAmount: number;

    constructor(Title: string, Percentage: number, TotalInterest: number, TotalAmount: number) {
        this.Title = Title;
        this.Percentage = Percentage;
        this.TotalInterest = TotalInterest;
        this.TotalAmount = TotalAmount;
    }




}
