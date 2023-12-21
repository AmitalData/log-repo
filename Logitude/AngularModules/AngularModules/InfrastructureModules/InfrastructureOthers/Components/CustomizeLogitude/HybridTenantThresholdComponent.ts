import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, Input}  from '@angular/core';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {HybridTenantThresholdPM} from '../../../../Common/EntityPMs/HybridTenantThresholdPM';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import { HybridTenantThresholdPMService } from '../../../../Common/Services/StandardPMs/HybridTenantThresholdPMService';
import { HybridTenantThresholdListService } from 'Common/Services/StandardLists/HybridTenantThresholdListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    selector: 'HybridTenantThresholdComponent',
    templateUrl: './HybridTenantThresholdComponent.html',
})

export class HybridTenantThresholdComponent extends BaseComponent {
    public DataContext: HybridTenantThresholdComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() { super();  }

    public EntityPM: HybridTenantThresholdPM;
    public Isupdate: boolean = false;
    public DataLoaded: boolean = false;
    private listService: HybridTenantThresholdListService = new HybridTenantThresholdListService();
    private service: HybridTenantThresholdPMService = new HybridTenantThresholdPMService();

     type:number=0;
     

    SetDataContext(type: number) {
      this.type=type
      this.LoadHybridTenantThreshold();
    }
    LoadHybridTenantThreshold() {
        this.CurrentSession.StartBusyIndicatorLoading();
        
        this.service.get(SessionLocator.Tenant,this.type).subscribe((res:any) => {
            if (!res.HasError) {
                this.EntityPM = res.Result;
                if (this.EntityPM == null) {
                    this.EntityPM = new HybridTenantThresholdPM();
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.EntityPM.WaitingThresold = 10;
                    this.EntityPM.FailedThresold = 10;
                    this.Isupdate = false;
                    this.EntityPM.TypeCode=this.type;
                
                }
                else {
                    this.Isupdate = true;


                }
            }
            this.CurrentSession.StopBusyIndicator();
            this.DataLoaded = true;

        });
    }

    public get WaitingThresold() {
        var waitingThresold: number = 0;
        if (this.EntityPM != null) {
            waitingThresold = this.EntityPM.WaitingThresold;
        }
        return waitingThresold; 
    }

    public set WaitingThresold(value: number) {
        if (value != this.EntityPM.WaitingThresold) {
            this.EntityPM.WaitingThresold = value;
        }
    }



    public get FailedThresold() {
        var failedThresold: number = 0;
        if (this.EntityPM != null) {
            failedThresold = this.EntityPM.FailedThresold;
        }
        return failedThresold;
    }

    public set FailedThresold(value: number) {
        if (value != this.EntityPM.FailedThresold) {
            this.EntityPM.FailedThresold = value;
        }
    }

  

    CloseButtonClicked() { this.CurrentSession.CloseCurrentWindow(); }
    SaveButtonClicked() {
         debugger
        if (!this.Isupdate) {
            this.service.insert(this.EntityPM).subscribe((res:any) => {
                this.CurrentSession.CloseCurrentWindow();
            });
        }
        else {
            this.service.update(this.EntityPM).subscribe((res:any) => {
                this.CurrentSession.CloseCurrentWindow();
            });
        }
    }
}

export enum ThresholdTypes {
    Hybrid,
    Cloud,
    
  };
