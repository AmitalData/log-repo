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
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
@Component({
    
    selector: 'IntegrationSystemsSetting',
    templateUrl: './IntegrationSystemsSetting.html',

})

export class IntegrationSystemsSetting extends BaseComponent implements OnInit {
    public tenantPMService: TenantPMService;
    DataContext: IntegrationSystemsSetting = this;
    ExportQuotationsToIntegratedSystem: boolean = false;
    myTenantPM: TenantPM;
    public QuotationTransferTriggersList: CodeNameClass[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();

        }


        this.QuotationTransferTriggersList.push(new CodeNameClass("OnSend", "On Send"));
        this.QuotationTransferTriggersList.push(new CodeNameClass("OnAccept", "On Accept"));

        this.LoadCurrentTenant();

    }

    ngOnInit(


    ) {




    }

    private selectedTransferToUnfreightTriggerType: CodeNameClass;
    get SelectedTransferToUnfreightTriggerType() { return this.selectedTransferToUnfreightTriggerType; }
    set SelectedTransferToUnfreightTriggerType(value: CodeNameClass) {
        if (this.selectedTransferToUnfreightTriggerType != value) {
            this.selectedTransferToUnfreightTriggerType = value;

             

            
        }
    }

    LoadCurrentTenant() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.tenantPMService.get(SessionInfo.LoggedUserTenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.myTenantPM = myResult;
                    if (this.myTenantPM) {
                        this.ExportQuotationsToIntegratedSystem = this.myTenantPM.ExportQuotationsToIntegratedSystem;
                        this.SelectedTransferToUnfreightTriggerType = this.QuotationTransferTriggersList.filter(f => f.Code === this.myTenantPM.TransferQuotationsToUnifreightTrigger)[0];
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
            if (this.ExportQuotationsToIntegratedSystem !== this.myTenantPM.ExportQuotationsToIntegratedSystem
                || this.SelectedTransferToUnfreightTriggerType?.Code !== this.myTenantPM.TransferQuotationsToUnifreightTrigger) {
                this.myTenantPM.ExportQuotationsToIntegratedSystem = this.ExportQuotationsToIntegratedSystem;
                this.myTenantPM.TransferQuotationsToUnifreightTrigger = this.SelectedTransferToUnfreightTriggerType?.Code;
                this.CurrentSession.StartBusyIndicatorSaving();
                this.tenantPMService.update(this.myTenantPM).subscribe((res:any) => {
                    this.CurrentSession.StopBusyIndicator();
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        SessionLocator.TenantPM.ExportQuotationsToIntegratedSystem = this.ExportQuotationsToIntegratedSystem;
                        SessionLocator.TenantPM.TransferQuotationsToUnifreightTrigger = this.SelectedTransferToUnfreightTriggerType?.Code;
                        this.CloseButtonClicked();
                    }
                });
            }
            else this.CloseButtonClicked();
        }
    }

  


}
