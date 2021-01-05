import { Component, OnDestroy, AfterViewInit } from '@angular/core';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    templateUrl: './ReferantWorkspaceComponent.html',
    providers: [],
})

export class ReferantWorkspaceComponent implements AfterViewInit {
    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    ngAfterViewInit(): void {
    }
    constructor() {
    

    }
    ViewReferantQuery(myQueryCode: string) {

    }

    BuildFiltersForQuery(filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
    }
}

