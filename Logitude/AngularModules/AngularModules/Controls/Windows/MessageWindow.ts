import {Component, AfterViewInit, ViewContainerRef, ViewChildren, QueryList, Output, EventEmitter, HostListener} from '@angular/core';
import {Settings} from '../../Infrastructure/Settings';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';

export class MessageWindow {
    public Width: number = 320;
    public Height: number = 170;
    //public Message: string = null;
    public Title: string = "Message";
    public IsOverAll: boolean = false;
    LayoutDirection: string = 'ltr';
    OkButtonText: string = "Ok";
    public ZIndex: number = 0;
    @Output() WindowClosed = new EventEmitter();
    public RTL: boolean = false;
    public ShowSuccessIcon: boolean = false;
    public ShowErrorIcon: boolean = false;
    public ShowWarningIcon: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsMessageMultiLine: boolean = false;
    constructor() {
        this.LayoutDirection = Settings.LayoutDirection;
        this.Title = TextCodeTranslator.Translate("General.O.Message");
        this.OkButtonText = TextCodeTranslator.Translate("General.B.Ok");
    }

    private message: string = null;
    get Message() { return this.message; }
    set Message(newValue: string) {
        this.message = newValue;
        if (this.InstanceComponent) {
            this.InstanceComponent.Message = newValue;
        }
    }

    private ComponentRef: any = null;
    private InstanceComponent: MessageWindowTemplateComponent = null;
    public Show(message: string) {
        this.Message = message;

        if (message.indexOf("Internet Connection Problem:") > -1) {
            this.Width = 440;
            this.Height = 440;
        }

        if (!this.CurrentSession) {
            this.CurrentSession = SessionLocator.SelectedSession;
        }

        if (this.CurrentSession.SessionLocation)
        {
            var viewContainerRefLocation: ViewContainerRef = this.CurrentSession.SessionLocation.viewContainerRef;
        }

        else {
            viewContainerRefLocation = SessionLocator.ApplicationLocation;
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

        SessionLocator.DynamicLoader.Load("./Controls/Windows/MessageWindowTemplateComponent", viewContainerRefLocation)
            .then(cmpRef => {
                this.ComponentRef = cmpRef;
                this.InstanceComponent = cmpRef.instance;
                this.InstanceComponent.InjectWindowComponent(this);
            });
    }

    public Close() {

        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;

            this.WindowClosed.emit("event");
        }

        this.InstanceComponent = null;
    }
}

@Component({
    selector: 'MessageWindow',
    
    templateUrl: "./MessageWindow.html",
})

export class MessageWindowTemplateComponent implements AfterViewInit {
    public Top: string = "50%";
    public Left: string = "50%";
    public Width: string = "320px";
    public Height: string = "170px";
    public Title: string = null;
    public Message: string = null;
    public ShowModal: boolean = true;
    public WindowId: string = null;
    public OkButtonId: string = null;
    public IsOverAll: boolean = false;
    OkButtonText: string = "Ok";
    public ZIndex: number = 0;
    LayoutDirection: string = 'ltr';
    public ShowSuccessIcon: boolean = false;
    public ShowErrorIcon: boolean = false;
    public ShowWarningIcon: boolean = false;
    public RTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsMessageMultiLine: boolean = false;
    constructor() {
        this.LayoutDirection = Settings.LayoutDirection;
        this.Title = TextCodeTranslator.Translate("General.O.Message");
        this.OkButtonText = TextCodeTranslator.Translate("General.B.Ok");
    }

    private isLoaderReady: boolean;
    ngAfterViewInit() {
        this.isLoaderReady = true;
        this.Focus();
    }

    private MessageWindow: MessageWindow
    public InjectWindowComponent(myWindow: MessageWindow) {

        this.CreateDynamicIds();

        this.MessageWindow = myWindow;
        this.Title = myWindow.Title;
        this.Message = myWindow.Message;
        this.IsOverAll = myWindow.IsOverAll;
        this.ZIndex = myWindow.ZIndex;
        this.RTL = myWindow.RTL;
        this.ShowSuccessIcon = myWindow.ShowSuccessIcon;
        this.ShowErrorIcon = myWindow.ShowErrorIcon;
        this.ShowWarningIcon = myWindow.ShowWarningIcon;
        this.IsMessageMultiLine = myWindow.IsMessageMultiLine;

        if (myWindow.Width != null) {
            this.Width = myWindow.Width + "px";
        }

        if (myWindow.Height != null) {
            this.Height = myWindow.Height + "px";
        }
    }

    private CreateDynamicIds() {
        this.WindowId = "MessageWindow_" + this.CurrentSession.SessionIndex;
        this.OkButtonId = "MessageWindow_Ok_" + this.CurrentSession.SessionIndex;
        this.Focus();
    }

    private Focus() {
        if (this.isLoaderReady) {
            if (this.OkButtonId != null) {
                document.getElementById(this.OkButtonId).focus();
            }
        }
    }

    private last: MouseEvent = null;
    public IsMouseCapture: boolean = false;
    OnMouseDown() {
        this.IsMouseCapture = true;
    }

    @HostListener('document:mouseup', ['$event'])
    onMouseup(event: MouseEvent) {
        this.IsMouseCapture = false;
    }

    @HostListener('document:mousemove', ['$event'])
    onMousemove(event: MouseEvent) {
        if (this.IsMouseCapture) {
            var windowObject = document.getElementById(this.WindowId);
            if (windowObject != null) {
                if (event != null) {

                    if (this.last != null) {

                        var rect = windowObject.getBoundingClientRect();

                        var yPosition = (windowObject.offsetTop + event.clientY - this.last.clientY);
                        var xPosition = (windowObject.offsetLeft + event.clientX - this.last.clientX);

                        var isDraggingLeft = false;
                        if ((event.clientX - this.last.clientX) < 0) {
                            isDraggingLeft = true;
                        }

                        if (isDraggingLeft) {
                            if (rect.left >= 10) {
                                windowObject.style.left = xPosition + 'px';
                            }
                        }

                        else {
                            if (rect.right <= (window.innerWidth - 10)) {
                                windowObject.style.left = xPosition + 'px';
                            }
                        }

                        var isDraggingTop = false;
                        if ((event.clientY - this.last.clientY) < 0) {
                            isDraggingTop = true;
                        }


                        if (isDraggingTop) {
                            if (rect.top >= 10) {
                                windowObject.style.top = yPosition + 'px';
                            }
                        }

                        else {
                            if (rect.bottom <= (window.innerHeight - 10)) {
                                windowObject.style.top = yPosition + 'px';
                            }
                        }
                    }

                    this.last = event;
                }
            }
        }
    }

    OkButtonClicked() {
        this.MessageWindow.Close();
    }
    OnCTRL_S_HotKeyPressed(){
        this.OkButtonClicked();
    }
    GetIconPath() {
        var path = "./Images/InfoIcon.png";
        if (this.ShowSuccessIcon)
            path = "./Images/SuccessIcon.png";
        if (this.ShowErrorIcon)
            path = "./Images/ErrorIcon.png";
        if (this.ShowWarningIcon)
            path = "./Images/SimplogIcons/Warning.png";
        
        return path;
    }
}
