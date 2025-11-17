declare var window: any;
import { Component, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { APInvoicePM } from '../../../../Invoice/EntityPMs/APInvoicePM';
import { ExpenseAllocationSettingExtendedService } from 'Invoice/Services/ExtendedPMs/ExpenseAllocationSettingExtendedService';
import { ExpenseAllocationSettingPM } from 'Invoice/EntityPMs/ExpenseAllocationSettingPM';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ExpenseAllocationFlowList } from 'Invoice/EntityLists/ExpenseAllocationFlowList';
import { ExpenseAllocationFlowExtendedService } from 'Invoice/Services/ExtendedPMs/ExpenseAllocationFlowExtendedService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AppTool } from 'Infrastructure/Tools';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { JournalPMService } from 'Accounting/Services/StandardPMs/JournalPMService';
import { JournalPM } from 'Accounting/EntityPMs/JournalPM';
import { FullAccountingSettingPMService } from 'Accounting/Services/StandardPMs/FullAccountingSettingPMService';
import { FullAccountingSettingPM } from 'Accounting/EntityPMs/FullAccountingSettingPM';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    templateUrl: './APInvoicePrepaidExpensesTabComponent.html',
})
export class APInvoicePrepaidExpensesTabComponent implements OnInit {
    public EntityPM: APInvoicePM = null;
    public journalPM: JournalPM = null;
    public ObjectTableName = 'APInvoice';
    public DataContext = this;
    public objectTableId: string = '';
    public amountPaid : number = 0;
    public amountDue : number = 0;
    public originalAmount : number = 0;
    public periodAmount
    public isReady: boolean = false;
    private currentSession = SessionLocator.SelectedSession;
    public isVoided: boolean = false;
    public expenseAllocationSettingPM = new ExpenseAllocationSettingPM();
    expenseAllocationSettingExtendedService =
        new ExpenseAllocationSettingExtendedService();
    expenseAllocationFlowExtendedService =
        new ExpenseAllocationFlowExtendedService();
    journalPMService =
        new JournalPMService();

    expenseAllocationFlowLists: ExpenseAllocationFlowList[] = [];
    entityResourceService: EntityResourceService = new EntityResourceService();
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();

    constructor(private entityArgs: EntityArgs) {
        this.GetResources();
        this.EntityPM = entityArgs.EntityPM;
        this.isVoided = this.EntityPM.StatusCode === "VD";
        var table = window.ObjectTables.filter((d) => d.Name == 'APInvoice')[0];
        if (table) this.objectTableId = table.Id;
    }
    private GetResources() {
        this.entityResourceService
            .getEntityResourceByTableName('ExpenseAllocationFlow')
            .subscribe((response: any) => {
                this.isReady = true;
            });
    }

