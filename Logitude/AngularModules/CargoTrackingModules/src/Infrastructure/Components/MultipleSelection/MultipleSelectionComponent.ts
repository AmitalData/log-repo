import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { ToggleFilter } from '../../../CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';

@Component({
    selector: 'multiple-selection',
    templateUrl: './MultipleSelectionComponent.html',
    styleUrls: ['./MultipleSelectionComponent.css']
})
export class MultipleSelectionComponent {

    MultipleSelection = new FormControl();
    @Input() MultipleSelectionList: any[];
    @Input() Title: string;
    @Input() ShowSortIcon: boolean = false;
    @Output() SelectionChanged: EventEmitter<any> = new EventEmitter();
    @Output() SelectToggleFilters: string;
    IsSortDescending: boolean = true;
    @Input() UseFixedTitle: boolean = false;
    @ViewChild('select') select: MatSelect;

    constructor() {
       
    }

    SelectFilter(selectedFilter: any) {
        this.SelectToggleFilters = selectedFilter.Code;
        if (selectedFilter.name == "Ascending") {
            this.IsSortDescending = false;
        }
        if (selectedFilter.name == "Descending") {
            this.IsSortDescending = true;
        }
        this.SelectionChanged.emit(this.SelectToggleFilters);
    }

    ClearFilters() {
        this.select.options.forEach((item: MatOption) => {
            item.deselect();
        });
    }

    DeselectFilter(filterCode: string) {
        this.select.options.find(d => d.value.Code == filterCode).deselect();
    }

}
