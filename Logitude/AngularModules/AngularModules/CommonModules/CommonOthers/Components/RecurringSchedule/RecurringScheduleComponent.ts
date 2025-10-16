import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ExpenseAllocationSettingPM } from 'Invoice/EntityPMs/ExpenseAllocationSettingPM';

@Component({
    selector: 'RecurringScheduleComponent',
    templateUrl: './RecurringScheduleComponent.html',
})
export class RecurringScheduleComponent extends BaseComponent {
    public dataContext = this;
    public isRTL: boolean = false;
    public allocationTypes = Object.values(AllocationDateType);
    public typeRadio: string = 'RecurrenceCount';
    private currentSession = SessionLocator.SelectedSession;
    private minDate: Date;
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl';
    }
    SetWindowArgs(args: any) {
        this.StartDateTime = args?.['StartDateTime'] ?? null;
        this.EndDateTime = args?.['EndDateTime'] ?? null;
        this.RecurrenceCount = args?.['RecurrenceCount'] ?? 0;
        this.MonthInterval = args?.['MonthInterval'] ?? 1;
        this.TotalAmount = args?.['TotalAmount'] ?? 0;
        this.AllocationDateType =
            args?.['AllocationDateType'] ?? AllocationDateType.SpecificDate;
        this.minDate = args?.['MinDate'] ?? null;
    }
    private isWeekly: boolean;
    get IsWeekly() {
        return this.isWeekly;
    }
    set IsWeekly(newValue: boolean) {
        if (this.isWeekly != newValue) {
            this.isWeekly = newValue;
        }
    }

    private isMonthly: boolean;
    get IsMonthly() {
        return this.isMonthly;
    }
    set IsMonthly(newValue: boolean) {
        if (this.isMonthly != newValue) {
            this.isMonthly = newValue;
        }
    }
    private startDateTime: Date;
    get StartDateTime() {
        return this.startDateTime;
    }
    set StartDateTime(newValue: Date) {
        if (this.startDateTime != newValue) {
            if(this.minDate && newValue < this.minDate) {
                this.startDateTime = newValue;
                this.recalculateAll();
                this.UIProperties.SetValidity("StartDateTime", null, true, "");
   
            }
            else{
                this.UIProperties.SetValidity("StartDateTime", null, false, "The start date must be greater than or equal to the invoice date.");
            }
        }
    }
    private endDateTime: Date;
    get EndDateTime() {
        return this.endDateTime;
    }
    set EndDateTime(newValue: Date) {
        if (this.endDateTime != newValue) {
            this.endDateTime = newValue;
            this.recalculateAll();
        }
    }

    private allocationDateType: string;
    get AllocationDateType() {
        return this.allocationDateType;
    }
    set AllocationDateType(newValue: string) {
        if (this.allocationDateType != newValue) {
            this.allocationDateType = newValue;
        }
    }
    private monthInterval: number;
    get MonthInterval() {
        return this.monthInterval;
    }
    set MonthInterval(newValue: number) {
        if (this.monthInterval != newValue) {
            this.monthInterval = newValue;
            this.recalculateAll();
        }
    }
    private recurrenceCount: number;
    get RecurrenceCount() {
        return this.recurrenceCount;
    }
    set RecurrenceCount(newValue: number) {
        if (this.recurrenceCount != newValue) {
            this.recurrenceCount = newValue;
            this.recalculateAll();
        }
    }
    private recurrenceAmount: number;
    get RecurrenceAmount() {
        return this.recurrenceAmount;
    }
    set RecurrenceAmount(newValue: number) {
        if (this.recurrenceAmount != newValue) {
            this.recurrenceAmount = newValue;
        }
    }
    private totalAmount: number;
    get TotalAmount() {
        return this.totalAmount;
    }
    set TotalAmount(newValue: number) {
        if (this.totalAmount != newValue) {
            this.totalAmount = newValue;
        }
    }
    setTigger(triggerType: string) {
        switch (triggerType) {
            case 'W': {
                this.IsWeekly = true;
                this.IsMonthly = false;
                break;
            }

            case 'M': {
                this.IsWeekly = false;
                this.IsMonthly = true;
                break;
            }

            default: {
                this.IsWeekly = false;
                this.IsMonthly = false;
                break;
            }
        }
    }
    getDisplayText(type: string): string {
        if (this.isRTL) {
            switch (type) {
                case AllocationDateType.StartOfMonth:
                    return 'תחילת חודש';
                case AllocationDateType.EndOfMonth:
                    return 'סוף חודש';
                case AllocationDateType.SpecificDate:
                    return 'תאריך ספציפי';
            }
        }
        return type;
    }
    onSelect(type: AllocationDateType) {
        this.AllocationDateType = type;
    }   
   
    recalculateAll() {
        const start = new Date(this.startDateTime);
    
        if (this.typeRadio === 'RecurrenceCount' && this.RecurrenceCount > 0) {
            const monthsToAdd = (this.RecurrenceCount - 1) * this.MonthInterval;
            const newEnd = new Date(start);
            newEnd.setMonth(start.getMonth() + monthsToAdd);
            this.endDateTime = newEnd;
        }
    
        if (this.typeRadio === 'EndDateTime' && this.EndDateTime) {
            const end = new Date(this.EndDateTime);
            let count = 0;
            let tempDate = new Date(start);
        
            while (tempDate <= end) {
                count++;
                tempDate.setMonth(tempDate.getMonth() + this.MonthInterval);
            }
        
            this.recurrenceCount = count;
        }
        
    
        if (this.TotalAmount && this.RecurrenceCount > 0) {
            this.recurrenceAmount = this.TotalAmount / this.RecurrenceCount;
        } else {
            this.recurrenceAmount = 0;
        }
    }
    radioTypeChanged(radioType) {
        this.typeRadio = radioType;
        this.RecurrenceCount = 1;
        this.EndDateTime = new Date(this.StartDateTime.getFullYear(), this.StartDateTime.getMonth() + 1, this.StartDateTime.getDate());
        
        
    }
    cancelButtonClicked() {
        this.currentSession.CloseCurrentWindow();
    }
    expenseAllocationSettingPM: ExpenseAllocationSettingPM;
    okButtonClicked() {
        this.expenseAllocationSettingPM = new ExpenseAllocationSettingPM(); 
        this.expenseAllocationSettingPM.StartDateTime = this.StartDateTime;
        this.expenseAllocationSettingPM.EndDateTime = this.EndDateTime;
        this.expenseAllocationSettingPM.NumberOfPayments = this.RecurrenceCount;
        this.expenseAllocationSettingPM.MonthInterval = this.MonthInterval;
        this.expenseAllocationSettingPM.PaymentDateType = this.AllocationDateType;
        this.currentSession.CloseCurrentWindowEmit("ok");
    }
}
export enum AllocationDateType {
    StartOfMonth = 'StartOfMonth',
    EndOfMonth = 'EndOfMonth',
    SpecificDate = 'SpecificDate',
}
