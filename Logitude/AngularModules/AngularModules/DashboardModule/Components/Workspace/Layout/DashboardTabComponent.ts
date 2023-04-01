import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { DashboardPM } from '../../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../../DashboardModule/EntityPMs/WidgetPM';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { DashboardPMService } from '../../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardMapping } from 'DashboardModule/Tools/DashboardMapping';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { MixPanelLocator } from '../../../../Common/MixPanel/MixPanelLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { DashboardAnalyticsService } from '../../../../DashboardModule/Services/DashboardAnalyticsService';
import { DashboardListService } from '../../../../DashboardModule/Services/StandardLists/DashboardListService';
import { DashboardList } from '../../../../DashboardModule/EntityLists/DashboardList';
import { DashboardCopyService } from 'DashboardModule/Tools/DashboardCopyService';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { GlobalFilterItem } from 'DashboardModule/Components/Windows/Filter/GlobalFilter/GlobalFilterItem';
import { AnalyticsFactsFieldsMetaDataPM } from 'DashboardModule/EntityPMs/AnalyticsFactsFieldsMetaDataPM';
import { formatDate } from '@angular/common';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { DashboardPMExtendedService } from 'DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';

@Component({
    templateUrl: 'DashboardTabComponent.html',
    selector: 'dashboard-tab',
})

export class DashboardTabComponent implements OnInit {
    @Input() DashboardId: string = null;
    @Input() OpenEditLayout: boolean = false;
    @Input() PresetFilters: AnalyticsFactsFieldsMetaDataPM[];
    @Output() DashboardDeleted = new EventEmitter<string>();
    @Output() DashboardChanged = new EventEmitter<DashboardPM>();
    @Output() TabHasChanges = new EventEmitter<boolean>();
    @Output() DashboardEntity = new EventEmitter<DashboardPM>();
    @Output() RefreshAfterCopy = new EventEmitter<DashboardPM>();

    public SelectedDashboardName: string = null;
    public SelectedDashboard: DashboardPM;
    private dashboardPMService: DashboardPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public reactWidgetsLayout: { lg: ReactWidgetPM[] } = { lg: [] };
    public IsGlobalFiltersOpened: boolean = false;
    public newWidgetWidth = 3;
    public newWidgetHeight = 5;
    public CloneDashboardLayout: WidgetPM[];
    public GlobalFilters: GlobalFilterItem[];
    private DashboardListService: DashboardListService;
    public ItemsSource: DashboardList[] = [];
    public CanCopy: boolean;
    public FilterCount: number = 0;
    public DateRangeLabel: string;
    public DateRangeNumber: number;
    public ValidationErrorsList: string[];

    constructor() {
        this.dashboardPMService = new DashboardPMService();
        this.DashboardListService = new DashboardListService();
        this.CanCopy = FeatureLocator.HasFeaturePermession("Dashboard", "CopyDashboard");
    }

    ngOnInit(): void {
        this.LoadSelectedDashboard();
    }

    DashboardDataBinding: DashboardDataBinding = {
        isOnEditLayout: new Subject(),
        onGetLayouts: new BehaviorSubject({ lg: [] }),
        widgetUpdated: new Subject(),
        onAddWidget: new Subject(),
        onEditWidget: new Subject(),
    }

    private hasChanges: boolean = false;
    get HasChanges() { return this.hasChanges; }
    set HasChanges(value: boolean) {
        if (this.hasChanges == value) return;
        this.hasChanges = value;
        if (this.IsEditLayoutModeActive) {
            this.DashboardEntity.emit(this.SelectedDashboard);
        }

        if (!this.IsEditLayoutModeActive && value) return;
        this.TabHasChanges.emit(value);
    }

    private isEditLayoutModeActive: boolean = false;
    get IsEditLayoutModeActive() { return this.isEditLayoutModeActive; }
    set IsEditLayoutModeActive(value: boolean) {
        if (this.isEditLayoutModeActive != value) {
            this.isEditLayoutModeActive = value;
        }
    }

    public IsEditDashboardButtonVisible: boolean = false;
    public IsEditLayoutButtonVisible: boolean = !this.IsEditLayoutModeActive && !AppTool.IsNullOrEmpty(this.DashboardId);

