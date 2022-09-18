import { Component, AfterViewInit, OnInit,Input } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BehaviorSubject } from 'rxjs';
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

@Component({
    templateUrl:'CustomDashboardComponent.html',
    styleUrls:['CustomDashboardComponent.css'],
    selector:'custom-dashboard',
})

export class CustomDashboardComponent extends BaseComponent implements OnInit, AfterViewInit {   
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;    
    private dashboardPMService: DashboardPMService;
    private dashboardPMExtendedService: DashboardPMExtendedService;
    private SelectedDashboard: DashboardPM;
    public SelectedDashboardName: string;
    public DataContext = this;
    constructor() {
        super();
        this.dashboardPMService = new DashboardPMService();
        this.dashboardPMExtendedService = new DashboardPMExtendedService();
        this.SelectedDashboard = new DashboardPM();
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

    @Input('Show') Show;

    private GetDefaultDashboard() {
        this.dashboardPMExtendedService.GetDefaultDashboardId().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboardId = myResponse.Result;
                this.GetSingleDashboardWithWidgets(this.SelectedDashboardId);
            }
        });
    }

    GetSingleDashboardWithWidgets(dashboardId: string) {
        this.dashboardPMService.get(dashboardId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                if (this.SelectedDashboard) {
                    this.SelectedDashboardName = this.SelectedDashboard.Name;

                    //var myReactDashboard: ReactDashboardPM = this.GetReactDashboard(this.myDashboardPM)
                    //this.dashboardDataBinding.onGetDashboard.next(myReactDashboard);
                    //this.selectedDashboard = myReactDashboard;

                    this.Clone();
                }
            }           
        });        
    }
    
    //private OpenDashboardWidgetWindow(widget: ReactWidgetPM) {
    //    var myWidget: WidgetPM = this.myDashboardPM.Widgets.filter(d => d.Id == widget.Id)[0];
    //    if (myWidget == null) {
    //        myWidget = new WidgetPM(this.myDashboardPM);
    //        myWidget.TypeCode = widget.TypeCode;
    //        myWidget.Tenant = SessionInfo.LoggedUserTenant;
    //        myWidget.StartPotistion = widget.StartPotistion;
    //        myWidget.EndPosition = widget.EndPosition;
    //        myWidget.DateGroupCode = widget.DateGroupCode;
    //        myWidget.SortDirection = widget.SortDirection;
    //        myWidget.SortBy = widget.SortBy;
    //        myWidget.MaximumGrouping = widget.MaximumGrouping;
    //    }

    //    var logitudeWindow = new LogitudeWindow();
    //    logitudeWindow.Title = !AppTool.IsNullOrEmpty(widget.Id) ? "Edit Widget" : "Add Widget";
    //    logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: widget.ChangeSetOp == "Insert", DashboardPM: this.myDashboardPM };
    //    logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
    //    logitudeWindow.ComponentLoaded.subscribe(comp => {
    //        logitudeWindow.WindowClosed.subscribe(s => {
    //            if (s) {
    //                this.selectedDashboard = this.GetReactDashboard(comp.DashboardPM);
    //                this.myDashboardPM = comp.DashboardPM;
    //                this.dashboardDataBinding.onGetDashboard.next(this.selectedDashboard);
    //                this.dashboardDataBinding.onAddUpdateWidget.next(true);
    //            }
    //        });
    //    });
    //}

    //GetReactDashboard(dashboard: DashboardPM): ReactDashboardPM {
    //    var myDashboard: ReactDashboardPM = {} as ReactDashboardPM;

    //    if (dashboard) {
    //        myDashboard.Id = dashboard.Id;
    //        myDashboard.Tenant = dashboard.Tenant;
    //        myDashboard.Name = dashboard.Name;
    //        myDashboard.Description = dashboard.Description;
    //        myDashboard.CreateDate = dashboard.CreateDate;
    //        myDashboard.CreatedByUserId = dashboard.CreatedByUserId;
    //        myDashboard.UpdateDate = dashboard.UpdateDate;
    //        myDashboard.UpdatedByUserId = dashboard.UpdatedByUserId;
    //        myDashboard.PermissionLevelCode = dashboard.PermissionLevelCode;
    //        myDashboard.Widgets = [];
    //        myDashboard.DashboardSharedUsers = [];

    //        dashboard.Widgets.forEach(item => {
    //            myDashboard.Widgets.push(this.GetReactWidget(item));
    //        });

    //        dashboard.DashboardSharedUsers.forEach(item => {
    //            myDashboard.DashboardSharedUsers.push(this.GetReactSharedUser(item));
    //        });
    //    }

    //    return myDashboard;
    //}    
    //GetReactSharedUser(user: DashboardSharedUserPM): ReactDashboardSharedUserPM {
    //    var myUser: ReactDashboardSharedUserPM = {} as ReactDashboardSharedUserPM;

    //    if (user) {
    //        myUser.Id = user.Id;
    //        myUser.Tenant = user.Tenant;
    //        myUser.UserId = user.UserId;
    //        myUser.UserName = user.UserName;
    //        myUser.DashboardId = user.DashboardId;
    //    }

    //    return myUser;
    //}  

    //private CheckDeletedWidgets(dashboard: ReactDashboardPM) {
    //    var deletedWidgets: WidgetPM[] = [];

    //    this.myDashboardPM.Widgets.forEach(item => {
    //        if (dashboard.Widgets.filter(d => d.Id == item.Id)[0] == null) {
    //            deletedWidgets.push(item);
    //        }
    //    });

    //    deletedWidgets.forEach(item => {
    //        this.myDashboardPM.RemoveWidget(item);
    //    });
    //}
    //private MapWidgetsPositions(dashboard: ReactDashboardPM) {
    //    dashboard.Widgets.forEach(item => {
    //        var myWidgetPm: WidgetPM = this.myDashboardPM.Widgets.filter(d => d.Id == item.Id)[0];
    //        if (myWidgetPm) {
    //            myWidgetPm.EndPosition = item.EndPosition;
    //            myWidgetPm.StartPotistion = item.StartPotistion;
    //        }
    //    });
    //}

    private selectedDashboardId: string;
    get SelectedDashboardId() { return this.selectedDashboardId; }
    set SelectedDashboardId(value: string) {
        if (this.selectedDashboardId != value) {
            this.selectedDashboardId = value;

            this.GetSingleDashboardWithWidgets(value);
        }
    }

    public BackButtonLable: string = "Back";
    public IsEditLayoutButtonVisible: boolean = true;
    public IsEditDashboardButtonVisible: boolean = false;
    public IsEditLayoutModeActive: boolean = false;
    public HasChanges: boolean = false;
    private ResetFlags() {
        this.IsEditLayoutButtonVisible = true;
        this.IsEditDashboardButtonVisible = false;
        this.IsEditLayoutModeActive = false;
    }

    EditLayoutClicked() {
        this.IsEditLayoutButtonVisible = false;
        this.IsEditDashboardButtonVisible = true;
        this.IsEditLayoutModeActive = true;

    }

    AdDashboardClicked() {
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

                }
            });
        });
    }

    EditDashboardClicked() {
        //this.IsEditLayoutButtonVisible = true;
        this.IsEditDashboardButtonVisible = false;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Dashboard";

        logitudeWindow.WindowArgs = { EntityPM: this.SelectedDashboard, };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    
                }
            });
        });
    }

    AddWidgetClicked() {
        var myWidget: WidgetPM = new WidgetPM(this.SelectedDashboard);
        myWidget.Tenant = SessionInfo.LoggedUserTenant;
        // myWidget.StartPotistion = widget.StartPotistion;
        // myWidget.EndPosition = widget.EndPosition;        

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: true, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    //this.selectedDashboard = this.GetReactDashboard(comp.DashboardPM);
                    //this.myDashboardPM = comp.DashboardPM;
                    //this.dashboardDataBinding.onGetDashboard.next(this.selectedDashboard);
                    //this.dashboardDataBinding.onAddUpdateWidget.next(true);
                }
            });
        });
    }

    private isBackButtonClicked: boolean = false;
    BackButtonClicked() {
        this.isBackButtonClicked = true;

        if (this.HasChanges) {
            this.ConfirmSave();
        }
        else {
            this.GoBack();
        }
    }
    private ConfirmSave() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm";
        confirmWindow.Show("");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SaveDashboard();
            }

            else if (confirmWindow.No) {
                
            }
        });
    }
    private GoBack() {
        this.ResetFlags();
        this.RejectChanges();
    }

    RefreshLayoutClicked() {

    }
    
    SaveDashboard() {
        //this.CheckDeletedWidgets(dashboard);
        //this.MapWidgetsPositions(dashboard);

        this.dashboardPMService.update(this.SelectedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SelectedDashboard = myResponse.Result;
                //this.selectedDashboard = this.GetReactDashboard(this.myDashboardPM);
                //this.dashboardDataBinding.onGetDashboard.next(this.selectedDashboard);
                //this.dashboardDataBinding.onAddUpdateWidget.next(true);

                if (this.isBackButtonClicked) {
                    this.GoBack();
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
        this.myCloner.RejectChanges();
    }
}



