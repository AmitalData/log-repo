import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {HybridTenantStateListExtendedService} from '../../../../Common/Services/ExtendedLists/HybridTenantStateListExtendedService';
import {HybridTenantStateList} from '../../../../Common/EntityLists/HybridTenantStateList';

@Component({
    moduleId: module.id,
    selector: 'HybridTenantStateComponent',
    templateUrl: './HybridTenantStateComponent.html',
})

export class HybridTenantStateComponent extends BaseComponent implements OnInit {

    hybridTenantStateListExtendedService: HybridTenantStateListExtendedService;
    SelectedHybridTenantStateList: HybridTenantStateList;
    HybridTenantStateLists: HybridTenantStateList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.hybridTenantStateListExtendedService = new HybridTenantStateListExtendedService();
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit() {

        this.LoadData();
    }
    


    LoadData() {

        this.HybridTenantStateLists = [];
        this.hybridTenantStateListExtendedService.GetHybridTenantStateLists().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.HybridTenantStateLists = myResponse.Result;
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
   

   CloseButtonClicked() {



        this.CurrentSession.CloseCurrentWindow();
    }


   SaveButtonClicked() {



        this.CurrentSession.CloseCurrentWindow();
    }

}

