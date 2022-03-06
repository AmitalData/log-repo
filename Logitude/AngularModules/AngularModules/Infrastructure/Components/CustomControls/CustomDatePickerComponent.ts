declare var window: any;
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FormGroup, FormBuilder } from '@angular/forms';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({


    selector: 'CustomDatePicker',
    templateUrl: './CustomDatePickerComponent.html',
    inputs: ['ObjectField', 'QueryId', 'QueryCode', 'IsDisabled']
})

export class CustomDatePickerComponent extends BaseComponent implements OnInit {
    public myForm: FormGroup;
    DataContext: any = this;
    QueryId: string;
    QueryCode: string;
    public FromDate: any = null;
    public ToDate: any = null;
    public Text: string = null;
    public WaterMark: string = "dd/mm/yy-dd/mm/yy";
    public ObjectField;
    public ControlId: string = null;
    public DropdownId: string = null;
    public ListControlId: string = null;
    // public DateId: string = null;
    public MinHeight: number = 30;
    public MaxHeight: number = 250;
    public PublicDate = new Date();
    Today: string;
    Yesterday: string;
    LastSevenDays: string;
    LastThirtyDays: string;
    CurrentYear: string;
    LessThanToday: string;
    LessThanOrEqualToday: string;
    LastYear: string;
    TodayDate: any;
    TommorowDate: any;
    YesterdayDate: any;
    LastSevenDaysDate: any;
    LastThirtyDaysDate: any;
    LastYearFromDate: any;
    LastYearToDate: any;
    CurrentYearFromDate: any;
    CurrentYearToDate: any;
    CloseMenu: boolean = true;
    IsMenuOpened: boolean = false;
    NoDateVisibile: boolean = true;
    mouseOver: boolean = false;
    public IsDisabled: boolean = false;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    public HasAdvancedFiltersOptionsToggle: boolean = false;
    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});
        if (this.CurrentSession == null) {
            this.ControlId = "ComboBox_-1_-1";
            this.DropdownId = "Dropdown_-1_-1";
            this.ListControlId = "List_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ComboBox");
            this.ControlId = "ComboBox_" + idIndex;
            this.DropdownId = "Dropdown_" + idIndex;
            this.ListControlId = "List_" + idIndex;
        }
        document.onmouseup = (e) => {
            if (this.mouseOver == false) {
                this.OnLostFocus();
            }
        };
    }

    ngOnInit() {
        this.checkAdvancedFiltersOptionsToggle();
        this.TommorowDate = DateTool.AddDays((new Date()), 1);
        this.TommorowDate.setHours(0, 0, 0, 0);
        this.TodayDate = new Date();
        this.TodayDate.setHours(0, 0, 0, 0);
        this.Today = (this.TodayDate.getDate() < 10 ? "0" : "") + this.TodayDate.getDate() + '-' + (this.TodayDate.getMonth() + 1 < 10 ? "0" : "") + (this.TodayDate.getMonth() + 1) + '-' + this.TodayDate.getFullYear();
        this.YesterdayDate = DateTool.AddDays((new Date()), -1);
        this.YesterdayDate.setHours(0, 0, 0, 0);
        this.Yesterday = (this.YesterdayDate.getDate() < 10 ? "0" : "") + (this.YesterdayDate.getDate()) + '-' + (this.YesterdayDate.getMonth() + 1 < 10 ? "0" : "") + (this.YesterdayDate.getMonth() + 1) + '-' + this.YesterdayDate.getFullYear();
        this.LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
        this.LastSevenDaysDate.setHours(0, 0, 0, 0);
        this.LastSevenDays = (this.LastSevenDaysDate.getDate() < 10 ? "0" : "") + (this.LastSevenDaysDate.getDate()) + '-' + (this.LastSevenDaysDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastSevenDaysDate.getMonth() + 1) + '-' + this.LastSevenDaysDate.getFullYear() + " - " + this.Today;
        this.LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
        this.LastThirtyDaysDate.setHours(0, 0, 0, 0);
        this.LastThirtyDays = (this.LastThirtyDaysDate.getDate() < 10 ? "0" : "") + this.LastThirtyDaysDate.getDate() + '-' + (this.LastThirtyDaysDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastThirtyDaysDate.getMonth() + 1) + '-' + this.LastThirtyDaysDate.getFullYear() + " - " + this.Today;
        this.CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
        this.CurrentYearFromDate.setHours(0, 0, 0, 0);
        this.CurrentYearToDate = DateTool.AddDays((new Date()), 1);
        this.CurrentYearToDate.setHours(0, 0, 0, 0);
        this.CurrentYear = '01-01-' + ((new Date()).getFullYear()) + ' - ' + 'Today';//(this.CurrentYearToDate.getDate() < 10 ? "0" : "") + (this.CurrentYearToDate.getDate()) + '-' + (this.CurrentYearToDate.getMonth() + 1 < 10 ? "0" : "") + (this.CurrentYearToDate.getMonth() + 1) + '-' + (this.CurrentYearToDate.getFullYear());
        this.LessThanToday = ' < ' + (this.TodayDate.getDate() < 10 ? "0" : "") + this.TodayDate.getDate() + '-' + (this.TodayDate.getMonth() + 1 < 10 ? "0" : "") + (this.TodayDate.getMonth() + 1) + '-' + this.TodayDate.getFullYear();
        this.LessThanOrEqualToday = ' <= ' + (this.TodayDate.getDate() < 10 ? "0" : "") + this.TodayDate.getDate() + '-' + (this.TodayDate.getMonth() + 1 < 10 ? "0" : "") + (this.TodayDate.getMonth() + 1) + '-' + this.TodayDate.getFullYear();

        //this.LastYearFromDate = DateTool.AddDays((new Date()), -365);
        //this.LastYearFromDate.setHours(0, 0, 0, 0);
        //this.LastYearToDate = DateTool.AddDays((new Date()), 1);
        //this.LastYearToDate.setHours(0, 0, 0, 0);
        //this.LastYear = (this.LastYearFromDate.getDate() < 10 ? "0" : "") + (this.LastYearFromDate.getDate() - 1) + '-' + (this.LastYearFromDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastYearFromDate.getMonth() + 1) + '-' + this.LastYearFromDate.getFullYear() + ' - ' + (this.LastYearFromDate.getDate() < 10 ? "0" : "") + (this.LastYearToDate.getDate() - 1) + '-' + (this.LastYearFromDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastYearToDate.getMonth() + 1) + '-' + (this.LastYearToDate.getFullYear());

        this.LastYearFromDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), -365);
        this.LastYearToDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 1);

        var lastYearDateParts1 = DateTool.GetDateParts(this.LastYearFromDate);
        var lastYearDateParts2 = DateTool.GetDateParts(this.LastYearToDate);

        this.LastYear = "";
        this.LastYear += AppTool.PadLeft(lastYearDateParts1.Day + "", 2, "0") + '-' + AppTool.PadLeft(lastYearDateParts1.Month + "", 2, "0") + '-' + lastYearDateParts1.Year;
        this.LastYear += " - ";
        this.LastYear += AppTool.PadLeft(lastYearDateParts2.Day + "", 2, "0") + '-' + AppTool.PadLeft(lastYearDateParts2.Month + "", 2, "0") + '-' + lastYearDateParts2.Year;

        var predefinedFilter = window.PreDefinedFilters.filter(d => d.ObjectFieldId == this.ObjectField.Id && d.QueryCode == this.QueryCode)[0];
        if (predefinedFilter != null && (this.ObjectField.DataTypeCode == "DateTime" || this.ObjectField.DataTypeCode == "Date")) {
            this.SelectedItem = predefinedFilter.PredefinedValue;
            if (this.SelectedItem == "NoDate") {
                this.Text = "No Date";
            }
            if (predefinedFilter.Operator == "LargerThan") {
                var myDate = DateTool.GetDateParts(predefinedFilter.PredefinedValue);
                var stringDate = (myDate.Day < 10 ? "0" : "") + myDate.Day + '-' + (myDate.Month < 10 ? "0" : "") + myDate.Month + '-' + myDate.Year;
                this.SelectedItem = "Greater Than";
                this.Text = "Greater Than " + stringDate;
                this.GreaterTextValue

            }
            if (predefinedFilter.Operator == "LessThan") {
                var myDate = DateTool.GetDateParts(predefinedFilter.PredefinedValue);
                var stringDate = (myDate.Day < 10 ? "0" : "") + myDate.Day + '-' + (myDate.Month < 10 ? "0" : "") + myDate.Month + '-' + myDate.Year;
                this.SelectedItem = "Less Than";
                this.Text = "Less Than " + stringDate;
            }
            if (predefinedFilter.Operator == "Between") {

                var myDate = DateTool.GetDateParts(predefinedFilter.PredefinedValue);
                var myDate1 = DateTool.GetDateParts(predefinedFilter.PredefinedValue2);
                var stringDate = (myDate.Day < 10 ? "0" : "") + myDate.Day + '-' + (myDate.Month < 10 ? "0" : "") + myDate.Month + '-' + myDate.Year;
                var endingDate = (myDate1.Day < 10 ? "0" : "") + myDate1.Day + '-' + (myDate1.Month < 10 ? "0" : "") + myDate1.Month + '-' + myDate1.Year;
                this.SelectedItem = stringDate + " - " + endingDate;
                this.Text = stringDate + " - " + endingDate;
            }

            if (this.QueryCode == 'LedgerTransaction.LedgerTransactions') {
                this.SelectedItem = "Last Year";
            }
            //Between
        }
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
        if (this.ObjectField.IsRequiered == true) {
            this.NoDateVisibile = false;
        }
        if (this.ObjectField.FieldName == "CreateDateTime") {
            this.NoDateVisibile = false;
        }
    }

    private checkAdvancedFiltersOptionsToggle() {
        let advancedFiltersOptionsToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "AFO")[0];
        if (advancedFiltersOptionsToggle) {
            this.HasAdvancedFiltersOptionsToggle = true;
        }
    }

    mousedown() {
        console.log("mousedown" + this.CloseMenu);
        if (this.mouseOver == false) {
            this.CloseMenu = true;
            this.IsMenuOpened = false;
            this.OnLostFocus();

        }
        else {
            this.IsMenuOpened = true;
            var item = document.getElementById(this.ControlId);
            if (item != null) {
                this.SetControlPosition();
                document.getElementById(this.DropdownId).style.width = item.offsetWidth + 50 + "px";

                var itemsCount = this.GetItemsCount();
                var itemsHeight = ((itemsCount * 23) + 3);
  
                if (itemsHeight > this.MaxHeight) {
                    document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                    document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                }

                else {
                    document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                    //document.getElementById(this.DropdownId).style.minHeight = "350px";
                    document.getElementById(this.ListControlId).style.height = "100%";
                }


                document.getElementById(this.DropdownId).style.visibility = "visible";
            }
        }

    }
    private timerToken: any;
    private GetItemsCount() {
        var itemsCount = 9;
        if (this.HasAdvancedFiltersOptionsToggle) {
            itemsCount = itemsCount + 2;
        }
        if (this.ObjectField.IsRequiered == false) {
            itemsCount = itemsCount + 1;
        }
        return itemsCount;
    }

    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.SetControlPosition(), 1);
    }

    private SetControlPosition() {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.position = "fixed";
            document.getElementById(this.DropdownId).style.top = (itemRect.top + 22) + 'px';
            document.getElementById(this.DropdownId).style.left = itemRect.left + 'px';
        }
    }
    OnFocus() {
        this.RunPositionTimer();
    }

    OnLostFocus() {
        if (this.mouseOver == false) {
            this.StopPositionTimer();
            if (document.getElementById(this.DropdownId)) {
                document.getElementById(this.DropdownId).style.height = "0px";
                document.getElementById(this.DropdownId).style.visibility = "hidden";
            }
        }
    }

    private IsOpened: boolean = false;
    ComboBoxClicked() {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.IsOpened = !this.IsOpened;

            if (!this.IsOpened) {
                item.blur();
            }
        }
    }

    ItemClicked(clickedItem: any) {
        //if (clickedItem == "Less Than" || clickedItem == "Greater Than") {
        //    this.CloseMenu = false;
        //}
        if (clickedItem != null) {
            if (this.SelectedItem != clickedItem) {
                this.SelectedItem = clickedItem;
                this.mouseOver = false;
                this.GreaterTextValue = null;
                this.LesstextValue = null;
                this.IsMenuOpened = false;
                this.StopPositionTimer();
                this.OnLostFocus();
                //var myComboBox = document.getElementById(this.ControlId);
                //if (myComboBox != null) {
                //    myComboBox.blur();
                //}

                //this.SetDisplayText();
                //this.SelectedItemChanged.emit(this.SelectedItem);
            }
        }
    }

    private SetDisplayText() {
        var myDisplayText: string = null;

        if (this.SelectedItem != null && this.SelectedItem != "Choose") {
            myDisplayText = this.Text;
        }

        this.Text = myDisplayText;
    }

    private selectedItem: any;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: any) {
        this.selectedItem = newValue;
        if (newValue != null) {
            switch (newValue) {
                case "Today":
                    {
                        this.Text = "Today " + this.Today;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.TodayDate, ToDate: this.TommorowDate, Operation: "Equals", MyName: "Today" });
                        break;
                    }
                case "Yesterday":
                    {
                        this.Text = "Yesterday " + this.Yesterday;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.YesterdayDate, ToDate: this.TodayDate, Operation: "Equals", MyName: "Yesterday" });
                        break;
                    }
                case "Last 7 Days":
                    {
                        this.Text = "Last 7 Days " + this.LastSevenDays;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.LastSevenDaysDate, ToDate: this.TommorowDate, Operation: "Equals", MyName: "Last 7 Days" });
                        break;
                    }
                case "Last 30 Days":
                    {
                        this.Text = "Last 30 Days " + this.LastThirtyDays;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.LastThirtyDaysDate, ToDate: this.TommorowDate, Operation: "Equals", MyName: "Last 30 Days" });
                        break;
                    }
                case "Current Year":
                    {
                        this.Text = "Current Year " + this.CurrentYear;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.CurrentYearFromDate, ToDate: this.CurrentYearToDate, Operation: "Equals", MyName: "Current Year" });
                        break;
                    }
                case "Last Year":
                    {
                        this.Text = "Last Year " + this.LastYear;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.LastYearFromDate, ToDate: this.LastYearToDate, Operation: "Equals", MyName: "Last Year" });
                        break;
                    }
                case "No Date":
                    {
                        this.Text = "No Date";
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit("NoDate");
                        break;
                    }
                case "Greater Than":
                    {
                        //this.SelectedItemChanged.emit(this.LastSevenDays);
                        break;
                    }
                case "Less Than":
                    {
                        //this.SelectedItemChanged.emit(this.LastSevenDays);
                        break;
                    }
                case "Less than Today":
                    {
                        this.Text = "Less than Today " + this.LessThanToday;
                        this.SelectedItemChanged.emit({ FromDate: null, ToDate: this.YesterdayDate, Operation: "Equals", MyName: "Less than Today" });
                        break;
                    }
                case "Less than or equal Today":
                    {
                        this.Text = "Less than or equal Today " + this.LessThanOrEqualToday;
                        this.SelectedItemChanged.emit({ FromDate: null, ToDate: this.TodayDate, Operation: "Equals", MyName: "Less than or equal Today" });
                        break;
                    }
                default: { break; }
            }

        }
    }
    private greatertextValue: any;
    public get GreaterTextValue() { return this.greatertextValue; }
    public set GreaterTextValue(newValue: any) {
        this.greatertextValue = newValue;
        if (newValue != null) {
            this.lesstextValue = null;
            this.Text = "Greater Than" + " " + newValue.getDate() + '/' + (newValue.getMonth() + 1) + '/' + newValue.getFullYear();
            this.SelectedItem = "Greater Than";
            this.SelectedItemChanged.emit({ Date: newValue, Operation: "LargerThan" });
            this.CloseMenu = true;
            this.mouseOver = false;
            this.OnLostFocus();
        }
        else {
            if (this.LesstextValue == null && this.SelectedItem == "Greater Than") {
                this.Text = null;

                this.SelectedItem = "Greater Than";
                this.SelectedItemChanged.emit({ Date: "", Operation: "LargerThan" });
                this.CloseMenu = true;
                this.mouseOver = false;
                this.OnLostFocus();
            }
        }
    }
    private lesstextValue: any;
    public get LesstextValue() { return this.lesstextValue; }
    public set LesstextValue(newValue: any) {
        this.lesstextValue = newValue;
        if (newValue != null) {
            this.greatertextValue = null;
            this.Text = "Less Than" + " " + newValue.getDate() + '/' + (newValue.getMonth() + 1) + '/' + newValue.getFullYear();
            this.SelectedItem = "Less Than";
            this.SelectedItemChanged.emit({ Date: newValue, Operation: "LessThan" });
            this.CloseMenu = true;
            this.mouseOver = false;
            this.OnLostFocus();
        }
        else {
            if (this.GreaterTextValue == null && this.SelectedItem == "Less Than") {
                this.Text = null;

                this.SelectedItem = "Less Than";
                this.SelectedItemChanged.emit({ Date: "", Operation: "LessThan" });
                this.CloseMenu = true;
                this.mouseOver = false;
                this.OnLostFocus();
            }
        }
    }
    ChooseDatesClicked() {
        this.GreaterTextValue = null;
        this.LesstextValue = null;
        var logitudeWindow = new LogitudeWindow();

        var windowArgs: any = {};
        windowArgs.QueryCode = this.QueryCode;


        logitudeWindow.Width = 408;
        logitudeWindow.Height = 330;
        logitudeWindow.Title = "Choose Dates";
        logitudeWindow.WindowArgs = windowArgs;
        
        logitudeWindow.Show('./Infrastructure/Components/CustomControls/ChooseDatesComponent');
        this.mouseOver = false;
        logitudeWindow.ComponentLoaded.subscribe((cmp) => {
            cmp.DateSelected.subscribe((res) => {
                this.FromDate = res.From;
                this.FromDate.setHours(0, 0, 0, 0);
                this.ToDate = res.To;
                this.ToDate.setHours(23, 59, 59, 999);
                this.SelectedItemChanged.emit({ FromDate: this.FromDate, ToDate: this.ToDate, Operation: "Between" });
                this.SelectedItem = this.FromDate.getDate() + '/' + (this.FromDate.getMonth() + 1) + '/' + this.FromDate.getFullYear() + "-" + this.ToDate.getDate() + '/' + (this.ToDate.getMonth() + 1) + '/' + this.ToDate.getFullYear();
                this.Text = this.FromDate.getDate() + '/' + (this.FromDate.getMonth() + 1) + '/' + this.FromDate.getFullYear() + "-" + this.ToDate.getDate() + '/' + (this.ToDate.getMonth() + 1) + '/' + this.ToDate.getFullYear();
                this.SetDisplayText();
                //alert(this.FromDate + " - " + this.ToDate);
            });
        });
        this.CloseMenu = true;
        this.OnLostFocus();
    }

    OnCalendarClick() {
        //console.log("OnCalendarClick");
        //this.CloseMenu = false; 
        ////alert("Hi");
        //this.IsMenuOpened = false;
        ////this.mousedown();
        //var DropdownId = this.DropdownId;
        //var ss = document.activeElement;
        //ss.addEventListener("blur", function (event) {
        //    document.getElementById(DropdownId).style.height = "0px";
        //    document.getElementById(DropdownId).style.visibility = "hidden"; 
        //});

    }

    onMouseOver() {
        this.mouseOver = true;
    }
    onMouseOut() {
        this.mouseOver = false;
    }
}
