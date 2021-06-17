import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWObjectFieldsDetails, DWQueryBuilderBaseComponent } from '../../../../InfrastructureModules/InfrastructureBIReport/Components/Workspaces/DWQueryBuilderBaseComponent';

declare var window: any;

@Component({
    selector: 'DWQueryBuilderFilters',
    
    templateUrl: './DWQueryBuilderFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'DataContext', 'SelectedFiltersDataSourceChanged']
})

export class DWQueryBuilderFiltersComponent extends DWQueryBuilderBaseComponent implements OnInit {
    public Types: any[] = [];
    DataContext: any;
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public SelectedFiltersDataSourceChanged: EventEmitter<any>;

    constructor(private CDR: ChangeDetectorRef) {
        super(CDR);
    }

    ngOnInit() {
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();

        if (this.SelectedFiltersDataSourceChanged) {
            this.SelectedFiltersDataSourceChanged.subscribe((res) => {
                this.SelectedFiltersDataSource = res;
            });
        }
    }
}
