import {Component, OnInit, OnDestroy, Output, EventEmitter, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../Infrastructure/Tools'
import {ObjectsLocator} from '../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'ComboBox',
    moduleId: module.id,
    templateUrl: './ComboBox.html',
    inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'IsDisabled', 'WaterMark', 'IsBlueBox', 'IsGreenButton', 'FocusOnMe', 'SelectedValue', 'SelectedValuePath', 'MaxHeight', 'WithIcons'],
})

export class ComboBox implements OnInit, AfterViewInit, OnDestroy {
    public Text: string = null;
    public WaterMark: string = null;
    private itemsSource: any[];
  //  public ItemsSource: any[];
    dropdownTimertoken: any;
    get ItemsSource() { return this.itemsSource; }
    set ItemsSource(value: any[]) {
        if (this.itemsSource != value) {

            this.itemsSource = value;
            if (this.ItemsButtonClicked) {
                //this.OnMouseDown(false);
                if (this.dropdownTimertoken) {
                    clearTimeout(this.dropdownTimertoken);
                }
                this.dropdownTimertoken = setTimeout(() => {
                    //this.OnMouseDown(false);
                    if (this.selectedIndex) this.SelectedItem = this.ItemsSource[this.selectedIndex];
            }, 1);
                //this.cd.detectChanges();
            }
            this.ItemsButtonClicked = false;

        }
    }

