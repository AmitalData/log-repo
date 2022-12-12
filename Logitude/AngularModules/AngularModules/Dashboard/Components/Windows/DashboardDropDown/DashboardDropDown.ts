import { Component, Input, OnInit, HostListener, ElementRef, Output, EventEmitter } from '@angular/core';

@Component({
    selector: "dashboard-dropdown",
    templateUrl: "./DashboardDropDown.html",
    styleUrls: ["./DashboardDropDown.scss"],
})
export class DashboardDropDownComponent implements OnInit {

    @Input() public DisplayMemberPath: string;
    @Input() public SelectedItem: any;
    @Input() public Loading: boolean;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();

    private itemsSource: any[];
    get ItemsSource() { return this.itemsSource; }
    @Input() set ItemsSource(value: any[]) {
        if (this.itemsSource == value) return;
        this.itemsSource = value;
        this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));
    }

    public DisplayItemsSource: any[] = [];
    public IsOpen: boolean = false;
    public SearchText: string;
    public DisplayText: string;
    public SkeletonCount = Array(10).fill(0).map((x, i) => i);

    constructor(private eRef: ElementRef) {

    }

    @HostListener('document:click', ['$event'])
    clickout(event) {
        if (!this.eRef.nativeElement.contains(event.target)) {
            this.IsOpen = false;
        }
    }


    ngOnInit(): void {

    }

    public OpenCloseDropDown() {
        this.IsOpen = !this.IsOpen;
    }

    public SelectItem(item: any) {
        this.IsOpen = false;
        this.SelectedItem = item;
        this.SelectedItemChanged.emit(item);
    }

    public GetItemText(item: any) {
        return this.DisplayMemberPath ? item[this.DisplayMemberPath] : item
    }

    public onSearchChange(text: string) {
        if (!this.ItemsSource) return;
        if (!text) this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));

        this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource.filter(item => {
            if (!this.DisplayMemberPath) return item.toLowerCase().includes(text.toLowerCase());
            return item[this.DisplayMemberPath].toLowerCase().includes(text.toLowerCase());
        })));
    }
}