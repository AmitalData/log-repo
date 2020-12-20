import {Component, AfterViewInit, ViewChild, ViewContainerRef, Output, EventEmitter, HostListener} from '@angular/core';
import { ChildDirective } from '../Directives/ChildDirective';
import { Settings } from '../../Infrastructure/Settings';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
declare var dragger: any;

export class LogitudeWindow {
    public Width: number = 750;
    public Height: number = 500;
    //public Title: string = null;
    public TitleIcon: string = null;
    public CustomTitleIcon: string = null;
    public WindowIndex: number = null;
    public IsOverWindow: boolean = false;
    public IsOverEditComponentWindow: boolean = false;
    public IsSameWindowSize: boolean = false;
    public IsShowCloseButton: boolean = false;
    public IsFillScreen: boolean = false;
    public IsFillScreenHeight: boolean = false;
    public WindowArgs: any;
    public NewWizardArgs: any;
    public DataContext: any;
    public WindowId: string = null;
    public WindowContainerId: string = null;
    public NotifyOnClose: boolean = false;
    public ShowCloseButton: boolean = false;
    public ShowHeaderButtons: boolean = false;
    public IsHideHeader: boolean = false;
    public IsEditComponent: boolean = false;
    public IsFullScreen: boolean = false;
    public IsOverAll: boolean = false;
    public IsShowAutomationDelayTitle: boolean = false;
    public ShowHelpIcon: boolean = false;
    public HelpText: string = null;
    public RTL: boolean = false;
    public BottomBorderForTitle: string = "none";
    public IsFillScreen_115: boolean = false;
    LayoutDirection: string = 'ltr';
    public ZIndex: number = 0;
    public IsFillScreen_90: boolean = false;
    public SuppressBusyIndicator: boolean = false;
    public IsHideWindowMargin: boolean = false;

