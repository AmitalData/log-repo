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
    @Input() AllowClear: boolean = false;

    @Output() ValueChanged = new EventEmitter();

    @ViewChild("treeSelect") TreeSelect: any;

    ngOnInit() {

    }

    ngAfterViewInit() {
        //this.TreeSelect.nativeElement.querySelector(".ant-select-selection-search-input").addEventListener("input", (event: any) => this.test(event));
    }

    ngOnDestroy() {
        //this.TreeSelect.nativeElement.querySelector(".ant-select-selection-search-input").removeEventListener("input", (event: any) => this.test(event));
    }

    treeSelectValueChanged(value: string) {
        this.ValueChanged.emit(value);
    }

}