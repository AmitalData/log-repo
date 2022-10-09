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

    //public SearchInputValue: string = "";
    public TreeItems: any = [];
    public FilteredTreeItems: any = [];

    //public TreeItemChildrenDictionary: { [key: string]: any } = {};

    ngOnInit() {
        this.TreeItems = JSON.parse(JSON.stringify(this.Items));
        this.setFilteredTreeItems("");
        //this.FilteredTreeItems = this.getFilteredTreeItems("");
        //this.initializeFilteredTreeItems();
        //this.test();
    }

    ngAfterViewInit() {
        //this.TreeSelect.nativeElement.querySelector(".ant-select-selection-search-input")?.addEventListener("input", (event: any) => this.treeSelectInputEvent(event));
        this.NzTreeSelect.nzSelectSearchComponent.onValueChange = (searchTerm: string) => {
            //this.SearchInputValue = searchTerm;

            this.NzTreeSelect.nzPlaceHolder = "";
            this.NzTreeSelect.inputValue = "";
            this.NzTreeSelect.value = [];

            this.ValueChanged.emit(null);
            this.Value = null;

            // let test = this.TreeSelect.nativeElement.querySelector(".ant-select-selection-item") as HTMLElement | null;

            // if (test && searchTerm) {
            //     if (searchTerm === "") {
            //         test.style.display = "block";
            //     } else {
            //         test.style.display = "none";
            //     }
            // }

            //this.FilteredTreeItems = this.getFilteredTreeItems((searchTerm ? searchTerm : ""));
            this.setFilteredTreeItems((searchTerm ? searchTerm : ""));
        }

        this.NzTreeSelect.onChange = (value: any) => {
            this.NzTreeSelect.nzPlaceHolder = "";
            this.NzTreeSelect.nzSelectSearchComponent.inputElement.nativeElement.value = "";

            //this.NzTreeSelect.value = [];

            this.ValueChanged.emit(value);
            this.Value = value;

            //if (value === null) {
            this.setFilteredTreeItems("");
            //}
        };
    }

    ngOnDestroy() {
        //this.TreeSelect.nativeElement.querySelector(".ant-select-selection-search-input")?.removeEventListener("input", (event: any) => this.treeSelectInputEvent(event));
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

    //treeSelectInputEvent(event: any) {
    //this.SearchInputValue = event.target.value;
    //this.test2(event.target.value);
    //}

    //this.NzTreeSelect?.getTreeNodes()[0].clearChildren()

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


    // test2(searchInputValue: string) {
    //     Object.keys(this.TreeItemChildrenDictionary).forEach(key => {
    //         let test = this.TreeItemChildrenDictionary[key].filter(i => i.title.toLowerCase().indexOf(searchInputValue.toLowerCase()) !== -1);
    //         if (test.length > 10) {
    //             test = test.slice(0, 10);
    //         }
    //         this.test3(key, test);
    //     });
    // }

    // test3(key: string, data: any, items: any = null) {
    //     for (let item of (items || this.TreeItems)) {
    //         if (item.key === key) {
    //             item.children = data;
    //         }

    //         if (item.children && item.children.length > 0) {
    //             this.test3(key, data, item.children);
    //         }
    //     }
    // }



    // handleTreeSelectUnmatchedItems() {
    //     // setTimeout(() => {
    //     //     let treeNodeTitles = Array.from(
    //     //         document.getElementsByClassName("ant-tree-node-content-wrapper") as HTMLCollectionOf<HTMLElement>
    //     //     );
    //     //     treeNodeTitles.forEach(treeNodeTitle => {
    //     //         let title = treeNodeTitle.innerText;
    //     //         if (this.SearchInputValue && this.SearchInputValue !== "" && title && title.toLowerCase().indexOf(this.SearchInputValue.toLowerCase()) === -1) {
    //     //             treeNodeTitle.style.opacity = "0.5";
    //     //         } else {
    //     //             treeNodeTitle.style.opacity = "1";
    //     //         }
    //     //     });
    //     // }, 200);
    // }

    treeSelectValueChanged(value: string) {
        //this.ValueChanged.emit(value);
    }
}