    ngOnInit() {
        this.loadDate();
    }
    settingsClicked() {
        const defaultEnd = new Date(
            this.expenseAllocationSettingPM?.StartDateTime ||
                this.EntityPM.AccountingDate
        );
        defaultEnd.setDate(defaultEnd.getDate() + 364);
        const paymentType = this.expenseAllocationSettingPM?.PaymentDateType;

        var IsMonthly = false;
        var AllocationDateType = '';
        var selectedDay = null;
        var selectedDayByWeek = null;

        if (paymentType?.startsWith('Monthly_')) {
            IsMonthly = true;
            const parts = paymentType.split('_');
            AllocationDateType = parts[1] || '';          
            selectedDay = parts[2] ? parts[2] : null;
        } else if (paymentType?.startsWith('Weekly_')) {
            IsMonthly = false;
            const parts = paymentType.split('_');
            selectedDayByWeek = parts[1] || null;
        }
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = {
            StartDateTime:
                this.expenseAllocationSettingPM?.StartDateTime ||
                this.EntityPM.AccountingDate,
            EndDateTime:
                this.expenseAllocationSettingPM?.EndDateTime || defaultEnd,
            RecurrenceCount:
                this.expenseAllocationSettingPM?.NumberOfPayments || null,
            MonthInterval: this.expenseAllocationSettingPM?.MonthInterval || 1,
            TotalAmount: this.EntityPM.SubTotalInInvoiceCurrency,
            TotalLocalAmount: this.EntityPM.SubTotalInLocalCurrency,
            CurrencyCode: this.EntityPM.InvoiceCurrencyCode,
            MinDate: this.EntityPM.AccountingDate,
            AllocationDateType: AllocationDateType,
            IsWeekly: !IsMonthly,
            SelectedDay: selectedDay,
            SelectedDayByWeek: selectedDayByWeek,
            Disabled : true
        };
        logWindow.Width = 550;
        logWindow.Height = 380;
        logWindow.Title = TextCodeTranslator.Translate("APInvoice.O.ExpenseAllocationSetting");

        logWindow.ShowCloseButton = true;
        logWindow.ComponentLoaded.subscribe((comp) => {
            logWindow.WindowClosed.subscribe((s) => {});
        });
        logWindow.Show(
            './CommonModules/CommonOthers/Components/RecurringSchedule/RecurringScheduleComponent'
        );
    }
    nonPrepaidPaidAmount: number = 0;
    loadDate() {
        var fullAccountingSettingPM = new FullAccountingSettingPM();
        this.currentSession.StartBusyIndicatorLoading()
        this.fullAccountingSettingPMService.get(SessionLocator.TenantPM.Id.toString()).subscribe((myResult:any) =>{
            if(myResult?.Result && !myResult.HasError){
                 fullAccountingSettingPM = myResult.Result;
                
            }
            this.journalPMService.get(this.EntityPM.JournalId).subscribe(res=>{
                if(res?.Result && !res.HasError){
                    this.journalPM = res.Result;
                    this.originalAmount = this.journalPM?.JournalLines
                    ?.filter(line => line.ActionCode === "2")
                    ?.reduce((sum, line) => sum + (line.LocalAmount || 0), 0);
                    console.log("originalAmount", this.originalAmount);
                }
               
                this.expenseAllocationSettingExtendedService
                .getExpenseAllocationSettingByEntityIdAndObjectTable(
                    this.EntityPM?.Id,
                    this.objectTableId
                )
                .subscribe((res: ServiceResponse) => {
                    if (res?.Result && !res.HasError) {
                        this.expenseAllocationSettingPM = res.Result;
                    }
                    this.expenseAllocationFlowExtendedService
                        .getExpenseAllocationFlowByEntityIdAndObjectTable(
                            this.EntityPM?.Id,
                            this.objectTableId
                        )
                        .subscribe((resFlow) => {
                            if (resFlow !== null) {
                                (resFlow as ExpenseAllocationFlowList[]).forEach((item: ExpenseAllocationFlowList) => {
                                    if(!AppTool.IsNullOrEmpty(item.JournalId) || item.Status === "failed")
                                       this.expenseAllocationFlowLists.push(item);
                                
                                });
                                this.expenseAllocationFlowLists = this.expenseAllocationFlowLists.sort((a, b) => (a.JournalId === this.EntityPM.JournalId ? -1 : b.JournalId === "1" ? 1 : 0))
                                this.nonPrepaidPaidAmount = this.journalPM?.JournalLines
                                ?.filter(line => line.ActionCode === "2" && line.DebitAccountId !== fullAccountingSettingPM?.PrepaidExpensesGLAccountId)    
                                ?.reduce((sum, line) => sum + (line.LocalAmount || 0), 0) 
                                this.periodAmount =  (this.originalAmount -  this.nonPrepaidPaidAmount)/ this.expenseAllocationSettingPM.NumberOfPayments
                                this.amountPaid = this.periodAmount * this.expenseAllocationFlowLists?.filter(item => !AppTool.IsNullOrEmpty(item.JournalId))?.length + this.nonPrepaidPaidAmount;
                                this.amountDue = this.originalAmount - this.amountPaid;
                            }
                            this.currentSession.StopBusyIndicator()
                        });
                });
            });
        })
       
       
    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load(
                "./Infrastructure/Components/EditComponent/EditComponent",
                this.currentSession.SessionLocation.viewContainerRef
            ).then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: "Journal"
                });
                cmpRef.instance.BackCompleted.subscribe(bk => { });
            });
        }
    }
}
