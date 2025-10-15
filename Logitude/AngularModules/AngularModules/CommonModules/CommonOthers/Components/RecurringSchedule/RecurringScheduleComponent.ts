import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'RecurringScheduleComponent',
    templateUrl: './RecurringScheduleComponent.html',
})
export class RecurringScheduleComponent extends BaseComponent {
    public dataContext = this;
    public isRTL: boolean = false;
    public allocationTypes = Object.values(AllocationDateType);
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl';
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
            this.startDateTime = newValue;
        }
    }
    private endDateTime: Date;
    get EndDateTime() {
        return this.endDateTime;
    }
    set EndDateTime(newValue: Date) {
        if (this.endDateTime != newValue) {
            this.endDateTime = newValue;
        }
    }

    private allocationDateType :string;
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
    
}
export enum AllocationDateType {
    StartOfMonth = "StartOfMonth",
    EndOfMonth = "EndOfMonth",
    SpecificDate = "SpecificDate"
}
