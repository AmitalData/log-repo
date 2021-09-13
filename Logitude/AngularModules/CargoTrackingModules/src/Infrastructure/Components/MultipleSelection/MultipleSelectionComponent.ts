import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { ToggleFilter } from '../../../CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';

@Component({
    selector: 'multiple-selection',
    templateUrl: './MultipleSelectionComponent.html',
    styleUrls: ['./MultipleSelectionComponent.css']
})
  
  

export class MultipleSelectionComponent implements OnInit  {

    MultipleSelection = new FormControl();
    @Input() MultipleSelectionList: any[];
    @Input() Title: string;
    @Output() SelectionAdded: EventEmitter<any> = new EventEmitter();
    @Output() SelectionRemoved: EventEmitter<any> = new EventEmitter();

    @Output() SelectedFilters: ToggleFilter[] = [];
    DefaultSelected: string;

    @ViewChild('select') select: MatSelect;

    constructor() {
    }

    ngOnInit(): void {
        this.DefaultSelected = this.MultipleSelectionList[0];
    }

    ToggleFilter(filter: ToggleFilter) {
        var isSelected = this.select.options.find(d => d.value.Code == filter.Code).selected;
        if (isSelected)
            this.SelectFilter(filter)
        else
            this.DeselectFilter(filter)

    }

    SelectFilter(filter: ToggleFilter) {
        this.SelectedFilters.push(filter);
        this.SelectionAdded.emit(this.SelectedFilters);
    }

    ClearFilters() {
        this.select.options.forEach((item: MatOption) => {
            item.deselect();
        });
        this.SelectedFilters = [];
    }

    DeselectFilter(filter: ToggleFilter) {
        var index = this.SelectedFilters.findIndex(d => d == filter);
        this.SelectedFilters.splice(index, 1);
        this.SelectionRemoved.emit(filter);
    }
}
