import {Component, ViewChild, ViewContainerRef, ComponentRef, Output, EventEmitter, ViewChildren, QueryList, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {LocationDirective} from '../../Utilities/LocationDirective';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EditComponent} from '../EditComponent/EditComponent';
import {ListComponent} from '../ListComponent/ListComponent';
import {SessionTabItem} from '../HomeComponent/HomeComponent';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {LogitudeGridHelper} from '../../Utilities/LogitudeGridHelper';
import {PubSubFiltersChangeEventService} from '../../Utilities/events/ApiFiltersChangeEvent'; 
import {MainMenuComponent} from '../MainMenuComponent/MainMenuComponent'; 
import {AmitalGatewayUtil} from '../../Utilities/AmitalGatewayUtil';
import { Subscription, TeardownLogic } from 'rxjs/Subscription';//itzik
import {EntityResourceService} from '../../Services/EntityResourceService';

@Component({
    selector: 'SessionComponent',
    moduleId: module.id,
    templateUrl: "./SessionComponent.html",
    providers: [PubSubFiltersChangeEventService],
})

export class SessionComponent {
    public SessionIndex: number;
    public SessionTabItem: SessionTabItem; 
    public Sessionkey: string;    
    public Imgs: any[];
    public LogitudeGridHelper: LogitudeGridHelper;
    public CopiedCell: any;
    public ComponentRef: ComponentRef<SessionComponent>
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @Output() SessionEvent: EventEmitter<any> = new EventEmitter();
    @Output() SessionInitialize: EventEmitter<any> = new EventEmitter();
    public MainMenuComponent: MainMenuComponent;
    entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(private temp: PubSubFiltersChangeEventService, public ChangeDetectorRef: ChangeDetectorRef) {
        this.PubSubFiltersChangeEventService = temp;
        this.SessionWindowIndex = null;
        this.CurrentWindow = null;
        this.Windows = new Array<LogitudeWindow>();
        this.EditControls = new Array<EditComponent>();
        this.ListControls = new Array<ListComponent>();
        this.IdCounters = new Array<SessionIdCounter>();
        this.MenuReferences = new Array<ComponentRef<any>>();

        window.onresize = this.onWindowResized.bind(this);
        //window.onmouseup = this.onMouseUp.bind(this);
        window.onmousedown = this.onMouseDown.bind(this); 
    }

    private iSessionLocation: LocationDirective;
    public get SessionLocation() { return this.iSessionLocation; }
    public set SessionLocation(value: LocationDirective) {
        if (this.iSessionLocation != value) {
            if (value) {
                this.iSessionLocation = value;
            }

            else if (this.isDestroingSession) {
                this.iSessionLocation = value;
            }
        }
    }

    private iSessionMenuLocation: LocationDirective;
    public get SessionMenuLocation() { return this.iSessionMenuLocation; }
    public set SessionMenuLocation(value: LocationDirective) {
        if (this.iSessionMenuLocation != value) {
            if (value) {
                this.iSessionMenuLocation = value;
            }

            else if (this.isDestroingSession) {
                this.iSessionMenuLocation = value;
            }
        }
    }

    OnSessionMouseUp($event) {
        this.MouseUpEvent.emit(event);
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;

                let locs = this.AllLocations.toArray().filter(f => f.Code == 'SessionContainer');
                let myLocation: LocationDirective = locs[0];
                this.SessionLocation = myLocation;

                if (this.LogitudeGridHelper == null) {
                    this.LogitudeGridHelper = new LogitudeGridHelper(this.SessionIndex);
                }

                this.SessionInitialize.emit(true);
                
                if (!SessionLocator.IsNewSignupTenant) {
                    this.entityResourceService.getEntityResourceByTableName("General", 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load("./Infrastructure/Components/MainMenu/MainMenuComponent", this.SessionLocation.viewContainerRef).then(cmpRef => {
                        this.MainMenuComponent = cmpRef.instance;
                        cmpRef.instance.RunComponent();
                        });
                    });
                }
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private IdCounters: SessionIdCounter[] = [];
    GetNewId(Name: string) {
        if (this.IdCounters == null) {
            this.IdCounters = new Array<SessionIdCounter>();
        }

        var idCounter = this.IdCounters.filter(f => f.Name == Name)[0];
        if (idCounter) {
            idCounter.Counter = idCounter.Counter + 1;
        }

        else {
            idCounter = new SessionIdCounter(Name);
            this.IdCounters.push(idCounter);
        }

        return this.SessionIndex + "_" + idCounter.Counter;
    }

