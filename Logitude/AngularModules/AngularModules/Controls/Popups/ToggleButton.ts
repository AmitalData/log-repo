import {Component, OnInit, Output, EventEmitter, ChangeDetectionStrategy, OnDestroy} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: "ToggleButton",
    moduleId: module.id,
    templateUrl: './ToggleButton.html',
    inputs: ['Title', 'IconPath', 'DropDownWidth', 'DropDownHeight', 'Position', 'IsEnabled', 'IsOpened'],
    //changeDetection: ChangeDetectionStrategy.OnPush,
})

export class ToggleButton implements OnInit, OnDestroy {
    public ComponentId: string = null;
    public ComponentButtonId: string = null;
    public ComponentContentId: string = null;
    public Title: string = null;
    public IconPath: string = null;
    public IsEnabled: boolean = true;
    public Position: string = "Right";
    public DropDownWidth: number = 0;
    public DropDownHeight: number = 0;
    @Output() Opened: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        var idIndex = this.CurrentSession.GetNewId("HelperNotes");
        this.ComponentId = "HelperNotes_" + idIndex;
        this.ComponentButtonId = "HelperNotesButton_" + idIndex;
        this.ComponentContentId = "HelperNotesContent_" + idIndex;

        this.CurrentSession.MouseDownEvent
    }

    private SessionEvent: any = null;
    ngOnInit() {
        this.SessionEvent = this.CurrentSession.MouseDownEvent.subscribe(s => {
            if (this.IsMouseOverButton == false && this.IsMouseOver == false) {
                this.IsOpened = false;
            }
        });
    }
    ngOnDestroy() {
        this.StopPositionTimer();
        AppTool.KillEventEmitter(this.SessionEvent);
        this.SessionEvent = null;
    }

    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                this.Opened.emit(value);

                if (value) {
                    this.SetPopupSize();
                    this.RunPositionTimer();
                }

                else {
                    this.StopPositionTimer();                    
                }
            }
        }
    }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.CalculateFixedPosition(), 0);
    }
    private CalculateFixedPosition() {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 23) + 'px';

            if (this.Position == "Left") {
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left) + 'px';
            }

            else {
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left + itemRect.width - this.DropDownWidth) + 'px';
            }
        }
    }
    private SetPopupSize() {

        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            if (this.DropDownWidth == 0) {
                this.DropDownWidth = itemRect.width;
            }

            if (this.DropDownHeight == 0) {
                this.DropDownHeight = 100;
            }
        }      
    }

    public IsMouseOver: boolean = false;
    public IsMouseOverButton: boolean = false;
    public IsMouseOverTextBox: boolean = false;
    OnButtonClicked() {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }

        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    }
}

@Component({
    selector: 'ToggleButtonItem',
    inputs: ['Text'],

    template:
    `
    <button class="LogitudeToggleButtonItem" (click)="OnClick()">{{Text}}</button>
    `,

    styles: [
    `

    .LogitudeToggleButtonItem:hover:not(:disabled):not(.IconToggleButton):not(.HyperlinkButton):not(.CustomButton) {
        background: rgba(255, 239, 43, 0.2);
        border: 1px solid #FFC92B;
    }

    .LogitudeToggleButtonItem:disabled {
        opacity: 0.5;
        cursor: default;
    }

    .LogitudeToggleButtonItem:not(.HyperlinkButton):not(.LogitudeIconButton):not(.CustomButton) {
        display: block;
        height: 23px;
        line-height: 23px;
        text-align: left;
        /*width: 100%;*/
        width:auto;
        min-width: 100%;
        border: 1px solid white;
        background: white;
        padding-left: 5px;
        padding-right: 3px;
    }

    `
    ]
})

export class ToggleButtonItem {
    public Text: string = "Item";
    @Output() click = new EventEmitter();
    @Output() Clicked = new EventEmitter();
    constructor(private toggleButton: ToggleButton) {

    }

    OnClick() {
        this.toggleButton.IsOpened = false;

        //setTimeout(() => this.Emit(), 10);
    }

    Emit() {
        //this.click.emit();
        //this.Clicked.emit();
    }
}
