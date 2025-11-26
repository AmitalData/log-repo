import { Component } from '@angular/core';
import { validate } from 'fast-json-patch';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DateTool } from 'Infrastructure/Tools';
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
    public isReady = false;
    public disabled = false;
    public IsDayDisabled = false;
    public validationErrorsList: string[];
    RecurringScheduleComponent = RecurringScheduleComponent;
    public typeRadio: 'RecurrenceCount' | 'EndDateTime' = 'RecurrenceCount';

    private currentSession = SessionLocator.SelectedSession;
    private minDate: Date | null = null;

    public allocationTypes = Object.values(AllocationDateType);
    public days = Object.values(Days);

    public IsWeekly = false;
    public IsMonthly = true;
    public IsSpecificDateEnabled = false;
    public NumberOfDayInMonth = null;
    public startDateTime: Date | null = null;
    get StartDateTime() {
        return this.startDateTime;
    }
    set StartDateTime(newValue: Date) {
        if (this.startDateTime != newValue) {
            this.startDateTime = newValue;
            this.validateDate();
            this.recalculateAll();
        }
    }
    public endDateTime: Date | null = null;

    get EndDateTime() {
        return this.endDateTime;
    }
    set EndDateTime(newValue: Date) {
        if (this.endDateTime != newValue) {
            this.endDateTime = newValue;
            if (this.minDate && new Date(newValue) < new Date(this.minDate)) {
                this.UIProperties.SetValidity(
                    'EndDateTime',
                    null,
                    false,
                    TextCodeTranslator.Translate(
                        'ExpenseAllocationSetting.O.EndDateError'
                    )
                );
            } else {
                this.UIProperties.SetValidity('EndDateTime', null, true, '');
            }
            this.recalculateAll();
        }
    }

    public intervalCount = 1;
    get IntervalCount() {
        return this.intervalCount;
    }
    set IntervalCount(newValue: number) {
        if (this.intervalCount != newValue) {
            this.intervalCount = newValue;
            this.recalculateAll();
        }
    }
    public TotalAmount = 0;
    public TotalLocalAmount = 0;
    public CurrencyCode = null;
    public RecurrenceAmount = 0;

    public recurrenceCount = 0;
    get RecurrenceCount() {
        return this.recurrenceCount;
    }
    set RecurrenceCount(newValue: number) {
        if (this.recurrenceCount != newValue) {
            this.recurrenceCount = newValue;
            this.recalculateAll();
        }
    }

    public AllocationDateType: AllocationDateType =
        AllocationDateType.SpecificDate;
    public selectedDay: string = 'Sunday';
    public selectedDayByWeek: string = '';

    private expenseAllocationSettingPM!: ExpenseAllocationSettingPM;
    entityResourceService: EntityResourceService = new EntityResourceService();

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
        this.GetResources();
        this.isRTL = ObjectsLocator.GlobalSetting?.LayoutDirection === 'rtl';
    }
    private GetResources() {
        this.entityResourceService
            .getEntityResourceByTableName('ExpenseAllocationSetting')
            .subscribe((response: any) => {
                this.isReady = true;
            });
    }

    public SetWindowArgs(args: any): void {
        this.minDate = args?.['MinDate'] ?? null;
        this.StartDateTime = args?.['StartDateTime'] ?? null;
        this.RecurrenceCount = args?.['RecurrenceCount'] ?? 14;
        this.IntervalCount = args?.['IntervalCount'] ?? 1;
        this.TotalAmount = args?.['TotalAmount'] ?? 0;
        this.TotalLocalAmount = args?.['TotalLocalAmount'] ?? 0;
        this.CurrencyCode = args?.['CurrencyCode'] ?? 'NIS';

        this.AllocationDateType =
            args?.['AllocationDateType'] ?? AllocationDateType.SpecificDate;
        if (this.AllocationDateType === AllocationDateType.SpecificDate) {
            this.NumberOfDayInMonth = args?.['SelectedDay'] ?? 1;
        } else {
            this.selectedDay = args?.['SelectedDay'] ?? null;
        }

        this.selectedDayByWeek = args?.['SelectedDayByWeek'] ?? null;
        this.IsWeekly = args?.['IsWeekly'] ?? false;
        this.IsMonthly = !this.IsWeekly;
        if (this.AllocationDateType === AllocationDateType.SpecificDate)
            this.IsSpecificDateEnabled = true;
        this.disabled = args?.['Disabled'] ?? false;
        this.EndDateTime = args?.['EndDateTime'] ?? null;
        this.recalculateAll();
    }

    public setTrigger(triggerType: string): void {
        this.IsWeekly = triggerType === 'W';
        this.IsMonthly = triggerType === 'M';
        this.recalculateAll();
    }

    public onSelectAllocationType(type: AllocationDateType): void {
        this.AllocationDateType = type;

        this.IsDayDisabled =
            type === AllocationDateType.StartOfMonth ||
            type === AllocationDateType.EndOfMonth ||
            type === AllocationDateType.SpecificDate;

        this.selectedDay = this.IsDayDisabled
            ? null
            : this.selectedDay || 'Sunday';
        if (type === AllocationDateType.SpecificDate)
            this.IsSpecificDateEnabled = true;
        else this.IsSpecificDateEnabled = false;
    }

    public onSelectDay(day: string): void {
        this.selectedDay = day;
        this.recalculateAll();
    }

    public onSelectDayByWeek(day: string): void {
        this.selectedDayByWeek = day;
        this.recalculateAll();
    }

    public radioTypeChanged(
        radioType: 'RecurrenceCount' | 'EndDateTime'
    ): void {
        this.typeRadio = radioType;
    }

    public cancelButtonClicked(): void {
        this.currentSession.CloseCurrentWindow();
    }

    public static getDisplayText(type: string,): string {

        const map: Record<string, string> = {
            [AllocationDateType.StartOfMonth]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.StartOfMonth'
            ),
            [AllocationDateType.EndOfMonth]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.EndOfMonth'
            ),
            [AllocationDateType.SpecificDate]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.SpecificDate'
            ),
            [AllocationDateType.FirstWeek]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.FirstWeek'
            ),
            [AllocationDateType.SecondWeek]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.SecondWeek'
            ),
            [AllocationDateType.ThirdWeek]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.ThirdWeek'
            ),
            [AllocationDateType.FourthWeek]: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.FourthWeek'
            ),
        };
        return map[type] || type;
    }

    public static getDisplayTextOfDay(day: string): string {
        

        const map: Record<string, string> = {
            Sunday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Sunday'
            ),
            Monday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Monday'
            ),
            Tuesday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Tuesday'
            ),
            Wednesday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Wednesday'
            ),
            Thursday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Thursday'
            ),
            Friday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Friday'
            ),
            Saturday: TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.Saturday'
            ),
        };
        return map[day] || day;
    }

    public recalculateAll(): void {
        if (!this.StartDateTime) return;

        const start = this.StartDateTime;
        let end = this.EndDateTime;

        if (!this.IntervalCount || this.IntervalCount < 1)
            this.IntervalCount = 1;
        if (
            this.typeRadio === 'RecurrenceCount' &&
            (this.RecurrenceCount ?? 0) > 0
        ) {
            const last = this.computeOccurrenceByIndex(
                start,
                (this.RecurrenceCount ?? 1) - 1
            );
            this.endDateTime = last;
            end = last;
        }
        if (this.typeRadio === 'EndDateTime' && end) {
            const count = this.countOccurrencesBetween(start, end);
            this.recurrenceCount = count;
        }
        if (this.TotalAmount && (this.RecurrenceCount ?? 0) > 0) {
            this.RecurrenceAmount =
                this.TotalAmount / (this.RecurrenceCount ?? 1);
        } else {
            this.RecurrenceAmount = 0;
        }
    }

    private MAX_ITER = 10000;

    private countOccurrencesBetween(start: Date, end: Date): number {
        if (end < start) return 0;
        let count = 1;
        let safety = 0;
        let current = new Date(start);
        while (safety++ < this.MAX_ITER) {
            const next = this.computeNextOccurrenceAfter(current);
            if (!next) break;
            if (next > end) break;
            count++;
            current = next;
        }

        return count;
    }

    private computeNextOccurrenceAfter(date: Date): Date | null {
        if (this.IsWeekly && this.selectedDayByWeek !== undefined) {
            const next = new Date(date);
            const targetDay = this.dayMap[this.selectedDayByWeek];
            let diff = (7 + targetDay - next.getUTCDay()) % 7;
            if (diff === 0) diff = 7;
            next.setUTCDate(next.getUTCDate() + diff);
            return next;
        }

        if (this.IsMonthly) {
            return this.nextMonthlyOccurrenceAfter(date);
        }
        const fallback = new Date(date);
        fallback.setUTCMonth(fallback.getUTCMonth() + this.IntervalCount);
        return fallback;
    }

    private nextMonthlyOccurrenceAfter(date: Date): Date | null {
        const interval = Math.max(1, this.IntervalCount || 1);
        const startCandidate = new Date(date);

        let attempts = 0;
        let monthIndex = startCandidate.getUTCMonth();
        let year = startCandidate.getUTCFullYear();

        while (attempts++ < this.MAX_ITER) {
            let candidate: Date;
            switch (this.AllocationDateType) {
                case AllocationDateType.StartOfMonth:
                    candidate = new Date(year, monthIndex, 1);
                    if (candidate > date) return candidate;
                    break;

                case AllocationDateType.EndOfMonth:
                    candidate = new Date(year, monthIndex + 1, 0);
                    if (candidate > date) return candidate;
                    break;

                case AllocationDateType.SpecificDate:
                    if (this.NumberOfDayInMonth) {
                        const daysInMonth = new Date(
                            year,
                            monthIndex + 1,
                            0
                        ).getUTCDate();
                        const day = Math.min(
                            this.NumberOfDayInMonth,
                            daysInMonth
                        );
                        candidate = new Date(year, monthIndex, day);
                        if (candidate > date) return candidate;
                    } else {
                    }
                    break;

                case AllocationDateType.FirstWeek:
                case AllocationDateType.SecondWeek:
                case AllocationDateType.ThirdWeek:
                case AllocationDateType.FourthWeek:
                    {
                        const weekIndex = {
                            FirstWeek: 0,
                            SecondWeek: 1,
                            ThirdWeek: 2,
                            FourthWeek: 3,
                        }[this.AllocationDateType];
                        const firstOfMonth = new Date(year, monthIndex, 1);
                        const dayOffset =
                            (this.dayMap[this.selectedDay] +
                                7 -
                                firstOfMonth.getUTCDay()) %
                            7;
                        candidate = new Date(firstOfMonth);
                        candidate.setUTCDate(1 + dayOffset + weekIndex * 7);
                        if (candidate > date) return candidate;
                    }
                    break;

                default:
                    const d = Math.min(
                        date.getUTCDate(),
                        new Date(year, monthIndex + 1, 0).getUTCDate()
                    );
                    candidate = new Date(year, monthIndex, d);
                    if (candidate > date) return candidate;
                    break;
            }

            monthIndex += interval;
            while (monthIndex > 11) {
                monthIndex -= 12;
                year += 1;
            }
        }

        return null;
    }

    private computeOccurrenceByIndex(start: Date, index: number): Date {
        if (index === 0) return new Date(start);

        let current = new Date(start);
        let safety = 0;
        for (let i = 0; i < index && safety++ < this.MAX_ITER; i++) {
            const next = this.computeNextOccurrenceAfter(current);
            if (!next) break;
            current = next;
        }
        return current;
    }

    public okButtonClicked(): void {
         if(!this.validateDate())
            return;
       
        this.expenseAllocationSettingPM = new ExpenseAllocationSettingPM();
        this.expenseAllocationSettingPM.StartDateTime = this.StartDateTime!;
        this.expenseAllocationSettingPM.EndDateTime = this.EndDateTime!;
        this.expenseAllocationSettingPM.NumberOfPayments = this.RecurrenceCount;
        this.expenseAllocationSettingPM.MonthInterval = this.IntervalCount;
        this.expenseAllocationSettingPM.PaymentDateType = this.IsMonthly
            ? `Monthly_${this.AllocationDateType}_${
                this.AllocationDateType === AllocationDateType.SpecificDate 
                    ? this.NumberOfDayInMonth 
                    : this.AllocationDateType === AllocationDateType.FirstWeek  || this.AllocationDateType === AllocationDateType.SecondWeek ||
                        this.AllocationDateType === AllocationDateType.ThirdWeek || this.AllocationDateType === AllocationDateType.FourthWeek
                        ? this.selectedDay 
                        : ''
              }`
            : `Weekly_${this.selectedDayByWeek}`;

        this.currentSession.CloseCurrentWindowEmit('ok');
    }
    validateDate(): boolean {
        this.validationErrorsList = [];

        if (this.minDate && new Date(this.startDateTime) < new Date(this.minDate)) {
            this.UIProperties.SetValidity(
                'StartDateTime',
                null,
                false,
                TextCodeTranslator.Translate(
                    'ExpenseAllocationSetting.O.EndDateError'
                )

            );
            this.validationErrorsList.push(TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.EndDateError'
            ));

            return false;
        } 
        else if( this.EndDateTime && new Date(this.startDateTime) > new Date(this.EndDateTime)) {
            this.UIProperties.SetValidity(
                'StartDateTime',
                null,
                false,
                TextCodeTranslator.Translate(
                    'ExpenseAllocationSetting.O.StartDateAfterEndDateError'
                )
            );
            this.validationErrorsList.push(TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.StartDateAfterEndDateError'
            ));

            return false;
        }
        else if(this.startDateTime < DateTool.GetCurrentDateAsUtc()) {
            this.UIProperties.SetValidity(
                'StartDateTime',
                null,
                false,
                TextCodeTranslator.Translate(
                    'ExpenseAllocationSetting.O.PastDateError'
                )
            );
            this.validationErrorsList.push(TextCodeTranslator.Translate(
                'ExpenseAllocationSetting.O.PastDateError'
            ));

            return false;
        }
        else {
            this.UIProperties.SetValidity('StartDateTime', null, true, '');
            return true;
        }
    }
}

export enum AllocationDateType {
    StartOfMonth = 'Start',
    EndOfMonth = 'End',
    SpecificDate = 'SpecificDate',
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
