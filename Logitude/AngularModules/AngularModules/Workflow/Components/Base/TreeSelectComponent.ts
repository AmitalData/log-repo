import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "TreeSelect",
    templateUrl: "./TreeSelectComponent.html"
})

export class TreeSelectComponent implements OnInit {

    @Input() Items: any = [];
    @Input() Width: string = "320px";
    @Input() ShowSearch: boolean = true;
    @Input() AllowClear: boolean = false;

    @Output() ValueChanged = new EventEmitter();

    ngOnInit() {

    }

    treeSelectValueChanged(value: string) {
        this.ValueChanged.emit(value);
    }

}