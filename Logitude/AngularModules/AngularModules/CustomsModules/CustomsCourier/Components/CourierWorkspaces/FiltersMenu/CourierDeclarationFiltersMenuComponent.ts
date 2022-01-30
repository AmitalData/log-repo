import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Component, AfterViewInit, EventEmitter, ContentChild, ViewChild, ViewChildren, QueryList, ChangeDetectorRef, Output, Input } from '@angular/core';
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { AppTool } from "Infrastructure/Tools";
import { FeatureLocator } from "Infrastructure/Utilities/FeatureLocator";

@Component({
    selector: 'CourierDeclarationFiltersMenuComponent',
    templateUrl: './CourierDeclarationFiltersMenuComponent.html',
})

export class CourierDeclarationFiltersMenuComponent
    extends BaseComponent
    implements AfterViewInit {
    @Output() SelectedValueChanged = new EventEmitter();
    @Input() ShowIntegratorFilter: boolean = false;;
    public showFilter:boolean;
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    public DataContext = this;
    constructor(private _entityResourceService: EntityResourceService, private _CD: ChangeDetectorRef) {
        super();
        this.showFilter=false;
        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus", 0).subscribe((response: any) => {
            if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "IntegratorFilter")) {
                this.showFilter = true;
            }
            this._CD.detectChanges();
        });
    }
    ngAfterViewInit(): void {

    }

    SelectedValueChangedEmitIntegrator() {
        var removeFilter = false;
        this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "IntegratorCode");
        this.apiQueryFilters.addAdditionalFilter("IntegratorCode", this.IntegratorCode, null, null, "Equal", true, false, false, "string", true);
        if (AppTool.IsNullOrEmpty(this.IntegratorCode)) {
            removeFilter=true;
        }
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters,RemoveFilter: removeFilter});

    }

    _IntegratorCode: string;
    get IntegratorCode() { return this._IntegratorCode; }
    set IntegratorCode(val: string) {
        this._IntegratorCode = val;
    }


}
