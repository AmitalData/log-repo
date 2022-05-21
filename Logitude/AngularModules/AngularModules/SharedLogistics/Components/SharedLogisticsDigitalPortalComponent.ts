import { Component, OnInit } from '@angular/core';
import { SessionInfo } from '../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { TenantPMService } from '../../Common/Services/StandardPMs/TenantPMService';
import { TenantPM } from '../../Common/EntityPMs/TenantPM';
import { ApiQueryFilters } from '../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../Infrastructure/Args';
import { EntityResourceService } from '../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './SharedLogisticsDigitalPortalComponent.html',
})

export class SharedLogisticsDigitalPortalComponent implements OnInit {

    public SharedLogisticsActivatedEnabled: boolean = false;
    private tenantPMService: TenantPMService;
    private myTenantPM: TenantPM;
    private filterAgrs: ApiQueryFilters;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        this.InitalizeServices();
    }

    InitalizeServices() {
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();
        }
    }

    ngOnInit() {
        this.LoadData();
    }

    LoadData() {
        this.LoadCurrentTenant();
    }

    LoadCurrentTenant() {
        this.tenantPMService.get(SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.myTenantPM = myResult;
                    this.RefreshTenantScreenData();
                }
            }
        });
    }

    RefreshTenantScreenData() {
        if (this.myTenantPM.IsSharedLogisticsActivated || this.myTenantPM.IsMobileActivated) {
            this.SharedLogisticsActivatedEnabled = true;
        }
        else {
            this.SharedLogisticsActivatedEnabled = false;
        }
    }

    InviteLinkClick() {
        var backButtonTitle = "Shared Logistics";
        var displayTitle = "Customers";
        var objectTableName = "Customer";
        var queryCode = "Shared Logistics Customers";
        this.filterAgrs = new ApiQueryFilters();
        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = backButtonTitle;
        listArgs.IsDigitalPortalMenuClicked = true;
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response: any) => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }
}