    GetNewCounter(Name: string) {
        if (this.IdCounters == null) {
            this.IdCounters = new Array<SessionIdCounter>();
        }

        var idCounter = this.IdCounters.filter(f => f.Name == Name)[0];
        if (idCounter) {
            idCounter.Counter = idCounter.Counter + 1;
        }

        else {
            idCounter = new SessionIdCounter(Name);
            this.IdCounters.push(idCounter);
        }

        return idCounter.Counter;
    }





    @Output() EndOfRowReachedEvent: EventEmitter<any> = new EventEmitter();
    @Output() PseventRowSelectEvent: EventEmitter<any> = new EventEmitter();
    @Output() WindowResizeEvent: EventEmitter<any> = new EventEmitter();
    @Output() MouseUpEvent: EventEmitter<any> = new EventEmitter();
    @Output() ObsNewElementInsertedEvent: EventEmitter<any> = new EventEmitter();
    @Output() QuantityTypeCodeLoadedEvent: EventEmitter<any> = new EventEmitter();
    @Output() CollateralAnswerRefreshEvent: EventEmitter<any> = new EventEmitter();
    @Output() ConnectedItemSelectedEvent: EventEmitter<any> = new EventEmitter();
    @Output() SelectItemEvent: EventEmitter<any> = new EventEmitter();
    @Output() CloseNotificationBellEvent: EventEmitter<any> = new EventEmitter();
    @Output() AccumulatedFilterChangedEvent: EventEmitter<any> = new EventEmitter();
    @Output() MouseDownEvent: EventEmitter<any> = new EventEmitter();
    @Output() SearchFilterChangedEvent: EventEmitter<any> = new EventEmitter();

    @Output() DisableFieldsEvent: EventEmitter<any> = new EventEmitter();
    @Output() CopyCellIntoMemory: EventEmitter<any> = new EventEmitter();

    //@Output() SelectInvoiceItemEvent: EventEmitter<any> = new EventEmitter();
    public PubSubFiltersChangeEventService: PubSubFiltersChangeEventService;
    private ChartId: number = null;
    public IsOpenDatabaseBackupWindowFromSetting: boolean = false;
    public IsOpenChangePasswordWindowFromSetting: boolean = false;
    public IsOpenSignatureWindowFromSetting: boolean = false;
    public IsOpenDocumentBackupWindowFromSetting: boolean = false;


    public isShiftClicked: boolean = false;
    public isTabWithShiftClicked: boolean = false;
    public AllowShiftTab: boolean = false;

    @Output() LostFocusEvent: EventEmitter<any> = new EventEmitter();
    public IsShowErrorWindow: boolean = false;
    
    private _Subscription: Subscription = new Subscription();//itzik///https://stackoverflow.com/a/42274637
    public SubscriptionAdd(teardown: TeardownLogic) {
        //    this.someService.change.subscribe(() => {
        //[...]
        //    })

        this._Subscription.add(teardown);
    }
    UnsubscribeStaticEvent() {
        this._Subscription.unsubscribe();//itzik
    }

    private onWindowResized(event: UIEvent): void {
        this.WindowResizeEvent.emit(event);
    }   
    private onMouseDown(event: UIEvent): void {
        this.MouseDownEvent.emit(event);
    }
  
    private onMouseUp(event: UIEvent): void {
        this.MouseUpEvent.emit(event);
    }
    public GetChartId() {

        if (this.ChartId == null) {
            this.ChartId = 0;
        }

        else {
            this.ChartId += 1;
        }
        return this.SessionIndex + "_" + this.ChartId;
    }

    private BusyIndicatorTimer: any;
    public BusyIndicatorText: string = null;

    private showBusyIndicator: boolean = false;
    get ShowBusyIndicator() { return this.showBusyIndicator; }
    set ShowBusyIndicator(newValue: boolean) {
        if (this.showBusyIndicator != newValue) {
            this.showBusyIndicator = newValue;
        }
    }

