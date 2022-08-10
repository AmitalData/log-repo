import { Component, OnInit, ChangeDetectorRef, EventEmitter, Output } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TreeFilter, TreeFilterType, WhereFilterType } from '../../../../Infrastructure/DataContracts/TreeFilter';

@Component({
    selector: 'TreeFiltersSettings',
    templateUrl: './TreeFiltersSettingsComponent.html',
    inputs: ['Entities', 'DataSource']
})

export class TreeFiltersSettingsComponent extends BaseComponent implements OnInit {
    ShownAdditionalFiltersSettings: boolean;
    public AndOrOps = ["And", "Or"];
    Entities: string[];
    DataSource: any[] = [];
    private EmptyTreeFilters: TreeFilter[] = [];
    @Output() onAdditionalFiltersChanged = new EventEmitter();

    constructor() {
        super();
    }

    ngOnInit() {
        this.LoadDefaultAdditionalFilters();
    }

    LoadDefaultAdditionalFilters() {
        this.DataSource = this.DataSource == null ? this.EmptyTreeFilters : this.DataSource;
        if (this.DataSource.length == 0) {
            this.AddDefaultAdditionalFiltersClicked();
        }
    }

    AddDefaultAdditionalFiltersClicked() {
        let additionalTreeFilter: TreeFilter = new TreeFilter();
        additionalTreeFilter.OperatorType = TreeFilterType.And;
        additionalTreeFilter.FilterType = WhereFilterType.Equal;
        additionalTreeFilter.MainEntityName = "";
        additionalTreeFilter.Field = "";
        additionalTreeFilter.SecondaryEntityName = "";
        this.DataSource.push(additionalTreeFilter);
        this.onAdditionalFiltersChanged.emit({ AdditionalFilters: this.DataSource });
    }

    ShowAdditionalFiltersSettingsClicked() {
        this.ShownAdditionalFiltersSettings = !this.ShownAdditionalFiltersSettings;
    }
}
