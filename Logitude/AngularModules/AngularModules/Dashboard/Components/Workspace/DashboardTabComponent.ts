import { Component, AfterViewInit } from '@angular/core';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DashboardMapping } from 'Dashboard/Services/DashboardMapping';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { MixPanelLocator } from '../../../Common/MixPanel/MixPanelLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { DashboardAnalyticsService } from '../../../DashboardModule/Services/DashboardAnalyticsService';
import { CustomDashboardComponent } from './CustomDashboardComponent';

@Component({
    templateUrl: 'DashboardTabComponent.html',
    selector: 'DashboardTabComponent',
})

export class DashboardTabComponent implements AfterViewInit {
    public SelectedDashboardId: string = null;
    public OpenEditLayout: boolean = false;
    public SelectedDashboardName: string = null;
    public SelectedDashboard: DashboardPM;
    private dashboardPMService: DashboardPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public reactWidgetsLayout: { lg: ReactWidgetPM[] } = { lg: [] };
    public IsGlobalFiltersOpened: boolean = false;
    public newWidgetWidth = 3;
    public newWidgetHeight = 5;
    public CloneDashboardLayout: WidgetPM[];
    public Show: boolean = false;
    public FatherComponent: CustomDashboardComponent;
    public GlobalFilters: string;

    constructor() {
        this.dashboardPMService = new DashboardPMService();
    }

    ngAfterViewInit(): void {
        this.Show = true;
    }

    Intialize(args: any) {
        this.SelectedDashboardId = args['SelectedDashboardId'];
        this.OpenEditLayout = args['OpenEditLayout'];
        this.FatherComponent = args['FatherComponent'];
        this.Listen();
        this.LoadSelectedDashboard();
    }

