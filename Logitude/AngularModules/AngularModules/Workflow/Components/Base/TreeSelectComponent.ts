import { Component, OnInit, Input, Output, EventEmitter, OnDestroy, ViewChild, AfterViewInit } from "@angular/core";

@Component({
    selector: "TreeSelect",
    templateUrl: "./TreeSelectComponent.html"
})

export class TreeSelectComponent implements OnInit, AfterViewInit, OnDestroy {

    @Input() Items: any = [];
    @Input() Value: string;
    @Input() Width: string = "300px";
    @Input() ShowSearch: boolean = true;
    @Input() AllowClear: boolean = true;

    @Output() ValueChanged = new EventEmitter();

    @ViewChild("treeSelect") TreeSelect: any;
    @ViewChild("nzTreeSelect") NzTreeSelect: any;

    public TreeItems: any = [];
    public FilteredTreeItems: any = [];

    ngOnInit() {
        this.TreeItems = JSON.parse(JSON.stringify(this.Items));
        this.setFilteredTreeItems("");
    }

    ngAfterViewInit() {
        this.NzTreeSelect.nzSelectSearchComponent.onValueChange = (searchTerm: string) => {
            this.NzTreeSelect.nzPlaceHolder = "";
            this.NzTreeSelect.inputValue = "";
            this.NzTreeSelect.value = [];

            this.ValueChanged.emit(null);
            this.Value = null;

            this.setFilteredTreeItems((searchTerm ? searchTerm : ""));
        }

        this.NzTreeSelect.onChange = (value: any) => {
            this.NzTreeSelect.nzPlaceHolder = "";
            this.NzTreeSelect.nzSelectSearchComponent.inputElement.nativeElement.value = "";

            this.ValueChanged.emit(value);
            this.Value = value;

            this.setFilteredTreeItems("");
        };
    }

    ngOnDestroy() {

    }

    setFilteredTreeItems(searchTerm: string) {
        this.FilteredTreeItems = this.getFilteredTreeItems(searchTerm);
        this.limitFilteredTreeItems();
    }

    getFilteredTreeItems(searchTerm: string, items: any = null) {
        let filteredTreeItems = [];
        JSON.parse(JSON.stringify((items ? items : this.TreeItems))).forEach((item: any) => {
            if (item.title.toLowerCase().indexOf(searchTerm.toLowerCase()) !== -1 || (this.Value && item.key.toLowerCase() === this.Value.toLowerCase())) {
                filteredTreeItems.push(item);
            } else {
                let childResults = this.getFilteredTreeItems(searchTerm, item.children);
                if (childResults.length) {
                    filteredTreeItems.push(Object.assign({}, item, { children: childResults }));
                }
            }
        });
        return filteredTreeItems;
    }

    limitFilteredTreeItems(items: any = null) {
        for (let item of (items || this.FilteredTreeItems)) {
            if (item.children && item.children.length > 10) {

                let limitedItems = item.children.slice(0, 10);

                if (this.Value) {
                    let selectedItem = item.children.filter((i: any) => i.key.toLowerCase() === this.Value.toLowerCase())[0];
                    let isSelectedItemInLimitedItems = limitedItems.filter((i: any) => i.key.toLowerCase() === this.Value.toLowerCase()).length > 0;
                    if (selectedItem && !isSelectedItemInLimitedItems) {
                        item.children = item.children.slice(0, 9);
                        item.children.unshift(selectedItem);
                    } else {
                        item.children = limitedItems;
                    }
                } else {
                    item.children = limitedItems;
                }
            }

            if (item.children && item.children.length > 0) {
                this.limitFilteredTreeItems(item.children);
            }
        }
    }

    displayItem = (item: any) => {
        return item.parentNode.title + " > " + item.title;
    }
}