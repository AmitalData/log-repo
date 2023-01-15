import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output } from "@angular/core";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { SessionInfo } from "Infrastructure/Utilities/SessionInfo";
declare var window: any;

@Component({
    selector: 'MultiSelectDropDown',
    templateUrl: './MultiSelectDropDownComponent.html',
    styleUrls: ['./MultiSelectDropDownComponent.scss']
})


export class MultiSelectDropDownComponent implements OnInit {
    @Input() ObjectTableName: string;
    @Input() DisplayName: string;
    @Input() SelectedItem: any;
    @Input() CanSearch: boolean;
    @Input() Placeholder: string;
    @Input() IsMultiSelect: boolean;
    @Input() KeyPropertyPath: string;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();

    private itemsSource: any[];
    get ItemsSource() { return this.itemsSource; }
    @Input() set ItemsSource(value: any[]) {
        if (this.itemsSource == value || !value) return;
        this.itemsSource = value;
        this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));
    }
    public DisplayItemsSource: any[] = [];

    private EntityListService: EntityListService;
    public Loading: boolean = true;
    public SearchText: string;
    public SkeletonCount = Array(6).fill(0).map((x, i) => i);
    private ObjectTable: ObjectTablePM;
    private SelectedItems: any[] = [];
    public DisplayText = "";

    private isOpen: boolean = false;
    get IsOpen(): boolean {
        return this.isOpen;
    }
    set IsOpen(value: boolean) {
        if (value == this.isOpen) return;
        this.isOpen = value;
        if (value) {
            if (!this.ItemsSource) return;
            this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));
            return;
        }
        this.SearchText = null;
        this.onSearchChange(null);
    }

    constructor(private eRef: ElementRef) {
        this.EntityListService = new EntityListService();
    }

    @HostListener('document:click', ['$event'])
    clickout(event) {
        if (!this.eRef.nativeElement.contains(event.target)) {
            this.IsOpen = false;
        }
    }

    ngOnInit(): void {
        this.GetObjectTable();
        this.GetDataItemsSource();
    }

    GetObjectTable() {
        if (!this.ObjectTableName) return;
        this.ObjectTable = window.ObjectTables.filter(d => d.Name?.toLowerCase() === this.ObjectTableName.toLocaleLowerCase())[0];
        this.KeyPropertyPath = this.KeyPropertyPath ?? this.ObjectTable.KeyPropertyPath
    }

    GetDataItemsSource() {
        if (!this.ObjectTableName || !this.DisplayName) {
            this.SetDisplayText();
            return;
        }

        var filters = new ApiQueryFilters();
        filters.Tenant = SessionInfo.LoggedUserTenant;
        filters.GetAll = true;
        filters.ForceCacheRefresh = true;

        this.EntityListService.getAllFromCache(this.ObjectTableName, filters).then((res: any) => {
            res.subscribe((response: any) => {
                if (!response.Result) return;
                this.ItemsSource = JSON.parse(JSON.stringify(response.Result));
                this.Loading = false;
                this.SetDisplayText();
            })
        });
    }

    public OpenCloseDropDown() {
        this.IsOpen = !this.IsOpen;
    }

    public GetItemText(item: any) {
        if (item) return this.DisplayName ? item[this.DisplayName] : item;
        return "";
    }

    public SetDisplayText() {
        if (!this.SelectedItem) return this.DisplayText = "Select " + this.Placeholder?.toLocaleLowerCase();
        if (!this.IsMultiSelect) return this.DisplayText = this.DisplayName ? this.SelectedItem[this.DisplayName] : this.SelectedItem;

        var texts = this.SelectedItems.map(a => a[this.DisplayName]);
        this.DisplayText = texts.join(", ")
    }

    public onSearchChange(text: string) {
        if (!this.ItemsSource) return;
        if (!text) {
            this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));
            return;
        }

        this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource.filter(item => {
            if (!this.DisplayName) return item.toLowerCase().includes(text.toLowerCase());
            return item[this.DisplayName].toLowerCase().includes(text.toLowerCase());
        })));
    }

    public SelectItem(item: any) {
        if (!this.IsMultiSelect) {
            this.IsOpen = false;
            this.SelectedItem = item;
            this.SelectedItemChanged.emit(item);
            this.SetDisplayText();
            return;
        }
        if (!this.SelectedItem) this.SelectedItem = "";

        var itemSource = this.ItemsSource.find(x => x[this.KeyPropertyPath] == item[this.KeyPropertyPath]);
        itemSource.IsMultiSelectDropDownSelected = itemSource.IsMultiSelectDropDownSelected == undefined ? true : !itemSource.IsMultiSelectDropDownSelected;

        var dItemSource = this.DisplayItemsSource.find(x => x[this.KeyPropertyPath] == item[this.KeyPropertyPath]);
        dItemSource.IsMultiSelectDropDownSelected = dItemSource.IsMultiSelectDropDownSelected == undefined ? true : !dItemSource.IsMultiSelectDropDownSelected;

        var itemKey = itemSource[this.KeyPropertyPath];

        if (itemSource.IsMultiSelectDropDownSelected) this.SelectedItems.push(itemSource);
        else {
            const index = this.SelectedItems.indexOf(this.SelectedItems.find(x => x[this.KeyPropertyPath] == itemKey));
            if (index > -1) this.SelectedItems.splice(index, 1);
        }

        var keys = this.SelectedItems.map(a => a[this.KeyPropertyPath]);

        var keysString = keys.join(",")
        this.SelectedItem = keysString;
        this.SelectedItemChanged.emit(keysString);
        this.SetDisplayText();
    }
}