    @Output() WindowClosed: EventEmitter<any> = new EventEmitter();
    @Output() ComponentLoaded: EventEmitter<any> = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LayoutDirection = Settings.LayoutDirection;
        if (this.LayoutDirection == 'rtl') {
            this.RTL = true;
        }
    }

    private ComponentRef: any = null;
    private InstanceComponent: LogitudeWindowTemplateComponent = null;
    public Show(myContent: any) {
        if (myContent != null) {
            var viewContainerRefLocation: ViewContainerRef = this.CurrentSession.SessionLocation.viewContainerRef;
            if (this.CurrentSession.CurrentEditComponent) {// itzik : due crush !!!- abdulllah add this lines ...
                if (this.CurrentSession.CurrentEditComponent.IsSplitBtnVisible) {
                    viewContainerRefLocation = this.CurrentSession.CurrentEditComponent.WindowLocationViewContainerRef;
                }
            }

            if (this.IsOverAll) {
                viewContainerRefLocation = SessionLocator.ApplicationLocation;
            }

            else if (this.CurrentSession.CurrentWindow) {
                if (this.CurrentSession.CurrentWindow.IsOverAll) {
                    this.IsOverAll = true;
                    viewContainerRefLocation = SessionLocator.ApplicationLocation;
                }
            }

            SessionLocator.DynamicLoader.Load("./Controls/Windows/LogitudeWindowTemplateComponent", viewContainerRefLocation)
                .then(cmpRef => {

                    this.ComponentRef = cmpRef;
                    this.InstanceComponent = cmpRef.instance;

                    this.WindowIndex = this.CurrentSession.GetNewWindowIndex();


                    if (this.CurrentSession.CurrentWindow != null) {

                        if (this.CurrentSession.CurrentWindow.IsEditComponent == true) {
                            this.IsOverEditComponentWindow = true;
                        }

                        else {
                            this.IsOverWindow = true;
                        }

                        if (this.CurrentSession.CurrentWindow.Width == this.Width && this.CurrentSession.CurrentWindow.Height == this.Height) {
                            this.IsSameWindowSize = true;
                        }
                    }

                    this.CurrentSession.AddWindow(this);
                    cmpRef.instance.InjectWindowComponent(myContent, this);
                });
        }
    }

    public ShowEditComponent(entityId: string, objectTableName: string, selectedTabCode: string = null, isFillScreen: boolean = true) {
        this.IsFillScreen = isFillScreen;
        this.IsEditComponent = true;

        var viewContainerRefLocation: ViewContainerRef = this.CurrentSession.SessionLocation.viewContainerRef;
        if (this.IsOverAll) {
            viewContainerRefLocation = SessionLocator.ApplicationLocation;
        }

        else if (this.CurrentSession.CurrentWindow) {
            if (this.CurrentSession.CurrentWindow.IsOverAll) {
                this.IsOverAll = true;
            }
        }

        SessionLocator.DynamicLoader.Load("./Controls/Windows/LogitudeWindowTemplateComponent", viewContainerRefLocation)
            .then(cmpRef => {

                this.ComponentRef = cmpRef;
                this.InstanceComponent = cmpRef.instance;

                this.WindowIndex = this.CurrentSession.GetNewWindowIndex();

                if (this.CurrentSession.CurrentWindow != null) {

                    if (this.CurrentSession.CurrentWindow.IsEditComponent == true) {
                        this.IsOverEditComponentWindow = true;
                    }

                    else {
                        this.IsOverWindow = true;
                    }

                    if (this.CurrentSession.CurrentWindow.Width == this.Width && this.CurrentSession.CurrentWindow.Height == this.Height) {
                        this.IsSameWindowSize = true;
                    }
                }

                this.CurrentSession.AddWindow(this);

                cmpRef.instance.InjectEditComponent(entityId, objectTableName, this, selectedTabCode);
            });
    }

    public Close(emit: string) {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }

        if (this.InstanceComponent != null) {
            this.InstanceComponent.Destroy();
        }

        this.CurrentSession.RemoveWindow(this);
        this.WindowClosed.emit(emit);
    }

    public Resize(width: number) {
        var currentWindowWidth = parseInt(this.InstanceComponent.Width.split('p')[0]);
        var currentWindowLeft = parseInt(this.InstanceComponent.Left.split('p')[0]);
        if (currentWindowWidth > width) {
            this.InstanceComponent.Left = (currentWindowLeft*2) + "px";
        }
        else if(currentWindowWidth < width){
            this.InstanceComponent.Left = (currentWindowLeft/2) + "px";
        }
        this.InstanceComponent.Width = width + "px";
    }

    public DestroyWindow() {
        //if (this.ComponentRef != null) {
        //    this.ComponentRef.destroy();
        //    this.ComponentRef = null;
        //}

        //if (this.InstanceComponent != null) {
        //    this.InstanceComponent.Dispose();
        //    this.InstanceComponent = null;
        //}

        //this.WindowArgs = null;
        //this.DataContext = null;
    }

    public StartBusyIndicator(myText: string) {
        if (this.InstanceComponent != null) {
            if (this.SuppressBusyIndicator) {
                ///
            } else {
                this.InstanceComponent.BusyIndicatorText = myText;
                this.InstanceComponent.ShowBusyIndicator = true;
            }
        }
    }

    public StopBusyIndicator() {
        if (this.InstanceComponent != null) {
            this.InstanceComponent.BusyIndicatorText = null;
            this.InstanceComponent.ShowBusyIndicator = false;
        }
    }

    public ShowCancelControl(isVisible: boolean) {
        if (this.InstanceComponent != null) {
            this.InstanceComponent.IsCancelControlVisible = isVisible;
        }
    }

    public ToShowCloseButton(isVisible: boolean) {
        if (this.InstanceComponent != null) {
            this.InstanceComponent.ShowCloseButton = isVisible;
        }
    }


    private title: string = null;
    get Title() { return this.title; }
    set Title(newValue: string) {
        this.title = newValue;
        if (this.InstanceComponent) {
            this.InstanceComponent.Title = newValue;
        }
    }


}

@Component({    
    templateUrl: "./LogitudeWindow.html",
})

