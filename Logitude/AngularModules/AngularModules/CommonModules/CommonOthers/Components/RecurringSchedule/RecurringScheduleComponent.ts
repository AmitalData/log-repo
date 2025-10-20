import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ExpenseAllocationSettingPM } from 'Invoice/EntityPMs/ExpenseAllocationSettingPM';

@Component({
    selector: 'RecurringScheduleComponent',
    templateUrl: './RecurringScheduleComponent.html',
})
export class RecurringScheduleComponent extends BaseComponent {


    public dataContext = this;
    public isRTL = false;
    public IsDayDisabled = false;
    public typeRadio: 'RecurrenceCount' | 'EndDateTime' = 'RecurrenceCount';

    private currentSession = SessionLocator.SelectedSession;
    private minDate: Date | null = null;

    public allocationTypes = Object.values(AllocationDateType);
    public days = Object.values(Days);

    public IsWeekly = false;
    public IsMonthly = true;

    public StartDateTime: Date | null = null;
    public EndDateTime: Date | null = null;
    public IntervalCount = 1;
    public RecurrenceCount = 0;
    public TotalAmount = 0;
    public RecurrenceAmount = 0;

    public AllocationDateType: AllocationDateType = AllocationDateType.SpecificDate;
    public selectedDay: string = 'Sunday';
    public selectedDayByWeek: string = '';

    private expenseAllocationSettingPM!: ExpenseAllocationSettingPM;

    private readonly dayMap: Record<string, number> = {
        Sunday: 0,
        Monday: 1,
        Tuesday: 2,
        Wednesday: 3,
        Thursday: 4,
        Friday: 5,
        Saturday: 6,
    };


    constructor() {
        super();
        this.isRTL = ObjectsLocator.GlobalSetting?.LayoutDirection === 'rtl';
    }


    public SetWindowArgs(args: any): void {
        this.minDate = args?.['MinDate'] ?? null;
        this.StartDateTime = args?.['StartDateTime'] ?? null;
        this.EndDateTime = args?.['EndDateTime'] ?? null;
        this.RecurrenceCount = args?.['RecurrenceCount'] ?? 0;
        this.IntervalCount = args?.['IntervalCount'] ?? 1;
        this.TotalAmount = args?.['TotalAmount'] ?? 0;
        this.AllocationDateType =
            args?.['AllocationDateType'] ?? AllocationDateType.SpecificDate;
        this.selectedDay = args?.['SelectedDay'] ?? 'Sunday';
        this.selectedDayByWeek = args?.['SelectedDayByWeek'] ?? '';
        this.IsWeekly = args?.['IsWeekly'] ?? false;
        this.IsMonthly = !this.IsWeekly;
        this.recalculateAll();
    }


    set StartDateTimeValue(newValue: Date) {
        if (this.minDate && new Date(newValue) < new Date(this.minDate)) {
            this.UIProperties.SetValidity(
                'StartDateTime',
                null,
                false,
                TextCodeTranslator.Translate('ExpenseAllocationSetting.O.StartDateError')
            );
            return;
        }

        this.StartDateTime = newValue;
        this.recalculateAll();
        this.UIProperties.SetValidity('StartDateTime', null, true, '');
    }

    set EndDateTimeValue(newValue: Date) {
        this.EndDateTime = newValue;
        this.recalculateAll();
    }


    public setTrigger(triggerType: string): void {
        this.IsWeekly = triggerType === 'W';
        this.IsMonthly = triggerType === 'M';
    }

    public onSelectAllocationType(type: AllocationDateType): void {
        this.AllocationDateType = type;

        this.IsDayDisabled =
            type === AllocationDateType.StartOfMonth ||
            type === AllocationDateType.EndOfMonth ||
            type === AllocationDateType.SpecificDate;

        this.selectedDay = this.IsDayDisabled ? '' : this.selectedDay || 'Sunday';
    }

    public onSelectDay(day: string): void {
        this.selectedDay = day;
        this.recalculateAll();
    }

    public onSelectDayByWeek(day: string): void {
        this.selectedDayByWeek = day;
        this.recalculateAll();
    }

    public radioTypeChanged(radioType: 'RecurrenceCount' | 'EndDateTime'): void {
        this.typeRadio = radioType;
    }

    public cancelButtonClicked(): void {
        this.currentSession.CloseCurrentWindow();
    }