    public WithIcons: boolean = false;
    public Binding: string = null;
    public ControlId: string = null;
    public DropdownId: string = null;
    public ListControlId: string = null;
    public MinHeight: number = 30;
    public MaxHeight: number = 250;
    public IsBlueBox: boolean = false;
    public IsDisabled: boolean = false;
    public IsMouseOverControl: boolean = false;
    public FocusOnMe: boolean = false;
    public LayoutDirection: string = 'ltr';
    public ItemsButtonClicked: boolean;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();
    @Output() ComboBoxDropDownClicked: EventEmitter<any> = new EventEmitter();
    @Output() LostFocus: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd:ChangeDetectorRef) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

        this.ItemsSource = [];

        if (this.CurrentSession == null) {
            this.ControlId = "ComboBox_-1_-1";
            this.DropdownId = "Dropdown_-1_-1";
            this.ListControlId = "List_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ComboBox");
            this.ControlId = "ComboBox_" + idIndex;
            this.DropdownId = "Dropdown_" + idIndex;
            this.ListControlId = "List_" + idIndex;
        }
    }

    private MouseDownEvent: any = null;
    ngOnInit() {
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }

        if (this.SelectedValue != null) {
            this.SetSelectedItemFromValue();
        }

        if (this.CurrentSession) {
            if (this.CurrentSession.MouseDownEvent) {
                this.MouseDownEvent = this.CurrentSession.MouseDownEvent.subscribe(s => {
                    if (s) {
                        if (this.IsOpened) {
                            if (this.IsMouseOverControl == false) {
                                this.OnLostFocus();
                            }
                        }
                    }
                });
            }
        }
    }
    ngAfterViewInit() {
        if (this.FocusOnMe) {

            var element = document.getElementById(this.ControlId);
            element.focus();

            this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id });
            //this.timerToken = setTimeout(() => {
            //    SelectingElement(element);
            //}, 1);

        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.MouseDownEvent);
    }

    //private itemsSource: any[];
    //get ItemsSource() { return this.itemsSource; }
    //set ItemsSource(value: any[]) {
    //    if (this.itemsSource != value) {

    //        this.itemsSource = value;

    //        if (this.ItemsButtonClicked && !this.IsOpened) {
    //            this.CloseDropDown();
    //        }

    //        this.ItemsButtonClicked = false;
    //    }
    //}

    private isGreenButton = false;
    get IsGreenButton() { return this.isGreenButton; }
    set IsGreenButton(value: boolean) {
        if (this.isGreenButton != value) {
            this.isGreenButton = value;
        }
    }

    private selectedItem = null;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: any) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
            this.SetDisplayText();
        }
    }

    private selectedValue = null;
    get SelectedValue() { return this.selectedValue; }
    set SelectedValue(value: any) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
            this.SetSelectedItemFromValue();
        }
    }

    private selectedValuePath: string = null;
    get SelectedValuePath() { return this.selectedValuePath; }
    set SelectedValuePath(value: string) {
        if (this.selectedValuePath != value) {
            this.selectedValuePath = value;
            this.SetSelectedItemFromValue();
        }
    }

    SetSelectedItemFromValue() {
        if (this.SelectedValue && this.SelectedValuePath) {
            var selectedItem = this.ItemsSource.filter(f => f[this.SelectedValuePath] === this.SelectedValue)[0];
            this.SelectedItem = selectedItem;
        }
        else {
            this.SelectedItem = null;
        }
    }

    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

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
        var item = document.getElementById(this.ControlId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.DropdownId).style.top = (itemRect.top + 23) + 'px';
            document.getElementById(this.DropdownId).style.left = (itemRect.left) + 'px';
        }
    }
    private SetPopupSize() {

        var item = document.getElementById(this.ControlId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.DropdownId).style.width = itemRect.width + "px";

            if (this.ItemsSource.length == 0) {
                document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
            }

            else {
                var itemsHeight = ((this.ItemsSource.length * 23) + 3);
                if (itemsHeight > this.MaxHeight) {
                    document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                    document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                }

                else {
                    document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                    document.getElementById(this.ListControlId).style.height = "100%";
                }
            }
        }
    }

    mousedown() {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }

        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;

            //this.ItemsButtonClicked = true;
            this.ComboBoxDropDownClicked.emit();
        }
    }    

    OnLostFocus() {
        this.CloseDropDown();
        this.LostFocus.emit(true);
    }

    CloseDropDown() {
        this.IsOpened = false;
        this.StopPositionTimer();
        this.IsOpened = false;
    }


    ItemClicked(clickedItem: any) {
        if (clickedItem != null) {
            if (this.SelectedItem != clickedItem) {
                this.SelectedItem = clickedItem;

                this.SetDisplayText();
                this.SelectedItemChanged.emit(this.SelectedItem);
                this.CloseDropDown();
                //}
            }
        }
    }

    private SetDisplayText() {
        var myDisplayText: string = null;

        if (this.SelectedItem != null) {
            if (this.WithIcons) {
                myDisplayText = this.SelectedItem;
            }

            else {
                if (this.Binding == null) {
                    myDisplayText = this.SelectedItem;
                }

                else {
                    myDisplayText = this.SelectedItem[this.Binding];
                }
            }
        }

        this.Text = myDisplayText;
    }

    selectedIndex: number;
    OnKeyDown($event) {
        if ($event) {
            switch ($event.keyCode) {
                case 8:
                case 46: // delete, backspace
                    {
                        this.SelectedItem = null;
                        this.SelectedItemChanged.emit(null);
                        break;
                    }
                case 40: // ↓
                    {
                        if (!this.IsOpened) {

                            this.OnMouseDown(true);
                            this.InitiateSelectedItem();

                        } else {
                            // navigate to items
                            this.selectedIndex = ((this.selectedIndex == this.ItemsSource.length - 1) ? 0 : this.selectedIndex + 1); // increment index
                            this.SetSelectedItem(this.selectedIndex);
                        }
                        break;
                    }
                case 38: // ↑
                    {
                        if (this.IsOpened) {
                            // navigate to items
                            this.selectedIndex = ((this.selectedIndex == 0) ? this.ItemsSource.length - 1 : this.selectedIndex - 1); // decrement index
                            this.SetSelectedItem(this.selectedIndex);
                        }
                        break;
                    }
                case 13: // Enter
                    {
                        if (!this.IsOpened) {
                            this.OnMouseDown(true);
                            this.IsOpened = true;
                        } else {
                            this.SetDisplayText();
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.CloseDropDown();
                        }
                        break;
                    }
                case 27: // Esc
                    {
                        if (this.IsOpened) {
                            this.CloseDropDown();
                        }
                        break;
                    }

                case 9: // Tab
                    {
                        if (this.IsOpened) {
                            this.SetDisplayText();
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.CloseDropDown();
                        }
                        break;
                    }
            }
        }
    }

    SetSelectedItem(index: number) {
        if (this.ItemsSource && !AppTool.IsNullOrEmpty(index)) {

            //Select the item
            this.SelectedItem = this.ItemsSource[index];
            this.cd.detectChanges();

            //Set scroll
            var selectedElements = document.getElementsByClassName("SelectedComboboxItem");
            if (selectedElements) {
                var selectedElement:any = selectedElements[0];
                if (selectedElement) selectedElement.scrollIntoView(false);
            }

        }
    }

    mousedownGreenButton() {
        var clickedItem = this.SelectedItem;
        if (clickedItem != null) {
            var myComboBox = document.getElementById(this.ControlId);
            if (myComboBox != null) {
                myComboBox.blur();
            }

            this.OnLostFocus();
            this.SetDisplayText();
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    }
    InitiateSelectedItem() {
        // set selected item for first time
        if (this.ItemsSource && this.ItemsSource.length > 0) {
            if (!this.SelectedItem) {
                this.selectedIndex = 0;
                this.SelectedItem = this.ItemsSource[0];
            } else {
                var index = this.ItemsSource.indexOf(this.SelectedItem);
                this.selectedIndex = index;
                this.SetSelectedItem(this.selectedIndex);

            }
        }
    }

    OnMouseDown(emitClicked: boolean) {
        var myRootControl = document.getElementById(this.ControlId);
        if (myRootControl) {
            var myDropDownControl = document.getElementById(this.DropdownId);
            if (myDropDownControl) {

                if (myDropDownControl.style.visibility == "visible") {
                    this.CloseDropDown();
                }

                else {
                    this.IsOpened = true;

                    // set selected item for first time
                    this.InitiateSelectedItem();

                    if (this.ItemsSource == null) {
                        this.ItemsSource = [];
                    }

                    myDropDownControl.style.width = myRootControl.offsetWidth + "px";

                    if (this.ItemsSource.length == 0) {
                        myDropDownControl.style.height = this.MinHeight + "px";
                    }

                    else {
                        var itemsHeight = ((this.ItemsSource.length * 23) + 3);
                        if (itemsHeight > this.MaxHeight) {
                            myDropDownControl.style.height = this.MaxHeight + "px";
                            document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                        }

                        else {
                            myDropDownControl.style.height = itemsHeight + "px";
                            document.getElementById(this.ListControlId).style.height = "100%";
                        }
                    }
                    if (emitClicked) {
                        this.ItemsButtonClicked = true;
                        this.ComboBoxDropDownClicked.emit();
                    }
                    myDropDownControl.style.visibility = "visible";
                    this.RunPositionTimer();
                }
            }
        }
    }

    public CheckSource: Array<boolean>;
    ClickItem(item: any, index: any) {
        if (this.CheckSource == null) {
            if (this.ItemsSource != null) {
                this.CheckSource = new Array(this.ItemsSource.length);
                for (var i = 0; i < this.CheckSource.length; i++)
                    this.CheckSource[i] = false;
            }
        }

        if (this.CheckSource[index] == false) {
            this.CheckSource[index] = true;
        }
        else {
            this.CheckSource[index] = false;
        }

        item.Checked = this.CheckSource[index];
        
        this.ItemsSource[index] = item;
        this.Text = "";

        for (var i = 0; i < this.ItemsSource.length; i++) {
            if (this.ItemsSource[i].Checked) {
                if (AppTool.IsNullOrEmpty(this.Text)) {
                    this.Text = this.ItemsSource[i].Code;
                }

                else {
                    this.Text = this.Text + ", " + this.ItemsSource[i].Code;
                }
            }
        }

        this.SelectedItemChanged.emit(this.Text);
    }
}