export class LogitudeWindowTemplateComponent implements AfterViewInit {
    public Top: string = "0";
    public Left: string = "0";
    public Width: string = "750px";
    public Height: string = "500px";
    public Title: string = null;
    public TitleIcon: string = null;
    public CustomTitleIcon: string = null;
    public ShowModal: boolean = true;
    public WindowId: string = null;
    public WindowContainerId: string = null;
    public FocusElementId: string = null;
    public IsShowCloseButton: boolean = false;
    public IsHideHeader: boolean = false;
    public IsCancelControlVisible: boolean = false;
    public WindowArgs: any;
    public NewWizardArgs: any;
    public DataContext: any;
    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public ShowCloseButton: boolean = false;
    public ShowHeaderButtons: boolean = false;
    public IsWindowMaximize: boolean = false;
    public NotifyOnClose: boolean = false;
    public IsFullScreen: boolean = false;
    public IsShowAutomationDelayTitle: boolean = false;
    public ShowHelpIcon: boolean = false;
    public HelpText: string = null;
    public RTL: boolean = false;
    public BottomBorderForTitle: string = "none";
    public IsHideWindowMargin: boolean = false;
    LayoutDirection: string = 'ltr';
    public ZIndex: number = 0;
    leftPadding: number = 0;
    public IsOverAll: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChild(ChildDirective) Child: ChildDirective;
    constructor() {
        this.LayoutDirection = Settings.LayoutDirection;

        if (this.LayoutDirection == 'rtl') {
            this.RTL = true;
        }
    }

    private isChildInjected: boolean = false;
    private ChildComponentPath: string = null;
    private isAfterViewInited: boolean = false;
    ngAfterViewInit() {
        this.isAfterViewInited = true;
        this.FocusWindow();
        this.LoadChildComponent();
    }

    private logWindow: LogitudeWindow;
    public InjectWindowComponent(myComponentPath: string, logWindow: LogitudeWindow) {
        this.logWindow = logWindow;
        this.IsEditComponent = false;
        this.CreateDynamicIds();

        this.Title = logWindow.Title;
        this.TitleIcon = logWindow.TitleIcon;
        this.WindowArgs = logWindow.WindowArgs;
        this.NewWizardArgs = logWindow.NewWizardArgs;
        this.DataContext = logWindow.DataContext;
        this.IsShowCloseButton = logWindow.IsShowCloseButton;
        this.ShowCloseButton = logWindow.ShowCloseButton;
        this.ShowHeaderButtons = logWindow.ShowHeaderButtons;
        this.NotifyOnClose = logWindow.NotifyOnClose;
        this.IsHideHeader = logWindow.IsHideHeader;
        this.IsFullScreen = logWindow.IsFullScreen;
        this.IsOverAll = logWindow.IsOverAll;
        this.IsShowAutomationDelayTitle = logWindow.IsShowAutomationDelayTitle;
        this.ShowHelpIcon = logWindow.ShowHelpIcon;
        this.ZIndex = logWindow.ZIndex;
        this.ChildComponentPath = myComponentPath;
        this.HelpText = logWindow.HelpText;
        this.RTL = logWindow.RTL;
        this.CustomTitleIcon = logWindow.CustomTitleIcon;
        this.BottomBorderForTitle = logWindow.BottomBorderForTitle;
        this.IsHideWindowMargin = logWindow.IsHideWindowMargin;
        this.SetWindowSize();
        this.isChildInjected = true;
        this.LoadChildComponent();
    }

    private EditComponentEntityId: string;
    private EditComponentTableName: string;
    private EditComponentTabCode: string;
    public IsEditComponent: boolean = false;
    public InjectEditComponent(entityId: string, objectTableName: string, logWindow: LogitudeWindow, selectedTabCode: string = null) {
        this.logWindow = logWindow;
        this.IsEditComponent = true;
        this.EditComponentEntityId = entityId;
        this.EditComponentTableName = objectTableName;
        this.EditComponentTabCode = selectedTabCode;
        this.ShowHeaderButtons = true;
        this.CreateDynamicIds();
        this.Title = logWindow.Title;
        this.TitleIcon = logWindow.TitleIcon;
        this.WindowArgs = logWindow.WindowArgs;
        this.NewWizardArgs = logWindow.NewWizardArgs;
        this.DataContext = logWindow.DataContext;
        this.IsShowCloseButton = logWindow.IsShowCloseButton;
        this.IsHideHeader = logWindow.IsHideHeader;
        this.IsFullScreen = logWindow.IsFullScreen;
        this.IsShowAutomationDelayTitle = logWindow.IsShowAutomationDelayTitle;
        this.ShowHelpIcon = logWindow.ShowHelpIcon;
        this.ZIndex = logWindow.ZIndex;
        this.NotifyOnClose = logWindow.NotifyOnClose;
        this.BottomBorderForTitle = logWindow.BottomBorderForTitle;
        this.IsHideWindowMargin = logWindow.IsHideWindowMargin;
        this.ChildComponentPath = "./Infrastructure/Components/EditComponent/EditComponent";
        this.HelpText = logWindow.HelpText;
        this.RTL = logWindow.RTL;
        this.CustomTitleIcon = logWindow.CustomTitleIcon;
        this.SetWindowSize();
        this.isChildInjected = true;
        this.LoadChildComponent();
    }

