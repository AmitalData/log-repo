import {Component, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {HybridPartnerList} from '../../../../Common/EntityLists/HybridPartnerList';
import {Headers} from '@angular/http';
import {AppTool} from '../../../../Infrastructure/Tools';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {HybridPartnerExtendedListService} from '../../../../Common/Services/ExtendedLists/HybridPartnerExtendedListService';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './PermissionsHybridPartnerTabComponent.html',
})

export class PermissionsHybridPartnerTabComponent extends BaseComponent {

    DataContext: PermissionsHybridPartnerTabComponent = this;
    hybridPartnerExtendedListService: HybridPartnerExtendedListService;

    AllowdHybridPartnerLists: HybridPartnerList[] = [];
    AllowingHybridPartnerLists: HybridPartnerList[] = [];
    IsCompleteLoadAllowdHybrid: boolean = false;
    IsCompleteLoadAllowingHybrid: boolean = false;
    partnerId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.hybridPartnerExtendedListService = new HybridPartnerExtendedListService();
        if (this.entityArgs.EntityPM) {
            this.LoadData(this.entityArgs.EntityPM.Id);
        }

     
    }

    LoadData(hybridPartnerId: string) {

        this.CurrentSession.StartBusyIndicatorLoading();
        this.LoadAllowdHybridPartner(hybridPartnerId);
        this.LoadAllowingHybridPartner(hybridPartnerId);
    }

    LoadAllowdHybridPartner(hybridPartnerId: string) {
        this.IsCompleteLoadAllowdHybrid = false;
        this.AllowdHybridPartnerLists = [];

        this.hybridPartnerExtendedListService.GetAllowdHybridPartnerLists(hybridPartnerId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.AllowdHybridPartnerLists = pmResponse.Result;
              
            }

            this.IsCompleteLoadAllowdHybrid = true;

            this.StopLoading();

        });

    }

    LoadAllowingHybridPartner(hybridPartnerId: string) {
        this.IsCompleteLoadAllowingHybrid = false;
        this.AllowingHybridPartnerLists = [];

        this.hybridPartnerExtendedListService.GetAllowingHybridPartnerLists(hybridPartnerId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.AllowingHybridPartnerLists = pmResponse.Result;

            }

            this.IsCompleteLoadAllowingHybrid = true;

            this.StopLoading();

        });

    }




    StopLoading() {

        if (this.IsCompleteLoadAllowingHybrid && this.IsCompleteLoadAllowdHybrid) {
            this.CurrentSession.StopBusyIndicator();
        }

    }


}
