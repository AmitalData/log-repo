import { Component, Input, OnInit, HostListener, ElementRef, Output, EventEmitter } from '@angular/core';
import { DashboardList } from '../../../../DashboardModule/EntityLists/DashboardList';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

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
        if (this.itemsSource == value || !value) return;
        this.itemsSource = value;
        this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));
    }

    @Input() public SectionsItemsSource: CodeNameClass[] = [];
    public DisplayItemsSource: DashboardList[] = [];
    public SearchText: string;
    public DisplayText: string;
    public SkeletonCount = Array(10).fill(0).map((x, i) => i);

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

    public GetSelectedItemText(item: any) {
        if (item) return this.DisplayMemberPath ? item[this.DisplayMemberPath] : item;
        return "Search for a Dashboard";
    }

    public GetItemText(item: any) {
        if (item)
            return this.DisplayMemberPath ? item[this.DisplayMemberPath] : item;
    }

    public onSearchChange(text: string) {
        if (!this.ItemsSource) return;
        if (!text) {
            this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource));
            return;
        }

        this.DisplayItemsSource = JSON.parse(JSON.stringify(this.ItemsSource.filter(item => {
            if (!this.DisplayMemberPath) return item.toLowerCase().includes(text.toLowerCase());
            return item[this.DisplayMemberPath].toLowerCase().includes(text.toLowerCase());
        })));
    }

    GetSectionDisplayItemsSource(code: string): any[] {
        switch (code) {
            case "SYS":
                return this.DisplayItemsSource.filter(d => (d.Tenant != 0 && d.PermissionLevelCode == "PUB" && d.CreatedByUserId != SessionLocator.LoggedUserId) || d.Tenant == 0);

            case "MYS":
                return this.DisplayItemsSource.filter(d => d.Tenant != 0 && d.CreatedByUserId == SessionLocator.LoggedUserId);

            case "SHR":
                return this.DisplayItemsSource.filter(d => d.Tenant != 0 && d.PermissionLevelCode == "SPF" && d.CreatedByUserId != SessionLocator.LoggedUserId);

            default:
                return this.DisplayItemsSource;
        }
    }
}