    private SetWindowSize() {
        var ApplicationSession = document.getElementById("ApplicationSession");
        if (ApplicationSession) {
            var SetOverProperty: boolean = false;

            var windowWidth: number = 100;
            var windowHeight: number = 100;
            var appWidth = ApplicationSession.clientWidth;
            var appHeight = ApplicationSession.clientHeight;

            if (this.logWindow.IsFullScreen) {
                windowWidth = appWidth;
                windowHeight = appHeight;
            }

            else if (this.logWindow.IsFillScreen) {
                windowWidth = appWidth - 50;
                windowHeight = appHeight - 50;
            }

            else if (this.logWindow.IsFillScreen_90) {
                windowWidth = appWidth - 90;
                windowHeight = appHeight - 90;
            }

            else if (this.logWindow.IsFillScreen_115) {
                windowWidth = appWidth - 115;
                windowHeight = appHeight - 115;
            }

            else if (this.logWindow.IsFillScreenHeight) {
                windowHeight = appHeight - 50;
                windowWidth = this.logWindow.Width;
            }


            else {
                SetOverProperty = true;

                if (this.logWindow.Width) {
                    windowWidth = this.logWindow.Width;
                }

                if (this.logWindow.Height) {
                    windowHeight = this.logWindow.Height;
                }
            }

            if (windowWidth >= appWidth) {
                windowWidth = appWidth - 20;
            }

            if (windowHeight >= appHeight) {
                windowHeight = appHeight - 20;
            }

            this.Width = windowWidth + "px";
            this.Height = windowHeight + "px";

            var topProperty: number = (appHeight - windowHeight) / 2;
            var leftProperty: number = (appWidth - windowWidth) / 2;


            //#region Abdullah: this code to paint the window over editcomponent section while split component is opened (customs)
            var windowPlaceholderWidth: number = null;
            var windowPlaceholderHeight: number = null;
            var isOverEditComponent: boolean = false;

            isOverEditComponent = this.CurrentSession.CurrentEditComponent != null && this.CurrentSession.CurrentEditComponent != undefined;

            // if (this.CurrentSession.CurrentWindow.IsOverWindow)
            //     isOverEditComponent = false;


            if (this.IsEditComponent && this.logWindow.IsOverEditComponentWindow) {
                // Task 64019
            }

            else if (this.IsEditComponent && this.logWindow.IsOverWindow && isOverEditComponent) {
            // Task 64019
            }

            //change window position according to editcomponent location
            else if (isOverEditComponent || (isOverEditComponent && this.logWindow.IsOverWindow)) {

                //get window location from edit component
                var editComponentCelId = this.CurrentSession.CurrentEditComponent.EditComponentCellId;
                var windowPlaceholderDiv = document.getElementById(editComponentCelId);
                if (windowPlaceholderDiv) {
                    windowPlaceholderWidth = windowPlaceholderDiv.clientWidth;
                    windowPlaceholderHeight = windowPlaceholderDiv.clientHeight;
                }

                //update top,left poisition
                topProperty = (windowPlaceholderHeight - windowHeight) / 2;
                leftProperty = (windowPlaceholderWidth - windowWidth) / 2;
            }

            //#endregion

            if (SetOverProperty) {
                if (this.logWindow.IsOverWindow) {
                    if (this.logWindow.IsSameWindowSize) {
                        if (!this.IsEditComponent) {
                            topProperty -= 10;
                            leftProperty += 10;
                        }
                    }
                }
            }

            this.Top = topProperty + "px";
            this.Left = leftProperty + "px";
        }
    }
    private CreateDynamicIds() {
        this.WindowId = "LogitudeWindow_" + this.CurrentSession.SessionIndex + "_" + this.CurrentSession.SessionWindowIndex;
        this.FocusElementId = "LogitudeWindowFocusElement_" + this.CurrentSession.SessionIndex + "_" + this.CurrentSession.SessionWindowIndex;
        this.WindowContainerId = "LogitudeWindowContainerElement_" + this.CurrentSession.SessionIndex + "_" + this.CurrentSession.SessionWindowIndex;
        this.logWindow.WindowId = this.WindowId;
        this.logWindow.WindowContainerId = this.WindowContainerId;
    }

