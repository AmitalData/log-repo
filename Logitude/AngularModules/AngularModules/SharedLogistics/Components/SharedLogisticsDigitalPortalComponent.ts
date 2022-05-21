import { Component, OnInit } from '@angular/core';
import { SessionInfo } from '../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { TenantPMService } from '../../Common/Services/StandardPMs/TenantPMService';
import { TenantPM } from '../../Common/EntityPMs/TenantPM';
import { ApiQueryFilters } from '../../Infrastructure/DataContracts/ApiQueryFilters';
import { DocumentTypeListService } from '../../Common/Services/StandardLists/DocumentTypeListService';
import { AttachmentsList } from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import { DocumentTypeList } from '../../Common/EntityLists/DocumentTypeList';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
import { DocsOutDataViewModel } from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import { DocumentOutPMService } from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import { DocumentTypePMExtendedService } from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import { EntityPartner } from '../../Infrastructure/DataContracts/EntityPartner';
import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';

@Component({
    templateUrl: './SharedLogisticsDigitalPortalComponent.html',
})

export class SharedLogisticsDigitalPortalComponent implements OnInit {

    SharedLogisticsActivatedEnabled: boolean = false;
    private tenantPMService: TenantPMService;
    private myTenantPM: TenantPM;
    private documentTypeCode = "SLCIN";
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

    }


}
