import { Component, OnInit, Input, Output, EventEmitter, ViewChild, AfterViewInit } from "@angular/core";

@Component({
    selector: "TreeSelect",
    templateUrl: "./TreeSelectComponent.html"
})

export class TreeSelectComponent implements OnInit, AfterViewInit {

    @Input() Items: any = [];
    @Input() Value: string;
    @Input() Width: string = "300px";
    @Input() ShowSearch: boolean = true;
    @Input() AllowClear: boolean = true;
    @Input() IsDisabled: boolean = false;
    @Input() DataCy: string | null = null;

    @Output() ValueChanged = new EventEmitter();

    @ViewChild("nzTreeSelect") NzTreeSelect: any;

    public TreeItems: any = [];
    public FilteredTreeItems: any = [];

    public SearchTerm: string = "";

    ngOnInit() {
        if (this.IsDisabled) {
            this.AllowClear = false;
        }
        this.TreeItems = JSON.parse(JSON.stringify(this.Items));
        this.setFilteredTreeItems("");
    }

    ngAfterViewInit() {
        this.NzTreeSelect.nzSelectSearchComponent.onValueChange = (searchTerm: string) => {
            this.onTreeSelectSearchChange(searchTerm);
        }

        this.NzTreeSelect.onChange = (value: string) => {
            this.onTreeSelectValueChange(value);
        };
    }

    onTreeSelectSearchChange(searchTerm: string) {
        this.SearchTerm = searchTerm ? searchTerm : "";
        this.NzTreeSelect.nzPlaceHolder = "";
        this.NzTreeSelect.inputValue = "";
        this.NzTreeSelect.value = [];

        this.Value = null;
        this.ValueChanged.emit(null);

        this.setFilteredTreeItems((searchTerm ? searchTerm : ""));
    }

    onTreeSelectValueChange(value: string) {
        this.SearchTerm = "";
        this.NzTreeSelect.nzPlaceHolder = "";
        this.NzTreeSelect.nzSelectSearchComponent.inputElement.nativeElement.value = "";

        this.Value = value;
        this.ValueChanged.emit(value);

        this.setFilteredTreeItems("");
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
                let childrenFilteredTreeItems = this.getFilteredTreeItems(searchTerm, item.children);
                if (childrenFilteredTreeItems.length) {
                    filteredTreeItems.push(Object.assign({}, item, { children: childrenFilteredTreeItems }));
                }
            }
        });
        return filteredTreeItems;
    }

    limitFilteredTreeItems(items: any = null) {
        for (let item of (items || this.FilteredTreeItems)) {
            if (item.children && item.children.length > 10) {
                let exactMatchItems = item.children.filter((i: any) => i.title.toLowerCase() === this.SearchTerm.toLowerCase());
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

                if (exactMatchItems.length > 0) {
                    exactMatchItems.forEach((exactMatchItem: any) => {
                        let isExactMatchItemInChildrenItems = item.children.filter((i: any) => i.key.toLowerCase() === exactMatchItem.key.toLowerCase()).length > 0;
                        if (!isExactMatchItemInChildrenItems) {
                            item.children.unshift(exactMatchItem);
                        }
                    });
                }
            }

            if (item.children && item.children.length > 0) {
                this.limitFilteredTreeItems(item.children);
            }
        }
    }

    displayItem = (item: any) => {
        let itemParentNode = item.parentNode;
        if (itemParentNode) {
            return itemParentNode.title + " > " + item.title;
        }
        return item.title;
    }
}