    private Listen() {
        if (this.FatherComponent != null) {
            this.FatherComponent.SaveDashboardCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.SelectedDashboard = this.FatherComponent.SavedDashboard;
                    this.applyWDashboard();
                    this.ResetFlags();
                    this.FatherComponent.NavigateToSelectedTab();
                }
            });

            this.FatherComponent.EditLayoutChanged.subscribe((isSuccess: boolean) => {
                if (isSuccess) {
                    this.applyWDashboard();
                    this.ResetFlags();
                    this.FatherComponent.NavigateToSelectedTab();
                }
            });
        }
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
        if (this.hasChanges != value) {
            this.hasChanges = value;

            if (this.IsEditLayoutModeActive) {
                this.FatherComponent.HasChanges = value;
                this.FatherComponent.SavedDashboard = this.SelectedDashboard;
            }
        }
    }

    private isEditLayoutModeActive: boolean = false;
    get IsEditLayoutModeActive() { return this.isEditLayoutModeActive; }
    set IsEditLayoutModeActive(value: boolean) {
        if (this.isEditLayoutModeActive != value) {
            this.isEditLayoutModeActive = value;
            this.FatherComponent.IsEditLayoutModeActive = value;
        }
    }

    public IsEditDashboardButtonVisible: boolean = false;
    public IsEditLayoutButtonVisible: boolean = !this.IsEditLayoutModeActive && !AppTool.IsNullOrEmpty(this.SelectedDashboardId);

    private LoadSelectedDashboard() {
        this.dashboardPMService.get(this.SelectedDashboardId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                this.IsEditLayoutButtonVisible = !this.IsEditLayoutModeActive && this.SelectedDashboard && this.SelectedDashboard.CreatedByUserId == SessionLocator.LoggedUserId;

                if (this.SelectedDashboard) {
                    this.applyWDashboard();
                }

                else {
                    this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone({ lg: [] }));
                }

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    get IsEmptyDashboardVisible() {
        return !this.IsEditLayoutModeActive && this.SelectedDashboard && this.SelectedDashboard.Widgets.length == 0;
    }

    get IsPermissionMessageVisible() {
        return !this.IsEditLayoutModeActive && this.SelectedDashboard && this.SelectedDashboard.CreatedByUserId != SessionLocator.LoggedUserId;
    }

    applyWDashboard() {
        this.SelectedDashboardId = this.SelectedDashboard.Id;
        this.SelectedDashboardName = this.SelectedDashboard.Name;
        this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
        this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone(this.reactWidgetsLayout));

        if (this.OpenEditLayout) this.EditLayoutClicked();
    }
    private BindReactWidgets(widgets: WidgetPM[]) {
        var reactWidgets: ReactWidgetPM[] = [];

        widgets.forEach(item => {
            item.Key = item.Id ?? item.Key;
            reactWidgets.push(DashboardMapping.GetReactWidget(item));
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
        MixPanelLocator.PostDashboardAction({ ActionName: "Refresh Click", DashboardId: this.SelectedDashboard?.Id });
        for (const item of this.reactWidgetsLayout.lg) {
            item.GlobalFilters = this.GlobalFilters;
            this.DashboardDataBinding.onEditWidget.next(item);
        }
    }

    EditDashboardClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Dashboard edit page", DashboardId: this.SelectedDashboard?.Id });

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Dashboard";
        logitudeWindow.WindowArgs = { EntityPM: this.SelectedDashboard, };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (s == "OK_delete") {
                        this.FatherComponent.RefreshTabsAfterDelete(this.SelectedDashboard?.Id);
                    }

                    else {
                        this.SelectedDashboardName = this.SelectedDashboard.Name;
                        this.FatherComponent.RefereshTabAfterEdit(this.SelectedDashboard);
                    }
                }
            });
        });
    }

    SaveDashboard(fromUI: boolean = false) {
        //this.CheckDeletedWidgets(dashboard);
        if (fromUI) MixPanelLocator.PostDashboardAction({ ActionName: "Submit Dashboard Save Click", DashboardId: this.SelectedDashboard?.Id });

        this.CurrentSession.StartBusyIndicatorSaving();
        this.dashboardPMService.update(this.SelectedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                this.SelectedDashboardId = this.SelectedDashboard?.Id;
                this.SelectedDashboardName = this.SelectedDashboard?.Name;
                this.applyWDashboard();
                this.ResetFlags();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private ResetFlags() {
        this.IsEditLayoutButtonVisible = !AppTool.IsNullOrEmpty(this.SelectedDashboardId) && this.SelectedDashboard.CreatedByUserId == SessionLocator.LoggedUserId;
        this.IsEditDashboardButtonVisible = false;
        this.IsEditLayoutModeActive = false;
        this.HasChanges = false;
        this.FatherComponent.HasChanges = false;
    }

    AddWidgetClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Widget add page", DashboardId: this.SelectedDashboard?.Id });
        var myWidget: WidgetPM = new WidgetPM(this.SelectedDashboard);
        myWidget.Tenant = SessionInfo.LoggedUserTenant;
        myWidget.StartPotistion = `${0},${100}`;
        myWidget.EndPosition = `${3},${5}`;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: true, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
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
        if (myWidget.TypeCode == "line" || myWidget.TypeCode == "bar") {
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
        return result[0].SeriesMeasureVulues.length;
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
        reactWidget.GlobalFilters = this.GlobalFilters;
        this.DashboardDataBinding.onAddWidget.next(reactWidget);
    }

    EditLayoutClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Edit Layout Clicked", DashboardId: this.SelectedDashboard?.Id });
        this.HasChanges = false;
        this.IsEditLayoutButtonVisible = false;
        this.IsEditDashboardButtonVisible = this.SelectedDashboard.CreatedByUserId == SessionLocator.LoggedUserId;
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
        this.EditLayoutClicked();
        this.AddWidgetClicked();
    }

    openEditWidget(widget: ReactWidgetPM) {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Widget edit page", DashboardId: this.SelectedDashboard?.Id });
        var myWidget: WidgetPM = this.SelectedDashboard.Widgets.find(d => d.Key == widget.key);
        myWidget.Key = widget.key;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: false, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    var widget = DashboardMapping.GetReactWidget(comp.EntityPM);
                    widget.GlobalFilters = this.GlobalFilters;
                    this.DashboardDataBinding.onEditWidget.next(widget);
                    this.HasChanges = true;
                }
            });
        });
    }

    ApplyFilters(filters: string) {
        // this.DashboardDataBinding.onApplyGlobalFilters.next(filters);
        this.GlobalFilters =  filters ?? "[]";
        for (const item of this.reactWidgetsLayout.lg) {
            item.GlobalFilters = this.GlobalFilters;
            this.DashboardDataBinding.onEditWidget.next(item);
        }
    }
}