    //public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        if (this.CurrentWindow != null && !this.CurrentWindow.SuppressBusyIndicator) {
            this.CurrentWindow.StartBusyIndicator(myText);
        }

        else if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.StartBusyIndicator(myText);
        }

        else {
            this.BusyIndicatorText = myText;
            this.ShowBusyIndicator = true;
        }

        if (this.BusyIndicatorTimer) {
            clearTimeout(this.BusyIndicatorTimer);
        }

        this.BusyIndicatorTimer = setTimeout(() => this.CheckBusyIndicator(), 5000);
    }
    CheckBusyIndicator() {
        if (SessionLocator.CurrentSession.SessionIndex != this.SessionIndex) {
            if (this.ShowBusyIndicator) {
                this.StopBusyIndicator();
            }
        }
    }

    public StartBusyIndicatorSaving() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
    }
    public StartBusyIndicatorLoading() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
    }
    public StartBusyIndicatorCreating() {
        this.StartBusyIndicator("Creating...");
    }
    public StopBusyIndicator() {
        if (this.CurrentWindow != null) {
            this.CurrentWindow.StopBusyIndicator();
        }

        if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.StopBusyIndicator();
        }

        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    // MenuReferences
    private MenuReferences: Array<ComponentRef<any>>;
    public AddMenuReference(element: ComponentRef<any>) {
        if (this.MenuReferences == null) {
            this.MenuReferences = new Array<ComponentRef<any>>();
        }

        this.MenuReferences.push(element);
    }
    public DestroyMenuReferences() {
        if (this.MenuReferences == null) {
            this.MenuReferences = new Array<ComponentRef<any>>();
        }

        this.MenuReferences.forEach((item) => {
            item.destroy();
        });

        this.MenuReferences = [];
    }
    
    // Windows
    public Windows: Array<LogitudeWindow>;
    public CurrentWindow: LogitudeWindow = null;
    public SessionWindowIndex: number = null;
    public GetNewWindowIndex() {
        if (this.SessionWindowIndex == null) {
            this.SessionWindowIndex = 0;
        }

        else {
            this.SessionWindowIndex += 1;
        }

        return this.SessionWindowIndex;
    }
    public AddWindow(element: LogitudeWindow) {
        if (this.Windows == null) {
            this.Windows = new Array<LogitudeWindow>();
        }

        this.Windows.push(element);
        this.CurrentWindow = element;
    }
    public RemoveWindow(element: LogitudeWindow) {
        var newCurrentWindow: LogitudeWindow = null;
        if (this.Windows != null) {

            var index = this.Windows.indexOf(element);
            if (index > -1) {
                this.Windows.splice(index, 1);
            }

            var biggestIndex = -1;
            this.Windows.forEach((item) => {
                if (item.WindowIndex > biggestIndex) {
                    biggestIndex = item.WindowIndex;
                }
            });

            if (biggestIndex > -1) {
                newCurrentWindow = this.Windows.filter(f => f.WindowIndex == biggestIndex)[0];
            }
        }

        this.CurrentWindow = newCurrentWindow;
    }
    public CloseCurrentWindow() {
        if (this.CurrentWindow != null) {
            this.CurrentWindow.Close(null);
        }
    }
    public CloseCurrentWindowEmit(emit: string) {
        if (this.CurrentWindow != null) {
            this.CurrentWindow.Close(emit);
        }
    }
    public DestroyWindows() {
        if (this.Windows == null) {
            this.Windows = new Array<LogitudeWindow>();
        }

        this.Windows.forEach((item) => {
            item.DestroyWindow();
        });

        this.Windows = [];
    }

    // EditControls
    private EditControls: Array<EditComponent>;
    public CurrentEditComponent: EditComponent = null;
    public SessionEditComponentIndex: number = null;
    public GetNewEditComponentIndex() {
        if (this.SessionEditComponentIndex == null) {
            this.SessionEditComponentIndex = 0;
        }

        else {
            this.SessionEditComponentIndex += 1;
        }

        return this.SessionEditComponentIndex;
    }
    public AddEditComponent(element: EditComponent) {
        if (this.EditControls == null) {
            this.EditControls = new Array<EditComponent>();
        }

        this.EditControls.push(element);
        this.CurrentEditComponent = element;
    }
    public RemoveEditComponent(element: EditComponent) {
        var newCurrentEditComponent: EditComponent = null;
        if (this.EditControls != null) {

            var index = this.EditControls.indexOf(element);
            if (index > -1) {
                this.EditControls.splice(index, 1);
            }

            var biggestIndex = -1;
            this.EditControls.forEach((item) => {
                if (item.ComponentIndex > biggestIndex) {
                    biggestIndex = item.ComponentIndex;
                }
            });

            if (biggestIndex > -1) {
                newCurrentEditComponent = this.EditControls.filter(f => f.ComponentIndex == biggestIndex)[0];
            }
        }

        this.CurrentEditComponent = newCurrentEditComponent;
    }
    public RealCloseCurrentEditComponent() {
        if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.Close();
        }
    }
    public CloseCurrentEditComponent() {
        if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.DestroyEditControl();
        }
    }
    public DestroyEditControls() {
        if (this.EditControls == null) {
            this.EditControls = new Array<EditComponent>();
        }

        this.EditControls.forEach((item) => {
            item.DestroyEditControl();
        });

        this.EditControls = [];
    }

    public CurrentLogGrid: string = "";

    // ListControls
    private ListControls: Array<ListComponent>;
    public CurrentListComponent: ListComponent = null;
    public SessionListComponentIndex: number = null;
    public GetNewListComponentIndex() {
        if (this.SessionListComponentIndex == null) {
            this.SessionListComponentIndex = 0;
        }

        else {
            this.SessionListComponentIndex += 1;
        }

        return this.SessionListComponentIndex;
    }
    public AddListComponent(element: ListComponent) {
        if (this.ListControls == null) {
            this.ListControls = new Array<ListComponent>();
        }

        this.ListControls.push(element);
        this.CurrentListComponent = element;
    }
    public DestroyListComponentReferences() {// itzik + ihab + mohammad !!!!

        SessionLocator.CurrentSession.UnsubscribeStaticEvent();
        this.ListControls = null;
        this.ListControls = new Array<ListComponent>();
        this.CurrentListComponent = null;;

        
    }
    public RemoveListComponent(element: ListComponent) {
        var newCurrentListComponent: ListComponent = null;
        if (this.ListControls != null) {

            var index = this.ListControls.indexOf(element);
            if (index > -1) {
                this.ListControls.splice(index, 1);
            }

            var biggestIndex = -1;
            this.ListControls.forEach((item) => {
                if (item.ComponentIndex > biggestIndex) {
                    biggestIndex = item.ComponentIndex;
                }
            });

            if (biggestIndex > -1) {
                newCurrentListComponent = this.ListControls.filter(f => f.ComponentIndex == biggestIndex)[0];
            }
        }

        this.CurrentListComponent = newCurrentListComponent;
    }
    public CloseCurrentListComponent() {
        if (this.CurrentListComponent != null) {
            this.CurrentListComponent.DestroyListControl();
        }
    }
    public DestroyListControls() {
        if (this.ListControls == null) {
            this.ListControls = new Array<ListComponent>();
        }

        this.ListControls.forEach((item) => {
            item.DestroyListControl();
        });

        this.ListControls = [];
    }

    //public DestroyS

    private isDestroingSession: boolean = false;
    public DestroySession() {
        this.isDestroingSession = true;

        this.DestroyWindows();
        this.DestroyEditControls();
        this.DestroyListControls();
        this.DestroyMenuReferences();

        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
        }

        this.SessionLocation = null;
        this.SessionMenuLocation = null;

        if (this.BusyIndicatorTimer) {
            clearTimeout(this.BusyIndicatorTimer);
        }
        //window.removeEventListener("keydown", (evt) => this.keydownevt(evt, this.SessionIndex));
    }
    public FireEvent(eventArgs: any) {
        this.SessionEvent.emit(eventArgs);
    }
    public ChangeSessionHeader(args: any) {
        this.SessionTabItem.ChangeSessionHeader(args);
    }

    StopChangeDetection() {
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && this.CurrentListComponent) {
            this.CurrentListComponent.DestroyMe = true;
            //setTimeout(() => { this.ChangeDetectorRef.detach(); }, 100);// cause malfunction !!!!!!!!!!!!!
        } else {
            this.ChangeDetectorRef.detach();
        }
        
    }
    StartChangeDetection() {
        this.ChangeDetectorRef.reattach();
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && this.CurrentListComponent) {
            this.CurrentListComponent.DestroyMe = false;
        }
    }
}
export class SessionIdCounter {
    public Name: string;
    public Counter: number;
    constructor(name: string) {
        this.Name = name;
        this.Counter = 0;
    }
}
