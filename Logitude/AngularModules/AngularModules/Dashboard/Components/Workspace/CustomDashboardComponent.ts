import { Component, AfterViewInit, OnInit,Input, OnDestroy } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BehaviorSubject, Subject } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { DashboardPMExtendedService } from '../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DashboardSharedUserPM } from '../../../DashboardModule/EntityPMs/DashboardSharedUserPM';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { DashboardMapping } from 'Dashboard/Services/DashboardMapping';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { LastFilterClass } from '../../../Infrastructure/Utilities/LastFilterClass';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';

@Component({
    templateUrl:'CustomDashboardComponent.html',
    styleUrls:['CustomDashboardComponent.css'],
    selector:'custom-dashboard',
})

export class CustomDashboardComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;    
    private dashboardPMService: DashboardPMService;
    private dashboardPMExtendedService: DashboardPMExtendedService;
    public SelectedDashboard: DashboardPM;
    public SelectedDashboardName: string;
    public DataContext = this;
    public newWidgetWidth = 3;
    public newWidgetHeight = 5;
    public CloneDashboardLayout: WidgetPM[] ;
    public reactWidgetsLayout:{lg:ReactWidgetPM[]} = {lg:[]};
    @Input('Show') Show;
    private filterName_SelectedDashboard: string = "SelectedDashboard";
    private filterControlNameSpace: string = "Workspace.CustomDashboard";
    constructor() {
        super();
        this.dashboardPMService = new DashboardPMService();
        this.dashboardPMExtendedService = new DashboardPMExtendedService();
        this.SelectedDashboard = new DashboardPM();
    }

    DashboardDataBinding: DashboardDataBinding = {
        isOnEditLayout: new Subject(),
        onGetLayouts: new BehaviorSubject({ lg: [] }),
        widgetUpdated: new Subject(),
        onAddWidget: new Subject(),
        onEditWidget: new Subject(),
    }

    ngOnInit(): void {
        this._entityResourceService.getEntityResourceByTableName("Dashboard").subscribe((res1: any) => {
            this._entityResourceService.getEntityResourceByTableName("Widget").subscribe((res2: any) => {
                this._entityResourceService.getEntityResourceByTableName("WidgetMeasure").subscribe((res2: any) => {
                    this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsMetaData").subscribe((res3: any) => {
                        this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsFieldsMetaData").subscribe((res4: any) => {
                            setTimeout(e => {
                                this.GetDefaultDashboard();
                            }, 70);
                        });
                    });
                });
            });
        });       
    }

    ngAfterViewInit(): void {
       
    }

    private SessionEvent: any = null;


    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    private GetDefaultDashboard() {
        var defaultId: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_SelectedDashboard);
        if (!AppTool.IsNullOrEmpty(defaultId)) {
            this.SelectedDashboardId = defaultId;
            this.ResetFlags();
            this.GetSingleDashboardWithWidgets(this.SelectedDashboardId);            
        }

        else {
            this.LoadDefaultDashboardFromServer();           
        }
    }
    private LoadDefaultDashboardFromServer() {
        this.dashboardPMExtendedService.GetDefaultDashboardId().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboardId = myResponse.Result;
                this.ResetFlags();
                this.GetSingleDashboardWithWidgets(this.SelectedDashboardId);
            }
        });
    }

    GetSingleDashboardWithWidgets(dashboardId: string) {
        this.dashboardPMService.get(dashboardId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                if (this.SelectedDashboard) {
                    this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
                    this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone(this.reactWidgetsLayout));
                    this.SelectedDashboardName = this.SelectedDashboard.Name;
                }

                else {
                    this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone({lg: []}));
                    this.SelectedDashboardName = null;
                }
            }           
        });        
    }
    private BindReactWidgets(widgets: WidgetPM[]) {
        var reactWidgets: ReactWidgetPM[] = [];

        widgets.forEach(item => {
            reactWidgets.push(DashboardMapping.GetReactWidget(item));
        });
        return {lg:reactWidgets};
    }

    private selectedDashboardId: string;
    get SelectedDashboardId() { return this.selectedDashboardId; }
    set SelectedDashboardId(value: string) {
        MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard drop down", DashboardId: this.SelectedDashboard?.Id });
        if (this.selectedDashboardId != value) {
            this.selectedDashboardId = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_SelectedDashboard, value);
            this.GetSingleDashboardWithWidgets(value);
        }
    }

    public BackButtonLable: string = "Back";
    public IsEditLayoutButtonVisible: boolean = !AppTool.IsNullOrEmpty(this.SelectedDashboardId);
    public IsEditDashboardButtonVisible: boolean = false;
    public IsEditLayoutModeActive: boolean = false;
    public HasChanges: boolean = false;
    private ResetFlags() {
        this.IsEditLayoutButtonVisible = !AppTool.IsNullOrEmpty(this.SelectedDashboardId);
        this.IsEditDashboardButtonVisible = false;
        this.IsEditLayoutModeActive = false;
        this.HasChanges = false;
    }

    EditLayoutClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Edit Layout Clicked", DashboardId: this.SelectedDashboard?.Id });
        this.HasChanges = false;
        this.IsEditLayoutButtonVisible = false;
        this.IsEditDashboardButtonVisible = true;
        this.IsEditLayoutModeActive = true;
        var cloneWidgets:WidgetPM[] = []
        for (const item of this.SelectedDashboard.Widgets) {
            cloneWidgets.push(ServiceHelper.CloneEntityPM(item))
        }
        this.CloneDashboardLayout = cloneWidgets;

    }

    AdDashboardClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Dashboard add page" });
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add Dashboard";

        var dashboardPM: DashboardPM = new DashboardPM();
        dashboardPM.Tenant = SessionInfo.LoggedUserTenant;
        dashboardPM.CreatedByUserId = SessionInfo.LoggedUserId;
        dashboardPM.UpdatedByUserId = SessionInfo.LoggedUserId;
        dashboardPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        dashboardPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        dashboardPM.PermissionLevelCode = "ONM";

        logitudeWindow.WindowArgs = { EntityPM: dashboardPM, };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.SelectedDashboard = comp.EntityPM;
                    this.SelectedDashboardId = comp.EntityPM.Id;
                    this.EditLayoutClicked();
                }
            });
        });
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
                        this.LoadDefaultDashboardFromServer();
                    }

                    else {
                        this.HasChanges = this.SelectedDashboard.IsDirty;
                    }
                }
            });
        });
    }

    AddWidgetClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Widget add page", DashboardId: this.SelectedDashboard?.Id });
        var myWidget: WidgetPM = new WidgetPM(this.SelectedDashboard);
        myWidget.Tenant = SessionInfo.LoggedUserTenant;
        var position = this.EvaluateNewWidgetPosition();
        myWidget.StartPotistion = `${position.x},${position.y}`;
        myWidget.EndPosition = `${position.w},${position.h}`;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: true, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.HasChanges = true;
                    this.AddWidgetToReactLayout(comp.EntityPM);
                }
            });
        });
    }
    onReactChangeLayouts(layouts: { lg: ReactWidgetPM[]; }) {


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
                MixPanelLocator.PostDashboardAction({ ActionName: "Widget Remove Click",Message :  "Widget Id : " + item?.Id, DashboardId: this.SelectedDashboard?.Id });
                this.SelectedDashboard.RemoveWidget(item);
            });
        }
     }

    openEditWidget(widget: ReactWidgetPM){
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
                    this.DashboardDataBinding.onEditWidget.next(DashboardMapping.GetReactWidget(comp.EntityPM));
                    this.HasChanges = true;
                }
            });
        });
    }
    AddWidgetToReactLayout(myWidget:WidgetPM){
        var reactWidget = DashboardMapping.GetReactWidget(myWidget);
        this.reactWidgetsLayout.lg.push(reactWidget);
        this.DashboardDataBinding.onAddWidget.next(reactWidget);
    }
    
    EvaluateNewWidgetPosition(){
        let widgetYPosition = 0;
        let widgetXPosition = 0;
        let allLayouts = this.reactWidgetsLayout.lg.map(e => e.Layout);
        for (let y = 0; true; y++) {
          widgetYPosition = y;
          for (let x = 0; x < 10; x++) {
            widgetXPosition = x;
    
            var exisitWidgetInNewPosition = allLayouts.find(e => {
              return e
                && widgetXPosition < e.x + e.w && widgetXPosition + this.newWidgetWidth > e.x
                && widgetYPosition < e.y + e.h && widgetYPosition + this.newWidgetHeight > e.y
            });
            if (!exisitWidgetInNewPosition)
              return { x: widgetXPosition, y: widgetYPosition, w: this.newWidgetWidth, h: this.newWidgetHeight };
          }
    
        }
    
      }

    private isBackButtonClicked: boolean = false;
    BackButtonClicked() {
        this.isBackButtonClicked = true;
        MixPanelLocator.PostDashboardAction({ ActionName: "Back Button Click", DashboardId: this.SelectedDashboard?.Id });
        if (this.HasChanges) {
            this.ConfirmSave();
        }
        else {
            this.GoBack(true);
        }
    }
    private ConfirmSave() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm";
        confirmWindow.Show("This Dashboard has unsaved changes do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SaveDashboard();
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window Yes Click", DashboardId: this.SelectedDashboard?.Id });
            }

            else if (confirmWindow.No) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window No Click", DashboardId: this.SelectedDashboard?.Id });
                this.GoBack(true);
            }
        });
    }
    private GoBack(reject: boolean) {
        this.ResetFlags();

        if (reject) {
            this.RejectChanges();
        }
    }

    RefreshLayoutClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Refresh Click", DashboardId: this.SelectedDashboard?.Id });
        for (const item of this.reactWidgetsLayout.lg) {
            this.DashboardDataBinding.onEditWidget.next(item);
        }
    }
    
    SaveDashboard(fromUI : boolean = false) {
        //this.CheckDeletedWidgets(dashboard);
        if(fromUI) MixPanelLocator.PostDashboardAction({ ActionName: "Submit Dashboard Save Click", DashboardId: this.SelectedDashboard?.Id });

        this.CurrentSession.StartBusyIndicatorSaving();
        this.dashboardPMService.update(this.SelectedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                this.SelectedDashboardId = this.SelectedDashboard?.Id;
                this.ResetFlags();

                if (this.isBackButtonClicked) {
                    this.GoBack(false);
                    this.isBackButtonClicked = false;
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.SelectedDashboard);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('PermissionLevelCode');

        this.myCloner.AddEntity(this.SelectedDashboard);
    }
    private RejectChanges() {
        this.SelectedDashboard.Widgets = this.CloneDashboardLayout;
        this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
        this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone(this.reactWidgetsLayout));
    }
    
}