    public getDisplayText(type: string): string {
        if (!this.isRTL) return type;

        const map: Record<string, string> = {
            [AllocationDateType.StartOfMonth]: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.StartOfMonth'),
            [AllocationDateType.EndOfMonth]: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.EndOfMonth'),
            [AllocationDateType.SpecificDate]:TextCodeTranslator.Translate('ExpenseAllocationSetting.O.SpecificDate'),
            [AllocationDateType.FirstWeek]: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.FirstWeek'),
            [AllocationDateType.SecondWeek]: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.SecondWeek'),
            [AllocationDateType.ThirdWeek]: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.ThirdWeek'),
            [AllocationDateType.FourthWeek]: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.FourthWeek')
        };
        return map[type] || type;
    }

    public getDisplayTextOfDay(day: string): string {
        if (!this.isRTL) return day;

        const map: Record<string, string> = {
            Sunday: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Sunday'),
            Monday: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Monday'),
            Tuesday: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Tuesday'),
            Wednesday: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Wednesday'),
            Thursday:TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Thursday'),
            Friday: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Friday'),
            Saturday: TextCodeTranslator.Translate('ExpenseAllocationSetting.O.Saturday'),
        };
        return map[day] || day;
    }


    public recalculateAll(): void {
        if (!this.StartDateTime) return;

        const start = new Date(this.StartDateTime);

        if (!this.EndDateTime) {
            const defaultEnd = new Date(start);
            defaultEnd.setDate(defaultEnd.getDate() + 364);
            this.EndDateTime = defaultEnd;
        }

        if (this.typeRadio === 'RecurrenceCount' && this.RecurrenceCount > 0) {
            const newEnd = new Date(start);

            if (this.IsMonthly) {
                newEnd.setMonth(
                    start.getMonth() + (this.RecurrenceCount - 1) * this.IntervalCount
                );
            } else if (this.IsWeekly) {
                newEnd.setDate(
                    start.getDate() + (this.RecurrenceCount - 1) * this.IntervalCount * 7
                );
            }

            this.EndDateTime = newEnd;
        }

        if (
            this.typeRadio === 'EndDateTime' &&
            this.EndDateTime &&
            this.selectedDay
        ) {
            const end = new Date(this.EndDateTime);
            const paymentDay = this.dayMap[this.selectedDay];
            let count = 0;
            const tempDate = new Date(start);

            if (tempDate.getDay() === paymentDay && tempDate > start) count++;
            tempDate.setDate(tempDate.getDate() + 1);

            while (tempDate <= end) {
                if (tempDate.getDay() === paymentDay) count++;
                tempDate.setDate(tempDate.getDate() + 1);
            }

            this.RecurrenceCount = count;
        }

        this.RecurrenceAmount =
            this.TotalAmount && this.RecurrenceCount > 0
                ? this.TotalAmount / this.RecurrenceCount
                : 0;
    }


    public okButtonClicked(): void {
        if (this.minDate && new Date(this.StartDateTime!) < new Date(this.minDate)) {
            this.UIProperties.SetValidity(
                'StartDateTime',
                null,
                false,
                TextCodeTranslator.Translate('ExpenseAllocationSetting.O.StartDateError')
            );
            return;
        }

        this.expenseAllocationSettingPM = new ExpenseAllocationSettingPM();
        this.expenseAllocationSettingPM.StartDateTime = this.StartDateTime!;
        this.expenseAllocationSettingPM.EndDateTime = this.EndDateTime!;
        this.expenseAllocationSettingPM.NumberOfPayments = this.RecurrenceCount;
        this.expenseAllocationSettingPM.MonthInterval = this.IntervalCount;
        this.expenseAllocationSettingPM.PaymentDateType = this.IsMonthly
            ? `Monthly_${this.AllocationDateType}_${this.selectedDay}`
            : `Weekly_${this.selectedDayByWeek}`;

        this.currentSession.CloseCurrentWindowEmit('ok');
    }
}


export enum AllocationDateType {
    StartOfMonth = 'Start',
    EndOfMonth = 'End',
    SpecificDate = 'Specific',
    FirstWeek = 'FirstWeek',
    SecondWeek = 'SecondWeek',
    ThirdWeek = 'ThirdWeek',
    FourthWeek = 'FourthWeek',
}

export enum Days {
    Sunday = 'Sunday',
    Monday = 'Monday',
    Tuesday = 'Tuesday',
    Wednesday = 'Wednesday',
    Thursday = 'Thursday',
    Friday = 'Friday',
    Saturday = 'Saturday',
}
