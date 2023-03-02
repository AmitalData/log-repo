import { Component, OnInit, Input, Output, EventEmitter, ViewChild, AfterViewInit } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    selector: "TreeSelect",
    templateUrl: "./TreeSelectComponent.html"
})

export class TreeSelectComponent implements OnInit, AfterViewInit {

    @Input() Items: TreeSelectItem[] = [];
    @Input() Value: string;
    @Input() Width: string = "300px";
    @Input() ShowSearch: boolean = true;
    @Input() AllowClear: boolean = true;
    @Input() IsDisabled: boolean = false;
    @Input() ShowExpand: boolean = true;
    @Input() IsReturnedTreeSelectItem: boolean = false;
    @Input() DisplayTitle: boolean = true;
    @Input() SetSelectedValue: boolean = true;
    @Input() PlaceHolder: string | null = null;
    @Input() Template: any = null;
    @Input() ShowItem: (treeSelectItem: TreeSelectItem) => boolean = (_treeSelectItem: TreeSelectItem) => { return true };

    @Input() DataCy: string | null = null;

    @Output() ValueChanged = new EventEmitter();

    @ViewChild("nzTreeSelect") NzTreeSelect: any;

    public TreeItems: TreeSelectItem[] = [];
    public FilteredTreeItems: TreeSelectItem[] = [];

    public SearchTerm: string = "";
    public Title: string | null = null;

    private ItemsLimit: number = 10;

    ngOnInit() {
        this.TreeItems = JSON.parse(JSON.stringify(this.Items));
        this.TreeItems = this.checkItemsToShow(this.TreeItems);
        this.setFilteredTreeItems("");
    }

    ngAfterViewInit() {
        setTimeout(() => {
            this.setTitle(this.Value);
        }, 10);

        this.NzTreeSelect.nzSelectSearchComponent.onValueChange = (searchTerm: string) => {
            this.onTreeSelectSearchChange(searchTerm);
        }

        this.NzTreeSelect.onChange = (value: string) => {
            this.onTreeSelectValueChange(value);
        };
    }

    checkItemsToShow = (items: TreeSelectItem[]) => items.filter(i => {
        if (i.children) {
            i.children = this.checkItemsToShow(i.children);
        }
        if (i.children.length === 0 && !i.selectable) {
            return false;
        }
        let showItem = this.ShowItem(i);
        if (i.children.length > 0 && !showItem) {
            i.selectable = false;
            return true;
        }
        return showItem;
    })

    onTreeSelectSearchChange(searchTerm: string) {
        this.SearchTerm = searchTerm ? searchTerm : "";
        this.NzTreeSelect.nzPlaceHolder = "";
        this.NzTreeSelect.inputValue = "";
        this.NzTreeSelect.value = [];

        this.Value = null;
        this.setTitle(null);
        this.emitValueChanged(null);

        this.setFilteredTreeItems((searchTerm ? searchTerm : ""));
    }

    onTreeSelectValueChange(value: string) {
        this.SearchTerm = "";
        this.NzTreeSelect.nzPlaceHolder = "";
        this.NzTreeSelect.nzSelectSearchComponent.inputElement.nativeElement.value = "";

        if (this.SetSelectedValue) {
            this.Value = value;
        } else {
            this.NzTreeSelect.value = [];
        }

        this.setTitle(value);
        this.emitValueChanged(value);

        this.setFilteredTreeItems("");
    }

    emitValueChanged(value: string | null) {
        if (value) {
            if (this.IsReturnedTreeSelectItem) {
                let nzTreeItem = this.NzTreeSelect.getTreeNodeByKey(value);
                let treeSelectItem = nzTreeItem ? nzTreeItem.origin : null;
                this.ValueChanged.emit(treeSelectItem ? treeSelectItem : null);
            } else {
                this.ValueChanged.emit(value);
            }
        } else {
            this.ValueChanged.emit(null);
        }
    }

    setFilteredTreeItems(searchTerm: string) {
        this.FilteredTreeItems = this.getFilteredTreeItems(searchTerm);
        this.limitParentFilteredTreeItems();
        this.limitFilteredTreeItems();
    }

