import { Component, Input, OnInit } from "@angular/core";
import { GlobalFilterItem } from "./GlobalFilterItem";

@Component({
    selector: 'GlobalFilterItem',
    templateUrl: './GlobalFilterItemComponent.html',
    styleUrls: ['./GlobalFilter.scss']
})

export class GlobalFilterItemComponent implements OnInit {
    @Input() FilterItem: GlobalFilterItem;
    @Input() Position: number;

    ngOnInit(): void {
        
    }

}