    FocusWindow() {
        if (this.FocusElementId) {
            var element = document.getElementById(this.FocusElementId);
            if (element) {
                element.focus();
            }
        }
    }

    private ComponentRef: any = null;
    private ComponentInstance: any = null;
    private LoadChildComponent() {
        if (this.isChildInjected && this.isAfterViewInited) {
            if (this.ChildComponentPath != null) {

                if (this.IsEditComponent) {

                    SessionLocator.DynamicLoader.Load(this.ChildComponentPath, this.Child.Location)
                        .then(cmpRef => {
                            this.ComponentRef = cmpRef;
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.IsInsideWindow = true;
                            cmpRef.instance.ComponentBackground = "transparent";
                            cmpRef.instance.Run({ EntityId: this.EditComponentEntityId, ObjectTableName: this.EditComponentTableName, SelectedTabCode: this.EditComponentTabCode });
                            this.logWindow.ComponentLoaded.emit(this.ComponentRef.instance);
                        });
                }

                else {
                    SessionLocator.DynamicLoader.Load(this.ChildComponentPath, this.Child.Location)
                        .then(cmpRef => {
                            this.ComponentRef = cmpRef;
                            this.ComponentInstance = this.ComponentRef.instance;

                            if (this.WindowArgs != null) {
                                if (this.ComponentRef.instance['SetWindowArgs']) {
                                    this.ComponentRef.instance.SetWindowArgs(this.WindowArgs);
                                }
                            }

                            if (this.NewWizardArgs != null) {
                                if (this.ComponentRef.instance['SetNewWizardArgs']) {
                                    this.ComponentRef.instance.SetNewWizardArgs(this.NewWizardArgs);
                                }
                            }

                            if (this.DataContext != null) {
                                if (this.ComponentRef.instance['SetDataContext']) {
                                    this.ComponentRef.instance.SetDataContext(this.DataContext);
                                }
                            }

                            this.logWindow.ComponentLoaded.emit(this.ComponentRef.instance);
                        });
                }
            }
        }
    }

    OnMouseDown(event: MouseEvent) {
        dragger.startMoving(this.WindowId, this.WindowContainerId, event);
    }
    OnMouseUp() {
        dragger.stopMoving(this.WindowContainerId);
    }

    public Destroy() {
        //if (this.ComponentRef != null) {
        //    this.ComponentRef.destroy();
        //    this.ComponentRef = null;
        //}

        //this.ChildComponent = null;
        //this.WindowArgs = null;
        //this.DataContext = null;
    }

    private defaultWidth: any;
    private defaultHeight: any;
    MaximizeClicked() {
        this.defaultWidth = this.Width;
        this.defaultHeight = this.Height;
        this.Width = "calc(100% - 10px)";
        this.Height = "calc(100% - 10px)";
        this.IsWindowMaximize = true;
    }
    RestoreClicked() {
        this.Width = this.defaultWidth;
        this.Height = this.defaultHeight;
        this.IsWindowMaximize = false;
    }

    CloseClicked() {
        if (this.IsEditComponent) {
            this.CurrentSession.CurrentEditComponent.BackButtonClicked();
        }

        else {
            if (this.NotifyOnClose) {
                if (this.ComponentInstance) {
                    this.ComponentInstance.OnWindowClosed();
                }
            }

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    OnCTRL_Shift_S_HotKeyPressed(){
        if (this.ComponentInstance && this.ComponentInstance.OnCTRL_Shift_S_HotKeyPressed) {
            this.ComponentInstance.OnCTRL_Shift_S_HotKeyPressed();
        }
    }

    OnEscHotKeyPressed(){
        if (this.ComponentInstance && this.ComponentInstance.OnEscHotKeyPressed) {
            this.ComponentInstance.OnEscHotKeyPressed();
        }
    }

    OnCTRL_S_HotKeyPressed(){
        if (this.ComponentInstance && this.ComponentInstance.OnCTRL_S_HotKeyPressed) {
            this.ComponentInstance.OnCTRL_S_HotKeyPressed();
        }
    }
}