    getFilteredTreeItems(searchTerm: string, items: TreeSelectItem[] | null = null) {
        let filteredTreeItems = [];
        JSON.parse(JSON.stringify((items ? items : this.TreeItems))).forEach((item: TreeSelectItem) => {
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

    limitParentFilteredTreeItems() {
        if (this.FilteredTreeItems.length > this.ItemsLimit) {
            let exactMatchItems = this.FilteredTreeItems.filter((i: TreeSelectItem) => i.title.toLowerCase() === this.SearchTerm.toLowerCase());
            let limitedItems = this.FilteredTreeItems.slice(0, this.ItemsLimit);
            if (this.Value) {
                let selectedItem = this.FilteredTreeItems.filter((i: TreeSelectItem) => i.key.toLowerCase() === this.Value.toLowerCase())[0];
                let isSelectedItemInLimitedItems = limitedItems.filter((i: TreeSelectItem) => i.key.toLowerCase() === this.Value.toLowerCase()).length > 0;
                if (selectedItem && !isSelectedItemInLimitedItems) {
                    this.FilteredTreeItems = this.FilteredTreeItems.slice(0, (this.ItemsLimit - 1));
                    this.FilteredTreeItems.unshift(selectedItem);
                } else {
                    this.FilteredTreeItems = limitedItems;
                }
            } else {
                this.FilteredTreeItems = limitedItems;
            }

            if (exactMatchItems.length > 0) {
                exactMatchItems.forEach((exactMatchItem: TreeSelectItem) => {
                    let isExactMatchItemInChildrenItems = this.FilteredTreeItems.filter((i: TreeSelectItem) => i.key.toLowerCase() === exactMatchItem.key.toLowerCase()).length > 0;
                    if (!isExactMatchItemInChildrenItems) {
                        this.FilteredTreeItems.unshift(exactMatchItem);
                    }
                });
            }
        }
    }

    limitFilteredTreeItems(items: TreeSelectItem[] | null = null) {
        for (let item of (items || this.FilteredTreeItems)) {
            if (item.children && item.children.length > this.ItemsLimit) {
                let exactMatchItems = item.children.filter((i: TreeSelectItem) => i.title.toLowerCase() === this.SearchTerm.toLowerCase());
                let limitedItems = item.children.slice(0, this.ItemsLimit);
                if (this.Value) {
                    let selectedItem = item.children.filter((i: TreeSelectItem) => i.key.toLowerCase() === this.Value.toLowerCase())[0];
                    let isSelectedItemInLimitedItems = limitedItems.filter((i: TreeSelectItem) => i.key.toLowerCase() === this.Value.toLowerCase()).length > 0;
                    if (selectedItem && !isSelectedItemInLimitedItems) {
                        item.children = item.children.slice(0, (this.ItemsLimit - 1));
                        item.children.unshift(selectedItem);
                    } else {
                        item.children = limitedItems;
                    }
                } else {
                    item.children = limitedItems;
                }

                if (exactMatchItems.length > 0) {
                    exactMatchItems.forEach((exactMatchItem: TreeSelectItem) => {
                        let isExactMatchItemInChildrenItems = item.children.filter((i: TreeSelectItem) => i.key.toLowerCase() === exactMatchItem.key.toLowerCase()).length > 0;
                        if (!isExactMatchItemInChildrenItems) {
                            item.children.unshift(exactMatchItem);
                        }
                    });
                }
            }

            if (!item.disabled && !item.isLeaf && item.children && item.children.length > 0) {
                item.expanded = item.expanded || (this.SearchTerm && this.SearchTerm !== "");
            }

            if (item.children && item.children.length > 0) {
                this.limitFilteredTreeItems(item.children);
            }
        }
    }

    setTitle(value: string) {
        if (value) {
            let nzTreeItem = this.NzTreeSelect.getTreeNodeByKey(value);
            this.Title = this.getDisplayTitle(nzTreeItem);
        } else {
            this.Title = null;
        }
    }

    getDisplayTitle = (nzTreeItem: any) => {
        if (!this.DisplayTitle) {
            this.NzTreeSelect.nzPlaceHolder = this.PlaceHolder
            return null
        }
        if (nzTreeItem) {
            let itemParentNode = nzTreeItem.parentNode;
            if (itemParentNode) {
                return itemParentNode.title + " > " + nzTreeItem.title;
            }
            return nzTreeItem.title;
        }
        return null;
    }

}