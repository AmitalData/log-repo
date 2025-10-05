import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
  selector: 'RecurringScheduleComponent',
  templateUrl: './RecurringScheduleComponent.html',
})
export class RecurringScheduleComponent extends BaseComponent {
 

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
  SetTigger(triggerType: string) {
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

}