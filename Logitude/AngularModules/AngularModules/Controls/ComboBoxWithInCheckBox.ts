import {Component, OnInit, Output, EventEmitter,AfterViewInit, Input} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../Infrastructure/Tools';

@Component({
    selector: 'ComboBoxWithInCheckBox',

    templateUrl: './ComboBoxWithInCheckBox.html',
    inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'IsDisabled', 'WaterMark', 'IsBlueBox', 'WithinImage', 'SelectionType', 'CheckBoxOnly', 'IsAreasMenu'],
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
    @Input() public MinHeight: number = 45;
    public MaxHeight: number = 250;
    @Input() public AutoHeight: boolean = false;

    public IsBlueBox: boolean = false;
    public IsDisabled: boolean = false;
    public IsOpened: boolean = false;
    public ShowSelected: boolean = false;
    public DataContext: ComboBoxWithInCheckBox = this;
    public IsAll: boolean = true;
    public IsSelected: boolean = false;
    public RadioFocus: boolean = false;
    public IsMouseOverInput: boolean = false;
    public TotalPickedItems: string ;
    public CheckBoxOnly: boolean = false;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();
    @Output() EditedItemSource: EventEmitter<any> = new EventEmitter();
    public CheckSource: Array<boolean>;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsAreasMenu: boolean = false;
    public SearchAreasId: string = "SearchAreasId";
    public InitialItemsSource: any[];

    public selectionType: string = " Products";
    get SelectionType(): string {
        return this.selectionType;
    }
    set SelectionType(value: string) {
        this.selectionType = value;
        this.ProductsSelectionType = " Selected " + this.SelectionType;
    }
    public ProductsSelectionType: string = " Selected " + this.SelectionType;
    
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

        this.SearchAreasId += this.CurrentSession.GetNewId("SearchAreasId_1");

    }

    ngAfterViewInit() {
        this.InitialItemsSource = this.ItemsSource;
        this.SetDefaultTotalPickedItems();
        if (!this.IsAreasMenu) {
            this.TotalPickedItems = " All";
        }
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
                if (this.AutoHeight) {
                    document.getElementById(this.DropdownId).style.height = "auto";
                    document.getElementById(this.DropdownId).style.maxHeight = this.MaxHeight + "px";
                    document.getElementById(this.DropdownId).style.minHeight = this.MinHeight + "px";
                }else{

                    var itemsHeight = ((this.ItemsSource.length * 23) + this.MinHeight);
                    if (itemsHeight > this.MaxHeight) {
                        document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                        //   document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                    } else if (itemsHeight < this.MinHeight) {
                        document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
                    }
                    else {
                            document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                        // document.getElementById(this.ListControlId).style.height = "100%";
                    }
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
        if (this.CheckSource[index] == false && !item.Checked) {
            this.CheckSource[index] = true;
        }

        else {
            this.CheckSource[index] = false;
        }

        item.Checked = this.CheckSource[index];

        this.TotalPickedItems += "," + item.Name;
        this.ItemsSource[index] = item;
        this.TotalPickedItems = "";
        if (!this.WithinImage) {
            if (this.ItemsSource.filter(i => i.Checked)[0] == null) {
                this.TotalPickedItems = " All";
            } else {
                for (var i = 0; i < this.ItemsSource.length; i++) {
                    if (this.ItemsSource[i].Checked) {
                        this.TotalPickedItems = [this.TotalPickedItems, this.ItemsSource[i].Name].filter(Boolean).join(",");
                    }
                }
            }
        }

        else {
            for (var i = 0; i < this.ItemsSource.length; i++) {
                if (this.ItemsSource[i].Checked) {
                    this.TotalPickedItems += this.ItemsSource[i].Code + ",";
                }
            }
        }

        this.EditedItemSource.emit(this.ItemsSource);
    }

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
    SetDefaultTotalPickedItems() {
        if (!this.WithinImage) {
            if (this.ItemsSource.filter(i => i.Checked)[0] == null) {
                this.TotalPickedItems = " All";
            } else {
                for (var i = 0; i < this.ItemsSource.length; i++) {
                    if (this.ItemsSource[i].Checked) {
                        this.TotalPickedItems = [this.TotalPickedItems, this.ItemsSource[i].Name].filter(Boolean).join(",");
                    }
                }
            }
        }
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

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchAreasId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.DropdownId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }
    FillPlaceHolder() {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchAreasId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.DropdownId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    OnDeleteValue() {
        var temp = document.getElementById(this.SearchAreasId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    }

    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
            this.ReBuildItemsSource();
        }
    }
    ReBuildItemsSource() {
        var tempData = this.InitialItemsSource;
        var data: any[] = [];

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            data = tempData.filter(f => f.Name.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1);
        }
        else {
            data = tempData;
        }

        this.ItemsSource = data;
    }
}
