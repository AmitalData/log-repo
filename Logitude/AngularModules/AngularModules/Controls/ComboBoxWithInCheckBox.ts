/// <reference path="../infrastructure/components/logitudecomponents/basecomponent.ts" />
import {Component, OnInit, Output, EventEmitter,AfterViewInit} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {} from "@angular/platform-browser/src/dom";
@Component({
    selector: 'ComboBoxWithInCheckBox',
    moduleId: module.id,
    templateUrl: './ComboBoxWithInCheckBox.html',
    inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'IsDisabled', 'WaterMark', 'IsBlueBox', 'WithinImage','SelectionType','CheckBoxOnly'],
})

export class ComboBoxWithInCheckBox implements OnInit,AfterViewInit {
    public Text: string = null;
    public WaterMark: string = null;
    public ItemsSource: any[];
    public WithinImage: boolean = false;
    public Binding: string = null;
    public ControlId: string = null;
    public DropdownId: string = null;
    public ListControlId: string = null;
    public MinHeight: number = 30;
    public MaxHeight: number = 250;
    public IsBlueBox: boolean = false;
    public IsDisabled: boolean = false;
    public IsOpened: boolean = false;
    public ShowSelected: boolean = false;
    public DataContext: ComboBoxWithInCheckBox = this;
    public IsAll: boolean = true;
    public IsSelected: boolean = false;
    public RadioFocus: boolean = false;
    public IsMouseOverInput: boolean = false;
    public SelectionType: string = " Products"
    public ProductsSelectionType: string = " Selected " + this.SelectionType;
    public TotalPickedItems: string = " All";
    public CheckBoxOnly: boolean = false;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();
    @Output() EditedItemSource: EventEmitter<any> = new EventEmitter();
    public CheckSource: Array<boolean>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
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
    ngAfterViewInit() {
       

    }
    private SetControlPosition() {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.position = "fixed";
            document.getElementById(this.DropdownId).style.top = (itemRect.top ) + 'px';
            document.getElementById(this.DropdownId).style.left = itemRect.left + 'px';
        }
    }

    public IsMouseOver: boolean = false;

    mousedown() {

        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.SetControlPosition();
            document.getElementById(this.DropdownId).style.width = item.offsetWidth + "px";
            if (this.ItemsSource.length == 0 || this.ItemsSource==null) {
                document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
            }
            else {
                var itemsHeight = ((this.ItemsSource.length * 23) + 3);
                if (itemsHeight > this.MaxHeight) {
                    document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                 //   document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                }
                else {
                    document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                   // document.getElementById(this.ListControlId).style.height = "100%";
                }
            }
          //  document.getElementById(this.DropdownId).style.visibility = "visible";
        }
    }

    setToggleButtonMenuTemp() {

        var ToggleBTN = document.getElementById(this.ControlId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

    } 
    setToggleButtonMenu() {
        var ToggleBTN = document.getElementById(this.ControlId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    ngOnInit() {
       


        if (this.SelectedItem != null) {
            this.SetDisplayText();
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


    clickItem(item: any, index: any) {
        if (this.CheckSource == null) {
            if (this.ItemsSource != null) {
                this.CheckSource = new Array(this.ItemsSource.length);
                for (var i = 0; i < this.CheckSource.length; i++)
                    this.CheckSource[i] = false;
            }
        }
        if (this.CheckSource[index] == false) 
            this.CheckSource[index] = true;
        else
            this.CheckSource[index] = false;

        item.Checked = this.CheckSource[index];

        


            
        this.TotalPickedItems += "," + item.Name;
        this.ItemsSource[index] = item;
        this.TotalPickedItems = "";
        if (!this.WithinImage) {
            for (var i = 0; i < this.ItemsSource.length; i++) {

                if (this.ItemsSource[i].Checked)
                    this.TotalPickedItems += this.ItemsSource[i].Name + ",";


            }
        }
        else {


            for (var i = 0; i < this.ItemsSource.length; i++) {

                if (this.ItemsSource[i].Checked)
                    this.TotalPickedItems += this.ItemsSource[i].Code + ",";


            }

        }
        
        this.EditedItemSource.emit(this.ItemsSource);
    }
    private timerToken: any;

    IsChecked() {

        return false;


    }


    ComboBoxClicked() {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.IsOpened = !this.IsOpened;

            if (!this.IsOpened) {
                item.blur();
            }
        }
    }

    isSelectedClicked() {
        this.ShowSelected = true;
        this.SelectedItem = "NotAll";
        this.TotalPickedItems = "";
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.IsAll = false;
    }

    isAllClicked() {
        this.ShowSelected = false;
        this.SelectedItem = "All";
        this.TotalPickedItems = "All";
        this.IsAll = true;
        if (this.ItemsSource != null) {
            this.ItemsSource.forEach(item => {
                item.Checked = false;
            });
        }
        this.CheckSource = null;    
        this.SelectedItemChanged.emit(this.SelectedItem);


    }

 

    ItemClicked(clickedItem: any) {
        if (this.CheckSource == null) {
            if (this.ItemsSource != null) {
                this.CheckSource = new Array(this.ItemsSource.length);
                for (var i = 0; i < this.CheckSource.length; i++)
                    this.CheckSource[i] = false;
            }
        }
    }

    private SetDisplayText() {
        var myDisplayText: string = null;

        if (this.SelectedItem != null) {
            if (this.Binding == null) {
                myDisplayText = this.SelectedItem;
            }

            else {
                myDisplayText = this.SelectedItem[this.Binding];
            }
        }

        this.Text = myDisplayText;
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
