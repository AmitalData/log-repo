import {Component, AfterViewInit, ViewContainerRef, ViewChildren, QueryList, Output, EventEmitter, HostListener} from '@angular/core';
import {Settings} from '../../Infrastructure/Settings';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';

export class ConfirmWindow {
    public Width: number = 320;
    public Height: number = 170;
    public Message: string = null;
    public ShowCheckBox: boolean = false;
    public Title: string = "Confirm";
    public No: boolean = false;
    public Yes: boolean = false;
    public Cancel: boolean = false;
    public ShowCancelButton: boolean = false;
    public ShowNoButton: boolean = true;
    public NoButtonText: string;
    public YesButtonText: string;
    public CancelButtonText: string;
    public IsOverAll: boolean = false;
    public ShowWarningImage: boolean = false;
    public ShowInfoImage: boolean = false;
    public ShowErorImage: boolean = false;
    LayoutDirection: string = 'ltr';
    @Output() WindowClosed = new EventEmitter();
    public IsChecked: boolean = false;
    public IsYesEnabled: boolean = true;
    public StringColor: string = "#6E7172";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LayoutDirection = Settings.LayoutDirection;
        this.Title = TextCodeTranslator.Translate("General.O.Confirm");
        this.CancelButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        this.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        this.NoButtonText = TextCodeTranslator.Translate("General.B.No");
    }

    private ComponentRef: any = null;
    private InstanceComponent: ConfirmWindowTemplateComponent = null;
    public Show(message: string) {
        this.Message = message;

        var viewContainerRefLocation: ViewContainerRef = this.CurrentSession.SessionLocation.viewContainerRef;

        if (this.IsOverAll) {
            viewContainerRefLocation = SessionLocator.ApplicationLocation;
        }

        else if (this.CurrentSession.CurrentWindow) {
            if (this.CurrentSession.CurrentWindow.IsOverAll) {
                this.IsOverAll = true;
                viewContainerRefLocation = SessionLocator.ApplicationLocation;
            }
        }

        SessionLocator.DynamicLoader.Load("./Controls/Windows/ConfirmWindowTemplateComponent", viewContainerRefLocation)
            .then(cmpRef => {
                this.ComponentRef = cmpRef;
                this.InstanceComponent = cmpRef.instance;
                this.InstanceComponent.InjectWindowComponent(this);
            });
    }
    public  WindowClosedPromise() {
        return new Promise(resolve =>
        {
            this.WindowClosed.subscribe((event: any) => {
                resolve(this);
            });
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
    selector: 'ConfirmWindow',
    
    templateUrl: "./ConfirmWindow.html",
})

export class ConfirmWindowTemplateComponent implements AfterViewInit {
    public Top: string = "50%";
    public Left: string = "50%";
    public Width: string = "320px";
    public Height: string = "170px";
    public Title: string = null;
    public Message: string = null;
    public ShowModal: boolean = true;
    public WindowId: string = null;
    public ShowCancelButton: boolean = false;
    public ShowNoButton: boolean = true;
    public NoButtonId: string = null;
    public YesButtonId: string = null;
    public CancelButtonId: string = null;
    public NoButtonText: string = "No";
    public YesButtonText: string = "Yes";
    public CancelButtonText: string = "Cancel";
    public IsOverAll: boolean = false;
    public ShowWarningImage: boolean = false;
    public ShowInfoImage: boolean = false;
    public ShowErorImage: boolean = false;
    LayoutDirection: string = 'ltr';
    public ShowCheckBox: boolean = false;
    public IsYesEnabled: boolean = true;
    public StringColor: string = "#6E7172";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LayoutDirection = Settings.LayoutDirection;
        this.Title = TextCodeTranslator.Translate("General.O.Confirm");
        this.CancelButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        this.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        this.NoButtonText = TextCodeTranslator.Translate("General.B.No");
    }

    private isLoaderReady: boolean;
    ngAfterViewInit() {
        this.isLoaderReady = true;
        this.Focus();
    }

    private ConfirmWindow: ConfirmWindow
    public InjectWindowComponent(myWindow: ConfirmWindow) {

        this.CreateDynamicIds();

        this.ConfirmWindow = myWindow;
        this.Title = myWindow.Title;
        this.Message = myWindow.Message;
        this.ShowCancelButton = myWindow.ShowCancelButton;
        this.ShowNoButton = myWindow.ShowNoButton;
        this.NoButtonText = myWindow.NoButtonText;
        this.YesButtonText = myWindow.YesButtonText;
        this.IsOverAll = myWindow.IsOverAll;
        this.ShowWarningImage = myWindow.ShowWarningImage;
        this.ShowInfoImage = myWindow.ShowInfoImage;

        this.ShowErorImage = myWindow.ShowErorImage;
        this.ShowCheckBox = myWindow.ShowCheckBox;
        this.IsYesEnabled = myWindow.IsYesEnabled;
        this.StringColor = myWindow.StringColor;

        if (myWindow.Width != null) {
            this.Width = myWindow.Width + "px";
        }

        if (myWindow.Height != null) {
            this.Height = myWindow.Height + "px";
        }
    }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(newValue) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;

            this.IsYesEnabled = newValue;
            this.ConfirmWindow.IsChecked = newValue;
        }
    }

    private CreateDynamicIds() {
        this.WindowId = "ConfirmWindow_" + this.CurrentSession.SessionIndex;
        this.NoButtonId = "ConfirmWindow_No_" + this.CurrentSession.SessionIndex;
        this.YesButtonId = "ConfirmWindow_Yes_" + this.CurrentSession.SessionIndex;
        this.CancelButtonId = "ConfirmWindow_Cancel_" + this.CurrentSession.SessionIndex;
        this.Focus();
    }

    private Focus() {
        if (this.isLoaderReady) {

            if (this.ShowCancelButton) {
                if (this.CancelButtonId != null) {
                    document.getElementById(this.CancelButtonId).focus();
                }
            }

            else {
                if (this.NoButtonId != null && this.ShowNoButton) {
                    document.getElementById(this.NoButtonId).focus();
                }
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

    CancelButtonClicked() {
        this.ConfirmWindow.Cancel = true;
        this.ConfirmWindow.Close();
    }

    NoButtonClicked() {
        this.ConfirmWindow.No = true;
        this.ConfirmWindow.Close();
    }

    YesButtonClicked() {
        this.ConfirmWindow.Yes = true;
        this.ConfirmWindow.Close();
    }

    OnEscHotKeyPressed(){
        this.CancelButtonClicked();
    }

    OnCTRL_S_HotKeyPressed(){
        this.YesButtonClicked();
    }
}
