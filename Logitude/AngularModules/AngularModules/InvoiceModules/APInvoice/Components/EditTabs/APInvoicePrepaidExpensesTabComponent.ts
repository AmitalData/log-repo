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
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { RecurringScheduleComponent } from 'CommonModules/CommonOthers/Components/RecurringSchedule/RecurringScheduleComponent';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';

@Component({
    templateUrl: './APInvoicePrepaidExpensesTabComponent.html',
})
export class APInvoicePrepaidExpensesTabComponent extends BaseComponent implements OnInit {
    public isRTL = false;
    public EntityPM: APInvoicePM = null;
    public journalPM: JournalPM = null;
    public ObjectTableName = 'APInvoice';
    public DataContext = this;
    public objectTableId: string = '';
    public amountPaid: number = 0;
    public amountDue: number = 0;
    public originalAmount: number = 0;
    public periodAmount;
    public isReady: boolean = false;
    private currentSession = SessionLocator.SelectedSession;
    public isVoided: boolean = false;
    public expenseAllocationSettingPM = new ExpenseAllocationSettingPM();
    expenseAllocationSettingExtendedService =
        new ExpenseAllocationSettingExtendedService();
    expenseAllocationFlowExtendedService =
        new ExpenseAllocationFlowExtendedService();
    journalPMService = new JournalPMService();

    expenseAllocationFlowLists: ExpenseAllocationFlowList[] = [];
    entityResourceService: EntityResourceService = new EntityResourceService();
    fullAccountingSettingPMService: FullAccountingSettingPMService =
        new FullAccountingSettingPMService();

    constructor(private entityArgs: EntityArgs) {
        super();
        this.GetResources();
        this.EntityPM = entityArgs.EntityPM;
        this.isVoided = this.EntityPM.StatusCode === 'VD';
        var table = window.ObjectTables.filter((d) => d.Name == 'APInvoice')[0];
        if (table) this.objectTableId = table.Id;
        this.isRTL = ObjectsLocator.GlobalSetting?.LayoutDirection === 'rtl';

    }
    private GetResources() {
        this.entityResourceService
            .getEntityResourceByTableName('ExpenseAllocationFlow')
            .subscribe((response: any) => {
                this.entityResourceService
                    .getEntityResourceByTableName('ExpenseAllocationSetting')
                    .subscribe((response: any) => {
                        
                    });
            });
    }

    ngOnInit() {
        this.loadDate();
    }
    
    loadDate() {
        var fullAccountingSettingPM = new FullAccountingSettingPM();
        this.currentSession.StartBusyIndicatorLoading();
        this.fullAccountingSettingPMService
            .get(SessionLocator.TenantPM.Id.toString())
            .subscribe((myResult: any) => {
                if (myResult?.Result && !myResult.HasError) {
                    fullAccountingSettingPM = myResult.Result;
                }
                this.journalPMService
                    .get(this.EntityPM.JournalId)
                    .subscribe((res) => {
                        if (res?.Result && !res.HasError) {
                            this.journalPM = res.Result;
                            this.originalAmount =
                                this.journalPM?.JournalLines?.filter(
                                    (line) =>
                                        line.DebitAccountId ===
                                        fullAccountingSettingPM.PrepaidExpensesGLAccountId
                                )?.reduce(
                                    (sum, line) =>
                                        sum + (line.LocalAmount || 0),
                                    0
                                );
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
                                            (
                                                resFlow as ExpenseAllocationFlowList[]
                                            ).forEach(
                                                (
                                                    item: ExpenseAllocationFlowList
                                                ) => {
                                                    if (
                                                        !AppTool.IsNullOrEmpty(
                                                            item.JournalId
                                                        ) ||
                                                        item.Status === 'failed'
                                                    )
                                                        this.expenseAllocationFlowLists.push(
                                                            item
                                                        );
                                                }
                                            );
                                            this.periodAmount =
                                                this.originalAmount /
                                                this.expenseAllocationSettingPM
                                                    .NumberOfPayments;
                                            this.expenseAllocationFlowLists =
                                                this.expenseAllocationFlowLists.sort(
                                                    (a, b) =>
                                                        a.JournalId ===
                                                        this.EntityPM.JournalId
                                                            ? -1
                                                            : b.JournalId ===
                                                              '1'
                                                            ? 1
                                                            : 0
                                                );
                                            this.amountPaid =
                                                this.expenseAllocationFlowLists?.filter(
                                                    (item) =>
                                                        !AppTool.IsNullOrEmpty(
                                                            item.JournalId
                                                        )
                                                )?.length *
                                                (this.originalAmount /
                                                    this
                                                        .expenseAllocationSettingPM
                                                        .NumberOfPayments);
                                            this.amountDue =
                                                this.originalAmount -
                                                this.amountPaid;
                                        }
                                        this.isReady = true;
                                        this.currentSession.StopBusyIndicator();
                                    });
                            });
                    });
            });
    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load(
                './Infrastructure/Components/EditComponent/EditComponent',
                this.currentSession.SessionLocation.viewContainerRef
            ).then((cmpRef) => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: 'Journal',
                });
                cmpRef.instance.BackCompleted.subscribe((bk) => {});
            });
        }
    }

    get StartDateTime() { return this.expenseAllocationSettingPM.StartDateTime; }
    set StartDateTime(newValue: Date) {
        if (this.expenseAllocationSettingPM.StartDateTime != newValue) {
            this.expenseAllocationSettingPM.StartDateTime = newValue;           
        }
    }
    get NumberOfPayments() { return this.expenseAllocationSettingPM.NumberOfPayments; }
    set NumberOfPayments(newValue: number) {
        if (this.expenseAllocationSettingPM.NumberOfPayments != newValue) {
            this.expenseAllocationSettingPM.NumberOfPayments = newValue;           
        }
    }
    get PaymentDateType() { 
           if(this.expenseAllocationSettingPM.PaymentDateType)
           {
            var paymentType = ""
            if (this.expenseAllocationSettingPM.PaymentDateType?.startsWith('Monthly_')) {
                paymentType += ' ' + TextCodeTranslator.Translate(
                    'ExpenseAllocationSetting.O.Monthly'
                );
                const parts = this.expenseAllocationSettingPM.PaymentDateType.split('_');
                var allocationDateType = parts[1] || '';
                paymentType += ' ' + RecurringScheduleComponent.getDisplayText(allocationDateType);
                const selectedDay = parts[2] ? parts[2] : "";
                paymentType += ' ' + RecurringScheduleComponent.getDisplayTextOfDay(selectedDay);
            } else if (this.expenseAllocationSettingPM.PaymentDateType?.startsWith('Weekly_')) {
                paymentType += ' ' + TextCodeTranslator.Translate(
                    'ExpenseAllocationSetting.O.Weekly'
                );
                const parts = this.expenseAllocationSettingPM.PaymentDateType.split('_');
                const selectedDayByWeek = parts[1] || "";
                paymentType += ' ' + RecurringScheduleComponent.getDisplayTextOfDay(selectedDayByWeek);
            }

            return paymentType;
           }

    }
    set PaymentDateType(newValue: string) {
        if (this.expenseAllocationSettingPM.PaymentDateType != newValue) {
            this.expenseAllocationSettingPM.PaymentDateType = newValue;    

        }
    }

}
