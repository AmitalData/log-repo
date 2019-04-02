declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
@Component({
    moduleId: module.id,
    selector: 'IntegrationSystemsSetting',
    templateUrl: './IntegrationSystemsSetting.html',

})

export class IntegrationSystemsSetting extends BaseComponent implements OnInit {
    public tenantPMService: TenantPMService;
    DataContext: IntegrationSystemsSetting = this;
    ExportQuotationsToIntegratedSystem: boolean = false;
    myTenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();

        } 

        this.LoadCurrentTenant();

    }

    ngOnInit(


    ) {




    }

    LoadCurrentTenant() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.tenantPMService.get(SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.myTenantPM = myResult;
                    if (this.myTenantPM) {
                        this.ExportQuotationsToIntegratedSystem = this.myTenantPM.ExportQuotationsToIntegratedSystem;
                    }
                }
            }
        });
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

  
   

    SaveButtonClicked() {
        if (this.myTenantPM) {
            if (this.ExportQuotationsToIntegratedSystem != this.myTenantPM.ExportQuotationsToIntegratedSystem) {
                this.myTenantPM.ExportQuotationsToIntegratedSystem = this.ExportQuotationsToIntegratedSystem;
                this.CurrentSession.StartBusyIndicatorSaving();
                this.tenantPMService.update(this.myTenantPM).subscribe(res => {
                    this.CurrentSession.StopBusyIndicator();
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        SessionLocator.TenantPM.ExportQuotationsToIntegratedSystem = this.ExportQuotationsToIntegratedSystem;
                        this.CloseButtonClicked();
                    }
                });
            }
            else this.CloseButtonClicked();
        }
    }

  


}
