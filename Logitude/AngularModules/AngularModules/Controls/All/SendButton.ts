import { Component, EventEmitter, Output, Input, OnInit, ElementRef, ChangeDetectorRef} from '@angular/core';
import { AppTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ControlsIdCounter } from 'Infrastructure/Utilities/ControlsIdCounter';

@Component({
    selector: 'SendButton',
    
    templateUrl: './SendButton.html',
    inputs: ['ItemsSource', 'SelectedItem', 'Binding'],
})

export class SendButton implements OnInit {

    @Input()
    public IsDisabled: boolean
    @Input()
    ButtonText: string = "Send";

    _ButtonCodeText: string;
    @Input()
    public get ButtonCodeText() { return this._ButtonCodeText; }
    public set ButtonCodeText(val: string) {
        if (this._ButtonCodeText == val) return;
        this._ButtonCodeText = val;
        this.ButtonText = TextCodeTranslator.Translate(val);
        this._CD.detectChanges();
    }
    public IsMouseOver: boolean = false;
    _DropdownDisplay: string = 'none';
    private _ElementRef: any;

    static MyId: number = 0;
    _CustomSendOptionsComponentId: string;
    _CustomSendOptionsComponentMenuId: string;
    ListControlId: string;
    public ControlId: string = null;

    _IsLoaded: boolean = false;

    public ItemsSource: any[];
    public Binding: string = null;
    public Text: string = null;
    public IsEnabled = true; 

    @Output() SelectedItemChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() LostFocus: EventEmitter<boolean> = new EventEmitter<boolean>();

    private selectedItem = null;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: any) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
            this.SetDisplayText();
        }
    }
    ItemClicked(clickedItem: any) {
        if (clickedItem != null && clickedItem.IsEnabled) {
            if (this.SelectedItem != clickedItem) {
                this.SelectedItem = clickedItem;

                this.SetDisplayText();
                this.SelectedItemChanged.emit(this.SelectedItem);
                this.DropdownDisplayClose();
            }
        }
    }
    private SetDisplayText() {
        var myDisplayText: string = null;

        if (this.SelectedItem != null) {
            this.IsEnabled = this.SelectedItem.IsEnabled; 

            if (this.Binding == null) {
                myDisplayText = this.SelectedItem;
            }

            else {
                myDisplayText = this.SelectedItem[this.Binding];
            }
        }
        this.Text = myDisplayText;
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _CD: ChangeDetectorRef, myElement: ElementRef) {
        this.ItemsSource = [];

        this._ElementRef = myElement;

        var buttonId = ControlsIdCounter.GetNextControlIdCounter("CustomSendOptionsComponent");

        // var idIndex = this.CurrentSession.GetNewId("SenButton");

        this._CustomSendOptionsComponentId = "SendButtom_" + ControlsIdCounter.GetNextControlIdCounter("CustomSendOptionsComponent");
        this._CustomSendOptionsComponentMenuId = "SendButtomMenu_" + ControlsIdCounter.GetNextControlIdCounter("CustomSendOptionsComponentMenuId");
        this.ControlId = ControlsIdCounter.GetNextControlIdCounter("ComboBox")+"";
        this.ListControlId = ControlsIdCounter.GetNextControlIdCounter("List")+"";
    }
    handleClick(event) {
        var clickedComponent = event.target;
        var inside = false;
        let conter = 0;
        do {
            if (clickedComponent === this._ElementRef.nativeElement) {
                inside = true;
                break;
            }
            if (conter > 10) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {

        } else {

            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle();
            }

            //alert("outside");
        }
    }
    ngOnInit() {
        this._IsLoaded = true;
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
    }
    DropdownDisplayClose() {
        this._DropdownDisplay = 'none';
    }
    Width = -30;
    Height = -20;
   
    dropdowndisplayToggle() {
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._CustomSendOptionsComponentId);
            var itemRect = item.getBoundingClientRect();

            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
            document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                itemRect.top + 'px';

            let DDLHeight = 67;//    height: 22px; * 3 +30 
            let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
            if (itemRect.bottom + DDLHeight > this.getScreenHeight()) {//this.PaintTop = true                
                document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                    (itemRect.top - DDLHeight - Extra) + 'px';
            }
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left =
            //    (itemRect.left + 80) + 'px';//min-width: 80px
            this._DropdownDisplay = 'block';
        } else {
            this._DropdownDisplay = 'none';
        }

    }
    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }

    mousedownGreenButton() {
        var clickedItem = this.SelectedItem;
        if (clickedItem != null) {
            var sendButton = document.getElementById(this.ControlId);
            if (sendButton != null) {
                sendButton.blur();
            }

            this.OnLostFocus();
            this.SetDisplayText();
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    }
    OnLostFocus() {
        this.DropdownDisplayClose();
        this.LostFocus.emit(true);

    }
    OnButtonLostFocus() {
        if (this.IsMouseOver) {
            document.getElementById(this._CustomSendOptionsComponentMenuId).focus();
        }
        else {
            this.DropdownDisplayClose();
        }
    }
    OnFocus() {
      
    }
    OnKeyDown($event) {
        if ($event) {
            switch ($event.keyCode) {
                case 8:
                case 46:
                    {
                        this.SelectedItem = null;
                        this.SelectedItemChanged.emit(null);
                        break;
                    }
            }
        }
    }
}
