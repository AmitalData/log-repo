declare var System: any;

import { Component, OnInit, Output, EventEmitter, Input} from '@angular/core';
@Component({
    moduleId: module.id,

    selector: 'LogTimePicker',
    templateUrl: './TimeSelectComponent.html',
})

export class TimeSelectComponent implements OnInit {
    TimeArray24: string[] = [
        "00:00", /*"00:30",*/
        "01:00", /*"01:30",*/
        "02:00", /*"02:30",*/
        "03:00", /*"03:30",*/
        "04:00", /*"04:30",*/
        "05:00", /*"05:30",*/
        "06:00", /*"06:30",*/
        "07:00", /*"07:30",*/
        "08:00", /*"08:30",*/
        "09:00", /*"09:30",*/
        "10:00", /*"10:30",*/
        "11:00", /*"11:30",*/
        "12:00", /*"12:30",*/
        "13:00", /*"13:30",*/
        "14:00", /*"14:30",*/
        "15:00", /*"15:30",*/
        "16:00", /*"16:30",*/
        "17:00", /*"17:30",*/
        "18:00", /*"18:30",*/
        "19:00", /*"19:30",*/
        "20:00", /*"20:30",*/
        "21:00", /*"21:30",*/
        "22:00", /*"22:30",*/
        "23:00", /*"23:30",*/ ];
    TimeArray12: string[] = [
        "12:00 AM", /*"12:30 AM",*/
        "01:00 AM", /*"01:30 AM",*/
        "02:00 AM", /*"02:30 AM",*/
        "03:00 AM", /*"03:30 AM",*/
        "04:00 AM", /*"04:30 AM",*/
        "05:00 AM", /*"05:30 AM",*/
        "06:00 AM", /*"06:30 AM",*/
        "07:00 AM", /*"07:30 AM",*/
        "08:00 AM", /*"08:30 AM",*/
        "09:00 AM", /*"09:30 AM",*/
        "10:00 AM", /*"10:30 AM",*/
        "11:00 AM", /*"11:30 AM",*/
        "12:00 PM", /*"12:30 PM",*/
        "01:00 PM", /*"01:30 PM",*/
        "02:00 PM", /*"02:30 PM",*/
        "03:00 PM", /*"03:30 PM",*/
        "04:00 PM", /*"04:30 PM",*/
        "05:00 PM", /*"05:30 PM",*/
        "06:00 PM", /*"06:30 PM",*/
        "07:00 PM", /*"07:30 PM",*/
        "08:00 PM", /*"08:30 PM",*/
        "09:00 PM", /*"09:30 PM",*/
        "10:00 PM", /*"10:30 PM",*/
        "11:00 PM", /*"11:30 PM", */];
    @Input() TimeMode: string;
    TimeItemsSource: string[];
    @Input() ToggleOpened: boolean;
    @Output() OnSelectedTimeChanged = new EventEmitter();
    @Input() SelectedTime: Date;
    constructor() {

    }

    ngOnInit() {
        //console.log("oninit1", this.SelectedTime.toUTCString());
       // if (this.TimeMode == '24') {
            this.TimeItemsSource = this.TimeArray24;
        //}
       // else {
           // this.TimeItemsSource = this.TimeArray12;
        //}
    }


    OnTimeClick(time: string) {
        var strArr: string[] = time.split(':');
        var hours: number = Number(strArr[0]);
        var minutes: number;
        var suffix: string = null;;
        if (strArr[1].indexOf('A') > -1 || strArr[1].indexOf('P')>-1) {
            var arr: string[] = strArr[1].split(' ');
            minutes = Number(arr[0]);
            suffix = arr[1];
        }
        else {
            minutes = Number(strArr[1]);
        }
        var year: number = 0;
        var month: number = 0;
        var day: number = 0;
        if (!this.SelectedTime) {
            var today: Date = new Date();
            year = today.getFullYear();
            month = today.getMonth();
            day = today.getDate();
        }
        else {
            year = this.SelectedTime.getUTCFullYear();
            month = this.SelectedTime.getUTCMonth();
            day = this.SelectedTime.getUTCDate();
        }
        

        this.SelectedTime = this.GetDate(year, month, day, hours, minutes, 0);
        this.OnSelectedTimeChanged.emit({SelectedTime: this.SelectedTime,Suffix: suffix });

    }
    GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
        var date: Date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    }
    ToDate(time: string) {
        var strArr: string[] = time.split(':');
        var hours: number = Number(strArr[0]);
        var minutes: number;
        var suffix: string = null;;
        if (strArr[1].indexOf('A') > -1 || strArr[1].indexOf('P') > -1) {
            var arr: string[] = strArr[1].split(' ');
            minutes = Number(arr[0]);
            suffix = arr[1];
        }
        else {
            minutes = Number(strArr[1]);
        }
        var year: number = this.SelectedTime.getFullYear();
        var month: number = this.SelectedTime.getMonth();
        var day: number = this.SelectedTime.getDate();
        var date = this.GetDate(year, month, day, hours, minutes, 0).toUTCString();
        return new Date(date);

        //var mydate = new Date(time);
        //console.log(mydate.toDateString());
        //return mydate;
    }
    isSelected(checktime: string) {

        
        //var currentDate:Date = this.ToDate(checktime);
        //var mydate: Date = new Date(this.SelectedTime.toUTCString());

        //if (currentDate.getTime() == mydate.getTime())
        //    return true;
        
        return false;
    }
}