    private LoadSelectedDashboard(refreshWidgets: boolean = false) {
        this.dashboardPMService.get(this.DashboardId).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) return;
            this.SelectedDashboard = myResponse.Result;
            this.SetIsEditLayoutButtonVisible();
            if (this.SelectedDashboard) this.applyWDashboard();
            else this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone({ lg: [] }));
            if (refreshWidgets) this.RefreshWidgets();
        });
    }

    private SetIsEditLayoutButtonVisible() {
        this.IsEditLayoutButtonVisible = !this.IsEditLayoutModeActive
            && this.SelectedDashboard
            && this.SelectedDashboard.Tenant == SessionLocator.Tenant
            && (this.SelectedDashboard.CreatedByUserId == SessionLocator.LoggedUserId || SessionLocator.LoggedUserPM.IsCustomerCare);
    }

    get IsEmptyDashboardVisible() {
        return !this.IsEditLayoutModeActive && this.SelectedDashboard && this.SelectedDashboard.Widgets.length == 0;
    }

    get IsPermissionMessageVisible() {
        return !this.IsEditLayoutModeActive
            && this.SelectedDashboard
            && this.SelectedDashboard.CreatedByUserId != SessionLocator.LoggedUserId
            && !SessionLocator.LoggedUserPM.IsCustomerCare;
    }

    applyWDashboard() {
        this.DashboardId = this.SelectedDashboard.Id;
        this.SelectedDashboardName = this.SelectedDashboard.Name;
        this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
        this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone(this.reactWidgetsLayout));
        if (this.OpenEditLayout) this.EditDashboardLayoutClicked();
    }

    private BindReactWidgets(widgets: WidgetPM[]) {
        var reactWidgets: ReactWidgetPM[] = [];

        widgets.forEach(item => {
            item.Key = item.Id ?? item.Key;
            var reactWidget = DashboardMapping.GetReactWidget(item);
            reactWidget.GlobalFilters = this.GetWidgetGlobalFilters(reactWidget);
            reactWidgets.push(reactWidget);
        });
        return { lg: reactWidgets };
    }

    onReactChangeLayouts(layouts: { lg: ReactWidgetPM[]; }) {
        if (!this.SelectedDashboard) return;
        layouts.lg.forEach(item => {
            var myWidget: WidgetPM = this.SelectedDashboard.Widgets.find(d => d.Key == item.key);
            if (myWidget) {
                myWidget.EndPosition = item.EndPosition;
                myWidget.StartPotistion = item.StartPotistion;
            }
        });
        this.CheckDeletedWidgets(layouts.lg);
        this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
        MixPanelLocator.PostDashboardAction({ ActionName: "Layout Changed", DashboardId: this.SelectedDashboard?.Id });
        this.HasChanges = true;
    }

    private CheckDeletedWidgets(teactWidgets: ReactWidgetPM[]) {
        var deletedWidgets: WidgetPM[] = [];

        if (this.SelectedDashboard) {
            this.SelectedDashboard.Widgets.forEach(item => {
                if (teactWidgets.find(d => d.Id == item.Id) == null) {
                    deletedWidgets.push(item);
                }
            });

            deletedWidgets.forEach(item => {
                MixPanelLocator.PostDashboardAction({ ActionName: "Widget Remove Click", Message: "Widget Id : " + item?.Id, DashboardId: this.SelectedDashboard?.Id });
                this.SelectedDashboard.RemoveWidget(item);
            });
        }
    }

    RefreshLayoutClicked() {
        if (!this.SelectedDashboard) return;
        this.CurrentSession.StartBusyIndicatorLoading();
        MixPanelLocator.PostDashboardAction({ ActionName: "Refresh Click", DashboardId: this.SelectedDashboard?.Id });
        this.LoadSelectedDashboard(true);
    }

    RefreshWidgets() {
        for (const item of this.reactWidgetsLayout.lg) {
            item.GlobalFilters = this.GetWidgetGlobalFilters(item);
            this.DashboardDataBinding.onEditWidget.next(item);
        }
    }

    EditDashboardClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Dashboard edit page", DashboardId: this.SelectedDashboard?.Id });

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Dashboard";
        logitudeWindow.WindowArgs = { EntityPM: this.SelectedDashboard, };
        logitudeWindow.Show('./DashboardModule/Components/Windows/AddEditDashboard/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.SelectedDashboardName = this.SelectedDashboard.Name;
                    this.DashboardChanged.emit(this.SelectedDashboard);
                }
            });
        });
    }

    SaveDashboard() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Submit Dashboard Save Click", DashboardId: this.SelectedDashboard?.Id });

        this.CurrentSession.StartBusyIndicatorSaving();
        this.dashboardPMService.update(this.SelectedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                this.DashboardId = this.SelectedDashboard?.Id;
                this.SelectedDashboardName = this.SelectedDashboard?.Name;
                this.applyWDashboard();
                this.ResetFlags();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private ResetFlags() {
        this.IsEditLayoutButtonVisible = !AppTool.IsNullOrEmpty(this.DashboardId)
            && this.SelectedDashboard.Tenant == SessionLocator.Tenant
            && (this.SelectedDashboard.CreatedByUserId == SessionLocator.LoggedUserId || SessionLocator.LoggedUserPM.IsCustomerCare);

        this.IsEditDashboardButtonVisible = false;
        this.IsEditLayoutModeActive = false;
        this.HasChanges = false;
        this.TabHasChanges.emit(false);
    }

    AddWidgetClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Widget add page", DashboardId: this.SelectedDashboard?.Id });
        var myWidget: WidgetPM = new WidgetPM(this.SelectedDashboard);
        myWidget.Tenant = SessionInfo.LoggedUserTenant;
        myWidget.StartPotistion = `${0},${100}`;
        myWidget.EndPosition = `${3},${5}`;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Title = "Add Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: true, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./DashboardModule/Components/Windows/AddEditWidget/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.HasChanges = true;
                    this.HandelPossion(comp.EntityPM).subscribe(e => {
                        comp.EntityPM.StartPotistion = e.StartPotistion;
                        comp.EntityPM.EndPosition = e.EndPosition;
                        this.AddWidgetToReactLayout(comp.EntityPM);
                    });

                }
            });
        });
    }
    HandelPossion(myWidget: WidgetPM): Observable<{ StartPotistion: string, EndPosition: string }> {
        if (myWidget.TypeCode == "line" || myWidget.TypeCode == "bar" || myWidget.TypeCode == "column") {
            var subject = new Subject<{ StartPotistion: string, EndPosition: string }>();
            var dashboardAnalyticsService = new DashboardAnalyticsService();
            dashboardAnalyticsService.GetData(DashboardMapping.GetReactWidget(myWidget)).subscribe(e => {
                var result = e.Result;
                if (e.HasError) {
                    subject.next(this.GetDefaultPosition())
                    return;
                }
                var numberOfGroup = this.GetNumberOfGroup(result);

                subject.next(this.GetWidgetPosition(myWidget, numberOfGroup))
            }, error => {
                subject.next(this.GetDefaultPosition());
            });
            return subject;
        }

        if (myWidget.TypeCode == "kpi") {
            var behaviorSubject = new BehaviorSubject<{ StartPotistion: string, EndPosition: string }>(this.GetPosition(2, 2));
            return behaviorSubject;
        }

        var behaviorSubject = new BehaviorSubject<{ StartPotistion: string, EndPosition: string }>(this.GetDefaultPosition());
        return behaviorSubject;
    }

    GetDefaultPosition() {
        var position = this.EvaluateNewWidgetPosition(this.newWidgetWidth, this.newWidgetHeight);
        var startPotistion = `${position.x},${position.y}`;
        var endPosition = `${position.w},${position.h}`;
        return { StartPotistion: startPotistion, EndPosition: endPosition };
    }

    GetWidgetPosition(myWidget: WidgetPM, numberOfGroup) {
        if (myWidget.TypeCode == "line") {
            if (numberOfGroup > 25)
                return this.GetPosition(12, 6);
            if (numberOfGroup > 10)
                return this.GetPosition(9, 6);
            if (numberOfGroup > 5)
                return this.GetPosition(5, 5);
            return this.GetPosition(this.newWidgetWidth, this.newWidgetHeight);
        }
        if (myWidget.TypeCode == "bar") {
            if (numberOfGroup > 30)
                return this.GetPosition(5, 23);
            if (numberOfGroup > 25)
                return this.GetPosition(5, 16);
            if (numberOfGroup > 10)
                return this.GetPosition(5, 12);
            if (numberOfGroup > 5)
                return this.GetPosition(5, 7);
            return this.GetPosition(this.newWidgetWidth, this.newWidgetHeight);
        }
        return this.GetPosition(this.newWidgetWidth, this.newWidgetHeight);
    }

    GetPosition(w, h) {
        var position = this.EvaluateNewWidgetPosition(w, h);
        var startPotistion = `${position.x},${position.y}`;
        var endPosition = `${position.w},${position.h}`;
        return { StartPotistion: startPotistion, EndPosition: endPosition };
    }
    GetNumberOfGroup(result) {
        if (!result || result.length == 0) return 0;
        return result[0].Values.length;
    }
    EvaluateNewWidgetPosition(w, h) {
        let widgetYPosition = 0;
        let widgetXPosition = 0;
        let allLayouts = this.reactWidgetsLayout.lg.map(e => e.Layout);
        for (let y = 0; true; y++) {
            widgetYPosition = y;
            for (let x = 0; x < 10; x++) {
                widgetXPosition = x;

                var exisitWidgetInNewPosition = allLayouts.find(e => {
                    return e
                        && widgetXPosition < e.x + e.w && widgetXPosition + w > e.x
                        && widgetYPosition < e.y + e.h && widgetYPosition + h > e.y
                });
                if (!exisitWidgetInNewPosition && widgetXPosition + w <= 12)
                    return { x: widgetXPosition, y: widgetYPosition, w: w, h: h };
            }
        }
    }

    AddWidgetToReactLayout(myWidget: WidgetPM) {
        var reactWidget = DashboardMapping.GetReactWidget(myWidget);
        this.reactWidgetsLayout.lg.push(reactWidget);
        reactWidget.GlobalFilters = this.GetWidgetGlobalFilters(reactWidget);
        this.DashboardDataBinding.onAddWidget.next(reactWidget);
    }

    ViewDashboardLayoutClicked() {
        if (this.HasChanges) {
            this.ConfirmSave();
        }else {
            this.ResetFlags();
        }
    }
    
    private ConfirmSave() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.NoButtonText = "Don't Save";
        confirmWindow.YesButtonText = "Save ";
        confirmWindow.CancelButtonText = "Cancel";
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        confirmWindow.Show("This Dashboard has unsaved changes do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window Yes Click", DashboardId: this.SelectedDashboard?.Id });
                this.SaveDashboard();
            }
            else if (confirmWindow.No) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window No Click", DashboardId: this.SelectedDashboard?.Id });                
                this.ResetFlags();
                this.RefreshLayoutClicked();
            }
        });
    }
    
    EditDashboardLayoutClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Edit Layout Clicked", DashboardId: this.SelectedDashboard?.Id });
        this.HasChanges = false;
        this.IsEditLayoutButtonVisible = false;
        this.IsEditDashboardButtonVisible = this.SelectedDashboard.CreatedByUserId == SessionLocator.LoggedUserId || SessionLocator.LoggedUserPM.IsCustomerCare;
        this.IsEditLayoutModeActive = true;
        this.IsGlobalFiltersOpened = false;
        var cloneWidgets: WidgetPM[] = [];

        for (const item of this.SelectedDashboard.Widgets) {
            cloneWidgets.push(ServiceHelper.CloneEntityPM(item))
        }

        this.CloneDashboardLayout = cloneWidgets;
    }

    OpenFilterAreaClick() {
        this.IsGlobalFiltersOpened = true;
    }
    CloseFilterAreaClick() {
        this.IsGlobalFiltersOpened = false;
    }

    HereClicked() {
        this.EditDashboardLayoutClicked();
        this.AddWidgetClicked();
    }

    openEditWidget(widget: ReactWidgetPM) {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Widget edit page", DashboardId: this.SelectedDashboard?.Id });
        var myWidget: WidgetPM = this.SelectedDashboard.Widgets.find(d => d.Key == widget.key);
        myWidget.Key = widget.key;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Title = "Edit Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: false, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./DashboardModule/Components/Windows/AddEditWidget/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    var widget = DashboardMapping.GetReactWidget(comp.EntityPM);
                    widget.GlobalFilters = this.GetWidgetGlobalFilters(widget);
                    this.DashboardDataBinding.onEditWidget.next(widget);
                    this.HasChanges = true;
                }
            });
        });
    }

    ApplyFilters(filters: GlobalFilterItem[]) {
        this.GlobalFilters = filters;
        this.SetDateRangeForCompareWithPrevious();
        for (const widget of this.reactWidgetsLayout.lg) {
            this.ApplyGlobalFilterToWidget(widget);
        }
    }

    SetDateRangeForCompareWithPrevious() {
        if (!this.GlobalFilters || this.GlobalFilters.length == 0) {
            this.DateRangeLabel = "";
            return;
        }
        var compareItem = this.GlobalFilters.find((x: any) => x.compareWithPrevious) as any;

        if (!compareItem?.compareWithPrevious) {
            this.DateRangeLabel = "";
            return;
        }
        if (compareItem?.Operator == "Between") {
            this.DateRangeLabel = this.ParseDateFormat(compareItem.fieldValue) + " - " + this.ParseDateFormat(compareItem.fieldValue2) + "    vs    " + this.GetCompareFromDateForBetween(compareItem.fieldValue, compareItem.fieldValue2) + " - " + this.GetCompareToDateForBetween(compareItem.fieldValue);
        }

        if (compareItem?.Operator == "Previous") {
            this.SetPreviousDates(compareItem);
        }
    }

    SetPreviousDates(compareItem: any) {
        var curDate = new Date(Date.now());
        let formattedDate = this.FormatDate(curDate);

        if (compareItem.dateGroupCode == "Quarter") {
            this.GetDateRangeLabelForQuarter(compareItem);
            return;
        }

        this.DateRangeLabel = this.GetFromDateForPrevious(compareItem) + " - " + formattedDate + "    vs    " + this.GetCompareFromDateForPrevious(compareItem, this.GetFromDateForPrevious(compareItem))
            + " - " + this.GetCompareToDateForPrevious(this.GetFromDateForPrevious(compareItem));
    }

    GetDateRangeLabelForQuarter(compareItem: any) {

        var numberPeriod = +compareItem.fieldValue3;
        var FromDate = this.GetFromDateForPrevious(compareItem);
        var CompareFromDate = this.GetCompareFromDateForQuarter(new Date(FromDate), numberPeriod);

        this.DateRangeLabel = FromDate + " - " + this.GetEndOfQuarter(new Date(FromDate), numberPeriod) + "    vs    " + CompareFromDate
            + " - " + this.GetEndOfQuarter(new Date(CompareFromDate), numberPeriod);
    }

    ParseDateFormat(date: string): string {
        var year = date.substring(0, 4);
        var month = date.substring(4, 6);
        var day = date.substring(6, 8);

        var dateString = year + "/" + month + "/" + day;
        return dateString;
    }

    GetCompareFromDateForBetween(fieldValue, fieldValue2): string {
        let dateFrom = new Date(this.ParseDateFormat(fieldValue));
        let dateTo = new Date(this.ParseDateFormat(fieldValue2));

        let numberOfDays = Math.floor((Date.UTC(dateTo.getFullYear(), dateTo.getMonth(), dateTo.getDate()) - Date.UTC(dateFrom.getFullYear(), dateFrom.getMonth(), dateFrom.getDate())) / (1000 * 60 * 60 * 24));

        dateFrom.setDate(dateFrom.getDate() - numberOfDays);
        let formattedDate = this.FormatDate(dateFrom);
        return formattedDate;

    }

    GetCompareToDateForBetween(fieldValue): string {
        let dateFrom = new Date(this.ParseDateFormat(fieldValue));

        dateFrom.setDate(dateFrom.getDate() - 1);
        let formattedDate = this.FormatDate(dateFrom);
        return formattedDate;

    }

    GetFromDateForPrevious(compareItem: any): string {
        let dateFrom = new Date(Date.now());
        let stringPeriod = compareItem.fieldValue3;
        var numberPeriod: number = +stringPeriod;
        dateFrom = this.SetDateFromAccordingDateGroupCode(compareItem, dateFrom, numberPeriod);
        let formattedDate = this.FormatDate(dateFrom);
        return formattedDate;
    }

    GetCompareFromDateForPrevious(compareItem: any, dateFrom: any) {
        let dateFromHere = new Date(dateFrom);
        let stringPeriod = compareItem.fieldValue3;
        var numberPeriod: number = +stringPeriod;
        dateFromHere = this.SetDateFromAccordingDateGroupCode(compareItem, dateFromHere, numberPeriod);
        let formattedDate = this.FormatDate(dateFromHere);
        return formattedDate;
    }

    SetDateFromAccordingDateGroupCode(compareItem: any, dateFromHere: Date, numberPeriod: number): Date {
        if (compareItem.dateGroupCode == 'Day') {
            dateFromHere.setDate(dateFromHere.getDate() - numberPeriod);
        }
        if (compareItem.dateGroupCode == 'Week') {
            dateFromHere.setDate(dateFromHere.getDate() - (7 * numberPeriod));
        }
        if (compareItem.dateGroupCode == 'Month') {
            dateFromHere.setMonth(dateFromHere.getMonth() - numberPeriod);
        }
        if (compareItem.dateGroupCode == 'Quarter') {
            var month = this.GetStartCurrentQuarter(dateFromHere);
            dateFromHere.setMonth(month);
            dateFromHere = new Date(dateFromHere.getFullYear(), dateFromHere.getMonth(), 1);
            dateFromHere.setMonth(dateFromHere.getMonth() - (3 * numberPeriod));
        }

        if (compareItem.dateGroupCode == 'Year') {
            dateFromHere.setFullYear(dateFromHere.getFullYear() - numberPeriod);
        }

        return dateFromHere;
    }
    GetEndOfQuarter(date: Date, numberPeriod: number): string {

        var lastDayInQuarter = new Date(date);
        lastDayInQuarter.setMonth(lastDayInQuarter.getMonth() + (3 * numberPeriod));
        lastDayInQuarter.setDate(lastDayInQuarter.getDate() - 1);
        let formattedDate = this.FormatDate(lastDayInQuarter);
        return formattedDate;
    }

    GetCompareFromDateForQuarter(date: Date, numberPeriod: number) {
        date = new Date(date.getFullYear(), date.getMonth(), 1);
        date.setMonth(date.getMonth() - (3 * numberPeriod));
        let formattedDate = this.FormatDate(date);
        return formattedDate;
    }

    GetStartCurrentQuarter(date: Date): number {
        if (date.getMonth() >= 4 && date.getMonth() <= 6)
            return 3;
        else if (date.getMonth() >= 7 && date.getMonth() <= 9)
            return 6;
        else if (date.getMonth() >= 10 && date.getMonth() <= 12)
            return 9;
        else
            return 0;
    }

    GetCompareToDateForPrevious(dateFrom: any) {
        let dateFromHere = new Date(dateFrom);

        dateFromHere.setDate(dateFromHere.getDate() - 1);
        let formattedDate = this.FormatDate(dateFromHere);
        return formattedDate;
    }

    FormatDate(date: Date): string {
        const format = 'yyyy/MM/dd';
        const locale = 'en-US';
        const formattedDate = formatDate(date, format, locale);
        return formattedDate;
    }

    private ApplyGlobalFilterToWidget(widget: ReactWidgetPM) {
        var widgetGlobalFilters = this.GetWidgetGlobalFilters(widget);
        if (!this.WidgetFilterChanged(widget, widget.GlobalFilters, widgetGlobalFilters)) return;
        widget.GlobalFilters = widgetGlobalFilters;
        this.DashboardDataBinding.onEditWidget.next(widget);
    }

    private WidgetFilterChanged(widget: ReactWidgetPM, oldFilterString: string, newFilterString: string): boolean {
        if (oldFilterString == newFilterString) return false;
        let oldFitlers: GlobalFilterItem[] = JSON.parse(oldFilterString ?? "[]") ?? [];
        let newFilters: GlobalFilterItem[] = JSON.parse(newFilterString ?? "[]") ?? [];

        var hasChanges = false;
        if (oldFitlers.length != newFilters.length) return true;
        newFilters.forEach(newFilterItem => {
            hasChanges = this.WidgetFilterItemChanged(widget, oldFitlers, newFilterItem);
            if (hasChanges) return;
        });

        if (!hasChanges) {
            oldFitlers.forEach(oldFilterItem => {
                this.WidgetFilterItemDeleted(newFilters, oldFilterItem);
            });
        }
        return hasChanges;
    }
    WidgetFilterItemDeleted(newFilters: GlobalFilterItem[], oldFilterItem: GlobalFilterItem): boolean {
        var newFilterItem = newFilters.find(x => x.FieldId == oldFilterItem.FieldId);
        if (!newFilterItem) {
            return true;
        }
        return false;
    }

    WidgetFilterItemChanged(widget: ReactWidgetPM, oldFilters: GlobalFilterItem[], newFilterItem: any): boolean {
        var oldFilterItem = oldFilters.find(x => x.FieldId == newFilterItem.FieldId) as any;
        if (!oldFilterItem) {
            return true;
        }

        if (oldFilterItem.fieldValue != newFilterItem.fieldValue ||
            oldFilterItem.fieldValue2 != newFilterItem.fieldValue2 ||
            oldFilterItem.fieldValue3 != newFilterItem.fieldValue3 ||
            oldFilterItem.dateGroupCode != newFilterItem.dateGroupCode ||
            oldFilterItem.Operator != newFilterItem.Operator) {
            return true;
        }

        if (widget.TypeCode == 'kpi' && oldFilterItem.compareWithPrevious != newFilterItem.compareWithPrevious) {
            return true;
        }
        return false;
    }

    GetWidgetGlobalFilters(widget: ReactWidgetPM): string {
        if (!this.GlobalFilters || this.GlobalFilters.length == 0) return null;
        var widgetFilters = [];
        this.GlobalFilters.forEach(item => {
            if ((!item.IsPreset && item.DataSetId == widget.EntityId)) widgetFilters.push(item);
            this.HandlePresetFilter(item, widget, widgetFilters);
        });
        if (!widgetFilters || widgetFilters.length == 0) return null;
        return JSON.stringify(widgetFilters, function (key, val) {
            if (key !== "Component" && key !== "UIProperties") return val;
        });
    }

    private HandlePresetFilter(item: GlobalFilterItem, widget: ReactWidgetPM, widgetFilters: any[]) {
        if (!item.IsPreset) return;
        var field = this.PresetFilters.find(x => x.CommonFilterCode == item.FieldId && x.AnalyticsFactsMetaDataId == widget.EntityId);
        if (!field) return;

        item.FieldName = field.FieldCode;
        item.DataTypeCode = field.DataTypeCode;
        widgetFilters.push(item);
    }

    DeleteDashboardClicked() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm";
        confirmWindow.Show("Are you sure you want to permanently delete this dashboard?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Deleting...");
                var service: DashboardPMExtendedService = new DashboardPMExtendedService();
                service.Delete(this.SelectedDashboard?.Id).subscribe((myResponse: ServiceResponse) => {
                    this.OnDeleteCompleted(myResponse);
                });
            }
        });
    }

    private OnDeleteCompleted(myResponse: ServiceResponse) {
        this.CurrentSession.StopBusyIndicator();
        if (!myResponse.HasError){
            this.DashboardDeleted.emit(this.SelectedDashboard?.Id);
        }
        // else{
        //     this.ValidationErrorsList = myResponse.ErrorsArray;
        // }
    }

    CopyDashboardClicked() {
        this.CopyDashboard();
    }

    CopyDashboard() {
        var dashboardPM = DashboardCopyService.CopyDashboard(this.SelectedDashboard);
        this.CreateCopiedDashBoard(dashboardPM);
    }

    CreateCopiedDashBoard(dashboardPM: DashboardPM) {
        this.dashboardPMService.insert(dashboardPM)
            .subscribe((myResponse: ServiceResponse) => {
                this.ChangeToCopyDashborad(myResponse);
            });
    }

    ChangeToCopyDashborad(myResponse: ServiceResponse) {
        this.SelectedDashboard = myResponse.Result;
        if (this.SelectedDashboard) {
            this.OpenEditDashboardWindowForCopiedDashboard();
        }

        else {
            this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone({ lg: [] }));
        }

        this.CurrentSession.StopBusyIndicator();
    }

    OpenEditDashboardWindowForCopiedDashboard() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Dashboard edit page for copied dashboard", DashboardId: this.SelectedDashboard?.Id });

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Copy Dashboard";
        logitudeWindow.WindowArgs = { EntityPM: this.SelectedDashboard, };
        logitudeWindow.Show('./DashboardModule/Components/Windows/AddEditDashboard/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (s == "OK_delete") {
                        this.DashboardDeleted.emit(this.SelectedDashboard?.Id);
                    }
                    else {
                        this.SelectedDashboardName = this.SelectedDashboard.Name;
                        this.RefreshAfterCopy.emit(this.SelectedDashboard);
                    }
                }
            });
        });

    }

    public ApplyFiltersCountChange(count: number) {
        this.FilterCount = count;
    }

    public get HideShowFilterText(): string {
        var countText = (!this.FilterCount || this.FilterCount == 0) ? "" : "(" + this.FilterCount + ")";
        if (this.IsGlobalFiltersOpened) return "Hide filters" + countText;
        return "Show filters" + countText;
    }

}

